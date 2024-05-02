using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AtmCommon.ViewModels {
    public class Web : Base {
#if NETCOREAPP3_0_OR_GREATER
        private Uri? _uri;
#else
        private Uri _uri;
#endif
#if NETCOREAPP3_0_OR_GREATER
        public Uri? WebSource {
#else
        public Uri WebSource {
#endif
            get => _uri;
            set {
                _uri = value;
                OnPropertyChanged();
            }
        }
    }
}
