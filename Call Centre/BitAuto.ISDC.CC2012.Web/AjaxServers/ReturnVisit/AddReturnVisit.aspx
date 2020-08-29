<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="AddReturnVisit.aspx.cs" Inherits="BitAuto.ISDC.CC2012.Web.AjaxServers.ReturnVisit.AddReturnVisit" %>

<script type="text/javascript">
    $(function () {
        $("#divBrandList").load("http://www.baidu.com");
        // $("#divBrandList").load("http://wp2013.sys1.bitauto.com/ReturnVisit/AddReturnVisit.aspx?pagetype=6&action=add&CustID=");
    });

</script>
<div id="jcDiv" class="pop pb15 openwindow" style="width: 860px">
    <div class="title bold">
        <h2>
            <em id="spanTitle" runat="server" style="color: #FFFFFF;">添加访问记录</em></h2>
        <span><a href="javascript:void(0)" onclick="javascript:$.closePopupLayer('ReturnVisitAjaxPopup');">
        </a></span>
    </div>
    
    <div class="Table2" id="divBrandList">
        
        
    </div>
</div>
