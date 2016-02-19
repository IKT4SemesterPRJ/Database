using System.Linq;
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
                db.AddProductToDatabase("Mælk");

                Assert.That(db.FindProduct("Mælk").ProductName, Is.EqualTo("Mælk"));

                db.RemoveProductFromDatabase("Mælk");
            }
        }

        //Test af AddProduct hvor vare findes i forvejen 

        //Test af RemoveProduct hvor vare findes

        //Test af RemoveProduct hvor der er mange relations til forretninger

        //Test af RemoveProduct hvor varen ikke findes



        [Test]
        public void AddStore_AddBilkaToTheDatabase_BilkaIsAddedToTheDatabase()
        {
            using (var db = new Database.Context())
            {
                db.AddStoreToDatabase("Bilka");

                Assert.That(db.FindStore("Bilka").StoreName, Is.EqualTo("Bilka"));

                db.RemoveStoreFromDatabse("Bilka");
            }
        }

        //Test af AddStore Hvor forretningen allerede findes

        //Test af RemoveStore Hvor forretningen findes


        //Test af RemoveStore, hvor forretningen har flere relationer til vare
        
        //Test af RemoveStore hvor forretningen ikke findes

        [Test]
        public void AddStoreProduct_AddRelationBetweenMælkAndBilka_StoreProductBetweenMælkAndBilkaIsAddedToTheDatabase()
        {
            using (var db = new Database.Context())
            {
                db.AddProductToDatabase("Fisk");
                db.AddStoreToDatabase("Aldi");
                db.AddStoreProductRelationToDatabase("Fisk", "Aldi", 9.95);

                Assert.That(db.FindStoreProduct("Aldi", "Fisk").Price, Is.EqualTo(9.95));

                db.RemoveStoreProductFromDatabse("Aldi", "Fisk");
                db.RemoveProductFromDatabase("Fisk");
                db.RemoveStoreFromDatabse("Aldi");
            }
            
        }

        //Test af AddStoreProduct, hvor den allerede findes

        //Test af RemoveStoreProduct, hvor den findes

        //Test af RemoveStoreProduct, hvor den ikke findes

        //Test af FindProduct, hvor den findes

        //Test af FindProduct, hvor den ikke findes

        //Test af FindStore, hvor den findes

        //Test af FindStore, hvor den ikke findes

        //Test af FindStoreProduct, hvor den findes

        //Test af FindStoreProduct, hvor varen ikke findes

        //Test af FindStoreProduct, hvor forretningen ikke findes

        //Test af ChangePrice, hvor varen findes i forretning

        //Test af ChangePrice, hvor varen ikke findes i forretningen


    }
}
