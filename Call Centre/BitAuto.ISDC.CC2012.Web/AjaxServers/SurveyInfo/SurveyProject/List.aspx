<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="List.aspx.cs" Inherits="BitAuto.ISDC.CC2012.Web.AjaxServers.SurveyInfo.SurveyProject.List" %>

<script type="text/javascript">
    function chkAllChecked() {
        if ($("#chkAll").attr("checked")) {
            $("input[name='chkSelect']").each(function () {
                $(this).attr("checked", true)
            });
        }
        else {
            $("input[name='chkSelect']").each(function () {
                $(this).attr("checked", false)
            });
        }
    }
    function CheckBoxControl(value) {
        switch (value) {
            case 1: //全选
                $("input[name='chkSelect']").each(function () {
                    $(this).attr("checked", true);
                });
                break;
            case 2: //反选
                $("input[name='chkSelect']").each(function () {
                    $(this).attr("checked", false);
                });
                break;
            case 3:
                $("input[name='chkSelect']").each(function () {
                    if ($(this).attr("checked")) {
                        $(this).attr("checked", false);
                    }
                    else {
                        $(this).attr("checked", true);
                    }
                });
                break;
        }
    }
    function DeleteSurveyProjectInfo(spiId) {
        $.jConfirm("确定删除此调查项目吗？", function (r) {
            if (r) {
                $.post("/AjaxServers/SurveyInfo/SurveyProject/Handler.ashx", { Action: "DeleteSurveyProject", SPIID: spiId }, function (data) {
                    if (data == "success") {
                        $.jPopMsgLayer("删除成功！");
                        search();
                    }
                    else {
                        $.jAlert(data);
                    }
                });
            }
        });
    }
    function CompleteSurveyProjectInfo(spiId) {
        $.jConfirm("确定将此调查项目设置为完成吗？", function (r) {
            if (r) {
                $.post("/AjaxServers/SurveyInfo/SurveyProject/Handler.ashx", { Action: "CompleteSurveyProject", SPIID: spiId }, function (data) {
                    if (data == "success") {
                        $.jPopMsgLayer("操作成功！");
                        search();
                    }
                    else {
                        $.jAlert(data);
                    }
                });
            }
        });
    }
    $(document).ready(function () {
        $(".hrefExport").each(function () {
            $(this).attr("href", $(this).attr("href") + "&Browser=" + GetBrowserName())
        });
    });
</script>
<table cellpadding="0" cellspacing="0" class="tableList mt10 mb15" width="99%" id="tableList">
    <tr class="back" onmouseout="this.className='back'">
        <th style="width: 10%;">
            分类
        </th>
        <th style="width: 20%;">
            调查名称
        </th>
        <th style="width: 10%;">
            预计完成
        </th>
        <th style="width: 10%;">
            实际完成
        </th>
        <th style="width: 15%;">
            调查日期
        </th>
        <th style="width: 10%;">
            创建人
        </th>
        <th style="width: 10%;">
            状态
        </th>
        <th>
            操作
        </th>
    </tr>
    <asp:repeater id="rptSurveyProject" runat="server">
        <ItemTemplate>
            <tr class="back" onmouseover="this.className='hover'" onmouseout="this.className='back'">
                <td>
                <%#Eval("CategoryName")%>
                </td>
                <td>
                    <%#Eval("Name") %>
                </td>                       
                <td>
                <%#ShowEstimatePersonNum(Eval("SPIID").ToString())%>
                </td>
                <td>
                <%#ShowTruePersonNum(Eval("SPIID").ToString())%>
                </td>
                <td>
                    <%# BitAuto.ISDC.CC2012.Entities.CommonFunction.GetDateTimeStrForPage(Eval("SurveyStartTime").ToString())%>&nbsp;
                </td>
                <td>
                <%#ShowCreateUserName(Eval("CreateUserID").ToString())%>
                </td>
                <td>
                    <%# ShowSurveyProjectStatusStr(Eval("SurveyStartTime").ToString(), Eval("SurveyEndTime").ToString(), Eval("Status").ToString())%>
                </td>
                <td>
                <%#ShowButtonHtml(Eval("SurveyStartTime").ToString(), Eval("SurveyEndTime").ToString(), Eval("Status").ToString(), Eval("SPIID").ToString())%>
                </td>
            </tr>
        </ItemTemplate>
    </asp:repeater>
</table>
<br />
<!--分页-->
<div class="pages1" style="text-align: right;">
    <uc:AjaxPager ID="AjaxPager_Project" runat="server" ContentElementId="ajaxTable" />
</div>
