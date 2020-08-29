<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="GroupManageList.aspx.cs"
    MasterPageFile="~/Controls/Top.Master" Inherits="BitAuto.ISDC.CC2012.Web.GroupManage.GroupManageList"
    Title="分组管理" %>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <script type="text/javascript">
        $(document).ready(function () {
            //敲回车键执行方法
            enterSearch(search);
            LoadingAnimation('bit_table');
            $("#ckb_status_0").attr("checked", true);
            var pody = params();
            $("#ajaxTable").load("/AjaxServers/GroupManage/GroupManageList.aspx", pody);
        });
        //查询-填充列表容器
        function search(page) {
            LoadingAnimation('bit_table');
            var pody = params();
            if (page != undefined) {
                pody = pody.replace(/&page=[0-9]*/, '') + "&page=" + page;
            }
            $("#ajaxTable").load("/AjaxServers/GroupManage/GroupManageList.aspx", pody);
        }
        //分页操作
        function ShowDataByPost1(pody) {
            LoadingAnimation('bit_table');
            $("#ajaxTable").load("/AjaxServers/GroupManage/GroupManageList.aspx", pody);
        }
        //获取查询条件
        function params() {
            var businesstype = GetMutilSelectValues("name", "ckb_btype");
            var status = GetMutilSelectValues("name", "ckb_status");
            var region = GetMutilSelectValues("name", "ckb_region");
            return 'businesstype=' + businesstype + '&status=' + status + '&region=' + region + '&random=' + Math.round(Math.random() * 10000);
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
    </script>
    <div>
        <div class="search" id="SearchCon">
            <ul>
                <li style="margin-right: 0;">
                    <label>
                        业务分类：
                    </label>
                    <span class="w400">
                        <input style="" type="checkbox" value="1" name="ckb_btype" id="ckb_businesstype_1" /><em
                            onclick="emChkIsChoose(this);">热线</em>
                        <input style="" type="checkbox" value="2" name="ckb_btype" id="ckb_businesstype_2" /><em
                            onclick="emChkIsChoose(this);">在线</em>
                        <input style="" type="checkbox" value="3" name="ckb_btype" id="ckb_businesstype_3" /><em
                            onclick="emChkIsChoose(this);">在线+热线</em> </span></li>
                <li style="margin-right: 0;">
                    <label>
                        状态：
                    </label>
                    <span class="w400">
                        <input style="" type="checkbox" value="0" name="ckb_status" id="ckb_status_0" /><em
                            onclick="emChkIsChoose(this);">在用</em>
                        <input style="" type="checkbox" value="1" name="ckb_status" id="ckb_status_1" /><em
                            onclick="emChkIsChoose(this);">停用</em> </span></li>
                <li style="margin-right: 0;">
                    <label>
                        所属区域：
                    </label>
                    <span class="w400">
                        <input style="" type="checkbox" value="1" name="ckb_region" id="ckb_region_1" /><em
                            onclick="emChkIsChoose(this);">北京</em>
                        <input style="" type="checkbox" value="2" name="ckb_region" id="ckb_region_2" /><em
                            onclick="emChkIsChoose(this);">西安</em> </span></li>
                <li class="btnsearch" style="padding: 5px; margin-top: -5px; margin-left: 0px; margin-right: 0px;
                    width: 150px;">
                    <input type="button" name="Search" value="查询" onclick="search()" style="float: right" />
                </li>
            </ul>
        </div>
        <div id="ajaxTable">
        </div>
    </div>
</asp:Content>
