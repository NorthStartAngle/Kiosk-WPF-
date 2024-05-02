using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AtmUi {
    public class AtmViewModel : INotifyPropertyChanged {
        private string? _location;
        public string? Location { 
            get => _location;
            set {
                _location = value;
                OnPropertyChanged(nameof(Location));
            }
        }

        public void Update(AtmApi.InstanceData instanceData) {
            Location = instanceData.location;
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName) {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

    }
}
