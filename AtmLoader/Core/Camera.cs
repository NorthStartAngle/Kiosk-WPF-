using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Threading;
using AForge.Imaging.Filters;
using AForge.Video;
using AForge.Video.DirectShow;
using AForge.Vision.Motion;
using AtmLib.Tracing;
using Newtonsoft.Json.Linq;
using AtmLib.Monitor;
using Microsoft.Win32;
using WIA;
using Emgu.CV;
using Emgu.CV.CvEnum;
using Emgu.CV.Util;

using DeviceManager = WIA.DeviceManager;

namespace AtmLoader.Core
{
    // Enum for box state
    public enum State { NONE, OPEN, CLOSE };

    internal class Camera
    {
        static private FilterInfoCollection videoDevices; // Stores available video devices
        static private VideoCaptureDevice videoSource;     // Represents the webcam
        static private MotionDetector motionDetector;      // Detects motion
        static private Bitmap currentFrame;                // Stores the current frame from the webcam
        static private bool isDetectingMotion = false;     // Indicates if motion detection is active

        static public double Quote = 0.3;                  // If darkness is lower than this value, the box is closed, otherwise, it's opened
        static public int MotionFrameRates = 10;           // Check images every this frame rates

        static private int motionFrames = 0;
        static public State BoxState = State.NONE;          // Camera status whether the box is opened or closed

        static private Timer openTimer = null;              // Timer for box opened
        static private Timer closeTimer = null;             // Timer for box closed

        static public int OpenTimerInterval = 5000;         // Timer interval for box opened in ms
        static public int CloseTimerInterval = 300000;      // Timer interval for box closed in ms

        static public readonly string CaptureDirectory = "captures";   // Image Saving Directory Path
        static public int CurrentDevice = -1;               // Current Device Index
        static public string CurrentCameraName {
            get
            {
                if (CurrentDevice < 0)
                    return "N/A";
                if (CurrentDevice < videoDevices.Count)
                    return "CAM: " + videoDevices[CurrentDevice].Name;
                else
                    return "IMG: " + deviceInfos[CurrentDevice - videoDevices.Count].Properties["Name"].ToString();
            }
        }
        // { get { return CurrentDevice >= 0 ? videoDevices[CurrentDevice].Name : "N/A"; } }

        static ApiConnector apiConnector = new ApiConnector();     // API connector to communicate with the server

        public delegate void MotionEventHandler(object sender, MotionEventArgs eventArgs);

        static public NewFrameEventHandler NewFrameEvent;
        static public MotionEventHandler MotionEvent;

        /* Using WIA Class */
        static private DeviceManager deviceManager = null;
        static private DeviceInfos deviceInfos = null;
        static private WIA.Device activeDevice = null;

        static private Timer captureTimer = null;             // Timer for image capturing
        static public int CaptureTimerInterval = 100;         // Timer interval for box opened in ms

        static public void Init()
        {
            using (var log = new Logger(TraceLevel.Verbose, "Core.Camera.Init"))
            {
                // Create captures directory
                if (!Directory.Exists(CaptureDirectory))
                {
                    Directory.CreateDirectory(CaptureDirectory);
                    log.WriteLine(TraceLevel.Verbose, "Created captures directory.");
                }

                // Init video device
                videoDevices = new FilterInfoCollection(FilterCategory.VideoInputDevice);
                deviceManager = new DeviceManager();
                deviceInfos = deviceManager.DeviceInfos;

                if (videoDevices.Count > 0 || deviceInfos.Count > 0)
                {
                    for (int i = 0; i < videoDevices.Count; i ++ )
                    {
                        log.WriteLine(TraceLevel.Verbose, "Video Device : " + videoDevices[i].Name + " " + videoDevices[i].ToString());
                    }

                    foreach( DeviceInfo device in deviceInfos )
                    {
                        log.WriteLine(TraceLevel.Verbose, "Imaging Device : " + device.Properties["Name"] + " " + device.ToString() );
                    }

                    ChangeCamera();
                }
                else
                {
                    log.WriteLine(TraceLevel.Error, "No video devices found.");
                }

                // Init Capture Timer
                captureTimer = new Timer(CaptureTimerCallback, null, 0, CaptureTimerInterval);

                apiConnector.RegisterAtm();
                // StartMotionDetect();
                log.WriteLine(TraceLevel.Verbose, "Component is initialized");
            }
        }

