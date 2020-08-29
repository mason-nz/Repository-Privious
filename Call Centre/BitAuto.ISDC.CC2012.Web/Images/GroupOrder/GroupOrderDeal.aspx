<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="GroupOrderDeal.aspx.cs"
    Inherits="BitAuto.ISDC.CC2012.Web.GroupOrder.GroupOrderDeal" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <meta http-equiv="X-UA-Compatible" content="IE=EmulateIE7" />
    <meta http-equiv="Expires" content="0">
    <meta http-equiv="Cache-Control" content="no-cache">
    <meta http-equiv="Pragma" content="no-cache">
    <title>团购订单处理</title>
    <link href="../css/base.css" type="text/css" rel="stylesheet" />
    <link href="../css/style.css" type="text/css" rel="stylesheet" />
    <link href="../css/CTIPopup.css" type="text/css" rel="stylesheet" />
    <script src="../Js/jquery-1.4.4.min.js" type="text/javascript"></script>
    <script src="../Js/jquery.jmpopups-0.5.1.pack.js" charset="utf-8" type="text/javascript"></script>
    <script src="../Js/common.js" type="text/javascript"></script>
    <script src="../Js/json2.js" type="text/javascript"></script>
    <script src="../Js/Enum/Area2.js" type="text/javascript"></script>
    <script src="../Js/jquery.blockUI.js" type="text/javascript"></script>
    <script src="/CTI/CTITool.js" type="text/javascript"></script>
    <script src="/Js/controlParams.js" type="text/javascript"></script>
    <script type="text/javascript" src="/js/bit.dropdownlist.js"></script>
    <script src="/Js/My97DatePicker/WdatePicker.js" type="text/javascript"></script>
    <link href="/Js/My97DatePicker/skin/WdatePicker.css" rel="stylesheet" type="text/css" />
    <script type="text/javascript">
        $(document).ready(function () {
            //绑定任务操作历史
            if (Len($('#txtUserName').val()) > 0) {
                $('#btnRegister').hide();
            }
            //加载意向车型
            BindMyBrand();
            //如果回访选择未知,让失败理由显示
            selReturnVisit_Change();
            //绑定任务处理历史
            bindHistory();
            //隐藏历史记录
            $("#divHis").attr("class", "toggle");
            $("#divHandlerHistory").hide();

        });
        //绑定历史记录
        function bindHistory() {
            LoadingAnimation("divHandlerHistory");
            $("#divHandlerHistory").load("../AjaxServers/GroupOrder/GroupOrderTaskOperLogList.aspx", { TaskID: '<%=TaskID %>', r: Math.random() });
        }


        function SaveInfo() {
            var sex = $("[name$='sex']").map(function () {
                if ($(this).attr("checked")) {
                    return $(this).val();
                }
            }).get().join(',');
            var selReturnVisit = $("select[id$='selReturnVisit']").val();
            var selFailReson = $("select[id$='selFailReson']").val();
            var TaskID = '<%=TaskID%>';
            var HistoryLogID = $("#hidHistoryLogID").val();
            var Remark = $("#Remark").val();
            //var callRecordID = $('#hidHistoryLogID').val();
            //应保存录音主键$('#hidCallID')
            var callRecordID = $('#hidCallID').val();
            var UserName = $('#txtUserName').val();

            var WantCarMasterID = $('#dllMyBrand').val();
            var WantCarMasterName="";
            if (WantCarMasterID != "0") {
                WantCarMasterName = $("#dllMyBrand option:selected").attr("text").split(' ')[1];
            }
            var WantCarSerialID = $('#dllMySerial').val();
            var WantCarSerialName = "";
            if (WantCarSerialID != "0") {
                WantCarSerialName = $("#dllMySerial option:selected").attr("text");
            }
            var WantCarID = $('#dllMyName').val();
            var WantCarName = "";
            if (WantCarID != "0") {
                WantCarName = $("#dllMyName option:selected").attr("text");
            }
            var PlanBuyCarTime = $("select[id$='dllPlanBuyCarTime']").val();

            $.post("../AjaxServers/GroupOrder/GroupOrderDeal.ashx", {
                Action: escape("saveinfo"),
                Sex: escape(sex),
                TaskID: escape(TaskID),
                IsReturnVisit: escape(selReturnVisit),
                FailReson: escape(selFailReson),
                Remark: escape(Remark),
                CallRecordID: escape(callRecordID),
                QCTUserName: escape(UserName),
                HistoryLogID: escape(HistoryLogID),
                WantCarMasterID: escape(WantCarMasterID),
                WantCarMasterName: escape(WantCarMasterName),
                WantCarSerialID: escape(WantCarSerialID),
                WantCarSerialName: escape(WantCarSerialName),
                WantCarID: escape(WantCarID),
                WantCarName: escape(WantCarName),
                PlanBuyCarTime:escape(PlanBuyCarTime)


            },
            function (data) {

                var jsonData = $.evalJSON(data);
                //不成功提示错误，成功把录音主键保存在隐藏域里面
                if (jsonData.Result == false) {
                    $.jAlert(jsonData.Msg);
                }
                else {
                    //清空处理记录主键
                    $('#hidHistoryLogID').val("");
                    //清空录音主键
                    $('#hidCallID').val("-2");
                    bindHistory();
                    $.jAlert("保存成功！");
                }
            });
        }
        function SubmitInfo() {
            //验证数据
            if (DataCheck()) {
                var sex = $("[name$='sex']").map(function () {
                    if ($(this).attr("checked")) {
                        return $(this).val();
                    }
                }).get().join(',');
                var selReturnVisit = $("select[id$='selReturnVisit']").val();
                var selFailReson = $("select[id$='selFailReson']").val();
                var TaskID = '<%=TaskID%>';
                var HistoryLogID = $("#hidHistoryLogID").val();
                var Remark = $.trim($("#Remark").val());
                //var callRecordID = $('#hidHistoryLogID').val();
                //应保存录音主键$('#hidCallID')
                var callRecordID = $('#hidCallID').val();
                var UserName = $('#txtUserName').val();

                var WantCarMasterID = $('#dllMyBrand').val();
                var WantCarMasterName = "";
                if (WantCarMasterID != "0") {
                    WantCarMasterName = $("#dllMyBrand option:selected").attr("text").split(' ')[1];
                }
                var WantCarSerialID = $('#dllMySerial').val();
                var WantCarSerialName = "";
                if (WantCarSerialID != "0") {
                    WantCarSerialName = $("#dllMySerial option:selected").attr("text");
                }
                var WantCarID = $('#dllMyName').val();
                var WantCarName = "";
                if (WantCarID != "0") {
                    WantCarName = $("#dllMyName option:selected").attr("text");
                }
                var PlanBuyCarTime = $("select[id$='dllPlanBuyCarTime']").val();
                $.blockUI({ message: "正在提交中，请等待..." });
                $.post("../AjaxServers/GroupOrder/GroupOrderDeal.ashx", {
                    Action: escape("subinfo"),
                    Sex: escape(sex),
                    TaskID: escape(TaskID),
                    IsReturnVisit: escape(selReturnVisit),
                    FailReson: escape(selFailReson),
                    Remark: escape(Remark),
                    CallRecordID: escape(callRecordID),
                    QCTUserName: escape(UserName),
                    HistoryLogID: escape(HistoryLogID),
                    WantCarMasterID: escape(WantCarMasterID),
                    WantCarMasterName: escape(WantCarMasterName),
                    WantCarSerialID: escape(WantCarSerialID),
                    WantCarSerialName: escape(WantCarSerialName),
                    WantCarID: escape(WantCarID),
                    WantCarName: escape(WantCarName),
                    PlanBuyCarTime: escape(PlanBuyCarTime)
                },
            function (data) {
                $.unblockUI();
                var jsonData = $.evalJSON(data);
                //不成功提示错误，成功把录音主键保存在隐藏域里面
                if (jsonData.Result == false) {
                    $.jAlert(jsonData.Msg);
                }
                else {
                    //清空处理记录主键
                    $('#hidHistoryLogID').val("");
                    //清空录音主键
                    $('#hidCallID').val("-2");
                    bindHistory();
                    $.jAlert("提交成功！", function () {
                        closePageExecOpenerSearch();
                    });
                }
            });
            }
        }
        function selReturnVisit_Change() {
            if ($("select[id$='selReturnVisit']").val() == "0") {
                $('#li_fail').css("display", "");
            }
            else {
                $("select[id$='selFailReson']").val("-2")
                $('#li_fail').css("display", "none");
            }
        }
        function DataCheck() {
            var sex = $("[name$='sex']").map(function () {
                if ($(this).attr("checked")) {
                    return $(this).val();
                }
            }).get().join(',');
            var selReturnVisit = $("select[id$='selReturnVisit']").val();
            var selFailReson = $("select[id$='selFailReson']").val();
            var Remark = $("#Remark").val();
            if (Len(sex) == 0) {
                $.jAlert("性别不能为空！");
                return false;
            }
            if (selReturnVisit == "-2") {
                $.jAlert("请选择处理状态！");
                return false;
            }
            if (selReturnVisit == "0" && selFailReson == "-2") {
                $.jAlert("请选择失败理由！");
                return false;
            }
            if (Len(Remark) > 1000) {
                $.jAlert("备注超过最大长度！");
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
        //绑定品牌信息
        function BindMyBrand() {

            //目前驾驶的车
            var options4 = {
                container: { master: "dllMyBrand", serial: "dllMySerial", cartype: "dllMyName" },
                include: { serial: "1", cartype: "1" },
                datatype: 0,
                binddefvalue: { master: '<%=WantCarBrandID%>', serial: '<%=WantCarSerialID%>', cartype: '<%=WantCarID%>' }
            };
            new BindSelect(options4).BindList();


        }
        //add by qizq 2013-1-6呼出电话回调函数
        ADTTool.onInitiated = function (data) {
            var onInitiatedTime = new Date();
            $('#hidCallID').val("-2");
            $('#hidonInitiatedTime').val(onInitiatedTime.getTime());

            //add by lihf 2013-8-19呼出电话记录CallRecord_ORIG_Business            
            var pody = { Action: "InsertCallRecordORIGBusiness",
                CallID: data.CallID,
                TaskID: escape('<%=TaskID%>')
            };
            //插入中间表记录
            $.post("../AjaxServers/GroupOrder/GroupOrderCallOut.ashx", pody, function (data) {
                callRecordID = data;
            });
        }
        //add by qizq 2013-1-6呼出电话坐席摘机回调函数//拨出电话到坐席摘机为坐席振铃时间
        ADTTool.onNetworkReached = function (data) {
            $('#hidCallID').val("-2");
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
            //清空处理记录主键
            $('#hidHistoryLogID').val("");
            //清空录音主键
            $('#hidCallID').val("-2");
            //通话中不容许提交
            $("#btnSubmit").attr("disabled", "disabled");
            //通话中不容许保存
            $("#btnSave").attr("disabled", "disabled");



            var EstablishedTime = new Date();
            //坐席摘机到客户接通电话为客户振铃时间
            var timespan = parseInt((EstablishedTime.getTime() - parseInt($('#hidNetworkReachedTime').val())) / 1000);


            $.post("../AjaxServers/GroupOrder/GroupOrderCallOut.ashx", {
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
                CustName: escape($("[id$='spantxtCustName']").html()),
                Contact: escape($("[id$='spantxtCustName']").html()),
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
                $("#btnSave").attr("disabled", "");
                //更新宇高录音接口 
                var taskid = '<%=TaskID%>';
                $.post("../AjaxServers/GroupOrder/GroupOrderCallOut.ashx", {
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
                    CustName: escape($("[id$='spantxtCustName']").html()),
                    Contact: escape($("[id$='spantxtCustName']").html()),
                    TaskID: escape('<%=TaskID%>'),
                    CallRecordID: escape($('#hidCallID').val()),
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
            }
        }
    </script>
    <script type="text/javascript">
        //注册汽车通 add lxw 13.4.9
        function registerCarTong(othis) {
            var telNum = $('#txtTel').html().length;
            var telPhone = $("[id^='txtTel']").map(function () {
                return $(this).html();
            }).get().join(',');

            var msg = "";
            $("[id^='txtTel']").each(function () {
                if (!isTelOrMobile($(this).html())) {
                    msg += "手机号码 " + $(this).html() + " 格式不正确<br/>";
                }
            });

            if (msg != "") {
                $.jAlert(msg);
                return false;
            }

            if (telNum == 0) {
                $.jAlert("电话号码不能为空");
                return false;
            }
            registerByWebService(telPhone);

        }
        //调用web服务接口注册车商通
        function registerByWebService(phoneNumber) {
            $.blockUI({ message: "正在注册，请等待..." });
            var custid = $("#hdnCustID").val();
            if (custid == "") {
                custid = "<%=CustID %>";
            }
            AjaxPost("/AjaxServers/CustCategory/BuyCarInfo.ashx", { Action: "RegisterCarTong", PhoneNumber: encodeURIComponent(phoneNumber), CustID: encodeURIComponent(custid), r: Math.random() }, null,
        function (data) {
            $.unblockUI();
            var jsonData = eval("(" + data + ")");
            if (jsonData.result == 'yes') {
                $.jAlert("注册成功", function () {
                    $("#txtUserName").val(jsonData.mobile + "/" + jsonData.pwd);
                    $("#btnRegister").hide(); //当汽车通注册成功时，注册 按钮隐藏
                });
            }
            else {
                $.jAlert(jsonData.msg);
            }
        });
        }
        //弹出 新增工单 弹出层 add lxw 13.9.5
        function selectAddWorkOrderPop() {
            var custId = $("#hdnCrmCustID").val();
            $.openPopupLayer({
                name: "AddWorkOrderPop",
                url: "/WorkOrder/AddWorkOrderPop.aspx",
                parameters: {
                    PopupName: 'AddWorkOrderPop',
                    CustID: "",
                    PTID: "<%=TaskID%>"
                },
                afterClose: function (effectiveAction) {
                    if (effectiveAction) { }
                }
            });
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
    <div class="w980">
        <div class="taskT">
            订单处理<span></span></div>
        <div class="baseInfo">
            <div class="title ft16">
                基本信息<a href="javascript:void(0)" onclick="divShowHideEvent('divBaseInfo',this)" class="toggle hide"></a><a
                    id="hrefAddWorkOrder" style="float: right; margin-right: 30px; *margin-top: -30px;"
                    href="javascript:void(0);" onclick="javascript:selectAddWorkOrderPop();"> 添加工单
                </a>
            </div>
            <div id="divBaseInfo">
                <ul class="">
                    <li>
                        <label>
                            姓名：</label><span id="spantxtCustName"><%=CustName%></span></li>
                    <li>
                        <label>
                            <span class="redColor">*</span>性别：</label><span><input type="radio" name="sex" id="rdoMan"
                                runat="server" value="1" /><em onclick="emChkIsChoose(this);">先生</em><input type="radio"
                                    id="rdoWomen" runat="server" name="sex" style="margin-left: 50px;" value="2" /><em
                                        onclick="emChkIsChoose(this);">女士</em></span></li>
                    <li>
                        <label>
                            电话：</label><span id="txtTel"><%=CustTel%></span>&nbsp;<img id="imgTel" alt="打电话"
                                style="cursor: pointer" src="../../../Images/phone.gif" border="0" onclick="javascript:ADTTool.openCallOutPopup(this,$('#txtTel').html(),'','9');" /></li>
                    <li>
                        <label>
                            地区：</label><span><input type="hidden" value="<%=ProvinceID%>" id="hidprovinceid" />
                                <input type="hidden" id="hidcityid" value="<%=CityID%>" /><%=LocationName%></span></li>
                    <li>
                        <label>
                            客户分类：</label><span>个人</span></li>
                    <li>
                        <label>
                            分属大区：</label><span><input type="hidden" value="<%=AreaID%>" id="hidAreaID" /><%=AreaName %></span></li>
                    <li>
                        <label>
                            汽车通：
                        </label>
                        <span>
                            <input type="text" id="txtUserName" class="w250" value="<%=UserName%>" readonly="readonly" />&nbsp;<input
                                type="button" value="注册" id="btnRegister" onclick="registerCarTong(this);" /></span>
                    </li>
                </ul>
            </div>
            <div class="title contact" style="clear: both;">
                订单信息<a href="javascript:void(0)" onclick="divShowHideEvent('divContactRecord',this)"
                    class="toggle hide"></a></div>
            <div id="divContactRecord">
                <ul class="clearfix ">
                    <li>
                        <label>
                            订单编号：</label><span><input type="hidden" value="<%=OrderID %>" id="OrderID" /><%=OrderCode %></span></li>
                    <li>
                        <label>
                            下单经销商：</label><span><a href="../CustCheck/CrmCustSearch/MemberDetail.aspx?MemberID=<%=MemberID%>&CustID=<%=CustID%>"
                                target="_blank"><%=DealerName%></a></span></li>
                    <li>
                        <label>
                            订购车款：</label><span><%=CarName%></span> </li>
                    <li>
                        <label>
                            下单时间：</label><span><%=OrderTime %></span> </li>
                    <li>
                        <label>
                            价格：</label><span><%=OrderPrice %></span> </li>
                    <li>
                        <label>
                            意向车型：</label>
                        <span>
                            <select id="dllMyBrand" name="BrandId" class="w125" style="width: 82px;">
                            </select>
                            <select id="dllMySerial" name="SerialId" class="w125" style="width: 82px;">
                            </select>
                            <select id="dllMyName" name="NameId" class="w125" style="width: 82px;" onmouseover="javascript:FixWidth(this);">
                            </select>
                        </span></li>
                    <li>
                        <label>
                            预计购车时间：</label>
                        <select id="dllPlanBuyCarTime" class="w255" onchange="selReturnVisit_Change()" runat="server">
                        </select>
                    </li>
                    <li>
                        <label>
                            <span class="redColor">*</span>处理状态：</label>
                        <select id="selReturnVisit" class="w255" onchange="selReturnVisit_Change()" runat="server">
                            <option value="-2">请选择</option>
                            <option value="1">是</option>
                            <option value="2">否</option>
                            <option value="0">未知</option>
                        </select>
                    </li>
                    <li id="li_fail" style="display: none">
                        <label>
                            <span class="redColor">*</span>失败理由：</label>
                        <select id="selFailReson" class="w255" style="width: 254px;" runat="server">
                        </select>
                    </li>
                    <li class="gdjl">
                        <label>
                            备注：</label><span><textarea id="Remark" style="width: 696px" name="Remark"><%=Remark %></textarea></span></li>
                </ul>
                <div class="btn">
                    <img id="imgLoadingPop" src="../../Images/blue-loading.gif" style="display: none;" />
                    <input type="button" name="" id="btnSave" onclick="SaveInfo()" value="保 存" />&nbsp;&nbsp;
                    <input type="button" name="" id="btnSubmit" onclick="SubmitInfo()" value="提 交" class="forwordbtn" />&nbsp;&nbsp;
                </div>
            </div>
            <div class="title contact">
                历史记录<a href="javascript:void(0)" id="divHis" onclick="divShowHideEvent('divHandlerHistory',this)"
                    class="toggle hide"></a></div>
            <div id="divHandlerHistory" class="tableList clearfix tableList2">
            </div>
        </div>
    </div>
    </form>
</body>
</html>
