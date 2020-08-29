<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="SalesProcess.aspx.cs" Inherits="BitAuto.ISDC.CC2012.Web.WorkOrder.SalesProcess" %>

<%@ Register Src="UControl/OperInfoControl.ascx" TagName="OperInfoControl" TagPrefix="uc1" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>工单处理</title>
    <link href="/css/base.css" type="text/css" rel="stylesheet" />
    <link href="/css/style.css" type="text/css" rel="stylesheet" />
    <script src="/Js/Enum/Area2.js" type="text/javascript"></script>
    <script language="javascript" type="text/javascript" src="/Js/jquery-1.4.4.min.js"></script>
    <script type="text/javascript">
        document.writeln("<s" + "cript type='text/javascript' src='/Js/config.js?r=" + Math.random() + "'></scr" + "ipt>");
    </script>
    <script type="text/javascript">
        loadJS("common");
        loadJS("controlParams");
        loadJS("CTITool");
        loadJS("ucCommon");
    </script>
    <script type="text/javascript" charset="utf-8" src="/Js/jquery.jmpopups-0.5.1.pack.js"></script>
    <script src="/Js/jquery.blockUI.js" type="text/javascript"></script>
    <script type="text/javascript">
        //选择营销顾问
        function uc_chkSelectSales() {

            if (!$("#chkIsSales").is(":checked")) {
                $("#hidReceiverID").val("");
                $("#txtReceiverName").val("");
                ReceiverNameFocus();
            }
            else {
                uc_popSelectUser("<%=modelInfo.CRMCustID %>");
            }
        }

        //弹出选择接收人
        function uc_selectSales(crmCustID) {
            $.openPopupLayer({
                name: "SelectCustUserPopup",
                parameters: { CrmCustID: crmCustID },
                url: "/WorkOrder/AjaxServers/SelectCustUserPoper.aspx", afterClose: function (e, data) {
                    if (e) {
                        var userid = data.UserID;
                        var username = data.UserName;

                        $("#hidReceiverID").val(userid);
                        $("#txtReceiverName").val(username);

                        ReceiverNameFocus();
                        if (crmCustID == "") {
                            //说明是直接点击的接收人文本框，需要清空营销顾问复选框
                            $("#chkIsSales").attr("checked", false);
                        }
                    }
                    else {
                        //说明没有选择，需要清空营销顾问复选框
                        //$("#chkIsSales").attr("checked", false);
                        ReceiverNameFocus();
                    }
                }
            });

        }

        function uc_popSelectUser(crmCustID) {
            $.post("/WorkOrder/AjaxServers/WorkOrderOperHandler.ashx", { Action: 'GetCustUser', CrmCustID: crmCustID }, function (data) {
                var jsonData = $.evalJSON(data);

                if (jsonData.totalcount == 0) {
                    ReceiverNameFocus();
                    $.jAlert("该客户下没有负责销售，请选择其他接收人！", function () {
                        $("#chkIsSales").attr("checked", false);
                        return false;
                    });
                }
                else if (jsonData.totalcount == 1) {
                    var userid = jsonData.userinfo.userid;
                    var username = jsonData.userinfo.username;

                    $("#hidReceiverID").val(userid);
                    $("#txtReceiverName").val(username);
                    ReceiverNameFocus();
                }
                else {
                    uc_selectSales(crmCustID);
                }
            });

        }

        function ReceiverNameFocus() {
            if ($("#txtReceiverName").val() == "" && $("#hidReceiverID").val() == "") {
                $("#txtReceiverName").val("如果要转回至呼叫中心无须填写此项");
                $("#txtReceiverName").attr("style", "color:#999;");
                $("#chkIsSales").attr("checked", false);
            }
            else {
                if ($("#txtReceiverName").val() != "如果要转回至呼叫中心无须填写此项") {
                    $("#txtReceiverName").removeAttr("style");
                }
                else {
                    $("#chkIsSales").attr("checked", false);
                }
            }
        }

    </script>
    <script type="text/javascript">
        var source = '<%=SourceUrl %>';
        if (source == 'wp2013' || source == 'crm2009') {
            document.domain = '<%=SysUrl %>';
        }

        $(function () {
            $("#form1").hide();

            ReceiverNameFocus();

            AjaxPostAsync("/WorkOrder/AjaxServers/OrderProcessHandler.ashx", { Action: "salesPermission", OrderID: "<%=OrderID %>", r: Math.random() }, null, function (data) {
                var jsonData = $.evalJSON(data);
                if ('<%=(!string.IsNullOrEmpty(Requester)&&Requester.Trim()=="intelligentplatform")%>' == 'True') {
                    $("#form1").show();
                }
                else if (jsonData.permission == "none") {
                    $.jAlert("您无权限访问该页面！", function () { closePage(); });
                }
                else if (jsonData.permission == "view") {
                    $("#form1").show();
                    //查看权限
                    $("#divRevertInfo").html("");
                    $("#divRevertInfo").attr("style", "border:none");
                }
                else if (jsonData.permission == "process") {
                    $("#form1").show();
                }
            });

        });

        function salesSubmit() {
            if ($("makeCallType" == "1")) {   //使用西门子外呼时
                var revertContent = $.trim($("#areaContent").val());
                if (revertContent == "") {
                    $.unblockUI();
                    $.jAlert("回复内容不能为空！");
                    return false;
                }
                if (Len(revertContent) > $("#areaContent").attr("lenstr")) {
                    $.unblockUI();
                    $.jAlert("回复内容超过300个字！");
                    return false;
                }

                var receiverName = "";
                var receiverID = $("#hidReceiverID").val();

                if (receiverID != "") {
                    receiverName = encodeURIComponent($.trim($("#txtReceiverName").val()));
                }

                var isSales = $("#chkIsSales").is(":checked") ? "true" : "false";

                if ($.jConfirm("是否确认提交？", function (r) {

                    if (r) {
                        //开始

                        $.blockUI({ message: '正在处理，请等待...' });

                        AjaxPostAsync("/WorkOrder/AjaxServers/OrderProcessHandler.ashx", { Action: "salesProcessSubmit", Requester: '<%=(!string.IsNullOrEmpty(Requester)&&Requester.Trim()=="intelligentplatform") ? "intelligentplatform" :""%>', OrderID: "<%=OrderID %>", RevertContent: revertContent, ReceiverName: receiverName, ReceiverID: receiverID, IsSales: isSales, CallID: $("#hidCallID").val(), RTWORID: $("#hidworid").val(), r: Math.random() }, null, function (data) {
                            $("#hidCallID").val("");
                            $("#hidworid").val("");

                            $.unblockUI();
                            var jsonData = $.evalJSON(data);
                            if (jsonData.permission == "none") {
                                $.jAlert("您无权限访问该页面！", function () {
                                    closePage();
                                });
                            }
                            else if (jsonData.permission == "view") {
                                $.jAlert("该工单现在不属于您处理！操作失败", function () {
                                    window.location.reload();
                                });
                            }
                            else if (jsonData.permission == "over") {
                                $.jAlert("该工单处理已结束，无法处理！", function () {
                                    window.location.reload();
                                });
                            }
                            else if (jsonData.result == "false") {
                                $.jAlert(jsonData.msg);
                                return false;
                            }
                            else if (jsonData.result == "true") {                                
                                $.jPopMsgLayer("操作成功！", function () {
                                    if (source == "wp2013") {
                                        closePageExecOpenerSearch();
                                    }
                                    else if (source == "crm2009") {
                                        closePageExecOpenerSearch("hrefWorkOrder")
                                    }
                                    else {
                                        closePage();
                                    }
                                });
                            }

                        });
                        //结束
                    }
                }));
            }
            else {
                var revertContent = $.trim($("#areaContent").val());
                if (revertContent == "") {
                    $.unblockUI();
                    $.jAlert("回复内容不能为空！");
                    return false;
                }
                if (Len(revertContent) > $("#areaContent").attr("lenstr")) {
                    $.unblockUI();
                    $.jAlert("回复内容超过300个字！");
                    return false;
                }

                var receiverName = "";
                var receiverID = $("#hidReceiverID").val();

                if (receiverID != "") {
                    receiverName = encodeURIComponent($.trim($("#txtReceiverName").val()));
                }

                var isSales = $("#chkIsSales").is(":checked") ? "true" : "false";

                if ($.jConfirm("是否确认提交？", function (r) {

                    if (r) {
                        //开始

                        $.blockUI({ message: '正在处理，请等待...' });

                        AjaxPostAsync("/WorkOrder/AjaxServers/OrderProcessHandler.ashx", { Action: "salesProcessSubmit", OrderID: "<%=OrderID %>", RevertContent: revertContent, ReceiverName: receiverName, ReceiverID: receiverID, IsSales: isSales, CallID: $("#hidCallID").val(), RTWORID: $("#hidworid").val(), r: Math.random() }, null, function (data) {
                            $("#hidCallID").val("");
                            $("#hidworid").val("");

                            $.unblockUI();
                            var jsonData = $.evalJSON(data);
                            if (jsonData.permission == "none") {
                                $.jAlert("您无权限访问该页面！", function () {
                                    closePage();
                                });
                            }
                            else if (jsonData.permission == "view") {
                                $.jAlert("该工单现在不属于您处理！操作失败", function () {
                                    window.location.reload();
                                });
                            }
                            else if (jsonData.permission == "over") {
                                $.jAlert("该工单处理已结束，无法处理！", function () {
                                    window.location.reload();
                                });
                            }
                            else if (jsonData.result == "false") {
                                $.jAlert(jsonData.msg);
                                return false;
                            }
                            else if (jsonData.result == "true") {                              
                                $.jPopMsgLayer("操作成功！", function () {
                                    if (source == "wp2013") {
                                        closePageExecOpenerSearch();
                                    }
                                    else if (source == "crm2009") {
                                        closePageExecOpenerSearch("hrefWorkOrder")
                                    }
                                    else {
                                        closePage();
                                    }
                                });
                            }

                        });
                        //结束
                    }
                }));
            }
        }

    </script>
    <script type="text/javascript">
        $(function () {
            //add by qizq 2013-1-6呼出电话回调函数
            ADTTool.onInitiated = function (data) {
                //电话未接通情况，业务记录录音主键初始化为-2，表示没有录音
                //否则会记录为上通电话录音主键
                $('#hidCallRecordID').val("-2");
                $("#hidReleaseCount").val("0");
                var onInitiatedTime = new Date();
                $('#hidonInitiatedTime').val(onInitiatedTime.getTime());

                //add by lihf 2013-8-19呼出电话记录CallRecord_ORIG_Business            
                var pody = { Action: "InsertCallRecordORIGBusiness",
                    CallID: data.CallID,
                    TaskID: escape('<%=OrderID%>')
                };
                //插入中间表记录
                $.post("/AjaxServers/TemplateManagement/CallOutDealHandler.ashx", pody, function (data) {
                    callRecordID = data;
                });
            };
            //add by qizq 2013-1-6呼出电话坐席摘机回调函数//拨出电话到坐席摘机为坐席振铃时间
            ADTTool.onNetworkReached = function (data) {
                //电话未接通情况，业务记录录音主键初始化为-2，表示没有录音
                //否则会记录为上通电话录音主键
                $('#hidCallRecordID').val("-2");
                $("#hidReleaseCount").val("0");
                var NetworkReachedTime = new Date();
                $('#hidNetworkReachedTime').val(NetworkReachedTime.getTime());
                var timespan = parseInt((NetworkReachedTime.getTime() - parseInt($('#hidonInitiatedTime').val())) / 1000);
                //拨出电话到坐席摘机为坐席振铃时间
                $('#hidTimeSpan').val(timespan);
            };

            //add by qizq 2013-1-4电话录音接通后回调函数//坐席摘机到客户接通电话为客户振铃时间
            ADTTool.Established = function (data) {
                $("#hidReleaseCount").val("0");
                //接通后让提交按钮不可用
                $('#btnSubmit').attr("disabled", "disabled");
                //通话中状态标示，1表示通话中
                $('#hidIsCalling').val("1");
                $('#hidCallID').val(escape(data.CallID));

                var EstablishedTime = new Date();
                //坐席摘机到客户接通电话为客户振铃时间
                var timespan = parseInt((EstablishedTime.getTime() - parseInt($('#hidNetworkReachedTime').val())) / 1000);
                $.post('/AjaxServers/TemplateManagement/CallOutDealHandler.ashx', {
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
                    CustID: escape('<%=crmCustID%>'),
                    CustName: escape('<%=crmCustName%>'),
                    Contact: escape('<%=contact%>'),
                    TaskID: escape('<%=OrderID%>'),
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
                        } else {
                            $('#hidCallRecordID').val(jsonData.recordid);
                            $('#hidworid').val(escape(jsonData.WORID));
                        }
                    });
            };

            ADTTool.onDisconnected = function (data) {
                $('#btnSubmit').attr("disabled", "");

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
                    $.post('/AjaxServers/TemplateManagement/CallOutDealHandler.ashx', {
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
                        CustID: escape('<%=crmCustID%>'),
                        CustName: escape('<%=crmCustName%>'),
                        Contact: escape('<%=contact%>'),
                        TaskID: escape('<%=OrderID%>'),
                        CallRecordID: escape($('#hidCallRecordID').val()),
                        EstablishEndTime: data.CurrentDate
                    },
                        function (data) {
                            var jsonData = $.evalJSON(data);
                            //不成功提示错误
                            if (jsonData.success == 'no') {
                                $.jAlert(jsonData.msg);
                            } else {
                                //加载处理记录
                            }
                        });
                    //}
                }

                //挂断后,保存业务记录
                if ($("#hidReleaseCount").val() == "0") {
                    $("#hidReleaseCount").val("1");
                    $("#hidden_CallID").val(data.CallID);
                    saveCustInfo();
                }
            };
        });

        function saveCustInfo() {
            var ccUserID = "<%=ccUserID %>";
            if (ccUserID != "") {
                //先判断用户是否有该电话，没有添加
                var tel = $("#hidCallPhone").val();
                operCustTel(ccUserID, tel);
                //操作业务联系记录
                //不能添加业务记录，会导致导出报错
                operCustHistoryInfo("<%=OrderID %>", "1", $('#hidCallRecordID').val(), "2", ccUserID, tel);
            }
        }

        function operCustTel(custid, tel) {
            AjaxPostAsync("/AjaxServers/CustBaseInfo/popCustBasicInfo.ashx",
            {
                Action: "operCustTel2",
                CustID: custid,
                CustName: "<%=contact %>", //联系人名称更新成客户名称
                OrderID: "<%=OrderID %>",
                Tel: tel,
                r: Math.random()
            },
            null, function (data) {
            });
        }
        //操作联系记录
        //businessType:业务类型1，工单，2团购订单，3客户核实，4其他任务
        //callRecordID:录音CallID
        //记录类型：1-呼入；2-呼出
        function operCustHistoryInfo(taskID, businessType, callRecordID, recordType, custID, tel) {
            AjaxPostAsync("/AjaxServers/CustBaseInfo/popCustBasicInfo.ashx",
            {
                Action: "operCustHistoryInfo",
                TaskID: taskID,
                BusinessType: businessType,
                CallRecordID: callRecordID,
                RecordType: recordType,
                CustID: custID,
                Tel: tel,
                CallID: $("#hidden_CallID").val(),
                r: Math.random()
            },
            null, function (data) {
            });
        }
    </script>
