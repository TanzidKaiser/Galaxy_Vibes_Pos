<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="SaleReportViewer.aspx.cs" Inherits="GalaxyVibesPos.Views.Sales.SaleReportViewer" %>

<%@ Register assembly="Microsoft.ReportViewer.WebForms, Version=12.0.0.0, Culture=neutral, PublicKeyToken=89845dcd8080cc91" namespace="Microsoft.Reporting.WebForms" tagprefix="rsweb" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
        <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>
        <rsweb:ReportViewer ID="ReportViewer1" runat="server" Width="600">
        </rsweb:ReportViewer>
    <div>
    
    </div>
    </form> 
</body>
</html>
