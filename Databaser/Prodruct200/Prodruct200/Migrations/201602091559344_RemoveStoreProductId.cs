namespace Prodruct200.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class RemoveStoreProductId : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.Products", "StoreProductId");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Products", "StoreProductId", c => c.Int(nullable: false));
        }
    }
}
