using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace AtmCommon.ViewModels {
    public class TemplateViewModel : DependencyObject, INotifyPropertyChanged {
        // Implement INotifyPropertyChanged interface

#if NETCOREAPP3_0_OR_GREATER
        public event PropertyChangedEventHandler? PropertyChanged;
#else
        public event PropertyChangedEventHandler PropertyChanged;
#endif

#if NETCOREAPP3_0_OR_GREATER
        protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null) {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
#else
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null) {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
#endif

        private Visibility _dialogVisibility = Visibility.Hidden;
        public Visibility DialogVisibility {
            get => _dialogVisibility;
            set {
                _dialogVisibility = value;
                OnPropertyChanged();
            }
        }
    }
}
