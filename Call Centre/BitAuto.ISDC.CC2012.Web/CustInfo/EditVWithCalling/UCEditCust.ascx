<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="UCEditCust.ascx.cs"
    Inherits="BitAuto.ISDC.CC2012.Web.CustInfo.EditVWithCalling.UCEditCust" %>
<!--功能废弃 强斐 2016-8-3-->
<%@ Register Src="~/CustInfo/DetailV/SurveyList.ascx" TagName="SurveyList" TagPrefix="uc1" %>
<script type="text/javascript">
    function custNameIsExist(name) {
        $.getJSON('/CustInfo/Handler.ashx?callback=?', {
            Action: 'CustNameExist',
            CustName: encodeURIComponent($.trim(name || '')),
            CustID: '<%= this.OriginalCustID %>'
        }, function (jd, textStatus, xhr) {
            if (textStatus != 'success') { $.jAlert('请求错误'); return; }
            if (jd.success) {
                if (jd.result == '1') {
                    $.jAlert('CRM库中已存在此客户名称<br>' + jd.message, function () {
                        return;
                    }
                    );
                }
            }
            else {
                $.jAlert('错误：' + jd.message, function () { return; });
            }
        });
    }

    var uCEditCustHelper = (function () {
        var triggerProvince = function () {//选中省份
            BindCity('<%= this.selProvince.ClientID %>', '<%= this.selCity.ClientID %>');
            BindCounty('<%= this.selProvince.ClientID %>', '<%= this.selCity.ClientID %>', '<%= this.selCounty.ClientID %>');
        },

        triggerCity = function () {//选中城市
            BindCounty('<%= this.selProvince.ClientID %>', '<%= this.selCity.ClientID %>', '<%= this.selCounty.ClientID %>');
            //若城市列表中，没有数据，则添加属性noCounty，值为1，否则不添加属性
            if ($('#<%= this.selCounty.ClientID %> option').size() == 1)
            { $('#<%= this.selCounty.ClientID %>').attr('noCounty', '1'); }
            else
            { $('#<%= this.selCounty.ClientID %>').removeAttr('noCounty'); }
        },


        openSelectCustomerPopup1 = function () {//选择交易市场 
            $.openPopupLayer({
                name: 'SelectCustomerPopup',
                parameters: { type: 3, ProvinceID: $('select[id$="selProvince"]', $('.CustInfoArea')).val() },
                url: "/AjaxServers/Base/SelectCustomer.aspx?page=1",
                beforeClose: function (e, data) {
                    if (e) {
                        $('#<%= this.tfCustPidName1.ClientID %>').val(data.CustName);
                        $('#<%= this.tfCustPid1.ClientID %>').val(data.CustID);
                    }
                }
            });
        },

       loadTaskLog = function () {//任务操作日志
           $('#divTaskLog').load('/CustInfo/DetailV/TaskLogList.aspx', {
               ContentElementId: 'divCustContacts',
               TID: '<%= this.TaskID %>',
               PageSize: 10
           });
       },
     loadCallRecord = function () {//通话记录
         $('#divCallRecordList').load('/CustInfo/DetailV/TaskCallRecordList.aspx', {
             ContentElementId: 'divCustContacts',
             TID: '<%= this.TaskID %>',
             PageSize: 10
         });
     },
            custNameExist = function (name) {
                $.getJSON('/CustInfo/Handler.ashx?callback=?', {
                    Action: 'CustNameExist',
                    CustName: encodeURIComponent($.trim(name || '')),
                    CustID: '<%= this.OriginalCustID %>'
                }, function (jd, textStatus, xhr) {
                    if (textStatus != 'success') { $.jAlert('请求错误'); return; }
                    if (jd.success) {
                        var msg = jd.result == '1' ? 'CRM库中已存在此客户名称' : 'CRM库中不存在此客户名称';
                        $.jAlert(msg);
                    }
                    else {
                        $.jAlert('错误：' + jd.message);
                    }
                });
            },
        openSelectCustomerPopup = function (type) {//选择厂商或集团
            var body = { type: type };
            if ($('#<%=selCustType.ClientID %>').val() == '<%=(int)BitAuto.YanFa.Crm2009.Entities.EnumCustomType.Licence %>') {
                body.CustType = '<%=(int)BitAuto.YanFa.Crm2009.Entities.EnumCustomType.FourS %>';
            }
            $.openPopupLayer({
                name: 'SelectCustomerPopup',
                parameters: body,
                url: "/AjaxServers/Base/SelectCustomer.aspx?page=1",
                beforeClose: function (e, data) {
                    if (e) {
                        //所属厂商
                        if (type == 1) {
                            $('#<%= this.tfCustPidName.ClientID %>').val(data.CustName);
                            $('#<%= this.tfCustPid.ClientID %>').val(data.CustID);
                        }
                        else {
                            //所属4S
                            if (type == 4) {
                                $('#divMembers').find('.MemberInfoArea').each(function (i, v) {
                                    memberTypeControl($(v), "selCustType", "selMemberType", true);
                                });
                                $('#<%= this.tf4sName.ClientID %>').val(data.CustName);
                                $('#<%= this.tf4sPid.ClientID %>').val(data.CustID);

                                $('#<%= this.tfPidName.ClientID %>').val(data.PidName);
                                $('#<%= this.tfPid.ClientID %>').val(data.Pid);

                                $("span[name='spanImg']").css("display", "none");
                            }
                            else {
                                $('#<%= this.tfPidName.ClientID %>').val(data.CustName);
                                $('#<%= this.tfPid.ClientID %>').val(data.CustID);
                            }
                            if ($('#<%=selCustType.ClientID %>').val() == '<%=(int)BitAuto.YanFa.Crm2009.Entities.EnumCustomType.Licence %>') {
                                $('#<%=this.tfCustPidName.ClientID %>').val(data.CustPidName); //所属厂商
                                $('#<%=this.tfCustPid.ClientID %>').val(data.CustPid);
                                $('#<%=this.tfBrandName.ClientID %>').val(data.BrandNames); //主营品牌
                                $('#<%=this.tfBrand.ClientID %>').val(data.BrandIDs);
                            }
                        }

                    }
                }
            });
        },
        openSelectBrandPopup = function () {//选择主营品牌
            $.openPopupLayer({
                name: "BrandSelectAjaxPopup",
                parameters: {},
                url: "/AjaxServers/Base/SelectBrand.aspx?BrandIDs=" + $('#<%= this.tfBrand.ClientID %>').val(),
                beforeClose: function (e, data) {
                    if (e) {
                        $('#<%= this.tfBrand.ClientID %>').val(data.BrandIDs);
                        $('#<%= this.tfBrandName.ClientID %>').val(data.BrandNames);
                    }
                }
            });
        },
        //------------------------
        newMember = function () {//新建会员
            var outer = $('<div/>');
            outer.load('/CustInfo/EditVWithCalling/EditMember.aspx', { TID: '<%= this.TaskID %>' }, function (jd, textStatus, xhr) {
                $('#divMembers').prepend(jd);
                memberAreaActionHelper.triggerFirst();
            });
        },
         newCstMember = function () {//新建车商通会员
             var outer = $('<div/>');
             outer.load('/CustInfo/EditVWithCalling/EditCstMember.aspx', { TID: '<%= this.TaskID %>' }, function (jd, textStatus, xhr) {
                 $('#divCstMembers').prepend(jd);

             });
         },
        loadMember = function () {//得到会员信息
            //$('#divMembers').load('/CustInfo/EditVWithCalling/EditMember.aspx', { TaskID: '' });
        },

        syncEleName = function (idFrom, idTo, eleType) {//同步控件名称
            if (memberAreaActionHelper.getCurrentMemerArea()) {
                if (!eleType) {
                    memberAreaActionHelper.getCurrentMemerArea().each(function () {
                        $(this).find('input[id$="' + idTo + '"]').val($('#' + idFrom).val());
                    })
                }
                else if (eleType == 'select') {
                    var to = memberAreaActionHelper.getCurrentMemerArea().find('select[id$="' + idTo + '"]');
                    var from = $('#' + idFrom);
                    to.val(from.val()).change();
                    //to.val(from.val()); //使用这个方法的话IE6会报错！
                }
            }
        },
        sysMemberName = function () {
            //同步易湃会员名称
            syncEleName('<%= this.tfCustName.ClientID %>', 'tfMemberName');
            //同步车商通会员名称
            $('#divCstMembers').find('.MemberInfoArea').each(function (i, v) {
                $(v).find("input[id$='tfCstMemberFullName']").val($("#<%= this.tfCustName.ClientID %>").val());
            });
        },
        sysMemberAbbr = function () {
            //同步易湃会员简称
            syncEleName('<%= this.tfCustAbbr.ClientID %>', 'tfMemberAbbr');
            //同步车商通会员简称
            $('#divCstMembers').find('.MemberInfoArea').each(function (i, v) {
                $(v).find("input[id$='tfCstMemberShortName']").val($("#<%= this.tfCustAbbr.ClientID %>").val()).trigger("onblur");
            });
        },
        sysZipcode = function () {
            //同步易湃会员邮编;
            syncEleName('<%= this.tfZipcode.ClientID %>', 'tfZipcode');
            //同步车商通会员邮编
            $('#divCstMembers').find('.MemberInfoArea').each(function (i, v) {
                $(v).find("input[id$='tfCstMemberPostCode']").val($("#<%= this.tfZipcode.ClientID %>").val());
            });
        },
        sysAddress = function () {
            //同步易湃会员地址
            syncEleName('<%= this.tfAddress.ClientID %>', 'tfAddress');
            //同步车商通会员地址
            $('#divCstMembers').find('.MemberInfoArea').each(function (i, v) {
                $(v).find("input[id$='tfCstMemberAddress']").val($("#<%= this.tfAddress.ClientID %>").val());
            });
        },
        syncAreaName = function () {//同步区域
            $('#divMembers').find('.MemberInfoArea').each(function (i, v) {
                syncEleName('<%= this.selProvince.ClientID %>', 'selProvince', 'select');
                syncEleName('<%= this.selCity.ClientID %>', 'selCity', 'select');
                syncEleName('<%= this.selCounty.ClientID %>', 'selCounty', 'select');
            });
            $('#divCstMembers').find('.MemberInfoArea').each(function (i, v) {
                $(v).find("select[id$='selCstMemberProvince']").val($("#<%= this.selProvince.ClientID %>").val()).change();

                $(v).find("select[id$='selCstMemberCity']").val($("#<%= this.selCity.ClientID %>").val()).change();
                $(v).find("select[id$='selCstMemberCounty']").val($("#<%= this.selCounty.ClientID %>").val());
            });
        },

        SearchSameCustName = function (flag) { //显示重复客户名称层
            var body = { Action: 'Search', ExceptCustID: '<%= this.OriginalCustID %>', TID: '<%= this.TaskID %>', r: Math.random() };
            if (!flag)
                body.IsShowdDelCustRelation = true;
            if ((!flag && '<%=IsExistDelCustRelationRecord %>' == 'True') || flag)
                $('#divAddDelCustRelation').load('/CustInfo/EditVWithCalling/AddDelCustRelation.aspx', body);
        },
        //弹出标注信息保存层
        openSavePopup = function () {
            var info = uCEditCustHelper.getAllDataInPage();

            var msg = uCEditCustHelper.validateData(info.CustInfo, info.MemberInfoArray, info.CstMemberInfoArray, false);
            if (msg) { $.jAlert(msg); return; }

            info = uCEditCustHelper.encodeParams(info);
            var data = JSON.stringify(info);
            $.openPopupLayer({
                name: "SaveCheckInfo",
                width: 550,
                url: "/CustInfo/EditVWithCalling/SavePopup.aspx",
                parameters: {
                    PopupName: 'SaveCheckInfo',
                    TID: '<%= this.TaskID %>'
                },
                afterClose: function (effectiveAction, cData) {
                    if (effectiveAction) {//提交保存

                        //info = uCEditCustHelper.encodeParams(info);
                        //var data = JSON.stringify(info);
                        $.post('/CustInfo/Handler.ashx?callback=?', {
                            Action: 'SaveCheckInfo',
                            CheckedInfo: data,
                            AdditionalStatus: encodeURIComponent(cData.AdditionalStatus),
                            G_description: encodeURIComponent(cData.G_description)
                        }, function (jd, status, xhr) {
                            if (status != 'success') { $.jAlert('请求错误'); return; }
                            if (jd.success) {
                                $.jPopMsgLayer('保存成功', function () {
                                    window.location = window.location;
                                });
                            }
                            else {
                                $.jAlert('错误：' + jd.message);
                            }
                        }, 'json');
                    }
                }
            });
        },
        //弹出三个问题层信息
        PopupDelCustRelation = function (urlGoto) {
            var info = uCEditCustHelper.getAllDataInPage();
            var msg = uCEditCustHelper.validateData(info.CustInfo, info.MemberInfoArray, info.SurveyInfoArray, true);
            if (msg) { $.jAlert(msg); return; }
            info = uCEditCustHelper.encodeParams(info);
            var data = JSON.stringify(info);

            //弹出填写3个问题层----------开始
            var selectObj = $('select[id$="selProvince"]', $('.CustInfoArea'))[0];
            var custProvinceName = '';
            var custCityName = '';
            var custCountyName = '';
            if (selectObj != null && selectObj != undefined)
                custProvinceName = selectObj.options[selectObj.selectedIndex].text;
            selectObj = $('select[id$="selCity"]', $('.CustInfoArea'))[0];
            if (selectObj != null && selectObj != undefined)
                custCityName = selectObj.options[selectObj.selectedIndex].text;
            selectObj = $('select[id$="selCounty"]', $('.CustInfoArea'))[0];
            if (selectObj != null && selectObj != undefined)
                custCountyName = selectObj.options[selectObj.selectedIndex].text;
            $.openPopupLayer({
                name: "DelCustRelationPopup",
                width: 550,
                url: "/CustInfo/EditVWithCalling/DelCustRelationPopup.aspx",
                parameters: {
                    PopupName: 'DelCustRelationPopup',
                    TID: '<%= this.TaskID %>',
                    CustID: '<%= this.OriginalCustID %>',
                    DelRelationCustIDs: $("#ulSameCustName table.cxjg tr:gt(0)").map(function () { return $.trim($(this).find('td:first :checkbox').val()); }).get().join(","),
                    CustName: escape($.trim($('input[id$="tfCustName"]', $('.CustInfoArea')).val())),
                    CustProvinceName: escape(custProvinceName),
                    CustCityName: escape(custCityName),
                    CustCountyName: escape(custCountyName),
                    CustBrandName: escape($.trim($('input[id$="tfBrandName"]', $('.CustInfoArea')).val()))
                },
                afterClose: function (effectiveAction, cData) {
                    if (effectiveAction) {//提交保存
                        $.post('/CustInfo/Handler.ashx?callback=?', {
                            Action: 'SubmitCheckInfo', CheckedInfo: data
                        }, function (jd, status, xhr) {
                            if (status != 'success') { $.jAlert('请求错误'); return; }
                            if (jd.success) {
                                $.jPopMsgLayer('提交成功', function () {
                                    closePageExecOpenerSearch();
                                });
                            }
                            else {
                                $.jAlert('错误：' + jd.message);
                            }
                        }, 'json');
                    }
                }
            });
            //弹出填写3个问题层----------结束
        },


        init = function () {
            //绑定区域
            BindProvince('<%= this.selProvince.ClientID %>'); //绑定省份
            $('#<%= this.selProvince.ClientID %>').val('<%= this.InitialProvinceID %>');
            triggerProvince();
            if ('<%= this.InitialCityID %>'.toString().length > 0 && '<%= this.InitialCityID %>' != '-1') {
                $('#<%= this.selCity.ClientID %>').val('<%= this.InitialCityID %>');
            }
            triggerCity();
            if ('<%= this.InitialCountyID %>'.toString().length > 0 && '<%= this.InitialCountyID %>' != '-1') {
                $('#<%= this.selCounty.ClientID %>').val('<%= this.InitialCountyID %>');
            }

            //绑定所属4S标签内容
            if ($('#<%=selCustType.ClientID %>').val() == '<%=(int)BitAuto.YanFa.Crm2009.Entities.EnumCustomType.Licence %>') {
                //如果4s不为空，集团、厂商、主营品牌不可编辑
                if ($("#<%=tfPidName.ClientID %>").val() != "") {
                    $("span[name='spanImg']").css("display", "none");
                }
                //$('#<%=tfCustPidName.ClientID %>').next().next().find('a').removeAttr('href'); //禁用所属厂商
                $('#<%=tfBrandName.ClientID %>').next().next().find('a').removeAttr('href'); //禁用主营品牌
            }
            //得到当前会员区域
            if ('<%=Task.CrmCustID %>' != '') {
                $('#two3').show();
                $('#two2').show();
                $('#two4').show();
                $('#two6').show();
            }
            loadTabList(1, 1);
            loadTaskLog();
            loadCallRecord();

            if ('<%=CarType%>' == '3') {
                $("#two3").css("display", "block");
                //如果是（新车、二手车），且类别为4s时，才可以新增车商通会员信息
                if ($("#<%=selCustType.ClientID%>").val() == '20003') {
                    //$("#aNewCstMember").css("display", "block");
                }
            }
        };

        return {
            triggerProvince: triggerProvince,
            triggerCity: triggerCity,
            openSelectCustomerPopup: openSelectCustomerPopup,
            openSelectCustomerPopup1: openSelectCustomerPopup1,
            openSelectBrandPopup: openSelectBrandPopup,
            //loadContacts: loadContacts,
            newMember: newMember,
            //loadMember: loadMember,
            newCstMember: newCstMember,
            init: init,
            syncEleName: syncEleName,
            sysMemberName: sysMemberName,
            sysMemberAbbr: sysMemberAbbr,
            syncAreaName: syncAreaName,
            sysZipcode: sysZipcode,
            sysAddress: sysAddress,
            custNameExist: custNameExist,
            SearchSameCustName: SearchSameCustName,
            openSavePopup: openSavePopup,
            PopupDelCustRelation: PopupDelCustRelation
        }
    })();
