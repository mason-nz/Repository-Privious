<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="IMDispose.aspx.cs" Inherits="BitAuto.ISDC.CC2012.Web.QualityStandard.IMDispose" %>

<%@ Register Src="~/QualityStandard/UCQualityStandard/UCQualityStandardEditForState.ascx"
    TagName="UCScoreTableDispose" TagPrefix="uc1" %>
<%@ Register Src="~/QualityStandard/UCQualityStandard/UCQualityStandardView.ascx"
    TagName="UCScoreTableView" TagPrefix="uc2" %>
<%@ Register Src="~/QualityStandard/UCQualityStandard/UCConversationsView.ascx" TagName="UCConversationsView"
    TagPrefix="uc3" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>
        <%=Title %>
    </title>
    <link type="text/css" href="../Css/base.css" rel="stylesheet" />
    <link type="text/css" href="../Css/style.css" rel="stylesheet" />
    <script type="text/javascript" src="../Js/jquery-1.4.4.min.js"></script>
    <script type="text/javascript" src="../Js/jquery.jmpopups-0.5.1.pack.js"></script>
    <script type="text/javascript" src="../Js/common.js"></script>
    <script type="text/javascript" src="../Js/jquery.blockUI.js"></script>
    <script type="text/javascript" src="../CTI/CTITool.js"></script>
    <script type="text/javascript" src="../Js/json2.js"></script>
    <script type="text/javascript">
        function OpenAppealPopup() {
            $.openPopupLayer({
                name: "AppealPopup",
                url: "/AjaxServers/QualityStandard/AppealPopup.aspx",
                parameters: {
                    QS_RID: '<%=QS_RID %>',
                    Type: 'IM',
                    r: Math.random()
                },
                afterClose: function (e) {
                    if (e) {
                        $("#btnAppeal").css("display", "none");
                        loadApprovalHistory();
                    }
                }
            });
        }
        function Dispose(action, isReject) {
            var remark = encodeURIComponent($.trim($("#txtRemark").val()));
            if (remark.length == 0) {
                $.jAlert("请填写审核建议", function () {
                });
                return;
            }
            if (action == "auditagain") {
                if (isReject != "yes") {
                    var submitResult = SubQualityStandarIM();
                    if (!submitResult) {
                        $.jAlert("表单提交失败");
                        return;
                    }
                }
            }
            $.post("/AjaxServers/QualityStandard/IMHandler.ashx", { Action: action, QS_RID: '<%=QS_RID %>',
                IsReject: isReject, Remark: remark
            }, function (data) {
                if (data == "success") {
                    $.jAlert("操作成功！", function () {
                        closePageExecOpenerSearch();
                    });
                }
                else {
                    $.jAlert(data);
                }
            });
        }
        function loadApprovalHistory() {
            $("#divApprovalHistory").load("/AjaxServers/QualityStandard/ApprovalHistoryList.aspx",
            { QS_RID: '<%=QS_RID %>' }, function () {
            });
        }
        $(document).ready(function () {
            loadApprovalHistory();
            if ("<%=CanSeeMessage%>".toLowerCase() == "true") {
                document.domain = "bitauto.com";  //这个代码会导致刷新父页面方法失效，因为会出现跨域问题
                GotoFConversation();
            }
            else {
                $(".framediv").css("display", "none");
                $(".w980").removeClass("zj_dh");
            }
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
        <div class="framediv" style="position: fixed; z-index: 3; width: 362px; height: 880px;
            background: #FFF; *margin-left: -370px; border-bottom: #CCC 1px solid;">
            <iframe id="iframepage" marginheight="0" marginwidth="0" frameborder="0" width="362"
                style="height: 400px; overflow: hidden;" name="iframepage" onload="contenerSize();">
            </iframe>
        </div>
        <!--左历史记录end-->
        <!--右内容start-->
        <div class="w980 zj zj_dh">
            <div class="taskT">
                <%=TableName%>
            </div>
            <!--录音基本信息-->
            <uc3:UCConversationsView ID="UCConversationsViewID" runat="server" />
            <uc1:UCScoreTableDispose ID="ucTableDispose" runat="server" />
            <uc2:UCScoreTableView ID="UCScoreTableView" runat="server" />
            <div id="divApprovalHistory" class="lybase czjl">
            </div>
            <%if ((FirstTrialButton && Status == "20003") || (RecheckButton && Status == "20004"))
              { %>
            <div class="lybase fwgf">
                <div class="title">
                    审核建议</div>
                <div class="pj">
                    <textarea name="" id="txtRemark" cols="" rows=""></textarea></div>
            </div>
            <%} %>
            <!--质检评价-->
            <div class="btn" style="margin: 20px auto">
                <%if (AppealButton && Status == "20002")
                  { %>
                <input type="button" id="btnAppeal" name="" value="申诉" onclick="OpenAppealPopup()" />
                <%} %>
                <%if (FirstTrialButton && Status == "20003")
                  { %>
                <input type="button" name="" value="通过" onclick="Dispose('firstaudit','no')" />&nbsp;&nbsp;<input
                    type="button" name="" value="拒绝" onclick="Dispose('firstaudit','yes')" />
                <%} %>
                <%if (RecheckButton && Status == "20004")
                  { %>
                <input type="button" name="" value="通过" onclick="Dispose('auditagain','no')" />&nbsp;&nbsp;<input
                    type="button" name="" value="拒绝" onclick="Dispose('auditagain','yes')" />
                <%} %>
            </div>
        </div>
        <!--右内容end-->
    </div>
    </form>
</body>
</html>
