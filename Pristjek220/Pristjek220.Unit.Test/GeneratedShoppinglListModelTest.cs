using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Consumer;
using Consumer_GUI.User_Controls;
using NSubstitute;
using NUnit.Framework;
using Pristjek220Data;

namespace Pristjek220.Unit.Test
{
    [TestFixture]
    class GeneratedShoppinglListModelTest
    {
        private IConsumer _user;
        private GeneratedShoppingListModel _uut;
        private IMail _mail;
        [SetUp]
        public void SetUp()
        {
            _user = Substitute.For<IConsumer>();
            _mail = Substitute.For<IMail>();
            _uut = new GeneratedShoppingListModel(_user, _mail);
        }

        [Test]
        public void SendEmail_WriteEmailWithWrongSyntax_ReturnStringTellingSo()
        {
            _uut.EmailAddress = "test";
            _uut.SendMailCommand.Execute(this);
            Assert.That(_uut.ErrorText, Is.EqualTo("E-mail skal overholde formatet: abc@mail.com"));
        }

        [Test]
        public void SendEmail_WriteEmailWithrightSyntax_ReturnStringTellingSo()
        {
            _uut.EmailAddress = "test@sesese.dk";
            _user.GeneratedShoppingListData = new ObservableCollection<StoreProductAndPrice>();
            _user.NotInAStore = new ObservableCollection<ProductInfo>() ;
            _uut.SendMailCommand.Execute(this);
            Assert.That(_uut.ErrorText, Is.EqualTo("E-mail afsendt"));
        }

        [Test]
        public void SendEmail_WriteEmailWithrightSyntax_callsMail()
        {
            _uut.EmailAddress = "test@sesese.dk";
            _user.TotalSum = "22";

            _user.GeneratedShoppingListData = new ObservableCollection<StoreProductAndPrice>();
            _user.NotInAStore = new ObservableCollection<ProductInfo>();
            _uut.SendMailCommand.Execute(this);
            Thread.Sleep(500); //Den kalder en baggrundstråd, så den skal lige være kaldt.
            _mail.Received(1).SendMail("test@sesese.dk", Arg.Any<ObservableCollection<StoreProductAndPrice>>(), Arg.Any<ObservableCollection<ProductInfo>>(), "22");
        }
    }
}
