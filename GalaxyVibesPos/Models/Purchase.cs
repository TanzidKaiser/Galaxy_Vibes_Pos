using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace GalaxyVibesPos.Models
{
    [Table("Purchase")]
    public class Purchase
    {
        [Key]
        public int PurchaseID { get; set; }
        public string PurchaseNo { get; set; }
        public Nullable<int> CompanyID { get; set; }
        public string PurchaseDate { get; set; }
        public Nullable<int> SupplierID { get; set; }
        public string PurchaseSupplierInvoiceNo { get; set; }
        public string PurchaseRemarks { get; set; }
        public Nullable<int> PurchaseProductID { get; set; }
        public Nullable<double> PurchaseProductPrice { get; set; }
        public Nullable<double> PurchaseQuantity { get; set; }
        public Nullable<double> PurchaseTotal { get; set; }
        
        [NotMapped]
        public Nullable<double> TotalAmount { get; set; }
        
        [ForeignKey("CompanyID")]
        public virtual SupplierCompany Company { get; set; }
        [ForeignKey("SupplierID")]
        public virtual Supplier Supplier { get; set; }
        [ForeignKey("PurchaseProductID")]
        public virtual ProductDetails Product { get; set; }

        // for invoive 
        [NotMapped]
        public int Tquantity { get; set; }

    }
}