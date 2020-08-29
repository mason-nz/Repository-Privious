<%@ Page Title="" Language="C#" MasterPageFile="~/Controls/Top.Master" AutoEventWireup="true"
    CodeBehind="CategoryManageList.aspx.cs" Inherits="BitAuto.ISDC.CC2012.Web.GroupManage.CategoryManageList" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head1" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <script type="text/javascript">
        $(document).ready(function () {
            getUserGroup();
            //敲回车键执行方法
            //enterSearch(search);
            LoadingAnimation('bit_table');
            var pody = params();
            $("#ajaxTable").load("/AjaxServers/GroupManage/CategoryManageList.aspx", pody);
        });
        //查询-填充列表容器
        function search(page) {
            LoadingAnimation('bit_table');
            var pody = params();
            if (page != undefined) {
                pody = pody.replace(/&page=[0-9]*/, '') + "&page=" + page;
            }
            $("#ajaxTable").load("/AjaxServers/GroupManage/CategoryManageList.aspx", pody);
        }
        //分页操作
        function ShowDataByPost1(pody) {
            LoadingAnimation('bit_table');
            $("#ajaxTable").load("/AjaxServers/GroupManage/CategoryManageList.aspx", pody);
        }
        //获取查询条件
        function params() {
            var popGroup = $("#ddlGroup").val();
            var typeId = GetMutilSelectValues("name", "ckb_type");
            var status = GetMutilSelectValues("name", "ckb_status");
            if (typeId == "") {
                typeId = -1;
            }
            if (status == "") {
                status = -1;
            }
            return 'popGroup=' + popGroup + '&typeId=' + typeId + '&status=' + status + '&random=' + Math.round(Math.random() * 10000);
        }
        //获取多选值
        function GetMutilSelectValues(key, value) {
            var ids = $("input[" + key + "='" + value + "']").map(function () {
                if ($(this).attr("checked")) {
                    return $(this).val();
                }
            }).get().join(',');
            return ids;
        }

        //加载登陆人业务组
        function getUserGroup() {
            $("#ddlGroup").append("<option value='-1'>请选择分组</option>");
            AjaxPostAsync("../AjaxServers/SurveyInfo/CommonHandler.ashx", { Action: "GetUserGroupByLoginUserID", r: Math.random() }, null, function (data) {
                var jsonData = $.evalJSON(data);
                if (jsonData != "") {
                    for (var i = 0; i < jsonData.length; i++) {
                        $("#ddlGroup").append("<option value=" + jsonData[i].BGID + ">" + jsonData[i].Name + "</option>");
                    }
                }
            });
        }
        //停用和启用
        function changeCategoryStatus(scid, obj) {
            AjaxPostAsync("../AjaxServers/SurveyInfo/SurveyInfoListHandle.ashx", { Action: "UpdateSurveyCategoryStatus", SCID: scid, r: Math.random() }, null, function (data) {
                var jsonData = $.evalJSON(data);
                if (jsonData.msg == "success") {
                    search();
                }
            });
        }

        //删除
        function groupDelete(scid) {
            if ($.jConfirm("是否确认删除？", function (r) {
                if (r) {
                    var TypeId = '2';
                    $.post("/AjaxServers/SurveyInfo/SurveyInfoListHandle.ashx", { Action: "surveyCategoryUpdate", SCID: scid, Status: -1, TypeId: TypeId, r: Math.random() }, function (data) {
                        var jsonData = $.evalJSON(data);
                        if (jsonData.msg == "操作成功") {
                            $.jPopMsgLayer(jsonData.msg, function () {
                                search();
                            });
                        }
                        else {
                            $.jAlert(jsonData.msg, function () {
                                search();
                            });
                        }
                    });
                }
                else {
                    return false;
                }
            }));
        }
    </script>
    <div>
        <div class="search" id="SearchCon">
            <ul class="clearfix">
                <li style="margin-right: 0;">
                    <label>
                        所属分组：
                    </label>
                    <span class="w400">
                        <select id="ddlGroup" class="w125" style="width: 128px">
                        </select>
                    </span></li>
                <li style="margin-right: 0;">
                    <label>
                        类型：
                    </label>
                    <span class="w400">
                        <input type="checkbox" value="0" name="ckb_type" id="ckb_type_0" /><em onclick="emChkIsChoose(this);">默认分组</em>
                        <input type="checkbox" value="1" name="ckb_type" checked="checked" id="ckb_type_1" /><em
                            onclick="emChkIsChoose(this);">自定义分组</em> </span></li>
                <li style="margin-right: 0;">
                    <label>
                        状态：
                    </label>
                    <span class="w400">
                        <input type="checkbox" value="0" name="ckb_status" checked="checked" id="ckb_start" /><em
                            onclick="emChkIsChoose(this);">启用</em>
                        <input type="checkbox" value="1" name="ckb_status" id="ckb_stop" /><em onclick="emChkIsChoose(this);">停用</em>
                    </span></li>
                <li class="btnsearch" style="padding: 5px; margin-top: -5px; margin-left: 0px; margin-right: 0px;">
                    <div>
                        <input type="button" name="Search" value="查询" onclick="search()" />
                    </div>
                </li>
            </ul>
        </div>
        <div id="ajaxTable">
        </div>
    </div>
</asp:Content>
