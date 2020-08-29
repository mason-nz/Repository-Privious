<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="EditProject.aspx.cs" Inherits="BitAuto.ISDC.CC2012.Web.ProjectManage.EditProject" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>
        <%=OperName%>项目</title>
    <script src="/Js/jquery-1.4.4.min.js" language="javascript" type="text/javascript"></script>
    <script src="/Js/jquery.jmpopups-0.5.1.pack.js" type="text/javascript"></script>
    <script src="/Js/common.js" language="javascript" type="text/javascript"></script>
    <script src="/Js/Enum/Area2.js" type="text/javascript"></script>
    <script src="../Js/json2.js" type="text/javascript"></script>
    <script src="../Js/jquery.blockUI.js" type="text/javascript"></script>
    <link href="/css/base.css" type="text/css" rel="stylesheet" />
    <link href="/css/style.css" type="text/css" rel="stylesheet" />
    <link href="../Css/uploadify.css" rel="stylesheet" type="text/css" />
    <script src="../CTI/CTITool.js" type="text/javascript"></script>
    <script type="text/javascript">
        $(document).ready(function () {
            $("#sltSurveyCategory").change(function () {
                IsClearImpData("如果要修改分类，则会清除以前的数据，确认要修改分类吗？");
                ShowBlackListUI();
            });
            $("#sltUserGroup").change(function () {
                IsClearImpData("如果要修改分类，则会清除以前的数据，确认要修改分类吗？");
            });
            $("input[name='radio_IsBlacklistCheck']").change(function () {
                ShowBlackListCheckTypeUI();
                ChangeBlackListCheckType();
            });
            $("input[name='ckb_BlackListCheckType']").change(function () {
                ChangeBlackListCheckType();
            });
            ShowBlackListCheckTypeUI();

            BindGroup();
            UserGroupChanged();

            var ProjectID = '<%=ProjectID %>';
            if (ProjectID == "") {
                //新增
            }
            else {
                //编辑
                $("#hidProjectID").val(ProjectID);

                $("#sltUserGroup").val('<%=BGID %>');
                //所属业务组选择时
                UserGroupChanged();
                $("#sltSurveyCategory").val('<%=CID %>');

                var TTCode = '<%=TTCode %>';
                $("#hidTTCode").val(TTCode);

                var objcid = '<%=CID %>';
                if (TTCode != "") {
                    $("#Button1").show();
                }
                else if (IsCustReturnVisit(objcid)) {   //如果是客户回访，则显示补录数据按钮
                    $("#Button1").show();
                    //客户回访
                    $("#hidAddDataType").val("1");
                }
                else {
                    //模板
                    $("#Button1").hide();
                }

                var status = '<%=statusId %>'; // 0 未开始 1进行中

                if (status != "0" && status != "1") {
                    $.jAlert("当前状态不允许编辑");
                }
                else {
                    //关联数据个数
                    var DataCount = '<%=DataCount %>';
                    $("#hidSelectIDsCount").val(DataCount);
                    $("#txtData").val("选择了" + DataCount + "条数据");

                    var SourceID = '<%=SourceID %>';
                    var selectDataIDs = '<%=selectDataIDs %>';
                    if (SourceID == "1" || SourceID == "4") {
                        $("#hidExportSelectIDs").val(selectDataIDs);
                    }
                    if (SourceID == "2" || SourceID == "3") {
                        $("#hidCrmSelectIDs").val(selectDataIDs);
                    }
                    //验证方式赋值
                    SetIsBlacklistCheck('<%=IsBlacklistCheck %>');
                    SetBlackListCheckType('<%=BlackListCheckType %>');
                    ShowBlackListCheckTypeUI();

                    //禁用控件
                    var statusId = '<%=statusId %>';
                    if (statusId != '0') {
                        $("#sltUserGroup").attr("disabled", "disabled");
                        $("#sltSurveyCategory").attr("disabled", "disabled");
                        $("#txtProjectName").attr("disabled", "disabled");
                        $("#txtDescription").attr("disabled", "disabled");
                        $("#btnSelectData").attr("disabled", "disabled");

                        BlacklistUIDisabled();
                    }
                    //绑定问卷
                    BindSurveryList(ProjectID);
                    //add by qizq 2014-11-24 页面加载数据导入日志
                    LoadLogRecord();
                    //保存数据的属性
                    SaveImpHidData();
                }
            }
        });
        //add by qizq 2014-11-24 页面加载数据导入日志
        function LoadLogRecord() {
            var pody = 'ProjectID=<%=ProjectID %>&r=' + Math.random();
            LoadingAnimation("divLog");
            $("#divLog").load("/AjaxServers/ProjectManage/ProjectImportHistoryList.aspx", pody);
        }
        //根据所选业务组和分类，确定数据Source数据来源
        //若：数据清洗组+数据清洗 ：Source=1或2，再根据选择的数据，如果是Excel，则Source=1，如果是Crm，则Source=2
        //若：4s+客户回访 && 非4s+客户回访 或 淘车业务组（17）+客户回访（143）：Source=3
        //若是除了上面3个组合都是“其他任务”的数据来源：Source=4
        //易集客：Source=5
        //厂商集客：Source=6

        //add lxw 13.3.22
        function getSource() {
            var source = 0;
            var userGroup = $("#sltUserGroup").val();
            var category = $("#sltSurveyCategory").val();
            if (userGroup == 2 && category == 4) {
                if ($("#hidCrmSelectIDs").val() == "" && $("#hidExportSelectIDs").val() != "") {
                    source = 1;
                }
                else if ($("#hidCrmSelectIDs").val() != "" && $("#hidExportSelectIDs").val() == "") {
                    source = 2;
                }
            }
            //Modfiy By Chybin At 2014-07-24
            else if (IsCustReturnVisit(category)) {
                source = 3;
            }
            else {
                source = 4;
            }

            return source;
        }
        //是否客户回访
        function IsCustReturnVisit(objcid) {
            if (objcid == "63" || objcid == "62" || objcid == "143" || objcid == "215" || objcid == "216") {
                $("#hidAddDataType").val("1");
                return true;
            }
            else {
                $("#hidAddDataType").val("0");
                return false;
            }
        }
        //验证方式控件是否显示
        function ShowBlackListCheckTypeUI() {
            if (IsBlacklistUIDisabled()) return;
            var value = $("input[name='radio_IsBlacklistCheck']:checked").val();
            if (value == 1) {
                $("#li_BlackListCheckType").css("display", "");
            }
            else {
                $("#li_BlackListCheckType").css("display", "none");
            }
        }
        //验证类型选择
        function CheckBlackListCheckTypeChoose() {
            var check = $("input[name='radio_IsBlacklistCheck']:checked").val();
            if (check == 1) {
                var value = $("input[name='ckb_BlackListCheckType']:checked").length;
                if (value == 0) {
                    $.jAlert("请至少选择一个验证类型！", function () {
                        $("input[name='ckb_BlackListCheckType']")[1].focus();
                    });
                    return false;
                }
            }
            return true;
        }
        //禁用验证选择
        function BlacklistUIDisabled() {
            $("input[name='radio_IsBlacklistCheck']").attr("disabled", "disabled");
            $("input[name='ckb_BlackListCheckType']").attr("disabled", "disabled");
        }
        //控件是否禁用
        function IsBlacklistUIDisabled() {
            return $("input[name='radio_IsBlacklistCheck']").attr("disabled");
        }
        //获取是否验证
        function GetIsBlacklistCheck() {
            return $("input[name='radio_IsBlacklistCheck']:checked").val();
        }
        //获取验证方式
        function GetBlackListCheckType() {
            var check = $("input[name='radio_IsBlacklistCheck']:checked").val();
            if (check == 1) {
                var value = 0;
                $("input[name='ckb_BlackListCheckType']:checked").each(function (i, n) {
                    value |= parseInt(n.value);
                });
                return value;
            }
            else {
                return 0;
            }
        }
        //设置是否验证
        function SetIsBlacklistCheck(value) {
            $("input[name='radio_IsBlacklistCheck'][value='" + value + "']")[0].checked = true;
        }
        //设置验证方式
        function SetBlackListCheckType(value) {
            $("input[name='ckb_BlackListCheckType']").each(function (i, n) {
                var BlackListCheckType = value;
                if ((parseInt(n.value) & parseInt(BlackListCheckType)) == parseInt(n.value)) {
                    n.checked = true;
                }
                else {
                    n.checked = false;
                }
            });
        }
        //存储导入数据的相关属性到hid
        function SaveImpHidData() {
            var BGID = $("#sltUserGroup").val();
            var CID = $("#sltSurveyCategory").val();
            var IsBlacklistCheck = GetIsBlacklistCheck();
            var BlackListCheckType = GetBlackListCheckType();

            $("#hidImp_Group").val(BGID);
            $("#hidImp_Category").val(CID);
            $("#hidImp_IsBlack").val(IsBlacklistCheck);
            $("#hidImp_BlackType").val(BlackListCheckType);
        }
        //设置控件值
        function SetUIFromHidData() {
            var BGID = $("#hidImp_Group").val();
            var CID = $("#hidImp_Category").val();
            var IsBlacklistCheck = $("#hidImp_IsBlack").val();
            var BlackListCheckType = $("#hidImp_BlackType").val();

            $("#sltUserGroup").val(BGID);
            //所属业务组选择时
            UserGroupChanged();
            $("#sltSurveyCategory").val(CID);

            SetIsBlacklistCheck(IsBlacklistCheck);
            SetBlackListCheckType(BlackListCheckType);
            ShowBlackListCheckTypeUI();
        }
        //修改验证方式，则清空现有的数据
        function ChangeBlackListCheckType() {
            if (IsBlacklistUIDisabled()) return;
            var new_IsBlacklistCheck = GetIsBlacklistCheck();
            var new_BlackListCheckType = GetBlackListCheckType();

            var old_IsBlacklistCheck = $("#hidImp_IsBlack").val();
            var old_BlackListCheckType = $("#hidImp_BlackType").val();

            if (new_IsBlacklistCheck != old_IsBlacklistCheck || new_BlackListCheckType != old_BlackListCheckType) {
                IsClearImpData("如果要修改验证方式，则会清除以前的数据，确认要修改验证方式吗？");
            }
        }
        //是否显示验证控件 客户回访不显示
        function ShowBlackListUI() {
            //确认验证控件是否显示
            var CID = $("#sltSurveyCategory").val();
            if (IsCustReturnVisit(CID)) {
                $("#li_IsBlacklistCheck").css("display", "none");
                SetIsBlacklistCheck(0);
                ShowBlackListCheckTypeUI();
            }
            else {
                $("#li_IsBlacklistCheck").css("display", "");
                ShowBlackListCheckTypeUI();
            }
        }
    </script>
    <script type="text/javascript">
        //删除问卷
        function DelItem(obj) {

            $.jConfirm("确认删除问卷吗？", function (r) {
                if (r) {
                    if ($(".itemAddDel").length == 1) {
                        // $.jAlert("最后一个问卷不能删除，如果要删除问卷，可以此问卷的名称、调查时间删除");
                        $(obj).find("[name='beginTime']").val("");
                        $($($(obj).parent()).parent()).parent().find("[name='beginTime']").val("");
                        $($($(obj).parent()).parent()).parent().find("[name='endTime']").val("");
                        $($($(obj).parent()).parent()).parent().find("[name='txtSurveyName']").val("");
                        $($($(obj).parent()).parent()).parent().find("[name='hdnSIID']").val("");
                    }
                    else {
                        $($($(obj).parent()).parent()).parent().remove();
                    }
                }
            });

        }

        function AddItem(obj) {
            var addObj = $($($(obj).parent()).parent()).parent();
            var newObj = $(addObj).clone(true);
            $(newObj).find("[name='beginTime']").val("");
            $(newObj).find("[name='endTime']").val("");
            $(newObj).find("[name='txtSurveyName']").val("");
            $(newObj).find("[name='hdnSIID']").val("");

            $(newObj).find("[name='btnSelectSurvey']").removeAttr("disabled");

            if (typeof ($(newObj).find("[name='btnAddSurvey'] a").attr("onclick")) == "undefined") {
                $(newObj).find("[name='btnAddSurvey'] a").unbind("click").bind("click", function () { AddItem(this) });
            }
            $(newObj).find("[name='btnDelSurvey'] a").show();
            $(addObj).after(newObj);
        }

        ///根据所选择的业务组和类型获取数据类型,显示/隐藏选择数据的对话框的单选按钮和导入模板的显示
        function GetProjectType() {
            var datatypeid = 2;

            var userGroup = $("#sltUserGroup").val();
            var category = $("#sltSurveyCategory").val();
            if (userGroup == 2 && category == 4) {
                datatypeid = 0;
            }
            //添加淘车业务组（17）——客户回访（143）分类，Add=Masj,Date=2014-02-21
            //添加智能平台运营支持部、二手车运营支持部下客户回访分类 （215,216） Modify By Chybin At 2014-07-24
            else if (IsCustReturnVisit(category)) {
                datatypeid = 1;
            }
            return datatypeid;
        }

        //选择数据
        function openSelectDataPopup() {
            var projecttype = GetProjectType();
            var BGID = $("#sltUserGroup").val();
            var CID = $("#sltSurveyCategory").val();
            var BGName = $.trim($("#sltUserGroup").find("option:selected").text());
            var CName = $.trim($("#sltSurveyCategory").find("option:selected").text());

            if (BGID == -1 || CID == -1) {
                $.jAlert("请选择所属分组和分类");
                return;
            }
            if (!CheckBlackListCheckTypeChoose()) {
                return;
            }

            var IsBlacklistCheck = GetIsBlacklistCheck();
            var BlackListCheckType = GetBlackListCheckType();

            $.openPopupLayer({
                name: "SelectDataPopup",
                parameters: {
                    projecttype: projecttype,
                    BGID: BGID,
                    CID: CID,
                    BGName: escape(BGName),
                    CName: escape(CName),
                    IsBlacklistCheck: IsBlacklistCheck,
                    BlackListCheckType: BlackListCheckType
                },
                url: "DataSelect.aspx",
                beforeClose: function (e, data) {
                    if (e) {
                        var count = $("#hidSelectIDsCount").val();
                        $("#txtData").val("选择了" + count + "条数据");
                        //存储数据
                        SaveImpHidData();
                    }
                }
            }
         );
        }

        ///增加数据
        function openAddDataPopup() {
            var TTCode = $("#hidTTCode").val();
            var IsBlacklistCheck = GetIsBlacklistCheck();
            var BlackListCheckType = GetBlackListCheckType();
            $.openPopupLayer({
                name: "AddDataPopup",
                parameters: {
                    TTCode: TTCode,
                    AddType: $("#hidAddDataType").val(),
                    IsBlacklistCheck: IsBlacklistCheck,
                    BlackListCheckType: BlackListCheckType
                },
                url: "DataAdd.aspx",
                beforeClose: function (e, data) {
                    if (e) {
                        var count = $("#hidSelectIDsCount").val();
                        $("#txtData").val("选择了" + count + "条数据");
                        $("#Button1").hide();
                        //存储数据
                        SaveImpHidData();
                    }
                }
            });
        }

        //选择试卷
        function openSelectSurveyInfoPopup(obj) {
            $.openPopupLayer({
                name: "SelectSurveyInfo",
                parameters: {},
                url: "../SurveyInfo/SurveyProject/SelectSurveyInfo.aspx",
                beforeClose: function (e, data) {
                    if (e) {
                        var siid = $('#popupLayer_' + 'SelectSurveyInfo').data('SIID');
                        var name = $('#popupLayer_' + 'SelectSurveyInfo').data('Name');

                        $($($($(obj).parent()).parent()).find("[name='txtSurveyName']")[0]).val(name);
                        $($($($(obj).parent()).parent()).find("[name='hdnSIID']")[0]).val(siid);
                    }
                }
            }
         );
        }

        function BindGroup() {
            AjaxPostAsync("/AjaxServers/SurveyInfo/CommonHandler.ashx", { Action: "GetUserGroupNoRightByLoginUserID", r: Math.random() }, null, function (data) {
                var jsonData = $.evalJSON(data);
                if (jsonData != "") {
                    for (var i = 0; i < jsonData.length; i++) {
                        $("#sltUserGroup").append("<option value=" + jsonData[i].BGID + ">" + jsonData[i].Name + "</option>");
                    }
                }
            });
        }

        //所属业务组改变时，重新加载分类
        function UserGroupChanged() {
            $("#sltSurveyCategory").children().remove();
            $("#sltSurveyCategory").append("<option value='-1'>请选择分类</option>");
            if ($("#sltUserGroup").val() != "-1") {
                AjaxPostAsync("/AjaxServers/SurveyInfo/CommonHandler.ashx",
                {
                    Action: "GetSurveyCategory",
                    BGID: $("#sltUserGroup").val(),
                    TypeId: "2",
                    IsFilterStop: "1",
                    //Exclude: "ReturnVisit",
                    r: Math.random()
                }, null, function (data) {
                    var jsonData = $.evalJSON(data);
                    for (var i = 0; i < jsonData.length; i++) {
                        if (jsonData[i].SCID != 4 && $.trim(jsonData[i].Name) != "工单分类") { //临时的，当分类增加了状态，这个地方就去掉
                            if (IsCustReturnVisit(jsonData[i].SCID)) {
                                $("#sltSurveyCategory").append("<option value='" + jsonData[i].SCID + "' disabled='disabled'>" + jsonData[i].Name + "</option>");
                            }
                            else {
                                $("#sltSurveyCategory").append("<option value='" + jsonData[i].SCID + "'>" + jsonData[i].Name + "</option>");
                            }
                        }
                    }
                });
            }
        }
        //切换分类
        function IsClearImpData(question) {
            //确认数据是否删除
            if ($("#hidCrmSelectIDs").val() != "" || $("#hidExportSelectIDs").val() != "") {
                //如果已经选择数据或者已经导入数据，给提示
                var msg = "";
                if ($("#hidCrmSelectIDs").val() != "") {
                    msg = "已选择数据，";
                }
                else {
                    msg = "已导入数据，";
                }
                msg += question;
                $.jConfirm(msg, function (r) {
                    if (r) {
                        //清除选择的数据
                        $("#hidCrmSelectIDs").val("");
                        $("#hidExportSelectIDs").val("");
                        $("#hidSelectIDsCount").val("");
                        $("#txtData").val("");

                        //清除补充的数据
                        $("#hidExportAddIDs").val("");
                        $("#Button1").hide();

                        $("#hidAddDataType").val("0");
                        $("#hidCrmAddIDs").val("");
                    }
                    else {
                        SetUIFromHidData();
                    }
                });
            }
        }
        //绑定问卷
        function BindSurveryList(ProjectID) {

            var SurveryCount = '<%=SurveryCount %>';
            for (var i = 1; i < SurveryCount; i++) {
                AddItem($("span.add").first());
            }

            AjaxPostAsync('/AjaxServers/ProjectManage/GetSurveryJsonList.ashx', { ProjectID: ProjectID }, null, function (data) {
                var jsonData = $.evalJSON(data);
                $(jsonData).each(function (i, v) {
                    $($(".itemAddDel").eq(i)).find("[name='hndProjectID']").val(ProjectID);
                    $($(".itemAddDel").eq(i)).find("[name='beginTime']").val(v.BeginDate);
                    $($(".itemAddDel").eq(i)).find("[name='endTime']").val(v.EndDate);
                    $($(".itemAddDel").eq(i)).find("[name='txtSurveyName']").val(v.SurveyName);
                    $($(".itemAddDel").eq(i)).find("[name='hdnSIID']").val(v.SIID);


                    //禁用控件
                    var statusId = '<%=statusId %>';
                    if (statusId != '0') {
                        $($(".itemAddDel").eq(i)).find("[name='btnSelectSurvey']").attr("disabled", "disabled");
                        $($(".itemAddDel").eq(i)).find("[name='txtSurveyName']").attr("disabled", "disabled");
                        // $($(".itemAddDel").eq(i)).find("[name='btnDelSurvey'] a").removeAttr("onclick");
                        $($(".itemAddDel").eq(i)).find("[name='btnDelSurvey'] a").hide();
                    }
                });
            });

        }
    </script>
    <%--提交--%>
    <script type="text/javascript">
        function SubmitProject() {
            var ProjectID = $.trim($("#hidProjectID").val());
            var txtProjectName = $.trim($("#txtProjectName").val());
            var sltUserGroup = $.trim($("#sltUserGroup").val());
            var sltSurveyCategory = $.trim($("#sltSurveyCategory").val());
            var txtDescription = $.trim($("#txtDescription").val());
            var hidCrmSelectIDs = $.trim($("#hidCrmSelectIDs").val());
            var hidExportSelectIDs = $.trim($("#hidExportSelectIDs").val());
            var hidExportAddIDs = $.trim($("#hidExportAddIDs").val());
            var hidTTCode = $.trim($("#hidTTCode").val());
            var hidCrmAddIDs = $.trim($("#hidCrmAddIDs").val());
            var IsBlacklistCheck = GetIsBlacklistCheck();
            var BlackListCheckType = GetBlackListCheckType();

            if (txtProjectName == "") {
                $.jAlert("请输入项目名称");
                return;
            }
            if (sltUserGroup == "") {
                $.jAlert("请选择所属分组");
                return;
            }
            if (sltSurveyCategory == "") {
                $.jAlert("请选择分类");
                return;
            }
            if (!CheckBlackListCheckTypeChoose()) {
                return;
            }
            if (txtDescription == "") {
                $.jAlert("请输入项目说明");
                return;
            }
            if ($("#hidAddDataType").val() == "1") {
                //客户回访
                if (hidCrmSelectIDs == "" && hidCrmAddIDs == "") {
                    $.jAlert("请选择数据");
                    return;
                }
            }
            else {
                //模板
                if (hidExportSelectIDs == "" && hidExportAddIDs == "") {
                    $.jAlert("请选择数据");
                    return;
                }
            }

            if ($("ul.itemAddDel").length > 0) {
                var errmsg = "";
                $("ul.itemAddDel").each(function (i, v) {
                    var beginTime = $(v).find("[name='beginTime']").val();
                    var endTime = $(v).find("[name='endTime']").val();
                    var txtSurveyName = $(v).find("[name='txtSurveyName']").val();
                    var hdnSIID = $(v).find("[name='hdnSIID']").val();
                    if (txtSurveyName != "" && !$(v).find("[name='btnSelectSurvey']").attr("disabled")) {
                        if (beginTime == "") {
                            errmsg += "问卷开始时间不能为空<br/>";
                        }
                        if (endTime == "") {
                            errmsg += "问卷结束时间不能为空<br/>";
                        }
                        if (txtSurveyName == "") {
                            errmsg += "请选择问卷<br/>";
                        }
                        if (hdnSIID == "") {
                            errmsg += "请选择问卷<br/>";
                        }

                        if (beginTime > endTime) {
                            errmsg += "【" + txtSurveyName + "】问卷结束时间不能早于问卷开始时间<br/>";
                        }
                        var nowdatestr = '<%=nowDate%>';
                    }
                    if (errmsg != "") {
                        return;
                    }
                });

                if (errmsg != "") {
                    $.jAlert(errmsg);
                    return;
                }
            }

            var surveryList = new Array();
            $("ul.itemAddDel").each(function (i, v) {
                var txtSurveyName = $(v).find("[name='txtSurveyName']").val();
                if (txtSurveyName != "") {
                    var surveryitem = {
                        ProjectID: $(v).find("[name='hndProjectID']").val(),
                        beginTime: $(v).find("[name='beginTime']").val(),
                        endTime: $(v).find("[name='endTime']").val(),
                        txtSurveyName: $(v).find("[name='txtSurveyName']").val(),
                        hdnSIID: $(v).find("[name='hdnSIID']").val()
                    }
                    surveryList.push(surveryitem);
                }
            });

            var source = getSource();

            var boby = {
                ProjectID: escape(ProjectID),
                txtProjectName: escape(txtProjectName),
                sltUserGroup: escape(sltUserGroup),
                sltSurveyCategory: escape(sltSurveyCategory),
                txtDescription: escape(txtDescription),
                Source: escape(source),
                hidExportSelectIDs: escape(hidExportSelectIDs),
                hidExportAddIDs: escape(hidExportAddIDs),
                SurveyList: surveryList,
                TTCode: hidTTCode,
                hidCrmAddIDs: escape(hidCrmAddIDs),
                IsBlacklistCheck: IsBlacklistCheck,
                BlackListCheckType: BlackListCheckType
            };
            var dataObj = escape(JSON.stringify(boby));

            var json = {
                projectid: $("#hidProjectID").val(),
                CrmSelectIDs: hidCrmSelectIDs,
                data: dataObj
            };
            AjaxPost('/AjaxServers/ProjectManage/EditProject.ashx', json,
               function () {
                   $.blockUI({ message: '正在处理，请等待...' });
               },
              function (data) {
                  $.unblockUI();
                  if (data.split('_')[0] == "success") {

                      var returnProjectid = data.split('_')[1];

                      var ProjectID = $.trim($("#hidProjectID").val());

                      var status = '<%=statusId %>'; // 0 未开始 1进行中

                      var source = getSource();

                      if ($("#hidAddDataType").val() != "1") {
                          //新增
                          $.openPopupLayer({
                              name: "AddOkPanel",
                              parameters: { returnProjectID: returnProjectid, status: status, RecCount: $("#hidSelectIDsCount").val(), Source: source },
                              url: "AddOkPanel.aspx",
                              afterClose: function (e, data) {
                                  if (e) {
                                      closePageExecOpenerSearch("btnsearch_id");
                                  }
                                  else {
                                      window.location = "EditProject.aspx?projectid=" + returnProjectid;
                                  }
                              }
                          });
                      }
                      else {
                          window.location = "EditProject.aspx?projectid=" + returnProjectid;
                      }
                  }
                  else {
                      $.jAlert("保存出错！" + data);
                  }
              });
        }
    </script>
