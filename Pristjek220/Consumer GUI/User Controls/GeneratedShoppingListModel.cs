using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading;
using System.Windows.Input;
using Consumer;
using GalaSoft.MvvmLight.Command;
using Pristjek220Data;
using System.Timers;

namespace Consumer_GUI.User_Controls
{
    public class GeneratedShoppingListModel : ObservableObject, IPageViewModel
    {
        private readonly System.Timers.Timer _timerErrorStore = new System.Timers.Timer(2500);
        private readonly System.Timers.Timer _timerErrorText = new System.Timers.Timer(2500);
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
        private string _errorStore;

        public string ErrorStore
        {
            get { return _errorStore; }
            set
            {
                _errorStore = value;
                OnPropertyChanged();
                _timerErrorStore.Stop();
                _timerErrorStore.Start();
                _timerErrorStore.Elapsed += delegate { _errorStore = ""; OnPropertyChanged(); };
            } 
            
        }
        public string EmailAddress
        {
            set
            {
                _emailAddress = value;
                OnPropertyChanged();
            }

            get { return _emailAddress; }
        }

        private bool _isTextConfirm;
        public bool IsTextConfirm
        {
            get { return _isTextConfirm; }
            set
            {
                _isTextConfirm = value;
                OnPropertyChanged();
            }
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
                _timerErrorText.Stop();
                _timerErrorText.Start();
                _timerErrorText.Elapsed += delegate { _errorText = ""; OnPropertyChanged(); };
            }
            get { return _errorText; }
        }

        public List<string> StoreNames => _user.StoreNames;

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

        public ICommand SendMailCommand => _sendMailCommand ?? (_sendMailCommand = new RelayCommand(SendMail));


        public ICommand StoreChangedCommand => _storeChangedCommand ?? (_storeChangedCommand = new RelayCommand(StoreChanged));

        public ICommand EnterKeyPressedCommand => _enterPressedCommand ?? (_enterPressedCommand = new RelayCommand<KeyEventArgs>(EnterKeyPressed));

        private void StoreChanged()
        {
            if (SelectedIndexGeneratedShoppingList == -1) return;
            if (
                _user.ChangeItemToAnotherStore(StoreNames[SelectedStoreIndex],
                    GeneratedShoppingListData[SelectedIndexGeneratedShoppingList]) == 1)
            {
                OnPropertyChanged("TotalSum");
                OnPropertyChanged("MoneySaved");
                ErrorStore = "";
            }
            else
            {
                IsTextConfirm = false;
                ErrorStore = StoreNames[SelectedStoreIndex] + " har ikke produktet \"" +
                             GeneratedShoppingListData[SelectedIndexGeneratedShoppingList].ProductName +
                             "\" i deres sortiment.";
            }
            OnPropertyChanged("ErrorStore");
            OnPropertyChanged("GeneratedShoppingListData");
        }

        private void sendmailThread()
        {
            _mail.SendMail(EmailAddress, GeneratedShoppingListData, NotInAStore, TotalSum);
        }

        private void SendMail()
        {
            if (string.IsNullOrEmpty(EmailAddress))
            {
                IsTextConfirm = false;
                ErrorText = "Indtast venligst din E-mail.";
                return;
            }

            _testEmail = new EmailAddressAttribute();
            if (_testEmail.IsValid(EmailAddress) && EmailAddress != null)
            {
                IsTextConfirm = true;
                ErrorText = "E-mail afsendt.";

                var thread = new Thread(this.sendmailThread);
                thread.Start(); 
            }
            else
            {
                IsTextConfirm = false;
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