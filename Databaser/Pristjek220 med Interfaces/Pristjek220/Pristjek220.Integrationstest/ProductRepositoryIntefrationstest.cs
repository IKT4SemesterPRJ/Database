using NUnit.Framework;
using Pristjek220Data;

namespace Pristjek220.Integrationstest
{
    [TestFixture]
    public class ProductRepositoryItergrationstest
    {
        private DataContext _context;
        private ProductRepository _productRepository;

        [SetUp]
        public void SetUp()
        {
            _context = new DataContext();
            _context.Database.Connection.ConnectionString = "Server=.\\SQLEXPRESS;Database=Pristjek220Data.DataContext; Trusted_Connection=True;";
            _context.Database.ExecuteSqlCommand("dbo.TestCleanTable");
            _productRepository = new ProductRepository(_context);
        }

        [Test]
        public void Add_AddAProductToDb_ProductIsInDb()
        {
            var prod = new Product() {ProductName = "Test"};
            _productRepository.Add(prod);
            _context.SaveChanges();

            Assert.That(_productRepository.FindProduct("Test").ProductName, Is.EqualTo(prod.ProductName));
        }
    }
}
