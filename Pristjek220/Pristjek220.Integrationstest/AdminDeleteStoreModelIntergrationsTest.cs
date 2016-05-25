using System;
using System.Collections.Generic;
using System.Linq;
using System.Security;
using System.Text;
using System.Threading.Tasks;
using Administration_GUI;
using Administration_GUI.User_Controls_Admin;
using NSubstitute;
using NUnit.Framework;
using Pristjek220Data;
using SharedFunctionalities;
using Admin = Administration.Admin;

namespace Pristjek220.Integrationstest
{
    class AdminDeleteStoreModelIntergrationsTest
    {
        private DataContext _context;
        private IUnitOfWork _unitOfWork;
        private AdminDeleteStoreModel _adminDeleteStoreModel;
        private IAutocomplete _autocomplete;
        private ICreateMsgBox _msgBox;
        private Admin _admin;

        [SetUp]
        public void SetUp()
        {
            _context = new DataContext();
            _unitOfWork = new UnitOfWork(_context);
            _autocomplete = new Autocomplete(_unitOfWork);
            _msgBox = Substitute.For<ICreateMsgBox>();
            _admin = new Admin(_unitOfWork);
            _adminDeleteStoreModel = new AdminDeleteStoreModel(_admin, _autocomplete, _msgBox);
            _context.Database.Connection.ConnectionString = "Server=.\\SQLEXPRESS;Database=Pristjek220Data.DataContext; Trusted_Connection=True;";
            _context.Database.ExecuteSqlCommand("dbo.TestCleanTable");

            _context.Stores.Add(new Store() {StoreName = "TestStore", StoreId = 1});

        }

        [TearDown]
        public void TearDown()
        {
            _context.Database.ExecuteSqlCommand("dbo.TestCleanTable");
        }

        [Test]
        public void DeleteStore_DeleteTestStoreFromDb_nullIsReturned()
        {
            _adminDeleteStoreModel.DeleteStoreName = "TestStore";
            _adminDeleteStoreModel.DeleteFromLoginDatabaseCommand.Execute(this);
            Assert.That(_unitOfWork.Stores.FindStore("TestStore"), Is.EqualTo(null));
        }

        [Test]
        public void DeleteStore_DeleteTestStore1FromDbRequestTestStore_TestStoreReturned()
        {
            _adminDeleteStoreModel.DeleteStoreName = "TestStore1";
            _adminDeleteStoreModel.DeleteFromLoginDatabaseCommand.Execute(this);
            Assert.That(_unitOfWork.Stores.FindStore("TestStore"), Is.Not.EqualTo(null));
        }


    }
}
