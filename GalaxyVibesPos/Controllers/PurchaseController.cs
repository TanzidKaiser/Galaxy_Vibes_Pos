using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using GalaxyVibesPos.Models;
using CrystalDecisions.CrystalReports.Engine;
using System.IO;
using System.Data.Entity;
using System.Data.Entity.Validation;
using System.Web.Script.Serialization;
using Microsoft.Reporting.WebForms;

namespace GalaxyVibesPos.Controllers
{
    public class PurchaseController : Controller
    {
        DatabaseContext db = new DatabaseContext();
        //
        // GET: /Purchase/
        public ActionResult PurchaseAdd()
        {
            string date = DateTime.Now.ToString("Mdyyyy");
            string time = DateTime.Now.ToString("hhmmss");
            Random rnd = new Random();
            string rndNo = Convert.ToString(rnd.Next(500));
            ViewBag.trackNo = date + "" + time + "" + rndNo;
            ViewBag.time = DateTime.Now.ToString("hh:mm:ss tt");
            ViewBag.date = DateTime.Now.ToString("yyyy/M/d");
            ViewBag.supplierGroups = db.SupplierGroup.ToList();

            return View();
        }
        [HttpPost]
        public ActionResult PurchaseAdd(List<Purchase> purchaseList)
        {

            return View();
        }
        [HttpPost]
        public JsonResult Save(List<Purchase> List)
        {

            Purchase aPurchaserForLedger = new Purchase();

            foreach (var item in List)
            {
                Purchase aPurchase = new Purchase();


                aPurchase.PurchaseNo = item.PurchaseNo;
                aPurchase.CompanyID = item.CompanyID;
                aPurchase.PurchaseDate = item.PurchaseDate;
                aPurchase.SupplierID = item.SupplierID;
                aPurchase.PurchaseSupplierInvoiceNo = item.PurchaseSupplierInvoiceNo;
                aPurchase.PurchaseRemarks = "Na";
                aPurchase.PurchaseProductID = item.PurchaseProductID;
                aPurchase.PurchaseProductPrice = item.PurchaseProductPrice;
                aPurchase.PurchaseQuantity = item.PurchaseQuantity;
                aPurchase.PurchaseTotal = item.PurchaseTotal;
                db.Purchase.Add(aPurchase);

                aPurchaserForLedger.TotalAmount = item.TotalAmount;
                aPurchaserForLedger.PurchaseSupplierInvoiceNo = item.PurchaseSupplierInvoiceNo;
                aPurchaserForLedger.SupplierID = item.SupplierID;
                aPurchaserForLedger.PurchaseDate = item.PurchaseDate;

                //Stoke increment function
                db.SaveChanges();

                StockIncrement(item.PurchaseProductID, item.PurchaseQuantity);
            }

            // Supplier ledger create function

            SupplierLedgerCreate(aPurchaserForLedger.PurchaseSupplierInvoiceNo, aPurchaserForLedger.TotalAmount, aPurchaserForLedger.SupplierID, aPurchaserForLedger.PurchaseDate);

            return Json("Save Successfull", JsonRequestBehavior.AllowGet);
        }

        public void ExportPurchaseInvoice(string encrift)
        {
            List<Purchase> purchase = new List<Purchase>();

            byte[] b = Convert.FromBase64String(encrift);
            System.Text.UTF8Encoding encoder = new System.Text.UTF8Encoding();
            System.Text.Decoder utf8Decoder = encoder.GetDecoder();

            int charCount = utf8Decoder.GetCharCount(b, 0, b.Length);
            char[] decodedChar = new char[charCount];
            utf8Decoder.GetChars(b, 0, b.Length, decodedChar, 0);
            string result = new string(decodedChar);

            JavaScriptSerializer js = new JavaScriptSerializer();
            Purchase[] PurchaseList = js.Deserialize<Purchase[]>(result);

            foreach(var a in PurchaseList)
            {
                Purchase aPurchase = new Purchase();
                aPurchase.PurchaseNo = a.PurchaseNo;
                aPurchase.PurchaseDate = a.PurchaseDate;
                aPurchase.PurchaseSupplierInvoiceNo = a.PurchaseSupplierInvoiceNo;
                aPurchase.ProductCode = db.productDetails.First(p => p.ProductDetailsID == a.PurchaseProductID).Code;
                aPurchase.ProductName = db.productDetails.First(p => p.ProductDetailsID == a.PurchaseProductID).ProductName;               
                purchase.Add(aPurchase);
            }
            ReportDataSource reportDataSource = new ReportDataSource();
            reportDataSource.Name = "PurchaseDataSet";
            reportDataSource.Value = purchase;

            string mimeType = string.Empty;
            string encodeing = string.Empty;
            string fileNameExtension = "pdf";
            Warning[] warnings;
            string[] streams;

            LocalReport localReport = new LocalReport();
            localReport.ReportPath = Server.MapPath("~/Report/Purchase/PurchaseReport.rdlc");
            localReport.DataSources.Add(reportDataSource);

            byte[] bytes = localReport.Render("PDF", null, out mimeType, out encodeing, out fileNameExtension, out streams, out warnings);

            Response.Buffer = true;
            Response.Clear();
            Response.ContentType = mimeType;
            Response.AddHeader("content-disposition", "attachment;filename=file." + fileNameExtension);
            Response.BinaryWrite(bytes);
            Response.Flush();           
        }


