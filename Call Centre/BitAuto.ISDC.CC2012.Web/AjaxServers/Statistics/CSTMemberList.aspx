<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="CSTMemberList.aspx.cs"
    Inherits="BitAuto.ISDC.CC2012.Web.AjaxServers.Statistics.CSTMemberList" %>

<head>
    <style type="text/css">
        .style1
        {
            width: 194px;
        }
    </style>
</head>
<script>
     
    //SetTableStyle('tableCSTMember');

    //...add by qizhiqiang 2012-4-13 判断是统计中的会员查询还是客户核实中的会员查询
    <%if(RequestComeExcel=="1"){%>
      $('#comeExcel').show();
    <%}else{%>
      $('#comeExcel').hide();
    <%}%>

    //弹出导入客户信息DIV层
    function openUploadExcelInfoAjaxPopup() {
        $.openPopupLayer({
            name: "UploadUserAjaxPopup",
            parameters: {},
            url: "../../Statistics/MemberIDImport/Main.aspx?Type=CstMember"
        });
    }

    //弹出导入客户信息DIV层，根据会员ID导出客户ID
    function openUploadExcelInfoAjaxPopupForExportCustID() {
        $.openPopupLayer({
            name: "UploadUserAjaxPopup",
            parameters: {},
            url: "../../Statistics/MemberIDImport/Main.aspx?Type=CustIDCST"
        });
    }
</script>
<form id="form1" runat="server">
<div class="optionBtn  clearfix" style="width:97%">
    <div>
        <span>查询结果</span><small><span>总计:<%= this.CountOfRecords.ToString() %></span></small>
        <a style="margin-left: 680px;" href="javascript:openUploadExcelInfoAjaxPopup()" id="comeExcel"
            runat="server">导入会员ID号Excel</a>
        <a style="margin-left: 10px;" href="javascript:openUploadExcelInfoAjaxPopupForExportCustID()" id="exportCustID"
        runat="server">导出客户ID</a>
    </div>
</div>
<div class="bit_table">
    <table width="99%" border="0" cellspacing="0" cellpadding="0" class="tableList"
        id="tableCSTMember">
        <tr>
            <th width="10%">
                会员ID
            </th>
            <th width="17%">
                会员名称
            </th>
            <th width="15%">
                会员简称
            </th>
            <th width="15%">
                所属客户
            </th>
            <th width="15%">
                地区
            </th>
            <%--<th width="15%">
                所属交易市场
            </th>
            <th class="style1" width="13%">
                总消费车商币
            </th>--%>
            <%--        <th width="10%">车商币余额</th>
        <th width="10%">车商币有效期</th>--%>
        </tr>
        <asp:repeater id="Repeater_DMSMember" runat="server">
        <ItemTemplate>
                    <tr class="back" onmouseover="this.className='hover'" onmouseout="this.className='back'">
                <td align="center" MemberCode='<%#Eval("CSTMemberID") %>'>
                    <a href='/CSTMember/CSTMemberInfo.aspx?CSTRecID=<%#Eval("CSTRecID")%>&CustID=<%#Eval("CustID")%>' target="_blank"><%#Eval("CSTMemberID").ToString() == "-2" ? "" : Eval("CSTMemberID")%></a>&nbsp;
                </td>                 
                <td align="center">
                   <%#Eval("CSTMemberID").ToString() != "-2"?Eval("FullName"):"  <a href='/CSTMember/CSTMemberInfo.aspx?CSTRecID="+Eval("CSTRecID")+"&CustID="+Eval("CustID")+"' target='_blank'>"+Eval("FullName")+"</a>&nbsp;" %> &nbsp;
                </td>                
                <td align="center">
                    <%#Eval("ShortName")%>&nbsp;
                </td>
                <td align="center" custid='<%#Eval("CustID")%>'>
                    <a href='/CustCheck/CrmCustSearch/CustDetail.aspx?CustID=<%#Eval("CustID")%>' target="_blank"><%#Eval("CustName")%></a>&nbsp;
                </td>
                 
                <td align="center">
                    <%#Eval("areafullname") %>&nbsp;
                </td>
                <%--<td align="center">
                    <%#Eval("tradearea")%>&nbsp;
                </td>
                <td align="center">
                    <%#Eval("UserdAmount").ToString()=="-2"? "":Eval("UserdAmount").ToString()%>&nbsp;
                </td>--%>
<%--                 <td align="center">
                    <%#Eval("RemainAmount")%>
                </td>
                 <td align="center">
                   <%# Eval("activeDate").ToString() == string.Empty ? string.Empty : Convert.ToDateTime(Eval("activeDate").ToString()).ToShortDateString()%>
                </td>--%>
            </tr>
        </ItemTemplate>
    </asp:repeater>
    </table>
    <div class="pages1" style="text-align: right;">
        <uc:AjaxPager ID="AjaxPager_DMSMember" runat="server" />
    </div>
</div>
</form>
