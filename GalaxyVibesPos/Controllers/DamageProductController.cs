using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using GalaxyVibesPos.Models;
namespace GalaxyVibesPos.Controllers
{
    public class DamageProductController : Controller
    {
        DatabaseContext db = new DatabaseContext();

        // GET: DamageProduct
        public ActionResult DamageReceive()
        {
            string date = DateTime.Now.ToString("Mdyyyy");
            string time = DateTime.Now.ToString("hhmmss");
            Random rnd = new Random();
            string rndNo = Convert.ToString(rnd.Next(500));
            ViewBag.trackNo = "DPN" + "" + date + "" + time + "" + rndNo;
            ViewBag.time = DateTime.Now.ToString("hh:mm:ss tt");
            //ViewBag.date = DateTime.Now.ToString("mm/dd/yyyy");

            return View();
        }

        [HttpPost]
        public ActionResult DamageReceive(List<DamageProductReceive> List)
        {
            var flag = 0;

            foreach (var product in List)
            {
                DamageProductReceive dmr = new DamageProductReceive();

                dmr.DamageProductNo = product.DamageProductNo;
                dmr.CompanyID = 0;
                dmr.DamageProductDate = product.DamageProductDate;
                dmr.SupplierID = 0;
                dmr.InvoiceNo = product.InvoiceNo;
                dmr.DamageProductRemarks = product.DamageProductRemarks;
                dmr.DamageProductProductID = product.DamageProductProductID;
                dmr.DamageProductPrice = product.DamageProductPrice;
                dmr.DamageProductQuantity = product.DamageProductQuantity;
                dmr.DamageProductTotal = product.DamageProductTotal;

                db.DamageProductReceive.Add(dmr);

                int i = db.SaveChanges();

                if(i>0)
                {
                    flag = 1;
                }
            }

            return Json(flag, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetProducts(string Prefix)
        {

            if (Prefix.StartsWith("p0") || Prefix.StartsWith("P0"))
            {
                var result = (from n in db.productDetails
                              where n.Code.StartsWith(Prefix)
                              select new { n.Code, n.Stoke, n.ProductName, n.ProductUnit.UnitSize, n.PurchasePrice, n.ProductDetailsID, key = 0 }).ToList();

                return Json(result, JsonRequestBehavior.AllowGet);
            }
            else
            {
                var result = (from n in db.productDetails
                              where n.ProductName.StartsWith(Prefix)
                              select new { n.Code, n.Stoke, n.ProductName, n.ProductUnit.UnitSize, n.PurchasePrice, n.ProductDetailsID, key = 1 }).ToList();

                return Json(result, JsonRequestBehavior.AllowGet);
            }

        }

    }
}