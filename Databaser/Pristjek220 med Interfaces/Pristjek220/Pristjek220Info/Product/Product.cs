using System.Collections.Generic;

namespace Pristjek220Data
{
    public class Product
    {
        public int ProductId { get; set; }
        public string ProductName { get; set; }

        public virtual List<HasA> HasARelation { get; } = new List<HasA>();
    }
}
