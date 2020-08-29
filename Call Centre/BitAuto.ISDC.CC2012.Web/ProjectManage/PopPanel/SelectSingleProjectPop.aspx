<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="SelectSingleProjectPop.aspx.cs"
    Inherits="BitAuto.ISDC.CC2012.Web.ProjectManage.PopPanel.SelectSingleProjectPop" %>

<script type="text/javascript">
    $(function () {
        ProjectPop_getUserGroup();
        enterSearch(ProjectSingleSelectSearch);
    });
    //加载登陆人业务组
    function ProjectPop_getUserGroup() {
        AjaxPostAsync("/AjaxServers/SurveyInfo/CommonHandler.ashx", { Action: "GetUserGroupByLoginUserID", r: Math.random() }, null, function (data) {
            $("#selPopBGroup").append("<option value='-1'>请选所属分组</option>");
            var jsonData = $.evalJSON(data);
            if (jsonData != "") {
                for (var i = 0; i < jsonData.length; i++) {
                    $("#selPopBGroup").append("<option value=" + jsonData[i].BGID + ">" + jsonData[i].Name + "</option>");
                }
            }
            ProjectSingleSelectSearch();
        });
    }
    function relocatePopWinPosition() {
        var popObj = $("#popupLayer_PopProjectSingleSelect");
        popObj.css("top", ($(window).height() - popObj.height()) / 2);
    }

    //查询
    function ProjectSingleSelectSearch() {
        var pody = _params();
        var podyStr = JsonObjToParStr(pody);
        LoadingAnimation("ajaxTableSelectSingleProjectPop");
        $("#ajaxTableSelectSingleProjectPop").load("/AjaxServers/ProjectManage/SelectSingleProjectPop.aspx? .bit_table > *", podyStr, function () {
            relocatePopWinPosition();
        });
    }


    //获取参数
    function _params() {
        var name = encodeURIComponent($.trim($("#txtPopProjectName").val()));
        var group = "";
        if ($("#selPopBGroup").val() != "-1" && $("#selPopBGroup").val() != undefined) {
            group = $("#selPopBGroup").val();
        }
        var pody = {
            name: name,
            group: group,
            rt: Math.random()
        };
        return pody;
    }

    //分页操作
    function ShowDataByPost100(pody) {

        LoadingAnimation("ajaxTableSelectSingleProjectPop");
        $('#ajaxTableSelectSingleProjectPop').load('/AjaxServers/ProjectManage/SelectSingleProjectPop.aspx?r=' + Math.random() + ' .bit_table > *', pody);
    }

    //选择操作
    function PopAjaxSelectProject(projectName, projectID) {
        $('#popupLayer_' + 'PopProjectSingleSelect').data('projectid', projectID);
        $('#popupLayer_' + 'PopProjectSingleSelect').data('projectname', projectName);
        $.closePopupLayer('PopProjectSingleSelect', true);
    }
    function ClearSelectedVal() {
        $('#popupLayer_' + 'PopProjectSingleSelect').data('projectid', '');
        $('#popupLayer_' + 'PopProjectSingleSelect').data('projectname', '');
        $.closePopupLayer('PopProjectSingleSelect', true);
    }
</script>
<div class="pop pb15 openwindow" style="width: 750px;" id="divProjectAutoCallPop">
    <div class="title bold">
        <h2 style="cursor: auto;">
            选择项目</h2>
        <span><a href="javascript:void(0)" onclick="javascript:$.closePopupLayer('PopProjectSingleSelect',false);">
        </a></span>
    </div>
    <ul class="clearfix  outTable">
        <li style="width: 230px; float: left;" class="name1">所属分组： <span>
            <select id="selPopBGroup" class="w125" style="cursor: pointer;">
            </select></span> </li>
        <li style="width: 230px; float: left;">项目名称：
            <input type="text" name="txtPopProjectName" id="txtPopProjectName" class="w125"/>
        </li>
        <li class="btn" style="width: 160px">
            <input type="button" name="" value="查 询" onclick="javascript:ProjectSingleSelectSearch();"
                class="btnSave bold" />&nbsp;&nbsp; <a id='popAClear' onclick="ClearSelectedVal();"
                    style="cursor: pointer">清空已选项</a> </li>
    </ul>
    <div id="ajaxTableSelectSingleProjectPop" class="Table2">
    </div>
</div>
