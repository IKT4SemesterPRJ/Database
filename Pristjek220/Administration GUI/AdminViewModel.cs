using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Input;
using Administration_GUI.User_Controls_Admin;
using Pristjek220Data;
using SharedFunctionalities;

namespace Administration_GUI
{
    /// <summary>
    ///     AdminViewModel is the view model for the Admin. Its used to change between the the different user controlls in
    ///     Admin
    /// </summary>
    internal class AdminViewModel : ObservableObject
    {
        private IPageViewModel _currentPageViewModel;
        private string _mainWindowTekst;
        private ObservableCollection<IPageViewModel> _pageViewModels;

        /// <summary>
        ///     AdminViewModel constructor takes a UnitOfWork to give to each of its user controls view models
        /// </summary>
        public AdminViewModel(IUnitOfWork unit)
        {
            // Add available pages
            PageViewModels.Add(new AdminNewStoreModel(unit));
            PageViewModels.Add(new AdminDeleteProductModel(unit));
            PageViewModels.Add(new AdminDeleteStoreModel(unit));

            // set startup page
            MainWindowTekst = "Pristjek220 - Administration - Tilføj Forretning";
            _currentPageViewModel = _pageViewModels[0];
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

        #region Commands

        private ICommand _adminChangeWindowNewStoreCommand;

        /// <summary>
        ///     Change the user controll to NewStore
        /// </summary>
        public ICommand AdminChangeWindowNewStoreCommand => _adminChangeWindowNewStoreCommand ??
                                                            (_adminChangeWindowNewStoreCommand =
                                                                new RelayCommand(AdminChangeWindowNewStore));

        private void AdminChangeWindowNewStore()
        {
            CurrentPageViewModel = PageViewModels[0];
            MainWindowTekst = "Pristjek220 - Administration - Tilføj Forretning";
        }

        private ICommand _adminChangeWindowDeleteProductCommand;

        /// <summary>
        ///     Change the user controll to DeleteProduct
        /// </summary>
        public ICommand AdminChangeWindowDeleteProductCommand => _adminChangeWindowDeleteProductCommand ??
                                                                 (_adminChangeWindowDeleteProductCommand =
                                                                     new RelayCommand(AdminChangeWindowDeleteProduct));

        private void AdminChangeWindowDeleteProduct()
        {
            CurrentPageViewModel = PageViewModels[1];
            MainWindowTekst = "Pristjek220 - Administration - Fjern Produkt";
        }

        private ICommand _adminChangeWindowDeleteStoreCommand;

        /// <summary>
        ///     Change the user controll to DeleteStore
        /// </summary>
        public ICommand AdminChangeWindowDeleteStoreCommand => _adminChangeWindowDeleteStoreCommand ??
                                                               (_adminChangeWindowDeleteStoreCommand =
                                                                   new RelayCommand(AdminChangeWindowDeleteStore));

        private void AdminChangeWindowDeleteStore()
        {
            CurrentPageViewModel = PageViewModels[2];
            MainWindowTekst = "Pristjek220 - Administration - Fjern Forretning";
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

        #endregion
    }
}