using System.Collections.Generic;
using System.Globalization;
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
    class NewProductModelUnitTest
    {
        private NewProductModel _uut;
        private IAutocomplete _autocomplete;
        private IStoremanager _storemanager;
        private Product _product = new Product();
        private ProductAndPrice _productAndPrice = new ProductAndPrice();

        [SetUp]
        public void SetUp()
        {
            _autocomplete = Substitute.For<IAutocomplete>();
            _storemanager = Substitute.For<IStoremanager>();

            _uut = new NewProductModel(_storemanager, _autocomplete);
            _product.ProductName = "Banan";
            _product.ProductId = 12;
            _productAndPrice.Name = "Banan";
            _productAndPrice.Price = 12;
        }

        [TestCase("2,224", "2,22")]
        [TestCase("2,226", "2,23")]
        public void ShoppingListItemPrice_Insert2point224_IsRoundedTo2point22(string price, string result)
        {
            _uut.ShoppingListItemPrice = double.Parse(price, new CultureInfo("da-DK")).ToString();
            Assert.That(_uut.ShoppingListItemPrice, Is.EqualTo(double.Parse(result, new CultureInfo("da-DK")).ToString()));
        }

        [Test]
        public void AddToStoreDatabase_ShoppingListItemIsEmpty_IsTextConfirmIsFalse()
        {
            _uut.IsTextConfirm = true;
            _uut.ShoppingListItem = "";
            _uut.AddToStoreDatabaseCommand.Execute(this);
            Assert.That(_uut.IsTextConfirm, Is.EqualTo(false));
        }

        [Test]
        public void AddToStoreDatabase_ShoppingListItemIsNull_IsTextConfirmIsFalse()
        {
            _uut.IsTextConfirm = true;
            _uut.ShoppingListItem = null;
            _uut.AddToStoreDatabaseCommand.Execute(this);
            Assert.That(_uut.IsTextConfirm, Is.EqualTo(false));
        }

        [Test]
        public void AddToStoreDatabase_ShoppingListItemIsEmpty_ConfirmTextIsError()
        {
            _uut.ShoppingListItem = "";
            _uut.AddToStoreDatabaseCommand.Execute(this);
            Assert.That(_uut.ConfirmText, Is.EqualTo("Indtast venligst navnet på det produkt der skal tilføjes."));
        }

        [Test]
        public void AddToStoreDatabase_ShoppingListItemIsNull_ConfirmTextIsError()
        {
            _uut.ShoppingListItem = null;
            _uut.AddToStoreDatabaseCommand.Execute(this);
            Assert.That(_uut.ConfirmText, Is.EqualTo("Indtast venligst navnet på det produkt der skal tilføjes."));
        }

        [Test]
        public void AddToStoreDatabase_ShoppingListItemPriceIsEmpty_ConfirmTextIsError()
        {
            _uut.ShoppingListItem = "Banan";
            _uut.ShoppingListItemPrice = "";
            _uut.AddToStoreDatabaseCommand.Execute(this);
            Assert.That(_uut.ConfirmText, Is.EqualTo("Prisen er ugyldig."));
        }

        [Test]
        public void AddToStoreDatabase_ShoppingListItemPriceIsNull_ConfirmTextIsError()
        {
            _uut.ShoppingListItem = "Banan";
            _uut.ShoppingListItemPrice = null;
            _uut.AddToStoreDatabaseCommand.Execute(this);
            Assert.That(_uut.ConfirmText, Is.EqualTo("Prisen er ugyldig."));
        }

        [Test]
        public void AddToStoreDatabase_ShoppingListItemPriceIsEmpty_IsTextConfirmIsFalse()
        {
            _uut.IsTextConfirm = true;
            _uut.ShoppingListItem = "Banan";
            _uut.ShoppingListItemPrice = "";
            _uut.AddToStoreDatabaseCommand.Execute(this);
            Assert.That(_uut.IsTextConfirm, Is.EqualTo(false));
        }

        [Test]
        public void AddToStoreDatabase_ShoppingListItemPriceIsNull_IsTextConfirmIsFalse()
        {
            _uut.IsTextConfirm = true;
            _uut.ShoppingListItem = "Banan";
            _uut.ShoppingListItemPrice = null;
            _uut.AddToStoreDatabaseCommand.Execute(this);
            Assert.That(_uut.IsTextConfirm, Is.EqualTo(false));
        }

        [Test]
        public void AddToStoreDatabase_ProductAlreadyExistInThatStore_ConfirmTextIsErrorMessage()
        {
            Product nullProduct = null;
            _uut.ShoppingListItem = "Banan";
            _uut.ShoppingListItemPrice = "12";
            _storemanager.FindProduct("Banan").Returns(nullProduct, _product);
            _storemanager.FindProductInStore("Banan").Returns(_productAndPrice);
            _uut.AddToStoreDatabaseCommand.Execute(this);
            Assert.That(_uut.ConfirmText, Is.EqualTo($"Produktet \"{_productAndPrice.Name}\" findes allerede."));
        }

        [Test]
        public void IllegalSignNewProduct_TestLegalString_TextConfirmIsStillTrue()
        {
            _uut.IsTextConfirm = true;
            _uut.ShoppingListItem = "test";
            _uut.IllegalSignNewProductCommand.Execute(this);

            Assert.That(_uut.IsTextConfirm, Is.EqualTo(true));
        }

        [Test]
        public void IllegalSignNewProduct_TestLegalString_TextConfirmIsFalse()
        {
            _uut.IsTextConfirm = true;
            _uut.ShoppingListItem = "test!";
            _uut.IllegalSignNewProductCommand.Execute(this);

            Assert.That(_uut.IsTextConfirm, Is.EqualTo(false));
        }

        [Test]
        public void IllegalSignNewProductCommand_ConfirmTextIsSet_IsErrorString()
        {
            _uut.ShoppingListItem = "test!";
            _uut.IllegalSignNewProductCommand.Execute(this);

            Assert.That(_uut.ConfirmText, Is.EqualTo("Der kan kun skrives bogstaverne fra a til å og tallene fra 0 til 9."));
        }

        [Test]
        public void IllegalSignNewProductCommand_ConfirmTextIsSetAndWait5Sec_StringIsEmpty()
        {
            _uut.ShoppingListItem = "test!";
            _uut.IllegalSignNewProductCommand.Execute(this);
            Thread.Sleep(5000);

            Assert.That(_uut.ConfirmText, Is.EqualTo(""));
        }

        [Test]
        public void IllegalSignNewProductCommand_itemisEmpty_StringIsNull()
        {
            _uut.ShoppingListItem = null;
            _uut.IllegalSignNewProductCommand.Execute(this);

            Assert.That(_uut.ConfirmText, Is.EqualTo(null));
        }

        [Test]
        public void PopulatingListNewProduct_GetThePopulatingList_ListIsEqualToTheRequestedList()
        {
            var list = new List<string>();
            list.Add("Test1");
            list.Add("Test2");
            _uut.ShoppingListItem = "Test";
            _autocomplete.AutoCompleteProduct("Test").Returns(list);
            _uut.PopulatingNewProductCommand.Execute(this);


            Assert.That(_uut.AutoCompleteList, Is.EqualTo(list));
        }

    }
}
