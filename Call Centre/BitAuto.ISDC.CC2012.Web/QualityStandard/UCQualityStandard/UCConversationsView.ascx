<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="UCConversationsView.ascx.cs"
    Inherits="BitAuto.ISDC.CC2012.Web.QualityStandard.UCQualityStandard.UCConversationsView" %>
<div class="lybase">
    <div class="title">
        对话基本信息 <a class="toggle2" onclick="divShowHideEvent('baseInfo',this)" href="javascript:void(0)"
            style="*margin-top: -25px;"></a>
    </div>
    <table border="1" cellspacing="0" cellpadding="0" width="100%" style="margin-top: 10px;">
        <tr>
            <td width="40%" class="bdlnone">
                客服：<span id="spUserName" runat="server"></span>
            </td>
            <td width="40%">
                对话开始时间：<span id="spBeginTime" runat="server"></span>
            </td>
            <td width="20%" rowspan="3" class="score" id="tdScore" runat="server" visible="false">
            </td>
        </tr>
        <tr>
            <td class="bdlnone">
                对话ID：<span id="spCsID" runat="server"></span>
            </td>
            <%--<td width="40%">
                工单ID：<span id="spOrderID" runat="server"></span>
            </td>--%>
            <td width="40%">
                对话时长：<span id="spCTime" runat="server"></span>
            </td>
        </tr>
 
        <tr>
            <td class="bdlnone">
                满意度：<span id="spSatisfaction" runat="server"></span>
            </td>
            <td width="40%">
                <%-- 消息次数：<span id="spCount" runat="server"></span>--%>
            </td>
        </tr>
    </table>
</div>
