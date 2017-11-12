namespace GalaxyVibesPos.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class SupplierCreate : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Supplier",
                c => new
                    {
                        SupplierID = c.Int(nullable: false, identity: true),
                        SupplierName = c.String(),
                        SupplierContactPerson = c.String(),
                        SupplierPhone = c.String(),
                        SupplierVatRegNo = c.String(),
                        SupplierEmail = c.String(),
                        SupplierAddress = c.String(),
                        GroupID = c.Int(),
                        CompanyID = c.Int(),
                        SupplierPreviousDue = c.Double(),
                    })
                .PrimaryKey(t => t.SupplierID)
                .ForeignKey("dbo.SupplierCompany", t => t.CompanyID)
                .ForeignKey("dbo.SupplierGroup", t => t.GroupID)
                .Index(t => t.GroupID)
                .Index(t => t.CompanyID);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Supplier", "GroupID", "dbo.SupplierGroup");
            DropForeignKey("dbo.Supplier", "CompanyID", "dbo.SupplierCompany");
            DropIndex("dbo.Supplier", new[] { "CompanyID" });
            DropIndex("dbo.Supplier", new[] { "GroupID" });
            DropTable("dbo.Supplier");
        }
    }
}
