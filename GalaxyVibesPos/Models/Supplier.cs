using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace GalaxyVibesPos.Models
{
    [Table("Supplier")]
    public class Supplier
    {
        [Key]
        public int SupplierID { get; set; }        
        public string SupplierName { get; set; }
        public string SupplierContactPerson { get; set; }
        public string SupplierPhone { get; set; }
        public string SupplierVatRegNo { get; set; }
        public string SupplierEmail { get; set; }
        public string SupplierAddress { get; set; }
        public Nullable<int> GroupID { get; set; }
        public Nullable<int> CompanyID { get; set; }
        public Nullable<double> SupplierPreviousDue { get; set; } 
        [ForeignKey("GroupID")]
        public virtual SupplierGroup supplierGroup { get; set; }
        [ForeignKey("CompanyID")]
        public virtual SupplierCompany supplierCompany { get; set; }
        public virtual List<SupplierLedger> SupplierLedgerList { get; set; }
        public virtual List<Purchase> PurchaseList { get; set; }
        
    }
}