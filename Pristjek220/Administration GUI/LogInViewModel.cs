using System;
using System.Runtime.InteropServices;
using System.Security;
using System.Windows;
using System.Windows.Input;
using Administration;
using Administration_GUI;
using Pristjek220Data;

namespace Administration_GUI
{
    class LogInViewModel
    {
        
        public SecureString SecurePassword { private get; set; }

        public string Username { get; set; } = string.Empty;

        private Administration.ILogIn _user;

        public LogInViewModel()
        {
            _user = new Administration.LogIn();
        }

        private ICommand _logInCommand;

        public ICommand LogInCommand
        {
            get
            {
                return _logInCommand ??
                       (_logInCommand = new RelayCommand(LogInbutton));
            }
        }

        private void LogInbutton()
        {
           _user.CheckUsernameAndPassword(Username, SecurePassword);
        }

        private ICommand _changeWindowAdminCommand;

        public ICommand ChangeWindowAdminCommand
        {
            get
            {
                return _changeWindowAdminCommand ??
                       (_changeWindowAdminCommand = new RelayCommand(ChangeWindowAdmin));
            }
        }

        private void ChangeWindowAdmin()
        {
            var LogInGui = Application.Current.MainWindow;
            Admin adminGUI = new Admin();
            adminGUI.Show();
            LogInGui.Close();
        }

        private ICommand _changeWindowStoremanagerCommand;

        public ICommand ChangeWindowStoremanagerCommand
        {
            get
            {
                return _changeWindowStoremanagerCommand ??
                       (_changeWindowStoremanagerCommand = new RelayCommand(ChangeWindowStoremanager));
            }
        }

        private void ChangeWindowStoremanager()
        {
            var LogInGui = Application.Current.MainWindow;
            StoremanagerGUI storemanagerGUI = new StoremanagerGUI();
            storemanagerGUI.Show();
            LogInGui.Close();
        }
    }
}
