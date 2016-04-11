using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Admin_GUI.User_Controls;

namespace Admin_GUI
{
    internal class ApplicationViewModel : ObservableObject
        {
            private IPageViewModel _currentPageViewModel;
            private ObservableCollection<IPageViewModel> _pageViewModels;

            public ApplicationViewModel()
            {
                // Add available pages
                PageViewModels.Add(new NewStoreModel());
                PageViewModels.Add(new DeleteStoreModel());
                PageViewModels.Add(new DeleteProductModel());
                PageViewModels.Add(new NewProductModel());


                // Set starting page
                CurrentPageViewModel = PageViewModels[3];
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

            private ICommand _changeWindowNewStoreCommand;

            public ICommand ChangeWindowNewStoreCommand
            {
                get { return _changeWindowNewStoreCommand ?? (_changeWindowNewStoreCommand = new RelayCommand(ChangeWindowNewStore)); }
            }

            private void ChangeWindowNewStore()
            {
                CurrentPageViewModel = PageViewModels[0];
            }

            private ICommand _changeWindowDeleteStoreCommand;

            public ICommand ChangeWindowDeleteStoreCommand
            {
                get
                {
                    return _changeWindowDeleteStoreCommand ??
                           (_changeWindowDeleteStoreCommand = new RelayCommand(ChangeWindowDeleteStore));
                }
            }

            private void ChangeWindowDeleteStore()
            {
                CurrentPageViewModel = PageViewModels[1];
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
                CurrentPageViewModel = PageViewModels[2];
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
                CurrentPageViewModel = PageViewModels[3];
            }

        
        #endregion
    }
    

    public interface IPageViewModel
    {
    }
   
}
