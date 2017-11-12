using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace GalaxyVibesPos.Models
{
    [Table("SupplierCompany")]
    public class SupplierCompany
    {
        [Key]
        public int CompanyID { get; set; }
        public Nullable<int> GroupID { get; set; }
        public string CompanyName { get; set; }
        [ForeignKey("GroupID")]
        public virtual SupplierGroup SupplierGroup { get; set; }
        //public virtual List<Sale> SaleList { get; set; }
        //public virtual List<Customer> CustomerList { get; set; }
    }
}