using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;
using Administration;
using Administration_GUI;
using GalaSoft.MvvmLight.Command;
using Pristjek220Data;
using SharedFunctionalities;

namespace Administration_GUI.User_Controls
{
    class ChangePriceModel : ObservableObject, IPageViewModel
    {
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
            if (string.IsNullOrEmpty(ShoppingListItem)) return;

            if (ShoppingListItemPrice > 0)
            {
                string productName = char.ToUpper(ShoppingListItem[0]) + ShoppingListItem.Substring(1).ToLower();

                var product = _manager.FindProduct(productName);
                if (product != null)
                {
                    _manager.changePriceOfProductInStore(product, ShoppingListItemPrice);
                    ConfirmText = ($"Prisen for produktet {productName} er ændret til {ShoppingListItemPrice} kr.");
                }
                else
                {
                    ConfirmText = ($"Produktet {productName} findes ikke");
                    return;
                }
            }
            else
                ConfirmText = "Prisen er ugyldig";
        }


        private void IllegalSignChangePrice()
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
