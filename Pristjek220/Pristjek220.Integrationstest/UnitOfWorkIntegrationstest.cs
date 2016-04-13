using System;
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
            _context.Database.Connection.ConnectionString = "Server=.\\SQLEXPRESS;Database=Pristjek220Data.DataContext; Trusted_Connection=True;";
            _context.Database.ExecuteSqlCommand("dbo.TestCleanTable");
            _unitOfWork = new UnitOfWork(_context);
        }

        [Test]
        public void Complete_SavesTheChangesToTheProductDatabase_ProductFoundInDatabase()
        {
            _unitOfWork.Products.Add(new Product() {ProductName = "Test"});
            _unitOfWork.Complete();

            Assert.That(_unitOfWork.Products.FindProduct("Test").ProductName, Is.EqualTo("Test"));
        }

        [Test]
        public void Complete_SavesTheChangesToTheStoreDatabase_StoreFoundInDatabase()
        {
            _unitOfWork.Stores.Add(new Store() { StoreName = "Test" });
            _unitOfWork.Complete();

            Assert.That(_unitOfWork.Stores.FindStore("Test").StoreName, Is.EqualTo("Test"));
        }

        [Test]
        public void Complete_SavesTheChangesToTheDatabase_ProductFoundInDatabase()
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
        public void Dispose_DisposeTheDataBase_ExceptionThrownWhenAnActionOnDatabaseIsMade()
        {
            _unitOfWork.Dispose();
            Assert.That(_unitOfWork.Products.ConnectToDb, Is.EqualTo(false));
        }
    }
}
