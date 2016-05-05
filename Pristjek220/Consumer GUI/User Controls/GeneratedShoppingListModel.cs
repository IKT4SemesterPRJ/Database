using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.Drawing.Text;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Windows.Documents;
using System.Windows.Input;
using Consumer;
using Pristjek220Data;

namespace Consumer_GUI.User_Controls
{
    public class GeneratedShoppingListModel : ObservableObject, IPageViewModel
    {
        private readonly IConsumer _user;
        public string TotalSum => _user.TotalSum;
        private readonly IMail _mail;
        private ICommand _sendMailCommand;
        public string EmailAddress { set; get; }
        public string BuyInOneStore => _user.BuyInOneStore;
        public string MoneySaved => _user.MoneySaved;

        private string _errorText;
        public string ErrorText
        {
            set
            {
                _errorText = value; 
                OnPropertyChanged();
            }
            get { return _errorText; }
        }
        private EmailAddressAttribute _testEmail; 

        public GeneratedShoppingListModel(IConsumer user, IMail mail)
        {
            _user = user;
            _mail = mail;
            ErrorText = "";
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
            get
            {
                return _sendMailCommand ?? (_sendMailCommand = new RelayCommand(SendMail));
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
    }
}