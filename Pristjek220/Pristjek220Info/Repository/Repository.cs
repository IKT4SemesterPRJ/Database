using System;
using System.Data.Entity;

namespace Pristjek220Data
{
    /// <summary>
    /// The Repository class is generic class that contains the functions all the different repositories need,
    /// to avoid duplicated code
    /// </summary>
    /// <typeparam name="TEntity"></typeparam>
    public class Repository<TEntity> : IRepository<TEntity> where TEntity : class
    {
        /// <summary>
        /// Protected attribute to the database so only the inherited repositories can access the database
        /// </summary>
        protected readonly DbContext Context;

        /// <summary>
        /// Contructor which set set the attribute to the given datacontext
        /// and sets it's connection string to the database used
        /// </summary>
        /// <param name="context"></param>
        public Repository(DbContext context)
        {
            Context = context;
            Context.Database.Connection.ConnectionString = "Data Source=i4dab.ase.au.dk; Initial Catalog = F16I4PRJ4Gr7; User ID = F16I4PRJ4Gr7; Password = F16I4PRJ4Gr7; MultipleActiveResultSets=True;";
        }

        /// <summary>
        /// Generic Add function to add object to the database
        /// </summary>
        /// <param name="entity"></param>
        public void Add(TEntity entity)
        {
            Context.Set<TEntity>().Add(entity);
        }

        /// <summary>
        /// Generic Remove function to remove objects from the database
        /// </summary>
        /// <param name="entity"></param>
        public void Remove(TEntity entity)
        {
            try
            {
                Context.Set<TEntity>().Remove(entity);
            }
            catch(InvalidOperationException)
            {
                //Entiteten findes ikke..
            }
        }
    }
}
