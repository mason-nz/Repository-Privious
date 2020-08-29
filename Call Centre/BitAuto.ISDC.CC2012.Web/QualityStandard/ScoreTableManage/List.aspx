<%@ Page Title="评分表管理" Language="C#" MasterPageFile="~/Controls/Top.Master" AutoEventWireup="true"
    CodeBehind="List.aspx.cs" Inherits="BitAuto.ISDC.CC2012.Web.QualityScoring.ScoreTableManage.List" %>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <script type="text/javascript">
        document.writeln("<s" + "cript type='text/javascript' src='/Js/config.js?r=" + Math.random() + "'></scr" + "ipt>");
    </script>
    <script type="text/javascript">
        loadJS("controlParams");
        $(document).ready(function () {
            $('#tfBeginTime').bind('click focus', function () { WdatePicker({ maxDate: '#F{$dp.$D(\'tfEndTime\')}', onpicked: function () { document.getElementById("tfEndTime").focus(); } }); });
            $('#tfEndTime').bind('click focus', function () { WdatePicker({ minDate: '#F{$dp.$D(\'tfBeginTime\')}' }); });
        });        
    </script>
    <form id="form1" action="Export.aspx">
    <div class="search clearfix">
        <ul>
            <li>
                <label>
                    评分表名称：</label>
                <input type="text" id="txtName" class="w200" name="Name" />
            </li>
            <li>
                <label>
                    状态：</label>
                <span>
                    <input type="checkbox" value="10001" id="chkUnfinished" name="RuleTableStatus" /><em
                        onclick="emChkIsChoose(this);">未完成</em></span> <span>
                            <input type="checkbox" value="10002" id="chkAudit" name="RuleTableStatus" /><em onclick="emChkIsChoose(this);">未审核</em></span>
                <span>
                    <input type="checkbox" value="10003" id="chkFinished" name="RuleTableStatus" /><em
                        onclick="emChkIsChoose(this);">已完成</em></span> </li>
            <li>
                <label>
                    创建人：</label>
                <select id="selCreater" class="w200" name="CreateUserID" style="width: 206px;">
                </select>
            </li>
        </ul>
        <ul>
            <li>
                <label>
                    创建时间：</label>
                <input type="text" name="BeginTime" id="tfBeginTime" vtype="isDate" vmsg="创建起始时间格式不正确"
                    class="w95" />-<input type="text" name="EndTime" id="tfEndTime" class="w96" vtype="isDate"
                        vmsg="创建结束时间格式不正确" />
            </li>
            <li style="width: 284px;">
                <label>
                    使用状态：</label>
                <span>
                    <input type="checkbox" value="10001" id="Checkbox1" name="RuleTableInUseStatus" /><em
                        onclick="emChkIsChoose(this);">未使用</em></span> <span>
                            <input type="checkbox" value="10002" id="Checkbox2" name="RuleTableInUseStatus" /><em
                                onclick="emChkIsChoose(this);">已使用</em></span> </li>
            <li class="btnsearch">
                <input id="btnSearch" style="float: left;" name="" type="button" value="查 询" onclick="javascript:search()" />
            </li>
        </ul>
    </div>
    <div class="optionBtn clearfix">
        <%if (right_export)
          { %>
        <input name="" type="button" value="导出" onclick="javascript:ExportExcel()" class="newBtn"
            style="*margin-top: 3px;" />
        <%} %>
        <input name="" type="button" value="新增" onclick="addTable()" class="newBtn" style="*margin-top: 3px;" />
    </div>
    <div id="ajaxTable">
    </div>
    <script type="text/javascript">
        //新增
        function addTable() {
            try {
                var dd = window.external.MethodScript('/browsercontrol/newpage?url=<%=BitAuto.Utils.Config.ConfigurationUtil.GetAppSettingValue("ExitAddress")%>/QualityStandard/ScoreTableManage/EditScoreTable.aspx');
            }
            catch (e) {
                window.open("/QualityStandard/ScoreTableManage/EditScoreTable.aspx");
            }
        }

        //删除评分表
        function deleteRulesTable(rtid) {

            if ($.jConfirm("是否确认删除该评分表？", function (r) {
                if (r) {
                    AjaxPostAsync("/AjaxServers/QualityStandard/ScoreTableManage/ApplicationHandler.ashx", { Action: "deleteRulesTable", QS_RTID: rtid, r: Math.random() }, null, function (data) {
                        var jsonData = $.evalJSON(data);
                        if (jsonData.msg == "操作成功") {
                            $.jPopMsgLayer(jsonData.msg, function () { search(); });
                        }
                        else {
                            $.jAlert(jsonData.msg, function () { search(); });
                        }
                    });
                }
            }));
        }

        //弹出层-应用范围
        function OpenScoreApplication() {
            $.openPopupLayer({
                name: "scoreApplication",
                parameters: { TypeId: "2" },
                url: "/QualityStandard/ScoreTableManage/ScoreApplication.aspx",
                beforeClose: function (e, data) {
                    //window.location.reload();
                }
            });
        }

        //评分表创建人
        function getCreater() {
            AjaxPostAsync("/AjaxServers/CommonHandler.ashx", { Action: "getCreater", GetCreaterType: "QS", TableName: "QS_RulesTable", ShowField: "CreateUserID", TableStatus: "", r: Math.random() }, null, function (data) {
                $("#selCreater").append("<option value='-1'>请选择</option>");
                var jsonData = $.evalJSON(data);
                if (jsonData != "") {
                    for (var i = 0; i < jsonData.length; i++) {
                        $("#selCreater").append("<option value=" + jsonData[i].UserID + ">" + jsonData[i].Name + "</option>");
                    }
                }
            });
        }

        //导出
        function ExportExcel() {
            $("#form1").submit();
        }

        function search() {
            showSearchList.getList("/AjaxServers/QualityStandard/ScoreTableManage/List.aspx", "form1", "ajaxTable");
        }

        $(function () {
            getCreater();

            //敲回车键执行方法
            enterSearch(search);

            search();
        });

    </script>
    </form>
</asp:Content>
