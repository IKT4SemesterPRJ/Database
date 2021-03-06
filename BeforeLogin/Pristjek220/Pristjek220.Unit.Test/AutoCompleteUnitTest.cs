﻿using System.Collections.Generic;
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

        [SetUp]
        public void SetUp()
        {
            _unitWork = Substitute.For<IUnitOfWork>();
            _products = new List<Product>();
            _stores = new List<Store>();
            _uut = new SharedFunctionalities.Autocomplete(_unitWork);
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
    }
}