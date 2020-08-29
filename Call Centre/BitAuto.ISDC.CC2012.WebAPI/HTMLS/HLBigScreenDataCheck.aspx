<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="HLBigScreenDataCheck.aspx.cs" Inherits="BitAuto.ISDC.CC2012.WebAPI.HTMLS.HLBigScreenDataCheck" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="refresh" content="5" />
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <asp:GridView runat="server" ID="GV" BackColor="#DEBA84" BorderColor="#DEBA84" 
        BorderStyle="None" BorderWidth="1px" CellPadding="10" CellSpacing="5" 
        Width="90%">
        <FooterStyle BackColor="#F7DFB5" ForeColor="#8C4510" />
        <HeaderStyle BackColor="#A55129" Font-Bold="True" ForeColor="White" />
        <PagerStyle ForeColor="#8C4510" HorizontalAlign="Center" />
        <RowStyle BackColor="#FFF7E7" ForeColor="#8C4510" />
        <SelectedRowStyle BackColor="#738A9C" Font-Bold="True" ForeColor="White" />
        <SortedAscendingCellStyle BackColor="#FFF1D4" />
        <SortedAscendingHeaderStyle BackColor="#B95C30" />
        <SortedDescendingCellStyle BackColor="#F1E5CE" />
        <SortedDescendingHeaderStyle BackColor="#93451F" />
    </asp:GridView>
    </form>
</body>
</html>
