﻿using System;
using System.Security;
using System.Windows;
using System.Windows.Input;
using Administration;
using GalaSoft.MvvmLight.Command;
using Pristjek220Data;
using SharedFunctionalities;

namespace Administration_GUI
{
    class LogInViewModel: ObservableObject
    {
        
        public SecureString SecurePassword { private get; set; }
        private Store _loginstore;
        private readonly IUnitOfWork _unit;
        private ICommand _enterPressedCommand;

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
            _unit = new UnitOfWork(new DataContext());
            _user = new Administration.LogIn(_unit);

            IDatabaseFunctions databaseFunctions = new DatabaseFunctions(_unit);

            if (databaseFunctions.ConnectToDB()) return;
            MessageBox.Show("Der kan ikke tilsluttes til serveren.", "ERROR", MessageBoxButton.OK);
            Application.Current.MainWindow.Close();
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
                    Error = "Kodeordet er ugyldigt.";
                    break;
                case -1:
                    Error = "Brugernavnet er ugyldigt.";
                    break;
            }
        }

        private void ChangeWindowAdmin()
        {
            var LogInGui = Application.Current.MainWindow;
            var adminGUI = new Admin(_unit);
            adminGUI.Show();
            LogInGui.Close();
            Application.Current.MainWindow = adminGUI;
        }

        private void ChangeWindowStoremanager()
        {
            var logInGui = Application.Current.MainWindow;
            var storemanagerGUI = new StoremanagerGUI(_loginstore, _unit);
            storemanagerGUI.Show();
            logInGui.Close();
            Application.Current.MainWindow = storemanagerGUI;
        }

        public ICommand EnterKeyPressedCommand => _enterPressedCommand ?? (_enterPressedCommand = new RelayCommand<KeyEventArgs>(EnterKeyPressed));
        
        private void EnterKeyPressed(KeyEventArgs e)
        {
            if ((e.Key == Key.Enter) || (e.Key == Key.Return))
            {
                LogInbutton();
            }
        }
    }
}
