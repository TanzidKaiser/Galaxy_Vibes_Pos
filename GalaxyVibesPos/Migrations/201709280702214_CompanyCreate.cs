namespace GalaxyVibesPos.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class CompanyCreate : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Companies",
                c => new
                    {
                        CompanyID = c.Int(nullable: false, identity: true),
                        GroupID = c.Int(),
                        CompanyName = c.String(),
                    })
                .PrimaryKey(t => t.CompanyID)
                .ForeignKey("dbo.Group", t => t.GroupID)
                .Index(t => t.GroupID);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Companies", "GroupID", "dbo.Group");
            DropIndex("dbo.Companies", new[] { "GroupID" });
            DropTable("dbo.Companies");
        }
    }
}
