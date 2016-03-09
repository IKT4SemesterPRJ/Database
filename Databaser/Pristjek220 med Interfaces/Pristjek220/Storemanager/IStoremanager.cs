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

        int AddProductToMyStore(string productName, double price);
    }
}
