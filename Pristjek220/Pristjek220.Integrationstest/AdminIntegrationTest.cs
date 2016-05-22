using System.Security;
using Administration;
using NSubstitute;
using NUnit.Framework;
using Pristjek220Data;

namespace Pristjek220.Integrationstest
{
    [TestFixture]
    class AdminIntegrationTest
    {
        private IAdmin _admin;
        private IUnitOfWork _unit;
        private DataContext _context;

        [SetUp]
        public void SetUp()
        {
            _context = new DataContext();
            _unit = new UnitOfWork(_context);
            _admin = new Admin(_unit);
            _context.Database.Connection.ConnectionString = "Server=.\\SQLEXPRESS;Database=Pristjek220Data.DataContext; Trusted_Connection=True;";
            _context.Database.ExecuteSqlCommand("dbo.TestCleanTable");
        }

        [Test]
        public void CheckPassword_PasswordsAreAlike_Return1()
        {
            var passwd1 = new SecureString();
            var passwd2 = new SecureString();
            passwd1.AppendChar('H');
            passwd2.AppendChar('H');

            Assert.That(_admin.CheckPasswords(passwd1, passwd2), Is.EqualTo(1));
        }

        [Test]
        public void CheckPassword_PasswordsAreNotAlike_Return0()
        {
            var passwd1 = new SecureString();
            var passwd2 = new SecureString();
            passwd1.AppendChar('H');
            passwd2.AppendChar('E');

            Assert.That(_admin.CheckPasswords(passwd1, passwd2), Is.EqualTo(0));
        }

        [Test]
        public void FindStore_FindStoreTestStoreIsFound_ReturnStore()
        {
            var store = new Store() {StoreName = "Test"};
            _unit.Stores.Add(store);
            _unit.Complete();

            Assert.That(_admin.FindStore("Test"), Is.EqualTo(store));
        }

        [Test]
        public void FindStore_FindStoreTestStoreIsNotFound_ReturnNull()
        {
            Assert.That(_admin.FindStore("Test"), Is.EqualTo(null));
        }

        [Test]
        public void CreateLogin_LoginAddedToDb_StoreCanBeFoundInDb()
        {
            var passwd = new SecureString();
            passwd.AppendChar('H');

            _admin.CreateLogin("Username", passwd, "TestStore");
            
            Assert.That(_unit.Stores.FindStore("TestStore").StoreName, Is.EqualTo("Teststore"));
        }

        [Test]
        public void CreateLogin_LoginHasNoPasswd_ReturnsMinus2()
        {
            var passwd = new SecureString();

            Assert.That(_admin.CreateLogin("Username", passwd, "TestStore"), Is.EqualTo(-2));
        }

        [Test]
        public void CreateLogin_StoreAlreadyExists_ReturnsMinus1()
        {
            var passwd = new SecureString();
            passwd.AppendChar('H');

            _unit.Stores.Add(new Store() {StoreName = "Teststore"});
            _unit.Complete();

            Assert.That(_admin.CreateLogin("Username", passwd, "TestStore"), Is.EqualTo(-1));
        }

        [Test]
        public void CreateLogin_NameOfStoreIsFormatetStartLetterBigTheRestIsSmall_NameOfStoreIsFormatted()
        {
            var passwd = new SecureString();
            passwd.AppendChar('H');

            _admin.CreateLogin("Username", passwd, "TesTStoRE");

            Assert.That(_unit.Stores.FindStore("TesTStoRe").StoreName, Is.EqualTo("Teststore"));
        }

        [Test]
        public void CreateLogin_PasswdIsEncryptedInDb_PasswdAddIsNotEqualToPasswdInDb()
        {
            var passwd = new SecureString();
            passwd.AppendChar('H');

            _admin.CreateLogin("Username", passwd, "TesTStoRE");

            Assert.AreNotEqual(_unit.Logins.CheckUsername("Username").Password, "H");
        }

        [Test]
        public void CreateLogin_PasswdIsEmpty_ReturnMinus2()
        {
            var passwd = new SecureString();

            Assert.That(_admin.CreateLogin("Username", passwd, "TesTStoRE"), Is.EqualTo(-2));
        }

        [Test]
        public void CreateLogin_PasswdIsNull_ReturnMinus2()
        {
            SecureString passwd = null;

            Assert.That(_admin.CreateLogin("Username", passwd, "TesTStoRE"), Is.EqualTo(-2));
        }

        [Test]
        public void DeleteStore_DeleteTestStoreFromDb_StoreCanNoLongerBeFoundInDb()
        {
            _context.Stores.Add(new Store() {StoreName = "TestStore"});
            _unit.Complete();

            _admin.DeleteStore("TestStore");

            Assert.That(_unit.Stores.FindStore("TestStore"), Is.EqualTo(null));
        }

        [Test]
        public void DeleteStore_DeleteTestStoreFromDbTestStoreNotInDb_ReturnsMinus1()
        {
            Assert.That(_admin.DeleteStore("TestStore"), Is.EqualTo(-1));
        }

    }
}
