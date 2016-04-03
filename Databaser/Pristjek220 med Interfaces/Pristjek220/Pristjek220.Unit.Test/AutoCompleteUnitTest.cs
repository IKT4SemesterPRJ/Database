using System.Collections.Generic;
using NSubstitute;
using NSubstitute.ReturnsExtensions;
using NUnit.Framework;
using Pristjek220Data;

namespace Pristjek220.Unit.Test
{
    [TestFixture]
    class AutoCompleteUnitTest
    {
        private IUnitOfWork _unitWork;
        private AutoComplete.Autocomplete _uut;
        private List<Store> _storesWithF;
        private List<Store> _storesWithA; 
        private List<Product> _productsWithB;
        private List<Product> _productsWithK;

        [SetUp]
        public void SetUp()
        {
            _unitWork = Substitute.For<IUnitOfWork>();
            
            _productsWithB = new List<Product>();
            var bananProduct = new Product() {ProductName = "Banan"};
            var baconProduct = new Product() {ProductName = "Bacon"};
            var bollerProduct = new Product() {ProductName = "Boller"};
            _productsWithB.Add(baconProduct);
            _productsWithB.Add(bananProduct);
            _productsWithB.Add(bollerProduct);

            _productsWithK = new List<Product>();
            var kakaoProduct = new Product() { ProductName = "Kakao" };
            _productsWithK.Add(kakaoProduct);

            _storesWithF = new List<Store>();
            var faktaStore = new Store() {StoreName = "Fakta"};
            var føtexStore = new Store() {StoreName = "Føtex"};
            var fleggardStore = new Store() {StoreName = "Fleggard"};
            _storesWithF.Add(faktaStore);
            _storesWithF.Add(føtexStore);
            _storesWithF.Add(fleggardStore);

            _storesWithA = new List<Store>();
            var aldiStore = new Store() { StoreName = "Aldi" };
            _storesWithA.Add(aldiStore);

            _uut = new AutoComplete.Autocomplete(_unitWork);
        }

        [Test]
        public void AutoCompleteProduct_LookUpWordDoesNotExist_ReturnNull()
        {
            _unitWork.Products.FindProductStartingWith("?").ReturnsNull();
            Assert.That(_uut.AutoCompleteProduct("?"), Is.EqualTo(null));
        }

        [Test]
        public void AutoCompleteProduct_LookUpWordIsEmpty_ReturnNull()
        {
            _unitWork.Products.FindProductStartingWith("").ReturnsNull();
            Assert.That(_uut.AutoCompleteProduct(""), Is.EqualTo(null));
        }

        [Test]
        public void AutoCompleteProduct_LookUpWordStartsWithB_ReturnListOfSize3()
        {
            _unitWork.Products.FindProductStartingWith("B").Returns(_productsWithB);
            Assert.That(_uut.AutoCompleteProduct("B").Count, Is.EqualTo(3));
        }

        [Test]
        public void AutoCompleteProduct_LookUpWordStartsWithK_ReturnListOfSize1()
        {
            _unitWork.Products.FindProductStartingWith("K").Returns(_productsWithK);
            Assert.That(_uut.AutoCompleteProduct("K").Count, Is.EqualTo(1));
        }

        [Test]
        public void AutoCompleteStore_LookUpWordDoesNotExist_ReturnNull()
        {
            _unitWork.Products.FindProductStartingWith("").ReturnsNull();
            Assert.That(_uut.AutoCompleteStore("?"), Is.EqualTo(null));
        }

        [Test]
        public void AutoCompleteStore_LookUpWordIsEmpty_ReturnNull()
        {
            _unitWork.Products.FindProductStartingWith("").ReturnsNull();
            Assert.That(_uut.AutoCompleteStore(""), Is.EqualTo(null));
        }

        [Test]
        public void AutoCompleteStore_LookUpWordStartsWithF_ReturnListOfSize3()
        {
            _unitWork.Stores.FindStoreStartingWith("F").Returns(_storesWithF);
            Assert.That(_uut.AutoCompleteStore("F").Count, Is.EqualTo(3));
        }

        [Test]
        public void AutoCompleteStore_LookUpWordStartsWithA_ReturnListOfSize1()
        {
            _unitWork.Stores.FindStoreStartingWith("A").Returns(_storesWithA);
            Assert.That(_uut.AutoCompleteStore("A").Count, Is.EqualTo(1));
        }
    }
}





//namespace Pristjek220.Unit.Test
//{
//    [TestFixture]
//    public class ConsumerUnitTest
//    {
        

//        [Test]
//        public void DoesProductExsist_DoesBananExsist_ReturnTrue()
//        {
//            _unitWork.Products.FindProduct(_product.ProductName).Returns(_product);

//            Assert.That(_uut.DoesProductExsist(_product.ProductName), Is.EqualTo(true));
//        }

//        [Test]
//        public void DoesProductExsist_DoesBananExsist_ReturnFalse()
//        {
//            _unitWork.Products.FindProduct(_product.ProductName).Returns((Product)null);

