using Pristjek220Data;

namespace Administration
{
    public interface IAdmin
    {
        void CreateLogin(string userName, string password, string storeName);
        void AddStore(Store store);
    }
}
