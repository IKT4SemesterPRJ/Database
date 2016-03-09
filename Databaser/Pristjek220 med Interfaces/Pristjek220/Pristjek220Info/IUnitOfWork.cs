using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pristjek220Data
{
    public interface IUnitOfWork : IDisposable
    {
        IStoreRepository Stores { get; }
        IHasARepository HasA { get; }
        IProductRepository Products { get; }
        int Complete();
    }
}
