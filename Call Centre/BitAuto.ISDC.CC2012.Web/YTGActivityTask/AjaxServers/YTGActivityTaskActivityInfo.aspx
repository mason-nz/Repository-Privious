<%@ Page Language="C#" Title="易团购活动信息页" AutoEventWireup="true" CodeBehind="YTGActivityTaskActivityInfo.aspx.cs" Inherits="BitAuto.ISDC.CC2012.Web.YTGActivityTask.AjaxServers.YTGActivityTaskActivityInfo" %>
 
<div class="pop pb15 openwindow" style="width: 600px;">
    <div class="title bold">
        <h2>活动信息</h2>
        <span><a href="javascript:void(0)" onclick="javascript:$.closePopupLayer('ViewActivityInfoAjaxPopup');"></a></span>
    </div>
    <ul class="clearfix  outTable" style="padding-top: 10px;">
        <li style="width: 590px; float: left;">
            <b>活动主题：</b>
            <span><%=model.ActivityName%></span>
        </li>
        <li style="width: 590px; float: left;">
            <b>报名时间：</b>
            <span><%=model.SignBeginTime.HasValue ? model.SignBeginTime.Value.ToString("yyyy-MM-dd"):""%>到<%=model.SignEndTime.HasValue ? model.SignEndTime.Value.ToString("yyyy-MM-dd"):""%></span>
        </li>
        <li style="width: 590px; float: left;">
            <b>活动时间：</b>
             <span><%=model.ActiveBeginTime.HasValue ? model.ActiveBeginTime.Value.ToString("yyyy-MM-dd") : ""%>到<%=model.ActiveEndTime.HasValue ? model.ActiveEndTime.Value.ToString("yyyy-MM-dd") : ""%></span>
        </li>
        <li style="width: 590px; float: left;">
            <b>活动地点：</b>
            <span><%=model.Address %></span>
        </li>
        <li style="width: 590px; float: left;">
            <b>活动车型：</b>
            <span><%=GetYiXiangCheXing()%></span>
        </li>

         <li style="width: 590px; float: left;">
            <b>活动描述：</b>
            <span><%=model.Content %></span>
        </li>
         <li style="width: 590px; float: left;">
            <b>活动专题页网址：</b>
            <span><a href="<%=model.Url %>" target="_blank"><%=model.Url %></a></span>
        </li>
         <li style="width: 590px; float: left;">
            <b>活动状态：</b>
            <span><%=StatusName%></span>
        </li>
    </ul>
</div>
