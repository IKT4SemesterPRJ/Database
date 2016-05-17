using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Consumer;
using Consumer_GUI.User_Controls;
using NSubstitute;
using NSubstitute.ReturnsExtensions;
using NUnit.Framework;
using Pristjek220Data;
using SharedFunctionalities;


namespace Pristjek220.Unit.Test
{
    [TestFixture]
    class FindProductModelTest
    {
        
        private Consumer_GUI.User_Controls.FindProductModel _findProduct;
        private IAutocomplete _autoComplete;
        private IUnitOfWork _unit;
        private IConsumer _user;


        [SetUp]
        public void SetUp()
        {
            _unit = Substitute.For<IUnitOfWork>();
            _autoComplete = Substitute.For<IAutocomplete>();
            _user = Substitute.For<IConsumer>();
            _findProduct = new FindProductModel(_user, _autoComplete);
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
            Assert.That(_findProduct.Error, Is.EqualTo($"Produktet \"{_findProduct.ProductName}\" findes ikke i Pristjek220."));   
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
        public void AddToStoreList_isEmpty_ErrorIsErrorMessage()
        {
            _findProduct.ProductName = string.Empty;
            _findProduct.AddToStoreListCommand.Execute(this);
            Assert.That(_findProduct.Error, Is.EqualTo("Indtast venligst det produkt du vil søge efter."));
        }

        [Test]
        public void AddToStoreList_isNull_ErrorIsErrorMessage()
        {
            _findProduct.ProductName = null;
            _findProduct.AddToStoreListCommand.Execute(this);
            Assert.That(_findProduct.Error, Is.EqualTo("Indtast venligst det produkt du vil søge efter."));
        }

        [Test]
        public void IllegalSigFindProduct_PunktumTyped_ErrorEqualsString()
        {
            _findProduct.ProductName = "banan.";
            _findProduct.IllegalSignFindProductCommand.Execute(this);

            Assert.That(_findProduct.Error, Is.EqualTo("Der kan kun skrives bogstaverne fra a til å og tallene fra 0 til 9."));

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

            Assert.That(_findProduct.Error, Is.EqualTo("Der kan kun skrives bogstaverne fra a til å og tallene fra 0 til 9."));

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

            Assert.That(_findProduct.Error, Is.EqualTo("Der kan kun skrives bogstaverne fra a til å og tallene fra 0 til 9."));
        }

        [Test]
        public void IllegalSigFindProduct_NumbersTyped_ErrorEqualsnull()
        {
            _findProduct.ProductName = "234567";
            _findProduct.IllegalSignFindProductCommand.Execute(this);

            Assert.That(_findProduct.Error, Is.EqualTo(null));

        }

        [Test]
        public void Error_commadTypedWait5sekToSeeErrorGetsCleared_ErrorEqualsEmptyString()
        {
            _findProduct.ProductName = "banan,";
            _findProduct.IllegalSignFindProductCommand.Execute(this);
            Thread.Sleep(5000);

            Assert.That(_findProduct.Error, Is.EqualTo(""));
        }

        [Test]
        public void IllegalSignFindProcuctShoppingList_NumbersTyped_IsTextConfirmIsFalse()
        {
            _findProduct.ProductName = "test";
            _findProduct.IllegalSignFindProductCommand.Execute(this);

            Assert.That(_findProduct.IsTextConfirm, Is.EqualTo(false));

        }

        [Test]
        public void PopulatingListShoppingList_GetThePopulatingList_ListIsEqualToTheRequestedList()
        {
            var list = new List<string>();
            list.Add("Test");
            _findProduct.ProductName = "Test";
            _autoComplete.AutoCompleteProduct("Test").Returns(list);
            _findProduct.PopulatingFindProductCommand.Execute(this);
            

            Assert.That(_findProduct.AutoCompleteList, Is.EqualTo(list));
        }
    }
}
