<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="UCEditSecondCarCust.ascx.cs"
    Inherits="BitAuto.ISDC.CC2012.Web.CustInfo.EditVWithCalling.UCEditSecondCarCust" %>
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
       loadTaskLog = function () {
           $('#divTaskLog').load('/CustInfo/DetailV/TaskLogList.aspx', {
               ContentElementId: 'divCustContacts',
               TID: '<%= this.TaskID %>',
               PageSize: 10
           });
       },
     loadCallRecord = function () {
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
            newCstMember = function () {//新建车商通会员
                var outer = $('<div/>');
                outer.load('/CustInfo/EditVWithCalling/EditCstMember.aspx', { TID: '<%= this.TaskID %>' }, function (jd, textStatus, xhr) {

                });
                $('#divCstMembers').prepend(outer);
            },
        openSelectCustomerPopup = function () {//选择交易市场 
            $.openPopupLayer({
                name: 'SelectCustomerPopup',
                parameters: { type: 3, ProvinceID: $('select[id$="selProvince"]', $('.CustInfoArea')).val() },
                url: "/AjaxServers/Base/SelectCustomer.aspx?page=1",
                beforeClose: function (e, data) {
                    if (e) {
                        $('#<%= this.tfCustPidName.ClientID %>').val(data.CustName);
                        $('#<%= this.tfCustPid.ClientID %>').val(data.CustID);
                    }
                }
            });
        },

        loadContacts = function () {//显示客户联系人
            $('#divCustContacts').load('/CustInfo/MoreInfo/CC_Contact/ListWithEdit.aspx', {
                ContentElementId: 'divCustContacts',
                TID: '<%= this.TaskID %>',
                PageSize: 10
            });
        },

        loadBusinessScale = function () {//显示二手车列表
            $('#divBusinessScaleList').load('/CustInfo/MoreInfo/CC_BusinessScale/List.aspx', {
                ContentElementId: 'divBusinessScaleList',
                TID: '<%=this.TaskID %>',
                Action: 'edit',
                IsPartShow: true
            });
        },

        loadCustUserList = function () {//显示负责员工列表
            $('#divCustUserList').load('/CustInfo/MoreInfo/CustUserList.aspx', {
                ContentElementId: 'divCustUserList',
                CustID: '<%=Task.CrmCustID %>',
                IsPartShow: true
            });
        },

        syncEleName = function (idFrom, idTo, eleType) {//同步控件名称
            if (memberAreaActionHelper.getCurrentMemerArea()) {
                if (!eleType) {
                    memberAreaActionHelper.getCurrentMemerArea().find('input[id$="' + idTo + '"]').val($('#' + idFrom).val());
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
            //同步易湃会员邮编
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
            //var custName = $('#' + custID).val();
            var body = { Action: 'Search', ExceptCustID: '<%= this.OriginalCustID %>', TID: '<%= this.TaskID %>', r: Math.random() };
            if (!flag)
                body.IsShowdDelCustRelation = true;
            if ((!flag && '<%=IsExistDelCustRelationRecord %>' == 'True') || flag)
                $('#divAddDelCustRelation').load('/CustInfo/EditVWithCalling/AddDelCustRelation.aspx', body);
        },
        //弹出标注信息保存层
        openSavePopup = function () {
            var info = uCEditCustHelper.getAllDataInPage();
            var msg = uCEditCustHelper.validateData(info.CustInfo, info.CstMemberInfoArray, false);
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
                        $.post('/CustInfo/SecondCarCustHandler.ashx?callback=?', {
                            Action: 'SaveCheckInfo',
                            CheckedInfo: data,
                            AdditionalStatus: encodeURIComponent(cData.AdditionalStatus),
                            G_description: encodeURIComponent(cData.G_description)
                        }, function (jd, status, xhr) {
                            if (status != 'success') { $.jAlert('请求错误'); return; }
                            if (jd.success) {
                                $.jPopMsgLayer('保存成功', function () {
                                    location = location;
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
            var msg = uCEditCustHelper.validateData(info.CustInfo, info.CstMemberInfoArray, true);
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
            var custAddress = $('#txtAddDelAddress', $('.CustInfoArea')).val();
            var custLinkName = $(':text[id$="tfContactName"]', $('.CustInfoArea')).val();
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
                    CustAddress: escape(custAddress),
                    CustLinkName: escape(custLinkName)
                },
                afterClose: function (effectiveAction, cData) {
                    if (effectiveAction) {//提交保存
                        $.post('/CustInfo/Handler.ashx?callback=?', {
                            Action: 'SubmitCheckInfo', CheckedInfo: data
                        }, function (jd, status, xhr) {
                            if (status != 'success') { $.jAlert('请求错误'); return; }
                            if (jd.success) {
                                $.jPopMsgLayer('提交成功', function () {
                                    if (urlGoto) {
                                        closePageExecOpenerSearch();
                                    }
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
            memberAreaActionHelper.triggerFirst();
            if ('<%= this.InitialCountyID %>'.toString().length > 0 && '<%= this.InitialCountyID %>' != '-1') {
                $('#<%= this.selCounty.ClientID %>').val('<%= this.InitialCountyID %>');
            }
            $('#two1').show();
            $('#two2').show();
            $('#two3').show();
            loadTabList(1, 1);
            loadTaskLog();
            loadCallRecord();
            //            if ($("#<%=selCustType.ClientID%>").val() == '20010' || $("#<%=selCustType.ClientID%>").val() == '20011') {
            //                $("#aNewCstMember").css("display", "block");
            //            }
        };

        return {
            triggerProvince: triggerProvince,
            triggerCity: triggerCity,
            init: init,
            custNameExist: custNameExist,
            SearchSameCustName: SearchSameCustName,
            openSavePopup: openSavePopup,
            LoadBusinessScale: loadBusinessScale,
            PopupDelCustRelation: PopupDelCustRelation,
            OpenSelectCustomerPopup: openSelectCustomerPopup,
            newCstMember: newCstMember,
            syncEleName: syncEleName,
            sysMemberName: sysMemberName,
            sysMemberAbbr: sysMemberAbbr,
            syncAreaName: syncAreaName,
            sysZipcode: sysZipcode,
            sysAddress: sysAddress
        }
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
                $('#divCstMembers .MemberInfoArea:visible').hide('fast');
                content.show('fast');
                currentMemberArea = content;
            }
        },

        triggerFirst = function () {
            $('#divCstMembers .MemberInfoArea:not(:first)').hide(); //如果一开始隐藏，隐藏区域的地图位置会偏移
            var f = $('#divCstMembers .MemberInfoArea:first');
            if (f && f.length > 0) { f.show('fast'); currentMemberArea = f; }
        },

        init = function () {
            currentMemberArea = $('#divCstMembers').find('.MemberInfoArea:visible');
            if (currentMemberArea.length == 0) { currentMemberArea = null; }
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
            OfficeTel: $.trim($('input[id$="tfOfficeTel"]', cia).val()),
            Fax: $.trim($('input[id$="tfFax"]', cia).val()),
            ContactName: $.trim($('input[id$="tfContactName"]', cia).val()),
            Notes: $.trim($('textarea[id$="tfNotes"]', cia).val()),
            DelRelationCustIDs: $("#ulSameCustName table.cxjg tr:gt(0)").map(function () { return $.trim($(this).find('td:first :checkbox').val()); }).get().join(","),
            DelRelationCustIDsContainLock: lockFlag,

            CarType:$(":checked[name$='CarType']",cia).map(function(){
            if($(this).attr("checked"))
            {
            return $(this).val();
            }
            }).get().join(','),
            CtsMemberID:$.trim($('input[id$="txtCtsMemberID"]',cia).val()).replace('，',','),
            TradeMarketID:$('#<%= this.tfCustPid.ClientID %>').val()
        };

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
                    SuperID:$.trim($('select[id$="sltSuperId"]', area).val()),
                    Address: $.trim($('input[id$="tfCstMemberAddress"]', area).val()),
                    PostCode: $.trim($('input[id$="tfCstMemberPostCode"]', area).val()),
                   Brand: $.trim($('input[id$="tfCstMemberBrand"]', area).val()),
                    LinkManName: $.trim($('input[id$="tfLinkManName"]', area).val()),
                    Mobile: $.trim($('input[id$="tfLinkManMobile"]', area).val()),
                    Email: $.trim($('input[id$="tfLinkManEmail"]', area).val())
                }
            }
            else { return null; }
        }

        return { CustInfo: custInfo,CstMemberInfoArray:cstMemberInfos };
    };

    uCEditCustHelper.validateData = function (custInfo,cstMemberInfoArray, onSubmit) {//onSubmit 指提交时的检查，默认是保存
        var msg = '';
        if (custInfo) {   
            if (custInfo.CustName.length == 0) { msg += '客户名称不可为空<br/>'; }
            if (onSubmit && (custInfo.TypeID.length == 0 || custInfo.TypeID == -1)) { msg += '必须选择客户类别<br/>'; }
            if (onSubmit && (custInfo.IndustryID.length == 0 || custInfo.IndustryID == -1)) { msg += '必须选择客户行业<br/>'; }
            if (onSubmit && (custInfo.CityID.length == 0 || custInfo.CityID == -1)) { msg += '必须选择客户地区<br/>'; }
            if (onSubmit && custInfo.Address.length == 0) { msg += '注册地址不可为空<br/>'; }
            if (onSubmit && custInfo.Zipcode.length == 0) { msg += '邮政编码不可为空<br/>'; }
            if (onSubmit && custInfo.Pid=='' && custInfo.TypeID == <%=(int)BitAuto.YanFa.Crm2009.Entities.EnumCustomType.Showroom %> ) { msg += '当客户类别为“展厅”时，必须选择客户所属4S<br/>'; }
        }
       if (cstMemberInfoArray && cstMemberInfoArray.length > 0) {
            for (var i = 0; i < cstMemberInfoArray.length; i++) {
                var mi = cstMemberInfoArray[i];
                if (mi.MemberName.length == 0) { msg += (mi.MemberName + '，会员名称不可为空<br/>'); }
                if (onSubmit && mi.MemberAbbr.length == 0) { msg += (mi.MemberName + '，会员简称不可为空<br/>'); }
                if (onSubmit && Len(mi.MemberAbbr) >16) { msg += (mi.MemberName + '，会员简称不能大于16个字符<br/>'); }

                if (onSubmit && (mi.CityID.length == 0 || mi.CityID == -1)) { msg += (mi.MemberName + '，必须选择会员地区<br/>'); }
                
               if(!$("select[id$='sltCstLinkMan']:eq(" + i + ")").attr("disabled")){
               //if (onSubmit && mi.VendorCode.length == 0) { msg += (mi.MemberName + '，会员编码不能为空<br/>'); }
               if (onSubmit && mi.SuperID==-1) { msg += (mi.SpanMemberName + '，请选择上级公司<br/>'); }

                if (onSubmit && (mi.LinkManName.length==0)) { msg += (mi.MemberName + '，联系人名称不能为空<br/>');}
//                if (onSubmit && (mi.Department.length==0)) { msg += (mi.MemberName + '，联系人部门不能为空<br/>');}
//                if (onSubmit && (mi.Position.length==0)) { msg += (mi.MemberName + '，联系人职位不能为空<br/>');}
                if (onSubmit && (mi.Mobile.length==0)) { msg += (mi.MemberName + '，联系人手机不能为空<br/>');}
                if(mi.Mobile.length>0&&!isMobile(mi.Mobile)){msg+=(mi.MemberName+',联系人手机格式不正确<br/>');}
                if(mi.Email.length>0&&isEmail(mi.Email)==false){msg+=mi.MemberName+',邮箱格式不正确<br/>'};
                if (onSubmit && (mi.Email.length==0)) { msg += (mi.MemberName + '，联系人邮箱不能为空<br/>');}
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
        return data;
    };

    uCEditCustHelper.saveInfo = function () {//保存

        var info = uCEditCustHelper.getAllDataInPage();
        var msg = uCEditCustHelper.validateData(info.CustInfo,info.CstMemberInfoArray, false);
        if (msg) { $.jAlert(msg); return; }

        info = uCEditCustHelper.encodeParams(info);
        var data = JSON.stringify(info);

        $.post('/CustInfo/SecondCarCustHandler.ashx?callback=?', {
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
    $.jConfirm("确定要提交核实的信息吗？如果提交后将进入审核阶段，座席将不能操作该客户及会员信息！",function(flag){
         if(flag)
         {
         var custName = $('#<%= this.tfCustName.ClientID %>').val();
            $.getJSON('/CustInfo/SecondCarCustHandler.ashx?callback=?', {
                Action: 'CustNameExist',
                CustName: encodeURIComponent($.trim(custName || '')),
                CustID: '<%= this.OriginalCustID %>'
            }, function (jd, textStatus, xhr) {
                if (textStatus != 'success') { $.jAlert('请求错误'); return; }
                if (jd.success) {
                    var info = uCEditCustHelper.getAllDataInPage();
                    var msg = uCEditCustHelper.validateData(info.CustInfo,info.CstMemberInfoArray, true);
                    if (msg) { $.jAlert(msg); return; }
                    info = uCEditCustHelper.encodeParams(info);
                    var data = JSON.stringify(info);
                    var lockFlag = false;
                    $.each($("#ulSameCustName table.cxjg tr:gt(0) td img[status]"), function (i, n) {
                        if ($(n).attr('status') == '1') {
                            lockFlag = true; return false;
                        }
                    });
                    if('<%=Task.Source %>'=='1' && 
                       ($("#ulSameCustName table.cxjg tr:gt(0)").size() > 0||jd.result == '1'))//Excel新增客户,用到“查重”功能或客户名称在CRM库存在
                        {$.jAlert('此客户在CRM库中已存在，请不要重复添加<br>' + jd.message, function () { return; });return;}

                    if (jd.result == '1') {//用户名称是否存在，1-存在，0-不存在
                        
                        $.jConfirm('CRM库中已存在此客户名称<br>' + jd.message + '<br>确定要添加重名申请吗？', function (flag) {
                            if (flag) {
                                var body = { Action: 'Search', ExceptCustID: '<%= this.OriginalCustID %>', TID: '<%= this.TaskID %>', CustName: escape($.trim(custName)),CustNameAllMatch:'yes', r: Math.random() };
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

                                        $.post('/CustInfo/SecondCarCustHandler.ashx?callback=?', {
                                            Action: 'AddDeleteCustRelationInfo',
                                            TID: '<%= this.TaskID %>',
                                            CustID: '<%= this.OriginalCustID %>',
                                            DelRelationCustIDs: $("#ulSameCustName table.cxjg tr:gt(0)").map(function () { return $.trim($(this).find('td:first :checkbox').val()); }).get().join(","),
                                            DescriptionWhenDelete: ''
                                        }, function (jd, textStatus, xhr) {
                                            if (textStatus != 'success') { $.jAlert('请求错误'); }
                                            else if (jd.success) {
                                                $.post('/CustInfo/SecondCarCustHandler.ashx?callback=?', {
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
                            $.post('/CustInfo/SecondCarCustHandler.ashx?callback=?', {
                                Action: 'AddDeleteCustRelationInfo',
                                TID: '<%= this.TaskID %>',
                                CustID: '<%= this.OriginalCustID %>',
                                DelRelationCustIDs: $("#ulSameCustName table.cxjg tr:gt(0)").map(function () { return $.trim($(this).find('td:first :checkbox').val()); }).get().join(","),
                                DescriptionWhenDelete: ''
                            }, function (jd, textStatus, xhr) {
                                if (textStatus != 'success') { $.jAlert('请求错误'); }
                                else if (jd.success) {
                                    $.post('/CustInfo/SecondCarCustHandler.ashx?callback=?', {
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
        //所属交易市场，隐藏“新建车商通会员”链接
        if ($('select[id$="selCustType"]').val() == "20012") {
            $("#aNewCstMember").hide();
        }
    });
    //客户类别下拉列表变更事件
    function CustTypeOnChange(ddlObj) {

        switch ($('select[id$="selCustType"]').val()) {
            case "20010":
            case "20011":
                $("#aNewCstMember").css("display", "block");
                break;
            case "20012": //所属交易市场，隐藏“新建车商通会员”链接
                $("#aNewCstMember").hide();
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
                        $('select[id$="selCustType"]').val($("#hdnCustType").val());
                    }
                }
                $("#aNewCstMember").css("display", "none");
                break;
        }
        $('#divCstMembers').find('.MemberInfoArea').each(function (i, v) {
            switch ($('select[id$="selCustType"]').val()) {
                case "20010":
                case "20011":
                    $('select[id$="selCstMemberType"]', $(v)).val("3");
                    break;
            }
        });
        $("#hdnCustType").val($('select[id$="selCustType"]').val());
    };
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
            case 2: //负责员工
                $("#con_two_2").load("/CustInfo/MoreInfo/CustUserList.aspx", { CustID: '<%=Task.CrmCustID %>', IsPartShow: true, ContentElementId: 'con_two_2' }, function () { });
                break;
            case 3: //二手车规模
                $("#con_two_3").load("/CustInfo/MoreInfo/CC_BusinessScale/List.aspx", { TID: '<%=this.TaskID %>', ContentElementId: 'con_two_3' }, function () { });
                break;
        }
    }
</script>
<div class="cont_cx khxx CustInfoArea">
    <div class="title ft16">
        基本信息 <a href="javascript:void(0)" onclick="divShowHideEvent('divCustBasicInfo',this)"
            class="toggle hide"></a><a href="javascript:uCEditCustHelper.newCstMember();" id="aNewCstMember"
                style="float: right; margin-right: 40px; *margin-top: -30px;">新建车商通会员</a>
    </div>
    <div id="divCustBasicInfo">
        <ul class="infoBlock firstPart">
            <li>
                <label style="width: 100px;">
                    客户名称：</label>
                <input id="tfCustName" name="CustName" class="w250" runat="server" />
                <em>*</em>
                <%--<a href="javascript:uCEditCustHelper.custNameExist($('#<%= this.tfCustName.ClientID %>').val());">
                    查重</a>--%>
                <a href="javascript:uCEditCustHelper.sysMemberName();">同步</a> <a href="javascript:uCEditCustHelper.SearchSameCustName(true);">
                    查重</a> </li>
            <li>
                <label style="width: 100px;">
                    客户简称：</label>
                <input id="tfCustAbbr" class="w250" name="CustAbbr" runat="server" />
                <a href="javascript:uCEditCustHelper.sysMemberAbbr();">同步</a> </li>
        </ul>
        <div id="divAddDelCustRelation">
        </div>
        <ul class="infoBlock firstPart">
            <li>
                <label style="width: 100px;">
                    客户类别：</label>
                <select id="selCustType" name="CustType" class="w255" runat="server" onchange="javascript:CustTypeOnChange(this);">
                </select>
                <input type="hidden" id="hdnCustType" />
                <em>*</em> </li>
            <li>
                <label style="width: 100px;">
                    经营范围：</label>
                <label style="width: 45px; margin-right: 10px;">
                    <input type="checkbox" id="chkNewCarType" name="CarType" value="1" style="height: auto;
                        width: auto;" disabled="disabled" />新车</label>
                <label style="width: 65px;">
                    <input type="checkbox" id="chkOldCarType" name="CarType" value="2" style="height: auto;
                        width: auto;" checked="true" disabled="disabled" runat="server" />二手车</label>
            </li>
            <li>
                <label style="width: 100px;">
                    客户行业：</label>
                <select id="selCustIndustry" class="w255" name="CustIndustry" runat="server">
                </select>
                <em>*</em> </li>
            <li>
                <label style="width: 100px;">
                    客户地区：</label>
                <select id="selProvince" name="Province" style="width: 82px;" class="area" onchange="javascript:uCEditCustHelper.triggerProvince();"
                    runat="server">
                </select>
                <select id="selCity" name="City" style="width: 82px;" class="area" onchange="javascript:uCEditCustHelper.triggerCity();"
                    runat="server">
                </select>
                <select id="selCounty" name="County" style="width: 82px;" class="lastArea" runat="server">
                </select>
                <em>*</em> <a href="javascript:uCEditCustHelper.syncAreaName();">同步</a> </li>
            <li>
                <label style="width: 100px;">
                    邮政编码：</label>
                <input id="tfZipcode" name="Zipcode" class="w250" runat="server" />
                <em>*</em> <a href="javascript:uCEditCustHelper.sysZipcode();">同步</a> </li>
            <li>
                <label style="width: 100px;">
                    注册地址：</label>
                <input id="tfAddress" name="Address" class="w250" runat="server" />
                <em>*</em> <a href="javascript:uCEditCustHelper.sysAddress();">同步</a></li>
        </ul>
        <div class="spliter">
        </div>
        <ul class="infoBlock ">
            <li id="liCustStatus" runat="server" style="display: none;">
                <label style="width: 100px;">
                    客户状态：</label>
                <img style="margin-left: 5px;" id="imgCustStatus" runat="server">
            </li>
            <li id="liCustLock" runat="server" style="display: none;">
                <label style="width: 100px;">
                    锁定：</label>
                <img style="margin-left: 5px;" id="imgCustStatusLock" runat="server" />
            </li>
            <li>
                <label style="width: 100px;">
                    客户级别：</label>
                <select id="selCustLevel" class="w255" name="CustLevel" runat="server">
                </select>
            </li>
            <li>
                <label style="width: 100px;">
                    所属交易市场：</label>
                <input id="tfCustPidName" name="CustPidName" class="w250" readonly="readonly" runat="server" />
                <input id="tfCustPid" name="CustPid" style="display: none;" runat="server" />
                <span><a href="javascript:uCEditCustHelper.OpenSelectCustomerPopup(1);">
                    <img border="0" src="/images/button_001.gif" /></a> </span></li>
            <li>
                <label style="width: 100px;">
                    联系人：</label>
                <input id="tfContactName" class="w250" name="ContactName" runat="server" />
            </li>
            <li>
                <label style="width: 100px;">
                    电话：</label>
                <input id="tfOfficeTel" name="OfficeTel" class="w250" runat="server" />
                <span><a href="javascript:void(0);" onclick="功能废弃">
                    <img alt="打电话" src="/images/phone.gif" border="0" /></a> </span></li>
            <li>
                <label style="width: 100px;">
                    传真：</label>
                <input id="tfFax" name="Fax" class="w250" runat="server" />
            </li>
            <li class="singleRow">
                <label style="width: 100px;">
                    备注：</label>
                <textarea id="tfNotes" name="Notes" cols="4" rows="2" class="fullRow" runat="server"></textarea>
            </li>
        </ul>
        <div class="spliter">
        </div>
        <div class="hd2">
            <ul>
                <li id='two1' onclick="setTab(1)" class='hover'>客户联系人</li>
                <li id='two2' onclick="setTab(2)" style='display: none;' class=''>负责员工</li>
                <li id='two3' onclick="setTab(3)" style='display: none;' class=''>二手车规模</li>
            </ul>
        </div>
        <div class="menuConbox menuConbox2">
            <!--客户联系人-->
            <div class='cont_cxjg'>
                <div id='con_two_1' class='hover'>
                </div>
            </div>
            <!--负责员工-->
            <div class='cont_cxjg'>
                <div id='con_two_2' style='display: none;'>
                </div>
            </div>
            <!--二手车规模-->
            <div class='cont_cxjg'>
                <div id='con_two_3' style='display: none;'>
                </div>
            </div>
        </div>
    </div>
</div>
<div id="divMembers">
    <asp:PlaceHolder ID="PlaceHolderDMSMember" runat="server"></asp:PlaceHolder>
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
