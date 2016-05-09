using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Consumer;
using Consumer_GUI.User_Controls;
using NSubstitute;
using NUnit.Framework;
using Pristjek220Data;

namespace Pristjek220.Unit.Test
{
    [TestFixture]
    class ShoppingListModelTest
    {

        private Consumer_GUI.User_Controls.ShoppingListModel _shoppingList;


        [SetUp]
        public void SetUp()
        {
            _shoppingList = new ShoppingListModel(Substitute.For<Consumer.IConsumer>(), Substitute.For<IUnitOfWork>());
        }

        [Test]
        public void AddToShoppingList__ProductIsNull_NothingHappens()
        {
            _shoppingList.ShoppingListItem = null;
            _shoppingList.AddToShoppingListCommand.Execute(this);
        }

        [Test]
        public void AddToShoppingList__BananAdd_WriteToFileIsCalled()
        {
            var banan = new ProductInfo("Banan");
            _shoppingList.ShoppingListItem = "Banan";
            _shoppingList.AddToShoppingListCommand.Execute(this);
            _shoppingList.User.Received(1).WriteToJsonFile();
            
        }

        [Test]
        public void AddToShoppingList__BananAdd_ShoppingListDataContainsBanan()
        {
            var banan = new ProductInfo("Banan");
            _shoppingList.ShoppingListItem = "Banan";
            _shoppingList.AddToShoppingListCommand.Execute(this);

            Assert.That(_shoppingList.ShoppingListData.Any(x => x.Name == banan.Name), Is.EqualTo(true));
        }

        [Test]
        public void AddToShoppingList__TwoBananAdd_ShoppingListDataContainsBanan()
        {
            var banan = new ProductInfo("Banan");
            _shoppingList.ShoppingListItem = "Banan";
            _shoppingList.AddToShoppingListCommand.Execute(this);
            _shoppingList.AddToShoppingListCommand.Execute(this);

            Assert.That(_shoppingList.ShoppingListData.Any(x => x.Name == banan.Name), Is.EqualTo(true));
        }

        [Test]
        public void AddToShoppingList__BananAdd_ShoppingListDataNotContainsÆble()
        {
            var banan = new ProductInfo("æble");
            _shoppingList.ShoppingListItem = "Banan";
            _shoppingList.AddToShoppingListCommand.Execute(this);
            _shoppingList.AddToShoppingListCommand.Execute(this);

            Assert.That(_shoppingList.ShoppingListData.Any(x => x.Name == banan.Name), Is.EqualTo(false));
        }

        [Test]
        public void AddToShoppingList_bAnAnAdd_ShoppingListDataContainsBanan()
        {
            var banan = new ProductInfo("bAnAn");
            _shoppingList.ShoppingListItem = "Banan";
            _shoppingList.AddToShoppingListCommand.Execute(this);

            Assert.That(_shoppingList.ShoppingListData.Any(x => x.Name == banan.Name), Is.EqualTo(true));
        }

        [Test]
        public void IllegalSignFindProcuctShoppingList_PunktumTyped_ErrorEqualsString()
        {
            _shoppingList.ShoppingListItem = "banan.";
            _shoppingList.IllegalSignShoppingListCommand.Execute(this);

            Assert.That(_shoppingList.Error, Is.EqualTo("Der kan kun skrives bogstaverne fra a til å og tallene fra 0 til 9"));

        }

        [Test]
        public void IllegalSignFindProcuctShoppingList_bananTyped_ErrorEqualsnull()
        {
            _shoppingList.ShoppingListItem = "banan";
            _shoppingList.IllegalSignShoppingListCommand.Execute(this);

            Assert.That(_shoppingList.Error, Is.EqualTo(string.Empty));

        }

        [Test]
        public void IllegalSignFindProcuctShoppingList_kommaTyped_ErrorEqualsString()
        {
            _shoppingList.ShoppingListItem = "banan,";
            _shoppingList.IllegalSignShoppingListCommand.Execute(this);

            Assert.That(_shoppingList.Error, Is.EqualTo("Der kan kun skrives bogstaverne fra a til å og tallene fra 0 til 9"));

        }

