using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using GalaxyVibesPos.Models;
namespace GalaxyVibesPos.Controllers
{
    public class WareHouseController : Controller
    {
        DatabaseContext db = new DatabaseContext();
        // GET: WareHouse
        public ActionResult AddWireHouse()
        {
            var mainLocations = db.LocationMain.ToList();
            List<SelectListItem> list = new List<SelectListItem>();
            list.Add(new SelectListItem { Text = "Select Warehouse", Value = "0" });
            foreach (var m in mainLocations)
            {
                list.Add(new SelectListItem { Text = m.LocationMainName, Value = Convert.ToString(m.LocationMainID) });

            }
            ViewBag.MainLocations = list;

            return View();
        }

        public ActionResult SaveWareHouseInDatabase(LocationMain model)
        {
            int i = 0;
            var nameExists = db.LocationMain.Where(p => p.LocationMainName == model.LocationMainName).FirstOrDefault();
            if (nameExists == null)
            {
                db.LocationMain.Add(model);
                i = db.SaveChanges();
            }
            else
            {
                i = 2;
            }

            return Json(i, JsonRequestBehavior.AllowGet);
        }
        public ActionResult SaveRackInDatabase(Location model)
        {
            int i = 0;
            var Msg = "";

            var locationExists = db.Location.Where(p => p.LocationMainID == model.LocationMainID && p.LocationName == model.LocationName).FirstOrDefault();
            if (locationExists == null)
            {
                try
                {
                    db.Location.Add(model);
                    i = db.SaveChanges();
                    if (i == 1)
                    {
                        Msg = "Save Successfull";
                    }
                }
                catch (Exception ex)
                {
                    Msg = Convert.ToString(ex);
                }


            }
            else
            {
                Msg = "Rack Name Already Exists";
            }



            return Json(Msg, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetLocationByMainLocation(int LocationMainID)
        {
            var Recks = db.Location.Where(p=> p.LocationMainID == LocationMainID).ToList();

            List<SelectListItem> CellList = new List<SelectListItem>();
            CellList.Add(new SelectListItem { Text = "Select Cell", Value = "0" });
            foreach(var m in Recks)
            {
                CellList.Add(new SelectListItem { Text = m.LocationName, Value =Convert.ToString(m.LocationID)});

            }


            return Json(new SelectList(CellList, "Value", "Text", JsonRequestBehavior.AllowGet));
        }

        public ActionResult SaveCallInDatabase(LocationSub model)
        {
            int i = 0;
            var Msg = "";
            var CallExists = db.LocationSub.Where(p => p.LocationSubName == model.LocationSubName && p.LocationID == model.LocationID && p.LocationMainID == model.LocationMainID).FirstOrDefault();
            if(CallExists == null)
            {
                try
                {
                    db.LocationSub.Add(model);
                    i = db.SaveChanges();
                    if(i == 1)
                    {
                        Msg = "Save Successfull";
                    }
                                        
                }catch(Exception ex)
                {
                    Msg = Convert.ToString(ex);
                }
            }
            else
            {
                Msg = "Call Name Exists";
            }

            return Json(Msg, JsonRequestBehavior.AllowGet);
        }
    }
}