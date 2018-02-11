using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace GalaxyVibesPos.Models
{
    [Table("Income")]
    public class Income
    {
        [Key]
        public int ID { get; set; }
        public Nullable<int> CompanyID { get; set; }
        public string Date { get; set; }
        public string Description { get; set; }
        public string Remarks { get; set; }
        public double Amount { get; set; }
    }
}