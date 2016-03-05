using System;
using System.Linq;
using NUnit.Framework;

namespace Database.Test.Unit
{
    [TestFixture]
    public class StoremanagerUnitTest
    {
        [Test]
        public void StoremangerForAldiAddProduct_AddBananToAldi_BananCanBefoundInTheDatabase()
        {
            using (var _uut = new Context())
            {
                var storeman = new Storemanager("Aldi");

                storeman.AddProduct("Banan", 2.95);

                var product = (from t in _uut.Products where t.ProductName == "Banan" select t).FirstOrDefault();

                Assert.That(product.ProductName, Is.EqualTo("Banan"));

                _uut.Stores.Remove(_uut.Stores.Find(product.StoreProducts.First().Store.StoreId));
                _uut.Products.Remove(product);
                _uut.SaveChanges();
            }
        }

        [Test]
        public void StoreManagerForFøtexAddProduct_AddFiskToFøtex_RelationBetweenFøtexAndFiskCanBeFoundInTheDatabase()
        {
            using (var _uut = new Context())
            {
                var storeman = new Storemanager("Føtex");

                storeman.AddProduct("Fisk", 10.66);

                var relation = (from t in _uut.StoreProducts where Math.Abs(t.Price - 10.66) < 0.1 select t).FirstOrDefault();

                Assert.That(relation.Price, Is.EqualTo(10.66));

                _uut.Products.Remove(_uut.Products.Find(relation.ProductId));
                _uut.Stores.Remove(_uut.Stores.Find(storeman.Store.StoreId));
                _uut.SaveChanges();
            }
        }

        [Test]
        public void StoreManagerForLidlAddProduct_AddFiskToLidlWhereLidlAlreadyGotFisk_AddProductReturnsMinusOne()
        {
            using (var _uut = new Context())
            {
                var storeman = new Storemanager("Lidl");

                storeman.AddProduct("Fisk", 10.66);

                Assert.That(storeman.AddProduct("Fisk", 12.00), Is.EqualTo(-1));

                var product = (from t in _uut.Products where t.ProductName == "Fisk" select t).FirstOrDefault();

                _uut.Products.Remove(product);
                _uut.Stores.Remove(_uut.Stores.Find(storeman.Store.StoreId));
                _uut.SaveChanges();   
            }
        }

        [Test]
        public void
            StoreManagerForFaktaAddProduct_AddTomatToFaktaWhereTomatExistsButNotInFakta_RelationBetweenFaktaAndTomatCanBeFound
            ()
        {
            using (var _uut = new Context())
            {
                var storeman = new Storemanager("Fakta");

                _uut.Products.Add(new Product() {ProductName = "Tomat"});
                _uut.SaveChanges();
                storeman.AddProduct("Tomat", 5.95);

                Assert.That(_uut.StoreProducts.FirstOrDefault().ProductId, Is.EqualTo(_uut.Products.FirstOrDefault().ProductId));

                var product = (from t in _uut.Products where t.ProductName == "Tomat" select t).FirstOrDefault();

                _uut.Products.Remove(product);
                _uut.Stores.Remove(_uut.Stores.Find(storeman.Store.StoreId));
                _uut.SaveChanges();
            }
        }

