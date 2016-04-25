using System;
using System.Security;
using System.Windows;
using System.Windows.Input;
using Administration;
using Pristjek220Data;

namespace Administration_GUI
{
    class LogInViewModel: ObservableObject
    {
        
        public SecureString SecurePassword { private get; set; }
        private Store _loginstore;

        public string Error
        {
            get { return _error; }
            set
            {
                _error = value;
                OnPropertyChanged();
            }
        }
        private string _error;

        public string Username { get; set; } = string.Empty;

        private readonly ILogIn _user;

        public LogInViewModel()
        {
            _user = new Administration.LogIn();
        }

        private ICommand _logInCommand;

        public ICommand LogInCommand => _logInCommand ??
                                        (_logInCommand = new RelayCommand(LogInbutton));

        private void LogInbutton()
        {
            Error = string.Empty;
            var log = _user.CheckUsernameAndPassword(Username, SecurePassword, ref _loginstore);
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
                    Error = "Password ugyldigt";
                    break;
                case -1:
                    Error = "Username ugyldigt";
                    break;
            }
        }

        private ICommand _changeWindowAdminCommand;

        public ICommand ChangeWindowAdminCommand => _changeWindowAdminCommand ??
                                                    (_changeWindowAdminCommand = new RelayCommand(ChangeWindowAdmin));

        private void ChangeWindowAdmin()
        {
            var LogInGui = Application.Current.MainWindow;
            Admin adminGUI = new Admin();
            adminGUI.Show();
            LogInGui.Close();
            Application.Current.MainWindow = adminGUI;
        }

        private ICommand _changeWindowStoremanagerCommand;

        public ICommand ChangeWindowStoremanagerCommand => _changeWindowStoremanagerCommand ??
                                                           (_changeWindowStoremanagerCommand = new RelayCommand(ChangeWindowStoremanager));

        private void ChangeWindowStoremanager()
        {
            var logInGui = Application.Current.MainWindow;
            StoremanagerGUI storemanagerGUI = new StoremanagerGUI(_loginstore);
            storemanagerGUI.Show();
            logInGui.Close();
            Application.Current.MainWindow = storemanagerGUI;
        }
    }
}
