using Pristjek220Data;

namespace Storemanager
{
    /// <summary>
    /// The namespace <c>Storemanager</c> contains the class <see cref="Storemanager"/> 
    /// and is placed in the Business Logic Layer.
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
        public Store Store { get; }

        public Storemanager(IUnitOfWork unitOfWork, Store store)
        {
            _unitwork = unitOfWork;
            var tmp = _unitwork.Stores.FindStore(store.StoreName);
            if (tmp != null)
            {
                Store = tmp;
                return;
            }
            Store = store;
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

            product.HasARelation.Add(hasA);
            Store.HasARelation.Add(hasA);

            _unitwork.HasA.Add(hasA);

            _unitwork.Complete();
            return 0;
        }

        public Product FindProduct(string productName)
        {
            return _unitwork.Products.FindProduct(productName);
        }
    }
}
