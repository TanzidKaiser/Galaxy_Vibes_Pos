using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GalaxyVibesPos.Models
{
    public class DamageProductReceive
    {
        public int DamageProductID { get; set; }
        public string DamageProductNo { get; set; }
        public Nullable<int> CompanyID { get; set; }
        public string DamageProductDate { get; set; }
        public Nullable<int> SupplierID { get; set; }
        public string InvoiceNo { get; set; }
        public string DamageProductRemarks { get; set; }
        public Nullable<int> DamageProductProductID { get; set; }
        public Nullable<double> DamageProductPrice { get; set; }
        public Nullable<double> DamageProductQuantity { get; set; }
        public Nullable<double> DamageProductTotal { get; set; }
    }
}