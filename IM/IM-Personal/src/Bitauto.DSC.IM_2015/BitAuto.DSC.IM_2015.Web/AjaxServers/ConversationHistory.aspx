<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ConversationHistory.aspx.cs" Inherits="BitAuto.DSC.IM_2015.Web.AjaxServers.ConversationHistory" %>

    <asp:repeater runat="server" id="Rt_CSHistoryData">
        <ItemTemplate>
            <div class="dh1">
                <div class="title">
                  <%#Eval("newName").ToString()%>
                </div>
                <div class="dhc">
                    <%#Eval("Content").ToString()%>
                </div>
            </div>
        </ItemTemplate>
    </asp:repeater>
    <br />
    <!--分页-->
    <div class="pageTurn mr10">
        <p>
            <asp:literal runat="server" id="litPagerDown_History"></asp:literal>
        </p>
    </div>
    <input type="hidden" value="<%=RecordCount %>" id="hidHistoryTotalCount" />

