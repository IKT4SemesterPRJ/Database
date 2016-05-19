using System.ComponentModel.DataAnnotations;

namespace Pristjek220Data
{
    /// <summary>
    /// Configures the columns in Login table in the database
    /// </summary>
    public class Login
    {
        /// <summary>
        /// Primary key
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Username of the login is required to have a login
        /// </summary>
        [Required]
        public string Username { get; set; }

        /// <summary>
        /// Password of the login is required to have a login
        /// </summary>
        [Required]
        public string Password { get; set; }

        /// <summary>
        /// Reference to the store wich the login is connected to is required to have a login
        /// </summary>
        [Required]
        public Store Store { get; set; }
    }
}
