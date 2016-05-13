using System.Windows;
using System.Windows.Controls;

namespace Administration_GUI.User_Controls_Admin
{
    /// <summary>
    /// Interaction logic for AdminNewStore.xaml
    /// </summary>
    public partial class AdminNewStore
    {
        /// <summary>
        /// 
        /// </summary>
        public AdminNewStore()
        {
            InitializeComponent();
        }

        private void PasswordBox_OnPasswordChanged(object sender, RoutedEventArgs e)
        {
            if (DataContext != null)
            {
                ((dynamic)DataContext).SecurePassword = ((PasswordBox)sender).SecurePassword;
            }
        }


        private void PasswordBox_OnPasswordConfirmChanged(object sender, RoutedEventArgs e)
        {
            if (DataContext != null)
            {
                ((dynamic)DataContext).SecurePasswordConfirm = ((PasswordBox)sender).SecurePassword;
            }
        }
    }
}
