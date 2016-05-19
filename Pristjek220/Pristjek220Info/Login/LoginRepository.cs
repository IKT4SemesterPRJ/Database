using System;
using System.Data.Entity;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security;
using System.Security.Cryptography;
using System.Text;

namespace Pristjek220Data
{
    /// <summary>
    /// The LoginRepository class is a part of the repository pattern,
    /// and it handles the interactions with the database login table
    /// </summary>
    public class LoginRepository : Repository<Login>, ILoginRepository
    {
        /// <summary>
        /// Constructor which sends the Datacontext to the base class
        /// </summary>
        /// <param name="context"></param>
        public LoginRepository(DbContext context) : base(context)
        {   
        }


        /// <summary>
        /// Chechs that the username is equal to a username in the database
        /// </summary>
        /// <param name="username"></param>
        /// <returns>Returns the login where the usernames matches else null</returns>
        public Login CheckUsername(string username)
        {
            var login = (from user in DataContext.Logins where username == user.Username select user).Include(b => b.Store).FirstOrDefault();
            return login;
        }

        /// <summary>
        /// Check if the password matchs
        /// </summary>
        /// <param name="password"></param>
        /// <param name="login"></param>
        /// <returns>Returns the store from the login if the passwords matches else null</returns>
        public Store CheckLogin(SecureString password, Login login)
        {
            var PasswordString = ConvertToUnsecureString(password);

            using (SHA256 hash = SHA256Managed.Create())
            {
                Encoding enc = Encoding.UTF8;
                
                string input = PasswordString;
                Byte[] result = hash.ComputeHash(enc.GetBytes(input));

                StringBuilder Sb = new StringBuilder();
                foreach (Byte b in result)
                    Sb.Append(b.ToString("x2"));
                PasswordString = Sb.ToString();
            }
            return PasswordString == login.Password ? login.Store : null;
        }

        /// <summary>
        /// When using DataContext it uses the Context from base class
        /// </summary>
        public DataContext DataContext
        {
            get { return Context as DataContext; }
        }

        private string ConvertToUnsecureString(SecureString securePassword)
        {
            if (securePassword == null)
            {
                return string.Empty;
            }

            IntPtr unmanagedString = IntPtr.Zero;
            try
            {
                unmanagedString = Marshal.SecureStringToGlobalAllocUnicode(securePassword);
                return Marshal.PtrToStringUni(unmanagedString);
            }
            finally
            {
                Marshal.ZeroFreeGlobalAllocUnicode(unmanagedString);
            }
        }
    }
}
