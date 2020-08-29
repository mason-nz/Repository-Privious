<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="SurveyStatResult.aspx.cs"
    Inherits="BitAuto.ISDC.CC2012.Web.SurveyInfo.SurveyProject.SurveyStatResult" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title>调查结果统计</title>
    <link href="/css/base.css" type="text/css" rel="stylesheet" />
    <link href="/css/style.css" type="text/css" rel="stylesheet" />
    <script src="../../Js/jquery-1.4.4.min.js" type="text/javascript" ></script>
    <script src="/Js/jquery.jmpopups-0.5.1.pack.js" type="text/javascript"></script>
    <script src="/Js/common.js" language="javascript" type="text/javascript"></script>
    <script src="/CTI/CTITool.js" type="text/javascript"></script>
    <script type="text/javascript">
        $(document).ready(function () {
            //$("[class='left chartC']:gt(0)").attr("class", "left chartC  mt10");
        });
        function OptionStatShowHide(obj) {
            if ($(obj).next().css("display") == "none") {
                $(obj).find("span").text("∨");
                $(obj).next().show("slow");
            }
            else {
                $(obj).find("span").text("∧");
                $(obj).next().hide("slow");
            }
        }
    </script>
</head>
<body>
    <div class="w980">
        <div class="taskT">
            调查问卷结果统计</div>
        <div class="addzs">
            <!--题型1-->
            <asp:Repeater ID="rptSurveyQuestion" OnItemDataBound="rptSurveyQuestion_DataBound" runat="server">
            <ItemTemplate>
            <div class="st clearfix">
                <div class="left chartC mt10">
                    <div class="chartBt"><%#Container.ItemIndex+1 %>、<%#Eval("Ask") %></div>
                        <asp:Literal ID="lblStatHtml" runat="server"></asp:Literal>                    
                </div>
                <div class="right chart">
                </div>
                <asp:Literal ID="lblOtherHtml" runat="server"></asp:Literal>
            </div>
            </ItemTemplate>
            </asp:Repeater>
            <!--题型1-->
        </div>
    </div>
</body>
</html>
