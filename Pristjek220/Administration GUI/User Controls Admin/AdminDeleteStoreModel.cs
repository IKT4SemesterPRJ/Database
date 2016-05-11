using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Forms;
using System.Windows.Input;
using Administration;
using Administration_GUI;
using GalaSoft.MvvmLight.Command;
using Pristjek220Data;
using SharedFunctionalities;
using KeyEventArgs = System.Windows.Input.KeyEventArgs;

namespace Administration_GUI.User_Controls_Admin
{
    class AdminDeleteStoreModel : ObservableObject, IPageViewModelAdmin
    {
        private readonly IAutocomplete _autocomplete;
        private readonly Administration.Admin _admin;
        private ICommand _deleteFromLoginDatabaseCommand;
        private ICommand _populatingDeleteStoreCommand;
        private ICommand _illegalSignDeleteProductCommand;
        private ICommand _enterPressedCommand;

        public ICommand DeleteFromLoginDatabaseCommand
            => _deleteFromLoginDatabaseCommand ?? (_deleteFromLoginDatabaseCommand = new RelayCommand(DeleteStore));

        public ICommand PopulatingDeleteStoreCommand
            =>
                _populatingDeleteStoreCommand ??
                (_populatingDeleteStoreCommand = new RelayCommand(PopulatingListDeleteStore));

        public ICommand EnterKeyPressedCommand
            => _enterPressedCommand ?? (_enterPressedCommand = new RelayCommand<KeyEventArgs>(EnterKeyPressed));

        public ICommand IllegalSignDeleteStoreCommand
            =>
                _illegalSignDeleteProductCommand ??
                (_illegalSignDeleteProductCommand = new RelayCommand(IllegalSignDeleteStore));

        private string _oldtext = string.Empty;
        private string _error = string.Empty;
        private string _deleteStoreName;

        public string DeleteStoreName
        {
            get { return _deleteStoreName; }
            set
            {
                _oldtext = _deleteStoreName;
                _deleteStoreName = value;
                OnPropertyChanged();
            }
        }

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

        public ObservableCollection<string> AutoCompleteList { get; } = new ObservableCollection<string>();
        private void PopulatingListDeleteStore()
        {
            AutoCompleteList?.Clear();
            foreach (var item in _autocomplete.AutoCompleteStore(DeleteStoreName))
            {
                AutoCompleteList?.Add(item);
            }
            OnPropertyChanged("AutoCompleteList");
        }

        private void IllegalSignDeleteStore()
        {
            if (DeleteStoreName == null) return;
            if (DeleteStoreName.All(chr => char.IsLetter(chr) || char.IsNumber(chr) || char.IsWhiteSpace(chr))) return;
            IsTextConfirm = false;
            Error = $"Der kan kun skrives bogstaverne fra a til å og tallene fra 0 til 9.";
            DeleteStoreName = _oldtext;
        }

        public string Error
        {
            get { return _error; }
            set
            {
                _error = value;
                OnPropertyChanged();
            }
        }

        public AdminDeleteStoreModel(IUnitOfWork unit)
        {
            _admin = new Administration.Admin(unit);
            _autocomplete = new SharedFunctionalities.Autocomplete(unit);
        }

        private void DeleteStore()
        {
            if (string.IsNullOrEmpty(DeleteStoreName))
            {
                IsTextConfirm = false;
                Error = "Indtast venligst navnet på den forretning der skal fjernes.";
                return;
            }

            var storeName = char.ToUpper(DeleteStoreName[0]) + DeleteStoreName.Substring(1).ToLower();

            var result = CustomMsgBox.Show($"Vil du fjerne forretningen \"{storeName}\" fra Pristjek220?", "Bekræftelse", "Ja",
                "Nej");
            if (result != DialogResult.Yes)
            {
                IsTextConfirm = false;
                Error = "Der blev ikke bekræftet.";
                return;
            }

            if (_admin.DeleteStore(storeName) == -1)
            {
                IsTextConfirm = false;
                Error = "Forretningen findes ikke i Pristjek220.";
                return;
            }
            IsTextConfirm = true;
            Error = ($"Forretningen \"{storeName}\" er blevet fjernet fra Pristjek220.");
        }

        private void EnterKeyPressed(KeyEventArgs e)
        {
            if((e.Key == Key.Enter || e.Key == Key.Return))
                DeleteStore();
        }
    }
}
