<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ExternalTaskProcess.aspx.cs"
    Inherits="BitAuto.ISDC.CC2012.Web.ExternalTask.ExternalTaskProcess" %>

<%@ Register Src="CustBaseInfo/UCCustBaseInfo/ViewCustBaseInfo.ascx" TagName="ViewCustBaseInfo"
    TagPrefix="uc1" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>呼叫中心-任务处理</title>
    <script src="../Js/jquery-1.4.4.min.js" type="text/javascript"></script>
    <script src="../Js/common.js" type="text/javascript"></script>
    <script src="../Js/jquery.jmpopups-0.5.1.pack.js" type="text/javascript"></script>
    <script src="../Js/jquery.uploadify.v2.1.4.min.js" type="text/javascript"></script>
    <script src="../Js/json2.js" type="text/javascript"></script>
    <link href="../Css/base.css" rel="stylesheet" type="text/css" />
    <link href="../Css/style.css" rel="stylesheet" type="text/css" />
    <script type="text/javascript">

        $(document).ready(function () {
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
            var pody = "RecID=" + consultDataID + "&RecordType=" + recordType;
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

        //员工姓名弹出层
        function openSelectEmployeePopup() {
            $.openPopupLayer({
                name: "SelectEmployeePopup",
                parameters: { DepartID: -1, Eid: -1, r: Math.random() },
                popupMethod: 'Post',
                url: "/TaskManager/SelectEmployees.aspx",
                beforeClose: function () {
                    var eid = $('#popupLayer_' + 'SelectEmployeePopup').data('EID');

                    if (eid != null) {
                        $("#employeeID").val(eid);
                        $("#domainAccount").val($('#popupLayer_' + 'SelectEmployeePopup').data('DomainAccount'));
                        $("#employeeName").val($('#popupLayer_' + 'SelectEmployeePopup').data('CnName'));
                    }
                }
            });
        }
        //提交
        function btnSubmit() {
            var isComplaint = $("#chkIsComplaint").attr("checked");
            var comment = encodeURIComponent($.trim($("#txtComment").val()));
            if (comment == "") {
                $.jAlert("处理意见必填！");
                return false;
            }
            var status = $("[name='rdoProcess']:checked").val();
            if (status == "undefined" || status == "" || status == null) {
                $.jAlert("处理状态必填！");
                return false;
            }
            var toNextSolveUserEID = $("#employeeID").val();
            if (toNextSolveUserEID != "") {
                if ($.jConfirm("选择下级受理人后您将不能再处理本任务，确认转出？", function (r) {
                    if (r) {
                        $.post
            (
                'AjaxServers/TaskManager/TaskManager.ashx',
                { 'Action': 'CustHistoryLogExternalSubmit', 'IsComplaint': isComplaint, 'CHITaskID': '<%=RequestTaskID %>', 'LoginEID': '<%=LoginEID %>',
                    'CHLStatus': status, 'Comment': comment, 'ToNextSolveUserEID': toNextSolveUserEID, 'random': Math.random()
                },
                    function (data) { 
                        var jsonData = $.evalJSON(data);
                        $.jAlert(jsonData.msg, function () { window.location.reload(); });
                    }
            )
                    }
                    else
                    { return false; }
                }));
            }
            else {
                $.post
            (
                'AjaxServers/TaskManager/TaskManager.ashx',
                { 'Action': 'CustHistoryLogExternalSubmit', 'IsComplaint': isComplaint, 'CHITaskID': '<%=RequestTaskID %>', 'LoginEID': '<%=LoginEID %>',
                    'CHLStatus': status, 'Comment': comment, 'ToNextSolveUserEID': toNextSolveUserEID, 'random': Math.random()
                },
                    function (data) { 
                        var jsonData = $.evalJSON(data);
                        $.jAlert(jsonData.msg, function () { window.location.reload(); });
                    }
            )
            }
        }

        //清空
        function clearEmployee() {
            $('#employeeName').val('');
            $('#employeeID').val('');
            $('#domainAccount').val('');
        }
    </script>
</head>
<body>
    <form id="form1" runat="server">
    <div class="w980">
        <div class="taskT">
            任务处理<span></span></div>
        <div class="baseInfo">
            <div class="title">
                基本信息<a href="javascript:void(0)" onclick="divShowHideEvent('divBaseInfo',this)" class="toggle hide"></a>
            </div>
            <div style="display: none" id="divBaseInfo">
                <uc1:ViewCustBaseInfo ID="ViewCustBaseInfo1" runat="server" />
            </div>
            <div class="title contact">
                联系记录<a href="javascript:void(0)" onclick="divShowHideEvent('ConsultHtml',this)" class="toggle"></a></div>
            <div id="ConsultHtml">
            </div>
            <div class="line">
            </div>
            <ul class="clearfix" runat="server" id="ulProcess">
                <li>
                    <label>
                        问题性质：</label><span id="QuestionQuality" runat="server" /></li>
                <li>
                    <label style="width: 126px;">
                        最晚处理时间:</label><span id="LastTreatmentTime" runat="server" /></li>
            </ul>
            <div class="deal" id="divDeal" runat="server" style="margin-left: 60px">
                <label class="bold">
                    <span class="redColor">*</span>填写处理意见</label>
                <textarea name="" cols="" rows="" id="txtComment"></textarea>
            </div>
            <ul class="clearfix" runat="server" id="liNextSolveUser">
                <li>
                    <label class="bold">
                        <span class="redColor">*</span>选择处理状态：</label>
                    <input type="radio" value="110001" id="rdoProcessSolve" name="rdoProcess" /><label for="rdoProcessSolve" style="float: none; font-weight:normal;">已解决</label>
                    <input type="radio" value="110003" id="rdoProcessUnresolved" name="rdoProcess" /><label for="rdoProcessUnresolved" style="float: none;font-weight:normal;">未解决</label>
                    <input type="radio" value="110002" id="rdoProcessNotSolve" name="rdoProcess" /><label for="rdoProcessNotSolve" style="float: none;font-weight:normal;">不解决</label></li>
                <li>
                    <label style="width: 126px" class="bold">
                        选择下一级受理人：</label>
                    <input style="width: 120px; padding: 4px 2px 0;" type="text" id="employeeName" value=""
                        class="w125" readonly="readonly" />
                    <input id="employeeID" style="display: none;" />
                    <input id="domainAccount" style="display: none;" />
                    <a href="javascript:void(0);" onclick="javascript:openSelectEmployeePopup();" style="text-decoration: none">
                        <input name="" type="button" value="选择" class="btnChoose" /></a><a href="javascript:void(0)"
                            onclick="javascirpt:clearEmployee();"> 取消选择</a> </li>
                <li>
                    <label>
                        是否确定投诉：</label><input type="checkbox" value="true" runat="server" id="chkIsComplaint" /><label for="chkIsComplaint" style="float: none;font-weight:normal;">是</label></li>
            </ul>
            <div class="title contact">
                处理记录<a href="javascript:void(0)" onclick="divShowHideEvent('ajaxTable',this)" class="toggle hide"></a></div>
            <div class="tableList clearfix tableList2" id="ajaxTable" style="display: none">
            </div>
            <div class="btn" style="margin: 20px auto">
                <span id="spanBtnSubmit" runat="server">
                    <input name="" type="button" value="提交" onclick="javascript:btnSubmit()" /></span>
            </div>
        </div>
    </div>
    </form>
</body>
</html>
