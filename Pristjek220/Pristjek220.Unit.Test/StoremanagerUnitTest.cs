using Administration;
using NSubstitute;
using NSubstitute.ReturnsExtensions;
using NUnit.Framework;
using Pristjek220Data;

namespace Pristjek220.Unit.Test
{
    [TestFixture]
    class StoremanagerUnitTest
    {
        private IUnitOfWork _unitWork;
        private Storemanager _uut;
        private Store _store;
        private Product _product;

        [SetUp]
        public void SetUp()
        {
            _unitWork = Substitute.For<IUnitOfWork>();
            _store = new Store() {StoreName = "Aldi", StoreId = 22};
            _product = new Product() {ProductName = "Banan", ProductId = 10};
            _unitWork.Stores.FindStore(_store.StoreName).Returns(_store);
            _uut = new Storemanager(_unitWork, _store);
        }

        [Test]
        public void CreateStoreManager_StoreAlreadyExsistInDatabase_StoremanangerCreatedWithoutAddingStore()
        {
            _unitWork.Stores.FindStore(_store.StoreName).Returns(_store);
            
            var manager = new Storemanager(_unitWork, _store);
            Assert.That(_uut.Store, Is.EqualTo(manager.Store));
        }

        [Test]
        public void AddProductToDb_BananCantBeFoundInDatabase_BananIsAdded()
        {
            _uut.AddProductToDb(_product);

            _unitWork.Products.Received(1).Add(_product);
        }

        [Test]
        public void AddProductToDb_BananIsAlreadyInDb_ReturnMinusOne()
        {
            _unitWork.Products.FindProduct(_product.ProductName).Returns(_product);

            Assert.That(_uut.AddProductToDb(_product), Is.EqualTo(-1));
        }

        [Test]
        public void AddProductToMyStore_BananIsAlreadyInStore_ReturnMinusOne()
        {
            var hasA = new HasA()
            {
                Price = 1,
                Product = _product,
                ProductId = _product.ProductId,
                Store = _store,
                StoreId = _store.StoreId
            };

            _unitWork.HasA.Get(_store.StoreId, _product.ProductId).Returns(hasA);

            Assert.That(_uut.AddProductToMyStore(_product, 3.95), Is.EqualTo(-1));
        }

        [Test]
        public void AddProductToMyStore_BananIsNotInStore_ChangesAreSavedInDatabase()
        {
            _uut.AddProductToMyStore(_product, 2.95);

            _unitWork.Received(1).Complete();
        }

        [Test]
        public void FindProduct_FindBananInDb_FindFunktionCalled()
        {
            _uut.FindProduct(_product.ProductName);

            _unitWork.Products.Received(1).FindProduct(_product.ProductName);
        }

        [Test]
        public void RemoveProductFromMyStore_BananWithHasAIsInStore_ChangesAreSavedInDatabase()
        {
            var hasA = new HasA()
            {
                Price = 1,
                Product = _product,
                ProductId = _product.ProductId,
                Store = _store,
                StoreId = _store.StoreId
            };

            _uut.AddProductToMyStore(_product, 1);
            _unitWork.HasA.FindHasA(_store.StoreName, _product.ProductName).Returns(hasA);
            _uut.RemoveProductFromMyStore(_product);

            _unitWork.Received(2).Complete();
        }

        [Test]
        public void RemoveProductFromMyStore_BananWithHasAIsInStore_RemoveIsCalled()
        {
            var hasA = new HasA()
            {
                Price = 1,
                Product = _product,
                ProductId = _product.ProductId,
                Store = _store,
                StoreId = _store.StoreId
            };

            _uut.AddProductToMyStore(_product, 1);
            _unitWork.HasA.FindHasA(_store.StoreName, _product.ProductName).Returns(hasA);
            _uut.RemoveProductFromMyStore(_product);

            _unitWork.HasA.Received(1).Remove(Arg.Any<HasA>());
        }

        [Test]
        public void RemoveProductFromMyStore_HasARelationDoesNotExistInDatabase_ReturnsMinusOne()
        {
            _unitWork.HasA.FindHasA(_store.StoreName, _product.ProductName).ReturnsNull();

            Assert.That(_uut.RemoveProductFromMyStore(_product), Is.EqualTo(-1));
        }

        [Test]
        public void RemoveProductFromMyStore_BananWithHasAIsInNoStore_ProductsRemoveCalled()
        {
            var hasA = new HasA()
            {
                Price = 1,
                Product = _product,
                ProductId = _product.ProductId,
                Store = _store,
                StoreId = _store.StoreId
            };

            _uut.AddProductToMyStore(_product, 1);
            _unitWork.HasA.FindHasA(_store.StoreName, _product.ProductName).Returns(hasA);
            _product.HasARelation.Clear();
            _uut.RemoveProductFromMyStore(_product);
            _unitWork.Received(1).Products.Remove(_product);
        }

        [Test]
        public void changePriceOfProductInStore_HasPriceBeenChangedFrom2to4_reutrns4()
        {
            var hasA = new HasA()
            {
                Price = 2,
                Product = _product,
                ProductId = _product.ProductId,
                Store = _store,
                StoreId = _store.StoreId
            };

            _uut.AddProductToMyStore(_product, 2);
            _unitWork.HasA.Get(_store.StoreId, _product.ProductId).Returns(hasA);
            _uut.changePriceOfProductInStore(_product, 4);

        }

        [Test]
        public void FindProductInStore_FindBananInAldi_FindFunktionCalled()
        {
            _uut.FindProductInStore(_product.ProductName);

            _unitWork.Stores.Received(1).FindProductInStore(_uut.Store.StoreName ,_product.ProductName);
        }
    }
}
