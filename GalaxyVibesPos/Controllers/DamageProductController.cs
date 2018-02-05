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
        public ActionResult Damage()
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
        public ActionResult Damage(Purchase model)
        {
            return View();
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