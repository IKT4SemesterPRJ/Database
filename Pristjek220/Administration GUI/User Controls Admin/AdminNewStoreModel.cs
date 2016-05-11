using System.Security;
using System.Timers;
using System.Windows;
using System.Windows.Input;
using Administration_GUI;
using GalaSoft.MvvmLight.Command;
using Pristjek220Data;

namespace Administration_GUI.User_Controls_Admin
{
    class AdminNewStoreModel : ObservableObject, IPageViewModelAdmin
    {
        private readonly Timer _timer = new Timer(2500);
        public string NewStoreName { get; set; }
        public SecureString SecurePassword { private get; set; }
        public SecureString SecurePasswordConfirm { private get; set; }
        private readonly Administration.Admin _admin;
        public string Error
        {
            get { return _error; }
            set
            {
                _error = value;
                OnPropertyChanged();
                _timer.Stop();
                _timer.Start();
                _timer.Elapsed += delegate { _error = ""; OnPropertyChanged(); };
            }
        }
        private string _error = string.Empty;

        private bool _isTextConfirm;
        public bool IsTextConfirm
        {
            get { return _isTextConfirm; }
            set
            {
                _isTextConfirm = value;
                OnPropertyChanged();
            }
        }

        public AdminNewStoreModel(IUnitOfWork unit)
        {
             _admin = new Administration.Admin(unit);
        }


        private ICommand _newStoreCommand;
        private ICommand _enterPressedCommand;

        public ICommand NewStoreCommand => _newStoreCommand ??
                                                    (_newStoreCommand = new RelayCommand(NewStore));

        private void NewStore()
        {
            if (string.IsNullOrEmpty(NewStoreName))
            {
                IsTextConfirm = false;
                Error = "Udfyld venligst alle felter.";
                return;
            }
            if (_admin.CheckPasswords(SecurePassword, SecurePasswordConfirm) != 0)
            {
                switch (_admin.CreateLogin(NewStoreName, SecurePassword, NewStoreName))
                {
                    case -1:
                        IsTextConfirm = false;
                        Error = "Forretningen findes allerede.";
                        break;
                    case -2:
                        IsTextConfirm = false;
                        Error = "Udfyld venligst alle felter.";
                        break;
                    default:
                        IsTextConfirm = true;
                        Error = $"Forretning oprettet med forretningsnavnet \"{NewStoreName}\".";
                        break;
                }
            }
            else
            {
                IsTextConfirm = false;
                Error = "Kodeordene matcher ikke.";
            }
        }

        public ICommand EnterKeyPressedCommand => _enterPressedCommand ?? (_enterPressedCommand = new RelayCommand<KeyEventArgs>(EnterKeyPressed));

        private void EnterKeyPressed(KeyEventArgs e)
        {
            if ((e.Key == Key.Enter) || (e.Key == Key.Return))
            {
                NewStore();
            }
        }

    }
}
