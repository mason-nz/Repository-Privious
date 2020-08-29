<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="UCCust.ascx.cs" Inherits="BitAuto.ISDC.CC2012.Web.CRMStopCust.UCCust" %>
<%@ Register Src="~/ReturnVisit/SurveyList.ascx" TagName="SurveyList" TagPrefix="uc1" %>
<!--客户核实专用控件 2016-8-3 强斐-->
<script type="text/javascript">
    var uCCustHelper = (function () {
        var loadContacts = function () {//显示客户联系人
            AjaxPostAsync('/ReturnVisit/CC_Contact/ListWithEdit.aspx?TaskType=' + escape("客户核实"), {
                ContentElementId: 'divCustContacts',
                CustID: '<%= this.CustID %>',
                NoTab: 1,
                PageSize: 5,
                isCBInfoPop: 1,
                CustType: 3 //客户分类：3-经销商；4-个人；
            }, null, function (html) {
                $('#divCustContacts').append(html);
            });
        },

        mao = function (menu) {
            var m = "";
            if (menu != "" && menu != undefined) {
                m = "#m";
                window.location.hash = m;
            }
        },

        loadTabList = function (custId, n, menu) {
            switch (n) {
                case 1:
                    $("#con_two_1").load("/CustInfo/MoreInfo/ContactInfoList.aspx", { CustID: custId, ContentElementId: 'con_two_1', PageSize: 5 }, function () { mao(menu); });
                    break;
                case 2:
                    $("#con_two_2").load("/CustInfo/MoreInfo/CooperationProjectList.aspx", { CustID: custId, ContentElementId: 'con_two_2', PageSize: 5 }, function () { mao(menu); });
                    break;
                case 3:
                    $("#con_two_3").load("/CustInfo/MoreInfo/CustUserList.aspx", { CustID: custId, ContentElementId: 'con_two_3', PageSize: 5 }, function () { mao(menu); });
                    break;
                case 4:
                    $("#con_two_4").load("/CustInfo/MoreInfo/ReturnVisitList.aspx", { CustID: custId, ContentElementId: 'con_two_4', PageSize: 5 }, function () { mao(menu); });
                    break;
                case 5:
                    $("#con_two_5").load("/CustInfo/MoreInfo/BusinessLicenseList.aspx", { CustID: custId, ContentElementId: 'con_two_5', PageSize: 5 }, function () { mao(menu); });
                    break;
                case 6:
                    $("#con_two_6").load("/CustInfo/MoreInfo/SecondCarList.aspx", { CustID: custId, ContentElementId: 'con_two_6', PageSize: 5 }, function () { mao(menu); });
                    break;
                case 7:
                    $("#con_two_7").load("/CustInfo/MoreInfo/TaskRecord.aspx", { CustID: custId, ContentElementId: 'con_two_7', PageSize: 5 }, function () { mao(menu); });
                    break;
            }
        },


        init = function () {
            //得到当前会员区域
            memberAreaActionHelper.triggerFirst();

            loadContacts();
            loadTabList('<%= this.CustID %>', 2);
        };

        return { init: init, loadTabList: loadTabList }
    })();
</script>
<script type="text/javascript">
    var memberAreaActionHelper = (function () {
        var currentMemberArea, //当前会员信息区域

        getCurrentMemerArea = function () {
            if (currentMemberArea && currentMemberArea.length > 0) { return currentMemberArea; }
            else { return null; }
        },

        triggerMemberArea = function (contentId) {//隐藏、显示区域
            var content = $('#' + contentId);
            if (content.length == 0) { return; }
            if (content.is(':visible')) {
                content.hide('fast');
                currentMemberArea = null;
            }
            else {
                $('#divMembers .MemberInfoArea:visible').hide('fast');
                content.show('fast');
                currentMemberArea = content;
            }
        },

        triggerFirst = function () {
            $('#divMembers .MemberInfoArea div[id^="divMemberInfo_"]:not(:first),#divMembers div[id^="divMemberInfo_"][.MemberInfoArea]:not(:first)').hide();
            //$('#divMembers .MemberInfoArea:not(:first)').hide();
            var f = $('#divMembers .MemberInfoArea:first');
            if (f && f.length > 0) { f.show('fast'); currentMemberArea = f; }
        },

        init = function () {
            currentMemberArea = $('#divMembers').find('.MemberInfoArea:visible');
            if (currentMemberArea.length == 0) { currentMemberArea = null; }
        };

        return {
            getCurrentMemerArea: getCurrentMemerArea,
            triggerMemberArea: triggerMemberArea,
            triggerFirst: triggerFirst//,
            //init: init
        };
    })();
</script>
<script type="text/javascript">
    function setTab(cursel) {
        $(".menuConbox2 [id^='con_two']").css("display", "none");
        $(".menuConbox2 .cont_cxjg").css("display", "none");
        $(".hd2 ul li[id^='two']").attr("class", "");
        $("#two" + cursel).attr("class", "hover");
        $("#con_two_" + cursel).css("display", "block").parent().css("display", "block");
        uCCustHelper.loadTabList('<%= this.CustID %>', cursel, 1);
    }
    $(function () {
        $("#two2").attr("class", "hover");
        uCCustHelper.init();
    });
