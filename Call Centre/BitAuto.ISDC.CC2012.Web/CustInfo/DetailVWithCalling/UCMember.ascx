﻿<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="UCMember.ascx.cs" Inherits="BitAuto.ISDC.CC2012.Web.CustInfo.DetailVWithCalling.UCMember" %>
<script type="text/javascript">
    var uCEditMemberHelper_<%= this.ID %> = (function() {
       //展示地图
       var showMap = function (lat,lng) {           
             var bPoint = new BMap.Point(lng, lat);
            //地图初始化
            var bm = new BMap.Map('<%=divMap.ClientID%>');
            
            bm.centerAndZoom(bPoint, 13);
           
            //bm.addControl(new BMap.NavigationControl({anchor: BMAP_ANCHOR_TOP_LEFT, type: BMAP_NAVIGATION_CONTROL_SMALL}));
            bm.enableScrollWheelZoom();
            
            marker = new BMap.Marker(bPoint);
            bm.addOverlay(marker);
            bm.setCenter(bPoint);

            $('#<%=divMap.ClientID%>').data('marker_lat',lat);
            $('#<%=divMap.ClientID%>').data('marker_lng',lng);
        },
        
        haveResetPositionOfMap = false,//修正过没
        triggerMemberArea = function(contentId){
            if(memberAreaActionHelper && memberAreaActionHelper.triggerMemberArea){
                 memberAreaActionHelper.triggerMemberArea(contentId);
                 
                 //重新显示地图，防止位置偏移
                 if($('#divMemberInfo_<%= this.ID %>').is(':visible') && !haveResetPositionOfMap) {
                    showMap();
                    haveResetPositionOfMap = true;
                 }
            }
        },
              
        init = function(){
            //显示地图
                showMap('<%= this.Lat %>','<%= this.Lng %>'); 
                
        };

        return {
            init: init,
            triggerMemberArea: triggerMemberArea 
        };
    })();       
</script>
<script type="text/javascript">
    $(function() {
        uCEditMemberHelper_<%= this.ID %>.init();
        //绑定服务信息
        //强斐 2014-11-26
        //加载服务信息
        $.getJSON('<%=CRMMemberServerInfo %>?Action=getservice&MemberID=<%=MemberID %>&r=' + Math.random() + '&jsoncallback=?', function (data) {
            var strHtml = "";
            $.each(data, function (i, item) {
                if (item.ServiceName == "DSA服务") {
                    strHtml += "<a class='cyttitle'>" + item.ServiceName + "<img title='" + item.Title + "' src='../../NewImages/dsa.png' align='absmiddle'/></a>";
                }
                else {
                    strHtml += item.HrefUrl == "" ? "<a href=\"javascript:void(0);\">" : "<a href=" + item.HrefUrl + " target='_blank'>";
                    strHtml += item.ServiceName + "</a>;";
                }
            });
            strHtml = TrimChar(strHtml, ';');
            if (strHtml == "") { strHtml = "当前无合作"; }
            $("#serviceInfo").html(strHtml);
        });         
    });


</script>
<div class="cont_cx khxx ">
    <div class="title contact" style="clear: both;">
        <%= this.ControlName %><a href="javascript:void(0)" onclick="divShowHideEvent('divMemberInfo_<%= this.ID %>',this)"
            class="toggle"></a></div>
    <div id="divMemberInfo_<%= this.ID %>" class="MemberInfoArea">
        <ul class="clearfix">
            <li id="liMemberCode" style="display: none;" runat="server">
                <label>
                    会员ID：</label>
                <span id="spanMemberCode" name="MemberCode" runat="server" />&nbsp;&nbsp;<a id="aTaoChe1"
                    runat="server" visible="false" href="#" target="_blank">淘车通会员页</a>&nbsp;&nbsp;<a
                        id="aTaoChe2" runat="server" visible="false" href="#" target="_blank">淘车通使用信息</a>
                <a style="cursor: pointer;" name="seeMemberInfo" target="_blank" href="/AjaxServers/RYP.aspx?tid=<%=Member.MemberCode %>">
                    查看经销商</a> </li>
            <li>
                <label>
                    会员名称：</label>
                <span id="spanMemberName" name="MemberName" runat="server" /></li>
            <li>
                <label>
                    会员状态：</label><span id="spanSyncStatus" runat="server"></span></li>
            <li>
                <label>
                    会员简称：</label>
                <span id="spanMemberAbbr" name="MemberAbbr" runat="server" /></li>
            <li>
                <label>
                    会员类型：</label>
                <span id="spanMemberType" name="MemberType" runat="server"></span></li>
            <li>
                <label>
                    服务信息：</label><span id="serviceInfo"></span> </li>
            <li>
                <label>
                    联系电话：</label>
                <span id="spanPhone" name="Phone" runat="server" /><span><a <%=AddWorderv2Url_Phone %>
                    ctel="<%=cTel %>" membercode='<%=Member.MemberCode %>'>
                    <img alt="打电话" src="/images/phone.gif" border="0" /></a> </span></li>
            <li>
                <label>
                    主营品牌：</label>
                <span id="spanBrandName" name="BrandName" runat="server" /></li>
            <li>
                <label>
                    会员地区：</label>
                <span id="spanArea" name="Area" runat="server" /></li>
            <li style="width: 700px;">
                <label>
                    销售地址：</label>
                <span id="spanAddress" name="Address" class="exceed" style="width: 560px;" runat="server" />
            </li>
            <li style="height: auto; width: 800px;">
                <label>
                    地图：</label>
                <div id="divMap" runat="server" style="margin-left: 78px; margin-top: 5px; width: 252px;
                    height: 125px;">
                </div>
            </li>
            <li>
                <label>
                    传真：</label>
                <span id="spanFax" name="Fax" runat="server" /></li>
            <li>
                <label>
                    公司网址：</label>
                <span id="spanCompanyWebSite" name="CompanyWebSite" runat="server" /></li>
            <li>
                <label>
                    Email：</label>
                <span id="spanEmail" name="Email" runat="server" /></li>
            <li>
                <label>
                    邮政编码：</label>
                <span id="spanZipcode" name="Zipcode" runat="server" /></li>
            <li>
                <label>
                    附加子品牌：</label>
                <span id="spanSerial" name="Serial" runat="server" /></li>
            <li>
                <label>
                    交通信息：</label>
                <span id="spanTrafficInfo" name="TrafficInfo" runat="server" /></li>
            <li class="singleRow">
                <label>
                    企业简介：</label>
                <span id="spanEnterpriseBrief" name="EnterpriseBrief" runat="server" /></li>
            <li class="singleRow">
                <label>
                    备注：</label>
                <span id="spanNotes" name="Notes" runat="server" /></li>
        </ul>
    </div>
</div>