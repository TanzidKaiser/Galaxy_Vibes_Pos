using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using GalaxyVibesPos.Models;
using GalaxyVibesPos.Models.Temp_Class;
using CrystalDecisions.CrystalReports.Engine;
using System.IO;
using System.Data.Entity;

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

                StockIncrement(item.PurchaseProductID, item.PurchaseQuantity);

                db.SaveChanges();


            }

            // Supplier ledger create function

            SupplierLedgerCreate(aPurchaserForLedger.PurchaseSupplierInvoiceNo, aPurchaserForLedger.TotalAmount, aPurchaserForLedger.SupplierID, aPurchaserForLedger.PurchaseDate);

            return Json("Save Successfull", JsonRequestBehavior.AllowGet);
        }


        public ActionResult ExportPurchaseInvoice(List<Purchase> purchaseList)
        {

            ReportDocument rd = new ReportDocument();
            rd.Load(Path.Combine(Server.MapPath("~/Report/Expense Report/PurchaseCrystalReport.rpt")));

            int serialNo = 0;
            List<PurchaseTemp> purchaseTempList = new List<PurchaseTemp>();
            Purchase aPurchase = new Purchase();

            foreach (var id in purchaseList)
            {
                aPurchase.SupplierID = id.SupplierID;

            }

            var supplier = db.Supplier.Where(s => s.SupplierID == aPurchase.SupplierID).FirstOrDefault();

            foreach (var purchase in purchaseList)
            {
                PurchaseTemp pt = new PurchaseTemp();
                var productInfo = from pd in db.productDetails.Where(x => x.ProductDetailsID == purchase.PurchaseProductID)
                                  join sc in db.CategorySub on pd.SubCategoryID equals sc.SubCategoryID
                                  select new { Code = pd.Code, ProductName = pd.ProductName, SubcategoryName = sc.SubCategoryName };
                foreach (var product in productInfo)
                {
                    pt.ProductCode = product.Code;
                    pt.SubCategoryName = product.SubcategoryName;
                    pt.ProductName = product.ProductName;
                }
                serialNo++;
                pt.SerialNO = serialNo;
                pt.PurchaseNo = purchase.PurchaseNo;
                pt.SupplierInvoiceNo = purchase.PurchaseSupplierInvoiceNo;
                pt.PurchasePrice = Convert.ToString(purchase.PurchaseProductPrice);
                pt.ProductQuantity = Convert.ToInt32(purchase.PurchaseQuantity);
                pt.Total = Convert.ToInt32(purchase.PurchaseTotal);
                pt.TQuantity = purchase.Tquantity;
                pt.SubTotal = Convert.ToInt32(purchase.TotalAmount);
                pt.Date = purchase.PurchaseDate;

                pt.Name = supplier.SupplierName;
                pt.Phone = supplier.SupplierPhone;
                pt.Email = supplier.SupplierEmail;
                pt.Address = supplier.SupplierAddress + "," + supplier.supplierGroup.GroupName + "," + supplier.supplierCompany.CompanyName;

                purchaseTempList.Add(pt);
            }
            rd.SetDataSource(purchaseTempList);
            Response.Buffer = false;
            Response.ClearContent();
            Response.ClearHeaders();

            Stream str = rd.ExportToStream(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat);
            str.Seek(0, SeekOrigin.Begin);
            return File(str, "application/pdf", "Expense Report.pdf");
        }

        private void StockIncrement(int? productID, double? quantity)
        {
            var aProductDetails = db.productDetails.SingleOrDefault(p => p.ProductDetailsID == productID);
            aProductDetails.Stoke = aProductDetails.Stoke + quantity;
            db.SaveChanges();

        }

        private void SupplierLedgerCreate(string invoiceNO, double? totalAmount, int? supplierID, string date)
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
                .GroupBy(c => c.SupplierID).Select(g => new { dabit = g.Sum(c => c.Debit) }).First();

            var totalCredit = db.SupplierLedger.Where(c => c.SupplierID == supplierID)
                .GroupBy(c => c.SupplierID).Select(g => new { credit = g.Sum(c => c.Credit) }).First();

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

            var allSearch = (from N in db.productDetails
                             where N.Code.StartsWith(Prefix)
                             select new { N.Code, N.Stoke, N.ProductName, N.UnitID, N.PurchasePrice, N.ProductDetailsID, N.CategorySub.SubCategoryName, N.SalePrice });



            return Json(allSearch, JsonRequestBehavior.AllowGet);
        }

        //Purchase Return

        public ActionResult PUrchaseReturn()
        {
            return View();
        }
        public class PurchaseViewbag
        {
            public int Code { get; set; }
            public int? Id { get; set; }
            public string Name { get; set; }
            public double? Price { get; set; }
            public double? Quantity { get; set; }
            public double? Total { get; set; }
        }
        [HttpPost]
        public ActionResult PUrchaseReturn(string Data)
        {

            var purchaseList = db.Purchase.Where(p => p.PurchaseNo == Data).ToList();

            List<PurchaseViewbag> list = new List<PurchaseViewbag>();
             
            foreach (var item in purchaseList)
            {
                var productName = db.productDetails.First(p => p.ProductDetailsID == item.PurchaseProductID).ProductName;

                var aPurchase = new PurchaseViewbag
                {
                    Code = item.PurchaseID,
                    Id = item.PurchaseProductID,
                    Name = productName,
                    Price = item.PurchaseProductPrice,
                    Quantity = item.PurchaseQuantity,
                    Total = item.PurchaseTotal
                };

                //var List = new[] { aPurchase };
                list.Add(aPurchase);
            }
            ViewData["purchaseList"] = list;
            return View();
            //return Json(list, JsonRequestBehavior.AllowGet);
        }


    }
}