</script>
<script type="text/javascript">
    var memberAreaActionHelper = (function () {
        var currentMemberArea, //当前会员信息区域

        getCurrentMemerArea = function () {
            return $('#divMembers').find('.MemberInfoArea:visible');
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

            $('#divMembers .MemberInfoArea:not(:first)').hide(); //如果一开始隐藏，隐藏区域的地图位置会偏移
            var f = $('#divMembers .MemberInfoArea:first');
            if (f && f.length > 0) {
                f.show('fast');
                currentMemberArea = f;
            }
        },

        init = function () {

            $("#hdnBeforeCustType").val($('select[id$="selCustType"]').val());
        };

        return {
            getCurrentMemerArea: getCurrentMemerArea,
            triggerMemberArea: triggerMemberArea,
            triggerFirst: triggerFirst//,
        };
    })();
</script>
<script type="text/javascript">
    //得到所填的数据
    uCEditCustHelper.getAllDataInPage = function () {
        var cia = $('.CustInfoArea');
        var lockFlag = false;
        $.each($("#ulSameCustName table.cxjg tr:gt(0) td:last"), function (i, n) {
            if ($(n).find('img').attr('status') == '1') {
                lockFlag = true; return false;
            }
        });
        var custInfo = {
            TID: '<%= this.TaskID %>',
            CustName: $.trim($('input[id$="tfCustName"]', cia).val()),
            CustAbbr: $.trim($('input[id$="tfCustAbbr"]', cia).val()),
            TypeID: $.trim($('select[id$="selCustType"]', cia).val()),
            IndustryID: $.trim($('select[id$="selCustIndustry"]', cia).val()),
            ProvinceID: $.trim($('select[id$="selProvince"]', cia).val()),
            CityID: $.trim($('select[id$="selCity"]', cia).val()),
            CountyID: $.trim($('select[id$="selCounty"]', cia).val()),
            IsHasCounty: ($('select[id$="selCounty"]', cia).attr('noCounty') == undefined ? 0 : $('select[id$="selCounty"]', cia).attr('noCounty')),
            Address: $.trim($('input[id$="tfAddress"]', cia).val()),
            Zipcode: $.trim($('input[id$="tfZipcode"]', cia).val()),
            LevelID: $.trim($('select[id$="selCustLevel"]', cia).val()),
            CustPid: $.trim($('input[id$="tfCustPid"]', cia).val()),
            Pid: $.trim($('input[id$="tfPid"]', cia).val()),
            ShopLevel: $.trim($('select[id$="selShopLevel"]', cia).val()),
            Brand: $.trim($('input[id$="tfBrand"]', cia).val()),
            OfficeTel: $.trim($('input[id$="tfOfficeTel"]', cia).val()),
            Fax: $.trim($('input[id$="tfFax"]', cia).val()),
            ContactName: $.trim($('input[id$="tfContactName"]', cia).val()),
            Notes: $.trim($('textarea[id$="tfNotes"]', cia).val()),
            DelRelationCustIDs: $("#ulSameCustName table.cxjg tr:gt(0)").map(function () { return $.trim($(this).find('td:first :checkbox').val()); }).get().join(","),
            DelRelationCustIDsContainLock: lockFlag,

            CarType: $(":checked[name$='CarType']", cia).map(function () {
                if ($(this).attr("checked")) {
                    return $(this).val();
                }
            }).get().join(','),
            CtsMemberID: $.trim($('input[id$="txtCtsMemberID"]', cia).val()).replace('，', ','),
            UsedCarBusinessType: $.trim($('select[id$="sltUsedCarBusinessType"]', cia).val()),
            TradeMarketID: $('#<%= this.tfCustPid1.ClientID %>').val(),
            FoursPid: $.trim($('input[id$="tf4sPid"]', cia).val())
        };

        var memberInfo = new Array();
        $('#divMembers').find('.MemberInfoArea').each(function (i, v) {
            var r = getMemberDataInOneArea($(v).parent().children());
            if (r) { memberInfo.push(r); }
        });


        function getMemberDataInOneArea(area) {
            if (area && area.length > 0) {
                var divMap = $('input[id$="hdnMap"]', area);
                var lat = divMap.data('marker_lat');
                var lng = divMap.data('marker_lng');
                return {
                    MemberID: $.trim($('input[id$="tfMemberID"]', area).val()),
                    MemberName: $.trim($('input[id$="tfMemberName"]', area).val()),
                    MemberAbbr: $.trim($('input[id$="tfMemberAbbr"]', area).val()),
                    MemberType: $.trim($('select[id$="selMemberType"]', area).val()),
                    //Phone: $.trim($('input[id$="tfPhone"]', area).val()),
                    Phone: $(":text[id*='tfPhone'][value!='']", area).map(function () { return $.trim($(this).val()); }).get().join(","), //改为3个输入框，存放联系电话
                    Brand: $.trim($('input[id$="tfBrand"]', area).val()),
                    ProvinceID: $.trim($('select[id$="selProvince"]', area).val()),
                    CityID: $.trim($('select[id$="selCity"]', area).val()),
                    CountyID: $.trim($('select[id$="selCounty"]', area).val()),
                    IsHasCounty: ($('select[id$="selCounty"]', area).attr('noCounty') == undefined ? 0 : $('select[id$="selCounty"]', area).attr('noCounty')),
                    Address: $.trim($('input[id$="tfAddress"]', area).val()),
                    Longitude: lng ? lng : '',
                    Lantitude: lat ? lat : '',
                    //Fax: $.trim($('input[id$="tfFax"]', area).val()),
                    Fax: $(":text[id*='tfFax'][value!='']", area).map(function () { return $.trim($(this).val()); }).get().join(","), //改为3个输入框，存放传真
                    CompanyWebSite: $.trim($('input[id$="tfCompanyWebSite"]', area).val()),
                    Email: $.trim($('input[id$="tfEmail"]', area).val()),
                    Postcode: $.trim($('input[id$="tfZipcode"]', area).val()),
                    SerialIds: $.trim($('input[id$="txtSerialIds"]', area).val()),
                    TrafficInfo: $.trim($('input[id$="tfTrafficInfo"]', area).val()),
                    EnterpriseBrief: $.trim($('textarea[id$="tfEnterpriseBrief"]', area).val()),
                    Notes: $.trim($('textarea[id$="tfNotes"]', area).val()),
                    SpanMemberName: $.trim($('span[id^="spanMemberName_"]', area).html())
                }
            }
            else { return null; }
        }

        var cstMemberInfos = new Array();
        $('#divCstMembers').find('.MemberInfoArea').each(function (i, v) {
            var r = getCstMemberDataInOneArea($(v).parent().children());
            if (r) { cstMemberInfos.push(r); }
        });


        function getCstMemberDataInOneArea(area) {
            if (area && area.length > 0) {
                return {
                    MemberID: $.trim($('input[id$="tfCstMemberID"]', area).val()),
                    MemberName: $.trim($('input[id$="tfCstMemberFullName"]', area).val()),
                    MemberAbbr: $.trim($('input[id$="tfCstMemberShortName"]', area).val()),
                    //VendorCode:$.trim($('input[id$="tfVendorCode"]', area).val()),
                    MemberType: $.trim($('select[id$="selCstMemberType"]', area).val()),
                    ProvinceID: $.trim($('select[id$="selCstMemberProvince"]', area).val()),
                    CityID: $.trim($('select[id$="selCstMemberCity"]', area).val()),
                    CountyID: $.trim($('select[id$="selCstMemberCounty"]', area).val()),
                    SuperID: $.trim($('select[id$="sltSuperId"]', area).val()),
                    Address: $.trim($('input[id$="tfCstMemberAddress"]', area).val()),
                    PostCode: $.trim($('input[id$="tfCstMemberPostCode"]', area).val()),
                    //                    TrafficInfo: $.trim($('input[id$="tfTrafficInfo"]', area).val()),
                    Brand: $.trim($('input[id$="tfCstMemberBrand"]', area).val()),

                    LinkManName: $.trim($('input[id$="tfLinkManName"]', area).val()),
                    //                    Department: $.trim($('input[id$="tfLinkManDepartment"]', area).val()),
                    //                    Position: $.trim($('input[id$="tfLinkManPosition"]', area).val()),
                    Mobile: $.trim($('input[id$="tfLinkManMobile"]', area).val()),
                    Email: $.trim($('input[id$="tfLinkManEmail"]', area).val())
                }
            }
            else { return null; }
        }

        return { CustInfo: custInfo, MemberInfoArray: memberInfo, CstMemberInfoArray: cstMemberInfos };
    };
    uCEditCustHelper.validateData = function (custInfo, memberInfoArray, cstMemberInfoArray, onSubmit) {//onSubmit 指提交时的检查，默认是保存
        var msg = '';
        if (custInfo) {
            if (custInfo.CustName.length == 0) { msg += '客户名称不可为空<br/>'; }
            if (onSubmit && (custInfo.TypeID.length == 0 || custInfo.TypeID == -1)) { msg += '必须选择客户类别<br/>'; }
            if (onSubmit && (custInfo.IndustryID.length == 0 || custInfo.IndustryID == -1)) { msg += '必须选择客户行业<br/>'; }
            if (onSubmit && (custInfo.CityID.length == 0 || custInfo.CityID == -1)) { msg += '必须选择客户地区<br/>'; }
            if (onSubmit && custInfo.Address.length == 0) { msg += '注册地址不可为空<br/>'; }
            if (onSubmit && custInfo.Zipcode.length == 0) { msg += '邮政编码不可为空<br/>'; }
            if (onSubmit && custInfo.CustPid == '' && custInfo.TypeID == '<%=(int)BitAuto.YanFa.Crm2009.Entities.EnumCustomType.CompanyRegion %>') { msg += '当客户类别为“厂商大区”时，必须选择所属厂商<br/>'; }
            if (onSubmit && custInfo.CarType == '1,2' && custInfo.TypeID == '<%=(int)BitAuto.YanFa.Crm2009.Entities.EnumCustomType.FourS %>' && custInfo.UsedCarBusinessType == '-1') { msg += '当客户所属4S时，并且经营范围是“新车、二手车”时,请选择二手车经营类型<br/>'; }
        }
        if (memberInfoArray && memberInfoArray.length > 0) {
            for (var i = 0; i < memberInfoArray.length; i++) {
                var mi = memberInfoArray[i];
                if (mi.MemberName.length == 0) { msg += (mi.SpanMemberName + '，会员名称不可为空<br/>'); }
                if (onSubmit && mi.MemberAbbr.length == 0) { msg += (mi.SpanMemberName + '，会员简称不可为空<br/>'); }
                if (onSubmit && Len(mi.MemberAbbr) > 14) { msg += (mi.SpanMemberName + '，会员简称不能大于14个字符<br/>'); }
                if (mi.MemberType != 3) {
                    if (onSubmit && mi.Brand.length == 0) { msg += (mi.SpanMemberName + '，必须选择主营品牌<br/>'); }
                }
                if (onSubmit && (mi.CityID.length == 0 || mi.CityID == -1)) { msg += (mi.SpanMemberName + '，必须选择会员地区<br/>'); }
                if (onSubmit && mi.Address.length == 0) { msg += (mi.SpanMemberName + '，销售地址不可为空<br/>'); }
                if (onSubmit && (!mi.Longitude || !mi.Lantitude)) { msg += (mi.SpanMemberName + '，必须在地图上标记地址<br/>'); }
            }
        }
        if (cstMemberInfoArray && cstMemberInfoArray.length > 0) {
            for (var i = 0; i < cstMemberInfoArray.length; i++) {
                var mi = cstMemberInfoArray[i];
                if (mi.MemberName.length == 0) { msg += (mi.MemberName + '，会员名称不可为空<br/>'); }
                if (onSubmit && mi.MemberAbbr.length == 0) { msg += (mi.MemberName + '，会员简称不可为空<br/>' + mi.MemberAbbr.length); }
                if (onSubmit && Len(mi.MemberAbbr) > 14) { msg += (mi.MemberName + '，会员简称不能大于14个字符<br/>'); }
                if (onSubmit && (mi.CityID.length == 0 || mi.CityID == -1)) { msg += (mi.MemberName + '，必须选择会员地区<br/>'); }

                if (!$("select[id$='sltCstLinkMan']:eq(" + i + ")").attr("disabled")) {
                    if (onSubmit && mi.SuperID == -1) { msg += (mi.MemberName + '，请选择上级公司<br/>'); }
                    //if (onSubmit && mi.VendorCode.length == 0) { msg += (mi.MemberName + '，会员编码不能为空<br/>'); }

                    if (onSubmit && (mi.LinkManName.length == 0)) { msg += (mi.MemberName + '，联系人名称不能为空<br/>'); }
                    //                if (onSubmit && (mi.Department.length==0)) { msg += (mi.MemberName + '，联系人部门不能为空<br/>');}
                    //                if (onSubmit && (mi.Position.length==0)) { msg += (mi.MemberName + '，联系人职位不能为空<br/>');}
                    if (onSubmit && (mi.Mobile.length == 0)) { msg += (mi.MemberName + '，联系人手机不能为空<br/>'); }
                    if (mi.Mobile.length > 0 && !isMobile(mi.Mobile)) { msg += (mi.MemberName + ',联系人手机格式不正确<br/>'); }
                    if (mi.Email.length > 0 && isEmail(mi.Email) == false) { msg += (mi.MemberName + ',邮件格式不正确<br/>'); }
                    if (onSubmit && (mi.Email.length == 0)) { msg += (mi.MemberName + '，联系人邮箱不能为空<br/>'); }
                }
            }
        }
        return msg;
    };

    uCEditCustHelper.encodeParams = function (data) {//为参数编码 info.CustInfo, info.MemberInfoArray
        if (data.CustInfo) {
            for (var k in data.CustInfo) {
                data.CustInfo[k] = encodeURIComponent(data.CustInfo[k]);
            }
        }
        if (data.MemberInfoArray && data.MemberInfoArray.length > 0) {
            for (var i = 0; i < data.MemberInfoArray.length; i++) {
                var d = data.MemberInfoArray[i];
                for (var k in d) {
                    d[k] = encodeURIComponent(d[k])
                }
            }
        }
        return data;
    };

    uCEditCustHelper.saveInfo = function () {//保存
        var info = uCEditCustHelper.getAllDataInPage();
        var msg = uCEditCustHelper.validateData(info.CustInfo, info.MemberInfoArray, info.CstMemberInfoArray, false);
        if (msg) { $.jAlert(msg); return; }

        info = uCEditCustHelper.encodeParams(info);
        var data = JSON.stringify(info);
        $.post('/CustInfo/Handler.ashx?callback=?', {
            Action: 'verifysavecheckinfo', CheckedInfo: data
        }, function (jd, status, xhr) {
            if (status != 'success') { $.jAlert('请求错误'); return; }
            if (jd.success) {
                var delrelationcustids = $("#ulSameCustName table.cxjg tr:gt(0)").map(function () { return $.trim($(this).find('td:first :checkbox').val()); }).get().join(",");
                $.post('/CustInfo/Handler.ashx?callback=?', {
                    Action: 'AddDeleteCustRelationInfo',
                    TID: '<%= this.TaskID %>',
                    CustID: '<%= this.OriginalCustID %>',
                    DelRelationCustIDs: delrelationcustids,
                    DescriptionWhenDelete: ''
                }, function (jd, textStatus, xhr) {
                    if (textStatus != 'success') { $.jAlert('请求错误'); }
                    else if (jd.success) {
                        //closePopup(true);
                        uCEditCustHelper.openSavePopup(); //弹出标注信息保存层
                    }
                    else {
                        $.jAlert('错误: ' + jd.message);
                    }
                }, 'json');
            }
            else {
                $.jAlert('错误：' + jd.message);
            }
        }, 'json');

    };

    uCEditCustHelper.submitInfo = function (urlGoto) {//提交

        $.jConfirm("确定要提交核实的信息吗？如果提交后将进入审核阶段，座席将不能操作该客户及会员信息！", function (flag) {
            if (flag) {
                var custName = $('#<%= this.tfCustName.ClientID %>').val();
                $.getJSON('/CustInfo/Handler.ashx?callback=?', {
                    Action: 'CustNameExist',
                    CustName: encodeURIComponent($.trim(custName || '')),
                    CustID: '<%= this.OriginalCustID %>'
                }, function (jd, textStatus, xhr) {
                    if (textStatus != 'success') { $.jAlert('请求错误'); return; }
                    if (jd.success) {
                        var info = uCEditCustHelper.getAllDataInPage();
                        var msg = uCEditCustHelper.validateData(info.CustInfo, info.MemberInfoArray, info.CstMemberInfoArray, true);
                        if (msg) { $.jAlert(msg); return; }
                        info = uCEditCustHelper.encodeParams(info);
                        var data = JSON.stringify(info);

                        var lockFlag = false;
                        $.each($("#ulSameCustName table.cxjg tr:gt(0) td img[status]"), function (i, n) {
                            if ($(n).attr('status') == '1') {
                                lockFlag = true; return false;
                            }
                        });
                        if ('<%=Task.Source %>' == '1' &&
                       ($("#ulSameCustName table.cxjg tr:gt(0)").size() > 0 || jd.result == '1'))//Excel新增客户,用到“查重”功能或客户名称在CRM库存在
                        { $.jAlert('此客户在CRM库中已存在，请不要重复添加<br>' + jd.message, function () { return; }); return; }

                        if (jd.result == '1') {//用户名称是否存在，1-存在，0-不存在

                            $.jConfirm('CRM库中已存在此客户名称<br>' + jd.message + '<br>确定要添加重名申请吗？', function (flag) {
                                if (flag) {
                                    var body = { Action: 'Search', ExceptCustID: '<%= this.OriginalCustID %>', TID: '<%= this.TaskID %>', CustName: escape($.trim(custName)), CustNameAllMatch: 'yes', r: Math.random() };
                                    $('#divAddDelCustRelation').load('/CustInfo/EditVWithCalling/AddDelCustRelation.aspx', body, function () {
                                        var lockFlag = false;
                                        $.each($("#ulSameCustName table.cxjg tr:gt(0) td img[status]"), function (i, n) {
                                            if ($(n).attr('status') == '1') {
                                                lockFlag = true; return false;
                                            }
                                        });
                                        if (lockFlag)
                                        { uCEditCustHelper.PopupDelCustRelation(urlGoto); }
                                        else {

                                            $.post('/CustInfo/Handler.ashx?callback=?', {
                                                Action: 'AddDeleteCustRelationInfo',
                                                TID: '<%= this.TaskID %>',
                                                CustID: '<%= this.OriginalCustID %>',
                                                DelRelationCustIDs: $("#ulSameCustName table.cxjg tr:gt(0)").map(function () { return $.trim($(this).find('td:first :checkbox').val()); }).get().join(","),
                                                DescriptionWhenDelete: ''
                                            }, function (jd, textStatus, xhr) {
                                                if (textStatus != 'success') { $.jAlert('请求错误'); }
                                                else if (jd.success) {
                                                    $.post('/CustInfo/Handler.ashx?callback=?', {
                                                        Action: 'SubmitCheckInfo', CheckedInfo: data
                                                    }, function (jd, status, xhr) {
                                                        if (status != 'success') { $.jAlert('请求错误'); return; }
                                                        if (jd.success) {
                                                            $.jPopMsgLayer('提交成功', function () {
                                                                closePageExecOpenerSearch();
                                                            });
                                                        }
                                                        else {
                                                            $.jAlert('错误：' + jd.message);
                                                        }
                                                    }, 'json');
                                                }
                                                else {
                                                    $.jAlert('错误: ' + jd.message);
                                                }
                                            }, 'json');

                                        }
                                    });
                                }
                                return;
                            });
                        }
                        else {
                            if (lockFlag) {//弹出填写3个问题层
                                uCEditCustHelper.PopupDelCustRelation(urlGoto);
                            }
                            else {
                                $.post('/CustInfo/Handler.ashx?callback=?', {
                                    Action: 'AddDeleteCustRelationInfo',
                                    TID: '<%= this.TaskID %>',
                                    CustID: '<%= this.OriginalCustID %>',
                                    DelRelationCustIDs: $("#ulSameCustName table.cxjg tr:gt(0)").map(function () { return $.trim($(this).find('td:first :checkbox').val()); }).get().join(","),
                                    DescriptionWhenDelete: ''
                                }, function (jd, textStatus, xhr) {
                                    if (textStatus != 'success') { $.jAlert('请求错误'); }
                                    else if (jd.success) {
                                        $.post('/CustInfo/Handler.ashx?callback=?', {
                                            Action: 'SubmitCheckInfo', CheckedInfo: data
                                        }, function (jd, status, xhr) {
                                            if (status != 'success') { $.jAlert('请求错误'); return; }
                                            if (jd.success) {
                                                $.jPopMsgLayer('提交成功', function () {
                                                    closePageExecOpenerSearch();
                                                });
                                            }
                                            else {
                                                $.jAlert('错误：' + jd.message);
                                            }
                                        }, 'json');
                                    }
                                    else {
                                        $.jAlert('错误: ' + jd.message);
                                    }
                                }, 'json');
                            }
                        }
                    }
                });
            }

        });
    };

    uCEditCustHelper.deleteInfo = function (urlGoto) {//删除
        $.openPopupLayer({
            name: "DeleteCheckInfo",
            width: 550,
            url: "/CustInfo/EditVWithCalling/DelPopup.aspx",
            parameters: {
                PopupName: 'DeleteCheckInfo',
                TID: '<%= this.TaskID %>'
            },
            afterClose: function (effectiveAction) {
                if (effectiveAction) {
                    $.jPopMsgLayer('删除成功', function () {
                        closePageExecOpenerSearch();
                    });
                }
            }
        });
    };
    uCEditCustHelper.stopCustInfo = function (urlGoto) {
        $.openPopupLayer({
            name: "StopCheckInfo",
            width: 550,
            url: "/CustInfo/EditVWithCalling/StopPopup.aspx",
            parameters: {
                PopupName: 'StopCheckInfo',
                TID: '<%= this.TaskID %>'
            },
            afterClose: function (effectiveAction) {
                if (effectiveAction) {
                    $.jPopMsgLayer('操作成功', function () {
                        closePageExecOpenerSearch();
                    });
                }
            }
        });
    }
    function divShowHideEvent(divId, obj) {
        if ($(obj).attr("class") == "toggle") {
            $("#" + divId).show("slow");
            $(obj).attr("class", "toggle hide");
        }
        else {
            $("#" + divId).hide("slow");
            $(obj).attr("class", "toggle");
        }
    }
    function setTab(cursel) {
        $(".menuConbox2 [id^='con_two']").css("display", "none");
        $(".menuConbox2 .cont_cxjg").css("display", "none");
        $(".hd2 ul li[id^='two']").attr("class", "");
        $("#two" + cursel).attr("class", "hover");
        $("#con_two_" + cursel).css("display", "block").parent().css("display", "block");
        loadTabList(cursel, 1);
    }
    function loadTabList(n, menu) {
        switch (n) {
            case 1: //客户联系人
                $("#con_two_1").load("/CustInfo/MoreInfo/CC_Contact/ListWithEdit.aspx", { TID: '<%= this.TaskID %>', CustID: '<%= Task.CrmCustID %>',
                    PageSize: 10, ContentElementId: 'con_two_1'
                }, function () { });
                break;
            case 2: //合作项
                $("#con_two_2").load("/CustInfo/MoreInfo/CooperationProjectList.aspx", { CustID: '<%=Task.CrmCustID %>', IsPartShow: true, ContentElementId: 'con_two_2' }, function () { });
                break;
            case 3: //负责员工
                $("#con_two_3").load("/CustInfo/MoreInfo/CustUserList.aspx", { CustID: '<%=Task.CrmCustID %>', IsPartShow: true, ContentElementId: 'con_two_3' }, function () { });
                break;
            case 4: //年检
                $("#con_two_4").load("/CustInfo/MoreInfo/BusinessLicenseList.aspx", { CustID: '<%=Task.CrmCustID %>', ContentElementId: 'con_two_4' }, function () { });
                break;
            case 5: //二手车规模
                $("#con_two_5").load("/CustInfo/MoreInfo/CC_BusinessScale/List.aspx", { TID: '<%=this.TaskID %>', ContentElementId: 'con_two_5' }, function () { });
                break;
            case 6: //品牌授权书
                $("#con_two_6").load("/CustInfo/MoreInfo/CustBrandLicenseList.aspx", { CustID: '<%=Task.CrmCustID %>', ContentElementId: 'con_two_6' }, function () { });
                break;
        }
    }
    function clear4S() {
        var value = $("#<%=tf4sName.ClientID %>").val();
        $("#<%=tf4sName.ClientID %>").val("");
        $("#<%=tf4sPid.ClientID %>").val("");
        $("[name='spanImg']").css("display", "");
        if (Len(value) > 0) {
            $('#divMembers').find('.MemberInfoArea').each(function (i, v) {
                memberTypeControl($(v), "selCustType", "selMemberType", true);
            });
            $("#<%=tfPidName.ClientID %>").val("");
            $("#<%=tfPid.ClientID %>").val("");
            $("#<%=tfCustPidName.ClientID %>").val("");
            $("#<%=tfCustPid.ClientID %>").val("");

            $("#<%=tfBrandName.ClientID %>").val("");
            $("#<%=tfBrand.ClientID %>").val("");
        }
    }
