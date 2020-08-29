<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="CategoryManageAdd.aspx.cs"
    Inherits="BitAuto.ISDC.CC2012.Web.GroupManage.CategoryManageAdd" %>

<div class="pop pb15 popuser openwindow" style="width: 400px;">
    <script type="text/javascript">
       
    </script>
    <div class="title bold">
        <h2>
            添加分类
        </h2>
        <span><a href="javascript:void(0)" onclick="javascript:$.closePopupLayer('CategoryManageAdd',false);">
        </a></span>
    </div>
    <div id="page1" class="moveC clearfix" style="margin-top: 5px;">
        <ul class="clearfix" style="padding-bottom: 10px; margin-left: 20px;">
            <li style="padding-top: 10px;">
                <label>
                    <span class="redColor">*</span>分类名称：
                </label>
                <span class="gh">
                    <input type="text" id="txtCategoryName" value='' class="w125" style="width: 200px;"
                        runat="server" onkeyup="LimitLength(this,15);" onafterpaste="LimitLength(this,15);" />
                </span></li>
            <li style="padding-top: 10px;">
                <label>
                    <span class="redColor">*</span>所属分组：
                </label>
                <span class="gh">
                    <select id="ddlGroupAdd" class="w125" style="width: 200px">
                    </select>
                </span></li>
        </ul>
        <div class="btn mt20" style="margin-left: -80px;">
            <input id="btnSave_Page1" type="button" onclick="AddCategory();" value="保 存" class="btnSave bold" />&nbsp;&nbsp;&nbsp;&nbsp;
            <input type="button" value="取 消" class="btnCannel bold" onclick="javascript:$.closePopupLayer('CategoryManageAdd',false);" />
        </div>
    </div>
</div>
<script type="text/javascript">
    //长度限制
    function LimitLength(txt, len) {
        var value = $.trim($(txt).val());
        if (value != "" && value.length > len) {
            value = value.substring(0, 15);
            $(txt).val(value);
        }
    }
    //加载登陆人业务组
    function getAddUserGroup() {
        $("#ddlGroupAdd").append("<option value='-1'>请选择分组</option>");
        AjaxPostAsync("../AjaxServers/SurveyInfo/CommonHandler.ashx", { Action: "GetUserGroupByLoginUserID", r: Math.random() }, null, function (data) {
            var jsonData = $.evalJSON(data);
            if (jsonData != "") {
                for (var i = 0; i < jsonData.length; i++) {
                    $("#ddlGroupAdd").append("<option value=" + jsonData[i].BGID + ">" + jsonData[i].Name + "</option>");
                }
            }
        });
    }
    $(function () {
        getAddUserGroup();
    })

    //保存数据
    function SaveData() {
        var categoryName = $.trim($("#txtCategoryName").val());
        var groupAdd = $("#ddlGroupAdd").val();

        //非空校验
        if (categoryName == '') {
            $.jAlert("请添写分类名称", function () { $("#txtCategoryName").focus(); });
            return;
        }
        if (groupAdd == undefined || groupAdd == "-1") {
            $.jAlert("请选择所属分组");
            return;
        }
        var param = {
            Action: "surveyCategoryInsert",
            BGID: groupAdd,
            Name: encodeURIComponent(categoryName),
            TypeId: 2,
            r: Math.random()
        }

        AjaxPostAsync("/AjaxServers/SurveyInfo/SurveyInfoListHandle.ashx", param, null, function (data) {
            var jsonData = $.evalJSON(data);
            $.jPopMsgLayer("保存成功", function () { $.closePopupLayer('CategoryManageAdd', true); });
        });
    }

    function AddCategory() {
        var categoryName = $.trim($("#txtCategoryName").val());
        var groupAdd = $("#ddlGroupAdd").val();

        //非空校验
        if (categoryName == '') {
            $.jAlert("请添写分类名称", function () { $("#txtCategoryName").focus(); });
            return;
        }
        if (groupAdd == undefined || groupAdd == "-1") {
            $.jAlert("请选择所属分组");
            return;
        }
        var param = { Action: "isExistsCategoryName",
            CategoryName: encodeURIComponent(categoryName),
            r: Math.random()};
            AjaxPostAsync("/AjaxServers/SurveyInfo/SurveyInfoListHandle.ashx", param, null, function (data) {
                var jsonData = $.evalJSON(data);
                if (jsonData.msg == "success") {
                    SaveData();
                }
                else {
                    $.jAlert("分类名称不能重复！");
                }
            });
        
    }
</script>
