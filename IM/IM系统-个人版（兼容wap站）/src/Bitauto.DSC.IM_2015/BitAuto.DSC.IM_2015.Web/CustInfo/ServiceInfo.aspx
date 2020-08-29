<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ServiceInfo.aspx.cs" Inherits="BitAuto.DSC.IM_2015.Web.CustInfo.ServiceInfo" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <div id="con_two_2">
        <asp:Repeater ID="repterList" runat="server">
            <ItemTemplate>
                <table <%# (Container.ItemIndex + 1)%2==0?"class='mt10'":""%> border="0" cellspacing="0"
                    cellpadding="0">
                    <tr>
                        <th width="38%">
                            <img src="../Images/import.png" width="16" height="18" <%#GetStrByOrderID(Eval("TaskID").ToString())%> />任务ID：
                        </th>
                        <td width="60%">
                            <a href="http://<%#Eval("TaskUrl").ToString()%>" target="_blank">
                                <%#Eval("TaskID").ToString()%></a>
                        </td>
                    </tr>
                    <tr>
                        <th>
                            类型：
                        </th>
                        <td>
                            <%#Eval("RecordType")%>
                        </td>
                    </tr>
                    <tr>
                        <th>
                            操作时间：
                        </th>
                        <td>
                            <%#Eval("Lastopertime")%>
                        </td>
                    </tr>
                    <tr>
                        <th>
                            操作人：
                        </th>
                        <td>
                            <%#BitAuto.YanFa.SysRightManager.Common.UserInfo.GerTrueName(int.Parse(Eval("AssignUserID").ToString()))%>
                        </td>
                    </tr>
                    <tr>
                        <th>
                            状态：
                        </th>
                        <td>
                            <%#Eval("Status")%>
                        </td>
                    </tr>
                </table>
            </ItemTemplate>
        </asp:Repeater>
        <%-- <table border="0" cellspacing="0" cellpadding="0" class="mt10">--%>
    </div>
    </form>
</body>
</html>
