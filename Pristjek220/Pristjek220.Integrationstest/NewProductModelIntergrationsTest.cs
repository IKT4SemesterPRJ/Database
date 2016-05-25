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
    class NewProductModelIntergrationsTest
    {
        private NewProductModel _newProduct;
        private IUnitOfWork _unit;
        private DataContext _context;
        private IStoremanager _storemanager;
        private Store _store;
        private IAutocomplete _autocomplete;
        private ICreateMsgBox _msgBox;


        [SetUp]
        public void SetUp()
        {
            _context = new DataContext();
            _unit = new UnitOfWork(_context);

            _context.Database.Connection.ConnectionString = "Server=.\\SQLEXPRESS;Database=Pristjek220Data.DataContext; Trusted_Connection=True;MultipleActiveResultSets=True;";
            _context.Database.ExecuteSqlCommand("dbo.TestCleanTable");

            _store = new Store() { StoreName = "TestStore" };
            _autocomplete = new Autocomplete(_unit);
            _msgBox = Substitute.For<ICreateMsgBox>();

            _context.Stores.Add(_store);
            _context.SaveChanges();
            
            _storemanager = new Storemanager(_unit, _store);
            _newProduct = new NewProductModel(_storemanager, _autocomplete, _msgBox );

            _context.Products.Add(new Product() {ProductName = "Test"});
            _context.Products.Add(new Product() { ProductName = "Test2" });
            _context.SaveChanges();
        }

        [Test]
        public void PopulatingListNewProduct_TeIsWrittenInTextBox_AutoCompleateListEqualsTwo()
        {
            _newProduct.ShoppingListItem = "Te";
            _newProduct.PopulatingNewProductCommand.Execute(this);
            
            Assert.That(_newProduct.AutoCompleteList.Count, Is.EqualTo(2));
        }

        [Test]
        public void PopulatingListNewProduct_QIsWrittenInTextBox_AutoCompleateListEqualsZero()
        {
            _newProduct.ShoppingListItem = "Q";
            _newProduct.PopulatingNewProductCommand.Execute(this);

            Assert.That(_newProduct.AutoCompleteList.Count, Is.EqualTo(0));
        }

        [Test]
        public void PopulatingListNewProduct_TwoMoreProductsAndTeIsWrittenInTextBox_AutoCompleateListEqualsThree()
        {
            _context.Products.Add(new Product() { ProductName = "Test3" });
            _context.Products.Add(new Product() { ProductName = "Test4" });
            _context.SaveChanges();

            _newProduct.ShoppingListItem = "Te";
            _newProduct.PopulatingNewProductCommand.Execute(this);

            Assert.That(_newProduct.AutoCompleteList.Count, Is.EqualTo(3));
        }

        [Test]
        public void AddToStoreDatabase_AddProductTestToDatabase_ProductIsConnectedToStore()
        {
            _newProduct.ShoppingListItem = "Test";
            _newProduct.ShoppingListItemPrice = "7";
            _msgBox.AddProductMgsConfirmation(_newProduct.ShoppingListItem, _newProduct.ShoppingListItemPrice).Returns(DialogResult.Yes);
            
            _newProduct.AddToStoreDatabaseCommand.Execute(this);

            Assert.That(_unit.Stores.FindProductInStore(_store.StoreName,_newProduct.ShoppingListItem).Name, Is.EqualTo("Test"));
        }

        [Test]
        public void AddToStoreDatabase_AddProductTest5ToDatabase_ProductIsConnectedToStore()
        {
            _newProduct.ShoppingListItem = "Test5";
            _newProduct.ShoppingListItemPrice = "7";
            _msgBox.AddProductMgsConfirmation(_newProduct.ShoppingListItem, _newProduct.ShoppingListItemPrice).Returns(DialogResult.Yes);

            _newProduct.AddToStoreDatabaseCommand.Execute(this);

            Assert.That(_unit.Stores.FindProductInStore(_store.StoreName, _newProduct.ShoppingListItem).Name, Is.EqualTo("Test5"));
        }

        [Test]
        public void AddToStoreDatabase_AddProductTwoTimesToDatabase_ErrorMessage()
        {
            _newProduct.ShoppingListItem = "Test";
            _newProduct.ShoppingListItemPrice = "7";
            _msgBox.AddProductMgsConfirmation(_newProduct.ShoppingListItem, _newProduct.ShoppingListItemPrice).Returns(DialogResult.Yes);

            _newProduct.AddToStoreDatabaseCommand.Execute(this);
            _newProduct.AddToStoreDatabaseCommand.Execute(this);

            Assert.That(_newProduct.ConfirmText, Is.EqualTo($"Produktet \"{_newProduct.ShoppingListItem}\" findes allerede."));
        }
    }
}
