using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using Pristjek220Data;

namespace Pristjek220.Integrationstest
{
    [TestFixture]
    public class ProductRepositoryItergrationstest
    {
        private Pristjek220Data.DataContext _context;
        private Pristjek220Data.ProductRepository _productRepository;

        [SetUp]
        public void SetUp()
        {
            _context = new DataContext();
            _context.Database.Connection.ConnectionString = "Server=.\\SQLEXPRESS;Database=SikkerhedsKopi af Datacontext; Trusted_Connection=True;";
            

            _productRepository = new ProductRepository(_context);
        }

        [Test]
        public void Add_AddAProductToDb_ProductIsInDb()
        {
            var prod = new Product() {ProductName = "Test"};
            _productRepository.Add(prod);
            _context.SaveChanges();

            Assert.That(_productRepository.Get(53), Is.EqualTo(prod));
        }
    }
}
