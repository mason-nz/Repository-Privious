<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ViewCustBaseInfo.ascx.cs"
    Inherits="BitAuto.ISDC.CC2012.Web.TaskManager.NoDealerOrder.UCNoDealerOrder.ViewCustBaseInfo" %>
<ul class="clearfix">
    <li>
        <label>
            姓名：</label><span><%=CustName%></span></li>
    <li>
        <label>
            地区：</label><span><%=PlaceStr%></span></li>
    <li>
        <label>
            性别：</label><span><%=Sex%></span></li>
    <li>
        <label>
            分属大区：</label><span><%=AreaStr%></span></li>
    <li>
        <label>
            电话：</label><span><%=Tels%></span></li>
    <li>
        <label>
            地址：</label><span class="exceed"><%=Address%></span></li>
    <li>
        <label>
            邮箱：</label><span><%=Email %></span></li>
    <li>
        <label>
            数据来源：</label><span>易湃</span></li>
    <li>
        <label>
            客户分类：</label><span><%=CustCategoryStr%></span></li>
</ul>
<div class="line">
</div>
