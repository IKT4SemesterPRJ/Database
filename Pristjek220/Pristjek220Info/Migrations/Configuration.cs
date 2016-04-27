using System.Security.Cryptography;
using System.Text;

namespace Pristjek220Data.Migrations
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<Pristjek220Data.DataContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
            ContextKey = "Pristjek220Data.DataContext";
        }

        protected override void Seed(Pristjek220Data.DataContext context)
        {
            string code = "Admin";
            using (SHA256 hash = SHA256Managed.Create())
            {
                Encoding enc = Encoding.UTF8;

                //the user id is the salt. 
                //So 2 users with same password have different hashes. 
                //For example if someone knows his own hash he can't see who has same password
                Byte[] result = hash.ComputeHash(enc.GetBytes(code));

                StringBuilder Sb = new StringBuilder();
                foreach (Byte b in result)
                    Sb.Append(b.ToString("x2"));
                code = Sb.ToString();
            }
            context.Logins.AddOrUpdate(p => p.Username, new Login() {Username = "Admin", Password = code, Store = new Store() {StoreName = "Admin"} });
            //  This method will be called after migrating to the latest version.

            //  You can use the DbSet<T>.AddOrUpdate() helper extension method 
            //  to avoid creating duplicate seed data. E.g.
            //
            //    context.People.AddOrUpdate(
            //      p => p.FullName,
            //      new Person { FullName = "Andrew Peters" },
            //      new Person { FullName = "Brice Lambson" },
            //      new Person { FullName = "Rowan Miller" }
            //    );
            //
        }
    }
}
