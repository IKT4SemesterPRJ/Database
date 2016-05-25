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
using SharedFunctionalities;
using Consumer = Consumer.Consumer;

namespace Pristjek220.Integrationstest
{
    [TestFixture]
    class ConsumerGuiIntergrationsTest
    {
        private GeneratedShoppingListModel _generatedShoppingList;
        private FindProductModel _findProduct;
        private ShoppingListModel _shoppingList;
        private IUnitOfWork _unit;
        private DataContext _context;
        private IConsumer _consumer;
        private IAutocomplete _autocomplete;
        private IMail _mail;
        private Product _product;
        private Product _product2;



        [SetUp]
        public void SetUp()
        {
            _context = new DataContext();
            _unit = new UnitOfWork(_context);

            _context.Database.Connection.ConnectionString =
                "Server=.\\SQLEXPRESS;Database=Pristjek220Data.DataContext; Trusted_Connection=True;MultipleActiveResultSets=True;";
            _context.Database.ExecuteSqlCommand("dbo.TestCleanTable");
            

            var store = new Store() {StoreName = "TestStore"};
            var store2 = new Store() { StoreName = "TestStore2" };
            _context.Stores.Add(store);
            _context.Stores.Add(store2);
            _context.SaveChanges();

            _product = new Product() {ProductName = "Test"};
            _product2 = new Product() {ProductName = "Test2"};
            var hasA = new HasA() {Price = 4, Product = _product, Store = store};
            var hasA2 = new HasA() {Price = 6, Product = _product2, Store = store};
            var hasA3 = new HasA() { Price = 5, Product = _product, Store = store2 };
            
            _context.Products.Add(_product);
            _context.Products.Add(_product2);
            _context.HasARelation.Add(hasA2);
            _context.HasARelation.Add(hasA);
            _context.HasARelation.Add(hasA3);
            _context.SaveChanges();

            _consumer = new global::Consumer.Consumer(_unit);
            _mail = Substitute.For<IMail>();
            _autocomplete = new Autocomplete(_unit);
            _generatedShoppingList = new GeneratedShoppingListModel(_consumer, _mail);
            _shoppingList = new ShoppingListModel(_consumer, _autocomplete);
            _findProduct = new FindProductModel(_consumer, _autocomplete);

            _shoppingList.ClearShoppingListCommand.Execute(this);
        }

        // FindProduct Tests
        [Test]
        public void AddToStoreListCommand_FindProductTest_ListShowsTwoStores()
        {
            _findProduct.ProductName = _product.ProductName;

            _findProduct.AddToStoreListCommand.Execute(this);

            Assert.That(_findProduct.StorePrice.Count, Is.EqualTo(2));
        }

        [Test]
        public void AddToStoreListCommand_FindProductTest2_ListShowsTwoStores()
        {
            _findProduct.ProductName = _product2.ProductName;

            _findProduct.AddToStoreListCommand.Execute(this);

            Assert.That(_findProduct.StorePrice.Count, Is.EqualTo(1));
        }

        [Test]
        public void AddToStoreListCommand_FindProductInvalid_NoStoresHaveProduct()
        {
            _findProduct.ProductName = "Invalid";

            _findProduct.AddToStoreListCommand.Execute(this);

            Assert.That(_findProduct.StorePrice.Count, Is.EqualTo(0));
        }

        [Test]
        public void PopulatingListFindProduct_TeIsWrittenInTextBox_AutoCompleateListEquals2()
        {
            _findProduct.ProductName = "Te";
            _findProduct.PopulatingFindProductCommand.Execute(this);

            Assert.That(_findProduct.AutoCompleteList.Count, Is.EqualTo(2));
        }

        [Test]
        public void PopulatingListFindProduct_QIsWrittenInTextBox_AutoCompleateListEqualsZero()
        {
            _findProduct.ProductName = "Q";
            _findProduct.PopulatingFindProductCommand.Execute(this);

            Assert.That(_findProduct.AutoCompleteList.Count, Is.EqualTo(0));
        }

        [Test]
        public void PopulatingListFindProduct_TwoMoreProductsAndTeIsWrittenInTextBox_AutoCompleateListEquals1()
        {
            _unit.Products.Add(new Product() { ProductName = "Test3" });
            _unit.Products.Add(new Product() { ProductName = "Test4" });
            _unit.Complete();

            _findProduct.ProductName = "Te";
            _findProduct.PopulatingFindProductCommand.Execute(this);

            Assert.That(_findProduct.AutoCompleteList.Count, Is.EqualTo(3));
        }


