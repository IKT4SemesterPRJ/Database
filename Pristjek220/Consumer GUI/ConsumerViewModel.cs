using System.Collections.ObjectModel;
using System.Net;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Input;
using Consumer;
using Consumer_GUI.User_Controls;
using Pristjek220Data;
using SharedFunctionalities;

[assembly: InternalsVisibleTo("Pristjek220.Unit.Test")]

namespace Consumer_GUI
{
    /// <summary>
    ///     ConsumerViewModel is the view model for the Consumer. Its used to change between the the different user controlls
    ///     in Consumer
    /// </summary>
    public class ConsumerViewModel : ObservableObject
    {
        private IPageViewModel _currentPageViewModel;
        private string _mainWindowTekst;
        private ObservableCollection<IPageViewModel> _pageViewModels;


        /// <summary>
        ///     ConsumerViewModel constructor creates a Consumer, adds the user controlls to a list and connects to the database 
        /// </summary>
        public ConsumerViewModel(IConsumer user, IAutocomplete autocomplete, IDatabaseFunctions databaseFunctions)
        {
            // Add available pages
            PageViewModels.Add(new HomeModel());
            PageViewModels.Add(new FindProductModel(user, autocomplete));
            PageViewModels.Add(new ShoppingListModel(user, autocomplete));
            PageViewModels.Add(new GeneratedShoppingListModel(user,
                new Mail(new SmtpClientWrapper("Smtp.gmail.com", 587,
                    new NetworkCredential("pristjek220@gmail.com", "pristjek"), true))));


            if (!databaseFunctions.ConnectToDb())
                //Force database to connect at startup, and close application if it cant connect
            {
                MessageBox.Show("Der kan ikke tilsluttes til serveren", "ERROR", MessageBoxButton.OK);
                Application.Current.MainWindow.Close();
            }


            // Set starting page
            MainWindowTekst = "Pristjek220 - Forbruger - Startside";
            CurrentPageViewModel = PageViewModels[0];
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
        public ObservableCollection<IPageViewModel> PageViewModels => _pageViewModels ?? (_pageViewModels = new ObservableCollection<IPageViewModel>());

        /// <summary>
        ///     Get and set method for the Current view model, with OnPropertyChanged
        /// </summary>
        public IPageViewModel CurrentPageViewModel
        {
            get { return _currentPageViewModel; }
            set
            {
                if (_currentPageViewModel != value)
                {
                    _currentPageViewModel = value;
                    OnPropertyChanged();
                }
            }
        }

        #region Commands

        private ICommand _changeWindowHomeCommand;

        /// <summary>
        ///     Change the user controll to Home
        /// </summary>
        public ICommand ChangeWindowHomeCommand => _changeWindowHomeCommand ?? (_changeWindowHomeCommand = new RelayCommand(ChangeWindowHome));

        private void ChangeWindowHome()
        {
            CurrentPageViewModel = PageViewModels[0];
            MainWindowTekst = "Pristjek220 - Forbruger - Startside";
        }

        private ICommand _changeWindowFindProductCommand;

        /// <summary>
        ///     Change the user controll to FindProduct
        /// </summary>
        public ICommand ChangeWindowFindProductCommand => _changeWindowFindProductCommand ??
                                                          (_changeWindowFindProductCommand = new RelayCommand(ChangeWindowFindProduct));

        private void ChangeWindowFindProduct()
        {
            CurrentPageViewModel = PageViewModels[1];
            MainWindowTekst = "Pristjek220 - Forbruger - Søg efter vare";
        }

        private ICommand _changeWindowShoppingListCommand;

        /// <summary>
        ///     Change the user controll to ShoppingList
        /// </summary>
        public ICommand ChangeWindowShoppingListCommand => _changeWindowShoppingListCommand ??
                                                           (_changeWindowShoppingListCommand = new RelayCommand(ChangeWindowShoppingList));

        private void ChangeWindowShoppingList()
        {
            CurrentPageViewModel = PageViewModels[2];
            MainWindowTekst = "Pristjek220 - Forbruger - Indkøbsliste";
        }


        private ICommand _changeWindowGeneratedShoppingListCommand;

        /// <summary>
        ///     Change the user controll to GeneratedShoppingList
        /// </summary>
        public ICommand ChangeWindowGeneratedShoppingListCommand => _changeWindowGeneratedShoppingListCommand ??
                                                                    (_changeWindowGeneratedShoppingListCommand = new RelayCommand(ChangeWindowGeneratedShoppingList));

        private void ChangeWindowGeneratedShoppingList()
        {
            CurrentPageViewModel = PageViewModels[3];
            MainWindowTekst = "Pristjek220 - Forbruger - Genereret Indkøbsliste";
        }

        #endregion
    }
}