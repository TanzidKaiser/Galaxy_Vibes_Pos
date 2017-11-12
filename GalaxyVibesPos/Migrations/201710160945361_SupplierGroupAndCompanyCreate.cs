namespace GalaxyVibesPos.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class SupplierGroupAndCompanyCreate : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.SupplierCompany",
                c => new
                    {
                        CompanyID = c.Int(nullable: false, identity: true),
                        GroupID = c.Int(),
                        CompanyName = c.String(),
                    })
                .PrimaryKey(t => t.CompanyID)
                .ForeignKey("dbo.SupplierGroup", t => t.GroupID)
                .Index(t => t.GroupID);
            
            CreateTable(
                "dbo.SupplierGroup",
                c => new
                    {
                        GroupID = c.Int(nullable: false, identity: true),
                        GroupName = c.String(nullable: false),
                    })
                .PrimaryKey(t => t.GroupID);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.SupplierCompany", "GroupID", "dbo.SupplierGroup");
            DropIndex("dbo.SupplierCompany", new[] { "GroupID" });
            DropTable("dbo.SupplierGroup");
            DropTable("dbo.SupplierCompany");
        }
    }
}
