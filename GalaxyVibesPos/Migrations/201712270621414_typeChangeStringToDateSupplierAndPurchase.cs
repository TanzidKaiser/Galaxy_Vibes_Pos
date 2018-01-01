namespace GalaxyVibesPos.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class typeChangeStringToDateSupplierAndPurchase : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Purchase", "PurchaseDate", c => c.DateTime(nullable: false));
            AlterColumn("dbo.SupplierLedgers", "ReceiveDate", c => c.DateTime(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.SupplierLedgers", "ReceiveDate", c => c.String());
            AlterColumn("dbo.Purchase", "PurchaseDate", c => c.String());
        }
    }
}
