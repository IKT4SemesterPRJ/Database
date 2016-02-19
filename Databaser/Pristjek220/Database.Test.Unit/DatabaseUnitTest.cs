using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;

namespace Database.Test.Unit
{
    [TestFixture]
    public class DatabaseUnitTest
    {
        [Test]
        public void AddProduct_AddMælkToTheDatabse_MælkIsAddedToTheDatabase()
        {
            using (var db = new Database.Context())
            {
                var product = new Product() {ProductName = "Mælk"};
                db.Products.Add(product);
                db.SaveChanges();

                product = (from t in db.Products where t.ProductName == "Mælk" select t).FirstOrDefault();

                Assert.That(product.ProductName, Is.EqualTo("Mælk"));

                db.Products.Remove(product);
            }
        }

        [Test]
        public void AddStore_AddBilkaToTheDatabase_BilkaIsAddedToTheDatabase()
        {
            using (var db = new Database.Context())
            {
                var store = new Store() { StoreName = "Bilka" };
                db.Stores.Add(store);
                db.SaveChanges();

                store = (from t in db.Stores where t.StoreName == "Bilka" select t).FirstOrDefault();

                Assert.That(store.StoreName, Is.EqualTo("Bilka"));

                db.Stores.Remove(store);
            }
        }

        [Test]
        public void AddStoreProduct_AddRelationBetweenMælkAndBilka_StoreProductBetweenMælkAndBilkaIsAddedToTheDatabase()
        {
            
        }
    }
}
