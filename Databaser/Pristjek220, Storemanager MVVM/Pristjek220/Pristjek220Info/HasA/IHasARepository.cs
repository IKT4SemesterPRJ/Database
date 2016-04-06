
namespace Pristjek220Data
{
    public interface IHasARepository : IRepository<HasA>
    {
        HasA Get(int id1, int id2);
        HasA FindHasA(string storeName, string productName);
        HasA FindCheapestHasA(Product product);
    }
}
