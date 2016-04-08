using NUnit.Framework;
using Pristjek220Data;

namespace Pristjek220.Integrationstest
{
    [TestFixture]
    class RepositoryIntergrationtest
    {
        private DataContext _context;
        private Repository<Product> _productRepository;
        private Repository<Store> _storeRepository;
        private Repository<HasA> _hasARepository;
        private Product _product;
        private Store _store;

        [SetUp]
        public void SetUp()
        {
            _context = new DataContext();
            _context.Database.Connection.ConnectionString = "Server=.\\SQLEXPRESS;Database=Pristjek220Data.DataContext; Trusted_Connection=True;";
            _context.Database.ExecuteSqlCommand("dbo.TestCleanTable");
            _productRepository = new Repository<Product>(_context);
            _storeRepository = new Repository<Store>(_context);
            _hasARepository = new Repository<HasA>(_context);
            _product = new Product() { ProductName = "TestProduct" };
            _store = new Store() { StoreName = "TestStore" };
        }

        [Test]
        public void Add_AddProductToProductDb_ProductCanBefoundInDb()
        {
            _productRepository.Add(_product);
            _context.SaveChanges();

            Assert.That(_context.Products.Find(_product.ProductId), Is.EqualTo(_product));
        }

        [Test]
        public void Add_AddStoreToStoreDb_StoreCanBefoundInDb()
        {
            _storeRepository.Add(_store);
            _context.SaveChanges();

            Assert.That(_context.Stores.Find(_store.StoreId), Is.EqualTo(_store));
        }

        [Test]
        public void Add_AddHasAToHasADb_HasACanBefoundInDb()
        {
            _storeRepository.Add(_store);
            _productRepository.Add(_product);
            _context.SaveChanges();
            var hasA = new HasA() {Price = 23, ProductId = _product.ProductId, StoreId = _store.StoreId, Store = _store, Product = _product};

            _hasARepository.Add(hasA);
            _context.SaveChanges();

            Assert.That(_context.HasARelation.Find(hasA.StoreId, hasA.ProductId), Is.EqualTo(hasA));
        }

        [Test]
        public void Remove_RemoveProductFromProductDb_ProductCantBeFoundInDb()
        {
            _productRepository.Add(_product);
            _context.SaveChanges();
            _productRepository.Remove(_product);
            _context.SaveChanges();

            Assert.That(_context.Products.Find(_product.ProductId), Is.EqualTo(null));
        }

        [Test]
        public void Remove_RemoveProductThatDoesNotExistProductDb_ReturnNull()
        {
            _productRepository.Remove(_product);
            _context.SaveChanges();

            Assert.That(_context.Products.Find(_product.ProductId), Is.EqualTo(null));
        }

        [Test]
        public void Remove_RemoveStoreFromStoreDb_StoreCantBeFoundInDb()
        {
            _storeRepository.Add(_store);
            _context.SaveChanges();
            _storeRepository.Remove(_store);
            _context.SaveChanges();

            Assert.That(_context.Stores.Find(_store.StoreId), Is.EqualTo(null));
        }

        [Test]
        public void Remove_RemoveStoreThatDoesNotExistStoreDb_ReturnNull()
        {
            _storeRepository.Remove(_store);
            _context.SaveChanges();

            Assert.That(_context.Stores.Find(_store.StoreId), Is.EqualTo(null));
        }

        [Test]
        public void Remove_RemoveHasAFromHasARelationDb_HasACantBeFoundInDb()
        {
            _storeRepository.Add(_store);
            _productRepository.Add(_product);
            _context.SaveChanges();
            var hasA = new HasA() { Price = 23, ProductId = _product.ProductId, StoreId = _store.StoreId, Store = _store, Product = _product };

            _hasARepository.Add(hasA);
            _context.SaveChanges();

            _hasARepository.Remove(hasA);
            _context.SaveChanges();

            Assert.That(_context.HasARelation.Find(hasA.StoreId, hasA.ProductId), Is.EqualTo(null));
        }

        [Test]
        public void Remove_RemoveHasAThatDoesNotExistInHasADb_ReturnNull()
        {
            var hasA = new HasA() { Price = 23, ProductId = _product.ProductId, StoreId = _store.StoreId, Store = _store, Product = _product };
            _hasARepository.Remove(hasA);
            _context.SaveChanges();

            Assert.That(_context.HasARelation.Find(hasA.ProductId, hasA.StoreId), Is.EqualTo(null));
        }
    }
}