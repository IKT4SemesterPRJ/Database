using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Forms;
using System.Windows.Input;
using Administration;
using SharedFunctionalities;
using KeyEventArgs = System.Windows.Input.KeyEventArgs;
using Timer = System.Timers.Timer;

namespace Administration_GUI.User_Controls
{
    /// <summary>
    ///     DeletePriceModel is the User Control model for the DeletePrice User Control
    ///     Its used to delete a product from a store
    /// </summary>
    public class DeleteProductModel : ObservableObject, IPageViewModel
    {
        private readonly IAutocomplete _autocomplete;
        private readonly IStoremanager _manager;
        private readonly Timer _timer = new Timer(2500);

        private string _confirmText;
        private ICommand _deleteFromStoreDatabaseCommand;
        private ICommand _enterPressedCommand;
        private ICommand _illegalSignDeleteProductCommand;
        private readonly ICreateMsgBox _msgBox;
        private bool _isTextConfirm;

        private string _oldtext = string.Empty;
        private ICommand _populatingDeleteProductCommand;

        private string _shoppingListItem;

        /// <summary>
        ///     DeletePriceModel constructor takes a UnitOfWork and a store to create a Storemanager and a AutoComplete
        /// </summary>
        /// <param name="storemanager"></param>
        /// <param name="autocomplete"></param>
        /// <param name="msgBox"></param>
        public DeleteProductModel(IStoremanager storemanager, IAutocomplete autocomplete, ICreateMsgBox msgBox)
        {
            _manager = storemanager;
            _autocomplete = autocomplete;
            _msgBox = msgBox;
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
        ///     Command that is used to delete a product from a store, if anything goes wrong it will print the reason to why it
        ///     did not delete the product to a label
        /// </summary>
        public ICommand DeleteFromStoreDatabaseCommand
            =>
                _deleteFromStoreDatabaseCommand ??
                (_deleteFromStoreDatabaseCommand = new RelayCommand(DeleteFromStoreDatabase));

        /// <summary>
        ///     Command that is used to see if Enter is pressed, if its pressed it calls the DeleteFromStoreDatabase
        /// </summary>
        public ICommand EnterKeyPressedCommand
            => _enterPressedCommand ?? (_enterPressedCommand = new GalaSoft.MvvmLight.Command.RelayCommand<KeyEventArgs>(EnterKeyPressed));

        /// <summary>
        ///     Command that is used whenever there is an TextChanged event to see if the text entered contains illegal signs
        /// </summary>
        public ICommand IllegalSignDeleteProductCommand => _illegalSignDeleteProductCommand ??
                                                           (_illegalSignDeleteProductCommand =
                                                               new RelayCommand(IllegalSignDeleteProduct));

        /// <summary>
        ///     Command that is used whenever there is an Populating event to populate the dropdown menu with the correct products
        /// </summary>
        public ICommand PopulatingDeleteProductCommand => _populatingDeleteProductCommand ??
                                                          (_populatingDeleteProductCommand =
                                                              new RelayCommand(PopulatingListDeleteProduct));


        /// <summary>
        ///     Get method for AutoCompleteList, that is the list with the items that is getting populated to the dropdown.
        /// </summary>
        public ObservableCollection<string> AutoCompleteList { get; } = new ObservableCollection<string>();

        /// <summary>
        ///     Get and Set method for ShoppingListItem. The set method, sets the old ShoppingListItem to an oldtext, and then
        ///     change the value to the new vaule and call OnPropertyChanged
        /// </summary>
        public string ShoppingListItem
        {
            set
            {
                _oldtext = _shoppingListItem;
                _shoppingListItem = value;
                OnPropertyChanged();
            }
            get { return _shoppingListItem; }
        }

        /// <summary>
        ///     The Text that is written to a label on the GUI that describes if the event has been successfully or if it has
        ///     failed, and why it failed
        /// </summary>
        public string ConfirmText
        {
            set
            {
                _confirmText = value;
                OnPropertyChanged();
                _timer.Stop();
                _timer.Start();
                _timer.Elapsed += delegate
                {
                    _confirmText = "";
                    OnPropertyChanged();
                };
            }
            get { return _confirmText; }
        }


        private void PopulatingListDeleteProduct()
        {
            AutoCompleteList?.Clear();
            foreach (
                var item in _autocomplete.AutoCompleteProductForOneStore(_manager.Store.StoreName, ShoppingListItem))
            {
                AutoCompleteList?.Add(item);
            }
            OnPropertyChanged("AutoCompleteList");
        }

        private void IllegalSignDeleteProduct()
        {
            if (ShoppingListItem == null) return;
            if (ShoppingListItem.All(chr => char.IsLetter(chr) || char.IsNumber(chr) || char.IsWhiteSpace(chr))) return;
            IsTextConfirm = false;
            ConfirmText = $"Der kan kun skrives bogstaverne fra a til å og tallene fra 0 til 9.";
            ShoppingListItem = _oldtext;
        }

        private void DeleteFromStoreDatabase()
        {
            if (string.IsNullOrEmpty(ShoppingListItem))
            {
                IsTextConfirm = false;
                ConfirmText = "Indtast venligst navnet på det produkt der skal fjernes.";
                return;
            }

            var productName = char.ToUpper(ShoppingListItem[0]) + ShoppingListItem.Substring(1).ToLower();

            var product = _manager.FindProduct(productName);
            if (product != null)
            {
                var result = _msgBox.DeleteProductMgsConfirmation(productName);
                if (result != DialogResult.Yes)
                {
                    IsTextConfirm = false;
                    ConfirmText = "Der blev ikke bekræftet.";
                    return;
                }
                if (_manager.RemoveProductFromMyStore(product) != 0)
                {
                    IsTextConfirm = false;
                    ConfirmText = $"Produktet \"{productName}\" findes ikke i din forretning.";
                    return;
                }
                IsTextConfirm = true;
                ConfirmText = $"Produktet \"{productName}\" er fjernet fra din forretning.";
            }
            else
            {
                IsTextConfirm = false;
                ConfirmText = $"Produktet \"{productName}\" findes ikke.";
            }
        }

        private void EnterKeyPressed(KeyEventArgs e)
        {
            if ((e.Key == Key.Enter) || (e.Key == Key.Return))
            {
                DeleteFromStoreDatabase();
            }
        }
    }
}