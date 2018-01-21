using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace GalaxyVibesPos.Models
{
    [Table("Rack")]
    public class Rack
    {
        [Key]
        public int RackID { get; set; }
        public string RackName { get; set; }
        public int WarehouseID { get; set; }
        [ForeignKey("WarehouseID")]
        public virtual Warehouse Warehouse { get; set; }
        public virtual List<Cell> Cell { get; set; }
        public virtual List<ProductDetails> ProductDetails { get; set; }
    }
}