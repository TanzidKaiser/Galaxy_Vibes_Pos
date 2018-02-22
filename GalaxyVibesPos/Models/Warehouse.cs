using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace GalaxyVibesPos.Models
{
    [Table("Warehouse")]
    public class Warehouse
    {
        [Key]
        public int WarehouseID { get; set; }
        public string WarehouseName { get; set; }
        public virtual List<Rack> Rack { get; set; }
        public virtual List<Cell> Cell { get; set; }
        public virtual List<ProductDetails> ProductDetails { get; set; }

    }
}