using System.Windows;
using System.Windows.Controls;

namespace Administration_GUI
{
    /// <summary>
    /// Interaction logic for LogIn.xaml
    /// </summary>
    public partial class LogIn : Window
    {
        /// <summary>
        /// 
        /// </summary>
        public LogIn()
        {
            InitializeComponent();
        }

        private void PasswordBox_OnPasswordChanged(object sender, RoutedEventArgs e)
        {
            if (DataContext != null)
            {
                ((dynamic) DataContext).SecurePassword = ((PasswordBox) sender).SecurePassword;
            }
        }
    }
}
