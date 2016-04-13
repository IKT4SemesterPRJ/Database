namespace Pristjek220Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Changed_Login_To_SecurePasswordd : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.Logins", "Password");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Logins", "Password", c => c.String());
        }
    }
}
