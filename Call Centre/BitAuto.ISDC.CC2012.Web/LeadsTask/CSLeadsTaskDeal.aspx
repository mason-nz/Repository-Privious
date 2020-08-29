<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="CSLeadsTaskDeal.aspx.cs"
    Inherits="BitAuto.ISDC.CC2012.Web.LeadsTask.CSLeadsTaskDeal" %>

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
    <script src="../Js/json2.js" type="text/javascript"></script>
    <script src="../Js/Enum/Area2.js" type="text/javascript"></script>
    <script src="../Js/jquery.blockUI.js" type="text/javascript"></script>
    <script type="text/javascript">
        document.writeln("<s" + "cript type='text/javascript' src='/Js/config.js?r=" + Math.random() + "'></scr" + "ipt>");
    </script>
    <script type="text/javascript">
        loadJS("common");
        loadJS("CTITool");
        loadJS("controlParams");
        loadJS("TemplateFiledData");
        loadJS("bitdropdownlist");
        loadJS("UserControl");
    </script>
    <script type="text/javascript">
        //默认可以外呼
        var IsCanTel = true;
        //外呼
        function CallOutForGR() {
            CheckTel();
            if (IsCanTel) {
                var tel = $.trim($("#txtTel").text())
                //注册个人用户信息获取方法
                HollyPhoneControl.SetInfoFunc(
                //客户类型
                    function () {
                        return "<%=(int)BitAuto.ISDC.CC2012.Entities.CustTypeEnum.T01_个人 %>";
                    },
                //客户姓名
                    function () {
                        //姓名
                        var username = $.trim($("input[id$='spantxtCustName']").val());
                        return $.trim(username);
                    },
                //客户性别
                    function () {
                        //性别
                        var Sex = $("[name$='sex']").map(function () {
                            if ($(this).attr("checked")) {
                                return $(this).val();
                            }
                        }).get().join(',');
                        return $.trim(Sex);
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
                        $("#NotEstablishReason_selectid").val('<%=BlackWhiteList%>');
                        $("#IsSuccess_selectid").val("0");
                        SubmitInfo();
                    }
                });
            }
        }
        //加载电话是否可以外呼
        function CheckTel() {
            //同步查询
            var tel = $.trim($("#txtTel").text())
            AjaxPostAsync("/LeadsTask/AjaxServers/Handler.ashx", { Action: escape("checktel"), Tel: escape(tel), r: Math.random() }, null, function (data) {
                var jsonData = eval("(" + data + ")");
                if (jsonData.result == "No") {
                    IsCanTel = false;
                }
                else {
                    IsCanTel = true;
                }
            });
        }

        $(document).ready(function () {
            //加载地区
            $("#<%=this.ddlTargetProvince.ClientID%>").val('<%=model.TargetProvinceID%>');
            CustBaseInfoHelper2.TriggerProvince();
            $("#<%=this.ddlTargetCity.ClientID%>").val('<%=model.TargetCityID%>');
            //如果失败，显示失败原因
            if ($("input[id$='rdoFail']").attr("checked")) {
                $("#li_fail").css("display", "");
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
            //添加是否接通等四个字段
            bindIsPassData();
            //初始化电话控件
            HollyPhoneControl.Init("厂家集客", "<%=TaskID %>", "<%=BGID %>", "<%=SCID %>", "");
            //注册接通事件
            HollyPhoneControl.SetEstablishedEvent(function (jsondata) {
                //接通后让提交按钮不可用
                $('#btnSubmit').attr("disabled", "disabled");
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
                $('#btnSubmit').attr("disabled", "");
                //加载录音记录
                BindCallReocrd();
                //保存hidCallID
                $("#hidCallID").val(jsondata.CallData.CallID);
            });
        });


        function SetAutoEstablish() {
            //自动设置接通=是
            if ($("#IsEstablish_selectid")[0] != null && $("#IsSuccess_selectid")[0] != null) {
                SetIsEstablishTrue($("#IsEstablish_selectid")[0], $("#IsSuccess_selectid")[0]);
            }
        }

        var CustBaseInfoHelper2 = (function () {
            var triggerProvince = function () {//选中省份
                BindCity('<%=this.ddlTargetProvince.ClientID%>', '<%=this.ddlTargetCity.ClientID%>');
            }
            return {
                TriggerProvince: triggerProvince
            }
        })();

        //页面交互使失败原因隐藏显示
        function IsSuccessChange() {
            if ($("input[id$='rdoSuccess']").attr("checked")) {
                $("#li_fail").css("display", "none");
                $("#txtThinkCar").val("");
                $("select[id$='selFailReson']").val("-2");
            }
            else if ($("input[id$='rdoFail']").attr("checked")) {
                $("#li_fail").css("display", "");
            }
        }
        //绑定话务记录
        function BindCallReocrd() {
            LoadingAnimation("divCallRecordList");
            $("#divCallRecordList").load("../AjaxServers/LeadsTask/LeadTaskCallRecordList.aspx", { TaskID: '<%=TaskID%>', r: Math.random() });
        }
        //绑定历史记录
        function BindHistory() {
            LoadingAnimation("divTaskLog");
            $("#divTaskLog").load("../AjaxServers/LeadsTask/LeadsTaskOperationLog.aspx", { TaskID: '<%=TaskID%>', r: Math.random() });
        }
        function SaveInfo() {
            if (DataCheck()) {
                //姓名
                var username = $.trim($("input[id$='spantxtCustName']").val());
                //备注
                var Remark = $.trim($("#Remark").val());
                //考虑车型
                var ThinkCar = $.trim($("#txtThinkCar").val());

                var isSuccess = $("#IsSuccess_selectid").val();
                //未接通原因 add
                var notEstablishReason = $("#NotEstablishReason_selectid").val();
                //失败原因 add
                var notSuccessReason = $("#NotSuccessReason_selectid").val();

                //目标下单地区省
                var selProvince = $("select[id$='<%=this.ddlTargetProvince.ClientID%>']").val();
                //目标下单地区市
                var selCity = $("select[id$='<%=this.ddlTargetCity.ClientID%>']").val();


                //性别
                var Sex = $("[name$='sex']").map(function () {
                    if ($(this).attr("checked")) {
                        return $(this).val();
                    }
                }).get().join(',');

                var IsJT = $("#IsEstablish_selectid").val();
                //目标配车型ID，车型name
                var seldllDSerialID = $("select[id$='dllCarSerial']").val();
                var seldllDSerialName = $("select[id$='dllCarSerial'] option:selected").text();

                //任务ID
                var TaskID = '<%=TaskID%>';
                //**********************
                //是否购车
                var isBuyCar = $("[name$='rdIsBuyCar']").map(function () {
                    if ($(this).attr("checked")) {
                        return $(this).val();
                    }
                }).get().join(',');

                var boughtCarMasterID = "";
                var boughtCarMasterName = "";
                var boughtSerialID = "";
                var boughtSerialName = "";
                var boughtYear = "";
                var boughtMonth = "";
                var memberCode = "";
                var memberName = "";

                //var hasBuyCarPlan = "";
                var isAttention = "";
                var isContactedDealer = "";
                var isSatisfiedService = "";
                var txtContactedWhichDealer = "";


                var selPBuyCarTime = "";
                var intentionCarMasterID = "";
                var intentionCarMaster = "";
                var intentionCarSerialID = "";
                var intentionCarSerial = "";

                if (isBuyCar == "1") {
                    //已购车品牌ID，已购车品牌name
                    boughtCarMasterID = $("select[id$='selBoughtCarMaster']").val();
                    if (boughtCarMasterID == "0") {
                        boughtCarMasterName = "";
                    }
                    else {
                        boughtCarMasterName = $("select[id$='selBoughtCarMaster'] option:selected").text();
                    }
                    //已购车系列ID，已购车系列name
                    boughtSerialID = $("select[id$='selBoughtSerial']").val();
                    if (boughtSerialID == "0") {
                        boughtSerialName = "";
                    }
                    else {
                        boughtSerialName = $("select[id$='selBoughtSerial'] option:selected").text();
                    }
                    //购车时间
                    boughtYear = $("select[id$='selBoughtYear']").val();
                    boughtMonth = $("select[id$='selBoughtMonth']").val();
                    //购车经销商
                    memberCode = $.trim($("#hidMemberCode").val());
                    memberName = $.trim($("#txt_MemberName").val());
                    //hasBuyCarPlan = "";
                    isAttention = "";
                    isContactedDealer = "";
                    isSatisfiedService = "";
                    txtContactedWhichDealer = "";
                    selPBuyCarTime = "";
                    intentionCarMasterID = "";
                    intentionCarMaster = "";
                    intentionCarSerialID = "";
                    intentionCarSerial = "";
                }
                else if (isBuyCar == "0") {
                    isAttention = $("[name$='IsAttention']").map(function () {
                        if ($(this).attr("checked")) {
                            return $(this).val();
                        }
                    }).get().join(',');
                    isContactedDealer = $("[name$='IsContactedDealer']").map(function () {
                        if ($(this).attr("checked")) {
                            return $(this).val();
                        }
                    }).get().join(',');
                    isSatisfiedService = $("[name$='IsSatisfiedService']").map(function () {
                        if ($(this).attr("checked")) {
                            return $(this).val();
                        }
                    }).get().join(',');
                    txtContactedWhichDealer = $("#txtContactedWhichDealer").val();


                    //预计购车时间
                    selPBuyCarTime = $("select[id$='selWantBuyCarTime']").val();
                    //意向车品牌ID，意向车品牌name
                    intentionCarMasterID = $("select[id$='selIntentionCarMaster']").val();
                    if (intentionCarMasterID == "0") {
                        intentionCarMaster = "";
                    }
                    else {
                        intentionCarMaster = $("select[id$='selIntentionCarMaster'] option:selected").text();
                    }
                    //意向车系列ID，意向车系列name
                    intentionCarSerialID = $("select[id$='selIntentionCarSerialName']").val();
                    if (intentionCarSerialID == "0") {
                        intentionCarSerial = "";
                    }
                    else {
                        intentionCarSerial = $("select[id$='selIntentionCarSerialName'] option:selected").text();
                    }

                    boughtCarMasterID = "";
                    boughtCarMasterName = "";
                    boughtSerialID = "";
                    boughtSerialName = "";
                    boughtYear = "";
                    boughtMonth = "";
                    memberCode = "";
                    memberName = "";
                }
                //**********************
                AjaxPost("../AjaxServers/LeadsTask/CSLeadsTaskDeal.ashx", {
                    Action: escape("saveinfo"),
                    ProjectID: escape('<%=model.ProjectID%>'),
                    TaskID: escape(TaskID),
                    IsSuccess: escape(isSuccess),
                    //FailReson: escape(selFailReson),

                    //add 未接通原因和接通后失败原因
                    NotEstablishReason: escape(notEstablishReason),
                    NotSuccessReason: escape(notSuccessReason),

                    Remark: escape(Remark),
                    GuidStr: escape('<%=model.RelationID.ToString()%>'),
                    SEX: escape(Sex),
                    IsJT: escape(IsJT),
                    UserName: encodeURIComponent(username),
                    MobilePhone: escape('<%=model.Tel%>'),
                    CityID: escape(selCity),
                    ProvinceID: escape(selProvince),
                    ThinkCar: escape(ThinkCar),
                    DSerialID: escape(seldllDSerialID),
                    DSerialName: escape(seldllDSerialName),
                    DemandID: escape('<%=model.DemandID%>'),

                    IsBuyCar: escape(isBuyCar),

                    BoughtCarMasterID: escape(boughtCarMasterID),
                    BoughtCarMasterName: encodeURIComponent(boughtCarMasterName),
                    BoughtSerialID: escape(boughtSerialID),
                    BoughtSerialName: encodeURIComponent(boughtSerialName),
                    BoughtYearMonth: encodeURIComponent(boughtYear + "," + boughtMonth),
                    BuyCarMemberCode: escape(memberCode),
                    BuyCarMemberName: encodeURIComponent(memberName),

                    //HasBuyCarPlan: escape(hasBuyCarPlan),
                    IsAttention: escape(isAttention),
                    IsContactedDealer: escape(isContactedDealer),
                    IsSatisfiedService: escape(isSatisfiedService),
                    ContactedWhichDealer: escape(txtContactedWhichDealer),


                    PBuyCarTime: escape(selPBuyCarTime),
                    IntentionCarMasterID: escape(intentionCarMasterID),
                    IntentionCarMaster: encodeURIComponent(intentionCarMaster),
                    IntentionCarSerialID: escape(intentionCarSerialID),
                    IntentionCarSerial: encodeURIComponent(intentionCarSerial)
                }, null,
            function (data) {
                var jsonData = $.evalJSON(data);
                //不成功提示错误，成功把录音主键保存在隐藏域里面
                if (jsonData.Result == false) {
                    $.jAlert(jsonData.Msg);
                }
                else {
                    BindHistory();
                    //                    $.jAlert("保存成功！");
                    $.jPopMsgLayer("保存成功！");

                }
            });
            }
        }

        function SubmitInfo() {
            //验证数据
            if (DataCheck()) {
                //姓名
                var username = $.trim($("input[id$='spantxtCustName']").val());
                //备注
                var Remark = $.trim($("#Remark").val());
                //其他考虑车型
                var ThinkCar = $.trim($("#txtThinkCar").val());


                //目标下单地区省
                var selProvince = $("select[id$='<%=this.ddlTargetProvince.ClientID%>']").val();
                //目标下单地区市
                var selCity = $("select[id$='<%=this.ddlTargetCity.ClientID%>']").val();


                //性别
                var Sex = $("[name$='sex']").map(function () {
                    if ($(this).attr("checked")) {
                        return $(this).val();
                    }
                }).get().join(',');
                //是否接通
                var IsJT = $("#IsEstablish_selectid").val();
                //是否成功
                var isSuccess = $("#IsSuccess_selectid").val();
                //需匹配车型ID，车型name
                var seldllDSerialID = $("select[id$='dllCarSerial']").val();
                var seldllDSerialName = $("select[id$='dllCarSerial'] option:selected").text();
                //预计购车时间
                var selPBuyCarTime = $("select[id$='selWantBuyCarTime']").val();
                //失败原因
                //var selFailReson = $("select[id$='selFailReson']").val();

                var notEstablishReason = $("#NotEstablishReason_selectid").val();
                //失败原因 add
                var notSuccessReason = $("#NotSuccessReason_selectid").val();

                //任务ID
                var TaskID = '<%=TaskID%>';

                //**********************
                //是否购车
                var isBuyCar = $("[name$='rdIsBuyCar']").map(function () {
                    if ($(this).attr("checked")) {
                        return $(this).val();
                    }
                }).get().join(',');

                var boughtCarMasterID = "";
                var boughtCarMasterName = "";
                var boughtSerialID = "";
                var boughtSerialName = "";
                var boughtYear = "";
                var boughtMonth = "";
                var memberCode = "";
                var memberName = "";

                //var hasBuyCarPlan = "";
                var isAttention = "";
                var isContactedDealer = "";
                var isSatisfiedService = "";
                var txtContactedWhichDealer = "";

                var selPBuyCarTime = "";
                var intentionCarMasterID = "";
                var intentionCarMaster = "";
                var intentionCarSerialID = "";
                var intentionCarSerial = "";

                if (isBuyCar == "1") {
                    //已购车品牌ID，已购车品牌name
                    boughtCarMasterID = $("select[id$='selBoughtCarMaster']").val();
                    if (boughtCarMasterID == "0") {
                        boughtCarMasterName = "";
                    }
                    else {
                        boughtCarMasterName = $("select[id$='selBoughtCarMaster'] option:selected").text();
                    }
                    //已购车系列ID，已购车系列name
                    boughtSerialID = $("select[id$='selBoughtSerial']").val();
                    if (boughtSerialID == "0") {
                        boughtSerialName = "";
                    }
                    else {
                        boughtSerialName = $("select[id$='selBoughtSerial'] option:selected").text();
                    }
                    //购车时间
                    boughtYear = $("select[id$='selBoughtYear']").val();
                    boughtMonth = $("select[id$='selBoughtMonth']").val();
                    //购车经销商
                    memberCode = $.trim($("#hidMemberCode").val());
                    memberName = $.trim($("#txt_MemberName").val());

                    //hasBuyCarPlan = "";
                    isAttention = "";
                    isContactedDealer = "";
                    isSatisfiedService = "";
                    txtContactedWhichDealer = "";

                    selPBuyCarTime = "";
                    intentionCarMasterID = "";
                    intentionCarMaster = "";
                    intentionCarSerialID = "";
                    intentionCarSerial = "";
                }
                else if (isBuyCar == "0") {
                    isAttention = $("[name$='IsAttention']").map(function () {
                        if ($(this).attr("checked")) {
                            return $(this).val();
                        }
                    }).get().join(',');
                    isContactedDealer = $("[name$='IsContactedDealer']").map(function () {
                        if ($(this).attr("checked")) {
                            return $(this).val();
                        }
                    }).get().join(',');
                    isSatisfiedService = $("[name$='IsSatisfiedService']").map(function () {
                        if ($(this).attr("checked")) {
                            return $(this).val();
                        }
                    }).get().join(',');
                    txtContactedWhichDealer = $("#txtContactedWhichDealer").val();

                    //预计购车时间
                    selPBuyCarTime = $("select[id$='selWantBuyCarTime']").val();
                    //意向车品牌ID，意向车品牌name
                    intentionCarMasterID = $("select[id$='selIntentionCarMaster']").val();
                    if (intentionCarMasterID == "0") {
                        intentionCarMaster = "";
                    }
                    else {
                        intentionCarMaster = $("select[id$='selIntentionCarMaster'] option:selected").text();
                    }
                    //意向车系列ID，意向车系列name
                    intentionCarSerialID = $("select[id$='selIntentionCarSerialName']").val();
                    if (intentionCarSerialID == "0") {
                        intentionCarSerial = "";
                    }
                    else {
                        intentionCarSerial = $("select[id$='selIntentionCarSerialName'] option:selected").text();
                    }

                    boughtCarMasterID = "";
                    boughtCarMasterName = "";
                    boughtSerialID = "";
                    boughtSerialName = "";
                    boughtYear = "";
                    boughtMonth = "";
                    memberCode = "";
                    memberName = "";
                }
                //**********************

                $.blockUI({ message: "正在提交中，请等待..." });
                AjaxPost("../AjaxServers/LeadsTask/CSLeadsTaskDeal.ashx", {
                    Action: escape("subinfo"),
                    ProjectID: escape('<%=model.ProjectID%>'),
                    TaskID: escape(TaskID),
                    IsSuccess: escape(isSuccess),
                    //add 未接通原因和接通后失败原因
                    NotEstablishReason: escape(notEstablishReason),
                    NotSuccessReason: escape(notSuccessReason),

                    Remark: escape(Remark),
                    GuidStr: escape('<%=model.RelationID.ToString()%>'),
                    SEX: escape(Sex),
                    IsJT: escape(IsJT),
                    UserName: encodeURIComponent(username),
                    MobilePhone: escape('<%=model.Tel%>'),
                    CityID: escape(selCity),
                    ProvinceID: escape(selProvince),
                    PBuyCarTime: escape(selPBuyCarTime),
                    ThinkCar: escape(ThinkCar),
                    DSerialID: escape(seldllDSerialID),
                    DSerialName: escape(seldllDSerialName),
                    DemandID: escape('<%=model.DemandID%>'),
                    BGID: escape('<%=BGID %>'),
                    SCID: escape('<%=SCID %>'),
                    DCarMaster: escape('<%=DBrand%>'),
                    DCarMasterID: escape('<%=DBrandID%>'),

                    IsBuyCar: escape(isBuyCar),

                    BoughtCarMasterID: escape(boughtCarMasterID),
                    BoughtCarMasterName: encodeURIComponent(boughtCarMasterName),
                    BoughtSerialID: escape(boughtSerialID),
                    BoughtSerialName: encodeURIComponent(boughtSerialName),
                    BoughtYearMonth: encodeURIComponent(boughtYear + "," + boughtMonth),
                    BuyCarMemberCode: escape(memberCode),
                    BuyCarMemberName: encodeURIComponent(memberName),

                    //HasBuyCarPlan: escape(hasBuyCarPlan),
                    IsAttention: escape(isAttention),
                    IsContactedDealer: escape(isContactedDealer),
                    IsSatisfiedService: escape(isSatisfiedService),
                    ContactedWhichDealer: escape(txtContactedWhichDealer),

                    PBuyCarTime: escape(selPBuyCarTime),
                    IntentionCarMasterID: escape(intentionCarMasterID),
                    IntentionCarMaster: encodeURIComponent(intentionCarMaster),
                    IntentionCarSerialID: escape(intentionCarSerialID),
                    IntentionCarSerial: encodeURIComponent(intentionCarSerial)
                }, null,
            function (data) {
                $.unblockUI();
                var jsonData = $.evalJSON(data);
                //不成功提示错误
                if (jsonData.Result == false) {
                    $.jAlert(jsonData.Msg);
                }
                else {
                    $.jPopMsgLayer("提交成功！", function () {
                        closePageExecOpenerSearch();
                    });
                }
            });
            }
        }

        function DataCheck() {
            //目标下单地区省
            var selProvince = $("select[id$='<%=this.ddlTargetProvince.ClientID%>']").val();
            //目标下单地区市
            var selCity = $("select[id$='<%=this.ddlTargetCity.ClientID%>']").val();

            //姓名
            var username = $.trim($("input[id$='spantxtCustName']").val());
            //备注
            var Remark = $.trim($("#Remark").val());
            //其他考虑车型
            var ThinkCar = $.trim($("#txtThinkCar").val());
            //性别
            var Sex = $("[name$='sex']").map(function () {
                if ($(this).attr("checked")) {
                    return $(this).val();
                }
            }).get().join(',');
            var IsJT = $("#IsEstablish_selectid").val();

            var isSuccess = $("#IsSuccess_selectid").val();
            //需匹配车款
            var dllCarSerial = $("select[id$='dllCarSerial']").val();

            //未接通原因 add
            var notEstablishReason = $("#NotEstablishReason_selectid").val();
            //失败原因 add
            var notSuccessReason = $("#NotSuccessReason_selectid").val();
            //失败原因
            //var selFailReson = $("select[id$='selFailReson']").val();

            if (Len(username) == 0) {
                $.jAlert("请输入姓名！");
                return false;
            }
            if (Len(username) > 50) {
                $.jAlert("姓名超长！");
                return false;
            }
            if (Len(Sex) == 0) {
                $.jAlert("请选择性别！");
                return false;
            }
            if (IsJT == "-1") {
                $.jAlert("请选择是否接通！");
                return false;
            }
            if (isSuccess == "-1" && $("#IsSuccess_selectid").css("display") != "none") {
                $.jAlert("请选择是否成功！");
                return false;
            }
            if ($(".li_NotBuyCar").css("display") != "none") {
                if ($.trim($("#txtContactedWhichDealer").val()).length > 10) {
                    $.jAlert("“哪家经销商联系”字段超长（限长10个字符）!");
                    return false;
                }
            }
            if (notEstablishReason == "-1" && $("#NotEstablishReason_selectid").css("display") != "none") {
                $.jAlert("请选择未接通原因！");
                return false;
            }

            if (notSuccessReason == "-1" && $("#NotSuccessReason_selectid").css("display") != "none") {
                $.jAlert("请选择接通后失败原因！");
                return false;
            }
            //            }
            if (Len(ThinkCar) > 50) {
                $.jAlert("考虑车型超长！");
                return false;
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

            var tels = "<%=model.Tel%>";
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
                            $("#imgTel").after(msg);
                        }
                        else if (jsonData.result == "3") {
                            //显示个人用户列表
                            var custTel = jsonData.Tel;
                            msg = "&nbsp;<a chistory='1' href='../CustBaseInfo/List.aspx?CustTel=" + custTel + "' title='历史记录' target='_blank' class='linkBlue'><img style='vertical-align:middle;' alt='历史记录' src='/images/history.png' border='0' /></a>";
                            $("#imgTel").after(msg);
                        }
                    }
                });
            });

        }

        function AppendEndHtml() {
            GetHtmlByShowCode({ TFShowCode: "100020", TFDesName: "是否接通", TFDes: "是否接通", TFName: "IsEstablish", TFValue: "<%=BoolStr %>" }, function (returnData, html) {
                $("#li_KL").before(html);
                $("#IsEstablish_selectid").attr("class", "w250");
                $("#ulTask li[name='" + returnData.TFName + "']").css("margin-top", "10px");

            });
            GetHtmlByShowCode({ TFShowCode: "100020", TFDesName: "未接通原因", TFDes: "未接通原因", TFName: "NotEstablishReason", TFValue: "<%=ALLNotEstablishReasonStr %>" }, function (returnData, html) {
                $("#li_KL").before(html);
                $("#ulTask li[name='" + returnData.TFName + "']").css("margin-top", "10px");
                $("#NotEstablishReason_selectid").attr("class", "w250");
            });


            GetHtmlByShowCode({ TFShowCode: "100020", TFDesName: "是否成功", TFDes: "是否成功", TFName: "IsSuccess", TFValue: "<%=BoolStr %>" }, function (returnData, html) {
                $("#li_KL").before(html);
                returnData.TFSortIndex = 103;
                returnData.TFIsExportShow = "1";
                returnData.TFInportIsNull = "1";
                returnData.TFIsNull = "0";
                $("#ulTask li[name='" + returnData.TFName + "']").data(returnData);
                $("#IsSuccess_selectid").attr("class", "w250");

            });
            GetHtmlByShowCode({ TFShowCode: "100020", TFDesName: "失败原因", TFDes: "失败原因", TFName: "NotSuccessReason", TFValue: "<%=ALLNotSuccessReasonStr %>" }, function (returnData, html) {
                $("#li_KL").before(html);
                returnData.TFSortIndex = 104;
                returnData.TFIsExportShow = "1";
                returnData.TFInportIsNull = "1";
                returnData.TFIsNull = "0";
                $("#ulTask li[name='" + returnData.TFName + "']").data(returnData);
                $("#NotSuccessReason_selectid").attr("class", "w250");
            });
        }
        //绑定 未接通原因等四个字段
        function bindIsPassData() {
            var isSucoptions = "<%=BoolStr %>";
            var NotEstoptions = "<%=ALLNotEstablishReasonStr %>";
            var NotSucoptions = "<%=ALLNotSuccessReasonStr %>";

            AppendEndHtml();
            var isPass = '<%=IsPass %>';
            $("#IsEstablish_selectid").val(isPass);
            var isSuccess = '<%=IsSuccess %>';
            $("#IsSuccess_selectid").val(isSuccess);
            var notEstablishReason = '<%=NotEstablishReason %>';
            var failReason = '<%=FailReason %>';
            $("#NotSuccessReason_selectid").val(failReason);
            $("#NotEstablishReason_selectid").val(notEstablishReason);

            InitChange("#IsEstablish_selectid", "#IsSuccess_selectid");
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
                            项目名称：</label><span><%=ProjectName%></span></li>
                    <li>
                        <label>
                            关联需求：</label><span><input type="hidden" value="<%=model.DemandID%>" id="DemandID" /><a
                                href="javascript:void(0)" onclick="OpenDemandID('<%=string.Format(DemandDetailsUrl,model.DemandID)%>')"><%=model.DemandID%></a></span></li>
                    <li>
                        <label>
                            <span class="redColor" id="Span2">*</span>姓名：</label><span><input type="text" runat="server"
                                class="w220" id="spantxtCustName" style="width: 250px" /></span></li>
                    <li>
                        <label>
                            电话：</label><span id="txtTel"><%=model.Tel%></span>&nbsp;
                        <img id="imgTel" alt="打电话" style="cursor: pointer; vertical-align: middle;" src="../../../Images/phone.gif"
                            border="0" onclick="CallOutForGR()" />
                        <img id="imgNodisturbTel" alt="添加为免打扰电话" disabled="disabled" style="cursor: pointer;
                            vertical-align: middle;" src="../../../Images/nodisturbgray.png" border="0" onclick="javascript:NoDisturbTool.openNoDisturbPopup(this,$.trim($('#txtTel').text()),$('#hidCallID').val());" />
                    </li>
                    <li>
                        <label>
                            <span class="redColor">*</span>性别：</label><label style="float: none; cursor: pointer;
                                font-weight: normal" for="rdoMan"><input type="radio" name="sex" id="rdoMan" runat="server"
                                    value="1" />先生</label><label style="float: none; cursor: pointer; font-weight: normal"
                                        for="rdoWomen"><input type="radio" id="rdoWomen" runat="server" name="sex" style="margin-left: 50px;"
                                            value="2" />女士</label></li>
                    <li>
                        <label>
                            下单地区：</label>
                        <span>
                            <%=ProvinceName%>
                            &nbsp;<%=CityName%></span></li>
                    <li>
                        <label>
                            下单经销商：</label><span><%=model.DealerName%></span></li>
                    <li>
                        <label>
                            目标下单地区：</label>
                        <select id="ddlTargetProvince" class="w125" style="margin-top: 5px;" onchange="javascript:CustBaseInfoHelper2.TriggerProvince()"
                            runat="server">
                        </select>
                        <select id="ddlTargetCity" class="w125" style="margin-top: 5px; margin-left: 7px"
                            runat="server">
                        </select></li>
                    <li>
                        <label>
                            下单车型：</label><span><%=model.OrderCarMaster%>
                                &nbsp;
                                <%=model.OrderCarSerial%></span></li>
                    <li>
                        <label>
                            目标车型：</label>
                        <select id="ddlCarBrand" class="w125" disabled="disabled" style="margin-top: 5px;">
                            <option>
                                <%=DBrand%></option>
                        </select>
                        <select id="dllCarSerial" class="w125" style="margin-top: 5px; margin-left: 7px"
                            runat="server">
                        </select>
                    </li>
                    <li>
                        <label>
                            下单日期：</label><span><%if (model.OrderCreateTime > (new DateTime(1900, 01, 01)))
                                                 {%><%=Convert.ToDateTime(model.OrderCreateTime).ToString("yyyy-MM-dd")%><%} %></span></li>
                </ul>
                <div style="width: 870px; height: 1px; border: 0px; border-top: 1px dashed #B5D6E6;
                    margin: 0px auto; text-align: center;">
                </div>
                <ul class="clearfix ">
                    <li>
                        <label>
                            是否购车：</label>
                        <span>
                            <input id="rdIsBuyCar" type="radio" value="1" name="rdIsBuyCar" onclick="IsBuyCarSwitch()" />
                            <em onclick="emChkIsChoose(this);IsBuyCarSwitch();">已购车</em> </span>&nbsp;&nbsp;
                        <span>
                            <input id="rdIsNotBuyCar" type="radio" value="0" name="rdIsBuyCar" onclick="IsBuyCarSwitch()"
                                style="margin-left: 27px; *margin-left: 24px;" />
                            <em onclick="emChkIsChoose(this);IsBuyCarSwitch();">未购车</em> </span>&nbsp;&nbsp;
                        <span>
                            <input id="rdDontKnowBuyCarInfo" type="radio" value="2" name="rdIsBuyCar" onclick="IsBuyCarSwitch()"
                                checked="checked" style="margin-left: 27px; *margin-left: 24px;" />
                            <em onclick="emChkIsChoose(this);IsBuyCarSwitch();">未知</em> </span></li>
                    <li class="li_BoughtCar" style="display: none">
                        <label>
                            已购车型：</label>
                        <span>
                            <select name="selBoughtCarMaster" id="selBoughtCarMaster" class="w125" style="margin-top: 5px;">
                            </select></span> <span>
                                <select name="selBoughtSerial" id="selBoughtSerial" class="w125" style="margin-top: 5px;
                                    margin-left: 7px">
                                </select></span> </li>
                    <li class="li_BoughtCar" style="display: none">
                        <label>
                            购车时间：</label>
                        <span>
                            <select name="selBoughtYear" id="selBoughtYear" class="w125" runat="server" style="margin-top: 5px;">
                            </select></span> <span>
                                <select name="selBoughtMonth" id="selBoughtMonth" class="w125" style="margin-top: 5px;
                                    margin-left: 7px; width: 120px; *width: 118px;">
                                </select></span> </li>
                    <li class="li_BoughtCar" style="display: none">
                        <label>
                            购车经销商：</label>
                        <span>
                            <input type="text" id="txt_MemberName" class="w250" style="width: 256px;" /><a id="btnselect"
                                onclick="selectMemberInfo()" style="cursor: pointer;">&nbsp;查询</a>
                            <input type="hidden" id="hidMemberCode" value="" /></span> </li>
                    <li class="li_NotBuyCar" style="display: none">
                        <label>
                            意向车型：</label>
                        <span>
                            <select name="selIntentionCarMaster" id="selIntentionCarMaster" class="w125" style="margin-top: 5px;">
                            </select></span> <span>
                                <select name="selIntentionCarSerialName" id="selIntentionCarSerialName" class="w125"
                                    style="margin-top: 5px; margin-left: 7px">
                                </select></span> </li>
                    <li class="li_NotBuyCar" style="display: none">
                        <label>
                            预计购车时间：</label>
                        <span>
                            <select id="selWantBuyCarTime" class="w255" style="margin-top: 5px; width: 254px;"
                                runat="server">
                            </select>
                        </span></li>
                    <%--<li class="li_NotBuyCar" style="display: none">
                        <label>
                            购车计划：</label>
                        <span>
                            <input type="radio" name="HasBuyCarPlan" id="rdHasBuyCarPlan" value="1" />
                            <em onclick="emChkIsChoose(this);">有</em> </span>&nbsp;&nbsp; <span>
                                <input type="radio" name="HasBuyCarPlan" id="rdNoBuyCarPlan" style="margin-left: 50px;
                                    *margin-left: 47px;" value="0" />
                                <em onclick="emChkIsChoose(this);">无</em> </span></li>--%>
                    <li class="li_NotBuyCar" style="display: none">
                        <label>
                            是否关注该品牌：</label>
                        <span>
                            <input type="radio" name="IsAttention" id="rdAttention" value="1" />
                            <em onclick="emChkIsChoose(this);">是</em> </span>&nbsp;&nbsp; <span>
                                <input type="radio" name="IsAttention" id="rdNoAttention" style="margin-left: 50px;
                                    *margin-left: 47px;" value="0" />
                                <em onclick="emChkIsChoose(this);">否</em> </span></li>
                    <li class="li_NotBuyCar  " style="display: none; float: left; clear: left;">
                        <label>
                            是否有经销商联系：</label>
                        <span>
                            <input type="radio" name="IsContactedDealer" onclick="fnIsContactedDealer()" id="rdContactedDealer"
                                value="1" />
                            <em onclick="emChkIsChoose(this);fnIsContactedDealer();">是</em> </span>&nbsp;&nbsp;
                        <span>
                            <input type="radio" name="IsContactedDealer" onclick="fnIsContactedDealer()" id="rdNoContactedDealer"
                                style="margin-left: 50px; *margin-left: 47px;" value="0" />
                            <em onclick="emChkIsChoose(this);fnIsContactedDealer();">否</em> </span></li>
                    <li class="li_HasContactedDealer  " style="display: none">
                        <label>
                            经销商服务是否满意：</label>
                        <span>
                            <input type="radio" name="IsSatisfiedService" id="rdSatisfiedService" value="1" />
                            <em onclick="emChkIsChoose(this);">是</em> </span>&nbsp;&nbsp; <span>
                                <input type="radio" name="IsSatisfiedService" id="rdNoSatisfiedService" style="margin-left: 50px;
                                    *margin-left: 47px;" value="0" />
                                <em onclick="emChkIsChoose(this);">否</em> </span></li>
                    <li class="li_HasContactedDealer" style="display: none">
                        <label>
                            哪家经销商联系：</label>
                        <span>
                            <input type="text" class="w220" id="txtContactedWhichDealer" style="width: 250px" />
                        </span></li>
                </ul>
                <div style="width: 870px; height: 1px; border: 0px; border-top: 1px dashed #B5D6E6;
                    margin: 0px auto; text-align: center;">
                </div>
                <script type="text/javascript">
                    $(function () {
                        BindBrand();
                        AttachEvent("selBoughtCarMaster", "change", popCarChange);

                        $("select[id$='selBoughtCarMaster']").val("<%=intBoughtCarMasterID%>");
                        $("select[id$='selIntentionCarMaster']").val("<%=intIntentionCarMasterID%>");
                        //已购车型   
                        var options1 = {
                            container: { master: "selBoughtCarMaster", serial: "selBoughtSerial" },
                            include: { serial: "1" },
                            datatype: 0,
                            binddefvalue: { master: "<%=intBoughtCarMasterID%>", serial: "<%=intBoughtCarSerialID%>" }
                        };
                        new BindSelect(options1).BindList();

                        //意向车型
                        var options2 = {
                            container: { master: "selIntentionCarMaster", serial: "selIntentionCarSerialName" },
                            include: { serial: "1" },
                            datatype: 0,
                            binddefvalue: { master: "<%=intIntentionCarMasterID%>", serial: "<%=intIntentionCarSerialID%>" }
                        };
                        new BindSelect(options2).BindList();

                        BindFullYearMohth("selBoughtMonth", 1);
                        if ("<%=intIsBoughtCar%>" == "1") {
                            $("#rdIsBuyCar").attr("checked", true);
                            $("#rdIsNotBuyCar").attr("checked", false);
                            $("#rdDontKnowBuyCarInfo").attr("checked", false);
                        }
                        else if ("<%=intIsBoughtCar%>" == "0") {
                            $("#rdIsNotBuyCar").attr("checked", true);
                            $("#rdIsBuyCar").attr("checked", false);
                            $("#rdDontKnowBuyCarInfo").attr("checked", false);

                            if ("<%=intIsAttention%>" == "1") {
                                $("#rdAttention").attr("checked", "checked");
                            }
                            else if ("<%=intIsAttention%>" == "0") {
                                $("#rdNoAttention").attr("checked", "checked");
                            }
                            if ("<%=intIsContactedDealer%>" == "1") {
                                $("#rdContactedDealer").attr("checked", "checked");
                                if ("<%=intIsSatisfiedService%>" == "1") {
                                    $("#rdSatisfiedService").attr("checked", "checked");
                                }
                                else if ("<%=intIsSatisfiedService%>" == "0") {
                                    $("#rdNoSatisfiedService").attr("checked", "checked");
                                }
                                //
                                $("#txtContactedWhichDealer").val("<%=strContactedWhichDealer%>");
                                //$(".li_HasContactedDealer").css("display", "block");
                            }
                            else if ("<%=intIsContactedDealer%>" == "0") {
                                $("#rdNoContactedDealer").attr("checked", "checked");
                                //$(".li_HasContactedDealer").css("display", "none");
                            }
                        }
                        else if ("<%=intIsBoughtCar%>" == "2") {
                            $("#rdIsNotBuyCar").attr("checked", false);
                            $("#rdIsBuyCar").attr("checked", false);
                            $("#rdDontKnowBuyCarInfo").attr("checked", true);
                        }


                        if ($("#rdIsBuyCar").attr("checked")) {
                            $(".li_BoughtCar").css("display", "block");
                            $(".li_NotBuyCar").css("display", "none");
                            $(".li_HasContactedDealer").css("display", "none");
                        }
                        else if ($("#rdIsNotBuyCar").attr("checked")) {
                            $(".li_NotBuyCar").css("display", "block");
                            if ($("#rdContactedDealer").attr("checked")) {
                                $(".li_HasContactedDealer").css("display", "block");
                            }
                            else {
                                $(".li_HasContactedDealer").css("display", "none");
                            }
                            $(".li_BoughtCar").css("display", "none");
                        }
                        else if ($("#rdDontKnowBuyCarInfo").attr("checked")) {
                            $(".li_NotBuyCar").css("display", "none");
                            $(".li_HasContactedDealer").css("display", "none");
                            $(".li_BoughtCar").css("display", "none");
                        }

                        $("select[id$='selBoughtYear']").val("<%=strBoughtCarYear%>");
                        $("select[id$='selBoughtMonth']").val("<%=strBoughtCarMonth%>");
                        $("#hidMemberCode").val("<%=strBoughtCarDealerID%>");
                        $("#txt_MemberName").val("<%=strBoughtCarDealerName%>");

                        //                        if ("<%=intHasBuyCarPlan%>" == "1") {
                        //                            $("#rdHasBuyCarPlan").attr("checked", "checked");
                        //                        }
                        //                        else if ("<%=intHasBuyCarPlan%>" == "0") {
                        //                            $("#rdNoBuyCarPlan").attr("checked", "checked");
                        //                        }
                        //  

                        //  

                    });

                    //点击文字，选中复选框
                    function emChkIsChoose(othis) {
                        var $checkbox = $(othis).prev();
                        if ($checkbox.attr("type") == "radio") {
                            $checkbox.attr("checked", "checked");
                        }
                        else {
                            if ($checkbox.is(":checked")) {
                                $checkbox.removeAttr("checked");
                            }
                            else {
                                $checkbox.attr("checked", "checked");
                            }
                        }
                    }

                    function IsBuyCarSwitch() {
                        if ($("#rdIsBuyCar").attr("checked")) {
                            $(".li_BoughtCar").css("display", "block");
                            $(".li_NotBuyCar").css("display", "none");
                            $(".li_HasContactedDealer").css("display", "none");
                        }
                        else if ($("#rdIsNotBuyCar").attr("checked")) {
                            $(".li_NotBuyCar").css("display", "block");
                            if ($("#rdContactedDealer").attr("checked")) {
                                $(".li_HasContactedDealer").css("display", "block");
                            }
                            else {
                                $(".li_HasContactedDealer").css("display", "none");
                            }
                            $(".li_BoughtCar").css("display", "none");
                        }
                        else if ($("#rdDontKnowBuyCarInfo").attr("checked")) {
                            $(".li_NotBuyCar").css("display", "none");
                            $(".li_HasContactedDealer").css("display", "none");
                            $(".li_BoughtCar").css("display", "none");
                        }
                    }

                    function fnIsContactedDealer() {
                        if ($("#rdContactedDealer").attr("checked")) {
                            $(".li_HasContactedDealer").css("display", "block");
                        }
                        else if ($("#rdNoContactedDealer").attr("checked")) {
                            $(".li_HasContactedDealer").css("display", "none");
                        }
                    }

                    function BindFullYearMohth(objId, hasState) {
                        var strHtml = "";
                        if (hasState == 1) {
                            strHtml += "<option value='-1'>请选择月份</option>";
                        }

                        for (var i = 1; i < 13; i++) {
                            strHtml += "<option value='" + i + "月'>" + i + "月</option>";
                        }
                        $("#" + objId).html(strHtml);
                    }
                    function popCarChange() {
                        //$("#txtNominateActivity").val("");
                        //$("#popHidActivityIDs").val("");
                    }
                    //绑定品牌信息
                    function BindBrand() {
                        //已购车型      selAttentionCarBrandName  selAttentionCarSerialName
                        var options1 = {
                            container: { master: "selBoughtCarMaster", serial: "selBoughtSerial" },
                            include: { serial: "1" },
                            datatype: 0,
                            binddefvalue: { master: '0', serial: '0' }
                        };
                        new BindSelect(options1).BindList();
                        //意向车型
                        var options2 = {
                            container: { master: "selIntentionCarMaster", serial: "selIntentionCarSerialName" },
                            include: { serial: "1" },
                            datatype: 0,
                            binddefvalue: { master: '0', serial: '0' }
                        };
                        new BindSelect(options2).BindList();
                    }

                    //选择经销商
                    function selectMemberInfo() {
                        var MemberName = $.trim($("#txtName").val())
                        $.openPopupLayer({
                            name: "DealerSelectAjaxPopup",
                            parameters: { MemberName: escape(MemberName) },
                            url: "../AjaxServers/CustCategory/DealerSelect.aspx",
                            beforeClose: function () {
                                var MemberName = $('#popupLayer_' + 'DealerSelectAjaxPopup').data('MemberName');
                                var MemberCode = $('#popupLayer_' + 'DealerSelectAjaxPopup').data('MemberCode');

                                var CustID = $('#popupLayer_' + 'DealerSelectAjaxPopup').data('CustID');
                                //给经销商工单初始化客户信息
                                if (typeof GetCustInfo == "function") {
                                    GetCustInfo(CustID);
                                }
                                $("#txt_MemberName").attr("value", MemberName);
                                $("#hidMemberCode").attr("value", MemberCode);
                            }
                        });
                    }

                </script>
                <ul class="clearfix " id="ulTask">
                    <li id="li_KL" style="display: none">
                        <label>
                            其他考虑车型：</label><span><input type="text" id="txtThinkCar" class="w220" style="width: 250px"
                                value="<%=model.ThinkCar%>" /></span></li>
                    <li class="gdjl" style="margin-top: 5px;">
                        <label>
                            备注：</label><span><textarea id="Remark" style="width: 703px; height: 70px" name="Remark"
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
    <input type="hidden" id="hidCallID" />
    </form>
</body>
</html>
