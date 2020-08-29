<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="TemplateView.aspx.cs" Inherits="BitAuto.ISDC.CC2012.Web.TemplateManagement.TemplateView" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http：//www.w3.org/1999/xhtml">
<head>
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title>模版预览</title>
    <script src="/Js/jquery-1.4.4.min.js" type="text/javascript"></script>
    <script src="/Js/jquery-ui.js" type="text/javascript"></script>
    <script src="/Js/Enum/Area.js" type="text/javascript"></script>
    <script src="/Js/Enum/Area2.js" type="text/javascript"></script>
    <script src="/Js/Enum/ProvinceCityCountry.js" type="text/javascript"></script>
    <script src="/Js/jquery.jmpopups-0.5.1.pack.js" type="text/javascript"></script>
    <script type="text/javascript" src="/js/GooCalendar.js"></script>
    <link rel="stylesheet" type="text/css" href="/css/GooCalendar.css" />
    <script src="http://img1.bitauto.com/bt/Price/js/autodata/brand.js" type="text/javascript"></script>
    <script type="text/javascript">
        document.writeln("<s" + "cript type='text/javascript' src='/Js/config.js?r=" + Math.random() + "'></scr" + "ipt>");
    </script>
    <script type="text/javascript">
        loadJS("common");
        loadJS("TemplateFiledData");
        loadJS("controlParams");
        loadJS("bitdropdownlist");
    </script>
    <link href="/css/base.css" type="text/css" rel="stylesheet" />
    <link href="/css/style.css" type="text/css" rel="stylesheet" />
    <script type="text/javascript">
        $(function () {
            var TTCode = '<%=TTCode %>';
            if (TTCode != "") {
                BindFields(TTCode);
            }
        });

        function BindFields(ttcode) {
            var isShowBtn = '<%=IsShowBtn %>';
            var IsShowWorkOrderBtn = '<%=IsShowWorkOrderBtn %>';
            var IsShowSendMsgBtn = '<%=IsShowSendMsgBtn %>';
            AjaxPost('/AjaxServers/TemplateManagement/GetFieldList.ashx', { ttcode: ttcode }, null, function (data) {
                var isLoadCustShow = 0;
                var jsonData = $.evalJSON(data);
                $(jsonData).each(function (i, v) {
                    GetHtmlByShowCode(this, function (returnData, html) {
                        if (returnData.TFShowCode == "100020") {
                            //模板End-话务结果
                            $("#divCallResult").append(html);
                            $("#divCallResult li[name='" + returnData.TFName + "']").data(returnData);
                        }
                        else {
                            $("#divBaseInfo").append(html);
                            $("#divBaseInfo li").last().data(returnData);
                            if (returnData.TFShowCode == "100014") {
                                $("#divCrmBlock").show();
                                $("#divCrmBaseInfo").load("/TemplateManagement/CustInfoView.aspx", { liWidth: "370px;" });
                                $("#divBaseInfo li").first().hide();
                                isLoadCustShow = 1;
                                if (isShowBtn == 1) {
                                    $("#crmHrefAddRecord").css("display", "");
                                }
                                if (IsShowWorkOrderBtn == 1) {
                                    $("#crmHrefAddWorkOrder").css("display", "");
                                }
                                if (IsShowSendMsgBtn == 1) {
                                    $("#crmHrefSendMsg").css("display", "");
                                }
                            }
                            else if (returnData.TFShowCode == "100015") {
                                //如果是个人用户
                                $("#divBaseInfo li").last().attr("code", "100015");
                            }
                        }
                    });
                });

                if (isLoadCustShow == 1) { //如果有CRM区域，按钮显示在CRM区域
                    $.post("/AjaxServers/OtherTask/OtherTaskDeal.ashx", { Action: "IsShowBtnByTTCode", TTCode: ttcode }, function (data) {
                        var jsonData = $.evalJSON(data);
                        if (jsonData.result == "true") {
                            if (jsonData.IsShowBtn == "true") {
                                $("#crmHrefAddRecord").css("display", "");
                            }
                            if (jsonData.IsShowWorkOrderBtn == "true") {
                                $("#crmHrefAddWorkOrder").css("display", "");
                            }
                            if (jsonData.IsShowSendMsgBtn == "true") {
                                $("#crmHrefSendMsg").css("display", "");
                            }
                        }
                    });
                }
                else { //如果没有CRM区域，按钮显示在基本信息区域
                    $.post("/AjaxServers/OtherTask/OtherTaskDeal.ashx", { Action: "IsShowBtnByTTCode", TTCode: ttcode }, function (data) {
                        var jsonData = $.evalJSON(data);
                        if (jsonData.result == "true") {
                            if (jsonData.IsShowWorkOrderBtn == "true") {
                                $("#hrefAddWorkOrder").css("display", "");
                            }
                            if (jsonData.IsShowSendMsgBtn == "true") {
                                $("#hrefSendMsg").css("display", "");
                            }
                        }
                    });
                }

                //显示TIP
                ShowTitle(jsonData);
            });
        }

        function ShowTitle(jsonData) {
            $(jsonData).each(function (i, v) {
                switch (v.TFShowCode) {
                    case '100001':
                        $("#" + v.TFName).attr("title", "单行文本");
                        break;
                    case '100002':
                        $("#" + v.TFName).attr("title", "多行文本");
                        break;
                    case '100005':
                        $("#" + v.TFName).attr("title", "下拉");
                        break;
                    case '100006':
                        $("#" + v.TFName).attr("title", "电话号码");
                        break;
                    case '100007':
                        $("#" + v.TFName).attr("title", "邮箱");
                        break;
                    case '100008':
                        $("#" + v.TFName).attr("title", "日期点");
                        break;
                    case '100010':
                        $("#" + v.TFName).attr("title", "时间点");
                        break;
                }
            });
        }
    </script>
    <%--  公用方法--%>
    <script type="text/javascript">
        var property2 = {
            divId: "calen1", //日历控件最外层DIV的ID
            needTime: true, //是否需要显示精确到秒的时间选择器，即输出时间中是否需要精确到小时：分：秒 默认为FALSE可不填
            yearRange: [1970, 2030], //可选年份的范围,数组第一个为开始年份，第二个为结束年份,如[1970,2030],可不填
            //week: ['Su', 'Mo', 'Tu', 'We', 'Th', 'Fr', 'Sa'], //数组，设定了周日至周六的显示格式,可不填
            //month: ['January', 'February', 'March', 'April', 'May', 'June', 'July', 'August', 'September', 'October', 'November', 'December'], //数组，设定了12个月份的显示格式,可不填
            format: "yyyy-MM-dd hh:mm:ss"
            /*设定日期的输出格式,可不填*/
        };

    
    </script>
