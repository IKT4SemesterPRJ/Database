using System.Security;
using Administration;
using NSubstitute;
using NUnit.Framework;
using Pristjek220Data;

namespace Pristjek220.Unit.Test
{
    [TestFixture]
    class AdminUnitTest
    {
        private IAdmin _uut;
        private IUnitOfWork _unitOfWork;

        [SetUp]
        public void SetUp()
        {
            _unitOfWork = Substitute.For<IUnitOfWork>();
            _uut = new Admin(_unitOfWork);
        }

        [Test]
        public void CreateLogin_CreateTheStoreToTheLogin_StoreIsCreatedAndSavedInDatabase()
        {
            var password = new SecureString();
            password.AppendChar('P');

            _uut.CreateLogin("Test", password, "Test");

            _unitOfWork.Stores.Received(1).Add(Arg.Any<Store>());
        }

        [Test]
        public void CreateLogin_CreateTheLogin_LoginIsCreatedAndSavedInDatabase()
        {
            var password = new SecureString();
            password.AppendChar('P');

            _uut.CreateLogin("Test", password, "Test");

            _unitOfWork.Logins.Received(1).Add(Arg.Any<Login>());
        }

        [Test]
        public void CreateLogin_CreateTheLoginButNoPasswordWasEntered_ReturnsMinus2()
        {
            Assert.That(_uut.CreateLogin("Test", new SecureString(), "Test"), Is.EqualTo(-2));
        }

        [Test]
        public void CheckPasswords_PasswordsIsMatching_Returns1()
        {
            SecureString secureString = new SecureString();
            secureString.AppendChar('a');
            secureString.AppendChar('a');
            secureString.AppendChar('a');
            
            Assert.That(_uut.CheckPasswords(secureString, secureString), Is.EqualTo(1));
        }

        [Test]
        public void CheckPasswords_PasswordIsNotMatching_Returns0()
        {
            SecureString secureString = new SecureString();
            secureString.AppendChar('a');
            secureString.AppendChar('a');
            secureString.AppendChar('a');
            SecureString secureString2 = new SecureString();
            secureString2.AppendChar('b');
            secureString2.AppendChar('b');
            secureString2.AppendChar('b');

            Assert.That(_uut.CheckPasswords(secureString, secureString2), Is.EqualTo(0));
        }

        [Test]
        public void CheckPasswords_PasswordIsNotMatchingWithOnePasswordBeingNull_Returns0()
        {
            SecureString secureString = new SecureString();
            secureString.AppendChar('a');
            secureString.AppendChar('a');
            secureString.AppendChar('a');
            SecureString secureString2 = null;


            Assert.That(_uut.CheckPasswords(secureString, secureString2), Is.EqualTo(0));
        }

        [Test]
        public void ssCheckPasswords_PasswordIsNotMatchingWithOnePasswordBeingNull_Returns0()
        {
            SecureString secureString = new SecureString();
            secureString.AppendChar('a');
            secureString.AppendChar('a');
            secureString.AppendChar('a');
            Store store = new Store();
            store.StoreName = "Test";
            _unitOfWork.Stores.FindStore("Test").Returns(store);
            Assert.That(_uut.CreateLogin("Test", secureString, "Test"), Is.EqualTo(-1));
        }
    }
}
