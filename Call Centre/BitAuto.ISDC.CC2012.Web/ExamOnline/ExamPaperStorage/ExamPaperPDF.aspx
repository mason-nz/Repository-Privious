<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ExamPaperPDF.aspx.cs" Inherits="BitAuto.ISDC.CC2012.Web.ExamOnline.ExamPaperStorage.ExamPaperPDF" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
     <asp:Button runat="server" ID="btnPDF" Text="导出PDF" OnClick="btnPDF_Click" />
    </div>
    </form>
</body>
</html>
