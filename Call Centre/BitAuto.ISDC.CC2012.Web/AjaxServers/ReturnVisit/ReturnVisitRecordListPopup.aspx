<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ReturnVisitRecordListPopup.aspx.cs"
    Inherits="BitAuto.ISDC.CC2012.Web.AjaxServers.ReturnVisit.ReturnVisitRecordListPopup" %>

<script>
    $('#tableReturnVisitRecord tr:even').addClass('color_hui'); //设置列表行样式
    SetTableStyle('tableReturnVisitRecord');
</script>
<div class="pop pb15 openwindow" id="divReturnVisitRecordListPopup">
    <div class="title bold">
        <h2>
            客户回访记录列表</h2>
        <span><a href="javascript:void(0)" onclick="javascript:$.closePopupLayer('ReturnVisitRecordListPopup');">
        </a></span>
    </div>
    <ul class="clearfix  outTable">
        <li style="width: 200px">
            <label>
                客户编号：</label>
            <span>
                <%=RequestCustID %></span> </li>
        <li style="width: 350px">
            <label>
                客户名称：</label>
            <span>
                <%=CustName%></span> </li>
    </ul>
    <div id="divSelectCustomerList" class="Table2">
        <table width="98%" class="Table2List" cellspacing="0" cellpadding="0" border="0"
            id="tableReturnVisitRecord" class="cxjg">
            <tbody>
                <tr>
                    <th width="15%">
                        访问方式
                    </th>
                    <th width="17%">
                        访问分类
                    </th>
                    <th style="width: 25%;">
                        访问描述
                    </th>
                    <th width="10%">
                        客户联系人
                    </th>
                    <th width="15%">
                        访问日期
                    </th>
                    <th width="10%">
                        访问人
                    </th>
                </tr>
                <asp:repeater id="repeaterList" runat="server">
                        <ItemTemplate>
                            <tr>
                            <td align="center">
                                    <%# getTypeStr(Eval("RVType").ToString())%>&nbsp;
                                </td>
                                <td align="center">
                                    <%# getVisitTypeStr(Eval("VisitType").ToString())%>&nbsp;
                                </td>
                                 <td align="center">  
                                    <%# Eval("Remark")%>&nbsp;
                                </td>
                                <td align="center">  
                                    <%# Eval("CName")%>&nbsp;
                                </td>
                                <td align="center">
                                    <%# BitAuto.ISDC.CC2012.Entities.CommonFunction.GetDateTimeStrForPage(Eval("createtime").ToString(), "yyyy-MM-dd")%>&nbsp;
                                </td>   
                                <td align="center">
                                    <%# Eval("truename")%>&nbsp;
                                </td>
                            </tr>
                        </ItemTemplate>
                        </asp:repeater>
            </tbody>
        </table>
        <div class="clear">
        </div>
        <div class="pageTurn mr10">
            <uc:AjaxPager ID="AjaxPager_Custs" runat="server" />
        </div>
    </div>
</div>
