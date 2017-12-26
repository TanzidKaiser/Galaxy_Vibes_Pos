namespace GalaxyVibesPos.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class salesDateTypeUpdate : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Sale", "SalesDate", c => c.DateTime(nullable: false));
            AlterColumn("dbo.CustomerLedgers", "ReceiveDate", c => c.DateTime(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.CustomerLedgers", "ReceiveDate", c => c.String());
            AlterColumn("dbo.Sale", "SalesDate", c => c.String());
        }
    }
}
