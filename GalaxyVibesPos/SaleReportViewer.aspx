<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="SaleReportViewer.aspx.cs" Inherits="GalaxyVibesPos.SaleReportViewer" %>

<%@ Register Assembly="Microsoft.ReportViewer.WebForms, Version=12.0.0.0, Culture=neutral, PublicKeyToken=89845dcd8080cc91" Namespace="Microsoft.Reporting.WebForms" TagPrefix="rsweb" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
        <div>
            From Date :
        <asp:TextBox ID="FrmDateTxt" runat="server"></asp:TextBox>
            &nbsp;<asp:ImageButton ID="FromDateImg" runat="server" Height="22px" ImageUrl="~/Content/images/calender.PNG" OnClick="FromDateImg_Click" Width="20px" />
            &nbsp; To Date :
        <asp:TextBox ID="ToDateTxt" runat="server"></asp:TextBox>
            &nbsp;<asp:ImageButton ID="ToDateImg" runat="server" Height="22px" ImageUrl="~/Content/images/calender.PNG" OnClick="ToDateImg_Click" Width="20px" />
            &nbsp;
        <asp:Button ID="SearchBtn" runat="server" Text="Search" OnClick="SearchBtn_Click"  />
            &nbsp;<br />
&nbsp;&nbsp;&nbsp;&nbsp; <asp:Calendar ID="Calendar" runat="server" BackColor="White" BorderColor="#999999" DayNameFormat="Shortest" Font-Names="Verdana" Font-Size="8pt" ForeColor="Black" Height="180px" Width="200px" CellPadding="4" OnSelectionChanged="Calendar_SelectionChanged">
            <DayHeaderStyle BackColor="#CCCCCC" Font-Bold="True" Font-Size="7pt" />
            <NextPrevStyle VerticalAlign="Bottom" />
            <OtherMonthDayStyle ForeColor="#808080" />
            <SelectedDayStyle BackColor="#666666" Font-Bold="True" ForeColor="White" />
            <SelectorStyle BackColor="#CCCCCC" />
            <TitleStyle BackColor="#999999" Font-Bold="True" BorderColor="Black" />
            <TodayDayStyle BackColor="#CCCCCC" ForeColor="Black" />
                <WeekendDayStyle BackColor="#FFFFCC" />
            </asp:Calendar>

            &nbsp;&nbsp;<br />

        </div>
        <div>
            <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>
            <rsweb:ReportViewer ID="ReportViewer1" runat="server"></rsweb:ReportViewer>
        </div>
    </form>
</body>
</html>
