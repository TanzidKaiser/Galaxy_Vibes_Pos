using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using GalaxyVibesPos.Models;
namespace GalaxyVibesPos.Controllers
{
    public class SupplierController : Controller
    {
        DatabaseContext db = new DatabaseContext();
        //
        // GET: /Supplier/
        public ActionResult AddSupplier()
        {
            ViewBag.Suppliergroup = db.SupplierGroup.ToList();
            var customerList = db.Supplier.ToList();
            return View(customerList);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddSupplier(Supplier supplier)
        {
            SupplierLedger aSupplierLedger = new SupplierLedger();
            List<Supplier> supplierList = new List<Supplier>();
            if (supplier.SupplierID <= 0 && supplier.SupplierName != null && supplier.SupplierPhone != null)
            {


                var PhoneExist = db.Supplier.Where(c => c.SupplierPhone == supplier.SupplierPhone).FirstOrDefault();

                if (PhoneExist == null)
                {
                    //var companyName = db.Company.Where(c => c.CompanyID == customer.CompanyID).Select(g => new { Name = g.CompanyName }).First();
                    //var groupName = db.Group.Where(c => c.GroupID == customer.GroupID).Select(g => new { Name = g.GroupName }).First();
                    int? MaxId = db.Supplier.Max(x => (int?)x.SupplierID);

                    if (MaxId == null)
                    {
                        MaxId = 0;

                    }
                    var id = MaxId + 1;

                    if (supplier.SupplierPreviousDue == null || supplier.SupplierPreviousDue == 0)
                    {
                        supplier.SupplierPreviousDue = 0;
                        aSupplierLedger.IsPreviousDue = 0;

                    }
                    else
                    {
                        aSupplierLedger.IsPreviousDue = 1;
                    }
                    string date = DateTime.Now.ToString("M/d/yyyy");
                    aSupplierLedger.ReceiveDate = date;
                    aSupplierLedger.InvoiceNo = "Previous Due";
                    aSupplierLedger.Remarks = "Previous Due";
                    aSupplierLedger.Debit = 0;
                    aSupplierLedger.Credit = supplier.SupplierPreviousDue;
                    aSupplierLedger.SupplierID = Convert.ToInt32(id);
                    db.Supplier.Add(supplier);
                    db.SupplierLedger.Add(aSupplierLedger);
                    db.SaveChanges();
                    supplierList = db.Supplier.ToList();
                    ViewBag.Message = 1;

                }
                else
                {
                    supplierList = db.Supplier.ToList();
                    ViewBag.Message = 0;
                }
            }
            else
            {
                supplierList = db.Supplier.ToList();
                ViewBag.Message = 2;
            }
            ViewBag.Suppliergroup = db.SupplierGroup.ToList();
            return View(supplierList);
        }

        [HttpPost]
        public ActionResult EditSupplier(int supplierId)
        {

            var select = db.Supplier.Where(a => a.SupplierID == supplierId).First();
            Supplier aSupplier = new Supplier()
            {
                SupplierID = select.SupplierID,
                SupplierName = select.SupplierName,
                SupplierContactPerson = select.SupplierContactPerson,
                SupplierEmail = select.SupplierEmail,
                SupplierPhone = select.SupplierPhone,
                SupplierVatRegNo = select.SupplierVatRegNo,
                SupplierPreviousDue = select.SupplierPreviousDue,
                SupplierAddress = select.SupplierAddress,
            };

            return Json(aSupplier, JsonRequestBehavior.AllowGet);
        }

        public JsonResult UpdateSupplier(Supplier model)
        {
            Supplier supplier = new Supplier();

            string Msg = null;

            if (ModelState.IsValid)
            {
                var select = db.Supplier.Where(x => x.SupplierID == model.SupplierID).First();
                var phone = select.SupplierPhone == model.SupplierPhone;
                if (phone != false)
                {

                    supplier = db.Supplier.SingleOrDefault(x => x.SupplierID == model.SupplierID);


                    if (model.CompanyID != null && model.GroupID != null)
                    {

                        supplier.GroupID = model.GroupID;
                        supplier.CompanyID = model.CompanyID;

                    }

                    supplier.SupplierName = model.SupplierName;
                    supplier.SupplierContactPerson = model.SupplierContactPerson;
                    supplier.SupplierEmail = model.SupplierEmail;
                    supplier.SupplierAddress = model.SupplierAddress;
                    supplier.SupplierPhone = model.SupplierPhone;
                    supplier.SupplierVatRegNo = model.SupplierVatRegNo;
                    db.SaveChanges();
                    Msg = "Information Update Successfully";


                }
                else
                {

                    var PhoneExist = db.Supplier.Where(c => c.SupplierPhone == model.SupplierPhone).FirstOrDefault();

                    if (PhoneExist == null)
                    {
                        supplier = db.Supplier.SingleOrDefault(x => x.SupplierID == model.SupplierID);


                        if (model.CompanyID != null && model.GroupID != null)
                        {

                            supplier.GroupID = model.GroupID;
                            supplier.CompanyID = model.CompanyID;

                        }
                        supplier.SupplierName = model.SupplierName;
                        supplier.SupplierContactPerson = model.SupplierContactPerson;
                        supplier.SupplierEmail = model.SupplierEmail;
                        supplier.SupplierAddress = model.SupplierAddress;
                        supplier.SupplierPhone = model.SupplierPhone;
                        supplier.SupplierVatRegNo = model.SupplierVatRegNo;
                        db.SaveChanges();
                        Msg = "Information Update Successfully";
                    }
                    else
                    {
                        Msg = "Phone Number Already Exist";
                    }
                }

            }
            return Json(Msg, JsonRequestBehavior.AllowGet);
        }

        //Get Company Name in Dropdown By Select Group Name 

        public JsonResult GetCompanyByGroupID(int GroupID)
        {
            var companes = db.SupplierCompany.Where(m => m.GroupID == GroupID).ToList();

            List<SelectListItem> companiesList = new List<SelectListItem>();

            companiesList.Add(new SelectListItem { Text = "Select Company", Value = "0" });

            foreach (var m in companes)
            {
                companiesList.Add(new SelectListItem { Text = m.CompanyName, Value = Convert.ToString(m.CompanyID) });
            }

            return Json(new SelectList(companiesList, "Value", "Text", JsonRequestBehavior.AllowGet));


        }

        // Supplier Ledger

        public ActionResult AddSupplierLedger()
        {

            ViewBag.supplierList = db.Supplier.ToList();
            var supplierLedgerList = db.SupplierLedger.ToList();
            return View(supplierLedgerList);

        }

        [HttpPost]
        public ActionResult AddSupplierLedger(SupplierLedger model)
        {
            List<SupplierLedger> supplierLedger = new List<SupplierLedger>();

            if (model.SupplierID != 0)
            {

                if (model.ID <= 0)
                {

                    var aSupplier = db.Supplier.SingleOrDefault(x => x.SupplierID == model.SupplierID);

                    var totalDebit = db.SupplierLedger.Where(c => c.SupplierID == model.SupplierID)
                        .GroupBy(c => c.SupplierID).Select(g => new { dabit = g.Sum(c => c.Debit) }).First();

                    var totalCredit = db.SupplierLedger.Where(c => c.SupplierID == model.SupplierID)
                        .GroupBy(c => c.SupplierID).Select(g => new { credit = g.Sum(c => c.Credit) }).First();


                    double? previousDue = totalCredit.credit - (totalDebit.dabit+model.Debit);

                    aSupplier.SupplierPreviousDue = previousDue;

                    if (previousDue == 0)
                    {
                        model.IsPreviousDue = 0;
                    }
                    else
                    {
                        model.IsPreviousDue = 1;
                    }

                    model.Credit = 0;
                    db.SupplierLedger.Add(model);
                    db.SaveChanges();
                    ViewBag.Message = 1;
                    supplierLedger = db.SupplierLedger.ToList();

                }
                else
                {
                    var aSupplier = db.Supplier.SingleOrDefault(x => x.SupplierID == model.SupplierID);
                    var aSupplierLedger = db.SupplierLedger.SingleOrDefault(x => x.ID == model.ID);
                    aSupplierLedger.Debit = model.Debit;
                    db.SaveChanges();

                    var totalDebit = db.SupplierLedger.Where(c => c.SupplierID == model.SupplierID)
                                            .GroupBy(c => c.SupplierID).Select(g => new { dabit = g.Sum(c => c.Debit) }).First();

                    var totalCredit = db.SupplierLedger.Where(c => c.SupplierID == model.SupplierID)
                        .GroupBy(c => c.SupplierID).Select(g => new { credit = g.Sum(c => c.Credit) }).First();

                    double? previousDue = totalCredit.credit - totalDebit.dabit;
                    
                    aSupplier.SupplierPreviousDue = previousDue;
                    
                    if(previousDue == 0 )
                    {
                        aSupplierLedger.IsPreviousDue = 0;
                    }
                    else
                    {
                        aSupplierLedger.IsPreviousDue = 1;
                    }


                    aSupplierLedger.SupplierID = model.SupplierID;

                    aSupplierLedger.ReceiveDate = model.ReceiveDate;

                    aSupplierLedger.Debit = model.Debit;

                    aSupplierLedger.Remarks = model.Remarks;


                    db.SaveChanges();

                    ViewBag.Message = 1;
                    
                    supplierLedger = db.SupplierLedger.ToList();
                }
            }
            else
            {
                ViewBag.Message = 0;
                supplierLedger = db.SupplierLedger.ToList();
            }

            ViewBag.supplierList = db.Supplier.ToList();

            return View(supplierLedger);
        }

        [HttpPost]
        public JsonResult GetPreviousDueBySupplierID(int Id)
        {
            if (Id != 0)
            {
                //int id = Convert.ToInt32(CustomerId);
                var select = db.Supplier.Where(a => a.SupplierID == Id).First();
                Supplier supplie = new Supplier()
                {
                    SupplierPreviousDue = select.SupplierPreviousDue
                                                      
                };
                return Json(supplie, JsonRequestBehavior.AllowGet);
            }
            return Json(null, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult EditSupplierLedger(int Id)
        {
            
            var select = db.SupplierLedger.Where(a => a.ID == Id).First();

            var due = db.Supplier.Where(a => a.SupplierID == select.SupplierID).First();
            SupplierLedger aSupplierLedger = new SupplierLedger()
            {
                ID = select.ID,
                SupplierID = select.SupplierID,
                ReceiveDate = select.ReceiveDate,
                PreviouaDue = due.SupplierPreviousDue,
                Debit = select.Debit,
                Remarks = select.Remarks
            };
            return Json(aSupplierLedger, JsonRequestBehavior.AllowGet);
        }
       
    }
}