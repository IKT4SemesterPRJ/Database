using System.Security;
using System.Windows;
using System.Windows.Input;
using Administration_GUI;
using Pristjek220Data;

namespace Administration_GUI.User_Controls_Admin
{
    class AdminNewStoreModel : ObservableObject, IPageViewModelAdmin
    {
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
            }
        }
        private string _error = string.Empty;

        public AdminNewStoreModel(IUnitOfWork unit)
        {
             _admin = new Administration.Admin(unit);
        }


        private ICommand _newStoreCommand;

        public ICommand NewStoreCommand => _newStoreCommand ??
                                                    (_newStoreCommand = new RelayCommand(NewStore));

        private void NewStore()
        {
            if (_admin.CheckPasswords(SecurePassword, SecurePasswordConfirm) == 0)
            {
                Error = "Passwords matcher ikke";
            }

            else if (-1 == _admin.CreateLogin(NewStoreName, SecurePassword, NewStoreName))
            {
                Error = "Butik findes allerede";
            }
            else
            {
                Error = $"Forretning oprettet med brugernavnet {NewStoreName}";
            }
        }


    }
}
