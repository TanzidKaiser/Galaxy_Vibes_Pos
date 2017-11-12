using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GalaxyVibesPos.Models.ViewBag
{
    public class GroupViewModel
    {
        public int GroupID { get; set; }
        public int CompanyID { get; set; }
        public string GroupName { get; set; }       
        public string CompanyName { get; set; }
    }
}