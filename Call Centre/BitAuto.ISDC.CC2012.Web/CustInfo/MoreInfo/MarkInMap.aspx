<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="MarkInMap.aspx.cs" Inherits="BitAuto.ISDC.CC2012.Web.CustInfo.MoreInfo.MarkInMap" %>

<script type="text/javascript">
    var bm;
    var marker;
    $(function () {
        var tmpPoint = new BMap.Point(116.404, 39.915); //默认为总部
        var lat, lng;
        lat = '<%= this.Marker_lat %>';
        lng = '<%= this.Marker_lng %>';
        //if (!(lat = '<%= this.Marker_lat %>') || !(lng = '<%= this.Marker_lng %>')) { return; }
        var bPoint;
        if (checkNum(lat) && checkNum(lng)) {
            bPoint = new BMap.Point(lng, lat);
        }
        else {
            bPoint = tmpPoint;
        }
        //地图初始化
        bm = new BMap.Map("map_canvas");
        bm.centerAndZoom(bPoint, 15);
        bm.addControl(new BMap.NavigationControl());
        bm.enableScrollWheelZoom();
        marker = new BMap.Marker(bPoint);
        <% if(this.DynamicView=="0"){ %>
        marker.enableDragging();
        bm.addEventListener('click', function (e) {
            marker.setPosition(e.point);
            bm.addOverlay(marker);
        });
        <%} %>
        <%else {%>
        marker.disableDragging();
        <%} %>
        bm.addOverlay(marker);
        bm.setCenter(bPoint);
    });
</script>
<script type="text/javascript">
    function search() {
        var address = document.getElementById("searchText").value;
        var local = new BMap.LocalSearch("中国", {
            renderOptions: {
                map: bm,
                panel: "r-result",
                autoViewport: true,
                selectFirstResult: false
            }
        });
        local.search(address);
    }
</script>
<script type="text/javascript">
    <% if (this.DynamicView == "0"){ %>
    
    function confirmMarkInMap() {
        var p = { lat: marker.getPosition().lat, lng: marker.getPosition().lng};
        closeMarkInMapPopup(true, p);
    }
    <%} %>

    function closeMarkInMapPopup(ea, cData) {
        $.closePopupLayer('<%= PopupName%>', ea, cData);
    }
</script>
<div class="pop pb15 openwindow" style="width: 650px">
    <div class="title bold">
        <h2>
            地图</h2>
        <span><a href="javascript:void(0)" onclick="javascript:closeMarkInMapPopup(false);">
        </a></span>
    </div>
    <div class="clearfix  outTable" style=" width:100%">
        <ul>
            <li style="width: 340px">
                <input id="searchText" type="text" value="中国" style="width: 340px; height: 20px;" />
            </li>
            <li class="btn">
                <input type="button" value="搜索" class="button" onclick="javascript:search();" />
            </li>
        </ul>
        <div style="clear: both;">
        </div>
        <div id="map_canvas" style="float: left; width: 95%; height: 320px;">
        </div>
    </div>
    <div class="btn" style="margin: 20px 10px 10px 0px;">
        <% if (this.DynamicView == "0")
           { %>
        <input type="button" class="btnSave bold" value="确定" onclick="javascript:confirmMarkInMap();" />
        <input type="button" class="btnSave bold" value="取消" onclick="javascript:closeMarkInMapPopup(false);" />
        <%} %>
    </div>
</div>
