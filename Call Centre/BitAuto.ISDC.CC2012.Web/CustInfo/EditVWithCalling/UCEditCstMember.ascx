<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="UCEditCstMember.ascx.cs"
    Inherits="BitAuto.ISDC.CC2012.Web.CustInfo.EditVWithCalling.UCEditCstMember" %>
<!--功能废弃 强斐 2016-8-3-->
<script type="text/javascript">
    //弹出客户名称变更历史
    function openAjaxCstMemberNameHistory(memberId) {
        $.openPopupLayer({
            name: "CstMemberFullNameHistoryShowAjaxPopup",
            parameters: { CstMemberID: memberId },
            url: "/AjaxServers/CSTMember/CstMemberFullNameHistory.aspx"
        });
    }
    var uCEditCstMemberHelper_<%= this.ID %> = (function() {
       var openSelectSerialBrandPopup = function() {
            $.openPopupLayer({
                name: "SerialBrandSelectAjaxPopup",
                parameters: { SerialIds: 1 },
                url: "/AjaxServers/Base/SelectCarSerial.aspx",
                beforeClose: function(e, data) {
                    if (e) {
                        if (e) {
                        }
                    }
                }
            });
        },

         openSelectBrandPopup = function() {//选择主营品牌
            $.openPopupLayer({
                name: "BrandSelectAjaxPopup",
                parameters: { BrandIDs: $('#<%= this.tfCstMemberBrand.ClientID %>').val() },
                url: "/AjaxServers/Base/SelectBrand.aspx",
                beforeClose: function(e, data) {
                    if (e) {
                        $('#<%= this.tfCstMemberBrand.ClientID %>').val(data.BrandIDs);
                        $('#<%= this.tfCstMemberBrandName.ClientID %>').val(data.BrandNames);
                    }
                }
            });
        },
        //12.11.7   lxw 会员编码不需要  注：已删除，该段代码保存在本地E:\程序备份\20121107老CC\cc删除代码.txt

        triggerProvince = function() {//选中省份
            BindCity('<%= this.selCstMemberProvince.ClientID %>', '<%= this.selCstMemberCity.ClientID %>');
            BindCounty('<%= this.selCstMemberProvince.ClientID %>', '<%= this.selCstMemberCity.ClientID %>', '<%= this.selCstMemberCounty.ClientID %>');
        },

        triggerCity = function() {//选中城市
            BindCounty('<%= this.selCstMemberProvince.ClientID %>', '<%= this.selCstMemberCity.ClientID %>', '<%= this.selCstMemberCounty.ClientID %>');
            //若城市列表中，没有数据，则添加属性noCounty，值为1，否则不添加属性
            if ($('#<%= this.selCstMemberCounty.ClientID %> option').size() == 1)
            { $('#<%= this.selCstMemberCounty.ClientID %>').attr('noCounty', '1'); }
            else
            { $('#<%= this.selCstMemberCounty.ClientID %>').removeAttr('noCounty'); }
        },
        
        
        haveResetPositionOfMap = false,//修正过没
        triggerMemberArea = function(contentId){
            if(memberAreaActionHelper && memberAreaActionHelper.triggerMemberArea){
                 memberAreaActionHelper.triggerMemberArea(contentId);
                 
                 //重新显示地图，防止位置偏移
                 if($('#divMemberInfo_<%= this.ID %>').is(':visible') && !haveResetPositionOfMap) {
                    //showMap();
                    haveResetPositionOfMap = true;
                 }
            }
        },
        
        removeMemberArea = function(){
            $('#divMemberInfo_<%= this.ID %>').parent().remove();
            if(memberAreaActionHelper && memberAreaActionHelper.init){ memberAreaActionHelper.init(); }
        },
        
        deleteMember = function(){
            $.jConfirm('您确定要删除此会员吗？', function(result) {
                if (result) {
                    if('<%= this.MemberID %>' > 0){                    
                        $.getJSON("/CustInfo/EditVWithCalling/Handler.ashx?callback=?", {
                             Action: 'DeleteCCCstMember',
                             MemberID: '<%= this.MemberID %>'
                        }, function(jd, textStatus, xhr) {
                            if (textStatus != 'success') { $.jAlert('请求错误'); }
                            else if (jd.success) {
                                removeMemberArea();
                            }
                            else {
                                $.jAlert('错误: ' + jd.message);
                            }
                        });
                    }
                    else{
                        removeMemberArea();
                    }
                }
            });
        },
        getContactInfo=function(){
        var  contactId=$("#<%=this.sltCstLinkMan.ClientID %>").val();
        if(contactId!="-1")
        {
            $("#<%=this.tfLinkManName.ClientID %>").val('');
            $("#<%=this.tfLinkManMobile.ClientID %>").val('');
            $("#<%=this.tfLinkManEmail.ClientID %>").val('');
         $.getJSON("/CustInfo/Handler.ashx?callback=?", {
                             Action: 'getcontactinfo',
                             ContactID: contactId
                        }, function(jd, textStatus, xhr) {
                            if (textStatus != 'success') { $.jAlert('请求错误'); }
                            else if (jd.success) {
                              if(jd.message!="")
                               {
                                   var dataJson = $.evalJSON(jd.message);
                                   $("#<%=this.tfLinkManName.ClientID %>").val(dataJson.CnName);
                                   $("#<%=this.tfLinkManMobile.ClientID %>").val(dataJson.Mobile);
                                   $("#<%=this.tfLinkManEmail.ClientID %>").val(dataJson.Email);
                               }
                            }
                            else {
                                $.jAlert('错误: ' + jd.message);
                            }
                        });
         }
        },
        init = function(){        
            $('#<%= this.selCstMemberProvince.ClientID %>').bind('change', triggerProvince);
            $('#<%= this.selCstMemberCity.ClientID %>').bind('change', triggerCity);

            BindProvince('<%= this.selCstMemberProvince.ClientID %>'); //绑定省份  
            if('<%= this.InitialProvinceID %>'!=''&&'<%= this.InitialProvinceID %>'!='-1'&&'<%= this.InitialProvinceID %>'!='-2') 
            {
            $('#<%= this.selCstMemberProvince.ClientID %>').val('<%= this.InitialProvinceID %>');
            }
            triggerProvince();
            if('<%= this.InitialCityID %>'!=''&&'<%= this.InitialCityID %>'!='-1'&&'<%= this.InitialCityID %>'!='-2') 
            {
            $('#<%= this.selCstMemberCity.ClientID %>').val('<%= this.InitialCityID %>');
            }
            triggerCity();
            if('<%= this.InitialCountyID %>'!=''&&'<%= this.InitialCountyID %>'!='-1'&&'<%= this.InitialCountyID %>'!='-2') 
            {
            $('#<%= this.selCstMemberCounty.ClientID %>').val('<%= this.InitialCountyID %>'); 
            }   
             //初始化车商通会员
             
            $('#divCstMembers').find('.MemberInfoArea').each(function (i, v) {
                 switch($('select[id$="selCustType"]').val()) {
                        case "20003":
                            $('select[id$="selCstMemberType"]',$(v)).val("2");
                            break;
                        case "20010":
                        case "20011":
                            $('select[id$="selCstMemberType"]',$(v)).val("3");
                            break;
                    }

                if ($('select[id$="selCustType"]').val() == "20003") {
                    if ($("#liCstMemberBrand", $(v))) {
                        $("#liCstMemberBrand", $(v)).show();
                    }
                }
                else {
                    $("#liCstMemberBrand", $(v)).hide();
                }
            });       
        };

        return {
            openSelectBrandPopup: openSelectBrandPopup,
            openSelectSerialBrandPopup: openSelectSerialBrandPopup,
            triggerProvince: triggerProvince,
            triggerCity: triggerCity,
            triggerMemberArea: triggerMemberArea,
            deleteMember: deleteMember,
            //createMemberCode:createMemberCode,
            getContactInfo:getContactInfo,
            init: init
        };
    })();       
