<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="KnowledgeView.aspx.cs"
    Inherits="BitAuto.ISDC.CC2012.Web.KnowledgeLib.KnowledgeView" %>

<%@ Register Src="UCKnowledgeLib/UCKnowledgeView.ascx" TagName="UCKnowledgeView"
    TagPrefix="uc1" %>
<%@ Register Src="UCKnowledgeLib/UCFAQView.ascx" TagName="UCFAQView" TagPrefix="uc1" %>
<%@ Register Src="UCKnowledgeLib/UCKLOptionLogList.ascx" TagName="UCKLOptionLogList"
    TagPrefix="uc2" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head>
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title>查看知识点</title>
    <script type="text/javascript" src="../Js/jquery-1.4.4.min.js"></script>
    <link href="/css/base.css" type="text/css" rel="stylesheet" />
    <link href="/css/style.css" type="text/css" rel="stylesheet" />
    <script language="javascript" type="text/javascript" src="/Js/common.js"></script>
    <script type="text/javascript" src="/Js/jquery.jmpopups-0.5.1.pack.js"></script>
    <script src="/CTI/CTITool.js" type="text/javascript"></script>
   
</head>
<body>   
    <form id="form1" runat="server">
    <div class="w980 browse">
        <div class="taskT">
            查看知识点</div>
        <div class="addzs">
            <uc1:UCKnowledgeView ID="UCKnowledgeView1" runat="server" />
            <!--FAQ开始-->
            <uc1:UCFAQView ID="UCFAQView1" runat="server" />
            <!--FAQ结束-->
            <!--试题开始-->
            <asp:PlaceHolder ID="phQuestion" runat="server"></asp:PlaceHolder>
            <!--试题结束-->
            <uc2:UCKLOptionLogList ID="UCKLOptionLogList1" runat="server" />
        </div>
    </div>
    </form>
</body>
</html>