        private void StockIncrement(int? productID, double? quantity)
        {
            var aProductDetails = db.productDetails.SingleOrDefault(p => p.ProductDetailsID == productID);
            aProductDetails.Stoke = aProductDetails.Stoke + quantity;
            db.SaveChanges();

        }

        private void SupplierLedgerCreate(string invoiceNO, double? totalAmount, int? supplierID, DateTime date)
        {

            SupplierLedger aSupplierLedger = new SupplierLedger();
            var aSupplier = db.Supplier.SingleOrDefault(p => p.SupplierID == supplierID);
            aSupplierLedger.ReceiveDate = date;
            aSupplierLedger.SupplierID = Convert.ToInt32(supplierID);
            aSupplierLedger.InvoiceNo = invoiceNO;
            aSupplierLedger.Debit = 0;
            aSupplierLedger.Credit = totalAmount;
            aSupplier.SupplierPreviousDue = GetPreviousDue(totalAmount, supplierID);
            if (aSupplier.SupplierPreviousDue != 0)
            {
                aSupplierLedger.IsPreviousDue = 1;
            }
            else
            {
                aSupplierLedger.IsPreviousDue = 0;
            }

            db.SupplierLedger.Add(aSupplierLedger);
            db.SaveChanges();
        }

        private double? GetPreviousDue(double? totalAmountPurchase, int? supplierID)
        {


            var totalDebit = db.SupplierLedger.Where(c => c.SupplierID == supplierID)
                .GroupBy(c => c.SupplierID).Select(g => new { dabit = g.Sum(c => c.Debit) }).FirstOrDefault();

            var totalCredit = db.SupplierLedger.Where(c => c.SupplierID == supplierID)
                .GroupBy(c => c.SupplierID).Select(g => new { credit = g.Sum(c => c.Credit) }).FirstOrDefault();

            double? previousDue = (totalCredit.credit + totalAmountPurchase) - totalDebit.dabit;

            return previousDue;
        }







        //Get Supplier Name in Dropdown By Select Company Name 


