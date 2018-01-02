namespace GalaxyVibesPos.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ProductUnitAdd : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.ProductUnit",
                c => new
                    {
                        UnitID = c.Int(nullable: false, identity: true),
                        UnitSize = c.String(),
                    })
                .PrimaryKey(t => t.UnitID);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.ProductUnit");
        }
    }
}
