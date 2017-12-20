using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace GalaxyVibesPos.Models
{
    [Table("Sale")]
    public class Sale
    {
        [Key]
        public int SalesID { get; set; }
        public int CompanyID { get; set; }
        public string SalesNo { get; set; }
        public string SalesDate { get; set; }
        public string SalesTime { get; set; }
        public Nullable<int> SalesCustomerID { get; set; }
        public string SalesRemarks { get; set; }
        public string Reference { get; set; }
        public Nullable<int> SalesProductID { get; set; }
        public Nullable<double> SalesPurchasePrice { get; set; }
        public Nullable<double> SalesSalePrice { get; set; }
        public Nullable<double> SalesQuantity { get; set; }
        public Nullable<double> SalesProductDiscount { get; set; }
        public Nullable<double> SalesTotal { get; set; }
        public string SalesCustomerName { get; set; }
        public string SalesSoldBy { get; set; }
        public Nullable<double> SalesReceivedAmount { get; set; }
        public Nullable<double> SalesChangeAmount { get; set; }
        public Nullable<double> SalesVatRate { get; set; }
        public Nullable<double> SalesVatTotal { get; set; }
        public string SalesPuechaseBy { get; set; }
        public string SalesPurchaseByContact { get; set; }
        public Nullable<int> PaymentType { get; set; }

        [ForeignKey("CompanyID")]
        public virtual Company Company { get; set; }
        [ForeignKey("SalesCustomerID")]
        public virtual Customer Customer { get; set; }
        [ForeignKey("SalesProductID")]
        public virtual ProductDetails ProductDetails { get; set; }

        [NotMapped]
        public string CompanyName { get; set; }
        [NotMapped]
        public int? DueAmount { get; set; }

        //For Sale Invoice Report

        [NotMapped]
        public string SubTotal { get; set; }
        [NotMapped]
        public string TotalDiscount { get; set; }
        [NotMapped]
        public string SalesVat { get; set; }
        [NotMapped]
        public string ProductName { get; set; }
        [NotMapped]
        public double? Total { get; set; }
        [NotMapped]
        public string TotalAmount { get; set; }
        [NotMapped]
        public string NetPayable { get; set; }

        [NotMapped]
        public string ReturnAmount { get; set; }
    }
   
}