        static private void VideoSource_NewFrame(object sender, NewFrameEventArgs eventArgs)
        {
            using (var log = new Logger(TraceLevel.Verbose, "Core.Camera.VideoSource_NewFrame"))
            {
                ProcessBitmap((Bitmap)eventArgs.Frame.Clone());
            }
        }

        static private void ProcessBitmap( Bitmap bitmap )
        {
            using (var log = new Logger(TraceLevel.Verbose, "Core.Camera.ProcessBitmap"))
            {
                currentFrame = bitmap;

                // Perform motion detection on the current frame
                if (motionDetector != null)
                {
                    double value = motionDetector.ProcessFrame(currentFrame);
                    if (value > 0.2)
                    {
                        // MotionEvent.Invoke(sender, new MotionEventArgs(currentFrame, value));
                        // MotionEvent.Invoke(this, new EventArgs())
                    }
                }

                // Check darkness every MotionFrameRates
                if (motionFrames++ >= MotionFrameRates)
                {
                    // Convert the frame to grayscale
                    Grayscale grayscaleFilter = new Grayscale(0.2125, 0.7154, 0.0721);
                    Bitmap grayscaleFrame = grayscaleFilter.Apply(currentFrame);

                    // Calculate average brightness
                    double averageBrightness = CalculateAverageBrightness(grayscaleFrame);

                    // Use a threshold to determine darkness
                    State dark = averageBrightness < Quote ? State.CLOSE : State.OPEN; // Adjust the threshold as needed

                    // Box state changed
                    if (dark != BoxState)
                    {
                        BoxState = dark;

                        if (BoxState == State.OPEN)
                        {
                            if (closeTimer != null)
                            {
                                closeTimer.Dispose();
                                closeTimer = null;
                            }

                            if (openTimer == null)
                            {
                                openTimer = new Timer(OpenTimerCallback, null, 0, OpenTimerInterval);
                            }

                            log.WriteLine(TraceLevel.Verbose, "Box opened");
                        }
                        else
                        {
                            if (openTimer != null)
                            {
                                openTimer.Dispose();
                                openTimer = null;
                            }

                            if (closeTimer == null)
                            {
                                closeTimer = new Timer(CloseTimerCallback, null, 0, CloseTimerInterval);
                            }

                            log.WriteLine(TraceLevel.Verbose, "Box closed");
                        }
                    }
                    else
                    {
                        // MotionEvent?.Invoke(sender, new MotionEventArgs(currentFrame, averageBrightness));
                    }

                    MotionEvent?.Invoke(null, new MotionEventArgs(currentFrame, averageBrightness, BoxState));
                    motionFrames = 0;

                    log.WriteLine(TraceLevel.Verbose, "Motion Event Occured");
                }


                NewFrameEventArgs eventArgs = new NewFrameEventArgs(currentFrame);
                NewFrameEvent?.Invoke(null, eventArgs);

                log.WriteLine(TraceLevel.Verbose, "New frame event occured");
            }
        }

        static private double CalculateAverageBrightness(Bitmap grayscaleFrame)
        {
            double sumBrightness = 0.0;
            int width = grayscaleFrame.Width;
            int height = grayscaleFrame.Height;

            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    Color pixel = grayscaleFrame.GetPixel(x, y);
                    sumBrightness += pixel.GetBrightness();
                }
            }

