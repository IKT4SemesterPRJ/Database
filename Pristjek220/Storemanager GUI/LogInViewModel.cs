using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace Storemanager_GUI
{
    class LogInViewModel
    {
        
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
            MainWindow storemanagerGUI = new MainWindow();
            storemanagerGUI.Show();
            LogInGui.Close();
        }

    }
}
