namespace GalaxyVibesPos.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class MainCategoryUpdate : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.CategoryMains", "MaincategoryName", c => c.String(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.CategoryMains", "MaincategoryName", c => c.String());
        }
    }
}
