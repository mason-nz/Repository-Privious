<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="OtherTaskDeal.aspx.cs"
    Inherits="BitAuto.ISDC.CC2012.Web.OtherTask.OtherTaskDeal" %>

<%@ Register Src="UCOtherTask/OtherTaskEdit.ascx" TagName="OtherTaskEdit" TagPrefix="uc1" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>其他任务处理</title>
    <link rel="stylesheet" type="text/css" href="/css/GooCalendar.css" />
    <link href="/css/base.css" type="text/css" rel="stylesheet" />
    <link href="/css/style.css" type="text/css" rel="stylesheet" />
    <link href="/Css/cc_checkStyle.css" type="text/css" rel="stylesheet" />
    <link href="/Css/adtPopup.css" rel="stylesheet" type="text/css" />
    <script src="/Js/jquery-1.4.4.min.js" type="text/javascript"></script>
    <script type="text/javascript">
        document.writeln("<s" + "cript type='text/javascript' src='/Js/config.js?r=" + Math.random() + "'></scr" + "ipt>");
    </script>
    <script type="text/javascript">
        loadJS("common");
        loadJS("CTITool");
        loadJS("ucCommon");
        loadJS("TemplateFiledData");
        loadJS("controlParams");
        loadJS("bitdropdownlist");
        loadJS("UserControl");
    </script>
    <script src="/Js/jquery-ui.js" type="text/javascript"></script>
    <script src="/Js/Enum/Area2.js" type="text/javascript"></script>
    <script src="/Js/Enum/ProvinceCityCountry.js" type="text/javascript"></script>
    <script src="/Js/jquery.jmpopups-0.5.1.pack.js" type="text/javascript"></script>
    <script type="text/javascript" src="/js/GooCalendar.js"></script>
    <script src="/Js/json2.js" type="text/javascript"></script>
    <script src="/Js/jquery.blockUI.js" type="text/javascript"></script>
    <script type="text/javascript" src="/Js/anchor.1.0.js"></script>
    <script src="http://img1.bitauto.com/bt/Price/js/autodata/brand.js" type="text/javascript"></script>
    <script src="/Js/My97DatePicker/WdatePicker.js" type="text/javascript"></script>
    <link href="/Js/My97DatePicker/skin/WdatePicker.css" rel="stylesheet" type="text/css" />
    <!--主-->
    <script type="text/javascript">
        $(function () {
            //加载用户控件
            //数组编码格式,给数组对象做扩展
            Array.prototype.S = String.fromCharCode(2);
            Array.prototype.in_array = function (e) {
                for (var i = 0; i < this.length; i++) {
                    if (this[i] == e)
                        return true;
                }
                return false;
            }
            //跳题
            if (GotoNumQuestion) {
                GotoNumQuestion();
            }
            //初始化电话控件
            HollyPhoneControl.Init("其他任务", "<%=OtherTaskID %>", "<%=BGID %>", "<%=SCID %>", "");
            //注册接通事件
            HollyPhoneControl.SetEstablishedEvent(function (jsondata) {
                //接通后让提交按钮不可用
                $('#btnSub').attr("disabled", "disabled");
                //免打扰按钮可用
                $("#imgNodisturbTel").attr("src", "/Images/nodisturb.png").attr("disabled", false);
                //自动设置接通=是
                SetAutoEstablish();
                //保存hidCallID
                $("#hidCallID").val(jsondata.CallData.CallID);
            });
            //注册挂断保存成功事件
            HollyPhoneControl.SetReleaseEvent(function (jsondata) {
                //挂断后让提交按钮可用
                $('#btnSub').removeAttr("disabled");
                //加载录音记录
                LoadCallRecord();
                //弹层，保存个人用户信息
                SaveCustBasicInfoPoP(jsondata);
                //保存hidCallID
                $("#hidCallID").val(jsondata.CallData.CallID);
            });
            //初始化短信控件
            SendMessageControl.Init("其他任务", "<%=OtherTaskID %>", "<%=BGID %>", "<%=SCID %>", "");
            //注册回调事件
            SendMessageControl.SetSendMessageCompleteEvent(function (recid, jsondata) {
                SaveCustBasicInfoPoP(jsondata);
            });
            //如果是自动外呼
            var IsAutoCall = "<%=IsAutoCall %>";
            var AutoCallData = "<%=AutoCallData %>";
            if (IsAutoCall == "true") {
                var data = $.evalJSON(AutoCallData);
                ADTTool.AutoCallInit(data);
                //提交按钮可用
                $('#btnSub').removeAttr("disabled");
            }
            //加载其他任务
            LoadOtherTaskComplete = false;
            setTimeout(AsyncLoadOtherTask, 100);
        });
        //基本信息是否加载完成
        var LoadOtherTaskComplete = false;
        //异步加载基本信息
        function AsyncLoadOtherTask() {
            LoadOtherTask();
            //加载完成
            LoadOtherTaskComplete = true;
        }
        //设置自动接通
        function SetAutoEstablish() {
            if ($("#IsEstablish_selectid")[0] != null && $("#IsSuccess_selectid")[0] != null) {
                SetIsEstablishTrue($("#IsEstablish_selectid")[0], $("#IsSuccess_selectid")[0]);
            }
            $("#IsAutoEstablish").val("1");
        }
        //跳题逻辑
        function GotoNumQuestion() {
            //为跳题
            $("input[type='radio']").each(function () {
                if ($(this).attr("yes") != undefined && $(this).attr("yes") != "0") {
                    $(this).zxxAnchor();
                }
            })
            $("span").each(function () {
                if ($(this).attr("yes") != undefined && $(this).attr("yes") != "0") {
                    $(this).zxxAnchor();
                }
            })
        }
        //是否使用了个人模板
        function isPersonalInfo() {
            //判断是否使用了 个人用户属性模板
            if ($("#divBaseInfo li[name='FullName']").length == 0 || $("#divBaseInfo li[name='FullSex']").length == 0 || $("#divBaseInfo li[name='FullTel']").length == 0) {
                //未使用
                return false;
            }

            return true;
        }
    </script>
    <!--监控-->
    <script language="javascript" type="text/javascript">
        var monitorPageTimeWeb = new Date().getTime(); //监控页面加载耗时_开始时间
        window.onload = function () {
            if (monitorPageTime && monitorPageTime != undefined) {
                var t = new Date().getTime() - monitorPageTimeWeb;
                $.post("/AjaxServers/LoginManager.ashx", { Action: "StatPageTime", DurationTime: t, CurrentURL: '' + window.location }, function (data) {
                });
            }
        } 
    </script>
    <!--电话，短信-->
    <script type="text/javascript">
        //个人版本外呼
        function CallOutForGR(tel) {
            //var input_name = $("li[name='FullName']").find("input[type='text']");
            //var input_sex = $("li[name='FullSex']").find("input[type='radio']:checked");
            //alert(tel + " " + input_name.val() + " " + input_sex.val());

            if (tel == "") {
                $.jAlert("电话号码为空，不能外呼！");
                return;
            }
            var IsCanTel = true;
            //请求后台方法，判断是否可以打电话
            $.post("/AjaxServers/OtherTask/OtherTaskDeal.ashx",
            {
                Action: escape("CheckTelByProjectID"),
                TelePhone: escape(tel),
                TaskID: escape('<%=OtherTaskID %>'),
                r: Math.random()
            }, function (data) {
                if (data == "No") {
                    IsCanTel = false;
                }
                if (IsCanTel) {
                    //注册个人用户信息获取方法
                    HollyPhoneControl.SetInfoFunc(
                    //客户类型
                    function () {
                        return "<%=(int)BitAuto.ISDC.CC2012.Entities.CustTypeEnum.T01_个人 %>";
                    },
                    //客户姓名
                    function () {
                        var input_name = $("li[name='FullName']").find("input[type='text']");
                        return $.trim(input_name.val());
                    },
                    //客户性别
                    function () {
                        var input_sex = $("li[name='FullSex']").find("input[type='radio']:checked");
                        return $.trim(input_sex.val());
                    },
                    //经销商id
                    function () {
                        return "";
                    },
                    //经销商名称
                    function () {
                        return "";
                    },
                    //crm客户id
                    ""
                    );
                    //外呼
                    HollyPhoneControl.CallOut(tel);
                }
                else {
                    $.jConfirm("该号码为免打扰号码，禁止进行外呼<br/>点击”确定“按钮，任务自动关闭", function (r) {
                        if (r) {
                            $("#IsEstablish_selectid").val("0");
                            $("#IsEstablish_selectid").attr("disabled", "disabled");
                            $("#NotEstablishReason_label").css("display", "block");
                            $("#NotEstablishReason_selectid").val('<%=BlackWhiteList%>');
                            $("#NotEstablishReason_selectid").css("display", "block");
                            $("#NotEstablishReason_selectid").attr("disabled", "disabled");
                            $("#IsSuccess_label").css("display", "block");
                            $("#IsSuccess_selectid").css("display", "block");
                            $("#IsSuccess_selectid").val("0");
                            $("#IsSuccess_selectid").attr("disabled", "disabled");
                            SubOtherTaskInfo();
                        }
                    });
                }
            });
        }
        //个人版本短信
        function SendSmSForGR(tel) {
            //注册个人用户信息获取方法
            SendMessageControl.SetInfoFunc(
            //客户类型
            function () {
                return "<%=(int)BitAuto.ISDC.CC2012.Entities.CustTypeEnum.T01_个人 %>";
            },
            //客户姓名
            function () {
                var input_name = $("li[name='FullName']").find("input[type='text']");
                return $.trim(input_name.val());
            },
            //客户性别
            function () {
                var input_sex = $("li[name='FullSex']").find("input[type='radio']:checked");
                return $.trim(input_sex.val());
            },
            //经销商id
            function () {
                return "";
            },
            //经销商名称
            function () {
                return "";
            },
            //crm客户id
            ""
            );
            //清空SetTemplateFunc的设置
            SendMessageControl.SetTemplateFunc(null, null, null);
            //短信（有模板发送方式）
            SendMessageControl.BtnSendMessageClick(tel, 2);
        }
    </script>
</head>
<body>
    <form id="form1" runat="server">
    <!--是否接通了-->
    <input type="hidden" id="IsAutoEstablish" />
    <!--使用了CRM模板且已加载历史记录图标-->
    <input type="hidden" id="hidIsUseCRM" value="0" />
    <div class="w980">
        <uc1:OtherTaskEdit ID="OtherTaskEdit1" runat="server" />
        <div class="btn" style="clear: both;">
            <input type="button" id="btnConfirm" onclick="javascript:SaveOtherTaskInfo();" class="button"
                value="保存" />
            <input type="button" id="btnSub" onclick="javascript:SubOtherTaskInfo();" class="button"
                value="提交" />
        </div>
    </div>
    </form>
</body>
</html>
