using Consumer;
using NUnit.Framework;
using Pristjek220Data;

namespace Pristjek220.Integrationstest
{
    [TestFixture]
    class ConsumerIntegrationTest
    {
        private IConsumer _consumer;
        private IUnitOfWork _unit;
        private DataContext _context;
        private HasA _hasA;
        private HasA _hasA1;
        private Store _store;
        private Store _store1;
        private Product _product;

        [SetUp]
        public void SetUp()
        {
            _context = new DataContext();
            _unit = new UnitOfWork(_context);
            _context.Database.Connection.ConnectionString = "Server=.\\SQLEXPRESS;Database=Pristjek220Data.DataContext; Trusted_Connection=True;MultipleActiveResultSets=True;";
            _context.Database.ExecuteSqlCommand("dbo.TestCleanTable");
            _consumer = new Consumer.Consumer(_unit);


            //Opsætning af 2 forretninger der begge har samme vare
            _product = new Product() { ProductName = "TestProduct" };
            _store = new Store() { StoreName = "TestStore" };
            _store1 = new Store() { StoreName = "TestStore1" };
            _context.Products.Add(_product);
            _context.Stores.Add(_store);
            _context.Stores.Add(_store1);
            _context.SaveChanges();
            _hasA = new HasA() { Product = _product, Store = _store, ProductId = _product.ProductId, StoreId = _store.StoreId, Price = 12 };
            _hasA1 = new HasA() { Product = _product, Store = _store1, ProductId = _product.ProductId, StoreId = _store1.StoreId, Price = 13 };
            _context.HasARelation.Add(_hasA);
            _context.HasARelation.Add(_hasA1);
            _context.SaveChanges();
        }

        [Test]
        public void ChangeProductToAnotherStore_ProductDoesNotGetSoldInOtherStore_ReturnMinus1()
        {
            var storProdAndPrice = new StoreProductAndPrice() { Price = _hasA.Price, StoreName = _hasA.Store.StoreName, ProductName = _product.ProductName, Quantity = "1", Sum = _hasA.Price };

            Assert.That(_consumer.ChangeProductToAnotherStore("NoStore", storProdAndPrice), Is.EqualTo(-1));
        }

        [Test]
        public void ChangeProductToAnotherStore_ProductDoesGetSoldInStore_Return1()
        {
            var storProdAndPrice = new StoreProductAndPrice() { Price = _hasA.Price, StoreName = _hasA.Store.StoreName, ProductName = _product.ProductName, Quantity = "1", Sum = _hasA.Price };
            _consumer.GeneratedShoppingListData.Add(storProdAndPrice);

            Assert.That(_consumer.ChangeProductToAnotherStore(_store1.StoreName, storProdAndPrice), Is.EqualTo(1));
        }
    }
}
