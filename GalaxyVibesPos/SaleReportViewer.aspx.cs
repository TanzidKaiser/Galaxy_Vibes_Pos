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
    public partial class SaleReportViewer : System.Web.UI.Page
    {
        DatabaseContext db = new DatabaseContext();
        protected void Page_Load(object sender, EventArgs e)
        {
            
        }

        protected void SearchBtn_Click(object sender, EventArgs e)
        {
            List<Sale> saleList = new List<Sale>();
            double? totalSale = 0;
            var FromDate = Convert.ToDateTime(FrmDateTxt.Text);
            var ToDate = Convert.ToDateTime(ToDateTxt.Text);
            var sales = (from a in db.Sale
                        where (a.SalesDate >= FromDate) && (a.SalesDate <= ToDate)
                        select a).ToList();
            
            foreach(var a in sales)
            {
                Sale aSale = new Sale();
                aSale.SalesNo = a.SalesNo;
                aSale.ProductName = a.ProductDetails.ProductName;
                aSale.SalesDate = a.SalesDate;
                aSale.SalesTime = a.SalesTime;
                aSale.SalesRemarks = a.SalesRemarks;
                aSale.SalesSalePrice = a.SalesSalePrice;
                aSale.SalesQuantity = a.SalesQuantity;
                aSale.SalesCustomerName = a.SalesCustomerName;
                aSale.SalesVatRate = a.SalesVatRate;
                aSale.SalesProductDiscount = a.SalesProductDiscount;
                aSale.SalesVatTotal = a.SalesVatTotal;
                if(a.SalesVatTotal != null)
                { 
                totalSale += a.SalesVatTotal;
                }
                saleList.Add(aSale);

            }
            SaleReportView.LocalReport.ReportPath = Server.MapPath("~/Report/Sale/SaleReportViewer.rdlc");
            SaleReportView.LocalReport.DataSources.Clear();
            ReportDataSource rds = new ReportDataSource("SalesDataSet", saleList);
            SaleReportView.LocalReport.DataSources.Add(rds);
            ReportParameter[] rptParam = new ReportParameter[]
            {
                new ReportParameter("Fdate",Convert.ToString(FrmDateTxt.Text)),
                new ReportParameter("Tdate",Convert.ToString(ToDateTxt.Text)),
                new ReportParameter("TSale",Convert.ToString(totalSale)),
            };
            SaleReportView.LocalReport.SetParameters(rptParam);
            SaleReportView.LocalReport.Refresh();

        }
    }

}
