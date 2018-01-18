using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace GalaxyVibesPos.Models
{
    [Table("ProductUnit")]
    public class ProductUnit
    {
        [Key]
        public int UnitID { get; set; }
        public string UnitSize { get; set; }
        public virtual List<ProductDetails> ProductList { get; set; }
    }
}