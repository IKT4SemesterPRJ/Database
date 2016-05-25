using Administration;
using Administration_GUI;
using NSubstitute;
using NUnit.Framework;
using Pristjek220Data;
using SharedFunctionalities;


namespace Pristjek220.Unit.Test
{
    [TestFixture]
    class StoremanagerViewModelUnitTest
    {
        private StoremanagerViewModel _uut;
        private IStoremanager _storemanager;
        private IAutocomplete _autocomplete;

        [SetUp]
        public void SetUp()
        {
            _storemanager = Substitute.For<IStoremanager>();
            _storemanager.Store = new Store() {StoreName = "Lidl", StoreId = 12};
            _autocomplete = Substitute.For<IAutocomplete>();
            _uut = new StoremanagerViewModel(_storemanager, _autocomplete);
        }

        [Test]
        public void ChangeWindowHomeCommand_ChangeWindowToHome_MainWindowTekstIsPristjek220ForbrugerStartside()
        {
            _uut.ChangeWindowChangePriceCommand.Execute(this);

            Assert.That(_uut.MainWindowTekst, Is.EqualTo($"Pristjek220 - {_storemanager.Store.StoreName} - Ændre pris"));
        }

        [Test]
        public void ChangeWindowHomeCommand_ChangeWindowToHome_CurrentPageViewModelIsHomeModel()
        {
            _uut.ChangeWindowChangePriceCommand.Execute(this);

            Assert.That(_uut.CurrentPageViewModel, Is.EqualTo(_uut.PageViewModels[0]));
        }

        [Test]
        public void ChangeWindowFindProductCommand_ChangeWindowToHome_MainWindowTekstIsPristjek220ForbrugerSøgEfterVare()
        {
            _uut.ChangeWindowDeleteProductCommand.Execute(this);

            Assert.That(_uut.MainWindowTekst, Is.EqualTo($"Pristjek220 - {_storemanager.Store.StoreName} - Fjern Produkt"));
        }

        [Test]
        public void ChangeWindowFindProductCommand_ChangeWindowToHome_CurrentPageViewModelIsFindProductModel()
        {
            _uut.ChangeWindowDeleteProductCommand.Execute(this);

            Assert.That(_uut.CurrentPageViewModel, Is.EqualTo(_uut.PageViewModels[1]));
        }

        [Test]
        public void ChangeWindowShoppingListCommand_ChangeWindowToHome_MainWindowTekstIsPristjek220ForbrugerIndkøbsliste()
        {
            _uut.ChangeWindowNewProductCommand.Execute(this);

            Assert.That(_uut.MainWindowTekst, Is.EqualTo($"Pristjek220 - {_storemanager.Store.StoreName} - Tilføj Produkt"));
        }

        [Test]
        public void ChangeWindowShoppingListCommand_ChangeWindowToHome_CurrentPageViewModelIsShoppingListModel()
        {
            _uut.ChangeWindowNewProductCommand.Execute(this);

            Assert.That(_uut.CurrentPageViewModel, Is.EqualTo(_uut.PageViewModels[2]));
        }
    }
}
