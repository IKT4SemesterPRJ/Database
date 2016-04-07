using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Consumer_GUI.User_Controls;
using NSubstitute;
using NSubstitute.ReturnsExtensions;
using NUnit.Framework;
using Pristjek220Data;


namespace Pristjek220.Unit.Test
{
    [TestFixture]
    class FindProductModelTest
    {
        private IUnitOfWork _unitWork;
        private Autocomplete.Autocomplete _uut;
        private List<Store> _stores;
        private List<Product> _products;
        private Consumer_GUI.User_Controls.FindProductModel _findProduct;
     

        [SetUp]
        public void SetUp()
        {
            _unitWork = Substitute.For<IUnitOfWork>();
            _products = new List<Product>();
            _stores = new List<Store>();
            _uut = new Autocomplete.Autocomplete(_unitWork);
            _findProduct = new FindProductModel(Substitute.For<Consumer.IConsumer>());
        }

        [Test]
        public void AddToStoreList_WithBananAsProductname_IsFindStoresThatSellsProductCalled()
        {
            _findProduct.ProductName = "Banan";
            _findProduct.User.FindStoresThatSellsProduct(_findProduct.ProductName).Returns(new List<StoreAndPrice>());
            _findProduct.AddToStoreListCommand.Execute(this);
            _findProduct.User.Received(1).FindStoresThatSellsProduct(_findProduct.ProductName);
        }

        [Test]
        public void AddToStoreList_ProductDoesNotExist_MessageBoxIsShown()
        {
            _findProduct.ProductName = "Banan";
            _findProduct.User.FindStoresThatSellsProduct(_findProduct.ProductName).ReturnsNull();
            _findProduct.AddToStoreListCommand.Execute(this);
    
            // test om messagebox is shown
        }

        [Test]
        public void AddToStoreList_BananIsAddedToList_AldiAnd8IsInTheList()
        {
            List<StoreAndPrice> list = new List<StoreAndPrice>();
            var storeandprice = new StoreAndPrice();
            storeandprice.Name = "Aldi";
            storeandprice.Price = 8;
            list.Add(storeandprice);
            _findProduct.ProductName = "Banan";
            _findProduct.User.FindStoresThatSellsProduct(_findProduct.ProductName).Returns(list);
            _findProduct.AddToStoreListCommand.Execute(this);
            Assert.That(_findProduct.StorePrice.Contains(storeandprice), Is.EqualTo(true));
            
        }





    }
}
