<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="List.aspx.cs" Inherits="BitAuto.ISDC.CC2012.Web.AjaxServers.QualityScoring.ScoreTableManage.List" %>

<table cellpadding="0" cellspacing="0" class="tableList mt10 mb15" width="99%" id="tableList">
    <tr class="back" onmouseout="this.className='back'">
        <th width="10%">
            评分表名称
        </th>
        <th width="13%">
            创建日期
        </th>
        <th width="8%">
            创建人
        </th>
        <th width="40%">
            应用范围
        </th>
        <th width="5%">
            状态
        </th>
        <th width="8%">
            使用状态
        </th>
       <%-- <th width="7%">
            适用区域
        </th>--%>
        <th width="7%">
            操作
        </th>
    </tr>
    <asp:repeater id="repeaterTableList" runat="server">
        <ItemTemplate>
            <tr class="back" onmouseover="this.className='hover'" onmouseout="this.className='back'">
                <td>
                    <a href='/QualityStandard/ScoreTableManage/ScoreTableView.aspx?QS_RTID=<%#Eval("QS_RTID") %>' target="_blank"><%# Eval("Name")%></a>&nbsp;
                </td> 
                <td>
                     <%# BitAuto.ISDC.CC2012.Entities.CommonFunction.GetDateTimeStrForPage(Eval("CreateTime").ToString())%>&nbsp;
                </td>                
                <td>
                    <%# Eval("TrueName")%>&nbsp;
                </td>  
                <td>
                    <%# Eval("Groups")%>&nbsp;
                </td>  
                <td>
                    <%# Eval("StatusName")%>&nbsp;
                </td> 
                <td>
                    <%# Eval("isInUse").ToString() == "0" ? "未使用" : "已使用"%>&nbsp;
                </td>
          <%--      <td>
                    <%# Eval("RegionID").ToString() == "1" ? "北京" : Eval("RegionID").ToString() == "2" ? "西安" : Eval("RegionID").ToString() == "1,2" ? "北京、西安" : ""%>&nbsp;
                </td>--%>
                <td >
                <%#oper(Eval("Status").ToString(), Eval("isInUse").ToString(), Eval("QS_RTID").ToString(), Eval("CreateUserID").ToString())%> &nbsp; </td> 
            </tr>
        </ItemTemplate>
    </asp:repeater>
</table>
<!--分页-->
<div class="pages1" style="text-align: right;">
    <uc:AjaxPager ID="AjaxPager" runat="server" ContentElementId="ajaxTable" />
</div>
