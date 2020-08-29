<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ConsultNewCarView.aspx.cs"
    Inherits="BitAuto.ISDC.CC2012.Web.ConsultManager.ConsultNewCarView" %>

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
                关注车型：</label>
            <span id="CarBrand" runat="server" /></li>
        <li>
            <label>
                推荐经销商：</label>
            <span id="DealerName" runat="server" /></li>
        <li>
            <label>
                购车预算：</label>
            <span id="BuyCarBudget" runat="server" /></li>
        <li>
            <label>
                推荐活动：</label>
            <span id="Activity" runat="server" /></li>
        <li>
            <label>
                购车时间：</label>
            <span id="BuyCarTime" runat="server" /></li>
        <li>
            <label>
                新购/置换：</label>
            <span id="BuyOrDisplace" runat="server" /></li>
       
        <li>
            <label>
                是否愿意接受电话：</label>
            <span id="AcceptTel" runat="server" /></li>
        <li>
            <label>
                操作人：</label><span> <span id="CreateUserID" runat="server" /></span></li>
        <li>
            <label>
                来电时间：</label><span> <span id="CreateTime" runat="server" /></span></li>
    </ul>

     <div class="line">
    </div>
         <ul class="clearfix">
            <li style="width: 700px;">
                <label>
                    来电记录：</label>
                <span id="CallRecord" runat="server" class="exceed" style="width: 550px;" />
            </li>
            <li>
            <label>
                录音记录：</label> <%=ShowCallRecord()%></li>
        </ul>
    </form>
</body>
</html>
