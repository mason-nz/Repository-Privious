<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Display.aspx.cs" Inherits="BitAuto.ISDC.CC2012.Web.SurveyInfo.SurveyProject.Display" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>调查项目查看</title>
    <script src="/Js/jquery-1.4.4.min.js" language="javascript" type="text/javascript"></script>
    <script src="/Js/jquery.jmpopups-0.5.1.pack.js" type="text/javascript"></script>
    <script src="/Js/common.js" language="javascript" type="text/javascript"></script>
    <script src="/CTI/CTITool.js" type="text/javascript"></script>
    <link href="../../css/base.css" type="text/css" rel="stylesheet" />
    <link href="../../css/style.css" type="text/css" rel="stylesheet" />
</head>
<body>
    <form id="form1" runat="server">
    <div class="w980">
        <div class="taskT">查看调查项目</div>
        <div class="addexam clearfix">
            <ul>
            <li>
                <label>调查名称：</label>
                <span id="spanProjectName" runat="server"></span>
            </li>
            <li>
                 <label>所属分组：</label><span id="spanBGID" runat="server"></span>
            </li>
            <li>
                 <label>分类：</label>
                 <span id="spanBCID" runat="server"></span>
            </li>
            <li>
                <label>调查说明：</label>
                <span id="spanDescription" runat="server"></span>
            </li>
            <li>
                <label>调查范围：</label>
                <span id="spanBusinessGroup" runat="server"></span>
            </li>
            <li>
                <label>调查问卷：</label>
                <span id="spanSIID" runat="server" ></span>
            </li>
            <li><label>调查对象：</label>
                <span id="spanPersons" style="float:left;width:786px;" runat="server"></span>&nbsp;
            </li>
            <li>
                <label>调查时间：</label>
                <span id="spanBeginTime" runat="server"></span>至<span id="spanEndTime" runat="server"></span>
            </li>
            <li><label>预计完成份数：</label>
                <span id="spanEstimateNumber" runat="server"></span>&nbsp;
            </li>
            <li><label>实际提交份数：</label>
                <span id="spanTrueNumber" runat="server"></span>&nbsp;
            </li>
            <li><label>满意度分数：</label>
                <span id="spanScore" runat="server"></span>&nbsp;
            </li>
            <li><label>创建人：</label>
                <span id="spanCreateUserName" runat="server"></span>&nbsp;
            </li>
            <li><label>创建时间：</label>
                <span id="spanCreateTime" runat="server"></span>&nbsp;
            </li>
            <li><label>完成时间：</label>
                <span id="spanFinshTime" runat="server"></span>&nbsp;
            </li>
            </ul>
        </div>
    </div>
    
    </form>
</body>
</html>
