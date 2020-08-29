<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="IframeContainer.aspx.cs"
    Inherits="BitAuto.ISDC.CC2012.Web.WOrderV2.AjaxServers.IframeContainer" %>

<%--<script type="text/javascript">
    $(document).ready(function () {
        //设置高度
        $("#iframecontainer").css("height", "766px");
        var height = "<%=Height %>";
        if (height != "" && height > 0) {
            $("#iframecontainer").css("height", height + "px");
        }
    });
</script>--%>
<iframe src="<%=Url %>" style="border: 0px; height: <%=(Height!=""?Height:"766") %>px; width: 100%;" seamless="seamless"
    id="iframecontainer" frameborder="0" marginheight="0" marginwidth="0" scrolling='auto'>
</iframe>
