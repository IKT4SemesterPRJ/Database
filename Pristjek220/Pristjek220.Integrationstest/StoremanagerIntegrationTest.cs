using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Administration;
using NUnit.Framework;
using Pristjek220Data;

namespace Pristjek220.Integrationstest
{
    [TestFixture]
    class StoremanagerIntegrationTest
    {
        private IStoremanager _storemanager;
        private IUnitOfWork _unit;
        private DataContext _context;

        [SetUp]
        public void SetUp()
        {
            _context = new DataContext();
            _unit = new UnitOfWork(_context);
            _context.Database.Connection.ConnectionString = "Server=.\\SQLEXPRESS;Database=Pristjek220Data.DataContext; Trusted_Connection=True;";
            _context.Database.ExecuteSqlCommand("dbo.TestCleanTable");

            var store = new Store() { StoreName = "TestStore" };
            _context.Stores.Add(store);
            _context.SaveChanges();
            _storemanager = new Storemanager(_unit, store);
        }


        [Test]
        public void AddProductToDb_ProductIsAdded_Return0()
        {
            var product = new Product() {ProductName = "TestProduct"};

            Assert.That(_storemanager.AddProductToDb(product), Is.EqualTo(0));
        }

        [Test]
        public void AddProductToDb_ProductIsAlreadyInDb_ReturnMinus1()
        {
            var product = new Product() { ProductName = "TestProduct" };
            _context.Products.Add(product);
            _context.SaveChanges();

            Assert.That(_storemanager.AddProductToDb(product), Is.EqualTo(-1));
        }

        [Test]
        public void AddProductToDb_ProductIsAdded_ProductIsFoundInDb()
        {
            var product = new Product() { ProductName = "TestProduct" };
            _storemanager.AddProductToDb(product);

            Assert.That(_unit.Products.FindProduct("TestProduct"), Is.EqualTo(product));
        }

        [Test]
        public void AddProductToMyStore_ProductIsAlreadyAddedToTestStore_ReturnMinus1()
        {
            var product = new Product() { ProductName = "TestProduct" };
            _context.Products.Add(product);
            _context.SaveChanges();
            var hasA = new HasA() {Price = 22, Product = product, Store = _storemanager.Store, StoreId = _storemanager.Store.StoreId, ProductId = product.ProductId};
            _context.HasARelation.Add(hasA);
            _context.SaveChanges();

            Assert.That(_storemanager.AddProductToMyStore(product, 24), Is.EqualTo(-1));
        }

        [Test]
        public void AddProductToMyStore_ProductIsAddedToTestStore_Return0()
        {
            var product = new Product() { ProductName = "TestProduct" };

            Assert.That(_storemanager.AddProductToMyStore(product, 24), Is.EqualTo(0));
        }

        [Test]
        public void AddProductToMyStore_ProductIsAddedToTestStore_HasARelationCanBeFoundForProductAndStore()
        {
            var product = new Product() { ProductName = "TestProduct" };
            _storemanager.AddProductToMyStore(product, 24);

            Assert.AreNotEqual(_context.HasARelation.Find(_storemanager.Store.StoreId, product.ProductId), null);
        }

        [Test]
        public void FindProduct_TestProductIsInDb_TestProductIsReturned()
        {
            var product = new Product() { ProductName = "TestProduct" };
            _context.Products.Add(product);
            _context.SaveChanges();

            Assert.That(_storemanager.FindProduct("TestProduct"), Is.EqualTo(product));
        }

        [Test]
        public void FindProduct_TestProductIsNotInDb_NullIsReturned()
        {
            Assert.That(_storemanager.FindProduct("TestProduct"), Is.EqualTo(null));
        }

        [Test]
        public void FindProductInStore_TestProductIsInDbAndInStore_NameOfReturnedObjectIsEqualToProductName()
        {
            var product = new Product() { ProductName = "TestProduct" };
            _context.Products.Add(product);
            _context.SaveChanges();
            var hasA = new HasA() { Price = 22, Product = product, Store = _storemanager.Store, StoreId = _storemanager.Store.StoreId, ProductId = product.ProductId };
            _context.HasARelation.Add(hasA);
            _context.SaveChanges();

            Assert.That(_storemanager.FindProductInStore(product.ProductName).Name, Is.EqualTo(product.ProductName));
        }

