<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="MonitorData.aspx.cs" Inherits="BitAuto.DSC.IM_DMS2014.Web.AjaxServers.QueueManage.MonitorData" %>

<!--列表开始-->
<asp:repeater runat="server" id="Rt_CSData">
            <HeaderTemplate>
                  <table border="0" cellspacing="0" cellpadding="0">
            </HeaderTemplate>
            <ItemTemplate>
                  <tr class="jk_bg">
                    <td width="23%" class="cName"><%#Eval("MemberName")%>&nbsp;</td>
                    <td width="14%"><%#Eval("ProvinceName")%>-<%#Eval("CityName")%>-<%#Eval("CountyName")%>&nbsp; </td>
                    <td width="8%"><%#Eval("DistrictName")%>&nbsp;</td>
                    <td width="10%"><%#Eval("TrueName")%>&nbsp;</td>
                    <td width="8%"><%#Eval("CreateTime")%>&nbsp;</td>
                    <td width="14%"><%#Eval("lastMessageTime")%>&nbsp;</td>
                    <td width="16%" class="cName"><%#Eval("UserReferTitle")%>&nbsp;</td>
                    <td width="7%"><div class="btn">
                          <input type="button" value="监控" onclick="startMonitor('<%#Eval("CSID")%>','initialTime','<%#Eval("MemberName")%>')"  class="w60 gray"/>
                          <input class="hidcsidinput" type="hidden" value='<%#Eval("CSID")%>'/>
                        </div>
                    </td>
                  </tr>
            </ItemTemplate>
            <FooterTemplate>
                </table>
            </FooterTemplate>
         </asp:repeater>
<!--列表结束-->
<br />
<!--分页-->
<div class="pageTurn mr10">
    <p>
        <asp:literal runat="server" id="litPagerDown"></asp:literal>
    </p>
</div>
<input type="hidden" value="<%=RecordCount %>" id="hidTotalCount" />
