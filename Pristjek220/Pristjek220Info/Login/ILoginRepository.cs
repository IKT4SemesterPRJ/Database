using System.Security;

namespace Pristjek220Data
{
    /// <summary>
    /// Interface to DAL LoginRepository, inherit from IRepository for the functions Add and Remove.
    /// </summary>
    public interface ILoginRepository : IRepository<Login>
    {
        /// <summary>
        /// Chechs that the username is equal to a username in the database
        /// </summary>
        /// <param name="username"></param>
        /// <returns>Returns the login where the usernames matches else null</returns>
        Login CheckUsername(string username);

        /// <summary>
        /// Check if the password matchs
        /// </summary>
        /// <param name="password"></param>
        /// <param name="login"></param>
        /// <returns>Returns the store from the login if the passwords matches else null</returns>
        Store CheckLogin(SecureString password, Login login);
    }
}
