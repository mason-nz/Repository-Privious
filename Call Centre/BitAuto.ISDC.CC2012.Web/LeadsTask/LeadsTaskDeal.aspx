<%@ Page Title="" Language="C#" AutoEventWireup="true" CodeBehind="LeadsTaskDeal.aspx.cs"
    Inherits="BitAuto.ISDC.CC2012.Web.LeadsTask.LeadsTaskDeal" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <meta http-equiv="X-UA-Compatible" content="IE=EmulateIE7" />
    <meta http-equiv="Expires" content="0">
    <meta http-equiv="Cache-Control" content="no-cache">
    <meta http-equiv="Pragma" content="no-cache">
    <title>线索处理</title>
    <link href="../css/base.css" type="text/css" rel="stylesheet" />
    <link href="../css/style.css" type="text/css" rel="stylesheet" />
    <link href="../css/CTIPopup.css" type="text/css" rel="stylesheet" />
    <script src="../Js/jquery-1.4.4.min.js" type="text/javascript"></script>
    <script src="../Js/jquery.jmpopups-0.5.1.pack.js" charset="utf-8" type="text/javascript"></script>
    <script src="../Js/common.js" type="text/javascript"></script>
    <script src="../Js/json2.js" type="text/javascript"></script>
    <script src="../Js/Enum/Area2.js" type="text/javascript"></script>
    <script src="../Js/jquery.blockUI.js" type="text/javascript"></script>
    <script src="/Js/controlParams.js" type="text/javascript"></script>
    <script type="text/javascript" src="/CTI/CTITool.js"></script>
    <script type="text/javascript">

        function CallOut() {
            var tel = $.trim($("#txtTel").text())
            //功能废弃
        }
        //防止电话图标被点击多次
        $(document).ready(function () {
            //如果失败，显示失败原因
            if ($("input[id$='rdoFail']").attr("checked")) {
                $("#li_fail").css("display", "");
                $("#spCar").css("display", "none");
            }
            else {
                $("#li_fail").css("display", "none");
            }
            //加载电话历史记录
            GetCallRecordORGIHistory();
            //绑定任务处理历史
            BindHistory();
            //绑定话务记录
            BindCallReocrd();
            //设置下拉选样式
            SetDllDCarClass();
        });
        //页面交互使失败原因隐藏显示
        function IsSuccessChange() {
            if ($("input[id$='rdoSuccess']").attr("checked")) {
                $("#li_fail").css("display", "none");
                $("select[id$='selFailReson']").val("-2");
                $("#spCar").css("display", "");
            }
            else if ($("input[id$='rdoFail']").attr("checked")) {
                $("#li_fail").css("display", "");
                $("#spCar").css("display", "none");
            }
        }
        //设置需匹配车款样式
        function SetDllDCarClass() {
            $("select[id$='dllDCarName'] option").each(function () {
                if ($(this).val() == "0") {
                    $(this).attr("disabled", true)
                    $(this).html("<span style='text-align:center;font-weight:bold;'>" + $(this).text() + "</span>");
                }
            });
        }
        //绑定话务记录
        function BindCallReocrd() {
            LoadingAnimation("divCallRecordList");
            $("#divCallRecordList").load("../AjaxServers/LeadsTask/LeadTaskCallRecordList.aspx", { TaskID: '<%=TaskID %>', r: Math.random() });
        }
        //绑定历史记录
        function BindHistory() {
            LoadingAnimation("divTaskLog");
            $("#divTaskLog").load("../AjaxServers/LeadsTask/LeadsTaskOperationLog.aspx", { TaskID: '<%=TaskID %>', r: Math.random() });
        }
        function SaveInfo() {
            //是否成功
            var IsSuccess = $("[name$='IsSuccess']").map(function () {
                if ($(this).attr("checked")) {
                    return $(this).val();
                }
            }).get().join(',');
            //需匹配车款
            var seldllDCar = $("select[id$='dllDCarName']").val();
            var seldllDCarName = $("select[id$='dllDCarName'] option:selected").text();
            //失败原因
            var selFailReson = $("select[id$='selFailReson']").val();
            //任务ID
            var TaskID = '<%=TaskID%>';
            //备注
            var Remark = $("#Remark").val();
            $.post("../AjaxServers/LeadsTask/LeadsTaskDeal.ashx", {
                Action: escape("saveinfo"),
                ProjectID: escape('<%=model.ProjectID%>'),
                TaskID: escape(TaskID),
                IsSuccess: escape(IsSuccess),
                FailReson: escape(selFailReson),
                Remark: escape(Remark),
                GuidStr: escape('<%=model.RelationID.ToString()%>'),
                MemberCode: escape('<%=model.DealerID%>'),
                UserName: escape('<%=BitAuto.ISDC.CC2012.BLL.Util.EscapeString(model.UserName)%>'),
                MobilePhone: escape('<%=model.Tel%>'),
                DCarID: escape(seldllDCar),
                DCarName: escape(seldllDCarName),
                CityID: escape('<%=model.CityID.ToString()%>'),
                DemandID: escape('<%=model.DemandID%>')
            },
            function (data) {
                var jsonData = $.evalJSON(data);
                //不成功提示错误，成功把录音主键保存在隐藏域里面
                if (jsonData.Result == false) {
                    $.jAlert(jsonData.Msg);
                }
                else {
                    BindHistory();
                    //$.jAlert("保存成功！");
                    $.jPopMsgLayer("保存成功！");
                }
            });
        }
        function SubmitInfo() {
            //验证数据
            if (DataCheck()) {
                //是否成功
                var IsSuccess = $("[name$='IsSuccess']").map(function () {
                    if ($(this).attr("checked")) {
                        return $(this).val();
                    }
                }).get().join(',');
                //需匹配车款
                var seldllDCar = $("select[id$='dllDCarName']").val();
                var seldllDCarName = $("select[id$='dllDCarName']").find("option:selected").text();
                //失败原因
                var selFailReson = $("select[id$='selFailReson']").val();
                //任务ID
                var TaskID = '<%=TaskID%>';
                //备注
                var Remark = $("#Remark").val();
                //促销信息版本号
                //var DemandVersion=$("[id$='hidDemandVersion']").val();

                $.blockUI({ message: "正在提交中，请等待..." });
                $.post("../AjaxServers/LeadsTask/LeadsTaskDeal.ashx", {
                    Action: escape("subinfo"),
                    ProjectID: escape('<%=model.ProjectID%>'),
                    TaskID: escape(TaskID),
                    IsSuccess: escape(IsSuccess),
                    FailReson: escape(selFailReson),
                    Remark: escape(Remark),
                    GuidStr: escape('<%=model.RelationID.ToString()%>'),
                    MemberCode: escape('<%=model.DealerID%>'),
                    UserName: escape('<%=BitAuto.ISDC.CC2012.BLL.Util.EscapeString(model.UserName)%>'),
                    MobilePhone: escape('<%=model.Tel%>'),
                    DCarID: escape(seldllDCar),
                    DCarName: escape(seldllDCarName),
                    CityID: escape('<%=model.CityID.ToString()%>'),
                    DemandID: escape('<%=model.DemandID%>')
                    //DemandVersion:escape(DemandVersion)
                },
            function (data) {
                $.unblockUI();
                var jsonData = $.evalJSON(data);
                //不成功提示错误
                if (jsonData.Result == false) {
                    $.jAlert(jsonData.Msg);
                }
                else {
                    //                    $.jAlert("提交成功！", function () {
                    //                        closePageExecOpenerSearch();
                    //                    });
                    $.jPopMsgLayer("提交成功！", function () {
                        closePageExecOpenerSearch();
                    });

                }
            });
            }
        }

        function DataCheck() {

            //是否成功
            var IsSuccess = $("[name$='IsSuccess']").map(function () {
                if ($(this).attr("checked")) {
                    return $(this).val();
                }
            }).get().join(',');
            //需匹配车款
            var seldllDCarName = $("select[id$='dllDCarName']").val();
            //失败原因
            var selFailReson = $("select[id$='selFailReson']").val();
            //备注
            var Remark = $("#Remark").val();
            if (Len(IsSuccess) == 0) {
                $.jAlert("请选择是否成功！");
                return false;
            }
            if (IsSuccess == "1") {
                if (seldllDCarName == "-2") {
                    $.jAlert("请选择需匹配车款！");
                    return false;
                }
            }
            if (IsSuccess == "0") {
                if (selFailReson == "-2") {
                    $.jAlert("请选择失败原因！");
                    return false;
                }
            }
            if (Len(Remark) > 200) {
                $.jAlert("备注超长！");
                return false;
            }
            return true;
        }
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
        function OpenDemandID(url) {
            url += "&R=" + Math.random();
            try {
                var dd = window.external.MethodScript('/browsercontrol/newpage?url=' + escape(url));
            }
            catch (e) {
                window.open(url);
            }
        }
        function GetCallRecordORGIHistory() {
            //先清空历史记录图标
            $("a[chistory='1']").each(function () {
                $(this).remove();
            });

            var tels = "<%=tel%>";
            tels = tels.replace(/\-/g, "");
            if (tels == "") {
                return;
            }

            var msg = "";
            AjaxPost('/AjaxServers/OtherTask/OtherTaskDeal.ashx', { Action: "GetCallRecordORGIHistory", TelePhones: tels, TaskID: '<%=TaskID%>' }, null, function (data) {
                var jsonDatas = $.evalJSON(data);
                $.each(jsonDatas, function (i, jsonData) {
                    if (jsonData.result != "1" && jsonData.result != "undefined") {
                        //等于1不显示图标
                        if (jsonData.result == "2") {
                            //显示个人用户查看页
                            var custid = jsonData.CustID;
                            msg = "&nbsp;<a chistory='1' href='../TaskManager/CustInformation.aspx?CustID=" + custid + "' title='历史记录' target='_blank' class='linkBlue'><img alt='历史记录' style='vertical-align:middle;' src='/images/history.png' border='0' /></a>";
                            $("#imgTel").after(msg);
                        }
                        else if (jsonData.result == "3") {
                            //显示个人用户列表
                            var custTel = jsonData.Tel;
                            msg = "&nbsp;<a chistory='1' href='../CustBaseInfo/List.aspx?CustTel=" + custTel + "' title='历史记录' target='_blank' class='linkBlue'><img alt='历史记录' style='vertical-align:middle;' src='/images/history.png' border='0' /></a>";
                            $("#imgTel").after(msg);
                        }
                    }
                });
            });

        }

        /*
        //西门子外呼
        */
        ADTTool.onInitiated = function (data) {
            //电话未接通情况，业务记录录音主键初始化为-2，表示没有录音
            //否则会记录为上通电话录音主键
            $('#hidCallID').val("-2");
            $("#hidReleaseCount").val("0");
            var onInitiatedTime = new Date();
            $('#hidonInitiatedTime').val(onInitiatedTime.getTime());

            //add by lihf 2013-8-19呼出电话记录CallRecord_ORIG_Business            
            var pody = { Action: "InsertCallRecordORIGBusiness",
                CallID: data.CallID,
                TaskID: escape('<%=TaskID%>'),
                BGID: escape('20'),
                SCID: escape('160')
            };
            //插入中间表记录
            $.post("/AjaxServers/LeadsTask/LeadsTaskDeal.ashx", pody, function (data) {
                callRecordID = data;
            });
        }
        //add by qizq 2013-1-6呼出电话坐席摘机回调函数//拨出电话到坐席摘机为坐席振铃时间
        ADTTool.onNetworkReached = function (data) {
            $('#hidCallID').val("-2");
            $("#hidReleaseCount").val("0");
            var NetworkReachedTime = new Date();
            $('#hidNetworkReachedTime').val(NetworkReachedTime.getTime());
            var timespan = parseInt((NetworkReachedTime.getTime() - parseInt($('#hidonInitiatedTime').val())) / 1000);
            //拨出电话到坐席摘机为坐席振铃时间
            $('#hidTimeSpan').val(timespan);
        }
        //add by qizq 2013-1-4电话录音接通后回调函数//坐席摘机到客户接通电话为客户振铃时间
        ADTTool.Established = function (data) {
            $("#hidReleaseCount").val("0");
            //将被叫号码放在隐藏域
            //$("#hidCallPhone").val(data.CallerNum);

            //            //接通后让提交按钮不可用
            //            $('#btnSave').attr("disabled", "disabled");

            //通话中状态标示，1表示通话中
            $('#hidIsCalling').val("1");
            $('#hidCallID').val("");

            //            //取联系人
            //            var Contract = "";
            //            if ($('#TelName') != undefined) {
            //                Contract = $('#TelName').val();
            //            }

            var EstablishedTime = new Date();
            //坐席摘机到客户接通电话为客户振铃时间
            var timespan = parseInt((EstablishedTime.getTime() - parseInt($('#hidNetworkReachedTime').val())) / 1000);
            $.post('/AjaxServers/LeadsTask/LeadsTaskDeal.ashx', {
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
                TaskID: escape('<%=TaskID%>'),
                NetworkRTimeSpan: escape($('#hidTimeSpan').val()),
                EstablishTimeSpan: escape(timespan),
                //取接通开始时间

                EstablishBeginTime: data.CurrentDate,
                BGID: escape('20'),
                SCID: escape('160'),
                CustName: '<%=BitAuto.ISDC.CC2012.BLL.Util.EscapeString(model.UserName)%>'
            },
            function (data) {
                var jsonData = $.evalJSON(data);
                //不成功提示错误，成功把录音主键保存在隐藏域里面
                if (jsonData.success == 'no') {
                    $.jAlert(jsonData.msg);
                }
                else {
                    $('#hidCallID').val(jsonData.recordid);
                    if (jsonData.recordid != "") {
                        $("#imgNodisturbTel").attr("src", "/Images/nodisturb.png").attr("disabled", false);
                    }
                }
            });
        }
        ADTTool.onDisconnected = function (data) {
            var callid = "";
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
            //判断是否是通话中的挂断
            if (data.IsEstablished == "True") {

                callid = data.CallID;
                //挂断后让提交按钮可用
                $('#btnSub').removeAttr("disabled");
                //通话中状态标示，0表示不是通话中
                //$('#hidIsCalling').val("0");
                //判断是否有录音主键，如果不存在说明页面已刷新，或通话开始回调函数执行不成功
                //if ($('#hidCallID').val() == "") {
                //}
                //有录音主键
                //else {
                //更新宇高录音接口
                //                        var jsonstr = '{"CustID":' + CrmCustID + ',"CallStatus":"呼出","TaskTypeID":"1"}';
                //                        var Result = window.external.MethodScript('/recordcontrol/updaterecorddatabyid?Refid=' + escape(data.RecordID) + '&data=' + escape(jsonstr));
                //
                $.post('/AjaxServers/LeadsTask/LeadsTaskDeal.ashx', {
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
                    EstablishEndTime: data.CurrentDate,
                    TaskID: escape('<%=TaskID%>')
                },
                    function (data) {
                        var jsonData = $.evalJSON(data);
                        //不成功提示错误
                        if (jsonData.success == 'no') {
                            $.jAlert(jsonData.msg);
                        }
                        else {
                            //加载录音记录
                            BindCallReocrd();

                            //保存客户
                            $.post("../AjaxServers/LeadsTask/LeadsTaskDeal.ashx", {
                                Action: escape("insertcustdata"),
                                BGID: escape('20'),
                                SCID: escape('160'),
                                CallID: escape(callid),
                                SEX: escape('<%=Sex%>'),
                                UserName: escape('<%=BitAuto.ISDC.CC2012.BLL.Util.EscapeString(model.UserName)%>'),
                                TaskID: escape('<%=TaskID%>'),
                                MobilePhone: escape('<%=tel%>')
                            },
                            function (data) {
                            });

                        }
                    });
            }

            //            //挂断后,保存个人用户
            //            if ($("#hidReleaseCount").val() == "0") {
            //                $("#hidReleaseCount").val("1");
            //                saveCustInfo();
            //            }
        }
    </script>
