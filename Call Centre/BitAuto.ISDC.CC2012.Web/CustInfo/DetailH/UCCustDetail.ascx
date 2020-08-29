<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="UCCustDetail.ascx.cs"
    Inherits="BitAuto.ISDC.CC2012.Web.CustInfo.DetailH.UCCustDetail" %>
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
            //            case 6:             
            //                $("#con_two_6").load("/CustInfo/MoreInfo/SecondCarList.aspx", { CustID: custId, ContentElementId: 'con_two_6' }, function () { mao(menu); });             
            //                break;             
            //            case 7:  
            //                $("#con_two_7").load("/CustInfo/MoreInfo/TaskRecord.aspx", { CustID: custId, ContentElementId: 'con_two_7' }, function () { mao(menu); });  
            //                break;  
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

</script>
<script type="text/javascript">
    $(function () {
        $(document).ready(function () {
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
        });
        if ('<%=TypeID %>' == '20009') {
            $("[id$='spanPid']").parent().find("label").text("所属4S：");
        }
        else {
            $("[id$='spanPid']").parent().find("label").text("所属集团：");
        }

        //该标签页可以指定显示哪个标签；add lxw 13.9.26
        var tbi = "<%=tabI %>";
        if (tbi != "" && tbi != undefined) {
            setTab(Number(tbi));
        }
        else {
            loadTabList('<%= this.CustID %>', 1);
        }

        switch ('<%=TypeID %>') {
            case '20003': //4s
            case '20004': //特许经销商
            case '20009': //展厅
            case '20010': //个人  
            case '20011': //经纪公司
            case '20013': //车易达
            case '20014': //二手车中心
            case '20005': //综合店
                $("#hrefAddCyt").show();
                if ($("#liSelect").val() != undefined) {
                    $("#hrefAddCyt").css("margin-right", "90px");
                }
                break;
        }
        if ($('#liSelect.select_box').size() == 0)//处理“新增会员”按钮，不对齐的情况
        { $('.baseInfo .hd ul ~ a').last().css("margin-right", "auto"); }
    });
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
        if ("<%=tabI %>" != "") {
            $(".hd ul li[id^='one']").attr("class", "");
            $("#one<%=tabI %>").attr("class", "hover");
        }
    });
</script>
<style type="text/css">
    .baseInfo ul li
    {
        width: 365px;
    }
</style>
<div class="hd">
    <ul>
        <li id="one100" class="hover">基本信息 </li>
        <asp:Literal ID="lbCSTMember" runat="server"></asp:Literal>
    </ul>
    <%if (BitAuto.ISDC.CC2012.BLL.Util.CheckButtonRight("SYS024BUT2212"))
      { %>
    &nbsp;&nbsp;<a href="http://<%=CrmUrl %>/CustomInfo/MemberMain.aspx?CustID=<%=CustID %>&SourceUrl=cc&Name=Cst"
        style="float: right; margin-left: 10px; margin-top: 6px; display: none;" id="hrefAddCst"
        target="_blank">新增车商通会员</a>
    <%} %>
    <%if (BitAuto.ISDC.CC2012.BLL.Util.CheckButtonRight("SYS024BUT2211"))
      { %><a href="http://<%=CrmUrl %>/CustomInfo/MemberMain.aspx?CustID=<%=CustID %>&SourceUrl=cc&Name=Cyt"
          style="float: right; margin-left: 10px; margin-top: 6px; margin-right: 90px;
          display: none;" id="hrefAddCyt" target="_blank">新增会员</a>
    <%} %>
