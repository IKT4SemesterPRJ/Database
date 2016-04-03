using System.Collections.Generic;

namespace Pristjek220Data
{
    public class Store
    {
        public int StoreId { get; set; }
        public string StoreName { get; set; }

        public virtual List<HasA> HasARelation { get; } = new List<HasA>();
    }
}
