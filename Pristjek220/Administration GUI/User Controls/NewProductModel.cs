using System;
using System.Collections.ObjectModel;
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
        

        private string _oldtext = string.Empty;


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
            if (ShoppingListItemPrice > 0 && ShoppingListItem != null)

            {
                var result = CustomMsgBox.Show($"Vil du tilføje produktet {ShoppingListItem} med prisen {ShoppingListItemPrice} kr til din forretning?", "Bekræftelse", "Ja", "Nej");
                if (result != DialogResult.Yes) return;

                var productName = char.ToUpper(ShoppingListItem[0]) + ShoppingListItem.Substring(1).ToLower();

                var product = _manager.FindProduct(productName);
                if (product == null)
                {
                    product = new Product() {ProductName = productName};
                    _manager.AddProductToDb(product);
                    product = _manager.FindProduct(productName);
                }

                if (_manager.AddProductToMyStore(product, ShoppingListItemPrice) != 0)
                {
                    ConfirmText = ($"Produktet {productName} findes allerede");
                    return;
                }

                ConfirmText =
                    ($"{ShoppingListItem} er indsat til prisen {ShoppingListItemPrice} kr. i din forretning");
            }
            else
                ConfirmText = "Prisen er ugyldig";
        }

        private void IllegalSignNewProduct()
        {
            if (ShoppingListItem != null)
            {
                 if (!ShoppingListItem.All(chr => char.IsLetter(chr) || char.IsNumber(chr) || char.IsWhiteSpace(chr)))
                 {
                     ConfirmText = ($"Der kan kun skrives bogstaverne fra a til å og tallene fra 0 til 9");
                     ShoppingListItem = _oldtext;
                 }
            }
            
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

        private double _shoppingListItemPrice;

        public double ShoppingListItemPrice
        {
            set
            {
                _shoppingListItemPrice = Math.Round(value, 2);
                OnPropertyChanged();
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
