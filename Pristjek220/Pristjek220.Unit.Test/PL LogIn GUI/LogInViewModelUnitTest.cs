using System.Runtime.InteropServices.ComTypes;
using System.Security;
using System.Windows;
using Administration;
using Administration_GUI;
using NSubstitute;
using NUnit.Framework;
using Pristjek220Data;
using SharedFunctionalities;


namespace Pristjek220.Unit.Test
{
    [TestFixture]
    class LoginViewModelUnitTest
    {
        private LogInViewModel _uut;
        private IAutocomplete _autocomplete;
        private IDatabaseFunctions _databaseFunctions;
        private ILogIn _logIn;
        private IStoremanager _storemanager;
        private IAdmin _admin;




        [SetUp]
        public void SetUp()
        {
            _logIn = Substitute.For<ILogIn>();
            _autocomplete = Substitute.For<IAutocomplete>();
            _storemanager = Substitute.For<IStoremanager>();
            _admin = Substitute.For<IAdmin>();
            _databaseFunctions = Substitute.For<IDatabaseFunctions>();
            
            _databaseFunctions.ConnectToDb().Returns(true);
            _uut = new LogInViewModel(_autocomplete, _logIn, _databaseFunctions, _storemanager, _admin);

        }

        [Test]
        public void ChangeWindowHomeCommand_ChangeWindowToHome_MainWindowTekstIsPristjek220ForbrugerStartside()
        {
            _uut.LogInCommand.Execute(this);
            _uut.Username = "TestName";
            var secureString = new SecureString();
            secureString.AppendChar('A');
            secureString.AppendChar('A');
            secureString.AppendChar('A');
            _uut.SecurePassword = secureString;
            Store loginstore = new Store() {StoreName = "Admin"};
            _logIn.CheckUsernameAndPassword(_uut.Username, secureString, ref loginstore).ReturnsForAnyArgs(0);
            Assert.That(_uut.Error, Is.EqualTo("Kodeordet er ugyldigt."));
        }

        [Test]
        public void ChangeWindowHomeCommand1_ChangeWindowToHome_MainWindowTekstIsPristjek220ForbrugerStartside()
        {
            _uut.Username = "TestName";
            var secureString = new SecureString();
            secureString.AppendChar('A');
            secureString.AppendChar('A');
            secureString.AppendChar('A');
            _uut.SecurePassword = secureString;
            Store loginstore = new Store() { StoreName = "Admin" };
            _logIn.CheckUsernameAndPassword(_uut.Username, secureString, ref loginstore).ReturnsForAnyArgs(-1);
            _uut.LogInCommand.Execute(this);
            Assert.That(_uut.Error, Is.EqualTo("Brugernavnet er ugyldigt."));
        }
    }
}
