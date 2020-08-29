<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="UCCust.ascx.cs" Inherits="BitAuto.ISDC.CC2012.Web.CustInfo.DetailVWithCalling.UCCust" %>
<%@ Register Src="~/ReturnVisit/SurveyList.ascx" TagName="SurveyList" TagPrefix="uc1" %>
<!--客户回访专用控件 2016-8-3 强斐-->
<script type="text/javascript">
    var uCCustHelper = (function () {
        var loadContacts = function () {
            //显示客户联系人
            AjaxPostAsync('/ReturnVisit/CC_Contact/ListWithEdit.aspx?TaskType=' + escape("客户回访"), {
                ContentElementId: 'divCustContacts',
                CustID: '<%= this.CustID %>',
                NoTab: 1,
                PageSize: 5
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
                //                case 6:                        
                //                    $("#con_two_6").load("/CustInfo/MoreInfo/SecondCarList.aspx", { CustID: custId, ContentElementId: 'con_two_6', PageSize: 5 }, function () { mao(menu); });                        
                //                    break;                        
                case 7:
                    $("#con_two_7").load("/CustInfo/MoreInfo/TaskRecord.aspx", { CustID: custId, ContentElementId: 'con_two_7', PageSize: 5 }, function () { mao(menu); });
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
    function setTab_UCCust(cursel) {
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
        <%if (AddReturnVistCustButton)
          { %>
        <a style="float: right; margin-right: 40px; *margin-top: -30px;" href="<%=AddWorderv2Url %>"
            target="_blank">添加工单 </a>
        <%} %>
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
                <span id="spanBrandName" name="BrandName" style="float: left; width: 70%;" runat="server" />
            </li>
            <%} %>
            <li>
                <label style="width: 115px">
                    电话：</label>
                <span id="spanOfficeTel" name="OfficeTel" runat="server" style="vertical-align: middle;" />
                <span style="vertical-align: middle;"><a ctel="<%=officeTel %>" <%=AddWorderv2Url_Phone %>>
                    <img alt="打电话" src="/images/phone.gif" border="0" style="vertical-align: middle;
                        top: 0px;" /></a> </span></li>
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
                <%= this.ListOfCooperationProjects == true ? "<li id='two2' onclick=\"setTab_UCCust(2)\" class=''>合作项</li>" : ""%>
                <%= this.ListOfCustUser == true ? "<li id='two3' onclick=\"setTab_UCCust(3)\" class=''>负责员工</li>" : ""%>
                <%= this.ListOfReturnVisit == true ? "<li id='two4' onclick=\"setTab_UCCust(4)\" class=''>访问记录</li>" : ""%>
                <%= this.ListOfBusinessLicense == true ? "<li id='two5' onclick=\"setTab_UCCust(5)\" class=''>年检记录</li>" : ""%>
                <%= this.ListOfBusinessBrandLicense == true ? "<li id='two8' onclick='setTab_UCCust(8)' class=''>品牌授权书</li>":""%>
                <%--<%= this.ListOfBusinessScaleInfo == true ? "<li id='two6' onclick=\"setTab_UCCust(6)\" class=''>二手车规模</li>" : ""%>--%>
                <%-- <%= this.ListOfTaskRecord == true ? "<li id='two7' onclick=\"setTab_UCCust(7)\" class=''>任务记录</li>" : ""%>--%>
                <%= this.ListOfWorkOrder == true ? "<li id='two9' onclick=\"setTab_UCCust(9)\" class=''>工单记录</li>" : ""%>
                <li id='two10' onclick="setTab_UCCust(10)" class=''>话务记录</li>
                <li id='two11' onclick="setTab_UCCust(11)" class=''>短信记录</li>
            </ul>
        </div>
        <div class="menuConbox menuConbox2">
            <!--内容1-->
            <!--内容1-->
            <%= this.ListOfCooperationProjects == true ? "<div class='cont_cxjg'><div id='con_two_2'></div></div>" : ""%>
            <%= this.ListOfCustUser == true ? "<div class='cont_cxjg'><div id='con_two_3'  style='display: none;'></div></div>" : ""%>
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
            <%-- <%= this.ListOfTaskRecord == true ? "<div class='cont_cxjg'><div id='con_two_7'  style='display: none;'></div></div>" : ""%>--%>
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
