using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using AtmCommon.ViewModels;

namespace AtmLoader.ViewModels
{
    public class CheckList : Blank
    {
        private ObservableCollection<CheckItem> _checkItems = new ObservableCollection<CheckItem>();
        public ObservableCollection<CheckItem> CheckItems
        {
            get => _checkItems;
            set { _checkItems = value; OnPropertyChanged(); }
        }

        public void UpdateCheckList( string title, CheckItem item)
        {
            for ( int i = 0; i < _checkItems.Count; i++ )
            {
                if (_checkItems[i].Title == title )
                {
                    _checkItems[i] = item;
                    break;
                }
            }

            OnPropertyChanged();
        }
    }

    public class CheckItem
    {
        public string Title { get; set; } = "";
        public string Description { get; set; } = "";
        public string IconPath { get; set; } = "";
        public string Tag { get; set; } = "";
        public Visibility Button_Visibility { get; set; } = Visibility.Visible;
    }
}
