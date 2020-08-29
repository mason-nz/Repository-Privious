<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ScoreApplication.aspx.cs"
    Inherits="BitAuto.ISDC.CC2012.Web.QualityScoring.ScoreTableManage.ScoreApplication" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <script src="/CTI/CTITool.js" type="text/javascript"></script>
</head>
<body>
    <form id="form1" runat="server">
    <div class="pop pb15 openwindow" style="width: 500px">
        <div class="title bold">
            <h2>
                应用范围管理</h2>
            <span><a href="javascript:void(0)" onclick="javascript:$.closePopupLayer('scoreApplication',false);">
            </a></span>
        </div>
        <br />
        <div class="Table2" id="divScoreApplicationList" style="margin-left: 20px; width: 464px;
            text-align: center">
            <table border="1" cellpadding="0" cellspacing="0" class="Table2List" id="tableQueryScoreApplication"
                width="98%">
                <tbody id="trList">
                    <tr class="bold">
                        <th style="color: Black; font-weight: bold">
                            所属分组
                        </th>
                        <th style="color: Black; font-weight: bold">
                            评分表
                        </th>
                    </tr>
                    <asp:Repeater ID="repeaterScoreApplicationList" runat="server">
                        <ItemTemplate>
                            <tr class="back" onmouseover="this.className='hover'" onmouseout="this.className='back'">
                                <td id='<%#Eval("BGID")%>'>
                                    <%#Eval("Name")%>&nbsp;
                                </td>
                                <td>
                                    <select id="selScoreTable<%#Eval("BGID")%>" class="w125" style='width: 193px; *width: 195px;'
                                        name="ScoreTable">
                                    </select>
                                    &nbsp;
                                </td>
                            </tr>
                        </ItemTemplate>
                    </asp:Repeater>
                </tbody>
            </table>
        </div>
        <ul class="clearfix" style="text-align: center;" id="btnSumbit">
            <li>
                <input type="button" onclick="allSave();" class="btnChoose" value="保存" style="margin-right: 18px" />
                <input type="button" onclick="javascript:$.closePopupLayer('scoreApplication',false);"
                    class="btnChoose" value="关闭页面" /></li>
        </ul>
    </div>
    <script type="text/javascript">

        //保存
        function allSave() {
            var rangeStr = "";
            $("#tableQueryScoreApplication tr:not(:eq(0))").each(function () {
                var bgid = $(this).find("td:eq(0)").attr("id");
                rangeStr += bgid + "$" + $(this).find("td select[id='selScoreTable" + bgid + "']").val() + "|";
            });
            rangeStr = rangeStr.substr(0, rangeStr.length - 1);
            AjaxPostAsync("/AjaxServers/QualityStandard/ScoreTableManage/ApplicationHandler.ashx", { Action: "RangeManage", RangeStr: rangeStr, r: Math.random() }, null, function (data) {
                var jsonData = $.evalJSON(data);
                msg = jsonData.msg;

                $.jAlert(msg, function () {
                    $.closePopupLayer('scoreApplication', false);
                }); 

            });

        }

        //评分表
        function getScoreTableName() {
            AjaxPostAsync("/AjaxServers/CommonHandler.ashx", { Action: "getFieldName", TableName: "QS_RulesTable", IDField: "QS_RTID", ShowField: "Name", TableStatus: "10003", r: Math.random() }, null, function (data) {
                $("select[name='ScoreTable']").append("<option value='-1'>请选择</option>");
                var jsonData = $.evalJSON(data);
                if (jsonData != "") {
                    for (var i = 0; i < jsonData.length; i++) {
                        $("select[name='ScoreTable']").append("<option value=" + jsonData[i].ID + ">" + jsonData[i].Name + "</option>");
                    }
                }
            });
        }

        //绑定原有值
        function bindRTID() {
            if ("<%=rangeStr %>" != "") {
                var range = "<%=rangeStr %>".split('|');
                for (var i = 0; i < range.length; i++) {
                    var itemStr = range[i].split('$');
                    if (itemStr.length == 2) {
                        $("#selScoreTable" + itemStr[0]).val(itemStr[1]);
                    }
                }
            }
        }

        $(function () {
            getScoreTableName();
            bindRTID();
        });
    </script>
    </form>
</body>
</html>
