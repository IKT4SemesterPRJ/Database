using System.Collections.Generic;
using System.Threading;
using System.Windows.Forms;
using Administration;
using Administration_GUI;
using Administration_GUI.User_Controls_Admin;
using NSubstitute;
using NUnit.Framework;
using Pristjek220Data;
using SharedFunctionalities;

namespace Pristjek220.Unit.Test
{
    [TestFixture]
    class AdminDeleteStoreModelUnitTest
    {
        private AdminDeleteStoreModel _uut;
        private IAdmin _admin;
        private IAutocomplete _autocomplete;
        private ICreateMsgBox _msgBox;
        private Store _store;

        [SetUp]
        public void SetUp()
        {
            _admin = Substitute.For<IAdmin>();
            _autocomplete = Substitute.For<IAutocomplete>();
            _msgBox = Substitute.For<ICreateMsgBox>();
            _uut = new AdminDeleteStoreModel(_admin, _autocomplete, _msgBox);
            _store = new Store() {StoreId = 12, StoreName = "Lidl"};
        }

        [Test]
        public void DeleteStore_DeleteStoreNameIsEmpty_IsTextConfirmIsFalse()
        {
            _uut.IsTextConfirm = true;
            _uut.DeleteStoreName = "";
            _uut.DeleteFromLoginDatabaseCommand.Execute(this);
            Assert.That(_uut.IsTextConfirm, Is.EqualTo(false));
        }

        [Test]
        public void DeleteStore_DeleteStoreNameIsNull_IsTextConfirmIsFalse()
        {
            _uut.IsTextConfirm = true;
            _uut.DeleteStoreName = null;
            _uut.DeleteFromLoginDatabaseCommand.Execute(this);
            Assert.That(_uut.IsTextConfirm, Is.EqualTo(false));
        }

        [Test]
        public void DeleteStore_DeleteStoreNameIsEmpty_ErrorIsError()
        {
            _uut.DeleteStoreName = "";
            _uut.DeleteFromLoginDatabaseCommand.Execute(this);
            Assert.That(_uut.Error, Is.EqualTo("Indtast venligst navnet på den forretning der skal fjernes."));
        }

        [Test]
        public void DeleteStore_DeleteStoreNameIsNull_ErrorIsError()
        {
            _uut.DeleteStoreName = null;
            _uut.DeleteFromLoginDatabaseCommand.Execute(this);
            Assert.That(_uut.Error, Is.EqualTo("Indtast venligst navnet på den forretning der skal fjernes."));
        }

        [Test]
        public void DeleteStore_StoreAlreadyExistInThatStore_ErrorIsErrorMessage()
        {
            Store nullStore = null;
            _uut.DeleteStoreName = "Lidl";
            _admin.FindStore("Lidl").Returns(nullStore);
            _uut.DeleteFromLoginDatabaseCommand.Execute(this);
            Assert.That(_uut.Error, Is.EqualTo($"Forretningen \"Lidl\" findes ikke i Pristjek220."));
        }

        [Test]
        public void DeleteStore_StoreAlreadyExistInStoreConfirm_ErrorIsThatBananHasBeenRemoved()
        {
            _uut.DeleteStoreName = "Lidl";
            _admin.FindStore("Lidl").Returns(_store);
            _msgBox.DeleteStoreMgsConfirmation(_uut.DeleteStoreName).Returns(DialogResult.Yes);
            _uut.DeleteFromLoginDatabaseCommand.Execute(this);
            Assert.That(_uut.Error, Is.EqualTo("Forretningen \"Lidl\" er blevet fjernet fra Pristjek220."));
        }

        [Test]
        public void DeleteStore_StoreAlreadyExistInStoreDeConfirm_ErrorIsThatitHasBenDeConfirmed()
        {
            _uut.DeleteStoreName = "Lidl";
            _admin.FindStore("Lidl").Returns(_store);
            _msgBox.DeleteStoreMgsConfirmation(_uut.DeleteStoreName).Returns(DialogResult.No);
            _uut.DeleteFromLoginDatabaseCommand.Execute(this);
            Assert.That(_uut.Error, Is.EqualTo("Der blev ikke bekræftet."));
        }

        [Test]
        public void DeleteStore_StoreAlreadyExistInStoreConfirm_IsTextConfirmIsTrue()
        {
            _uut.IsTextConfirm = false;
            _uut.DeleteStoreName = "Lidl";
            _admin.FindStore("Lidl").Returns(_store);
            _msgBox.DeleteStoreMgsConfirmation(_uut.DeleteStoreName).Returns(DialogResult.Yes);
            _uut.DeleteFromLoginDatabaseCommand.Execute(this);
            Assert.That(_uut.IsTextConfirm, Is.EqualTo(true));
        }

        [Test]
        public void DeleteStore_StoreAlreadyExistInStoreDeConfirm_IsTextConfirmIsFalse()
        {
            _uut.IsTextConfirm = true;
            _uut.DeleteStoreName = "Lidl";
            _admin.FindStore("Lidl").Returns(_store);
            _msgBox.DeleteStoreMgsConfirmation(_uut.DeleteStoreName).Returns(DialogResult.No);
            _uut.DeleteFromLoginDatabaseCommand.Execute(this);
            Assert.That(_uut.IsTextConfirm, Is.EqualTo(false));
        }

        [Test]
        public void IllegalSignDeleteStore_TestLegalString_TextConfirmIsStillTrue()
        {
            _uut.IsTextConfirm = true;
            _uut.DeleteStoreName = "test";
            _uut.IllegalSignDeleteStoreCommand.Execute(this);

            Assert.That(_uut.IsTextConfirm, Is.EqualTo(true));
        }

        [Test]
        public void IllegalSignDeleteStore_TestIlegalString_TextConfirmIsFalse()
        {
            _uut.IsTextConfirm = true;
            _uut.DeleteStoreName = "test!";
            _uut.IllegalSignDeleteStoreCommand.Execute(this);

            Assert.That(_uut.IsTextConfirm, Is.EqualTo(false));
        }

        [Test]
        public void IllegalSignDeleteStore_ErrorIsSet_IsErrorString()
        {
            _uut.DeleteStoreName = "test!";
            _uut.IllegalSignDeleteStoreCommand.Execute(this);

            Assert.That(_uut.Error, Is.EqualTo("Der kan kun skrives bogstaverne fra a til å og tallene fra 0 til 9."));
        }

        [Test]
        public void IllegalSignDeleteStore_ErrorIsSetAndWait5Sec_StringIsEmpty()
        {
            _uut.DeleteStoreName = "test!";
            _uut.IllegalSignDeleteStoreCommand.Execute(this);
            Thread.Sleep(5000);

            Assert.That(_uut.Error, Is.EqualTo(""));
        }

        [Test]
        public void IllegalSignDeleteStore_itemisEmpty_StringIsNull()
        {
            _uut.DeleteStoreName = null;
            _uut.IllegalSignDeleteStoreCommand.Execute(this);

            Assert.That(_uut.Error, Is.EqualTo(""));
        }

        [Test]
        public void PopulatingListDeleteStore_GetThePopulatingList_ListIsEqualToTheRequestedList()
        {
            var list = new List<string>();
            list.Add("Test1");
            list.Add("Test2");
            _uut.DeleteStoreName = "Test";
            _autocomplete.AutoCompleteStore("Test").Returns(list);
            _uut.PopulatingDeleteStoreCommand.Execute(this);

            Assert.That(_uut.AutoCompleteList, Is.EqualTo(list));
        }
    }
}
