using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace GalaxyVibesPos.Models
{
    [Table("CategorySub")]
    public class CategorySub
    {
        [Key]
        public int SubCategoryID { get; set; }
        [Required]
        public int MainCategoryID { get; set; }
        [Required]
        public int CategoryID { get; set; }
        [Required]
        public string SubCategoryName { get; set; }

        public virtual CategoryMain CategoryMain { get; set; }
        public virtual Category Category { get; set; }
        public virtual List<ProductDetails> ProductDetailsList { get; set; }
    }
}