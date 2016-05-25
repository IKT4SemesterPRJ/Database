using System.Security;
using Administration;
using Administration_GUI.User_Controls_Admin;
using NUnit.Framework;
using Pristjek220Data;

namespace Pristjek220.Integrationstest
{
    class AdminNewStoreModelIntergrationsTest
    {
        private DataContext _context;
        private IUnitOfWork _unitOfWork;
        private AdminNewStoreModel _adminNewStoreModel;
        private Admin _admin;
        private readonly SecureString _secureStringTest = new SecureString();

        [SetUp]
        public void SetUp()
        {
            _context = new DataContext();
            _unitOfWork = new UnitOfWork(_context);
            _admin = new Admin(_unitOfWork);
            _adminNewStoreModel = new AdminNewStoreModel(_admin);
            _context.Database.Connection.ConnectionString = "Server=.\\SQLEXPRESS;Database=Pristjek220Data.DataContext; Trusted_Connection=True;";
            _context.Database.ExecuteSqlCommand("dbo.TestCleanTable");

            _secureStringTest.AppendChar('A');
            _secureStringTest.AppendChar('B');
            _secureStringTest.AppendChar('C');
        }

        [TearDown]
        public void TearDown()
        {
            _context.Database.ExecuteSqlCommand("dbo.TestCleanTable");
        }

        [Test]
        public void NewStore_AddTestStoreToDb_TestStoreIsReturned()
        {
            _adminNewStoreModel.NewStoreName = "TestStore";
            _adminNewStoreModel.SecurePassword = _secureStringTest;
            _adminNewStoreModel.SecurePasswordConfirm = _secureStringTest;
            _adminNewStoreModel.NewStoreCommand.Execute(this);
            Assert.That(_unitOfWork.Stores.FindStore("TestStore"), Is.Not.EqualTo(null));
        }

        [Test]
        public void NewStore_AddTestStoreToDbWithPasswordsNotEqual_NullIsReturned()
        {
            _adminNewStoreModel.NewStoreName = "TestStore";
            _adminNewStoreModel.SecurePassword = _secureStringTest;
            _adminNewStoreModel.SecurePasswordConfirm = new SecureString();
            _adminNewStoreModel.NewStoreCommand.Execute(this);
            Assert.That(_unitOfWork.Stores.FindStore("TestStore"), Is.EqualTo(null));
        }

        [Test]
        public void NewStore_AddEmptyStringToDb_NullIsReturned()
        {
            _adminNewStoreModel.NewStoreName = "";
            _adminNewStoreModel.SecurePassword = _secureStringTest;
            _adminNewStoreModel.SecurePasswordConfirm = new SecureString();
            _adminNewStoreModel.NewStoreCommand.Execute(this);
            Assert.That(_unitOfWork.Stores.FindStore(""), Is.EqualTo(null));
        }
    }
}
