<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="KLQuestionList.aspx.cs"
    Inherits="BitAuto.ISDC.CC2012.Web.AjaxServers.KnowledgeLib.KLQuestionList" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <script type="text/javascript">
        //分页操作
        function ShowDataByPost1(pody) {
            LoadingAnimation("tableList");
            $('.bit_table').load('../AjaxServers/KnowledgeLib/KLQuestionList.aspx .bit_table > *', pody);
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
                <th width="25%">
                    试题
                </th>
                <th>
                    题型
                </th>
                <th>
                    分类
                </th>
                <th>
                    修改人
                </th>
                <th>
                    修改日期
                </th>
                <th>
                    知识点状态
                </th>
                <th>
                    试题状态
                </th>
                <th>
                    操作
                </th>
            </tr>
            <asp:Repeater ID="repeaterTableList" runat="server">
                <ItemTemplate>
                    <tr class="back" onmouseover="this.className='hover'" onmouseout="this.className='back'">
                        <td align="center">
                            <%# getCheckStr(Eval("Status").ToString(),Eval("KLQID").ToString())%>
                           <%-- <input type="checkbox" name="chkOper" value="<%#Eval("KLQID")%>" />--%>
                        </td>
                        <td>
                            <div class="deal" title="<%#Eval("Ask")%>">
                                <a href="/KnowledgeLib/KnowledgeView.aspx?kid=<%#Eval("KLID")%>#<%#Eval("KLQID") %>" target="_blank">
                                    <%#Eval("Ask")%></a></div>
                        </td>
                        <td>
                            <%#getAskCategory(Eval("AskCategory").ToString())%>&nbsp;
                        </td>
                        <td>
                            <%#Eval("kcName")%>&nbsp;
                        </td>
                        <td>
                            <%#getCreateUserName(Eval("ModifyUserid").ToString())%>&nbsp;
                        </td>
                        <td>
                            <%#BitAuto.ISDC.CC2012.Entities.CommonFunction.GetDateTimeStrForPage(Eval("ModifyTime").ToString(), "yyyy-MM-dd HH:mm:ss")%>&nbsp;
                        </td>
                        <td>
                            <%#getStatusName(Eval("libStatus").ToString())%>&nbsp;
                        </td>
                        <td>
                            <%#getQuestionStatusName(Eval("Status").ToString())%>&nbsp;
                        </td>
                        <td>
                         <%#getOperator(Eval("libStatus").ToString(), Eval("libKLID").ToString(), Eval("CreateUserID").ToString())%>&nbsp;
                        </td>
                    </tr>
                </ItemTemplate>
            </asp:Repeater>
        </table>
        <!--分页-->
        <div class="optionBtn clearfix" style=" margin-top:10px;">
            <span style="display: inline; width: 540px;">
                <input name="" type="button" value="停用" onclick="javascript:QuestionDisable(getKLIDs())"
                    class="newBtn" runat="server" id="btnDisable" style="float: left;" />
            </span>
            <p style="float: right; display: inline;">
                <asp:Literal runat="server" ID="litPagerDown"></asp:Literal>
            </p>
        </div>
    </div>
    </form>
</body>
</html>