        [Test]
        public void FindProductInStore_TestProductIsInDbAndInStore_PriceOfReturnedObjectIsEqualToHasARelationsPrice()
        {
            var product = new Product() { ProductName = "TestProduct" };
            _context.Products.Add(product);
            _context.SaveChanges();
            var hasA = new HasA() { Price = 22, Product = product, Store = _storemanager.Store, StoreId = _storemanager.Store.StoreId, ProductId = product.ProductId };
            _context.HasARelation.Add(hasA);
            _context.SaveChanges();

            Assert.That(_storemanager.FindProductInStore(product.ProductName).Price, Is.EqualTo(hasA.Price));
        }

        [Test]
        public void ChangePriceOfProductInStore_PriceOfTestProductInStoreIsSetTo10_HasARelationWithNewPriceCanBeFoundInDb()
        {
            var product = new Product() { ProductName = "TestProduct" };
            _context.Products.Add(product);
            _context.SaveChanges();
            var hasA = new HasA() { Price = 22, Product = product, Store = _storemanager.Store, StoreId = _storemanager.Store.StoreId, ProductId = product.ProductId };
            _context.HasARelation.Add(hasA);
            _context.SaveChanges();

            _storemanager.ChangePriceOfProductInStore(product, 10);

            Assert.That(_context.HasARelation.Find(_storemanager.Store.StoreId, product.ProductId).Price, Is.EqualTo(10));
        }

        [Test]
        public void RemoveProductFromMyStore_ProductIsInStore_HasARelationCannotBeFoundForProductAndStore()
        {
            var product = new Product() { ProductName = "TestProduct" };
            _context.Products.Add(product);
            _context.SaveChanges();
            var hasA = new HasA() { Price = 22, Product = product, Store = _storemanager.Store, StoreId = _storemanager.Store.StoreId, ProductId = product.ProductId };
            _context.HasARelation.Add(hasA);
            _context.SaveChanges();

            _storemanager.RemoveProductFromMyStore(product);

            Assert.That(_context.HasARelation.Find(_storemanager.Store.StoreId, product.ProductId), Is.EqualTo(null));
        }

        [Test]
        public void RemoveProductFromMyStore_ProductIsNotInStore_HasARelationCannotBeFoundForProductAndStore()
        {
            var product = new Product() { ProductName = "TestProduct" };
            _context.Products.Add(product);
            _context.SaveChanges();

            Assert.That(_storemanager.RemoveProductFromMyStore(product), Is.EqualTo(-1));
        }

        [Test]
        public void RemoveProductFromMyStore_ProductNoLongerHasAnyRelationsToAnyStore_ProductCannotBeFoundInDb()
        {
            var product = new Product() { ProductName = "TestProduct" };
            _context.Products.Add(product);
            _context.SaveChanges();
            var hasA = new HasA() { Price = 22, Product = product, Store = _storemanager.Store, StoreId = _storemanager.Store.StoreId, ProductId = product.ProductId };
            _context.HasARelation.Add(hasA);
            _context.SaveChanges();

            _storemanager.RemoveProductFromMyStore(product);

            Assert.That(_unit.Products.FindProduct("TestProduct"), Is.EqualTo(null));
        }

        [Test]
        public void RemoveProductFromMyStore_RemovalOfProductSuccesfull_Return0()
        {
            var product = new Product() { ProductName = "TestProduct" };
            _context.Products.Add(product);
            _context.SaveChanges();
            var hasA = new HasA() { Price = 22, Product = product, Store = _storemanager.Store, StoreId = _storemanager.Store.StoreId, ProductId = product.ProductId };
            _context.HasARelation.Add(hasA);
            _context.SaveChanges();

            Assert.That(_storemanager.RemoveProductFromMyStore(product), Is.EqualTo(0));
        }
    }
}
