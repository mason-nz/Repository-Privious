<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="CategoryManageEdit.aspx.cs"
    Inherits="BitAuto.ISDC.CC2012.Web.GroupManage.CategoryManageEdit" %>

<div class="pop pb15 popuser openwindow" style="width: 400px;">
    <script type="text/javascript">
       
    </script>
    <div class="title bold">
        <h2>
            编辑分类
        </h2>
        <span><a href="javascript:void(0)" onclick="javascript:$.closePopupLayer('CategoryManageEdit',false);">
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
                    <select id="ddlGroupAdd" class="w125" disabled="disabled" style="width: 200px" onfocus="this.defaultIndex=this.selectedIndex;"
                        onchange="this.selectedIndex=this.defaultIndex;">
                    </select>
                </span></li>
        </ul>
        <div class="btn mt20" style="margin-left: -80px;">
            <input id="btnSave_Page1" type="button" onclick="EditCategory();" value="保 存" class="btnSave bold" />&nbsp;&nbsp;&nbsp;&nbsp;
            <input type="button" value="取 消" class="btnCannel bold" onclick="javascript:$.closePopupLayer('CategoryManageEdit',false);" />
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
    function getEditUserGroup() {
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
        getEditUserGroup();
        var bgid=<%=BGID %>;
        var categoryName='<%=CategoryName %>';
        $("#ddlGroupAdd").val(bgid);
        $("#txtCategoryName").val(categoryName);
    })

    //保存数据
    function SaveData() {
    var scid=<%=SCID %>;
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

        AjaxPostAsync("/AjaxServers/SurveyInfo/SurveyInfoListHandle.ashx", { Action: "surveyCategoryUpdate", SCID: scid, Name: categoryName, r: Math.random() }, null, function (data) {
            var jsonData = $.evalJSON(data);
            if(jsonData.msg== "操作成功")
            {
            $.jPopMsgLayer(jsonData.msg, function () { $.closePopupLayer('CategoryManageEdit', true); });
            }
            else{
            $.jAlert(jsonData.msg, function () { $.closePopupLayer('CategoryManageEdit', true); });
            }
        });
    }

    function EditCategory() {
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
        var param = { Action: "isExistsCategoryName",CategoryName:categoryName,
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
