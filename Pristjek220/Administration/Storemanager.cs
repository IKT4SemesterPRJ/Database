using Pristjek220Data;

namespace Administration
{
    /// <summary>
    /// The namespace <c>Storemanager</c> contains the class <see cref="Storemanager"/> 
    /// along with the interface <see cref="IStoremanager"/> and is placed in the Business Logic Layer.
    /// </summary>
    [System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    class NamespaceDoc
    { }

    /// <summary>
    /// This class is used to handle all of the store managers functionality 
    /// when interacting with the program.
    /// </summary>
    public class Storemanager : IStoremanager
    {
        private readonly IUnitOfWork _unitwork;
        public Store Store { get;}

        public Storemanager(IUnitOfWork unitOfWork, Store store)
        {
            _unitwork = unitOfWork;
            Store = _unitwork.Stores.FindStore(store.StoreName);
        }

        public int AddProductToDb(Product product)
        {
            if (FindProduct(product.ProductName) != null)
                return -1;

            _unitwork.Products.Add(product);
            _unitwork.Complete();
            return 0;
        }

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

        public Product FindProduct(string productName)
        {
            return _unitwork.Products.FindProduct(productName);
        }

        public void changePriceOfProductInStore(Product product, double price)
        {
            var hasA = _unitwork.HasA.Get(Store.StoreId, product.ProductId);
            hasA.Price = price;
            _unitwork.Complete();
        }

        public int RemoveProductFromMyStore(Product product)
        {
            var hasA = _unitwork.HasA.FindHasA(Store.StoreName, product.ProductName);

            if (hasA == null)
                return -1;

            hasA.Product.HasARelation.Remove(hasA);
            Store.HasARelation.Remove(hasA);
            _unitwork.HasA.Remove(hasA);

            _unitwork.Complete();

            return 0;
        }
    }
}
