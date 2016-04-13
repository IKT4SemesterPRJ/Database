using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using Administration;
using GalaSoft.MvvmLight.Command;
using Pristjek220Data;
using SharedFunctionalities;
using Administration_GUI;
using RelayCommand = Administration_GUI.RelayCommand;

namespace Administration_GUI.User_Controls
{
    internal class NewProductModel : ObservableObject, IPageViewModel
    {
        private readonly UnitOfWork _unit = new UnitOfWork(new DataContext());
        private readonly IAutocomplete _autocomplete;
        private readonly IStoremanager _manager;

        private ICommand _addToStoreDatabaseCommand;
        private ICommand _populatingNewProductCommand;
        private ICommand _illegalSignNewProductCommand;
        private ICommand _enterPressedCommand;


        private string _oldtext = string.Empty;


        public ICommand AddToStoreDatabaseCommand
        {
            get
            {
                return _addToStoreDatabaseCommand ?? (_addToStoreDatabaseCommand = new RelayCommand(AddToStoreDatabase));
            }
        }

        public ICommand PopulatingNewProductCommand
        {
            get
            {
                return _populatingNewProductCommand ??
                       (_populatingNewProductCommand = new RelayCommand(PopulatingListNewProduct));
            }
        }

        public ICommand IllegalSignNewProductCommand
        {
            get
            {
                return _illegalSignNewProductCommand ??
                       (_illegalSignNewProductCommand = new RelayCommand(IllegalSignNewProduct));
            }
        }

        public ObservableCollection<string> AutoCompleteList { get; } = new ObservableCollection<string>();

        public NewProductModel()
        {
            _manager = new Storemanager(new UnitOfWork(new DataContext()), new Store() { StoreName = "Aldi" });
            _autocomplete = new SharedFunctionalities.Autocomplete(_unit);
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
            if (ShoppingListItemPrice > 0)
            {
                string productName = char.ToUpper(ShoppingListItem[0]) + ShoppingListItem.Substring(1).ToLower();

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
                    ($"{ShoppingListItem} er indsat, med prisen {ShoppingListItemPrice} i butikken {_manager.Store.StoreName}");
            }
            else
                ConfirmText = "Prisen er ugyldig";
        }

        private void IllegalSignNewProduct()
        {
            if (!ShoppingListItem.All(chr => char.IsLetter(chr) || char.IsNumber(chr) || char.IsWhiteSpace(chr)))
            {
                MessageBox.Show(
                    "Der kan kun skrives bogstaverne fra a til å og tallene fra 0 til 9", "ERROR", MessageBoxButton.OK);
                ShoppingListItem = _oldtext;
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
                _shoppingListItemPrice = value;
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
        public ICommand EnterKeyPressedCommand
        {
            get
            {
                return _enterPressedCommand ?? (_enterPressedCommand = new RelayCommand<KeyEventArgs>(EnterKeyPressed));
            }
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
