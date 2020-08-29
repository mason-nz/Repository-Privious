<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="WOrderDealView.ascx.cs"
    Inherits="BitAuto.ISDC.CC2012.Web.WOrderV2.UserControl.WOrderDealControl.WOrderDealView" %>
<%@ Register Src="WOrderProcessList.ascx" TagName="WOrderProcessList" TagPrefix="uc1" %>
<div class="content" style="padding-top: 0px; padding-bottom: 0px;">
    <div class="titles bd ft14">
        处理记录</div>
    <table id="<%=TableHtmlId %>" border="0" cellspacing="0" cellpadding="0" class="xm_View_bs xm_View_hf">
        <uc1:WOrderProcessList ID="WOrderProcessList1" runat="server" />
    </table>
</div>