        // ShoppingListModel Tests
        [Test]
        public void ClearShoppingListCommand_ClearShoppingList_ShoppinglistEqualsIsEmpty()
        {
            var item = new ProductInfo("Test");
            _shoppingList.ShoppingListData.Add(item);
            _shoppingList.ClearShoppingListCommand.Execute(this);
            
            Assert.That(_shoppingList.ShoppingListData.Count, Is.EqualTo(0));
        }

        [Test]
        public void DeleteFromShoppingListCommand_DeleteTest_ShoppinglistEqualsIsEmpty()
        {
            var item = new ProductInfo("Test");
            _shoppingList.ShoppingListData.Add(item);
            _shoppingList.SelectedItem = item;

            _shoppingList.DeleteFromShoppingListCommand.Execute(this);

            Assert.That(_shoppingList.ShoppingListData.Count, Is.EqualTo(0));
        }

        [Test]
        public void DeleteFromShoppingListCommand_TwoItemsInListAndDeleteTest_ShoppinglistEqualsOne()
        {
            var item = new ProductInfo("Test");
            var item2 = new ProductInfo("Test2");
            _shoppingList.ShoppingListData.Add(item);
            _shoppingList.ShoppingListData.Add(item2);
            _shoppingList.SelectedItem = item;

            _shoppingList.DeleteFromShoppingListCommand.Execute(this);

            Assert.That(_shoppingList.ShoppingListData.Count, Is.EqualTo(1));
        }

        [Test]
        public void DeleteFromShoppingListCommand_TwoItemsInListAndDeleteTest_ShoppinglistFirstPlaceEqualsItem2()
        {
            var item = new ProductInfo("Test");
            var item2 = new ProductInfo("Test2");
            _shoppingList.ShoppingListData.Add(item);
            _shoppingList.ShoppingListData.Add(item2);
            _shoppingList.SelectedItem = item;

            _shoppingList.DeleteFromShoppingListCommand.Execute(this);

            Assert.That(_shoppingList.ShoppingListData[0].Name, Is.EqualTo(item2.Name));
        }

        [Test]
        public void PopulatingListShoppingList_TeIsWrittenInTextBox_AutoCompleateListEquals2()
        {
            _shoppingList.ShoppingListItem = "Te";
            _shoppingList.PopulatingShoppingListCommand.Execute(this);

            Assert.That(_shoppingList.AutoCompleteList.Count, Is.EqualTo(2));
        }

        [Test]
        public void PopulatingListShoppingList_QIsWrittenInTextBox_AutoCompleateListEqualsZero()
        {
            _shoppingList.ShoppingListItem = "Q";
            _shoppingList.PopulatingShoppingListCommand.Execute(this);

            Assert.That(_shoppingList.AutoCompleteList.Count, Is.EqualTo(0));
        }

        [Test]
        public void PopulatingListShoppingList_TwoMoreProductsAndTeIsWrittenInTextBox_AutoCompleateListEquals3()
        {
            _unit.Products.Add(new Product() { ProductName = "Test3" });
            _unit.Products.Add(new Product() { ProductName = "Test4" });
            _unit.Complete();

            _shoppingList.ShoppingListItem = "Te";
            _shoppingList.PopulatingShoppingListCommand.Execute(this);

            Assert.That(_shoppingList.AutoCompleteList.Count, Is.EqualTo(3));
        }

        [Test]
        public void GeneratedShoppingListFromShoppingList_MakeANewListWithTwoProducts_GeneratedshoppinglistCountEquals2()
        {
            var item = new ProductInfo("Test");
            var item2 = new ProductInfo("Test2");

            _shoppingList.ShoppingListData.Add(item2);
            _shoppingList.ShoppingListData.Add(item);

            _shoppingList.GeneratedShoppingListCommand.Execute(this);

            Assert.That(_consumer.GeneratedShoppingListData.Count, Is.EqualTo(2));
        }

        [Test]
        public void GeneratedShoppingListFromShoppingList_MakeANewListWithTwoProducts_NotInAStoreCountEquals0()
        {
            var item = new ProductInfo("Test");
            var item2 = new ProductInfo("Test2");

            _shoppingList.ShoppingListData.Add(item2);
            _shoppingList.ShoppingListData.Add(item);

            _shoppingList.GeneratedShoppingListCommand.Execute(this);

            Assert.That(_consumer.NotInAStore.Count, Is.EqualTo(0));
        }

