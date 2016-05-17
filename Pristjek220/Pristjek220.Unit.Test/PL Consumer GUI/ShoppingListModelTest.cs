using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms.VisualStyles;
using Consumer;
using Consumer_GUI.User_Controls;
using NSubstitute;
using NUnit.Framework;
using Pristjek220Data;
using SharedFunctionalities;

namespace Pristjek220.Unit.Test
{
    [TestFixture]
    class ShoppingListModelTest
    {

        private ShoppingListModel _uut;
        private IAutocomplete _autoComplete;
        private IConsumer _user;


        [SetUp]
        public void SetUp()
        {
            _autoComplete = Substitute.For<IAutocomplete>();
            _user = Substitute.For<IConsumer>();
            _uut = new ShoppingListModel(_user, _autoComplete);
        }

        [Test]
        public void AddToShoppingList__ProductIsNull_NothingHappens()
        {
            _uut.ShoppingListItem = null;
            _uut.AddToShoppingListCommand.Execute(this);
        }

        [Test]
        public void AddToShoppingList__BananAdd_WriteToFileIsCalled()
        {
            var banan = new ProductInfo("Banan");
            _uut.ShoppingListItem = "Banan";
            _uut.AddToShoppingListCommand.Execute(this);
            _uut.User.Received(1).WriteToJsonFile();
            
        }

        [Test]
        public void AddToShoppingList__BananAdd_ShoppingListDataContainsBanan()
        {
            var banan = new ProductInfo("Banan");
            _uut.ShoppingListItem = "Banan";
            _uut.AddToShoppingListCommand.Execute(this);

            Assert.That(_uut.ShoppingListData.Any(x => x.Name == banan.Name), Is.EqualTo(true));
        }

        [Test]
        public void AddToShoppingList__TwoBananAdd_ShoppingListDataContainsBanan()
        {
            var banan = new ProductInfo("Banan");
            _uut.ShoppingListItem = "Banan";
            _uut.AddToShoppingListCommand.Execute(this);
            _uut.AddToShoppingListCommand.Execute(this);

            Assert.That(_uut.ShoppingListData.Any(x => x.Name == banan.Name), Is.EqualTo(true));
        }

        [Test]
        public void AddToShoppingList__BananAdd_ShoppingListDataNotContainsÆble()
        {
            var banan = new ProductInfo("æble");
            _uut.ShoppingListItem = "Banan";
            _uut.AddToShoppingListCommand.Execute(this);
            _uut.AddToShoppingListCommand.Execute(this);

            Assert.That(_uut.ShoppingListData.Any(x => x.Name == banan.Name), Is.EqualTo(false));
        }

        [Test]
        public void AddToShoppingList_bAnAnAdd_ShoppingListDataContainsBanan()
        {
            var banan = new ProductInfo("bAnAn");
            _uut.ShoppingListItem = "Banan";
            _uut.AddToShoppingListCommand.Execute(this);

            Assert.That(_uut.ShoppingListData.Any(x => x.Name == banan.Name), Is.EqualTo(true));
        }

        [Test]
        public void IllegalSignFindProcuctShoppingList_PunktumTyped_ErrorEqualsString()
        {
            _uut.ShoppingListItem = "banan.";
            _uut.IllegalSignShoppingListCommand.Execute(this);

            Assert.That(_uut.Error, Is.EqualTo("Der kan kun skrives bogstaverne fra a til å og tallene fra 0 til 9."));

        }

        [Test]
        public void IllegalSignFindProcuctShoppingList_bananTyped_ErrorEqualsnull()
        {
            _uut.ShoppingListItem = "banan";
            _uut.IllegalSignShoppingListCommand.Execute(this);

            Assert.That(_uut.Error, Is.EqualTo(string.Empty));

        }

        [Test]
        public void IllegalSignFindProcuctShoppingList_kommaTyped_ErrorEqualsString()
        {
            _uut.ShoppingListItem = "banan,";
            _uut.IllegalSignShoppingListCommand.Execute(this);

            Assert.That(_uut.Error, Is.EqualTo("Der kan kun skrives bogstaverne fra a til å og tallene fra 0 til 9."));

        }

        [Test]
        public void IllegalSignFindProcuctShoppingList_whitespacesTyped_ErrorEqualsnull()
        {
            _uut.ShoppingListItem = "  ";
            _uut.IllegalSignShoppingListCommand.Execute(this);

            Assert.That(_uut.Error, Is.EqualTo(string.Empty));

        }

        [Test]
        public void IllegalSignFindProcuctShoppingList_slashTyped_ErrorEqualsString()
        {
            _uut.ShoppingListItem = "banan,";
            _uut.IllegalSignShoppingListCommand.Execute(this);

            Assert.That(_uut.Error, Is.EqualTo("Der kan kun skrives bogstaverne fra a til å og tallene fra 0 til 9."));

        }

