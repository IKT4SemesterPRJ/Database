using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using AutoComplete;
using Consumer;
using Consumer_GUI.Annotations;
using Consumer_GUI.User_Controls;
using Pristjek220Data;

namespace Consumer_GUI
{
    class ViewModel : INotifyPropertyChanged
    {
        private UnitOfWork _unit = new UnitOfWork(new DataContext());
        private IConsumer _user;
        //readonly ShoppingListItem _shoppingListItem = new ShoppingListItem();
        private string _oldtext = string.Empty;
        private static readonly UserControl HomeWindow = new Home();
        private static readonly UserControl FindProductWindow = new FindProduct();
        private static readonly UserControl ShoppingListWindow = new ShoppingList();
        private static readonly UserControl GeneratedShoppingListWindow = new GeneratedShoppingList();


        #region Commands

        ICommand _searchCommand;

        public ICommand SearchCommand
        {
            get { return _searchCommand ?? (_searchCommand = new RelayCommand(SearchAfterProduct)); }
        }

        private void SearchAfterProduct()
        {
            _user = new Consumer.Consumer(_unit);

            string product = ProductName;
            var store = _user.FindCheapestStore(product);
            if (store != null)
                MessageBox.Show($"Det er billigst i {ProductName} {store.StoreName}",
                    "Billigste forretning", MessageBoxButton.OK);
            else
                MessageBox.Show($" hej {ProductName}", "Billigste forretning", MessageBoxButton.OK);
        }

        ICommand _populatingFindProductCommand;

        public ICommand PopulatingFindProductCommand
        {
            get
            {
                return _populatingFindProductCommand ??
                       (_populatingFindProductCommand = new RelayCommand(PopulatingListFindProduct));
            }
        }


        private void PopulatingListFindProduct()
        {
            IAutocomplete autocomplete = new Autocomplete(_unit);
            AutoCompleteList?.Clear();
            foreach (var item in autocomplete.AutoCompleteProduct(ProductName))
            {
                AutoCompleteList?.Add(item);
            }
            OnPropertyChanged("AutoCompleteList");
        }

        ICommand _populatingShoppingListCommand;

