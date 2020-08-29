<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="DealerInfoList_YP.aspx.cs"
    Inherits="BitAuto.ISDC.CC2012.Web.WOrderV2.AjaxServers.DealerInfoList_YP" %>

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
        <th style="width: 238px;">
            地址
        </th>
        <th style="width: 100px;">
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
                    <td class="l" style=" width:222px; text-align:left;word-wrap:break-word;word-break:break-all;">
                        <a href='<%# DealerPageURL + "/" + Eval("DealerId")%>' target="_blank">
                                <%# Eval("DealerFullName")%></a>
                    </td>
                    <td style=" width:142px;word-wrap:break-word;word-break:break-all;">
                        <%# Eval("vendorName")%>&nbsp;
                    </td>
                    <td class="l" style=" width:60px;word-wrap:break-word;word-break:break-all;">
                       <%# Eval("vendorBizMode").ToString()=="1"?"4S":"综合"%>&nbsp;
                    </td> 
                    <td  style=" width:120px;text-align:left;word-wrap:break-word;word-break:break-all;">
                       <div style="float: left; word-wrap: break-word; word-break: break-all; text-align: left;">
                                <%# Eval("vendorTel").ToString().Trim()%>&nbsp;</div>
                    </td> 
                    <td class="l" style=" width:238px;word-wrap:break-word;word-break:break-all;">
                        <%# Eval("vendorSaleAddr")%>&nbsp;
                    </td> 
                    <td class="l" style=" width:100px;word-wrap:break-word;word-break:break-all;">
                        <%# Eval("ishavePrice").ToString() == "1" ? "有" : "无"%>&nbsp;
                    </td> 
                    <td class="1" style=" width:90px;word-wrap:break-word;word-break:break-all;"><a href="/AjaxServers/TaskManager/NoDealerOrder/ReturnMemberDetail.aspx?DealerID=<%# Eval("DealerId") %>" target="_blank">会员详情</a></td>
                    <td class="1" style=" width:90px;word-wrap:break-word;word-break:break-all;"> 
                        <a href="javascript:void(0)"><img src="/Images/fdx.png" onclick="SendmailOption('<%# Eval("DealerFullName")%>','<%# Eval("vendorSaleAddr")%>','<%# Eval("vendorTel")%>');"  border="0" title="发短信"/></a> 
                        <a href="javascript:void(0)"><img src="/Images/copy.png" 
                          onclick='<%# GetCopyParaStr(Eval("DealerFullName"),Eval("DealerId"),Eval("vendorBizMode"),Eval("vendorSaleAddr"))%>' border="0" title="复制"/></a>
                    </td>
                </tr>
            </ItemTemplate>
        </asp:repeater>
</table>
