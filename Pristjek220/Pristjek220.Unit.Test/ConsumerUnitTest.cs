using System;
using System.Collections.ObjectModel;
using System.IO;
using Consumer;
using NSubstitute;
using NUnit.Framework;
using Pristjek220Data;


namespace Pristjek220.Unit.Test
{
    [TestFixture]
    public class ConsumerUnitTest
    {
        private IUnitOfWork _unitWork;
        private Consumer.Consumer _uut;
        private Store _store;
        private Product _product;

        [SetUp]
        public void SetUp()
        {
            _unitWork = Substitute.For<IUnitOfWork>();
            _store = new Store() { StoreName = "Aldi" , StoreId = 22};
            _product = new Product() {ProductName = "Banan", ProductId = 10};
            _uut = new Consumer.Consumer(_unitWork);
        }

        [Test]
        public void DoesProductExsist_DoesBananExsist_ReturnTrue()
        {
            _unitWork.Products.FindProduct(_product.ProductName).Returns(_product);

            Assert.That(_uut.DoesProductExist(_product.ProductName), Is.EqualTo(true));
        }

        [Test]
        public void DoesProductExsist_DoesBananExsist_ReturnFalse()
        {
            _unitWork.Products.FindProduct(_product.ProductName).Returns((Product) null);

            Assert.That(_uut.DoesProductExist(_product.ProductName), Is.EqualTo(false));
        }

        [Test]
        public void FindCheapestStore_FindCheapestStoreForBananButBananIsNotInDb_ReturnNull()
        {
            _unitWork.Products.FindProduct(_product.ProductName).Returns((Product) null);

            Assert.That(_uut.FindCheapestStore(_product.ProductName), Is.EqualTo(null));
        }

        [Test]
        public void FindCheapestStore_FindCheapestStoreForBananButBananHaveNoRelationToAStore_ReturnNull()
        {
            _unitWork.Products.FindProduct(_product.ProductName).Returns(_product);

            Assert.That(_uut.FindCheapestStore(_product.ProductName), Is.EqualTo(null));
        }

        [Test]
        public void FindCheapestStore_FindCheapestStoreForBanan_ReturnsStore()
        {
            var fakta = new Store() {StoreName = "Fakta"};
            _unitWork.Products.FindProduct(_product.ProductName).Returns(_product);
            _product.HasARelation.Add(new HasA() {Price = 2.95, Store = _store});
            _product.HasARelation.Add(new HasA() { Price = 1.95, Store = fakta });

            Assert.That(_uut.FindCheapestStore(_product.ProductName), Is.EqualTo(fakta));
        }

        [Test]
        public void FindStoreAssortment_FindAldisAssortment_FindProductsInStoreFunctionCalled()
        {
            _uut.FindStoresAssortment(_store.StoreName);

            _unitWork.Stores.Received(1).FindProductsInStore(_store.StoreName);
        }

        [Test]
        public void FindStoreThatSells_FindWhichStoreSellsBanan_FunctionToGenerateListCalled()
        {
            _uut.FindStoresThatSellsProduct(_product.ProductName);

            _unitWork.Products.Received(1).FindStoresThatSellsProduct(_product.ProductName);
        }

        //[Test]
        //public void CreateShoppingList_CreateShoppingListForBanan_ListHasCorrectProductName()
        //{
        //    var shoppingList = new List<string> { _product.ProductName };

        //    var fakta = new Store() { StoreName = "Fakta" };
        //    _unitWork.Products.FindProduct(_product.ProductName).Returns(_product);
        //    _product.HasARelation.Add(new HasA() { Price = 2.95, Store = _store });
        //    _product.HasARelation.Add(new HasA() { Price = 1.95, Store = fakta });
        //    _store.HasARelation.Add(new HasA() {Price = 2.95, Product = _product, Store = _store});
        //    fakta.HasARelation.Add(new HasA() { Price = 1.95, Product = _product, Store = fakta });

        //    var createdShoppingList = _uut.CreateShoppingList(shoppingList);

        //    Assert.That(createdShoppingList.Find(x => x.ProductName == _product.ProductName).ProductName,
        //        Is.EqualTo("Banan"));
        //}

        //[Test]
        //public void CreateShoppingList_CreateShoppingListForBanan_ListHasCorrectStoreName()
        //{
        //    var shoppingList = new List<string> { _product.ProductName };

        //    var fakta = new Store() { StoreName = "Fakta" };
        //    _unitWork.Products.FindProduct(_product.ProductName).Returns(_product);
        //    _product.HasARelation.Add(new HasA() { Price = 2.95, Store = _store });
        //    _product.HasARelation.Add(new HasA() { Price = 1.95, Store = fakta });
        //    _store.HasARelation.Add(new HasA() { Price = 2.95, Product = _product, Store = _store });
        //    fakta.HasARelation.Add(new HasA() { Price = 1.95, Product = _product, Store = fakta });

        //    var createdShoppingList = _uut.CreateShoppingList(shoppingList);

        //    Assert.That(createdShoppingList.Find(x => x.ProductName == _product.ProductName).StoreName,
        //        Is.EqualTo(fakta.StoreName));
        //}

