using System;
using System.Data.Entity;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security;
using System.Security.Cryptography;
using System.Text;

namespace Pristjek220Data
{
    public class LoginRepository : Repository<Login>, ILoginRepository
    {
        public LoginRepository(DbContext context) : base(context)
        {   
        }

        public Login CheckUsername(string username)
        {
            var login = (from user in DataContext.Logins where username == user.Username select user).Include(b => b.Store).FirstOrDefault();
            return login;
        }

        public Store CheckLogin(SecureString password, Login login)
        {
            var PasswordString = ConvertToUnsecureString(password);

            using (SHA256 hash = SHA256Managed.Create())
            {
                Encoding enc = Encoding.UTF8;

                //the user id is the salt. 
                //So 2 users with same password have different hashes. 
                //For example if someone knows his own hash he can't see who has same password
                string input = PasswordString;
                Byte[] result = hash.ComputeHash(enc.GetBytes(input));

                StringBuilder Sb = new StringBuilder();
                foreach (Byte b in result)
                    Sb.Append(b.ToString("x2"));
                PasswordString = Sb.ToString();
            }
            return PasswordString == login.Password ? login.Store : null;
        }

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
