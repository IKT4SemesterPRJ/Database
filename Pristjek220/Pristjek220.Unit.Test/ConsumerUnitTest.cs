using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
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
            _store = new Store() {StoreName = "Aldi", StoreId = 22};
            _product = new Product() {ProductName = "Banan", ProductId = 10};
            _unitWork.Stores.GetAllStores().Returns(new List<Store>() {new Store() {StoreName = "Fakta"}});
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
            _product.HasARelation.Add(new HasA() {Price = 1.95, Store = fakta});

            Assert.That(_uut.FindCheapestStore(_product.ProductName), Is.EqualTo(fakta));
        }

        [Test]
        public void FindStoreThatSells_FindWhichStoreSellsBanan_FunctionToGenerateListCalled()
        {
            _uut.FindStoresThatSellsProduct(_product.ProductName);

            _unitWork.Products.Received(1).FindStoresThatSellsProduct(_product.ProductName);
        }


        [Test]
        public void CreateShoppingList_CreateShoppingListForBanan_ListHasCorrectPrice() // Skal fejle, FOR JENKINS!!!
        {
            _uut.ShoppingListData.Add(new ProductInfo(_product.ProductName));

            var fakta = new Store() {StoreName = "Fakta"};
            _unitWork.Products.FindProduct(_product.ProductName).Returns(_product);
            _product.HasARelation.Add(new HasA() {Price = 2.95, Store = _store});
            _product.HasARelation.Add(new HasA() {Price = 1.95, Store = fakta});
            _store.HasARelation.Add(new HasA() {Price = 2.95, Product = _product, Store = _store});
            fakta.HasARelation.Add(new HasA() {Price = 1.95, Product = _product, Store = fakta});
            _uut.CreateShoppingList();

            Assert.That(_uut.TotalSum, Is.EqualTo(double.Parse("1,95", new CultureInfo("da-DK")).ToString() + " kr"));
        }

        [Test]
        public void CreateShoppingList_CreateShoppingListForBanan_ListHasCorrectProductName()
        {
            _uut.ShoppingListData.Add(new ProductInfo(_product.ProductName));

            var fakta = new Store() {StoreName = "Fakta"};
            _unitWork.Products.FindProduct(_product.ProductName).Returns(_product);
            _product.HasARelation.Add(new HasA() {Price = 2.95, Store = _store});
            _product.HasARelation.Add(new HasA() {Price = 1.95, Store = fakta});
            _store.HasARelation.Add(new HasA() {Price = 2.95, Product = _product, Store = _store});
            fakta.HasARelation.Add(new HasA() {Price = 1.95, Product = _product, Store = fakta});
            _uut.CreateShoppingList();

            Assert.That(_uut.GeneratedShoppingListData[0].ProductName, Is.EqualTo("Banan"));
        }

        [Test]
        public void CreateShoppingList_CreateShoppingListForBanan_ListHasCorrectStoreName()
        {
            _uut.ShoppingListData.Add(new ProductInfo(_product.ProductName));

            var fakta = new Store() {StoreName = "Fakta"};
            _unitWork.Products.FindProduct(_product.ProductName).Returns(_product);
            _product.HasARelation.Add(new HasA() {Price = 2.95, Store = _store});
            _product.HasARelation.Add(new HasA() {Price = 1.95, Store = fakta});
            _store.HasARelation.Add(new HasA() {Price = 2.95, Product = _product, Store = _store});
            fakta.HasARelation.Add(new HasA() {Price = 1.95, Product = _product, Store = fakta});
            _uut.CreateShoppingList();

            Assert.That(_uut.GeneratedShoppingListData[0].StoreName, Is.EqualTo("Fakta"));
        }

        [Test]
        public void CreateShoppingList_CreateShoppingListForBananAndBuyInFakta_YouSave0Kr()
        {
            _uut.ShoppingListData.Add(new ProductInfo(_product.ProductName));
            List<StoreAndPrice> storeAndPriceList = new List<StoreAndPrice>();
            StoreAndPrice storeAndPrice = new StoreAndPrice();
            storeAndPrice.Price = 1.95;
            storeAndPrice.Name = "fakta";
            storeAndPriceList.Add(storeAndPrice);


            _unitWork.Products.FindCheapestStoreForAllProductsWithSum(Arg.Any<List<ProductInfo>>())
                .Returns(storeAndPriceList);

            var fakta = new Store() {StoreName = "Fakta"};
            _unitWork.Products.FindProduct(_product.ProductName).Returns(_product);
            _product.HasARelation.Add(new HasA() {Price = 2.95, Store = _store});
            _product.HasARelation.Add(new HasA() {Price = 1.95, Store = fakta});
            _store.HasARelation.Add(new HasA() {Price = 2.95, Product = _product, Store = _store});
            fakta.HasARelation.Add(new HasA() {Price = 1.95, Product = _product, Store = fakta});
            _uut.CreateShoppingList();

            Assert.That(_uut.MoneySaved, Is.EqualTo("0 kr"));
        }

        [Test]
        public void CreateShoppingList_CreateShoppingListForBananAndBuyInAldi_YouSave1Kr()
        {
            _uut.ShoppingListData.Add(new ProductInfo(_product.ProductName));
            List<StoreAndPrice> storeAndPriceList = new List<StoreAndPrice>();
            StoreAndPrice storeAndPrice = new StoreAndPrice();
            storeAndPrice.Price = 2.95;
            storeAndPrice.Name = _store.StoreName;
            storeAndPriceList.Add(storeAndPrice);


            _unitWork.Products.FindCheapestStoreForAllProductsWithSum(Arg.Any<List<ProductInfo>>())
                .Returns(storeAndPriceList);

            var fakta = new Store() {StoreName = "Fakta"};
            _unitWork.Products.FindProduct(_product.ProductName).Returns(_product);
            _product.HasARelation.Add(new HasA() {Price = 2.95, Store = _store});
            _product.HasARelation.Add(new HasA() {Price = 1.95, Store = fakta});
            _store.HasARelation.Add(new HasA() {Price = 2.95, Product = _product, Store = _store});
            fakta.HasARelation.Add(new HasA() {Price = 1.95, Product = _product, Store = fakta});
            _uut.CreateShoppingList();

            Assert.That(_uut.MoneySaved, Is.EqualTo("1 kr"));
        }

        [Test]
        public void CreateShoppingList_CreateShoppingListForAppleThatIsNotInStore_ListHasCorrectStoreName()
        {
            _uut.ShoppingListData.Add(new ProductInfo("Apple"));

            Product productApple = new Product();

            _unitWork.Products.FindProduct(_product.ProductName).Returns(productApple);
            _uut.CreateShoppingList();

            Assert.That(_uut.NotInAStore[0].Name, Is.EqualTo("Apple"));
        }


        [Test]
        public void WriteToJsonFile_AddAProductAndWriteItTOJson_ReadTheAddedProductInJson()
        {
            ObservableCollection<ProductInfo> productInfos = new ObservableCollection<ProductInfo>();
            productInfos.Add(new ProductInfo("test"));
            _uut.ShoppingListData = productInfos;
            _uut.WriteToJsonFile();
            string path = Path.Combine(Environment.ExpandEnvironmentVariables("%userprofile%"), "Documents") +
                          @"\Pristjek220";
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
            string path = Path.Combine(Environment.ExpandEnvironmentVariables("%userprofile%"), "Documents") +
                          @"\Pristjek220";
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
            string path = Path.Combine(Environment.ExpandEnvironmentVariables("%userprofile%"), "Documents") +
                          @"\Pristjek220" + @"\Shoppinglist.json";
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

        [Test]
        public void FindCheapestStoreWithSumForListOfProducts_OneProductInList_ReturnsTheCheapestStoreWithPrice()
        {
            var testList = new List<ProductInfo>();
            testList.Add(new ProductInfo(_product.ProductName));

            var returnList = new List<StoreAndPrice>();
            var cheapestStoreWithPrice = new StoreAndPrice() {Name = "Fakta", Price = 20};
            returnList.Add(cheapestStoreWithPrice);

            _unitWork.Products.FindCheapestStoreForAllProductsWithSum(testList).Returns(returnList);

            Assert.That(_uut.FindCheapestStoreWithSumForListOfProducts(testList).Price,
                Is.EqualTo(cheapestStoreWithPrice.Price));
        }

        [Test]
        public void FindCheapestStoreWithSumForListOfProducts_TwoProductInList_ReturnsTheCheapestStoreWithPrice()
        {
            var testList = new List<ProductInfo>();
            testList.Add(new ProductInfo(_product.ProductName));
            testList.Add(new ProductInfo("Tomat"));

            var returnList = new List<StoreAndPrice>();
            var store1WithPrice = new StoreAndPrice() {Name = "Fakta", Price = 20};
            var store2WithPrice = new StoreAndPrice() {Name = "Føtex", Price = 10};
            returnList.Add(store1WithPrice);
            returnList.Add(store2WithPrice);
            _unitWork.Products.FindCheapestStoreForAllProductsWithSum(testList).Returns(returnList);

            Assert.That(_uut.FindCheapestStoreWithSumForListOfProducts(testList).Price,
                Is.EqualTo(store2WithPrice.Price));
        }

        [Test]
        public void
            FindCheapestStoreWithSumForListOfProducts_TwoProductInListWith2InQuantity_ReturnsTheCheapestStoreWithPrice()
        {
            var testList = new List<ProductInfo>();
            testList.Add(new ProductInfo(_product.ProductName, "2"));
            testList.Add(new ProductInfo("Tomat", "2"));

            var returnList = new List<StoreAndPrice>();
            var store1WithPrice = new StoreAndPrice() {Name = "Fakta", Price = 20};
            var store2WithPrice = new StoreAndPrice() {Name = "Føtex", Price = 10};
            returnList.Add(store1WithPrice);
            returnList.Add(store2WithPrice);
            _unitWork.Products.FindCheapestStoreForAllProductsWithSum(testList).Returns(returnList);

            Assert.That(_uut.FindCheapestStoreWithSumForListOfProducts(testList).Price,
                Is.EqualTo(store2WithPrice.Price));
        }

        [Test]
        public void FindCheapestStoreWithSumForListOfProducts_TwoProductsFromOneStore_ReturnsFaktaAsTheCheapestStore()
        {
            var testList = new List<ProductInfo>();
            testList.Add(new ProductInfo(_product.ProductName, "2"));
            testList.Add(new ProductInfo("Tomat", "2"));
            testList.Add(new ProductInfo("Agurk", "2"));

            var returnList = new List<StoreAndPrice>();
            var store1WithPrice = new StoreAndPrice() {Name = "Fakta", Price = 5};
            var store1WithPrice2 = new StoreAndPrice() {Name = "Fakta", Price = 4};
            var store2WithPrice = new StoreAndPrice() {Name = "Føtex", Price = 10};
            returnList.Add(store1WithPrice);
            returnList.Add(store1WithPrice2);
            returnList.Add(store2WithPrice);
            _unitWork.Products.FindCheapestStoreForAllProductsWithSum(testList).Returns(returnList);

            Assert.That(_uut.FindCheapestStoreWithSumForListOfProducts(testList).Price, Is.EqualTo(9));
        }

        [Test]
        public void StoreNames_GetStoreNamesWhenCountEqual0_ReturnListWithStoreNameAldi()
        {
            List<Store> storeList = new List<Store>();
            storeList.Add(_store);
            List<string> stringList = new List<string>() {_store.StoreName};
            _unitWork.Stores.GetAllStores().Returns(storeList);
            Assert.That(_uut.StoreNames, Is.EqualTo(stringList));
        }

        [Test]
        public void StoreNames_GetStoreNamesWhenCountEqual1_ReturnListWithStoreNameAldi()
        {
            List<Store> storeList = new List<Store>();
            storeList.Add(_store);
            List<string> stringList = new List<string>() {_store.StoreName};
            _unitWork.Stores.GetAllStores().Returns(storeList);
            var test = _uut.StoreNames;
            Assert.That(_uut.StoreNames, Is.EqualTo(stringList));
        }

        [Test]
        public void ChangeItemToAnotherStores_ChangeFromLidlToAldiWhenItemExistInAldi_returns1()
        {
            StoreProductAndPrice storeProductAndPrice = new StoreProductAndPrice() {Price = 2, ProductName = "Test", Quantity = "2", StoreName = "Lidl"};
            _uut.GeneratedShoppingListData.Add(storeProductAndPrice);
            ProductAndPrice productAndPrice = new ProductAndPrice() {Price = 3, Name = "Aldi"};
            _unitWork.Stores.FindProductInStore("Aldi", storeProductAndPrice.ProductName).Returns(productAndPrice);

            

            List<StoreAndPrice> storeAndPriceList = new List<StoreAndPrice>();
            StoreAndPrice storeAndPrice = new StoreAndPrice{Price = 6, Name = _store.StoreName};
            storeAndPriceList.Add(storeAndPrice);

            _unitWork.Products.FindCheapestStoreForAllProductsWithSum(Arg.Any<List<ProductInfo>>())
                .Returns(storeAndPriceList);

            _uut.ShoppingListData.Add(new ProductInfo(_product.ProductName, "2"));


            Assert.That(_uut.ChangeProductToAnotherStore("Aldi", storeProductAndPrice), Is.EqualTo(1));
        }

        [Test]
        public void ChangeItemToAnotherStores_ChangeFromLidlToAldiWhenItemDontExistInAldi_returnsMinus1()
        {
            StoreProductAndPrice storeProductAndPrice = new StoreProductAndPrice() { Price = 2, ProductName = "Test", Sum = 4, Quantity = "2", StoreName = "Lidl" };
            _uut.GeneratedShoppingListData.Add(storeProductAndPrice);
            ProductAndPrice productAndPrice = null;
            _unitWork.Stores.FindProductInStore("Aldi", storeProductAndPrice.ProductName).Returns(productAndPrice);

            Assert.That(_uut.ChangeProductToAnotherStore("Aldi", storeProductAndPrice), Is.EqualTo(-1));
        }


        [Test]
        public void BuyInOneStore_SetValueToABC_GetValueABC()
        {
            _uut.BuyInOneStore = "ABC";
            Assert.That(_uut.BuyInOneStore, Is.EqualTo("ABC"));
        }

        [Test]
        public void StoreNames_SetValueToABC_GetValueABC()
        {
            List<string> storeList = new List<string> { "Test" };
            _uut.StoreNames = storeList;
            Assert.That(_uut.StoreNames, Is.EqualTo(storeList));
        }
    }
}