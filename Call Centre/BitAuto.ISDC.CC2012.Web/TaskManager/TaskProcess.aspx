<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="TaskProcess.aspx.cs" Inherits="BitAuto.ISDC.CC2012.Web.TaskManager.TaskProcess" %>
<!--功能废弃 强斐 2016-8-19-->
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>任务处理</title>
    <link href="../Css/base.css" rel="stylesheet" type="text/css" />
    <link href="../Css/style.css" rel="stylesheet" type="text/css" />
    <link href="../css/CTIPopup.css" type="text/css" rel="stylesheet" />
    <script src="../Js/jquery-1.4.4.min.js" type="text/javascript"></script>
    <script src="../Js/jquery.jmpopups-0.5.1.pack.js" type="text/javascript"></script>
    <script src="../Js/common.js" type="text/javascript"></script>
    <script src="../Js/json2.js" type="text/javascript"></script>
    <script src="../Js/jquery.uploadify.v2.1.4.min.js" type="text/javascript"></script>
    <script src="../Js/jquery.blockUI.js" type="text/javascript"></script>
    <script src="../CTI/CTITool.js" type="text/javascript"></script>
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
            var taskId = "<%=RequestTaskID %>";
            var pody = "RecID=" + consultDataID + "&RecordType=" + recordType + "&TaskID=" + taskId;
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
                url: "SelectEmployees.aspx",
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
            //modify by qizq 2012-1-4在请求中加了3个参数，IsCalling是否在通话中，1表示在通话中，CallRecordID本地录音表主键，HistoryLogID处理历史记录表主键
            if (toNextSolveUserEID != "") {
                if ($.jConfirm("选择下级受理人后您将不能再处理本任务，确认转出？", function (r) {
                    if (r) {
                        $.post
            (
                '../AjaxServers/TaskManager/TaskManager.ashx',
                { 'Action': 'CustHistoryLogSubmit', 'IsComplaint': isComplaint, 'CHITaskID': '<%=RequestTaskID %>',
                    'CHLStatus': status, 'Comment': comment, 'ToNextSolveUserEID': toNextSolveUserEID, 'random': Math.random(), 'IsCalling': $('#hidIsCalling').val(),
                    'CallRecordID': $('#hidCallID').val(), 'HistoryLogID': $('#hidHistoryLogID').val()
                },
                    function (data) {
                        var jsonData = $.evalJSON(data);

                        //modify by qizq 2012-1-4 提交成功后
                        if (jsonData.result == "yes") {
                            //标识已提交过
                            $('#hidIsSub').val("1");
                            //清空处理记录主键
                            $('#hidHistoryLogID').val("");
                            //清空备注
                            $("#txtComment").val("");
                        }
                        //


                        $.jAlert(jsonData.msg, function () {
                            //有电话录音不容许刷新页面
                            //window.location.reload(); 
                            //加载处理记录
                            loadProcessDetail();
                        });
                    }
            )
                    }
                    else {
                        return false;
                    }
                }));
            }
            else {




                $.post
            (
                '../AjaxServers/TaskManager/TaskManager.ashx',
                { 'Action': 'CustHistoryLogSubmit', 'IsComplaint': isComplaint, 'CHITaskID': '<%=RequestTaskID %>',
                    'CHLStatus': status, 'Comment': comment, 'ToNextSolveUserEID': toNextSolveUserEID, 'random': Math.random(), 'IsCalling': $('#hidIsCalling').val(),
                    'CallRecordID': $('#hidCallID').val(), 'HistoryLogID': $('#hidHistoryLogID').val()
                },
                    function (data) {
                        var jsonData = $.evalJSON(data);
                        //modify by qizq 2012-1-4 提交成功后，
                        if (jsonData.result == "yes") {
                            //标识已提交过
                            $('#hidIsSub').val("1");
                            //清空处理记录主键
                            $('#hidHistoryLogID').val("");
                            //清空备注
                            $("#txtComment").val("");
                        }
                        $.jAlert(jsonData.msg, function () {
                            //有电话录音不容许刷新页面
                            //window.location.reload(); 
                            //加载处理记录
                            loadProcessDetail();
                        });
                    }
            )
            }
        }
        //同意转出
        function agreeTurnOut() {
            if ($("#hidIsTurnOut").val() == "1") {
                $.jAlert("您已操作同意转出，无法再次操作");
                return false;
            }

            var isComplaint = $("#chkIsComplaint").attr("checked");
            var Comment = $("#txtComment").val();
            if ($.jConfirm("转出后将邮件通知受理人，确认转出？", function (r) {
                if (r) {
                    $.post
            (
                '../AjaxServers/TaskManager/TaskManager.ashx',
                { 'Action': 'AgreeTurnOut', 'TCSTaskID': '<%=RequestTaskID %>',
                    'IsComplaint': isComplaint, 'Comment': Comment, 'random': Math.random()
                },
                    function (data) {
                        var jsonData = $.evalJSON(data);
                        $.jAlert(jsonData.msg, function () {
                            $("#hidIsTurnOut").val("1");
                            $("#btnTurnOut").hide();
                            //                            window.location.reload(); 
                        });
                    }
            )
                }
                else
                { return false; }
            }));
        }
        //结束任务
        function endTask() {
            var isComplaint = $("#chkIsComplaint").attr("checked");
            var Comment = $("#txtComment").val();
            if ($.jConfirm("结束后此任务不可再处理，确认结束？", function (r) {
                if (r) {
                    $.post
            (
                '../AjaxServers/TaskManager/TaskManager.ashx',
                { 'Action': 'TaskEnd', 'CHITaskID': '<%=RequestTaskID %>', 'Comment': Comment, 'IsComplaint': isComplaint, 'random': Math.random()
                },
                    function (data) {
                        var jsonData = $.evalJSON(data);
                        $.jAlert(jsonData.msg, function () { window.location.reload(); });
                    }
            )
                }
                else {
                    return false;
                }
            }));
        }
        //清空
        function clearEmployee() {
            $('#employeeName').val('');
            $('#employeeID').val('');
            $('#domainAccount').val('');
        }

        //add by qizq 2013-1-6呼出电话回调函数
        ADTTool.onInitiated = function (data) {
            var onInitiatedTime = new Date();
            $('#hidonInitiatedTime').val(onInitiatedTime.getTime());

            //add by lihf 2013-8-19呼出电话记录CallRecord_ORIG_Business            
            var pody = { Action: "InsertCallRecordORIGBusiness",
                CallID: data.CallID,
                TaskID: escape('<%=RequestTaskID%>')
            };
            //插入中间表记录
            $.post("../AjaxServers/TaskManager/CallOutDeal.ashx", pody, function (data) {
                callRecordID = data;
            });
        }
        //add by qizq 2013-1-6呼出电话坐席摘机回调函数//拨出电话到坐席摘机为坐席振铃时间
        ADTTool.onNetworkReached = function (data) {
            var NetworkReachedTime = new Date();
            $('#hidNetworkReachedTime').val(NetworkReachedTime.getTime());
            var timespan = parseInt((NetworkReachedTime.getTime() - parseInt($('#hidonInitiatedTime').val())) / 1000);
            //拨出电话到坐席摘机为坐席振铃时间
            $('#hidTimeSpan').val(timespan);



        }

        //add by qizq 2013-1-4电话录音接通后回调函数//坐席摘机到客户接通电话为客户振铃时间
        ADTTool.Established = function (data) {
            //通话中状态标示，1表示通话中
            $('#hidIsCalling').val("1");

            //清空保存过标识
            $('#hidIsSub').val("");
            //清空处理记录主键
            $('#hidHistoryLogID').val("");
            //清空录音主键
            $('#hidCallID').val("");

            var EstablishedTime = new Date();
            //坐席摘机到客户接通电话为客户振铃时间
            var timespan = parseInt((EstablishedTime.getTime() - parseInt($('#hidNetworkReachedTime').val())) / 1000);

            $.post('../AjaxServers/TaskManager/CallOutDeal.ashx', {
                Action: escape("Established"),
                UserEvent: escape(data.UserEvent),
                UserName: escape(data.UserName),
                CalledNum: escape(data.CalledNum), //对方
                CallerNum: escape(data.CallerNum),  //本机
                CallID: escape(data.CallID),
                UserChoice: escape(data.UserChoice),
                RecordID: escape(data.RecordID),
                RecordIDURL: escape(data.RecordIDURL),
                AgentState: escape(data.AgentState),
                AgentAuxState: escape(data.AgentAuxState),
                MediaType: escape(data.MediaType),
                CustID: escape($('#hidcustID').val()),
                CustName: escape($('#hidcustName').val()),
                Contact: escape($('#hidcustName').val()),
                TaskID: escape('<%=RequestTaskID%>'),
                NetworkRTimeSpan: escape($('#hidTimeSpan').val()),
                EstablishTimeSpan: escape(timespan),
                //取接通开始时间
                EstablishBeginTime: data.CurrentDate
            },
            function (data) {
                var jsonData = $.evalJSON(data);
                //不成功提示错误，成功把录音主键保存在隐藏域里面
                if (jsonData.success == 'no') {
                    $.jAlert(jsonData.msg);
                }
                else {
                    $('#hidCallID').val(jsonData.recordid)
                }
            });
        }
        //add by qizq 2013-1-4电话挂断后回调函数
        ADTTool.onDisconnected = function (data) {
            var logMsg = "data.Action:" + data.UserEvent;
            logMsg += ",data.UserName:" + data.UserName;
            logMsg += ",data.CalledNum:" + data.CalledNum;
            logMsg += ",data.CallerNum:" + data.CallerNum;
            logMsg += ",data.CallID:" + data.CallID;
            logMsg += ",data.UserChoice:" + data.UserChoice;
            logMsg += ",data.RecordID:" + data.RecordID;
            logMsg += ",data.RecordIDURL:" + data.RecordIDURL;
            logMsg += ",data.AgentState:" + data.AgentState;
            logMsg += ",data.AgentAuxState:" + data.AgentAuxState;
            logMsg += ",data.MediaType:" + data.MediaType;
            logMsg += ",data.CallType:" + data.CallType;
            InsertCallRecordEventLog("qizq_onDisconnected," + data.IsEstablished, data.RecordID, logMsg);
            if (data.IsEstablished == "True") {

                //通话中状态标示，0表示不是通话中
                $('#hidIsCalling').val("0");
                //判断是否有录音主键，如果不存在说明页面已刷新，或通话开始回调函数执行不成功
                //if ($('#hidCallID').val() == "") {

                //}
                //有录音主键
                //else {
                //更新宇高录音接口
                //var jsonstr = '{"CustID":' + $('#hidcustID').val() + ',"CallStatus":"呼出","TaskTypeID":"3"}';
                //var Result = window.external.MethodScript('/recordcontrol/updaterecorddatabyid?Refid=' + escape(data.RecordID) + '&data=' + escape(jsonstr));
                $.post('../AjaxServers/TaskManager/CallOutDeal.ashx', {
                    Action: escape("Released"),
                    UserEvent: escape(data.UserEvent),
                    UserName: escape(data.UserName),
                    CalledNum: escape(data.CalledNum), //对方
                    CallerNum: escape(data.CallerNum),  //本机
                    CallID: escape(data.CallID),
                    UserChoice: escape(data.UserChoice),
                    RecordID: escape(data.RecordID),
                    RecordIDURL: escape(data.RecordIDURL),
                    AgentState: escape(data.AgentState),
                    AgentAuxState: escape(data.AgentAuxState),
                    MediaType: escape(data.MediaType),
                    CustID: escape($('#hidcustID').val()),
                    CustName: escape($('#hidcustName').val()),
                    Contact: escape($('#hidcustName').val()),
                    TaskID: escape('<%=RequestTaskID%>'),
                    CallRecordID: escape($('#hidCallID').val()),
                    //是否提交过，提交过不插入处理记录
                    IsSub: escape($('#hidIsSub').val()),
                    EstablishEndTime: data.CurrentDate

                },
            function (data) {
                var jsonData = $.evalJSON(data);
                //不成功提示错误
                if (jsonData.success == 'no') {
                    $.jAlert(jsonData.msg);
                }
                else {
                    $('#hidHistoryLogID').val(jsonData.recordid)
                    //加载处理记录
                    loadProcessDetail();
                }
            });
                //}
            }
        }

    </script>