        //[Test]
        //public void CreateShoppingList_CreateShoppingListForBanan_ListHasCorrectPrice()
        //{
        //    var shoppingList = new List<string> { _product.ProductName };

        //    var fakta = new Store() { StoreName = "Fakta" };
        //    _unitWork.Products.FindProduct(_product.ProductName).Returns(_product);
        //    _product.HasARelation.Add(new HasA() { Price = 2.95, Store = _store });
        //    _product.HasARelation.Add(new HasA() { Price = 1.95, Store = fakta });
        //    _store.HasARelation.Add(new HasA() { Price = 2.95, Product = _product, Store = _store });
        //    fakta.HasARelation.Add(new HasA() { Price = 1.95, Product = _product, Store = fakta });

        //    var createdShoppingList = _uut.CreateShoppingList(shoppingList);

        //    Assert.That(createdShoppingList.Find(x => x.ProductName == _product.ProductName).Price,
        //        Is.EqualTo(fakta.HasARelation.Find(x => x.Product.ProductName == _product.ProductName).Price));
        //}

        [Test]
        public void WriteToJsonFile_AddAProductAndWriteItTOJson_ReadTheAddedProductInJson()
        {
            ObservableCollection<ProductInfo> productInfos = new ObservableCollection<ProductInfo>();
            productInfos.Add(new ProductInfo("test"));
            _uut.ShoppingListData = productInfos;
            _uut.WriteToJsonFile();
            string path = Path.Combine(Environment.ExpandEnvironmentVariables("%userprofile%"), "Documents") + @"\Pristjek220";
            string stringFromFile = string.Empty;
            using (var file = File.OpenText(path + @"\Shoppinglist.json"))
            {
                stringFromFile = file.ReadLine();
            }
            Assert.That(stringFromFile, Is.EqualTo("[{\"Name\":\"Test\",\"Quantity\":\"1\"}]"));
        }

        [Test]
        public void WriteToJsonFile_AddAProductAndGetList_ReadTheAddedProductInJson()
        {
            ObservableCollection<ProductInfo> productInfos = new ObservableCollection<ProductInfo>();
            productInfos.Add(new ProductInfo("test"));
            _uut.ShoppingListData = productInfos;
            var test = _uut.ShoppingListData;
            string path = Path.Combine(Environment.ExpandEnvironmentVariables("%userprofile%"), "Documents") + @"\Pristjek220";
            string stringFromFile = string.Empty;
            using (var file = File.OpenText(path + @"\Shoppinglist.json"))
            {
                stringFromFile = file.ReadLine();
            }
            Assert.That(stringFromFile, Is.EqualTo("[{\"Name\":\"Test\",\"Quantity\":\"1\"}]"));
        }



        [Test]
        public void ReadFromJsonFile_FindWhichStoreSellsBanan_FunctionToGenerateListCalled()
        {
            ObservableCollection<ProductInfo> productInfos = new ObservableCollection<ProductInfo>();
            ProductInfo test = new ProductInfo("test");
            productInfos.Add(test);
            _uut.ShoppingListData = productInfos;
            _uut.WriteToJsonFile();
            _uut.ReadFromJsonFile();
            Assert.That(productInfos[0], Is.EqualTo(test));
        }

        [Test]
        public void ReadFromJsonFile_ReadFromAFileThatDoesNotExist_ExceptionIscatchedAndListStillGotSameValues()
        {
            ObservableCollection<ProductInfo> productInfos = new ObservableCollection<ProductInfo>();
            ProductInfo test = new ProductInfo("test");
            productInfos.Add(test);
            _uut.ShoppingListData = productInfos;
            string path = Path.Combine(Environment.ExpandEnvironmentVariables("%userprofile%"), "Documents") + @"\Pristjek220" + @"\Shoppinglist.json";
            File.Delete(path);
            _uut.ReadFromJsonFile();
            Assert.That(_uut.ShoppingListData[0], Is.EqualTo(test));
        }
        [Test]
        public void ClearGeneratedShoppingListData_AddOneItemToListCallClear_ItemHasBeenDeleted()
        {
            StoreProductAndPrice storeProductAndPriceItem = new StoreProductAndPrice();
            storeProductAndPriceItem.ProductName = "test";
            storeProductAndPriceItem.Price = 2;
            storeProductAndPriceItem.Quantity = "2";
            storeProductAndPriceItem.StoreName = "Aldi";
            storeProductAndPriceItem.Sum = 4;

            _uut.GeneratedShoppingListData.Add(storeProductAndPriceItem);
            _uut.ClearGeneratedShoppingListData();
            Assert.That(_uut.GeneratedShoppingListData.Count, Is.EqualTo(0));
        }
        [Test]
        public void ClearNotInAStore_AddOneItemToListCallClear_ItemHasBeenDeleted()
        {
            _uut.NotInAStore.Add(new ProductInfo("test"));
            _uut.ClearNotInAStore();
            Assert.That(_uut.NotInAStore.Count, Is.EqualTo(0));
        }

    }
}