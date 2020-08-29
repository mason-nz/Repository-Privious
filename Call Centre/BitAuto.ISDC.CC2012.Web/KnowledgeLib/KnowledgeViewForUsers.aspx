<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="KnowledgeViewForUsers.aspx.cs"
    Inherits="BitAuto.ISDC.CC2012.Web.KnowledgeLib.KnowledgeViewForUsers" %>

<%@ Register Src="UCKnowledgeLib/UCKnowledgeView.ascx" TagName="UCKnowledgeView"
    TagPrefix="uc1" %>
<%@ Register Src="UCKnowledgeLib/UCFAQView.ascx" TagName="UCFAQView" TagPrefix="uc1" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head>
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title>查看知识点</title>
    <link href="/css/base.css" type="text/css" rel="stylesheet" />
    <link href="/css/style.css" type="text/css" rel="stylesheet" />
    <script type="text/javascript" src="../Js/jquery-1.4.4.min.js"></script>
    <script src="../Js/jquery.jmpopups-0.5.1.pack.js" charset="utf-8" type="text/javascript"></script>
    <script src="../Js/common.js" type="text/javascript"></script>
    <script src="/CTI/CTITool.js" type="text/javascript"></script>

</head>
<body>
    <form id="form1" runat="server">
    <div class="w980">
        <div class="taskT">
            查看知识点</div>
        <div class="addzs">
            <uc1:UCKnowledgeView ID="UCKnowledgeView1" runat="server" />
            <!--FAQ开始-->
            <uc1:UCFAQView ID="UCFAQView1" runat="server" />
            <!--FAQ结束-->
        </div>
    </div>
    </form>
</body>
</html>
