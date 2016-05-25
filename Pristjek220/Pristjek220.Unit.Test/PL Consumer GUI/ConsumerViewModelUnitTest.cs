using Consumer;
using Consumer_GUI;
using NSubstitute;
using NUnit.Framework;
using SharedFunctionalities;


namespace Pristjek220.Unit.Test
{
    [TestFixture]
    class ConsumerViewModelUnitTest
    {
        private ConsumerViewModel _uut;
        private IConsumer _consumer;
        private IAutocomplete _autocomplete;
        private IDatabaseFunctions _databaseFunctions;




        [SetUp]
        public void SetUp()
        {
            _consumer = Substitute.For<IConsumer>();
            _autocomplete = Substitute.For<IAutocomplete>();
            _databaseFunctions = Substitute.For<IDatabaseFunctions>();
            _databaseFunctions.ConnectToDb().Returns(true);
            _uut = new ConsumerViewModel(_consumer, _autocomplete, _databaseFunctions);

        }

        [Test]
        public void ChangeWindowHomeCommand_ChangeWindowToHome_MainWindowTekstIsPristjek220ForbrugerStartside()
        {
            _uut.ChangeWindowHomeCommand.Execute(this);

            Assert.That(_uut.MainWindowTekst, Is.EqualTo("Pristjek220 - Forbruger - Startside"));
        }

        [Test]
        public void ChangeWindowHomeCommand_ChangeWindowToHome_CurrentPageViewModelIsHomeModel()
        {
            _uut.ChangeWindowHomeCommand.Execute(this);

            Assert.That(_uut.CurrentPageViewModel, Is.EqualTo(_uut.PageViewModels[0]));
        }

        [Test]
        public void ChangeWindowFindProductCommand_ChangeWindowToHome_MainWindowTekstIsPristjek220ForbrugerSøgEfterVare()
        {
            _uut.ChangeWindowFindProductCommand.Execute(this);

            Assert.That(_uut.MainWindowTekst, Is.EqualTo("Pristjek220 - Forbruger - Søg efter vare"));
        }

        [Test]
        public void ChangeWindowFindProductCommand_ChangeWindowToHome_CurrentPageViewModelIsFindProductModel()
        {
            _uut.ChangeWindowFindProductCommand.Execute(this);

            Assert.That(_uut.CurrentPageViewModel, Is.EqualTo(_uut.PageViewModels[1]));
        }

        [Test]
        public void ChangeWindowShoppingListCommand_ChangeWindowToHome_MainWindowTekstIsPristjek220ForbrugerIndkøbsliste()
        {
            _uut.ChangeWindowShoppingListCommand.Execute(this);

            Assert.That(_uut.MainWindowTekst, Is.EqualTo("Pristjek220 - Forbruger - Indkøbsliste"));
        }

        [Test]
        public void ChangeWindowShoppingListCommand_ChangeWindowToHome_CurrentPageViewModelIsShoppingListModel()
        {
            _uut.ChangeWindowShoppingListCommand.Execute(this);

            Assert.That(_uut.CurrentPageViewModel, Is.EqualTo(_uut.PageViewModels[2]));
        }

        [Test]
        public void ChangeWindowGeneratedShoppingListCommand_ChangeWindowToHome_MainWindowTekstIsPristjek220ForbrugerGenereretIndkøbsliste()
        {
            _uut.ChangeWindowGeneratedShoppingListCommand.Execute(this);

            Assert.That(_uut.MainWindowTekst, Is.EqualTo("Pristjek220 - Forbruger - Genereret Indkøbsliste"));
        }

        [Test]
        public void ChangeWindowGeneratedShoppingListCommand_ChangeWindowToHome_CurrentPageViewModelIsGeneratedShoppingListModel()
        {
            _uut.ChangeWindowGeneratedShoppingListCommand.Execute(this);

            Assert.That(_uut.CurrentPageViewModel, Is.EqualTo(_uut.PageViewModels[3]));
        }


    }
}
