using GalaxyVibesPos.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace GalaxyVibesPos.Controllers
{
    public class AccountsController : Controller
    {
        DatabaseContext db = new DatabaseContext();

        // GET: Accounts
        public ActionResult OtherIncome()
        {
            //var incomeList = db.Income.ToList();
            ViewBag.incomeList = db.Income.ToList();
            return View();
        }
        [HttpPost]
        public ActionResult OtherIncome(Income data)
        {
            var Msg = "";
            db.Income.Add(data);
            int i = db.SaveChanges();
            if(i>0)
            {
                Msg = "Save Successfully";
            }
            ViewBag.incomeList = db.Income.ToList();
            return Json(Msg,JsonRequestBehavior.AllowGet);
            
        }
        public JsonResult EditIncome(int Id)
        {
            var income = db.Income.Where(i => i.ID == Id).First();
            var item = new Income()
            {
                ID = income.ID,
                Date = income.Date,
                Description = income.Description,
                Remarks = income.Remarks,
                Amount = income.Amount
            };

            return Json(item, JsonRequestBehavior.AllowGet);
        }
        public JsonResult IncomeUpdate(Income data)
        {
            string msg = null;
            int i = 0;
            try
            {
                var item = db.Income.SingleOrDefault(p => p.ID == data.ID);

                item.Date = data.Date;
                item.Description = data.Description;
                item.Remarks = data.Remarks;
                item.Amount = data.Amount;                
                i =  db.SaveChanges();
                if(i>0)
                {
                    msg = "Save Successfully";
                }
            }
            catch(Exception)
            {
                msg = "Inner exception";
            }
            

            return Json(msg,JsonRequestBehavior.AllowGet);
        }
    }
}