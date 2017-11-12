using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace GalaxyVibesPos.Models
{
    [Table("Group")]
    public class Group
    {
        [Key]
        public int GroupID { get; set; }
        [Required]
        public string GroupName { get; set; }
        public virtual List<Company> Companylist { get; set; }
    }
}