using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using AtmCommon.ViewModels;

namespace AtmLoader.ViewModels {
    public class DataGridSample : Base {
        public DataGridSample() {
            Users = new ObservableCollection<User> {
                new User { Name = "Peter", Authority = "A", Permission = "P1", IsLogin = "Yes" },
                new User { Name = "Paul", Authority = "B", Permission = "P2", IsLogin = "No" },
                new User { Name = "Mary", Authority = "C", Permission = "P3", IsLogin = "Yes" },
            };
        }

        public ObservableCollection<User> Users { get; set; }
    }

    public class User {
        public string Name { get; set; }
        public string Authority { get; set; }
        public string Permission { get; set; }
        public string IsLogin { get; set; }
    }
}