            double averageBrightness = sumBrightness / (width * height);
            return averageBrightness;
        }

        // Timer callback at box state of OPEN
        static private void OpenTimerCallback(object state)
        {
            Upload();
        }

        // Timer callback at box state of CLOSE
        static private void CloseTimerCallback(object state)
        {
            Upload();
        }

        static private bool SaveImage(string fileName)
        {
            using (var log = new Logger(TraceLevel.Verbose, "Core.Camera.SaveImage"))
            {
                if (currentFrame == null)
                {
                    log.WriteLine(TraceLevel.Error, "Curren frame is null. Can not save to file.");
                    return false;
                }

                try
                {
                    currentFrame.Save(fileName);
                }
                catch (Exception ex)
                {
                    Logger.WriteException(ex);
                    return false;
                }

                return true;
            }
        }

        // Upload Camera Image to server
        static public bool Upload()
        {
            using (var log = new Logger(TraceLevel.Verbose, "Core.Camera.Upload"))
            {
                // Save to temp file
                DateTime currentDateTime = DateTime.Now;
                string fileName = CaptureDirectory + "/" + BoxState + "_" + currentDateTime.ToString("yyyy_MM_dd HH_mm_ss") + ".jpg";

                if (SaveImage(fileName))
                {
                    apiConnector.SendImageToServer(fileName);
                    log.WriteLine(TraceLevel.Verbose, "Http Request with a camera image sent to the server.");
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }

        // Change active camera and returns the moniker name, if not available returns N/A
        static public string ChangeCamera()
        {
            using (var log = new Logger(TraceLevel.Verbose, "Core.Camera.ChangeCamera"))
            {
                // No video devices available
                if (videoDevices.Count <= 0 && deviceInfos.Count <= 0)
                {
                    log.WriteLine(TraceLevel.Verbose, "No video devices and image devices.");
                    return "N/A";
                }

                CurrentDevice = (CurrentDevice + 1) % (videoDevices.Count + deviceInfos.Count);

                if (videoSource != null)
                {
                    videoSource.Stop();
                    videoSource = null;

                    log.WriteLine(TraceLevel.Verbose, "Stopped last video device.");
                }

                if (activeDevice != null)
                {
                    activeDevice = null;
                    log.WriteLine(TraceLevel.Verbose, "Stopped last imaging device.");
                }

                if (CurrentDevice < videoDevices.Count)
                {
                    videoSource = new VideoCaptureDevice(videoDevices[CurrentDevice].MonikerString);
                    videoSource.NewFrame += new NewFrameEventHandler(VideoSource_NewFrame);
                    videoSource.Start();

                    log.WriteLine(TraceLevel.Verbose, "Changed active camera to " + videoDevices[CurrentDevice].Name);
                    return "CAM: " + videoDevices[CurrentDevice].Name;
                }
                else
                {
                    activeDevice = deviceInfos[CurrentDevice - videoDevices.Count].Connect();

                    log.WriteLine(TraceLevel.Verbose, "Changed active imaging device to " + deviceInfos[CurrentDevice - videoDevices.Count].Properties["Name"].ToString());
                    return "IMG: " + deviceInfos[CurrentDevice - videoDevices.Count].Properties["Name"].ToString();
                }
            }
            
        }

        static private void CaptureTimerCallback(object state)
        {
            if ( activeDevice != null )
            {
                using (var log = new Logger(TraceLevel.Verbose, "Core.Camera.CaptureTimerCallback"))
                {
                    ImageFile image = activeDevice.Items[1].Transfer();
                    Bitmap bitmap = ConvertImageFileToBitmap(image);
                    ProcessBitmap(bitmap);


                    log.WriteLine(TraceLevel.Verbose, "Active Device: " + CurrentCameraName + " Items: " + activeDevice.Items + " " + image );
                }
            }
        }

        // Helper function to convert ImageFile to Bitmap
        static private Bitmap ConvertImageFileToBitmap(ImageFile imageFile)
        {
            byte[] imageData = (byte[])imageFile.FileData.get_BinaryData();

            // Create a MemoryStream from the byte array
            using (MemoryStream memoryStream = new MemoryStream(imageData))
            {
                // Create a Bitmap from the MemoryStream
                Bitmap bitmap = new Bitmap(memoryStream);

                // Return the Bitmap
                return bitmap;
            }
        }

        static public void StartMotionDetect()
        {
            if (!isDetectingMotion)
            {
                motionDetector = new MotionDetector(new TwoFramesDifferenceDetector(), new MotionAreaHighlighting());
                isDetectingMotion = true;
            }
        }

        static public void StopMotionDetect()
        {
            if (isDetectingMotion)
            {
                motionDetector = null;
                isDetectingMotion = false;
            }
        }

        static public void Destroy()
        {
            if (videoSource != null && videoSource.IsRunning)
            {
                videoSource.SignalToStop();
                // videoSource.WaitForStop();
            }
        }
    }

    public class MotionEventArgs : EventArgs
    {
        private Bitmap frame;

        //
        // Summary:
        //     New frame from video source.
        public Bitmap Frame => frame;

        private double value;
        // Motion value
        public double Value => value;

        private State state;
        public State State => state;
        //
        // Summary:
        //     Initializes a new instance of the AForge.Video.NewFrameEventArgs class.
        //
        // Parameters:
        //   frame:
        //     New frame.
        public MotionEventArgs(Bitmap frame, double value, State state = State.NONE)
        {
            this.frame = frame;
            this.value = value;
            this.state = state;
        }
    }
}
