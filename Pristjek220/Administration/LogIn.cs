using System.Security;
using Pristjek220Data;

namespace Administration
{
    /// <summary>
    ///     Business logic layer for LogIn
    /// </summary>
    public class LogIn : ILogIn
    {
        private readonly IUnitOfWork _user;

        /// <summary>
        ///     LogIn constructor takes a UnitOfWork to access the database
        /// </summary>
        /// <param name="unit"></param>
        public LogIn(IUnitOfWork unit)
        {
            _user = unit;
        }

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
        public int CheckUsernameAndPassword(string username, SecureString securePassword, ref Store store)
        {
            Login login;
            if ((login = _user.Logins.CheckUsername(username)) == null) return -1;
            return (store = _user.Logins.CheckLogin(securePassword, login)) != null ? 1 : 0;
        }
    }
}