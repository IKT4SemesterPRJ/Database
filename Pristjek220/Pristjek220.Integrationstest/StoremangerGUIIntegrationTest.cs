using System.Windows.Forms;
using Administration;
using Administration_GUI;
using Administration_GUI.User_Controls;
using NSubstitute;
using NUnit.Framework;
using Pristjek220Data;
using SharedFunctionalities;

namespace Pristjek220.Integrationstest
{
    [TestFixture]
    class StoremangerGUIIntegrationTest
    {
        private IStoremanager _manager;
        private IAutocomplete _autocomplete;
        private ICreateMsgBox _createMsgBox;
        private IUnitOfWork _unit;
        private ChangePriceModel _changePriceModel;
        private DeleteProductModel _deleteProductModel;
        private Store _store;
        private Product _product;
        

        [SetUp]
        public void SetUp()
        {
            var context = new DataContext();
            _unit = new UnitOfWork(context);
            context.Database.Connection.ConnectionString = "Server=.\\SQLEXPRESS;Database=Pristjek220Data.DataContext; Trusted_Connection=True;";
            context.Database.ExecuteSqlCommand("dbo.TestCleanTable");
            _store = new Store() {StoreName = "TestStore"};
            _unit.Stores.Add(_store);
            _product = new Product() {ProductName = "Testproduct"};
            _unit.Products.Add(_product);
            _unit.Complete();
            _unit.HasA.Add(new HasA() {Product = _product, ProductId = _product.ProductId, Store = _store, StoreId = _store.StoreId, Price = 10});
            _unit.Complete();

            _manager = new Storemanager(_unit, _store);
            _autocomplete = new Autocomplete(_unit);
            _createMsgBox = Substitute.For<ICreateMsgBox>();
            _changePriceModel = new ChangePriceModel(_manager, _autocomplete, _createMsgBox);
            _deleteProductModel = new DeleteProductModel(_manager, _autocomplete, _createMsgBox);
        }

        [Test]
        public void PopulatingListChangePriceProduct_TeIsWrittenInTextBox_AutoCompleateListEquals1()
        {
            _changePriceModel.ShoppingListItem = "Te";
            _changePriceModel.PopulatingChangePriceProductCommand.Execute(this);

            Assert.That(_changePriceModel.AutoCompleteList.Count, Is.EqualTo(1));
        }

        [Test]
        public void PopulatingListChangePriceProduct_QIsWrittenInTextBox_AutoCompleateListEqualsZero()
        {
            _changePriceModel.ShoppingListItem = "Q";
            _changePriceModel.PopulatingChangePriceProductCommand.Execute(this);

            Assert.That(_changePriceModel.AutoCompleteList.Count, Is.EqualTo(0));
        }

        [Test]
        public void PopulatingListChangePriceProduct_TwoMoreProductsAndTeIsWrittenInTextBox_AutoCompleateListEquals1()
        {
            _unit.Products.Add(new Product() { ProductName = "Test3" });
            _unit.Products.Add(new Product() { ProductName = "Test4" });
            _unit.Complete();

            _changePriceModel.ShoppingListItem = "Te";
            _changePriceModel.PopulatingChangePriceProductCommand.Execute(this);

            Assert.That(_changePriceModel.AutoCompleteList.Count, Is.EqualTo(1));
        }

        [Test]
        public void ChangeProductPriceInStoreDatabaseCommand_ChangeThePriceOfTestProductTo20kr_ThePriceIsChanged()
        {
            _changePriceModel.ShoppingListItemPrice = "20,00";
            _changePriceModel.ShoppingListItem = _product.ProductName;
            _createMsgBox.ChangePriceMgsConfirmation(_changePriceModel.ShoppingListItem, _changePriceModel.ShoppingListItemPrice).Returns(DialogResult.Yes);
            _changePriceModel.ChangeProductPriceInStoreDatabaseCommand.Execute(this);

            Assert.That(_unit.HasA.Get(_store.StoreId, _product.ProductId).Price, Is.EqualTo(20));
        }

