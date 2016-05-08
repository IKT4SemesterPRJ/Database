using System.Collections.Generic;
using NUnit.Framework;
using Pristjek220Data;

namespace Pristjek220.Integrationstest
{
    [TestFixture]
    public class ProductRepositoryItergrationstest
    {
        private DataContext _context;
        private ProductRepository _productRepository;
        private Product _prod;

        [SetUp]
        public void SetUp()
        {
            _context = new DataContext();
            _prod = new Product() {ProductName = "Test"};
            _context.Database.Connection.ConnectionString = "Server=.\\SQLEXPRESS;Database=Pristjek220Data.DataContext; Trusted_Connection=True; MultipleActiveResultSets=True;";
            _context.Database.ExecuteSqlCommand("dbo.TestCleanTable");
            _productRepository = new ProductRepository(_context);
        }

        [TearDown]
        public void TearDown()
        {
            _context.Database.ExecuteSqlCommand("dbo.TestCleanTable");
        }

        [Test]
        public void Find_FindProductProductIsFound_ProductFound()
        {
            _context.Products.Add(_prod);
            _context.SaveChanges();

            Assert.That(_productRepository.FindProduct(_prod.ProductName), Is.EqualTo(_prod));
        }

        [Test]
        public void Find_FindProductProductIsNotFound_ReturnNull()
        {
            Assert.That(_productRepository.FindProduct("24"), Is.EqualTo(null));
        }

        [Test]
        public void Get_GetProduct_ProductReturned()
        {
            _context.Products.Add(_prod);
            _context.SaveChanges();

            Assert.That(_productRepository.Get(_prod.ProductId), Is.EqualTo(_prod));
        }

        [Test]
        public void Get_GetProductNoProductWithThatId_ReturnNull()
        {
            Assert.That(_productRepository.Get(_prod.ProductId), Is.EqualTo(null));
        }

        [Test]
        public void FindProductStartingWith_MakeListForBa_ListReturnedContaining3Elements()
        {
            _context.Products.Add(_prod);
            _context.Products.Add(new Product() {ProductName = "Baloon"});
            _context.Products.Add(new Product() {ProductName = "Brie"});
            _context.Products.Add(new Product() {ProductName = "Bacon"});
            _context.Products.Add(new Product() {ProductName = "Banan"});
            _context.SaveChanges();

            Assert.That(_productRepository.FindProductStartingWith("Ba").Count, Is.EqualTo(3));
        }

        [Test]
        public void FindProductStartingWith_MakeListForKWithNoProductsStartingWithK_ListReturnedContaining0Elements()
        {
            Assert.That(_productRepository.FindProductStartingWith("K").Count, Is.EqualTo(0));
        }

        [Test]
        public void ConnectToDB_MakesAConnectionToDB_ReturnsTrue()
        {
            Assert.That(_productRepository.ConnectToDb(), Is.EqualTo(true));
        }

        [Test]
        public void ConnectToDB_CantMakeAConnectionToDB_ReturnsFalse()
        {
            _context.Database.Connection.ConnectionString = "Data Source=i4dab.ase.au.dk; Initial Catalog = F16I4PRJ4Gr7; User ID = F16I4PRJ4Gr7; Password = F16I4PRJ4Gr7; ";
            Assert.That(_productRepository.ConnectToDb(), Is.EqualTo(false));
            _context.Database.Connection.ConnectionString = "Server=.\\SQLEXPRESS;Database=Pristjek220Data.DataContext; Trusted_Connection=True;";
        }

        [Test]
        public void FindStoresThatSellsProduct_NoStoreSellsTest_ReturnAListContaing0Elements()
        {
            _context.Products.Add(_prod);
            Assert.That(_productRepository.FindStoresThatSellsProduct(_prod.ProductName).Count, Is.EqualTo(0));
        }

        [Test]
        public void FindStoresThatSellsProduct_AldiAndFøtexSellsTest_ReturnAListContaing2Elements()
        {
            var aldi = new Store() {StoreName = "Aldi"};
            var føtex = new Store() {StoreName = "Føtex"};
            _context.Products.Add(_prod);
            _context.Stores.Add(aldi);
            _context.Stores.Add(føtex);
            _context.SaveChanges();

            _context.HasARelation.Add(new HasA() {Price = 2.95, Store = aldi, Product = _prod, ProductId = _prod.ProductId, StoreId = aldi.StoreId});
            _context.HasARelation.Add(new HasA() { Price = 2.95, Store = føtex, Product = _prod, ProductId = _prod.ProductId, StoreId = føtex.StoreId });
            _context.SaveChanges();

            Assert.That(_productRepository.FindStoresThatSellsProduct(_prod.ProductName).Count, Is.EqualTo(2));
        }

