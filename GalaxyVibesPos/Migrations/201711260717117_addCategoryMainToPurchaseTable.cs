namespace GalaxyVibesPos.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addCategoryMainToPurchaseTable : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Category",
                c => new
                    {
                        CategoryID = c.Int(nullable: false, identity: true),
                        MainCategoryID = c.Int(nullable: false),
                        CategoryName = c.String(nullable: false),
                    })
                .PrimaryKey(t => t.CategoryID)
                .ForeignKey("dbo.CategoryMain", t => t.MainCategoryID, cascadeDelete: true)
                .Index(t => t.MainCategoryID);
            
            CreateTable(
                "dbo.CategoryMain",
                c => new
                    {
                        MainCategoryID = c.Int(nullable: false, identity: true),
                        MaincategoryName = c.String(nullable: false),
                    })
                .PrimaryKey(t => t.MainCategoryID);
            
            CreateTable(
                "dbo.CategorySub",
                c => new
                    {
                        SubCategoryID = c.Int(nullable: false, identity: true),
                        MainCategoryID = c.Int(nullable: false),
                        CategoryID = c.Int(nullable: false),
                        SubCategoryName = c.String(nullable: false),
                    })
                .PrimaryKey(t => t.SubCategoryID)
                .ForeignKey("dbo.Category", t => t.CategoryID, cascadeDelete: false)
                .ForeignKey("dbo.CategoryMain", t => t.MainCategoryID, cascadeDelete: false)
                .Index(t => t.MainCategoryID)
                .Index(t => t.CategoryID);
            
            CreateTable(
                "dbo.ProductDetails",
                c => new
                    {
                        ProductDetailsID = c.Int(nullable: false, identity: true),
                        MainCategoryID = c.Int(nullable: false),
                        CategoryID = c.Int(nullable: false),
                        SubCategoryID = c.Int(nullable: false),
                        ProductName = c.String(nullable: false),
                        Code = c.String(),
                        PurchasePrice = c.Double(),
                        SalePrice = c.Double(),
                        Stoke = c.Double(),
                        Description = c.String(),
                        UnitID = c.Int(),
                        MinimumProductQuantity = c.Double(),
                        WarehouseID = c.Int(),
                        RackID = c.Int(),
                        CellID = c.Int(),
                        Status = c.Int(),
                    })
                .PrimaryKey(t => t.ProductDetailsID)
                .ForeignKey("dbo.Category", t => t.CategoryID, cascadeDelete: false)
                .ForeignKey("dbo.CategoryMain", t => t.MainCategoryID, cascadeDelete: false)
                .ForeignKey("dbo.CategorySub", t => t.SubCategoryID, cascadeDelete: false)
                .Index(t => t.MainCategoryID)
                .Index(t => t.CategoryID)
                .Index(t => t.SubCategoryID);
            
            CreateTable(
                "dbo.Purchase",
                c => new
                    {
                        PurchaseID = c.Int(nullable: false, identity: true),
                        PurchaseNo = c.String(),
                        CompanyID = c.Int(),
                        PurchaseDate = c.String(),
                        SupplierID = c.Int(),
                        PurchaseSupplierInvoiceNo = c.String(),
                        PurchaseRemarks = c.String(),
                        PurchaseProductID = c.Int(),
                        PurchaseProductPrice = c.Double(),
                        PurchaseQuantity = c.Double(),
                        PurchaseTotal = c.Double(),
                    })
                .PrimaryKey(t => t.PurchaseID)
                .ForeignKey("dbo.SupplierCompany", t => t.CompanyID)
                .ForeignKey("dbo.ProductDetails", t => t.PurchaseProductID)
                .ForeignKey("dbo.Supplier", t => t.SupplierID)
                .Index(t => t.CompanyID)
                .Index(t => t.SupplierID)
                .Index(t => t.PurchaseProductID);
            
            CreateTable(
                "dbo.SupplierCompany",
                c => new
                    {
                        CompanyID = c.Int(nullable: false, identity: true),
                        GroupID = c.Int(),
                        CompanyName = c.String(),
                    })
                .PrimaryKey(t => t.CompanyID)
                .ForeignKey("dbo.SupplierGroup", t => t.GroupID)
                .Index(t => t.GroupID);
            
            CreateTable(
                "dbo.SupplierGroup",
                c => new
                    {
                        GroupID = c.Int(nullable: false, identity: true),
                        GroupName = c.String(nullable: false),
                    })
                .PrimaryKey(t => t.GroupID);
            
            CreateTable(
                "dbo.Supplier",
                c => new
                    {
                        SupplierID = c.Int(nullable: false, identity: true),
                        SupplierName = c.String(),
                        SupplierContactPerson = c.String(),
                        SupplierPhone = c.String(),
                        SupplierVatRegNo = c.String(),
                        SupplierEmail = c.String(),
                        SupplierAddress = c.String(),
                        GroupID = c.Int(),
                        CompanyID = c.Int(),
                        SupplierPreviousDue = c.Double(),
                    })
                .PrimaryKey(t => t.SupplierID)
                .ForeignKey("dbo.SupplierCompany", t => t.CompanyID)
                .ForeignKey("dbo.SupplierGroup", t => t.GroupID)
                .Index(t => t.GroupID)
                .Index(t => t.CompanyID);
            
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
            
            CreateTable(
                "dbo.Sale",
                c => new
                    {
                        SalesID = c.Int(nullable: false, identity: true),
                        CompanyID = c.Int(nullable: false),
                        SalesNo = c.String(),
                        SalesDate = c.String(),
                        SalesTime = c.String(),
                        SalesCustomerID = c.Int(),
                        SalesRemarks = c.String(),
                        Reference = c.String(),
                        SalesProductID = c.Int(),
                        SalesPurchasePrice = c.Double(),
                        SalesSalePrice = c.Double(),
                        SalesQuantity = c.Double(),
                        SalesProductDiscount = c.Double(),
                        SalesTotal = c.Double(),
                        SalesCustomerName = c.String(),
                        SalesSoldBy = c.String(),
                        SalesReceivedAmount = c.Double(),
                        SalesChangeAmount = c.Double(),
                        SalesVatRate = c.Double(),
                        SalesVatTotal = c.Double(),
                        SalesPuechaseBy = c.String(),
                        SalesPurchaseByContact = c.String(),
                        PaymentType = c.Int(),
                    })
                .PrimaryKey(t => t.SalesID)
                .ForeignKey("dbo.Customer", t => t.SalesCustomerID)
                .ForeignKey("dbo.Company", t => t.CompanyID, cascadeDelete: true)
                .ForeignKey("dbo.ProductDetails", t => t.SalesProductID)
                .Index(t => t.CompanyID)
                .Index(t => t.SalesCustomerID)
                .Index(t => t.SalesProductID);
            
            CreateTable(
                "dbo.Company",
                c => new
                    {
                        CompanyID = c.Int(nullable: false, identity: true),
                        GroupID = c.Int(),
                        CompanyName = c.String(),
                    })
                .PrimaryKey(t => t.CompanyID)
                .ForeignKey("dbo.Group", t => t.GroupID)
                .Index(t => t.GroupID);
            
            CreateTable(
                "dbo.Customer",
                c => new
                    {
                        CustomerID = c.Int(nullable: false, identity: true),
                        CompanyID = c.Int(),
                        GroupName = c.String(),
                        CompanyName = c.String(),
                        CustomerName = c.String(),
                        Gender = c.String(),
                        Phone = c.String(),
                        VatRegNo = c.String(),
                        Email = c.String(),
                        Address = c.String(),
                        PreviousDue = c.Double(),
                    })
                .PrimaryKey(t => t.CustomerID)
                .ForeignKey("dbo.Company", t => t.CompanyID)
                .Index(t => t.CompanyID);
            
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
            
            CreateTable(
                "dbo.Group",
                c => new
                    {
                        GroupID = c.Int(nullable: false, identity: true),
                        GroupName = c.String(nullable: false),
                    })
                .PrimaryKey(t => t.GroupID);
            
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
            DropForeignKey("dbo.Sale", "SalesProductID", "dbo.ProductDetails");
            DropForeignKey("dbo.Sale", "CompanyID", "dbo.Company");
            DropForeignKey("dbo.Company", "GroupID", "dbo.Group");
            DropForeignKey("dbo.Sale", "SalesCustomerID", "dbo.Customer");
            DropForeignKey("dbo.CustomerLedgers", "CustomerID", "dbo.Customer");
            DropForeignKey("dbo.Customer", "CompanyID", "dbo.Company");
            DropForeignKey("dbo.SupplierLedgers", "SupplierID", "dbo.Supplier");
            DropForeignKey("dbo.Supplier", "GroupID", "dbo.SupplierGroup");
            DropForeignKey("dbo.Supplier", "CompanyID", "dbo.SupplierCompany");
            DropForeignKey("dbo.Purchase", "SupplierID", "dbo.Supplier");
            DropForeignKey("dbo.Purchase", "PurchaseProductID", "dbo.ProductDetails");
            DropForeignKey("dbo.SupplierCompany", "GroupID", "dbo.SupplierGroup");
            DropForeignKey("dbo.Purchase", "CompanyID", "dbo.SupplierCompany");
            DropForeignKey("dbo.ProductDetails", "SubCategoryID", "dbo.CategorySub");
            DropForeignKey("dbo.ProductDetails", "MainCategoryID", "dbo.CategoryMain");
            DropForeignKey("dbo.ProductDetails", "CategoryID", "dbo.Category");
            DropForeignKey("dbo.CategorySub", "MainCategoryID", "dbo.CategoryMain");
            DropForeignKey("dbo.CategorySub", "CategoryID", "dbo.Category");
            DropForeignKey("dbo.Category", "MainCategoryID", "dbo.CategoryMain");
            DropIndex("dbo.CustomerLedgers", new[] { "CustomerID" });
            DropIndex("dbo.Customer", new[] { "CompanyID" });
            DropIndex("dbo.Company", new[] { "GroupID" });
            DropIndex("dbo.Sale", new[] { "SalesProductID" });
            DropIndex("dbo.Sale", new[] { "SalesCustomerID" });
            DropIndex("dbo.Sale", new[] { "CompanyID" });
            DropIndex("dbo.SupplierLedgers", new[] { "SupplierID" });
            DropIndex("dbo.Supplier", new[] { "CompanyID" });
            DropIndex("dbo.Supplier", new[] { "GroupID" });
            DropIndex("dbo.SupplierCompany", new[] { "GroupID" });
            DropIndex("dbo.Purchase", new[] { "PurchaseProductID" });
            DropIndex("dbo.Purchase", new[] { "SupplierID" });
            DropIndex("dbo.Purchase", new[] { "CompanyID" });
            DropIndex("dbo.ProductDetails", new[] { "SubCategoryID" });
            DropIndex("dbo.ProductDetails", new[] { "CategoryID" });
            DropIndex("dbo.ProductDetails", new[] { "MainCategoryID" });
            DropIndex("dbo.CategorySub", new[] { "CategoryID" });
            DropIndex("dbo.CategorySub", new[] { "MainCategoryID" });
            DropIndex("dbo.Category", new[] { "MainCategoryID" });
            DropTable("dbo.TarminalInformation");
            DropTable("dbo.Group");
            DropTable("dbo.CustomerLedgers");
            DropTable("dbo.Customer");
            DropTable("dbo.Company");
            DropTable("dbo.Sale");
            DropTable("dbo.SupplierLedgers");
            DropTable("dbo.Supplier");
            DropTable("dbo.SupplierGroup");
            DropTable("dbo.SupplierCompany");
            DropTable("dbo.Purchase");
            DropTable("dbo.ProductDetails");
            DropTable("dbo.CategorySub");
            DropTable("dbo.CategoryMain");
            DropTable("dbo.Category");
        }
    }
}
