using System;
using System.Data.Entity.Core;
using System.Data.Entity.Core.Metadata.Edm;
using System.Data.SqlClient;
using NUnit.Framework;
using Pristjek220Data;

namespace Pristjek220.Integrationstest
{
    [TestFixture]
    public class ProductRepositoryItergrationstest
    {
        private DataContext _context;
        private ProductRepository _productRepository;
        private readonly Product _prod = new Product() {ProductName = "Test"};

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
            _productRepository.Add(_prod);
            _context.SaveChanges();

            Assert.That(_productRepository.Get(_prod.ProductId), Is.EqualTo(_prod));
        }

        [Test]
        public void Remove_RemoveAProductFromDb_ProductIsRemoved()
        {
            _productRepository.Add(_prod);
            _context.SaveChanges();
            _productRepository.Remove(_prod);
            _context.SaveChanges();

            Assert.That(_productRepository.Get(_prod.ProductId), Is.EqualTo(null));
        }

        [Test]
        public void Find_FindProductProductIsFound_ProductFound()
        {
            _productRepository.Add(_prod);
            _context.SaveChanges();

            Assert.That(_productRepository.FindProduct(_prod.ProductName), Is.EqualTo(_prod));
        }

        [Test]
        public void Find_FindProductProductIsNotFound_ReturnNull()
        {
            Assert.That(_productRepository.FindProduct("24"), Is.EqualTo(null));
        }

        [Test]
        public void Remove_RemoveProductProductIsNotInTheDataBase()
        {
            _productRepository.Remove(_prod);
            _context.SaveChanges();

            Assert.That(_productRepository.Get(_prod.ProductId), Is.EqualTo(null));
        }

        [Test]
        public void Get_GetProduct_ProductReturned()
        {
            var product = new Product() {ProductName = "Tester"};
            _productRepository.Add(product);
            _context.SaveChanges();

            Assert.That(_productRepository.Get(product.ProductId), Is.EqualTo(product));
        }

        [Test]
        public void Get_GetProductNoProductWithThatId_ReturnNull()
        {
            Assert.That(_productRepository.Get(_prod.ProductId), Is.EqualTo(null));
        }

        [Test]
        public void FindProductStartingWith_MakeListForBa_ListReturnedContaining3Elements()
        {
            _productRepository.Add(_prod);
            _productRepository.Add(new Product() {ProductName = "Baloon"});
            _productRepository.Add(new Product() {ProductName = "Brie"});
            _productRepository.Add(new Product() {ProductName = "Bacon"});
            _productRepository.Add(new Product() {ProductName = "Banan"});
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
            //Assert.Throws<EntityException>(() => _productRepository.ConnectToDb());
        }
    }
}
