using System.Windows;

namespace AtmUi
{
    public static class PageProperties
    {
        // ... existing code ...

        public static readonly DependencyProperty UserNameProperty =
            DependencyProperty.RegisterAttached(
                "DialogText",
                typeof(string),
                typeof(PageProperties),
                new PropertyMetadata(null));

        public static readonly DependencyProperty TransactionProperty =
            DependencyProperty.RegisterAttached(
                "Transaction",
                typeof(string),
                typeof(PageProperties),
                new PropertyMetadata(null));

        public static readonly DependencyProperty TimeProperty =
            DependencyProperty.RegisterAttached(
                "Time",
                typeof(string),
                typeof(PageProperties),
                new PropertyMetadata(null));

        public static readonly DependencyProperty DateProperty =
            DependencyProperty.RegisterAttached(
                "Date",
                typeof(string),
                typeof(PageProperties),
                new PropertyMetadata(null));

        public static readonly DependencyProperty LocationProperty =
            DependencyProperty.RegisterAttached(
                "Location",
                typeof(string),
                typeof(PageProperties),
                new PropertyMetadata(null));

        public static string GetUserName(DependencyObject obj)
        {
            return (string)obj.GetValue(UserNameProperty);
        }

        public static void SetUserName(DependencyObject obj, string? value)
        {
            obj.SetValue(UserNameProperty, value);
        }

        public static string GetTransaction(DependencyObject obj)
        {
            return (string)obj.GetValue(TransactionProperty);
        }

        public static void SetTransaction(DependencyObject obj, string? value)
        {
            obj.SetValue(TransactionProperty, value);
        }

        public static string GetTime(DependencyObject obj)
        {
            return (string)obj.GetValue(TimeProperty);
        }

        public static void SetTime(DependencyObject obj, string? value)
        {
            obj.SetValue(TimeProperty, value);
        }

        public static string GetDate(DependencyObject obj)
        {
            return (string)obj.GetValue(DateProperty);
        }

        public static void SetDate(DependencyObject obj, string? value)
        {
            obj.SetValue(DateProperty, value);
        }

        public static string GetLocation(DependencyObject obj)
        {
            return (string)obj.GetValue(LocationProperty);
        }

        public static void SetLocation(DependencyObject obj, string? value)
        {
            obj.SetValue(LocationProperty, value);
        }
    }
}
