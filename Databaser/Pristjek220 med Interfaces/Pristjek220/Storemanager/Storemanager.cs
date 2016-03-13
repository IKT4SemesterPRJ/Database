using System.Linq;
using Pristjek220Data;

namespace Storemanager
{
    public class Storemanager : IStoremanager
    {
        private readonly IUnitOfWork _unitwork;
        public Store Store { get; private set; }

        public Storemanager(IUnitOfWork unitOfWork, Store store)
        {
            _unitwork = unitOfWork;
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

            _unitwork.HasA.Add(hasA);

            _unitwork.Complete();
            return 0;
        }

        public Product FindProduct(string productName)
        {
            return _unitwork.Products.Find(c => c.ProductName == productName).FirstOrDefault();
        }
    }
}
