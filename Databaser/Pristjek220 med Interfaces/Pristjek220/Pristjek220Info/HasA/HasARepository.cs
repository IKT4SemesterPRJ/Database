using System.Linq;

namespace Pristjek220Data
{
    public class HasARepository : Repository<HasA>, IHasARepository
    {
        public HasARepository(DataContext context) : base(context)
        {
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
