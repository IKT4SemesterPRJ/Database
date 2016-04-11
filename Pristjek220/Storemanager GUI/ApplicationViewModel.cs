using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;
using System.Reflection.Emit;
using System.Windows.Input;
using Storemanager_GUI.User_Controls;

namespace Storemanager_GUI

{
    internal class ApplicationViewModel : ObservableObject
    {
        private IPageViewModel _currentPageViewModel;
        private ObservableCollection<IPageViewModel> _pageViewModels;

        public ApplicationViewModel()
        {
            // Add available pages
            PageViewModels.Add(new ChangePriceModel());
            PageViewModels.Add(new DeleteProductModel());
            PageViewModels.Add(new NewProductModel());

            // set startup page
            _currentPageViewModel = _pageViewModels[2];

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

        
        private ICommand _changeWindowChangePriceCommand;

        public ICommand ChangeWindowChangePriceCommand
        {
            get { return _changeWindowChangePriceCommand ?? (_changeWindowChangePriceCommand = new RelayCommand(ChangeWindowChangePrice)); }
        }

        private void ChangeWindowChangePrice()
        {
            CurrentPageViewModel = PageViewModels[0];
        }

        private ICommand _changeWindowDeleteProductCommand;

        public ICommand ChangeWindowDeleteProductCommand
        {
            get
            {
                return _changeWindowDeleteProductCommand ??
                       (_changeWindowDeleteProductCommand = new RelayCommand(ChangeWindowDeleteProduct));
            }
        }

        private void ChangeWindowDeleteProduct()
        {
            CurrentPageViewModel = PageViewModels[1];
        }

        private ICommand _changeWindowNewProductCommand;

        public ICommand ChangeWindowNewProductCommand
        {
            get
            {
                return _changeWindowNewProductCommand ??
                       (_changeWindowNewProductCommand = new RelayCommand(ChangeWindowNewProduct));
            }
        }

        private void ChangeWindowNewProduct()
        {
            CurrentPageViewModel = PageViewModels[2];
        }

     
        #endregion
    }

    public interface IPageViewModel
    {
    }
}