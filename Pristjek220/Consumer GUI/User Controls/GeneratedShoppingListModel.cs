using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Drawing.Text;
using System.Linq;
using System.Net.Mail;
using System.Windows.Documents;
using System.Windows.Input;
using Consumer;
using Pristjek220Data;

namespace Consumer_GUI.User_Controls
{
    internal class GeneratedShoppingListModel : ObservableObject, IPageViewModel
    {
        private UnitOfWork _unit = new UnitOfWork(new DataContext());
        private readonly IConsumer _user;
        public string TotalSum => _user.TotalSum;
        private readonly Mail _mail;
        private ICommand _sendMailCommand;

        public GeneratedShoppingListModel(Consumer.Consumer user)
        {
            _user = user;
            _mail = new Mail();
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
            List<StoreProductAndPrice> test = new List<StoreProductAndPrice>();
            StoreProductAndPrice test2 = new StoreProductAndPrice();
            test2.StoreName = "asd";
            test2.Price = 12;
            test2.ProductName = "Tis";
            test2.Quantity = "3";
            test2.Sum = 23;
            test.Add(test2);
            test.Add(test2);

            _mail.SendMail("AndersMeidahl@gmail.com", test, 252);
        }
    }
}