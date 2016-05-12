using System;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.Windows.Forms;
using System.Windows.Input;
using Administration;
using Administration_GUI;
using GalaSoft.MvvmLight.Command;
using Pristjek220Data;
using SharedFunctionalities;
using KeyEventArgs = System.Windows.Input.KeyEventArgs;

namespace Administration_GUI.User_Controls
{
    public class ChangePriceModel : ObservableObject, IPageViewModel
    {
        private readonly System.Timers.Timer _timer = new System.Timers.Timer(2500);
        private readonly IAutocomplete _autocomplete;
        private readonly IStoremanager _manager;
        private Store _store;
        private ICommand _changeProductPriceInStoreDatabaseCommand;
        private ICommand _populatingChangePriceCommand;
        private ICommand _illegalSignChangePriceCommand;
        private ICommand _enterPressedCommand;
        private ICommand _populatingDeleteProductCommand;

        private string _oldtext = string.Empty;

        public ChangePriceModel(Store store, IUnitOfWork unit)
        {
            _store = store;
            _manager = new Storemanager(unit, _store);
            _autocomplete = new SharedFunctionalities.Autocomplete(unit);
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

        public ICommand ChangeProductPriceInStoreDatabaseCommand
            =>
                _changeProductPriceInStoreDatabaseCommand ??
                (_changeProductPriceInStoreDatabaseCommand = new RelayCommand(ChangeProductPriceInStoreDatabase));

        public ICommand IllegalSignChangePriceCommand => _illegalSignChangePriceCommand ??
                                                         (_illegalSignChangePriceCommand =
                                                             new RelayCommand(IllegalSignChangePrice));

        public ICommand PopulatingChangePriceProductCommand => _populatingDeleteProductCommand ??
                                                          (_populatingDeleteProductCommand = new RelayCommand(PopulatingListChangePriceProduct));



        public ObservableCollection<string> AutoCompleteList { get; } = new ObservableCollection<string>();

        private void PopulatingListChangePriceProduct()
        {

            AutoCompleteList?.Clear();
            foreach (var item in _autocomplete.AutoCompleteProductForOneStore(_manager.Store.StoreName, ShoppingListItem))
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

                var product = _manager.FindProduct(productName);
                if (product != null && _manager.FindProductInStore(productName) != null)
                {
                    var result =
                        CustomMsgBox.Show(
                            $"Vil du ændre prisen på produktet \"{ShoppingListItem}\" til {ShoppingListItemPrice} kr?",
                            "Bekræftelse", "Ja", "Nej");
                    if (result != DialogResult.Yes)
                    {
                        IsTextConfirm = false;
                        ConfirmText = "Der blev ikke bekræftet.";
                        return;
                    }
                    _manager.changePriceOfProductInStore(product, resultPrice);
                    IsTextConfirm = true;
                    ConfirmText = ($"Prisen for produktet \"{productName}\" er ændret til {ShoppingListItemPrice} kr.");
                }
                else
                {
                    IsTextConfirm = false;
                    ConfirmText = ($"Produktet \"{productName}\" findes ikke i din forretning.");
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
            ConfirmText = ($"Der kan kun skrives bogstaverne fra a til å og tallene fra 0 til 9.");
            ShoppingListItem = _oldtext;
        }

        private string _shoppingListItem;

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

        private string _shoppingListItemPrice = "0";

        public string ShoppingListItemPrice
        {
            set
            {
                double result;

                _shoppingListItemPrice = double.TryParse(value, NumberStyles.Number, CultureInfo.CurrentCulture, out result) ? Math.Round(result, 2).ToString("F") : "0";
            }
            get { return _shoppingListItemPrice; }
        }

        private string _confirmText;

        public string ConfirmText
        {
            set
            {
                _confirmText = value;
                OnPropertyChanged();
                _timer.Stop();
                _timer.Start();
                _timer.Elapsed += delegate { _confirmText = ""; OnPropertyChanged(); };
            }
            get { return _confirmText; }
        }

        public ICommand EnterKeyPressedCommand
            => _enterPressedCommand ?? (_enterPressedCommand = new RelayCommand<KeyEventArgs>(EnterKeyPressed));

        private void EnterKeyPressed(KeyEventArgs e)
        {
            if ((e.Key == Key.Enter) || (e.Key == Key.Return))
            {
                ChangeProductPriceInStoreDatabase();
            }
        }
    }
}
