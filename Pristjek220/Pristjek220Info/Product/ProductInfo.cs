using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pristjek220Data
{
    public class ProductInfo
    {
        public ProductInfo(string name, string quantity = "1")
        {
            Name = char.ToUpper(name[0]) + name.Substring(1).ToLower();
            Quantity = quantity;
        }

        public string Name { set; get; }
        public string Quantity { set; get; }
    }
}
