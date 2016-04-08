using System;
using System.Collections.Generic;
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
            _shoppingList = new ShoppingListModel(Substitute.For<Consumer.IConsumer>());
        }

        [Test]
        public void AddToShoppingList__ProductIsNull_NothingHappens()
        {
            _shoppingList.ShoppingListItem = null;
            _shoppingList.AddToShoppingListCommand.Execute(this);
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
        public void AddToShoppingList__bAnAnAdd_ShoppingListDataContainsBanan()
        {
            var banan = new ProductInfo("bAnAn");
            _shoppingList.ShoppingListItem = "Banan";
            _shoppingList.AddToShoppingListCommand.Execute(this);

            Assert.That(_shoppingList.ShoppingListData.Any(x => x.Name == banan.Name), Is.EqualTo(true));
        }

    }
}
