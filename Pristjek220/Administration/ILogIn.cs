using System.Security;
using Pristjek220Data;

namespace Administration
{
    public interface ILogIn
    {
        Store CheckUsernameAndPassword(string username, SecureString securePassword);
    }
}