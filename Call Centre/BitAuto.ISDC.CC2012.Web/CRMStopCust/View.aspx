<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="View.aspx.cs" Inherits="BitAuto.ISDC.CC2012.Web.CRMStopCust.View" %>
<%@ Register Src="UCCust.ascx" TagName="UCCust" TagPrefix="uc1" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>客户核实-查看</title>
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
    </script>
    <script src="../Js/json2.js" type="text/javascript"></script>
    <script src="../../Js/jquery.jmpopups-0.5.1.pack.js" type="text/javascript"></script>
    <script src="../../Js/jquery.free.ajaxTabPanel.js" type="text/javascript"></script>
    <script type="text/javascript" src="http://api.map.baidu.com/api?v=1.4"></script>
 
    <%--审批、驳回操作 --%>
    <script type="text/javascript">
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
            $("a:not(.toggle)").hide();
            $("div[id$='divMap'] img").show(); //地图显示
            $("[name='aAudio']").show(); //播放录音 显示
        }
    </script>
</head>
<body>
    <form id="form1" runat="server">
    <!--客户联系人信息-->
    <input type="hidden" id="hidCallPhone" />
    <input type="hidden" id="hidCustName" />
    <input type="hidden" id="hidSex" />
    <input type="hidden" id="hidCustID" value="" />
    <input type="hidden" id="hidReleaseCount" value="0" />
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

        </div>
    </div>
    <div>
        <input type="hidden" id="hidOk" value="" />
    </div>
    </form>
</body>
</html>
