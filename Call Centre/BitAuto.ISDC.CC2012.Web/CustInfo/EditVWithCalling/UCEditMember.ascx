<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="UCEditMember.ascx.cs"
    Inherits="BitAuto.ISDC.CC2012.Web.CustInfo.EditVWithCalling.UCEditMember" %>
<!--功能废弃 强斐 2016-8-3-->
<script type="text/javascript">

    var uCEditMemberHelper_<%= this.ID %> = (function() {
       var marker;
        var openSelectBrandPopup = function() {//选择主营品牌
                $.openPopupLayer({
                    name: "BrandSelectAjaxPopup",
                    parameters: {},
                    url: "/AjaxServers/CustCategory/SelectBrand.aspx?BrandIDs=" + $('#<%= this.tfBrand.ClientID %>').val(),
                    beforeClose: function (e, data) {
                        if (e) {
                            var brandids = $('#popupLayer_' + 'BrandSelectAjaxPopup').data('brandids');
                            var brandnames = $('#popupLayer_' + 'BrandSelectAjaxPopup').data('brandnames');
                            $('#<%= this.tfBrand.ClientID %>').val(brandids);
                        $('#<%= this.tfBrandName.ClientID %>').val(brandnames); 
                        }
                    }
                });
        },

        openSelectSerialBrandPopup = function() {
            $.openPopupLayer({
                name: "SerialBrandSelectAjaxPopup",
                parameters: { SerialIds: $('#<%= this.txtSerialIds.ClientID %>').val() },
                url: "/AjaxServers/Base/SelectCarSerial.aspx",
                beforeClose: function(e, data) {
                    if (e) {
                        if (e) {
                            $('#<%= this.txtSerial.ClientID %>').val(data.BrandNames);
                            $('#<%= this.txtSerialIds.ClientID %>').val(data.BrandIDs);
                        }
                    }
                }
            });
        },

        triggerProvince = function() {//选中省份
            BindCity('<%= this.selProvince.ClientID %>', '<%= this.selCity.ClientID %>');
            BindCounty('<%= this.selProvince.ClientID %>', '<%= this.selCity.ClientID %>', '<%= this.selCounty.ClientID %>');
        },

        triggerCity = function() {//选中城市
            BindCounty('<%= this.selProvince.ClientID %>', '<%= this.selCity.ClientID %>', '<%= this.selCounty.ClientID %>');
            //若城市列表中，没有数据，则添加属性noCounty，值为1，否则不添加属性
            if ($('#<%= this.selCounty.ClientID %> option').size() == 1)
            { $('#<%= this.selCounty.ClientID %>').attr('noCounty', '1'); }
            else
            { $('#<%= this.selCounty.ClientID %>').removeAttr('noCounty'); }
        },
        
        markInMap = function () {
            $.openPopupLayer({
                name: 'MarkInMapPopup',
                url: '/CustInfo/MoreInfo/MarkInMap.aspx',
                parameters: {
                    PopupName: 'MarkInMapPopup',
                    marker_lat:  marker.getPosition().lat,
                    marker_lng:  marker.getPosition().lng
                },
                afterClose: function(ea, cData) {
                    if (ea && cData) {
                        showMap(cData.lat,cData.lng);
                    }
                }
            });
        },

        //展示地图
       
        showMap = function (lat,lng) { 
            var bPoint = new BMap.Point(lng, lat);
            //地图初始化
            
            marker = new BMap.Marker(bPoint);
            $('input[id$="hdnMap"]',$('#divMemberInfo_<%= this.ID %>')).data('marker_lat', lat).data('marker_lng', lng);
             if(lat>0)
             {
             $("#txtMark",$('#divMemberInfo_<%= this.ID %>')).val("已标识");
             }
        },
        
        haveResetPositionOfMap = false,//修正过没
        triggerMemberArea = function(contentId){
            if(memberAreaActionHelper && memberAreaActionHelper.triggerMemberArea){
                 memberAreaActionHelper.triggerMemberArea(contentId);
                 
                 //重新显示地图，防止位置偏移
                 if($('#divMemberInfo_<%= this.ID %>').is(':visible') && !haveResetPositionOfMap) {
                    
                    haveResetPositionOfMap = true;
                 }
            }
        },
        
        removeMemberArea = function(){
            $('#divMemberInfo_<%= this.ID %>').parent().remove();
            if(memberAreaActionHelper && memberAreaActionHelper.init){ memberAreaActionHelper.init(); }
        },
        //删除会员
        deleteMember = function(){
            $.jConfirm('您确定要删除此会员吗？', function(result) {
                if (result) {
                    if('<%= this.MemberID %>' > 0){                    
                        $.getJSON("/CustInfo/EditVWithCalling/Handler.ashx?callback=?", {
                             Action: 'DeleteCCMember',
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
        
        init = function(){        
            $('#<%= this.selProvince.ClientID %>').bind('change', triggerProvince);
            $('#<%= this.selCity.ClientID %>').bind('change', triggerCity);
            BindProvince('<%= this.selProvince.ClientID %>'); //绑定省份 
            if('<%= this.InitialProvinceID %>'!=''&&'<%= this.InitialProvinceID %>'!='-1'&&'<%= this.InitialProvinceID %>'!='-2') 
            {
            $('#<%= this.selProvince.ClientID %>').val('<%= this.InitialProvinceID %>');
            }
            triggerProvince();
            if('<%= this.InitialCityID %>'!=''&&'<%= this.InitialCityID %>'!='-1'&&'<%= this.InitialCityID %>'!='-2') 
            {
            $('#<%= this.selCity.ClientID %>').val('<%= this.InitialCityID %>');
            }
            triggerCity();
            if('<%= this.InitialCountyID %>'!=''&&'<%= this.InitialCountyID %>'!='-1'&&'<%= this.InitialCountyID %>'!='-2') 
            {
            $('#<%= this.selCounty.ClientID %>').val('<%= this.InitialCountyID %>'); 
            }
            
            //如果是新增会员或者是打回的会员，会员类型会根据客户类型而定
           if('<% =OriginalDMSMemberID%>'==''||('<% =OriginalDMSMemberID%>'!=''&&'<%=SyncStatus %>'=='170008'))
            {
              if( $("[id$='selCustType']").val()!=undefined){
                memberTypeControl($("#divMemberInfo_<%= this.ID %>"),"selCustType","selMemberType",false);
              }
              else
              {
                memberTypeControl($("#divMemberInfo_<%= this.ID %>"),"hdnCustType","selMemberType",false);
              }
            }
           showMap('<%= this.Lat %>','<%= this.Lng %>'); 
            
            $('#<%= this.linkMarkInMap.ClientID %>').click(function(){
                markInMap();
            }).show();
                    
        };

        return {
            openSelectBrandPopup: openSelectBrandPopup,
            openSelectSerialBrandPopup: openSelectSerialBrandPopup,
            triggerProvince: triggerProvince,
            triggerCity: triggerCity,
            triggerMemberArea: triggerMemberArea,
            deleteMember: deleteMember,
            init: init
        };
    })();  
    function memberTypeControl(area,sltCustTypeName,sltMemberTypeName,isCustTypeChange)
    {
            //如果客户类别是展厅，会员为4s，且不可编辑           
            var v = $(area);
            var memberArea=$('select[id$="'+sltMemberTypeName+'"]',$(v));
            var custType = $("[id$='"+sltCustTypeName+"']").val();
            if (custType == "20005") {
                $(memberArea).val("3");
                $(memberArea).attr("disabled", "disabled");
            }
            else if (custType == "20009") {
                $(memberArea).val("1");
                $(memberArea).attr("disabled", "disabled");
            }
            else if (custType == "20013") {
                $(memberArea).val("4");
                $(memberArea).attr("disabled", "disabled");
            }   
            else
            {
              $(memberArea).removeAttr("disabled");
              
              //如果客户类型是经销商,而且会员类型不是特许经销商、综合店时，更改会员类型默认为特许经销商
              if(custType=="20004"&&($(memberArea).val()!="2"||$(memberArea).val()!="3")){
                 $(memberArea).val("2");
              }
              //如果客户为4S类型，而且会员类型不是4S、特许经销商、综合店,更改会员类型默认为4s
              else if(custType=="20003"&&($(memberArea).val()!="1"||$(memberArea).val()!="2"||$(memberArea).val()!="3")){
                 $(memberArea).val("1");
              }
            }
            if(isCustTypeChange)
            {
              $('select[id$='+sltMemberTypeName+'] option', $(v)).remove();
              $('select[id$='+sltMemberTypeName+']',$(v)).append("<option value='1'>4S</option>");
              $('select[id$='+sltMemberTypeName+']',$(v)).append("<option value='2'>特许经销商</option>");
              $('select[id$='+sltMemberTypeName+']',$(v)).append("<option value='3'>综合店</option>");
              $('select[id$='+sltMemberTypeName+']',$(v)).append("<option value='4'>车易达</option>");
            }
           //如果客户为4S类型，会员只能是4S、特许经销商、综合店
           if(custType=="20003"){
              $('select[id$='+sltMemberTypeName+'] option[value=4]', $(v)).remove();
           }
           //如果客户为特许经销商类型，会员只能是特许经销商、综合店
           else if(custType=="20004"){
              $('select[id$='+sltMemberTypeName+'] option[value="1"]', $(v)).remove();
              $('select[id$='+sltMemberTypeName+'] option[value="4"]', $(v)).remove();
              if(Len($("input[id$='tf4sName']").val())!=0)
              {
                $('select[id$='+sltMemberTypeName+'] option[value="3"]', $(v)).remove();
              }
           }
           //如果客户为综合店，会员只能是综合店
           else if(custType=="20005"){
              $('select[id$='+sltMemberTypeName+'] option[value="1"]', $(v)).remove();
              $('select[id$='+sltMemberTypeName+'] option[value="2"]', $(v)).remove();
              $('select[id$='+sltMemberTypeName+'] option[value="4"]', $(v)).remove();
           }
           //如果客户为车易达，会员只能是车易达
           else if (custType == "20013") {
            $('select[id$='+sltMemberTypeName+'] option[value="1"]', $(v)).remove();
            $('select[id$='+sltMemberTypeName+'] option[value="2"]', $(v)).remove();
            $('select[id$='+sltMemberTypeName+'] option[value="3"]', $(v)).remove();
           }
           memberTypeChange();
    }    
    function memberTypeChange()
    {
     var memberType = $("#<%=selMemberType.ClientID %>").val();
     if(memberType=="3")
     {
        $("#<%=tfBrand.ClientID %>").parent().find("em").css("display","none");
      }
      else
      {
      $("#<%=tfBrand.ClientID %>").parent().find("em").css("display","");
      }
    }
</script>
<script type="text/javascript">
    $(function() {
        uCEditMemberHelper_<%= this.ID %>.init();
        var heightNum=$($('#<%=liMemberCooperated.ClientID %>').html()).find('br').size();
        $('#<%=liMemberCooperated.ClientID %>').height("auto");
         memberTypeChange();
    }); 
    function extendInformationControl<%= this.ID %>()
    {
       if($("#ulExtendInformation",$("#divMemberInfo_<%= this.ID %>")).css("display")=="none")
       {
        $("#ulExtendInformation",$("#divMemberInfo_<%= this.ID %>")).show('fast');
         $("#hrefExtendInformation",$("#divMemberInfo_<%= this.ID %>")).text("收起扩展信息<<");
       }
       else
       {
       $("#ulExtendInformation",$("#divMemberInfo_<%= this.ID %>")).hide('fast');
       $("#hrefExtendInformation",$("#divMemberInfo_<%= this.ID %>")).text("展开扩展信息>>");
       }
    }
    function phone_<%= this.ID %>() {
        var Tels = "";
        if ($.trim($("#<%=this.tfPhone1.ClientID %>",$("#divMemberInfo_<%= this.ID %>")).val()) != "") {
            Tels = Tels + $.trim($("#<%=this.tfPhone1.ClientID %>",$("#divMemberInfo_<%= this.ID %>")).val()) + ",";
        }
        if ($.trim($("#<%=this.tfPhone2.ClientID %>",$("#divMemberInfo_<%= this.ID %>")).val()) != "") {
            Tels = Tels + $.trim($("#<%=this.tfPhone2.ClientID %>",$("#divMemberInfo_<%= this.ID %>")).val()) + ",";
        }
        if ($.trim($("#<%=this.tfPhone3.ClientID %>",$("#divMemberInfo_<%= this.ID %>")).val()) != "") {
            Tels = Tels + $.trim($("#<%=this.tfPhone3.ClientID %>",$("#divMemberInfo_<%= this.ID %>")).val()) + ",";
        }
        if (Tels != "") {
            Tels = Tels.substring(0, Tels.length - 1);
        }
        return Tels;
    }
</script>
<div class="cont_cx khxx ">
    <div class="title contact" style="clear: both;">
        <%--<span id="spanMemberName_<%= this.ID %>">--%>
        <%= this.ControlName %><%--</span>--%><a href="javascript:void(0)" onclick="divShowHideEvent('divMemberInfo_<%= this.ID %>',this)"
            class="toggle"></a>&nbsp;
        <%= this.OriginalDMSMemberID.Length > 0 ? "" : "<a href=\"javascript:uCEditMemberHelper_" + this.ID + ".deleteMember();\" style=\"float:right; margin-right:20px;*margin-top:-30px;\">删除</a>"%></div>
    <div id="divMemberInfo_<%= this.ID %>" class="MemberInfoArea">
        <ul class="clearfix">
            <li id="liMemberID" runat="server" style="display: none;">
                <label>
                    会员ID：</label>
                <span id="spanMemberID" runat="server"></span></li>
            <li id="liMemberSyncStatus" runat="server" style="display: none;">
                <label>
                    账号状态：</label>
                <span id="spanMemberSyncStatus" runat="server"></span></li>
            <li id="liMemberCooperated" runat="server" style="display: none;">
                <label>
                    已确认排期：</label>
                <span id="spanMemberCooperated" class="exceed" runat="server"></span></li>
            <li id="li400" runat="server" style="display: none;">
                <label>
                    400号码：</label>
                <span id="span400" runat="server"></span></li>
            <li>
                <label>
                    会员名称：</label>
                <input id="tfMemberName" name="MemberName" runat="server" class="w250" />
                <input id="tfMemberID" name="MemberID" style="display: none;" runat="server" />
                <em style="color: Red;">*</em> </li>
            <li>
                <label>
                    会员简称：</label>
                <input id="tfMemberAbbr" name="MemberAbbr" runat="server" class="w250" />
                <em style="color: Red;">*</em> </li>
            <li>
                <label>
                    会员类型：</label>
                <select id="selMemberType" runat="server" class="w255" onchange="memberTypeChange()">
                    <option value="1">4S</option>
                    <option value="2">特许经销商</option>
                    <option value="3">综合店</option>
                    <option value="4">车易达</option>
                </select><em style="color: Red;">*</em> </li>
            <li>
                <label>
                    联系电话：</label>
                <input id="tfPhone1" name="Phone" runat="server" style="width: 79px; *width: 78px;"
                    class="w250" />
                <input id="tfPhone2" name="Phone" runat="server" style="width: 79px; *width: 78px;"
                    class="w250" />
                <input id="tfPhone3" name="Phone" runat="server" style="width: 79px; *width: 78px;"
                    class="w250" />
                <span>
                    <img alt="打电话" style="cursor: pointer" src="/images/phone.gif" border="0" onclick="功能废弃" /></span></li>
            <li>
                <label>
                    主营品牌：</label>
                <input id="tfBrandName" name="BrandName" type="text" readonly="readonly" runat="server"
                    class="w250" />
                <input id="tfBrand" name="Brand" style="display: none;" runat="server" />
                <span><a href="javascript:uCEditMemberHelper_<%= this.ID %>.openSelectBrandPopup();">
                    <img alt="选择主营品牌" src="/images/button_001.gif" border="0" /></a> </span><em style="color: Red;">
                        *</em> </li>
            <li>
                <label>
                    会员地区：</label>
                <select id="selProvince" name="Province" class="area" runat="server" style="width: 82px;">
                </select>
                <select id="selCity" name="City" class="area" runat="server" style="width: 82px;">
                </select>
                <select id="selCounty" name="County" class="lastArea" runat="server" style="width: 82px;">
                </select>
                <em style="color: Red;">*</em> </li>
            <li style="height: auto;">
                <label>
                    地图：</label>
                <input type="text" value="未标识" class="w250" id="txtMark" disabled="disabled" />
                <input type="button" id="linkMarkInMap" value="标点" style="display: none; width: 40px;"
                    class="button" runat="server" />
                <em style="margin-left: 5px; color: Red;">*</em>
                <input type="hidden" id="hdnMap" />
                <%--<div id="divMap" runat="server" style="margin-left: 78px; margin-top: 5px; width: 252px;
                height: 123px;">
            </div>--%>
            </li>
            <li>
                <label>
                    销售地址：</label>
                <input id="tfAddress" name="Address" runat="server" class="w250" />
                <em style="color: Red;">*</em> </li>
            <li style="width: 100%; text-align: right;"><a href="javascript:void(0)" id="hrefExtendInformation"
                onclick='extendInformationControl<%= this.ID %>()'>展开扩展信息>></a></li>
        </ul>
        <ul id="ulExtendInformation" style="display: none;">
            <li>
                <label>
                    传真：</label>
                <input id="tfFax1" name="Fax" runat="server" style="width: 122px;" class="w250" />
                <input id="tfFax2" name="Fax" runat="server" style="width: 122px;" class="w250" />
            </li>
            <li>
                <label>
                    公司网址：</label>
                <input id="tfCompanyWebSite" name="CompanyWebSite" runat="server" class="w250" />
            </li>
            <li>
                <label>
                    Email：</label>
                <input id="tfEmail" name="Email" runat="server" class="w250" />
            </li>
            <li>
                <label>
                    邮政编码：</label>
                <input id="tfZipcode" name="Zipcode" runat="server" class="w250" />
            </li>
            <li>
                <label>
                    附加子品牌：</label>
                <input name="Serial" type="text" id="txtSerial" readonly="readonly" runat="server"
                    class="w250" />
                <input type="text" id="txtSerialIds" readonly="readonly" style="display: none;" runat="server" />
                <a href="javascript:uCEditMemberHelper_<%=this.ID%>.openSelectSerialBrandPopup();">
                    <img alt="选择子品牌" src="/images/button_001.gif" border="0" /></a> </li>
            <li>
                <label>
                    交通信息：</label>
                <input id="tfTrafficInfo" name="TrafficInfo" runat="server" class="w250" />
            </li>
            <li class="singleRow" style="clear: both; width: 900px;">
                <label>
                    企业简介：</label>
                <textarea id="tfEnterpriseBrief" name="EnterpriseBrief" cols="4" rows="2" class="fullRow"
                    style="width: 705px; height: 70px;" runat="server"></textarea>
            </li>
            <li class="singleRow" style="clear: both; width: 900px;">
                <label>
                    备注：</label>
                <textarea id="tfNotes" name="Notes" cols="4" rows="2" class="fullRow" runat="server"
                    style="width: 705px; height: 70px;"></textarea>
            </li>
        </ul>
    </div>
</div>
