<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="BusinessLicenseList.aspx.cs"
    Inherits="BitAuto.ISDC.CC2012.Web.CustInfo.MoreInfo.BusinessLicenseList" %>

<%@ Register Src="../../Controls/AjaxPager.ascx" TagName="AjaxPager" TagPrefix="uc1" %>
<form id="form1" runat="server">
<%--<h2>
    <span>年检列表</span>
</h2>--%>
<table width="100%" border="0" cellspacing="0" cellpadding="0" class="cxjg" id="tableALInfoList">
    <tr>
        <th width="25%">
            客户名称
        </th>
        <th width="18%">
            营业执照号
        </th>
        <th width="8%">
            注册资本
        </th>
        <th width="19%">
            营业期限
        </th>
        <th width="10%">
            年检日期
        </th>
        <th width="8%">
            附件
        </th>
        <%--<th width="7%">
            操作
        </th>--%>
    </tr>
    <asp:repeater runat="server" id="repeater">
        <ItemTemplate>
            <tr>   
                <td>
                    <%# Eval("CustName").ToString() %>
                </td>                
                <td>
                    <%# Eval("LicenseNumber").ToString()%>
                </td>
                 <td>
                    <%# Eval("RegisteredCapital").ToString()%>
                </td>  
                <td >
                    <%# DateTime.Parse(Eval("BeginTime").ToString()).ToString("yyyy-MM-dd")%> - <%#DateTime.Parse(Eval("EndTime").ToString()).ToString("yyyy-MM-dd")%>
                </td>
                <td >
                    <%# DateTime.Parse(Eval("AnnualSurveyTime").ToString()).ToString("yyyy")%>
                </td>
                <%--<td>
                    <%# string.IsNullOrEmpty(Eval("ID").ToString().Trim())
                        ? "<span style=\"color:Gray;\">查看</span>"
                                                                                                : "<a target=\"_blank\" href=\"/CustInfo/MoreInfo/BusinessLicensePicView.aspx?BLID=" + Eval("ID").ToString() + "\">查看</a>"
                                                                                                
                    %>
                </td>--%>

                <td>
                   <a target="_blank" href="/CustInfo/MoreInfo/BusinessLicensePicView.aspx?BLID=<%#Eval("ID").ToString()%>">查看</a>
                </td>
                <%--<td>
                    <a href="javascript:openEditBLPopup(<%# Eval("ID").ToString() %>);">修改</a>
                </td>--%>
            </tr>
        </ItemTemplate>
    </asp:repeater>
</table>
<%--<div class="xinzeng">
    <input type="button" onclick="javascript:openAddBLPopup();" value="新增" class="button" />
</div>--%>
<div class="pages" style="float: right">
    <uc1:AjaxPager ID="AjaxPager1" runat="server" PageSize="5" />
</div>
</form>
