using System;

namespace Pristjek220Data
{
    /// <summary>
    /// Interface for DAL UnitOfWork
    /// </summary>
    public interface IUnitOfWork : IDisposable
    {
        /// <summary>
        /// Interface to StoreRepository
        /// </summary>
        IStoreRepository Stores { get; }
        
        /// <summary>
        /// Interface to HasARepository
        /// </summary>
        IHasARepository HasA { get; }
        
        /// <summary>
        /// Interface to ProductRepository
        /// </summary>
        IProductRepository Products { get; }
        
        /// <summary>
        /// Interface to LoginRepository
        /// </summary>
        ILoginRepository Logins { get; }

        /// <summary>
        /// Saves the changes to the database
        /// </summary>
        /// <returns>Returns the number of changes made</returns>
        int Complete();
    }
}
