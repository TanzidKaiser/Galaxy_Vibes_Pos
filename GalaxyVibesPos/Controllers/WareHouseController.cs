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

            ViewBag.Warehouse = GetWarehouse();
            return View();
        }

        // WarehouseNames return For Dropdown
        public dynamic GetWarehouse()
        {
            var warehouses = db.Warehouse.ToList();
            List<SelectListItem> list = new List<SelectListItem>();
            list.Add(new SelectListItem { Text = "Select Warehouse", Value = "0" });
            foreach (var m in warehouses)
            {
                list.Add(new SelectListItem { Text = m.WarehouseName, Value = Convert.ToString(m.WarehouseID) });

            }
            return list;
        }
        public dynamic GetRackName()
        {
            var racks = db.Rack.ToList();
            List<SelectListItem> list = new List<SelectListItem>();
            list.Add(new SelectListItem { Text = "Select Rack", Value = "0" });
            foreach (var m in racks)
            {
                list.Add(new SelectListItem { Text = m.RackName, Value = Convert.ToString(m.RackID) });

            }
            return list;
        }

        public ActionResult SaveWareHouseInDatabase(Warehouse model)
        {
            int i = 0;
            var nameExists = db.Warehouse.Where(p => p.WarehouseName == model.WarehouseName).FirstOrDefault();
            if (nameExists == null)
            {
                db.Warehouse.Add(model);
                i = db.SaveChanges();
            }
            else
            {
                i = 2;
            }

            return Json(i, JsonRequestBehavior.AllowGet);
        }

        public ActionResult SaveRackInDatabase(Rack model)
        {
            int i = 0;
            var Msg = "";

            var locationExists = db.Rack.Where(p => p.WarehouseID == model.WarehouseID && p.RackName == model.RackName).FirstOrDefault();
            if (locationExists == null)
            {
                try
                {
                    db.Rack.Add(model);
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

        public ActionResult GetRacksByWarehouse(int WarehouseID)
        {
            var recks = db.Rack.Where(p => p.WarehouseID == WarehouseID).ToList();

            List<SelectListItem> CellList = new List<SelectListItem>();
            CellList.Add(new SelectListItem { Text = "Select Rack", Value = "0" });
            foreach (var m in recks) 
            {
                CellList.Add(new SelectListItem { Text = m.RackName, Value = Convert.ToString(m.RackID) });

            }


            return Json(new SelectList(CellList, "Value", "Text", JsonRequestBehavior.AllowGet));
        }
        public ActionResult GetCellByRack(int CellID)
        {
            var racks = db.Cell.Where(p => p.RackID == CellID).ToList();

            List<SelectListItem> RackList = new List<SelectListItem>();
            RackList.Add(new SelectListItem { Text = "Select Cell", Value = "0" });
            foreach (var m in racks)
            {
                RackList.Add(new SelectListItem { Text = m.CellName, Value = Convert.ToString(m.CellID) });

            }


            return Json(new SelectList(RackList, "Value", "Text", JsonRequestBehavior.AllowGet));
        }

        public ActionResult SaveCallInDatabase(Cell model)
        {
            int i = 0;
            var Msg = "";
            var CallExists = db.Cell.Where(p => p.CellName == model.CellName && p.RackID == model.RackID && p.WarehouseID == model.WarehouseID).FirstOrDefault();
            if (CallExists == null)
            { 
                try
                {
                    db.Cell.Add(model);
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
            return View(db.Warehouse.ToList());
            }
            else
            {
                try {

                    var Wirehousedelete = db.Warehouse.Find(id);
                    db.Warehouse.Remove(Wirehousedelete);
                    db.SaveChanges();
                    return View(db.Warehouse.ToList());
                }catch(Exception)
                {
                    ViewBag.Msg = "আপনি সরাসরি ওয়্যারহাউজের নাম মুছে দিতে পারেন না । এক্ষেত্রে আগে আপনাকে একই ওয়্যারহাউজের রেকের নাম ও তার নিকট থাকা Cell নাম মুছতে হবে । ধন্যবাদ !";
                }
            }
            return View(db.Warehouse.ToList());
        }        
        public ActionResult WirehouseEdit(int id)
        { 
            var wirehouse = db.Warehouse.Find(id);
            if (wirehouse == null)
            {
                ViewBag.Msg = 1;
                return View();

            }
            return View(wirehouse);
        }
        [HttpPost]
        public ActionResult WirehouseUpdate(Warehouse data)
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
            var location = db.Rack.ToList();

            if (id == null)
            {
                return View(location);
            }
            else
            {
                try
                {

                    var Wirehousedelete = db.Rack.Find(id);
                    db.Rack.Remove(Wirehousedelete);
                    db.SaveChanges();
                    return View(location);
                }
                catch (Exception)
                {
                    ViewBag.Msg = "আপনি সরাসরি রেকের নাম মুছে দিতে পারেন না । এক্ষেত্রে আগে আপনাকে একই রেকের নিকট থাকা Cell এর নাম মুছতে হবে । ধন্যবাদ !";
                }
            }
            return View(location);

        }
        public ActionResult RackEdit(int id)
        {
            var rack = db.Rack.Find(id);
            if (rack == null)
            {
                ViewBag.Msg = 1;
                return View();

            }
            ViewBag.Warehouse = GetWarehouse();
            return View(rack);
        }
        [HttpPost]
        public ActionResult RackUpdate(Rack data)
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
            var locationSub = db.Cell.ToList();

            if (id == null)
            {
                return View(locationSub);
            }
            else
            {
                try
                {
                    var Wirehousedelete = db.Cell.Find(id);
                    db.Cell.Remove(Wirehousedelete);
                    db.SaveChanges();
                    return View(locationSub);
                }
                catch (Exception)
                {
                    ViewBag.Msg = "ভিতরগত কোন সমস্যা হয়েছে, পুনরায় চেষ্টা করুন । ধন্যবাদ !";
                }
            }
            return View(locationSub);

        }
        public ActionResult CellEdit(int id)
        {
            var cell = db.Cell.Find(id);
            if (cell == null)
            {
                ViewBag.Msg = 1;
                return View();

            }
            ViewBag.Warehouse = GetWarehouse();
            ViewBag.RackName = GetRackName();
            return View(cell);
        }
        [HttpPost]
        public ActionResult CellUpdate(Cell data)
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




    }
}