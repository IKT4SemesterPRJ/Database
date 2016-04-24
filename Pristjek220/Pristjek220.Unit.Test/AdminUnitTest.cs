using System.Security;
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
        public void CreateLogin_CreateTheStoreToTheLogin_StoreIsCreatedAndSavedInDatabase()
        {
            _uut.CreateLogin("Test", new SecureString(), "Test");

            _unitOfWork.Stores.Received(1).Add(Arg.Any<Store>());
        }

        [Test]
        public void CreateLogin_CreateTheLogin_LoginIsCreatedAndSavedInDatabase()
        {
            _uut.CreateLogin("Test", new SecureString(), "Test");

            _unitOfWork.Logins.Received(1).Add(Arg.Any<Login>());
        }
    }
}
