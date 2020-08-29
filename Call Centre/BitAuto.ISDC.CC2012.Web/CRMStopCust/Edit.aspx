<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Edit.aspx.cs" Inherits="BitAuto.ISDC.CC2012.Web.CRMStopCust.Edit" %>

<%@ Register Src="UCCust.ascx" TagName="UCCust" TagPrefix="uc1" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>客户核实处理</title>
    <link href="../../Css/base.css" type="text/css" rel="stylesheet" />
    <link href="../../Css/style.css" type="text/css" rel="stylesheet" />
    <link href="../../Css/cc_checkStyle.css" type="text/css" rel="stylesheet" />
    <link href="../../Css/adtPopup.css" rel="stylesheet" type="text/css" />
    <script type="text/javascript">
        document.writeln("<s" + "cript type='text/javascript' src='/Js/config.js?r=" + Math.random() + "'></scr" + "ipt>");
    </script>
    <script src="../../Js/jquery-1.4.4.min.js" type="text/javascript"></script>
    <script type="text/javascript">
        loadJS("common");
        loadJS("CTITool");
        loadJS("UserControl");
    </script>
    <script src="../Js/json2.js" type="text/javascript"></script>
    <script src="../../Js/jquery.jmpopups-0.5.1.pack.js" type="text/javascript"></script>
    <script src="../../Js/jquery.free.ajaxTabPanel.js" type="text/javascript"></script>
    <script type="text/javascript" src="http://api.map.baidu.com/api?v=1.4"></script>
    <script type="text/javascript">
        $(function () {
            //初始化电话控件
            HollyPhoneControl.Init("客户核实", "<%=TaskID %>", "<%=BGID %>", "<%=SCID %>", "");
            //注册接通事件
            HollyPhoneControl.SetEstablishedEvent(function () {
                //接通后让提交按钮不可用
                $('#btnPend').attr("disabled", "disabled");
                $('#btnReject').attr("disabled", "disabled");
            });
            //注册挂断保存成功事件
            HollyPhoneControl.SetReleaseEvent(function (jsondata) {
                //挂断后让提交按钮可用
                $('#btnPend').attr("disabled", "");
                $('#btnReject').attr("disabled", "");
                //加载处理记录
                loadCallRecord();
                SaveCustBasicInfoPoP(jsondata);
            });
            //初始化短信控件
            SendMessageControl.Init("客户核实", "<%=TaskID %>", "<%=BGID %>", "<%=SCID %>", "");
            //注册回调事件
            SendMessageControl.SetSendMessageCompleteEvent(function (recid, jsondata) {
                SaveCustBasicInfoPoP(jsondata);
            });
        });
    </script>
    <!--审批、驳回操作 -->
    <script type="text/javascript">
        //驳回理由弹出层
        function Pending(type) {
            $.openPopupLayer({
                name: "RejectReasonPopup",
                parameters: { CRMStopCustApplyID: "<%=CRMStopCustApplyID %>", TaskID: "<%=TaskID %>", Type: type, r: Math.random() },
                popupMethod: 'Post',
                url: "ApprovalPop.aspx",
                beforeClose: function () {
                    if ($("#hidOk").data("result") == "true") {
                        closePageExecOpenerSearch();
                    }
                }
            });
        }

        //展开收缩
        function divShowHideEvent(divId, obj) {
            if ($("#" + divId).css("display") == "none") {
                $("#" + divId).show("slow");
                $(obj).attr("class", "toggle");
            }
            else {
                $("#" + divId).hide("slow");
                $(obj).attr("class", "toggle hide");
            }
        }
        function loadTaskLog() {
            $('#divTaskLog').load('HistoryList.aspx', {
                ContentElementId: 'divTaskLog',
                TaskID: '<%= this.TaskID %>'
            });
        }
        function loadCallRecord() {
            $('#divCallRecordList').load('CallRecord.aspx', {
                ContentElementId: 'divCallRecordList',
                TaskID: '<%= this.TaskID %>',
                PageSize: 10
            });
        }

        $(function () {
            $("div[name='uc_survey']").hide();
            loadTaskLog();
            loadCallRecord();
            viewLimit();
        });
        window.onload = function () {
            viewLimit();
        }
        function viewLimit() {
            $("a:not(.toggle),img").hide();
            $("div[id$='divMap'] img").show(); //地图显示
            $("[name='aAudio']").show(); //播放录音 显示
            if ("<%=operType %>" == "e") {
                //是编辑时显示操作
                $("a,img").show();
            }
        }
    </script>
</head>
<body>
    <form id="form1" runat="server">
    <div class="w980">
        <div class="taskT">
            客户信息<span></span></div>
        <div class="baseInfo clearfix">
            <uc1:UCCust ID="UCCust1" runat="server" />
            <div class="title contact" style="clear: both;">
                操作记录<a href="javascript:void(0)" onclick="divShowHideEvent('divTaskLog',this)" class="toggle"></a></div>
            <div id="divTaskLog" class="fullRow  cont_cxjg" style="margin: 0 18px;">
            </div>
            <div class="title contact">
                通话记录<a href="javascript:void(0)" onclick="divShowHideEvent('divCallRecordList',this)"
                    class="toggle"></a></div>
            <div id="divCallRecordList" class="fullRow  cont_cxjg" style="margin: 0 18px;">
            </div>
        </div>
        <div>
        </div>
        <div class="btn" style="clear: both;">
            <% if (stopStatus)
               { %>
            <input type="button" onclick="javascript:Pending(1);" class="button" value="审核通过"
                id="btnPend" />
            <input type="button" onclick="javascript:Pending(2);" class="button" value="驳 回"
                id="btnReject" />
            <% } %>
        </div>
    </div>
    <div>
        <input type="hidden" id="hidOk" value="" />
    </div>
    </form>
</body>
</html>
