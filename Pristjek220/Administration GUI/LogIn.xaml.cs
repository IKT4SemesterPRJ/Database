using System.Windows;
using System.Windows.Controls;
using Administration;
using Pristjek220Data;
using SharedFunctionalities;

namespace Administration_GUI
{
    /// <summary>
    /// Interaction logic for LogIn.xaml
    /// </summary>
    public partial class LogIn : Window
    {
        /// <summary>
        ///     Sets the datacontext, to the LogInViewModel with all the needed parameters; AutomComplete, Login, DatabaseFunctions, Storemanager, Admin
        /// </summary>
        public LogIn()
        {
            InitializeComponent();
            IUnitOfWork unit = new UnitOfWork(new DataContext());
            DataContext = new LogInViewModel(new Autocomplete(unit), new Administration.LogIn(unit), new DatabaseFunctions(unit), new Storemanager(unit, new Store()), new Administration.Admin(unit));
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
