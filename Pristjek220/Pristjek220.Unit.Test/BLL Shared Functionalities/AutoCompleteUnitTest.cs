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
        private SharedFunctionalities.Autocomplete _uut;
        private List<Store> _stores;
        private List<Product> _products;
        private List<ProductAndPrice> _productAndPrices;

        [SetUp]
        public void SetUp()
        {
            _unitWork = Substitute.For<IUnitOfWork>();
            _products = new List<Product>();
            _stores = new List<Store>();
            _productAndPrices = new List<ProductAndPrice>();
            _uut = new SharedFunctionalities.Autocomplete(_unitWork);
        }

        [Test]
        public void AutoCompleteProduct_SortList_ListIsSorted()
        {
            var bananProduct = new Product() { ProductName = "Banan" };
            var baconProduct = new Product() { ProductName = "Bacon" };
            var bollerProduct = new Product() { ProductName = "Boller" };
            var brombaerProduct = new Product() { ProductName = "Brombaer" };
            _products.Add(brombaerProduct);
            _products.Add(bananProduct);
            _products.Add(baconProduct);
            _products.Add(bollerProduct);
           

            _unitWork.Products.FindProductStartingWith("B").Returns(_products);
            Assert.That(_uut.AutoCompleteProduct("B")[0], Is.EqualTo("Bacon"));
        }

        [Test]
        public void AutoCompleteProduct_SortList_ListIsSortedCheckTwo()
        {
            var bananProduct = new Product() { ProductName = "Banan" };
            var baconProduct = new Product() { ProductName = "Bacon" };
            var bollerProduct = new Product() { ProductName = "Boller" };
            var brombaerProduct = new Product() { ProductName = "Brombaer" };
            _products.Add(brombaerProduct);
            _products.Add(bananProduct);
            _products.Add(baconProduct);
            _products.Add(bollerProduct);


            _unitWork.Products.FindProductStartingWith("B").Returns(_products);
            Assert.That(_uut.AutoCompleteProduct("B")[1], Is.EqualTo("Banan"));
        }


        [Test]
        public void AutoCompleteProduct_LookUpWordStartsWithB_ReturnListOfSize3()
        {
            var bananProduct = new Product() { ProductName = "Banan" };
            var baconProduct = new Product() { ProductName = "Bacon" };
            var bollerProduct = new Product() { ProductName = "Boller" };
            var brombaerProduct = new Product() { ProductName = "Brombaer" };
            _products.Add(baconProduct);
            _products.Add(bananProduct);
            _products.Add(bollerProduct);
            _products.Add(brombaerProduct);
            
            _unitWork.Products.FindProductStartingWith("B").Returns(_products);
            Assert.That(_uut.AutoCompleteProduct("B").Count, Is.EqualTo(3));
        }

        [Test]
        public void AutoCompleteProduct_LookUpWordStartsWithK_ReturnListOfSize1()
        {
            var kakaoProduct = new Product() { ProductName = "Kakao" };
            _products.Add(kakaoProduct);

            _unitWork.Products.FindProductStartingWith("K").Returns(_products);
            Assert.That(_uut.AutoCompleteProduct("K").Count, Is.EqualTo(1));
        }

        [Test]
        public void AutoCompleteStore_SortList_ListIsSorted()
        {
            var faktaStore = new Store() { StoreName = "Fakta" };
            var føtexStore = new Store() { StoreName = "Føtex" };
            var fleggardStore = new Store() { StoreName = "Fleggard" };
            var fantasiStore = new Store() { StoreName = "Fantasi" };
            _stores.Add(fantasiStore);
            _stores.Add(føtexStore);
            _stores.Add(fleggardStore);
            _stores.Add(faktaStore);
            
            _unitWork.Stores.FindStoreStartingWith("F").Returns(_stores);
            Assert.That(_uut.AutoCompleteStore("F")[0], Is.EqualTo("Fakta"));
        }

        [Test]
        public void AutoCompleteStore_SortList_ListIsSortedCheckTwo()
        {
            var faktaStore = new Store() { StoreName = "Fakta" };
            var føtexStore = new Store() { StoreName = "Føtex" };
            var fleggardStore = new Store() { StoreName = "Fleggard" };
            var fantasiStore = new Store() { StoreName = "Fantasi" };
            _stores.Add(fantasiStore);
            _stores.Add(føtexStore);
            _stores.Add(fleggardStore);
            _stores.Add(faktaStore);

            _unitWork.Stores.FindStoreStartingWith("F").Returns(_stores);
            Assert.That(_uut.AutoCompleteStore("F")[1], Is.EqualTo("Fantasi"));
        }

        [Test]
        public void AutoCompleteStore_LookUpWordStartsWithF_ReturnListOfSize3()
        {
            var faktaStore = new Store() { StoreName = "Fakta" };
            var føtexStore = new Store() { StoreName = "Føtex" };
            var fleggardStore = new Store() { StoreName = "Fleggard" };
            var fantasiStore = new Store() { StoreName = "Fantasi" };
            _stores.Add(faktaStore);
            _stores.Add(føtexStore);
            _stores.Add(fleggardStore);
            _stores.Add(fantasiStore);

            _unitWork.Stores.FindStoreStartingWith("F").Returns(_stores);
            Assert.That(_uut.AutoCompleteStore("F").Count, Is.EqualTo(3));
        }

        [Test]
        public void AutoCompleteStore_LookUpWordStartsWithA_ReturnListOfSize1()
        {
            var aldiStore = new Store() { StoreName = "Aldi" };
            _stores.Add(aldiStore);

            _unitWork.Stores.FindStoreStartingWith("A").Returns(_stores);
            Assert.That(_uut.AutoCompleteStore("A").Count, Is.EqualTo(1));
        }

        [Test]
        public void AutoCompleteProductForOneStore_LookUpWordStartsWithB_ReturnsListOfSize3()
        {
            var aldiStoreName = "Aldi";
            var bananProductAndPrice = new ProductAndPrice() {Name = "Banan"};
            var baconProductAndPrice = new ProductAndPrice() { Name = "Bacon" };
            var bollerProductAndPrice = new ProductAndPrice() { Name = "Boller" };
            var brombaerProductAndPrice = new ProductAndPrice() { Name = "Brombaer" };
            _productAndPrices.Add(bananProductAndPrice);
            _productAndPrices.Add(baconProductAndPrice);
            _productAndPrices.Add(bollerProductAndPrice);
            _productAndPrices.Add(brombaerProductAndPrice);

            _unitWork.Stores.FindProductsInStoreStartingWith(aldiStoreName, "B").Returns(_productAndPrices);
            Assert.That(_uut.AutoCompleteProductForOneStore(aldiStoreName, "B").Count, Is.EqualTo(3));
        }

        [Test]
        public void AutoCompleteProductForOneStore_LookUpWordStartsWithB_ReturnsListOfSize1()
        {
            var aldiStoreName = "Aldi";
            var bananProductAndPrice = new ProductAndPrice() { Name = "Banan" };
            _productAndPrices.Add(bananProductAndPrice);

            _unitWork.Stores.FindProductsInStoreStartingWith(aldiStoreName, "B").Returns(_productAndPrices);
            Assert.That(_uut.AutoCompleteProductForOneStore(aldiStoreName, "B").Count, Is.EqualTo(1));
        }

        [Test]
        public void AutoCompleteProductForOneStore_SortList_ListIsSorted()
        {
            var aldiStoreName = "Aldi";
            var bananProductAndPrice = new ProductAndPrice() { Name = "Banan" };
            var baconProductAndPrice = new ProductAndPrice() { Name = "Bacon" };
            var bollerProductAndPrice = new ProductAndPrice() { Name = "Boller" };
            var brombaerProductAndPrice = new ProductAndPrice() { Name = "Brombaer" };
            _productAndPrices.Add(brombaerProductAndPrice);
            _productAndPrices.Add(bananProductAndPrice);
            _productAndPrices.Add(baconProductAndPrice);
            _productAndPrices.Add(bollerProductAndPrice);
            

            _unitWork.Stores.FindProductsInStoreStartingWith(aldiStoreName, "B").Returns(_productAndPrices);
            Assert.That(_uut.AutoCompleteProductForOneStore(aldiStoreName, "B")[0], Is.EqualTo("Bacon"));
        }

        [Test]
        public void AutoCompleteProductForOneStore_SortList_ListIsSortedCheckTwo()
        {
            var aldiStoreName = "Aldi";
            var bananProductAndPrice = new ProductAndPrice() { Name = "Banan" };
            var baconProductAndPrice = new ProductAndPrice() { Name = "Bacon" };
            var bollerProductAndPrice = new ProductAndPrice() { Name = "Boller" };
            var brombaerProductAndPrice = new ProductAndPrice() { Name = "Brombaer" };
            _productAndPrices.Add(brombaerProductAndPrice);
            _productAndPrices.Add(bananProductAndPrice);
            _productAndPrices.Add(baconProductAndPrice);
            _productAndPrices.Add(bollerProductAndPrice);


            _unitWork.Stores.FindProductsInStoreStartingWith(aldiStoreName, "B").Returns(_productAndPrices);
            Assert.That(_uut.AutoCompleteProductForOneStore(aldiStoreName, "B")[1], Is.EqualTo("Banan"));
        }

        [Test]
        public void AutoCompleteProductForOneStore_FourProductsInListThatStartsWithB_FirstProductIsInReturnedList()
        {
            var aldiStoreName = "Aldi";
            var bananProductAndPrice = new ProductAndPrice() { Name = "Banan" };
            var baconProductAndPrice = new ProductAndPrice() { Name = "Bacon" };
            var bollerProductAndPrice = new ProductAndPrice() { Name = "Boller" };
            var brombaerProductAndPrice = new ProductAndPrice() { Name = "Brombaer" };
            _productAndPrices.Add(bananProductAndPrice);
            _productAndPrices.Add(baconProductAndPrice);
            _productAndPrices.Add(bollerProductAndPrice);
            _productAndPrices.Add(brombaerProductAndPrice);

            _unitWork.Stores.FindProductsInStoreStartingWith(aldiStoreName, "B").Returns(_productAndPrices);
            Assert.That(_uut.AutoCompleteProductForOneStore(aldiStoreName, "B").Contains(bananProductAndPrice.Name), Is.EqualTo(true));
        }

        [Test]
        public void AutoCompleteProductForOneStore_FourProductsInListThatStartsWithB_SecondProductIsInReturnedList()
        {
            var aldiStoreName = "Aldi";
            var bananProductAndPrice = new ProductAndPrice() { Name = "Banan" };
            var baconProductAndPrice = new ProductAndPrice() { Name = "Bacon" };
            var bollerProductAndPrice = new ProductAndPrice() { Name = "Boller" };
            var brombaerProductAndPrice = new ProductAndPrice() { Name = "Brombaer" };
            _productAndPrices.Add(bananProductAndPrice);
            _productAndPrices.Add(baconProductAndPrice);
            _productAndPrices.Add(bollerProductAndPrice);
            _productAndPrices.Add(brombaerProductAndPrice);

            _unitWork.Stores.FindProductsInStoreStartingWith(aldiStoreName, "B").Returns(_productAndPrices);
            Assert.That(_uut.AutoCompleteProductForOneStore(aldiStoreName, "B").Contains(baconProductAndPrice.Name), Is.EqualTo(true));
        }

        [Test]
        public void AutoCompleteProductForOneStore_FourProductsInListThatStartsWithB_ThirdProductIsInReturnedList()
        {
            var aldiStoreName = "Aldi";
            var bananProductAndPrice = new ProductAndPrice() { Name = "Banan" };
            var baconProductAndPrice = new ProductAndPrice() { Name = "Bacon" };
            var bollerProductAndPrice = new ProductAndPrice() { Name = "Boller" };
            var brombaerProductAndPrice = new ProductAndPrice() { Name = "Brombaer" };
            _productAndPrices.Add(bananProductAndPrice);
            _productAndPrices.Add(baconProductAndPrice);
            _productAndPrices.Add(bollerProductAndPrice);
            _productAndPrices.Add(brombaerProductAndPrice);

            _unitWork.Stores.FindProductsInStoreStartingWith(aldiStoreName, "B").Returns(_productAndPrices);
            Assert.That(_uut.AutoCompleteProductForOneStore(aldiStoreName, "B").Contains(bollerProductAndPrice.Name), Is.EqualTo(true));
        }

        [Test]
        public void AutoCompleteProductForOneStore_FourProductsInListThatStartsWithB_FourthProductIsNotInReturnedList()
        {
            var aldiStoreName = "Aldi";
            var bananProductAndPrice = new ProductAndPrice() { Name = "Banan" };
            var baconProductAndPrice = new ProductAndPrice() { Name = "Bacon" };
            var bollerProductAndPrice = new ProductAndPrice() { Name = "Boller" };
            var brombaerProductAndPrice = new ProductAndPrice() { Name = "Brombaer" };
            _productAndPrices.Add(bananProductAndPrice);
            _productAndPrices.Add(baconProductAndPrice);
            _productAndPrices.Add(bollerProductAndPrice);
            _productAndPrices.Add(brombaerProductAndPrice);

            _unitWork.Stores.FindProductsInStoreStartingWith(aldiStoreName, "B").Returns(_productAndPrices);
            Assert.That(_uut.AutoCompleteProductForOneStore(aldiStoreName, "B").Contains(brombaerProductAndPrice.Name), Is.EqualTo(false));
        }
    }
}