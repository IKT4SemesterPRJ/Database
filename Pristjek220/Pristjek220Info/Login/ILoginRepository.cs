using System.Security;

namespace Pristjek220Data
{
    public interface ILoginRepository : IRepository<Login>
    {
        Login CheckUsername(string username);
        Store CheckLogin(SecureString password, Login login);
    }
}
