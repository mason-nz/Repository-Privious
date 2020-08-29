<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="TaskDetail.aspx.cs" Inherits="BitAuto.ISDC.CC2012.Web.TaskManager.TaskDetail" %>

<%@ Register Src="NoDealerOrder/UCNoDealerOrder/ContactRecordView.ascx" TagName="ContactRecordView"
    TagPrefix="uc1" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>任务信息查看</title>
    <script src="../Js/jquery-1.4.4.min.js" type="text/javascript"></script>
    <script src="../Js/common.js" type="text/javascript"></script>
    <script src="../Js/jquery.jmpopups-0.5.1.pack.js" type="text/javascript"></script>
    <script src="../Js/jquery.uploadify.v2.1.4.min.js" type="text/javascript"></script>
    <script src="../Js/json2.js" type="text/javascript"></script>
    <script src="/CTI/CTITool.js" type="text/javascript"></script>
    <link href="../Css/base.css" rel="stylesheet" type="text/css" />
    <link href="../Css/style.css" rel="stylesheet" type="text/css" />
    <script type="text/javascript">
        $(document).ready(function () {
            $('#imgTel').hide();
            getConsultInfo();

            loadProcessDetail();
        });
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

        //加载联系记录中咨询类型表信息
        //modify by qizq 2012-12-20 把经销商合作，经销商反馈，经销商其他分三个页面查看
        function getConsultInfo() {
            var consultDataID = "<%=RequestConsultDataID %>";
            var recordType = "<%=RequestRecordType %>";
            var taskId = "<%=RequestTaskID %>";
            var pody = "RecID=" + consultDataID + "&RecordType=" + recordType + "&TaskID="+taskId+"&r=" + Math.random();
            switch ("<%=RequestConsultID %>") {
                case "60001": loadConsultHtml("ConsultNewCarView.aspx", pody);
                    break;
                case "60002": loadConsultHtml("ConsultSecondCarView.aspx", pody);
                    break;
                case "60003": loadConsultHtml("ConsultPFeedbackView.aspx", pody);
                    break;
                case "60004": loadConsultHtml("ConsultActivityView.aspx", pody);
                    break;
                case "60005": loadConsultHtml("ConsultPUseCarView.aspx", pody);
                    break;
                case "60006": loadConsultHtml("ConsultPOtherView.aspx", pody);
                    break;
                case "60007":
                    loadConsultHtml("ConsultDCoopView.aspx", pody);
                    break;
                case "60008":
                    loadConsultHtml("ConsultDCFeedbackView.aspx", pody);
                    break;
                case "60009":
                    loadConsultHtml("ConsultDCoopOtherView.aspx", pody);
                    break;
                case "60010":
                case "60011":
                    $("#ucCar").show();
                    bindHistory();
                    $("#divNoOrder").show();
                    break;
            }
        }

        //根据对应页面加载7个表中的一个页面
        function loadConsultHtml(aspx, pody) {
            LoadingAnimation("ConsultHtml");
            $("#ConsultHtml").load("../ConsultManager/" + aspx, pody);
        }

        //加载处理记录
        function loadProcessDetail() {
            var pody = 'TaskID=<%=RequestTaskID %>&r=' + Math.random();
            LoadingAnimation("ajaxTable");
            $("#ajaxTable").load("../AjaxServers/TaskManager/TaskProcessDetail.aspx", pody);
        }
        //绑定无主订单的处理（历史）记录
        function bindHistory() {
            LoadingAnimation("ajaxTableByNoOrder");
            $("#ajaxTableByNoOrder").load("../AjaxServers/TaskManager/NoDealerOrder/OrderTaskOperLogList.aspx", { TaskID: '<%=RequestTaskID %>', r: Math.random() });
        }
    </script>
</head>
<body>
    <form id="form1" runat="server">
    <div class="w980">
        <div class="taskT">
            任务查看<span></span></div>
        <div class="baseInfo">
            <div class="title">
                基本信息<a href="javascript:void(0)" onclick="divShowHideEvent('divBaseInfo',this)" class="toggle"></a>
            </div>
            <div id="divBaseInfo">
                <uc2:ViewCustBaseInfo ID="ViewCustBaseInfo1" runat="server" />
            </div>
            <div class="title contact">
                联系记录<a href="javascript:void(0)" onclick="divShowHideEvent('ConsultHtml',this)" class="toggle"></a></div>
            <div id="ConsultHtml">
                <div id="ucCar" style="display: none">
                    <uc1:ContactRecordView ID="ContactRecordView1" runat="server" />
                </div>
            </div>
            <div class="line" id="divLine" runat="server">
            </div>
            <ul class="clearfix ft14" id="ulDeal" runat="server">
                <li>
                    <label>
                        问题性质：</label><span id="QuestionQuality" runat="server" /></li>
                <li>
                    <label style="width: 126px;">
                        最晚处理时间:</label><span id="LastTreatmentTime" runat="server" /></li>
                <li>
                    <label>
                        是否投诉：</label><span id="IsComplaint" runat="server" /></li>
            </ul>
            <div id="divDeal" runat="server">
                <div class="title contact">
                    处理记录<a href="javascript:void(0)" onclick="divShowHideEvent('ajaxTable',this)" class="toggle"></a></div>
                <div class="tableList clearfix tableList2" id="ajaxTable">
                </div>
            </div>
            <%--<div id="divNoOrder" runat="server" style="display:none">
                <div class="title contact">
                    处理记录<a href="javascript:void(0)" onclick="divShowHideEvent('ajaxTableByNoOrder',this)" class="toggle"></a></div>
                <div class="tableList clearfix tableList2" id="ajaxTableByNoOrder">
                </div>
            </div>--%>
        </div>
    </div>
    </form>
</body>
</html>
