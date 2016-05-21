using Administration;
using Administration_GUI;
using Consumer;
using Consumer_GUI;
using NSubstitute;
using NUnit.Framework;
using SharedFunctionalities;


namespace Pristjek220.Unit.Test
{
    [TestFixture]
    class AdminViewModelUnitTest
    {
        private AdminViewModel _uut;
        private IAdmin _admin;
        private IAutocomplete _autocomplete;

        [SetUp]
        public void SetUp()
        {
            _admin = Substitute.For<IAdmin>();
            _autocomplete = Substitute.For<IAutocomplete>();
            _uut = new AdminViewModel(_admin, _autocomplete);
        }

        [Test]
        public void ChangeWindowHomeCommand_ChangeWindowToHome_MainWindowTekstIsPristjek220ForbrugerStartside()
        {
            _uut.AdminChangeWindowNewStoreCommand.Execute(this);

            Assert.That(_uut.MainWindowTekst, Is.EqualTo("Pristjek220 - Administration - Tilføj Forretning"));
        }

        [Test]
        public void ChangeWindowHomeCommand_ChangeWindowToHome_CurrentPageViewModelIsHomeModel()
        {
            _uut.AdminChangeWindowNewStoreCommand.Execute(this);

            Assert.That(_uut.CurrentPageViewModel, Is.EqualTo(_uut.PageViewModels[0]));
        }

        [Test]
        public void ChangeWindowFindProductCommand_ChangeWindowToHome_MainWindowTekstIsPristjek220ForbrugerSøgEfterVare()
        {
            _uut.AdminChangeWindowDeleteProductCommand.Execute(this);

            Assert.That(_uut.MainWindowTekst, Is.EqualTo("Pristjek220 - Administration - Fjern Produkt"));
        }

        [Test]
        public void ChangeWindowFindProductCommand_ChangeWindowToHome_CurrentPageViewModelIsFindProductModel()
        {
            _uut.AdminChangeWindowDeleteProductCommand.Execute(this);

            Assert.That(_uut.CurrentPageViewModel, Is.EqualTo(_uut.PageViewModels[1]));
        }

        [Test]
        public void ChangeWindowShoppingListCommand_ChangeWindowToHome_MainWindowTekstIsPristjek220ForbrugerIndkøbsliste()
        {
            _uut.AdminChangeWindowDeleteStoreCommand.Execute(this);

            Assert.That(_uut.MainWindowTekst, Is.EqualTo("Pristjek220 - Administration - Fjern Forretning"));
        }

        [Test]
        public void ChangeWindowShoppingListCommand_ChangeWindowToHome_CurrentPageViewModelIsShoppingListModel()
        {
            _uut.AdminChangeWindowDeleteStoreCommand.Execute(this);

            Assert.That(_uut.CurrentPageViewModel, Is.EqualTo(_uut.PageViewModels[2]));
        }
    }
}
