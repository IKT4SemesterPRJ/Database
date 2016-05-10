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
        
        private Consumer_GUI.User_Controls.FindProductModel _findProduct;
     

        [SetUp]
        public void SetUp()
        {
           _findProduct = new FindProductModel(Substitute.For<Consumer.IConsumer>(), Substitute.For<IUnitOfWork>());
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
            _findProduct.User.FindStoresThatSellsProduct(_findProduct.ProductName).Returns(new List<StoreAndPrice>());
            _findProduct.AddToStoreListCommand.Execute(this);
            Assert.That(_findProduct.Error, Is.EqualTo("Produktet findes ikke"));   
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

        [Test]
        public void AddToStoreList_BananIsAddedToList_AldiN8AndKiwiN4IsInTheList()
        {
            List<StoreAndPrice> list = new List<StoreAndPrice>();
            var storeandprice = new StoreAndPrice();
            storeandprice.Name = "Aldi";
            storeandprice.Price = 8;
            list.Add(storeandprice);
            var storeandprice1 = new StoreAndPrice();
            storeandprice1.Name = "Kiwi";
            storeandprice1.Price = 4;
            list.Add(storeandprice1);
            _findProduct.ProductName = "Banan";
            _findProduct.User.FindStoresThatSellsProduct(_findProduct.ProductName).Returns(list);
            _findProduct.AddToStoreListCommand.Execute(this);
            Assert.That(_findProduct.StorePrice.Contains(storeandprice), Is.EqualTo(true));
            Assert.That(_findProduct.StorePrice.Contains(storeandprice1), Is.EqualTo(true));

        }

       [Test]
        public void IllegalSigFindProduct_PunktumTyped_ErrorEqualsString()
        {
            _findProduct.ProductName = "banan.";
            _findProduct.IllegalSignFindProductCommand.Execute(this);

            Assert.That(_findProduct.Error, Is.EqualTo("Der kan kun skrives bogstaverne fra a til å og tallene fra 0 til 9"));

        }

        [Test]
        public void IllegalSigFindProduct_bananTyped_ErrorEqualsnull()
        {
            _findProduct.ProductName = "banan";
            _findProduct.IllegalSignFindProductCommand.Execute(this);

            Assert.That(_findProduct.Error, Is.EqualTo(null));

        }

        [Test]
        public void IllegalSigFindProduct_kommaTyped_ErrorEqualsString()
        {
            _findProduct.ProductName = "banan,";
            _findProduct.IllegalSignFindProductCommand.Execute(this);

            Assert.That(_findProduct.Error, Is.EqualTo("Der kan kun skrives bogstaverne fra a til å og tallene fra 0 til 9"));

        }

        [Test]
        public void IllegalSigFindProduct_whitespacesTyped_ErrorEqualsnull()
        {
            _findProduct.ProductName = "  ";
            _findProduct.IllegalSignFindProductCommand.Execute(this);

            Assert.That(_findProduct.Error, Is.EqualTo(null));

        }

        [Test]
        public void IllegalSigFindProduct_slashTyped_ErrorEqualsString()
        {
            _findProduct.ProductName = "banan,";
            _findProduct.IllegalSignFindProductCommand.Execute(this);

            Assert.That(_findProduct.Error, Is.EqualTo("Der kan kun skrives bogstaverne fra a til å og tallene fra 0 til 9"));

        }

        [Test]
        public void IllegalSigFindProduct_NumbersTyped_ErrorEqualsnull()
        {
            _findProduct.ProductName = "234567";
            _findProduct.IllegalSignFindProductCommand.Execute(this);

            Assert.That(_findProduct.Error, Is.EqualTo(null));

        }
    }
}
