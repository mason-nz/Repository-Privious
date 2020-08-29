<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="SurveyInfoList.aspx.cs"
    Inherits="BitAuto.ISDC.CC2012.Web.AjaxServers.SurveyInfo.SurveyInfoList" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <script type="text/javascript" language="javascript">
    </script>
</head>
<body>
    <form id="form1" runat="server">
    <div class="bit_table">
        <table cellpadding="0" cellspacing="0" class="tableList" width="99%" id="tableList">
            <tr>
                <th>
                    所属分组
                </th>
                <th>
                    分类
                </th>
                <th>
                    问卷名称
                </th>
                <th>
                    创建日期
                </th>
                <th>
                    创建人
                </th>
                <th>
                    状态
                </th>
                <th>
                    是否可用
                </th>
                <th>
                    操作
                </th>
            </tr>
            <asp:Repeater ID="repeaterTableList" runat="server">
                <ItemTemplate>
                    <tr class="back" onmouseover="this.className='hover'" onmouseout="this.className='back'">
                        <td>
                            <%#Eval("GroupName") %>&nbsp;
                        </td>
                        <td>
                            <%#Eval("CategoryName")%>&nbsp;
                        </td>
                        <td>
                            <%#Eval("NAME")%>&nbsp;
                        </td>
                        <td>
                            <%# BitAuto.ISDC.CC2012.Entities.CommonFunction.GetDateTimeStrForPage(Eval("CreateTime").ToString())%>&nbsp;
                        </td>
                        <td>
                            <%#getOperator(Eval("CreateUserID").ToString())%>&nbsp;
                        </td>
                        <td>
                            <%#getStatus(Eval("Status").ToString())%>&nbsp;
                        </td>
                        <td>
                            <%#Eval("IsAvailable").ToString()!="1"?(Eval("IsAvailable").ToString()=="0"?"停用":"不可用"):"启用" %>&nbsp;
                        </td>
                        <td>
                            <%#getOperLink(Eval("Status").ToString(),Eval("SIID").ToString(),Eval("IsAvailable").ToString())%>
                        </td>
                    </tr>
                </ItemTemplate>
            </asp:Repeater>
        </table> 
        <div class="pageTurn mr10" style="margin-top: 10px;">
            <p>
                <asp:Literal runat="server" ID="litPagerDown"></asp:Literal>
            </p>
        </div>
    </div>
    </form>
</body>
</html>
