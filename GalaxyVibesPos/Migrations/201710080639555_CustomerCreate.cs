namespace GalaxyVibesPos.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class CustomerCreate : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Customer",
                c => new
                    {
                        CustomerID = c.Int(nullable: false, identity: true),
                        CompanyID = c.Int(),
                        GroupName = c.String(),
                        CompanyName = c.String(),
                        CustomerName = c.String(),
                        Gender = c.String(),
                        Phone = c.String(),
                        VatRegNo = c.String(),
                        Email = c.String(),
                        Address = c.String(),
                        PreviousDue = c.Double(),
                    })
                .PrimaryKey(t => t.CustomerID)
                .ForeignKey("dbo.Company", t => t.CompanyID)
                .Index(t => t.CompanyID);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Customer", "CompanyID", "dbo.Company");
            DropIndex("dbo.Customer", new[] { "CompanyID" });
            DropTable("dbo.Customer");
        }
    }
}
