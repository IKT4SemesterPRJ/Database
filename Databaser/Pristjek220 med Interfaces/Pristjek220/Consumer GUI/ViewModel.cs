using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using AutoComplete;
using Consumer;
using Consumer_GUI.Annotations;
using Pristjek220Data;

namespace Consumer_GUI
{
    class ViewModel : INotifyPropertyChanged
    {
        private UnitOfWork _unit = new UnitOfWork(new DataContext());
        private IConsumer _user;
        //readonly ShoppingListItem _shoppingListItem = new ShoppingListItem();
        private string oldtext = string.Empty;
        private readonly UserControl _homeWindow;
        private readonly UserControl _findProductWindow;
        private readonly UserControl _shoppingListWindow;
        private readonly UserControl _generatedShppingListWindow;



        #region Commands
        ICommand _searchCommand;
        public ICommand SearchCommand
        {
            get { return _searchCommand ?? (_searchCommand = new RelayCommand(SearchAfterProduct)); }
        }

        private void SearchAfterProduct()
        {
            var unit = new UnitOfWork(new DataContext());
            IConsumer _user = new Consumer.Consumer(unit);

            string product = ProductName;
            var store = _user.FindCheapestStore(product);
            if (store != null)
                System.Windows.MessageBox.Show($"Det er billigst i {ProductName} {store.StoreName}", "Billigste forretning", MessageBoxButton.OK);
            else
                System.Windows.MessageBox.Show($" hej {ProductName}", "Billigste forretning", MessageBoxButton.OK);
        }

        ICommand _populatingFindProductCommand;
        public ICommand PopulatingFindProductCommand
        {
            get { return _populatingFindProductCommand ?? (_populatingFindProductCommand = new RelayCommand(PopulatingListFindProduct)); }
        }


        private void PopulatingListFindProduct()
        {
            UnitOfWork unit = new UnitOfWork(new DataContext());
            IAutocomplete autocomplete = new Autocomplete(unit);
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
            get { return _populatingShoppingListCommand ?? (_populatingShoppingListCommand = new RelayCommand(PopulatingListShoppingList)); }
        }


        private void PopulatingListShoppingList()
        {
            UnitOfWork unit = new UnitOfWork(new DataContext());
            IAutocomplete autocomplete = new Autocomplete(unit);
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
            get { return _illegalSignFindProductCommand ?? (_illegalSignFindProductCommand = new RelayCommand(IllegalSignFindProduct)); }
        }


        private void IllegalSignFindProduct()
        {
            if (!ProductName.All(chr => char.IsLetter(chr) || char.IsNumber(chr) || char.IsWhiteSpace(chr)))
            {
                System.Windows.MessageBox.Show(
                "Der kan kun skrives bogstaverne fra a til å og tallene fra 0 til 9", "ERROR", MessageBoxButton.OK);
                ProductName = oldtext;
            }
        }


        ICommand _illegalSignShoppingListCommand;
        public ICommand IllegalSignShoppingListCommand
        {
            get { return _illegalSignShoppingListCommand ?? (_illegalSignShoppingListCommand = new RelayCommand(IllegalSignFindProductShoppingList)); }
        }


        private void IllegalSignFindProductShoppingList()
        {
            if (!ShoppingListItem.All(chr => char.IsLetter(chr) || char.IsNumber(chr) || char.IsWhiteSpace(chr)))
            {
                System.Windows.MessageBox.Show(
                "Der kan kun skrives bogstaverne fra a til å og tallene fra 0 til 9", "ERROR", MessageBoxButton.OK);
                ShoppingListItem = oldtext;
            }
        }

        ICommand _addToShoppingListCommand;
        public ICommand AddToShoppingListCommand
        {
            get { return _addToShoppingListCommand ?? (_addToShoppingListCommand = new RelayCommand(AddToShoppingList)); }
        }

        private void AddToShoppingList()
        {
            UnitOfWork unit = new UnitOfWork(new DataContext());
            _user = new Consumer.Consumer(unit);
            //ShoppingListItem _shoppingListItem = new ShoppingListItem();

            if (_user.DoesProductExsist(ShoppingListItem))
                ShoppingList.Add(new ProduktInfo(ShoppingListItem));
            else
                System.Windows.MessageBox.Show("produktet findes ikke", "Error", MessageBoxButton.OK);
        }


        ICommand _deleteFromShoppingListCommand;
        public ICommand DeleteFromShoppingListCommand
        {
            get { return _deleteFromShoppingListCommand ?? (_deleteFromShoppingListCommand = new RelayCommand(DeleteFromShoppingList)); }
        }

        private void DeleteFromShoppingList()
        {
            if (SelectedRow == -1)
                System.Windows.MessageBox.Show("Du skal markere at produkt før du kan slette", "Error",
                    MessageBoxButton.OK);
            else if(ShoppingList.Count == 0)
                System.Windows.MessageBox.Show("Der er ikke tilføjet nogen produkter", "Error",
                    MessageBoxButton.OK);
            else
                ShoppingList.RemoveAt(SelectedRow);
         }
        //System.ArgumentOutOfRangeException


        #endregion

        #region Attributes

        private string _productName;
        public string ProductName
        {
            set
            {
                oldtext = _productName;
                _productName = value;
                OnPropertyChanged();
            }
            get { return _productName; }
        }


        public ObservableCollection<string> AutoCompleteList { get;} = new ObservableCollection<string>();
        
        public ObservableCollection<ProduktInfo> ShoppingList { get; } = new ObservableCollection<ProduktInfo>();

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

        public UserControl WindowContent { set; get; }


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
        public string _name { set; get; }
        public string _quantity { set; get; }

        public ProduktInfo(string Name, string Quantity = "1")
        {
            _name = Name;
            _quantity = Quantity;
        }
    }
    #endregion
}
