using Administration;
using NSubstitute;
using NUnit.Framework;
using Pristjek220Data;

namespace Pristjek220.Unit.Test
{
    [TestFixture]
    class AdminUnitTest
    {
        private IAdmin _uut;
        private IUnitOfWork _unitOfWork;

        [SetUp]
        public void SetUp()
        {
            _unitOfWork = Substitute.For<IUnitOfWork>();
            _uut = new Admin(_unitOfWork);
        }

        [Test]
        public void AddStore_AddAStoreToDatabase_AddStoreReturns1()
        {
            var store = new Store() {StoreName = "Fakta"};
            _uut.AddStore(store);
            _unitOfWork.Stores.Received(1).Add(store);
        }

        [Test]
        public void AddStore_AddfakTaToDb_FaktaAdded()
        {
            var store = new Store() {StoreName = "fakTa"};
            _uut.AddStore(store);

            Assert.That(store.StoreName, Is.EqualTo("Fakta"));
        }
    }
}
