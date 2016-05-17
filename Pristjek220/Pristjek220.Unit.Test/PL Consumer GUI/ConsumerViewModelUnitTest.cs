using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Consumer;
using Consumer_GUI;
using Consumer_GUI.User_Controls;
using NSubstitute;
using NSubstitute.ReturnsExtensions;
using NUnit.Framework;
using Pristjek220Data;
using SharedFunctionalities;


namespace Pristjek220.Unit.Test
{
    [TestFixture]
    class ConsumerViewModelUnitTest
    {
        private ConsumerViewModel _uut;


        [SetUp]
        public void SetUp()
        {
            _uut = new ConsumerViewModel();
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
