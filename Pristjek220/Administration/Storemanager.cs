using System.Runtime.CompilerServices;
using Pristjek220Data;

namespace Administration
{
    /// <summary>
    ///     The namespace <c>Storemanager</c> contains the class <see cref="Storemanager" />
    ///     along with the interface <see cref="IStoremanager" /> and is placed in the Business Logic Layer.
    /// </summary>
    [CompilerGenerated]
    internal class NamespaceDoc
    {
    }

    /// <summary>
    ///     This class is used to handle all of the store managers functionality
    ///     when interacting with the program.
    /// </summary>
    public class Storemanager : IStoremanager
    {
        private readonly IUnitOfWork _unitwork;

        /// <summary>
        ///     Storemanager constructor takes a UnitOfWork to access the database and a store to set as his Store
        /// </summary>
        /// <param name="unitOfWork"></param>
        /// <param name="store"></param>
        public Storemanager(IUnitOfWork unitOfWork, Store store)
        {
            _unitwork = unitOfWork;
            Store = _unitwork.Stores.FindStore(store.StoreName);
        }

        /// <summary>
        ///     Store get function, that is used when the Storemanager needs his Store
        /// </summary>
        public Store Store { get; set; }

        /// <summary>
        ///     Add product to database
        /// </summary>
        /// <param name="product"></param>
        /// <returns>-1 if product is not found and return 0 if product is added</returns>
        public int AddProductToDb(Product product)
        {
            if (FindProduct(product.ProductName) != null)
                return -1;

            _unitwork.Products.Add(product);
            _unitwork.Complete();
            return 0;
        }

        /// <summary>
        ///     Add product to storemanagers store with price
        /// </summary>
        /// <param name="product"></param>
        /// <param name="price"></param>
        /// <returns>-1 if the product exist in that store and 0 if it has been added</returns>
        public int AddProductToMyStore(Product product, double price)
        {
            if (_unitwork.HasA.Get(Store.StoreId, product.ProductId) != null)
                return -1;

            var hasA = new HasA
            {
                Price = price,
                Product = product,
                Store = Store,
                ProductId = product.ProductId,
                StoreId = Store.StoreId
            };

            _unitwork.HasA.Add(hasA);
            product.HasARelation.Add(hasA);
            Store.HasARelation.Add(hasA);

            _unitwork.Complete();
            return 0;
        }

        /// <summary>
        ///     Finds a product with the requested productName
        /// </summary>
        /// <param name="productName"></param>
        /// <returns>Returns the product that got the productName</returns>
        public Product FindProduct(string productName)
        {
            return _unitwork.Products.FindProduct(productName);
        }

        /// <summary>
        ///     Finds a product and its price with the requested productName in the storemanagers store
        /// </summary>
        /// <param name="productName"></param>
        /// <returns>Returns the product and its price</returns>
        public ProductAndPrice FindProductInStore(string productName)
        {
            return _unitwork.Stores.FindProductInStore(Store.StoreName, productName);
        }

        /// <summary>
        ///     Changes the price of the requested product
        /// </summary>
        /// <param name="product"></param>
        /// <param name="price"></param>
        public void ChangePriceOfProductInStore(Product product, double price)
        {
            var hasA = _unitwork.HasA.Get(Store.StoreId, product.ProductId);
            hasA.Price = price;
            _unitwork.Complete();
        }

        /// <summary>
        ///     Remove the requested product from the storemanagers store, and if the product got no other relations remove the
        ///     product too
        /// </summary>
        /// <param name="product"></param>
        /// <returns>-1 if the product does not exist and 0 if it has been removed successfully</returns>
        public int RemoveProductFromMyStore(Product product)
        {
            var hasA = _unitwork.HasA.FindHasA(Store.StoreName, product.ProductName);

            if (hasA == null)
                return -1;

            hasA.Product.HasARelation.Remove(hasA);
            Store.HasARelation.Remove(hasA);
            _unitwork.HasA.Remove(hasA);

            if (product.HasARelation.Count == 0)
                _unitwork.Products.Remove(product);

            _unitwork.Complete();

            return 0;
        }
    }
}