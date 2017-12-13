using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using GalaxyVibesPos.Models;
using CrystalDecisions.CrystalReports.Engine;
using System.IO;
using GalaxyVibesPos.Models.Temp_Class;
using Microsoft.Reporting.WebForms;

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
            ExportSaleInvoice(List);
            return Json(flag, JsonRequestBehavior.AllowGet);
        }

        private void ExportSaleInvoice(List<Sale> list)
        {
            var SaleReport = new[]
            {
               new {
                   SalesNo = string.Empty,
                   SalesDate = string.Empty,
                   SalesTime = string.Empty,
                   SalesRemarks = string.Empty,
                   SalePrice = (double?)0,
                   Quantity = (double?)0,
                   NetPayable = string.Empty,
                   CustomerName = string.Empty,
                   SoldBy = string.Empty,
                   ReceivedAmount = (double?)0,
                   ReturnAmount = string.Empty,
                   Vat = string.Empty,
                   ProductName = string.Empty,
                   Discount = string.Empty,
                   SubTotal = string.Empty,
                   TotalAmount = string.Empty,
                   Total = (double?)0
               }
            }.ToList();
            SaleReport.Clear();
            foreach (var item in list)
            {
                var aItem = new { SalesNo = item.SalesNo, SalesDate = item.SalesDate,
                    SalesTime = item.SalesTime,
                    SalesRemarks = item.SalesRemarks,
                    SalePrice = item.SalesSalePrice,
                    Quantity = item.SalesQuantity,
                    NetPayable = item.NetPayable,
                    CustomerName = item.SalesCustomerName,
                    SoldBy = item.SalesSoldBy,
                    ReceivedAmount = item.SalesReceivedAmount,
                    ReturnAmount = item.ReturnAmount,
                    Vat = item.SalesVat,
                    ProductName = item.ProductName,
                    Discount = item.TotalDiscount,
                    SubTotal = item.SubTotal,
                    TotalAmount = item.TotalAmount,
                    Total = item.Total
                };
                SaleReport.Add(aItem);
            }
            ReportDataSource reportDataSource = new ReportDataSource();
            reportDataSource.Name = "SaleDataSet";
            reportDataSource.Value = SaleReport;

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
        public ActionResult SalesReturn(string SalesProductID, string InvoiceNo, string PurchaseReturnQty, string NetReturnPrice, string AvgDiscount)
        {

            int? productID = Convert.ToInt32(SalesProductID);
            double? returnQty = Convert.ToInt32(PurchaseReturnQty);
            double? avgDiscount = Convert.ToDouble(AvgDiscount);
            double? returnPrice = Convert.ToDouble(NetReturnPrice);

            SaleUpdate(InvoiceNo, returnQty, returnPrice, avgDiscount);
            CustomerLedgerUpdate(InvoiceNo);
            int i = StokeUpdate(productID, returnQty);
            ViewBag.Msg = i;
            return View();
        }

        private void CustomerLedgerUpdate(string invoiceNo)
        {
            double? debit = 0;
            double? credit = 0;

            var aSale = db.Sale.Where(p => p.SalesNo == invoiceNo).FirstOrDefault();

            var customerInfo = db.CustomerLedger.Where(p => p.InvoiceNo == invoiceNo).Select(x => new { CustomerID = x.CustomerID, ID = x.ID, NetAmount = x.Credit }).FirstOrDefault();
            var LedgerList = db.CustomerLedger.Where(p => p.CustomerID == customerInfo.CustomerID).ToList();



            foreach (var a in LedgerList)
            {


                debit += a.Debit;
                credit += a.Credit;

            }

            //debit = debit + aSale.SalesVatTotal;
            //credit = credit + aSale.SalesReceivedAmount;

            var previousDue = debit - credit;

            var aLedger = db.CustomerLedger.SingleOrDefault(p => p.InvoiceNo == invoiceNo);
            aLedger.Debit = aSale.SalesVatTotal;
            aLedger.Credit = aSale.SalesReceivedAmount;

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

        private void SaleUpdate(string invoiceNo, double? returnQty, double? returnPrice, double? avgDiscount)
        {
            var aSale = db.Sale.Where(p => p.SalesNo == invoiceNo).FirstOrDefault();
            if (aSale != null)
            {

                var SalesQty = aSale.SalesQuantity - returnQty;
                var SalesProductDiscount = aSale.SalesProductDiscount - (avgDiscount * returnQty);
                var SalesTotal = (aSale.SalesSalePrice * SalesQty) - (avgDiscount * returnQty);
                var SalesVatRate = ((5 * SalesTotal) / 100);
                var SalesVatTotal = SalesVatRate + SalesTotal;

                var sales = db.Sale.SingleOrDefault(p => p.SalesNo == invoiceNo);

                sales.SalesQuantity = SalesQty;
                sales.SalesProductDiscount = SalesProductDiscount;
                sales.SalesTotal = SalesTotal;
                sales.SalesVatRate = SalesVatRate;
                sales.SalesVatTotal = SalesVatTotal;

                aSale.SalesReceivedAmount = aSale.SalesReceivedAmount - returnPrice;

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
            var id = Convert.ToInt32(Data);
            var customerInfo = db.Customer.Where(p => p.CustomerID == id).FirstOrDefault();
            var aCustomer = new
            {
                CustomerName = customerInfo.CustomerName,
                GroupName = customerInfo.GroupName,
                Address = customerInfo.Address
            };
            return Json(aCustomer, JsonRequestBehavior.AllowGet);
        }



    }
}
