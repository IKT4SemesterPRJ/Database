using NSubstitute;
using NUnit.Framework;
using Pristjek220Data;

namespace Pristjek220.Unit.Test
{
    [TestFixture]
    class StoremanagerUnitTest
    {
        private IUnitOfWork _unitWork;
        private Storemanager.Storemanager _uut;
        private Store _store;
        private Product _product;

        [SetUp]
        public void SetUp()
        {
            _unitWork = Substitute.For<IUnitOfWork>();
            _store = new Store() {StoreName = "Aldi"};
            _product = new Product() {ProductName = "Banan"};
            _uut = new Storemanager.Storemanager(_unitWork, _store);
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
    }
}
