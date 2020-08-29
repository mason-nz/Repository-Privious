<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="CustUserLogList.aspx.cs"
    Inherits="BitAuto.ISDC.CC2012.Web.CustInfo.MoreInfo.CustUserLogList" %>

<%@ Register Src="../../Controls/AjaxPager.ascx" TagName="AjaxPager" TagPrefix="uc1" %>
<script type="text/javascript" language="javascript">
    var cUMLogHelper = (function () {
        var goToCUMList = function () {
            var url = '/CustInfo/MoreInfo/CustUserList.aspx';
            var data = { CustID: '<%=CustID %>', ContentElementId: '<%=this.AjaxPager_CUMLog.ContentElementId %>' };
            $('#<%=this.AjaxPager_CUMLog.ContentElementId %>').load(url, data);
        };

        return { goToCUMList: goToCUMList };
    })();    
</script>
<form id="form1" runat="server">
<h2>
    <span>历史分配记录</span><a href="#this" onclick="javascript:cUMLogHelper.goToCUMList();">[员工列表]</a>
</h2>
<table width="100%" border="0" cellspacing="0" cellpadding="0" class="cxjg" id="tableCustUserLogList">
    <tr>
        <th width="8%">
            员工姓名
        </th>
        <th width="18%">
            部门
        </th>
        <th width="8%">
            负责会员
        </th>
        <th width="12%">
            业务线
        </th>
        <th width="12%">
            分机
        </th>
        <th width="10%">
            移动电话
        </th>
        <th width="10%">
            分配时间
        </th>
        <th width="10%">
            收回时间
        </th>
        <th width="9%">
            状态
        </th>
    </tr>
    <asp:repeater runat="server" id="repeater">
        <ItemTemplate>
            <tr>
                <td>
                    <%#Eval("TrueName").ToString() %>/(<%# Eval("Sex").ToString().Trim()=="1" ? "先生":"女士"%>)
                </td>
                <td class="l">
                    <%#Eval("DepartName").ToString() %>
                </td>
                <td>
                   <%#GetManageMember(Eval("UserID").ToString())%>
                </td>
                <td>
                    <%#GetBussinessName(Eval("UserID").ToString())%>
                </td>
                <td>
                    <%#Eval("Officetel").ToString() %>
                </td>
                <td>
                    <%#Eval("Mobile").ToString() %>
                </td>
                <td>
                    <%# BitAuto.ISDC.CC2012.Entities.CommonFunction.GetDateTimeStrForPage(Eval("BeginTime").ToString(),"yyyy-MM-dd")%>&nbsp;
                </td>
                <td>
                    <%# BitAuto.ISDC.CC2012.Entities.CommonFunction.GetDateTimeStrForPage(Eval("EndTime").ToString(),"yyyy-MM-dd")%>&nbsp;
                </td>
                <td>
                    <%#Eval("EndTime").ToString().Trim()=="" ? "未收回" : "已收回"%>
                </td>
            </tr>               
        </ItemTemplate>
        <AlternatingItemTemplate>
            <tr class="color_hui">
                <td>
                    <%#Eval("TrueName").ToString() %>/(<%# Eval("Sex").ToString().Trim()=="1" ? "先生":"女士"%>)
                </td>
                <td class="l">
                    <%#Eval("DepartName").ToString() %>
                </td>
                <td>
                    <%#GetManageMember(Eval("UserID").ToString())%>
                </td>
                 <td>
                    <%#GetBussinessName(Eval("UserID").ToString())%>
                </td>
                <td>
                    <%#Eval("Officetel").ToString() %>
                </td>
                <td>
                    <%#Eval("Mobile").ToString() %>
                </td>
                <td>
                    <%# BitAuto.ISDC.CC2012.Entities.CommonFunction.GetDateTimeStrForPage(Eval("CreateTime").ToString(), "yyyy-MM-dd")%>&nbsp;
                </td>
                <td>
                     <%# BitAuto.ISDC.CC2012.Entities.CommonFunction.GetDateTimeStrForPage(Eval("EndTime").ToString(), "yyyy-MM-dd")%>&nbsp;
                </td>
                <td>
                    <%#Eval("EndTime").ToString().Trim()=="" ? "未收回" : "已收回"%>
                </td>
            </tr>
        </AlternatingItemTemplate>
    </asp:repeater>
</table>
<div class="pages" style="float: right">
    <uc1:AjaxPager ID="AjaxPager_CUMLog" runat="server" PageSize="5" />
</div>
</form>
