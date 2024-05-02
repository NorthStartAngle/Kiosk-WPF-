using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows;
using AtmCommon;
using AtmCommon.ViewModels;

namespace AtmUi.ViewModels {
    public class SampleExplorer : OptionScreen {
        private string _textContent = """
            This content you are reading in this text block comes from the derived view-model class via binding. 
            I created a new view-model class derived from AtmUi.ViewModels.OptionScreen and called it 
            AtmUi.ViewModels.SampleExplorer. Any binding properties specific to this page will go into the 
            SampleExplorer view model.
            """;

        public string TextContent {
            get => _textContent;
            set { _textContent = value; OnPropertyChanged(); }
        }
    }
}
