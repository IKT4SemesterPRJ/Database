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
        private Store _store;
        private Product _product;
        

        [SetUp]
        public void SetUp()
        {
            var _context = new DataContext();
            _unit = new UnitOfWork(_context);
            _context.Database.Connection.ConnectionString = "Server=.\\SQLEXPRESS;Database=Pristjek220Data.DataContext; Trusted_Connection=True;";
            _context.Database.ExecuteSqlCommand("dbo.TestCleanTable");
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
        }

        [Test]
        public void ChangeProductPriceInStoreDatabaseCommand_ChangeThePriceOfTestProductTo20kr_ThePriceIsChanged()
        {
            _changePriceModel.ShoppingListItemPrice = "20.00";
            _changePriceModel.ShoppingListItem = _product.ProductName;
            _createMsgBox.ChangePriceMgsConfirmation(_changePriceModel.ShoppingListItem, _changePriceModel.ShoppingListItemPrice).Returns(DialogResult.Yes);
            _changePriceModel.ChangeProductPriceInStoreDatabaseCommand.Execute(this);

            Assert.That(_unit.HasA.Get(_store.StoreId, _product.ProductId).Price, Is.EqualTo(20));
        }
    }
}
