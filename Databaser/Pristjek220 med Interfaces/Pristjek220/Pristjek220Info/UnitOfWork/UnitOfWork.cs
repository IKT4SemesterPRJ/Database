using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pristjek220Data
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly DataContext _context;
        public IStoreRepository Stores { get; private set; }
        public IProductRepository Products { get; private set; }
        public IHasARepository HasA { get; private set; }

        public UnitOfWork(DataContext context)
        {
            _context = context;
            Stores = new StoreRepository(_context);
            Products = new ProductRepository(_context);
            HasA = new HasARepository(_context);
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
