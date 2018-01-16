using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace GalaxyVibesPos.Models
{
    [Table("LocationMain")]
    public class LocationMain
    {
        [Key]
        public int LocationMainID { get; set; }
        public string LocationMainName { get; set; }
       
    }
}