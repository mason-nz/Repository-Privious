<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="List.aspx.cs" Inherits="BitAuto.ISDC.CC2012.Web.AjaxServers.Demand.DemandHandle.List" %>

<div class="optionBtn clearfix">
    <input type="button" class="newBtn mr10" onclick="ExportData()" id="lbtnExport" value="导出"
        name="">
    <span>查询结果</span><small><span>总计:<%=totalCount%></span></small>
</div>
<div id="divList" class="bit_table">
    <table width="99%" border="0" cellspacing="0" cellpadding="0" class="tableList" id="tableReturnVisitCust">
        <tr class="color_hui">
            <th width="8%">
                <strong>需求编号</strong>
            </th>
            <th width="6%">
                <strong>会员ID</strong>
            </th>
            <th width="18%">
                <strong>经销商名称</strong>
            </th>
            <th width="15%">
                <strong>服务周期</strong>
            </th>
            <th width="7%">
                <strong>集客数量</strong>
            </th>
            <th width="10%">
                <strong>已集客数量</strong>
            </th>
            <th width="10%">
                <strong>申请日期</strong>
            </th>
            <th width="6%">
                <strong>需求状态</strong>
            </th>
            <th width="6%">
                <strong>负责客服</strong>
            </th>
             <th width="6%">
                <strong>负责销售</strong>
            </th>
            <th width="5%">
                <strong>操作</strong>
            </th>
        </tr>
        <asp:repeater runat="server" id="rplist">
         <ItemTemplate>
                            <tr  class="back" onmouseover="this.className='hover'" onmouseout="this.className='back'">
                           <td align="center">
                                    <%#Eval("DemandID")%>&nbsp;
                                </td>
                                <td align="center">
                                    <%# Eval("MemberCode")%>&nbsp;
                                </td>
                                <td align="center">
                                    <%# Eval("MemberName")%>&nbsp;
                                </td>
                                <td align="center">
                                    <%# BitAuto.ISDC.CC2012.Entities.CommonFunction.GetDateTimeStrForPage(Eval("BeginTime").ToString(), "yyyy-MM-dd")%>
                                    至 
                                    <%# BitAuto.ISDC.CC2012.Entities.CommonFunction.GetDateTimeStrForPage(Eval("EndTime").ToString(), "yyyy-MM-dd")%>
                                </td>
                               <td align="center">
                                    <%#Eval("ExpectedNum")%>&nbsp;
                                </td>
                                <td align="center">
                                    <%# Eval("PracticalNum").ToString() == "0" ? "-" : Eval("PracticalNum")%>&nbsp;
                                </td>
                                 <td align="center">
                                    <%# BitAuto.ISDC.CC2012.Entities.CommonFunction.GetDateTimeStrForPage(Eval("CreateTime").ToString(), "yyyy-MM-dd")%>&nbsp;
                                </td>
                                <td align="center">
                                    <%#BitAuto.YanFa.Crm2009.BLL.DictInfo.Instance.GetDictName(int.Parse(Eval("Status").ToString()))%>
                                </td>
                                <td align="center">
                                    <%#Eval("KeFuName")%>&nbsp;
                                </td>
                                 <td align="center">                                
                                    <%#Eval("SaleName")%>&nbsp;
                                </td>
                                <td align="center">
                                <a href="<%=demandDetailsUrl %>?DemandID=<%#Eval("DemandID") %>&R=<%=GetRandomStr() %>" target="Demand_List">查看</a>
                                </td>
                            </tr>
                        </ItemTemplate>
        </asp:repeater>
    </table>
    <div class="pageTurn mr10" style="margin-top: 10px;">
        <p>
            <asp:literal runat="server" id="litPagerDown"></asp:literal>
        </p>
    </div>
</div>
