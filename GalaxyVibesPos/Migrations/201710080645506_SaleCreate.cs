namespace GalaxyVibesPos.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class SaleCreate : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Sale",
                c => new
                    {
                        SalesID = c.Int(nullable: false, identity: true),
                        CompanyID = c.Int(nullable: false),
                        SalesNo = c.String(),
                        SalesDate = c.String(),
                        SalesTime = c.String(),
                        SalesCustomerID = c.Int(),
                        SalesRemarks = c.String(),
                        Reference = c.String(),
                        SalesProductID = c.Int(),
                        SalesPurchasePrice = c.Double(),
                        SalesSalePrice = c.Double(),
                        SalesQuantity = c.Double(),
                        SalesProductDiscount = c.Double(),
                        SalesTotal = c.Double(),
                        SalesCustomerName = c.String(),
                        SalesSoldBy = c.String(),
                        SalesReceivedAmount = c.Double(),
                        SalesChangeAmount = c.Double(),
                        SalesVatRate = c.Double(),
                        SalesVatTotal = c.Double(),
                        SalesPuechaseBy = c.String(),
                        SalesPurchaseByContact = c.String(),
                        PaymentType = c.Int(),
                    })
                .PrimaryKey(t => t.SalesID)
                .ForeignKey("dbo.Customer", t => t.SalesCustomerID)
                .ForeignKey("dbo.Company", t => t.CompanyID, cascadeDelete: true)
                .ForeignKey("dbo.ProductDetails", t => t.SalesProductID)
                .Index(t => t.CompanyID)
                .Index(t => t.SalesCustomerID)
                .Index(t => t.SalesProductID);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Sale", "SalesProductID", "dbo.ProductDetails");
            DropForeignKey("dbo.Sale", "CompanyID", "dbo.Company");
            DropForeignKey("dbo.Sale", "SalesCustomerID", "dbo.Customer");
            DropIndex("dbo.Sale", new[] { "SalesProductID" });
            DropIndex("dbo.Sale", new[] { "SalesCustomerID" });
            DropIndex("dbo.Sale", new[] { "CompanyID" });
            DropTable("dbo.Sale");
        }
    }
}