        [Test]
        public void IllegalSignFindProcuctShoppingList_whitespacesTyped_ErrorEqualsnull()
        {
            _shoppingList.ShoppingListItem = "  ";
            _shoppingList.IllegalSignShoppingListCommand.Execute(this);

            Assert.That(_shoppingList.Error, Is.EqualTo(string.Empty));

        }

        [Test]
        public void IllegalSignFindProcuctShoppingList_slashTyped_ErrorEqualsString()
        {
            _shoppingList.ShoppingListItem = "banan,";
            _shoppingList.IllegalSignShoppingListCommand.Execute(this);

            Assert.That(_shoppingList.Error, Is.EqualTo("Der kan kun skrives bogstaverne fra a til å og tallene fra 0 til 9"));

        }

        [Test]
        public void IllegalSignFindProcuctShoppingList_NumbersTyped_ErrorEqualsnull()
        {
            _shoppingList.ShoppingListItem = "234567";
            _shoppingList.IllegalSignShoppingListCommand.Execute(this);

            Assert.That(_shoppingList.Error, Is.EqualTo(string.Empty));

        }

        [Test]
        public void DeleteFromShoppingList_DeleteIsPressedWithNoItemSelected_ErrorEqualsErrorMassage()
        {

            _shoppingList.DeleteFromShoppingListCommand.Execute(this);

            Assert.That(_shoppingList.Error, Is.EqualTo("Du skal markere et produkt, før du kan slette"));

        }

        [Test]
        public void DeleteFromShoppingList_DeleteIsPressedWithNoItemsInList_ErrorEqualsErrorMassage()
        {

            ProductInfo test = new ProductInfo("hej");

            _shoppingList.SelectedItem = test;
            _shoppingList.DeleteFromShoppingListCommand.Execute(this);

            Assert.That(_shoppingList.Error, Is.EqualTo("Der er ikke tilføjet nogen produkter"));
            //Error, skal fixes
        }

        [Test]
        public void DeleteFromShoppingList_DeleteIsPressedWithBananInList_BananRemoved()
        {
            ProductInfo Banan = new ProductInfo("Banan");

            _shoppingList.ShoppingListData.Add(Banan);

            _shoppingList.SelectedItem = Banan;
            _shoppingList.DeleteFromShoppingListCommand.Execute(this);

            Assert.That(_shoppingList.ShoppingListData, Is.Empty);

        }

        [Test]
        public void DeleteFromShoppingList_DeleteIsPressedWithBananAndKiwiInListAndBananSelected_BananRemoved()
        {
           
            ProductInfo Banan = new ProductInfo("Banan");
            ProductInfo Kiwi = new ProductInfo("Kiwi");
            _shoppingList.ShoppingListData.Add(Banan);
            _shoppingList.ShoppingListData.Add(Kiwi);

            _shoppingList.SelectedItem = Banan;
            _shoppingList.DeleteFromShoppingListCommand.Execute(this);

            Assert.That(_shoppingList.ShoppingListData.Any(x => x.Name == Banan.Name), Is.EqualTo(false));

        }

        [Test]
        public void GeneratedShoppingList_GeneratedButtonClicked_CreateShoppingListCalled()
        {
            
            _shoppingList.GeneratedShoppingListCommand.Execute(this);
            _shoppingList.User.Received(1).CreateShoppingList();

        }

        [Test]
        public void GeneratedShoppingList_GeneratedButtonClicked_GeneratedShoppinglistDadaClearCalled()
        {
            _shoppingList.GeneratedShoppingListCommand.Execute(this);
            _shoppingList.User.Received(1).ClearGeneratedShoppingListData();
        }

        [Test]
        public void GeneratedShoppingList_GeneratedButtonClicked_NotInAStoreClearCalled()
        {
            _shoppingList.GeneratedShoppingListCommand.Execute(this);
            _shoppingList.User.Received(1).ClearNotInAStore();
        }
    }
}
