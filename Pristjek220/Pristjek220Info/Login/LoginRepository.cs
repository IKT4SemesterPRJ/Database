using System.Data.Entity;
using System.Linq;
using System.Security;

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
            var Securepassword = new SecureString();
            foreach (var item in login.Password)
            {
                Securepassword.AppendChar(item);
            }
            return password == Securepassword ? login.Store : null;
        }

        public DataContext DataContext
        {
            get { return Context as DataContext; }
        }
    }
}
