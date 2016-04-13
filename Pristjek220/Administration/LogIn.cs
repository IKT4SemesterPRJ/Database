using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security;
using System.Text;
using System.Threading.Tasks;
using Pristjek220Data;

namespace Administration
{
    public class LogIn : ILogIn
    {
        private IUnitOfWork _user;

        public LogIn()
        {
            _user = new UnitOfWork(new DataContext());
        }

        // send ref med i stedet
        public Store CheckUsernameAndPassword(string username, SecureString securePassword)
        {
            var password = ConvertToUnsecureString(securePassword);
            Login login;
            Store store;
            if ((login =_user.Logins.CheckUsername(username)) != null)
            {
                if ((store = _user.Logins.CheckLogin(securePassword, login)) != null)
                {
                    return store;
                }
                //throw password fail
            }
            
            return null;
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
