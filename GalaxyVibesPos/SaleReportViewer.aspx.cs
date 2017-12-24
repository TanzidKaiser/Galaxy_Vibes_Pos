using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace GalaxyVibesPos
{
    public partial class SaleReportViewer : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            Calendar.Visible = false;
        }

        protected void SearchBtn_Click(object sender, EventArgs e)
        {

        }

        protected void FromDateImg_Click(object sender, ImageClickEventArgs e)
        {
            //if (Calendar.Visible)
            //{
            //    Calendar.Visible = false;
            //}
            //else
            //{
            //    Calendar.Visible = true;
            //}
            Calendar.Visible = true;
        }

        protected void ToDateImg_Click(object sender, ImageClickEventArgs e)
        {
            if (Calendar.Visible)
            {
                Calendar.Visible = false;
            }
            else
            {
                Calendar.Visible = true;
            }
        }

        protected void Calendar_SelectionChanged(object sender, EventArgs e)
        {
            FrmDateTxt.Text = Calendar.SelectedDate.ToShortDateString();
            Calendar.Visible = false;
        }
    }
}