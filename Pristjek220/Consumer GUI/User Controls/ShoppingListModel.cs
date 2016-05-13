using System.Collections.ObjectModel;
using System.Linq;
using System.Timers;
using System.Windows.Input;
using Consumer;
using GalaSoft.MvvmLight.Command;
using Pristjek220Data;
using SharedFunctionalities;
using RelayCommand = SharedFunctionalities.RelayCommand;

namespace Consumer_GUI.User_Controls
{
    internal class ShoppingListModel : ObservableObject, IPageViewModel
    {
        private readonly Timer _timer = new Timer(2500);
        private readonly IUnitOfWork _unit;

        private ICommand _addToShoppingListCommand;
        private ICommand _clearShoppingListCommand;
        private ICommand _deleteFromShoppingListCommand;
        private ICommand _enterKeyPressedCommand;
        private string _error;
        private ICommand _generatedShoppingListCommand;
        private ICommand _illegalSignShoppingListCommand;

        private bool _isTextConfirm;

        private string _oldtext = string.Empty;
        private ICommand _populatingShoppingListCommand;


        private ProductInfo _selectedItem;
        private string _shoppinglistItem = string.Empty;

        public ShoppingListModel(IConsumer user, IUnitOfWork unit)
        {
            User = user;
            ShoppingListData = new ObservableCollection<ProductInfo>();
            _unit = unit;
            User.ReadFromJsonFile();
            Error = "";
        }

        public IConsumer User { get; }

        public ObservableCollection<StoresInPristjek> OptionsStores => User.OptionsStores;

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

        public bool IsTextConfirm
        {
            get { return _isTextConfirm; }
            set
            {
                _isTextConfirm = value;
                OnPropertyChanged();
            }
        }

        public ICommand AddToShoppingListCommand
            => _addToShoppingListCommand ?? (_addToShoppingListCommand = new RelayCommand(AddToShoppingList));

        public ICommand DeleteFromShoppingListCommand => _deleteFromShoppingListCommand ??
                                                         (_deleteFromShoppingListCommand =
                                                             new RelayCommand(DeleteFromShoppingList));

        public ICommand PopulatingShoppingListCommand => _populatingShoppingListCommand ??
                                                         (_populatingShoppingListCommand =
                                                             new RelayCommand(PopulatingListShoppingList));

        public ICommand IllegalSignShoppingListCommand => _illegalSignShoppingListCommand ??
                                                          (_illegalSignShoppingListCommand =
                                                              new RelayCommand(IllegalSignFindProductShoppingList));

        public ICommand GeneratedShoppingListCommand => _generatedShoppingListCommand ??
                                                        (_generatedShoppingListCommand =
                                                            new RelayCommand(GeneratedShoppingListFromShoppingList));

        public ICommand EnterKeyPressedCommand => _enterKeyPressedCommand ??
                                                  (_enterKeyPressedCommand =
                                                      new RelayCommand<KeyEventArgs>(EnterKeyPressed));


        public ICommand ClearShoppingListCommand => _clearShoppingListCommand ??
                                                    (_clearShoppingListCommand = new RelayCommand(ClearShoppingList));

        public ObservableCollection<ProductInfo> ShoppingListData
        {
            get { return User.ShoppingListData; }
            set { User.ShoppingListData = value; }
        }

        public ObservableCollection<string> AutoCompleteList { get; } = new ObservableCollection<string>();

        public string ShoppingListItem
        {
            set
            {
                _oldtext = _shoppinglistItem;
                _shoppinglistItem = value;
                OnPropertyChanged();
            }
            get { return _shoppinglistItem; }
        }

        public ProductInfo SelectedItem
        {
            get { return _selectedItem; }
            set
            {
                _selectedItem = value;
                OnPropertyChanged();
            }
        }

        private void ClearShoppingList()
        {
            User.ShoppingListData.Clear();
        }

        private void AddToShoppingList()
        {
            if (string.IsNullOrEmpty(ShoppingListItem))
            {
                IsTextConfirm = false;
                Error = "Indtast venligst navnet på det produkt du vil tilføje til indkøbslisten.";
                return;
            }
            if (
                ShoppingListData.Any(
                    x => x.Name == char.ToUpper(ShoppingListItem[0]) + ShoppingListItem.Substring(1).ToLower()))
            {
                var item = User.ShoppingListData.First(
                    s => s.Name == char.ToUpper(ShoppingListItem[0]) + ShoppingListItem.Substring(1).ToLower());
                if (item == null) return;
                var intQuantity = int.Parse(item.Quantity);
                intQuantity++;
                item.Quantity = intQuantity.ToString();
                OnPropertyChanged("Quantity");
            }
            else
            {
                ShoppingListData.Add(new ProductInfo(ShoppingListItem));
                User.WriteToJsonFile();
            }
        }

        private void DeleteFromShoppingList()
        {
            if (SelectedItem == null)
            {
                IsTextConfirm = false;
                Error = "Du skal markere et produkt, før du kan slette.";
            }

            else if (User.ShoppingListData.Count == 0)
            {
                IsTextConfirm = false;
                Error = "Der er ikke tilføjet nogen produkter.";
            }

            else
            {
                User.ShoppingListData.Remove(SelectedItem);
                User.WriteToJsonFile();
                Error = string.Empty;
            }
        }


        private void PopulatingListShoppingList()
        {
            IAutocomplete autocomplete = new Autocomplete(_unit);
            AutoCompleteList?.Clear(); // not equal null
            foreach (var item in autocomplete.AutoCompleteProduct(ShoppingListItem))
            {
                AutoCompleteList?.Add(item);
            }
            OnPropertyChanged("AutoCompleteList");
        }


        private void IllegalSignFindProductShoppingList()
        {
            if (!ShoppingListItem.All(chr => char.IsLetter(chr) || char.IsNumber(chr) || char.IsWhiteSpace(chr)))
            {
                IsTextConfirm = false;
                Error = "Der kan kun skrives bogstaverne fra a til å og tallene fra 0 til 9.";
                ShoppingListItem = _oldtext;
            }
            else if (ShoppingListItem == _oldtext)
            {
                Error = string.Empty;
            }
        }


        private void GeneratedShoppingListFromShoppingList()
        {
            ClearGeneratedLists();
            User.CreateShoppingList();
        }

        private void ClearGeneratedLists()
        {
            User.ClearGeneratedShoppingListData();
            User.ClearNotInAStore();
        }

        private void EnterKeyPressed(KeyEventArgs e)
        {
            if ((e.Key == Key.Enter) || (e.Key == Key.Return))
            {
                AddToShoppingList();
            }
        }
    }
}