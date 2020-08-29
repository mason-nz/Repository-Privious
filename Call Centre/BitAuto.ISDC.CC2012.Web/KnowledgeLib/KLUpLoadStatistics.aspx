<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="KLUpLoadStatistics.aspx.cs"
    MasterPageFile="~/Controls/Top.Master" Title="上传统计" Inherits="BitAuto.ISDC.CC2012.Web.KnowledgeLib.KLUpLoadStatistics" %>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <script src="../Js/jquery.uploadify.v2.1.4.min.js" type="text/javascript"></script>
    <script src="../Js/json2.js" type="text/javascript"></script>
    <script type="text/javascript" src="../Js/swfobject.js"></script>
    <script src="/CTI/CTITool.js" type="text/javascript"></script>
    <script type="text/javascript">
        function BindSelectChange() {
            var n = 2;
            var pid = $("#<%=selKCID1.ClientID %>").val();
            $.get("/AjaxServers/KnowledgeLib/KnowledgeLibList.ashx", { Action: 'BindKnowledgeCategory', Level: n, KCID: pid }, function (data) {
                $("#selKCID" + n).html("");
                $("#selKCID" + n).append("<option value='-1'>请选择</option>");
                if (data != "") {
                    var jsonData = $.evalJSON(data);
                    if (jsonData != "") {
                        $.each(jsonData.root, function (idx, item) {
                            $("#selKCID" + n).append("<option value='" + item.kcid + "'>" + item.name + "</option>");
                        });
                    }
                }
            });
        }
        $(function () {
            enterSearch(search);
            InitWdatePicker(2, ["mfBeginTime", "mfEndTime"]);
            search();
        });

        function search() {

            var pody = params();
            LoadingAnimation("ajaxTable");
            $("#ajaxTable").load("../AjaxServers/KnowledgeLib/KnowledgeLibCount.aspx", pody, function () { });
        }

        //参数KCID
        function params() {
            var kcid;
            if ($("#selKCID2").val() != "-1") {
                kcid = $("#selKCID2").val();
            }
            else if ($("#<%=selKCID1.ClientID %>").val() != "-1") {
                kcid = $("#<%=selKCID1.ClientID %>").val();
            }
            else {
                kcid = "";
            }

            var pody = "KCID=" + kcid + "&mBeginTime=" + escape($('#mfBeginTime').val()) + "&mEndTime=" + escape($('#mfEndTime').val()) + "&r=" + Math.random();
            return pody;
        }
    </script>
    <form id="form1" runat="server">
    <div class="searchTj" style="width: 1000px;">
        <ul>
            <li style="width: auto; margin-left: 55px;"><span style="font-weight: bold;"><b>分类：</b></span>
                <select id="selKCID1" style="width: 134px; margin-left: 5px;" class="w60" runat="server"
                    onchange="javascript:BindSelectChange()">
                    <option value='-1'>请选择</option>
                </select>
                <select id="selKCID2" class="w60" style="width: 134px;">
                    <option value='-1'>请选择</option>
                </select>
                <%--<select id="selKCID3" class="w60">
                    <option value='-1'>请选择</option>
                </select>--%>
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
    <br />
    <div style="clear: both;">
    </div>
    <div id="ajaxTable">
    </div>
    </form>
</asp:Content>
