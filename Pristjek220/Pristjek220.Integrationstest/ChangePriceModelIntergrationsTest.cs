using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
    class ChangePriceModelIntergrationsTest
    {
        private ChangePriceModel _uut;
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
            _uut = new ChangePriceModel(_storemanager, _autocomplete, _msgBox);

            _context.Products.Add(new Product() { ProductName = "Test" });
            _context.Products.Add(new Product() { ProductName = "Test2" });
            _context.SaveChanges();
        }

       
    }
}