        [Test]
        public void ChangeProductPriceInStoreDatabaseCommand_ChangeThePriceOfTestProductTo0kr_ThePriceIsNotChanged()
        {
            _changePriceModel.ShoppingListItemPrice = "0,00";
            _changePriceModel.ShoppingListItem = _product.ProductName;
            _createMsgBox.ChangePriceMgsConfirmation(_changePriceModel.ShoppingListItem, _changePriceModel.ShoppingListItemPrice).Returns(DialogResult.Yes);
            _changePriceModel.ChangeProductPriceInStoreDatabaseCommand.Execute(this);

            Assert.That(_unit.HasA.Get(_store.StoreId, _product.ProductId).Price, Is.EqualTo(10));
        }

        [Test]
        public void PopulatingListDeleteProduct_TeIsWrittenInTextBox_AutoCompleateListEquals1()
        {
            _changePriceModel.ShoppingListItem = "Te";
            _changePriceModel.PopulatingChangePriceProductCommand.Execute(this);

            Assert.That(_changePriceModel.AutoCompleteList.Count, Is.EqualTo(1));
        }

        [Test]
        public void PopulatingListDeleteProduct_QIsWrittenInTextBox_AutoCompleateListEqualsZero()
        {
            _changePriceModel.ShoppingListItem = "Q";
            _changePriceModel.PopulatingChangePriceProductCommand.Execute(this);

            Assert.That(_changePriceModel.AutoCompleteList.Count, Is.EqualTo(0));
        }

        [Test]
        public void PopulatingListDeleteProduct_TwoMoreProductsAndTeIsWrittenInTextBox_AutoCompleateListEquals1()
        {
            _unit.Products.Add(new Product() { ProductName = "Test3" });
            _unit.Products.Add(new Product() { ProductName = "Test4" });
            _unit.Complete();

            _changePriceModel.ShoppingListItem = "Te";
            _changePriceModel.PopulatingChangePriceProductCommand.Execute(this);

            Assert.That(_changePriceModel.AutoCompleteList.Count, Is.EqualTo(1));
        }

        [Test]
        public void DeleteFromStoreDatabase_RemoveTestproductFromStore_HasARelationRemoved()
        {
            _deleteProductModel.ShoppingListItem = _product.ProductName;
            _createMsgBox.DeleteProductMgsConfirmation(_deleteProductModel.ShoppingListItem).Returns(DialogResult.Yes);
            _deleteProductModel.DeleteFromStoreDatabaseCommand.Execute(this);

            Assert.That(_deleteProductModel.ConfirmText, Is.EqualTo($"Produktet \"{_product.ProductName}\" er fjernet fra din forretning."));
        }

        [Test]
        public void DeleteFromStoreDatabase_RemoveProductFromStoreButIsNotSoldInStore_LabelSet()
        {
            _unit.Products.Add(new Product() {ProductName = "Notinstore"});
            _unit.Complete();
            _deleteProductModel.ShoppingListItem = "Notinstore";
            _createMsgBox.DeleteProductMgsConfirmation(_deleteProductModel.ShoppingListItem).Returns(DialogResult.Yes);
            _deleteProductModel.DeleteFromStoreDatabaseCommand.Execute(this);

            Assert.That(_deleteProductModel.ConfirmText, Is.EqualTo("Produktet \"Notinstore\" findes ikke i din forretning."));
        }

        [Test]
        public void DeleteFromStoreDatabase_RemoveProductFromButIsNotInDatabase_LabelSet()
        {
            _deleteProductModel.ShoppingListItem = "Notindatabase";
            _createMsgBox.DeleteProductMgsConfirmation(_deleteProductModel.ShoppingListItem).Returns(DialogResult.Yes);
            _deleteProductModel.DeleteFromStoreDatabaseCommand.Execute(this);

            Assert.That(_deleteProductModel.ConfirmText, Is.EqualTo("Produktet \"Notindatabase\" findes ikke."));
        }
    }
}
