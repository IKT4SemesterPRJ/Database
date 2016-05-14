using System.Security;
using NUnit.Framework;
using Pristjek220Data;

namespace Pristjek220.Integrationstest
{
    [TestFixture]
    class LoginRepositoryIntegrationsTest
    {
        private DataContext _context;
        private LoginRepository _loginRepository;
        private Login _login;
        private Store _store;
        private SecureString _passwd;

        [SetUp]
        public void SetUp()
        {
            _context = new DataContext();
            _store = new Store() {StoreName = "TestStore"};
            _loginRepository = new LoginRepository(_context);
            _context.Database.Connection.ConnectionString = "Server=.\\SQLEXPRESS;Database=Pristjek220Data.DataContext; Trusted_Connection=True;";
            _context.Database.ExecuteSqlCommand("dbo.TestCleanTable");
            _context.Stores.Add(_store);
            _login = new Login() { Username = "TestUser", Password = "0d6be69b264717f2dd33652e212b173104b4a647b7c11ae72e9885f11cd312fb", Store = _store };
            _context.Logins.Add(_login);
            _context.SaveChanges();
            
            _passwd = new SecureString();
            _passwd.AppendChar('p');
            _passwd.AppendChar('a');
            _passwd.AppendChar('s');
            _passwd.AppendChar('s');
            _passwd.AppendChar('w');
            _passwd.AppendChar('d');
        }

        [Test]
        public void CheckUserName_TestUserIsTheCorrectUsername_TheLoginIsforTestUserIsReturned()
        {
            Assert.That(_loginRepository.CheckUsername("TestUser"), Is.EqualTo(_login));
        }

        [Test]
        public void CheckUserName_TestUserIsNotTheCorrectUsername_NullIsReturned()
        {
            Assert.That(_loginRepository.CheckUsername("NotCorrect"), Is.EqualTo(null));
        }

        [Test]
        public void CheckLogin_CorrectUsernameAndPasswordEntered_TestStoreIsReturned()
        {
            Assert.That(_loginRepository.CheckLogin(_passwd, _login), Is.EqualTo(_store));
        }

        [Test]
        public void CheckLogin_WrongUsernameAndPasswordEntered_TestStoreIsReturned()
        {
            var wrongLogin = new Login();
            Assert.That(_loginRepository.CheckLogin(_passwd, wrongLogin), Is.EqualTo(null));
        }

        [Test]
        public void CheckLogin_NoPasswdEntered_NullIsReturned()
        {
            var wrongLogin = new Login();
            SecureString emptyPasswd = null;
            Assert.That(_loginRepository.CheckLogin(emptyPasswd, wrongLogin), Is.EqualTo(null));
        }
    }
}
