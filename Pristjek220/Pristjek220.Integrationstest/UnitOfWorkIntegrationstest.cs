using System;
using System.Security;
using NUnit.Framework;
using Pristjek220Data;

namespace Pristjek220.Integrationstest
{
    [TestFixture]
    class UnitOfWorkIntegrationstest
    {
        private DataContext _context;
        private IUnitOfWork _unitOfWork;

        [SetUp]
        public void SetUp()
        {
            _context = new DataContext();
            _unitOfWork = new UnitOfWork(_context);
            _context.Database.Connection.ConnectionString = "Server=.\\SQLEXPRESS;Database=Pristjek220Data.DataContext; Trusted_Connection=True;";
            _context.Database.ExecuteSqlCommand("dbo.TestCleanTable");
        }

        [Test]
        public void Complete_SavesTheChangesToTheProductsTable_ProductFoundInDatabase()
        {
            _unitOfWork.Products.Add(new Product() {ProductName = "Test"});
            _unitOfWork.Complete();

            Assert.That(_unitOfWork.Products.FindProduct("Test").ProductName, Is.EqualTo("Test"));
        }

        [Test]
        public void Complete_SavesTheChangesToTheStoresTable_StoreFoundInDatabase()
        {
            _unitOfWork.Stores.Add(new Store() { StoreName = "Test" });
            _unitOfWork.Complete();

            Assert.That(_unitOfWork.Stores.FindStore("Test").StoreName, Is.EqualTo("Test"));
        }

        [Test]
        public void Complete_SavesTheChangesToTheHasATable_HasAFoundInDatabase()
        {
            var banan = new Product() { ProductName = "Banan" };
            var føtex = new Store() {StoreName = "Føtex"};

            _context.Products.Add(banan);
            _context.Stores.Add(føtex);
            _context.SaveChanges();
            var hasA = new HasA()
            {
                Price = 3.95,
                Store = føtex,
                Product = banan,
                ProductId = banan.ProductId,
                StoreId = føtex.StoreId
            };
            _context.HasARelation.Add(hasA);
            _context.SaveChanges();

            Assert.That(_unitOfWork.HasA.FindHasA(føtex.StoreName, banan.ProductName), Is.EqualTo(hasA));
        }

        [Test]
        public void Complete_SavesTheChangesToTheLoginTable_LoginFoundInDatabase()
        {
            var login = new Login() {Username = "Test", Password = "Password", Store = new Store() {StoreName = "Test"} };
            _unitOfWork.Logins.Add(login);
            _unitOfWork.Complete();

            Assert.That(_unitOfWork.Logins.CheckUsername("Test"), Is.EqualTo(login));
        }

        [Test]
        public void Dispose_DisposeTheDataBase_ExceptionThrownWhenAnActionOnDatabaseIsMade()
        {
            _unitOfWork.Dispose();
            Assert.That(_unitOfWork.Products.ConnectToDb, Is.EqualTo(false));
        }
    }
}
