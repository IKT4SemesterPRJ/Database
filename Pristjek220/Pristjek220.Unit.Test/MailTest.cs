using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using Consumer;
using Consumer_GUI.User_Controls;
using NSubstitute;
using NUnit.Framework;
using NUnit.Framework.Internal;
using Pristjek220Data;

namespace Pristjek220.Unit.Test
{
    [TestFixture()]
    class MailTest
    {
        private IMail _mail;
        private SmtpClient _smtpClient;
        [SetUp]
        public void SetUp()
        {
            _mail = new Mail(new SmtpClient("Smtp.gmail.com"));
            _smtpClient = Substitute.For<SmtpClient>();
        }

        [Test]
        public void SendMail_send2listWithAnSingleItemInEach_SeeThatSmtpReceives1Call()
        {
            ObservableCollection<StoreProductAndPrice> storeProductAndPricesList = new ObservableCollection<StoreProductAndPrice>();
            StoreProductAndPrice storeProductAndPriceItem = new StoreProductAndPrice();
            storeProductAndPriceItem.ProductName = "test";
            storeProductAndPriceItem.Price = 2;
            storeProductAndPriceItem.Quantity = "2";
            storeProductAndPriceItem.StoreName = "Aldi";
            storeProductAndPriceItem.Sum = 4;
            storeProductAndPricesList.Add(storeProductAndPriceItem);

            ObservableCollection<ProductInfo> productInfoList = new ObservableCollection<ProductInfo>();
            ProductInfo ProductInfoItem = new ProductInfo("test");
            productInfoList.Add(ProductInfoItem);


            _mail.SendMail("test@123dsa.dk",storeProductAndPricesList , productInfoList, "22");
            _smtpClient.Received(1);
        }
    }
}
