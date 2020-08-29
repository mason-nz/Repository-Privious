<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="MonitorData.aspx.cs" Inherits="BitAuto.DSC.IM_2015.Web.AjaxServers.QueueManage.MonitorData" %>

<!--列表开始-->
<asp:repeater runat="server" id="Rt_CSData">
            <ItemTemplate>
                  <tr class="jk_bg">
                    <td width="10%" class="cName"><%#Eval("UserName")%>&nbsp;</td>
                    <td width="12%"><%#Eval("ProvinceCityName")%>&nbsp; </td>
                    <td width="10%"><%#Eval("TrueName")%>&nbsp;</td>
                    <td width="8%"><%#Eval("AgentNum")%>&nbsp;</td>
                    <td width="11%"><%#Eval("BGName")%>&nbsp;</td>
                    <td width="10%"><%#Eval("CreateTime")%>&nbsp;</td>
                    <td width="10%"><%#Eval("newLastClientTime")%>&nbsp;</td>
                    <td width="10%"><%# GetSourceTypeName(Eval("SourceType").ToString())%>&nbsp;</td>
                    <td width="12%" class="cName"><%#Eval("UserReferTitle")%>&nbsp;</td>
                    <td width="7%"><div class="btn">
                          <input type="button" value="监控" onclick="startMonitor('<%#Eval("CSID")%>','initialTime','<%#Eval("UserName")%>')"  class="w60 gray"/>
                          <input class="hidcsidinput" type="hidden" value='<%#Eval("CSID")%>'/>
                        </div>
                    </td>
                  </tr>
            </ItemTemplate>
         </asp:repeater>
