<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="YTGActivityTaskDeal.aspx.cs"
    Inherits="BitAuto.ISDC.CC2012.Web.YTGActivityTask.YTGActivityTaskDeal" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <meta http-equiv="X-UA-Compatible" content="IE=EmulateIE7" />
    <meta http-equiv="Expires" content="0">
    <meta http-equiv="Cache-Control" content="no-cache">
    <meta http-equiv="Pragma" content="no-cache">
    <title>邀约处理页</title>
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
        document.writeln("<s" + "cript type='text/javascript' src='/Js/config.js?r=" + Math.random() + "'></scr" + "ipt>");
    </script>
    <link href="../../Css/jquery.autocomplete.css" rel="stylesheet" type="text/css" />
    <script src="../../Js/jquery.autocomplete.min.js" type="text/javascript"></script>
    <script type="text/javascript">
        loadJS("common");
        loadJS("controlParams");
        loadJS("CTITool");
        loadJS("ucCommon");
    </script>
    <script type="text/javascript">
        $(function () {
            var sexValue = "<%=sexValue%>";
            var isSuccess = "<%=isSuccess%>";
            var isConnected = "<%=isConnected%>";

            if (sexValue == "1") {
                $("#rdoMale").attr("checked", true);
                $("#rdoFemale").attr("checked", false);
            }
            else if (sexValue == "2") {
                $("#rdoMale").attr("checked", false);
                $("#rdoFemale").attr("checked", true);
            }
            else {
                $("#rdoMale").attr("checked", false);
                $("#rdoFemale").attr("checked", false);
            }

            if (isSuccess == "1") {
                $("#rdoSuccess").attr("checked", true);
                $("#rdoFail").attr("checked", false);
            }
            else if (isSuccess == "0") {
                $("#rdoSuccess").attr("checked", false);
                $("#rdoFail").attr("checked", true);
            }
            else {
                $("#rdoSuccess").attr("checked", false);
                $("#rdoFail").attr("checked", false);
            }

            if (isConnected == "1") {
                $("#rdoConnected").attr("checked", true);
                $("#rdoNotConnected").attr("checked", false);
            }
            else if (isConnected == "0") {
                $("#rdoConnected").attr("checked", false);
                $("#rdoNotConnected").attr("checked", true);
            }
            else {
                $("#rdoConnected").attr("checked", false);
                $("#rdoNotConnected").attr("checked", false);
            }

            //如果失败，显示失败原因
            if ($("input[id$='rdoFail']").attr("checked")) {
                $("#li_fail").css("display", "");
                $("#spCar").css("display", "none");
            }
            else {
                $("#li_fail").css("display", "none");
            }
            //加载电话历史记录
            //  GetCallRecordORGIHistory();
            //绑定任务处理历史
            BindHistory();
            //绑定话务记录
            BindCallReocrd();

            //绑定下单地区信息
            BindProvince('selXiaDanArea_Province');
            $("#selXiaDanArea_Province option[value='<%=provinceID%>']").attr("selected", true);
            BindCity('selXiaDanArea_Province', 'selXiaDanArea_City');
            $("#selXiaDanArea_City option[value='<%=cityID%>']").attr("selected", true);

            //绑定试驾地区信息
            BindProvince('selShiJiaArea_Province');
            $("#selShiJiaArea_Province option[value='<%=testDriveProvinceID%>']").attr("selected", true);
            BindCity('selShiJiaArea_Province', 'selShiJiaArea_City');
            $("#selShiJiaArea_City option[value='<%=testDriveCityID%>']").attr("selected", true);

        });

        var CustBaseInfoHelper = (function () {
            var triggerProvince = function () {//选中省份
                BindCity('selShiJiaArea_Province', 'selShiJiaArea_City');
            }
            return {
                TriggerProvince: triggerProvince
            }
        })();

        function OpenDemandID(url) {
            url += "&R=" + Math.random();
            try {
                var dd = window.external.MethodScript('/browsercontrol/newpage?url=' + escape(url));
            }
            catch (e) {
                window.open(url);
            }
        }
        //页面交互使失败原因隐藏显示
        function IsSuccessChange() {
            if ($("input[id$='rdoSuccess']").attr("checked")) {
                $("#li_fail").css("display", "none");
                $("select[id$='<%=selFailReason.ClientID%>']").val("-2");
            }
            else if ($("input[id$='rdoFail']").attr("checked")) {
                $("#li_fail").css("display", "");
            }
        }
        //隐藏/显示div容器
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
        //绑定话务记录
        function BindCallReocrd() {
            LoadingAnimation("divCallRecordList");
            $("#divCallRecordList").load("../YTGActivityTask/AjaxServers/YTGActivityTaskCallRecordList.aspx", { TaskID: '<%=TaskID %>', r: Math.random() });
        }
        //绑定历史记录
        function BindHistory() {
            LoadingAnimation("divTaskLog");
            $("#divTaskLog").load("../YTGActivityTask/AjaxServers/YTGActivityTaskOperationLog.aspx", { TaskID: '<%=TaskID %>', r: Math.random() });
        }

        //--------------------------------------------------------------------------------------------------------------------------------
        $(function () {
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
                    SCID: escape('310')
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

                //通话中状态标示，1表示通话中
                $('#hidIsCalling').val("1");
                $('#hidCallID').val("");

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
                    SCID: escape('310'),
                    CustName: '<%=BitAuto.ISDC.CC2012.BLL.Util.EscapeString(model.UserName)%>'
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
                                SCID: escape('310'),
                                CallID: escape(callid),
                                SEX: escape('<%=sexValue%>'),
                                UserName: escape('<%=BitAuto.ISDC.CC2012.BLL.Util.EscapeString(model.UserName)%>'),
                                TaskID: escape('<%=TaskID%>'),
                                MobilePhone: escape('<%=model.Tel%>')
                            },
                            function (data) {
                            });

                        }
                    });
                }
            }
        });
    </script>
