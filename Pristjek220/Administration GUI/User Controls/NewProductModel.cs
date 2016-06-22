using System;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.Windows.Forms;
using System.Windows.Input;
using Administration;
using Pristjek220Data;
using SharedFunctionalities;
using KeyEventArgs = System.Windows.Input.KeyEventArgs;
using Timer = System.Timers.Timer;

namespace Administration_GUI.User_Controls
{
    /// <summary>
    ///     NewPriceModel is the User Control model for the NewPrice User Control
    ///     Its used to delete a product from a store
    /// </summary>
    public class NewProductModel : ObservableObject, IPageViewModel
    {
        private readonly IAutocomplete _autocomplete;
        private readonly IStoremanager _manager;
        private readonly Timer _timer = new Timer(2500);

        private ICommand _addToStoreDatabaseCommand;

        private string _confirmText;
        private ICommand _enterPressedCommand;
        private ICommand _illegalSignNewProductCommand;

        private bool _isTextConfirm;
        private ICreateMsgBox _msgBox;
        private string _oldtext = string.Empty;
        private ICommand _populatingNewProductCommand;

        private string _shoppingListItem;

        private string _shoppingListItemPrice = "0";

        /// <summary>
        ///     AdminDeleteStoreModel constructor takes a UnitOfWork and a store to create an Storemanger and a autocomplete
        /// </summary>
        /// <param name="storemanager"></param>
        /// <param name="autoComplete"></param>
        /// <param name="msgBox"></param>
        public NewProductModel(IStoremanager storemanager, IAutocomplete autoComplete, ICreateMsgBox msgBox)
        {
            _manager = storemanager;
            _autocomplete = autoComplete;
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
        ///     Command that is used to add a product to a store, if anything goes wrong it will print the reason to why it
        ///     did not add the product to a label
        /// </summary>
        public ICommand AddToStoreDatabaseCommand
            => _addToStoreDatabaseCommand ?? (_addToStoreDatabaseCommand = new RelayCommand(AddToStoreDatabase));

        /// <summary>
        ///     Command that is used to see if Enter is pressed, if its pressed it calls the AddToStoreDatabase
        /// </summary>
        public ICommand EnterKeyPressedCommand
            => _enterPressedCommand ?? (_enterPressedCommand = new GalaSoft.MvvmLight.Command.RelayCommand<KeyEventArgs>(EnterKeyPressed));

        /// <summary>
        ///     Command that is used whenever there is an Populating event to populate the dropdown menu with the correct products
        /// </summary>
        public ICommand PopulatingNewProductCommand => _populatingNewProductCommand ??
                                                       (_populatingNewProductCommand =
                                                           new RelayCommand(PopulatingListNewProduct));

        /// <summary>
        ///     Command that is used whenever there is an TextChanged event to see if the text entered contains illegal signs
        /// </summary>
        public ICommand IllegalSignNewProductCommand => _illegalSignNewProductCommand ??
                                                        (_illegalSignNewProductCommand =
                                                            new RelayCommand(IllegalSignNewProduct));

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
        ///     Get and Set method for ShoppingListItemPrice. The set method sets the price, and force it to have two decimals, and
        ///     if there is any illegal signs sets it to zero.
        /// </summary>
        public string ShoppingListItemPrice
        {
            set
            {
                double result;

                _shoppingListItemPrice = double.TryParse(value, NumberStyles.Number, CultureInfo.CurrentCulture,
                    out result)
                    ? Math.Round(result, 2).ToString("F", new CultureInfo("da-DK"))
                    : "0";
            }
            get { return _shoppingListItemPrice; }
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


        private void PopulatingListNewProduct()
        {
            AutoCompleteList?.Clear();
            foreach (var item in _autocomplete.AutoCompleteProduct(ShoppingListItem))
            {
                AutoCompleteList?.Add(item);
            }
            OnPropertyChanged("AutoCompleteList");
        }

        private void AddToStoreDatabase()
        {
            if (string.IsNullOrEmpty(ShoppingListItem))
            {
                IsTextConfirm = false;
                ConfirmText = "Indtast venligst navnet på det produkt der skal tilføjes.";
                return;
            }

            var resultPrice = double.Parse(ShoppingListItemPrice, CultureInfo.CurrentCulture);
            if (resultPrice > 0)
            {
                var productName = char.ToUpper(ShoppingListItem[0]) + ShoppingListItem.Substring(1).ToLower();

                var product = _manager.FindProduct(productName);

                if (_manager.FindProductInStore(productName) != null)
                {
                    IsTextConfirm = false;
                    ConfirmText = $"Produktet \"{productName}\" findes allerede.";
                    return;
                }

                var result =
                    _msgBox.AddProductMgsConfirmation(ShoppingListItem, ShoppingListItemPrice);
                if (result != DialogResult.Yes)
                {
                    IsTextConfirm = false;
                    ConfirmText = "Der blev ikke bekræftet.";
                    return;
                }

                if (product == null)
                {
                    product = new Product { ProductName = productName };
                    _manager.AddProductToDb(product);
                    product = _manager.FindProduct(productName);
                }
                _manager.AddProductToMyStore(product, resultPrice);
                IsTextConfirm = true;
                ConfirmText =
                    $"Produktet \"{ShoppingListItem}\" er indsat til prisen {ShoppingListItemPrice} kr. i din forretning.";
            }
            else
            {
                IsTextConfirm = false;
                ConfirmText = "Prisen er ugyldig.";
            }
        }

        private void IllegalSignNewProduct()
        {
            if (ShoppingListItem == null) return;
            if (ShoppingListItem.All(chr => char.IsLetter(chr) || char.IsNumber(chr) || char.IsWhiteSpace(chr))) return;
            IsTextConfirm = false;
            ConfirmText = $"Der kan kun skrives bogstaverne fra a til å og tallene fra 0 til 9.";
            ShoppingListItem = _oldtext;
        }

        private void EnterKeyPressed(KeyEventArgs e)
        {
            if ((e.Key == Key.Enter) || (e.Key == Key.Return))
            {
                AddToStoreDatabase();
            }
        }
    }
}