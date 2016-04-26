using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Input;
using Administration_GUI.User_Controls_Admin;
using Administration_GUI;
using Pristjek220Data;

namespace Administration_GUI
{
    class AdminViewModel : ObservableObject
    {
        private IPageViewModelAdmin _currentPageViewModel;
        private ObservableCollection<IPageViewModelAdmin> _pageViewModels;

        public AdminViewModel(IUnitOfWork unit)
        {
            // Add available pages
            PageViewModels.Add(new AdminNewStoreModel(unit));
            PageViewModels.Add(new AdminDeleteProductModel(unit));
            PageViewModels.Add(new AdminDeleteStoreModel(unit));

            // set startup page
            _currentPageViewModel = _pageViewModels[0];
        }

        public ObservableCollection<IPageViewModelAdmin> PageViewModels
        {
            get { return _pageViewModels ?? (_pageViewModels = new ObservableCollection<IPageViewModelAdmin>()); }
        }

        public IPageViewModelAdmin CurrentPageViewModel
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

        public ICommand AdminChangeWindowNewStoreCommand => _adminChangeWindowNewStoreCommand ??
                                                            (_adminChangeWindowNewStoreCommand = new RelayCommand(AdminChangeWindowNewStore));

        private void AdminChangeWindowNewStore()
        {
            CurrentPageViewModel = PageViewModels[0];
        }

        private ICommand _adminChangeWindowDeleteProductCommand;

        public ICommand AdminChangeWindowDeleteProductCommand => _adminChangeWindowDeleteProductCommand ??
                                                                 (_adminChangeWindowDeleteProductCommand = new RelayCommand(AdminChangeWindowDeleteProduct));

        private void AdminChangeWindowDeleteProduct()
        {
            CurrentPageViewModel = PageViewModels[1];
        }

        private ICommand _adminChangeWindowDeleteStoreCommand;

        public ICommand AdminChangeWindowDeleteStoreCommand => _adminChangeWindowDeleteStoreCommand ??
                                                               (_adminChangeWindowDeleteStoreCommand = new RelayCommand(AdminChangeWindowDeleteStore));

        private void AdminChangeWindowDeleteStore()
        {
            CurrentPageViewModel = PageViewModels[2];
        }

        private ICommand _logOutCommand;

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

    public interface IPageViewModelAdmin
    {
    }
}
