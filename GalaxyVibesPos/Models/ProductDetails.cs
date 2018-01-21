using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace GalaxyVibesPos.Models
{
    [Table("ProductDetails")]
    public class ProductDetails
    {
        [Key]
        public int ProductDetailsID { get; set; }
        [Required]
        [DisplayName("Main Category")]
        public int MainCategoryID { get; set; }
        [Required]
        [DisplayName("Category")]
        public int CategoryID { get; set; }
        [Required]
        [DisplayName("Sub Category")]
        public int SubCategoryID { get; set; }
        [Required]
        [DisplayName("Product Name")]
        public string ProductName { get; set; }
        
        public string Code { get; set; }
        [DisplayName("Purchase price :")]
        public Nullable<double> PurchasePrice { get; set; }
        [DisplayName("Sale Price :")]
        public Nullable<double> SalePrice { get; set; }
        public Nullable<double> Stoke { get; set; }
        public string Description { set; get; }
        public Nullable<int> UnitID { get; set; }
        [DisplayName("Minimum order :")]
        public Nullable<double> MinimumProductQuantity { get; set; }
        public Nullable<int> WarehouseID { get; set; }
        public Nullable<int> RackID { get; set; }
        public Nullable<int> CellID { get; set; }
        public Nullable<int> Status { get; set; }

        [ForeignKey("MainCategoryID")]
        public virtual CategoryMain CategoryMain { get; set; }
        [ForeignKey("CategoryID")]
        public virtual Category Category { get; set; }
        [ForeignKey("SubCategoryID")]       
        public virtual CategorySub CategorySub { get; set; }

        [ForeignKey("UnitID")]
        public virtual ProductUnit ProductUnit { get; set; }

        [ForeignKey("WarehouseID")]
        public virtual Warehouse Warehouse { get; set; }
        [ForeignKey("RackID")]
        public virtual Rack Rack { get; set; }
        [ForeignKey("CellID")]
        public virtual Cell Cell { get; set; }


        public virtual List<Sale> SaleList { get; set; }
        public virtual List<Purchase> PurchaseList { get; set; }
               
    }
}