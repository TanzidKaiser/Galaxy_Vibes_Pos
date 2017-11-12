using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GalaxyVibesPos.Models
{
    public class ItemSattingViewModel
    {
        // For main Category

        public int MainCategoryID { get; set; }
        public string MaincategoryName { get; set; }        
        
        //For category

        public int CategoryID { get; set; }                 
        public string CategoryName { get; set; }

        //For Sub Category

        public int SubCategoryID { get; set; }
        public string SubCategoryName { get; set; }

        ////For Add Product
        //public int ProductID { get; set; }
        //public string ProductName { get; set; }

        //For Unit Product
        public int UnitID { get; set; }
        public string UnitSize { get; set; }

        //For Product Details
        public int ProductDetailsID { get; set; }
        public string ProductName { get; set; }
        public string Code { get; set; }
        public Nullable<double> PurchasePrice { get; set; }
        public Nullable<double> SalePrice { get; set; }
        public Nullable<double> Stoke { get; set; }
        public string Description { set; get; }        
        public Nullable<double> MinimumProductQuantity { get; set; }
        public Nullable<int> WarehouseID { get; set; }
        public Nullable<int> RackID { get; set; }
        public Nullable<int> CellID { get; set; }
        public Nullable<int> Status { get; set; }

    }
}