using System.Security;
using Pristjek220Data;

namespace Administration
{
    /// <summary>
    ///     Interface for Business logic layer for LogIn
    /// </summary>
    public interface ILogIn
    {
        /// <summary>
        ///     Checks if Username and password matches the database
        /// </summary>
        /// <param name="username"></param>
        /// <param name="securePassword"></param>
        /// <param name="store"></param>
        /// <returns>
        ///     -1 if the username is not in the database, return 1 if the username and password match and return 0 if it does
        ///     not match
        /// </returns>
        int CheckUsernameAndPassword(string username, SecureString securePassword, ref Store store);
    }
}