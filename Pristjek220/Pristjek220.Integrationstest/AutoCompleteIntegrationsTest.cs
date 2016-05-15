using NUnit.Framework;
using Pristjek220Data;
using SharedFunctionalities;

namespace Pristjek220.Integrationstest
{
    [TestFixture]
    class AutoCompleteIntegrationsTest
    {
        private IAutocomplete _autoCom;
        private IUnitOfWork _unit;
        private DataContext _context;

        [SetUp]
        public void SetUp()
        {
            _context = new DataContext();
            _unit = new UnitOfWork(_context);
            _autoCom = new Autocomplete(_unit);
            _context.Database.Connection.ConnectionString = "Server=.\\SQLEXPRESS;Database=Pristjek220Data.DataContext; Trusted_Connection=True;";
            _context.Database.ExecuteSqlCommand("dbo.TestCleanTable");
        }

        [Test]
        public void AutoCompleteProduct_3ProductsStartingWithBa_ReturnsAListContaining3Elements()
        {
            var bacon = new Product() {ProductName = "Bacon"};
            var banan = new Product() { ProductName = "Banan" };
            var bambus = new Product() { ProductName = "Bambus" };
            _context.Products.Add(bacon);
            _context.Products.Add(banan);
            _context.Products.Add(bambus);
            _context.SaveChanges();

            Assert.That(_autoCom.AutoCompleteProduct("Ba").Count, Is.EqualTo(3));
        }

        [Test]
        public void AutoCompleteProduct_1ProductsStartingWithBa_ReturnsAListContaining1Elements()
        {
            var bacon = new Product() { ProductName = "Bacon" };
            var banan = new Product() { ProductName = "Buster" };
            var bambus = new Product() { ProductName = "NotBa" };
            _context.Products.Add(bacon);
            _context.Products.Add(banan);
            _context.Products.Add(bambus);
            _context.SaveChanges();

            Assert.That(_autoCom.AutoCompleteProduct("Ba").Count, Is.EqualTo(1));
        }

        [Test]
        public void AutoCompleteProduct_0ProductsStartingWithBa_ReturnEmptyList()
        {
            var bacon = new Product() { ProductName = "Bussemand" };
            var banan = new Product() { ProductName = "Buster" };
            var bambus = new Product() { ProductName = "NotBa" };
            _context.Products.Add(bacon);
            _context.Products.Add(banan);
            _context.Products.Add(bambus);
            _context.SaveChanges();

            Assert.That(_autoCom.AutoCompleteProduct("?").Count, Is.EqualTo(0));
        }

        [Test]
        public void AutoCompleteProduct_5ProductsStartingWithBa_ReturnAListContaning3Elements()
        {
            var bacon = new Product() { ProductName = "Bacon" };
            var banan = new Product() { ProductName = "Banan" };
            var bambus = new Product() { ProductName = "Bambus" };
            var bass = new Product() { ProductName = "Bass" };
            var baller = new Product() { ProductName = "Baller" };
            _context.Products.Add(bacon);
            _context.Products.Add(banan);
            _context.Products.Add(bambus);
            _context.Products.Add(bass);
            _context.Products.Add(baller);
            _context.SaveChanges();

            Assert.That(_autoCom.AutoCompleteProduct("Ba").Count, Is.EqualTo(3));
        }

        [Test]
        public void AutoCompleteStore_3StoresStartingWithBa_ReturnsAListContaining3Elements()
        {
            var bacon = new Store() { StoreName = "Bacon" };
            var banan = new Store() { StoreName = "Banan" };
            var bambus = new Store() { StoreName = "Bambus" };
            _context.Stores.Add(bacon);
            _context.Stores.Add(banan);
            _context.Stores.Add(bambus);
            _context.SaveChanges();

            Assert.That(_autoCom.AutoCompleteStore("Ba").Count, Is.EqualTo(3));
        }

        [Test]
        public void AutoCompleteStore_1storesStartingWithBa_ReturnsAListContaining1Elements()
        {
            var bacon = new Store() { StoreName = "Bacon" };
            var banan = new Store() { StoreName = "Buster" };
            var bambus = new Store() { StoreName = "NotBa" };
            _context.Stores.Add(bacon);
            _context.Stores.Add(banan);
            _context.Stores.Add(bambus);
            _context.SaveChanges();

            Assert.That(_autoCom.AutoCompleteStore("Ba").Count, Is.EqualTo(1));
        }

        [Test]
        public void AutoCompleteStore_0StoresStartingWithBa_ReturnEmptyList()
        {
            var bacon = new Store() { StoreName = "Bussemand" };
            var banan = new Store() { StoreName = "Buster" };
            var bambus = new Store() { StoreName = "NotBa" };
            _context.Stores.Add(bacon);
            _context.Stores.Add(banan);
            _context.Stores.Add(bambus);
            _context.SaveChanges();

            Assert.That(_autoCom.AutoCompleteStore("?").Count, Is.EqualTo(0));
        }

        [Test]
        public void AutoCompleteStore_5StoresStartingWithBa_ReturnAListContaning3Elements()
        {
            var bacon = new Store() { StoreName = "Bacon" };
            var banan = new Store() { StoreName = "Banan" };
            var bambus = new Store() { StoreName = "Bambus" };
            var bass = new Store() { StoreName = "Bass" };
            var baller = new Store() { StoreName = "Baller" };
            _context.Stores.Add(bacon);
            _context.Stores.Add(banan);
            _context.Stores.Add(bambus);
            _context.Stores.Add(bass);
            _context.Stores.Add(baller);
            _context.SaveChanges();

            Assert.That(_autoCom.AutoCompleteStore("Ba").Count, Is.EqualTo(3));
        }