</script>
<div class="cont_cx khxx CustInfoArea">
    <div class="title ft16">
        基本信息 <a href="javascript:void(0)" onclick="divShowHideEvent('divCustBasicInfo',this)"
            class="toggle hide"></a>
    </div>
    <div id="divCustBasicInfo">
        <ul class="infoBlock firstPart">
            <li>
                <label style="width: 115px">
                    客户名称：</label>
                <span id="spanCustName" name="CustName" runat="server" style="width: 300px; float: left;
                    clear: none;" /></li>
            <li>
                <label style="width: 115px">
                    客户简称：</label>
                <span id="spanCustAbbr" name="CustAbbr" runat="server" /></li>
        </ul>
        <ul class="infoBlock firstPart">
            <li>
                <label style="width: 115px">
                    客户类别：</label>
                <span id="spanCustType" name="CustType" runat="server" /></li>
            <li>
                <label style="width: 115px">
                    客户行业：</label>
                <span id="spanCustIndustry" name="CustIndustry" runat="server" /></li>
            <li>
                <label style="width: 115px">
                    客户地区：</label>
                <span id="spanArea" name="CustIndustry" runat="server" /></li>
            <li>
                <label style="width: 115px">
                    注册地址：</label>
                <span id="spanAddress" name="Address" runat="server" /></li>
            <li>
                <label style="width: 115px">
                    邮政编码：</label>
                <span id="spanZipcode" name="Zipcode" runat="server" /></li>
        </ul>
        <div class="spliter">
        </div>
        <ul class="infoBlock ">
            <li>
                <label style="width: 115px">
                    客户级别：</label>
                <span id="spanCustLevel" name="CustLevel" runat="server" /></li>
            <%if (CarType != 2)
              { %>
            <li id="liCustPid" runat="server">
                <label style="width: 115px">
                    所属厂商：</label>
                <span id="spanCustPidName" name="CustPidName" runat="server" /></li>
            <li id="liPid" runat="server">
                <label style="width: 115px">
                    所属集团：</label>
                <span id="spanPidName" name="PidName" runat="server" /></li>
            <li>
                <label style="width: 115px">
                    经营店级别：</label>
                <span id="spanShopLevel" name="ShopLevel" runat="server" /></li>
            <li>
                <label style="width: 115px">
                    主营品牌：</label>
                <span id="spanBrandName" name="BrandName" runat="server" /></li>
            <%} %>
            <li>
                <label style="width: 115px">
                    电话：</label>
                <span id="spanOfficeTel" name="OfficeTel" runat="server" /><span><a href="javascript:void(0);"
                    onclick="CallOutForCRM('<%=OfficeTel %>','<%=FirstMemberCode %>','<%=FirstMemberName %>','<%= CustID %>','<%=ContactName %>','-1');">
                    <img alt="打电话" src="/images/phone.gif" border="0" /></a> </span></li>
            <li>
                <label style="width: 115px">
                    传真：</label>
                <span id="spanFax" name="Fax" runat="server" /></li>
            <li>
                <label style="width: 115px">
                    联系人：</label>
                <span id="spanContactName" name="ContactName" runat="server" /></li>
            <li class="singleRow">
                <label>
                    备注：</label>
                <span id="spanNotes" name="Notes" runat="server" /></li>
            <li class="singleRow">
                <label style="width: 115px;">
                    客户联系人：</label>
                <a style="float: right; margin-right: 120px; cursor: Pointer;" onclick="javascript:if(contactInfoList_CC_Helper){contactInfoList_CC_Helper.addNewContactInfo($('select[id$=selCustType]').val());}">
                    添加联系人 </a>
                <div id="divCustContacts" class="fullRow  cont_cxjg" style="margin-left: 78px; float: left;">
                </div>
            </li>
        </ul>
        <div class="spliter">
        </div>
        <div class="hd2">
            &nbsp;&nbsp;<ul>
                <%= this.ListOfCooperationProjects == true ? "<li id='two2' onclick=\"setTab(2)\" class=''>合作项</li>" : ""%>
                <%= this.ListOfCustUser == true ? "<li id='two3' onclick=\"setTab(3)\" class=''>负责员工</li>" : ""%>
            </ul>
        </div>
        <div class="menuConbox menuConbox2">
            <!--内容1-->
            <!--内容1-->
            <%= this.ListOfCooperationProjects == true ? "<div class='cont_cxjg'><div id='con_two_2'></div></div>" : ""%>
            <%= this.ListOfCustUser == true ? "<div class='cont_cxjg'><div id='con_two_3'  style='display: none;'></div></div>" : ""%>
            <!--内容2-->
            <!--内容2-->
            <!--内容3-->
            <!--内容3-->
            <!--内容4-->
            <!--内容7-->
        </div>
    </div>
</div>
<div id="divMembers">
    <asp:PlaceHolder ID="PlaceHolder" runat="server"></asp:PlaceHolder>
</div>
<div id="divCSTMembers">
    <asp:PlaceHolder ID="CSTPlaceHolder" runat="server"></asp:PlaceHolder>
</div>
<div class="cont_cx khxx CustInfoArea" name="uc_survey">
    <div class="title ft16" style="clear: both">
        问卷调查 <a class="toggle hide" onclick="divShowHideEvent('divSurvey',this)" href="javascript:void(0)">
        </a>
    </div>
    <div id="divSurvey">
        <uc1:SurveyList ID="SurveyListID" runat="server" />
    </div>
</div>
