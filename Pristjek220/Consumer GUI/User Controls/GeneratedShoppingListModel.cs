using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading;
using System.Windows.Input;
using Consumer;
using GalaSoft.MvvmLight.Command;
using Pristjek220Data;
using SharedFunctionalities;
using RelayCommand = SharedFunctionalities.RelayCommand;
using Timer = System.Timers.Timer;

namespace Consumer_GUI.User_Controls
{
    /// <summary>
    ///     GeneratedShoppingListModel is the User Control model for the GeneratedShoppingList User Control
    ///     Its used to see the generated lists and change in then, and then send them as an email
    /// </summary>
    public class GeneratedShoppingListModel : ObservableObject, IPageViewModel
    {
        private readonly IMail _mail;
        private readonly Timer _timerErrorStore = new Timer(2500);
        private readonly Timer _timerErrorText = new Timer(2500);
        private readonly IConsumer _user;
        private string _emailAddress = string.Empty;
        private ICommand _enterPressedCommand;
        private string _errorStore;

        private string _errorText = string.Empty;

        private bool _isTextConfirm;

        private ICommand _sendMailCommand;
        private ICommand _storeChangedCommand;
        private EmailAddressAttribute _testEmail;

        /// <summary>
        ///     GeneratedShoppingListModel constructor takes a user and a Mail
        /// </summary>
        /// <param name="user"></param>
        /// <param name="mail"></param>
        public GeneratedShoppingListModel(IConsumer user, IMail mail)
        {
            _user = user;
            _mail = mail;
        }

        /// <summary>
        ///     Total sum, shows the sum of all the products, and binds into the business logic layer
        /// </summary>
        public string TotalSum => _user.TotalSum;

        /// <summary>
        ///     Get and Set method for ErrorStore. The set method, sets the old ErrorStore to an oldtext, and then
        ///     change the value to the new vaule, call OnPropertyChanged and start a timer, that resets the label.
        /// </summary>
        public string ErrorStore
        {
            get { return _errorStore; }
            set
            {
                _errorStore = value;
                OnPropertyChanged();
                _timerErrorStore.Stop();
                _timerErrorStore.Start();
                _timerErrorStore.Elapsed += delegate
                {
                    _errorStore = "";
                    OnPropertyChanged();
                };
            }
        }

        /// <summary>
        ///     Get and Set method for EmailAddress. with OnPropertyChanged
        /// </summary>
        public string EmailAddress
        {
            set
            {
                _emailAddress = value;
                OnPropertyChanged();
            }

            get { return _emailAddress; }
        }

        /// <summary>
        ///     Is a bool that is used to set the color of a label to red if it's a fail and green if it's expected behaviour
        /// </summary>
        public bool IsTextConfirm
        {
            get { return _isTextConfirm; }
            set
            {
                _isTextConfirm = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        ///     Buy in one store, is a string that explains if a user can buy all the products in one store, and how much it is
        ///     gonnna cost. It is binded into the business logic layer
        /// </summary>
        public string BuyInOneStore => _user.BuyInOneStore;

        /// <summary>
        ///     Money saved, is a string that explains how much a user can save, by following the generated list compared to buying
        ///     all the products in one store. It is binded into the business logic layer
        /// </summary>
        public string MoneySaved => _user.MoneySaved;

        /// <summary>
        ///     Is the an int that shows what index in the generated Shopping list that is selected
        /// </summary>
        public int SelectedIndexGeneratedShoppingList { get; set; }

        /// <summary>
        ///     Is the an int that shows what store is selected from the dropdown
        /// </summary>
        public int SelectedStoreIndex { get; set; }

        /// <summary>
        ///     Get and Set method for ErrorText90. The set method, changes the value to the new vaule, call OnPropertyChanged and
        ///     start a timer, that resets the label.
        /// </summary>
        public string ErrorText
        {
            set
            {
                _errorText = value;
                OnPropertyChanged();
                _timerErrorText.Stop();
                _timerErrorText.Start();
                _timerErrorText.Elapsed += delegate
                {
                    _errorText = "";
                    OnPropertyChanged();
                };
            }
            get { return _errorText; }
        }

        /// <summary>
        ///     Store names, is a list of all the stores in the system. It is binded into the business logic layer
        /// </summary>
        public List<string> StoreNames => _user.StoreNames;

        /// <summary>
        ///     ObservableCollection that contains the items that is found in the database with the info: Store, name, price,
        ///     quantity
        ///     and sum. It is binded into the business logic layer and sorts the list after storeName, productName
        /// </summary>
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

        /// <summary>
        ///     ObservableCollection that contains the items that is in the shoopinglist that is not in the database. It is binded
        ///     into the business logic layer and sorts the list after productName
        /// </summary>
        public ObservableCollection<ProductInfo> NotInAStore
        {
            get
            {
                return
                    new ObservableCollection<ProductInfo>(
                        _user.NotInAStore.OrderBy(listData => listData.Name));
            }
        }

        /// <summary>
        ///     Command that is used to send a mail, if anything goes wrong it will print the reason to why it
        ///     did not send the mail to a label
        /// </summary>
        public ICommand SendMailCommand => _sendMailCommand ?? (_sendMailCommand = new RelayCommand(SendMail));

        /// <summary>
        ///     Command that is used to change a store for a given product, if anything goes wrong it will print the reason to why
        ///     it
        ///     did not change the store to a label
        /// </summary>
        public ICommand StoreChangedCommand
            => _storeChangedCommand ?? (_storeChangedCommand = new RelayCommand(StoreChanged));

        /// <summary>
        ///     Command that is used to see if Enter is pressed, if its pressed it calls the AddToStoreDatabase
        /// </summary>
        public ICommand EnterKeyPressedCommand
            => _enterPressedCommand ?? (_enterPressedCommand = new RelayCommand<KeyEventArgs>(EnterKeyPressed));

        private void StoreChanged()
        {
            if (SelectedIndexGeneratedShoppingList == -1) return;
            if (
                _user.ChangeProductToAnotherStore(StoreNames[SelectedStoreIndex],
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

        private void SendMailThread()
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

                var thread = new Thread(SendMailThread);
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