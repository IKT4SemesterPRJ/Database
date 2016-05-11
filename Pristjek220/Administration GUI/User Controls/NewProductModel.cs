using System;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Forms;
using System.Windows.Input;
using Administration;
using GalaSoft.MvvmLight.Command;
using Pristjek220Data;
using SharedFunctionalities;
using Administration_GUI;
using KeyEventArgs = System.Windows.Input.KeyEventArgs;


namespace Administration_GUI.User_Controls
{
    public class NewProductModel : ObservableObject, IPageViewModel
    {
        private readonly IAutocomplete _autocomplete;
        private readonly IStoremanager _manager;

        private ICommand _addToStoreDatabaseCommand;
        private ICommand _populatingNewProductCommand;
        private ICommand _illegalSignNewProductCommand;
        private ICommand _enterPressedCommand;

        public string NumberError { get; set; } = "";
        private string _oldtext = string.Empty;

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


        public ICommand AddToStoreDatabaseCommand => _addToStoreDatabaseCommand ?? (_addToStoreDatabaseCommand = new RelayCommand(AddToStoreDatabase));

        public ICommand PopulatingNewProductCommand => _populatingNewProductCommand ??
                                                       (_populatingNewProductCommand = new RelayCommand(PopulatingListNewProduct));

        public ICommand IllegalSignNewProductCommand => _illegalSignNewProductCommand ??
                                                        (_illegalSignNewProductCommand = new RelayCommand(IllegalSignNewProduct));

        public ObservableCollection<string> AutoCompleteList { get; } = new ObservableCollection<string>();

        public NewProductModel(Store store, IUnitOfWork unit)
        {
            _manager = new Storemanager(unit, store);
            _autocomplete = new SharedFunctionalities.Autocomplete(unit);
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

            double resultPrice = double.Parse(ShoppingListItemPrice, CultureInfo.CurrentCulture);
            if (resultPrice > 0 && ShoppingListItem != null)
            {
                var result =
                    CustomMsgBox.Show(
                        $"Vil du tilføje produktet \"{ShoppingListItem}\" med prisen {ShoppingListItemPrice} kr til din forretning?",
                        "Bekræftelse", "Ja", "Nej");
                if (result != DialogResult.Yes)
                {
                    IsTextConfirm = false;
                    ConfirmText = "Der blev ikke bekræftet.";
                    return;
                }

                var productName = char.ToUpper(ShoppingListItem[0]) + ShoppingListItem.Substring(1).ToLower();

                var product = _manager.FindProduct(productName);
                if (product == null)
                {
                    product = new Product() {ProductName = productName};
                    _manager.AddProductToDb(product);
                    product = _manager.FindProduct(productName);
                }

                if (_manager.AddProductToMyStore(product, resultPrice) != 0)
                {
                    IsTextConfirm = false;
                    ConfirmText = ($"Produktet \"{productName}\" findes allerede.");
                    return;
                }
                IsTextConfirm = true;
                ConfirmText =
                    ($"Produktet \"{ShoppingListItem}\" er indsat til prisen {ShoppingListItemPrice} kr. i din forretning.");
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

                if (double.TryParse(value, NumberStyles.Number, CultureInfo.CurrentCulture, out result))
                {
                    _shoppingListItemPrice = Math.Round(result, 2).ToString("F");
                }
                else
                {
                    _shoppingListItemPrice = "0";
                }
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
            }
            get { return _confirmText; }
        }
        public ICommand EnterKeyPressedCommand => _enterPressedCommand ?? (_enterPressedCommand = new RelayCommand<KeyEventArgs>(EnterKeyPressed));

        private void EnterKeyPressed(KeyEventArgs e)
        {
            if ((e.Key == Key.Enter) || (e.Key == Key.Return))
            {
                AddToStoreDatabase();
            }
        }
    }
}
