using NUnit.Framework;
using Pristjek220Data;

namespace Pristjek220.Integrationstest
{
    [TestFixture]
    public class HasARepositoryItegrationstest
    {
        private DataContext _context;
        private HasARepository _hasARepository;
        private HasA _hasA;
        private Product _prod;
        private Store _store;

        [SetUp]
        public void SetUp()
        {
            _prod = new Product() {ProductName = "TestProduct"};
            _store = new Store() { StoreName = "TestStore" };
            _hasA = new HasA() {Price = 10};
            _context = new DataContext();
            _hasARepository = new HasARepository(_context);
            _context.Database.Connection.ConnectionString = "Server=.\\SQLEXPRESS;Database=Pristjek220Data.DataContext; Trusted_Connection=True;";
            _context.Database.ExecuteSqlCommand("dbo.TestCleanTable");
        }

        [TearDown]
        public void TearDown()
        {
            _context.Database.ExecuteSqlCommand("dbo.TestCleanTable");
        }

        [Test]
        public void AddHasA_AddAHasAToDb_HasAIsInDb()
        {
            _context.Stores.Add(_store);
            _context.Products.Add(_prod);
            _context.SaveChanges();
            _hasA.Product = _prod;
            _hasA.Store = _store;
            _hasA.ProductId = _prod.ProductId;
            _hasA.StoreId = _store.StoreId;
            _context.HasARelation.Add(_hasA);
            _context.SaveChanges();

            Assert.That(_hasARepository.Get(_store.StoreId, _prod.ProductId), Is.EqualTo(_hasA));
        }

        [Test]
        public void RemoveHasA_RemoveAHasAFromDb_HasAIsRemoved()
        {
            _hasA.Product = _prod;
            _hasA.Store = _store;
            _context.HasARelation.Add(_hasA);
            _context.SaveChanges();
            _hasARepository.Remove(_hasA);
            _context.SaveChanges();

            Assert.That(_hasARepository.Get(_prod.ProductId, _store.StoreId), Is.EqualTo(null));
        }

        [Test]
        public void RemoveHasA_HasAToRemoveIsNotInDb_ReturnsNull()
        {
            _hasARepository.Remove(_hasA);
            _context.SaveChanges();

            Assert.That(_hasARepository.Get(_prod.ProductId, _store.StoreId), Is.EqualTo(null));
        }

        [Test]
        public void Get_GetHasA_HasAReturned()
        {
            _context.Stores.Add(_store);
            _context.Products.Add(_prod);
            _context.SaveChanges();
            _hasA.Product = _prod;
            _hasA.Store = _store;
            _hasA.ProductId = _prod.ProductId;
            _hasA.StoreId = _store.StoreId;
            _context.HasARelation.Add(_hasA);
            _context.SaveChanges();

            Assert.That(_hasARepository.Get(_store.StoreId, _prod.ProductId), Is.EqualTo(_hasA));
        }

        [Test]
        public void Get_GetHasANoProductWithThatId_ReturnNull()
        {
            Assert.That(_hasARepository.Get(_prod.ProductId, _store.StoreId), Is.EqualTo(null));
        }

        [Test]
        public void FindHasA_FindHasAHasAIsFound_HasAFound()
        {
            _context.Stores.Add(_store);
            _context.Products.Add(_prod);
            _context.SaveChanges();
            _hasA.Product = _prod;
            _hasA.Store = _store;
            _hasA.ProductId = _prod.ProductId;
            _hasA.StoreId = _store.StoreId;
            _context.HasARelation.Add(_hasA);
            _context.SaveChanges();

            Assert.That(_hasARepository.FindHasA(_store.StoreName, _prod.ProductName), Is.EqualTo(_hasA));
        }

        [Test]
        public void FindHasA_FindHasAHasAIsNotFound_ReturnNull()
        {
            Assert.That(_hasARepository.FindHasA("Store that doesn't exist", "Product that doesn't exist"), Is.EqualTo(null));
        }

        [Test]
        public void FindCheapestHasA_HasAsWithPrices10And12And14IsSentIn_ReturnHasAWithProductThatHasPrice10()
        {
            _hasA.Store = _store;
            _hasA.Product = _prod;
            _context.HasARelation.Add(_hasA);
            _context.HasARelation.Add(new HasA() { Price = 12, Product = _prod, Store = new Store() { StoreName = "Store with product that costs 12" } });
            _context.HasARelation.Add(new HasA() { Price = 14, Product = _prod, Store = new Store() { StoreName = "Store with product that costs 14" } });

            _context.SaveChanges();

            Assert.That(_hasARepository.FindCheapestHasA(_prod), Is.EqualTo(_hasA));
        }

        [Test]
        public void FindCheapestHasA_NoHasAForProductExists_ReturnsNull()
        {
            Assert.That(_hasARepository.FindCheapestHasA(_prod), Is.EqualTo(null));
        }
    }
}
