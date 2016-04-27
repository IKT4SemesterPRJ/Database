namespace Pristjek220Data
{
    public interface IRepository<TEntity> where TEntity: class
    {
        void Add(TEntity entity);
        void Remove(TEntity entity);
    }
}
