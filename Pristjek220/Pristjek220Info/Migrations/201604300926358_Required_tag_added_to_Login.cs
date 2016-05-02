namespace Pristjek220Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Required_tag_added_to_Login : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Logins", "Store_StoreId", "dbo.Stores");
            DropIndex("dbo.Logins", new[] { "Store_StoreId" });
            AlterColumn("dbo.Logins", "Username", c => c.String(nullable: false));
            AlterColumn("dbo.Logins", "Password", c => c.String(nullable: false));
            AlterColumn("dbo.Logins", "Store_StoreId", c => c.Int(nullable: false));
            CreateIndex("dbo.Logins", "Store_StoreId");
            AddForeignKey("dbo.Logins", "Store_StoreId", "dbo.Stores", "StoreId", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Logins", "Store_StoreId", "dbo.Stores");
            DropIndex("dbo.Logins", new[] { "Store_StoreId" });
            AlterColumn("dbo.Logins", "Store_StoreId", c => c.Int());
            AlterColumn("dbo.Logins", "Password", c => c.String());
            AlterColumn("dbo.Logins", "Username", c => c.String());
            CreateIndex("dbo.Logins", "Store_StoreId");
            AddForeignKey("dbo.Logins", "Store_StoreId", "dbo.Stores", "StoreId");
        }
    }
}
