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
    class FindProductModelUnitTest
    {
        
        private FindProductModel _uut;
        private IAutocomplete _autoComplete;

        private IConsumer _user;


        [SetUp]
        public void SetUp()
        {
            _autoComplete = Substitute.For<IAutocomplete>();
            _user = Substitute.For<IConsumer>();
            _uut = new FindProductModel(_user, _autoComplete);
        }

        [Test]
        public void AddToStoreList_WithBananAsProductname_IsFindStoresThatSellsProductCalled()
        {
            _uut.ProductName = "Banan";
            _uut.User.FindStoresThatSellsProduct(_uut.ProductName).Returns(new List<StoreAndPrice>());
            _uut.AddToStoreListCommand.Execute(this);
            _uut.User.Received(1).FindStoresThatSellsProduct(_uut.ProductName);
        }

        [Test]
        public void AddToStoreList_ProductDoesNotExist_MessageBoxIsShown()
        {
            _uut.ProductName = "Banan";
            _uut.User.FindStoresThatSellsProduct(_uut.ProductName).Returns(new List<StoreAndPrice>());
            _uut.AddToStoreListCommand.Execute(this);
            Assert.That(_uut.Error, Is.EqualTo($"Produktet \"{_uut.ProductName}\" findes ikke i Pristjek220."));   
        }

        [Test]
        public void AddToStoreList_BananIsAddedToList_AldiAnd8IsInTheList()
        {
            List<StoreAndPrice> list = new List<StoreAndPrice>();
            var storeandprice = new StoreAndPrice();
            storeandprice.Name = "Aldi";
            storeandprice.Price = 8;
            list.Add(storeandprice);
            _uut.ProductName = "Banan";
            _uut.User.FindStoresThatSellsProduct(_uut.ProductName).Returns(list);
            _uut.AddToStoreListCommand.Execute(this);
            Assert.That(_uut.StorePrice.Contains(storeandprice), Is.EqualTo(true));
            
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
            _uut.ProductName = "Banan";
            _uut.User.FindStoresThatSellsProduct(_uut.ProductName).Returns(list);
            _uut.AddToStoreListCommand.Execute(this);
            Assert.That(_uut.StorePrice.Contains(storeandprice), Is.EqualTo(true));
            Assert.That(_uut.StorePrice.Contains(storeandprice1), Is.EqualTo(true));
        }

        [Test]
        public void AddToStoreList_isEmpty_ErrorIsErrorMessage()
        {
            _uut.ProductName = string.Empty;
            _uut.AddToStoreListCommand.Execute(this);
            Assert.That(_uut.Error, Is.EqualTo("Indtast venligst det produkt du vil søge efter."));
        }

        [Test]
        public void AddToStoreList_isNull_ErrorIsErrorMessage()
        {
            _uut.ProductName = null;
            _uut.AddToStoreListCommand.Execute(this);
            Assert.That(_uut.Error, Is.EqualTo("Indtast venligst det produkt du vil søge efter."));
        }

        [Test]
        public void IllegalSigFindProduct_PunktumTyped_ErrorEqualsString()
        {
            _uut.ProductName = "banan.";
            _uut.IllegalSignFindProductCommand.Execute(this);

            Assert.That(_uut.Error, Is.EqualTo("Der kan kun skrives bogstaverne fra a til å og tallene fra 0 til 9."));

        }

        [Test]
        public void IllegalSigFindProduct_bananTyped_ErrorEqualsnull()
        {
            _uut.ProductName = "banan";
            _uut.IllegalSignFindProductCommand.Execute(this);

            Assert.That(_uut.Error, Is.EqualTo(null));

        }

        [Test]
        public void IllegalSigFindProduct_kommaTyped_ErrorEqualsString()
        {
            _uut.ProductName = "banan,";
            _uut.IllegalSignFindProductCommand.Execute(this);

            Assert.That(_uut.Error, Is.EqualTo("Der kan kun skrives bogstaverne fra a til å og tallene fra 0 til 9."));

        }

        [Test]
        public void IllegalSigFindProduct_whitespacesTyped_ErrorEqualsnull()
        {
            _uut.ProductName = "  ";
            _uut.IllegalSignFindProductCommand.Execute(this);

            Assert.That(_uut.Error, Is.EqualTo(null));

        }

        [Test]
        public void IllegalSigFindProduct_slashTyped_ErrorEqualsString()
        {
            _uut.ProductName = "banan,";
            _uut.IllegalSignFindProductCommand.Execute(this);

            Assert.That(_uut.Error, Is.EqualTo("Der kan kun skrives bogstaverne fra a til å og tallene fra 0 til 9."));
        }

        [Test]
        public void IllegalSigFindProduct_NumbersTyped_ErrorEqualsnull()
        {
            _uut.ProductName = "234567";
            _uut.IllegalSignFindProductCommand.Execute(this);

            Assert.That(_uut.Error, Is.EqualTo(null));

        }

        [Test]
        public void Error_commadTypedWait5sekToSeeErrorGetsCleared_ErrorEqualsEmptyString()
        {
            _uut.ProductName = "banan,";
            _uut.IllegalSignFindProductCommand.Execute(this);
            Thread.Sleep(5000);

            Assert.That(_uut.Error, Is.EqualTo(""));
        }

        [Test]
        public void IllegalSignFindProcuctShoppingList_NumbersTyped_IsTextConfirmIsFalse()
        {
            _uut.ProductName = "test";
            _uut.IllegalSignFindProductCommand.Execute(this);

            Assert.That(_uut.IsTextConfirm, Is.EqualTo(false));
        }

        [Test]
        public void PopulatingListShoppingList_GetThePopulatingList_ListIsEqualToTheRequestedList()
        {
            var list = new List<string>();
            list.Add("Test");
            _uut.ProductName = "Test";
            _autoComplete.AutoCompleteProduct("Test").Returns(list);
            _uut.PopulatingFindProductCommand.Execute(this);
            

            Assert.That(_uut.AutoCompleteList, Is.EqualTo(list));
        }
    }
}
