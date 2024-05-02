using AtmLib.CheckList;
using AtmLib.Tracing;
using AtmLoader.ViewModels;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using AForge.Video;
using AForge.Video.DirectShow;
using AForge.Vision.Motion;
using System.Windows.Forms;
using MessageBox = System.Windows.Forms.MessageBox;
using System.IO;

namespace AtmLoader.Pages {
    /// <summary>
    /// Interaction logic for Configuration.xaml
    /// </summary>
    public partial class Camera : Page {

        public Camera() {
            using (var log = new Logger(TraceLevel.Verbose, "Camera.Camera"))
            {
                InitializeComponent();

                // Add new frame event handler for capturing camera
                AtmLoader.Core.Camera.NewFrameEvent += new NewFrameEventHandler(Camera_NewFrame);
                AtmLoader.Core.Camera.MotionEvent += new AtmLoader.Core.Camera.MotionEventHandler(Camera_Motion);

                ViewModel.ActiveCamera = AtmLoader.Core.Camera.CurrentCameraName;
                log.WriteLine(TraceLevel.Verbose, "Component is initialized");
            }
        }

        private void Camera_NewFrame(object sender, NewFrameEventArgs eventArgs)
        {
            try
            {
                Bitmap currentFrame = (Bitmap)eventArgs.Frame;
                Dispatcher.BeginInvoke(new Action(() =>
                {
                    ViewModel.CameraImage = ConvertBitmapToBitmapImage(currentFrame);
                }));
            }
            catch (Exception ex)
            {
                Logger.WriteException(ex);
            }
        }

        private void Camera_Motion(object sender, AtmLoader.Core.MotionEventArgs eventArgs)
        {
            // Bitmap currentFrame = (Bitmap)eventArgs.Frame.Clone();
            // double value = eventArgs.Value;

            Dispatcher.BeginInvoke(new Action(() =>
            {
                ViewModel.Status = String.Format("{0}, {1}", eventArgs.State, eventArgs.Value.ToString($"F{2}"));
            }));
        }

        private BitmapImage ConvertBitmapToBitmapImage(Bitmap bitmap)
        {
            using (MemoryStream memoryStream = new MemoryStream())
            {
                bitmap.Save(memoryStream, System.Drawing.Imaging.ImageFormat.Png);

                BitmapImage bitmapImage = new BitmapImage();
                bitmapImage.BeginInit();
                bitmapImage.StreamSource = new MemoryStream(memoryStream.ToArray());
                bitmapImage.EndInit();

                return bitmapImage;
            }
        }

        // Do Upload Action Here
        private void UploadButton_Click(object sender, RoutedEventArgs e)
        {
            AtmLoader.Core.Camera.Upload();
        }

        // Do Change Camera Action Here
        private void ChangeCamButton_Click(object sender, RoutedEventArgs e)
        {
            ViewModel.ActiveCamera = AtmLoader.Core.Camera.ChangeCamera();
        }

        public ViewModels.Camera ViewModel {
            get => (ViewModels.Camera)DataContext;
        }

        public static string Path { get; } = $"Pages/Camera.xaml";
        public static string LinkName { get; } = "Camera";
        public static bool IsNavigationEnabled { get; } = true;

        private void basePage_Loaded(object sender, RoutedEventArgs e) {
            // Additional programmatic changes to the template controls may be 
            // performed here.
        }

        private void Refresh()
        {

        }


        public static RoutedCommand OkButton_Command = new RoutedCommand();

        public void OkButton_Executed(object sender, ExecutedRoutedEventArgs e)
        {

        }

        public void OkButton_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }

        public static RoutedCommand CancelButton_Command = new RoutedCommand();

        public void CancelButton_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            ViewModel.DialogVisibility = Visibility.Hidden;
        }

        public void CancelButton_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }
    }
}
