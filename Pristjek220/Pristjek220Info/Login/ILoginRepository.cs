namespace Pristjek220Data
{
    public interface ILoginRepository : IRepository<Login>
    {
        Login CheckUsername(string username);
        Store CheckLogin(string password, Login login);
    }
}
