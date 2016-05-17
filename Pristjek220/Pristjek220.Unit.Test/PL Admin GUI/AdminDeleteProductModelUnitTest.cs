using System.Collections.Generic;
using System.Threading;
using Administration;
using Administration_GUI.User_Controls;
using Administration_GUI.User_Controls_Admin;
using NSubstitute;
using NUnit.Framework;
using Pristjek220Data;
using SharedFunctionalities;

namespace Pristjek220.Unit.Test
{
    [TestFixture]
    class AdminDeleteProductModelUnitTest
    {
        private AdminDeleteProductModel _uut;
        private IUnitOfWork _unit;


        [SetUp]
        public void SetUp()
        {
            _unit = Substitute.For<IUnitOfWork>();
            _uut = new AdminDeleteProductModel(_unit);
        }

        [Test]
        public void ReadyForTest()
        {
            Assert.That(2, Is.EqualTo(2));
        }
    }
}
