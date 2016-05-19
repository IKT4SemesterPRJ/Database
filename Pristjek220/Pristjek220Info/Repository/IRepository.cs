namespace Pristjek220Data
{
    /// <summary>
    /// Interface for the generic class Repository
    /// </summary>
    /// <typeparam name="TEntity"></typeparam>
    public interface IRepository<TEntity> where TEntity: class
    {
        /// <summary>
        /// Generic Add function to add objects to the database
        /// </summary>
        /// <param name="entity"></param>
        void Add(TEntity entity);

        /// <summary>
        /// Generic Remove function to remove objects from the database
        /// </summary>
        /// <param name="entity"></param>
        void Remove(TEntity entity);
    }
}
