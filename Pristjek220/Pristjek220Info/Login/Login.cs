using System.Security;

namespace Pristjek220Data
{
    public class Login
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public SecureString Password { get; set; }

        public Store Store { get; set; }
    }
}
