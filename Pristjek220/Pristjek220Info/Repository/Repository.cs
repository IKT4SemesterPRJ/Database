using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

namespace Pristjek220Data
{
    public class Repository<TEntity> : IRepository<TEntity> where TEntity : class
    {
        protected readonly DbContext Context;

        public Repository(DbContext context)
        {
            Context = context;
            Context.Database.Connection.ConnectionString = "Data Source=i4dab.ase.au.dk; Initial Catalog = F16I4PRJ4Gr7; User ID = F16I4PRJ4Gr7; Password = F16I4PRJ4Gr7; MultipleActiveResultSets=True;";
        }

        public void Add(TEntity entity)
        {
            Context.Set<TEntity>().Add(entity);
        }

        public void Remove(TEntity entity)
        {
            try
            {
                Context.Set<TEntity>().Remove(entity);
            }
            catch(InvalidOperationException)
            {
                //Entiteten findes ikke.. Do something
            }
        }
    }
}
