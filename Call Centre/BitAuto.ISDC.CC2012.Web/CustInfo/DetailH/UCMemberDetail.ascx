<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="UCMemberDetail.ascx.cs"
    Inherits="BitAuto.ISDC.CC2012.Web.CustInfo.DetailH.UCMemberDetail" %>
<script type="text/javascript">
    document.domain = "<%=SysUrl %>";
</script>
<script type="text/javascript" language="javascript">
    function loadTabList(custId, n, menu) {
        switch (n) {
            case 1:
                $("#con_two_1").load("/CustInfo/MoreInfo/ContactInfoList.aspx", { CustID: custId, ContentElementId: 'con_two_1' }, function () { mao(menu); });
                break;
            case 2:
                $("#con_two_2").load("/CustInfo/MoreInfo/CooperationProjectList.aspx", { CustID: custId, ContentElementId: 'con_two_2' }, function () { mao(menu); });
                break;
            case 3:
                $("#con_two_3").load("/CustInfo/MoreInfo/CustUserList.aspx", { CustID: custId, ContentElementId: 'con_two_3' }, function () { mao(menu); });
                break;
            case 4:
                $("#con_two_4").load("/CustInfo/MoreInfo/ReturnVisitList.aspx", { CustID: custId, ContentElementId: 'con_two_4' }, function () { mao(menu); });
                break;
            case 5:
                $("#con_two_5").load("/CustInfo/MoreInfo/BusinessLicenseList.aspx", { CustID: custId, ContentElementId: 'con_two_5' }, function () { mao(menu); });
                break;
            case 6:
                $("#con_two_6").load("/CustInfo/MoreInfo/SecondCarList.aspx", { CustID: custId, ContentElementId: 'con_two_6' }, function () { mao(menu); });
                break;
            case 7:
                $("#con_two_7").load("/CustInfo/MoreInfo/TaskRecord.aspx", { CustID: custId, ContentElementId: 'con_two_7' }, function () { mao(menu); });
                break;
            case 8:
                $("#con_two_8").load("/CustInfo/MoreInfo/CustBrandLicenseList.aspx", { CustID: custId, ContentElementId: 'con_two_8' }, function () { mao(menu); });
            case 9:
                $("#con_two_9").load("/WorkOrder/WorkOrderRecordTag.aspx", { CustID: custId, ContentElementId: 'con_two_9' }, function () { mao(menu); });
                break;
            case 10:
                $("#con_two_10").load("/CustInfo/MoreInfo/CallRecordList.aspx", { CustID: custId, ContentElementId: 'con_two_10' }, function () { mao(menu); });
                break;
            case 11:
                $("#con_two_11").load("/CustInfo/MoreInfo/SMSSendHistoryList.aspx", { CustID: custId, ContentElementId: 'con_two_11' }, function () { mao(menu); });
                break;
            case 12:
                $("#con_two_12").load("/CustInfo/MoreInfo/StopCustApplyList.aspx", { CustID: custId, ContentElementId: 'con_two_12' }, function () { mao(menu); });
                break;
        }
    }

    function mao(menu) {
        var m = "";
        if (menu != "" && menu != undefined) {
            m = "#m";
            window.location.hash = m;
        }
    }
    var viewJiCaiProject = {
        name: 'ViewJiCaiProject',
        url: '/AjaxServers/CustCheck/ViewJiCaiProject.aspx?MemberID=<%=MemberID %>',
        open: function () {
            $.openPopupLayer({
                name: this.name,
                url: this.url
            });
        },
        close: function (e, data) {
            $.closePopupLayer(this.name, e, data);
        }
    };
    var ucMemberDetailHelper = (function () {
        //展示地图
        showMap = function () {
            var lat = '<%= this.Lat %>';
            var lng = '<%= this.Lng %>';
            var bPoint = new BMap.Point(lng, lat);
            //地图初始化
            var bm = new BMap.Map("divMap");
            bm.centerAndZoom(bPoint, 13);
            //bm.addControl(new BMap.NavigationControl({anchor: BMAP_ANCHOR_TOP_LEFT, type: BMAP_NAVIGATION_CONTROL_SMALL}));
            bm.enableScrollWheelZoom();
            marker = new BMap.Marker(bPoint);
            bm.addOverlay(marker);
            bm.setCenter(bPoint);
        },

        showBigMap = function (lat, lng) {

            if (!lat || !lng) { return; }

            $.openPopupLayer({
                name: 'ViewInMapPopup',
                url: '/CustInfo/MoreInfo/MarkInMap.aspx',
                parameters: {
                    PopupName: 'ViewInMapPopup',
                    marker_lat: lat,
                    marker_lng: lng,
                    DynamicView: true
                }
            });
        };

        return {
            showMap: showMap,
            showBigMap: showBigMap
        };
    })();
