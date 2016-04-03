using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;
using System.Windows.Input;
using Prism.Events;
using Storemanager_GUI.User_Controls;

namespace Storemanager_GUI

{
    internal class ApplicationViewModel : ObservableObject
    {
        private IPageViewModel _currentPageViewModel;
        private ObservableCollection<IPageViewModel> _pageViewModels;

        public ApplicationViewModel()
        {
            IEventAggregator Event = new EventAggregator();
            // Add available pages
            PageViewModels.Add(new ChangePriceModel());
            PageViewModels.Add(new DeleteProductModel());
            PageViewModels.Add(new NewProductModel(Event));


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

        #endregion
    }

    public interface IPageViewModel
    {
    }
}