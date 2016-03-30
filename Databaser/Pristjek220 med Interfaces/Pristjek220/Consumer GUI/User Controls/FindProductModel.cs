using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using AutoComplete;
using Consumer;
using Pristjek220Data;

namespace Consumer_GUI.User_Controls
{
    internal class FindProductModel : ObservableObject, IPageViewModel
    {
        private readonly UnitOfWork _unit = new UnitOfWork(new DataContext());
        private ICommand _addToStoreListCommand;

        private ICommand _illegalSignFindProductCommand;
        private string _oldtext = string.Empty;

        private ICommand _populatingFindProductCommand;


        private string _productName;
        private IConsumer _user;

        public ICommand AddToStoreListCommand
        {
            get { return _addToStoreListCommand ?? (_addToStoreListCommand = new RelayCommand(AddToStoreList)); }
        }

        public ICommand PopulatingFindProductCommand
        {
            get
            {
                return _populatingFindProductCommand ??
                       (_populatingFindProductCommand = new RelayCommand(PopulatingListFindProduct));
            }
        }

        public ICommand IllegalSignFindProductCommand
        {
            get
            {
                return _illegalSignFindProductCommand ??
                       (_illegalSignFindProductCommand = new RelayCommand(IllegalSignFindProduct));
            }
        }

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
        public ObservableCollection<StoreAndPrice> StorePrice { get; set; } = new ObservableCollection<StoreAndPrice>();

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


        private void IllegalSignFindProduct()
        {
            if (!ProductName.All(chr => char.IsLetter(chr) || char.IsNumber(chr) || char.IsWhiteSpace(chr)))
            {
                MessageBox.Show(
                    "Der kan kun skrives bogstaverne fra a til å og tallene fra 0 til 9", "ERROR", MessageBoxButton.OK);
                ProductName = _oldtext;
            }
        }
    }
}