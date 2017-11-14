using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using GalaxyVibesPos.Models;
using CrystalDecisions.CrystalReports.Engine;
using System.IO;

namespace GalaxyVibesPos.Controllers
{
    public class SalesController : Controller
    {
        DatabaseContext db = new DatabaseContext();
        //
        // GET: /Sales/
        public ActionResult AddSales()
        {


            int? MaxId = db.Sale.Max(x => (int?)x.SalesID);
            
            if (MaxId == null)
            {
                
                ViewBag.SalesNo = "010000" + "" + 1;
            }           
            else
            {
                var saleNoSelect = db.Sale.Where(x => x.SalesID == MaxId).Select(x => x.SalesNo).First();
                var saleNo = Convert.ToInt32(saleNoSelect);
                saleNo += 1;
                ViewBag.SalesNo = saleNo;
            }

            ViewBag.date = DateTime.Now.ToString("M/d/yyyy");
            ViewBag.time = DateTime.Now.ToString("hh:mm:ss tt");

            return View();
        }

        //Get Customer Name in Dropdown By Select Company Name 

        public JsonResult GetCustomerBySearch(string Prefix)
        {
            DatabaseContext db = new DatabaseContext();

            var allSearch = (from N in db.Customer
                             where N.CustomerName.StartsWith(Prefix)
                             select new { N.CustomerName, N.GroupName, N.CompanyName, N.PreviousDue, N.Address });



            return Json(allSearch, JsonRequestBehavior.AllowGet);
        }
        public JsonResult Save(List<Sale> List)
        {
            var flag = 0;
            CustomerLedger aCustomerLedger = new CustomerLedger();
            aCustomerLedger.Debit = 0;
            
            foreach (var item in List)
            {
                Sale aSale = new Sale();

                aSale.CompanyID = item.CompanyID = GetCompanyID(item.CompanyName, item.SalesCustomerName);
                aSale.PaymentType = item.PaymentType = 1;
                aSale.SalesCustomerID = item.SalesCustomerID = GetCustomerID(item.CompanyName, item.SalesCustomerName);
                aSale.SalesPurchasePrice = item.SalesPurchasePrice = GetPurchasePrice(item.SalesProductID);
                aSale.SalesVatRate = item.SalesVatRate = 5;
                aSale.SalesSoldBy = item.SalesSoldBy = "admin";


                aSale.Reference = item.Reference;
                aSale.SalesCustomerName = item.SalesCustomerName;
                aSale.SalesDate = item.SalesDate;
                aSale.SalesNo = item.SalesNo;
                aSale.SalesPuechaseBy = item.SalesPuechaseBy;
                aSale.SalesPurchaseByContact = item.SalesPurchaseByContact;
                aSale.SalesReceivedAmount = item.SalesReceivedAmount;
                if (aSale.SalesReceivedAmount == null)
                {
                    aSale.SalesReceivedAmount = 0;
                }
                aSale.SalesRemarks = item.SalesRemarks;

                aSale.SalesProductDiscount = item.SalesProductDiscount;
                aSale.SalesProductID = item.SalesProductID;
                aSale.SalesQuantity = item.SalesQuantity;
                aSale.SalesSalePrice = item.SalesSalePrice;
                aSale.SalesTime = item.SalesTime;
                aSale.SalesTotal = item.SalesTotal;
                //aSale.SalesVatTotal = item.SalesVatTotal;

                //double? vatTotal = (((item.SalesTotal * 5) / 100) + item.SalesTotal);

                double? vatTotal = (((item.SalesTotal * 5) / 100) + item.SalesTotal);


                aSale.SalesVatTotal = vatTotal;

                if (item.SalesChangeAmount != null)
                {
                    aSale.SalesChangeAmount = item.SalesChangeAmount;
                }
                else
                {

                    aSale.SalesChangeAmount = -(item.DueAmount);

                }

                StokeDecrement(item.SalesProductID, item.SalesQuantity);

                //For Customer Ledger

                aCustomerLedger.InvoiceNo = item.SalesNo;

                aCustomerLedger.Debit += vatTotal;

                if (item.SalesReceivedAmount != null)
                {
                    aCustomerLedger.Credit = item.SalesReceivedAmount;

                }
                else
                {
                    aCustomerLedger.Credit = 0;
                }
                aCustomerLedger.CustomerID = Convert.ToInt32(aSale.SalesCustomerID);
                aCustomerLedger.ReceiveDate = item.SalesDate;
                
                db.Sale.Add(aSale);
                 int i = db.SaveChanges();
                if(i>0)
                {
                    flag = 1;
                }


            }

            CustomerLedgerCreate(aCustomerLedger);
            //ExportSaleInvoice(List);
            return Json(flag, JsonRequestBehavior.AllowGet);
        }

      

