using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using AutoComplete;
using Consumer;
using Pristjek220Data;

namespace Consumer_GUI.User_Controls
{
    internal class ShoppingListModel : ObservableObject, IPageViewModel
    {
        private ICommand _addToShoppingListCommand;


        private ICommand _deleteFromShoppingListCommand;


        private ICommand _illegalSignShoppingListCommand;
        private readonly string _oldtext = string.Empty;

        private ICommand _populatingShoppingListCommand;
        private int _selectedRow;
        private readonly UnitOfWork _unit = new UnitOfWork(new DataContext());
        private IConsumer _user;

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


        public ObservableCollection<ProduktInfo> ShoppingListData { get; } = new ObservableCollection<ProduktInfo>();
        public ObservableCollection<string> AutoCompleteList { get; } = new ObservableCollection<string>();
        public string ShoppingListItem { set; get; }

        public int SelectedRow
        {
            get { return _selectedRow; }
            set
            {
                _selectedRow = value;
                OnPropertyChanged();
            }
        }

        private void AddToShoppingList()
        {
            _user = new Consumer.Consumer(_unit);
            //ShoppingListItem _shoppingListItem = new ShoppingListItem();

            if (_user.DoesProductExsist(ShoppingListItem))
                ShoppingListData.Add(new ProduktInfo(ShoppingListItem));
            else
                MessageBox.Show("produktet findes ikke", "Error", MessageBoxButton.OK);
        }

        private void DeleteFromShoppingList()
        {
            if (SelectedRow == -1)
                MessageBox.Show("Du skal markere at produkt før du kan slette", "Error",
                    MessageBoxButton.OK);
            else if (ShoppingListData.Count == 0)
                MessageBox.Show("Der er ikke tilføjet nogen produkter", "Error",
                    MessageBoxButton.OK);
            else
                ShoppingListData.RemoveAt(SelectedRow);
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
                MessageBox.Show(
                    "Der kan kun skrives bogstaverne fra a til å og tallene fra 0 til 9", "ERROR", MessageBoxButton.OK);
                ShoppingListItem = _oldtext;
            }
        }

        private void ToGeneratedShoppingList()
        {
            //if (ShoppingList.Count == 0)
            //{
            //    return;
            //}

            //List<String> toGeneratedList = ShoppingList.Select(item => item.Name).ToList();

            //var TempGeneretedShopList = _user.CreateShoppingList(toGeneratedList);
            //foreach (var item in TempGeneretedShopList)
            //{
            //    GeneratedShoppingList.Add(item);
            //}
        }
    }
}