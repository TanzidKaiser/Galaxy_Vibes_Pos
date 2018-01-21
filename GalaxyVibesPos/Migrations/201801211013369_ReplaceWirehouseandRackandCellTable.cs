namespace GalaxyVibesPos.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ReplaceWirehouseandRackandCellTable : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Location", "LocationMainID", "dbo.LocationMain");
            DropForeignKey("dbo.LocationSub", "LocationID", "dbo.Location");
            DropForeignKey("dbo.LocationSub", "LocationMainID", "dbo.LocationMain");
            DropIndex("dbo.Location", new[] { "LocationMainID" });
            DropIndex("dbo.LocationSub", new[] { "LocationMainID" });
            DropIndex("dbo.LocationSub", new[] { "LocationID" });
            CreateTable(
                "dbo.Cell",
                c => new
                    {
                        CellID = c.Int(nullable: false, identity: true),
                        CellName = c.String(),
                        WarehouseID = c.Int(nullable: false),
                        RackID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.CellID)
                .ForeignKey("dbo.Rack", t => t.RackID, cascadeDelete: false)
                .ForeignKey("dbo.Warehouse", t => t.WarehouseID, cascadeDelete: false)
                .Index(t => t.WarehouseID)
                .Index(t => t.RackID);
            
            CreateTable(
                "dbo.Rack",
                c => new
                    {
                        RackID = c.Int(nullable: false, identity: true),
                        RackName = c.String(),
                        WarehouseID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.RackID)
                .ForeignKey("dbo.Warehouse", t => t.WarehouseID, cascadeDelete: true)
                .Index(t => t.WarehouseID);
            
            CreateTable(
                "dbo.Warehouse",
                c => new
                    {
                        WarehouseID = c.Int(nullable: false, identity: true),
                        WarehouseName = c.String(),
                    })
                .PrimaryKey(t => t.WarehouseID);
            
            CreateIndex("dbo.ProductDetails", "WarehouseID");
            CreateIndex("dbo.ProductDetails", "RackID");
            CreateIndex("dbo.ProductDetails", "CellID");
            AddForeignKey("dbo.ProductDetails", "CellID", "dbo.Cell", "CellID");
            AddForeignKey("dbo.ProductDetails", "RackID", "dbo.Rack", "RackID");
            AddForeignKey("dbo.ProductDetails", "WarehouseID", "dbo.Warehouse", "WarehouseID");
            DropTable("dbo.Location");
            DropTable("dbo.LocationMain");
            DropTable("dbo.LocationSub");
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.LocationSub",
                c => new
                    {
                        LocationSubID = c.Int(nullable: false, identity: true),
                        LocationSubName = c.String(),
                        LocationMainID = c.Int(nullable: false),
                        LocationID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.LocationSubID);
            
            CreateTable(
                "dbo.LocationMain",
                c => new
                    {
                        LocationMainID = c.Int(nullable: false, identity: true),
                        LocationMainName = c.String(),
                    })
                .PrimaryKey(t => t.LocationMainID);
            
            CreateTable(
                "dbo.Location",
                c => new
                    {
                        LocationID = c.Int(nullable: false, identity: true),
                        LocationName = c.String(),
                        LocationMainID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.LocationID);
            
            DropForeignKey("dbo.Rack", "WarehouseID", "dbo.Warehouse");
            DropForeignKey("dbo.ProductDetails", "WarehouseID", "dbo.Warehouse");
            DropForeignKey("dbo.Cell", "WarehouseID", "dbo.Warehouse");
            DropForeignKey("dbo.ProductDetails", "RackID", "dbo.Rack");
            DropForeignKey("dbo.Cell", "RackID", "dbo.Rack");
            DropForeignKey("dbo.ProductDetails", "CellID", "dbo.Cell");
            DropIndex("dbo.Rack", new[] { "WarehouseID" });
            DropIndex("dbo.Cell", new[] { "RackID" });
            DropIndex("dbo.Cell", new[] { "WarehouseID" });
            DropIndex("dbo.ProductDetails", new[] { "CellID" });
            DropIndex("dbo.ProductDetails", new[] { "RackID" });
            DropIndex("dbo.ProductDetails", new[] { "WarehouseID" });
            DropTable("dbo.Warehouse");
            DropTable("dbo.Rack");
            DropTable("dbo.Cell");
            CreateIndex("dbo.LocationSub", "LocationID");
            CreateIndex("dbo.LocationSub", "LocationMainID");
            CreateIndex("dbo.Location", "LocationMainID");
            AddForeignKey("dbo.LocationSub", "LocationMainID", "dbo.LocationMain", "LocationMainID", cascadeDelete: true);
            AddForeignKey("dbo.LocationSub", "LocationID", "dbo.Location", "LocationID", cascadeDelete: true);
            AddForeignKey("dbo.Location", "LocationMainID", "dbo.LocationMain", "LocationMainID", cascadeDelete: true);
        }
    }
}
