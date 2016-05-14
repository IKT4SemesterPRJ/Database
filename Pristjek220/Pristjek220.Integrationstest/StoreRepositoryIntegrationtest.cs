using NUnit.Framework;
using Pristjek220Data;

namespace Pristjek220.Integrationstest
{
    [TestFixture]
    class StoreRepositoryIntegrationtest
    {
        private DataContext _context;
        private StoreRepository _storeRepository;
        private Store _store;

        [SetUp]
        public void SetUp()
        {
            _context = new DataContext();
            _store = new Store() {StoreName = "TestStore"};
            _storeRepository = new StoreRepository(_context);
            _context.Database.Connection.ConnectionString = "Server=.\\SQLEXPRESS;Database=Pristjek220Data.DataContext; Trusted_Connection=True;";
            _context.Database.ExecuteSqlCommand("dbo.TestCleanTable");
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
        public void FindProductsInStore_FindProductsSoldByStoreTheStoreDontSellAnything_ListWithProductsReturnedContaining0Elements()
        {
            _context.Stores.Add(_store);
            _context.SaveChanges();

            Assert.That(_storeRepository.FindProductsInStore(_store.StoreName).Count, Is.EqualTo(0));
        }

        [Test]
        public void GetAllStores_3StoresAreInDatabase_ListWithStoresReturnedContaining3Elements()
        {
            _context.Stores.Add(_store);
            _context.Stores.Add(new Store() {StoreName = "Test1"});
            _context.Stores.Add(new Store() { StoreName = "Test2" });
            _context.SaveChanges();

            Assert.That(_storeRepository.GetAllStores().Count, Is.EqualTo(3));
        }

        [Test]
        public void GetAllStores_1StoreIsInDatabase_ListWithStoresReturnedContaining1Elements()
        {
            _context.Stores.Add(_store);
            _context.SaveChanges();

            Assert.That(_storeRepository.GetAllStores().Count, Is.EqualTo(1));
        }

        [Test]
        public void GetAllStores_0StoresIsInDatabase_ListWithStoresReturnedContaining0Elements()
        {
            Assert.That(_storeRepository.GetAllStores().Count, Is.EqualTo(0));
        }

        [Test]
        public void FindProductInStore_TestProductIsSoldInStore_TestIsReturned()
        {
            _context.Stores.Add(_store);
            var product = new Product() {ProductName = "TestProduct"};
            _context.Products.Add(product);
            _context.SaveChanges();
            var hasA = new HasA() {Product = product, ProductId = product.ProductId, Store = _store, StoreId = _store.StoreId, Price = 10};
            _context.HasARelation.Add(hasA);
            _context.SaveChanges();

            Assert.That(_storeRepository.FindProductInStore(_store.StoreName, product.ProductName).Name, Is.EqualTo(product.ProductName));
        }

        [Test]
        public void FindProductsInStore_TestProductsIsSoldInStore_ListContaining3ElementsAreReturned()
        {
            _context.Stores.Add(_store);
            var product1 = new Product() { ProductName = "TestProduct1" };
            var product2 = new Product() { ProductName = "TestProduct2" };
            var product3 = new Product() { ProductName = "TestProduct3" };
            _context.Products.Add(product1);
            _context.Products.Add(product2);
            _context.Products.Add(product3);
            _context.SaveChanges();
            var hasA1 = new HasA() { Product = product1, ProductId = product1.ProductId, Store = _store, StoreId = _store.StoreId, Price = 10 };
            var hasA2 = new HasA() { Product = product2, ProductId = product2.ProductId, Store = _store, StoreId = _store.StoreId, Price = 11 };
            var hasA3 = new HasA() { Product = product3, ProductId = product3.ProductId, Store = _store, StoreId = _store.StoreId, Price = 12 };
            _context.HasARelation.Add(hasA1);
            _context.HasARelation.Add(hasA2);
            _context.HasARelation.Add(hasA3);
            _context.SaveChanges();

            Assert.That(_storeRepository.FindProductsInStore(_store.StoreName).Count, Is.EqualTo(3));
        }

        [Test]
        public void FindProductsInStoreStartingWith_TestProductsIsSoldInStore_ListContaining1ElementsAreReturned()
        {
            _context.Stores.Add(_store);
            var product1 = new Product() { ProductName = "TestProduct1" };
            var product2 = new Product() { ProductName = "TastProduct2" };
            var product3 = new Product() { ProductName = "TqstProduct3" };
            _context.Products.Add(product1);
            _context.Products.Add(product2);
            _context.Products.Add(product3);
            _context.SaveChanges();
            var hasA1 = new HasA() { Product = product1, ProductId = product1.ProductId, Store = _store, StoreId = _store.StoreId, Price = 10 };
            var hasA2 = new HasA() { Product = product2, ProductId = product2.ProductId, Store = _store, StoreId = _store.StoreId, Price = 11 };
            var hasA3 = new HasA() { Product = product3, ProductId = product3.ProductId, Store = _store, StoreId = _store.StoreId, Price = 12 };
            _context.HasARelation.Add(hasA1);
            _context.HasARelation.Add(hasA2);
            _context.HasARelation.Add(hasA3);
            _context.SaveChanges();

            Assert.That(_storeRepository.FindProductsInStoreStartingWith(_store.StoreName, "Te").Count, Is.EqualTo(1));
        }

        [Test]
        public void FindProductsInStoreStartingWith_TestProductsIsSoldInStore_ListContaining3ElementsAreReturned()
        {
            _context.Stores.Add(_store);
            var product1 = new Product() { ProductName = "TestProduct1" };
            var product2 = new Product() { ProductName = "TestProduct2" };
            var product3 = new Product() { ProductName = "TestProduct3" };
            _context.Products.Add(product1);
            _context.Products.Add(product2);
            _context.Products.Add(product3);
            _context.SaveChanges();
            var hasA1 = new HasA() { Product = product1, ProductId = product1.ProductId, Store = _store, StoreId = _store.StoreId, Price = 10 };
            var hasA2 = new HasA() { Product = product2, ProductId = product2.ProductId, Store = _store, StoreId = _store.StoreId, Price = 11 };
            var hasA3 = new HasA() { Product = product3, ProductId = product3.ProductId, Store = _store, StoreId = _store.StoreId, Price = 12 };
            _context.HasARelation.Add(hasA1);
            _context.HasARelation.Add(hasA2);
            _context.HasARelation.Add(hasA3);
            _context.SaveChanges();

            Assert.That(_storeRepository.FindProductsInStoreStartingWith(_store.StoreName, "Test").Count, Is.EqualTo(3));
        }
    }
}
