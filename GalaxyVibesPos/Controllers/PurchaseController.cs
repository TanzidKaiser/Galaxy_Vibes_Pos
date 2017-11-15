using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using GalaxyVibesPos.Models;
using GalaxyVibesPos.Models.Temp_Class;

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
        public JsonResult save(List<Purchase> List)
        {

            var flag = 0;

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
                
                flag =  db.SaveChanges();

                if (flag>0)
                {
                    flag = 1;
                }

            }
            
            // Supplier ledger create function
       
            SupplierLedgerCreate(aPurchaserForLedger.PurchaseSupplierInvoiceNo, aPurchaserForLedger.TotalAmount, aPurchaserForLedger.SupplierID, aPurchaserForLedger.PurchaseDate);

            return Json(flag, JsonRequestBehavior.AllowGet);
        }


        public ActionResult ExportPurchaseInvoice(List<Purchase> purchaseList)
        {
            List<PurchaseTemp> purchaseTempList = new List<PurchaseTemp>();
            Purchase aPurchase = new Purchase();
           

            foreach(var supplier in purchaseList)
            {
                aPurchase.SupplierID = supplier.SupplierID;
                aPurchase.PurchaseProductID = supplier.PurchaseProductID;
            }
            
            var supplierinfo = db.Supplier.Where(s => s.SupplierID == aPurchase.SupplierID).FirstOrDefault();

            //var productInfo = from cs in db.CategorySub
            //                  join pd in db.productDetails on cs.SubCategoryID equals pd.SubCategoryID
            //                  join pi in aPurchase on pd.pro

            //var productInfo = db.productDetails.Where(p => p.ProductDetailsID == aPurchase.PurchaseProductID).Select(db.CategorySub.Whe));
            //from a in tableA
            //from b in tableB.Where(x => x.A == a.A || x.B == a.B)
            //select new { a, b };

            //var productinfo = from a in db.productDetails.Where(x => x.ProductDetailsID == aPurchase.PurchaseProductID)
            //                  from b in db.CategorySub.Where(m => m.SubCategoryID == a.SubCategoryID)
            //                  select new{ ProductCode = a.Code,name = b.SubCategoryName };
                              


            foreach (var model in purchaseList)
            {
                PurchaseTemp aPurchaseTemp = new PurchaseTemp();
                aPurchaseTemp.Name = supplierinfo.SupplierName;
                aPurchaseTemp.Address = supplierinfo.SupplierAddress+"\n"+ 

            }
            return View();
        }

        private void StockIncrement(int? productID, double? quantity)
        {
            var aProductDetails = db.productDetails.FirstOrDefault(p => p.ProductDetailsID == productID);
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


    }
}