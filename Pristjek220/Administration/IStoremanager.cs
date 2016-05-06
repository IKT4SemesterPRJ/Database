using Pristjek220Data;

namespace Administration
{
    public interface IStoremanager
    {
        Store Store { get; }

        int AddProductToDb(Product product);
        int AddProductToMyStore(Product product, double price);
        Product FindProduct(string productName);
        ProductAndPrice FindProductInStore(string productName);
        int RemoveProductFromMyStore(Product product);
        void changePriceOfProductInStore(Product product, double price);
    }
}
