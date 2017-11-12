namespace GalaxyVibesPos.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class CategoryCreate : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Categories",
                c => new
                    {
                        CategoryID = c.Int(nullable: false, identity: true),
                        MainCategoryID = c.Int(nullable: false),
                        CategoryName = c.String(nullable: false),
                    })
                .PrimaryKey(t => t.CategoryID)
                .ForeignKey("dbo.CategoryMains", t => t.MainCategoryID, cascadeDelete: true)
                .Index(t => t.MainCategoryID);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Categories", "MainCategoryID", "dbo.CategoryMains");
            DropIndex("dbo.Categories", new[] { "MainCategoryID" });
            DropTable("dbo.Categories");
        }
    }
}
