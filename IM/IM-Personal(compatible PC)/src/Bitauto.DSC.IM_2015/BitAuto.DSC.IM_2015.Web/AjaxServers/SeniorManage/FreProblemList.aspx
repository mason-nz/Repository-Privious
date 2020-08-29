<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="FreProblemList.aspx.cs"
    Inherits="BitAuto.DSC.IM_2015.Web.AjaxServers.SeniorManage.FreProblemList" %>

<script type="text/javascript">
    //修改
    function ModFreProblem(recid) {
        $.openPopupLayer({
            name: "AddFreProblem",
            url: "/SeniorManage/AddFreProblem.aspx?RecID=" + recid + "&r=" + Math.random(),
            beforeClose: function (e) {
                if (e) {
                    Search();
                }
            }
        });
    }
    //删除
    function DelFreProblem(recid) {
        $.jConfirm("确定删除此常见问题吗？", function (r) {
            if (r) {
                var pody = {
                    Action: 'del',
                    RecID: recid,
                    r: Math.random
                };
                AjaxPostAsync("/AjaxServers/SeniorManage/FreProblemHandler.ashx", pody, function () { }, function (data) {
                    var jsonData = $.evalJSON(data);
                    if (jsonData.result == "success") {
                        Search();
                    }
                    else {
                        $.jAlert("删除失败：" + jsonData.msg);
                    }
                });
            }
        });
    }
    //移动
    function MoveUpOrDown(recid, dir) {
        var pody = {
            Action: 'move',
            RecID: recid,
            Direct: dir,
            r: Math.random
        };
        AjaxPostAsync("/AjaxServers/SeniorManage/FreProblemHandler.ashx", pody, function () { }, function (data) {
            var jsonData = $.evalJSON(data);
            if (jsonData.result == "success") {
                Search();
            }
            else {
                $.jAlert("移动失败：" + jsonData.msg);
            }
        });
    }
</script>
<table border="0" cellspacing="0" cellpadding="0">
    <tr>
        <th width="15%">
            标题
        </th>
        <th width="30%">
            链接地址
        </th>
        <th width="20%">
            备注
        </th>
        <th width="20%">
            应用业务线
        </th>
        <th width="15%">
            操作
        </th>
    </tr>
    <asp:repeater id="dataRepeater" runat="server">
        <ItemTemplate>
            <tr>
                <td class="cName">
                    <%#Eval("Title")%>
                </td>
                <td class="cName">
                    <%#Eval("Url")%>
                </td>
                  <td class="cName">
                    <%#Eval("Remark")%>
                </td>
                   <td class="cName">
                    <%#GetSourceTypeName(Eval("SourceType").ToString())%>
                </td>
                <td>
                    <a href="javascript:void(0)" onclick="ModFreProblem('<%#Eval("RecID") %>')">修改</a>
                    <a href="javascript:void(0)" onclick="DelFreProblem('<%#Eval("RecID") %>')">删除</a> 
                    <%#Eval("RecID").ToString().Trim() != MinRecID.ToString() ? "<a href=\"javascript:void(0)\" onclick=\"MoveUpOrDown('"+Eval("RecID").ToString()+"','up')\">上移</a>" : "<span style=\"color:#666;\">上移</span>"%>
                    <%#Eval("RecID").ToString().Trim() != MaxRecID.ToString() ? "<a href=\"javascript:void(0)\" onclick=\"MoveUpOrDown('" + Eval("RecID").ToString() + "','down')\">下移</a>" : "<span style=\"color:#666;\">下移</span>"%>
                </td>
            </tr>
        </ItemTemplate>
    </asp:repeater>
    <tr>
        <td colspan="11">
            <div class="pagesnew" style="float: right; margin: 10px;" id="itPage">
                <p>
                    <asp:literal runat="server" id="litPagerDown"></asp:literal>
                </p>
            </div>
        </td>
    </tr>
</table>
