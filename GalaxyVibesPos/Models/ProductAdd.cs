using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace GalaxyVibesPos.Models
{
    [Table("ProductAdd")]
    public class ProductAdd
    {
        [Key]
        public int ProductID { get; set; }
        [Required]
        public string ProductName { get; set; }
        [Required]
        public int MainCategoryID { get; set; }
        [Required]
        public int CategoryID { get; set; }
        [Required]
        public int SubCategoryID { get; set; }

        public virtual CategoryMain CategoryMain { get; set; }
        public virtual Category Category { get; set; }
        public virtual CategorySub CategorySub { get; set; }
    }
}