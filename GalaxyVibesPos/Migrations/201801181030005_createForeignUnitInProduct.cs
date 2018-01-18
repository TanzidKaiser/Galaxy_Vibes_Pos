namespace GalaxyVibesPos.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class createForeignUnitInProduct : DbMigration
    {
        public override void Up()
        {
            CreateIndex("dbo.ProductDetails", "UnitID");
            AddForeignKey("dbo.ProductDetails", "UnitID", "dbo.ProductUnit", "UnitID");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.ProductDetails", "UnitID", "dbo.ProductUnit");
            DropIndex("dbo.ProductDetails", new[] { "UnitID" });
        }
    }
}
