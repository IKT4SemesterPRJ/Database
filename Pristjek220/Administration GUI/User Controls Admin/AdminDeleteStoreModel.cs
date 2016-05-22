using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Forms;
using System.Windows.Input;
using Administration;
using Pristjek220Data;
using SharedFunctionalities;
using KeyEventArgs = System.Windows.Input.KeyEventArgs;
using Timer = System.Timers.Timer;

namespace Administration_GUI.User_Controls_Admin
{

    /// <summary>
    ///     AdminDeleteStoreModel is the User Control model for the AdminDeleteStore User Control
    ///     Its used to delete a store
    /// </summary>
    internal class AdminDeleteStoreModel : ObservableObject, IPageViewModel
    {
        private readonly IAdmin _admin;
        private readonly IAutocomplete _autocomplete;
        private readonly Timer _timer = new Timer(2500);
        private ICommand _deleteFromLoginDatabaseCommand;
        private string _deleteStoreName;
        private ICommand _enterPressedCommand;
        private string _error = string.Empty;
        private ICommand _illegalSignDeleteProductCommand;
        private ICreateMsgBox _msgBox;

        private bool _isTextConfirm;

        private string _oldtext = string.Empty;
        private ICommand _populatingDeleteStoreCommand;

        /// <summary>
        ///     AdminDeleteStoreModel constructor takes a UnitOfWork to create an admin
        /// </summary>
        /// <param name="autocomplete"></param>
        /// <param name="msgBox"></param>
        /// <param name="admin"></param>
        public AdminDeleteStoreModel(IAdmin admin, IAutocomplete autocomplete, ICreateMsgBox msgBox)
        {
            _admin = admin;
            _autocomplete = autocomplete;
            _msgBox = msgBox;
        }

        /// <summary>
        ///     Command that is used to delete a store, if anything goes wrong it will print the reason to why it did not delete
        ///     the product to a label
        /// </summary>
        public ICommand DeleteFromLoginDatabaseCommand
            => _deleteFromLoginDatabaseCommand ?? (_deleteFromLoginDatabaseCommand = new RelayCommand(DeleteStore));

        /// <summary>
        ///     Command that is used whenever there is an Populating event to populate the dropdown menu with the correct stores
        /// </summary>
        public ICommand PopulatingDeleteStoreCommand
            =>
                _populatingDeleteStoreCommand ??
                (_populatingDeleteStoreCommand = new RelayCommand(PopulatingListDeleteStore));

        /// <summary>
        ///     Command that is used to see if Enter is pressed, if its pressed it calls the DeleteStore
        /// </summary>
        public ICommand EnterKeyPressedCommand
            => _enterPressedCommand ?? (_enterPressedCommand = new GalaSoft.MvvmLight.Command.RelayCommand<KeyEventArgs>(EnterKeyPressed));

        /// <summary>
        ///     Command that is used whenever there is an TextChanged event to see if the text entered contains illegal signs
        /// </summary>
        public ICommand IllegalSignDeleteStoreCommand
            =>
                _illegalSignDeleteProductCommand ??
                (_illegalSignDeleteProductCommand = new RelayCommand(IllegalSignDeleteStore));

        /// <summary>
        ///     Get and Set method for  DeleteStoreName. The set method, sets the old DeleteStoreName to an oldtext, and then
        ///     change the value to the new vaule and call OnPropertyChanged
        /// </summary>
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

        /// <summary>
        ///     Is a bool that is used to set the color of a label to red if it's a fail and green if it's expected behaviour
        /// </summary>
        public bool IsTextConfirm
        {
            get { return _isTextConfirm; }
            set
            {
                _isTextConfirm = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        ///     Get method for AutoCompleteList, that is the list with the items that is getting populated to the dropdown.
        /// </summary>
        public ObservableCollection<string> AutoCompleteList { get; } = new ObservableCollection<string>();

        /// <summary>
        ///     The Error string that is written to a label on the GUI describes if the event has been successfully or if it has
        ///     failed, and why it failed
        /// </summary>
        public string Error
        {
            get { return _error; }
            set
            {
                _error = value;
                OnPropertyChanged();
                _timer.Stop();
                _timer.Start();
                _timer.Elapsed += delegate
                {
                    _error = "";
                    OnPropertyChanged();
                };
            }
        }

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

        private void DeleteStore()
        {
            if (string.IsNullOrEmpty(DeleteStoreName))
            {
                IsTextConfirm = false;
                Error = "Indtast venligst navnet på den forretning der skal fjernes.";
                return;
            }

            var storeName = char.ToUpper(DeleteStoreName[0]) + DeleteStoreName.Substring(1).ToLower();
            var store = _admin.FindStore(storeName);

            if (store == null || store.StoreName == "Admin")
            {
                IsTextConfirm = false;
                Error = $"Forretningen \"{storeName}\" findes ikke i Pristjek220.";
            }
            else
            {
                var result = _msgBox.DeleteStoreMgsConfirmation(storeName);
                if (result != DialogResult.Yes)
                {
                    IsTextConfirm = false;
                    Error = "Der blev ikke bekræftet.";
                    return;
                }
                _admin.DeleteStore(storeName);
                IsTextConfirm = true;
                Error = $"Forretningen \"{storeName}\" er blevet fjernet fra Pristjek220.";
            }
        }

        private void EnterKeyPressed(KeyEventArgs e)
        {
            if (e.Key == Key.Enter || e.Key == Key.Return)
                DeleteStore();
        }
    }
}