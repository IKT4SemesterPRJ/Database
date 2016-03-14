using System.Collections.Generic;
using System.Linq;
using NSubstitute;
using NUnit.Framework;
using Pristjek220Data;

namespace Pristjek220.Unit.Test
{
    [TestFixture]
    class StoremanagerUnitTest
    {
        private IUnitOfWork unitWork;
        private Storemanager.Storemanager _uut;
        private Store store;
        private Product product;

        [SetUp]
        public void SetUp()
        {
            unitWork = Substitute.For<IUnitOfWork>();
            store = new Store() {StoreName = "Aldi"};
            product = new Product() {ProductName = "Banan", ProductId = 10};
            _uut = new Storemanager.Storemanager(unitWork, store);
        }

        [Test]
        public void AddProductToDb_BananCantBeFoundInDatabase_BananIsAdded()
        {
            _uut.AddProductToDb(product);

            unitWork.Products.Received(1).Add(product);
        }

        [Test]
        public void AddProductToDb_BananIsAlreadyInDb_ReturnMinusOne()
        {
            unitWork.Products.FindProduct(product.ProductName).Returns(product);

            Assert.That(_uut.AddProductToDb(product), Is.EqualTo(-1));
        }

        [Test]
        public void AddProductToMyStore_BananIsAlreadyInStore_ReturnMinusOne()
        {
            var hasA = new HasA()
            {
                Price = 1,
                Product = product,
                ProductId = product.ProductId,
                Store = store,
                StoreId = store.StoreId
            };

            unitWork.HasA.Get(store.StoreId, product.ProductId).Returns(hasA);

            Assert.That(_uut.AddProductToMyStore(product, 3.95), Is.EqualTo(-1));
        }

        [Test]
        public void AddProductToMyStore_BananIsNotInStore_ChangesAreSavedInDatabase()
        {
            _uut.AddProductToMyStore(product, 2.95);

            unitWork.Received(1).Complete();
        }

        [Test]
        public void FindProduct_FindBananInDb_FindFunktionCalled()
        {
            _uut.FindProduct(product.ProductName);

            unitWork.Products.Received(1).FindProduct(product.ProductName);
        }
    }
}
