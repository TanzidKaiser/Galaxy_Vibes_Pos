namespace GalaxyVibesPos.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class IncomeTblCreate : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Income",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        CompanyID = c.Int(),
                        Date = c.String(),
                        Description = c.String(),
                        Remarks = c.String(),
                        Amount = c.Double(nullable: false),
                    })
                .PrimaryKey(t => t.ID);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.Income");
        }
    }
}
