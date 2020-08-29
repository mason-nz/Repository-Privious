<%@ Page Title="" Language="C#" AutoEventWireup="true" CodeBehind="LeadTaskView.aspx.cs"
    Inherits="BitAuto.ISDC.CC2012.Web.LeadsTask.LeadTaskView" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <meta http-equiv="X-UA-Compatible" content="IE=EmulateIE7" />
    <meta http-equiv="Expires" content="0">
    <meta http-equiv="Cache-Control" content="no-cache">
    <meta http-equiv="Pragma" content="no-cache">
    <title>线索查看</title>
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
    <script type="text/javascript" src="/CTI/CTITool.js"></script>
    <script type="text/javascript">
        $(document).ready(function () {
            //如果失败，显示失败原因
            var Issuccess = '<%=model.IsSuccess%>';
            if (Issuccess == '0') {
                $("#li_fail").css("display", "");
                $("#spCar").css("display", "none");
            }
            else {
                $("#li_fail").css("display", "none");
            }
            //加载电话历史记录
            GetCallRecordORGIHistory();

            //设置下拉选样式
            SetDllDCarClass();
        });
        function SetDllDCarClass() {
            $("[select][id$='dllDCarName'] option").each(function () {
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

        function GetCallRecordORGIHistory() {
            //先清空历史记录图标
            $("a[chistory='1']").each(function () {
                $(this).remove();
            });

            var tels = "<%=model.Tel.Trim()%>";
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
                            msg = "&nbsp;<a chistory='1' href='../TaskManager/CustInformation.aspx?CustID=" + custid + "' title='历史记录' target='_blank' class='linkBlue'><img style='vertical-align:middle;' alt='历史记录' src='/images/history.png' border='0' /></a>";
                            $("#txtTel").after(msg);
                        }
                        else if (jsonData.result == "3") {
                            //显示个人用户列表
                            var custTel = jsonData.Tel;
                            msg = "&nbsp;<a chistory='1' href='../CustBaseInfo/List.aspx?CustTel=" + custTel + "' title='历史记录' target='_blank' class='linkBlue'><img style='vertical-align:middle;' alt='历史记录' src='/images/history.png' border='0' /></a>";
                            $("#txtTel").after(msg);
                        }
                    }
                });
            });

        }
        //是否加载过历史记录
        var isLoadList = 0;
        function divShowHideEvent(divId, obj) {
            if ($(obj).attr("class") == "toggle") {
                if (isLoadList == 0 && divId == 'infoBlock1') {
                    //绑定任务处理历史
                    BindHistory();
                    //绑定话务记录
                    BindCallReocrd();
                    isLoadList = 1;
                }
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
    </script>
</head>
<body>
    <form id="form1" runat="server">
    <div class="w980">
        <div class="taskT">
            线索查看<span></span></div>
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
                            电话：</label><span id="txtTel"><%=model.Tel.Trim()%></span>&nbsp;</li>
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
                            <span class="redColor" id="spCar"></span>需匹配车款：</label>
                        <span>
                            <%=DCarName %></span></li>
                    <li>
                        <label>
                            <span class="redColor"></span>是否成功：</label><span><%if (model.IsSuccess == 1)
                                                                                { %>是<%}
                                                                                else if (model.IsSuccess == 0)
                                                                                {%>否<%}%></span> </li>
                    <li id="li_fail" style="display: none">
                        <label>
                            <span class="redColor"></span>失败原因：</label>
                        <span>
                            <%=FailReason%></span> </li>
                    <li class="gdjl">
                        <label>
                            备注：</label><span id="Remark" runat="server"></span></li>
                </ul>
            </div>
            <div class="cont_cx khxx CustInfoArea">
                <div class="title contact" style="clear: both;">
                    记录历史<a class="toggle" onclick="divShowHideEvent('infoBlock1',this)" href="javascript:void(0)"></a>
                </div>
                <div id="infoBlock1" style="display: none;">
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
            <div class="btn" style="clear: both; padding-top: 30px">
            </div>
        </div>
    </div>
    </form>
</body>
</html>
