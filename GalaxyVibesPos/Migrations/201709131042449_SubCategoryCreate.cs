namespace GalaxyVibesPos.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class SubCategoryCreate : DbMigration
    {
        public override void Up()
        {
            RenameTable(name: "dbo.Categories", newName: "Category");
            RenameTable(name: "dbo.CategoryMains", newName: "CategoryMain");
            CreateTable(
                "dbo.CategorySub",
                c => new
                    {
                        SubCategoryID = c.Int(nullable: false, identity: true),
                        MainCategoryID = c.Int(nullable: false),
                        CategoryID = c.Int(nullable: false),
                        SubCategoryName = c.String(nullable: false),
                    })
                .PrimaryKey(t => t.SubCategoryID)
                .ForeignKey("dbo.Category", t => t.CategoryID, cascadeDelete: false)
                .ForeignKey("dbo.CategoryMain", t => t.MainCategoryID, cascadeDelete: false)
                .Index(t => t.MainCategoryID)
                .Index(t => t.CategoryID);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.CategorySub", "MainCategoryID", "dbo.CategoryMain");
            DropForeignKey("dbo.CategorySub", "CategoryID", "dbo.Category");
            DropIndex("dbo.CategorySub", new[] { "CategoryID" });
            DropIndex("dbo.CategorySub", new[] { "MainCategoryID" });
            DropTable("dbo.CategorySub");
            RenameTable(name: "dbo.CategoryMain", newName: "CategoryMains");
            RenameTable(name: "dbo.Category", newName: "Categories");
        }
    }
}
