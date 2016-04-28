using System.Collections.ObjectModel;
using System.Net.Mail;
using System.Windows;
using System.Windows.Input;
using Consumer;
using Consumer_GUI.User_Controls;
using Pristjek220Data;
using SharedFunctionalities;

namespace Consumer_GUI
{
    internal class ApplicationViewModel : ObservableObject
    {
        private IPageViewModel _currentPageViewModel;
        private ObservableCollection<IPageViewModel> _pageViewModels;

        public ApplicationViewModel()
        {
            UnitOfWork unit = new UnitOfWork(new DataContext());
            Consumer.Consumer user = new Consumer.Consumer(unit);
            // Add available pages
            PageViewModels.Add(new HomeModel());
            PageViewModels.Add(new FindProductModel(user));
            PageViewModels.Add(new ShoppingListModel(user));
            PageViewModels.Add(new GeneratedShoppingListModel(user));

            IDatabaseFunctions databaseFunctions = new DatabaseFunctions(unit);

            if (!databaseFunctions.ConnectToDB()) //Force database to connect at startup, and close application if it cant connect
            {
                MessageBox.Show("Der kan ikke tilsluttes til serveren", "ERROR", MessageBoxButton.OK);
                Application.Current.MainWindow.Close();
            }


            // Set starting page
            CurrentPageViewModel = PageViewModels[0];
        }

        public ObservableCollection<IPageViewModel> PageViewModels
        {
            get
            {
                if (_pageViewModels == null)
                    _pageViewModels = new ObservableCollection<IPageViewModel>();

                return _pageViewModels;
            }
        }

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

        public ICommand ChangeWindowHomeCommand
        {
            get { return _changeWindowHomeCommand ?? (_changeWindowHomeCommand = new RelayCommand(ChangeWindowHome)); }
        }

        private void ChangeWindowHome()
        {
            CurrentPageViewModel = PageViewModels[0];
        }

        private ICommand _changeWindowFindProductCommand;

        public ICommand ChangeWindowFindProductCommand
        {
            get
            {
                return _changeWindowFindProductCommand ??
                       (_changeWindowFindProductCommand = new RelayCommand(ChangeWindowFindProduct));
            }
        }

        private void ChangeWindowFindProduct()
        {
            CurrentPageViewModel = PageViewModels[1];
        }

        private ICommand _changeWindowShoppingListCommand;

        public ICommand ChangeWindowShoppingListCommand
        {
            get
            {
                return _changeWindowShoppingListCommand ??
                       (_changeWindowShoppingListCommand = new RelayCommand(ChangeWindowShoppingList));
            }
        }

        private void ChangeWindowShoppingList()
        {
            CurrentPageViewModel = PageViewModels[2];
        }


        private ICommand _changeWindowGeneratedShoppingListCommand;

        public ICommand ChangeWindowGeneratedShoppingListCommand
        {
            get
            {
                return _changeWindowGeneratedShoppingListCommand ??
                       (_changeWindowGeneratedShoppingListCommand = new RelayCommand(ChangeWindowGeneratedShoppingList));
            }
        }

        private void ChangeWindowGeneratedShoppingList()
        {
            CurrentPageViewModel = PageViewModels[3];
        }
        #endregion
    }

    public interface IPageViewModel
    {
    }
}