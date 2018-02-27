using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using GalaxyVibesPos.Models;
using CrystalDecisions.CrystalReports.Engine;
using System.IO;
using Microsoft.Reporting.WebForms;
using System.Web.Script.Serialization;
using System.Printing;
using System.Drawing.Printing;
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
            //ViewBag.time = DateTime.Now.ToString("hh:mm:ss tt");

            return View();
        }
        //[HttpPost]
        //public ActionResult AddSales(Sale aSale,)
        //{
        //    //ExportSaleInvoice(temp);
        //    return RedirectToAction("AddSales",false);
        //}

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
                aSale.SalesTotal = item.Total;


                double? vatTotal = (((item.Total * 5) / 100) + item.Total);


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
                    if (item.SalesReceivedAmount < vatTotal)
                    {
                        aCustomerLedger.Credit = item.SalesReceivedAmount;
                    }
                    else
                    {
                        aCustomerLedger.Credit = vatTotal;
                    }
                }
                else
                {
                    aCustomerLedger.Credit = 0;
                }
                aCustomerLedger.CustomerID = Convert.ToInt32(aSale.SalesCustomerID);
                aCustomerLedger.ReceiveDate = item.SalesDate;

                db.Sale.Add(aSale);
                int i = db.SaveChanges();
                if (i > 0)
                {
                    flag = 1;
                }


            }

            CustomerLedgerCreate(aCustomerLedger);
            //ExportSaleInvoice(List);
            return Json(flag, JsonRequestBehavior.AllowGet);
        }

        public void ExportSaleInvoice(string encrift)
        {

            byte[] b = Convert.FromBase64String(encrift);
            System.Text.UTF8Encoding encoder = new System.Text.UTF8Encoding();
            System.Text.Decoder utf8Decoder = encoder.GetDecoder();

            int charCount = utf8Decoder.GetCharCount(b, 0, b.Length);
            char[] decodedChar = new char[charCount];
            utf8Decoder.GetChars(b, 0, b.Length, decodedChar, 0);
            string result = new string(decodedChar);

            JavaScriptSerializer js = new JavaScriptSerializer();
            Sale[] SaleList = js.Deserialize<Sale[]>(result);

            ReportDataSource reportDataSource = new ReportDataSource();
            reportDataSource.Name = "SalesDataSet";
            reportDataSource.Value = SaleList;

            string mimeType = string.Empty;
            string encodeing = string.Empty;
            string fileNameExtension = "pdf";
            Warning[] warnings;
            string[] streams;

            LocalReport localReport = new LocalReport();
            localReport.ReportPath = Server.MapPath("~/Report/Sale/SaleReport.rdlc");
            localReport.DataSources.Add(reportDataSource);

            byte[] bytes = localReport.Render("PDF", null, out mimeType, out encodeing, out fileNameExtension, out streams, out warnings);

            Response.Buffer = true;
            Response.Clear();
            Response.ContentType = mimeType;
            Response.AddHeader("content-disposition", "attachment;filename=file." + fileNameExtension);
            Response.BinaryWrite(bytes);
            Response.Flush();

            //var pq = LocalPrintServer.GetDefaultPrintQueue();
            //using (var job = pq.AddJob())
            //using (var s = job.JobStream)
            //{
            //    s.Write(bytes, 0, bytes.Length);
            //}


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

        // Sales Return 

        public ActionResult SalesReturn()
        {
            return View();
        }
        [HttpPost]
        public ActionResult SalesReturn(string SalesID, string SalesProductID, string InvoiceNo, string PurchaseReturnQty, string NetReturnPrice, string AvgDiscount)
        {

            int salesID = Convert.ToInt32(SalesID);
            int? productID = Convert.ToInt32(SalesProductID);
            double? returnQty = Convert.ToInt32(PurchaseReturnQty);
            double? avgDiscount = Convert.ToDouble(AvgDiscount);
            double? returnPrice = Convert.ToDouble(NetReturnPrice);

            SaleUpdate(InvoiceNo, returnQty, returnPrice, avgDiscount,salesID);
            CustomerLedgerUpdate(InvoiceNo);
            int i = StokeUpdate(productID, returnQty);
            ViewBag.Msg = i;
            return View();
        }

        private void CustomerLedgerUpdate(string invoiceNo)
        {
            double? debit = 0;
            double? credit = 0;
            double? salesVatTotal = 0;
            double? salesReceivedAmount = 0;

            var aSale = db.Sale.Where(p => p.SalesNo == invoiceNo).ToList();

            var customerInfo = db.CustomerLedger.Where(p => p.InvoiceNo == invoiceNo).Select(x => new { CustomerID = x.CustomerID, ID = x.ID, NetAmount = x.Credit }).FirstOrDefault();
            var LedgerList = db.CustomerLedger.Where(p => p.CustomerID == customerInfo.CustomerID).ToList();

          
            foreach (var a in LedgerList)
            {


                debit += a.Debit;
                credit += a.Credit;

            }

            foreach (var a in aSale)
            {
                salesVatTotal += a.SalesVatTotal;
                salesReceivedAmount = a.SalesReceivedAmount-a.SalesChangeAmount;
            }

            if(salesVatTotal % 1 != 0)
            {
                var n = salesVatTotal % 1;
                var adjust = 1.00 - n;
                salesVatTotal += adjust;
            }

            var previousDue = debit - credit;

            var aLedger = db.CustomerLedger.SingleOrDefault(p => p.InvoiceNo == invoiceNo);
            aLedger.Debit = salesVatTotal;
            aLedger.Credit = salesReceivedAmount;

            var aCustomer = db.Customer.SingleOrDefault(p => p.CustomerID == customerInfo.CustomerID);
            aCustomer.PreviousDue = previousDue;

        }

        private int StokeUpdate(int? productID, double? returnQty)
        {
            var aProduct = db.productDetails.SingleOrDefault(p => p.ProductDetailsID == productID);
            aProduct.Stoke = aProduct.Stoke + returnQty;
            int i = db.SaveChanges();
            return i;
        }

        private void SaleUpdate(string invoiceNo, double? returnQty, double? returnPrice, double? avgDiscount, int salesID)
        {
            
            var receiveAmountList = db.Sale.Where(p => p.SalesNo == invoiceNo).ToList(); 
           
            var aSale = db.Sale.Where(p => p.SalesNo == invoiceNo && p.SalesID == salesID).FirstOrDefault();
            
            if (aSale != null)
            {

                var SalesQty = aSale.SalesQuantity - returnQty;
                var SalesProductDiscount = aSale.SalesProductDiscount - (avgDiscount * returnQty);
                var SalesTotal = (aSale.SalesSalePrice * SalesQty) - (avgDiscount * SalesQty);
                var SalesVatRate = ((5 * SalesTotal) / 100);
                var SalesVatTotal = SalesVatRate + SalesTotal;
                if(SalesVatTotal % 1 != 0)
                {
                    SalesVatTotal += (1.00 - (SalesVatTotal % 1));
                }
                var sales = db.Sale.SingleOrDefault(p => p.SalesNo == invoiceNo && p.SalesID == salesID);

                sales.SalesQuantity = SalesQty;
                sales.SalesProductDiscount = SalesProductDiscount;
                sales.SalesTotal = SalesTotal;
                sales.SalesVatRate = SalesVatRate;
                sales.SalesVatTotal = SalesVatTotal;

                foreach(var a in receiveAmountList)
                {
                    var receiveAmountUpdate = db.Sale.SingleOrDefault(p => p.SalesID == a.SalesID);

                    receiveAmountUpdate.SalesReceivedAmount = receiveAmountUpdate.SalesReceivedAmount - SalesVatTotal;
                }
                

            }
        }


        [HttpPost]
        public JsonResult GetSaleListbyInvoiceNo(string Data)
        {
            var SaleList = db.Sale.Where(p => p.SalesNo == Data).ToList();
            var List = new[]
            {
                new
                {
                    SalesID = (int)0,
                    ProductID =(int?)0,
                    Name = string.Empty,
                    Price = (double?)0,
                    Quantity = (double?)0,
                    Discount = (double?)0

                }

            }.Where(e => false).ToList();

            foreach (var item in SaleList)
            {
                var productName = db.productDetails.First(p => p.ProductDetailsID == item.SalesProductID).ProductName;

                var aSale = new
                {
                    SalesID = item.SalesID,
                    ProductID = item.SalesProductID,
                    Name = productName,
                    Price = item.SalesSalePrice,
                    Quantity = item.SalesQuantity,
                    Discount = item.SalesProductDiscount
                };

                List.Add(aSale);
            }
            return Json(List, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetSalesCustomerInfo(string Data)
        {
            var aCustomer = new { CustomerName = string.Empty, GroupName = string.Empty, Address = string.Empty };
                       
            var id = Convert.ToInt32(Data);            
            var customerId = db.Sale.First(p => p.SalesID == id).SalesCustomerID;
            var customerInfo = db.Customer.Where(p => p.CustomerID == customerId).FirstOrDefault();

            if (customerInfo != null)
            {
                aCustomer = new
                {
                    CustomerName = customerInfo.CustomerName,
                    GroupName = customerInfo.GroupName,
                    Address = customerInfo.Address
                };
            }

            return Json(aCustomer, JsonRequestBehavior.AllowGet);
        }



    }
}