        [Test]
        public void GeneratedShoppingListFromShoppingList_MakeANewListWithThreeProducts_GeneratedshoppinglistCountEquals2()
        {
            var item = new ProductInfo("Test");
            var item2 = new ProductInfo("Test2");
            var item3 = new ProductInfo("NotInAStore");

            _shoppingList.ShoppingListData.Add(item2);
            _shoppingList.ShoppingListData.Add(item);
            _shoppingList.ShoppingListData.Add(item3);

            _shoppingList.GeneratedShoppingListCommand.Execute(this);

            Assert.That(_consumer.GeneratedShoppingListData.Count, Is.EqualTo(2));
        }

        [Test]
        public void GeneratedShoppingListFromShoppingList_MakeANewListWithThreeProducts_NotInAStoreCountEquals1()
        {
            var item = new ProductInfo("Test");
            var item2 = new ProductInfo("Test2");
            var item3 = new ProductInfo("NotInAStore");

            _shoppingList.ShoppingListData.Add(item2);
            _shoppingList.ShoppingListData.Add(item);
            _shoppingList.ShoppingListData.Add(item3);

            _shoppingList.GeneratedShoppingListCommand.Execute(this);

            Assert.That(_consumer.NotInAStore.Count, Is.EqualTo(1));
        }

        [Test]
        public void GeneratedShoppingListFromShoppingList_MakeANewListWithNoProducts_GeneratedshoppinglistCountEquals0()
        {
            _shoppingList.GeneratedShoppingListCommand.Execute(this);

            Assert.That(_consumer.GeneratedShoppingListData.Count, Is.EqualTo(0));
        }

        [Test]
        public void GeneratedShoppingListFromShoppingList_MakeANewListWithNoProducts_NotInAStoreCountEquals0()
        {
            _shoppingList.GeneratedShoppingListCommand.Execute(this);

            Assert.That(_consumer.NotInAStore.Count, Is.EqualTo(0));
        }



        // GenereratedShoppingList Tests
        [Test]
        public void SendMailCommand_SendEMail_EMailSent()
        {
            _generatedShoppingList.SendMailCommand.Execute(this);
            _mail.Received(1);
        }

        [Test]
        public void SendMailCommand_SendTwoEMails_TwoEMailSent()
        {
            _generatedShoppingList.SendMailCommand.Execute(this);
            _generatedShoppingList.SendMailCommand.Execute(this);
            _mail.Received(2);
        }

        [Test]
        public void StoreChangedCommand_StoreChangedToTestTwo_StoreChangesToTestTwo()
        {
            var item = new ProductInfo("Test");
            var item2 = new ProductInfo("Test2");

            _shoppingList.ShoppingListData.Add(item);
            _shoppingList.ShoppingListData.Add(item2);

            _shoppingList.GeneratedShoppingListCommand.Execute(this);

            _generatedShoppingList.SelectedStoreIndex = 1;
            _generatedShoppingList.SelectedIndexGeneratedShoppingList = 0;
            _generatedShoppingList.StoreChangedCommand.Execute(this);

            Assert.That(_generatedShoppingList.ErrorStore, Is.EqualTo(""));
        }

        [Test]
        public void StoreChangedCommand_StoreChanged_ErrorMessage()
        {
            var item = new ProductInfo("Test");
            var item2 = new ProductInfo("Test2");

            _shoppingList.ShoppingListData.Add(item);
            _shoppingList.ShoppingListData.Add(item2);

            _shoppingList.GeneratedShoppingListCommand.Execute(this);

            _generatedShoppingList.SelectedStoreIndex = 1;
            _generatedShoppingList.SelectedIndexGeneratedShoppingList = 1;
            _generatedShoppingList.StoreChangedCommand.Execute(this);

            Assert.That(_generatedShoppingList.ErrorStore, Is.EqualTo(_generatedShoppingList.StoreNames[_generatedShoppingList.SelectedStoreIndex] + " har ikke produktet \"" +
                             _generatedShoppingList.GeneratedShoppingListData[_generatedShoppingList.SelectedIndexGeneratedShoppingList].ProductName +
                             "\" i deres sortiment."));
        }




    }
}
