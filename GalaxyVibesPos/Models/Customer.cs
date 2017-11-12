using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace GalaxyVibesPos.Models
{
    [Table("Customer")]
    public class Customer
    {
        [Key]
        public int CustomerID { get; set; }
        
        public Nullable<int> CompanyID { get; set; }
        public string GroupName { get; set; }
        public string CompanyName { get; set; }
        public string CustomerName { get; set; }
        public string Gender { get; set; }
        public string Phone { get; set; }
        public string VatRegNo { get; set; }
        public string Email { get; set; }
        public string Address { get; set; }
        public Nullable<double> PreviousDue { get; set; }
        [ForeignKey("CompanyID")]
        public virtual Company Company { get; set; }
        public virtual List<Sale> SaleList { get; set; }
        public virtual List<CustomerLedger> CustomerLedgerList { get; set; }
        [NotMapped]
        public int GroupID { get; set; }
    }
}