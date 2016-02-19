namespace Projekt___Product220.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class RemovalOfIDInStoreProduct : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Products",
                c => new
                    {
                        ProductId = c.Int(nullable: false, identity: true),
                        ProductName = c.String(),
                    })
                .PrimaryKey(t => t.ProductId);
            
            CreateTable(
                "dbo.StoreProducts",
                c => new
                    {
                        StoreProductId = c.Int(nullable: false, identity: true),
                        Price = c.Double(nullable: false),
                        StoreId = c.Int(nullable: false),
                        ProductId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.StoreProductId)
                .ForeignKey("dbo.Products", t => t.ProductId, cascadeDelete: true)
                .ForeignKey("dbo.Stores", t => t.StoreId, cascadeDelete: true)
                .Index(t => t.StoreId)
                .Index(t => t.ProductId);
            
            CreateTable(
                "dbo.Stores",
                c => new
                    {
                        StoreId = c.Int(nullable: false, identity: true),
                        StoreName = c.String(),
                    })
                .PrimaryKey(t => t.StoreId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.StoreProducts", "StoreId", "dbo.Stores");
            DropForeignKey("dbo.StoreProducts", "ProductId", "dbo.Products");
            DropIndex("dbo.StoreProducts", new[] { "ProductId" });
            DropIndex("dbo.StoreProducts", new[] { "StoreId" });
            DropTable("dbo.Stores");
            DropTable("dbo.StoreProducts");
            DropTable("dbo.Products");
        }
    }
}
