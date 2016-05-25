using System;
using System.Collections.ObjectModel;
using System.Globalization;
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
    ///     ChangePriceModel is the User Control model for the ChangePrice User Control
    ///     Its used to change the price of a product
    /// </summary>
    public class ChangePriceModel : ObservableObject, IPageViewModel
    {
        private readonly IAutocomplete _autocomplete;
        private readonly IStoremanager _manager;
        private readonly Timer _timer = new Timer(2500);
        private ICommand _changeProductPriceInStoreDatabaseCommand;

        private string _confirmText;
        private ICommand _enterPressedCommand;
        private ICommand _illegalSignChangePriceCommand;
        private ICreateMsgBox _msgBox;
        private bool _isTextConfirm;

        private string _oldtext = string.Empty;
        private ICommand _populatingChangePriceCommand;

        private string _shoppingListItem;

        private string _shoppingListItemPrice = "0";

        /// <summary>
        ///     ChangePriceModel constructor takes a UnitOfWork and a store to create a Storemanager and a AutoComplete
        /// </summary>
        /// <param name="storemanager"></param>
        /// <param name="autocomplete"></param>
        /// <param name="msgBox"></param>
        public ChangePriceModel(IStoremanager storemanager, IAutocomplete autocomplete, ICreateMsgBox msgBox)
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
        ///     Command that is used to change the product price, if anything goes wrong it will print the reason to why it did not
        ///     change the price to a label
        /// </summary>
        public ICommand ChangeProductPriceInStoreDatabaseCommand
            =>
                _changeProductPriceInStoreDatabaseCommand ??
                (_changeProductPriceInStoreDatabaseCommand = new RelayCommand(ChangeProductPriceInStoreDatabase));

        /// <summary>
        ///     Command that is used whenever there is an TextChanged event to see if the text entered contains illegal signs
        /// </summary>
        public ICommand IllegalSignChangePriceCommand => _illegalSignChangePriceCommand ??
                                                         (_illegalSignChangePriceCommand =
                                                             new RelayCommand(IllegalSignChangePrice));

        /// <summary>
        ///     Command that is used whenever there is an Populating event to populate the dropdown menu with the correct products
        /// </summary>
        public ICommand PopulatingChangePriceProductCommand => _populatingChangePriceCommand ??
                                                               (_populatingChangePriceCommand =
                                                                   new RelayCommand(PopulatingListChangePriceProduct));


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

        /// <summary>
        ///     Command that is used to see if Enter is pressed, if its pressed it calls the ChangeProductPriceInStoreDatabase
        /// </summary>
        public ICommand EnterKeyPressedCommand
            => _enterPressedCommand ?? (_enterPressedCommand = new GalaSoft.MvvmLight.Command.RelayCommand<KeyEventArgs>(EnterKeyPressed));

        private void PopulatingListChangePriceProduct()
        {
            AutoCompleteList?.Clear();
            foreach (
                var item in _autocomplete.AutoCompleteProductForOneStore(_manager.Store.StoreName, ShoppingListItem))
            {
                AutoCompleteList?.Add(item);
            }
            OnPropertyChanged("AutoCompleteList");
        }

        private void ChangeProductPriceInStoreDatabase()
        {
            if (string.IsNullOrEmpty(ShoppingListItem))
            {
                IsTextConfirm = false;
                ConfirmText = "Indtast venligst navnet på det produkt hvis pris skal ændres.";
                return;
            }
            var resultPrice = double.Parse(ShoppingListItemPrice, CultureInfo.CurrentCulture);
            if (resultPrice > 0)
            {
                var productName = char.ToUpper(ShoppingListItem[0]) + ShoppingListItem.Substring(1).ToLower();

                if (_manager.FindProductInStore(productName) != null)
                {
                    var result =
                        _msgBox.ChangePriceMgsConfirmation(ShoppingListItem, ShoppingListItemPrice);
                    if (result != DialogResult.Yes)
                    {
                        IsTextConfirm = false;
                        ConfirmText = "Der blev ikke bekræftet.";
                        return;
                    }
                    _manager.ChangePriceOfProductInStore(_manager.FindProduct(productName), resultPrice);
                    IsTextConfirm = true;
                    ConfirmText = $"Prisen for produktet \"{productName}\" er ændret til {ShoppingListItemPrice} kr.";
                }
                else
                {
                    IsTextConfirm = false;
                    ConfirmText = $"Produktet \"{productName}\" findes ikke i din forretning.";
                }
            }
            else
            {
                IsTextConfirm = false;
                ConfirmText = "Prisen er ugyldig.";
            }
        }


        private void IllegalSignChangePrice()
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
                ChangeProductPriceInStoreDatabase();
            }
        }
    }
}