</script>
<script type="text/javascript">
    function showMoreMember(showtype) {

        if ($('#moreinfo')) {
            if (showtype) {
                $('#moreinfo').show();
            }
            else {
                $('#moreinfo').hide();
            }
        }

    }
    function deleteMember() {
        $.jConfirm("您确认要删除会员吗？", function (data) {
            if (data) {
                $.post("/CustInfo/Handler.ashx", { Action: 'deletedmsmember', MemberID: '<%=MemberID %>' }, function (jd, textStatus, xhr) {
                    var jdData = $.evalJSON(jd);
                    if (jdData.success) {
                        $.jPopMsgLayer("删除成功", function () {
                            window.location = $("#hrefCustInfoHref").attr("href");
                        });
                    }
                    else {
                        $.jAlert(jdData.message);
                    }
                });
            }
        });
    }
</script>
<script type="text/javascript">
    $(document).ready(function () {
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

        $('.s_ul').hide(); //初始ul隐藏（建议直接写style）

        $('.select_box span').hover(function () {
            $(this).parent().find('ul.s_ul').show();
            //li的hover效果
            $(this).parent().hover(function () {
            }, function () {
                $(this).parent().find("ul.s_ul").hide();
            });
        }, function () {
        });

        $('ul.s_ul li').click(function () {
            $(this).parents('li').find('span').html($(this).html());
            $(this).parents('li').find('ul').hide();
        });
        loadTabList('<%= this.CustID %>', 1);
        $('#linkShowBigMap').show();
    });
</script>
<script type="text/javascript">
    /*第一种形式 第二种形式 更换显示样式*/
    function setTab(cursel) {
        $(".menuConbox2 [id^='con_two']").css("display", "none");
        $(".menuConbox2 .cont_cxjg").css("display", "none");
        $(".hd2 ul li[id^='two']").attr("class", "");
        $("#two" + cursel).attr("class", "hover");
        $("#con_two_" + cursel).css("display", "block").parent().css("display", "block");
        loadTabList('<%= this.CustID %>', cursel, 1);
    }

    $(function () {
        ucMemberDetailHelper.showMap();

        if ("<%=tabI %>" != "") {
            $(".hd ul li[id^='one']").attr("class", "");
            $("#one<%=tabI %>").attr("class", "hover");
        }
        
    });
 
</script>
<style type="text/css">
    .baseInfo ul li
    {
        width: 405px;
    }
</style>
<div class="hd">
    <ul>
        <li id="one100" class=""><a id="hrefCustInfoHref" href="<%= this.CustInfoHref %>?CustID=<%= this.CustID %>"
            style="text-decoration: none; color: #555;">基本信息</a></li>
        <asp:Literal ID="lbCSTMember" runat="server"></asp:Literal>
    </ul>
