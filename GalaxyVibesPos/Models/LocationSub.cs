using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace GalaxyVibesPos.Models
{
    [Table("LocationSub")]
    public class LocationSub
    {
        [Key]
        public int LocationSubID { get; set; }
        public String LocationSubName { get; set; }
        public int LocationMainID { get; set; }
        public int LocationID { get; set; }
        [ForeignKey("LocationMainID")]
        public virtual LocationMain LocationMain { get; set; }
        [ForeignKey("LocationID")]
        public virtual Location Location { get; set; }
    }
}