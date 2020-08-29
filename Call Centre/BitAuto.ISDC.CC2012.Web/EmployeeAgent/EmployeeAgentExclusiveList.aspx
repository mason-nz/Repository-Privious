<%@ Page Title="专属客服管理" Language="C#" MasterPageFile="~/Controls/Top.Master" AutoEventWireup="true"
    CodeBehind="EmployeeAgentExclusiveList.aspx.cs" Inherits="BitAuto.ISDC.CC2012.Web.EmployeeAgentExclusive.EmployeeAgentExclusiveList" %>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <script type="text/javascript">
        document.writeln("<s" + "cript type='text/javascript' src='../Js/config.js?r=" + Math.random() + "'></scr" + "ipt>");
    </script>
    <script type="text/javascript">
        loadJS("common");
        loadJS("controlParams");
        $(document).ready(function () {
            //敲回车键执行方法
            enterSearch(search);
            getUserGroup();
            search();
        });
        //查询
        function search() {
            LoadingAnimation('bit_table');
            var pody = params();
            $("#ajaxTable").load("../AjaxServers//EmployeeAgent/EmployeeAgentExclusiveList.aspx", pody);
        }
        function params() {
            var EmpName = $.trim($("#EmpName").val());
            var agentNum = $.trim($("#agentNum").val());
            var role = "";
            var questionQuality = $("#Erole option:selected").val();
            if (questionQuality == '-1') {
                questionQuality = '';
            }
            else {
                role = "'" + $("#Erole option:selected").val() + "'";
            }
            var regionid = $("#sltArea").val();
            var isexclusive = $(":checkbox[name='cb_IsExclusive'][checked=true]").map(function () { return "'" + $(this).val() + "'"; }).get().join(",");
            var pody = 'TrueName=' + escape(EmpName) + '&agentNum=' + agentNum + '&role=' + role + '&bgId=' + $("#sltGroup").val() + '&IsExclusive=' + isexclusive
            + '&page=' + $("#pageHiddenMain").val() + '&random=' + Math.round(Math.random() * 10000);
            return pody;
        }
        //分页
        function ShowDataByPost1(pody) {
            LoadingAnimation('bit_table');
            $("#ajaxTable").load("../AjaxServers//EmployeeAgent/EmployeeAgentExclusiveList.aspx", pody);
        }

        //设置专属
        function setExclusive(type) {
            // $.jAlert(type)

            var checkUsers = $(":checkbox[name='UserID'][checked=true]");
            var length = checkUsers.length;

            if (length == 0) {
                $.jAlert("至少选择一项！");
                return;
            }
            var userids = checkUsers.map(function () { return "'" + $(this).val() + "'"; }).get().join(",");

            $.post("../AjaxServers/EmployeeAgent/SetExclusive.ashx", { UserIDs: userids, IsExclusive: type }, function (data) {

                var alertstr = "";
                if (data != null && data != undefined && data.toLowerCase() == "true") {
                    if (type == 1) {
                        alertstr = "设置成功";
                    }
                    else {
                        alertstr = "取消成功";
                    }
                }
                else {
                    if (type == 1) {
                        alertstr = "设置失败";
                    }
                    else {
                        alertstr = "取消失败";
                    }
                }

                $.jPopMsgLayer(alertstr, function () {
                    search(0);
                });
            });


        }

        //加载分组

        function getUserGroup() {

            $("#sltGroup").empty();
            AjaxPostAsync("../AjaxServers/SurveyInfo/CommonHandler.ashx", { Action: "GetUserGroupByLoginUserID", ShowSelfGroup: false, r: Math.random() }, null, function (data) {
                $("#sltGroup").append("<option value='-2'>请选所属分组</option>");
                var jsonData = $.evalJSON(data);
                if (jsonData != "") {
                    for (var i = 0; i < jsonData.length; i++) {
                        $("#sltGroup").append("<option value=" + jsonData[i].BGID + ">" + jsonData[i].Name + "</option>");
                    }
                }
            });
        }
      
    </script>
    <form id="form1" action="">
    <div class="searchTj" style="width: 100%;">
        <ul style="width: 98%;">
            <li style="clear: both;">
                <label>
                    姓名：</label>
                <input type="text" id="EmpName" class="w200" name="EmpName" />
            </li>
            <li>
                <label>
                    工号：
                </label>
                <input type="text" name="agentNum" id="agentNum" class="w220" style="width: 200px;" />
            </li>
            <li>
                <label>
                    所属分组：</label>
                <select id="sltGroup" class="w200" style="width: 206px" name="sltGroup">
                </select>
            </li>
            <li class="btnsearch" style="clear: none; margin-top: 5px; width: 100px;">
                <input class="cx" name="" id="btnSearch" type="button" value="查 询" onclick="javascript:search()" />
            </li>
        </ul>
        <ul style="width: 98%;">
            <li style="clear: both;">
                <label>
                    专属客服：</label>
                <span>
                    <label>
                        <input type="checkbox" value="1" id="cb_IsExclusive1" name="cb_IsExclusive" /><em>是</em></label>
                    <label>
                        <input type="checkbox" value="0" id="cb_IsExclusive2" name="cb_IsExclusive" /><em>否</em></label></span>
            </li>
        </ul>
        <input type="hidden" id="hidBrowser" name="Browser" />
    </div>
    <div class="optionBtn clearfix">
        <%if (right_set)
          {%>
        <input name="" type="button" value="取消设置" onclick="setExclusive(0)" class="newBtn"
            style="margin-top: 3px;" />
        <input name="" type="button" value="设置客服" onclick="setExclusive(1)" class="newBtn"
            style="margin-top: 3px;" />
        <%}%>
    </div>
    <div id="ajaxTable">
    </div>
    </form>
</asp:Content>
