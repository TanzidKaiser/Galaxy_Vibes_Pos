using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using GalaxyVibesPos.Models;
using Microsoft.Reporting.WebForms;

namespace GalaxyVibesPos
{
    public partial class PurchaseReportView : System.Web.UI.Page
    {
        DatabaseContext db = new DatabaseContext();
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void SrcBtn_Click(object sender, EventArgs e)
        {
            var count = 0;
            DateTime ToDate;

            List<Purchase> PurchaseList = new List<Purchase>();
            double? totalSale = 0;
            var FromDate = Convert.ToDateTime(FromDateTxt.Text);
            if(ToDateTxt.Text == "All" || ToDateTxt.Text =="")
            {
                string d = DateTime.Today.ToString("yyyy-MM-dd");
                ToDate = Convert.ToDateTime(d);
            }
            else
            {
                 ToDate = Convert.ToDateTime(ToDateTxt.Text);
            }
            
            var purchases = (from a in db.Purchase
                             where (a.PurchaseDate >= FromDate) && (a.PurchaseDate <= ToDate)
                             select a).ToList();

            //SELECT DISTINCT Country FROM Customers;
            var purchasesNo = (from a in db.Purchase
                               where (a.PurchaseDate >= FromDate) && (a.PurchaseDate <= ToDate)
                               select new { no = a.PurchaseNo }).Distinct();


            //For invoice drop Down.............

            var list = new[]
           {

                new { ID = 0, no = "Select Invoice NO"},

            }.ToList();
            


            foreach (var d in purchasesNo)
            {
                count++;
                var data = new { ID = count, no = d.no };
                list.Add(data);
            }


            PurchaseDropDownList.DataSource = null;
            PurchaseDropDownList.DataSource = list;
            PurchaseDropDownList.DataTextField = "no";
            PurchaseDropDownList.DataValueField = "ID";
            PurchaseDropDownList.DataBind();


            //End Invoice Drop down...........


            foreach (var a in purchases)
            {
                Purchase aPurchase = new Purchase();
                aPurchase.PurchaseNo = a.PurchaseNo;
                aPurchase.ProductName = a.Product.ProductName;
                aPurchase.PurchaseDate = a.PurchaseDate;
                aPurchase.SupplierName = a.Supplier.SupplierName;
                aPurchase.CompanyName = a.Company.CompanyName;
                aPurchase.PurchaseSupplierInvoiceNo = a.PurchaseSupplierInvoiceNo;
                aPurchase.PurchaseRemarks = a.PurchaseRemarks;
                aPurchase.PurchaseProductPrice = a.PurchaseProductPrice;
                aPurchase.PurchaseQuantity = a.PurchaseQuantity;
                aPurchase.PurchaseTotal = a.PurchaseTotal;
                totalSale += a.PurchaseTotal;
                PurchaseList.Add(aPurchase);

            }
            PurchaseReport.LocalReport.ReportPath = Server.MapPath("~/Report/Purchase/PurchaseReportViewer.rdlc");
            PurchaseReport.LocalReport.DataSources.Clear();
            ReportDataSource rds = new ReportDataSource("PurchaseDataSet", PurchaseList);
            PurchaseReport.LocalReport.DataSources.Add(rds);
            ReportParameter[] rptParam = new ReportParameter[]
            {
                new ReportParameter("FDate",Convert.ToString(FromDateTxt.Text)),
                new ReportParameter("TDate",Convert.ToString(ToDateTxt.Text)),
                new ReportParameter("TotalSum",Convert.ToString(totalSale)),
            };
            PurchaseReport.LocalReport.SetParameters(rptParam);
            PurchaseReport.LocalReport.Refresh();

        }

        protected void PurchaseDropDownList_SelectedIndexChanged(object sender, EventArgs e)
        {
            List<Purchase> PurchaseList = new List<Purchase>();
            double? totalSale = 0;

            string invoiceNo = PurchaseDropDownList.SelectedItem.Text;
            var inventoryList = db.Purchase.Where(p => p.PurchaseNo == invoiceNo).ToList();

            foreach (var a in inventoryList)
            {
                Purchase aPurchase = new Purchase();
                aPurchase.PurchaseNo = a.PurchaseNo;
                aPurchase.ProductName = a.Product.ProductName;
                aPurchase.PurchaseDate = a.PurchaseDate;
                aPurchase.SupplierName = a.Supplier.SupplierName;
                aPurchase.CompanyName = a.Company.CompanyName;
                aPurchase.PurchaseSupplierInvoiceNo = a.PurchaseSupplierInvoiceNo;
                aPurchase.PurchaseRemarks = a.PurchaseRemarks;
                aPurchase.PurchaseProductPrice = a.PurchaseProductPrice;
                aPurchase.PurchaseQuantity = a.PurchaseQuantity;
                aPurchase.PurchaseTotal = a.PurchaseTotal;
                totalSale += a.PurchaseTotal;
                PurchaseList.Add(aPurchase);

                PurchaseReport.LocalReport.ReportPath = Server.MapPath("~/Report/Purchase/PurchaseReportViewer.rdlc");
                PurchaseReport.LocalReport.DataSources.Clear();
                ReportDataSource rds = new ReportDataSource("PurchaseDataSet", PurchaseList);
                PurchaseReport.LocalReport.DataSources.Add(rds);
                ReportParameter[] rptParam = new ReportParameter[]    
                {                   
                    new ReportParameter("TotalSum",Convert.ToString(totalSale)),
                };
                PurchaseReport.LocalReport.SetParameters(rptParam);
                PurchaseReport.LocalReport.Refresh();
            }
        }
    }
}