        [Test]
        public void FindCheapestStoreForAllProductsWithSum_EmptyListGiven_ReturnsNull()
        {
            Assert.That(_productRepository.FindCheapestStoreForAllProductsWithSum(new List<ProductInfo>()), Is.EqualTo(null));
        }

        [Test]
        public void FindCheapestStoreForAllProductsWithSum_OneProductSoldInFøtex_ReturnsAListWithFøtexAndThePrice()
        {
            var føtex = new Store() { StoreName = "Føtex" };

            _context.Stores.Add(føtex);
            _context.Products.Add(_prod);
            _context.SaveChanges();
            _context.HasARelation.Add(new HasA() { Price = 2.95, Store = føtex, Product = _prod, ProductId = _prod.ProductId, StoreId = føtex.StoreId });
            _context.SaveChanges();

            Assert.That(_productRepository.FindCheapestStoreForAllProductsWithSum(new List<ProductInfo>() {new ProductInfo("Test")})[0].Price, Is.EqualTo(2.95));
        }

        [Test]
        public void FindCheapestStoreForAllProductsWithSum_AldiAndFøtexSellsTest_ReturnAListContaing2Elements()
        {
            var føtex = new Store() { StoreName = "Føtex" };
            var aldi = new Store() { StoreName = "Aldi" };

            _context.Stores.Add(føtex);
            _context.Stores.Add(aldi);
            _context.Products.Add(_prod);
            _context.SaveChanges();
            _context.HasARelation.Add(new HasA() { Price = 2.95, Store = føtex, Product = _prod, ProductId = _prod.ProductId, StoreId = føtex.StoreId });
            _context.HasARelation.Add(new HasA() { Price = 3.95, Store = aldi, Product = _prod, ProductId = _prod.ProductId, StoreId = aldi.StoreId });
            _context.SaveChanges();

            Assert.That(_productRepository.FindCheapestStoreForAllProductsWithSum(new List<ProductInfo>() { new ProductInfo("Test") }).Count, Is.EqualTo(2));
        }

        [Test]
        public void FindCheapestStoreForAllProductsWithSum_AldiAndFøtexSellsTestAndTest1ButFaktaOnlySellsTest_ReturnAListContaing4Elements()
        {
            var føtex = new Store() { StoreName = "Føtex" };
            var aldi = new Store() { StoreName = "Aldi" };
            var fakta = new Store() { StoreName = "Fakta" };
            var test1 = new Product() {ProductName = "Test1"};

            _context.Stores.Add(føtex);
            _context.Stores.Add(aldi);
            _context.Stores.Add(fakta);
            _context.Products.Add(_prod);
            _context.SaveChanges();
            _context.HasARelation.Add(new HasA() { Price = 2.95, Store = føtex, Product = _prod, ProductId = _prod.ProductId, StoreId = føtex.StoreId });
            _context.HasARelation.Add(new HasA() { Price = 4.95, Store = føtex, Product = test1, ProductId = test1.ProductId, StoreId = føtex.StoreId });
            _context.HasARelation.Add(new HasA() { Price = 1.95, Store = aldi, Product = _prod, ProductId = _prod.ProductId, StoreId = aldi.StoreId });
            _context.HasARelation.Add(new HasA() { Price = 5.95, Store = aldi, Product = test1, ProductId = test1.ProductId, StoreId = aldi.StoreId });
            _context.HasARelation.Add(new HasA() { Price = 1.95, Store = fakta, Product = _prod, ProductId = _prod.ProductId, StoreId = fakta.StoreId });
            _context.SaveChanges();

            Assert.That(_productRepository.FindCheapestStoreForAllProductsWithSum(new List<ProductInfo>() { new ProductInfo("Test") , new ProductInfo("Test1")}).Count, Is.EqualTo(4));
        }

        [Test]
        public void FindCheapestStoreForAllProductsWithSum_4TestPriceIsMulipliedWithQuantity_ReturnAListWithPriceMultipied()
        {
            var føtex = new Store() { StoreName = "Føtex" };

            _context.Stores.Add(føtex);
            _context.Products.Add(_prod);
            _context.SaveChanges();
            _context.HasARelation.Add(new HasA() { Price = 2.95, Store = føtex, Product = _prod, ProductId = _prod.ProductId, StoreId = føtex.StoreId });
            _context.SaveChanges();

            Assert.That(_productRepository.FindCheapestStoreForAllProductsWithSum(new List<ProductInfo>() { new ProductInfo("Test", "4")})[0].Price, Is.EqualTo(4*2.95));
        }
    }
}
