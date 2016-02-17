namespace Prodruct200.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialCreate : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Products",
                c => new
                    {
                        ProductName = c.String(nullable: false, maxLength: 128),
                        StoreProductId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.ProductName);
            
            CreateTable(
                "dbo.StoreProducts",
                c => new
                    {
                        StoreProductId = c.String(nullable: false, maxLength: 128),
                        Price = c.Double(nullable: false),
                        StoreName = c.String(maxLength: 128),
                        ProductName = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.StoreProductId)
                .ForeignKey("dbo.Products", t => t.ProductName)
                .ForeignKey("dbo.Stores", t => t.StoreName)
                .Index(t => t.StoreName)
                .Index(t => t.ProductName);
            
            CreateTable(
                "dbo.Stores",
                c => new
                    {
                        StoreName = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => t.StoreName);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.StoreProducts", "StoreName", "dbo.Stores");
            DropForeignKey("dbo.StoreProducts", "ProductName", "dbo.Products");
            DropIndex("dbo.StoreProducts", new[] { "ProductName" });
            DropIndex("dbo.StoreProducts", new[] { "StoreName" });
            DropTable("dbo.Stores");
            DropTable("dbo.StoreProducts");
            DropTable("dbo.Products");
        }
    }
}