</head>
<body>
    <form id="form1" runat="server">
    <%-- 记录播出电话时间--%>
    <input type="hidden" id="hidonInitiatedTime" />
    <div class="w980">
        <div class="taskT">
            线索处理<span></span></div>
        <div class="baseInfo">
            <div class="title contact" style="clear: both;">
                基本信息<a href="javascript:void(0)" onclick="divShowHideEvent('divContactRecord',this)"
                    class="toggle hide"></a></div>
            <div id="divContactRecord">
                <ul class="clearfix ">
                    <li>
                        <label>
                            姓名：</label><span id="spantxtCustName"><%=model.UserName%></span></li>
                    <li>
                        <label>
                            电话：</label><span id="txtTel"><%=model.Tel%></span> &nbsp;<img id="imgTel" alt="打电话"
                                style="cursor: pointer; vertical-align: middle;" src="../../../Images/phone.gif"
                                border="0" onclick="CallOut()" />
                        <img id="imgNodisturbTel" alt="添加为免打扰电话" disabled="disabled" style="cursor: pointer;
                            vertical-align: middle;" src="../../../Images/nodisturbgray.png" border="0" onclick="javascript:NoDisturbTool.openNoDisturbPopup(this,$.trim($('#txtTel').text()),$('#hidCallID').val());" />
                    </li>
                    <li>
                        <label>
                            性别：</label><span><%if (model.Sex == 2)
                                               { 
                            %>女士
                                <%}
                                               else
                                               {%>先生<%}%></span></li>
                    <li>
                        <label>
                            地区：</label><span><%=PlaceStr %></span></li>
                    <li>
                        <label>
                            下单车款：</label><span><%=OrderCarInfo %></span></li>
                    <li>
                        <label>
                            下单经销商：</label><span><%=model.DealerName %></span></li>
                    <li>
                        <label>
                            下单日期：</label><span><%if (model.OrderCreateTime > (new DateTime(1900, 01, 01)))
                                                 {%><%=Convert.ToDateTime(model.OrderCreateTime).ToString("yyyy-MM-dd")%><%} %></span></li>
                    <li>
                        <label>
                            关联需求：</label><span><input type="hidden" value="<%=model.DemandID %>" id="DemandID" /><a
                                href="javascript:void(0)" onclick="OpenDemandID('<%=string.Format(DemandDetailsUrl,model.DemandID)%>')"><%=model.DemandID %></a></span></li>
                    <li>
                        <label>
                            需匹配车型：</label><span><%=DCarInfo %></span></li>
                    <li>
                        <label>
                            <span class="redColor" id="spCar">*</span>需匹配车款：</label>
                        <span>
                            <select id="dllDCarName" name="CarNameId" runat="server" class="w255" style="width: 254px;">
                            </select>
                        </span></li>
                    <%--<li>
                        <label>
                            <span class="redColor">*</span>是否成功：</label><span><input type="radio" name="IsSuccess"
                                id="rdoSuccess" onchange="IsSuccessChange()" runat="server" value="1" /><em onclick="emChkIsChoose(this);IsSuccessChange();">成功</em><input
                                    type="radio" id="rdoFail" onchange="IsSuccessChange()" runat="server" name="IsSuccess"
                                    style="margin-left: 50px;" value="0" /><em onclick="emChkIsChoose(this);IsSuccessChange();">失败</em></span>
                    </li>--%>
                    <li>
                        <label>
                            <span class="redColor">*</span>是否成功：</label><label style="float: none; cursor: pointer;
                                font-weight: normal" for="rdoSuccess"><input type="radio" name="IsSuccess" id="rdoSuccess"
                                    onclick="IsSuccessChange()" runat="server" value="1" />成功</label><label style="float: none;
                                        cursor: pointer; font-weight: normal" for="rdoFail"><input type="radio" id="rdoFail"
                                            onclick="IsSuccessChange()" runat="server" name="IsSuccess" style="margin-left: 50px;"
                                            value="0" /><span>失败</span></label>
                    </li>
                    <%--<li>
                        <label>
                            <span class="redColor">*</span>是否成功：</label><span style="width: 50px; text-align: left;"
                                for="rdoSuccess"><input type="radio" name="IsSuccess" id="rdoSuccess" onclick="IsSuccessChange()"
                                    runat="server" value="1" />成功</span><span style="width: 60px; text-align: left;"
                                        for="rdoFail"><input type="radio" id="rdoFail" onclick="IsSuccessChange()" runat="server"
                                            name="IsSuccess" value="0" />失败</span> </li>--%>
                    <li id="li_fail" style="display: none">
                        <label>
                            <span class="redColor">*</span>失败原因：</label>
                        <select id="selFailReson" class="w255" style="width: 254px;" runat="server">
                        </select>
                    </li>
                    <li class="gdjl">
                        <label>
                            备注：</label><span><textarea id="Remark" style="width: 696px; height: 70px" name="Remark"
                                runat="server"></textarea></span></li>
                </ul>
            </div>
            <div class="cont_cx khxx CustInfoArea">
                <div class="title contact" style="clear: both;">
                    记录历史<a class="toggle hide" onclick="divShowHideEvent('infoBlock1',this)" href="javascript:void(0)"></a>
                </div>
                <div id="infoBlock1">
                    <ul class="infoBlock firstPart">
                        <li style="width: 900px; height: auto;">
                            <div style="">
                                &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;操作记录</div>
                            <div id="divTaskLog" class="fullRow cont_cxjg" style="margin-left: 78px;">
                            </div>
                        </li>
                        <li style="width: 900px; height: auto;">
                            <div style="">
                                &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;通话记录</div>
                            <div id="divCallRecordList" class="fullRow cont_cxjg" style="margin-left: 78px;">
                            </div>
                        </li>
                    </ul>
                </div>
            </div>
            <br />
            <div class="btn" style="clear: both; padding-top: 30px">
                <input type="button" name="" id="btnSave" onclick="SaveInfo()" value="保 存" />&nbsp;&nbsp;
                <input type="button" name="" id="btnSubmit" onclick="SubmitInfo()" value="提 交" class="forwordbtn" />&nbsp;&nbsp;
            </div>
            <br />
        </div>
    </div>
    <%-- 记录电话播出到坐席接起电话时间间隔秒数--%>
    <input type="hidden" id="hidTimeSpan" />
    <%-- 记录坐席接起电话时间--%>
    <input type="hidden" id="hidNetworkReachedTime" />
    <%-- 记录播出电话时间--%>
    <input type="hidden" id="Hidden1" />
    <%-- 是否在通话中，通话中为1--%>
    <input type="hidden" id="hidIsCalling" />
    <%-- 本地录音表主键--%>
    <input type="hidden" id="hidCallID" />
    <input type="hidden" id="hidCallPhone" value="" />
    <input type="hidden" id="hidReleaseCount" value="0" />
    <%-- 需求单促销信息版本号--%>
    <%--<input type="hidden" id="hidDemandVersion" runat="server" value="" />--%>
    </form>
</body>
</html>