        [Test]
        public void AutoCompleteProductForOneStore_3ProductsStartingWithBa_ReturnsAListContaining3Elements()
        {
            var bacon = new Product() { ProductName = "Bacon" };
            var banan = new Product() { ProductName = "Banan" };
            var bambus = new Product() { ProductName = "Bambus" };
            var store = new Store() {StoreName = "TestStore"};
            _context.Stores.Add(store);
            _context.Products.Add(bacon);
            _context.Products.Add(banan);
            _context.Products.Add(bambus);
            _context.SaveChanges();
            var hasA1 = new HasA() {Product = bacon, ProductId = bacon.ProductId, Store = store, StoreId = store.StoreId, Price = 12};
            var hasA2 = new HasA() { Product = banan, ProductId = banan.ProductId, Store = store, StoreId = store.StoreId, Price = 13 };
            var hasA3 = new HasA() { Product = bambus, ProductId = bambus.ProductId, Store = store, StoreId = store.StoreId, Price = 14 };
            _context.HasARelation.Add(hasA1);
            _context.HasARelation.Add(hasA2);
            _context.HasARelation.Add(hasA3);
            _context.SaveChanges();

            Assert.That(_autoCom.AutoCompleteProductForOneStore("TestStore", "Ba").Count, Is.EqualTo(3));
        }

        [Test]
        public void AutoCompleteProductForOneStore_1ProductsStartingWithBa_ReturnsAListContaining1Elements()
        {
            var bacon = new Product() { ProductName = "Bacon" };
            var banan = new Product() { ProductName = "Bunan" };
            var bambus = new Product() { ProductName = "Bembus" };
            var store = new Store() { StoreName = "TestStore" };
            _context.Stores.Add(store);
            _context.Products.Add(bacon);
            _context.Products.Add(banan);
            _context.Products.Add(bambus);
            _context.SaveChanges();
            var hasA1 = new HasA() { Product = bacon, ProductId = bacon.ProductId, Store = store, StoreId = store.StoreId, Price = 12 };
            var hasA2 = new HasA() { Product = banan, ProductId = banan.ProductId, Store = store, StoreId = store.StoreId, Price = 13 };
            var hasA3 = new HasA() { Product = bambus, ProductId = bambus.ProductId, Store = store, StoreId = store.StoreId, Price = 14 };
            _context.HasARelation.Add(hasA1);
            _context.HasARelation.Add(hasA2);
            _context.HasARelation.Add(hasA3);
            _context.SaveChanges();

            Assert.That(_autoCom.AutoCompleteProductForOneStore("TestStore", "Ba").Count, Is.EqualTo(1));
        }

        [Test]
        public void AutoCompleteProductForOneStore_0ProductsStartingWithBa_ReturnEmptyList()
        {
            var bacon = new Product() { ProductName = "Bacon" };
            var banan = new Product() { ProductName = "Banan" };
            var bambus = new Product() { ProductName = "Bambus" };
            var store = new Store() { StoreName = "TestStore" };
            _context.Stores.Add(store);
            _context.Products.Add(bacon);
            _context.Products.Add(banan);
            _context.Products.Add(bambus);
            _context.SaveChanges();
            var hasA1 = new HasA() { Product = bacon, ProductId = bacon.ProductId, Store = store, StoreId = store.StoreId, Price = 12 };
            var hasA2 = new HasA() { Product = banan, ProductId = banan.ProductId, Store = store, StoreId = store.StoreId, Price = 13 };
            var hasA3 = new HasA() { Product = bambus, ProductId = bambus.ProductId, Store = store, StoreId = store.StoreId, Price = 14 };
            _context.HasARelation.Add(hasA1);
            _context.HasARelation.Add(hasA2);
            _context.HasARelation.Add(hasA3);
            _context.SaveChanges();

            Assert.That(_autoCom.AutoCompleteProductForOneStore("TestStore", "?").Count, Is.EqualTo(0));
        }

        [Test]
        public void AutoCompleteProductForOneStore_5ProductsStartingWithBa_ReturnAListContaning3Elements()
        {
            var bacon = new Product() { ProductName = "Bacon" };
            var banan = new Product() { ProductName = "Banan" };
            var bambus = new Product() { ProductName = "Bambus" };
            var basser = new Product() { ProductName = "Basser" };
            var bass = new Product() { ProductName = "Bass" };
            var store = new Store() { StoreName = "TestStore" };
            _context.Stores.Add(store);
            _context.Products.Add(bacon);
            _context.Products.Add(basser);
            _context.Products.Add(bass);
            _context.Products.Add(banan);
            _context.Products.Add(bambus);
            _context.SaveChanges();
            var hasA1 = new HasA() { Product = bacon, ProductId = bacon.ProductId, Store = store, StoreId = store.StoreId, Price = 12 };
            var hasA2 = new HasA() { Product = banan, ProductId = banan.ProductId, Store = store, StoreId = store.StoreId, Price = 13 };
            var hasA3 = new HasA() { Product = bambus, ProductId = bambus.ProductId, Store = store, StoreId = store.StoreId, Price = 14 };
            var hasA4 = new HasA() { Product = basser, ProductId = basser.ProductId, Store = store, StoreId = store.StoreId, Price = 15 };
            var hasA5 = new HasA() { Product = bass, ProductId = bass.ProductId, Store = store, StoreId = store.StoreId, Price = 16 };
            _context.HasARelation.Add(hasA1);
            _context.HasARelation.Add(hasA2);
            _context.HasARelation.Add(hasA3);
            _context.HasARelation.Add(hasA4);
            _context.HasARelation.Add(hasA5);
            _context.SaveChanges();

            Assert.That(_autoCom.AutoCompleteProductForOneStore("TestStore", "Ba").Count, Is.EqualTo(3));
        }
    }
}
