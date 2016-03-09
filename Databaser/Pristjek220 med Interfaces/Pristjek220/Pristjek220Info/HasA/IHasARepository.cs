using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pristjek220Data
{
    public interface IHasARepository : IRepository<HasA>
    {
        HasA FindCheapestHasA(Product product);
    }
}
