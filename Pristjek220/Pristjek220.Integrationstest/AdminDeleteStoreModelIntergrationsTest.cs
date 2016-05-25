using System;
using System.Collections.Generic;
using System.Linq;
using System.Security;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
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
        private readonly SecureString _secureStringTest = new SecureString();

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
            _secureStringTest.AppendChar('A');
            _secureStringTest.AppendChar('B');
            _secureStringTest.AppendChar('C');

            _admin.CreateLogin("Teststore", _secureStringTest, "Teststore");
            _msgBox.DeleteStoreMgsConfirmation("Teststore").Returns(DialogResult.Yes);
        }

        [TearDown]
        public void TearDown()
        {
            _context.Database.ExecuteSqlCommand("dbo.TestCleanTable");
        }

        [Test]
        public void DeleteStore_DeleteTestStoreFromDb_nullIsReturned()
        {
            _adminDeleteStoreModel.DeleteStoreName = "Teststore";
            _adminDeleteStoreModel.DeleteFromLoginDatabaseCommand.Execute(this);
            Assert.That(_unitOfWork.Stores.FindStore("Teststore"), Is.EqualTo(null));
        }

        [Test]
        public void DeleteStore_DeleteTestStoreFromDb_ErrorIsStoreHasBeenRemoved()
        {
            _adminDeleteStoreModel.DeleteStoreName = "Teststore";
            _adminDeleteStoreModel.DeleteFromLoginDatabaseCommand.Execute(this);
            Assert.That(_adminDeleteStoreModel.Error, Is.EqualTo("Forretningen \"Teststore\" er blevet fjernet fra Pristjek220."));
        }

        [Test]
        public void DeleteStore_DeleteTestStore1FromDbRequestTestStore_TestStoreReturned()
        {
            _adminDeleteStoreModel.DeleteStoreName = "Teststore1";
            _adminDeleteStoreModel.DeleteFromLoginDatabaseCommand.Execute(this);
            Assert.That(_unitOfWork.Stores.FindStore("Teststore"), Is.Not.EqualTo(null));
        }

        [Test]
        public void DeleteStore_DeleteTestStore1FromDbRequestTestStore_ErrorIsIsNotFoundInStore()
        {
            _adminDeleteStoreModel.DeleteStoreName = "Teststore1";
            _adminDeleteStoreModel.DeleteFromLoginDatabaseCommand.Execute(this);
            Assert.That(_adminDeleteStoreModel.Error, Is.EqualTo("Forretningen \"Teststore1\" findes ikke i Pristjek220."));
        }

        [Test]
        public void DeleteStore_DeleteemptyStringFromDbRequestTestStore_TestStoreReturned()
        {
            _adminDeleteStoreModel.DeleteStoreName = "";
            _adminDeleteStoreModel.DeleteFromLoginDatabaseCommand.Execute(this);
            Assert.That(_unitOfWork.Stores.FindStore("Teststore"), Is.Not.EqualTo(null));
        }

        [Test]
        public void DeleteStore_DeleteemptyStringFromDbRequestTestStore_ErrorPleaseEnterNameOfStore()
        {
            _adminDeleteStoreModel.DeleteStoreName = "";
            _adminDeleteStoreModel.DeleteFromLoginDatabaseCommand.Execute(this);
            Assert.That(_adminDeleteStoreModel.Error, Is.EqualTo("Indtast venligst navnet på den forretning der skal fjernes."));
        }

        [Test]
        public void DeleteStore_DeleteTestStoreFromDbDeConfirmed_ErrorIsHasNotBeenConfirmed()
        {
            _msgBox.DeleteStoreMgsConfirmation("Teststore").Returns(DialogResult.No);
            _adminDeleteStoreModel.DeleteStoreName = "Teststore";
            _adminDeleteStoreModel.DeleteFromLoginDatabaseCommand.Execute(this);
            Assert.That(_adminDeleteStoreModel.Error, Is.EqualTo("Der blev ikke bekræftet."));
        }

    }
}