</div>
<asp:Literal ID="literalMemberNameList" runat="server"></asp:Literal>
<div class="menuConbox">
    <div class="title" style="margin: 0">
        客户基本信息<a href="javascript:void(0)" onclick="divShowHideEvent('con_one_100',this)"
            class="toggle"></a></div>
    <!--内容1-->
    <div id="con_one_100" class="hover">
        <ul class="clearfix">
            <li style="width: 695px;">
                <label>
                    客户名称：</label>
                <span id="spanCustName" runat="server" style="float: left;"></span>
                <div id="spanStatus" runat="server" style="float: left; margin: 0 5px; display: none;">
                    <img alt="停用" src="/Images/xt_1.gif" /></div>
                <div id="spanLock" runat="server" style="float: left; margin: 0; display: none;">
                    <img alt="锁定" src="/Images/lock.gif" /></div>
                <div style="float: left; padding-left: 10px;">
                    <% for (int i = 0; i < this.StarLevel; i++)
                       { %>
                    <img alt="信用" src="/Images/star.gif" />
                    <% }%>
                </div>
            </li>
            <li>
                <label>
                    客户简称：</label><span id="spanAbbrName" runat="server"></span></li>
            <li>
                <label>
                    客户编号：</label><span><%=this.CustID%></span></li>
            <li>
                <label>
                    营业执照号：</label><span id="spanLicenseNumber" runat="server"></span>
                <%=  this.BLAnnualSurvey == "-1" ? "<img style='margin-left:5px;' alt='未通过' src='/Images/xt_1.gif'/>" : (this.BLAnnualSurvey == "1" ? "<img style='margin-left:5px;' alt='通过' src='/Images/xt.gif'/>" : "")%>
            </li>
            <% if (CarType != 2)
               {%>
            <li id="liShopLevel">
                <label>
                    经营店级别：</label><span id="spanShopLevel" runat="server"></span></li><%} %>
            <li>
                <label>
                    客户类别：</label><span id="spanType" runat="server"></span></li>
            <li>
                <label>
                    客户行业：</label><span id="spanIndustry" runat="server"></span></li>
            <li>
                <label>
                    客户级别：</label><span id="spanLevel" runat="server"></span></li>
            <%if (CarType != 2)
              { %>
            <li id="liFourS" visible="false" runat="server">
                <label>
                    所属4S：</label>
                <span id="spanFourPidName" name="CustPidName" runat="server" /></li>
            <li id="liPid" runat="server">
                <label>
                    所属集团：</label><span id="spanPid" runat="server"></span></li>
            <li id="liCustPid" runat="server">
                <label>
                    所属厂商：</label><span id="spanCustPid" runat="server"></span></li>
            <li style="width: 700px;">
                <label>
                    主营品牌：</label><span id="spanBrand" runat="server" style="float: none; width: 560px;"
                        class="exceed"></span></li>
            <%} %>
            <%if (CarType == 3)
              { %>
            <li>
                <label>
                    二手车经营类型：</label>
                <span id="spanUsedCarBusiness" name="UsedCarBusiness" runat="server" /></li>
            <%} %>
            <li>
                <%if (CarType != 1)
                  { %>
                <label>
                    所属交易市场：</label>
                <span id="spanTradeMarketID" name="TradeMarketID" runat="server" /></li>
            <%} %>
            <li>
                <label>
                    经营范围：</label><span id="spanCarType" runat="server" style="float: none;"></span></li>
            <li>
                <label>
                    联系电话：</label><span id="spanOfficeTel" runat="server"></span></li>
            <li>
                <label>
                    传真：</label><span id="spanFax" runat="server"></span></li>
            <li>
                <label>
                    联系人：</label><span id="spanContractName" runat="server"></span></li>
            <li>
                <label>
                    客户地区：</label><span id="spanProvinceCity" runat="server"></span></li>
            <li style="width: 700px;">
                <label>
                    注册地址：</label><span id="spanAddress" runat="server" class="exceed" style="width: 560px;"></span></li>
            <li>
                <label>
                    邮政编码：</label><span id="spanZipCode" runat="server"></span></li>
            <li>
                <label>
                    创建日期：</label><span id="spanCreateTime" runat="server"></span></li>
            <li>
                <label>
                    创建人：</label><span id="spanCreateUserName" runat="server"></span></li>
            <li>
                <label>
                    修改日期：</label><span id="spanModifyTime" runat="server"></span></li>
            <li>
                <label>
                    修改人：</label><span id="spanModifyUserName" runat="server"></span></li>
            <li class="singleRow" style="width: 700px;">
                <label>
                    备注：</label><span id="spanNotes" runat="server" class="exceed" style="width: 560px;"></span></li>
        </ul>
    </div>
    <div id="con_one_101" class=" ">
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
        <%--<%= this.ListOfBusinessScaleInfo == true ? "<li id='two6' onclick=\"setTab(6)\" class=''>二手车规模</li>" : ""%>--%>
        <%-- <%= this.ListOfTaskRecord == true ? "<li id='two7' onclick=\"setTab(7)\" class=''>任务记录</li>" : ""%>--%>
        <%= this.ListOfWorkOrder == true ? "<li id='two9' onclick=\"setTab(9)\" class=''>工单记录</li>" : ""%>
        <li id='two10' onclick="setTab(10)" class=''>话务记录</li>
        <li id='two11' onclick="setTab(11)" class=''>短信记录</li>
        <li id='two12' onclick="setTab(12)" class=''>申请记录</li>
    </ul>
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
    <%--<%= this.ListOfBusinessScaleInfo == true ? "<div class='cont_cxjg'><div id='con_two_6'  style='display: none;'></div></div>" : ""%>--%>
    <!--内容6-->
    <!--内容7-->
    <%--  <%= this.ListOfTaskRecord == true ? "<div class='cont_cxjg'><div id='con_two_7'  style='display: none;'></div></div>" : ""%>--%>
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
<div class="clear">
</div>
