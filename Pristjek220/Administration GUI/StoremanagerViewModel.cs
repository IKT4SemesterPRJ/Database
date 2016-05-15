using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Input;
using Administration_GUI.User_Controls;
using Pristjek220Data;
using SharedFunctionalities;

namespace Administration_GUI

{
    /// <summary>
    ///     StoremanagerViewModel is the view model for the Storemanager. Its used to change between the the different user controlls in Storemanager
    /// </summary>
    internal class StoremanagerViewModel : ObservableObject
    {
       private IPageViewModel _currentPageViewModel;
        private ObservableCollection<IPageViewModel> _pageViewModels;
        private readonly Store _store;
        private string _mainWindowTekst;

        /// <summary>
        ///     StoremanagerViewModel constructor takes a UnitOfWork to give to each of its user controls view models
        /// </summary>
        public StoremanagerViewModel(Store store, IUnitOfWork unit)
        {

            _store = store;
            // Add available pages

            PageViewModels.Add(new ChangePriceModel(_store, unit));
            PageViewModels.Add(new DeleteProductModel(_store, unit));
            PageViewModels.Add(new NewProductModel(_store, unit));

            // set startup page
            MainWindowTekst = $"Pristjek220 - {_store.StoreName} - Tilføj Produkt";
            _currentPageViewModel = _pageViewModels[2];

        }

        /// <summary>
        ///     The MainWindowTekst string is written to a label on the GUI that describes where the user is at
        /// </summary>
        public string MainWindowTekst
        {
            get { return _mainWindowTekst; }
            set { _mainWindowTekst = value; OnPropertyChanged(); }
        }



        /// <summary>
        ///     ObservableCollection that contains the user controlls, that inherit from IPageViewModel
        /// </summary>
        public ObservableCollection<IPageViewModel> PageViewModels => _pageViewModels ?? (_pageViewModels = new ObservableCollection<IPageViewModel>());

        /// <summary>
        ///     Get and set method for the Current view model, with OnPropertyChanged
        /// </summary>
        public IPageViewModel CurrentPageViewModel
        {
            get { return _currentPageViewModel; }
            set
            {
                if (_currentPageViewModel == value) return;
                _currentPageViewModel = value;
                OnPropertyChanged();
            }
        }
        
        private ICommand _changeWindowChangePriceCommand;

        /// <summary>
        ///     Change the user controll to ChangePrice
        /// </summary>
        public ICommand ChangeWindowChangePriceCommand => _changeWindowChangePriceCommand ?? (_changeWindowChangePriceCommand = new RelayCommand(ChangeWindowChangePrice));

        private void ChangeWindowChangePrice()
        {
            CurrentPageViewModel = PageViewModels[0];
            MainWindowTekst = $"Pristjek220 - {_store.StoreName} - Ændre pris";
        }

        private ICommand _changeWindowDeleteProductCommand;

        /// <summary>
        ///     Change the user controll to DeleteProduct
        /// </summary>
        public ICommand ChangeWindowDeleteProductCommand => _changeWindowDeleteProductCommand ??
                                                            (_changeWindowDeleteProductCommand = new RelayCommand(ChangeWindowDeleteProduct));

        private void ChangeWindowDeleteProduct()
        {
            CurrentPageViewModel = PageViewModels[1];
            MainWindowTekst = $"Pristjek220 - {_store.StoreName} - Fjern Produkt";
        }

        private ICommand _changeWindowNewProductCommand;

        /// <summary>
        ///     Change the user controll to NewProduct
        /// </summary>
        public ICommand ChangeWindowNewProductCommand => _changeWindowNewProductCommand ??
                                                         (_changeWindowNewProductCommand = new RelayCommand(ChangeWindowNewProduct));

        private void ChangeWindowNewProduct()
        {
            CurrentPageViewModel = PageViewModels[2];
            MainWindowTekst = $"Pristjek220 - {_store.StoreName} - Tilføj Produkt";
        }

        private ICommand _logOutCommand;

        /// <summary>
        ///     Close the window and open the LogIn window
        /// </summary>
        public ICommand LogOutCommand => _logOutCommand ??
                                         (_logOutCommand = new RelayCommand(LogOut));

        private void LogOut()
        {
            var currentWindow = Application.Current.MainWindow;
            var logIn = new LogIn();
            logIn.Show();
            currentWindow.Close();
            Application.Current.MainWindow = logIn;

        }
    }
}