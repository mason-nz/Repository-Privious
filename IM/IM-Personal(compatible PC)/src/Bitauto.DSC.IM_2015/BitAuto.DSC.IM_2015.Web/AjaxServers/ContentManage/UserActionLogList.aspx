<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="UserActionLogList.aspx.cs"
    Inherits="BitAuto.DSC.IM_2015.Web.AjaxServers.ContentManage.UserActionLogList" %>

<table border="0" cellspacing="0" cellpadding="0">
    <tr>
        <th width="15%">
            操作人
        </th>
        <th width="10%">
            操作人类型
        </th>
        <th width="15%">
            日志类型
        </th>
        <th width="50%">
            日志内容
        </th>
        <th width="10%">
            操作时间
        </th>
    </tr>
    <asp:repeater id="repeaterTableList" runat="server">
                        <ItemTemplate>
                            <tr style="cursor:pointer" class="" onclick="">
                                <td class="cName" name="csName">
                                    <%# Eval("OperUserType").ToString() == "1"?Eval("agentName"):(Eval("OperUserType").ToString() == "2"?Eval("truename"):"系统") %>&nbsp;
                                </td>
                                <td name="ltName">
                                   <%#Eval("OperUserType").ToString() == "1"?"坐席":(Eval("OperUserType").ToString() == "2"?"网友":"系统")%>&nbsp;
                                </td>
                                <td>
                                   <%#GetLoginfotype(Eval("LogInType").ToString())%>&nbsp;
                                </td>
                                <td class="cName" title="<%#Eval("loginfo")%>">
                                   <%#Eval("loginfo").ToString().Length > 80 ? Eval("loginfo").ToString().Substring(0, 80) + "..." : Eval("loginfo").ToString()%>
                                </td>  
                                <td>
                                    <%#Eval("createtime")%>&nbsp;
                                </td>                                
                            </tr>
                        </ItemTemplate>
                    </asp:repeater>
</table>
<!--列表结束-->
<!--分页开始-->
<div class="pagesnew" style="float: right; margin: 10px;" id="itPage">
    <p>
        <asp:literal runat="server" id="litPagerDown"></asp:literal>
    </p>
</div>
