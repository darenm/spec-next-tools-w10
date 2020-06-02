using SpecNextTiler.ViewModel;
using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;

// The User Control item template is documented at https://go.microsoft.com/fwlink/?LinkId=234236

namespace SpecNextTiler.Controls
{
    public sealed partial class LogControl : UserControl
    {
        public LogViewModel Log
        {
            get { return (LogViewModel)GetValue(LogProperty); }
            set { SetValue(LogProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Log.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty LogProperty =
            DependencyProperty.Register("Log", typeof(LogViewModel), typeof(LogControl), new PropertyMetadata(null, LogChanged));

        private static void LogChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is LogControl logControl)
            {
                ScrollToBottom(logControl.LogTextBox);
            }
        }

        private static void ScrollToBottom(TextBox textBox)
        {
            var grid = (Grid)VisualTreeHelper.GetChild(textBox, 0);
            if (grid != null)
            {
                for (var i = 0; i <= VisualTreeHelper.GetChildrenCount(grid) - 1; i++)
                {
                    object obj = VisualTreeHelper.GetChild(grid, i);
                    if (!(obj is ScrollViewer)) continue;
                    ((ScrollViewer)obj).ChangeView(0.0f, ((ScrollViewer)obj).ExtentHeight, 1.0f, true);
                    break;
                }
            }
        }

        public LogControl()
        {
            this.InitializeComponent();
            Loaded += LogControl_Loaded;
        }

        private void LogControl_Loaded(object sender, RoutedEventArgs e)
        {
            LogTextBox.TextChanged += LogTextBox_TextChanged;
        }

        private void LogTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            ScrollToBottom(sender as TextBox);
        }
    }
}