        public JsonResult GetSupplierByCompanyID(int CompanyID)
        {

            var suppliersList = db.Supplier.Where(m => m.CompanyID == CompanyID).ToList();
            var suppliers = suppliersList.Select(m => new SelectListItem()
            {
                Text = m.SupplierName,
                Value = m.SupplierID.ToString(),
            });

            return Json(suppliers, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult GetSupplierAddress(int Id)
        {


            var select = db.Supplier.Where(a => a.SupplierID == Id).First();
            Supplier aSupplier = new Supplier()
            {

                SupplierAddress = select.SupplierAddress,
            };

            return Json(aSupplier, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetProductBySearch(string Prefix)
        {
            DatabaseContext db = new DatabaseContext();

            if (Prefix.StartsWith("p0") || Prefix.StartsWith("P0"))
            {
                var allSearch = (from N in db.productDetails
                                 where N.Code.StartsWith(Prefix)
                                 select new { N.Code, N.Stoke, N.ProductName, N.ProductUnit.UnitSize, N.PurchasePrice, N.ProductDetailsID, N.CategorySub.SubCategoryName, N.SalePrice, key = 0 });
                return Json(allSearch, JsonRequestBehavior.AllowGet);

            }
            else
            {
                var allSearch = (from N in db.productDetails
                                 where N.ProductName.StartsWith(Prefix)
                                 select new { N.Code, N.Stoke, N.ProductName, N.ProductUnit.UnitSize, N.PurchasePrice, N.ProductDetailsID, N.CategorySub.SubCategoryName, N.SalePrice, key = 1 });
                return Json(allSearch, JsonRequestBehavior.AllowGet);

            }

          
        }

        //Purchase Return

        public ActionResult PUrchaseReturn()
        {
            return View();
        }

        [HttpPost]
        public ActionResult GetPurchaseListbyInvoiceNo(string Data)
        {

            var purchaseList = db.Purchase.Where(p => p.PurchaseNo == Data).ToList();

            var _List = new[]
            {
                new
                {
                    Code =(int)0,
                    ID = (int?)0,
                    Name = string.Empty,
                    Price = (double?)0,
                    Quantity = (double?)0,
                    Total = (double?)0
                }
            }.Where(e => false).ToList();

            foreach (var item in purchaseList)
            {
                var productName = db.productDetails.First(p => p.ProductDetailsID == item.PurchaseProductID).ProductName;

                var aPurchase = new
                {
                    Code = item.PurchaseID,
                    ID = item.PurchaseProductID,
                    Name = productName,
                    Price = item.PurchaseProductPrice,
                    Quantity = item.PurchaseQuantity,
                    Total = item.PurchaseTotal
                };

                //var List = new[] { aPurchase };
                _List.Add(aPurchase);
            }

            return Json(_List, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public ActionResult PUrchaseReturn(Purchase purchase)
        {
            // ---  Product Return on Same inventory ------
            int i = 0;
            productReturn(purchase);

            StokeUpdate(purchase);

            i = LedgerUpdate(purchase);
            if (i == 1)
            {
                ViewBag.Msg = i;
            }

            ViewBag.Msg = i;
            //-- Stoke Update on Return Purchase Qty

            return View();
        }

        private int LedgerUpdate(Purchase purchase)
        {
            int i = 0;
            var aPurchase = db.Purchase.Where(p => p.PurchaseID == purchase.PurchaseID).FirstOrDefault();

            var aSupplierLedger = db.SupplierLedger.SingleOrDefault(p => p.InvoiceNo == aPurchase.PurchaseSupplierInvoiceNo);

            aSupplierLedger.Credit = aPurchase.PurchaseTotal;

            i = db.SaveChanges();

            if (i != 0)
            {

                //------ Supplier update due Calculate

                var totalDebit = db.SupplierLedger.Where(c => c.SupplierID == aSupplierLedger.SupplierID)
                   .GroupBy(c => c.SupplierID).Select(g => new { dabit = g.Sum(c => c.Debit) }).FirstOrDefault();

                var totalCredit = db.SupplierLedger.Where(c => c.SupplierID == aSupplierLedger.SupplierID)
                    .GroupBy(c => c.SupplierID).Select(g => new { credit = g.Sum(c => c.Credit) }).FirstOrDefault();

                double? previousDue = totalCredit.credit - totalDebit.dabit;

                var aSupplier = db.Supplier.SingleOrDefault(p => p.SupplierID == aSupplierLedger.SupplierID);

                aSupplier.SupplierPreviousDue = previousDue;

                if (previousDue != 0)
                {
                    aSupplierLedger.IsPreviousDue = 1;
                }
                else
                {
                    aSupplierLedger.IsPreviousDue = 0;
                }


                i = db.SaveChanges();
            }

            return i;

        }

        private void StokeUpdate(Purchase purchase)
        {
            var aProduct = db.productDetails.SingleOrDefault(p => p.ProductDetailsID == purchase.PurchaseProductID);
            aProduct.Stoke = aProduct.Stoke - purchase.PurchaseReturnQty;

        }

        private void productReturn(Purchase purchase)
        {

            var aPurchase = db.Purchase.SingleOrDefault(p => p.PurchaseID == purchase.PurchaseID);

            double? _CurrentQty = aPurchase.PurchaseQuantity - purchase.PurchaseReturnQty;
            double? _CurrentPurchaseTotal = purchase.PurchaseProductPrice * _CurrentQty;

            aPurchase.PurchaseQuantity = _CurrentQty;
            aPurchase.PurchaseTotal = _CurrentPurchaseTotal;



        }
    }
}