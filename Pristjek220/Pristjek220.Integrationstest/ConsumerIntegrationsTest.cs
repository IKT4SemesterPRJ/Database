using System.Collections.Generic;
using System.Globalization;
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

            _consumer = new Consumer.Consumer(_unit);
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

            Assert.That(_consumer.MoneySaved, Is.EqualTo(double.Parse("-1,00", new CultureInfo("da-DK")).ToString("F2") + " kr"));
        }

        [Test]
        public void ChangeProductToAnotherStore_ProductChangeToGetSoldInTestStore1_TotalSumChangedTo13()
        {
            var storProdAndPrice = new StoreProductAndPrice() { Price = _hasA.Price, StoreName = _hasA.Store.StoreName, ProductName = _product.ProductName, Quantity = "1", Sum = _hasA.Price };
            _consumer.GeneratedShoppingListData.Add(storProdAndPrice);
            _consumer.ChangeProductToAnotherStore(_store1.StoreName, storProdAndPrice);

            Assert.That(_consumer.TotalSum, Is.EqualTo(double.Parse("13,00", new CultureInfo("da-DK")).ToString("F2") + " kr"));
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

        [Test]
        public void StoreNames_GetStoreNames_UnitofWorkReturnsAllStores()
        {
            var list = _consumer.StoreNames;

            Assert.That(list.Count, Is.EqualTo(2));
        }

        [Test]
        public void StoreNames_GetStoreNames_FirstSpaceOfListIsEqualToFirstStoreInDatabase()
        {
            var list = _consumer.StoreNames;

            Assert.That(list[0], Is.EqualTo("TestStore"));
        }

        [Test]
        public void StoreNames_GetStoreNames_SecondSpaceOfListIsEqualToSecondStoreInDatabase()
        {
            var list = _consumer.StoreNames;

            Assert.That(list[1], Is.EqualTo("TestStore1"));
        }

        [Test]
        public void StoreNames_GetStoreNamesWithAdmin_UnitofWorkReturnsTwoStores()
        {
            var adminStore = new Store() { StoreName = "Admin" };
            _context.Stores.Add(adminStore);
            var list = _consumer.StoreNames;

            Assert.That(list.Count, Is.EqualTo(2));
        }

        [Test]
        public void FindStroesThatSellsProduct_InsetValidProduct_UnitofWorkReturnsStoresThatsSellProduct()
        {
            var list = _consumer.FindStoresThatSellsProduct(_product.ProductName);

            Assert.That(list.Count, Is.EqualTo(2));
        }

        [Test]
        public void FindStroesThatSellsProduct_AddANewStoreANdInsetValidProduct_UnitofWorkReturnsStoresThatsSellProduct()
        {
            var TestStore = new Store() { StoreName = "TestStore2" };
            _context.Stores.Add(TestStore);
            _context.SaveChanges();
            var HasA = new HasA() { Product = _product, Store = TestStore, ProductId = _product.ProductId, StoreId = TestStore.StoreId, Price = 13 };
            _context.HasARelation.Add(HasA);
            _context.SaveChanges();

            var list = _consumer.FindStoresThatSellsProduct(_product.ProductName);

            Assert.That(list.Count, Is.EqualTo(3));
        }

        [Test]
        public void FindStroesThatSellsProduct_InsetInvalidProduct_UnitofWorkReturnsEmptyList()
        {
            var list = _consumer.FindStoresThatSellsProduct("Invalid");

            Assert.That(list.Count, Is.EqualTo(0));
        }

        [Test]
        public void FillOptionsStore_CreateNewConsumerClass_OptionsStoresHaveAllStores()
        {
            var consumer = _consumer = new Consumer.Consumer(_unit);
            Assert.That(consumer.OptionsStores.Count, Is.EqualTo(2));
        }

        [Test]
        public void FillOptionsStore_ConstructerCall_OptionsStoresHaveTwoStores()
        {
            Assert.That(_consumer.OptionsStores.Count, Is.EqualTo(2));
        }

        [Test]
        public void FillOptionsStore_AddAdmin_OptionsStoresHaveTwoStores()
        {
            var adminStore = new Store() { StoreName = "Admin" };
            _context.Stores.Add(adminStore);
            _context.SaveChanges();

            var consumer = new Consumer.Consumer(_unit);

            Assert.That(consumer.OptionsStores.Count, Is.EqualTo(2));
        }

        [Test]
        public void FillOptionsStore_AddStore_OptionsStoresHaveThreeStores()
        {
            var NewStore = new Store() { StoreName = "NewStore" };
            _context.Stores.Add(NewStore);
            _context.SaveChanges();

            var consumer = new Consumer.Consumer(_unit);

            Assert.That(consumer.OptionsStores.Count, Is.EqualTo(3));
        }

    }
}