</head>
<body>
    <form id="form1" runat="server">
    <div class="w980">
        <div class="taskT">
            项目</div>
        <div class="addexam clearfix">
            <ul>
                <li>
                    <label>
                        <span class="redColor">*</span>项目名称：</label>
                    <span>
                        <input type="text" value="" id="txtProjectName" class="w190" runat="server" /><input
                            type="hidden" id="hdnSPIID" runat="server" /></span> </li>
                <li>
                    <label>
                        <span class="redColor">*</span>所属分组：</label><select id="sltUserGroup" runat="server"
                            style="width: 160px;" onchange="UserGroupChanged()"></select>
                </li>
                <li>
                    <label>
                        <span class="redColor">*</span>分类：</label><select id="sltSurveyCategory" runat="server"
                            style="width: 160px;">
                            <option value="-1">请选择分类</option>
                        </select>
                </li>
                <li>
                    <label>
                        <span class="redColor">*</span>项目说明：</label>
                    <span>
                        <textarea name="" id="txtDescription" runat="server" style="width: 463px;"></textarea></span>
                </li>
                <li id="li_IsBlacklistCheck">
                    <label>
                        <span class="redColor">*</span>免打扰验证：</label>
                    <span>
                        <input type="radio" name="radio_IsBlacklistCheck" value="1" />
                        <em onclick="emChkIsChoose(this);ShowBlackListCheckTypeUI();ChangeBlackListCheckType();">
                            是&nbsp;</em>
                        <input type="radio" name="radio_IsBlacklistCheck" value="0" checked="checked" />
                        <em onclick="emChkIsChoose(this);ShowBlackListCheckTypeUI();ChangeBlackListCheckType();">
                            否&nbsp;</em> </span></li>
                <li id="li_BlackListCheckType">
                    <label>
                        <span class="redColor">*</span>验证类型：</label>
                    <span>
                        <input type="checkbox" name="ckb_BlackListCheckType" value="1" />
                        <em onclick="emChkIsChoose(this);ChangeBlackListCheckType();">经销商验证&nbsp;</em>
                        <input type="checkbox" name="ckb_BlackListCheckType" value="2" checked="checked" />
                        <em onclick="emChkIsChoose(this);ChangeBlackListCheckType();">C端用户验证&nbsp;</em>
                    </span></li>
                <li>
                    <label>
                        <span class="redColor">*</span>选择数据：</label>
                    <span>
                        <input type="text" id="txtData" disabled="disabled" runat="server" class="w550" style="width: 300px;" /></span><span
                            class="btnOption">
                            <input id="btnSelectData" name="" type="button" value="选择" onclick="openSelectDataPopup()" /></span>
                    <span class="btnOption">
                        <input id="Button1" name="" type="button" value="补充数据" onclick="openAddDataPopup()"
                            style="display: none;" /></span> </li>
                <li>
                    <ul class="itemAddDel" style="margin: 0px; margin-top: 10px; width: 570px;">
                        <li>
                            <label>
                                选择问卷：</label>
                            <span>
                                <input type="text" value="" class="w220" name="txtSurveyName" style="width: 300px;" /></span>
                            <input type="hidden" name="hdnSIID" class="w550" />
                            <input type="hidden" name="hndProjectID" class="w550" />
                            <span class="btnOption">
                                <input name="btnSelectSurvey" type="button" value="选择" onclick="openSelectSurveyInfoPopup(this)" /></span>
                            <span name="btnAddSurvey" class="add"><a onclick="AddItem(this)" href="javascript:void(0)">
                            </a></span><span name="btnDelSurvey" class="delete"><a onclick="DelItem(this)" href="javascript:void(0)">
                            </a></span></li>
                        <li>
                            <label>
                                调查时间：</label>
                            <span>
                                <input type="text" id="txtBeginTime" name="beginTime" class="w90" style="width: 140px;"
                                    onclick="MyCalendar.SetDate(this,this)" />
                            </span><span style="float: left;">-</span> <span>
                                <input type="text" id="txtEndTime" name="endTime" class="w90" style="width: 140px;"
                                    onclick="MyCalendar.SetDate(this,this)" /></span> </li>
                    </ul>
                </li>
                <li style="width: 551px; <%=importhistroycss%>">
                    <label>
                        数据导入日志：</label>
                    <div id="divLog" class="fullRow cont_cxjg" style="margin-left: 78px;">
                    </div>
                </li>
            </ul>
        </div>
        <div class="btn" style="margin: 20px auto;">
            <input id="btnSave" type="button" name="" value="保 存" onclick="SubmitProject()" />&nbsp;&nbsp;
        </div>
    </div>
    <input type="hidden" id="hidProjectID" value="" />
    <input type="hidden" id="hidCrmSelectIDs" value="" />
    <input type="hidden" id="hidSelectIDsCount" value="" />
    <input type="hidden" id="hidExportSelectIDs" value="" />
    <input type="hidden" id="hidExportAddIDs" value="" />
    <input type="hidden" id="hidTTCode" value="" />
    <input type="hidden" id="hidAddDataType" value="0" />
    <input type="hidden" id="hidCrmAddIDs" value="" />
    <%--导入数据之后相关属性存储--%>
    <input type="hidden" id="hidImp_Group" value="" />
    <input type="hidden" id="hidImp_Category" value="" />
    <input type="hidden" id="hidImp_IsBlack" value="" />
    <input type="hidden" id="hidImp_BlackType" value="" />
    </form>
</body>
</html>