</script>
<script type="text/javascript">
    $(function () {
        uCEditCustHelper.init();
        uCEditCustHelper.SearchSameCustName(false);
        //控制展开和收起
        $('img.shouqizhankai').toggle(
          function () {
              $(this).attr({ src: '/images/zhankai.gif', title: '展开' })
              .next().hide("normal");
          },
          function () {
              $(this).attr({ src: '/images/shouqi.gif', title: '收起' })
            .next().show("normal");

          }
        );

        OldCarTypeClick();
        if ('<%=(int)BitAuto.YanFa.Crm2009.Entities.EnumCustomType.Licence %>' == $('select[id$="selCustType"]').val()) {
            //如果客户类别为特许经销商时,显示所属4S
            $("#li4S").css("display", "");
            //如果所属4S不为空时，所属集团、所属厂商、主营品牌不可编辑
            if (Len($("#<%=tf4sName.ClientID %>").val()) > 0) {
                $("[name='spanImg']").css("display", "none");
            }
        }
    });
    //客户类别下拉列表变更事件
    function CustTypeOnChange(ddlObj) {
        $('#divCstMembers').find('.MemberInfoArea').each(function (i, v) {

            switch ($('select[id$="selCustType"]').val()) {
                case "20003":
                    $('select[id$="selCstMemberType"]', $(v)).val("2");
                    break;
                case "20010":
                case "20011":
                    $('select[id$="selCstMemberType"]', $(v)).val("3");
                    break;
                default:
                    if ($("#divCstMembers").find('.MemberInfoArea').size() > 0) {
                        if (confirm("在此客户类型下，不允许出现车商通会员，请确认是否删除新建车商通会员")) {
                            $.post('/CustInfo/Handler.ashx?callback=?', {
                                Action: 'DeleteNewCC_CstMember',
                                TID: '<%= this.TaskID %>'
                            }, function (jd, textStatus, xhr) {
                                if (jd.success) {
                                    $('#divCstMembers').find('div[class="cont_cx khxx"]').each(function (i, v) {
                                        $(v).attr("name", "").remove();
                                    });
                                }
                                else {
                                    $.jAlert('请求错误');
                                }
                            }, 'json');
                        }
                        else {
                            $("#<%=selCustType.ClientID %>").val("20003");
                        }
                    }
                    break;
            }

        });
        $('#divMembers').find('.MemberInfoArea').each(function (i, v) {
            memberTypeControl($(v), "selCustType", "selMemberType", true);
        });
        if ('<%=(int)BitAuto.YanFa.Crm2009.Entities.EnumCustomType.Licence %>' == $('select[id$="selCustType"]').val()) {
            $("#li4S").css("display", "");
            $("[name='spanImg']").css("display", "none");

            $('#<%=tfBrandName.ClientID %>').next().next().find('a').removeAttr('href'); //禁用主营品牌

        }
        else {
            $("#li4S").css("display", "none");
            $("[name='spanImg']").css("display", "");

            if ($("#hdnBeforeCustType").val() == '<%=(int)BitAuto.YanFa.Crm2009.Entities.EnumCustomType.Licence %>') {
                //如果从展厅类型转换为其他类型时，清空所属厂商信息
                //$('#<%=tfCustPidName.ClientID %>').val('');
                //$('#<%=tfCustPid.ClientID %>').val('');
            }

            $('#<%=tfCustPid.ClientID %>').parent().find('em').remove();
            $('#<%=tfBrandName.ClientID %>').next().next().find('a').attr('href', 'javascript:uCEditCustHelper.openSelectBrandPopup();'); //启用主营品牌
        }

        $("#hdnBeforeCustType").val($('select[id$="selCustType"]').val())
        OldCarTypeClick();
    }

    function OldCarTypeClick() {
        if ($(':checked[id$="chkOldCarType"]').attr("checked")) {
            if ($('#<%=selCustType.ClientID %>').val() == '20003') {
                //$("#aNewCstMember").css("display", "block");
            }
            else {
                $("#aNewCstMember").css("display", "none");
            }
            $("li.carType").css("display", "block");
            $("#liSecondCar").css("display", "block");
        }
        else {
            $("#aNewCstMember").css("display", "none");
            $("li.carType").css("display", "none");
            $("#liSecondCar").css("display", "none");
        }
    }


