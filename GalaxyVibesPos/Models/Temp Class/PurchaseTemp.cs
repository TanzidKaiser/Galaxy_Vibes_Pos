using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GalaxyVibesPos.Models.Temp_Class
{
    public class PurchaseTemp
    {
   
        public string Name { get; set; }
        public string Address { get; set; }
        public string Phone { get; set; }
        public string Mobile { get; set; }
        public string Email { get; set; }
        public string PurchaseNo { get; set; }
        public string SupplierInvoiceNo { get; set; }
        public string ProductCode { get; set; }
        public string SubCategoryName { get; set; }
        public string ProductName { get; set; }
        public string PurchasePrice { get; set; }
        public int ProductQuantity { get; set; }
        public double Total { get; set; }
        public int TQuantity { get; set; }
        public double SubTotal { get; set; }
        public double PayAmount { get; set; }
        public double DueAmount { get; set; }
        
    }
}