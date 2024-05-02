using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media.Imaging;
using AtmCommon.ViewModels;

namespace AtmLoader.ViewModels {
    public class Camera : Blank
    {
        private string _activeCamera = "N/A";
        public string ActiveCamera
        {
            get => _activeCamera;
            set { _activeCamera = value; OnPropertyChanged(); }
        }

        private string _status = "N/A";
        public string Status
        {
            get => _status;
            set { _status = value; OnPropertyChanged(); }
        }

        private BitmapImage _cameraImage = null;
        public BitmapImage CameraImage
        {
            get => _cameraImage;
            set { _cameraImage = value; OnPropertyChanged(); }
        }

        //private Image _cameraImage = new Image();


        private string _message_Foreground = "white";
        public string Message_Foreground
        {
            get => _message_Foreground;
            set { _message_Foreground = value; OnPropertyChanged(); }
        }

        private string _dialogScript = "";
        public string DialogScript
        {
            get => _dialogScript;
            set { _dialogScript = value; OnPropertyChanged(); }
        }
    }
}
