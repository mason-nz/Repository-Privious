<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ConsultActivityView.aspx.cs"
    Inherits="BitAuto.ISDC.CC2012.Web.ConsultManager.ConsultActivityView" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <ul class="clearfix">
        <li>
            <label>
                咨询类型：</label>
            <span id="ConsultID" runat="server" /></li>
        <li>
            <label>
                记录类型：</label>
            <span id="RecordType" runat="server" /></li>
        <li>
            <label>
                问题类别：</label>
            <span id="QuestionType" runat="server" /></li>
        <li>
            <label>
                品牌：</label>
            <span id="BrandName" runat="server" /></li>
        <li>
            <label>
                关注活动：</label>
            <span id="ShowActivity" runat="server" /></li>
      
    </ul>
    <div class="line">
    </div>
    <ul class="clearfix">
        <li>
            <label>
                来电记录：</label> <span id="CallRecord" runat="server"  class="exceed"/></li>
            <li>
            <label>
                录音记录：</label><%=ShowCallRecord()%></li>
    </ul>
    </form>
</body>
</html>
