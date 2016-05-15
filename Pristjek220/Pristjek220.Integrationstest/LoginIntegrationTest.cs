using System.Security;
using Administration;
using NUnit.Framework;
using Pristjek220Data;

namespace Pristjek220.Integrationstest
{
    [TestFixture]
    class LoginIntegrationTest
    {
        private ILogIn _login;
        private IUnitOfWork _unit;
        private DataContext _context;

        [SetUp]
        public void SetUp()
        {
            _context = new DataContext();
            _unit = new UnitOfWork(_context);
            _login = new LogIn(_unit);
            _context.Database.Connection.ConnectionString = "Server=.\\SQLEXPRESS;Database=Pristjek220Data.DataContext; Trusted_Connection=True;";
            _context.Database.ExecuteSqlCommand("dbo.TestCleanTable");
        }

        [Test]
        public void CheckUsernameAndPassword_UsernamesDoesNotMatch_ReturnMinus1()
        {
            Store store = new Store();
            Assert.That(_login.CheckUsernameAndPassword("", new SecureString(), ref store), Is.EqualTo(-1));
        }

        [Test]
        public void CheckUsernameAndPassword_PasswordDoesNotMatch_Return0()
        {
            Store store = new Store();
            var login = new Login() {Username = "TestUser", Password = "0d6be69b264717f2dd33652e212b173104b4a647b7c11ae72e9885f11cd312fb", Store = new Store() {StoreName = "TestStore"} };
            _context.Logins.Add(login);
            _context.SaveChanges();
            var pass = new SecureString();
            pass.AppendChar('P');
            pass.AppendChar('a');
            pass.AppendChar('s');
            pass.AppendChar('s');
            pass.AppendChar('w');
            pass.AppendChar('d');

            Assert.That(_login.CheckUsernameAndPassword("TestUser", pass, ref store), Is.EqualTo(0));
        }

        [Test]
        public void CheckUsernameAndPassword_UsernameAndPasswdBothMatch_Return1()
        {
            Store store = new Store();
            var login = new Login() { Username = "TestUser", Password = "0d6be69b264717f2dd33652e212b173104b4a647b7c11ae72e9885f11cd312fb", Store = new Store() { StoreName = "TestStore" } };
            _context.Logins.Add(login);
            _context.SaveChanges();
            var pass = new SecureString();
            pass.AppendChar('p');
            pass.AppendChar('a');
            pass.AppendChar('s');
            pass.AppendChar('s');
            pass.AppendChar('w');
            pass.AppendChar('d');

            Assert.That(_login.CheckUsernameAndPassword("TestUser", pass, ref store), Is.EqualTo(1));
        }

        [Test]
        public void CheckUsernameAndPassword_UsernameAndPasswdBothMatch_StoreNameIsEqualToTestStore()
        {
            Store store = new Store();
            var login = new Login() { Username = "TestUser", Password = "0d6be69b264717f2dd33652e212b173104b4a647b7c11ae72e9885f11cd312fb", Store = new Store() { StoreName = "TestStore" } };
            _context.Logins.Add(login);
            _context.SaveChanges();
            var pass = new SecureString();
            pass.AppendChar('p');
            pass.AppendChar('a');
            pass.AppendChar('s');
            pass.AppendChar('s');
            pass.AppendChar('w');
            pass.AppendChar('d');

            _login.CheckUsernameAndPassword("TestUser", pass, ref store);

            Assert.That(store.StoreName, Is.EqualTo("TestStore"));
        }
    }
}
