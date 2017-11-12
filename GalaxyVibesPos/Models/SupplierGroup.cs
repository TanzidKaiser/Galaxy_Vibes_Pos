using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace GalaxyVibesPos.Models
{
    [Table("SupplierGroup")]
    public class SupplierGroup
    {
        [Key]
        public int GroupID { get; set; }
        [Required]
        public string GroupName { get; set; }
        public virtual List<SupplierCompany> SupplierCompanyList { get; set; }
    }
}