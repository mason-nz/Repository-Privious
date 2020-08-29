<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="TemplateEdit.aspx.cs" Inherits="BitAuto.ISDC.CC2012.Web.TemplateManagement.TemplateEdit" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head>
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title>编辑模版</title>
    <script src="/Js/jquery-1.4.4.min.js" type="text/javascript"></script>
    <script src="/Js/jquery-ui.js" type="text/javascript"></script>
    <script src="/Js/Enum/Area.js" type="text/javascript"></script>
    <script src="/Js/Enum/Area2.js" type="text/javascript"></script>
    <script src="/Js/Enum/ProvinceCityCountry.js" type="text/javascript"></script>
    <script src="/Js/jquery.jmpopups-0.5.1.pack.js" type="text/javascript"></script>
    <script type="text/javascript" src="/js/GooCalendar.js"></script>
    <script src="/Js/json2.js" type="text/javascript"></script>
    <script src="/Js/jquery.blockUI.js" type="text/javascript"></script>
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
    <link rel="stylesheet" type="text/css" href="/css/GooCalendar.css" />
    <link href="/css/base.css" type="text/css" rel="stylesheet" />
    <link href="/css/style.css" type="text/css" rel="stylesheet" />
    <style type="text/css">
        .liSelect
        {
            background: #dfefff;
            padding-bottom: 5px;
        }
        .hidediv
        {
            display: none;
        }
        
        #Ul1
        {
            margin: 10px 20px;
        }
    </style>
    <%--页面加载--%>
    <script type="text/javascript">
        $(function () {
            //设置可以拖动
            $("#divBaseInfo").sortable({
                connectWith: ".clearfix",
                cursor: "move"
            });
            //点击切换到添加字段的标签
            $("#divNoControl").click(function () {
                setTab('one', 1, 3);
            });
            //设置鼠标样式
            $(".clearfix li,#divCustInfo li").live("mouseover", function () { $(this).css("cursor", "pointer"); });
            $(".clearfix li,#divCustInfo li").live("mouseout", function () { $(this).css("cursor", "default"); });

            $(".clearfix li,#divCustInfo li").live("mouseup", function () {
                $(".clearfix li,#divCustInfo li").removeClass("liSelect");
                $(this).addClass("liSelect");
            });

            getUserGroup(); //绑定业务组

            BindByEnum('selShowType', 'EnumTFieldShow'); //绑定字段类型

            //点击添加字段按钮时
            $(".annu").click(function () {
                var showcodes = $(this).attr("showcodes");
                //获取html，添加到左边区域
                GetHtmlByShowCode({ TFShowCode: showcodes }, function (returnData, html) {
                    if (returnData.TFShowCode == "100014") {
                        //如果是客户ID,插入到最前面
                        if ($("#divBaseInfo li[code='100014']").length == 0) {
                            $("#divBaseInfo").prepend(html);
                            $("#divBaseInfo li").first().attr("code", "100014");
                        }
                        $("#divBaseInfo li").first().data(returnData);
                        $("#divBaseInfo li").first().hide();

                        $("#LiCustInfo").show();
                    }
                    else if (returnData.TFShowCode == "100015") {
                        //如果是个人用户
                        //初始化个人用户各字段属性
                        if ($("#divBaseInfo li[code='100015']").length == 0) {
                            $("#divBaseInfo").append(html);
                            //电话
                            $("#divBaseInfo li").last().attr("code", "100015");
                            returnData.TFDesName = "电话";
                            returnData.TFSortIndex = 0;
                            returnData.TFIsExportShow = "1";
                            returnData.TFInportIsNull = "0";
                            returnData.TFIsNull = "0";
                            returnData.TFDes = "电话";
                            $("#divBaseInfo li").last().data(returnData);

                            //性别
                            $("#divBaseInfo li").last().prev().attr("code", "100015");
                            returnData.TFDesName = "性别";
                            returnData.TFDes = "性别";
                            returnData.TFValue = "1|先生;2|女士";
                            $("#divBaseInfo li").last().prev().data(returnData);

                            //姓名
                            $("#divBaseInfo li").last().prev().prev().attr("code", "100015");
                            returnData.TFDesName = "姓名";
                            returnData.TFDes = "姓名";
                            returnData.TFValue = "";
                            $("#divBaseInfo li").last().prev().prev().data(returnData);
                        }
                    }
                    else if (returnData.TFShowCode == "100016" || returnData.TFShowCode == "100017" || returnData.TFShowCode == "100018") {
                        if ($("#divBaseInfo li[code='" + returnData.TFShowCode + "']").length == 0) {
                            $("#divBaseInfo").append(html);
                            $("#divBaseInfo li").last().data(returnData);
                            $("#divBaseInfo li").last().attr("code", returnData.TFShowCode);
                        }
                    }
                    else if (returnData.TFShowCode == "100012" || returnData.TFShowCode == "100013" || returnData.TFShowCode == "100019") {
                        if ($("#divBaseInfo li[code='" + returnData.TFShowCode + "']").length == 0) {
                            $("#divBaseInfo").append(html);
                            $("#divBaseInfo li").last().data(returnData);
                            $("#divBaseInfo li").last().attr("code", returnData.TFShowCode);
                        }
                    }
                    else {
                        $("#divBaseInfo").append(html);
                        $("#divBaseInfo li").last().data(returnData);
                    }

                    $("#divHasControl").show();
                    $("#divNoControl").hide();
                });
            });

            //点击模板头部
            $("#divTemplateHead").live("mouseup", function () { setTab('one', 3, 3); BindHeadAtRight(this); });

            //点击左边字段
            $(".clearfix li").live("mouseup", function () {
                setTab('one', 2, 3); //选中第二个选项卡
                BindFiledAtRight(this); //绑定右侧的控件
                $(".clearfix li,#divCustInfo li").removeClass("currentLi");
                $(this).addClass("currentLi"); //设置当前字段
                DisabledContrl($(this).data());
            });

            //点击左边CRM 客户区域
            $("#divCustInfo li").live("mouseup", function () {
                setTab('one', 2, 3); //选中第二个选项卡
                BindFiledAtRight($("#divBaseInfo li").first()); //绑定右侧的控件
                $(".clearfix li,#divCustInfo li").removeClass("currentLi");
                $($("#divBaseInfo li").first()).addClass("currentLi"); //设置当前字段
                $("#LiCustInfo").addClass("liSelect").addClass("currentLi");
                DisabledContrl($("#divBaseInfo li").first().data());
            });

            //删除字段
            $("#btnDelField").click(function () {
                var jsonData = $(".clearfix li.currentLi").data();
                $.jConfirm("确定要删除[" + jsonData.TFDesName + "]字段吗？", function (r) {
                    if (r) {
                        if (jsonData.RecID != "") {
                            $("#hidDelFiledIDs").val($("#hidDelFiledIDs").val() + jsonData.RecID + ",");
                        }
                        $(".clearfix li.currentLi").remove();
                        if (jsonData.TFShowCode == "100015") {
                            try {
                                $("#divBaseInfo li[code='100015']").each(function (i, v) {
                                    var dataDel = $(this).data();
                                    $("#hidDelFiledIDs").val($("#hidDelFiledIDs").val() + dataDel.RecID + ",");
                                    $(this).remove();
                                });
                            } catch (e) {
                                alert("error:" + e.Message);
                            }
                        }
                        if (jsonData.TFShowCode == "100014") {
                            $("#LiCustInfo").hide();
                        }
                        if ($(".clearfix li").length == 0) {
                            $("#divHasControl").hide();
                            $("#divNoControl").show();
                        }
                    }
                });
            });

            //保存按钮
            $("#btnSave").click(function () {
                $("#hidSaveOrPreview").val("save");
                save();
            });

            //预览按钮
            $("#btnPreview").click(function () {
                $("#hidSaveOrPreview").val("preview");
                save();
            });

            var TTCode = '<%=TTCode %>';
            if (TTCode == "") {
                //新增
                SetHead({}); //初始化模板头
            }
            else {
                //编辑
                BindFields(TTCode);
                var TPageName = '<%=TPageName %>';
                var BGID = '<%=BGID %>';
                var CID = '<%=CID %>';
                var Desc = '<%=Desc %>';
                var IsShowBtn = '<%=IsShowBtn %>';
                var IsShowWorkOrderBtn = '<%=IsShowWorkOrderBtn %>';
                var IsShowSendMsgBtn = '<%=IsShowSendMsgBtn %>';
                var IsShowQiCheTong = '<%=IsShowQiCheTong %>';
                var IsShowSubmitOrder = '<%=IsShowSubmitOrder %>';

                SetHead({
                    templateName: TPageName,
                    BGID: BGID,
                    CID: CID,
                    Desc: Desc,
                    IsShowBtn: IsShowBtn,
                    IsShowWorkOrderBtn: IsShowWorkOrderBtn,
                    IsShowSendMsgBtn: IsShowSendMsgBtn,
                    IsShowQiCheTong: IsShowQiCheTong,
                    IsShowSubmitOrder: IsShowSubmitOrder
                });

                IsShowBtn == "1" ? $("#divHuifang").show() : $("#divHuifang").hide();
                IsShowWorkOrderBtn == "1" ? $("#divWorkOrder").show() : $("#divWorkOrder").hide();
                IsShowSendMsgBtn == "1" ? $("#divSendMsg").show() : $("#divSendMsg").hide();
                IsShowQiCheTong == "1" ? $("#divQiCheTong").show() : $("#divQiCheTong").hide();
                IsShowSubmitOrder == "1" ? $("#btnSubmitOrder").show() : $("#btnSubmitOrder").hide();
            }
            //载入Crm 客户显示块
            $("#LiCustInfo").load("/TemplateManagement/CustInfoView.aspx");

            //最后默认加载【话务结果】
            AppendEndHtml();
        });

        //加载【话务结果】
        function AppendEndHtml() {
            GetHtmlByShowCode({ TFShowCode: "100020", TFDesName: "是否接通", TFDes: "是否接通", TFName: "IsEstablish", TFValue: "<%=BoolStr %>" }, function (returnData, html) {
                //取消联动方法
                html = html.replace(/IsEstablish_Change/g, "NullFunction");
                $("#divCallResult").append(html);
                returnData.TFSortIndex = 101;
                returnData.TFIsExportShow = "1";
                returnData.TFInportIsNull = "1";
                returnData.TFIsNull = "0";
                $("#divCallResult li[name='" + returnData.TFName + "']").data(returnData);
            });
            GetHtmlByShowCode({ TFShowCode: "100020", TFDesName: "未接通原因", TFDes: "未接通原因", TFName: "NotEstablishReason", TFValue: "<%=ALLNotEstablishReasonStr %>" }, function (returnData, html) {
                $("#divCallResult").append(html);
                returnData.TFSortIndex = 102;
                returnData.TFIsExportShow = "1";
                returnData.TFInportIsNull = "1";
                returnData.TFIsNull = "0";
                $("#divCallResult li[name='" + returnData.TFName + "']").data(returnData);
            });
            GetHtmlByShowCode({ TFShowCode: "100020", TFDesName: "是否成功", TFDes: "是否成功", TFName: "IsSuccess", TFValue: "<%=BoolStr %>" }, function (returnData, html) {
                //取消联动方法
                html = html.replace(/IsSuccess_Change/g, "NullFunction");
                $("#divCallResult").append(html);
                returnData.TFSortIndex = 103;
                returnData.TFIsExportShow = "1";
                returnData.TFInportIsNull = "1";
                returnData.TFIsNull = "0";
                $("#divCallResult li[name='" + returnData.TFName + "']").data(returnData);
            });
            //默认不选
            GetHtmlByShowCode({ TFShowCode: "100020", TFDesName: "失败原因", TFDes: "失败原因", TFName: "NotSuccessReason", TFValue: "" }, function (returnData, html) {
                $("#divCallResult").append(html);
                returnData.TFSortIndex = 104;
                returnData.TFIsExportShow = "1";
                returnData.TFInportIsNull = "1";
                returnData.TFIsNull = "0";
                $("#divCallResult li[name='" + returnData.TFName + "']").data(returnData);
            });
        }

        function NullFunction() {
        }
        //禁用按钮
        function DisabledContrl(returnData) {
            //删除按钮可用
            $("#btnDelField").css("display", "block");

            if (returnData.TFShowCode == "100014") {
                $("#txtFieldName").attr("disabled", "disabled");
                $("#checkInportIsNull").attr("disabled", "disabled");
                $("#checkIsNull").attr("disabled", "disabled");
                $("#checkCssName").attr("disabled", "disabled");
            }
            else if (returnData.TFShowCode == "100015") {
                $("#txtFieldName").attr("disabled", "disabled");
                $("#checkInportIsNull").attr("disabled", "disabled");
                $("#checkIsNull").attr("disabled", "disabled");
                $("#checkCssName").attr("disabled", "disabled");
                $("#txtTFDes").attr("disabled", "disabled");

                $("div[class='zdmc  option']").each(function (i, v) {
                    $(this).find("input").attr("disabled", "disabled");
                    $(this).find("span").hide();
                });
            }
            else if (returnData.TFShowCode == "100016" || returnData.TFShowCode == "100017" || returnData.TFShowCode == "100018") {
                $("#txtFieldName").attr("disabled", "disabled");

                $("#checkCssName").attr("disabled", "disabled");
            }
            else if (returnData.TFShowCode == "100019") {
                //推荐活动导入时必填禁用，其它属性可修改
                $("#txtFieldName").attr("disabled", "disabled");
                $("#checkInportIsNull").attr("disabled", "disabled");
                $("#txtTFDes").attr("disabled", "disabled");

                $("div[class='zdmc  option']").each(function (i, v) {
                    $(this).find("input").attr("disabled", "disabled");
                    $(this).find("span").hide();
                });
            }
            else if (returnData.TFShowCode == "100020") {
                $("#txtFieldName").attr("disabled", "disabled");
                $("#checkInportIsNull").attr("disabled", "disabled");
                $("#checkIsNull").attr("disabled", "disabled");
                $("#checkCssName").attr("disabled", "disabled");
                $("#txtTFDes").attr("disabled", "disabled");
                $("#btnDelField").css("display", "none");
            }
            else {
                $("#txtFieldName").removeAttr("disabled");
                $("#checkInportIsNull").removeAttr("disabled");
                $("#checkIsNull").removeAttr("disabled");
                $("#checkCssName").removeAttr("disabled");
            }
        }

        //绑定业务组
        function getUserGroup() {
            AjaxPostAsync("/AjaxServers/SurveyInfo/CommonHandler.ashx", { Action: "GetUserGroupNoRightByLoginUserID", r: Math.random() }, null, function (data) {
                $("#selGroup").append("<option value='-1'>请选择所属分组</option>");
                var jsonData = $.evalJSON(data);
                if (jsonData != "") {
                    for (var i = 0; i < jsonData.length; i++) {
                        $("#selGroup").append("<option value=" + jsonData[i].BGID + ">" + jsonData[i].Name + "</option>");
                    }
                }
            });
        }

        //根据选择的分组绑定对应的分类
        function selGroupChange() {
            $("#selCategory").children().remove();
            $("#selCategory").append("<option value='-1'>请选择分类</option>");
            if ($("#selGroup").val() != "-1") {
                AjaxPostAsync("/AjaxServers/SurveyInfo/CommonHandler.ashx", { Action: "GetSurveyCategory", BGID: $("#selGroup").val(), SCStatus: "-3", TypeId: "2", IsFilterStop: "1", r: Math.random() }, null, function (data) {
                    var jsonData = $.evalJSON(data);
                    for (var i = 0; i < jsonData.length; i++) {
                        if (jsonData[i].SCID != "4" && jsonData[i].SCID != "62" && jsonData[i].SCID != "63" && $.trim(jsonData[i].Name) != "工单分类"
                        ) {
                            //数据清洗（4）、客户回访（62、63）不能添加自定义模板
                            $("#selCategory").append("<option value='" + jsonData[i].SCID + "'>" + jsonData[i].Name + "</option>");
                        }
                    }
                });
            }
        }

    </script>
    <%--模板头相关--%>
    <script type="text/javascript">
        ///根据JSon对象设置模板头信息
        function SetHead(jsonData) {
            //默认值
            var defaultJsonData = {
                templateName: '点击设置模板名',
                BGID: '-1',
                CID: '-1',
                Desc: '',
                IsShowBtn: '0',
                IsShowWorkOrderBtn: '0',
                IsShowSendMsgBtn: '0',
                IsShowQiCheTong: '0',
                IsShowSubmitOrder: '0'
            };

            jsonData = $.extend(defaultJsonData, jsonData);
            $("#spanHeadName").text(jsonData.templateName);
            $("#divTemplateHead").data(jsonData);
            BindHeadAtRight($("#divTemplateHead"));

        }

        ///绑定右侧模板属性
        function BindHeadAtRight(selectHead) {
            var dataJson = $(selectHead).data();
            var templateName = '';
            var BGID = '-1';
            var CID = '';
            var Desc = '';
            var IsShowBtn = '0';
            var IsShowWorkOrderBtn = '0';
            var IsShowSendMsgBtn = '0';
            var IsShowQiCheTong = '0';
            var IsShowSubmitOrder = '0';

            if ($.isEmptyObject(dataJson)) {//判断是否为空，为空返回true
                //为空

            }
            else {
                //不为空

                if (dataJson.templateName) {
                    templateName = dataJson.templateName;
                }
                if (dataJson.BGID && isNum(dataJson.BGID)) {
                    BGID = dataJson.BGID;
                }
                if (dataJson.CID && isNum(dataJson.CID)) {
                    CID = dataJson.CID;
                }
                if (dataJson.Desc) {
                    Desc = dataJson.Desc;
                }

                $("#txtTemplateName").val(templateName);
                $("#selGroup").val(BGID);

                //绑定分类----
                selGroupChange();

                $("#selCategory").val(CID);
                $("#txtDesc").val(Desc);

                $("#ckbHuifang").attr("checked", dataJson.IsShowBtn == "1" ? true : false);
                $("#ckbWorkOrder").attr("checked", dataJson.IsShowWorkOrderBtn == "1" ? true : false);
                $("#ckbSendMsg").attr("checked", dataJson.IsShowSendMsgBtn == "1" ? true : false);
                $("#ckbQiCheTong").attr("checked", dataJson.IsShowQiCheTong == "1" ? true : false);
                $("#ckbSubmitOrder").attr("checked", dataJson.IsShowSubmitOrder == "1" ? true : false);
            }
        }
    </script>
    <%--字段设置相关--%>
    <script type="text/javascript">
        //根据左边选定的LI标签，绑定数据到右边的控件上
        function BindFiledAtRight(selectliobj) {
            //隐藏
            $(".checkboxOption").css("display", "none");
            var dataJson = $(selectliobj).data();
            if ($.isEmptyObject(dataJson)) {//判断是否为空，为空返回true
                //为空
                $.jAlert("没有找到这个字段的相关信息");
            }
            else {
                //不为空
                $("#txtFieldName").val(dataJson.TFDesName);
                if (dataJson.TFShowCode) {
                    if (dataJson.TFShowCode == "100015") {
                        if (dataJson.TFDesName == "姓名") {
                            $("#selShowType").val("100001");
                        }
                        else if (dataJson.TFDesName == "电话") {
                            $("#selShowType").val("100006");
                        }
                        else if (dataJson.TFDesName == "性别") {
                            $("#selShowType").val("100003");
                        }
                    }
                    else if (dataJson.TFShowCode == "100019") {
                        $("#selShowType").val("100001");
                    }
                    else {
                        $("#selShowType").val(dataJson.TFShowCode);
                    }
                }
                else {
                    $("#selShowType").val("-1");
                }
                if (dataJson.TFInportIsNull) {
                    if (dataJson.TFInportIsNull == "1") {
                        //导入可以为空
                        $("#checkInportIsNull").attr("checked", false);
                    }
                    else {
                        $("#checkInportIsNull").attr("checked", true);
                    }
                }
                else {
                    $("#checkInportIsNull").attr("checked", false);
                }

                if (dataJson.TFIsNull) {
                    if (dataJson.TFIsNull == "1") {
                        //提交可以为空
                        $("#checkIsNull").attr("checked", false);
                    }
                    else {
                        $("#checkIsNull").attr("checked", true);
                    }
                }
                else {
                    $("#checkIsNull").attr("checked", false);
                }

                if (dataJson.TFCssName != null) {
                    if (dataJson.TFCssName == "") {
                        //不单占一行
                        $("#checkCssName").attr("checked", false);
                    }
                    else {
                        $("#checkCssName").attr("checked", true);
                    }
                }
                else {
                    $("#checkCssName").attr("checked", false);
                }

                $("#txtTFDes").val(dataJson.TFDes);

                //绑定选项
                $(".option").not('.hidediv').remove();
                if (dataJson.TFValue && dataJson.TFValue != "" && (dataJson.TFShowCode == "100003" || dataJson.TFShowCode == "100004" || dataJson.TFShowCode == "100005" || (dataJson.TFShowCode == "100015" && dataJson.TFDesName == "性别"))) {
                    var list = dataJson.TFValue.split(';');

                    $(list).each(function (i, v) {
                        AddoneOption(v.split('|')[0], v.split('|')[1]);
                    });

                    $(".optionTitle").show();
                }
                else {
                    $(".option").not('.hidediv').hide();
                    $(".optionTitle").hide();
                }

                if (dataJson.TFShowCode == "100020") {
                    $("#selShowType").val("100005");
                    var hasoptions = dataJson.TFValue;
                    var alloptions = "";
                    var name = dataJson.TFName;
                    var canuse = false;
                    if (dataJson.TFDesName == "是否接通") {
                        alloptions = "<%=BoolStr %>";
                    }
                    else if (dataJson.TFDesName == "未接通原因") {
                        alloptions = "<%=ALLNotEstablishReasonStr %>";
                    }
                    else if (dataJson.TFDesName == "是否成功") {
                        alloptions = "<%=BoolStr %>";
                    }
                    else if (dataJson.TFDesName == "失败原因") {
                        alloptions = "<%=ALLNotSuccessReasonStr %>";
                        canuse = true;
                    }
                    else return;
                    SetCheckBoxHtml(hasoptions, alloptions, name, canuse);
                }
            }
        }

        //多选按钮
        function SetCheckBoxHtml(hasoptions, alloptions, name, canuse) {
            $(".optionTitle").css("display", "block");
            $(".checkboxOption").css("display", "block");
            var htmlstr = "<ul>";
            var hasarray = hasoptions.split(';');
            if (!Array.indexOf) {
                Array.prototype.indexOf = function (obj) {
                    for (var i = 0; i < this.length; i++) {
                        if (this[i] == obj) {
                            return i;
                        }
                    }
                    return -1;
                }
            }
            $(alloptions.split(';')).each(function (i, v) {
                if (v != "") {
                    var checked = "";
                    if (hasarray.indexOf(v) >= 0) {
                        checked = "checked='checked'";
                    }
                    var disable = "";
                    if (!canuse) {
                        disable = "disabled='disabled'";
                    }
                    htmlstr += "<li style='height: 25px; line-height: 25px;'>"
                    + "<input type='checkbox' value='" + v.split('|')[0] + "' name='" + name + "_checkbox' onchange='CheckBoxNotNull(this);' "
                    + checked + " " + disable + " text='" + v.split('|')[1] + "'></input>"
                    + "<em onclick='emChkIsChoose2(this);'> " + v.split('|')[1] + "</em></li>";
                }
            });
            htmlstr += "</ul>";
            $(".checkboxOption").html(htmlstr);
        }
        function emChkIsChoose2(em) {
            var ckb = $(em).parents('li').find('input[type=checkbox]')[0];
            if (ckb.disabled == false)
                ckb.checked = !ckb.checked;
            CheckBoxNotNull(ckb);
        }
        //不允许不选
        function CheckBoxNotNull(ckb) {
            var name = $(ckb).attr("name");
            if (name == undefined) return;
            if ($("input[name='" + name + "']:checked").length == 0) {
                $.jAlert("下拉列表的选项不允许为空", function () { $(ckb).attr("checked", "checked"); });
                return;
            }

            //刷新左侧数据
            var str = "";
            var array = $("input[name='" + name + "']:checked");
            array.each(function (i, v) {
                var v = $.trim($(v).val());
                var t = $.trim($($("input[name='" + name + "']:checked")[i]).attr("text"));
                str += v + "|" + t + ";";
            });
            str = str.substring(0, str.length - 1);
            var dataJson = $(".clearfix li.currentLi").data();
            dataJson.TFValue = str;
            ReLoadLeftItem(dataJson); //重新加载左侧项
        }

        //添加一个选项
        function AddoneOption(id, values) {
            var count = $(".option").not(".hidediv").length;
            //下拉列表由原来最多的5项，改为10项了，Add=Masj,Date=2014-02-14
            if (count >= 10) {
                $.jAlert("选项都10个了，到上限了，不能再添加了哦");
            }
            else {
                var newItem = $("div[class='zdmc  hidediv option']").clone(true);

                //定义选项框输入事件
                $(newItem).find("[name='txtOptionText']").keyup(function () { ReSetItemData(); });

                //定义添加事件
                $(newItem).find(".add").click(function () {
                    AddoneOption(
                    function () {
                        var maxid = $("input[name='hidOptionID']").first().val(); //找当前最大ID值
                        $("input[name='hidOptionID']").each(function (i, v) {
                            if ($(v).val() * 1 > maxid * 1) {
                                maxid = $(v).val();
                            }
                        });
                        return Number(maxid) + 1;
                    }, '');
                });   //当前索引+1 为ID

                //定义删除事件
                $(newItem).find(".delete").click(function () { DeloneOption(this); });

                $(newItem).find("[name='hidOptionID']").val(id);
                $(newItem).find("[name='txtOptionText']").val(values);
                $(newItem).removeClass("hidediv");

                $(".option").last().after(newItem);
            }
        }

        //删除选项
        function DeloneOption(currDelObj) {
            var currText = $.trim($($(currDelObj).parent()[0]).find("[name='txtOptionText']").val());

            var count = $(".option").not(".hidediv").length;
            if (count <= 2) {
                $.jAlert("选项只有2个了，再删没得选了");
            }
            else {
                if (currText != "") {
                    $.jConfirm("确定要删除选项【" + currText + "】吗？", function (r) {
                        if (r) {
                            $(currDelObj).parent().remove();
                            ReSetItemData();
                        }
                    });
                }
                else {
                    $(currDelObj).parent().remove();

                    ReSetItemData();
                }
            }
        }

        ///单选、复选、下拉，修改选项时，重新给左侧当前项赋值
        function ReSetItemData() {
            var dataJson = $(".clearfix li.currentLi").data();
            var tfv = '';
            $(".option").not(".hidediv").each(function (i, v) {
                var id = $(this).find("[name='hidOptionID']").val();
                var txt = $(this).find("[name='txtOptionText']").val();
                if (txt != "" && id != undefined && txt != undefined) {
                    tfv = tfv + id + '|' + txt + ';';
                }
            });

            if (tfv != "") {
                tfv = tfv.substr(0, tfv.length - 1);
            }
            dataJson.TFValue = tfv;

            ReLoadLeftItem(dataJson); //重新加载左侧项
        }

        //根据JSON数据，重新加载左侧LI项
        function ReLoadLeftItem(dataJson) {
            GetHtmlByShowCode(dataJson, function (returnData, html) {
                $(".clearfix li.currentLi").replaceWith($(html).addClass("currentLi").addClass("liSelect"));
                $(".clearfix li.currentLi").data(dataJson);
                if (returnData.TFShowCode == "100012" || returnData.TFShowCode == "100013" || returnData.TFShowCode == "100019" || returnData.TFShowCode == "100017") {
                    $(".clearfix li.currentLi").eq(0).attr("code", returnData.TFShowCode);
                }
            });
        }

    </script>
    <%--右侧元素的事件--%>
    <script type="text/javascript">
        $(function () {
            //模板名称
            $("#txtTemplateName").keyup(function () {
                var dataJson = $("#divTemplateHead").data();
                dataJson.templateName = $(this).val();

                $("#spanHeadName").text(dataJson.templateName);
                $("#divTemplateHead").data(dataJson);
                // SetHead(dataJson);
            });

            //业务组
            $("#selGroup").change(function () {
                var dataJson = $("#divTemplateHead").data();
                dataJson.BGID = $(this).val();
                SetHead(dataJson);
            });
            //模板分类
            $("#selCategory").change(function () {
                var dataJson = $("#divTemplateHead").data();
                dataJson.CID = $(this).val();
                SetHead(dataJson);
            });
            //模板描述
            $("#txtDesc").keyup(function () {
                var dataJson = $("#divTemplateHead").data();
                dataJson.Desc = $(this).val();

                $("#divTemplateHead").data(dataJson);
            });

            //是否显示回访记录按钮
            $("#ckbHuifang").click(function () {
                var dataJson = $("#divTemplateHead").data();
                dataJson.IsShowBtn = $(this).attr("checked") == true ? "1" : "0";

                $("#divTemplateHead").data(dataJson);

                $(this).attr("checked") == true ? $("#divHuifang").show() : $("#divHuifang").hide();
            });

            //是否显示添加工单按钮
            $("#ckbWorkOrder").click(function () {
                var dataJson = $("#divTemplateHead").data();
                dataJson.IsShowWorkOrderBtn = $(this).attr("checked") == true ? "1" : "0";

                $("#divTemplateHead").data(dataJson);
                $(this).attr("checked") == true ? $("#divWorkOrder").show() : $("#divWorkOrder").hide();
            });

            //是否显示发送短信按钮
            $("#ckbSendMsg").click(function () {
                var dataJson = $("#divTemplateHead").data();
                dataJson.IsShowSendMsgBtn = $(this).attr("checked") == true ? "1" : "0";

                $("#divTemplateHead").data(dataJson);
                $(this).attr("checked") == true ? $("#divSendMsg").show() : $("#divSendMsg").hide();
            });

            //是否显示注册汽车通按钮
            $("#ckbQiCheTong").click(function () {
                var dataJson = $("#divQiCheTong").data();
                dataJson.IsShowQiCheTong = $(this).attr("checked") == true ? "1" : "0";

                $("#divQiCheTong").data(dataJson);
                $(this).attr("checked") == true ? $("#divQiCheTong").show() : $("#divQiCheTong").hide();
            });

            //是否显示插入订单按钮
            $("#ckbSubmitOrder").click(function () {
                var dataJson = $("#divHasControl").data();
                dataJson.IsShowSubmitOrder = $(this).attr("checked") == true ? "1" : "0";

                $("#divHasControl").data(dataJson);
                $(this).attr("checked") == true ? $("#btnSubmitOrder").show() : $("#btnSubmitOrder").hide();
            });

            //-----以下是字段设置
            //字段名
            $("#txtFieldName").keyup(function () {
                //最多输入5个字
                var name = $(this).val();
                if (name.length >= 5) {
                    name = name.substr(0, 5);
                    $(this).val(name);
                }
                var dataJson = $(".clearfix li.currentLi").data();
                dataJson.TFDesName = name;

                ReLoadLeftItem(dataJson); //重新加载左侧项
            });

            //导入时必填 
            $("#checkInportIsNull").click(function () {
                var dataJson = $(".clearfix li.currentLi").data();
                var isCheck = $("#checkInportIsNull").attr("checked") == true ? "0" : "1";
                dataJson.TFInportIsNull = isCheck;

                ReLoadLeftItem(dataJson); //重新加载左侧项
            });

            // 提交时必填  
            $("#checkIsNull").click(function () {
                var dataJson = $(".clearfix li.currentLi").data();
                var isCheck = $("#checkIsNull").attr("checked") == true ? "0" : "1";
                dataJson.TFIsNull = isCheck;

                ReLoadLeftItem(dataJson); //重新加载左侧项
            });

            //  单列一行  
            $("#checkCssName").click(function () {
                var dataJson = $(".clearfix li.currentLi").data();
                var className = $("#checkCssName").attr("checked") == true ? "hkId" : "";
                dataJson.TFCssName = className;

                ReLoadLeftItem(dataJson); //重新加载左侧项

            });

            //字段描述
            $("#txtTFDes").keyup(function () {
                var dataJson = $(".clearfix li.currentLi").data();
                dataJson.TFDes = $(this).val();

                ReLoadLeftItem(dataJson); //重新加载左侧项

            });

        });
    
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

        function setTab(name, cursel, n) {
            for (i = 1; i <= n; i++) {
                var menu = document.getElementById(name + i);
                var con = document.getElementById("con_" + name + "_" + i);
                menu.className = i == cursel ? "hover" : "";
                con.style.display = i == cursel ? "block" : "none";
            }
        }
    </script>
    <%--编辑--%>
    <script type="text/javascript">
        function BindFields(ttcode) {
            AjaxPost('/AjaxServers/TemplateManagement/GetFieldList.ashx', { ttcode: ttcode }, null, function (data) {
                var jsonData = $.evalJSON(data);
                $(jsonData).each(function (i, v) {
                    GetHtmlByShowCode(this, function (returnData, html) {
                        if (returnData.TFShowCode == "100014") {
                            //如果是客户ID,插入到最前面
                            $("#divBaseInfo").prepend(html);
                            $("#divBaseInfo li").first().data(returnData);
                            $("#divBaseInfo li").first().hide();
                            $("#LiCustInfo").show();
                        }
                        else if (returnData.TFShowCode == "100015") {
                            //如果是个人用户
                            $("#divBaseInfo").append(html);
                            $("#divBaseInfo li").last().attr("code", "100015");
                            $("#divBaseInfo li").last().data(returnData);
                        }
                        else if (returnData.TFShowCode == "100016" || returnData.TFShowCode == "100017" || returnData.TFShowCode == "100018") {
                            $("#divBaseInfo").append(html);
                            $("#divBaseInfo li").last().data(returnData);
                            $("#divBaseInfo li").last().attr("code", returnData.TFShowCode);
                        }
                        else if (returnData.TFShowCode == "100012" || returnData.TFShowCode == "100013" || returnData.TFShowCode == "100019") {
                            $("#divBaseInfo").append(html);
                            $("#divBaseInfo li").last().data(returnData);
                            $("#divBaseInfo li").last().attr("code", returnData.TFShowCode);
                        }
                        else if (returnData.TFShowCode == "100020") {
                            //刷新左侧数据
                            var dataJson = $("#divCallResult li[name='" + returnData.TFName + "']").data();
                            dataJson.TFValue = returnData.TFValue;
                            GetHtmlByShowCode(dataJson, function (returnData, html) {
                                //取消联动事件
                                html = html.replace(/IsEstablish_Change/g, "NullFunction");
                                html = html.replace(/IsSuccess_Change/g, "NullFunction");
                                $("#divCallResult li[name='" + returnData.TFName + "']").replaceWith($(html));
                                $("#divCallResult li[name='" + returnData.TFName + "']").data(dataJson);
                            });
                        }
                        else {
                            $("#divBaseInfo").append(html);
                            $("#divBaseInfo li").last().data(returnData);
                        }
                        $("#divHasControl").show();
                        $("#divNoControl").hide();
                    });
                });
            });
        }
    </script>
    <%--保存相关--%>
    <script type="text/javascript">
       function _hasTSZF(str) {
            var re =/^[^\\/:*?""<>|,]+$/;
            return re.test(str);
        }
        function save() {

            var validateMsg = VerificationRule();
            if (validateMsg != "") {//推荐活动验证不通过
                $.jAlert(validateMsg);
                return;
            }

            if(!_hasTSZF($.trim($("#txtTemplateName").val()))){
                $.jAlert("模板名称不能含有以下字符：\ / : \ * ? ' < > |");
                return;
            }
            //alert($("select[name='NotSuccessReason_select'] option").length);
            if ($("select[name='NotSuccessReason_select'] option").length<=1) {
                $.jAlert("失败原因的选项内容必须至少选择一个", function () { });
                return;
            }

            var bgid = $("#selGroup").val();
            var arrList = new Array();

            var fieldList = $("#divBaseInfo li");
            var fieldList_end = $("#divCallResult li");

            $(fieldList).each(function (i, v) {
                //编码字段值
                var itemJson = $(this).clone(true).data(); //克隆一份后对值编码

                //删除排序自动增加的项
                delete itemJson['sortable-item'];
                delete itemJson['sortable.preventClickEvent'];
                delete itemJson['olddisplay'];

                $.each(itemJson, function (key) {
                    var val = itemJson[key]; //属性值
                    itemJson[key] = escape(val);
                });

                //字段顺序
                var index = $("#divBaseInfo li").index(this) + 1;
                itemJson.TFSortIndex = index;

                arrList.push(itemJson);
            });
            $(fieldList_end).each(function (i, v) {
                //编码字段值
                var itemJson = $(this).clone(true).data(); //克隆一份后对值编码
                //删除排序自动增加的项
                delete itemJson['sortable-item'];
                delete itemJson['sortable.preventClickEvent'];
                delete itemJson['olddisplay'];

                $.each(itemJson, function (key) {
                    var val = itemJson[key]; //属性值
                    itemJson[key] = escape(val);
                });

                arrList.push(itemJson);
            });

            var IsShowBtn = $("#ckbHuifang").attr("checked") == true ? "1" : "0";
            var IsShowWorkOrderBtn = $("#ckbWorkOrder").attr("checked") == true ? "1" : "0";
            var IsShowSendMsgBtn = $("#ckbSendMsg").attr("checked") == true ? "1" : "0";
            var IsShowQiCheTong = $("#ckbQiCheTong").attr("checked") == true ? "1" : "0";
            var IsShowSubmitOrder = $("#ckbSubmitOrder").attr("checked") == true ? "1" : "0";

            var TTCode = '<%=TTCode %>';
            var json = {
                ttcode: TTCode,
                templateName: escape($.trim($("#txtTemplateName").val())),
                BGID: escape($("#selGroup").val()),
                CID: escape($("#selCategory").val()),
                templateDesc: escape($.trim($("#txtDesc").val())),
                IsShowBtn: IsShowBtn,
                IsShowWorkOrderBtn: IsShowWorkOrderBtn,
                IsShowSendMsgBtn: IsShowSendMsgBtn,
                IsShowQiCheTong: IsShowQiCheTong,
                IsShowSubmitOrder: IsShowSubmitOrder,
                fieldListInfo: arrList
            };

            var delIDs = $("#hidDelFiledIDs").val();
            if (delIDs != "") {
                delIDs = delIDs.substr(0, delIDs.length - 1);
            };

            var jsondata = {
                DataStr: JSON.stringify(json),
                delFiledIds: delIDs
            };

            AjaxPost("/AjaxServers/TemplateManagement/TemplateSave.ashx", jsondata,
            function () {
                $.blockUI({ message: '正在处理，请等待...' });
            },
            function (data) {
                $.unblockUI();
                if (data.split('_')[0] == "success") {

                    if ($("#hidSaveOrPreview").val() == "save") {
                        $.jPopMsgLayer("保存成功！", function () {
                            window.location = "TemplateEdit.aspx?ttcode=" + data.split('_')[1];
                        });
                    }
                    else if ($("#hidSaveOrPreview").val() == "preview") {
                        window.location = "TemplateEdit.aspx?ttcode=" + data.split('_')[1];

                        try {
                            window.external.MethodScript('/browsercontrol/newpage?url=' + escape('<%=BitAuto.Utils.Config.ConfigurationUtil.GetAppSettingValue("ExitAddress")%>/TemplateManagement/TemplateView.aspx?ttcode=' + data.split('_')[1]));
                        }
                        catch (e) {
                            window.open("/TemplateManagement/TemplateView.aspx?ttcode=" + data.split('_')[1]);
                        }
                    }
                }
                else {
                    $.jAlert(data);
                }
            });
        }
        //验证推荐活动提交时必选插件
        //验证通过返回值空字符串，未通过返回非空字符串     
        function VerificationRule() {
            //二级省市（100012），三级省市（100013），下单车型（100016），意向车型（100017），出售车型（100018）
            //推荐活动（100019）
            //规则：二级省市、三级省市至少有其一，意向车型必须有
            var fieldList = $("#divBaseInfo li[code='100019']");
            if (fieldList.index() == -1) { //没有推荐活动
                return "";
            }
            else {
                var fieldList1 = $("#divBaseInfo li[code='100012']");
                var fieldList2 = $("#divBaseInfo li[code='100013']");
                var fieldList3 = $("#divBaseInfo li[code='100017']");

                var msg = "";
                if (fieldList1.index() == -1 && fieldList2.index() == -1 && fieldList3.index() == -1) {
                    msg = "请添加地区插件和意向车型插件!";
                }
                else if ((fieldList1.index() == -1 && fieldList2.index() == -1)) {
                    msg = "请添加地区插件!";
                }
                else if (fieldList3.index() == -1) {
                    msg = "请添加意向车型插件!";
                }
                return msg;
            }

        }
    </script>
