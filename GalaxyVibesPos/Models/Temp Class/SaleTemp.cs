using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GalaxyVibesPos.Models
{
    public class SaleTemp
    {
        
        public int SalesID { get; set; }
        public int CompanyID { get; set; }
        public string SalesNo { get; set; }
        public string SalesDate { get; set; }
        public string SalesTime { get; set; }
        public int SalesCustomerID { get; set; }
        public string SalesRemarks { get; set; }
        public string Reference { get; set; }
        public int SalesProductID { get; set; }
        public double SalesPurchasePrice { get; set; }
        public double SalesSalePrice { get; set; }
        public double SalesQuantity { get; set; }
        public double SalesProductDiscount { get; set; }
        public double SalesTotal { get; set; }
        public string SalesCustomerName { get; set; }
        public string SalesSoldBy { get; set; }
        public double SalesReceivedAmount { get; set; }
        public double SalesChangeAmount { get; set; }
        public double SalesVatRate { get; set; }
        public double SalesVatTotal { get; set; }
        public string SalesPuechaseBy { get; set; }
        public string SalesPurchaseByContact { get; set; }
        public int PaymentType { get; set; }

        
        public virtual Company Company { get; set; }
        
        public virtual Customer Customer { get; set; }
       
        public virtual ProductDetails ProductDetails { get; set; }

        
        public string CompanyName { get; set; }
      
        public int DueAmount { get; set; }

        //For Sale Invoice Report

       
        public string SubTotal { get; set; }
       
        public string TotalDiscount { get; set; }
       
        public string SalesVat { get; set; }
        
        public string ProductName { get; set; }
       
        public double Total { get; set; }
       
        public string TotalAmount { get; set; }
        
        public string NetPayable { get; set; }

        
        public string ReturnAmount { get; set; }
    }
}