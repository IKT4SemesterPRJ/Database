
namespace Pristjek220Data
{
    /// <summary>
    /// This class is use to connect the BLL to the Repositories.
    /// </summary>
    public class UnitOfWork : IUnitOfWork
    {
        private DataContext _context;
        /// <summary>
        /// Makes the UnitOfWork able to call functions to IStoreRepository
        /// </summary>
        public IStoreRepository Stores { get; }

        /// <summary>
        /// Makes the UnitOfWork able to call functions to IProductRepository
        /// </summary>
        public IProductRepository Products { get; }

        /// <summary>
        /// Makes the UnitOfWork able to call functions to IHasARepository
        /// </summary>
        public IHasARepository HasA { get; }

        /// <summary>
        /// Makes the UnitOfWork able to call functions to ILoginRepository
        /// </summary>
        public ILoginRepository Logins { get; }

        /// <summary>
        /// Contructor for UnitOfWork that sets the reference to the repositories
        /// </summary>
        /// <param name="context"></param>
        public UnitOfWork(DataContext context)
        {
            _context = context;
            Stores = new StoreRepository(_context);
            Products = new ProductRepository(_context);
            HasA = new HasARepository(_context);
            Logins = new LoginRepository(_context);
        }

        /// <summary>
        /// Saves the changes made to the database
        /// </summary>
        /// <returns>Returns the number of changes made to the database</returns>
        public int Complete()
        {
            return _context.SaveChanges();
        }

        /// <summary>
        /// Dispose the database
        /// </summary>
        public void Dispose()
        {
            _context.Dispose();
        }
    }
}
