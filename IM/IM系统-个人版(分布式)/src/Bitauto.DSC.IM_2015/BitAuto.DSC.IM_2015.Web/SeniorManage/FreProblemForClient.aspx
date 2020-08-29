<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="FreProblemForClient.aspx.cs"
    Inherits="BitAuto.DSC.IM_2015.Web.SeniorManage.FreProblemForClient" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html>
<body>
    <asp:Repeater ID="repeaterList" runat="server">
        <ItemTemplate>
            <ul>
                <li><a href="<%# Eval("url")%>" target="_blank">
                    <%# Eval("Title")%></a> </li>
            </ul>
        </ItemTemplate>
    </asp:Repeater>
    <ul class="more" style="float: right">
        <li><a href="<%=MoreURL%>" target="_blank">更多&gt;&gt;</a> </li>
    </ul>
</body>
</html>
