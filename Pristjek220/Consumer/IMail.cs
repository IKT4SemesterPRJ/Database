using System.Collections.ObjectModel;
using Pristjek220Data;

namespace Consumer
{
    /// <summary>
    ///     Interface for Business logic layer for Mail
    /// </summary>
    public interface IMail
    {
        /// <summary>
        ///     Sends a mail
        /// </summary>
        /// <param name="email"></param>
        /// <param name="productListWithStore"></param>
        /// <param name="productListWithNoStore"></param>
        /// <param name="sum"></param>
        void SendMail(string email, ObservableCollection<StoreProductAndPrice> productListWithStore, ObservableCollection<ProductInfo> productListWithNoStore, string sum);
    }
}