</script>
<script type="text/javascript">
    $(function() {
        uCEditCstMemberHelper_<%= this.ID %>.init();
    });
</script>
<div class="cont_cx khxx" name="<%=CrmMember %>">
    <div class="title contact" style="clear: both;">
        <%--<span id="spanMemberName_<%= this.ID %>">--%>
        <%= this.ControlName %><%--</span>--%><a href="javascript:void(0)" onclick="divShowHideEvent('divMemberInfo_<%= this.ID %>',this)"
            class="toggle"></a>&nbsp;
        <%= this.OriginalDMSMemberID.Length > 0 ? "" : "<a href=\"javascript:uCEditCstMemberHelper_" + this.ID + ".deleteMember();\" style=\"float:right; margin-right:20px;*margin-top:-30px;\">删除</a>"%></div>
    <div id="divMemberInfo_<%= this.ID %>" class="MemberInfoArea<%=CrmMember %>">
        <ul class="infoBlock firstPart">
            <li>
                <label>
                    会员名称：</label>
                <input id="tfCstMemberFullName" class="w250" runat="server" />
                <input id="tfCstMemberID" name="CstMemberID" style="display: none;" runat="server" />
                <em>*</em>
                <%if (FullNameHavHistory)
                  { %>
                <a href="javascript:void(0)" onclick="javascript:openAjaxCstMemberNameHistory('<%=this.MemberID %>')">
                    曾用名</a>
                <%} %></li>
            <li>
                <label>
                    会员简称：</label>
                <input id="tfCstMemberShortName" class="w250" runat="server" />
                <em>*</em> </li>
            <%-- lxw 12.11.7 <li>
                <label>
                    会员编码：</label>
                <input id="tfVendorCode" runat="server" maxlength="8"/>
                <em>*</em> </li>--%>
            <li>
                <label>
                    会员类型：</label>
                <select id="selCstMemberType" runat="server" class="w255" disabled="disabled">
                    <option value="-1">请选择</option>
                    <option value="1">个人</option>
                    <option value="2">4S店</option>
                    <option value="3">经纪公司</option>
                </select><em>*</em> </li>
            <li>
                <label>
                    会员地区：</label>
                <select id="selCstMemberProvince" name="Province" style="width: 82px;" class="area"
                    runat="server">
                </select>
                <select id="selCstMemberCity" name="City" style="width: 82px;" class="area" runat="server">
                </select>
                <select id="selCstMemberCounty" name="County" style="width: 82px;" class="lastArea"
                    runat="server">
                </select>
                <em>*</em> </li>
            <li>
                <label>
                    上级公司：</label>
                <select id="sltSuperId" runat="server" class="w255">
                </select><em>*</em> </li>
            <li>
                <label>
                    地址：</label>
                <input id="tfCstMemberAddress" runat="server" class="w250" />
            </li>
            <li>
                <label>
                    邮编：</label>
                <input id="tfCstMemberPostCode" class="w250" runat="server" />
            </li>
            <%--<li>
                <label>交通信息：</label>
                <input id="tfTrafficInfo"   runat="server" />
            </li>--%>
            <li style="display: none;" id="liCstMemberBrand">
                <label>
                    主营品牌：</label>
                <input id="tfCstMemberBrandName" name="BrandName" type="text" class="w250" readonly="readonly"
                    runat="server" />
                <input id="tfCstMemberBrand" name="Brand" style="display: none;" runat="server" />
                <span><a href="javascript:uCEditCstMemberHelper_<%= this.ID %>.openSelectBrandPopup();">
                    <img alt="选择主营品牌" src="/images/button_001.gif" border="0" /></a> </span><em>*</em>
            </li>
        </ul>
        <div class="spliter">
        </div>
        <ul class="infoBlock ">
            <li>
                <label style="">
                    请选择联系人：</label>
                <select id="sltCstLinkMan" runat="server" class="w255">
                </select>
            </li>
            <li>
                <label>
                    联系人姓名：</label>
                <input id="tfLinkManName" name="Email" runat="server" class="w250" /><em>*</em>
            </li>
            <%-- <li style=" width:260px;">
                <label>
                    部门：</label>
                <input id="tfLinkManDepartment"  runat="server"  style=" width:150px;"/><em>*</em> 
            </li>
            <li style=" width:260px;">
                <label>
                    职务：</label>
                <input id="tfLinkManPosition"  runat="server"  style=" width:150px;"/><em>*</em> 
            </li>--%>
            <li>
                <label>
                    手机：</label>
                <input id="tfLinkManMobile" runat="server" class="w250" /><em>*</em> </li>
            <li>
                <label>
                    电子邮件：</label>
                <input id="tfLinkManEmail" runat="server" class="w250" /><em>*</em> </li>
        </ul>
        <%--暂时不要车商通信息 --%>
        <%if (1 == 2) %>
        <%{ %>
        <asp:PlaceHolder ID="PlaceHolderCstMemberUCount" runat="server"></asp:PlaceHolder>
        <%} %>
    </div>
