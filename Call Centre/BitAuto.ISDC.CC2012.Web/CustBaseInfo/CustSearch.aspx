<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/Controls/Top.Master"
    Title="CRM客户" CodeBehind="CustSearch.aspx.cs" Inherits="BitAuto.ISDC.CC2012.Web.CustBaseInfo.CustSearch" %>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <script src="../Js/Enum/Area2.js" type="text/javascript"></script>
    <script type="text/javascript">
        //document.domain = "<%=SysUrl %>";

        var params = {};
        var crmCustCheckHelper = (function () {
            var triggerProvince = function () {//选中省份
                BindCity('<%=ddlProvince.ClientID %>', '<%=ddlCity.ClientID %>');
                BindCounty('<%=ddlProvince.ClientID %>', '<%=ddlCity.ClientID %>', '<%=ddlCounty.ClientID %>');
            },

            openSelectCustomerPopup = function () {//选择交易市场 
                $.openPopupLayer({
                    name: 'SelectCustomerPopup',
                    parameters: { type: 3 },
                    url: "../../AjaxServers/Base/SelectCustomer.aspx?page=1",
                    beforeClose: function (e, data) {
                        if (e) {
                            $('#<%= this.tfCustPidName.ClientID %>').val(data.CustName);
                            $('#<%= this.tfCustPid.ClientID %>').val(data.CustID);
                        }
                    },
                    afterClose: function () {
                        //敲回车键执行方法
                        enterSearch(crmCustCheckHelper.search);
                    }
                });
            },
            triggerCity = function () {//选中城市
                BindCounty('<%=ddlProvince.ClientID %>', '<%=ddlCity.ClientID %>', '<%=ddlCounty.ClientID %>');
            },

            openSelectBrandPopup = function () {//选择主营品牌
                $.openPopupLayer({
                    name: "BrandSelectAjaxPopup",
                    parameters: {},
                    url: "/AjaxServers/CustCategory/SelectBrand.aspx?BrandIDs=" + $('#tfBrand').val(),
                    beforeClose: function (e, data) {
                        if (e) {
                            var brandids = $('#popupLayer_' + 'BrandSelectAjaxPopup').data('brandids');
                            var brandnames = $('#popupLayer_' + 'BrandSelectAjaxPopup').data('brandnames');
                            $('#tfBrand').val(brandids);
                            $('#tfBrandName').val(brandnames);
                        }
                    },
                    afterClose: function () {
                        //敲回车键执行方法
                        enterSearch(crmCustCheckHelper.search);
                    }
                });
            };

            //默认查询参数（全局）
            var defaultPostData = {},
            //初始化defaultPostData      
            initDefaultPostData = function () { },

            //列表容器ID
            contentElementId = 'divQueryResultContent',
            //配置分页控件
            pagerConfig = {
                ContentElementId: contentElementId
                //GroupLengthInPager: 5,
                //pageSize: 10
            },

            //查询基本方法
            _query = function (postData) {
                //初始化列表
                LoadingAnimation(contentElementId);
                var monitorPageTime = new Date().getTime(); //监控页面加载耗时_开始时间
                $('#' + contentElementId).load(
                    '/AjaxServers/Statistics/CrmCustList.aspx',
                        $.extend({}, pagerConfig, defaultPostData, postData), function () {

                            var msg = '';
                            //--------生成查询参数并进行校验--------
                            var custID = $.trim($('#tfCustID').val());
                            var custName = $.trim($('#txtCustName').val());
                            var custAbbr = $.trim($('#txtCustAbbr').val());
                            var provinceID = $.trim($('#<%=ddlProvince.ClientID %>').val());
                            var cityID = $.trim($('#<%=ddlCity.ClientID %>').val());
                            var countyID = $.trim($('#ddlCounty').val());
                            var address = $.trim($('#txtAddress').val());
                            var brandIDs = $.trim($('#tfBrand').val());
                            var Status = $(":checkbox[id^='chkStatus'][checked=true]").map(function () { return $(this).val(); }).get().join(",");
                            var lockStatus = $(":checkbox[id^='chkLock'][checked=true]").map(function () { return $(this).val(); }).get().join(",");
                            var chxIsHaveMember = $('#chxIsHaveMember').attr('checked');
                            var chxIsHaveNoMember = $('#chxIsHaveNoMember').attr('checked');
                            var txtCreateCustStartDate = $.trim($('#txtCreateCustStartDate').val());
                            var txtCreateCustEndDate = $.trim($('#txtCreateCustEndDate').val());
                            var carType = $('input[name="carType"]:checked').map(function () { return $(this).val() }).get().join(",");
                            var tradeMarketID = $('#<%= this.tfCustPid.ClientID %>').val();

                            //有排期、无排期标识
                            var cooperatedStatusIDs = $(":checkbox[id*='MemberCooperated'][checked]").map(function () {
                                return $(this).val();
                            }).get().join(",");
                            //有排期开始时间
                            var txtSearchBeginMemberCooperatedTime = $.trim($('#txtSearchBeginMemberCooperatedTime:enabled').val());
                            //有排期结束时间
                            var txtSearchEndMemberCooperatedTime = $.trim($('#txtSearchEndMemberCooperatedTime:enabled').val());
                            //无排期开始时间
                            var txtSearchBeginNoMemberCooperatedTime = $.trim($('#txtSearchBeginNoMemberCooperatedTime:enabled').val());
                            //无排期结束时间
                            var txtSearchEndNoMemberCooperatedTime = $.trim($('#txtSearchEndNoMemberCooperatedTime:enabled').val());
                            //有排期中，销售或试用
                            var ckxMemberCooperateStatus = $(":checkbox[id^='ckxMemberCooperateStatus']:enabled:checked:visible").map(function () {
                                return $(this).val();
                            }).get().join(",");
                            //客户类别
                            var clientType = $('input[name="clientType"]:checked').map(function () { return $(this).val() }).get().join(",");
                            //数据来源 
                            var dataSource = $(":checkbox[id^='chkSource']:checked").map(function () {
                                return $(this).val();
                            }).get().join(",");
                            //                //行政区划
                            //                var areaTypeID = $(":checkbox[name^='AreaType']:checked").map(function () {
                            //                    return $(this).val();
                            //                }).get().join(",");
                            var districtName = $.trim($("#<%=ddlDistrict.ClientID%>").val()); ;
                            if ($.trim(txtCreateCustStartDate).length > 0) {
                                if (!($.trim(txtCreateCustStartDate).isDate())) {
                                    $.jAlert("您输入的创建客户开始时间格式不正确", function () {
                                        $('#txtCreateCustStartDate').val('');
                                        $('#txtCreateCustStartDate').focus();
                                    });
                                    return;
                                }
                            }
                            if ($.trim(txtCreateCustEndDate).length > 0) {
                                if (!($.trim(txtCreateCustEndDate).isDate())) {
                                    $.jAlert("您输入的创建客户结束时间格式不正确", function () {
                                        $('#txtCreateCustEndDate').val('');
                                        $('#txtCreateCustEndDate').focus();
                                    });
                                    return;
                                }
                            }
                            var reg = new RegExp("-", "g");
                            if ((txtSearchBeginMemberCooperatedTime != '' && txtSearchEndMemberCooperatedTime == '') ||
                (txtSearchBeginMemberCooperatedTime == '' && txtSearchEndMemberCooperatedTime != '')) {
                                $.jAlert('您必须输入有排期开始时间或有排期结束时间内容'); return;
                            }
                            if ((txtSearchBeginNoMemberCooperatedTime != '' && txtSearchEndNoMemberCooperatedTime == '') ||
                (txtSearchBeginNoMemberCooperatedTime == '' && txtSearchEndNoMemberCooperatedTime != '')) {
                                $.jAlert('您必须输入无排期开始时间或无排期结束时间内容'); return;
                            }
                            if (Date.parse(txtSearchEndMemberCooperatedTime.replace(reg, '/')) - Date.parse(txtSearchBeginMemberCooperatedTime.replace(reg, '/')) < 0) {
                                $.jAlert("您必须输入有排期开始时间必须小于等于有排期结束时间");
                                return false;
                            }
                            if (Date.parse(txtSearchEndNoMemberCooperatedTime.replace(reg, '/')) - Date.parse(txtSearchBeginNoMemberCooperatedTime.replace(reg, '/')) < 0) {
                                $.jAlert("您必须输入无排期开始时间必须小于等于无排期结束时间");
                                return false;
                            }

                            var newparams = {
                                CustName: custName,
                                CustAbbr: custAbbr,
                                ProvinceID: provinceID,
                                CityID: cityID,
                                CountyID: countyID,
                                BrandIDs: brandIDs,
                                DistrictName: districtName,
                                CustStatus: Status,
                                CustLockStatus: lockStatus,
                                CustHaveMember: chxIsHaveMember,
                                CustHaveNoMember: chxIsHaveNoMember,
                                Address: address,
                                CustID: custID,
                                CreateCustStartDate: txtCreateCustStartDate,
                                CreateCustEndDate: txtCreateCustEndDate,
                                DataSource: dataSource,
                                CooperatedStatusIDs: cooperatedStatusIDs,
                                BeginMemberCooperatedTime: txtSearchBeginMemberCooperatedTime,
                                EndMemberCooperatedTime: txtSearchEndMemberCooperatedTime,
                                BeginNoMemberCooperatedTime: txtSearchBeginNoMemberCooperatedTime,
                                EndNoMemberCooperatedTime: txtSearchEndNoMemberCooperatedTime,
                                MemberCooperateStatus: ckxMemberCooperateStatus,
                                CarType: carType,
                                TradeMarketID: tradeMarketID,
                                CustType: clientType
                            };

                            var strParam = JSON.stringify(newparams);
                            var reg = new RegExp(":", "g");
                            var reg1 = new RegExp(",", "g");
                            var reg2 = new RegExp("\"", "g");
                            strParams = strParam.replace(reg, "=");
                            strParams = strParams.replace(reg1, "&");
                            strParams = strParams.replace(reg2, "");
                            strParams = strParams.substring(1, strParams.length - 1);
                            StatAjaxPageTime(monitorPageTime, '/AjaxServers/Statistics/CrmCustList.aspx' + "?" + strParams);
                        }
                );
            },
            search = function () {
                var msg = '';
                //--------生成查询参数并进行校验--------
                var custID = $.trim($('#tfCustID').val());
                var custName = $.trim($('#txtCustName').val());
                var custAbbr = $.trim($('#txtCustAbbr').val());
                var provinceID = $.trim($('#<%=ddlProvince.ClientID %>').val());
                var cityID = $.trim($('#<%=ddlCity.ClientID %>').val());
                var countyID = $.trim($('#ddlCounty').val());
                var address = $.trim($('#txtAddress').val());
                var brandIDs = $.trim($('#tfBrand').val());
                var Status = $(":checkbox[id^='chkStatus'][checked=true]").map(function () { return $(this).val(); }).get().join(",");
                var lockStatus = $(":checkbox[id^='chkLock'][checked=true]").map(function () { return $(this).val(); }).get().join(",");
                var chxIsHaveMember = $('#chxIsHaveMember').attr('checked');
                var chxIsHaveNoMember = $('#chxIsHaveNoMember').attr('checked');
                var txtCreateCustStartDate = $.trim($('#txtCreateCustStartDate').val());
                var txtCreateCustEndDate = $.trim($('#txtCreateCustEndDate').val());
                var carType = $('input[name="carType"]:checked').map(function () { return $(this).val() }).get().join(",");
                var tradeMarketID = $('#<%= this.tfCustPid.ClientID %>').val();

                //有排期、无排期标识
                var cooperatedStatusIDs = $(":checkbox[id*='MemberCooperated'][checked]").map(function () {
                    return $(this).val();
                }).get().join(",");
                //有排期开始时间
                var txtSearchBeginMemberCooperatedTime = $.trim($('#txtSearchBeginMemberCooperatedTime:enabled').val());
                //有排期结束时间
                var txtSearchEndMemberCooperatedTime = $.trim($('#txtSearchEndMemberCooperatedTime:enabled').val());
                //无排期开始时间
                var txtSearchBeginNoMemberCooperatedTime = $.trim($('#txtSearchBeginNoMemberCooperatedTime:enabled').val());
                //无排期结束时间
                var txtSearchEndNoMemberCooperatedTime = $.trim($('#txtSearchEndNoMemberCooperatedTime:enabled').val());
                //有排期中，销售或试用
                var ckxMemberCooperateStatus = $(":checkbox[id^='ckxMemberCooperateStatus']:enabled:checked:visible").map(function () {
                    return $(this).val();
                }).get().join(",");
                //客户类别
                var clientType = $('input[name="clientType"]:checked').map(function () { return $(this).val() }).get().join(",");
                //数据来源 
                var dataSource = $(":checkbox[id^='chkSource']:checked").map(function () {
                    return $(this).val();
                }).get().join(",");
                var districtName = $.trim($("#<%=ddlDistrict.ClientID%>").val()); ;
                if ($.trim(txtCreateCustStartDate).length > 0) {
                    if (!($.trim(txtCreateCustStartDate).isDate())) {
                        $.jAlert("您输入的创建客户开始时间格式不正确", function () {
                            $('#txtCreateCustStartDate').val('');
                            $('#txtCreateCustStartDate').focus();
                        });
                        return;
                    }
                }
                if ($.trim(txtCreateCustEndDate).length > 0) {
                    if (!($.trim(txtCreateCustEndDate).isDate())) {
                        $.jAlert("您输入的创建客户结束时间格式不正确", function () {
                            $('#txtCreateCustEndDate').val('');
                            $('#txtCreateCustEndDate').focus();
                        });
                        return;
                    }
                }
                var reg = new RegExp("-", "g");
                if ((txtSearchBeginMemberCooperatedTime != '' && txtSearchEndMemberCooperatedTime == '') ||
                (txtSearchBeginMemberCooperatedTime == '' && txtSearchEndMemberCooperatedTime != '')) {
                    $.jAlert('您必须输入有排期开始时间或有排期结束时间内容'); return;
                }
                if ((txtSearchBeginNoMemberCooperatedTime != '' && txtSearchEndNoMemberCooperatedTime == '') ||
                (txtSearchBeginNoMemberCooperatedTime == '' && txtSearchEndNoMemberCooperatedTime != '')) {
                    $.jAlert('您必须输入无排期开始时间或无排期结束时间内容'); return;
                }
                if (Date.parse(txtSearchEndMemberCooperatedTime.replace(reg, '/')) - Date.parse(txtSearchBeginMemberCooperatedTime.replace(reg, '/')) < 0) {
                    $.jAlert("您必须输入有排期开始时间必须小于等于有排期结束时间");
                    return false;
                }
                if (Date.parse(txtSearchEndNoMemberCooperatedTime.replace(reg, '/')) - Date.parse(txtSearchBeginNoMemberCooperatedTime.replace(reg, '/')) < 0) {
                    $.jAlert("您必须输入无排期开始时间必须小于等于无排期结束时间");
                    return false;
                }

                params = {
                    CustName: encodeURIComponent(custName),
                    CustAbbr: encodeURIComponent(custAbbr),
                    ProvinceID: encodeURIComponent(provinceID),
                    CityID: encodeURIComponent(cityID),
                    CountyID: encodeURIComponent(countyID),
                    BrandIDs: encodeURIComponent(brandIDs),
                    DistrictName: encodeURIComponent(districtName),
                    CustStatus: encodeURIComponent(Status),
                    CustLockStatus: encodeURIComponent(lockStatus),
                    CustHaveMember: chxIsHaveMember,
                    CustHaveNoMember: chxIsHaveNoMember,
                    Address: encodeURIComponent(address),
                    CustID: encodeURIComponent(custID),
                    CreateCustStartDate: encodeURIComponent(txtCreateCustStartDate),
                    CreateCustEndDate: encodeURIComponent(txtCreateCustEndDate),
                    DataSource: encodeURIComponent(dataSource),
                    CooperatedStatusIDs: encodeURIComponent(cooperatedStatusIDs),
                    BeginMemberCooperatedTime: escapeStr(txtSearchBeginMemberCooperatedTime),
                    EndMemberCooperatedTime: escapeStr(txtSearchEndMemberCooperatedTime),
                    BeginNoMemberCooperatedTime: escapeStr(txtSearchBeginNoMemberCooperatedTime),
                    EndNoMemberCooperatedTime: escapeStr(txtSearchEndNoMemberCooperatedTime),
                    MemberCooperateStatus: escapeStr(ckxMemberCooperateStatus),
                    CarType: carType,
                    TradeMarketID: tradeMarketID,
                    CustType: escapeStr(clientType)
                };
                if (params) {
                    _query(params);
                }
            };

            return {
                triggerProvince: triggerProvince,
                triggerCity: triggerCity,
                openSelectBrandPopup: openSelectBrandPopup,
                search: search,
                openSelectCustomerPopup: openSelectCustomerPopup
            };
        })();
        function ExportCust() {
            var paramsStr = "";
            for (var attr in params) {
                paramsStr += "&" + attr + "=" + params[attr];
            }
            window.location = "/CustBaseInfo/ExportCustList.aspx?Browser=" + GetBrowserName() + paramsStr;
        }
    </script>
    <script type="text/javascript">
        $(function () {

            $('#txtSearchBeginNoMemberCooperatedTime').bind('click', function () { document.domain = '<%=SysUrl %>'; MyCalendar.SetDate(this, document.getElementById('txtSearchBeginNoMemberCooperatedTime')) });
            $('#txtSearchEndNoMemberCooperatedTime').bind('click', function () { document.domain = '<%=SysUrl %>'; MyCalendar.SetDate(this, document.getElementById('txtSearchEndNoMemberCooperatedTime')) });
            //敲回车键执行方法
            enterSearch(crmCustCheckHelper.search);

            BindProvince('<%=ddlProvince.ClientID %>'); //绑定省份
            initChkPending();
            //crmCustCheckHelper.search();
        });
        function initChkPending() {//初始化‘处理中’checkbox事件
            var obj = $('#txtSearchBeginMemberCooperatedTime,#txtSearchEndMemberCooperatedTime,:checkbox[id^="ckxMemberCooperateStatus"],#txtSearchBeginNoMemberCooperatedTime,#txtSearchEndNoMemberCooperatedTime,#chkNoMemberTimeCooperated');
            obj.attr('disabled', 'disabled');
            //初始化‘有排期’选中逻辑
            $('#chkMemberCooperated').change(function () {
                if (this.checked) {
                    $('#chkNoMemberTimeCooperated').attr('checked', 'checked');
                    obj.removeAttr('disabled');
                    $('#chkNoMemberCooperated').removeAttr('checked');
                }
                else {
                    obj.attr('disabled', 'disabled');
                }
            });
            //初始化‘无排期’选中逻辑
            $('#chkNoMemberCooperated').change(function () {
                if (this.checked) {
                    $('#chkMemberCooperated').removeAttr('checked');
                    obj.attr('disabled', 'disabled');
                }
            });
            //初始化‘无排期时间段’选中逻辑
            $('#chkNoMemberTimeCooperated').change(function () {
                var txtObj = $('#txtSearchBeginNoMemberCooperatedTime,#txtSearchEndNoMemberCooperatedTime');
                if (this.checked) {
                    txtObj.removeAttr('disabled');
                }
                else {
                    txtObj.attr('disabled', 'disabled');
                }
            });
        };
        function openAddCustPage() {
            document.domain = "<%=SysUrl %>";
            try {
                var dd = window.external.MethodScript('/browsercontrol/newpage?url=http://<%=WpUrl %>/CustomerManager/AddCustomer.aspx?SourceUrl=cc');
            }
            catch (e) {
                window.open('http://<%=WpUrl %>/CustomerManager/AddCustomer.aspx?SourceUrl=cc', '', 'height=900,width=1050,left=200,toolbar=no,menubar=no,scrollbars=yes,resizable=no,location=no,status=no');
            }
        }
    </script>
    <div class="searchTj clearfix" style="width: 100%;">
        <ul>
            <li style="clear: both;">
                <label>
                    客户ID：</label>
                <input type="text" id="tfCustID" name="CustID" class="w190" />
            </li>
            <li>
                <label>
                    客户名称：</label>
                <input type="text" class="w190" name="txtCustName" id="txtCustName" />
            </li>
            <li>
                <label>
                    客户简称：</label>
                <input type="text" class="w190" name="txtCustAbbr" id="txtCustAbbr" />
            </li>
        </ul>
        <ul>
            <li style="clear: both;">
                <label>
                    主营品牌：</label><span>
                        <input type="text" class="w190" name="tfBrandName" id="tfBrandName" readonly="readonly"
                            onclick="javascript:crmCustCheckHelper.openSelectBrandPopup();" />
                    </span>
                <input type="hidden" id="tfBrand" name="tfBrand" />
            </li>
            <li style="display: none;">
                <label>
                    所属交易市场：</label>
                <input id="tfCustPidName" name="CustPidName" readonly="readonly" class="w190" runat="server"
                    onclick="javascript:crmCustCheckHelper.openSelectCustomerPopup(1);" />
                <span>
                    <input id="tfCustPid" name="CustPid" style="display: none;" runat="server" />
                </span></li>
            <li>
                <label>
                    客户地区：</label>
                <select id="ddlProvince" name="ddlProvince" style="width: 66px" class="w50" onchange="crmCustCheckHelper.triggerProvince();"
                    runat="server">
                </select>
                <select id="ddlCity" name="ddlCity" class="w50" onchange="crmCustCheckHelper.triggerCity();"
                    runat="server">
                    <option value="-1">城市</option>
                </select>
                <select id="ddlCounty" name="ddlCounty" class="w50" runat="server">
                    <option value="-1">区/县</option>
                </select>
            </li>
            <li>
                <label>
                    创建日期：</label>
                <input type="text" onclick="MyCalendar.SetDate(this,document.getElementById('txtCreateCustStartDate'))"
                    style="width: 83px; *width: 83px; width: 83px\9;" class="w85" id="txtCreateCustStartDate"
                    name="txtCreateCustStartDate" />
                至
                <input type="text" onclick="MyCalendar.SetDate(this,document.getElementById('txtCreateCustEndDate'))"
                    style="width: 83px; *width: 83px; width: 83px\9;" class="w85" id="txtCreateCustEndDate"
                    name="txtCreateCustEndDate" />
            </li>
        </ul>
        <ul>
            <li style="clear: both; display: none;">
                <label>
                    注册地址：</label>
                <input type="text" id="txtAddress" class="w190" name="txtAddress" />
            </li>
            <li style="display: none;">
                <label>
                    数据来源：</label>
                <input id="chkSourceExcel" type="checkbox" value="1" /><em onclick="emChkIsChoose(this)">CC创建</em>
                <input id="chkSourceCrm" type="checkbox" value="0" /><em onclick="emChkIsChoose(this)">Crm客户</em>
            </li>
        </ul>
        <ul>
            <li style="clear: both;">
                <%--<label>
                    业务区划：</label>
                <input name="AreaType" value='1' type="checkbox" /><em onclick="emChkIsChoose(this)">163城区</em>
                <input name="AreaType" value='2' type="checkbox" /><em onclick="emChkIsChoose(this)">163郊区</em>
                <input type="checkbox" name="AreaType" value='3' /><em onclick="emChkIsChoose(this)">178无人城</em></li>--%>
                <label>
                    分属大区：</label>
                <select id="ddlDistrict" name="ddlDistrict" class="w195" style="width: 194px;" runat="server" />
            </li>
            <li>
                <label>
                    客户状态：</label>
                <input id="chkStatusUseing" style="border: none;" value='0' type="checkbox" /><em
                    onclick="emChkIsChoose(this)">在用</em>
                <input id="chkStatusNoUse" style="border: none;" value='1' type="checkbox" /><em
                    onclick="emChkIsChoose(this)">停用</em>
                <input type="checkbox" style="border: none;" id="chkLockNo" value='0' /><em onclick="emChkIsChoose(this)">未锁定</em>
                <input type="checkbox" style="border: none;" id="chkLockYes" value='1' /><em onclick="emChkIsChoose(this)">锁定</em>
            </li>
            <li>
                <label>
                    经营范围：</label>
                <input id="chkTypeNew" style="border: none;" name='carType' type="checkbox" value="1" /><em
                    onclick="emChkIsChoose(this)">新车</em>&nbsp;&nbsp;&nbsp;
                <input id="chkTypeNewSnd" style="border: none;" type="checkbox" name='carType' value="3" /><em
                    onclick="emChkIsChoose(this)">新车/二手车</em>
                <input id="chkTypeSnd" style="border: none;" type="checkbox" name='carType' value="2" /><em
                    onclick="emChkIsChoose(this)">二手车</em> </li>
        </ul>
        <ul>
            <li style="clear: both; margin-right: 0; width: 790px">
                <label>
                    客户类别：</label>
                <%--<input id="chkCompany" name='clientType' type="checkbox" value="20001" />厂商
                <input id="chkBloc" type="checkbox" name='clientType' value="20002" />集团--%>
                <input id="chkFourS" type="checkbox" style="border: none;" name='clientType' value="20003" /><em
                    onclick="emChkIsChoose(this)">4s</em>
                <input id="chkLicence" type="checkbox" style="border: none;" name='clientType' value="20004" /><em
                    onclick="emChkIsChoose(this)">特许经销商</em>
                <input id="chkSynthesizedShop" style="border: none;" name='clientType' type="checkbox"
                    value="20005" /><em onclick="emChkIsChoose(this)">综合店</em>
                <input id="chkHall" name='clientType' style="border: none;" type="checkbox" value="20009" /><em
                    onclick="emChkIsChoose(this)">展厅</em>
                <input id="chkSP" type="checkbox" style="border: none;" name='clientType' value="20007" /><em
                    onclick="emChkIsChoose(this)">汽车服务商</em>
                <input id="chkBrokerageFirm" style="border: none;" type="checkbox" name='clientType'
                    value="20011" /><em onclick="emChkIsChoose(this)">经纪公司</em>
                <input id="chkPersonal" type="checkbox" style="border: none;" name='clientType' value="20010" /><em
                    onclick="emChkIsChoose(this)">个人</em>
                <input id="chkTradingMarket" style="border: none;" type="checkbox" name='clientType'
                    value="20012" /><em onclick="emChkIsChoose(this)">交易市场</em>
                <input id="chkOther" name='clientType' style="border: none;" type="checkbox" value="20006" /><em
                    onclick="emChkIsChoose(this)">其它</em>
                <input id="chkEasyCar" type="checkbox" style="border: none;" name='clientType' value="20013" /><em
                    onclick="emChkIsChoose(this)">车易达</em>
                <%--<input id="chkSndCar" name='clientType' style="border: none;" type="checkbox" value="20014" /><em onclick="emChkIsChoose(this)">二手车中心</em>--%>
                <input id="chkSndCar" name='clientType' style="border: none;" type="checkbox" value="20015" /><em
                    onclick="emChkIsChoose(this)">中介</em> </li>
        </ul>
        <ul>
            <li style="margin-right: 27px; *margin-right: 28px; display: none">
                <label>
                    合作状态：</label>
                <input id="chkMemberCooperated" type="checkbox" value="1" /><em onclick="emChkIsChoose(this)">有排期</em>
                <span style="margin-left: 8px;">
                    <input type="text" onclick="MyCalendar.SetDate(this,document.getElementById('txtSearchBeginMemberCooperatedTime'))"
                        class="w165" maxlength="10" id="txtSearchBeginMemberCooperatedTime" name="txtSearchBeginMemberCooperatedTime" /></span>---<span>
                            <input type="text" onclick="MyCalendar.SetDate(this,document.getElementById('txtSearchEndMemberCooperatedTime'))"
                                maxlength="10" class="w165" id="txtSearchEndMemberCooperatedTime" name="txtSearchEndMemberCooperatedTime" /></span>
                <input id="ckxMemberCooperateStatus1" type="checkbox" value="1" /><em onclick="emChkIsChoose(this)">销售</em>
                <input id="ckxMemberCooperateStatus2" type="checkbox" value="2" /><em onclick="emChkIsChoose(this)">试用</em>
            </li>
            <li>
                <label>
                    有无会员：
                </label>
                <input type="checkbox" style="border: none;" value="0" id="chxIsHaveMember" name="chxIsHaveMember" /><em
                    onclick="emChkIsChoose(this)">有会员</em>
                <input type="checkbox" style="border: none;" value="0" id="chxIsHaveNoMember" name="chxIsHaveNoMember" /><em
                    onclick="emChkIsChoose(this)">无会员</em> </li>
            <li style="clear: both; width: 615px; display: none"><span style="margin-left: 158px;">
                <input id="chkNoMemberTimeCooperated" type="checkbox" value="0" /><em onclick="emChkIsChoose(this)">无排期时间段</em></span>
                <span>
                    <input type="text" class="w165" maxlength="10" id="txtSearchBeginNoMemberCooperatedTime"
                        name="txtSearchBeginNoMemberCooperatedTime" /></span><span>---</span> <span>
                            <input type="text" maxlength="10" class="w165" id="txtSearchEndNoMemberCooperatedTime"
                                name="txtSearchEndNoMemberCooperatedTime" /></span></li>
            <%--            <li style="width: 280px;">
                <label style="width: 75px;">
                    会员状态：
                </label>
                <input type="checkbox" value="0" id="chkNotOpen" name="chkIsOpen" /><em onclick="emChkIsChoose(this)">未开通</em>
                <input type="checkbox" value="1" id="chkOpen" name="chkIsOpen" /><em onclick="emChkIsChoose(this)">已开通</em>
                <input type="checkbox" value="2" id="chkCallBack" name="chkIsOpen" /><em onclick="emChkIsChoose(this)">已打回</em>
            </li>--%>
            <li style="clear: both; width: 500px; display: none"><span style="margin-left: 90px;">
                <input id="chkNoMemberCooperated" type="checkbox" value="0" /><em onclick="emChkIsChoose(this)">无排期</em></span>
            </li>
            <li class="btnsearch" style="clear: none; margin-top: 5px; width: 100px;">
                <input class="cx" name="" type="button" value="查 询" onclick="javascript:crmCustCheckHelper.search()" />
            </li>
        </ul>
    </div>
    <div id="divQueryResultContent" class="bit_table">
        <div class="optionBtn  clearfix" style="width: 97%">
            <div>
                <%if (BitAuto.ISDC.CC2012.BLL.Util.CheckButtonRight("SYS024BUT2210"))
                  { %>
                <input name="" type="button" value="导出" id="lbtnContactExport" onclick="" class="newBtn mr10" />
                <%} %>
                <%if (BitAuto.ISDC.CC2012.BLL.Util.CheckButtonRight("SYS024BUT2209"))
                  { %>
                <input name="" type="button" value="新增客户" onclick="openAddCustPage()" class="newBtn mr10" />
                <%} %>
                <span>查询结果</span><small><span>总计：0</span></small>
            </div>
        </div>
    </div>
</asp:Content>
