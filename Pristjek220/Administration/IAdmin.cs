using System.Security;
using Pristjek220Data;

namespace Administration
{
    public interface IAdmin
    {
        int CreateLogin(string userName, SecureString password, string storeName);
        void AddStore(Store store);
    }
}
