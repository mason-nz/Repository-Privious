<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="SMSSendHistoryList.aspx.cs"
    Inherits="BitAuto.ISDC.CC2012.Web.CustInfo.MoreInfo.SMSSendHistoryList" %>

<!--客户回访-短信记录 强斐 2016-8-17-->
<!--根据客户（crm or cc）查询短信 强斐 2016-8-10-->
<%@ Register Src="../../Controls/AjaxPager.ascx" TagName="AjaxPager" TagPrefix="uc1" %>
<div class="bit_table4">
    <table width="100%" border="0" cellspacing="0" cellpadding="0" class="cxjg">
        <tr>
            <th style="width: 10%;">
                接收人
            </th>
            <th style="width: 10%;">
                手机
            </th>
            <th style="width: 10%;">
                任务ID
            </th>
            <th style="width: 5%;">
                客户ID
            </th>
            <th style="width: 31%;">
                发送内容
            </th>
            <th style="width: 15%;">
                发送日期
            </th>
            <th style="width: 8%;">
                是否成功
            </th>
            <th style="width: 8%;">
                客服
            </th>
        </tr>
        <asp:repeater id="repeaterTableList" runat="server">
        <ItemTemplate>
            <tr class="back" onmouseover="this.className='hover'" onmouseout="this.className='back'">   
                 <td >
                    <a href="/TaskManager/CustInformation.aspx?CustID=<%# Eval("CustID")%>" target="_blank"><%# Eval("custname")%></a>&nbsp;
                </td>         
                 <td >
                    <%# Eval("Phone")%>&nbsp;
                </td>  
                <td >
                    <%# GetTaskUrl(Eval("TaskID").ToString())%>   
                </td>  
                <td >   
                    <%# GetCrmUrl(Eval("CRMCustID"))%>   
                </td>  
                <td title="<%#Eval("Content")%>">
                    <%# Eval("Content").ToString().Length > 20 ? Eval("Content").ToString().Substring(0,20)+"...": Eval("Content").ToString()%>&nbsp;
                </td>   
                <td>
                    <%# BitAuto.ISDC.CC2012.Entities.CommonFunction.GetDateTimeStrForPage(Eval("CreateTime").ToString())%>&nbsp;
                </td>              
                <td >
                    <%# Eval("Status").ToString()=="0"?"成功":"失败"%>&nbsp;
                </td> 
                 <td>
                    <%#Eval("TrueName")%>&nbsp;
                </td> 
            </tr>
        </ItemTemplate>
        <AlternatingItemTemplate>
            <tr class="color_hui">
                <td >
                    <a href="/TaskManager/CustInformation.aspx?CustID=<%# Eval("CustID")%>" target="_blank"><%# Eval("custname")%></a>&nbsp;
                </td>         
                 <td >
                    <%# Eval("Phone")%>&nbsp;
                </td>  
                <td >
                    <%# GetTaskUrl(Eval("TaskID").ToString())%>   
                </td>  
                <td >   
                    <%# GetCrmUrl(Eval("CRMCustID"))%>   
                </td>  
                <td title="<%#Eval("Content")%>">
                    <%# Eval("Content").ToString().Length > 20 ? Eval("Content").ToString().Substring(0,20)+"...": Eval("Content").ToString()%>&nbsp;
                </td>   
                <td>
                    <%# BitAuto.ISDC.CC2012.Entities.CommonFunction.GetDateTimeStrForPage(Eval("CreateTime").ToString())%>&nbsp;
                </td>              
                <td >
                    <%# Eval("Status").ToString()=="0"?"成功":"失败"%>&nbsp;
                </td> 
                 <td>
                    <%#Eval("TrueName")%>&nbsp;
                </td> 
            </tr>
        </AlternatingItemTemplate>
    </asp:repeater>
    </table>
    <!--分页-->
    <div class="pages" style="float: right; margin-right: 20px;">
        <uc1:AjaxPager ID="AjaxPager_SMSRecord" runat="server" PageSize="5" />
    </div>
</div>
