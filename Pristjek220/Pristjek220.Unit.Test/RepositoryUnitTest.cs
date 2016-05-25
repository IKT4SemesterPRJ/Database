using System.Data.Entity;
using NUnit.Framework;
using Pristjek220Data;

namespace Pristjek220.Unit.Test
{
    [TestFixture]
    class RepositoryUnitTest
    {
        private DbContext _context;
        private Repository<Product> _uut;

        [SetUp]
        public void SetUp()
        {
            _context = new DataContext();
            _uut = new Repository<Product>(_context);
        }

        [Test]
        public void Constructor_ConnectionStringSat_ConnectionStringMatch()
        {
            Assert.That(_context.Database.Connection.ConnectionString, Is.EqualTo("Data Source=i4dab.ase.au.dk; Initial Catalog = F16I4PRJ4Gr7; User ID = F16I4PRJ4Gr7; Password = F16I4PRJ4Gr7; MultipleActiveResultSets=True;"));
        }
    }
}
