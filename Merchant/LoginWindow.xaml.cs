using MerchantService;
using MerchantService.Models;
using System;
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
using System.Windows.Shapes;

namespace Merchant
{
    /// <summary>
    /// Interaction logic for LoginWindow.xaml
    /// </summary>
    public partial class LoginWindow : Window
    {
        public LoginWindow()
        {
            InitializeComponent();
            UpdateDateLabel();
            Loaded += LoginWindow_Loaded;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {

        }
        private void LoginWindow_Loaded(object sender, RoutedEventArgs e)
        {
            // Set the window startup location to CenterScreen when the window is loaded
            WindowStartupLocation = WindowStartupLocation.CenterScreen;
            // Set focus to the TextBox when the window is loaded
            txtUserName.Focus();

            ////test bypass login -- remove it 
            //MainWindow mainWindow = new MainWindow();
            //mainWindow.Show();
            //Close();

        }

        private void UpdateDateLabel()
        {
            // Get the current date and format it with the day name
            string currentDate = DateTime.Now.ToString("dddd, MMMM dd, yyyy");

            // Set the formatted date as the content of the label
            lblCurrentDate.Content = currentDate;
        }

        private async void btnLogin_Click(object sender, RoutedEventArgs e)
        {
            StringBuilder errorMessage = new StringBuilder();
            try
            {
                ToggleLoader(true);
                var userName = txtUserName.Text.Trim().ToLower();
                var password = txtPassword.Password;

                if (string.IsNullOrEmpty(userName)) errorMessage.AppendLine("Invalid User Name.");
                if (string.IsNullOrEmpty(password)) errorMessage.AppendLine("Invalid Password.");
                if (errorMessage.Length == 0)
                {
                    UserService fetchUser = new UserService();
                    var isValidUser = await fetchUser.ValidateLoginAsync(userName, password);
                    if (isValidUser == null)
                    {
                        errorMessage.AppendLine("Invalid password. Kindly try again.");
                    }
                    else
                    {
                        UserSession.Instance.Username = isValidUser.UserName;
                        UserSession.Instance.UserId = isValidUser.Id;

                        MainWindow mainWindow = new MainWindow();
                        mainWindow.Show();
                        Close();
                    }
                }
            }
            catch (Exception ex)
            {
                errorMessage.AppendLine($"There is a technical error {ex.InnerException}");
            }
            ToggleLoader(false);
            if (errorMessage.Length > 0)
                MessageBox.Show(errorMessage.ToString());
        }

        private void ToggleLoader(bool IsVisible)
        {
            if (IsVisible)
                loader.Visibility = Visibility.Visible;
            else
                loader.Visibility = Visibility.Collapsed;
        }

        private void TextBox_Loaded(object sender, RoutedEventArgs e)
        {
            // Calculate the vertical offset for centering the text
            TextBox textBox = (TextBox)sender;
            double verticalOffset = (textBox.ActualHeight - textBox.FontSize) / 6;

            // Set the top padding to vertically center the text
            textBox.Padding = new Thickness(0, verticalOffset, 0, 0);
        }

        private void PasswordBox_Loaded(object sender, RoutedEventArgs e)
        {
            // Calculate the vertical offset for centering the text
            PasswordBox textBox = (PasswordBox)sender;
            double verticalOffset = (textBox.ActualHeight - textBox.FontSize) / 6;

            // Set the top padding to vertically center the text
            textBox.Padding = new Thickness(0, verticalOffset, 0, 0);
        }

        private void Login_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                // Simulate a click on the login button when Enter is pressed
                btnLogin_Click(sender, e);
            }
        }
    }
}
