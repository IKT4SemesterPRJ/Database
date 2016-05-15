using NUnit.Framework;
using Pristjek220Data;
using SharedFunctionalities;

namespace Pristjek220.Integrationstest
{
    [TestFixture]
    class DatabaseFunctionsIntegrationsTest
    {
        private IDatabaseFunctions _databaseFunctions;
        private IUnitOfWork _unit;
        private DataContext _context;

        [SetUp]
        public void SetUp()
        {
            _context = new DataContext();
            _unit = new UnitOfWork(_context);
            _databaseFunctions = new DatabaseFunctions(_unit);
            _context.Database.Connection.ConnectionString = "Server=.\\SQLEXPRESS;Database=Pristjek220Data.DataContext; Trusted_Connection=True;";
            _context.Database.ExecuteSqlCommand("dbo.TestCleanTable");
        }

        [Test]
        public void ConnectToDb_ConnectionMadeToDb_ReturnTrue()
        {
            Assert.That(_databaseFunctions.ConnectToDb());
        }

        [Test]
        public void ConnectToDb_ConnectingToNoneExsistingDb_ReturnFalse()
        {
            _context.Database.Connection.ConnectionString = "Server=.\\SQLEXPRESS;Database=No; Trusted_Connection=True;";
            Assert.That(_databaseFunctions.ConnectToDb(), Is.EqualTo(false));
        }
    }
}
