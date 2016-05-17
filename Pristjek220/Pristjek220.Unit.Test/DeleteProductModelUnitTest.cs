using Administration_GUI.User_Controls;
using NSubstitute;
using NUnit.Framework;
using Pristjek220Data;

namespace Pristjek220.Unit.Test
{
    [TestFixture]
    class DeleteProductModelUnitTest
    {
        private DeleteProductModel _deleteProduct;
        private Store _store;

        [SetUp]
        public void SetUp()
        {
            _store = new Store() { StoreName = "Aldi", StoreId = 22 };
            _deleteProduct = new DeleteProductModel(_store, Substitute.For<IUnitOfWork>());
        }
        [Test]
        public void ShoppingListItemPrice_Insert2point224_IsRoundedTo2point22()
        {

        }
    }
}
