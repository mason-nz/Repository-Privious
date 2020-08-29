<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="QualityResultEdit.aspx.cs"
    Inherits="BitAuto.ISDC.CC2012.Web.QualityStandard.IMQualityResultManage.QualityResultEdit" %>

<%@ Register Src="~/QualityStandard/UCQualityStandard/UCQualityStandardEdit.ascx"
    TagName="UCQualityStandardEdit" TagPrefix="uc1" %>
<%@ Register Src="~/QualityStandard/UCQualityStandard/UCConversationsView.ascx" TagName="UCConversationsView"
    TagPrefix="uc2" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>对话质检评分</title>
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <link type="text/css" href="../../Css/base.css" rel="stylesheet" />
    <link type="text/css" href="../../Css/style.css?v=201633123" rel="stylesheet" />
    <script src="/Js/jquery-1.4.4.min.js" type="text/javascript"></script>
    <script src="/Js/jquery-ui.js" type="text/javascript"></script>
    <script src="/Js/common.js" type="text/javascript"></script>
    <script src="/Js/jquery.jmpopups-0.5.1.pack.js" type="text/javascript"></script>
    <script src="/Js/json2.js" type="text/javascript"></script>
    <script src="/Js/jquery.blockUI.js" type="text/javascript"></script>
    <script src="/CTI/CTITool.js" type="text/javascript"></script>
    <script type="text/javascript" src="/Js/anchor.1.0.js"></script>
    <script type="text/javascript">
        $(function () {
            document.domain = "bitauto.com";  //这个代码会导致刷新父页面方法失效，因为会出现跨域问题
            GotoFConversation();
        });
        function GotoFConversation() {
            var paras = "{'CSID':'" + $.trim("<%=CSID%>") + "','OrderID':'','AgentID':'" + "<%=userId%>" + "','TimeStamp':'" + new Date().getTime() + "'}";
            AjaxPostAsync("/AjaxServers/CommonHandler.ashx", { Action: "EncryptString", EncryptInfo: paras, r: Math.random() }, null,
                function (data) {
                    var strhref = "<%=GetIMUrl()%>" + "?data=" + data;
                    $("#iframepage").attr("src", strhref);
                    contenerSize();
                }
             );
        };
        function contenerSize() {
            //var visibleAreaHeight = document.documentElement.clientHeight;
            var screenHeight = $(window).height();
           // console.log("visibleAreaHeight = " + visibleAreaHeight + "  ; screenHeight = " + screenHeight);
            $(".framediv").css("height", screenHeight - 10);
            $("#iframepage").css("height", screenHeight - 10);

        }
        $(window).resize(function () {
            contenerSize();           
        });
        function ShowImg(imgurl, width, height, left, top, ext) {
            if (!ext) {
                ext = "";
            }
            var boarddiv = "<div id='div_bigImg' style='position:fixed; z-index: 100; display: block; border: 4px solid #ddd;-moz-border-radius: 3px; border-radius: 3px;";
            boarddiv += "background:url(" + unescape(imgurl) + ") no-repeat 0 center; background-size: 100%100%; ";
            boarddiv += "width:" + width + "; height:" + height + "; left:" + left + ";top:" + top + "; " + ext + "'></div>";
            $(document.body).append(boarddiv);
        }
        function HiddenImg() {
            $("#div_bigImg").remove();
        }
    </script>
</head>
<body>
    <form id="form1" runat="server">
    <div class="lsjl_zj">
        <!--左历史记录start-->
        <div class="framediv" style="position: fixed; z-index: 3;width: 362px;height:880px;background: #FFF; *margin-left:-370px;border-bottom: #CCC 1px solid;">
                        <iframe id="iframepage" marginheight="0" marginwidth="0" frameborder="0" width="362"
                            style="height:400px; overflow: hidden;" name="iframepage"  onLoad="contenerSize();">
                        </iframe>
        </div>
        <!--左历史记录end-->
        <!--右内容start-->
        <div  class="w980 zj zj_dh">
            <div class="taskT" style=" height:35px;">
                <%= TableName%>
            </div>
            <uc2:UCConversationsView ID="UCConversationsViewID" runat="server" />
            <uc1:UCQualityStandardEdit ID="QualityStandardEditID" runat="server" />
            <div class="btn" style="margin: 20px auto">
                <input type="button" value="保存" onclick="SaveQualityStandarIM()" name="" />
                <input type="button" value="提交" onclick="SubQualityStandarIM()" name="" />
            </div>
        </div>
        <!--右内容end-->
    </div>
    </form>
</body>
</html>
