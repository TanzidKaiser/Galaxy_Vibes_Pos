using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace GalaxyVibesPos.Models
{
    [Table("CategoryMain")]
    public class CategoryMain
    {
        [Key]
        public int MainCategoryID { get; set; }
        //[Display(Name="Main Category:")]
        [Required(ErrorMessage="Select Main Category Name")]
        public string MaincategoryName { get; set; }
        public virtual List<Category> CategoryList { get; set; }
        public virtual List<CategorySub> CategorySubList { get; set; }
        public virtual List<ProductDetails> ProductDetailsList { get; set; }
    }
}