</head>
<body>
    <form id="form1" runat="server">
    <%-- 记录播出电话时间--%>
    <input type="hidden" id="hidonInitiatedTime" />
    <%--怎么获取此值--%>
    <input type="hidden" id="hidCallPhone" value="<%=modelInfo.ContactTel%>" />
    <%-- CallID--%>
    <input type="hidden" id="hidCallID" value="" />
    <%--- 接通时插入[WorkOrderRevert]表中数据时返回的WORID --%>
    <input type="hidden" id="hidworid" value="" />
    <input type="hidden" id="hidCallRecordID" />
    <input type="hidden" id="hidReleaseCount" />
    <%-- 记录电话播出到坐席接起电话时间间隔秒数--%>
    <input type="hidden" id="hidTimeSpan" />
    <%-- 是否在通话中，通话中为1--%>
    <input type="hidden" id="hidIsCalling" />
    <input type="hidden" id="hidNetworkReachedTime" />
    <input type="hidden" id="hidden_CallID" />
    <div class="w980 gdcl">
        <div class="taskT">
            工单处理<span></span></div>
        <div class="content">
            <div class="gd_title">
                <%=modelInfo.Title%>
                <span>（来源：<%=dataSource%>）</span></div>
            <div class="Menubox8">
                <ul>
                    <li></li>
                </ul>
            </div>
            <!--内容开始-->
            <div class="Contentbox8">
                <!--内容1开始-->
                <div class="hover">
                    <ul class="clearfix Info">
                        <li class="w260" style="width: 320px">
                            <label>
                                关联客户：</label>
                            <%=CrmCustInfoUrl%>
                        </li>
                        <li style="width: 300px">
                            <label>
                                客户地区：</label>
                            <%=custArea%></li>
                        <li style="width: 160px">
                            <label>
                                联系人：</label><span><%=modelInfo.Contact%></span></li>
                        <li style="position: relative; top: -8px; width: 180px;">
                            <label>
                                联系电话：</label><span><%=modelInfo.ContactTel%></span>
                            <img src="/Images/phone.gif" <%=telVisible %> onclick="javascript:ADTTool.openCallOutPopup(this, '<%=modelInfo.ContactTel%>','','0');" />
                        </li>
                    </ul>
                    <ul class="clearfix Info2" id="ulReturn" runat="server">
                        <li>
                            <label id="lbRelateDemands">
                                关联需求：</label><span>
                                    <asp:HyperLink ID="hlRelateDemand" runat="server"></asp:HyperLink>
                                </span></li>
                        <li>&nbsp;&nbsp;&nbsp;&nbsp;</li>
                    </ul>
                </div>
                <div class="hfq clearfix" id="divRevertInfo">
                    <div class="hfl">
                        <h2 style="font-weight: bold;">
                            回复：</h2>
                        <textarea id="areaContent" name="Content" lenstr="600" style="height: 90px"></textarea>
                        <!--按钮-->
                        <ul style="width: 400px; float: left; *margin-top: -22px; *margin-left: 20px;">
                            <li>
                                <label>
                                    接收人：</label><span><input name="ReceiverName" id="txtReceiverName" type="text" class="w250"
                                        readonly="readonly" onclick="uc_selectSales('')" /><input name="ReceiverID" id="hidReceiverID"
                                            type="hidden" value="" /></span><span><input name="IsSales" id="chkIsSales" type="checkbox"
                                                value="true" class="dx" onclick="uc_chkSelectSales()" /><em onclick="emChkIsChoose(this);uc_chkSelectSales();">营销顾问</em></span>
                            </li>
                        </ul>
                        <div class="btn" style="float: right; text-align: right; *margin-top: -25px; *margin-right: 10px;
                            width: 300px;">
                            <input type="button" name="" id="btnSubmit" value="提 交" onclick="salesSubmit()" class="btnSave bold" />
                        </div>
                        <!--按钮-->
                    </div>
                </div>
                <div id="con_one_1" class="hover">
                    <!--回复区-->
                    <uc1:OperInfoControl ID="OperInfoControl1" runat="server" />
                    <!--回复区-->
                </div>
                <!--内容1结束-->
            </div>
            <!--内容结束-->
        </div>
    </div>
    </form>
</body>
</html>
