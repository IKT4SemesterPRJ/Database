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
        private Autocomplete.Autocomplete _uut;
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

            _uut = new Autocomplete.Autocomplete(_unitWork);
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