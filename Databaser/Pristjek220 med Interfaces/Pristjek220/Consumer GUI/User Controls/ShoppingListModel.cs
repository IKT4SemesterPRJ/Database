using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using Autocomplete;
using Consumer;
using Prism.Events;
using Pristjek220Data;

namespace Consumer_GUI.User_Controls
{
    internal class ShoppingListModel : ObservableObject, IPageViewModel
    {
        private ICommand _addToShoppingListCommand;
        private ICommand _deleteFromShoppingListCommand;
        private ICommand _populatingShoppingListCommand;
        private ICommand _illegalSignShoppingListCommand;
        private ICommand _generatedShoppingListCommand;

        private readonly string _oldtext = string.Empty;

        private ProductInfo _selectedItem;
        private readonly UnitOfWork _unit = new UnitOfWork(new DataContext());
        private IConsumer _user;
        
        public ShoppingListModel(Consumer.Consumer user)
        {
            
            _user = user;
        }

        public ICommand AddToShoppingListCommand
        {
            get
            {
                return _addToShoppingListCommand ?? (_addToShoppingListCommand = new RelayCommand(AddToShoppingList));
            }
        }

        public ICommand DeleteFromShoppingListCommand
        {
            get
            {
                return _deleteFromShoppingListCommand ??
                       (_deleteFromShoppingListCommand = new RelayCommand(DeleteFromShoppingList));
            }
        }

        public ICommand PopulatingShoppingListCommand
        {
            get
            {
                return _populatingShoppingListCommand ??
                       (_populatingShoppingListCommand = new RelayCommand(PopulatingListShoppingList));
            }
        }

        public ICommand IllegalSignShoppingListCommand
        {
            get
            {
                return _illegalSignShoppingListCommand ??
                       (_illegalSignShoppingListCommand = new RelayCommand(IllegalSignFindProductShoppingList));
            }
        }

        public ICommand GeneratedShoppingListCommand
        {
            get
            {
                return _generatedShoppingListCommand ??
                       (_generatedShoppingListCommand = new RelayCommand(GeneratedShoppingListFromShoppingList));
            }
        }


        public ObservableCollection<ProductInfo> ShoppingListData => _user.ShoppingListData;
        public ObservableCollection<string> AutoCompleteList { get; } = new ObservableCollection<string>();
        public string ShoppingListItem { set; get; }

        public ProductInfo SelectedItem
        {
            get { return _selectedItem; }
            set
            {
                _selectedItem = value;
                OnPropertyChanged();
            }
        }

        private void AddToShoppingList()
        {
            if (_user.DoesProductExist(ShoppingListItem))
                _user.ShoppingListData.Add(new ProductInfo(ShoppingListItem));
            else
                MessageBox.Show("produktet findes ikke", "Error", MessageBoxButton.OK);
        }

        private void DeleteFromShoppingList()
        {
            if (SelectedItem == null)
                MessageBox.Show("Du skal markere at produkt før du kan slette", "Error",
                    MessageBoxButton.OK);
            else if (_user.ShoppingListData.Count == 0)
                MessageBox.Show("Der er ikke tilføjet nogen produkter", "Error",
                    MessageBoxButton.OK);
            else
                _user.ShoppingListData.Remove(SelectedItem);
        }


        private void PopulatingListShoppingList()
        {
            IAutocomplete autocomplete = new Autocomplete.Autocomplete(_unit);
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
                MessageBox.Show(
                    "Der kan kun skrives bogstaverne fra a til å og tallene fra 0 til 9", "ERROR", MessageBoxButton.OK);
                ShoppingListItem = _oldtext;
            }
        }


        private void GeneratedShoppingListFromShoppingList()
        {
            _user.GeneratedShoppingListData.Clear();
            _user.CreateShoppingList();
        }
    }
}