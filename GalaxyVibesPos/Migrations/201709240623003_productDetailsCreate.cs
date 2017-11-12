namespace GalaxyVibesPos.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class productDetailsCreate : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.ProductDetails",
                c => new
                    {
                        ProductDetailsID = c.Int(nullable: false, identity: true),
                        MainCategoryID = c.Int(nullable: false),
                        CategoryID = c.Int(nullable: false),
                        SubCategoryID = c.Int(nullable: false),
                        ProductName = c.String(nullable: false),
                        Code = c.String(),
                        PurchasePrice = c.Double(),
                        SalePrice = c.Double(),
                        Stoke = c.Double(),
                        Description = c.String(),
                        UnitID = c.Int(),
                        MinimumProductQuantity = c.Double(),
                        WarehouseID = c.Int(),
                        RackID = c.Int(),
                        CellID = c.Int(),
                        Status = c.Int(),
                    })
                .PrimaryKey(t => t.ProductDetailsID)
                .ForeignKey("dbo.Category", t => t.CategoryID, cascadeDelete: false)
                .ForeignKey("dbo.CategoryMain", t => t.MainCategoryID, cascadeDelete: false)
                .ForeignKey("dbo.CategorySub", t => t.SubCategoryID, cascadeDelete: false)
                .Index(t => t.MainCategoryID)
                .Index(t => t.CategoryID)
                .Index(t => t.SubCategoryID);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.ProductDetails", "SubCategoryID", "dbo.CategorySub");
            DropForeignKey("dbo.ProductDetails", "MainCategoryID", "dbo.CategoryMain");
            DropForeignKey("dbo.ProductDetails", "CategoryID", "dbo.Category");
            DropIndex("dbo.ProductDetails", new[] { "SubCategoryID" });
            DropIndex("dbo.ProductDetails", new[] { "CategoryID" });
            DropIndex("dbo.ProductDetails", new[] { "MainCategoryID" });
            DropTable("dbo.ProductDetails");
        }
    }
}
