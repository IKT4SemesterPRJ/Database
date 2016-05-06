using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Windows.Input;
using Consumer;
using GalaSoft.MvvmLight.Command;
using Pristjek220Data;

namespace Consumer_GUI.User_Controls
{
    public class GeneratedShoppingListModel : ObservableObject, IPageViewModel
    {
        private readonly IMail _mail;
        private readonly IConsumer _user;

        private string _errorText;
        private string _emailAddress = string.Empty;

        private ICommand _sendMailCommand;
        private ICommand _storeChangedCommand;
        private ICommand _enterPressedCommand;
        private EmailAddressAttribute _testEmail;

        public GeneratedShoppingListModel(IConsumer user, IMail mail)
        {
            _user = user;
            _mail = mail;
            ErrorText = "";
        }

        public string TotalSum => _user.TotalSum;
        public string ErrorStore { get; set; }
        public string EmailAddress
        {
            set
            {
                _emailAddress = value;
                OnPropertyChanged();
            }

            get { return _emailAddress; }
        }
        public string BuyInOneStore => _user.BuyInOneStore;
        public string MoneySaved => _user.MoneySaved;

        public int SelectedIndexGeneratedShoppingList { get; set; }

        public int SelectedStoreIndex { get; set; }

        public string ErrorText
        {
            set
            {
                _errorText = value;
                OnPropertyChanged();
            }
            get { return _errorText; }
        }

        public string ChoosenStoreName { get; set; }

        public List<string> StoreNames
        {
            get { return _user.StoreNames; }
        }

        public ObservableCollection<StoreProductAndPrice> GeneratedShoppingListData
        {
            get
            {
                return
                    new ObservableCollection<StoreProductAndPrice>(
                        _user.GeneratedShoppingListData.OrderBy(listData => listData.StoreName)
                            .ThenBy(listData => listData.ProductName));
            }
        }

        public ObservableCollection<ProductInfo> NotInAStore
        {
            get
            {
                return
                    new ObservableCollection<ProductInfo>(
                        _user.NotInAStore.OrderBy(listData => listData.Name));
            }
        }

        public ICommand SendMailCommand
        {
            get { return _sendMailCommand ?? (_sendMailCommand = new RelayCommand(SendMail)); }
        }


        public ICommand StoreChangedCommand
        {
            get { return _storeChangedCommand ?? (_storeChangedCommand = new RelayCommand(StoreChanged)); }
        }

        public ICommand EnterKeyPressedCommand
        {
            get
            {
                return _enterPressedCommand ?? (_enterPressedCommand = new RelayCommand<KeyEventArgs>(EnterKeyPressed));
            }
        }

        private void StoreChanged()
        {
            if (SelectedIndexGeneratedShoppingList != -1)
            {
                if (
                    _user.ChangeItemToAnotherStore(StoreNames[SelectedStoreIndex],
                        GeneratedShoppingListData[SelectedIndexGeneratedShoppingList]) == 1)
                {
                    OnPropertyChanged("TotalSum");
                    ErrorStore = "";
                }
                else
                {
                    ErrorStore = StoreNames[SelectedStoreIndex] + " har ikke " +
                                 GeneratedShoppingListData[SelectedIndexGeneratedShoppingList].ProductName +
                                 " i deres sortiment";
                }
                OnPropertyChanged("ErrorStore");
                OnPropertyChanged("GeneratedShoppingListData");
            }
        }

        private void SendMail()
        {
            _testEmail = new EmailAddressAttribute();
            if (_testEmail.IsValid(EmailAddress) && EmailAddress != null)
            {
                ErrorText = "E-mail afsendt";
                _mail.SendMail(EmailAddress, GeneratedShoppingListData, NotInAStore, TotalSum);
            }
            else
            {
                ErrorText = "E-mail skal overholde formatet: abc@mail.com";
            }
        }

        private void EnterKeyPressed(KeyEventArgs e)
        {
            if ((e.Key == Key.Enter) || (e.Key == Key.Return))
            {
                SendMail();
            }
        }
    }
}