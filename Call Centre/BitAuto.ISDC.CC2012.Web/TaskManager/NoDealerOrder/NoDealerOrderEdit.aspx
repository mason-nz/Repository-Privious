<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="NoDealerOrderEdit.aspx.cs"
    Inherits="BitAuto.ISDC.CC2012.Web.TaskManager.NoDealerOrder.NoDealerOrderEdit" %>

<%@ Register Src="UCNoDealerOrder/UCOrderConsult.ascx" TagName="UCOrderConsult" TagPrefix="uc1" %>
<%@ Register Src="UCNoDealerOrder/EditCustBaseInfo.ascx" TagName="EditCustBaseInfo"
    TagPrefix="uc2" %>
<%@ Register Src="UCNoDealerOrder/BuyCarInfo.ascx" TagName="BuyCarInfo" TagPrefix="uc3" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <meta http-equiv="X-UA-Compatible" content="IE=EmulateIE7" />
    <title>无主订单</title>
    <link href="/css/base.css" type="text/css" rel="stylesheet" />
    <link href="/css/CTIPopup.css" type="text/css" rel="stylesheet" />
    <link href="/css/style.css" type="text/css" rel="stylesheet" />
    <script src="/Js/jquery-1.4.4.min.js" type="text/javascript"></script>
    <script src="/Js/common.js" type="text/javascript"></script>
    <script src="/Js/jquery.jmpopups-0.5.1.pack.js" charset="utf-8" type="text/javascript"></script>
    <script src="/Js/json2.js" type="text/javascript"></script>
    <script src="/Js/Enum/Area2.js" type="text/javascript"></script>
    <script src="/Js/jquery.blockUI.js" type="text/javascript"></script>
    <script src="../../CTI/CTITool.js" type="text/javascript"></script>
    <script type="text/javascript">
        function DeleteTask() {
            $.openPopupLayer({
                name: "ReasonPopup",
                parameters: { TaskID: "<%=TaskID %>", r: Math.random() },
                popupMethod: 'Post',
                url: "DeleteTask.aspx",
                beforeClose: function () {

                }
            });
        }
    </script>
    <script type="text/javascript">
        $(document).ready(function () {

            ShowControl();

            $("#btnSave").click(Save);
            $("#btnSubmit").click(Submit);
            $("#btnDelete").click(DeleteTask);

            bindHistory();

            //隐藏历史记录
            $("#divHis").attr("class", "toggle");
            $("#divHandlerHistory").hide();

            //根据订单状态禁用保存、提交按钮
            CheckStata();

        });

        function CheckStata() {

            //判断任务状态
            var taskStata = '<%=TaskStatus %>';
            if (taskStata != 2 && taskStata != 3) {

                $("#btnSave").attr("disabled", "disabled");
                $("#btnSumbit").attr("disabled", "disabled");

                $.jAlert("当前任务状态不能保存和提交！", function () {
                    closePage();
                });

            }

            //判断权限
            var isEditTask = '<%=IsEditTask %>';
            if (isEditTask == "0") {

                $("#btnSave").attr("disabled", "disabled");
                $("#btnSumbit").attr("disabled", "disabled");
                $.jAlert("您没有处理订单的权限！", function () {
                    closePage();
                });
            }

            //判断处理人是否是当前用户
            var IsBelong = '<%=IsBelong %>';
            if (IsBelong == "0") {

                $("#btnSave").attr("disabled", "disabled");
                $("#btnSumbit").attr("disabled", "disabled");
                $.jAlert("此订单没有分配给你！", function () {
                    closePage();
                });
            }

        }

        //绑定历史记录
        function bindHistory() {
            LoadingAnimation("divHandlerHistory");
            $("#divHandlerHistory").load("../../AjaxServers/TaskManager/NoDealerOrder/OrderTaskOperLogList.aspx", { TaskID: '<%=TaskID %>', r: Math.random() });
        }

        function ShowControl() {
            var TaskID = '<%=TaskID %>';
            if (TaskID == "") {
                $.jAlert("没有指定任务ID", function () { closePage(); })
            }
            else {
                //类型   1新车订单   2 置换订单
                var source = '<%=Source %>';
                if (source == "1" || source == "3") {
                    $("ul[tagcode='60001']").show(); //新车联系记录控件显示
                }
                else if (source == "2") {
                    $("ul[tagcode='60002']").show(); //置换联系记录控件显示
                }
            }
        }
 
    </script>
    <script type="text/javascript">

        function Save() {

            $("#inputSaveOrSub").val("save");

            var info = CustBaseInfoHelper.GetAllDataInPage();
            var msg = CustBaseInfoHelper.ValidateData(info.CustBaseInfo);
            if (Len(msg) > 0) {
                $.jAlert(msg);
                return;
            }
            if (!DataCheck()) {
                return;
            }
            var msg = CheckControl();
            if (msg != "") {
                $.jAlert(msg)
            }
            else {

                SaveInfo();
            }
        }

        function Submit() {
            $("#inputSaveOrSub").val("sub");

            var info = CustBaseInfoHelper.GetAllDataInPage();
            var msg = CustBaseInfoHelper.ValidateData(info.CustBaseInfo);
            if (Len(msg) > 0) {
                $.jAlert(msg);
                return;
            }
            if (!DataCheck()) {
                return;
            }
            var msg = CheckControl();

            if (msg != "") {
                $.jAlert(msg)
            }
            else {
                //获取经销商Code
                var source = '<%=Source %>';
                var DMSCode = "";
                if (source == "1" || source == "3") {
                    DMSCode = $("[id$='hidNewCarDMSmemberCode']").val();
                }
                else if (source == "2") {
                    DMSCode = $("[id$='hidWantDMSMemberCode']").val();
                }

                if (DMSCode == "") {
                    //没有选择，就弹出框输入原因

                    $.openPopupLayer({
                        name: "ReasonPopup",
                        parameters: {},
                        url: "/AjaxServers/TaskManager/NoDealerOrder/NoDealerReasonDialog.aspx",
                        beforeClose: function (e) {
                            $("#hidReasonId").val($("[id$='dllReason']").val());
                            $("#hidNoDealerReason").val($("[id$='txtContent']").val());
                        },
                        afterClose: function (e) {
                            if (e) {

                                SaveInfo();
                            }
                        }
                    });
                }
                else {
                    SaveInfo();
                }

            }
        }
        function SaveInfo() {

            var isSub = $("#inputSaveOrSub").val();
            if (isSub == "sub") {

                $.openPopupLayer({
                    name: "ADTToolConverPopup",
                    parameters: {},
                    url: "Waiting.aspx",
                    success: function () {
                        //alert("如果个人客户库里有同名的客户，将会合并信息");
                        $.jAlert("如果个人客户库里有同名的客户，将会合并信息", function () {
                            if (SubmitDataBasicInfo()) {
                                if (subBuyCarInfo()) {
                                    //2、保存联系记录信息
                                    SaveCon();
                                }
                            }
                        });

                    }

                });

            }
            else {
                //1、调用基本信息保存                
                if (SubmitDataBasicInfo()) {
                    if (subBuyCarInfo()) {
                        //2、保存联系记录信息
                        SaveCon();
                    }
                }
            }
        }

        ///保存联系记录信息
        function SaveCon() {

            //基本信息
            var age = $("#txtAge").val();
            var IDCards = $("#txtIDCards").val();
            var Vocation = $("select[id$='selVocation']").val();
            var InCome = $("select[id$='selInCome']").val();
            var Marriage = $("input[name='radMarriage']:checked").val();
            if (Marriage == undefined) {
                Marriage = "";
            }
            var CarName = $("#txtCarName").val();
            var legalize = $("input[name='legalize']:checked").val();
            if (legalize == undefined) {
                legalize = "";
            }
            var DriveAge = $("#txtDriveAge").val();
            var UserName = $("#txtUserName").val();
            var CarNumber = $("#txtCarNumber").val();
            var Remark = $("#txtNote").val();
            var Type = 0;
            $("input[name$='custType']").each(function () {
                if ($(this).attr("checked")) {
                    Type = $(this).val();
                }
            });

            var json = {
                Action: escape($("#inputSaveOrSub").val()),
                TaskID: escape('<%=TaskID %>'),
                Source: escape('<%=Source %>'),
                NewCarConsultInfo: escape(JSON.stringify(GetNewCarJson())),
                ReplaceCarConsultInfo: escape(JSON.stringify(GetReplaceJson())),
                NoDealerReasonID: escape($("#hidReasonId").val()),
                NoDealerReason: escape($("#hidNoDealerReason").val()),

                Age: encodeURIComponent(age),
                IDCard: encodeURIComponent(IDCards),
                Vocation: encodeURIComponent(Vocation),
                InCome: encodeURIComponent(InCome),
                Marriage: encodeURIComponent(Marriage),
                CarName: escape(CarName),
                IsAttestation: encodeURIComponent(legalize),
                DriveAge: encodeURIComponent(DriveAge),
                UserName: escape(UserName),
                CarNo: escape(CarNumber),
                Remark: escape(Remark),
                Type: encodeURIComponent(Type),
                //add by qizq 2013-1-5 在请求中加了3个参数，IsCalling是否在通话中，1表示在通话中，CallRecordID本地录音表主键，HistoryLogID处理历史记录表主键
                IsCalling: $('#hidIsCalling').val(),
                CallRecordID: $('#hidCallID').val(),
                HistoryLogID: $('#hidHistoryLogID').val()
            };





            AjaxPost("/AjaxServers/TaskManager/NoDealerOrder/OrderConsultInfo.ashx", json,
                         function () {

                             $("#btnSave").attr("disabled", "disabled");
                             $("#btnSumbit").attr("disabled", "disabled");
                             $("#imgLoadingPop").show();
                         },
                         function (data) {
                             $.closePopupLayer('ADTToolConverPopup');

                             if (data == "success") {

                                 if ($("#inputSaveOrSub").val() == "save") {
                                     $.jPopMsgLayer("保存成功！", function () {
                                         //modify by qizq 2013-1-5,保存成功后
                                         //标识已保存过
                                         $('#hidIsSub').val("1");
                                         //清空处理记录主键
                                         $('#hidHistoryLogID').val("");
                                         //保存不允许刷新页面
                                         //window.location = "NoDealerOrderEdit.aspx?TaskID=" + "<%=TaskID %>";
                                         //刷新历史记录
                                         bindHistory();
                                     });
                                 }
                                 else if ($("#inputSaveOrSub").val() == "sub") {
                                     $.jPopMsgLayer("提交成功！", function () {
                                         closePageExecOpenerSearch();
                                     });
                                 }
                             }
                             else if (data.indexOf("InterfaceErr") != -1) {
                                 $.jAlert("提交成功！但调用易湃接口出错！【" + data.split('|')[1] + "】", function () {
                                     closePageExecOpenerSearch();
                                 });
                             }
                             else {
                                 $.jAlert(data);
                             }

                             $("#btnSave").attr("disabled", "");
                             $("#btnSumbit").attr("disabled", "");
                             $("#imgLoadingPop").hide();

                         });
        }
    </script>
    <script type="text/javascript">
        function divShowHideEvent(divId, obj) {
            if ($(obj).attr("class") == "toggle") {
                $("#" + divId).show("slow");
                $(obj).attr("class", "toggle hide");
            }
            else {
                $("#" + divId).hide("slow");
                $(obj).attr("class", "toggle");
            }
        }

        //add by qizq 2013-1-6呼出电话回调函数
        ADTTool.onInitiated = function (data) {
            var onInitiatedTime = new Date();
            $('#hidonInitiatedTime').val(onInitiatedTime.getTime());

            //add by lihf 2013-8-19呼出电话记录CallRecord_ORIG_Business            
            var pody = { Action: "InsertCallRecordORIGBusiness",
                CallID: data.CallID,
                TaskID: escape('<%=TaskID%>')
            };
            //插入中间表记录
            $.post("../../AjaxServers/TaskManager/NoDealerOrder/NoDealerCallOutDeal.ashx", pody, function (data) {
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

        //add by qizq 2013-1-5电话录音接通后回调函数
        ADTTool.Established = function (data) {
            //通话中状态标示，1表示通话中
            $('#hidIsCalling').val("1");
            //清空保存过标识
            $('#hidIsSub').val("");
            //清空处理记录主键
            $('#hidHistoryLogID').val("");
            //清空录音主键
            $('#hidCallID').val("");
            //通话中不容许提交
            $("#btnSubmit").attr("disabled", "disabled");



            var EstablishedTime = new Date();
            //坐席摘机到客户接通电话为客户振铃时间
            var timespan = parseInt((EstablishedTime.getTime() - parseInt($('#hidNetworkReachedTime').val())) / 1000);


            $.post('../../AjaxServers/TaskManager/NoDealerOrder/NoDealerCallOutDeal.ashx', {
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
                CustID: escape(""),
                CustName: escape($("[id$='txtCustName']").val()),
                Contact: escape($("[id$='txtCustName']").val()),
                TaskID: escape('<%=TaskID%>'),
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
        //add by qizq 2013-1-5电话挂断后回调函数
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

                //电话挂断后提交按钮恢复可用
                $("#btnSubmit").attr("disabled", "");
                //判断是否有录音主键，如果不存在说明页面已刷新，或通话开始回调函数执行不成功
                //if ($('#hidCallID').val() == "") {

                //}
                //有录音主键
                //else {


                //更新宇高录音接口 
                var taskid = '<%=TaskID%>';
                //var jsonstr = '{"CustID":' + taskid + ',"CallStatus":"呼出","TaskTypeID":"2"}';
                //var Result = window.external.MethodScript('/recordcontrol/updaterecorddatabyid?Refid=' + escape(data.RecordID) + '&data=' + escape(jsonstr));
                $.post('../../AjaxServers/TaskManager/NoDealerOrder/NoDealerCallOutDeal.ashx', {
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
                    CustID: escape(""),
                    CustName: escape($("[id$='txtCustName']").val()),
                    Contact: escape($("[id$='txtCustName']").val()),
                    TaskID: escape('<%=TaskID%>'),
                    CallRecordID: escape($('#hidCallID').val()),
                    //是否保存过，保存过不插入处理记录
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
                    $('#hidHistoryLogID').val(jsonData.recordid);
                    //刷新历史记录
                    bindHistory();
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
    <%-- 是否已保存，已保存为1，在挂断回调函数要查看此状态，从而判断是否插入历史处理记录--%>
    <input type="hidden" id="hidIsSub" />
    <div class="w980">
        <div class="taskT">
            订单信息<span></span></div>
        <div class="baseInfo">
            <div class="title ft16">
                基本信息<a href="javascript:void(0)" onclick="divShowHideEvent('divBaseInfo',this)" class="toggle hide"></a></div>
            <div id="divBaseInfo">
                <%--客户基本信息控件放在此处--%>
                <uc2:EditCustBaseInfo ID="EditCustBaseInfo1" runat="server" />
                <uc3:BuyCarInfo ID="BuyCarInfo1" runat="server" />
            </div>
            <div class="title contact">
                联系记录<a href="javascript:void(0)" onclick="divShowHideEvent('divContactRecord',this)"
                    class="toggle hide"></a></div>
            <div id="divContactRecord">
                <uc1:UCOrderConsult ID="UCOrderConsult1" runat="server" />
            </div>
            <div class="btn">
                <img id="imgLoadingPop" src="../../Images/blue-loading.gif" style="display: none;" />
                <input type="button" name="" id="btnDelete" value="删  除" />&nbsp;&nbsp;
                <input type="button" name="" id="btnSave" value="保  存" />&nbsp;&nbsp;
                <input type="button" name="" id="btnSubmit" value="提  交" />&nbsp;&nbsp;
            </div>
            <div class="title contact">
                历史记录<a href="javascript:void(0)" id="divHis" onclick="divShowHideEvent('divHandlerHistory',this)"
                    class="toggle hide"></a></div>
            <div id="divHandlerHistory" class="tableList clearfix tableList2">
            </div>
        </div>
    </div>
    <%--保存还是提交(保存：save  提交: sub)--%>
    <input type="hidden" id="inputSaveOrSub" value="" />
    <%--原因iD--%>
    <input type="hidden" id="hidReasonId" value="" />
    <%--原因备注--%>
    <input type="hidden" id="hidNoDealerReason" value="" />
    <input type="hidden" id="hidSendName" value="" />
    <input type="hidden" id="hidSendSex" value="" />
    </form>
</body>
</html>
