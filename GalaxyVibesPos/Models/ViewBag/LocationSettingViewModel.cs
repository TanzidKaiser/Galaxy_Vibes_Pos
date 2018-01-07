using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GalaxyVibesPos.Models.ViewBag
{
    public class LocationSettingViewModel
    {
        //Location Main 
        public int LocationMainID { get; set; }
        public string LocationMainName { get; set; }

        //Location

        public int LocationID { get; set; }
        public string LocationName { get; set; }

        // Location Sub

        public int LocationSubID { get; set; }
        public String LocationSubName { get; set; }
   

    }
}