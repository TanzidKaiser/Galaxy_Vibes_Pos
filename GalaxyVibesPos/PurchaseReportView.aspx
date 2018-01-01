<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="PurchaseReportView.aspx.cs" Inherits="GalaxyVibesPos.PurchaseReportView" %>

<%@ Register Assembly="Microsoft.ReportViewer.WebForms, Version=12.0.0.0, Culture=neutral, PublicKeyToken=89845dcd8080cc91" Namespace="Microsoft.Reporting.WebForms" TagPrefix="rsweb" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">  
    <title></title>
    <link href="Content/jquery-ui.css" rel="stylesheet" />
    <script src="Scripts/jquery-3.2.1.min.js"></script>    
    <script src="Scripts/jquery-ui.js"></script>
    <script>
        $(function () {
            $('#FromDateTxt').datepicker(
            {
                dateFormat: 'yy-mm-dd',
                changeMonth: true,
                changeYear: true,
                yearRange: '1950:2100'
            });
            $('#ToDateTxt').datepicker(
           {
               dateFormat: 'yy-mm-dd',
               changeMonth: true,
               changeYear: true,
               yearRange: '1950:2100'
           });
        })
    </script>
</head>
<body>
    <form id="form1" runat="server">
    <div>
    
        From Date
        <asp:TextBox ID="FromDateTxt" runat="server"></asp:TextBox>
&nbsp; To Date
        <asp:TextBox ID="ToDateTxt" runat="server" Text="All"></asp:TextBox>
&nbsp;
        <asp:Button ID="SrcBtn" runat="server" Text="Search" OnClick="SrcBtn_Click" />
    
    &nbsp;&nbsp; Invoice No :&nbsp;&nbsp;&nbsp;&nbsp;
        <asp:DropDownList ID="PurchaseDropDownList" runat="server" Width="200px" AutoPostBack="true" OnSelectedIndexChanged="PurchaseDropDownList_SelectedIndexChanged">
        </asp:DropDownList>
    
    </div>
        <div>
            <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>
            <rsweb:ReportViewer ID="PurchaseReport" runat="server" Width="1500px" Height="900px"></rsweb:ReportViewer>
        </div>
    </form>
</body>
</html>
