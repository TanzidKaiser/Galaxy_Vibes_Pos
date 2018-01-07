namespace GalaxyVibesPos.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addMainLocationAndLocationAndSubLocationCreate : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Location",
                c => new
                    {
                        LocationID = c.Int(nullable: false, identity: true),
                        LocationName = c.String(),
                        LocationMainID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.LocationID)
                .ForeignKey("dbo.LocationMain", t => t.LocationMainID, cascadeDelete: true)
                .Index(t => t.LocationMainID);
            
            CreateTable(
                "dbo.LocationMain",
                c => new
                    {
                        LocationMainID = c.Int(nullable: false, identity: true),
                        LocationMainName = c.String(),
                    })
                .PrimaryKey(t => t.LocationMainID);
            
            CreateTable(
                "dbo.LocationSub",
                c => new
                    {
                        LocationSubID = c.Int(nullable: false, identity: true),
                        LocationSubName = c.String(),
                        LocationMainID = c.Int(nullable: false),
                        LocationID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.LocationSubID)
                .ForeignKey("dbo.Location", t => t.LocationID, cascadeDelete: false)
                .ForeignKey("dbo.LocationMain", t => t.LocationMainID, cascadeDelete: false)
                .Index(t => t.LocationMainID)
                .Index(t => t.LocationID);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.LocationSub", "LocationMainID", "dbo.LocationMain");
            DropForeignKey("dbo.LocationSub", "LocationID", "dbo.Location");
            DropForeignKey("dbo.Location", "LocationMainID", "dbo.LocationMain");
            DropIndex("dbo.LocationSub", new[] { "LocationID" });
            DropIndex("dbo.LocationSub", new[] { "LocationMainID" });
            DropIndex("dbo.Location", new[] { "LocationMainID" });
            DropTable("dbo.LocationSub");
            DropTable("dbo.LocationMain");
            DropTable("dbo.Location");
        }
    }
}
