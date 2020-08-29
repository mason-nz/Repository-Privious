<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="KLClassifyReport.aspx.cs"
    MasterPageFile="~/Controls/Top.Master" Title="分类统计" Inherits="BitAuto.ISDC.CC2012.Web.KnowledgeLib.KLClassifyReport" %>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <script language="javascript" type="text/javascript" src="/Js/jquery-1.4.4.min.js"></script>
    <script language="javascript" type="text/javascript" src="/Js/common.js"></script>
    <script src="/Js/jquery.uploadify.v3.2.min.js" type="text/javacript"></script>
    <script type="text/javascript" src="/Js/jquery.jmpopups-0.5.1.pack.js"></script>
    <script src="/CTI/CTITool.js" type="text/javascript"></script>
    <script type="text/javascript">
        $(function () {
            enterSearch(search);
            InitWdatePicker(2, ["mfBeginTime", "mfEndTime"]);
            if (location.search != null && location.search != "") {
                var pindex = location.search.indexOf('pid=');
                if (pindex > 0) {
                    var pid = location.search.substr(pindex + 4);
                    LoadingAnimation("ajaxTable");
                    $("#ajaxTable").load("../AjaxServers/KnowledgeLib/KLClassifyReport.aspx?pid=" + pid, null, function () { });
                }
            } else {
                search();
            }
        });

        function search() {

            var pody = params();
            LoadingAnimation("ajaxTable");
            $("#ajaxTable").load("../AjaxServers/KnowledgeLib/KLClassifyReport.aspx", pody, function () { });
        }

        //参数
        function params() {
            var pid = $.trim($("#<%=selKCID1.ClientID %>").val());
            pid = pid == null ? 0 : pid;
            var pody = "pid=" + pid + "&mBeginTime=" + escape($('#mfBeginTime').val()) + "&mEndTime=" + escape($('#mfEndTime').val()) + "&r=" + Math.random();
            return pody;
        }
    </script>
    <form id="form1" runat="server">
    <div class="searchTj" style="width: 1000px;">
        <ul>
            <li>
                <label>
                    分类：</label>
                <select id="selKCID1" class="w200" style="width: 205px;" runat="server">
                    <option value='0'>请选择</option>
                </select>
            </li>
            <li style="width: 320px;">
                <label>
                    统计时间：</label>
                <input type="text" name="BeginTime" id="mfBeginTime" class="w95" />
                <span>-</span>
                <input type="text" name="EndTime" id="mfEndTime" class="w95" />
            </li>
            <li class="btnsearch" style="width: 300px; clear: none">
                <input class="cx" type="button" value="查 询" onclick="javascript:search()" />
            </li>
        </ul>
    </div>
    <%--<div style="height: 20px; background-color: #eee;"></div>--%>
    <div style="clear: both;">
    </div>
    <div id="ajaxTable">
    </div>
    </form>
</asp:Content>
