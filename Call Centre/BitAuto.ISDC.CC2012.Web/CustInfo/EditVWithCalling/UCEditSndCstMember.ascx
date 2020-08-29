<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="UCEditSndCstMember.ascx.cs"
    Inherits="BitAuto.ISDC.CC2012.Web.CustInfo.EditVWithCalling.UCEditSndCstMember" %>
<!--功能废弃 强斐 2016-8-3-->
<script type="text/javascript">
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
        
        // 12.11.7 lxw 会员编码不需要 注：该段代码保存在本地E:\程序备份\20121107老CC\cc删除代码2.txt
        //
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
            $('#divCSTMembers').find('.MemberInfoArea').each(function (i, v) {
                var type = $('#<%= this.selCstMemberType.ClientID %>').val(); 
                $('select[id$="selCstMemberType"]', $(v)).val(type); 
                if (type == "2") {
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
//            openSelectBrandPopup: openSelectBrandPopup,
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
<div class="MemberInfoArea">
    <div class="title">
        <%= this.ControlName %><a href="javascript:void(0)" onclick="divShowHideEvent('divMemberInfo_<%= this.ID %>',this)"
            class="toggle"></a></div>
    <div id="divMemberInfo_<%= this.ID %>">
        <ul class="clearfix">
            <li>
                <label>
                    会员名称：</label>
                <input id="tfCstMemberFullName" runat="server" class="w250" /><em>*</em>
                <input id="tfCstMemberID" name="CstMemberID" style="display: none;" runat="server" />
                <input id="tfRecID" name="RecID" style="display: none;" runat="server" />
            </li>
            <li>
                <label>
                    会员简称：</label>
                <input id="tfCstMemberShortName" runat="server" class="w250" />
                <em>*</em> </li>
            <%--<li>
                <label>
                    会员编码：</label>
                <input id="tfVendorCode" runat="server" />
                <em>*</em> </li>--%>
            <li>
                <label>
                    会员类型：</label>
                <select id="selCstMemberType" runat="server" disabled="disabled" class="w255">
                    <option value="-1">请选择</option>
                    <option value="1">个人</option>
                    <option value="2">4S店</option>
                    <option value="3">经纪公司</option>
                    <option value="4">厂商</option>
                    <option value="5">其他</option>
                </select><em>*</em> </li>
            <li>
                <label>
                    会员地区：</label>
                <select id="selCstMemberProvince" name="Province" class="area" runat="server" style="width: 98px;">
                </select>
                <select id="selCstMemberCity" name="City" class="area" runat="server" style="width: 98px;">
                </select>
                <select id="selCstMemberCounty" name="County" class="lastArea" runat="server" style="width: 98px;">
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
                <input id="tfCstMemberPostCode" runat="server" class="w250" /></li>
            <%-- <li>
                <label>
                    交通信息：</label>
                <input id="tfTrafficInfo" runat="server" />
            </li>
            --%>
            <li style="display: none;" id="liCstMemberBrand">
                <label>
                    主营品牌：</label>
                <input id="tfCstMemberBrandName" name="BrandName" type="text" readonly="readonly"
                    class="w250" runat="server" />
                <input id="tfCstMemberBrand" name="Brand" style="display: none;" runat="server" />
                <span><a href="javascript:uCEditCstMemberHelper_<%= this.ID %>.openSelectBrandPopup();">
                    <img alt="选择主营品牌" src="/images/button_001.gif" border="0" /></a> </span><em>*</em>
            </li>
            <li>
                <label>
                    请选择联系人：</label>
                <select id="sltCstLinkMan" runat="server" class="w255">
                </select>
            </li>
            <li>
                <label>
                    联系人姓名：</label>
                <input id="tfLinkManName" name="Email" runat="server" class="w250" /><em>*</em>
            </li>
            <%-- <li style="width: 260px;">
                <label>
                    部门：</label>
                <input id="tfLinkManDepartment" runat="server" style="width: 150px;" /><em>*</em>
            </li>
            <li style="width: 260px;">
                <label>
                    职务：</label>
                <input id="tfLinkManPosition" runat="server" style="width: 150px;" /><em>*</em>
            </li>--%>
            <li>
                <label>
                    手机：</label>
                <input id="tfLinkManMobile" runat="server" class="w250" /><em>*</em> </li>
            <li>
                <label>
                    电子邮件：</label>
                <input id="tfLinkManEmail" runat="server" class="w250" /><em>*</em> </li>
            <%--暂时不要车商通信息 --%>
            <%if (1 == 2) %>
            <%{ %>
            <asp:PlaceHolder ID="PlaceHolderCstMemberUCount" runat="server"></asp:PlaceHolder>
            <%} %>
        </ul>
    </div>
</div>
