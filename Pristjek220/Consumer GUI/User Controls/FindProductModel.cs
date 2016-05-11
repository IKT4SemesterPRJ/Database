using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;
using Consumer;
using GalaSoft.MvvmLight.Command;
using Pristjek220Data;
using System.Runtime.CompilerServices;
using System.Timers;
using SharedFunctionalities;

[assembly:InternalsVisibleTo("Pristjek220.Unit.Test")]


namespace Consumer_GUI.User_Controls
{
    internal class FindProductModel : ObservableObject, IPageViewModel
    {
        private readonly Timer _timer = new Timer(2500);
        private readonly IUnitOfWork _unit;
        private ICommand _addToStoreListCommand;
        private ICommand _enterPressedCommand;

        private ICommand _illegalSignFindProductCommand;
        private string _oldtext = string.Empty;

        private ICommand _populatingFindProductCommand;

        private string _productName = string.Empty;
        public IConsumer User { get; }

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

        private string _error;

        public string Error
        {
            get { return _error; }
            set
            {
                _error = value;
                OnPropertyChanged();
                _timer.Stop();
                _timer.Start();
                _timer.Elapsed += delegate { _error = ""; OnPropertyChanged(); };
            }
        }

        public ICommand AddToStoreListCommand => _addToStoreListCommand ?? (_addToStoreListCommand = new RelayCommand(AddToStoreList));

        public ICommand PopulatingFindProductCommand => _populatingFindProductCommand ??
                                                        (_populatingFindProductCommand = new RelayCommand(PopulatingListFindProduct));

        public ICommand IllegalSignFindProductCommand => _illegalSignFindProductCommand ??
                                                         (_illegalSignFindProductCommand = new RelayCommand(IllegalSignFindProduct));

        public ICommand EnterKeyPressedCommand => _enterPressedCommand ?? (_enterPressedCommand = new RelayCommand<KeyEventArgs>(EnterKeyPressed));

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

        public FindProductModel(IConsumer user, IUnitOfWork unit)
        {
            User = user;
            _unit = unit;
        }

        private void AddToStoreList()
        {
            if (string.IsNullOrEmpty(ProductName))
            {
                IsTextConfirm = false;
                Error = "Indtast venligst det produkt du vil søge efter.";
                return;
            }
            
            StorePrice.Clear();

            var list = User.FindStoresThatSellsProduct(ProductName);
            if (list.Count != 0)
            {
                foreach (var store in list)
                {
                    StorePrice.Add(store);
                }
                StorePrice = new ObservableCollection<StoreAndPrice>(StorePrice.OrderBy(storePrice => storePrice.Price));
                OnPropertyChanged("StorePrice");
                Error = "";
            }
            else
            {
                IsTextConfirm = false;
                Error = $"Produktet \"{ProductName}\" findes ikke i Pristjek220.";
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


        private void IllegalSignFindProduct()
        {
            if (ProductName.All(chr => char.IsLetter(chr) || char.IsNumber(chr) || char.IsWhiteSpace(chr))) return;
            IsTextConfirm = false;
            Error = "Der kan kun skrives bogstaverne fra a til å og tallene fra 0 til 9.";
            ProductName = _oldtext;
        }

        private void EnterKeyPressed(KeyEventArgs e)
        {
            if ((e.Key == Key.Enter) || (e.Key == Key.Return))
            {
                AddToStoreList();
            }
        }
    }
}