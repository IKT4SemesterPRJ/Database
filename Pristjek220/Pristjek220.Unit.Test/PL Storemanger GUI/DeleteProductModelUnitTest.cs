using System.Collections.Generic;
using System.Threading;
using System.Windows.Forms;
using Administration;
using Administration_GUI;
using Administration_GUI.User_Controls;
using NSubstitute;
using NUnit.Framework;
using Pristjek220Data;
using SharedFunctionalities;

namespace Pristjek220.Unit.Test
{
    [TestFixture]
    class DeleteProductModelUnitTest
    {
        private DeleteProductModel _uut;
        private IStoremanager _storemanager;
        private IAutocomplete _autocomplete;
        private ICreateMsgBox _msgBox;
        private Product _product;

        [SetUp]
        public void SetUp()
        {
            _storemanager = Substitute.For<IStoremanager>();
            _autocomplete = Substitute.For<IAutocomplete>();
            _msgBox = Substitute.For<ICreateMsgBox>();
            _uut = new DeleteProductModel(_storemanager, _autocomplete, _msgBox);
            _product = new Product() { ProductName = "Banan", ProductId = 12 };
        }

        [Test]
        public void DeleteFromStoreDatabase_ShoppingListItemIsEmpty_IsTextConfirmIsFalse()
        {
            _uut.IsTextConfirm = true;
            _uut.ShoppingListItem = "";
            _uut.DeleteFromStoreDatabaseCommand.Execute(this);
            Assert.That(_uut.IsTextConfirm, Is.EqualTo(false));
        }

        [Test]
        public void DeleteFromStoreDatabase_ShoppingListItemIsNull_IsTextConfirmIsFalse()
        {
            _uut.IsTextConfirm = true;
            _uut.ShoppingListItem = null;
            _uut.DeleteFromStoreDatabaseCommand.Execute(this);
            Assert.That(_uut.IsTextConfirm, Is.EqualTo(false));
        }

        [Test]
        public void DeleteFromStoreDatabase_ShoppingListItemIsEmpty_ConfirmTextIsError()
        {
            _uut.ShoppingListItem = "";
            _uut.DeleteFromStoreDatabaseCommand.Execute(this);
            Assert.That(_uut.ConfirmText, Is.EqualTo("Indtast venligst navnet på det produkt der skal fjernes."));
        }

        [Test]
        public void DeleteFromStoreDatabase_ShoppingListItemIsNull_ConfirmTextIsError()
        {
            _uut.ShoppingListItem = null;
            _uut.DeleteFromStoreDatabaseCommand.Execute(this);
            Assert.That(_uut.ConfirmText, Is.EqualTo("Indtast venligst navnet på det produkt der skal fjernes."));
        }

        [Test]
        public void DeleteFromStoreDatabase_ProductAlreadyExistInThatStore_ConfirmTextIsErrorMessage()
        {
            Product nullProduct = null;
            _uut.ShoppingListItem = "Banan";
            _storemanager.FindProduct("Banan").Returns(nullProduct);
            _uut.DeleteFromStoreDatabaseCommand.Execute(this);
            Assert.That(_uut.ConfirmText, Is.EqualTo($"Produktet \"{_uut.ShoppingListItem}\" findes ikke."));
        }

        [Test]
        public void ChangeProductPriceInStoreDatabase_ProductAlreadyExistInStore_ConfirmTextIsThatBananHasBeenRemoved()
        {
            _uut.ShoppingListItem = "Banan";
            _storemanager.FindProduct("Banan").Returns(_product);
            _msgBox.DeleteProductMgsConfirmation(_uut.ShoppingListItem).Returns(DialogResult.Yes);
            _uut.DeleteFromStoreDatabaseCommand.Execute(this);
            Assert.That(_uut.ConfirmText, Is.EqualTo("Produktet \"Banan\" er fjernet fra din forretning."));
        }

        [Test]
        public void ChangeProductPriceInStoreDatabase_ProductAlreadyExistInStore_ConfirmTextIsThatitHasBenDeConfirmed()
        {
            _uut.ShoppingListItem = "Banan";
            _storemanager.FindProduct("Banan").Returns(_product);
            _msgBox.DeleteProductMgsConfirmation(_uut.ShoppingListItem).Returns(DialogResult.No);
            _uut.DeleteFromStoreDatabaseCommand.Execute(this);
            Assert.That(_uut.ConfirmText, Is.EqualTo("Der blev ikke bekræftet."));
        }

