using System.Data.Entity;
using System.Linq;

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

        public Store CheckLogin(string password, Login login)
        {
            return password == login.Password ? login.Store : null;
        }

        public DataContext DataContext
        {
            get { return Context as DataContext; }
        }
    }
}
