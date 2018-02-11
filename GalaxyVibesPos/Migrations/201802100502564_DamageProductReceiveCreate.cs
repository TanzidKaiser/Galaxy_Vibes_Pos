namespace GalaxyVibesPos.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class DamageProductReceiveCreate : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.DamageProductReceive",
                c => new
                    {
                        DamageProductID = c.Int(nullable: false, identity: true),
                        DamageProductNo = c.String(),
                        CompanyID = c.Int(),
                        DamageProductDate = c.String(),
                        SupplierID = c.Int(),
                        InvoiceNo = c.String(),
                        DamageProductRemarks = c.String(),
                        DamageProductProductID = c.Int(),
                        DamageProductPrice = c.Double(),
                        DamageProductQuantity = c.Double(),
                        DamageProductTotal = c.Double(),
                    })
                .PrimaryKey(t => t.DamageProductID);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.DamageProductReceive");
        }
    }
}
