using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using GalaxyVibesPos.Models;
namespace GalaxyVibesPos.Controllers
{
    public class SettingsController : Controller
    {
        DatabaseContext db = new DatabaseContext();
        // GET: Settings
        public ActionResult AddUnit()
        {
            var model = db.ProductUnit.ToList();
            return View(model);
        }
        [HttpPost]
        public ActionResult AddUnit(ProductUnit model)
        {
            int i = 0;

           
                

                var unit = db.ProductUnit.Where(p => p.UnitSize == model.UnitSize).FirstOrDefault();
                if (unit == null)
                {
                    i = 1;
                    db.ProductUnit.Add(model);
                    db.SaveChanges();
                }
                else
                {
                    i = 0;
                }
            
            ViewBag.Message =  i ;
            return View(db.ProductUnit.ToList());
        }
        public ActionResult EditUnit(string unitId)
        {
            var id = Convert.ToInt64(unitId);
            var units = db.ProductUnit.Where(p => p.UnitID == id).First();
            ProductUnit aProdudtunit = new ProductUnit()
            {
                UnitID = units.UnitID,
                UnitSize = units.UnitSize
            };

            return Json(aProdudtunit, JsonRequestBehavior.AllowGet);
        }
        public ActionResult UpdateUnit(ProductUnit model)
        {
            var unit = db.ProductUnit.SingleOrDefault(p => p.UnitID == model.UnitID);
            unit.UnitSize = model.UnitSize;
            db.SaveChanges();
            return Json("Update Successfully", JsonRequestBehavior.AllowGet);
        }
        public ActionResult DelateUnit(string unitId)
        {
            var id = Convert.ToInt32(unitId);
            var aUnit = db.ProductUnit.Where(p => p.UnitID == id).First();
            db.ProductUnit.Remove(aUnit);
            db.SaveChanges();
            return Json("Delete Success", JsonRequestBehavior.AllowGet);

        }

    }
}