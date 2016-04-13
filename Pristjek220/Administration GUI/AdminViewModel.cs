using System.Collections.ObjectModel;
using System.Windows.Input;
using Administration_GUI.User_Controls_Admin;
using Storemanager_GUI;

namespace Administration_GUI
{
    class AdminViewModel : ObservableObject
    {
        private IPageViewModelAdmin _currentPageViewModel;
        private ObservableCollection<IPageViewModelAdmin> _pageViewModels;

        public AdminViewModel()
        {
            // Add available pages
            PageViewModels.Add(new AdminNewProductModel());
            PageViewModels.Add(new AdminNewStoreModel());
            PageViewModels.Add(new AdminDeleteProductModel());
            PageViewModels.Add(new AdminDeleteStoreModel());

            // set startup page
            _currentPageViewModel = _pageViewModels[0];
        }

        public ObservableCollection<IPageViewModelAdmin> PageViewModels
        {
            get
            {
                if (_pageViewModels == null)
                    _pageViewModels = new ObservableCollection<IPageViewModelAdmin>();

                return _pageViewModels;
            }
        }

        public IPageViewModelAdmin CurrentPageViewModel
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


        private ICommand _adminChangeWindowNewProductCommand;

        public ICommand AdminChangeWindowNewProductCommand
        {
            get
            {
                return _adminChangeWindowNewProductCommand ??
                       (_adminChangeWindowNewProductCommand = new RelayCommand(AdminChangeWindowNewProduct));
            }
        }

        private void AdminChangeWindowNewProduct()
        {
            CurrentPageViewModel = PageViewModels[0];
        }

        private ICommand _adminChangeWindowNewStoreCommand;

        public ICommand AdminChangeWindowNewStoreCommand
        {
            get
            {
                return _adminChangeWindowNewStoreCommand ??
                       (_adminChangeWindowNewStoreCommand = new RelayCommand(AdminChangeWindowNewStore));
            }
        }

        private void AdminChangeWindowNewStore()
        {
            CurrentPageViewModel = PageViewModels[1];
        }

        private ICommand _adminChangeWindowDeleteProductCommand;

        public ICommand AdminChangeWindowDeleteProductCommand
        {
            get
            {
                return _adminChangeWindowDeleteProductCommand ??
                       (_adminChangeWindowDeleteProductCommand = new RelayCommand(AdminChangeWindowDeleteProduct));
            }
        }

        private void AdminChangeWindowDeleteProduct()
        {
            CurrentPageViewModel = PageViewModels[2];
        }

        private ICommand _adminChangeWindowDeleteStoreCommand;

        public ICommand AdminChangeWindowDeleteStoreCommand
        {
            get
            {
                return _adminChangeWindowDeleteStoreCommand ??
                       (_adminChangeWindowDeleteStoreCommand = new RelayCommand(AdminChangeWindowDeleteStore));
            }
        }

        private void AdminChangeWindowDeleteStore()
        {
            CurrentPageViewModel = PageViewModels[3];
        }


        #endregion
    }

    public interface IPageViewModelAdmin
    {
    }
}