</div>
<asp:Literal ID="literalMemberNameList" runat="server"></asp:Literal>
<div class="menuConbox">
    <div class="title" style="margin: 0">
        易湃会员信息<a href="javascript:void(0)" onclick="divShowHideEvent('con_one_101',this)"
            class="toggle"></a>
        <%if (IsCanDelete)
          { %>
        <a id="A1" href="javascript:void(0)" onclick="deleteMember()" style="float: right;
            margin-right: 20px;">删除</a>
        <%} %>
        <%if (IsCanEdit)
          { %>
        <a id="hrefEditCyt" href="http://<%=CrmUrl %>/CustomInfo/MemberMain.aspx?CustID=<%=CustID %>&SourceUrl=cc&Name=CytEdit&MemberID=<%=MemberID%>"
            style="float: right; margin-right: 20px;" target="_blank">编辑</a>
        <%} %>
    </div>
    <!--内容1-->
    <div id="con_one_100" class="hover">
    </div>
    <div id="con_one_101" class="hover">
        <ul class="clearfix">
            <li>
                <label>
                    会员全称：</label><span id="spanMemberName" runat="server"></span><a id="lnkViewJiCaiProject"
                        runat="server" href="javascript:viewJiCaiProject.open()" style="margin-left: 10px;
                        display: none;">查看集采</a> </li>
            <li>
                <label>
                    会员简称：</label><span id="spanAbbrName" runat="server"></span> </li>
            <li>
                <label>
                    会员ID：</label><span id="spanMemberCode" runat="server"></span> <a style="cursor: pointer;"
                        name="seeMemberInfo"  target="_blank" href="/AjaxServers/RYP.aspx?tid=<%=spanMemberCode.InnerText.Trim() %>">
                        查看经销商</a> </li>
            <li>
                <label>
                    服务信息：</label><span id="serviceInfo"></span> </li>
            <li id="li400" runat="server" style="display: none;">
                <label>
                    400号码：</label>
                <span id="span400" runat="server"></span></li>
            <li>
                <label>
                    会员类型：</label><span id="spanMemberType" runat="server"></span> </li>
            <li>
                <label>
                    传真：</label><span id="spanFax" runat="server"></span> </li>
            <li>
                <label>
                    联系电话：</label><span id="spanPhone" runat="server"></span> </li>
            <li>
                <label>
                    公司网址：</label><span id="spanCompanyWebSite" runat="server"></span> </li>
            <li>
                <label>
                    Email：</label><span id="spanEmail" runat="server"></span> </li>
            <li>
                <label>
                    邮编：</label><span id="spanPostcode" runat="server"></span> </li>
            <li>
                <label>
                    客户地区：</label><span id="spanArea" runat="server"></span> </li>
            <li style="width: 700px;">
                <label>
                    销售地址：</label><span id="spanAddress" runat="server" class="exceed" style="width: 560px;"></span>
            </li>
            <li>
                <label>
                    地图：</label>
                <input type="button" id="linkShowBigMap" value="查看地图" onclick="javascript:ucMemberDetailHelper.showBigMap(<%= this.Lat %>,<%= this.Lng %>);"
                    style="display: none;" class="button" />
                <div id="divMap" style="margin-left: 128px; margin-top: 5px; width: 186px; height: 125px;
                    overflow: hidden; position: relative; z-index: 0; background-color: rgb(243, 241, 236);
                    color: rgb(0, 0, 0); text-align: left;">
                </div>
            </li>
            <li class="singleRow">
                <label>
                    主营品牌：</label><span id="spanBrand" class="exceed" style="width: 270px;" runat="server"></span></li>
            <li class="singleRow" style="width: 700px;">
                <label>
                    关联品牌：</label><span id="spanSerialOfBrand" class="exceed" style="width: 560px;" runat="server"></span></li>
            <li class="singleRow" style="width: 700px;">
                <label>
                    附加子品牌：</label><span id="spanSerial" runat="server" class="exceed" style="width: 560px;"></span></li>
            <li class="singleRow" style="width: 700px;">
                <label>
                    交通信息：</label><span id="spanTrafficInfo" runat="server" class="exceed" style="width: 560px;"></span></li>
            <li class="singleRow" style="width: 700px;">
                <label>
                    企业介绍：</label><span id="spanEnterpriseBrief" runat="server" class="exceed" style="width: 560px;"></span></li>
            <li class="singleRow" style="width: 700px;">
                <label>
                    备注：</label><span id="spanRemarks" runat="server" class="exceed" style="width: 560px;"></span></li>
            <li class="singleRow">
                <label>
                    状态：</label><span id="spanSyncStatus" runat="server"></span></li>
        </ul>
    </div>