        [Test]
        public void ChangeProductPriceInStoreDatabase_ProductdoesnotExistInStore_ConfirmTextIsThatBananDoesNotExist()
        {
            _uut.ShoppingListItem = "Banan";
            _storemanager.FindProduct("Banan").Returns(_product);
            _storemanager.RemoveProductFromMyStore(_product).Returns(1);
            _msgBox.DeleteProductMgsConfirmation(_uut.ShoppingListItem).Returns(DialogResult.Yes);
            _uut.DeleteFromStoreDatabaseCommand.Execute(this);
            Assert.That(_uut.ConfirmText, Is.EqualTo("Produktet \"Banan\" findes ikke i din forretning."));
        }

        [Test]
        public void ChangeProductPriceInStoreDatabase_ProductAlreadyExistInStore_IsTextConfirmIsTrue()
        {
            _uut.IsTextConfirm = false;
            _uut.ShoppingListItem = "Banan";
            _storemanager.FindProduct("Banan").Returns(_product);
            _msgBox.DeleteProductMgsConfirmation(_uut.ShoppingListItem).Returns(DialogResult.Yes);
            _uut.DeleteFromStoreDatabaseCommand.Execute(this);
            Assert.That(_uut.IsTextConfirm, Is.EqualTo(true));
        }

        [Test]
        public void ChangeProductPriceInStoreDatabase_ProductAlreadyExistInStore_IsTextConfirmIsFalse()
        {
            _uut.IsTextConfirm = true;
            _uut.ShoppingListItem = "Banan";
            _storemanager.FindProduct("Banan").Returns(_product);
            _msgBox.DeleteProductMgsConfirmation(_uut.ShoppingListItem).Returns(DialogResult.No);
            _uut.DeleteFromStoreDatabaseCommand.Execute(this);
            Assert.That(_uut.IsTextConfirm, Is.EqualTo(false));
        }

        [Test]
        public void ChangeProductPriceInStoreDatabase_ProductdoesnotExistInStore_IsTextConfirmIsFalse()
        {
            _uut.IsTextConfirm = true;
            _uut.ShoppingListItem = "Banan";
            _storemanager.FindProduct("Banan").Returns(_product);
            _storemanager.RemoveProductFromMyStore(_product).Returns(1);
            _msgBox.DeleteProductMgsConfirmation(_uut.ShoppingListItem).Returns(DialogResult.Yes);
            _uut.DeleteFromStoreDatabaseCommand.Execute(this);
            Assert.That(_uut.IsTextConfirm, Is.EqualTo(false));
        }

        [Test]
        public void IllegalSignDeleteProduct_TestLegalString_TextConfirmIsStillTrue()
        {
            _uut.IsTextConfirm = true;
            _uut.ShoppingListItem = "test";
            _uut.IllegalSignDeleteProductCommand.Execute(this);

            Assert.That(_uut.IsTextConfirm, Is.EqualTo(true));
        }

        [Test]
        public void IllegalSignDeleteProduct_TestIlegalString_TextConfirmIsFalse()
        {
            _uut.IsTextConfirm = true;
            _uut.ShoppingListItem = "test!";
            _uut.IllegalSignDeleteProductCommand.Execute(this);

            Assert.That(_uut.IsTextConfirm, Is.EqualTo(false));
        }

        [Test]
        public void IllegalSignDeleteProductCommand_ConfirmTextIsSet_IsErrorString()
        {
            _uut.ShoppingListItem = "test!";
            _uut.IllegalSignDeleteProductCommand.Execute(this);

            Assert.That(_uut.ConfirmText, Is.EqualTo("Der kan kun skrives bogstaverne fra a til å og tallene fra 0 til 9."));
        }

        [Test]
        public void IllegalSignDeleteProductCommand_ConfirmTextIsSetAndWait5Sec_StringIsEmpty()
        {
            _uut.ShoppingListItem = "test!";
            _uut.IllegalSignDeleteProductCommand.Execute(this);
            Thread.Sleep(5000);

            Assert.That(_uut.ConfirmText, Is.EqualTo(""));
        }

        [Test]
        public void IllegalSignDeleteProductCommand_itemisEmpty_StringIsNull()
        {
            _uut.ShoppingListItem = null;
            _uut.IllegalSignDeleteProductCommand.Execute(this);

            Assert.That(_uut.ConfirmText, Is.EqualTo(null));
        }

        [Test]
        public void PopulatingListDeleteProduct_GetThePopulatingList_ListIsEqualToTheRequestedList()
        {
            var list = new List<string>();
            list.Add("Test1");
            list.Add("Test2");
            _uut.ShoppingListItem = "Test";
            _storemanager.Store = new Store() {StoreName = "TestStore", StoreId = 12};
            _autocomplete.AutoCompleteProductForOneStore("TestStore","Test").Returns(list);
            _uut.PopulatingDeleteProductCommand.Execute(this);

            Assert.That(_uut.AutoCompleteList, Is.EqualTo(list));
        }
    }
}
