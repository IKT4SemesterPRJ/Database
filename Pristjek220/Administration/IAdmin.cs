using System.Security;
using Pristjek220Data;

namespace Administration
{
    public interface IAdmin
    {
        int CreateLogin(string userName, SecureString password, string storeName);
        int CheckPasswords(SecureString pass1, SecureString pass2);
        int DeleteStore(string storeName);
    }
}
