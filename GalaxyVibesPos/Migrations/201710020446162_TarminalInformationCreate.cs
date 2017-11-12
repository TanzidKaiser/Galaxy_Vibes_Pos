namespace GalaxyVibesPos.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class TarminalInformationCreate : DbMigration
    {
        public override void Up()
        {
            RenameTable(name: "dbo.Companies", newName: "Company");
            CreateTable(
                "dbo.TarminalInformation",
                c => new
                    {
                        CompanyID = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        Address = c.String(),
                        AddressOptional = c.String(),
                        Mobile = c.String(),
                        MobileOptional = c.String(),
                        Phone = c.String(),
                        PhoneOptional = c.String(),
                        Fax = c.String(),
                        FaxOptional = c.String(),
                        VatNo = c.String(),
                        TradeLicense = c.String(),
                        TinNo = c.String(),
                        Email = c.String(),
                        Website = c.String(),
                        VatRate = c.Double(),
                        Status = c.Int(nullable: false),
                        Image = c.Binary(),
                        IsShowRoom = c.Int(),
                    })
                .PrimaryKey(t => t.CompanyID);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.TarminalInformation");
            RenameTable(name: "dbo.Company", newName: "Companies");
        }
    }
}
