<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="KnowledgeList.aspx.cs"
    Inherits="BitAuto.ISDC.CC2012.Web.AjaxServers.KnowledgeLib.KnowledgeList" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <script type="text/javascript">
        function checkboxAll() {
            if ($("#chkAll").attr("checked") == true) {
                $(":checkbox[name='chkOper']:enabled").attr("checked", true);
            }
            else {
                $(":checkbox[name='chkOper']:enabled").attr("checked", false);
            }
        }
        //分页操作
        function ShowDataByPost1(pody) {
            LoadingAnimation("tableList");
            $('.bit_table').load('../AjaxServers/KnowledgeLib/KnowledgeList.aspx .bit_table > *', pody);
        }
    </script>
</head>
<body>
    <form id="form1" runat="server">
    <div class="bit_table">
        <table cellpadding="0" cellspacing="0" class="tableList" id="tableList" width="99%">
            <tr class="bold">
                <th width="5%">
                    <input type="checkbox" id="chkAll" onclick="checkboxAll()" />
                </th>
                <th width="20%">
                    标题
                </th>
                <th>
                    分类
                </th>
                <th>
                    修改人
                </th>
                <th>
                    修改时间
                </th>
                <th>
                    状态
                </th>
                <th>
                    FAQ
                </th>
                <th>
                    试题
                </th>
                <th>
                    操作
                </th>
            </tr>
            <asp:Repeater ID="repeaterTableList" runat="server">
                <ItemTemplate>
                    <tr class="back" onmouseover="this.className='hover'" onmouseout="this.className='back'">
                        <td align="center">
                            <input type="checkbox" name="chkOper" value="<%#Eval("KLID")%>" />
                        </td>
                        <td>
                            <div class="deal" title="<%#Eval("Title")%>">
                                <a href="/KnowledgeLib/KnowledgeView.aspx?kid=<%#Eval("KLID")%>" target="_blank">
                                    <%#Eval("Title")%></a>&nbsp;</div>
                        </td>
                        <td>
                            <%#Eval("kcName")%>&nbsp;
                        </td>
                        <td>
                            <%#getCreateUserName(Eval("LastModifyUserID").ToString())%>&nbsp;
                        </td>
                        <td>
                            <%#BitAuto.ISDC.CC2012.Entities.CommonFunction.GetDateTimeStrForPage(Eval("LastModifyTime").ToString(), "yyyy-MM-dd HH:mm:ss")%>&nbsp;
                        </td>
                        <td>
                            <%#getStatusName(Eval("Status").ToString())%>&nbsp;
                        </td>
                        <%--<td>
                            <%#Eval("UploadFileCount")%>&nbsp;
                        </td>--%>
                        <td>
                            <%#Eval("FAQCount")%>&nbsp;
                        </td>
                        <td>
                            <%#Eval("QuestionCount")%>&nbsp;
                        </td>
                        <td>
                            <%#getOperator(Eval("KLID").ToString(), Eval("Status").ToString(), Eval("CreateUserID").ToString())%>&nbsp;
                        </td>
                    </tr>
                </ItemTemplate>
            </asp:Repeater>
        </table>
        <!--分页-->
        <div class="optionBtn clearfix" style="margin-top: 10px;">
            <span style="display: inline; width: 540px;">
                <input name="" type="button" value="审核通过" onclick="javascript:LibByApproval(getKLIDs())"
                    class="newBtn" runat="server" id="btnApproval" style="float: left;" />
                <input name="" type="button" value="驳回" onclick="javascript:openRejectReasonPopup(getKLIDs())"
                    class="newBtn" runat="server" id="btnReject" style="float: left;" />
                <input name="" type="button" value="移动" onclick="javascript:openMovePopup()" class="newBtn"
                    runat="server" id="btnMove" style="float: left;" />
                <input name="" type="button" value="停用" onclick="javascript:LibDisable(getKLIDs())"
                    class="newBtn" runat="server" id="btnDisable" style="float: left;" />
                <input name="" type="button" value="删除" onclick="javascript:LibDelete(getKLIDs())"
                    class="newBtn" id="btnDelete" runat="server" style="float: left;" />
            </span>
            <p style="float: right; display: inline;">
                <asp:Literal runat="server" ID="litPagerDown"></asp:Literal>
            </p>
        </div>
    </div>
    </form>
</body>
</html>
