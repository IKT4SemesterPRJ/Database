using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Input;
using Administration;
using Administration_GUI.User_Controls;
using Pristjek220Data;
using SharedFunctionalities;

namespace Administration_GUI

{
    /// <summary>
    ///     StoremanagerViewModel is the view model for the Storemanager. Its used to change between the the different user
    ///     controlls in Storemanager
    /// </summary>
    internal class StoremanagerViewModel : ObservableObject
    {
        private readonly IStoremanager _storemanager;

        private ICommand _changeWindowChangePriceCommand;

        private ICommand _changeWindowDeleteProductCommand;

        private ICommand _changeWindowNewProductCommand;
        private IPageViewModel _currentPageViewModel;

        private ICommand _logOutCommand;
        private string _mainWindowTekst;
        private ObservableCollection<IPageViewModel> _pageViewModels;

        /// <summary>
        ///     StoremanagerViewModel constructor takes a UnitOfWork to give to each of its user controls view models and add them
        ///     to a list.
        /// </summary>
        public StoremanagerViewModel(IStoremanager storemanager, IAutocomplete autocomplete)
        {
            // Add available pages
            _storemanager = storemanager;
            PageViewModels.Add(new ChangePriceModel(storemanager, autocomplete, new CreateMsgBox()));
            PageViewModels.Add(new DeleteProductModel(storemanager, autocomplete, new CreateMsgBox()));
            PageViewModels.Add(new NewProductModel(storemanager, autocomplete, new CreateMsgBox()));
            
            // set startup page
            MainWindowTekst = $"Pristjek220 - {_storemanager.Store.StoreName} - Tilføj Produkt";
            _currentPageViewModel = _pageViewModels[2];
        }

        /// <summary>
        ///     The MainWindowTekst string is written to a label on the GUI that describes where the user is at
        /// </summary>
        public string MainWindowTekst
        {
            get { return _mainWindowTekst; }
            set
            {
                _mainWindowTekst = value;
                OnPropertyChanged();
            }
        }


        /// <summary>
        ///     ObservableCollection that contains the user controlls, that inherit from IPageViewModel
        /// </summary>
        public ObservableCollection<IPageViewModel> PageViewModels
            => _pageViewModels ?? (_pageViewModels = new ObservableCollection<IPageViewModel>());

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

        /// <summary>
        ///     Change the user controll to ChangePrice
        /// </summary>
        public ICommand ChangeWindowChangePriceCommand
            =>
                _changeWindowChangePriceCommand ??
                (_changeWindowChangePriceCommand = new RelayCommand(ChangeWindowChangePrice));

        /// <summary>
        ///     Change the user controll to DeleteProduct
        /// </summary>
        public ICommand ChangeWindowDeleteProductCommand => _changeWindowDeleteProductCommand ??
                                                            (_changeWindowDeleteProductCommand =
                                                                new RelayCommand(ChangeWindowDeleteProduct));

        /// <summary>
        ///     Change the user controll to NewProduct
        /// </summary>
        public ICommand ChangeWindowNewProductCommand => _changeWindowNewProductCommand ??
                                                         (_changeWindowNewProductCommand =
                                                             new RelayCommand(ChangeWindowNewProduct));

        /// <summary>
        ///     Close the window and open the LogIn window
        /// </summary>
        public ICommand LogOutCommand => _logOutCommand ??
                                         (_logOutCommand = new RelayCommand(LogOut));

        private void ChangeWindowChangePrice()
        {
            CurrentPageViewModel = PageViewModels[0];
            MainWindowTekst = $"Pristjek220 - {_storemanager.Store.StoreName} - Ændre pris";
        }

        private void ChangeWindowDeleteProduct()
        {
            CurrentPageViewModel = PageViewModels[1];
            MainWindowTekst = $"Pristjek220 - {_storemanager.Store.StoreName} - Fjern Produkt";
        }

        private void ChangeWindowNewProduct()
        {
            CurrentPageViewModel = PageViewModels[2];
            MainWindowTekst = $"Pristjek220 - {_storemanager.Store.StoreName} - Tilføj Produkt";
        }

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