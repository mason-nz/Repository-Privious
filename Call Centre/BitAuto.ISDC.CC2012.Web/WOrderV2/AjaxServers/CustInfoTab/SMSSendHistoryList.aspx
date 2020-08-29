<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="SMSSendHistoryList.aspx.cs"
    Inherits="BitAuto.ISDC.CC2012.Web.WOrderV2.AjaxServers.CustInfoTab.SMSSendHistoryList" %>

<!--根据手机号码查询短信 强斐 2016-8-10-->
<script type="text/javascript">
    //分页操作
    function ShowDataByPost1014(pody) {
        LoadingAnimation("bit_table1");
        $('#bit_table4').load('/WOrderV2/AjaxServers/CustInfoTab/SMSSendHistoryList.aspx #bit_table4 > *', pody);
    }
</script>
<div id="bit_table4" class="search_list2">
    <table width="100%" border="0" cellspacing="0" cellpadding="0">
        <tr>
            <th style="width: 8%;">
                接收人
            </th>
            <th style="width: 10%;">
                手机
            </th>
            <th style="width: 10%;">
                任务ID
            </th>
            <th style="width: 10%;">
                CRM客户
            </th>
            <th style="width: 27%;">
                发送内容
            </th>
            <th style="width: 17%;">
                发送日期
            </th>
            <th style="width: 10%;">
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
    </asp:repeater>
    </table>
    <br />
    <!--分页-->
    <div class="pageTurn mr10" style="margin-right: 20px;">
        <p>
            <asp:literal runat="server" id="litPagerDown"></asp:literal>
        </p>
    </div>
</div>
