using System.Security;
using System.Windows;
using System.Windows.Input;
using Administration;
using GalaSoft.MvvmLight.Command;
using Pristjek220Data;
using SharedFunctionalities;
using RelayCommand = SharedFunctionalities.RelayCommand;

namespace Administration_GUI
{
    /// <summary>
    ///     LogInViewModel is the view model for the LogIn. Its used to open the Admin or the Storemanager view model
    /// </summary>
    internal class LogInViewModel : ObservableObject
    {
        private readonly ILogIn _logIn;
        private readonly IAutocomplete _autocomplete;
        private readonly IStoremanager _storemanager;
        private readonly IAdmin _admin;
        private ICommand _enterPressedCommand;
        private string _error;

        private ICommand _logInCommand;
        private Store _loginstore = new Store();

        /// <summary>
        ///     LogInViewModel constructor creates a LogIn and connects to the database 
        /// </summary>
        public LogInViewModel(IAutocomplete autocomplete, ILogIn logIn, IDatabaseFunctions databaseFunctions, IStoremanager storemanager, IAdmin admin)
        {
            _logIn = logIn;
            _autocomplete = autocomplete;
            _storemanager = storemanager;
            _admin = admin;

            if (databaseFunctions.ConnectToDb()) return;
            MessageBox.Show("Der kan ikke tilsluttes til serveren.", "ERROR", MessageBoxButton.OK);
            Application.Current.MainWindow.Close();
        }

        /// <summary>
        ///     Get and set method for the password of type SecureString
        /// </summary>
        public SecureString SecurePassword { private get; set; }

        /// <summary>
        ///     Get and set method for the Error, with OnPropertyChanged
        /// </summary>
        public string Error
        {
            get { return _error; }
            set
            {
                _error = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        ///     Get and set method for the Username
        /// </summary>
        public string Username { get; set; } = string.Empty;


        /// <summary>
        ///     Command that is used to log in to either the Admin or the Storemanger, if anything goes wrong it will print the reason to why it
        ///     did not log in to a label
        /// </summary>
        public ICommand LogInCommand => _logInCommand ??
                                        (_logInCommand = new RelayCommand(LogInbutton));

        /// <summary>
        ///     Command that is used to see if Enter is pressed, if its pressed it calls the LogInbutton
        /// </summary>
        public ICommand EnterKeyPressedCommand
            => _enterPressedCommand ?? (_enterPressedCommand = new RelayCommand<KeyEventArgs>(EnterKeyPressed));

        private void LogInbutton()
        {
            Error = string.Empty;
            var log = _logIn.CheckUsernameAndPassword(Username, SecurePassword, ref _loginstore);
            switch (log)
            {
                case 1:
                    if (_loginstore.StoreName == "Admin") // admin login lig med Storename Admin
                    {
                        ChangeWindowAdmin();
                    }
                    else
                    {

                        ChangeWindowStoremanager();
                    }
                    break;
                case 0:
                    Error = "Kodeordet er ugyldigt.";
                    break;
                case -1:
                    Error = "Brugernavnet er ugyldigt.";
                    break;
            }
        }

        private void ChangeWindowAdmin()
        {
            var logInGui = Application.Current.MainWindow;
            var adminGui = new Admin(_admin, _autocomplete);
            adminGui.Show();
            logInGui.Close();
            Application.Current.MainWindow = adminGui;
        }

        private void ChangeWindowStoremanager()
        {
            var logInGui = Application.Current.MainWindow;
            _storemanager.Store = _loginstore;
            var storemanagerGui = new StoremanagerGUI(_autocomplete, _storemanager);
            storemanagerGui.Show();
            logInGui.Close();
            Application.Current.MainWindow = storemanagerGui;
        }

        private void EnterKeyPressed(KeyEventArgs e)
        {
            if ((e.Key == Key.Enter) || (e.Key == Key.Return))
            {
                LogInbutton();
            }
        }
    }
}