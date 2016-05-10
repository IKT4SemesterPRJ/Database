using System;
using System.Collections.Generic;
using System.Linq;
using System.Security;
using System.Text;
using System.Threading.Tasks;
using Administration;
using NSubstitute;
using NUnit.Framework;
using Pristjek220Data;

namespace Pristjek220.Unit.Test
{
    [TestFixture]
    class LogInUnitTest
    {
        private ILogIn _uut;
        private IUnitOfWork _unitOfWork;
        private Store _store;


        [SetUp]
        public void SetUp()
        {
            _unitOfWork = Substitute.For<IUnitOfWork>();
            _uut = new LogIn(_unitOfWork);
            _store = new Store() { StoreName = "Aldi", StoreId = 22 };
        }

        [Test]
        public void CheckUsernameAndPassword_CheckIfItRefStoreIsEqualToReqeustedStoreIfPasswordIsCorrect_RefStoreSameAsRequested()
        {
            Store storeGotten = _store;
            string username = "test";
            Login login = new Login();
            login.Id = 1;
            login.Password = "123";
            login.Store = _store;
            login.Username = username;

            SecureString secureString = new SecureString();
            secureString.AppendChar('1');
            secureString.AppendChar('2');
            secureString.AppendChar('3');
            _unitOfWork.Logins.CheckUsername(username).Returns(login);
            _unitOfWork.Logins.CheckLogin(secureString, login).Returns(_store);
            _uut.CheckUsernameAndPassword(username, secureString, ref storeGotten);
            Assert.That(storeGotten, Is.EqualTo(_store));
        }

        [Test]
        public void CheckUsernameAndPassword_CheckIfUsernameIsNotInDatabase_returnMinus1()
        {
            Store storeGotten = _store;
            string username = "test";
            Login login = null;

            SecureString secureString = new SecureString();
            secureString.AppendChar('1');
            secureString.AppendChar('2');
            secureString.AppendChar('3');
            _unitOfWork.Logins.CheckUsername(username).Returns(login);
            
            Assert.That(_uut.CheckUsernameAndPassword(username, secureString, ref storeGotten), Is.EqualTo(-1));
        }
    }
}
