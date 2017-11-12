using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using GalaxyVibesPos.Models;
namespace GalaxyVibesPos.Controllers
{
    public class CustomerGroupSettingsController : Controller
    {
        DatabaseContext db = new DatabaseContext();
        //
        // GET: /Settings/
        public ActionResult GroupSettings()
        {
            var groupLIst = db.Group.ToList();
            List<SelectListItem> list = new List<SelectListItem>();
            list.Add(new SelectListItem { Text = "Select Group", Value = "0" });
            foreach (var m in groupLIst)
            {
                list.Add(new SelectListItem { Text = m.GroupName, Value = Convert.ToString(m.GroupID) });

            }
            ViewBag.GroupList = list;

            return View();
        }
        // Save Group
        public JsonResult SaveGroup(Group model)
        {


            string Msg = null;

            Group aGroup = new Group();
            if (model.GroupName != null)
            {
                var groupNames = db.Group.Where(c => c.GroupName == model.GroupName).FirstOrDefault();

                if (ModelState.IsValid)
                {
                    if (groupNames == null)
                    {
                        if (model.GroupID > 0)
                        {
                            aGroup = db.Group.SingleOrDefault(x => x.GroupID == model.GroupID);
                            aGroup.GroupName = model.GroupName;
                            db.SaveChanges();
                            Msg = "Update Successfully";
                        }
                        else
                        {
                            db.Group.Add(model);
                            db.SaveChanges();
                            Msg = "Group Name Save Successfully";

                        }
                    }
                    else
                    {
                        Msg = "Group Name already Exists";
                    }
                }
            }
            else
            {
                Msg = "Null field Not Except";
            }
            return Json(Msg, JsonRequestBehavior.AllowGet);
        }

        // Save Company

        public JsonResult SaveCompany(Company model)
        {


            string Msg = null;

            Company aCompany = new Company();
            if (model.CompanyName != null)
            {
                var companyNames = db.Company.Where(c => c.GroupID == model.GroupID && c.CompanyName == model.CompanyName).FirstOrDefault();

                if (ModelState.IsValid)
                {
                    if (companyNames == null)
                    {
                        if (model.CompanyID > 0)
                        {
                            aCompany = db.Company.SingleOrDefault(x => x.CompanyID == model.CompanyID);
                            aCompany.CompanyName = model.CompanyName;
                            db.SaveChanges();
                            Msg = "Update Successfully";
                        }
                        else
                        {
                            db.Company.Add(model);
                            db.SaveChanges();
                            Msg = "Save Successfully";

                        }
                    }
                    else
                    {
                        Msg = "company already Exists";
                    }
                }
            }
            else
            {
                Msg = "Null Field Not Except";
            }
            return Json(Msg, JsonRequestBehavior.AllowGet);
        }
    }
}