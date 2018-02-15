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
            if (i > 0)
            {
                Msg = "Save Successfully";
            }
            ViewBag.incomeList = db.Income.ToList();
            return Json(Msg, JsonRequestBehavior.AllowGet);

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
                i = db.SaveChanges();
                if (i > 0)
                {
                    msg = "Save Successfully";
                }
            }
            catch (Exception)
            {
                msg = "Inner exception";
            }


            return Json(msg, JsonRequestBehavior.AllowGet);
        }

        //Expense Create

        public ActionResult ExpenseCreate()
        {

            var lsit = GetExpenseTypelist();

            ViewBag.ExpenseType = lsit;
            ViewBag.ExpenseList = db.Expense.ToList();

            return View();

        }

        [HttpPost]
        public ActionResult ExpenseCreate(Expense model)
        {
            var msg = "";

            model.CompanyID = 0;
            db.Expense.Add(model);
            int i = db.SaveChanges();

            var list = GetExpenseTypelist();
            ViewBag.ExpenseList = db.Expense.ToList();

            if (i >= 0)
            {
                msg = "Save Successfully";
            }
            return Json(msg, JsonRequestBehavior.AllowGet);
        }

        private dynamic GetExpenseTypelist()
        {
            var expenseTypes = new[] {

                new { Name = "Ganeral Expense", Id = "General Expense" },
            new { Name = "Special Discount", Id = "Special Discount" }
            }.ToList();

            List<SelectListItem> list = new List<SelectListItem>();

            list.Add(new SelectListItem { Text = "Select Expense Type", Value = "0" });

            foreach (var m in expenseTypes)
            {
                list.Add(new SelectListItem { Text = m.Name, Value = m.Id });
            }
            return list;
        }

        //Expense Edit And Update 

        public ActionResult EditExpense(int Id)
        {
            var income = db.Expense.Where(i => i.ID == Id).First();
            var item = new Expense()
            {
                ID = income.ID,
                Date = income.Date,
                Description = income.Description,
                ExpenseType = income.ExpenseType,
                Remarks = income.Remarks,
                Amount = income.Amount
            };

            return Json(item, JsonRequestBehavior.AllowGet);
        }
        public ActionResult ExpenseUpdate(Expense model)
        {
            string msg = "Save";
            int i = 0;
            try
            {
                var item = db.Expense.SingleOrDefault(p => p.ID == model.ID);

                item.Date = model.Date;
                item.Description = model.Description;
                item.Remarks = model.Remarks;
                item.Amount = model.Amount;
                item.ExpenseType = model.ExpenseType;
                i = db.SaveChanges();
                if (i > 0)
                {
                    msg = "Save Successfully";
                }
            }
            catch (Exception)
            {
                msg = "Inner exception";
            }


            return Json(msg, JsonRequestBehavior.AllowGet);
        }

        // Customer payment Or Receive

        public ActionResult CustomerPaymentOrReceive()
        {
            ViewBag.Customers = GetCustomers();
            ViewBag.PaymentTypes = GetPaymentType();

            return View();
        }
        [HttpPost]
        public ActionResult CustomerPaymentOrReceive(CustomerLedger model)
        {

            return View();
        }

        public ActionResult CustomerPaymentOrReceiveIndex()
        {
            var customerLedgerList = db.CustomerLedger.ToList();
            return View(customerLedgerList);
        }

        public dynamic GetCustomers()
        {
            var customers = from cust in db.Customer
                            select new { Name = cust.CustomerName, Id = cust.CustomerID };

            List<SelectListItem> list = new List<SelectListItem>();
            list.Add(new SelectListItem { Text = "Select Customer", Value="0" });
            foreach (var a in customers)
            {
                list.Add(new SelectListItem { Text = a.Name, Value = Convert.ToString(a.Id) });
            }

            return list;
        }
        public dynamic GetPaymentType()
        {
            List<SelectListItem> list = new List<SelectListItem>();

            var paymentTypes = new[] {

                new { Name="Cash" ,Id ="Cash"},
                new {Name="Check" ,Id ="Check" }
            }.ToList();

            foreach (var a in paymentTypes)
            {
                list.Add(new SelectListItem { Text = a.Name, Value = a.Id });
            }

            return list;
        }

    }
}