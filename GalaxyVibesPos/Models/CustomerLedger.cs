using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace GalaxyVibesPos.Models
{
    public class CustomerLedger
    {
        public int ID { get; set; }
        public string ReceiveDate { get; set; }
        public int CustomerID { get; set; }
        public string InvoiceNo { get; set; }
        public Nullable<double> Debit { get; set; }
        public Nullable<double> Credit { get; set; }
        public Nullable<double> Adjustment { get; set; }
        public string PaymentType { get; set; }
        public string BankName { get; set; }
        public string ChequeNo { get; set; }
        public string ChequeDate { get; set; }
        public string Remarks { get; set; }
        public string NextPaymentDate { get; set; }
        public Nullable<int> IsPreviousDue { get; set; }
        [ForeignKey("CustomerID")]
        public virtual Customer Customer { get; set; }
        [NotMapped]
        public Nullable<double> PreviouaDue { get; set; }
    }
}