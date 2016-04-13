using Pristjek220Data;

namespace Administration
{
    public interface IStoremanager
    {
        Store Store { get; }

        int AddProductToDb(Product product);
        int AddProductToMyStore(Product product, double price);
        Product FindProduct(string productName);
    }
}