</head>
<body>
    <div class="w980">
        <div class="taskT">
            <%=TPageName %></div>
        <!--crm客户开始-->
        <div class="baseInfo clearfix" id="divCrmBlock" style="width: 1000px; display: none;">
            <!--左边开始-->
            <div class="mbInfo">
                Crm客户基本信息<a id="crmHrefAddRecord" style="float: right; margin-right: 40px; *margin-top: -30px;
                    display: none;" href="javascript:void(0);"> 添加回访记录 </a><a id="crmHrefAddWorkOrder"
                        style="float: right; margin-right: 40px; *margin-top: -30px; display: none;"
                        href="javascript:void(0);">添加工单 </a><a id="crmHrefSendMsg" style="float: right; margin-right: 40px;
                            *margin-top: -30px; display: none;" href="javascript:void(0);">发送短信
                </a>
            </div>
            <div class="clearfix" id='divCrmBaseInfo'>
            </div>
            <!--左边结束-->
            <div class="clear">
            </div>
        </div>
        <!--crm客户结束-->
        <!--模版开始-->
        <div class="editTemplate readTemplate">
            <!--左边开始-->
            <div class="mbInfo">
                基本信息<a id="hrefAddRecord" style="float: right; margin-right: 40px; *margin-top: -30px;
                    display: none;" href="javascript:void(0);"> 添加回访记录 </a><a id="hrefAddWorkOrder" style="float: right;
                        margin-right: 40px; *margin-top: -30px; display: none;" href="javascript:void(0);">
                        添加工单 </a><a id="hrefSendMsg" style="float: right; margin-right: 40px; *margin-top: -30px;
                            display: none;" href="javascript:void(0);">发送短信 </a>
            </div>
            <div class="clearfix">
                <ul id='divBaseInfo'>
                </ul>
                <div class="clear">
                </div>
                <ul id="divCallResult" class="clear" style="border-top: #999 1px dotted; height: 100px;">
                </ul>
                <div class="clear">
                </div>
            </div>
            <!--左边结束-->
            <div class="clear">
            </div>
        </div>
        <!--模版结束-->
    </div>
</body>
</html>
