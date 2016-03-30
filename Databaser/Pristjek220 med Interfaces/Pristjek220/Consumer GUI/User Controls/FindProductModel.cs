using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using AutoComplete;
using Consumer;
using Pristjek220Data;

namespace Consumer_GUI.User_Controls
{
    class FindProductModel : ObservableObject, IPageViewModel
    {
        private UnitOfWork _unit = new UnitOfWork(new DataContext());
        private IConsumer _user;
        private string _oldtext = string.Empty;
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
        public ObservableCollection<StoreAndPrice> StorePrice { get; set; } = new ObservableCollection<StoreAndPrice>();

    }
}
