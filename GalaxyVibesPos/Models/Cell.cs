using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace GalaxyVibesPos.Models
{
    [Table("Cell")]
    public class Cell
    {
        [Key]
        public int CellID { get; set; }
        public String CellName { get; set; }
        public int WarehouseID { get; set; }
        public int RackID { get; set; }
        [ForeignKey("WarehouseID")]
        public virtual Warehouse Warehouse { get; set; }
        [ForeignKey("RackID")]
        public virtual Rack Rack { get; set; }
        public virtual List<ProductDetails> ProductDetails { get; set; }
    }
}