        public ICommand PopulatingShoppingListCommand
        {
            get
            {
                return _populatingShoppingListCommand ??
                       (_populatingShoppingListCommand = new RelayCommand(PopulatingListShoppingList));
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


        ICommand _illegalSignFindProductCommand;

        public ICommand IllegalSignFindProductCommand
        {
            get
            {
                return _illegalSignFindProductCommand ??
                       (_illegalSignFindProductCommand = new RelayCommand(IllegalSignFindProduct));
            }
        }


        private void IllegalSignFindProduct()
        {
            if (!ProductName.All(chr => char.IsLetter(chr) || char.IsNumber(chr) || char.IsWhiteSpace(chr)))
            {
                MessageBox.Show(
                    "Der kan kun skrives bogstaverne fra a til å og tallene fra 0 til 9", "ERROR", MessageBoxButton.OK);
                ProductName = _oldtext;
            }
        }


        ICommand _illegalSignShoppingListCommand;

        public ICommand IllegalSignShoppingListCommand
        {
            get
            {
                return _illegalSignShoppingListCommand ??
                       (_illegalSignShoppingListCommand = new RelayCommand(IllegalSignFindProductShoppingList));
            }
        }


        private void IllegalSignFindProductShoppingList()
        {
            if (!ShoppingListItem.All(chr => char.IsLetter(chr) || char.IsNumber(chr) || char.IsWhiteSpace(chr)))
            {
                MessageBox.Show(
                    "Der kan kun skrives bogstaverne fra a til å og tallene fra 0 til 9", "ERROR", MessageBoxButton.OK);
                ShoppingListItem = _oldtext;
            }
        }

        ICommand _addToShoppingListCommand;

        public ICommand AddToShoppingListCommand
        {
            get
            {
                return _addToShoppingListCommand ?? (_addToShoppingListCommand = new RelayCommand(AddToShoppingList));
            }
        }

        private void AddToShoppingList()
        {
            _user = new Consumer.Consumer(_unit);
            //ShoppingListItem _shoppingListItem = new ShoppingListItem();

            if (_user.DoesProductExsist(ShoppingListItem))
                ShoppingList.Add(new ProduktInfo(ShoppingListItem));
            else
                MessageBox.Show("produktet findes ikke", "Error", MessageBoxButton.OK);
        }


        ICommand _deleteFromShoppingListCommand;

        public ICommand DeleteFromShoppingListCommand
        {
            get
            {
                return _deleteFromShoppingListCommand ??
                       (_deleteFromShoppingListCommand = new RelayCommand(DeleteFromShoppingList));
            }
        }

        private void DeleteFromShoppingList()
        {
            if (SelectedRow == -1)
                MessageBox.Show("Du skal markere at produkt før du kan slette", "Error",
                    MessageBoxButton.OK);
            else if (ShoppingList.Count == 0)
                MessageBox.Show("Der er ikke tilføjet nogen produkter", "Error",
                    MessageBoxButton.OK);
            else
                ShoppingList.RemoveAt(SelectedRow);
        }



        ICommand _changeWindowHomeCommand;

        public ICommand ChangeWindowHomeCommand
        {
            get { return _changeWindowHomeCommand ?? (_changeWindowHomeCommand = new RelayCommand(ChangeWindowHome)); }
        }

        private void ChangeWindowHome()
        {
            WindowContent = HomeWindow;
        }

        ICommand _changeWindowFindProductCommand;

        public ICommand ChangeWindowFindProductCommand
        {
            get { return _changeWindowFindProductCommand ?? (_changeWindowFindProductCommand = new RelayCommand(ChangeWindowFindProduct)); }
        }

        private void ChangeWindowFindProduct()
        {
            WindowContent = FindProductWindow;
        }

        ICommand _changeWindowShoppingListCommand;

        public ICommand ChangeWindowShoppingListCommand
        {
            get { return _changeWindowShoppingListCommand ?? (_changeWindowShoppingListCommand = new RelayCommand(ChangeWindowShoppingList)); }
        }

        private void ChangeWindowShoppingList()
        {
            WindowContent = ShoppingListWindow;
        }


        ICommand _changeWindowGeneratedShoppingListCommand;

        public ICommand ChangeWindowGeneratedShoppingListCommand
        {
            get { return _changeWindowGeneratedShoppingListCommand ?? (_changeWindowGeneratedShoppingListCommand = new RelayCommand(ChangeWindowGeneratedShoppingList)); }
        }

        private void ChangeWindowGeneratedShoppingList()
        {
            WindowContent = GeneratedShoppingListWindow;

            ToGeneratedShoppingList();
          
        }

        private void ToGeneratedShoppingList()
        {
            if (ShoppingList.Count == 0)
            {
                return;
            }

            List<String> toGeneratedList = ShoppingList.Select(item => item.Name).ToList();

            var TempGeneretedShopList = _user.CreateShoppingList(toGeneratedList);
            foreach (var item in TempGeneretedShopList)
            {
                GeneratedShoppingList.Add(item);
            }
        }


        ICommand _addToStoreListCommand;

        public ICommand AddToStoreListCommand
        {
            get
            {
                return _addToStoreListCommand ?? (_addToStoreListCommand = new RelayCommand(AddToStoreList));
            }
        }

        private void AddToStoreList()
        {
            _user = new Consumer.Consumer(_unit);

            StorePrice.Clear();

            var list = _user.FindStoresThatSellsProduct(ProductName);
            if (list.Count != 0)
            {
                foreach (var store in list)
                {
                    StorePrice.Add(store);
                }
            }
            else
                MessageBox.Show("produktet findes ikke", "Error", MessageBoxButton.OK);
        }


        #endregion

        #region Attributes

        private string _productName;

        public string ProductName
        {
            set
            {
                _oldtext = _productName;
                _productName = value;
                OnPropertyChanged();
            }
            get { return _productName; }
        }


        public ObservableCollection<string> AutoCompleteList { get; } = new ObservableCollection<string>();

        public ObservableCollection<ProduktInfo> ShoppingList { get; } = new ObservableCollection<ProduktInfo>();

        public ObservableCollection<StoreProductAndPrice> GeneratedShoppingList { get; } = new ObservableCollection<StoreProductAndPrice>();

        public ObservableCollection<StoreAndPrice> StorePrice { get; set; } = new ObservableCollection<StoreAndPrice>();

        public string ShoppingListItem { set; get; }

        private int _selectedRow;

        public int SelectedRow
        {
            get { return _selectedRow; }
            set
            {
                _selectedRow = value;
                OnPropertyChanged();
            }

        }

        private UserControl _windowContent = HomeWindow;

        public UserControl WindowContent
        {
            get { return _windowContent; }
            set
            {
                _windowContent = value;
                OnPropertyChanged("WindowContent");
            }
        }


        #endregion

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

    }


    #region classes
    public class ProduktInfo
    {
        public string Name { set; get; }
        public string Quantity { set; get; }

        public ProduktInfo(string name, string quantity = "1")
        {
            Name = name;
            Quantity = quantity;
        }
    }
    #endregion
}