        [Test]
        public void IllegalSignFindProcuctShoppingList_NumbersTyped_ErrorEqualsnull()
        {
            _uut.ShoppingListItem = "234567";
            _uut.IllegalSignShoppingListCommand.Execute(this);

            Assert.That(_uut.Error, Is.EqualTo(string.Empty));

        }

        [Test]
        public void IllegalSignFindProcuctShoppingList_NumbersTyped_IsTextConfirmIsFalse()
        {
            _uut.ShoppingListItem = "234567";
            _uut.IllegalSignShoppingListCommand.Execute(this);

            Assert.That(_uut.IsTextConfirm, Is.EqualTo(false));

        }

        [Test]
        public void DeleteFromShoppingList_DeleteIsPressedWithNoItemSelected_ErrorEqualsErrorMassage()
        {

            _uut.DeleteFromShoppingListCommand.Execute(this);

            Assert.That(_uut.Error, Is.EqualTo("Du skal markere et produkt, før du kan slette."));
        }

        [Test]
        public void DeleteFromShoppingList_DeleteIsPressedWithNoItemsInList_ErrorEqualsErrorMassage()
        {

            ProductInfo test = new ProductInfo("hej");

            _uut.SelectedItem = test;
            _uut.DeleteFromShoppingListCommand.Execute(this);

            Assert.That(_uut.Error, Is.EqualTo("Der er ikke tilføjet nogen produkter."));
            //Error, skal fixes
        }

        [Test]
        public void DeleteFromShoppingList_DeleteIsPressedWithBananInList_BananRemoved()
        {
            ProductInfo Banan = new ProductInfo("Banan");

            _uut.ShoppingListData.Add(Banan);

            _uut.SelectedItem = Banan;
            _uut.DeleteFromShoppingListCommand.Execute(this);

            Assert.That(_uut.ShoppingListData, Is.Empty);

        }

        [Test]
        public void DeleteFromShoppingList_DeleteIsPressedWithBananAndKiwiInListAndBananSelected_BananRemoved()
        {
           
            ProductInfo Banan = new ProductInfo("Banan");
            ProductInfo Kiwi = new ProductInfo("Kiwi");
            _uut.ShoppingListData.Add(Banan);
            _uut.ShoppingListData.Add(Kiwi);

            _uut.SelectedItem = Banan;
            _uut.DeleteFromShoppingListCommand.Execute(this);

            Assert.That(_uut.ShoppingListData.Any(x => x.Name == Banan.Name), Is.EqualTo(false));

        }

        [Test]
        public void GeneratedShoppingList_GeneratedButtonClicked_CreateShoppingListCalled()
        {
            
            _uut.GeneratedShoppingListCommand.Execute(this);
            _uut.User.Received(1).CreateShoppingList();

        }

        [Test]
        public void GeneratedShoppingList_GeneratedButtonClicked_GeneratedShoppinglistDadaClearCalled()
        {
            _uut.GeneratedShoppingListCommand.Execute(this);
            _uut.User.Received(1).ClearGeneratedShoppingListData();
        }

        [Test]
        public void GeneratedShoppingList_GeneratedButtonClicked_NotInAStoreClearCalled()
        {
            _uut.GeneratedShoppingListCommand.Execute(this);
            _uut.User.Received(1).ClearNotInAStore();
        }

        [Test]
        public void Error_DeleteIsPressedWithNoItemSelectedWait5Sec_ErrorIsEmpty()
        {
            _uut.DeleteFromShoppingListCommand.Execute(this);
            Thread.Sleep(5000);

            Assert.That(_uut.Error, Is.EqualTo(""));
        }

        [Test]
        public void PopulatingListShoppingList_GetThePopulatingList_ListIsEqualToTheRequestedList()
        {
            var list = new List<string>();
            list.Add("Test");
            _uut.ShoppingListItem = "Test";
            _autoComplete.AutoCompleteProduct("Test").Returns(list);
            _uut.PopulatingShoppingListCommand.Execute(this);

            Assert.That(_uut.AutoCompleteList, Is.EqualTo(list));
        }

        [Test]
        public void ClearShoppingList_AddProductsToListThenClear_ListIsEmpty()
        {
            _uut.ShoppingListData.Add(new ProductInfo("Test"));
            _uut.ShoppingListData.Add(new ProductInfo("Test1"));
            _uut.ClearShoppingListCommand.Execute(this);


            Assert.That(_uut.ShoppingListData, Is.EqualTo(new ObservableCollection<ProductInfo>()));
        }

        [Test]
        public void OptionsStores_SetAndGetOptionsStore_IsEqualToTheSetValue()
        {
            _user.OptionsStores = new ObservableCollection<StoresInPristjek>();
            _uut.OptionsStores.Add(new StoresInPristjek("TestButik"));
            
            Assert.That(_uut.OptionsStores[0].Store, Is.EqualTo("TestButik"));
        }
    }
}
