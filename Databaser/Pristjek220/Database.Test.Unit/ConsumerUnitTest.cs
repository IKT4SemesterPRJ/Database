using System.Linq;
using NUnit.Framework;

namespace Database.Test.Unit
{
    [TestFixture]
    class ConsumerUnitTest
    {
        private Consumer.Consumer _uut;

        [SetUp]
        public void SetUp()
        {
            _uut = new Consumer.Consumer();
        }

        [Test]
        public void FindCheapestStore_AppleIsNotInTheDatabase_ReturnNull()
        {
            Assert.That(_uut.FindCheapestStore("Apple"), Is.EqualTo(null));
        }

        [Test]
        public void FindCheapestStore_ProductApple_ReturnRelationBetweenFaktaAndApple()
        {
            var aldi = new Storemanager("Aldi");
            var føtex = new Storemanager("Føtex");
            var fakta = new Storemanager("Fakta");

            aldi.AddProduct("Apple", 4.95);
            føtex.AddProduct("Apple", 5.95);
            fakta.AddProduct("Apple", 3.95);

            Assert.That(_uut.FindCheapestStore("Apple").StoreId, Is.EqualTo(fakta.Store.StoreId));

            using (var db = new Context())
            {
                db.Stores.Remove(db.Stores.Find(aldi.Store.StoreId));
                db.Stores.Remove(db.Stores.Find(føtex.Store.StoreId));
                db.Stores.Remove(db.Stores.Find(fakta.Store.StoreId));

                var product = (from t in db.Products where t.ProductName == "Apple" select t).FirstOrDefault();

                db.Products.Remove(product);
                db.SaveChanges();
            }
        }
    }
}
