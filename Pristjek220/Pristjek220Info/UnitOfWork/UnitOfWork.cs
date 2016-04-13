
namespace Pristjek220Data
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly DataContext _context;
        public IStoreRepository Stores { get; }
        public IProductRepository Products { get; }
        public IHasARepository HasA { get; }
        public ILoginRepository Logins { get; }

        public UnitOfWork(DataContext context)
        {
            _context = context;
            Stores = new StoreRepository(_context);
            Products = new ProductRepository(_context);
            HasA = new HasARepository(_context);
            Logins = new LoginRepository(_context);
        }

        public int Complete()
        {
            return _context.SaveChanges();
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}
