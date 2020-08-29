<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Check.aspx.cs" Inherits="BitAuto.ISDC.CC2012.Web.CustCheck.NewCustCheck.Check" %>

<!--功能废弃 强斐 2016-8-19-->
<%@ Register Src="../../CustInfo/EditVWithCalling/UCEditCust.ascx" TagName="UCEditCust"
    TagPrefix="uc1" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>新增4S客户信息核实</title>
    <link href="../../Css/base.css" type="text/css" rel="stylesheet" />
    <link href="../../Css/style.css" type="text/css" rel="stylesheet" />
    <link href="../../Css/cc_checkStyle.css" type="text/css" rel="stylesheet" />
    <link href="../../Css/adtPopup.css" rel="stylesheet" type="text/css" />
    <script src="../../Js/jquery-1.4.4.min.js" type="text/javascript"></script>
    <script src="../../Js/json2.js" type="text/javascript"></script>
    <script src="../../Js/common.js" type="text/javascript"></script>
    <script src="../../Js/Enum/Area2.js" type="text/javascript"></script>
    <script src="../../Js/jquery.jmpopups-0.5.1.pack.js" type="text/javascript"></script>
    <script src="../../Js/jquery.free.ajaxTabPanel.js" type="text/javascript"></script>
    <script src="/CTI/CTITool.js" type="text/javascript"></script>
    <script src="../../Js/jquery.blockUI.js" type="text/javascript"></script>
    <script type="text/javascript" src="http://api.map.baidu.com/api?v=1.3"></script>
    <script type="text/javascript">
        $(function () {
            //add by qizq 2013-1-6呼出电话回调函数
            ADTTool.onInitiated = function (data) {
                var onInitiatedTime = new Date();
                $('#hidonInitiatedTime').val(onInitiatedTime.getTime());

                //add by lihf 2013-8-19呼出电话记录CallRecord_ORIG_Business            
                var pody = { Action: "InsertCallRecordORIGBusiness",
                    CallID: data.CallID,
                    TaskID: escape('<%=TID%>')
                };
                //插入中间表记录
                $.post("/AjaxServers/CustCheck/CallOutDeal.ashx", pody, function (data) {
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

                //接通后让提交按钮不可用
                $('#btnSub').attr("disabled", "disabled");
                //通话中状态标示，1表示通话中
                $('#hidIsCalling').val("1");
                $('#hidCallID').val("");

                //取联系人
                var Contract = "";
                if ($('#TelName') != undefined) {
                    Contract = $('#TelName').val();
                }

                var EstablishedTime = new Date();
                //坐席摘机到客户接通电话为客户振铃时间
                var timespan = parseInt((EstablishedTime.getTime() - parseInt($('#hidNetworkReachedTime').val())) / 1000);

                $.post('/AjaxServers/CustCheck/CallOutDeal.ashx', {
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
                    NewCustID: escape('<%=NewCustID %>'),
                    CustName: escape('<%= this.CustName %>'),
                    Contact: escape(Contract),
                    TaskID: escape('<%=TID%>'),
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
                //判断是否是通话中的挂断
                if (data.IsEstablished == "True") {

                    $('#btnSub').removeAttr("disabled");


                    //清空联系人
                    if ($('#TelName') != undefined) {
                        $('#TelName').val("");
                    }

                    //通话中状态标示，0表示不是通话中
                    //$('#hidIsCalling').val("0");
                    //判断是否有录音主键，如果不存在说明页面已刷新，或通话开始回调函数执行不成功
                    //if ($('#hidCallID').val() == "") {
                    //}
                    //有录音主键
                    //else {

                    //更新宇高录音接口
                    var CrmCustID = '<%=TID%>';
                    //var jsonstr = '{"CustID":' + CrmCustID + ',"CallStatus":"呼出","TaskTypeID":"1"}';
                    //var Result = window.external.MethodScript('/recordcontrol/updaterecorddatabyid?Refid=' + escape(data.RecordID) + '&data=' + escape(jsonstr));
                    //
                    var SessionIDstr = data.RecordID;
                    //录音自动绑定会员
                    var PTID = '<%=TID %>';
                    var postBody = { Action: 'AutoBindRecord', SessionID: data.RecordID, TID: PTID, DataSource: 1, EstablishEndTime: data.CurrentDate };
                    AjaxPost('/AjaxServers/RecordMonitor/RecordBindManager.ashx', postBody, null, function (data) {
                        var jsonData = $.evalJSON(data);
                        if (jsonData.AutoBindRecord == 'no')
                        { $.jAlert('录音绑定失败'); }
                        else if (jsonData.AutoBindRecord == 'NotExistRecord')
                        { $.jAlert('录音记录不存在'); }
                        else if (jsonData.AutoBindRecord == 'ExistMoreMember') {
                            $.openPopupLayer({
                                name: "BindCallRecordPopup",
                                parameters: { TID: PTID, SessionID: SessionIDstr },
                                popupMethod: 'Post',
                                url: "/CustInfo/DetailV/TaskCallRecordBind.aspx",
                                afterClose: function (e, data) {
                                    //if (e) {
                                    //重新加载通话记录列表
                                    $('#divCallRecordList').load('/CustInfo/DetailV/TaskCallRecordList.aspx', {
                                        ContentElementId: 'divCustContacts',
                                        TID: PTID,
                                        PageSize: 10
                                    });
                                    //}
                                }
                            });
                        }
                        else if (jsonData.AutoBindRecord == 'yes') {
                            $('#divCallRecordList').load('/CustInfo/DetailV/TaskCallRecordList.aspx', {
                                ContentElementId: 'divCustContacts',
                                TID: PTID,
                                PageSize: 10
                            });
                        }
                    });
                    //}
                }
            }

            //GMapService.loadMapJs(); //加载google地图相关脚本，完成后回调相应方法队列
        });
        
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
    <div class="w980">
        <div class="taskT">
            客户信息<input type="hidden" id="hdnCallRecordID" /><span></span></div>
        <%--<uc:Top ID="Top" runat="server" />--%>
        <div class="baseInfo clearfix">
        </div>
        <div class="btn">
            <% if (!RequestAction.Equals("view") || TaskStatus == (int)BitAuto.ISDC.CC2012.Entities.EnumProjectTaskStatus.NoAssign || TaskStatus == (int)BitAuto.ISDC.CC2012.Entities.EnumProjectTaskStatus.Assigning)
               { %>
            <input type="button" id="Button1" onclick="javascript:uCEditCustHelper.deleteInfo('/CustCheck/CheckTaskList.aspx');"
                class="button" style="margin-right: 50px;" value="删除" />
            <input type="button" id="btnConfirm" onclick="javascript:uCEditCustHelper.saveInfo();"
                class="button" value="保存" />
            <input type="button" id="btnSub" onclick="javascript:uCEditCustHelper.submitInfo('/CustCheck/CheckTaskList.aspx');"
                class="button" value="提交" />
            <%} %>
        </div>
    </div>
    </form>
</body>
</html>