</head>
<body>
    <form id="form1" runat="server">
    <%-- 记录播出电话时间--%>
    <input type="hidden" id="hidonInitiatedTime" />
    <%-- 记录电话播出到坐席接起电话时间间隔秒数--%>
    <input type="hidden" id="hidTimeSpan" />
    <%-- 记录坐席接起电话时间--%>
    <input type="hidden" id="hidNetworkReachedTime" />
    <%-- 是否在通话中，通话中为1--%>
    <input type="hidden" id="hidIsCalling" />
    <%-- 本地录音表主键--%>
    <input type="hidden" id="hidCallID" />
    <input type="hidden" id="hidCallPhone" value="" />
    <input type="hidden" id="hidReleaseCount" value="0" />
    <div class="w980">
        <div class="taskT">
            邀约处理<span></span>
        </div>
        <div class="baseInfo">
            <div class="title contact" style="clear: both;">
                基本信息<a href="javascript:void(0)" onclick="divShowHideEvent('divContactRecord',this)"
                    class="toggle hide"></a>
            </div>
            <div id="divContactRecord">
                <ul class="clearfix ">
                    <li>
                        <label>
                            项目名称：</label>
                        <span id="spanProjectName">
                            <%=GetProjectName()%></span> </li>
                    <li>
                        <label>
                            任务ID：</label>
                        <span id="spanTaskID">
                            <%=TaskID%></span> </li>
                    <li>
                        <label>
                            <span class="redColor">*</span>姓名：</label>
                        <span id="spanUserName">
                            <input type="text" id="txtUserName" class="w255" style="width: 251px;" value="<%=model.UserName%>" />
                        </span></li>
                    <li>
                        <label>
                            <span class="redColor">*</span>性别：</label>
                        <label style="float: none; cursor: pointer; font-weight: normal" for="rdoMale">
                            <input type="radio" name="rdoGender" id="rdoMale" value="1" />
                            先生
                        </label>
                        <label style="float: none; cursor: pointer; font-weight: normal" for="rdoFemale">
                            <input type="radio" name="rdoGender" id="rdoFemale" style="margin-left: 50px;" value="2" />
                            女士
                        </label>
                    </li>
                    <li>
                        <label>
                            电话：</label>
                        <span id="spanTel">
                            <%=model.Tel%></span>&nbsp;<img id="imgTel" alt="打电话" onclick="功能废弃"
                                style="cursor: pointer" src="../../../Images/phone.gif" border="0" />
                    </li>
                    <li>
                        <label>
                            下单地区：</label>
                        <span>
                            <select id="selXiaDanArea_Province" class="w255" style="width: 126px;" disabled="disabled">
                            </select>
                            <select id="selXiaDanArea_City" class="w255" style="width: 125px;" disabled="disabled">
                            </select>
                        </span></li>
                    <li>
                        <label>
                            关联活动主题：</label>
                        <span><a href="javascript:void(0)" onclick="javascript:ViewActivityInfo()">
                            <%=GetActivityName()%>
                        </a></span></li>
                    <li>
                        <label>
                            下单日期：</label>
                        <span>
                            <%if (model.ValueOrDefault_OrderCreateTime > (new DateTime(1900, 01, 01)))
                              {%>
                            <%=Convert.ToDateTime(model.ValueOrDefault_OrderCreateTime).ToString("yyyy-MM-dd")%>
                            <%} %>
                        </span></li>
                    <li>
                        <label>
                            下单车型：</label>
                        <span>
                            <%=GetOrderCarSerialName() %></span> </li>
                    <li>
                        <label>
                            <span class="redColor">*</span>试驾地区：</label>
                        <span>
                            <select id="selShiJiaArea_Province" class="w255" style="width: 126px;" onchange="javascript:CustBaseInfoHelper.TriggerProvince()">
                            </select>
                            <select id="selShiJiaArea_City" class="w255" style="width: 125px;">
                            </select>
                        </span></li>
                    <li>
                        <label>
                            <span class="redColor" id="spCar">*</span>意向车型：</label>
                        <span>
                            <select id="selYiXiangCheXing" runat="server" class="w255">
                            </select>
                        </span></li>
                    <li>
                        <label>
                            <span class="redColor" id="Span4">*</span>预计购车时间：</label>
                        <span>
                            <select id="selPredictBuyTime" runat="server" class="w255">
                            </select>
                        </span></li>
                    <li>
                        <label>
                            <span class="redColor">*</span>是否成功：</label>
                        <label style="float: none; cursor: pointer; font-weight: normal" for="rdoSuccess">
                            <input type="radio" name="IsSuccess" id="rdoSuccess" onclick="IsSuccessChange()"
                                value="1" />
                            成功
                        </label>
                        <label style="float: none; cursor: pointer; font-weight: normal" for="rdoFail">
                            <input type="radio" name="IsSuccess" id="rdoFail" onclick="IsSuccessChange()" style="margin-left: 50px;"
                                value="0" />
                            失败
                        </label>
                    </li>
                    <li>
                        <label>
                            <span class="redColor">*</span>是否接通：</label>
                        <label style="float: none; cursor: pointer; font-weight: normal" for="rdoConnected">
                            <input type="radio" name="IsConnected" id="rdoConnected" value="1" />
                            是
                        </label>
                        <label style="float: none; cursor: pointer; font-weight: normal" for="rdoNotConnected">
                            <input type="radio" name="IsConnected" id="rdoNotConnected" style="margin-left: 50px;"
                                value="0" />
                            否
                        </label>
                    </li>
                    <li id="li_fail" style="display: none">
                        <label>
                            <span class="redColor">*</span>失败原因：
                        </label>
                        <select id="selFailReason" class="w255" style="width: 255px;" runat="server">
                        </select>
                    </li>
                    <li class="gdjl">
                        <label>
                            备注：</label>
                        <span>
                            <textarea id="Remark" style="width: 697px; height: 70px" name="Remark" runat="server"></textarea>
                        </span></li>
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
    <script type="text/javascript">
    function ViewActivityInfo() {
            $.openPopupLayer({
                name: "ViewActivityInfoAjaxPopup",
                parameters: {},
                url: "/YTGActivityTask/AjaxServers/YTGActivityTaskActivityInfo.aspx?ActivityID=" + <%=model.ActivityID%> + "&r=" + Math.random()
            });

        }
        function SaveInfo() {
            if (1==1) {
                //姓名
                var txtUserName = $("#txtUserName").val();
                //性别
                var rdoGender = $("[name$='rdoGender']").map(function () {
                    if ($(this).attr("checked")) {
                        return $(this).val();
                    }
                }).get().join(',');
                //试驾省份
                var selShiJiaArea_Province = $("select[id$='selShiJiaArea_Province']").val();
                //试驾城市
                var selShiJiaArea_City = $("select[id$='selShiJiaArea_City']").val();
                //意向车型
                var selYiXiangCheXing = $("select[id$='selYiXiangCheXing']").val();
                //预计购车时间
                var selPredictBuyTime = $("select[id$='selPredictBuyTime']").val();
                //是否成功
                var IsSuccess = $("[name$='IsSuccess']").map(function () {
                    if ($(this).attr("checked")) {
                        return $(this).val();
                    }
                }).get().join(',');
                //是否接通
                var IsConnected = $("[name$='IsConnected']").map(function () {
                    if ($(this).attr("checked")) {
                        return $(this).val();
                    }
                }).get().join(',');
                //失败原因
                var selFailReson = $("select[id$='selFailReason']").val();
                if (IsSuccess == "1") {
                    selFailReson = "-2";
                }
                //备注
                var Remark = $("#<%=Remark.ClientID%>").val();
                $.post("../YTGActivityTask/AjaxServers/Handler.ashx", {
                    Action: escape("saveYTGTaskInfo"),
                    TaskID: escape('<%=TaskID%>'),
                    UserName: escape(txtUserName),
                    Sex: escape(rdoGender),
                    TestDriveProvinceID: escape(selShiJiaArea_Province),
                    TestDriveCityID: escape(selShiJiaArea_City),
                    DCarSerialID: escape(selYiXiangCheXing),
                    PBuyCarTime: escape(selPredictBuyTime),
                    IsSuccess: escape(IsSuccess),
                    IsJT: escape(IsConnected),
                    FailReason: escape(selFailReson),
                    Remark: escape(Remark),
                    r: Math.random()
                },
            function (data) {
                var jsonData = $.evalJSON(data);
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
        }

        function SubmitInfo() {
            //验证数据
            if (DataCheck()) {
                //姓名
                var txtUserName = $("#txtUserName").val();
                //性别
                var rdoGender = $("[name$='rdoGender']").map(function () {
                    if ($(this).attr("checked")) {
                        return $(this).val();
                    }
                }).get().join(',');
                //试驾省份
                var selShiJiaArea_Province = $("select[id$='selShiJiaArea_Province']").val();
                //试驾城市
                var selShiJiaArea_City = $("select[id$='selShiJiaArea_City']").val();
                //意向车型
                var selYiXiangCheXing = $("select[id$='selYiXiangCheXing']").val();
                //预计购车时间
                var selPredictBuyTime = $("select[id$='selPredictBuyTime']").val();
                //是否成功
                var IsSuccess = $("[name$='IsSuccess']").map(function () {
                    if ($(this).attr("checked")) {
                        return $(this).val();
                    }
                }).get().join(',');
                //是否接通
                var IsConnected = $("[name$='IsConnected']").map(function () {
                    if ($(this).attr("checked")) {
                        return $(this).val();
                    }
                }).get().join(',');
                //失败原因
                var selFailReson = $("select[id$='selFailReason']").val();
                if (IsSuccess == "1") {
                    selFailReson = "-2";
                }
                //备注
                var Remark = $("#<%=Remark.ClientID%>").val();

                $.blockUI({ message: "正在提交中，请等待..." });
                $.post("../YTGActivityTask/AjaxServers/Handler.ashx", {
                    Action: escape("submitytgtaskinfo"),
                    TaskID: escape('<%=TaskID%>'),
                    UserName: escape(txtUserName),
                    Sex: escape(rdoGender),
                    TestDriveProvinceID: escape(selShiJiaArea_Province),
                    TestDriveCityID: escape(selShiJiaArea_City),
                    DCarSerialID: escape(selYiXiangCheXing),
                    PBuyCarTime: escape(selPredictBuyTime),
                    IsSuccess: escape(IsSuccess),
                    IsJT: escape(IsConnected),
                    FailReason: escape(selFailReson),
                    Remark: escape(Remark),
                    r: Math.random()
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
            var msg = "";
            var isTrue = true;
            //姓名
            var txtUserName = $("#txtUserName").val();
            if ($.trim(txtUserName) == "") {
                isTrue = false;
                msg += "请填写姓名信息<br/>";
            }
            if ($.trim(txtUserName).length > 20) {
                isTrue = false;
                msg += "姓名超长：填写的姓名长度不能多于20个字符<br/>";
            }
            //性别
            var rdoGender = $("[name$='rdoGender']").map(function () {
                if ($(this).attr("checked")) {
                    return $(this).val();
                }
            }).get().join(',');
            if ($.trim(rdoGender) == "") {
                isTrue = false;
                msg += "请选择性别<br/>";
            }
            //试驾省份
            var selShiJiaArea_Province = $("select[id$='selShiJiaArea_Province']").val();
            if ($.trim(selShiJiaArea_Province) == "-1") {
                isTrue = false;
                msg += "请选择试驾地区的省/直辖市<br/>";
            }
            //试驾城市
            var selShiJiaArea_City = $("select[id$='selShiJiaArea_City']").val();
            if ($("select[id$='selShiJiaArea_City'] option").length>1 && $.trim(selShiJiaArea_City) == "-1") {
                isTrue = false;
                msg += "请选择试驾地区的城市<br/>";
            }
            //意向车型
            var selYiXiangCheXing = $("select[id$='selYiXiangCheXing']").val();
            if ($.trim(selYiXiangCheXing) == "-2") {
                isTrue = false;
                msg += "请选择意向车型<br/>";
            }
            //预计购车时间
            var selPredictBuyTime = $("select[id$='selPredictBuyTime']").val();
            if ($.trim(selPredictBuyTime) == "-2") {
                isTrue = false;
                msg += "请选择预计购车时间<br/>";
            }
            //是否成功
            var IsSuccess = $("[name$='IsSuccess']").map(function () {
                if ($(this).attr("checked")) {
                    return $(this).val();
                }
            }).get().join(',');
            if ($.trim(IsSuccess)=="") {
                isTrue = false;
                msg += "请选择是否成功<br/>";
            }
            //是否接通
            var IsConnected = $("[name$='IsConnected']").map(function () {
                if ($(this).attr("checked")) {
                    return $(this).val();
                }
            }).get().join(',');
            if ($.trim(IsConnected) == "") {
                msg += "请选择是否接通<br/>";
                isTrue = false;
            }
            //失败原因
            var selFailReson = $("select[id$='selFailReason']").val();
            if (IsSuccess == "0") {
                if ($.trim(selFailReson) == "-2") {
                    msg += "请选择失败原因<br/>";
                    isTrue = false;
                }
            }
            //备注
            var Remark = $("#<%=Remark.ClientID%>").val();
            if (Len(Remark) > 200) {
                msg += "备注超长：填写的备注信息长度不能多于200个字符<br/>";
                isTrue = false;
            }

            if (msg != "") {
                $.jAlert(msg);
            }
            return isTrue;
        }
    </script>
    </form>
</body>
</html>