</head>
<body>
    <form id="form1" runat="server">
    <%-- 记录电话播出到坐席接起电话时间间隔秒数--%>
    <input type="hidden" id="hidTimeSpan" />
    <%-- 记录坐席接起电话时间--%>
    <input type="hidden" id="hidNetworkReachedTime" />
    <%-- 记录播出电话时间--%>
    <input type="hidden" id="hidonInitiatedTime" />
    <%-- 是否在通话中，通话中为1--%>
    <input type="hidden" id="hidIsCalling" />
    <%-- 本地录音表主键--%>
    <input type="hidden" id="hidCallID" />
    <%-- 处理历史记录主键--%>
    <input type="hidden" id="hidHistoryLogID" />
    <%-- 是否已提交，已提交为1，在挂断回调函数要查看此状态，从而判断是否插入历史处理记录--%>
    <input type="hidden" id="hidIsSub" />
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
            <ul class="clearfix">
                <li>
                    <label>
                        问题性质：</label><span id="QuestionQuality" runat="server" /></li>
                <li>
                    <label style="width: 126px;">
                        最晚处理时间：</label><span id="LastTreatmentTime" runat="server" /></li>
            </ul>
            <div class="deal" id="divDeal" runat="server" style="margin-left: 60px">
                <label class="bold">
                    <span class="redColor">*</span>填写处理意见</label>
                <textarea name="" cols="" rows="" id="txtComment"></textarea>
            </div>
            <ul class="clearfix" runat="server" id="ulProcess">
                <li><span runat="server" id="spanProcessStatus">
                    <label class="bold">
                        <span class="redColor">*</span>选择处理状态：</label>
                    <input type="radio" value="110001" id="rdoProcessSolve" name="rdoProcess" /><label
                        for="rdoProcessSolve" style="float: none;">已解决</label>
                    <input type="radio" value="110003" id="rdoProcessUnresolved" name="rdoProcess" /><label
                        for="rdoProcessUnresolved" style="float: none;">未解决</label>
                    <input type="radio" value="110002" id="rdoProcessNotSolve" name="rdoProcess" /><label
                        for="rdoProcessNotSolve" style="float: none;">不解决</label></span></li>
                <li><span runat="server" id="liNextSolveUser">
                    <label style="width: 126px" class="bold">
                        选择下一级受理人：</label>
                    <input style="padding: 4px 2px 0;" type="text" id="employeeName" value="" class="w125"
                        readonly="readonly" disabled="disabled" />
                    <input id="employeeID" style="display: none;" />
                    <input id="domainAccount" style="display: none;" />
                    <a href="javascript:void(0);" onclick="javascript:openSelectEmployeePopup();" style="text-decoration: none">
                        <input name="" type="button" value="选择" class="btnChoose" /></a> <a href="javascript:void(0)"
                            onclick="javascirpt:clearEmployee();">取消选择</a> </span></li>
                <li>
                    <label>
                        是否确定投诉：</label><input type="checkbox" value="true" runat="server" id="chkIsComplaint" /><label
                            for="chkIsComplaint" style="float: none; font-weight: normal;">是</label></li>
                <li><span id="spanHighOper" runat="server">
                    <label style="width: 126px" class="bold">
                        高级操作：</label><input name="" type="button" value="同意转出" onclick="javascript:agreeTurnOut()"
                            class="btnChoose" id="btnTurnOut" runat="server" />
                    <input name="" type="button" value="结束任务" class="btnChoose" onclick="javascript:endTask()"
                        id="btnTurnOver" runat="server" /></span> </li>
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
    <input type="hidden" id="hidIsTurnOut" value="0" />
    </form>
</body>
</html>
