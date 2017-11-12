using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace GalaxyVibesPos.Models
{
    [Table("Category")]
    public class Category
    {
        [Key]
        public int CategoryID { get; set; }
        [Required(ErrorMessage="Please Select Main category")]
        public int MainCategoryID { get; set; }
        public virtual CategoryMain CategoryMain { get; set; }
        [Required(ErrorMessage="Please Select Category Name")]
        public string CategoryName { get; set; }
        public virtual List<CategorySub> CategorySubList { get; set; }
        public virtual List<ProductDetails> ProductDetailsList { get; set; }
    }
}