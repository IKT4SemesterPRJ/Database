using System.Linq;
using Pristjek220Data;

namespace Storemanager
{
    public class Storemanager : IStoremanager
    {
        private IUnitOfWork _unitwork;
        public Store Store { get; private set; }

        public Storemanager(IUnitOfWork unitOfWork, Store store)
        {
            _unitwork = unitOfWork;
            Store = store;
        }

        public int AddProductToMyStore(string productName, double price)
        {
            var product = _unitwork.Products.Find(c => c.ProductName == productName).FirstOrDefault();
            if (product == null)
            {
                product = new Product() {ProductName = productName};
                _unitwork.Products.Add(product);
            }

            if (_unitwork.HasA.Get(product.ProductId, Store.StoreId) != null)
                return -1;

            var hasA = new HasA()
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
    }
}
