using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Pristjek220Data;

namespace Storemanager
{
    public interface IStoremanager
    {
        Store Store { get; }

        int AddProductToDb(Product product);
        int AddProductToMyStore(Product product, double price);
        Product FindProduct(string productName);
    }
}
