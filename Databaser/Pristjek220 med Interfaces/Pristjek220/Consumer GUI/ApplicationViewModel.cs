using System.Collections.ObjectModel;
using System.Windows.Input;
using Consumer_GUI.User_Controls;
using Prism.Events;
using Pristjek220Data;

namespace Consumer_GUI
{
    internal class ApplicationViewModel : ObservableObject
    {
        private IPageViewModel _currentPageViewModel;
        private ObservableCollection<IPageViewModel> _pageViewModels;

        public ApplicationViewModel()
        {
            Consumer.Consumer user = new Consumer.Consumer(new UnitOfWork(new DataContext()));
            // Add available pages
            PageViewModels.Add(new HomeModel());
            PageViewModels.Add(new FindProductModel());
            PageViewModels.Add(new ShoppingListModel(user));
            PageViewModels.Add(new GeneratedShoppingListModel(user));


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

            //ToGeneratedShoppingList();
        }

        //private void ToGeneratedShoppingList()
        //{
        //    if (ShoppingList.Count == 0)
        //    {
        //        return;
        //    }

        //    List<String> toGeneratedList = ShoppingList.Select(item => item.Name).ToList();

        //    var TempGeneretedShopList = _user.CreateShoppingList(toGeneratedList);
        //    foreach (var item in TempGeneretedShopList)
        //    {
        //        GeneratedShoppingList.Add(item);
        //    }
        //}

        #endregion
    }

    public interface IPageViewModel
    {
    }
}