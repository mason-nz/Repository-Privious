<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="DealerInfoList.aspx.cs"
    Inherits="BitAuto.ISDC.CC2012.Web.WOrderV2.AjaxServers.DealerInfoList" %>

<table border="1" cellspacing="0" cellpadding="0" style="margin: 0px 12px;">
    <tr>
        <th style="width: 222px;">
            经销商全称
        </th>
        <th style="width: 142px;">
            经销商简称
        </th>
        <th style="width: 60px;">
            类别
        </th>
        <th style="width: 120px;">
            电话
        </th>
        <th style="width: 228px;">
            地址
        </th>
        <th style="width: 110px;">
            是否有报价
        </th>
        <th style="width: 90px;">
            会员详情
        </th>
        <th style="width: 90px;">
            操作
        </th>
    </tr>
    <asp:repeater id="DealerList" runat="server">
            <ItemTemplate>
                <tr>
                    <td class="l" style=" width:222px; text-align:left; word-wrap:break-word;word-break:break-all;">
                        <a href='<%# DealerPageURL +"/"+ Eval("MemberCode").ToString()%>' target="_blank"><%# Eval("name")%></a>&nbsp;
                    </td>
                    <td style="  width:142px;text-align:center;word-wrap:break-word;word-break:break-all;">
                        <%# Eval("Abbr")%>&nbsp;
                    </td>
                    <td class="l" style=" width:60px;text-align:center;word-wrap:break-word;word-break:break-all;">
                        <%# Eval("membertype").ToString() == "1" ? "4s" : Eval("membertype").ToString() == "2" ? "特许经销商" : "综合店"%>&nbsp;
                    </td> 
                    <td style="width:120px; text-align:left;word-wrap:break-word;word-break:break-all;">
                        <%# Eval("Phone")%>&nbsp;
                    </td>
                    <td style="width:228px; text-align:left;word-wrap:break-word;word-break:break-all;">
                        <%# Eval("ContactAddress")%>&nbsp;
                    </td>
                    <td style=" width:110px;word-wrap:break-word;word-break:break-all;">
                         &nbsp;
                    </td>
                    <td style=" width:90px;text-align:center;word-wrap:break-word;word-break:break-all;">
                        <a href='/CustCheck/CrmCustSearch/MemberDetail.aspx?MemberID=<%# Eval("ID") %>&CustID=<%# Eval("CustID") %>&i=0' target="_blank">会员详情</a>
                    </td> 
                    <td class="option_img" style="width:90px; text-align:center;word-wrap:break-word;word-break:break-all;">
                        <a href="javascript:void(0)"><img src="/Images/fdx.png" onclick="SendmailOption('<%# Eval("name")%>','<%# Eval("ContactAddress")%>','<%# Eval("Phone")%>');"  border="0" title="发短信"/></a> 
                        <a href="javascript:void(0)"><img src="/Images/copy.png" 
                        onclick='<%# GetCopyParaStr(Eval("name"),Eval("MemberCode"),Eval("membertype"),Eval("ContactAddress"))%>'  border="0" title="复制"/></a>
                    </td>   
                </tr>
            </ItemTemplate>
        </asp:repeater>
</table>
