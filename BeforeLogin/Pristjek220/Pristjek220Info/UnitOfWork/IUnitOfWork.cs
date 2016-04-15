using System;

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
