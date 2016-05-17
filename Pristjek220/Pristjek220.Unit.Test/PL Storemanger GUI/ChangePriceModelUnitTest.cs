using System.Collections.Generic;
using System.Threading;
using Administration;
using Administration_GUI.User_Controls;
using NSubstitute;
using NUnit.Framework;
using Pristjek220Data;
using SharedFunctionalities;

namespace Pristjek220.Unit.Test
{
    [TestFixture]
    class ChangePriceModelUnitTest
    {
        private ChangePriceModel _uut;
        private IStoremanager _storemanager;
        private IAutocomplete _autocomplete;
        private ProductAndPrice _productAndPrice = new ProductAndPrice();

        [SetUp]
        public void SetUp()
        {
            _storemanager = Substitute.For<IStoremanager>();
            _autocomplete = Substitute.For<IAutocomplete>();
            _uut = new ChangePriceModel(_storemanager, _autocomplete);
            _productAndPrice.Name = "Banan";
            _productAndPrice.Price = 12;
        }

        [Test]
        public void ChangeProductPriceInStoreDatabase_ShoppingListItemIsEmpty_IsTextConfirmIsFalse()
        {
            _uut.IsTextConfirm = true;
            _uut.ShoppingListItem = "";
            _uut.ChangeProductPriceInStoreDatabaseCommand.Execute(this);
            Assert.That(_uut.IsTextConfirm, Is.EqualTo(false));
        }

        [Test]
        public void ChangeProductPriceInStoreDatabase_ShoppingListItemIsNull_IsTextConfirmIsFalse()
        {
            _uut.IsTextConfirm = true;
            _uut.ShoppingListItem = null;
            _uut.ChangeProductPriceInStoreDatabaseCommand.Execute(this);
            Assert.That(_uut.IsTextConfirm, Is.EqualTo(false));
        }

        [Test]
        public void ChangeProductPriceInStoreDatabase_ShoppingListItemIsEmpty_ConfirmTextIsError()
        {
            _uut.ShoppingListItem = "";
            _uut.ChangeProductPriceInStoreDatabaseCommand.Execute(this);
            Assert.That(_uut.ConfirmText, Is.EqualTo("Indtast venligst navnet på det produkt hvis pris skal ændres."));
        }

        [Test]
        public void ChangeProductPriceInStoreDatabase_ShoppingListItemIsNull_ConfirmTextIsError()
        {
            _uut.ShoppingListItem = null;
            _uut.ChangeProductPriceInStoreDatabaseCommand.Execute(this);
            Assert.That(_uut.ConfirmText, Is.EqualTo("Indtast venligst navnet på det produkt hvis pris skal ændres."));
        }

        [Test]
        public void ChangeProductPriceInStoreDatabase_ProductAlreadyExistInThatStore_ConfirmTextIsErrorMessage()
        {
            Product nullProduct = null;
            _uut.ShoppingListItem = "Banan";
            _storemanager.FindProduct("Banan").Returns(nullProduct);
            _uut.ChangeProductPriceInStoreDatabaseCommand.Execute(this);
            Assert.That(_uut.ConfirmText, Is.EqualTo("Prisen er ugyldig."));
        }

        [Test]
        public void ChangeProductPriceInStoreDatabase_ProductAlreadyExistInThatStore_IsTextConfirmIsFalse()
        {
            _uut.IsTextConfirm = true;
            Product nullProduct = null;
            _uut.ShoppingListItem = "Banan";
            _storemanager.FindProduct("Banan").Returns(nullProduct);
            _uut.ChangeProductPriceInStoreDatabaseCommand.Execute(this);
            Assert.That(_uut.IsTextConfirm, Is.EqualTo(false));
        }

        [Test]
        public void ChangeProductPriceInStoreDatabase_ProductAlreadyExistInThatStoresa_IsTextConfirmIsFalse()
        {
            _uut.IsTextConfirm = true;
            _uut.ShoppingListItemPrice = "12";
            ProductAndPrice nullProduct = null;
            _uut.ShoppingListItem = "Banan";
            _storemanager.FindProductInStore("Banan").Returns(nullProduct);
            _uut.ChangeProductPriceInStoreDatabaseCommand.Execute(this);
            Assert.That(_uut.IsTextConfirm, Is.EqualTo(false));
        }

        [Test]
        public void ChangeProductPriceInStoreDatabase_ProductAlreadyExistInThatStoresaas_IsTextConfirmIsFalse()
        {
            _uut.ShoppingListItemPrice = "12";
            ProductAndPrice nullProduct = null;
            _uut.ShoppingListItem = "Banan";
            _storemanager.FindProductInStore("Banan").Returns(nullProduct);
            _uut.ChangeProductPriceInStoreDatabaseCommand.Execute(this);
            Assert.That(_uut.ConfirmText, Is.EqualTo("Produktet \"Banan\" findes ikke i din forretning."));
        }

        [Test]
        public void IllegalSignDeleteProduct_TestLegalString_TextConfirmIsStillTrue()
        {
            _uut.IsTextConfirm = true;
            _uut.ShoppingListItem = "test";
            _uut.IllegalSignChangePriceCommand.Execute(this);

            Assert.That(_uut.IsTextConfirm, Is.EqualTo(true));
        }

        [Test]
        public void IllegalSignDeleteProduct_TestLegalString_TextConfirmIsFalse()
        {
            _uut.IsTextConfirm = true;
            _uut.ShoppingListItem = "test!";
            _uut.IllegalSignChangePriceCommand.Execute(this);

            Assert.That(_uut.IsTextConfirm, Is.EqualTo(false));
        }

        [Test]
        public void IllegalSignDeleteProductCommand_ConfirmTextIsSet_IsErrorString()
        {
            _uut.ShoppingListItem = "test!";
            _uut.IllegalSignChangePriceCommand.Execute(this);

            Assert.That(_uut.ConfirmText, Is.EqualTo("Der kan kun skrives bogstaverne fra a til å og tallene fra 0 til 9."));
        }

        [Test]
        public void IllegalSignDeleteProductCommand_ConfirmTextIsSetAndWait5Sec_StringIsEmpty()
        {
            _uut.ShoppingListItem = "test!";
            _uut.IllegalSignChangePriceCommand.Execute(this);
            Thread.Sleep(5000);

            Assert.That(_uut.ConfirmText, Is.EqualTo(""));
        }

        [Test]
        public void IllegalSignDeleteProductCommand_itemisEmpty_StringIsNull()
        {
            _uut.ShoppingListItem = null;
            _uut.IllegalSignChangePriceCommand.Execute(this);

            Assert.That(_uut.ConfirmText, Is.EqualTo(null));
        }

        [Test]
        public void PopulatingListDeleteProduct_GetThePopulatingList_ListIsEqualToTheRequestedList()
        {
            var list = new List<string>();
            list.Add("Test1");
            list.Add("Test2");
            _uut.ShoppingListItem = "Test";
            _storemanager.Store = new Store() { StoreName = "TestStore", StoreId = 12 };
            _autocomplete.AutoCompleteProductForOneStore("TestStore", "Test").Returns(list);
            _uut.PopulatingChangePriceProductCommand.Execute(this);

            Assert.That(_uut.AutoCompleteList, Is.EqualTo(list));
        }
    }
}
