<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="CSLeadTaskView.aspx.cs"
    Inherits="BitAuto.ISDC.CC2012.Web.LeadsTask.CSLeadTaskView" %>

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
            var IsJT = '<%=model.IsJT%>';
            //            liNotEstablish liNotSuccess
            if (IsJT == '0') {
                $("#liNotEstablish").css("display", "");
                //$("#spCar").css("display", "none");
                //$("#li_kl").css("display", "");
            }
            else {
                if (Issuccess == '0') {
                    $("#liNotSuccess").css("display", "");
                }
                $("#liNotEstablish").css("display", "none");
                // $("#li_kl").css("display", "none");
            }
            //加载电话历史记录
            GetCallRecordORGIHistory();

            //设置下拉选样式
            SetDllDCarClass();

            if ("<%=model.IsBoughtCar%>" == "1") {
                $(".li_BoughtCar").css("display", "block");
                $(".li_NotBuyCar").css("display", "none");
            }
            else if ("<%=model.IsBoughtCar%>" == "0") {
                $(".li_NotBuyCar").css("display", "block");
                $(".li_BoughtCar").css("display", "none");
            }
            else {
                $(".li_BoughtCar").css("display", "none");
                $(".li_NotBuyCar").css("display", "none");
            }
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
                            msg = "&nbsp;<a chistory='1' href='../TaskManager/CustInformation.aspx?CustID=" + custid + "' title='历史记录' target='_blank' class='linkBlue'><img alt='历史记录' style='vertical-align:middle;' src='/images/history.png' border='0' /></a>";
                            $("#txtTel").after(msg);
                        }
                        else if (jsonData.result == "3") {
                            //显示个人用户列表
                            var custTel = jsonData.Tel;
                            msg = "&nbsp;<a chistory='1' href='../CustBaseInfo/List.aspx?CustTel=" + custTel + "' title='历史记录' target='_blank' class='linkBlue'><img alt='历史记录' style='vertical-align:middle;' src='/images/history.png' border='0' /></a>";
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
                            项目名称：</label><span><%=ProjectName%></span></li>
                    <li>
                        <label>
                            关联需求：</label><span><input type="hidden" value="<%=model.DemandID %>" id="DemandID" /><a
                                href="javascript:void(0)" onclick="OpenDemandID('<%=string.Format(DemandDetailsUrl,model.DemandID)%>')"><%=model.DemandID %></a></span></li>
                    <li>
                        <label>
                            <span class="redColor"></span>姓名：</label><span id="spantxtCustName"><%=model.UserName%></span></li>
                    <li>
                        <label>
                            电话：</label><span id="txtTel"><%=model.Tel.Trim()%></span>&nbsp;</li>
                    <li>
                        <label>
                            <span class="redColor"></span>性别：</label><span>
                                <%if (model.Sex == 2)
                                  { 
                                %>女士
                                <%}
                                  else if (model.Sex == 1)
                                  {%>先生<%}%>
                            </span></li>
                    <li>
                        <label>
                            下单地区：</label>
                        <span>
                            <%=ProvinceName %>
                            &nbsp;<%=CityName%></span></li>
                    <li>
                        <label>
                            下单经销商：</label><span><%=model.DealerName%></span></li>
                    <li>
                        <label>
                            目标下单地区：</label>
                        <span>
                            <%=TargetProvinceName%>&nbsp;<%=TargetCityName%></span></li>
                    <li>
                        <label>
                            下单车型：</label><span><%=model.OrderCarMaster%>
                                &nbsp;
                                <%=model.OrderCarSerial%></span></li>
                    <li>
                        <label>
                            目标车型：</label><span><%=DBrand%>
                                &nbsp;
                                <%=model.DCarSerial%></span> </li>
                    <li>
                        <label>
                            下单日期：</label><span><%if (model.OrderCreateTime > (new DateTime(1900, 01, 01)))
                                                 {%><%=Convert.ToDateTime(model.OrderCreateTime).ToString("yyyy-MM-dd")%><%} %></span></li>
                    <li>
                        <label>
                            是否购车：</label>
                        <span>
                            <%if (model.IsBoughtCar == 1)
                              { 
                            %>已购车
                            <%}
                              else if (model.IsBoughtCar == 0)
                              {%>未购车<%}
                              else if (model.IsBoughtCar == 2)
                              {%>未知<%} %></span></li>
                    <li class="li_BoughtCar" style="display: block">
                        <label>
                            已购车型：</label>
                        <span>
                            <%=model.BoughtCarMaster%>&nbsp;
                            <%=model.BoughtCarSerial%></span></li>
                    <li class="li_BoughtCar" style="display: block">
                        <label>
                            购车时间：</label>
                        <span>
                            <%=strBoughtCarYear%><%=strBoughtCarMonth%></span></li>
                    <li class="li_BoughtCar" style="display: block">
                        <label>
                            购车经销商：</label>
                        <span>
                            <%=model.BoughtCarDealerName%></span> </li>

                     <li class="li_NotBuyCar" style="display: none">
                        <label>
                            意向车型：</label>
                        <span>
                            <%=model.IntentionCarMaster%>&nbsp;
                            <%=model.IntentionCarSerial%></span> </li> 
                     <li class="li_NotBuyCar" style="display: none">
                        <label>
                            预计购车时间：</label>
                        <span>
                            <%=PlanBuyCarTime%></span></li>
                   
                    <%--<li class="li_NotBuyCar" style="display: none">
                        <label>
                            购车计划：</label>
                        <span>
                            <%if (model.HasBuyCarPlan == 1)
                              { 
                            %>有
                            <%}
                              else if (model.HasBuyCarPlan == 0)
                              {%>无<%}%></span></li>--%>
                    <li class="li_NotBuyCar" style="display: none">
                        <label>
                            是否关注该品牌：</label>
                        <span>
                            <%if (model.IsAttention == 1)
                              { 
                            %>是
                            <%}
                              else if (model.IsAttention == 0)
                              {%>否<%}%></span></li>
                   <li class="li_NotBuyCar" style="display: none">
                        <label>
                            是否有经销商联系：</label>
                        <span>
                            <%if (model.IsContactedDealer == 1)
                              { 
                            %>是
                            <%}
                              else if (model.IsContactedDealer == 0)
                              {%>否<%}%></span></li>
                <%if (model.IsContactedDealer == 1)
                  { 
                            %>
                   <li class="li_NotBuyCar" style="display: none">
                        <label>
                            经销商服务是否满意：</label>
                        <span>
                            <%if (model.IsSatisfiedService == 1)
                              { 
                            %>是
                            <%}
                              else if (model.IsSatisfiedService == 0)
                              {%>否<%}%></span></li>
                   <li class="li_NotBuyCar" style="display: none">
                        <label>
                            哪家经销商联系：</label>
                         <span>
                            <%=model.ContactedWhichDealer%></span></li>

                 <%} %>


                    
                    <li>
                        <label>
                            <span class="redColor"></span>是否接通：</label>
                        <span>
                            <%if (model.IsJT == 1)
                              { 
                            %>是
                            <%}
                              else if (model.IsJT == 0)
                              {%>否<%}%></span></li>
                    <li>
                        <label>
                            <span class="redColor"></span>是否成功：</label><span><%if (model.IsSuccess == 1)
                                                                               { %>成功<%}
                                                                               else if (model.IsSuccess == 0)
                                                                               {%>失败<%}%></span> </li>
                    <li id="li_kl" style="display: none">
                        <label>
                            其他考虑车型：</label><span><%=model.ThinkCar %></span></li>
                    <%-- <li id="li_fail" style="display: none">
                        <label>
                            <span class="redColor">*</span>失败原因：</label>
                        <span>
                            <%=FailReason%></span> </li>--%>
                    <li id="liNotEstablish" style="display: none">
                        <label>
                            <span class="redColor"></span>未接通原因：</label>
                        <span>
                            <%=NotEstablishReason%></span> </li>
                    <li id="liNotSuccess" style="display: none">
                        <label>
                            <span class="redColor"></span>接通后失败原因：</label>
                        <span>
                            <%=NotSuccessReason%></span> </li>
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
