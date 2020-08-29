<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="KnowledgeLibCount.aspx.cs"
    Inherits="BitAuto.ISDC.CC2012.Web.AjaxServers.KnowledgeLib.KnowledgeLibCount" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <script type="text/javascript">
        //分页操作
        function ShowDataByPost1(pody) {
            LoadingAnimation("tableList");
            $('.bit_table').load('../AjaxServers/KnowledgeLib/KnowledgeLibCount.aspx .bit_table > *', pody);
        }
    </script>
</head>
<body>
    <form id="form1" runat="server">
    <div class="bit_table">
        <table cellpadding="0" cellspacing="0" class="tableList" id="tableList" width="99%">
            <tr class="bold">
                <th>
                    创建人
                </th> 
                <th>
                    未提交
                </th>
                <th>
                    待审核
                </th>
                <th>
                    已发布
                </th>
                <th>
                    已驳回
                </th>
                <th>
                    已停用
                </th>
            </tr>
            <asp:Repeater ID="repeaterTableList" runat="server">
                <ItemTemplate>
                    <tr class="back" onmouseover="this.className='hover'" onmouseout="this.className='back'">
                        <td>
                            <%#getCreateUserName(Eval("CreateUserID").ToString())%>&nbsp;
                        </td> 
                        <td>
                            <%#Eval("UncommittedCount")%>&nbsp;
                        </td>
                        <td>
                            <%#Eval("WaitApprovalCount")%>&nbsp;
                        </td>
                        <td>
                            <%#Eval("ReleaseCount")%>&nbsp;
                        </td>
                        <td>
                            <%#Eval("RejectCount")%>&nbsp;
                        </td>
                        <td>
                            <%#Eval("DisableCount")%>&nbsp;
                        </td>
                    </tr>
                </ItemTemplate>
            </asp:Repeater>
        </table> 
        <!--分页-->
        <div class="pageTurn mr10" style=" margin-top:10px;">
            <p>
                <asp:Literal runat="server" ID="litPagerDown"></asp:Literal>
            </p>
        </div>
    </div>
    </form>
</body>
</html>
