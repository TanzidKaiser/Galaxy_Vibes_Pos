namespace GalaxyVibesPos.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class CustomerLedgerCreate : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.CustomerLedgers",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        ReceiveDate = c.String(),
                        CustomerID = c.Int(nullable: false),
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
                .ForeignKey("dbo.Customer", t => t.CustomerID, cascadeDelete: true)
                .Index(t => t.CustomerID);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.CustomerLedgers", "CustomerID", "dbo.Customer");
            DropIndex("dbo.CustomerLedgers", new[] { "CustomerID" });
            DropTable("dbo.CustomerLedgers");
        }
    }
}
