<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="PopTitle.aspx.cs" Inherits="BitAuto.DSC.APPReport2016.WebAPI.Html.PopTitle" %>
<script type="text/javascript">
    $('div.PopTitle').bind('click', function () {
        $.closePopupLayer('popTitle');
    });
</script>
    
<div class="PopTitle">
    <!--<div style=" text-align:right; ">关闭</div>-->
    <div class='textContent'>
        <%=Content %>
        <%--测试
        <ul>
            <li>总覆盖用户数：易车网PC+易车网PC+易车网PC+易车网PC+易车网PC+易车网PC+易车网PC+</li>
            <li>总覆盖用户数：易车网PC+易车网PC+易车网PC+易车网PC+易车网PC+易车网PC+易车网PC+</li>
            <li>总覆盖用户数：易车网PC+易车网PC+易车网PC+易车网PC+易车网PC+易车网PC+易车网PC+</li>
            <li>总覆盖用户数：易车网PC+易车网PC+易车网PC+易车网PC+易车网PC+易车网PC+易车网PC+</li>
        </ul>--%>
    </div>
</div>