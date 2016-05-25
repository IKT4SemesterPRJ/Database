using Consumer_GUI;
using NUnit.Framework;
using Pristjek220Data;
using SharedFunctionalities;

namespace Pristjek220.Integrationstest
{
    [TestFixture]
    class ConsumerViewModelIntergrationsTest
    {
        private ConsumerViewModel _consumerView;
        private IDatabaseFunctions _databaseFunctions;
        private IUnitOfWork _unit;
        private DataContext _context;

        [SetUp]
        public void SetUp()
        {
            
            _context = new DataContext();
            _unit = new UnitOfWork(_context);
            
            _context.Database.Connection.ConnectionString =
                "Server=.\\SQLEXPRESS;Database=Pristjek220Data.DataContext; Trusted_Connection=True;MultipleActiveResultSets=True;";
            _context.Database.ExecuteSqlCommand("dbo.TestCleanTable");

            _databaseFunctions = new DatabaseFunctions(_unit);

            _consumerView = new ConsumerViewModel(new global::Consumer.Consumer(_unit), new Autocomplete(_unit), _databaseFunctions  );
            
        }


        [Test]
        public void ChangeWindowHome_SimulateBtnClick_CurrrentPageViewModelEqualsHome()
        {
            
            _consumerView.ChangeWindowHomeCommand.Execute(this);
            
            Assert.That(_consumerView.CurrentPageViewModel, Is.EqualTo(_consumerView.PageViewModels[0]));
        }

        [Test]
        public void ChangeWindowFindProduct_SimulateBtnClick_CurrrentPageViewModelEqualsFindProduct()
        {

            _consumerView.ChangeWindowFindProductCommand.Execute(this);

            Assert.That(_consumerView.CurrentPageViewModel, Is.EqualTo(_consumerView.PageViewModels[1]));
        }

        [Test]
        public void ChangeWindowShoppingList_SimulateBtnClick_CurrrentPageViewModelEqualsShoppingList()
        {

            _consumerView.ChangeWindowShoppingListCommand.Execute(this);

            Assert.That(_consumerView.CurrentPageViewModel, Is.EqualTo(_consumerView.PageViewModels[2]));
        }

        [Test]
        public void ChangeWindowGeneratedShopppingList_SimulateBtnClick_CurrrentPageViewModelEqualsGeneratedShopppingList()
        {

            _consumerView.ChangeWindowGeneratedShoppingListCommand.Execute(this);

            Assert.That(_consumerView.CurrentPageViewModel, Is.EqualTo(_consumerView.PageViewModels[3]));
        }

    }
}
