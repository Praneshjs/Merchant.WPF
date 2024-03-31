using System.Timers;
using System.Windows;
using System.Windows.Controls;

namespace Merchant.Controls
{
    /// <summary>
    /// Interaction logic for ValidationPlaceholderControl.xaml
    /// </summary>
    public partial class ValidationPlaceholderControl : UserControl
    {
        private Timer timer;
        public ValidationPlaceholderControl()
        {
            InitializeComponent();

            timer = new Timer(3000);
            timer.Elapsed += TimerElapsed;
        }

        private void TimerElapsed(object sender, ElapsedEventArgs e)
        {
            Dispatcher.Invoke(() => ValidationPlaceholder.Visibility = Visibility.Collapsed);
            timer.Stop();
        }
        public void ShowValidationBox(string message)
        {
            ValidationPlaceholder.Visibility = Visibility.Visible;
            ((TextBlock)((Grid)ValidationPlaceholder.Child).Children[0]).Text = message;
            timer.Start();
        }

        public void CloseValidationBox_Click(object sender, RoutedEventArgs e)
        {
            // Close button click event handler
            ValidationPlaceholder.Visibility = Visibility.Collapsed;
            timer.Stop(); // Stop the timer
        }
    }
}