</head>
<body>
    <div class="w980">
        <div class="taskT">
            编辑模版</div>
        <!--模版开始-->
        <div class="editTemplate">
            <!--左边开始-->
            <div class="w660">
                <div class="mbName" id="divTemplateHead">
                    <span id="spanHeadName">点击设置模板名</span></div>
                <div class="" id="divHuifang" style="display: none;">
                    <a href="javascript:void(0);" style="float: right; margin-right: 40px; *margin-top: -30px;">
                        添加回访记录 </a>
                </div>
                <div class="" id="divWorkOrder" style="display: none;">
                    <a href="javascript:void(0);" style="float: right; margin-right: 40px; *margin-right: 80px;
                        *margin-top: -30px;">添加工单 </a>
                </div>
                <div class="" id="divSendMsg" style="display: none;">
                    <a href="javascript:void(0);" style="float: right; margin-right: 40px; *margin-right: 140px;
                        *margin-top: -30px;">发送短信 </a>
                </div>
                <div class="" id="divQiCheTong" style="display: none;">
                    <a href="javascript:void(0);" style="float: right; margin-right: 40px; *margin-right: 140px;
                        *margin-top: -30px;">注册汽车通 </a>
                </div>
                <div class="" id="divCustInfo" style="width: 639px;">
                    <ul id="Ul1" class="ui-sortable">
                        <li class="hkId" style="cursor: default; display: none;" id="LiCustInfo"></li>
                    </ul>
                </div>
                <div class="zdset clearfix">
                    <div id="divNoControl" style="text-align: center; padding-top: 80px; cursor: pointer;"
                        class="mbName">
                        您还没有定义模板字段，点击右边"添加字段"里的样式按钮。
                    </div>
                    <ul id="divBaseInfo" class="clear">
                    </ul>
                    <div class="clear">
                    </div>
                    <ul id="divCallResult" class="clear" style="border-top: #999 1px dotted; height: 100px;">
                    </ul>
                    <div class="clear">
                    </div>
                    <div class="btn mt20" id="divHasControl" style="display: none;">
                        <input type="button" id="btnSubmitOrder" value="插入订单" style="display: none;" />&nbsp;
                        <input type="button" id="btnSave" value="保存" />&nbsp;
                        <input type="button" id="btnPreview" value="预览" />
                    </div>
                </div>
            </div>
            <!--左边结束-->
            <!--右边开始-->
            <div class="w300" style="width: 328px;">
                <div id="Tab1">
                    <div class="Menubox">
                        <ul>
                            <li id="one1" onclick="setTab('one',1,3)" class="hover last">添加字段</li>
                            <li id="one2">字段设置</li>
                            <li class="last" id="one3" onclick="setTab('one',3,3)" style="border-right: #CCC  1px solid;">
                                模版属性</li>
                        </ul>
                    </div>
                    <div class="Contentbox" style="width: 310px;">
                        <div id="con_one_1" class="hover">
                            <div class="title">
                                标准样式</div>
                            <ul>
                                <li>
                                    <input type="button" value="单行文本" class="annu" showcodes='100001' /></li>
                                <li>
                                    <input type="button" value="多行文本" class="annu" showcodes='100002' /></li>
                                <li>
                                    <input type="button" value="单选" class="annu" showcodes='100003' /></li>
                                <li>
                                    <input type="button" value="复选" class="annu" showcodes='100004' /></li>
                                <li>
                                    <input type="button" value="下拉" class="annu" showcodes='100005' /></li>
                            </ul>
                            <div class="clear">
                            </div>
                            <div class="title">
                                定制样式</div>
                            <ul>
                                <li>
                                    <input type="button" value="电话号码" class="annu" showcodes='100006' /></li>
                                <li>
                                    <input type="button" value="邮箱" class="annu" showcodes='100007' /></li>
                                <li>
                                    <input type="button" value="日期点" class="annu" showcodes='100008' /></li>
                                <li>
                                    <input type="button" value="日期段" class="annu" showcodes='100009' /></li>
                                <li>
                                    <input type="button" value="时间点" class="annu" showcodes='100010' /></li>
                                <li>
                                    <input type="button" value="时间段" class="annu" showcodes='100011' /></li>
                                <li>
                                    <input type="button" value="二级省市" class="annu" showcodes='100012' /></li>
                                <li>
                                    <input type="button" value="三级省市县" class="annu" showcodes='100013' /></li>
                                <li>
                                    <input type="button" value="下单车型" class="annu" showcodes='100016' /></li>
                                <li>
                                    <input type="button" value="意向车型" class="annu" showcodes='100017' /></li>
                                <li>
                                    <input type="button" value="出售车型" class="annu" showcodes='100018' /></li>
                                <li>
                                    <input type="button" value="推荐活动" class="annu" showcodes='100019' /></li>
                            </ul>
                            <div class="clear">
                            </div>
                            <div class="title">
                                客户属性</div>
                            <ul>
                                <li>
                                    <input type="button" value="客户ID" class="annu" showcodes='100014' /></li>
                                <li>
                                    <input type="button" value="个人用户" class="annu" showcodes='100015' /></li>
                            </ul>
                            <div class="clear">
                            </div>
                        </div>
                        <div id="con_one_2" style="display: none">
                            <div class="title">
                                字段名称</div>
                            <div class="zdmc">
                                <input id="txtFieldName" type="text" value="" class="w280" style="width: 300px;" /></div>
                            <div class="title title2">
                                字段类型</div>
                            <div class="zdmc">
                                <select class="w300" id="selShowType" disabled="disabled">
                                </select></div>
                            <div class="clear">
                            </div>
                            <div class="title title2 optionTitle ">
                                选项内容</div>
                            <div class="zdmc  hidediv option">
                                <input type="hidden" name="hidOptionID" />
                                <input type="text" value="" class="w220" name="txtOptionText" /><span class="delete"></span><span
                                    class="add"></span></div>
                            <div class="zdmc  hidediv checkboxOption">
                            </div>
                            <div class="clear">
                            </div>
                            <div class="title title2">
                                字段选项</div>
                            <div class="zdmc">
                                <input id="checkInportIsNull" name="" type="checkbox" value="" /><label for='checkInportIsNull'
                                    style="cursor: pointer;">导入时必填</label>
                                <input id="checkIsNull" name="" type="checkbox" value="" /><label for='checkIsNull'
                                    style="cursor: pointer;">提交时必填</label>
                                <input id="checkCssName" name="" type="checkbox" value="" /><label for='checkCssName'
                                    style="cursor: pointer;">单列一行</label></div>
                            <div class="title title2">
                                填写说明</div>
                            <div class="zdmc">
                                <textarea name="" cols="" rows="" id="txtTFDes"></textarea></div>
                            <div class="btn mt20" style="width: 150px;">
                                <input id="btnDelField" type="button" value="删除" />
                            </div>
                        </div>
                        <div id="con_one_3" style="display: none">
                            <div class="title">
                                模版名称</div>
                            <div class="zdmc">
                                <input type="text" id="txtTemplateName" value="" class="w280" style="width: 300px;" /></div>
                            <div class="title title2">
                                模版分类</div>
                            <div class="zdmc">
                                <select id="selGroup" class="w125" onchange="javascript:selGroupChange()">
                                </select>&nbsp;&nbsp;<select id='selCategory' class="w165"><option>经销商客户</option>
                                </select></div>
                             <div class="title title2"  style="display:none">
                                功能按钮</div> <!--edit by wangtonghai  2016/4/27  模板隐藏-->
                            <div class="zdmc" style="display:none"> <!--edit by wangtonghai  2016/4/27  模板隐藏-->
                                <%--    <input id="ckbHuifang" type="checkbox" value="" name="">
                                <label style="cursor: pointer;" for="ckbHuifang">
                                    显示添加回访记录按钮</label>--%>
                                <input id="ckbWorkOrder" type="checkbox" value="" name="">
                                <label style="cursor: pointer;" for="ckbWorkOrder">
                                    显示添加工单按钮</label>
                                <input id="ckbSendMsg" type="checkbox" value="" name="">
                                <label style="cursor: pointer;" for="ckbSendMsg">
                                    显示发送短信按钮</label>
                                <br />
                                <input id="ckbQiCheTong" type="checkbox" value="" name="">
                                <label style="cursor: pointer;" for="ckbQiCheTong">
                                    显示注册汽车通按钮</label>
                                <input id="ckbSubmitOrder" type="checkbox" value="" name="">
                                <label style="cursor: pointer;" for="ckbSubmitOrder">
                                    显示插入订单按钮</label>
                            </div>
                            <div class="title title2">
                                模版说明</div>
                            <div class="zdmc">
                                <textarea id="txtDesc" name="" cols="" rows=""></textarea></div>
                        </div>
                    </div>
                </div>
            </div>
            <!--右边结束-->
            <div class=" clear">
            </div>
        </div>
        <!--模版结束-->
    </div>
    <input type="hidden" id="hidDelFiledIDs" value="" />
    <input type="hidden" id="hidSaveOrPreview" value="" />
</body>
</html>
