using System.Collections.ObjectModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Timers;
using System.Windows.Input;
using Consumer;
using GalaSoft.MvvmLight.Command;
using Pristjek220Data;
using SharedFunctionalities;
using RelayCommand = SharedFunctionalities.RelayCommand;

[assembly: InternalsVisibleTo("Pristjek220.Unit.Test")]

namespace Consumer_GUI.User_Controls
{
    /// <summary>
    ///     FindProductModel is the User Control model for the FindProduct User Control
    ///     Its used to find a product and display the stores that it is sold in and the price
    /// </summary>
    internal class FindProductModel : ObservableObject, IPageViewModel
    {
        private readonly Timer _timer = new Timer(2500);
        private readonly IUnitOfWork _unit;
        private ICommand _addToStoreListCommand;
        private ICommand _enterPressedCommand;

        private string _error;
        private ICommand _illegalSignFindProductCommand;


        private bool _isTextConfirm;
        private string _oldtext = string.Empty;
        private ICommand _populatingFindProductCommand;


        private string _productName = string.Empty;

        /// <summary>
        ///     FindProductModel constructor takes a UnitOfWork and a user
        /// </summary>
        /// <param name="user"></param>
        /// <param name="unit"></param>
        public FindProductModel(IConsumer user, IUnitOfWork unit)
        {
            User = user;
            _unit = unit;
        }

        /// <summary>
        ///     Get method for Consumer
        /// </summary>
        public IConsumer User { get; }

        /// <summary>
        ///     Is a bool that is used to set the color of a label to red if it's a fail and green if it's expected behaviour
        /// </summary>
        public bool IsTextConfirm
        {
            get { return _isTextConfirm; }
            set
            {
                _isTextConfirm = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        ///     Get and Set method for Error. The set method, changes the value to the new vaule, call OnPropertyChanged and start
        ///     a timer, that resets the label.
        /// </summary>
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

        /// <summary>
        ///     Command that is used to look up a product, if anything goes wrong it will print the reason to why it
        ///     did not look up the product to a label
        /// </summary>
        public ICommand AddToStoreListCommand
            => _addToStoreListCommand ?? (_addToStoreListCommand = new RelayCommand(AddToStoreList));

        /// <summary>
        ///     Command that is used whenever there is an Populating event to populate the dropdown menu with the correct products
        /// </summary>
        public ICommand PopulatingFindProductCommand => _populatingFindProductCommand ??
                                                        (_populatingFindProductCommand =
                                                            new RelayCommand(PopulatingListFindProduct));

        /// <summary>
        ///     Command that is used whenever there is an TextChanged event to see if the text entered contains illegal signs
        /// </summary>
        public ICommand IllegalSignFindProductCommand => _illegalSignFindProductCommand ??
                                                         (_illegalSignFindProductCommand =
                                                             new RelayCommand(IllegalSignFindProduct));

        /// <summary>
        ///     Command that is used to see if Enter is pressed, if its pressed it calls the AddToStoreDatabase
        /// </summary>
        public ICommand EnterKeyPressedCommand
            => _enterPressedCommand ?? (_enterPressedCommand = new RelayCommand<KeyEventArgs>(EnterKeyPressed));

        /// <summary>
        ///     Get and Set method for ProductName. The set method, sets the old DeleteStoreName to an oldtext, and then
        ///     change the value to the new vaule and call OnPropertyChanged
        /// </summary>
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

        /// <summary>
        ///     Get method for AutoCompleteList, that is the list with the items that is getting populated to the dropdown.
        /// </summary>
        public ObservableCollection<string> AutoCompleteList { get; } = new ObservableCollection<string>();

        /// <summary>
        ///     Get and set method for StorePrice, that is the list with a price and a store, that is used to show what a product
        ///     cost and where it can be bought
        /// </summary>
        public ObservableCollection<StoreAndPrice> StorePrice { get; set; } = new ObservableCollection<StoreAndPrice>();

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