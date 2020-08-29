<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ContentsDetailForm.aspx.cs"
    Inherits="BitAuto.DSC.IM_DMS2014.Web.AjaxServers.ContentManage.ContentsDetailForm" %>

<script type="text/javascript">
    function closeAddRemarkLayerAjaxPopup() {
        $.closePopupLayer('ContentDetailLayerAjaxPopup');
    }
</script>
<div class="online_kf online_kf2" style="background-color: #E4E4E4; width: 400px;">
    <div class="title_kf">
        留言内容<span><a href="#" onclick="closeAddRemarkLayerAjaxPopup()"><img src="/Images/c_btn.png"
            border="0" alt="关闭" /></a></span></div>
    <div class="content_kf content_kf_ms" style="height: 200px; padding-bottom:0px;">

      <textarea name="" id="taContents" readonly style="width:97%; height:177px; border:1px solid #CCC; font-size:12px; padding:5px;" runat="server"></textarea>

    </div>
    <div class="clearfix">
    </div>
</div>
