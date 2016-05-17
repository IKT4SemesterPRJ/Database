using System.Collections.Generic;
using System.Threading;
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

        [SetUp]
        public void SetUp()
        {
            _storemanager = Substitute.For<IStoremanager>();
            _autocomplete = Substitute.For<IAutocomplete>();
            _msgBox = Substitute.For<ICreateMsgBox>();
            _uut = new DeleteProductModel(_storemanager, _autocomplete, _msgBox);
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
        public void IllegalSignDeleteProduct_TestLegalString_TextConfirmIsStillTrue()
        {
            _uut.IsTextConfirm = true;
            _uut.ShoppingListItem = "test";
            _uut.IllegalSignDeleteProductCommand.Execute(this);

            Assert.That(_uut.IsTextConfirm, Is.EqualTo(true));
        }

        [Test]
        public void IllegalSignDeleteProduct_TestLegalString_TextConfirmIsFalse()
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
