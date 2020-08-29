<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/Controls/Top.Master"
    Title="车商通会员查询" CodeBehind="CSTMemberSearch.aspx.cs" Inherits="BitAuto.ISDC.CC2012.Web.CustBaseInfo.CSTMemberSearch" %>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <script src="../Js/Enum/Area2.js" type="text/javascript"></script>
    <script type="text/javascript">
        $(document).ready(function () {
            //敲回车键执行方法
            enterSearch(CRMCSTMemberSearchHelper.search);

            BindProvince('<%=selProvince.ClientID %>'); //绑定省份
            CRMCSTMemberSearchHelper.search();
        });
       
    </script>
    <script type="text/javascript">
        var CRMCSTMemberSearchHelper = (function () {
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


            openSelectCustomerPopup = function () {//选择交易市场 
                $.openPopupLayer({
                    name: 'SelectCustomerPopup',
                    parameters: { type: 3 },
                    url: "/AjaxServers/Base/SelectCustomer.aspx?page=1",
                    beforeClose: function (e, data) {
                        if (e) {
                            $('#<%= this.tfCustPidName1.ClientID %>').val(data.CustName);
                            $('#<%= this.tfCustPid1.ClientID %>').val(data.CustID);
                        }
                    },
                    afterClose: function () {
                        //敲回车键执行方法
                        enterSearch(CRMCSTMemberSearchHelper.search);
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
                $('#' + contentElementId).load(
                    '/AjaxServers/Statistics/CSTMemberList.aspx',
                    $.extend({}, pagerConfig, defaultPostData, postData)
                );
            },

            search = function () {
                var params = {};
                var msg = '';
                //--------生成查询参数并进行校验--------
                var txtCSTMemberCode = $.trim($('#txtCSTMemberCode').val());
                var txtCSTMemberName = $.trim($('#txtCSTMemberName').val());
                var provinceID = $.trim($('#<%=selProvince.ClientID %>').val());
                var cityID = $.trim($('#<%=selCity.ClientID %>').val());
                var countyID = $.trim($('#<%=selCounty.ClientID %>').val());
                //所属交易市场,逗号隔开
                var tfCustPids = $.trim($('#<%=tfCustPid1.ClientID %>').val());

                //会员类型
                var memberTypeIDs = $(":checkbox[name='MemberType']:checked").map(function () {
                    return $(this).val();
                }).get().join(",");

                //总充值车商币
                var loggingAmountStart = $.trim($('#txtloggingAmountStart').val());
                var loggingAmountEnd = $.trim($('#txtloggingAmountEnd').val());

                //车商币余额
                //                var RemainAmountStart = $.trim($('#txtRemainAmountStart').val());
                //                var RemainAmountEnd = $.trim($('#txtRemainAmountEnd').val());

                //车商币有效期
                //                var AvailabilityTimeStart = $('#txtAvailabilityTimeStart').val();
                //                var AvailabilityTimeEnd = $('#txtAvailabilityTimeEnd').val();

                //总消费车商币
                var UserdAmountStart = $.trim($('#txtUserdAmountStart').val());
                var UserdAmountEnd = $.trim($('#txtUserdAmountEnd').val());
                var memberSyncStatus = $("#sltMemberStatus").val();
                if (verify()) {
                    params = {
                        CSTMemberCode: encodeURIComponent(txtCSTMemberCode),
                        CSTMemberName: encodeURIComponent(txtCSTMemberName),
                        ProvinceID: encodeURIComponent(provinceID),
                        CityID: encodeURIComponent(cityID),
                        CountyID: encodeURIComponent(countyID),
                        //所属交易市场
                        TFCustPids: encodeURIComponent(tfCustPids),
                        //会员类型
                        MemberTypeIDs: escapeStr(memberTypeIDs),
                        //总充值
                        LoggingAmountStarts: encodeURIComponent(loggingAmountStart),
                        LoggingAmountEnds: encodeURIComponent(loggingAmountEnd),

                        //车商币余额
                        //                        RemainAmountStarts: encodeURIComponent(RemainAmountStart),
                        //                        RemainAmountEnds: encodeURIComponent(RemainAmountEnd),

                        //                        //车商币有效期
                        //                        AvailabilityTimeStarts: encodeURIComponent(AvailabilityTimeStart),
                        //                        AvailabilityTimeEnds: encodeURIComponent(AvailabilityTimeEnd),

                        //总消费车商币
                        UserdAmountStarts: encodeURIComponent(UserdAmountStart),
                        UserdAmountEnds: encodeURIComponent(UserdAmountEnd),
                        //add by qizhiqiang 2012-4-13 为了判断调用查询是统计还是客户核实
                        ComeExcel: encodeURIComponent('1'),
                        MemberSyncStatus: encodeURIComponent(memberSyncStatus)
                    };
                    if (params) {
                        _query(params);
                    }
                }
            },

            verify = function () {
                var txtCSTMemberCode = $.trim($('#txtCSTMemberCode').val());
                if (!isNum(txtCSTMemberCode)) {
                    $.jAlert('会员ID格式不正确！');
                    return false;
                }
                var txtCSTMemberName = $('#txtCSTMemberName').val();

                //总充值车商币
                var loggingAmountStart = $.trim($('#txtloggingAmountStart').val());
                var loggingAmountEnd = $.trim($('#txtloggingAmountEnd').val());

                //车商币余额
                //                var RemainAmountStart = $.trim($('#txtRemainAmountStart').val());
                //                var RemainAmountEnd = $.trim($('#txtRemainAmountEnd').val());

                //车商币有效期
                //                var AvailabilityTimeStart = $('#txtAvailabilityTimeStart').val();
                //                var AvailabilityTimeEnd = $('#txtAvailabilityTimeEnd').val();

                //总消费车商币
                var UserdAmountStart = $.trim($('#txtUserdAmountStart').val());
                var UserdAmountEnd = $.trim($('#txtUserdAmountEnd').val());


                if (GetStringRealLength(txtCSTMemberCode) > 50) {
                    $.jAlert('会员ID超过50个字符！');
                    return false;
                }
                if (GetStringRealLength(txtCSTMemberName) > 256) {
                    $.jAlert('会员名称超过256个字符！');
                    return false;
                }
                var reg = new RegExp("-", "g");
                //总充值U
                if ((loggingAmountStart != '' && loggingAmountEnd == '') ||
                (loggingAmountStart == '' && loggingAmountEnd != '')) {
                    $.jAlert('您必须输入总充值车商币开始范围或总充值车商币结束范围'); return false;
                }
                if ($.trim(loggingAmountStart).length > 0) {
                    if (isNaN(loggingAmountStart) || loggingAmountStart <= 0) {
                        $.jAlert("您输入的总充值车商币开始范围格式不正确", function () {
                            $('#txtloggingAmountStart').val('');
                            $('#txtloggingAmountStart').focus();
                        });
                        return false;
                    }
                }
                if ($.trim(loggingAmountEnd).length > 0) {
                    if (isNaN(loggingAmountEnd) || loggingAmountEnd <= 0) {
                        $.jAlert("您输入的总充值车商币结束范围格式不正确", function () {
                            $('#txtloggingAmountEnd').val('');
                            $('#txtloggingAmountEnd').focus();
                        });
                        return false;
                    }
                }
                if (loggingAmountEnd != '' && loggingAmountStart != '') {
                    if (parseInt(loggingAmountEnd) - parseInt(loggingAmountStart) < 0) {
                        $.jAlert("您必须输入的总充值车商币开始范围必须小于等于总充值车商币结束范围");
                        return false;
                    }
                }


                //车商币余额
                //                if ((RemainAmountStart != '' && RemainAmountEnd == '') ||
                //                (RemainAmountStart == '' && RemainAmountEnd != '')) {
                //                    $.jAlert('您必须输入车商币余额开始范围或车商币余额结束范围'); return false;
                //                }
                //                if ($.trim(RemainAmountStart).length > 0) {
                //                    if (isNaN(RemainAmountStart) || RemainAmountStart <= 0) {
                //                        $.jAlert("您输入的车商币余额开始范围格式不正确", function () {
                //                            $('#txtRemainAmountStart').val('');
                //                            $('#txtRemainAmountStart').focus();
                //                        });
                //                        return false;
                //                    }
                //                }
                //                if ($.trim(RemainAmountEnd).length > 0) {
                //                    if (isNaN(RemainAmountEnd) || RemainAmountEnd <= 0) {
                //                        $.jAlert("您输入的车商币余额结束范围格式不正确", function () {
                //                            $('#txtRemainAmountEnd').val('');
                //                            $('#txtRemainAmountEnd').focus();
                //                        });
                //                        return false;
                //                    }
                //                }
                //                if (RemainAmountEnd != '' && RemainAmountStart != '') {
                //                    if (parseInt(RemainAmountEnd) - parseInt(RemainAmountStart) < 0) {
                //                        $.jAlert("您输入的车商币余额开始范围必须小于等于车商币余额结束范围");
                //                        return false;
                //                    }
                //                }


                //总消费车商币
                if ((UserdAmountStart != '' && UserdAmountEnd == '') ||
                (UserdAmountStart == '' && UserdAmountEnd != '')) {
                    $.jAlert('您必须输入总消费车商币开始范围或总消费车商币结束范围'); return false;
                }

                if ($.trim(UserdAmountStart).length > 0) {
                    if (isNaN(UserdAmountStart) || UserdAmountStart <= 0) {
                        $.jAlert("您输入的总消费车商币开始范围格式不正确", function () {
                            $('#txtUserdAmountStart').val('');
                            $('#txtUserdAmountStart').focus();
                        });
                        return false;
                    }
                }
                if ($.trim(UserdAmountEnd).length > 0) {
                    if (isNaN(UserdAmountEnd) || UserdAmountEnd <= 0) {
                        $.jAlert("您输入的总消费车商币结束范围格式不正确", function () {
                            $('#txtUserdAmountEnd').val('');
                            $('#txtUserdAmountEnd').focus();
                        });
                        return false;
                    }
                }
                if (UserdAmountEnd != '' && UserdAmountStart != '') {
                    if (parseInt(UserdAmountEnd) - parseInt(UserdAmountStart) < 0) {
                        $.jAlert("您输入的总消费车商币开始范围必须小于等于总消费车商币结束范围");
                        return false;
                    }
                }

                //车商币有效期
                //                if ((AvailabilityTimeStart != '' && AvailabilityTimeEnd == '') ||
                //                (AvailabilityTimeStart == '' && AvailabilityTimeEnd != '')) {
                //                    $.jAlert('您必须输入车商币有效期开始时间或车商币有效期结束时间'); return false;
                //                }

                //                if ($.trim(AvailabilityTimeStart).length > 0) {
                //                    if (!($.trim(AvailabilityTimeStart).isDate())) {
                //                        $.jAlert("您输入的车商币有效期开始时间格式不正确", function () {
                //                            $('#txtAvailabilityTimeStart').val('');
                //                            $('#txtAvailabilityTimeStart').focus();
                //                        });
                //                        return false;
                //                    }
                //                }
                //                if ($.trim(AvailabilityTimeEnd).length > 0) {
                //                    if (!($.trim(AvailabilityTimeEnd).isDate())) {
                //                        $.jAlert("您输入的车商币有效期结束时间格式不正确", function () {
                //                            $('#txtAvailabilityTimeEnd').val('');
                //                            $('#txtAvailabilityTimeEnd').focus();
                //                        });
                //                        return false;
                //                    }
                //                }


                //                if (AvailabilityTimeStart != '' && AvailabilityTimeEnd != '') {
                //                    if (Date.parse(AvailabilityTimeEnd.replace(reg, '/')) - Date.parse(AvailabilityTimeStart.replace(reg, '/')) < 0) {
                //                        $.jAlert("您输入的车商币有效期开始时间必须小于等于有效期结束时间");
                //                        return false;
                //                    }
                //                }

                return true;
            };

            return {
                triggerProvince: triggerProvince,
                triggerCity: triggerCity,
                initProvince: initProvince,
                //initAreaType: initAreaType,
                //openSelectBrandPopup: openSelectBrandPopup,
                openSelectCustomerPopup: openSelectCustomerPopup,
                search: search
            };
        })();
    </script>
    <div class="search clearfix">
        <ul>
            <li>
                <label>
                    会员ID：</label>
                <input type="text" name="txtCSTMemberCode" id="txtCSTMemberCode" class="w190" />
            </li>
            <li>
                <label>
                    会员名称：</label>
                <input type="text" id="txtCSTMemberName" name="txtCSTMemberName" class="w190" />
            </li>
            <li>
                <label>
                    会员地区：</label>
                <select id="selProvince" name="Province" class="w50" onchange="javascript:CRMCSTMemberSearchHelper.triggerProvince();"
                    runat="server">
                </select>
                <select id="selCity" name="City" class="w50" onchange="javascript:CRMCSTMemberSearchHelper.triggerCity();"
                    runat="server">
                    <option value="-1">城市</option>
                </select>
                <select id="selCounty" name="County" class="w50" runat="server">
                    <option value="-1">区/县</option>
                </select>
            </li>
        </ul>
        <ul class="clear">
            <li>
                <label>
                    总充值车商币：</label>
                <input type="text" class="w85" style="width: 84px; *width: 83px; width: 83px\9;"
                    id="txtloggingAmountStart" />
                至
                <input type="text" class="w85" style="width: 84px; *width: 83px; width: 83px\9;"
                    id="txtloggingAmountEnd" />
            </li>
            <li>
                <label>
                    总消费车商币：</label>
                <input type="text" class="w85" style="width: 84px; *width: 83px; width: 83px\9;"
                    id="txtUserdAmountStart" />
                至
                <input type="text" class="w85" style="width: 84px; *width: 83px; width: 83px\9;"
                    id="txtUserdAmountEnd" />
            </li>
            <li>
                <label>
                    所属交易市场：</label>
                <input id="tfCustPidName1" name="CustPidName1" readonly="readonly" runat="server"
                    class="w190" onclick="javascript:CRMCSTMemberSearchHelper.openSelectCustomerPopup(1);" />
                <input id="tfCustPid1" name="CustPid1" style="display: none;" runat="server" /></li>
        </ul>
        <ul class="clear">
            <li style=" margin-right:33px;">
                <label>
                    会员状态：</label>
                <select id="sltMemberStatus" name="txtOfficeTypeCode" style="width: 194px;">
                    <option value="-1">全部</option>
                    <option value="170001">已申请</option>
                    <option value="170002">已开通</option>
                    <option value="170008">已打回</option>
                    <option value="170003">开通失败</option>
                </select></li>
            <li style="width: 272px;">
                <label>
                    会员类别：</label>
                <label style="margin-right: 10px; width: auto;">
                    <input type="checkbox" value="3" style="border: none;" name="MemberType" class="checkbox" />
                    <em style="font-weight: normal">经纪公司</em>
                </label>
                <%--<label style="margin-right: 10px; width: auto;">
                                <input type="checkbox" value="1" style="border: none;" name="MemberType" class="checkbox" />
                                个人
                            </label>--%>
                <label style="margin-right: 10px; width: auto;">
                    <input type="checkbox" value="2" style="border: none;" name="MemberType" class="checkbox" />
                    <em style="font-weight: normal">4S店</em>
                </label>
                <%-- <label style="margin-right: 10px; width: auto;">
                                <input type="checkbox" value="4" style="border: none;" name="MemberType" class="checkbox" />
                                厂商
                            </label>
                            <label style="margin-right: 10px; width: auto;">
                                <input type="checkbox" value="5" style="border: none;" name="MemberType" class="checkbox" />
                                其他
                            </label>--%>
            </li>
            <li class="btnsearch">
                <input style="float: right;" name="" type="button" value="查 询" onclick="javascript:CRMCSTMemberSearchHelper.search()" />
            </li>
        </ul>
    </div>
    <div id="divQueryResultContent" class="cont_cxjg">
    </div>
</asp:Content>
