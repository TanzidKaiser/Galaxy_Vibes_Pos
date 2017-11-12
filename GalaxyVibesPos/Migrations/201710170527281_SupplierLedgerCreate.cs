namespace GalaxyVibesPos.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class SupplierLedgerCreate : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.SupplierLedgers",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        ReceiveDate = c.String(),
                        SupplierID = c.Int(nullable: false),
                        InvoiceNo = c.String(),
                        Debit = c.Double(),
                        Credit = c.Double(),
                        Adjustment = c.Double(),
                        PaymentType = c.String(),
                        BankName = c.String(),
                        ChequeNo = c.String(),
                        ChequeDate = c.String(),
                        Remarks = c.String(),
                        NextPaymentDate = c.String(),
                        IsPreviousDue = c.Int(),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.Supplier", t => t.SupplierID, cascadeDelete: true)
                .Index(t => t.SupplierID);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.SupplierLedgers", "SupplierID", "dbo.Supplier");
            DropIndex("dbo.SupplierLedgers", new[] { "SupplierID" });
            DropTable("dbo.SupplierLedgers");
        }
    }
}
