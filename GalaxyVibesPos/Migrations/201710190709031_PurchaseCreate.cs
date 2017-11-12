namespace GalaxyVibesPos.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class PurchaseCreate : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Purchase",
                c => new
                    {
                        PurchaseID = c.Int(nullable: false, identity: true),
                        PurchaseNo = c.String(),
                        CompanyID = c.Int(),
                        PurchaseDate = c.String(),
                        SupplierID = c.Int(),
                        PurchaseSupplierInvoiceNo = c.String(),
                        PurchaseRemarks = c.String(),
                        PurchaseProductID = c.Int(),
                        PurchaseProductPrice = c.Double(),
                        PurchaseQuantity = c.Double(),
                        PurchaseTotal = c.Double(),
                    })
                .PrimaryKey(t => t.PurchaseID)
                .ForeignKey("dbo.Company", t => t.CompanyID)
                .ForeignKey("dbo.ProductDetails", t => t.PurchaseProductID)
                .ForeignKey("dbo.Supplier", t => t.SupplierID)
                .Index(t => t.CompanyID)
                .Index(t => t.SupplierID)
                .Index(t => t.PurchaseProductID);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Purchase", "SupplierID", "dbo.Supplier");
            DropForeignKey("dbo.Purchase", "PurchaseProductID", "dbo.ProductDetails");
            DropForeignKey("dbo.Purchase", "CompanyID", "dbo.Company");
            DropIndex("dbo.Purchase", new[] { "PurchaseProductID" });
            DropIndex("dbo.Purchase", new[] { "SupplierID" });
            DropIndex("dbo.Purchase", new[] { "CompanyID" });
            DropTable("dbo.Purchase");
        }
    }
}