        public virtual ActionResult ExportSaleInvoice(List<Sale> list)
        {
            ReportDocument rd = new ReportDocument();
            rd.Load(Path.Combine(Server.MapPath("~/Report/CristalReportSalesInvoiceReport.rpt")));
            List<SaleTemp> SaleList = new List<SaleTemp>();
            foreach(var name in list)
            {
                SaleTemp aSale = new SaleTemp();
                aSale.SalesDate = name.SalesDate;
                aSale.SalesTime = name.SalesTime;
                aSale.SalesNo = name.SalesNo;
                aSale.SalesCustomerName = name.SalesCustomerName;

                var ProductName = db.productDetails.Where(x => x.ProductDetailsID == name.SalesProductID).Select(x => x.ProductName).FirstOrDefault();
                aSale.ProductName = ProductName;

                aSale.SalesQuantity = Convert.ToInt32(name.SalesQuantity);
                aSale.SalesSalePrice = Convert.ToInt32(name.SalesSalePrice); 
                                
                var total = name.SalesQuantity * name.SalesSalePrice;
                aSale.Total = Convert.ToInt32(total);

                aSale.SubTotal = name.SubTotal;
                aSale.TotalDiscount = name.TotalDiscount;
                aSale.TotalAmount = name.TotalAmount;
                aSale.SalesVat = name.SalesVat;
                aSale.NetPayable = name.NetPayable;
                aSale.SalesReceivedAmount = Convert.ToInt32(name.SalesReceivedAmount);
                aSale.ReturnAmount = name.ReturnAmount;
                aSale.SalesRemarks = name.SalesRemarks;
                aSale.SalesSoldBy = name.SalesSoldBy;

                SaleList.Add(aSale);

            }            
            rd.SetDataSource(SaleList);
            Response.Buffer = false;
            Response.ClearContent();
            Response.ClearHeaders();

            Stream str = rd.ExportToStream(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat);
            str.Seek(0, SeekOrigin.Begin);
            return File(str, "application/pdf", "report.pdf");
                    
        }

        private void CustomerLedgerCreate(CustomerLedger aCustomerLedger)
        {


            var aCustomer = db.Customer.SingleOrDefault(p => p.CustomerID == aCustomerLedger.CustomerID);
            if (aCustomer.CustomerName != "CASH")
            {
                aCustomer.PreviousDue = GetPriviousDue(aCustomerLedger);
                if (aCustomer.PreviousDue != 0)
                {
                    aCustomerLedger.IsPreviousDue = 1;
                }
                else
                {
                    aCustomerLedger.IsPreviousDue = 0;
                }

                db.CustomerLedger.Add(aCustomerLedger);
                db.SaveChanges();
            }
            else
            {
                db.CustomerLedger.Add(aCustomerLedger);
                db.SaveChanges();
            }

        }

        private double? GetPriviousDue(CustomerLedger aCustomerLedger)
        {

            var totalDebit = db.CustomerLedger.Where(c => c.CustomerID == aCustomerLedger.CustomerID)
                .GroupBy(c => c.CustomerID).Select(g => new { debit = g.Sum(c => c.Debit) }).First();

            var totalCredit = db.CustomerLedger.Where(c => c.CustomerID == aCustomerLedger.CustomerID)
                .GroupBy(c => c.CustomerID).Select(g => new { credit = g.Sum(c => c.Credit) }).First();

            double? previousDue = (totalDebit.debit + aCustomerLedger.Debit) - (totalCredit.credit + aCustomerLedger.Credit);

            return previousDue;
        }

        private void StokeDecrement(int? productID, double? quantity)
        {


            var aProductDetails = db.productDetails.SingleOrDefault(p => p.ProductDetailsID == productID);
            aProductDetails.Stoke -= quantity;
            db.SaveChanges();


        }


        private double? GetPurchasePrice(int? productID)
        {
            var purChasePrice = db.productDetails.First(p => p.ProductDetailsID == productID).PurchasePrice;

            return purChasePrice;
        }

        // Get Customer ID Function

        private int? GetCustomerID(string companyName, string CustomerName)
        {
            if (CustomerName != null)
            {
                var customerID = db.Customer.First(p => p.CustomerName == CustomerName && p.CompanyName == companyName).CustomerID;

                return Convert.ToInt32(customerID);
            }
            else
            {
                var customerID = db.Customer.First(p => p.CustomerName == "CASH" && p.CompanyName == "CASH").CustomerID;

                return Convert.ToInt32(customerID);
            }
        }

        // Get Company ID Function

        private int GetCompanyID(string companyName, string CustomerName)
        {
            if (companyName != null)
            {
                var companyID = db.Customer.First(p => p.CustomerName == CustomerName && p.CompanyName == companyName).CompanyID;

                return Convert.ToInt32(companyID);
            }
            else
            {
                var companyID = db.Customer.First(p => p.CustomerName == "CASH" && p.CompanyName == "CASH").CompanyID;

                return Convert.ToInt32(companyID);
            }

        }





    }
}
