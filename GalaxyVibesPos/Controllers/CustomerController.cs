using GalaxyVibesPos.Models;
using GalaxyVibesPos.Models.ViewBag;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace GalaxyVibesPos.Controllers
{
    public class CustomerController : Controller
    {
        private DatabaseContext db = new DatabaseContext();
        //
        // GET: /Customer/
        public ActionResult AddCustomer()
        {
            
            ViewBag.group = db.Group.ToList();
            var customers = db.Customer.ToList();
            return View(customers);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddCustomer(Customer customer)
        {
            CustomerLedger aCustomerLedger = new CustomerLedger();
            List<Customer> customers = new List<Customer>();
            if (customer.CustomerID <= 0 && customer.CustomerName != null && customer.Phone != null)
            {


                var PhoneExist = db.Customer.Where(c => c.Phone == customer.Phone).FirstOrDefault();

                if (PhoneExist == null)
                {
                    var companyName = db.Company.Where(c => c.CompanyID == customer.CompanyID).Select(g => new {Name = g.CompanyName}).First();
                    var groupName = db.Group.Where(c => c.GroupID == customer.GroupID).Select(g => new { Name = g.GroupName }).First();
                    int? MaxId = db.Customer.Max(x => (int?)x.CustomerID);

                    if (MaxId == null)
                    {
                        MaxId = 0;

                    }
                    var id = MaxId + 1;

                    if (customer.PreviousDue == null || customer.PreviousDue == 0)
                    {
                        customer.PreviousDue = 0;
                        aCustomerLedger.IsPreviousDue = 0;

                    }
                    else
                    {
                        aCustomerLedger.IsPreviousDue = 1;
                    }
                    var nowDate = DateTime.Now.ToString("M/d/yyyy");
                    var date = Convert.ToDateTime(nowDate);
                    aCustomerLedger.ReceiveDate = date;
                    aCustomerLedger.InvoiceNo = "Previous Due";
                    aCustomerLedger.Remarks = "Previous Due";
                    aCustomerLedger.Debit = customer.PreviousDue;
                    aCustomerLedger.Credit = 0;
                    aCustomerLedger.CustomerID = Convert.ToInt32(id);

                    customer.GroupName = groupName.Name;
                    customer.CompanyName = companyName.Name;

                    db.Customer.Add(customer);
                    db.CustomerLedger.Add(aCustomerLedger);
                    db.SaveChanges();
                    customers = db.Customer.ToList();
                    ViewBag.Message = 1;

                }
                else
                {
                    customers = db.Customer.ToList();
                    ViewBag.Message = 0;
                }
            }
            else
            {
                customers = db.Customer.ToList();
                ViewBag.Message = 2;
            }
            ViewBag.group = db.Group.ToList();
            return View(customers);
        }
        [HttpPost]
        public ActionResult EditCustomer(string CustomerId)
        {
            int id = Convert.ToInt32(CustomerId);
            var select = db.Customer.Where(a => a.CustomerID == id).First();            
            Customer customer = new Customer()
            {
                CustomerID = select.CustomerID,
                CustomerName = select.CustomerName,
                Email = select.Email,
                Phone = select.Phone,
                Address = select.Address,
                PreviousDue = select.PreviousDue,
                Gender = select.Gender,
                VatRegNo = select.VatRegNo,               
            };



            return Json(customer, JsonRequestBehavior.AllowGet);
        }

        public JsonResult UpdateCustomer(Customer model)
        {
            Customer customer = new Customer();
            //CustomerLedger aCustomerLedger = new CustomerLedger();

            string Msg = null;

            if (ModelState.IsValid)
            {
                var select = db.Customer.Where(x => x.CustomerID == model.CustomerID).First();
                var phone = select.Phone == model.Phone;
                if (phone != false)
                {


                    customer = db.Customer.SingleOrDefault(x => x.CustomerID == model.CustomerID);
                    //aCustomerLedger = db.CustomerLedger.SingleOrDefault(x => x.CustomerID == model.CustomerID);

                    //if (customer.PreviousDue == null || customer.PreviousDue == 0)
                    //{
                    //    customer.PreviousDue = 0;
                    //    aCustomerLedger.IsPreviousDue = 0;

                    //}
                    //else
                    //{
                    //    aCustomerLedger.IsPreviousDue = 1;
                    //}
                    //aCustomerLedger.Debit = customer.PreviousDue;
                    //aCustomerLedger.Credit = 0;

                    if(model.CompanyID !=null && model.GroupID != null)
                    {

                        var companyName = db.Company.Where(c => c.CompanyID == model.CompanyID).Select(g => new { Name = g.CompanyName }).First();
                        var groupName = db.Group.Where(c => c.GroupID == model.GroupID).Select(g => new { Name = g.GroupName }).First();
                        customer.CompanyName = companyName.Name;
                        customer.GroupName = groupName.Name;
                        customer.CompanyID = model.CompanyID;
                    }

                    customer.CustomerID = model.CustomerID;
                    customer.CustomerName = model.CustomerName;                                      
                    customer.Gender = model.Gender;
                    customer.Phone = model.Phone;
                    customer.Email = model.Email;
                    customer.Address = model.Address;
                    customer.VatRegNo = model.VatRegNo;
                    customer.PreviousDue = model.PreviousDue;
                    db.SaveChanges();
                    Msg = "Customer Information Update Successfully";


                }
                else
                {
                    var PhoneExist = db.Customer.Where(c => c.Phone == model.Phone).FirstOrDefault();

                    if (PhoneExist == null)
                    {
                        customer = db.Customer.SingleOrDefault(x => x.CustomerID == model.CustomerID);
                        //aCustomerLedger = db.CustomerLedger.SingleOrDefault(x => x.CustomerID == model.CustomerID);

                        //if (customer.PreviousDue == null || customer.PreviousDue == 0)
                        //{
                        //    customer.PreviousDue = 0;
                        //    aCustomerLedger.IsPreviousDue = 0;

                        //}
                        //else
                        //{
                        //    aCustomerLedger.IsPreviousDue = 1;
                        //}
                        //aCustomerLedger.Debit = customer.PreviousDue;
                        //aCustomerLedger.Credit = 0;

                        if (model.CompanyID != null && model.GroupID != null )
                        {

                            var companyName = db.Company.Where(c => c.CompanyID == model.CompanyID).Select(g => new { Name = g.CompanyName }).First();
                            var groupName = db.Group.Where(c => c.GroupID == model.GroupID).Select(g => new { Name = g.GroupName }).First();
                            customer.CompanyName = companyName.Name;
                            customer.GroupName = groupName.Name;
                            customer.CompanyID = model.CompanyID;
                        }

                        customer.CustomerID = model.CustomerID;
                        customer.CustomerName = model.CustomerName;
                        customer.Gender = model.Gender;                        
                        customer.Phone = model.Phone;
                        customer.Email = model.Email;
                        customer.VatRegNo = model.VatRegNo;
                        customer.Address = model.Address;
                        customer.PreviousDue = model.PreviousDue;
                        db.SaveChanges();
                        Msg = "Customer Information Update Successfully";
                    }
                    else
                    {
                        Msg = "Phone Number Already Exist";
                    }
                }

            }
            return Json(Msg, JsonRequestBehavior.AllowGet);
        }

        // Customer Ledger.........

        public ActionResult AddCustomerLedger()
        {

            ViewBag.Customer = db.Customer.ToList();
            var CustomerLedger = db.CustomerLedger.ToList();
            return View(CustomerLedger);

        }

        [HttpPost]
        public ActionResult AddCustomerLedger(CustomerLedger model)
        {

            List<CustomerLedger> customerLedger = new List<CustomerLedger>();

            if (model.CustomerID != 0)
            {
               
                if (model.ID <= 0)
                {
                    
                    model.Debit = 0;
                    db.CustomerLedger.Add(model);
                    db.SaveChanges();
                    ViewBag.Message = 1;
                    customerLedger = db.CustomerLedger.ToList();
                    
                    var aCustomer = db.Customer.SingleOrDefault(x => x.CustomerID == model.CustomerID);

                    var totalDebit = db.CustomerLedger.Where(c => c.CustomerID == model.CustomerID)
                        .GroupBy(c => c.CustomerID).Select(g => new { dabit = g.Sum(c => c.Debit) }).First();

                    var totalCredit = db.CustomerLedger.Where(c => c.CustomerID == model.CustomerID)
                        .GroupBy(c => c.CustomerID).Select(g => new { credit = g.Sum(c => c.Credit) }).First();

                    
                    aCustomer.PreviousDue = totalDebit.dabit - totalCredit.credit;
                    
                    db.SaveChanges();
                    
                }
                else
                {
                    var aCustomer = db.Customer.SingleOrDefault(x => x.CustomerID == model.CustomerID);
                    var aCustomerLedger = db.CustomerLedger.SingleOrDefault(x => x.ID == model.ID);

                    

                    aCustomerLedger.CustomerID = model.CustomerID;

                    aCustomerLedger.ReceiveDate = model.ReceiveDate;
                    
                    aCustomerLedger.Credit = model.Credit;
                   
                    aCustomerLedger.Remarks = model.Remarks;
                   

                    db.SaveChanges();
                   
                    ViewBag.Message = 1;

                    var totalDebit = db.CustomerLedger.Where(c => c.CustomerID == model.CustomerID)
                        .GroupBy(c => c.CustomerID).Select(g => new { dabit = g.Sum(c => c.Debit) }).First();

                    var totalCredit = db.CustomerLedger.Where(c => c.CustomerID == model.CustomerID)
                        .GroupBy(c => c.CustomerID).Select(g => new { credit = g.Sum(c => c.Credit) }).First();

                    aCustomer.PreviousDue = totalDebit.dabit - totalCredit.credit;
                    
                    db.SaveChanges();

                    customerLedger = db.CustomerLedger.ToList();
                }
            }
            else
            {
                ViewBag.Message = 0;
                customerLedger = db.CustomerLedger.ToList();
            }

            ViewBag.Customer = db.Customer.ToList();

            return View(customerLedger);
        }


        [HttpPost]
        public JsonResult GetPreviousDueByCustomerID(string CustomerId)
        {
            if (CustomerId != "0")
            {
                int id = Convert.ToInt32(CustomerId);
                var select = db.Customer.Where(a => a.CustomerID == id).First();
                Customer customer = new Customer()
                {
                    PreviousDue = select.PreviousDue

                };
                return Json(customer, JsonRequestBehavior.AllowGet);
            }
            return Json(null, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult EditCustomerLedger(string CustomerId)
        {
            int id = Convert.ToInt32(CustomerId);
            var select = db.CustomerLedger.Where(a => a.ID == id).First();

            var due = db.Customer.Where(a => a.CustomerID == select.CustomerID).First();
            CustomerLedger aCustomerLedger = new CustomerLedger()
            {
                ID = select.ID,
                CustomerID = select.CustomerID,
                ReceiveDate = select.ReceiveDate,
                PreviouaDue = due.PreviousDue,
                Credit = select.Credit,
                Remarks = select.Remarks
            };
            return Json(aCustomerLedger, JsonRequestBehavior.AllowGet);
        }

        // GetCompanyByGroupID

        public JsonResult GetCompanyByGroupID(int GroupID)
        {
            var companes = db.Company.Where(m => m.GroupID == GroupID).ToList();

            List<SelectListItem> companiesList = new List<SelectListItem>();

            companiesList.Add(new SelectListItem { Text = "Select Company", Value = "0" });

            foreach (var m in companes)
            {
                companiesList.Add(new SelectListItem { Text = m.CompanyName, Value = Convert.ToString(m.CompanyID) });
            }

            return Json(new SelectList(companiesList, "Value", "Text", JsonRequestBehavior.AllowGet));


        }
    }
}