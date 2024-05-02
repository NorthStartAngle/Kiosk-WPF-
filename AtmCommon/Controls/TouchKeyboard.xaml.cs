using System;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Collections.Generic;
using System.Windows.Input;

// The User Control item template is documented at https://go.microsoft.com/fwlink/?LinkId=234236

namespace AtmCommon.Controls {
    public sealed partial class TouchKeyboard : UserControl {
        public TouchKeyboard() {
            this.InitializeComponent();
        }

        public static readonly DependencyProperty FocusedControlProperty =
            DependencyProperty.Register("FocusedControl", typeof(Control), typeof(TouchKeyboard),
                new PropertyMetadata(defaultValue: null));

#if NETCOREAPP3_0_OR_GREATER
        public Control? FocusedControl {
#else
        public Control FocusedControl {
#endif
            get => (Control)GetValue(FocusedControlProperty);
            set => SetValue(FocusedControlProperty, value);
        }

        private List<Control> Controls { get; set; } = new List<Control>();

        private int _maxTabIndex = int.MaxValue;

        public void RegisterControl(Control control) {
            Controls.Add(control);
            Controls.Sort((c1, c2) => c1.TabIndex.CompareTo(c2.TabIndex));
            control.GotFocus += Control_GotFocus;

            if (control.TabIndex <= _maxTabIndex) {
                FocusedControl = control;
                _maxTabIndex = control.TabIndex;
            }
        }

        private void Control_GotFocus(object sender, RoutedEventArgs e) {
            if (sender is Control control) {
                FocusedControl = control;
                Visibility = Visibility.Visible;
            }

            e.Handled = true;
        }

        private void Tab_Click(object sender, RoutedEventArgs e) {
            if (Controls.Count == 0) {
                return;
            }

            var current = FocusedControl as Control;
            int currentIndex = 0;

            if (current != null) {
                currentIndex = Controls.IndexOf(current);
            }

            if (IsShifted) {
                if (currentIndex == 0) {
                    currentIndex = Controls.Count - 1;
                }
                else {
                    --currentIndex;
                }
            }
            else {
                if (currentIndex == Controls.Count - 1) {
                    currentIndex = 0;
                }
                else { 
                    ++currentIndex;
                }
            }

            var control = Controls[currentIndex];
            // Move focus to the next control in the tab index
            control.Focus();
        }

        public static readonly DependencyProperty ShowCloseButtonProperty =
            DependencyProperty.Register("ShowCloseButton", typeof(bool), typeof(TouchKeyboard),
                new PropertyMetadata(true));

        public bool ShowCloseButton {
            get => (bool)GetValue(ShowCloseButtonProperty);
            set => SetValue(ShowCloseButtonProperty, value);
        }

        private bool ShiftLock { get; set; } = false;
        private bool CapsLock { get; set; } = false;

        private void Button_Click(object sender, RoutedEventArgs e) {
            // Here we will find a TextBox in the parent window to apply the input.
            var button = (Button)sender;
            var input = button.Tag.ToString();

            if (input == null) {
                return;
            }

            if (input.Length > 1 && input[0] == '\\') {
                input = input.Substring(1);
            }

            if (FocusedControl != null) {
                if (FocusedControl is TextBox textBox) {
                    TextBoxInput(textBox, input);
                }

                // FocusedControl.Focus();
            }

            if (IsShifted && !ShiftLock) {
                IsShifted = !IsShifted;
                UpdateKeyDisplay();
            }
        }

        private void TextBoxInput(TextBox textBox, string input) {
            int caretIndex = textBox.CaretIndex;

            if (input == "Back") {
                if (!string.IsNullOrEmpty(textBox.Text) && caretIndex > 0) {
                    textBox.Text = textBox.Text.Remove(caretIndex - 1, 1);
                    caretIndex--;
                }
            }
            else {
                textBox.Text = textBox.Text.Insert(caretIndex, input);
                caretIndex += input.Length;
            }

            // Clear the selection after input
            textBox.SelectionLength = 0;
            textBox.CaretIndex = caretIndex;
        }

        private DateTime LastShiftClick { get; set; } = DateTime.MinValue;

        private bool IsShifted { get; set; } = false;

        private SolidColorBrush bgShifted = new SolidColorBrush((Color)Application.Current.Resources["ShiftedColor"]);
        private SolidColorBrush bgUnshifted = new SolidColorBrush((Color)Application.Current.Resources["UnshiftedColor"]);

        [DllImport("user32.dll")]
        private static extern uint GetDoubleClickTime();

        private void Shift_Click(object sender, RoutedEventArgs e) {
            bool shiftLockPress = false;
            double doubleClickTime = (double)GetDoubleClickTime();
            var thisClick = DateTime.UtcNow;

            if (thisClick.Subtract(LastShiftClick).TotalMilliseconds <= doubleClickTime) {
                shiftLockPress = true;
            }

            LastShiftClick = DateTime.UtcNow;

            if (shiftLockPress && !ShiftLock) {
                ShiftLock = true;

                if (IsShifted) {
                    return;
                }
            }

            ShiftLock = false;
            IsShifted = !IsShifted;

            UpdateKeyDisplay();
        }

        private void UpdateKeyDisplay() {
            if (IsShifted) {
                Unshifted.Visibility = Visibility.Hidden;
                Shifted.Visibility = Visibility.Visible;
                Lowercase.Visibility = Visibility.Hidden;
                Uppercase.Visibility = Visibility.Visible;
                LeftShift.Background = bgShifted;
                RightShift.Background = bgShifted;
            }
            else {
                Unshifted.Visibility = Visibility.Visible;
                Shifted.Visibility = Visibility.Hidden;
                Lowercase.Visibility = Visibility.Visible;
                Uppercase.Visibility = Visibility.Hidden;
                LeftShift.Background = bgUnshifted;
                RightShift.Background = bgUnshifted;
            }

            if (CapsLock) {
                Lowercase.Visibility = Visibility.Hidden;
                Uppercase.Visibility = Visibility.Visible;
            }
        }

        private void CapsButton_Click(object sender, RoutedEventArgs e) {
            CapsLock = !CapsLock;

            if (CapsLock) {
                CapsButton.Background = bgShifted;
            }
            else {
                CapsButton.Background = bgUnshifted;
            }

            if (!(ShiftLock || IsShifted)) {
                if (CapsLock) {
                    Lowercase.Visibility = Visibility.Hidden;
                    Uppercase.Visibility = Visibility.Visible;
                }
                else {
                    Lowercase.Visibility = Visibility.Visible;
                    Uppercase.Visibility = Visibility.Hidden;
                }
            }

            if (FocusedControl != null) {
                FocusedControl.Focus();
            }
        }


        enum ArrowDirection {
            Up,
            Down,
            Left,
            Right
        }

        private void LeftButton_Click(object sender, RoutedEventArgs e) {
            if (FocusedControl != null) {
                if (FocusedControl is TextBox textBox) {
                    TextBoxArrow(textBox, ArrowDirection.Left);
                }

                FocusedControl.Focus();
            }
        }

        private void RightButton_Click(object sender, RoutedEventArgs e) {
            if (FocusedControl != null) {
                if (FocusedControl is TextBox textBox) {
                    TextBoxArrow(textBox, ArrowDirection.Right);
                }

                FocusedControl.Focus();
            }
        }

        private void UpButton_Click(object sender, RoutedEventArgs e) {
            if (FocusedControl != null) {
                if (FocusedControl is TextBox textBox) {
                    TextBoxArrow(textBox, ArrowDirection.Up);
                }

                FocusedControl.Focus();
            }
        }

        private void DownButton_Click(object sender, RoutedEventArgs e) {
            if (FocusedControl != null) {
                if (FocusedControl is TextBox textBox) {
                    TextBoxArrow(textBox, ArrowDirection.Down);
                }

                FocusedControl.Focus();
            }
        }

        private void TextBoxArrow(TextBox textBox, ArrowDirection arrowDirection) {
            var caretIndex = textBox.CaretIndex;

            switch (arrowDirection) {
                case ArrowDirection.Up:
                    break;
                case ArrowDirection.Down:
                    break;
                case ArrowDirection.Left:
                    if (caretIndex > 0) {
                        textBox.CaretIndex--;
                    }
                    break;
                case ArrowDirection.Right:
                    if (caretIndex < textBox.Text.Length) {
                        textBox.CaretIndex++;
                    }
                    break;
                default:
                    break;
            }
        }

        private void CloseButton_Click(object sender, RoutedEventArgs e) {
            this.Visibility = Visibility.Hidden;
        }

        private void TouchKeyboard_IsVisibleChanged(object sender, DependencyPropertyChangedEventArgs e) {
            if (e.NewValue is Visibility visibility) {
                if (visibility == Visibility.Visible) {
                    if (FocusedControl != null) {
                        FocusedControl.Focus();
                    }
                }
            }
        }
    }
}
