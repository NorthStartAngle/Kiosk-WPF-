﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
using AtmCommon;
using AtmCommon.ViewModels;

namespace AtmUi.Templates {
    /// <summary>
    /// Interaction logic for Form.xaml
    /// </summary>
    public partial class Form : UserControl {
        public Form() {
            InitializeComponent();
        }

        public AtmCommon.ViewModels.Form ViewModel {
            get => (AtmCommon.ViewModels.Form)DataContext;
        }

        new public static readonly DependencyProperty ContentProperty =
            DependencyProperty.Register("Content", typeof(object), typeof(Form), 
                new PropertyMetadata(defaultValue: null));

        new public object Content {
            get { return GetValue(ContentProperty); }
            set { SetValue(ContentProperty, value); }
        }

        private void Base_Loaded(object sender, RoutedEventArgs e) {
            if (ViewModel.Button1_Command == null) {
                ViewModel.Button1_Command = Button1_Command;
            }

            if (ViewModel.BackButton_Command == null) {
                ViewModel.BackButton_Command = BackButton_Command;
            }
        }

        public static RoutedCommand Button1_Command = new RoutedCommand();

        private void Button1_CommandExecuted(object sender, ExecutedRoutedEventArgs e) {
            if (ViewModel.Button1_Link != null) {
                Navigator.Navigate(this, ViewModel.Button1_Link);
            }
        }

        private void Button1_CommandCanExecute(object sender, CanExecuteRoutedEventArgs e) {
            e.CanExecute = ViewModel.Button1_Link != null;
        }

        public static RoutedCommand BackButton_Command = new RoutedCommand();

        private void BackButton_CommandExecuted(object sender, ExecutedRoutedEventArgs e) {
            Navigator.GoBack(this);
        }

        private void BackButton_CommandCanExecute(object sender, CanExecuteRoutedEventArgs e) {
            e.CanExecute = true;
        }
    }
}
