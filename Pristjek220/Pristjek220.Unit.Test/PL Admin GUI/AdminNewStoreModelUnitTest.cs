using System.Security;
using System.Threading;
using Administration;
using Administration_GUI.User_Controls_Admin;
using NSubstitute;
using NUnit.Framework;

namespace Pristjek220.Unit.Test
{
    [TestFixture]
    class AdminNewStoreModelUnitTest
    {
        private AdminNewStoreModel _uut;
        private IAdmin _admin;
        private SecureString _secureString1;
        private SecureString _secureString2;
        private SecureString _secureString3;


        [SetUp]
        public void SetUp()
        {
            _admin = Substitute.For<IAdmin>();
            _uut = new AdminNewStoreModel(_admin);
            _secureString1 = new SecureString();
            _secureString1.AppendChar('a');
            _secureString1.AppendChar('b');
            _secureString1.AppendChar('c');

            _secureString2 = new SecureString();
            _secureString2.AppendChar('c');
            _secureString2.AppendChar('b');
            _secureString2.AppendChar('a');

            _secureString3 = new SecureString();

        }

        [Test]
        public void NewStore_NewStoreNameIsEmpty_IsTextConfirmIsFalse()
        {
            _uut.IsTextConfirm = true;
            _uut.NewStoreName = "";
            _uut.NewStoreCommand.Execute(this);
            Assert.That(_uut.IsTextConfirm, Is.EqualTo(false));
        }

        [Test]
        public void NewStore_NewStoreNameIsNull_IsTextConfirmIsFalse()
        {
            _uut.IsTextConfirm = true;
            _uut.NewStoreName = null;
            _uut.NewStoreCommand.Execute(this);
            Assert.That(_uut.IsTextConfirm, Is.EqualTo(false));
        }

        [Test]
        public void NewStore_NewStoreNameIsEmpty_ErrorIsError()
        {
            _uut.NewStoreName = "";
            _uut.NewStoreCommand.Execute(this);
            Assert.That(_uut.Error, Is.EqualTo("Udfyld venligst alle felter."));
        }

        [Test]
        public void NewStore_NewStoreNameIsNull_ErrorIsError()
        {
            _uut.NewStoreName = null;
            _uut.NewStoreCommand.Execute(this);
            Assert.That(_uut.Error, Is.EqualTo("Udfyld venligst alle felter."));
        }

        [Test]
        public void NewStore_PasswordsDoesNotMatch_ErrorIsErrorMessage()
        {
            _uut.NewStoreName = "Lidl";
            _admin.CheckPasswords(_secureString1, _secureString2).Returns(0);
            _uut.SecurePassword = _secureString1;
            _uut.SecurePasswordConfirm = _secureString2;
            _uut.NewStoreCommand.Execute(this);
            Assert.That(_uut.Error, Is.EqualTo($"Kodeordene matcher ikke."));
        }

        [Test]
        public void NewStore_PasswordsDoesMatch_ErrorIsSucessMessage()
        {
            _uut.NewStoreName = "Lidl";
            _admin.CheckPasswords(_secureString1, _secureString1).Returns(1);
            _admin.CreateLogin(_uut.NewStoreName, _secureString1, _uut.NewStoreName).Returns(0);
            _uut.SecurePassword = _secureString1;
            _uut.SecurePasswordConfirm = _secureString1;
            _uut.NewStoreCommand.Execute(this);
            Assert.That(_uut.Error, Is.EqualTo($"Forretning oprettet med forretningsnavnet \"{_uut.NewStoreName}\"."));
        }

        [Test]
        public void NewStore112_PasswordsDoesMatchButStoreExist_ErrorIsStoreExist()
        {
            _uut.NewStoreName = "Lidl";
            _admin.CheckPasswords(_secureString1, _secureString1).Returns(1);
            _admin.CreateLogin(_uut.NewStoreName, _secureString1, _uut.NewStoreName).Returns(-1);
            _uut.SecurePassword = _secureString1;
            _uut.SecurePasswordConfirm = _secureString1;
            _uut.NewStoreCommand.Execute(this);
            Assert.That(_uut.Error, Is.EqualTo($"Forretningen findes allerede."));
        }

        [Test]
        public void NewStore12_PasswordsIsEmpty_ErrorIsErrorMessage()
        {
            _uut.NewStoreName = "Lidl";
            _admin.CheckPasswords(_secureString3, _secureString3).Returns(1);
            _admin.CreateLogin(_uut.NewStoreName, _secureString3, _uut.NewStoreName).Returns(-2);
            _uut.SecurePassword = _secureString3;
            _uut.SecurePasswordConfirm = _secureString3;
            _uut.NewStoreCommand.Execute(this);
            Assert.That(_uut.Error, Is.EqualTo("Udfyld venligst alle felter."));
        }



        [Test]
        public void IllegalSignNewStore_TestLegalString_TextConfirmIsStillTrue()
        {
            _uut.IsTextConfirm = true;
            _uut.NewStoreName = "test";
            _uut.IllegalSignNewStoreCommand.Execute(this);

            Assert.That(_uut.IsTextConfirm, Is.EqualTo(true));
        }

        [Test]
        public void IllegalSignNewStore_TestIlegalString_TextConfirmIsFalse()
        {
            _uut.IsTextConfirm = true;
            _uut.NewStoreName = "test!";
            _uut.IllegalSignNewStoreCommand.Execute(this);

            Assert.That(_uut.IsTextConfirm, Is.EqualTo(false));
        }

        [Test]
        public void IllegalSignNewStore_ErrorIsSet_IsErrorString()
        {
            _uut.NewStoreName = "test!";
            _uut.IllegalSignNewStoreCommand.Execute(this);

            Assert.That(_uut.Error, Is.EqualTo("Der kan kun skrives bogstaverne fra a til å og tallene fra 0 til 9."));
        }

        [Test]
        public void IllegalSignNewStore_ErrorIsSetAndWait5Sec_StringIsEmpty()
        {
            _uut.NewStoreName = "test!";
            _uut.IllegalSignNewStoreCommand.Execute(this);
            Thread.Sleep(5000);

            Assert.That(_uut.Error, Is.EqualTo(""));
        }

        [Test]
        public void IllegalSignNewStore_itemisEmpty_StringIsNull()
        {
            _uut.NewStoreName = null;
            _uut.IllegalSignNewStoreCommand.Execute(this);

            Assert.That(_uut.Error, Is.EqualTo(""));
        }
    }
}
