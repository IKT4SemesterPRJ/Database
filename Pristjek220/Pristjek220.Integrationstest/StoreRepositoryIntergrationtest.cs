using NUnit.Framework;
using Pristjek220Data;

namespace Pristjek220.Integrationstest
{
    [TestFixture]
    class StoreRepositoryIntergrationtest
    {
        private DataContext _context;
        private StoreRepository _storeRepository;
        private Store _store;

        [SetUp]
        public void SetUp()
        {
            _context = new DataContext();
            _store = new Store() {StoreName = "Test"};
            _context.Database.Connection.ConnectionString = "Server=.\\SQLEXPRESS;Database=Pristjek220Data.DataContext; Trusted_Connection=True;";
            _context.Database.ExecuteSqlCommand("dbo.TestCleanTable");
            _storeRepository = new StoreRepository(_context);
        }

        [TearDown]
        public void TearDown()
        {
            _context.Database.ExecuteSqlCommand("dbo.TestCleanTable");
        }

        [Test]
        public void Get_GetStore_StoreReturned()
        {
            _context.Stores.Add(_store);
            _context.SaveChanges();

            Assert.That(_storeRepository.Get(_store.StoreId), Is.EqualTo(_store));
        }

        [Test]
        public void Get_GetProductNoProductWithThatId_ReturnNull()
        {
            Assert.That(_storeRepository.Get(_store.StoreId), Is.EqualTo(null));
        }

        [Test]
        public void Find_FindStoreStoreIsFound_StoreFound()
        {
            _context.Stores.Add(_store);
            _context.SaveChanges();

            Assert.That(_storeRepository.FindStore(_store.StoreName), Is.EqualTo(_store));
        }

        [Test]
        public void Find_FindProductProductIsNotFound_ReturnNull()
        {
            Assert.That(_storeRepository.FindStore("24"), Is.EqualTo(null));
        }

        [Test]
        public void FindStoreStartingWith_MakeListForF_ListReturnedContaining4Elements()
        {
            _context.Stores.Add(_store);
            _context.Stores.Add(new Store() { StoreName = "Føtex" });
            _context.Stores.Add(new Store() { StoreName = "Fakta" });
            _context.Stores.Add(new Store() { StoreName = "Fleggaard" });
            _context.Stores.Add(new Store() { StoreName = "Fantasy" });
            _context.SaveChanges();

            Assert.That(_storeRepository.FindStoreStartingWith("F").Count, Is.EqualTo(4));
        }

        [Test]
        public void FindStoreStartingWith_MakeListForKWithNoStoresStartingWithK_ListReturnedContaining0Elements()
        {
            Assert.That(_storeRepository.FindStoreStartingWith("K").Count, Is.EqualTo(0));
        }

        [Test]
        public void FindProductsInStore_FindProductsSoldByStore_ListWithProductsReturnedContaining3Elements()
        {
            var banan = new Product() {ProductName = "Banan"};
            var tomat = new Product() {ProductName = "Tomat"};
            var bacon = new Product() {ProductName = "Bacon"};

            _context.Products.Add(bacon);
            _context.Products.Add(tomat);
            _context.Products.Add(banan);
            _context.Stores.Add(_store);
            _context.SaveChanges();

            _context.HasARelation.Add(new HasA() { Price = 3.95, Store = _store, Product = tomat, ProductId = tomat.ProductId, StoreId = _store.StoreId });
            _context.HasARelation.Add(new HasA() { Price = 2.95, Store = _store, Product = banan, ProductId = banan.ProductId, StoreId = _store.StoreId });
            _context.HasARelation.Add(new HasA() { Price = 1.95, Store = _store, Product = bacon, ProductId = bacon.ProductId, StoreId = _store.StoreId });
            _context.SaveChanges();

            Assert.That(_storeRepository.FindProductsInStore(_store.StoreName).Count, Is.EqualTo(3));
        }

        [Test]
        public void FindProductsInStore_FindProductsSoldByStoreTheStoreDontSellAnything_ListWithProductsReturnedConjtaining0Elements()
        {
            _context.Stores.Add(_store);
            _context.SaveChanges();

            Assert.That(_storeRepository.FindProductsInStore(_store.StoreName).Count, Is.EqualTo(0));
        }
    }
}
