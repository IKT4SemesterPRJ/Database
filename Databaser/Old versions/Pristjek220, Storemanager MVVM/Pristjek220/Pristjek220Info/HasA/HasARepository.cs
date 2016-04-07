using System.Linq;

namespace Pristjek220Data
{
    public class HasARepository : Repository<HasA>, IHasARepository
    {
        public HasARepository(DataContext context) : base(context)
        {
        }

        public HasA Get(int id, int id2)
        {
            return Context.Set<HasA>().Find(id, id2);
        }

        public HasA FindHasA(string storeName, string productName)
        {
            return (from t in DataContext.HasARelation where storeName == t.Store.StoreName && productName == t.Product.ProductName select t).FirstOrDefault();
        }

        public HasA FindCheapestHasA(Product product)
        {
            return DataContext.HasARelation.OrderBy(c => c.Price).FirstOrDefault();
        }

        public DataContext DataContext
        {
            get { return Context as DataContext; }
        }
    }
}
