<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="CCProcess.aspx.cs" Inherits="BitAuto.ISDC.CC2012.Web.WorkOrder.CCProcess" %>

<%@ Register Src="UControl/OperInfoControl.ascx" TagName="OperInfoControl" TagPrefix="uc1" %>
<%@ Register Src="UControl/CustSalesControl.ascx" TagName="CustSalesControl" TagPrefix="uc2" %>
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
    <link href="../../Css/jquery.autocomplete.css" rel="stylesheet" type="text/css" />
    <script src="../../Js/jquery.autocomplete.min.js" type="text/javascript"></script>
    <script type="text/javascript">
        loadJS("common");
        loadJS("controlParams");
        loadJS("CTITool");
        loadJS("ucCommon");
    </script>
    <%-- <script src="/WorkOrder/UControl/ucCommon.js" type="text/javascript"></script>--%>
    <script type="text/javascript" charset="utf-8" src="/Js/jquery.jmpopups-0.5.1.pack.js"></script>
    <script src="/Js/jquery.blockUI.js" type="text/javascript"></script>
    <script type="text/javascript">
        // JavaScript Document
        /*第一种形式 第二种形式 更换显示样式*/
        function setTab(name, cursel, n) {
            for (i = 1; i <= n; i++) {
                var menu = document.getElementById(name + i);
                var con = document.getElementById("con_" + name + "_" + i);
                menu.className = i == cursel ? "hover" : "";
                con.style.display = i == cursel ? "block" : "none";
            }
        }


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
                //$('#btnReject').attr("disabled", "disabled");
                //通话中状态标示，1表示通话中
                $('#hidIsCalling').val("1");
                //$('#hidCallRecordID').val("");
                //alert("aa=" + escape(data.CallID));
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
                            //$('#hidCallID').val(escape(data.CallID));
                        }
                    });
            };

            ADTTool.onDisconnected = function (data) {
                $('#btnSubmit').attr("disabled", "");
                //$('#btnReject').attr("disabled", "");

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
                        CallRecordID: escape($('#hidCallRecordID').val()), //escape($('#hidCallID').val()),
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
    </script>
    <script type="text/javascript">
        function saveCustInfo() {
            var ccUserID = "<%=ccUserID %>";
            if (ccUserID != "") {
                //先判断用户是否有该电话，没有添加
                var tel = $("#hidCallPhone").val();
                operCustTel(ccUserID, tel);
                //操作业务联系记录
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
    <%-- 记录电话播出到坐席接起电话时间间隔秒数--%>
    <input type="hidden" id="hidTimeSpan" />
    <%-- 记录坐席接起电话时间--%>
    <input type="hidden" id="hidNetworkReachedTime" />
    <%-- 记录播出电话时间--%>
    <input type="hidden" id="hidonInitiatedTime" />
    <%-- 是否在通话中，通话中为1--%>
    <input type="hidden" id="hidIsCalling" />
    <%-- 本地录音表主键--%>
    <input type="hidden" id="hidCallRecordID" value="" />
    <%-- CallID--%>
    <input type="hidden" id="hidCallID" value="" />
    <%--录音表SessionID--%>
    <input type="hidden" id="hidSessionID" />
    <input type="hidden" id="hidCallPhone" value="<%=tel %>" />
    <input type="hidden" id="hidReleaseCount" value="0" />
    <input type="hidden" id="hidden_CallID" />
    <div class="w980 gdcl">
        <div class="taskT">
            工单处理<span></span></div>
        <div class="content">
            <div class="gd_title">
                <%=title%>
                <span>（来源：<%=dataSource%>）</span> &nbsp;&nbsp;&nbsp;<%=viewCCUserUrl%>
                &nbsp;&nbsp;&nbsp;&nbsp; <span>
                    <%=CHUrl%></span>
            </div>
            <!--切换标签开始-->
            <div class="Menubox8">
                <ul style="*margin-right: 20px">
                    <li id="one1" onclick="setTab('one',1,2)" class="hover">回复工单</li>
                    <li id="one2" onclick="setTab('one',2,2)">操作记录</li>
                </ul>
            </div>
            <!--切换标签结束-->
            <!--内容开始-->
            <div class="Contentbox8">
                <!--内容1开始-->
                <div class="hover">
                    <ul class="clearfix Info">
                        <li class="w260" style="width: 320px">
                            <label>
                                关联客户：</label>
                            <%=crmCustNameUrl%>
                        </li>
                        <li style="width: 300px">
                            <label>
                                客户地区：</label>
                            <%=custArea%></li>
                        <li style="width: 160px">
                            <label>
                                联系人：</label><span><%=contact%></span></li>
                        <li style="position: relative; top: -8px; width: 180px;">
                            <label>
                                联系电话：</label><span><%=tel%>
                                </span>
                            <img src="/Images/phone.gif" onclick="javascript:ADTTool.openCallOutPopup(this,'<%=tel%>','','0');" />
                        </li>
                    </ul>
                    <ul class="clearfix Info2" id="ulReturn">
                        <li>
                            <label>
                                <span class="redColor">*</span>工单分类：</label><span><select name="CategoryID1" vtype="notFirstOption"
                                    id="selCategoryID1" onchange="uc_BindCategory2('selCategoryID2','selCategoryID1');uc_BindCategoryJudgeDemand('selCategoryID','selCategoryID2')"
                                    vmsg="请选择工单一级分类！" class="w80"><option value='-1'>请选择</option>
                                </select></span>&nbsp;&nbsp;<span><select name="CategoryID2" onchange="uc_BindCategoryJudgeDemand('selCategoryID','selCategoryID2')"
                                    class="w80" vtype="notFirstOption" id="selCategoryID2" vmsg="请选择工单二级分类！"><option
                                        value='-1'>请选择</option>
                                </select></span>&nbsp;<span><select name="CategoryID" class="w80" vtype="notFirstOption"
                                    id="selCategoryID" vmsg="请选择工单三级分类！" onchange="changeLabelText()"><option value='-1'>
                                        请选择</option>
                                </select></span></li>
                        <li>
                            <label>
                                工单类型：</label><span><input name="IsComplaintType" type="checkbox" value="true" class="dx"
                                    id="chkIsComplaintType" /><em onclick="emChkIsChoose(this);">投拆</em></span></li>
                        <li>
                            <label>
                                <span class="redColor">*</span>工单状态：</label><span><select name="WorkOrderStatus"
                                    vtype="notFirstOption" vmsg="请选择工单状态！" class="w250" id="selWorkOrderStatus"><option>
                                        请选择</option>
                                </select></span></li>
                        <li id="liDemandCC" runat="server">
                            <label id="lbRelateDemands">
                                关联需求：</label><span>
                                    <asp:HyperLink ID="hlRelateDemand" runat="server"></asp:HyperLink>
                                </span></li>
                        <li>
                            <label>
                                <span class="redColor">*</span>优&nbsp;先&nbsp;级：</label><span><select name="PriorityLevel"
                                    id="selPriorityLevel" class="w250" style="width: 255px;" vtype="notFirstOption"
                                    vmsg="请选择优先级！"><option>请选择</option>
                                </select></span></li>
                        <li>
                            <uc2:CustSalesControl ID="CustSalesControl1" runat="server" />
                        </li>
                        <li>
                            <label>
                                标&nbsp;&nbsp;&nbsp;签：</label><div class="coupon-box02" style="float: left; width: 248px;">
                                    <input type="text" value="" style="width: 240px;" class="text02" id="txtTagInfo"><b><a
                                        href="#">选择</a></b></div>
                        </li>
                    </ul>
                </div>
                <div class="hfq clearfix">
                    <div class="hfl">
                        <h2 style="font-weight: bold;">
                            回复：</h2>
                        <textarea id="areaContent" name="Content" lenstr="600" style="height: 90px"></textarea>
                        <!--按钮-->
                        <div class="btn" style="float: right; text-align: right; *margin-top: -25px; *margin-right: 10px;">
                            <input type="button" id="btnSubmit" name="" value="提 交" onclick="CCProcessSubmit()"
                                class="btnSave bold" />
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
                <!--内容2开始-->
                <div id="con_one_2" style="display: none;">
                    <!--回复区-->
                    <uc1:OperInfoControl ID="OperInfoControl2" runat="server" />
                    <!--回复区-->
                </div>
                <!--内容2结束-->
            </div>
            <!--内容结束-->
        </div>
    </div>
    </form>
    <script type="text/javascript">
        function uc_triggerProvince() {//选中省份
            BindCity('selProvince', 'selCity');
            BindCounty('selProvince', 'selCity', 'selCounty');
        }

        function uc_triggerCity() {//选中城市
            BindCounty('selProvince', 'selCity', 'selCounty');
            //若城市列表中，没有数据，则添加属性noCounty，值为1，否则不添加属性
            if ($('#selCounty option').size() == 1)
            { $('#selCounty').attr('noCounty', '1'); }
            else
            { $('#selCounty').removeAttr('noCounty'); }
        }

        //工单表信息
        function GetOperData() {

            var operData = "";
            if ($("#selCategoryID option").length > 1) {
                operData += "CategoryID=" + $("#selCategoryID").val() + "&";
            }
            else {
                operData += "CategoryID=" + $("#selCategoryID2").val() + "&";
            }

            operData += "IsComplaintType=" + $("#chkIsComplaintType").is(":checked") + "&";

            operData += "WorkOrderStatus=" + $("#selWorkOrderStatus").val() + "&";

            operData += "ReceiverName=" + encodeURIComponent($.trim($("#txtReceiverName").val())) + "&";

            var receiverID = $.trim($("#hidReceiverID").val());
            if (receiverID == "") {
                receiverID = "-2";
            }
            operData += "ReceiverID=" + receiverID + "&";

            operData += "IsSales=" + $("#chkIsSales").is(":checked") + "&";

            operData += "PriorityLevel=" + $.trim($("#selPriorityLevel").val()) + "&";

            operData = operData.substring(0, operData.length - 1);

            return operData;
        }

        function CCProcessSubmit() {
            var jsonData = "";

            var ulHtml = $("#ulReturn").html();
            ulHtml = "<form name='ReturnInfoForm'>" + ulHtml + "</form>"; //ul

            var serializeData = $(ulHtml).serializeArray();

            jsonData = $.evalJSON(validateMsg(serializeData));

            var errorMsg = judgeStatusReceiver();
            if (errorMsg != "") {
                jsonData.result = "false";
                jsonData.msg = jsonData.msg == undefined ? errorMsg : jsonData.msg + errorMsg;
            }

            //前台数据格式验证成功
            if (jsonData.result == "true") {

                var alertMsg = "";

                //如果状态为-处理中，且没选择接收人，需要提示一下未选择接收人
                var receiverNameMsg = ($.trim($("#txtReceiverName").val()) == "" && $.trim($("#selWorkOrderStatus option:selected").text()) == "处理中") ? "未选择接收人，" : "";

                alertMsg = receiverNameMsg + "是否确认提交？";

                if ($.trim($("#selWorkOrderStatus option:selected").text()) == "已处理") {
                    alertMsg = "工单状态已改为已处理，是否确认结束本工单？";
                }
                else if ($.trim($("#selWorkOrderStatus option:selected").text()) == "已关闭") {
                    alertMsg = "工单状态已调整为已关闭，关闭后不可再处理，确认提交？";
                }


                if ($.jConfirm(alertMsg, function (r) {

                    if (r) {
                        //开始 
                        $.blockUI({ message: '正在处理，请等待...' });

                        AjaxPostAsync("/WorkOrder/AjaxServers/OrderProcessHandler.ashx", { Action: "ccProcessSubmit", Requester: '<%=(!string.IsNullOrEmpty(Requester)&&Requester.Trim()=="intelligentplatform") ? "intelligentplatform" :""%>', OrderID: "<%=OrderID %>", ValidateData: GetAreaValidateMsg("ulReturn"), OperData: GetOperData(), RevertContent: encodeURIComponent($.trim($("#areaContent").val())), TagIDs: $("#txtTagInfo").attr('did'), TagNames: encodeURIComponent($.trim($("#txtTagInfo").val())), CallID: $("#hidCallID").val(), r: Math.random() }, null, function (data) {
                            $.unblockUI();
                            var jsonData = $.evalJSON(data);
                            if (jsonData.permission == "") {
                                $.jAlert("您无权限访问该页面！", function () {
                                    closePage();
                                });
                            }
                            else if (jsonData.permission == "over") {
                                $.jAlert("该工单处理已结束，无法进入处理页面！", function () {
                                    closePage();
                                });
                            }
                            else if (jsonData.result = 'true') {
                                //                                $.jAlert("操作成功！", function () {
                                //                                    closePageExecOpenerSearch("btnsearch");
                                //                                });
                                $.jPopMsgLayer("操作成功！", function () { closePageExecOpenerSearch("btnsearch"); });
                            }
                        });

                        //结束
                    }
                }));
            }
            else {
                $.unblockUI();

                $.jAlert(jsonData.msg, function () {
                    return false;
                });
            }

        }

        //判断状态和接收人之间是否正确
        function judgeStatusReceiver() {

            var errorMsg = "";

            var status = $("#selWorkOrderStatus option:selected").text();

            if (status == "待处理") {
                if ($.trim($("#txtReceiverName").val()) == "") {
                    errorMsg = "状态为待处理时必须选择接收人！<br/>";
                }
            }
            else if (status != "已关闭" && status != "已处理") {
                if ($.trim($("#areaContent").val()) == "") {
                    errorMsg = "状态为" + status + "时请填写回复内容！<br/>";
                }
            }

            if (status == "已关闭" && $.trim($("#txtReceiverName").val()) != "") {
                //$.jAlert("状态为已关闭时选择接收人但该工单已结束并不会流转！");
            }

            if (Len($.trim($("#areaContent").val())) > $("#areaContent").attr("lenstr")) {
                errorMsg += "回复内容超过300个字！";
            }
            return errorMsg;
        }

        function ccProcessInit() {

            BindProvince("selProvince");

            uc_triggerProvince();

            BindByEnum("selPriorityLevel", "PriorityLevelEnum");

            //优先级默认为：普通
            $("#selPriorityLevel").val("1");

            uc_BindCategory1('selCategoryID1', "1,3");

            BindByEnum("selWorkOrderStatus", "WorkOrderStatus");
            StatusInit();
            loadCategory();
            $("#selPriorityLevel").val("<%=priorityLevel%>");

            if ("<%=isComplaintType%>".toLocaleLowerCase() == "true") {
                $("#chkIsComplaintType").attr("checked", true);
            }

            $("#txtTagInfo").val("<%=tagInfo %>");
            $("#txtTagInfo").attr("did", '<%=tagIDs %>');
            // $("#hidTagID").val("<%=tagIDs %>");


        }

        function StatusInit() {

            var nowStatus = "<%=status %>";
            //1-待审核、2-待处理、3-处理中、4-已处理、5-已完成、6-已关闭
            //如果是1-待审核，隐藏1、3、4、5；默认待处理
            if (nowStatus == "1") {
                $("#selWorkOrderStatus option[value='1']").remove();
                $("#selWorkOrderStatus option[value='3']").remove();
                $("#selWorkOrderStatus option[value='4']").remove();
                $("#selWorkOrderStatus option[value='5']").remove();

                $("#selWorkOrderStatus").val("2");
            }
            //如果是2-待处理或3-处理中，隐藏1、2、5；2-待处理默认处理中
            else if (nowStatus == "2" || nowStatus == "3") {
                $("#selWorkOrderStatus option[value='1']").remove();
                $("#selWorkOrderStatus option[value='2']").remove();
                $("#selWorkOrderStatus option[value='5']").remove();

                $("#selWorkOrderStatus").val("3");
            }

            //如果3-处理中，默认已处理
            if (nowStatus == "3") {
                $("#selWorkOrderStatus").val("4");
            }
        }

        //绑定工单分类
        function loadCategory() {

            $("#selCategoryID1").val("<%=categoryID1 %>");
            uc_BindCategory2('selCategoryID2', 'selCategoryID1');
            if ("<%=categoryID2 %>" != "-1") {
                $("#selCategoryID2").val("<%=categoryID2 %>");
                uc_BindCategoryJudgeDemand('selCategoryID', 'selCategoryID2');
                $("#selCategoryID").val("<%=categoryID %>");
            }
            else {
                $("#selCategoryID2").val("<%=categoryID %>");
            }

        }

        $(function () {
            $('#txtTagInfo').ChooseWorkTag();
            $("#form1").hide();
            AjaxPostAsync("/WorkOrder/AjaxServers/OrderProcessHandler.ashx", { Action: "permission", OrderID: "<%=OrderID %>", r: Math.random() }, null, function (data) {
                var jsonData = $.evalJSON(data);
                if ('<%=(!string.IsNullOrEmpty(Requester)&&Requester.Trim()=="intelligentplatform")%>' == 'True') {
                    $("#form1").show();
                    ccProcessInit();
                    changeLabelText();
                }
                else if (jsonData.permission == "") {
                    $.jAlert("您无权限访问该页面！", function () {
                        closePage();
                    });
                }
                else if (jsonData.permission == "over") {
                    $.jAlert("该工单处理已结束，无法进入处理页面！", function () {
                        closePageExecOpenerSearch("btnsearch");
                    });
                }
                else {
                    $("#form1").show();
                    ccProcessInit();
                    changeLabelText();
                }
            });

        });

    </script>
</body>
</html>
