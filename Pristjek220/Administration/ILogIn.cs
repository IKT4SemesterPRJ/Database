using System.Security;
using Pristjek220Data;

namespace Administration
{
    public interface ILogIn
    {
        int CheckUsernameAndPassword(string username, SecureString securePassword, ref Store store);
    }

}