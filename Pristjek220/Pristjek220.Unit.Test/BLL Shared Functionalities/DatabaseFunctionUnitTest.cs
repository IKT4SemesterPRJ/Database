using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NSubstitute;
using NUnit.Framework.Internal;
using NUnit.Framework;
using Pristjek220Data;
using SharedFunctionalities;

namespace Pristjek220.Unit.Test
{
    [TestFixture]
    class DatabaseFunctionUnitTest
    {
        private IDatabaseFunctions databaseFunctions;
        private IUnitOfWork _unitWork;

        [Test]
        public void Constructor_setWithUnitOfWork_()
        {
            _unitWork = Substitute.For<IUnitOfWork>();
            databaseFunctions = new DatabaseFunctions(_unitWork);
            databaseFunctions.ConnectToDb();
            _unitWork.Products.Received(1).ConnectToDb();
        }
    }
}