//            Assert.That(_uut.DoesProductExsist(_product.ProductName), Is.EqualTo(false));
//        }

//        [Test]
//        public void FindCheapestStore_FindCheapestStoreForBananButBananIsNotInDb_ReturnNull()
//        {
//            _unitWork.Products.FindProduct(_product.ProductName).Returns((Product)null);

//            Assert.That(_uut.FindCheapestStore(_product.ProductName), Is.EqualTo(null));
//        }

//        [Test]
//        public void FindCheapestStore_FindCheapestStoreForBananButBananHaveNoRelationToAStore_ReturnNull()
//        {
//            _unitWork.Products.FindProduct(_product.ProductName).Returns(_product);

//            Assert.That(_uut.FindCheapestStore(_product.ProductName), Is.EqualTo(null));
//        }

//        [Test]
//        public void FindCheapestStore_FindCheapestStoreForBanan_ReturnsStore()
//        {
//            var fakta = new Store() { StoreName = "Fakta" };
//            _unitWork.Products.FindProduct(_product.ProductName).Returns(_product);
//            _product.HasARelation.Add(new HasA() { Price = 2.95, Store = _store });
//            _product.HasARelation.Add(new HasA() { Price = 1.95, Store = fakta });

//            Assert.That(_uut.FindCheapestStore(_product.ProductName), Is.EqualTo(fakta));
//        }

//        [Test]
//        public void FindStoreAssortment_FindAldisAssortment_FindProductsInStoreFunctionCalled()
//        {
//            _uut.FindStoresAssortment(_store.StoreName);

//            _unitWork.Stores.Received(1).FindProductsInStore(_store.StoreName);
//        }

//        [Test]
//        public void FindStoreThatSells_FindWhichStoreSellsBanan_FunctionToGenerateListCalled()
//        {
//            _uut.FindStoresThatSellsProduct(_product.ProductName);

//            _unitWork.Products.Received(1).FindStoresThatSellsProduct(_product.ProductName);
//        }

        //[Test]
        //public void CreateShoppingList_CreateShoppingListForBanan_ListHasCorrectProductName()
        //{
        //    var shoppingList = new List<string> { _product.ProductName };

        //    var fakta = new Store() { StoreName = "Fakta" };
        //    _unitWork.Products.FindProduct(_product.ProductName).Returns(_product);
        //    _product.HasARelation.Add(new HasA() { Price = 2.95, Store = _store });
        //    _product.HasARelation.Add(new HasA() { Price = 1.95, Store = fakta });
        //    _store.HasARelation.Add(new HasA() {Price = 2.95, Product = _product, Store = _store});
        //    fakta.HasARelation.Add(new HasA() { Price = 1.95, Product = _product, Store = fakta });

        //    var createdShoppingList = _uut.CreateShoppingList(shoppingList);

        //    Assert.That(createdShoppingList.Find(x => x.ProductName == _product.ProductName).ProductName,
        //        Is.EqualTo("Banan"));
        //}

        //[Test]
        //public void CreateShoppingList_CreateShoppingListForBanan_ListHasCorrectStoreName()
        //{
        //    var shoppingList = new List<string> { _product.ProductName };

        //    var fakta = new Store() { StoreName = "Fakta" };
        //    _unitWork.Products.FindProduct(_product.ProductName).Returns(_product);
        //    _product.HasARelation.Add(new HasA() { Price = 2.95, Store = _store });
        //    _product.HasARelation.Add(new HasA() { Price = 1.95, Store = fakta });
        //    _store.HasARelation.Add(new HasA() { Price = 2.95, Product = _product, Store = _store });
        //    fakta.HasARelation.Add(new HasA() { Price = 1.95, Product = _product, Store = fakta });

        //    var createdShoppingList = _uut.CreateShoppingList(shoppingList);

        //    Assert.That(createdShoppingList.Find(x => x.ProductName == _product.ProductName).StoreName,
        //        Is.EqualTo(fakta.StoreName));
        //}

        //[Test]
        //public void CreateShoppingList_CreateShoppingListForBanan_ListHasCorrectPrice()
        //{
        //    var shoppingList = new List<string> { _product.ProductName };

        //    var fakta = new Store() { StoreName = "Fakta" };
        //    _unitWork.Products.FindProduct(_product.ProductName).Returns(_product);
        //    _product.HasARelation.Add(new HasA() { Price = 2.95, Store = _store });
        //    _product.HasARelation.Add(new HasA() { Price = 1.95, Store = fakta });
        //    _store.HasARelation.Add(new HasA() { Price = 2.95, Product = _product, Store = _store });
        //    fakta.HasARelation.Add(new HasA() { Price = 1.95, Product = _product, Store = fakta });

        //    var createdShoppingList = _uut.CreateShoppingList(shoppingList);

        //    Assert.That(createdShoppingList.Find(x => x.ProductName == _product.ProductName).Price,
        //        Is.EqualTo(fakta.HasARelation.Find(x => x.Product.ProductName == _product.ProductName).Price));
        //}