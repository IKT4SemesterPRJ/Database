using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
            product = new Product() {ProductName = "Banan"};
            _uut = new Storemanager.Storemanager(unitWork, store);
        }

        [Test]
        public void AddProductToMyStore_BananCantBeFoundInDatabase_BananIsAdded()
        {
            _uut.AddProductToMyStore(product.ProductName, 4.95);

            unitWork.Products.Received().Add(product);
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
            
            unitWork.HasA.Get(product.ProductId, store.StoreId).Returns(hasA);

            Assert.That(_uut.AddProductToMyStore(product.ProductName, 3.95), Is.EqualTo(-1));
        }

        [Test]
        public void AddProductToMyStore_BananIsFoundButNoConnectionToStore_ChangesAreSavedInDatabase()
        {
            _uut.AddProductToMyStore(product.ProductName, 3.95);

            unitWork.Received().Complete();
        }
    }
}
