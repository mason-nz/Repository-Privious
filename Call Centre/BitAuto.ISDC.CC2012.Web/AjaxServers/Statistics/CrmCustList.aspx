<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="CrmCustList.aspx.cs" Inherits="BitAuto.ISDC.CC2012.Web.AjaxServers.Statistics.CrmCustList" %>

<form id="form1" runat="server">
<div class="optionBtn  clearfix" style="width: 97%">
    <div>
        <%if (BitAuto.ISDC.CC2012.BLL.Util.CheckButtonRight("SYS024BUT2210"))
          { %>
        <input name="" type="button" value="导出" id="lbtnContactExport" onclick="ExportCust()"
            class="newBtn mr10" />
        <%} %>
        <%if (BitAuto.ISDC.CC2012.BLL.Util.CheckButtonRight("SYS024BUT2209"))
          { %>
        <input name="" type="button" value="新增客户" onclick="openAddCustPage()" class="newBtn mr10" />
        <%} %>
        <span>查询结果</span><small><span>总计：<%= RecordCount%></span></small>
    </div>
</div>
<div class="bit_table">
    <table width="99%" border="0" cellspacing="0" cellpadding="0" class="tableList" id="tableCustInfo">
        <tr>
            <th>
                客户ID
            </th>
            <th width="200px">
                客户名称
            </th>
            <th>
                主营品牌
            </th>
            <th>
                客户地区
            </th>
            <th>
                会员ID
            </th>
            <th>
                状态
            </th>
            <th>
                锁定
            </th>
            <th>
                年检
            </th>
        </tr>
        <asp:repeater id="Repeater_Custs" runat="server">
        <ItemTemplate>
                    <tr class="back" onmouseover="this.className='hover'" onmouseout="this.className='back'">
                <td align="center">
                    <a target="_blank" href="/CustCheck/CrmCustSearch/CustDetail.aspx?CustID=<%# Eval("CustID").ToString() %>"><%#Eval("CustID")%></a>&nbsp;
                </td>  
                <td align="center">
                    <%#Eval("CustName")%>&nbsp;
                </td>               
                <td align="center">
                    <%#Eval("BrandNames") %>&nbsp;
                </td>
                <td align="center">
                    <%#Eval("ProvinceName")%> <%#Eval("CityName")%> <%#Eval("CountyName")%>&nbsp;
                </td>
                <td align="center">
                    <%#ctrLen(Eval("BrandIDs").ToString()) %>&nbsp;
                </td>
                <td align="center">
                                    <%# Eval("status").ToString() == "0" ? "<img title='在用' src='/Images/xt.gif'/>" : Eval("status").ToString() == "1" ? "<img title='停用' src='/Images/xt_1.gif'/>" : "" %>&nbsp;
                                </td>
                                <td align="center">
                                    <%# (Eval("lock") == DBNull.Value || Eval("lock").ToString() == "0") ? "<img title='未锁定' src='/Images/unlock.gif'/>" : "<img title='锁定' src='/Images/lock.gif'/>"%>&nbsp;
                                </td>
                                <td align="center">
                                    <%#  Eval("BLAnnualSurvey").ToString() == "-1" ? "<img title='未通过' src='/Images/xt_1.gif'/>" : (Eval("BLAnnualSurvey").ToString() == "1" ? "<img title='通过' src='/Images/xt.gif'/>" : "")%>&nbsp;
                                </td>
            </tr>
        </ItemTemplate>
    </asp:repeater>
    </table>
    <div class="pages1" style="text-align: right;">
        <uc:AjaxPager ID="AjaxPager_Custs" runat="server" />
    </div>
</div>
</form>
