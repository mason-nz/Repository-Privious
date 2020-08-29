<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="WOrderBasicInfo.ascx.cs"
    Inherits="BitAuto.ISDC.CC2012.Web.WOrderV2.UserControl.WOrderBasicInfo" %>
<script type="text/javascript">
    $(function () {
        BindSlider($("#workOrderTableInfo img.order_file_img"), "data");
    }); </script>
<div class="content" style="padding-top: 0px; padding-bottom: 0px;">
    <div class="titles bd ft14">
        工单信息</div>
    <div class="lineS">
    </div>
    <table id="workOrderTableInfo" border="0" cellspacing="0" cellpadding="0" class="xm_View_bs">
        <%=StrHtml%>
    </table>
</div>
