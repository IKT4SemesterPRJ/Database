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
        private readonly IUnitOfWork _user;
        
        public LogIn(IUnitOfWork unit)
        {
            _user = unit;
        }

        public int CheckUsernameAndPassword(string username, SecureString securePassword, ref Store store)
        {
            Login login;
            if ((login = _user.Logins.CheckUsername(username)) == null) return -1;
            return ((store = _user.Logins.CheckLogin(securePassword, login))) != null ? 1 : 0;
        }
    }
}
