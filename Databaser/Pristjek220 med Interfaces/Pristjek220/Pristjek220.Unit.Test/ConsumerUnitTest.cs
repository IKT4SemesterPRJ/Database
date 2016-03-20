using System;
using System.Collections.Generic;
using NSubstitute;
using NUnit.Framework;
using Pristjek220Data;


namespace Pristjek220.Unit.Test
{
    [TestFixture]
    public class ConsumerUnitTest
    {
        private IUnitOfWork _unitWork;
        private Consumer.Consumer _uut;
        private Store _store;
        private Product _product;

        [SetUp]
        public void SetUp()
        {
            _unitWork = Substitute.For<IUnitOfWork>();
            _store = new Store() { StoreName = "Aldi" };
            _product = Substitute.For<Product>();
            _product.ProductName = "Banan";
            _product.HasARelation = new List<HasA>();
            _uut = new Consumer.Consumer(_unitWork);
        }

        [Test]
        public void DoesProductExsist_DoesBananExsist_ReturnTrue()
        {
            _unitWork.Products.FindProduct(_product.ProductName).Returns(_product);

            Assert.That(_uut.DoesProductExsist(_product.ProductName), Is.EqualTo(true));
        }

        [Test]
        public void DoesProductExsist_DoesBananExsist_ReturnFalse()
        {
            _unitWork.Products.FindProduct(_product.ProductName).Returns((Product) null);

            Assert.That(_uut.DoesProductExsist(_product.ProductName), Is.EqualTo(false));
        }

        [Test]
        public void FindCheapestStore_FindCheapestStoreForBananButBananIsNotInDb_ReturnNull()
        {
            _unitWork.Products.FindProduct(_product.ProductName).Returns((Product) null);

            Assert.That(_uut.FindCheapestStore(_product.ProductName), Is.EqualTo(null));
        }

        [Test]
        public void FindCheapestStore_FindCheapestStoreForBananButBananHaveNoRelationToAStore_ReturnNull()
        {
            _unitWork.Products.FindProduct(_product.ProductName).Returns(_product);
            _product.HasARelation.Returns(new List<HasA>());

            Assert.That(_uut.FindCheapestStore(_product.ProductName), Is.EqualTo(null));
        }

        [Test]
        public void FindCheapestStore_FindCheapestStoreForBanan_ReturnsStore()
        {
            var fakta = new Store() {StoreName = "Fakta"};
            _unitWork.Products.FindProduct(_product.ProductName).Returns(_product);
            _product.HasARelation.Add(new HasA() {Price = 2.95, Store = _store});
            _product.HasARelation.Add(new HasA() { Price = 1.95, Store = fakta });

            Assert.That(_uut.FindCheapestStore(_product.ProductName), Is.EqualTo(fakta));
        }

        [Test]
        public void FindStoreAssortment_FindAldisAssortment_ReturnListWithAssortmentAndPrice()
        {
            _uut.FindStoresAssortment(_store.StoreName);

            _unitWork.Stores.Received(1).FindProductsInStore(_store.StoreName);
        }

        [Test]
        public void CreateShoppingList_CreateShoppingListForBanan_ListHasCorrectProductName()
        {
            var shoppingList = new List<string> { _product.ProductName };

            var fakta = new Store() { StoreName = "Fakta" };
            _unitWork.Products.FindProduct(_product.ProductName).Returns(_product);
            _product.HasARelation.Add(new HasA() { Price = 2.95, Store = _store });
            _product.HasARelation.Add(new HasA() { Price = 1.95, Store = fakta });
            _store.HasARelation.Add(new HasA() {Price = 2.95, Product = _product, Store = _store});
            fakta.HasARelation.Add(new HasA() { Price = 1.95, Product = _product, Store = fakta });

            var createdShoppingList = _uut.CreateShoppingList(shoppingList);

            Assert.That(createdShoppingList.Find(x => x.ProductName == _product.ProductName).ProductName,
                Is.EqualTo("Banan"));
        }

        [Test]
        public void CreateShoppingList_CreateShoppingListForBanan_ListHasCorrectStoreName()
        {
            var shoppingList = new List<string> { _product.ProductName };

            var fakta = new Store() { StoreName = "Fakta" };
            _unitWork.Products.FindProduct(_product.ProductName).Returns(_product);
            _product.HasARelation.Add(new HasA() { Price = 2.95, Store = _store });
            _product.HasARelation.Add(new HasA() { Price = 1.95, Store = fakta });
            _store.HasARelation.Add(new HasA() { Price = 2.95, Product = _product, Store = _store });
            fakta.HasARelation.Add(new HasA() { Price = 1.95, Product = _product, Store = fakta });

            var createdShoppingList = _uut.CreateShoppingList(shoppingList);

            Assert.That(createdShoppingList.Find(x => x.ProductName == _product.ProductName).StoreName,
                Is.EqualTo(fakta.StoreName));
        }

        [Test]
        public void CreateShoppingList_CreateShoppingListForBanan_ListHasCorrectPrice()
        {
            var shoppingList = new List<string> { _product.ProductName };

            var fakta = new Store() { StoreName = "Fakta" };
            _unitWork.Products.FindProduct(_product.ProductName).Returns(_product);
            _product.HasARelation.Add(new HasA() { Price = 2.95, Store = _store });
            _product.HasARelation.Add(new HasA() { Price = 1.95, Store = fakta });
            _store.HasARelation.Add(new HasA() { Price = 2.95, Product = _product, Store = _store });
            fakta.HasARelation.Add(new HasA() { Price = 1.95, Product = _product, Store = fakta });

            var createdShoppingList = _uut.CreateShoppingList(shoppingList);

            Assert.That(createdShoppingList.Find(x => x.ProductName == _product.ProductName).Price,
                Is.EqualTo(fakta.HasARelation.Find(x => x.Product.ProductName == _product.ProductName).Price));
        }
    }
}