</div>
<a name="m"></a>
<div class="hd2">
    <ul>
        <%= this.ListOfContact == true ? "<li id='two1' onclick=\"setTab(1)\" class='hover'>客户联系人</li>" : ""%>
        <%= this.ListOfCooperationProjects == true ? "<li id='two2' onclick=\"setTab(2)\" class=''>合作项</li>" : ""%>
        <%= this.ListOfCustUser == true ? "<li id='two3' onclick=\"setTab(3)\" class=''>负责员工</li>" : ""%>
        <%= this.ListOfReturnVisit == true ? "<li id='two4' onclick=\"setTab(4)\" class=''>访问记录</li>" : ""%>
        <%= this.ListOfBusinessLicense == true ? "<li id='two5' onclick=\"setTab(5)\" class=''>年检记录</li>" : ""%>
        <%= this.ListOfBusinessBrandLicense == true ? "<li id='two8' onclick='setTab(8)' class=''>品牌授权书</li>":""%>
        <%= this.ListOfBusinessScaleInfo == true ? "<li id='two6' onclick=\"setTab(6)\" class=''>二手车规模</li>" : ""%>
        <%= this.ListOfTaskRecord == true ? "<li id='two7' onclick=\"setTab(7)\" class=''>任务记录</li>" : ""%>
        <%= this.ListOfWorkOrder == true ? "<li id='two9' onclick=\"setTab(9)\" class=''>工单记录</li>" : ""%>
        <li id='two10' onclick="setTab(10)" class=''>话务记录</li>
        <li id='two11' onclick="setTab(11)" class=''>短信记录</li>
        <li id='two12' onclick="setTab(12)" class=''>申请记录</li>
    </ul>
    <%--<asp:Literal ID="literalMemberNameList" runat="server"></asp:Literal> --%>
</div>
<div class="menuConbox menuConbox2">
    <!--内容1-->
    <%= this.ListOfContact == true ? "<div class='cont_cxjg'><div id='con_two_1' class='hover'></div></div>" : ""%>
    <!--内容1-->
    <!--内容2-->
    <%= this.ListOfCooperationProjects == true ? "<div class='cont_cxjg'><div id='con_two_2'  style='display: none;'></div></div>" : ""%>
    <!--内容2-->
    <!--内容3-->
    <%= this.ListOfCustUser == true ? "<div class='cont_cxjg'><div id='con_two_3'  style='display: none;'></div></div>" : ""%>
    <!--内容3-->
    <!--内容4-->
    <%= this.ListOfReturnVisit == true ? "<div class='cont_cxjg'><div id='con_two_4'  style='display: none;'></div></div>" : ""%>
    <!--内容4-->
    <!--内容5-->
    <%= this.ListOfBusinessLicense == true ? "<div class='cont_cxjg'><div id='con_two_5'  style='display: none;'></div></div>" : ""%>
    <!--内容5-->
    <!--内容5-->
    <%= this.ListOfBusinessBrandLicense == true ? "<div class='cont_cxjg'> <div id='con_two_8' style='display: none;'> </div> </div>": ""%>
    <!--内容5-->
    <!--内容6-->
    <%= this.ListOfBusinessScaleInfo == true ? "<div class='cont_cxjg'><div id='con_two_6'  style='display: none;'></div></div>" : ""%>
    <!--内容6-->
    <!--内容7-->
    <%= this.ListOfTaskRecord == true ? "<div class='cont_cxjg'><div id='con_two_7'  style='display: none;'></div></div>" : ""%>
    <!--内容7-->
    <%= this.ListOfWorkOrder == true ? "<div class='cont_cxjg'><div id='con_two_9'  style='display: none;'></div></div>" : ""%>
    <div class='cont_cxjg'>
        <div id='con_two_10' style='display: none;'>
        </div>
    </div>
    <div class='cont_cxjg'>
        <div id='con_two_11' style='display: none;'>
        </div>
    </div>
    <div class='cont_cxjg'>
        <div id='con_two_12' style='display: none;'>
        </div>
    </div>
</div>
