namespace Pristjek220Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialCreate : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.HasAs",
                c => new
                    {
                        StoreId = c.Int(nullable: false),
                        ProductId = c.Int(nullable: false),
                        Price = c.Double(nullable: false),
                    })
                .PrimaryKey(t => new { t.StoreId, t.ProductId })
                .ForeignKey("dbo.Products", t => t.ProductId, cascadeDelete: true)
                .ForeignKey("dbo.Stores", t => t.StoreId, cascadeDelete: true)
                .Index(t => t.StoreId)
                .Index(t => t.ProductId);
            
            CreateTable(
                "dbo.Products",
                c => new
                    {
                        ProductId = c.Int(nullable: false, identity: true),
                        ProductName = c.String(),
                    })
                .PrimaryKey(t => t.ProductId);
            
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
            DropForeignKey("dbo.HasAs", "StoreId", "dbo.Stores");
            DropForeignKey("dbo.HasAs", "ProductId", "dbo.Products");
            DropIndex("dbo.HasAs", new[] { "ProductId" });
            DropIndex("dbo.HasAs", new[] { "StoreId" });
            DropTable("dbo.Stores");
            DropTable("dbo.Products");
            DropTable("dbo.HasAs");
        }
    }
}
