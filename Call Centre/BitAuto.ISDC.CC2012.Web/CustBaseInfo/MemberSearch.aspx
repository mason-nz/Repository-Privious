<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/Controls/Top.Master"
    Title="易湃会员查询" CodeBehind="MemberSearch.aspx.cs" Inherits="BitAuto.ISDC.CC2012.Web.CustBaseInfo.MemberSearch" %>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <script src="../Js/Enum/Area2.js" type="text/javascript"></script>
    <script type="text/javascript">
        $(function () {
            //初始化日历控件，前面的日期不能大于后面的日期
            $('#txtMemberCreateTimeStart').bind('click focus', function () { WdatePicker({ maxDate: '#F{$dp.$D(\'txtMemberCreateTimeEnd\')}', onpicked: function () { document.getElementById("txtMemberCreateTimeEnd").focus(); } }); });
            $('#txtMemberCreateTimeEnd').bind('click focus', function () { WdatePicker({ minDate: '#F{$dp.$D(\'txtMemberCreateTimeStart\')}' }); });

            $('#txtSearchStartMemberCooperatedBeginTime').bind('click focus', function () { WdatePicker({ maxDate: '#F{$dp.$D(\'txtSearchEndMemberCooperatedBeginTime\')}', onpicked: function () { document.getElementById("txtSearchEndMemberCooperatedBeginTime").focus(); } }); });
            $('#txtSearchEndMemberCooperatedBeginTime').bind('click focus', function () { WdatePicker({ minDate: '#F{$dp.$D(\'txtSearchStartMemberCooperatedBeginTime\')}' }); });

            $('#txtConfirmDateStart').bind('click focus', function () { WdatePicker({ maxDate: '#F{$dp.$D(\'txtConfirmDateEnd\')}', onpicked: function () { document.getElementById("txtConfirmDateEnd").focus(); } }); });
            $('#txtConfirmDateEnd').bind('click focus', function () { WdatePicker({ minDate: '#F{$dp.$D(\'txtConfirmDateStart\')}' }); });

            $('#txtSearchBeginMemberCooperatedTime').bind('click focus', function () { WdatePicker({ maxDate: '#F{$dp.$D(\'txtSearchEndMemberCooperatedTime\')}', onpicked: function () { document.getElementById("txtSearchEndMemberCooperatedTime").focus(); } }); });
            $('#txtSearchEndMemberCooperatedTime').bind('click focus', function () { WdatePicker({ minDate: '#F{$dp.$D(\'txtSearchBeginMemberCooperatedTime\')}' }); });

            $('#txtSearchBeginNoMemberCooperatedTime').bind('click focus', function () { WdatePicker({ maxDate: '#F{$dp.$D(\'txtSearchEndNoMemberCooperatedTime\')}', onpicked: function () { document.getElementById("txtSearchEndNoMemberCooperatedTime").focus(); } }); });
            $('#txtSearchEndNoMemberCooperatedTime').bind('click focus', function () { WdatePicker({ minDate: '#F{$dp.$D(\'txtSearchBeginNoMemberCooperatedTime\')}' }); });

            $('#txtReturnVisitTimeStart').bind('click focus', function () { WdatePicker({ maxDate: '#F{$dp.$D(\'txtReturnVisitTimeEnd\')}', onpicked: function () { document.getElementById("txtReturnVisitTimeEnd").focus(); } }); });
            $('#txtReturnVisitTimeEnd').bind('click focus', function () { WdatePicker({ minDate: '#F{$dp.$D(\'txtReturnVisitTimeStart\')}' }); });

            //敲回车键执行方法
            enterSearch(CRMDMSMemberSearchHelper.search);

            BindProvince('<%=selProvince.ClientID %>'); //绑定省份
            initChkPending();
            ///CRMDMSMemberSearchHelper.search();
            rdoReturnMagazineChange();
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

        function rdoReturnMagazineChange() {
            if ($("#chkReturnMagazineNo").attr("checked")) {
                $("#chkReturnMagazineYes").attr("checked", false);
                $("#[id$='sltExecCycle']").css("display", "none");

            }
            else if ($("#chkReturnMagazineYes").attr("checked")) {
                $("#chkReturnMagazineNo").attr("checked", false)
                $("#[id$='sltExecCycle']").css("display", "");
            }
        }
    </script>
    <script type="text/javascript">
        var CRMDMSMemberSearchHelper = (function () {
            var triggerProvince = function () {//选中省份
                BindCity('<%=selProvince.ClientID %>', '<%=selCity.ClientID %>');
                BindCounty('<%=selProvince.ClientID %>', '<%=selCity.ClientID %>', '<%=selCounty.ClientID %>');
            },

            triggerCity = function () {//选中城市
                BindCounty('<%=selProvince.ClientID %>', '<%=selCity.ClientID %>', '<%=selCounty.ClientID %>');
            },

            initProvince = function () {
                $('#<%=selProvince.ClientID %>').val('-1');
                this.triggerProvince();
            },

            initAreaType = function () {
                $('#ckx86Area,#ckx246Area').removeAttr('checked');
                $('#ddl86Area').val('-1');
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
                            $('#<%=tfBrandName.ClientID %>').val(brandnames);
                        }
                    },
                    afterClose: function () {
                        //敲回车键执行方法
                        enterSearch(CRMDMSMemberSearchHelper.search);
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
                    '/AjaxServers/Statistics/MemberList.aspx',
                    $.extend({}, pagerConfig, defaultPostData, postData), function () {
                        var msg = '';
                        //--------生成查询参数并进行校验--------
                        var txtDMSMemberCode = $.trim($('#txtDMSMemberCode').val());
                        var txtDMSMemberName = $.trim($('#txtDMSMemberName').val());
                        var provinceID = $.trim($('#<%=selProvince.ClientID %>').val());
                        var cityID = $.trim($('#<%=selCity.ClientID %>').val());
                        var countyID = $.trim($('#<%=selCounty.ClientID %>').val());
                        var brandIDs = $.trim($('#tfBrand').val());
                        var AddMemberFlag = $(":checkbox[id$='AddMember'][checked=true]").map(function () { return $(this).val(); }).get().join(",");
                        var UserMappingFlag = $(":checkbox[id$='CCUserMapping'][checked=true]").map(function () { return $(this).val(); }).get().join(",");
                        var ReturnVisitFlag = $(":checkbox[id$='CCReturnVisit'][checked=true]").map(function () { return $(this).val(); }).get().join(",");
                        //var AreaType86_Value = $('#ckx86Area:checked').val() == undefined ? '' : $('#ckx86Area:checked').val();
                        //var AreaValue86 = $('#ddl86Area').val();
                        //var AreaType246_Value = $('#ckx246Area:checked').val() == undefined ? '' : $('#ckx246Area:checked').val();
                        //行政区划
                        //                var areaTypeID = $(":checkbox[name^='AreaType']:checked").map(function () {
                        //                    return $(this).val();
                        //                }).get().join(",");
                        var strDeptS = "";
                        $('input[name=DeptX]:checked').each(function () {
                            strDeptS += "," + $(this).val();
                        });
                        if (strDeptS.length > 1) {
                            strDeptS = strDeptS.substr(1);
                        }
                        var selectDeptID = $('#deptSelect>select').val();

                        var MemberCooperateStatus = $('#ddlMemberCooperateStatus').val();
                        var txtMemberCreateTimeStart = $('#txtMemberCreateTimeStart').val();
                        var txtMemberCreateTimeEnd = $('#txtMemberCreateTimeEnd').val();
                        var txtReturnVisitTimeStart = $('#txtReturnVisitTimeStart').val();
                        var txtReturnVisitTimeEnd = $('#txtReturnVisitTimeEnd').val();

                        //排期确认时间add by qizq 2012-8-2
                        var txtConfirmDateStart = $('#txtConfirmDateStart').val();
                        var txtConfirmDateEnd = $('#txtConfirmDateEnd').val();

                        //var AreaValue = $(":checkbox[id='ckx86Area'][checked=true],:checkbox[id='ckx246Area'][checked=true]").map(function() { return $(this).val(); }).get().join(",");
                        //if ($("#ddlArea").val() != '-1')
                        //{ AreaValue += ',' + $("#ddlArea").val(); }
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
                        //会员类型
                        var memberTypeIDs = $(":checkbox[name='MemberType']:checked").map(function () {
                            return $(this).val();
                        }).get().join(",");
                        //是否投递杂志
                        var isReturnMagazine = $(":checkbox[id^='chkReturnMagazine']:checked").map(function () {
                            return $(this).val();
                        }).get().join(",");
                        //杂志期数
                        var execCycle = $("#<%=sltExecCycle.ClientID %>").val();
                        //alert(execCycle);
                        var contactOfficeTypeCode = $('#txtOfficeTypeCode').val();
                        //有排期开始时间段（开始）
                        var txtSearchStartMemberCooperatedBeginTime = $.trim($('#txtSearchStartMemberCooperatedBeginTime').val());
                        //有排期开始时间段（结束）
                        var txtSearchEndMemberCooperatedBeginTime = $.trim($('#txtSearchEndMemberCooperatedBeginTime').val());
                        var memberSyncStatus = $("#sltMemberStatus").val();

                        if (verify()) {
                            var newparams = {
                                DMSMemberCode: txtDMSMemberCode,
                                DMSMemberName: txtDMSMemberName,
                                ProvinceID: provinceID,
                                CityID: cityID,
                                CountyID: countyID,
                                selectDeptID: selectDeptID,
                                strDeptS: strDeptS,
                                AddMemberFlag: AddMemberFlag,
                                UserMappingFlag: UserMappingFlag,
                                ReturnVisitFlag: ReturnVisitFlag,
                                MemberCooperateStatus: MemberCooperateStatus,
                                MemberCreateTimeStart: txtMemberCreateTimeStart,
                                MemberCreateTimeEnd: txtMemberCreateTimeEnd,
                                ReturnVisitTimeStart: txtReturnVisitTimeStart,
                                ReturnVisitTimeEnd: txtReturnVisitTimeEnd,
                                //排期确认时间add by qizq 2012-8-2
                                ConfirmDateStart: txtConfirmDateStart,
                                ConfirmDateEnd: txtConfirmDateEnd,

                                BrandIDs: brandIDs,
                                CooperatedStatusIDs: cooperatedStatusIDs,
                                BeginMemberCooperatedTime: txtSearchBeginMemberCooperatedTime,
                                EndMemberCooperatedTime: txtSearchEndMemberCooperatedTime,
                                BeginNoMemberCooperatedTime: txtSearchBeginNoMemberCooperatedTime,
                                EndNoMemberCooperatedTime: txtSearchEndNoMemberCooperatedTime,
                                MemberCooperateStatus: ckxMemberCooperateStatus,
                                MemberTypeIDs: memberTypeIDs,
                                //add by qizhiqiang 2012-4-13 为了判断调用查询是统计还是客户核实
                                ComeExcel: '1',
                                //add by masj 2013-06-28
                                ContactOfficeTypeCode: contactOfficeTypeCode,
                                IsReturnMagazine: isReturnMagazine,
                                ExecCycle: execCycle,
                                StartMemberCooperatedBeginTime: txtSearchStartMemberCooperatedBeginTime,
                                EndMemberCooperatedBeginTime: txtSearchEndMemberCooperatedBeginTime,
                                MemberSyncStatus: memberSyncStatus
                            };
                            var strParam = JSON.stringify(newparams);
                            var reg = new RegExp(":", "g");
                            var reg1 = new RegExp(",", "g");
                            var reg2 = new RegExp("\"", "g");
                            strParams = strParam.replace(reg, "=");
                            strParams = strParams.replace(reg1, "&");
                            strParams = strParams.replace(reg2, "");
                            strParams = strParams.substring(1, strParams.length - 1);
                            StatAjaxPageTime(monitorPageTime, '/AjaxServers/Statistics/MemberList.aspx' + "?" + strParams);
                        }
                    });
            },

            search = function () {
                var params = {};
                var msg = '';
                //--------生成查询参数并进行校验--------
                var txtDMSMemberCode = $.trim($('#txtDMSMemberCode').val());
                var txtDMSMemberName = $.trim($('#txtDMSMemberName').val());
                var provinceID = $.trim($('#<%=selProvince.ClientID %>').val());
                var cityID = $.trim($('#<%=selCity.ClientID %>').val());
                var countyID = $.trim($('#<%=selCounty.ClientID %>').val());
                var brandIDs = $.trim($('#tfBrand').val());
                var AddMemberFlag = $(":checkbox[id$='AddMember'][checked=true]").map(function () { return $(this).val(); }).get().join(",");
                var UserMappingFlag = $(":checkbox[id$='CCUserMapping'][checked=true]").map(function () { return $(this).val(); }).get().join(",");
                var ReturnVisitFlag = $(":checkbox[id$='CCReturnVisit'][checked=true]").map(function () { return $(this).val(); }).get().join(",");
                //var AreaType86_Value = $('#ckx86Area:checked').val() == undefined ? '' : $('#ckx86Area:checked').val();
                //var AreaValue86 = $('#ddl86Area').val();
                //var AreaType246_Value = $('#ckx246Area:checked').val() == undefined ? '' : $('#ckx246Area:checked').val();
                //行政区划
                //                var areaTypeID = $(":checkbox[name^='AreaType']:checked").map(function () {
                //                    return $(this).val();
                //                }).get().join(",");
                var strDeptS = "";
                $('input[name=DeptX]:checked').each(function () {
                    strDeptS += "," + $(this).val();
                }
                );
                if (strDeptS.length > 1) {
                    strDeptS = strDeptS.substr(1);
                }
                var selectDeptID = $('#deptSelect>select').val();

                var MemberCooperateStatus = $('#ddlMemberCooperateStatus').val();
                var txtMemberCreateTimeStart = $('#txtMemberCreateTimeStart').val();
                var txtMemberCreateTimeEnd = $('#txtMemberCreateTimeEnd').val();
                var txtReturnVisitTimeStart = $('#txtReturnVisitTimeStart').val();
                var txtReturnVisitTimeEnd = $('#txtReturnVisitTimeEnd').val();

                //排期确认时间add by qizq 2012-8-2
                var txtConfirmDateStart = $('#txtConfirmDateStart').val();
                var txtConfirmDateEnd = $('#txtConfirmDateEnd').val();

                //var AreaValue = $(":checkbox[id='ckx86Area'][checked=true],:checkbox[id='ckx246Area'][checked=true]").map(function() { return $(this).val(); }).get().join(",");
                //if ($("#ddlArea").val() != '-1')
                //{ AreaValue += ',' + $("#ddlArea").val(); }
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
                //会员类型
                var memberTypeIDs = $(":checkbox[name='MemberType']:checked").map(function () {
                    return $(this).val();
                }).get().join(",");
                //是否投递杂志
                var isReturnMagazine = $(":checkbox[id^='chkReturnMagazine']:checked").map(function () {
                    return $(this).val();
                }).get().join(",");
                //杂志期数
                var execCycle = $("#<%=sltExecCycle.ClientID %>").val();
                //alert(execCycle);
                var contactOfficeTypeCode = $('#txtOfficeTypeCode').val();
                //有排期开始时间段（开始）
                var txtSearchStartMemberCooperatedBeginTime = $.trim($('#txtSearchStartMemberCooperatedBeginTime').val());
                //有排期开始时间段（结束）
                var txtSearchEndMemberCooperatedBeginTime = $.trim($('#txtSearchEndMemberCooperatedBeginTime').val());
                var memberSyncStatus = $("#sltMemberStatus").val();

                if (verify()) {
                    params = {
                        DMSMemberCode: encodeURIComponent(txtDMSMemberCode),
                        DMSMemberName: encodeURIComponent(txtDMSMemberName),
                        ProvinceID: encodeURIComponent(provinceID),
                        CityID: encodeURIComponent(cityID),
                        CountyID: encodeURIComponent(countyID),
                        selectDeptID: encodeURIComponent(selectDeptID),
                        strDeptS: encodeURIComponent(strDeptS),
                        AddMemberFlag: encodeURIComponent(AddMemberFlag),
                        UserMappingFlag: encodeURIComponent(UserMappingFlag),
                        ReturnVisitFlag: encodeURIComponent(ReturnVisitFlag),
                        //AreaType86_Value: encodeURIComponent(AreaType86_Value),
                        //AreaValue86: encodeURIComponent(AreaValue86),
                        //AreaType246_Value: encodeURIComponent(AreaType246_Value),
                        //AreaTypeIDs: encodeURIComponent(areaTypeID),
                        MemberCooperateStatus: encodeURIComponent(MemberCooperateStatus),
                        MemberCreateTimeStart: encodeURIComponent(txtMemberCreateTimeStart),
                        MemberCreateTimeEnd: encodeURIComponent(txtMemberCreateTimeEnd),
                        ReturnVisitTimeStart: encodeURIComponent(txtReturnVisitTimeStart),
                        ReturnVisitTimeEnd: encodeURIComponent(txtReturnVisitTimeEnd),
                        //排期确认时间add by qizq 2012-8-2
                        ConfirmDateStart: encodeURIComponent(txtConfirmDateStart),
                        ConfirmDateEnd: encodeURIComponent(txtConfirmDateEnd),

                        BrandIDs: encodeURIComponent(brandIDs),
                        CooperatedStatusIDs: encodeURIComponent(cooperatedStatusIDs),
                        BeginMemberCooperatedTime: escapeStr(txtSearchBeginMemberCooperatedTime),
                        EndMemberCooperatedTime: escapeStr(txtSearchEndMemberCooperatedTime),
                        BeginNoMemberCooperatedTime: escapeStr(txtSearchBeginNoMemberCooperatedTime),
                        EndNoMemberCooperatedTime: escapeStr(txtSearchEndNoMemberCooperatedTime),
                        MemberCooperateStatus: escapeStr(ckxMemberCooperateStatus),
                        MemberTypeIDs: escapeStr(memberTypeIDs),
                        //add by qizhiqiang 2012-4-13 为了判断调用查询是统计还是客户核实
                        ComeExcel: encodeURIComponent('1'),
                        //add by masj 2013-06-28
                        ContactOfficeTypeCode: contactOfficeTypeCode,
                        IsReturnMagazine: isReturnMagazine,
                        ExecCycle: encodeURIComponent(execCycle),
                        StartMemberCooperatedBeginTime: escapeStr(txtSearchStartMemberCooperatedBeginTime),
                        EndMemberCooperatedBeginTime: escapeStr(txtSearchEndMemberCooperatedBeginTime),
                        MemberSyncStatus: escapeStr(memberSyncStatus)
                    };
                    if (params) {
                        _query(params);
                    }
                }
            },

            verify = function () {
                var txtDMSMemberCode = $('#txtDMSMemberCode').val();
                var txtDMSMemberName = $('#txtDMSMemberName').val();
                var txtMemberCreateTimeStart = $('#txtMemberCreateTimeStart').val();
                var txtMemberCreateTimeEnd = $('#txtMemberCreateTimeEnd').val();
                var txtReturnVisitTimeStart = $('#txtReturnVisitTimeStart').val();
                var txtReturnVisitTimeEnd = $('#txtReturnVisitTimeEnd').val();

                //排期确认时间add by qizq 2012-8-2
                var txtConfirmDateStart = $('#txtConfirmDateStart').val();
                var txtConfirmDateEnd = $('#txtConfirmDateEnd').val();


                //有排期开始时间
                var txtSearchBeginMemberCooperatedTime = $.trim($('#txtSearchBeginMemberCooperatedTime:enabled').val());
                //有排期结束时间
                var txtSearchEndMemberCooperatedTime = $.trim($('#txtSearchEndMemberCooperatedTime:enabled').val());
                //无排期开始时间
                var txtSearchBeginNoMemberCooperatedTime = $.trim($('#txtSearchBeginNoMemberCooperatedTime:enabled').val());
                //无排期结束时间
                var txtSearchEndNoMemberCooperatedTime = $.trim($('#txtSearchEndNoMemberCooperatedTime:enabled').val());
                //有排期开始时间段（开始）
                var txtSearchStartMemberCooperatedBeginTime = $.trim($('#txtSearchStartMemberCooperatedBeginTime').val());
                //有排期开始时间段（结束）
                var txtSearchEndMemberCooperatedBeginTime = $.trim($('#txtSearchEndMemberCooperatedBeginTime').val());

                if (GetStringRealLength(txtDMSMemberCode) > 50) {
                    $.jAlert('会员ID超过50个字符！');
                    return;
                }
                if (GetStringRealLength(txtDMSMemberName) > 256) {
                    $.jAlert('会员名称超过256个字符！');
                    return;
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
                if ($.trim(txtMemberCreateTimeStart).length > 0) {
                    if (!($.trim(txtMemberCreateTimeStart).isDate())) {
                        $.jAlert("您输入的创建会员开始时间格式不正确", function () {
                            $('#txtMemberCreateTimeStart').val('');
                            $('#txtMemberCreateTimeStart').focus();
                        });
                        return;
                    }
                }
                if ($.trim(txtMemberCreateTimeEnd).length > 0) {
                    if (!($.trim(txtMemberCreateTimeEnd).isDate())) {
                        $.jAlert("您输入的创建会员结束时间格式不正确", function () {
                            $('#txtMemberCreateTimeEnd').val('');
                            $('#txtMemberCreateTimeEnd').focus();
                        });
                        return;
                    }
                }
                if ($.trim(txtReturnVisitTimeStart).length > 0) {
                    if (!($.trim(txtReturnVisitTimeStart).isDate())) {
                        $.jAlert("您输入的回访记录开始时间格式不正确", function () {
                            $('#txtReturnVisitTimeStart').val('');
                            $('#txtReturnVisitTimeStart').focus();
                        });
                        return;
                    }
                }
                if ($.trim(txtReturnVisitTimeEnd).length > 0) {
                    if (!($.trim(txtReturnVisitTimeEnd).isDate())) {
                        $.jAlert("您输入的回访记录结束时间格式不正确", function () {
                            $('#txtReturnVisitTimeEnd').val('');
                            $('#txtReturnVisitTimeEnd').focus();
                        });
                        return;
                    }
                }

                //排期确认时间add by qizq 2012-8-2
                if ($.trim(txtConfirmDateStart).length > 0) {
                    if (!($.trim(txtConfirmDateStart).isDate())) {
                        $.jAlert("您输入的排期确认日期开始时间格式不正确", function () {
                            $('#txtConfirmDateStart').val('');
                            $('#txtConfirmDateStart').focus();
                        });
                        return;
                    }
                }
                if ($.trim(txtConfirmDateEnd).length > 0) {
                    if (!($.trim(txtConfirmDateEnd).isDate())) {
                        $.jAlert("您输入的排期确认日期结束时间格式不正确", function () {
                            $('#txtConfirmDateEnd').val('');
                            $('#txtConfirmDateEnd').focus();
                        });
                        return;
                    }
                }
                if ($.trim(txtSearchStartMemberCooperatedBeginTime).length > 0) {
                    if (!($.trim(txtSearchStartMemberCooperatedBeginTime).isDate())) {
                        $.jAlert("您输入的有排期开始时间段（开始）时间格式不正确", function () {
                            $('#txtSearchStartMemberCooperatedBeginTime').val('');
                            $('#txtSearchStartMemberCooperatedBeginTime').focus();
                        });
                        return;
                    }
                }
                if ($.trim(txtSearchEndMemberCooperatedBeginTime).length > 0) {
                    if (!($.trim(txtSearchEndMemberCooperatedBeginTime).isDate())) {
                        $.jAlert("您输入的有排期开始时间段（结束）时间格式不正确", function () {
                            $('#txtSearchEndMemberCooperatedBeginTime').val('');
                            $('#txtSearchEndMemberCooperatedBeginTime').focus();
                        });
                        return;
                    }
                }
                if (Date.parse(txtSearchEndMemberCooperatedBeginTime.replace(reg, '/')) - Date.parse(txtSearchStartMemberCooperatedBeginTime.replace(reg, '/')) < 0) {
                    $.jAlert("您输入有排期开始时间段（开始）必须小于等于有排期开始时间段（结束）");
                    return false;
                }
                return true;
            };

            return {
                triggerProvince: triggerProvince,
                triggerCity: triggerCity,
                initProvince: initProvince,
                initAreaType: initAreaType,
                openSelectBrandPopup: openSelectBrandPopup,
                search: search
            };
        })();

        //根据导入的会员ID导出会员信息
        function openUploadExcelInfoAjaxPopup() {
            $.openPopupLayer({
                name: "UploadUserAjaxPopup",
                parameters: {},
                url: "../../Statistics/MemberIDImport/Main.aspx?Type=Member"
            });
        }
        //根据导入的会员ID导出客户信息
        function openUploadExcelInfoAjaxPopupForExportCustID() {
            $.openPopupLayer({
                name: "UploadUserAjaxPopup",
                parameters: {},
                url: "../../Statistics/MemberIDImport/Main.aspx?Type=CustIDYP"
            });
        }
        //杂志回访导入
        function openMagazineExcelInfoAjaxPopup() {
            $.openPopupLayer({
                name: "UploadUserAjaxPopup",
                parameters: {},
                url: "/MagazineReturn/MagezineInfoImport/Main.aspx"
            });
        }
    </script>
    <div class="searchTj clearfix">
        <ul>
            <li>
                <label>
                    会员ID：</label>
                <input type="text" name="txtDMSMemberCode" id="txtDMSMemberCode" class="w190" />
            </li>
            <li>
                <label>
                    会员名称：</label>
                <input type="text" id="txtDMSMemberName" name="txtDMSMemberName" class="w190" />
            </li>
            <li>
                <label>
                    会员地区：</label>
                <select id="selProvince" name="Province" style="width: 80px" class="w50" onchange="javascript:CRMDMSMemberSearchHelper.initAreaType();CRMDMSMemberSearchHelper.triggerProvince();"
                    runat="server">
                </select>
                <select id="selCity" name="City" class="w50" onchange="javascript:CRMDMSMemberSearchHelper.initAreaType();CRMDMSMemberSearchHelper.triggerCity();"
                    runat="server">
                    <option value="-1">城市</option>
                </select>
                <select id="selCounty" name="County" class="w50" runat="server">
                    <option value="-1">区/县</option>
                </select>
            </li>
        </ul>
        <ul class="clear">
            <li style="width: 280px">
                <label>
                    主营品牌：</label><span>
                        <input id="tfBrandName" name="BrandName" type="text" readonly="readonly" runat="server"
                            onclick="javascript:CRMDMSMemberSearchHelper.openSelectBrandPopup();" class="w190" />
                        <input id="tfBrand" name="tfBrand" type="hidden" />
                    </span></li>
            <li>
                <label style="width: 90px">
                    排期开始日期：</label>
                <input type="text" name="txtSearchStartMemberCooperatedBeginTime" id="txtSearchStartMemberCooperatedBeginTime"
                    style="width: 83px; *width: 83px; width: 83px\9;" class="w85">
                至
                <input type="text" name="txtSearchEndMemberCooperatedBeginTime" id="txtSearchEndMemberCooperatedBeginTime"
                    style="width: 83px; *width: 83px; width: 83px\9;" class="w85">
            </li>
            <li style="display: none;">
                <label>
                    创建会员：</label>
                <input type="checkbox" value="1" style="border: none;" id="chkCCAddMember" name="AddMember"
                    class="checkbox" />
                <em onclick="emChkIsChoose(this);">呼叫中心创建</em>
                <input type="checkbox" value="0" style="border: none;" id="chkAreaAddMember" name="AddMember"
                    class="checkbox" />
                <em onclick="emChkIsChoose(this);">区域创建</em> </li>
            <li style="margin-left: -2px; margin-left: -2px\9; *margin-left: -3px; display: none;">
                <label>
                    排期确认日期：</label>
                <input type="text" class="w85" style="width: 84px; *width: 83px; width: 83px\9;"
                    id="txtConfirmDateStart" name="txtConfirmDateStart" />
                至
                <input type="text" class="w85" style="width: 84px; *width: 83px; width: 83px\9;"
                    id="txtConfirmDateEnd" name="txtConfirmDateEnd" />
            </li>
            <li id="deptSelect" style="width: 310px">
                <label style="width: 90px">
                    分属大区：</label>
                <select id="area" class="w160" style="width: 208px" runat="server">
                </select>
            </li>
        </ul>
        <ul class="clear">
            <li style="display: none; margin-right: 19px; *margin-right: 20px">
                <label>
                    负责员工：</label>
                <input type="checkbox" value="1" style="border: none;" id="ckxCCUserMapping" name="UserMapping"
                    class="checkbox" />
                <em onclick="emChkIsChoose(this);">呼叫中心负责</em>
                <input type="checkbox" value="0" style="border: none;" id="ckxNoCCUserMapping" name="UserMapping"
                    class="checkbox" />
                <em onclick="emChkIsChoose(this);">非呼叫中心负责</em> </li>
            <li style="display: none; margin-left: 16px; *margin-left: 23px">
                <label>
                    回访部门：</label>
                <input type="checkbox" value="1" style="border: none;" id="ckxCCReturnVisit" name="ReturnVisit"
                    class="checkbox" />
                <em onclick="emChkIsChoose(this);">呼叫中心回访</em>
                <input type="checkbox" value="0" style="border: none;" id="ckxNoCCReturnVisit" name="ReturnVisit"
                    class="checkbox" />
                <em onclick="emChkIsChoose(this);">非呼叫中心回访</em> </li>
            <li style="margin-right: 37px; *margin-right: 38px; display: none;">
                <label>
                    会员创建时间：</label>
                <input type="text" class="w85" style="width: 84px; *width: 83px; width: 83px\9;"
                    id="txtMemberCreateTimeStart" name="txtMemberCreateTimeStart" />
                至
                <input type="text" class="w85" style="width: 84px; *width: 83px; width: 83px\9;"
                    id="txtMemberCreateTimeEnd" name="txtMemberCreateTimeEnd" />
            </li>
        </ul>
        <ul class="clear">
            <li style="margin-right: 16px; *margin-right: 19px; display: none;">
                <label>
                    合作状态：</label>
                <input id="chkMemberCooperated" type="checkbox" value="1" class="checkbox" style="border: none;" />
                <em onclick="emChkIsChoose(this);">有排期</em> <span style="margin-left: 8px;">
                    <input type="text" class="w160" maxlength="10" id="txtSearchBeginMemberCooperatedTime"
                        name="txtSearchBeginMemberCooperatedTime" /><span>---</span>
                    <input type="text" maxlength="10" class="w160" id="txtSearchEndMemberCooperatedTime"
                        name="txtSearchEndMemberCooperatedTime" /></span>
                <input id="ckxMemberCooperateStatus1" type="checkbox" value="1" class="checkbox"
                    style="border: none;" />
                <em onclick="emChkIsChoose(this);">销售</em>
                <input id="ckxMemberCooperateStatus2" type="checkbox" value="2" class="checkbox"
                    style="border: none;" />
                <em onclick="emChkIsChoose(this);">试用</em> </li>
        </ul>
        <ul class="clear">
            <li style="width: 604px; *width: 610px; display: none;"><span style="margin-left: 90px;">
                <input id="chkNoMemberTimeCooperated" type="checkbox" value="0" class="checkbox"
                    style="border: none;" />
                <em onclick="emChkIsChoose(this);">无排期时间段</em></span><span><input type="text" class="w160"
                    maxlength="10" id="txtSearchBeginNoMemberCooperatedTime" name="txtSearchBeginNoMemberCooperatedTime"
                    style="width: 157px; *width: 159px;" /></span><span>---</span> <span>
                        <input type="text" maxlength="10" class="w160" id="txtSearchEndNoMemberCooperatedTime"
                            name="txtSearchEndNoMemberCooperatedTime" style="width: 157px; *width: 159px;" /></span>
            </li>
            <li>
                <label>
                    联系人职级：</label>
                <select id="txtOfficeTypeCode" name="txtOfficeTypeCode" style="width: 194px;">
                    <option value="-1">请选择</option>
                    <option value="160001">总裁（股东/董事长/董事/总裁…）</option>
                    <option value="160002">高管（高层/总经理/副总经理/店长…）</option>
                    <option value="160003">总监（中层/市场总监/销售总监…）</option>
                    <option value="160004">经理（基层/部门经理/主管…）</option>
                    <option value="160005">专员（员工/市场/销售/财务/客服/公关…）</option>
                    <option value="160000">其它</option>
                </select></li>
            <li>
                <label style="*width: 84px;">
                    负责部门：</label>
                <input name="DeptX" value='1' type="checkbox" style="border: none;" class="checkbox" />
                <em onclick="emChkIsChoose(this);">电销</em>
                <input name="DeptX" value='0' type="checkbox" style="border: none;" class="checkbox" />
                <em onclick="emChkIsChoose(this);">非电销</em> </li>
            <li style="width: 290px">
                <label>
                    会员状态：</label>
                <select id="sltMemberStatus" name="txtOfficeTypeCode" style="width: 208px;">
                    <option value="-1">全部</option>
                    <option value="170001">已申请</option>
                    <option value="170002">已开通</option>
                    <option value="170008">已打回</option>
                    <option value="170003">开通失败</option>
                </select></li>
        </ul>
        <ul class="clear">
            <li style="margin-right: 16px; *margin-right: 17px; width: 214px; *width: 621px;
                display: none;"><span style="margin-left: 90px;">
                    <input id="chkNoMemberCooperated" type="checkbox" value="0" class="checkbox" style="border: none;" />
                    <em onclick="emChkIsChoose(this);">无排期</em> </span></li>
            <li style="display: none; width: 365px;">
                <label>
                    回访记录时间：</label>
                <input type="text" class="w85" style="width: 84px; *width: 83px; width: 83px\9;"
                    id="txtReturnVisitTimeStart" name="txtReturnVisitTimeStart" />
                至
                <input type="text" class="w85" style="width: 84px; *width: 83px; width: 83px\9;"
                    id="txtReturnVisitTimeEnd" name="txtReturnVisitTimeEnd" />
            </li>
        </ul>
        <ul class="clear">
            <li style="width: 316px; margin-right: 0px; margin-left: -2px; margin-left: 0px\9;
                *margin-left: 0px; display: none;">
                <label>
                    是否投寄杂志：</label>
                <input type="checkbox" id="chkReturnMagazineYes" value="1" class="checkbox" onclick="rdoReturnMagazineChange(1);" /><em
                    onclick="emChkIsChoose(this);rdoReturnMagazineChange();">是</em>
                <input type="checkbox" id="chkReturnMagazineNo" value="0" class="checkbox" onclick="rdoReturnMagazineChange(0);" /><em
                    onclick="emChkIsChoose(this);rdoReturnMagazineChange();">否</em>
                <select id="sltExecCycle" runat="server" style="width: 107px; margin-left: 15px;
                    display: none">
                </select>
            </li>
        </ul>
        <ul class="clear">
            <li style="width: 700px">
                <label>
                    会员类型：</label>
                <input type="checkbox" value="1" style="border: none;" name="MemberType" class="checkbox" />
                <em onclick="emChkIsChoose(this);">4S</em>
                <input type="checkbox" value="2" style="border: none;" name="MemberType" class="checkbox" />
                <em onclick="emChkIsChoose(this);">特许经销商</em>
                <input type="checkbox" value="3" style="border: none;" name="MemberType" class="checkbox" />
                <em onclick="emChkIsChoose(this);">综合店</em>
                <input type="checkbox" value="6" style="border: none;" name="MemberType" class="checkbox" />
                <em onclick="emChkIsChoose(this);">经纪公司</em>
                <input type="checkbox" value="7" style="border: none;" name="MemberType" class="checkbox" />
                <em onclick="emChkIsChoose(this);">经纪人</em>
                <input type="checkbox" value="4" style="border: none;" name="MemberType" class="checkbox" />
                <em onclick="emChkIsChoose(this);">车易达</em>
                <%--<input type="checkbox" value="5" style="border: none;" name="MemberType" class="checkbox" />
                <em onclick="emChkIsChoose(this);">二手车中心</em>--%> </li>
            <li class="btnsearch" style="clear: none; margin-top: 5px; width: 170px;">
                <input name="" class="cx" type="button" value="查 询" onclick="javascript:CRMDMSMemberSearchHelper.search()" />
            </li>
        </ul>
    </div>
    <div id="divQueryResultContent" class="cont_cxjg">
        <div class="optionBtn  clearfix">
            <div>
                <%if (right_Contact)
                  { %>
                <input name="" type="button" value="联系人导出" id="lbtnContactExport" class="newBtn mr10" />
                <%} %>
                <input name="" type="button" value="客户ID导出" onclick="openUploadExcelInfoAjaxPopupForExportCustID()"
                    class="newBtn mr10" />
                <%if (right_Member)
                  { %>
                <input name="" type="button" value="合作会员导出" onclick="openUploadExcelInfoAjaxPopup()"
                    class="newBtn mr10" />
                <%} %>
                <%if (right_Magazine)
                  { %>
                <input name="" type="button" value="杂志回访导入" onclick="openMagazineExcelInfoAjaxPopup()"
                    class="newBtn mr10" />
                <%} %>
                <span>查询结果 </span><small><span>总计: 0</span></small>
            </div>
        </div>
    </div>
</asp:Content>
