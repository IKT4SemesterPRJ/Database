using System.Collections.ObjectModel;
using Pristjek220Data;

namespace Consumer
{
    public interface IMail
    {
        void SendMail(string email, ObservableCollection<StoreProductAndPrice> productListWithStore, ObservableCollection<ProductInfo> productListWithNoStore, string sum);
    }
}