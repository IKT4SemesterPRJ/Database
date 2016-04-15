namespace Pristjek220Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Login_added : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Logins",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Username = c.String(),
                        Password = c.String(),
                        Store_StoreId = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Stores", t => t.Store_StoreId)
                .Index(t => t.Store_StoreId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Logins", "Store_StoreId", "dbo.Stores");
            DropIndex("dbo.Logins", new[] { "Store_StoreId" });
            DropTable("dbo.Logins");
        }
    }
}
