using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace GalaxyVibesPos.Models
{
    [Table("TarminalInformation")]
    public class TarminalInformation
    {
        [Key]
        public int CompanyID { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public string AddressOptional { get; set; }
        public string Mobile { get; set; }
        public string MobileOptional { get; set; }
        public string Phone { get; set; }
        public string PhoneOptional { get; set; }
        public string Fax { get; set; }
        public string FaxOptional { get; set; }
        public string VatNo { get; set; }
        public string TradeLicense { get; set; }
        public string TinNo { get; set; }
        public string Email { get; set; }
        public string Website { get; set; }
        public Nullable<double> VatRate { get; set; }
        public int Status { get; set; }
        public byte[] Image { get; set; }
        public Nullable<int> IsShowRoom { get; set; }
       
    }
}