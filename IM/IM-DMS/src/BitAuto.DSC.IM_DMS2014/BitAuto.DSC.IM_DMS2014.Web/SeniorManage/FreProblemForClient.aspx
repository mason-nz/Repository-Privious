﻿<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="FreProblemForClient.aspx.cs"
    Inherits="BitAuto.DSC.IM_DMS2014.Web.SeniorManage.FreProblemForClient" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html>
<body>
    <ul>
        <asp:Repeater ID="repeaterList" runat="server">
            <ItemTemplate>
                <li><a href="<%# Eval("url")%>" target="_blank">
                    <%# Eval("Title")%></a></li>
            </ItemTemplate>
        </asp:Repeater>
        <li class="more"><a href="#">更多&gt;&gt;</a></li>
    </ul>
</body>
</html>
