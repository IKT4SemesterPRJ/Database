using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using AutoComplete;
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

        private int _selectedRow;
        private readonly UnitOfWork _unit = new UnitOfWork(new DataContext());
        private IConsumer _user;
        private IEventAggregator Event;

        public ShoppingListModel(IEventAggregator eventAggregator)
        {
            Event = eventAggregator;
            _user = new Consumer.Consumer(_unit);
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


        private void GeneratedShoppingListFromShoppingList()
        {

            if (ShoppingListData.Count == 0)
            {
                return;
            }

            List<ProduktInfo> toGeneratedList = new List<ProduktInfo>();
            foreach (var item in ShoppingListData)
            {
                ProduktInfo Data = new ProduktInfo(item.Name, item.Quantity);
                toGeneratedList.Add(Data);
            }
            
            Event.GetEvent<PubSubEvent<List<ProduktInfo>>>().Publish(toGeneratedList);

        }
    }
}