</script>
<div class="cont_cx khxx CustInfoArea">
    <div class="title ft16">
        基本信息 <a href="javascript:void(0)" onclick="divShowHideEvent('divCustBasicInfo',this)"
            class="toggle hide"></a>
        <% if (IsShowAddMembers)
           {
        %>
        <a href="javascript:uCEditCustHelper.newMember();" style="float: right; margin-right: 40px;
            *margin-top: -30px;">新建易湃会员</a>
        <%} %>
        <a href="javascript:uCEditCustHelper.newCstMember();" id="aNewCstMember" style="float: right;
            margin-right: 20px; display: none;">新建车商通会员 </a>
    </div>
    <div id="divCustBasicInfo">
        <ul class="infoBlock firstPart">
            <li>
                <label style="width: 115px;">
                    客户名称：</label>
                <input id="tfCustName" name="CustName" class="w250" runat="server" />
                <em>*</em>
                <%--<a href="javascript:uCEditCustHelper.custNameExist($('#<%= this.tfCustName.ClientID %>').val());">
                    查重</a>--%>
                <a href="javascript:uCEditCustHelper.sysMemberName();">同步</a> <a href="javascript:uCEditCustHelper.SearchSameCustName(true);">
                    查重</a> </li>
            <li>
                <label style="width: 115px;">
                    客户简称：</label>
                <input id="tfCustAbbr" name="CustAbbr" class="w250" runat="server" />
                <a href="javascript:uCEditCustHelper.sysMemberAbbr();">同步</a> </li>
        </ul>
        <div id="divAddDelCustRelation">
        </div>
        <ul class="infoBlock firstPart">
            <li>
                <label style="width: 115px;">
                    客户类别：</label>
                <select id="selCustType" name="CustType" class="w255" runat="server" onchange="javascript:CustTypeOnChange(this);">
                </select>
                <input id="hdnBeforeCustType" type="hidden" />
                <em>*</em> </li>
            <li>
                <label style="width: 115px;">
                    经营范围：</label>
                <label style="width: 50px; margin-right: 40px;">
                    <input type="checkbox" id="chkNewCarType" name="CarType" value="1" style="height: auto;
                        width: auto;" checked="true" disabled="disabled" />新车</label>
                <label style="width: 60px; margin-right: 10px;">
                    <input type="checkbox" id="chkOldCarType" name="CarType" value="2" style="height: auto;
                        width: auto;" onclick="OldCarTypeClick()" runat="server" />二手车</label>
                <em>*</em> </li>
            <li>
                <label style="width: 115px;">
                    客户行业：</label>
                <select id="selCustIndustry" name="CustIndustry" class="w255" runat="server">
                </select>
                <em>*</em> </li>
            <li>
                <label style="width: 115px;">
                    客户地区：</label>
                <select id="selProvince" name="Province" style="width: 82px;" onchange="javascript:uCEditCustHelper.triggerProvince();"
                    runat="server">
                </select>
                <select id="selCity" name="City" style="width: 82px;" onchange="javascript:uCEditCustHelper.triggerCity();"
                    runat="server">
                </select>
                <select id="selCounty" name="County" style="width: 82px;" runat="server">
                </select>
                <em>*</em> <a href="javascript:uCEditCustHelper.syncAreaName();">同步</a> </li>
            <li>
                <label style="width: 115px;">
                    邮政编码：</label>
                <input id="tfZipcode" class="w250" name="Zipcode" runat="server" />
                <em>*</em> <a href="javascript:uCEditCustHelper.sysZipcode();">同步</a> </li>
            <li>
                <label style="width: 115px;">
                    注册地址：</label>
                <input id="tfAddress" class="w250" name="Address" runat="server" />
                <em>*</em> <a href="javascript:uCEditCustHelper.sysAddress();">同步</a> </li>
            <li class="carType" style="display: none;">
                <label style="width: 115px;" class='carType'>
                    二手车经营类型：</label>
                <select id="sltUsedCarBusinessType" class="w255" runat="server">
                    <option value="-1">请选择</option>
                    <option value="1">置换型</option>
                    <option value="2">零售型</option>
                </select><em>*</em> </li>
        </ul>
        <div class="spliter">
        </div>
        <ul class="infoBlock ">
            <li id="liCustStatus" runat="server" style="display: none;">
                <label style="width: 115px;">
                    客户状态：</label>
                <img style="margin-left: 5px;" id="imgCustStatus" runat="server">
            </li>
            <li id="liCustLock" runat="server" style="display: none;">
                <label style="width: 115px;">
                    锁定：</label>
                <img style="margin-left: 5px;" id="imgCustStatusLock" runat="server" />
            </li>
            <li>
                <label style="width: 115px;">
                    客户级别：</label>
                <select id="selCustLevel" class="w255" name="CustLevel" runat="server">
                </select>
            </li>
            <li id='li4S' style="display: none;">
                <label style="width: 115px;" runat="server">
                    所属4S：</label>
                <input id="tf4sName" name="PidName" class="w250" readonly="readonly" runat="server" />
                <input id="tf4sPid" name="Pid" style="display: none;" runat="server" />
                <span><a href="javascript:uCEditCustHelper.openSelectCustomerPopup(4);">
                    <img border="0" src="/images/button_001.gif" /></a> </span><span id="span4S"><a href="javascript:clear4S();">
                        清空</a> </span></li>
            <li>
                <label style="width: 115px;" id='lblGroup' runat="server">
                    所属集团：</label>
                <input id="tfPidName" name="PidName" class="w250" readonly="readonly" runat="server" />
                <input id="tfPid" name="Pid" style="display: none;" runat="server" />
                <span name="spanImg"><a href="javascript:uCEditCustHelper.openSelectCustomerPopup(2);">
                    <img border="0" src="/images/button_001.gif" /></a> </span></li>
            <li>
                <label style="width: 115px;">
                    所属厂商：</label>
                <input id="tfCustPidName" name="CustPidName" class="w250" readonly="readonly" runat="server" />
                <input id="tfCustPid" name="CustPid" style="display: none;" runat="server" />
                <span name="spanImg"><a href="javascript:uCEditCustHelper.openSelectCustomerPopup(1);">
                    <img border="0" src="/images/button_001.gif" /></a> </span></li>
            <li>
                <label style="width: 115px;">
                    主营品牌：</label>
                <input id="tfBrandName" name="BrandName" class="w250" type="text" readonly="readonly"
                    runat="server" />
                <input id="tfBrand" name="Brand" style="display: none;" runat="server" />
                <span name="spanImg"><a href="javascript:uCEditCustHelper.openSelectBrandPopup();">
                    <img alt="选择主营品牌" src="/images/button_001.gif" border="0" /></a> </span>
            </li>
            <li>
                <label style="width: 115px;">
                    经营店级别：</label>
                <select id="selShopLevel" name="ShopLevel" class="w255" runat="server">
                </select>
            </li>
            <li>
                <label style="width: 115px;">
                    电话：</label>
                <input id="tfOfficeTel" name="OfficeTel" class="w250" runat="server" />
                <span><a href="javascript:void(0);" onclick="功能废弃">
                    <img alt="打电话" src="/images/phone.gif" border="0" /></a> </span></li>
            <li>
                <label style="width: 115px;">
                    传真：</label>
                <input id="tfFax" name="Fax" class="w250" runat="server" />
            </li>
            <li>
                <label style="width: 115px;">
                    联系人：</label>
                <input id="tfContactName" name="ContactName" class="w250" runat="server" />
            </li>
            <li style="display: none;" class='carType'>
                <label style="width: 115px;">
                    所属交易市场：</label>
                <input id="tfCustPidName1" name="CustPidName1" class="w250" readonly="readonly" runat="server" />
                <input id="tfCustPid1" name="CustPid1" style="display: none;" runat="server" />
                <span><a href="javascript:uCEditCustHelper.openSelectCustomerPopup1(1);">
                    <img border="0" src="/images/button_001.gif" /></a> </span></li>
            <li class="singleRow">
                <label style="width: 115px;">
                    备注：</label>
                <textarea id="tfNotes" name="Notes" cols="4" rows="2" class="fullRow" style="width: 696px;"
                    runat="server"></textarea>
            </li>
        </ul>
        <div class="spliter">
        </div>
        <div class="hd2">
            <ul>
                <li id='two1' onclick="setTab(1)" class='hover'>客户联系人</li>
                <li id='two2' onclick="setTab(2)" style='display: none;' class=''>合作项</li>
                <li id='two3' onclick="setTab(3)" style='display: none;' class=''>负责员工</li>
                <li id='two4' onclick="setTab(4)" style='display: none;' class=''>年检记录</li>
                <li id='two5' onclick="setTab(5)" style='display: none;' class=''>二手车规模</li>
                <li id='two6' onclick="setTab(6)" style='display: none;' class=''>品牌授权书</li>
            </ul>
        </div>
        <div class="menuConbox menuConbox2">
            <!--内容1-->
            <div class='cont_cxjg'>
                <div id='con_two_1'>
                </div>
            </div>
            <!--内容1-->
            <!--内容2-->
            <div class='cont_cxjg'>
                <div id='con_two_2' style='display: none;'>
                </div>
            </div>
            <!--内容2-->
            <!--内容3-->
            <div class='cont_cxjg'>
                <div id='con_two_3' style='display: none;'>
                </div>
            </div>
            <!--内容3-->
            <!--内容4-->
            <div class='cont_cxjg'>
                <div id='con_two_4' style='display: none;'>
                </div>
            </div>
            <!--内容4-->
            <!--内容5-->
            <div class='cont_cxjg'>
                <div id='con_two_5' style='display: none;'>
                </div>
            </div>
            <!--内容5-->
            <!--内容6-->
            <div class='cont_cxjg'>
                <div id='con_two_6' style='display: none;'>
                </div>
            </div>
        </div>
    </div>
