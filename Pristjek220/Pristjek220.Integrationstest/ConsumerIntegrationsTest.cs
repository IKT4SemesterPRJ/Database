using System.Collections.Generic;
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

        [Test]
        public void ChangeProductToAnotherStore_ProductChangeToGetSoldInTestStore1_MoneySavedChangedToMinus1()
        {
            var storProdAndPrice = new StoreProductAndPrice() { Price = _hasA.Price, StoreName = _hasA.Store.StoreName, ProductName = _product.ProductName, Quantity = "1", Sum = _hasA.Price };
            _consumer.GeneratedShoppingListData.Add(storProdAndPrice);
            _consumer.ChangeProductToAnotherStore(_store1.StoreName, storProdAndPrice);

            Assert.That(_consumer.MoneySaved, Is.EqualTo("-1 kr"));
        }

        [Test]
        public void ChangeProductToAnotherStore_ProductChangeToGetSoldInTestStore1_TotalSumChangedTo13()
        {
            var storProdAndPrice = new StoreProductAndPrice() { Price = _hasA.Price, StoreName = _hasA.Store.StoreName, ProductName = _product.ProductName, Quantity = "1", Sum = _hasA.Price };
            _consumer.GeneratedShoppingListData.Add(storProdAndPrice);
            _consumer.ChangeProductToAnotherStore(_store1.StoreName, storProdAndPrice);

            Assert.That(_consumer.TotalSum, Is.EqualTo("13 kr"));
        }

        [Test]
        public void DoesProductExist_ProductIsInDb_ReturnTrue()
        {
            Assert.That(_consumer.DoesProductExist("TestProduct"));
        }

        [Test]
        public void DoesProductExist_ProductIsNotInDb_ReturnFalse()
        {
            Assert.That(_consumer.DoesProductExist("NotAProduct"), Is.EqualTo(false));
        }

        [Test]
        public void FindCheapestStore_CheapestStoreForTestProductIsTestStore_ReturnTestStore()
        {
            _consumer.OptionsStores.Add(new StoresInPristjek("TestStore"));
            Assert.That(_consumer.FindCheapestStore(_product.ProductName), Is.EqualTo(_store));
        }

        [Test]
        public void FindCheapestStore_ProductIsNotInDb_ReturnNull()
        {
            Assert.That(_consumer.FindCheapestStore("NotAProduct"), Is.EqualTo(null));
        }

        [Test]
        public void FindCheapestStoreWithSumForListOfProducts_2StoresSellsTestProduct_ReturnsTheCheapestStoreWithPriceOf12()
        {
            var list = new List<ProductInfo>() {new ProductInfo(_product.ProductName, "1")};
            Assert.That(_consumer.FindCheapestStoreWithSumForListOfProducts(list).Price, Is.EqualTo(12));
        }

        [Test]
        public void FindCheapestStoreWithSumForListOfProducts_2StoresSellsTestProduct_ReturnsTheCheapestStoreWithPriceOf72()
        {
            var list = new List<ProductInfo>() { new ProductInfo(_product.ProductName, "6") };
            Assert.That(_consumer.FindCheapestStoreWithSumForListOfProducts(list).Price, Is.EqualTo(72));
        }

        [Test]
        public void FindCheapestStoreWithSumForListOfProducts_2StoresSellsTestProductAndTestProduct1_ReturnsTheCheapestStoreWithPriceOf12()
        {
            var product1 = new Product() {ProductName = "TestProduct1"};
            _context.Products.Add(product1);
            var hasA3 = new HasA() {};
            var list = new List<ProductInfo>() { new ProductInfo(_product.ProductName, "1") };
            Assert.That(_consumer.FindCheapestStoreWithSumForListOfProducts(list).Price, Is.EqualTo(12));
        }
    }
}
