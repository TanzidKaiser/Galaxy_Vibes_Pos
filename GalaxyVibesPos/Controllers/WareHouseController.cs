using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using GalaxyVibesPos.Models;
using System.Data.Entity;

namespace GalaxyVibesPos.Controllers
{
    public class WareHouseController : Controller
    {
        DatabaseContext db = new DatabaseContext();
        // GET: WareHouse
        public ActionResult AddWireHouse()
        {

            ViewBag.MainLocations = GetWarehouse();
            return View();
        }

        // WarehouseNames return For Dropdown
        public dynamic GetWarehouse()
        {
            var mainLocations = db.LocationMain.ToList();
            List<SelectListItem> list = new List<SelectListItem>();
            list.Add(new SelectListItem { Text = "Select Warehouse", Value = "0" });
            foreach (var m in mainLocations)
            {
                list.Add(new SelectListItem { Text = m.LocationMainName, Value = Convert.ToString(m.LocationMainID) });

            }
            return list;
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
            var Recks = db.Location.Where(p => p.LocationMainID == LocationMainID).ToList();

            List<SelectListItem> CellList = new List<SelectListItem>();
            CellList.Add(new SelectListItem { Text = "Select Cell", Value = "0" });
            foreach (var m in Recks)
            {
                CellList.Add(new SelectListItem { Text = m.LocationName, Value = Convert.ToString(m.LocationID) });

            }


            return Json(new SelectList(CellList, "Value", "Text", JsonRequestBehavior.AllowGet));
        }

        public ActionResult SaveCallInDatabase(LocationSub model)
        {
            int i = 0;
            var Msg = "";
            var CallExists = db.LocationSub.Where(p => p.LocationSubName == model.LocationSubName && p.LocationID == model.LocationID && p.LocationMainID == model.LocationMainID).FirstOrDefault();
            if (CallExists == null)
            {
                try
                {
                    db.LocationSub.Add(model);
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
                Msg = "Call Name Exists";
            }

            return Json(Msg, JsonRequestBehavior.AllowGet);
        }
        //Warehouse Edit
        //[HttpGet]
        public ActionResult WirehouseIndex(int? id)
        {
            if(id == null)
            { 
            return View(db.LocationMain.ToList());
            }
            else
            {
                try {

                    var Wirehousedelete = db.LocationMain.Find(id);
                    db.LocationMain.Remove(Wirehousedelete);
                    db.SaveChanges();
                    return View(db.LocationMain.ToList());
                }catch(Exception)
                {
                    ViewBag.Msg = "আপনি সরাসরি ওয়্যারহাউজের নাম মুছে দিতে পারেন না । এক্ষেত্রে আগে আপনাকে একই ওয়্যারহাউজের রেকের নাম ও তার নিকট থাকা Cell নাম মুছতে হবে । ধন্যবাদ !";
                }
            }
            return View(db.LocationMain.ToList());
        }        
        public ActionResult WirehouseEdit(int id)
        {
            var wirehouse = db.LocationMain.Find(id);
            if (wirehouse == null)
            {
                ViewBag.Msg = 1;
                return View();

            }
            return View(wirehouse);
        }
        [HttpPost]
        public ActionResult WirehouseUpdate(LocationMain data)
        {
            var Msg = "";
            db.Entry(data).State = EntityState.Modified;
            int i = db.SaveChanges();
            if (i == 1)
            {
                Msg = "Update Successfully";
            }
            else
            {
                Msg = "Exception, Please Try Again";
            }


            return Json(Msg, JsonRequestBehavior.AllowGet);
        }

        //Rack Edit And Index 


        public ActionResult RackIndex(int? id)
        {
            var location = db.Location.ToList();

            if (id == null)
            {
                return View(location);
            }
            else
            {
                try
                {

                    var Wirehousedelete = db.Location.Find(id);
                    db.Location.Remove(Wirehousedelete);
                    db.SaveChanges();
                    return View(location);
                }
                catch (Exception)
                {
                    ViewBag.Msg = "আপনি সরাসরি রেকের নাম মুছে দিতে পারেন না । এক্ষেত্রে আগে আপনাকে একই রেকের নিকট থাকা Cell নাম মুছতে হবে । ধন্যবাদ !";
                }
            }
            return View(location);

        }
        public ActionResult RackEdit(int id)
        {
            var rack = db.Location.Find(id);
            if (rack == null)
            {
                ViewBag.Msg = 1;
                return View();

            }
            ViewBag.Warehouse = GetWarehouse();
            return View(rack);
        }
        [HttpPost]
        public ActionResult RackUpdate(Location data)
        {
            var Msg = "";
            db.Entry(data).State = EntityState.Modified;
            int i = db.SaveChanges();
            if (i == 1)
            {
                Msg = "Update Successfully";
            }
            else
            {
                Msg = "Exception, Please Try Again";
            }
            return Json(Msg, JsonRequestBehavior.AllowGet);
        }
        public ActionResult CellIndex(int? id)
        {
            var locationSub = db.LocationSub.ToList();

            if (id == null)
            {
                return View(locationSub);
            }
            else
            {
                try
                {

                    var Wirehousedelete = db.Location.Find(id);
                    db.Location.Remove(Wirehousedelete);
                    db.SaveChanges();
                    return View(locationSub);
                }
                catch (Exception)
                {
                    ViewBag.Msg = "আপনি সরাসরি রেকের নাম মুছে দিতে পারেন না । এক্ষেত্রে আগে আপনাকে একই রেকের নিকট থাকা Cell নাম মুছতে হবে । ধন্যবাদ !";
                }
            }
            return View(locationSub);

        }



    }
}