</div>
<div id="divMembers">
    <asp:PlaceHolder ID="PlaceHolder" runat="server"></asp:PlaceHolder>
</div>
<div>
    <asp:PlaceHolder ID="PlaceHolderShowCstMember" runat="server"></asp:PlaceHolder>
</div>
<div id="divCstMembers">
    <asp:PlaceHolder ID="PlaceHolderCstMember" runat="server"></asp:PlaceHolder>
</div>
<div class="cont_cx khxx CustInfoArea">
    <div class="title ft16" style="clear: both">
        问卷调查 <a class="toggle hide" onclick="divShowHideEvent('divSurvey',this)" href="javascript:void(0)">
        </a>
    </div>
    <div id="divSurvey">
        <uc1:SurveyList ID="SurveyListID" runat="server" />
    </div>
</div>
<div class="cont_cx khxx CustInfoArea">
    <div class="title ft16" style="clear: both">
        记录历史 <a href="javascript:void(0)" onclick="divShowHideEvent('infoBlock1',this)" class="toggle hide">
        </a>
    </div>
    <div id="infoBlock1">
        <ul class="infoBlock firstPart">
            <li class="singleRow">
                <div style="float: left">
                    操作记录</div>
                <div id="divTaskLog" class="fullRow cont_cxjg" style="margin-left: 78px;">
                </div>
            </li>
            <li class="singleRow">
                <div style="float: left">
                    通话记录</div>
                <div id="divCallRecordList" class="fullRow cont_cxjg" style="margin-left: 78px;">
                </div>
            </li>
        </ul>
    </div>
</div>
