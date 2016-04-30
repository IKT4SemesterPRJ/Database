using System.ComponentModel.DataAnnotations;
using System.Security;

namespace Pristjek220Data
{
    public class Login
    {
        public int Id { get; set; }
        [Required]
        public string Username { get; set; }
        [Required]
        public string Password { get; set; }

        [Required]
        public Store Store { get; set; }
    }
}
