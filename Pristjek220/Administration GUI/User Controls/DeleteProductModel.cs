using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Forms;
using System.Windows.Input;
using Administration;
using GalaSoft.MvvmLight.Command;
using Pristjek220Data;
using SharedFunctionalities;
using KeyEventArgs = System.Windows.Input.KeyEventArgs;

namespace Administration_GUI.User_Controls
{
    public class DeleteProductModel : ObservableObject, IPageViewModel
    {
        private readonly System.Timers.Timer _timer = new System.Timers.Timer(2500);
        private readonly IAutocomplete _autocomplete;
        private readonly IStoremanager _manager;
        private ICommand _deleteFromStoreDatabaseCommand;
        private ICommand _populatingDeleteProductCommand;
        private ICommand _illegalSignDeleteProductCommand;
        private ICommand _enterPressedCommand;

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

        public DeleteProductModel(Store store, IUnitOfWork unit)
        {
            _manager = new Storemanager(unit, store);
            _autocomplete = new SharedFunctionalities.Autocomplete(unit);
        }

        public ICommand DeleteFromStoreDatabaseCommand
            =>
                _deleteFromStoreDatabaseCommand ??
                (_deleteFromStoreDatabaseCommand = new RelayCommand(DeleteFromStoreDatabase));

        public ICommand PopulatingDeleteProductCommand => _populatingDeleteProductCommand ??
                                                          (_populatingDeleteProductCommand = new RelayCommand(PopulatingListDeleteProduct));

        public ICommand IllegalSignDeleteProductCommand => _illegalSignDeleteProductCommand ??
                                                           (_illegalSignDeleteProductCommand = new RelayCommand(IllegalSignDeleteProduct));

        public ObservableCollection<string> AutoCompleteList { get; } = new ObservableCollection<string>();

        

        private void PopulatingListDeleteProduct()
        {

            AutoCompleteList?.Clear();
            foreach (var item in _autocomplete.AutoCompleteProductForOneStore(_manager.Store.StoreName, ShoppingListItem))
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
            ConfirmText = ($"Der kan kun skrives bogstaverne fra a til å og tallene fra 0 til 9.");
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
                var result = CustomMsgBox.Show($"Vil du fjerne produktet \"{productName}\" fra din forretning?",
                    "Bekræftelse", "Ja", "Nej");
                if (result != DialogResult.Yes)
                {
                    IsTextConfirm = false;
                    ConfirmText = "Der blev ikke bekræftet.";
                    return;
                }
                if (_manager.RemoveProductFromMyStore(product) != 0)
                {
                    IsTextConfirm = false;
                    ConfirmText = ($"Produktet \"{productName}\" findes ikke i din forretning.");
                    return;
                }
                IsTextConfirm = true;
                ConfirmText = ($"Produktet \"{productName}\" er fjernet fra din forretning.");
            }
            else
            {
                IsTextConfirm = false;
                ConfirmText = ($"Produktet \"{productName}\" findes ikke.");
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

        public ICommand EnterKeyPressedCommand => _enterPressedCommand ?? (_enterPressedCommand = new RelayCommand<KeyEventArgs>(EnterKeyPressed));

        private void EnterKeyPressed(KeyEventArgs e)
        {
            if ((e.Key == Key.Enter) || (e.Key == Key.Return))
            {
                DeleteFromStoreDatabase();
            }
        }
    }
}