        /*
        [Test]
        public void AddProduct_AddMælkToTheDatabse_MælkIsAddedToTheDatabase()
        {
            using (var _uut = new Database.Context())
            {
                _uut.AddProductToDatabase("Mælk");

                Assert.That(_uut.FindProduct("Mælk").ProductName, Is.EqualTo("Mælk"));

                var product = (from t in _uut.Products where t.ProductName == "Mælk" select t).FirstOrDefault();

                _uut.Products.Remove(product);
                _uut.SaveChanges();
            }
        }

        //Test af AddProduct hvor vare findes i forvejen 
        [Test]
        public void AddProduct_TheProductAlreadyExsistInTheDb_Return()
        {
            using (var _uut = new Context())
            {
                //_uut.AddProductToDatabase("Smør");

                //Assert.That(_uut.AddProductToDatabase("Smør"), Is.EqualTo("Product exsist"));
            }
        }

        //Test af RemoveProduct hvor vare findes
        [Test]
        public void RemoveProductFromDatabse_RemoveBananFromDatabase_BananCantBeFoundInTheDatabase()
        {
            using (var _uut = new Context())
            {
                _uut.Products.Add(new Product() {ProductName = "Banan"});
                _uut.SaveChanges();
                _uut.RemoveProductFromDatabase("Banan");
                _uut.SaveChanges();
                var product = (from t in _uut.Products where t.ProductName == "Banan" select t).FirstOrDefault();

                Assert.That(product, Is.EqualTo(null));
            }
        }

        //Test af RemoveProduct hvor der er mange relations til forretninger
        [Test]
        public void
            RemoveProductFromDatabaseWithRelations_RemoveAgurkFromDatabase_AgurkHasBeenRemovedAndAllTheRelationsToo()
        {
            using (var _uut = new Context())
            {
                var banan = new Product() {ProductName = "Banan"};
                var bilka = new Store() {StoreName = "Bilka"};
                var aldi = new Store() {StoreName = "Aldi"};
                var fakta = new Store() {StoreName = "Fakta"};
                var bananBilka = new StoreProduct() { Price = 2.95, Product = banan, Store = bilka, ProductId = banan.ProductId, StoreId = bilka.StoreId};
                var bananAldi = new StoreProduct() { Price = 3.95, Product = banan, Store = aldi, ProductId = banan.ProductId, StoreId = aldi.StoreId };
                var bananFakta = new StoreProduct() { Price = 4.95, Product = banan, Store = fakta, ProductId = banan.ProductId, StoreId = fakta.StoreId };


                _uut.Products.Add(banan);
                _uut.Stores.Add(bilka);
                _uut.Stores.Add(aldi);
                _uut.Stores.Add(fakta);

                _uut.StoreProducts.Add(bananBilka);
                _uut.StoreProducts.Add(bananAldi);
                _uut.StoreProducts.Add(bananFakta);
                _uut.SaveChanges();

                _uut.RemoveProductFromDatabase("Banan");

                var bananBilka1 = (from t in _uut.StoreProducts where t.ProductId == banan.ProductId && t.StoreId == bilka.StoreId select t).FirstOrDefault();
                var bananAldi1 = (from t in _uut.StoreProducts where t.ProductId == banan.ProductId && t.StoreId == aldi.StoreId select t).FirstOrDefault();
                var bananFakta1 = (from t in _uut.StoreProducts where t.ProductId == banan.ProductId && t.StoreId == fakta.StoreId select t).FirstOrDefault();

                Assert.That(_uut.Products.Find(banan.ProductId), Is.EqualTo(null));
                Assert.That(bananBilka1, Is.EqualTo(null));
                Assert.That(bananAldi1, Is.EqualTo(null));
                Assert.That(bananFakta1, Is.EqualTo(null));
                Assert.That(_uut.Stores.Find(aldi.StoreId), Is.EqualTo(aldi));
                Assert.That(_uut.Stores.Find(bilka.StoreId), Is.EqualTo(bilka));
                Assert.That(_uut.Stores.Find(fakta.StoreId), Is.EqualTo(fakta));

                _uut.Stores.Remove(bilka);
                _uut.Stores.Remove(aldi);
                _uut.Stores.Remove(fakta);
                _uut.SaveChanges();
            }
        }

        //Test af RemoveProduct hvor varen ikke findes



        [Test]
        public void AddStore_AddBilkaToTheDatabase_BilkaIsAddedToTheDatabase()
        {
            using (var _uut = new Database.Context())
            {
                _uut.AddStoreToDatabase("Bilka");

                Assert.That(_uut.FindStore("Bilka").StoreName, Is.EqualTo("Bilka"));

                _uut.RemoveStoreFromDatabse("Bilka");
            }
        }

        //Test af AddStore Hvor forretningen allerede findes

        //Test af RemoveStore Hvor forretningen findes


        //Test af RemoveStore, hvor forretningen har flere relationer til vare
        
        //Test af RemoveStore hvor forretningen ikke findes

        [Test]
        public void AddStoreProduct_AddRelationBetweenMælkAndBilka_StoreProductBetweenMælkAndBilkaIsAddedToTheDatabase()
        {
            using (var _uut = new Database.Context())
            {
                _uut.AddProductToDatabase("Fisk");
                _uut.AddStoreToDatabase("Aldi");
                _uut.AddStoreProductRelationToDatabase("Fisk", "Aldi", 9.95);

                Assert.That(_uut.FindStoreProduct("Aldi", "Fisk").Price, Is.EqualTo(9.95));

                _uut.RemoveStoreProductFromDatabse("Aldi", "Fisk");
                _uut.RemoveProductFromDatabase("Fisk");
                _uut.RemoveStoreFromDatabse("Aldi");
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

    */
    }
}