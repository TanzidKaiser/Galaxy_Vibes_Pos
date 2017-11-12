namespace GalaxyVibesPos.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class MainCategoryCreate : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.CategoryMains",
                c => new
                    {
                        MainCategoryID = c.Int(nullable: false, identity: true),
                        MaincategoryName = c.String(),
                    })
                .PrimaryKey(t => t.MainCategoryID);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.CategoryMains");
        }
    }
}
