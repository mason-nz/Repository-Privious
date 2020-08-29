<%@ Page Title="查询变更信息" Language="C#" MasterPageFile="~/Controls/Top.Master" AutoEventWireup="true"
    CodeBehind="ExportChangedInfo.aspx.cs" Inherits="BitAuto.ISDC.CC2012.Web.CustAudit.ExportChangedInfo" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head1" runat="server">
    <script src="../Js/Enum/Area.js" type="text/javascript"></script>
    <script src="../Js/Enum/Area2.js" type="text/javascript"></script>
    <script type="text/javascript">
        $(function () {
            //敲回车键执行方法
            enterSearch(ExportChangedInfoSearchHelper.search);

            BindProvince('<%=ddlSearchMemberProvince.ClientID %>'); //绑定会员省份

            $("[id$=ddlSearchMemberProvince]").change(function () {
                BindCity('<%=ddlSearchMemberProvince.ClientID %>', '<%=ddlSearchMemberCity.ClientID %>');
                BindCounty('<%=ddlSearchMemberProvince.ClientID %>', '<%=ddlSearchMemberCity.ClientID %>', '<%=ddlSearchMemberCounty.ClientID %>');
            });
            $("[id$=ddlSearchMemberCity]").change(function () {
                BindCounty('<%=ddlSearchMemberProvince.ClientID %>', '<%=ddlSearchMemberCity.ClientID %>', '<%=ddlSearchMemberCounty.ClientID %>');
            });

            ExportChangedInfoSearchHelper.search();


            $('#txtCreateBeginTime').bind('click focus', function () { WdatePicker({ maxDate: '#F{$dp.$D(\'txtCreateEndTime\')}', onpicked: function () { document.getElementById("txtCreateEndTime").focus(); } }); });
            $('#txtCreateEndTime').bind('click focus', function () { WdatePicker({ minDate: '#F{$dp.$D(\'txtCreateBeginTime\')}' }); });
        });
    </script>
    <script type="text/javascript">

        var ExportChangedInfoSearchHelper = (function () {
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
                    '/AjaxServers/CustAudit/ExportChangeList.aspx',
                    $.extend({}, pagerConfig, defaultPostData, postData)
                );
            },



            search = function () {

                var params = getParams();
                if (params && verifyLogic()) {
                    LoadingAnimation("divQueryResultContent");
                    $('#divQueryResultContent').load('/AjaxServers/CustAudit/ExportChangeList.aspx', params);
                }
            },

            verifyLogic = function () {
                var reg = new RegExp("-", "g");
                var txtCustIDORMemberID = $.trim($('#txtCustIDORMemberID').val());
                var txtCustNameORMemberName = $.trim($('#txtCustNameORMemberName').val());
                var txtCreateBeginTime = $.trim($('#txtCreateBeginTime').val());
                var txtCreateEndTime = $.trim($('#txtCreateEndTime').val());
                var txtSeatTrueName = $.trim($('#txtSeatTrueName').val());
                var ddlTaskBatch = $.trim($('#ddlTaskBatch').val());

                if (GetStringRealLength(txtCustIDORMemberID) > 100) {
                    $.jAlert('客户或会员编号不能超过100个字符！');
                    return false;
                }
                if (GetStringRealLength(txtCustNameORMemberName) > 100) {
                    $.jAlert('客户或会员名称不能超过100个字符！');
                    return false;
                }
                if ($.trim(txtCreateBeginTime).length > 0) {
                    if (!($.trim(txtCreateBeginTime).isDate())) {
                        $.jAlert("您输入的开始时间格式不正确", function () {
                            $('#txtCreateBeginTime').val('');
                            $('#txtCreateBeginTime').focus();
                        });
                        return false;
                    }
                }
                if ($.trim(txtCreateEndTime).length > 0) {
                    if (!($.trim(txtCreateEndTime).isDate())) {
                        $.jAlert("您输入的结束时间格式不正确", function () {
                            $('#txtCreateEndTime').val('');
                            $('#txtCreateEndTime').focus();
                        });
                        return false;
                    }
                }
                if (Date.parse(txtCreateEndTime.replace(reg, '/')) - Date.parse(txtCreateBeginTime.replace(reg, '/')) < 0) {
                    $.jAlert("您输入的开始时间必须小于或等于结束时间");
                    return false;
                }
                if (GetStringRealLength(txtSeatTrueName) > 100) {
                    $.jAlert('坐席姓名不能超过100个字符！');
                    return false;
                }
                return true;
            };

            return {
                search: search
            };
        })();


        function getParams() {

            var params = {};
            var msg = '';
            //--------生成查询参数并进行校验--------
            var txtCustIDORMemberID = $.trim($('#txtCustIDORMemberID').val());
            var txtCustNameORMemberName = $.trim($('#txtCustNameORMemberName').val());
            var txtCreateBeginTime = $.trim($('#txtCreateBeginTime').val());
            var txtCreateEndTime = $.trim($('#txtCreateEndTime').val());
            //var ddlExportStatus = $.trim($('#ddlExportStatus').val());
            var ddlDisposeStatus = $.trim($('#ddlDisposeStatus').val());
            var txtSeatTrueName = $.trim($('#txtSeatTrueName').val());
            var ddlTaskBatch = $.trim($('#ddlTaskBatch').val());
            //var carType = $('input[name="carType"]:checked').map(function () { return $(this).val() }).get().join(",");

            //var radioType = $("input[name='radioType']:checked").val();
            var ddlContrastType = $.trim($('#ddlContrastType').val());
            //                if (radioType == "0") {
            //                    var ddlContrastType = $.trim($('#ddlContrastType').val());
            //                }
            //                else if (radioType == "1") {
            //                    var ddlContrastType = $.trim($('#ddlContrastCSTType').val());
            //                }

            //会员地区 add by qizq 绑定会员地区2012-5-21
            var ddlSearchMemberProvince = $.trim($("[id$='ddlSearchMemberProvince']").val());
            var ddlSearchMemberCity = $.trim($("[id$='ddlSearchMemberCity']").val());
            var ddlSearchMemberCounty = $.trim($("[id$='ddlSearchMemberCounty']").val());

            params = {
                CustIDORMemberID: encodeURIComponent(txtCustIDORMemberID),
                CustNameORMemberName: encodeURIComponent(txtCustNameORMemberName),
                CreateBeginTime: encodeURIComponent(txtCreateBeginTime),
                CreateEndTime: encodeURIComponent(txtCreateEndTime),
                //ExportStatus: encodeURIComponent(ddlExportStatus),
                ContrastType: encodeURIComponent(ddlContrastType),
                DisposeStatus: encodeURIComponent(ddlDisposeStatus),
                SeatTrueName: encodeURIComponent(txtSeatTrueName),
                TaskBatch: encodeURIComponent(ddlTaskBatch),
                //CarType: carType,
                //Type: encodeURIComponent(radioType)
                //add by qizhiqiang 2012-5-21
                MemberProvince: encodeURIComponent(ddlSearchMemberProvince),
                MemberCity: encodeURIComponent(ddlSearchMemberCity),
                MemberCounty: encodeURIComponent(ddlSearchMemberCounty)

            };
            return params;
        }

        //分页操作
        function ShowDataByPost1(pody) {
            LoadingAnimation("divQueryResultContent");
            $('#divQueryResultContent').load('/AjaxServers/CustAudit/ExportChangeList.aspx', pody);
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="search clearfix">
        <ul class="clear">
            <li>
                <label style="width: 100px;">
                    客户或会员编号：</label>
                <input type="text" name="txtCustIDORMemberID" id="txtCustIDORMemberID" class="w190"
                    style="width: 203px; margin-right: 2px; *width: 201px; *margin-right: 1px; width: 201px\9;">
            </li>
            <li>
                <label style="width: 100px;">
                    客户或会员名称：</label>
                <input type="text" id="txtCustNameORMemberName" name="txtCustNameORMemberName" class="w190"
                    style="width: 186px; *width: 188px; width: 188px\9;">
            </li>
            <li>
                <label style="width: 100px;">
                    变更类型：</label>
                <select id="ddlContrastType" name="ddlContrastType">
                    <option value="-1">请选择</option>
                    <option value="1">客户名称变更</option>
                    <option value="2">易湃会员4个关键项变更</option>
                    <option value="3">易湃会员其他信息变更</option>
                    <option value="4">客户名称变更（有重名）</option>
                    <option value="5">客户停用申请</option>
                    <option value="6">易湃会员主营品牌变更</option>
                    <option value="7">车商通会员4个关键项变更</option>
                    <option value="8">车商通会员3个关键项变更</option>
                    <option value="9">有排期车易通信息变更</option>
                </select>
            </li>
        </ul>
        <ul class="clear">
            <li>
                <label style="width: 100px;">
                    处理状态：</label>
                <select id="ddlDisposeStatus" name="ddlDisposeStatus" style="width: 206px">
                    <option value="-1">请选择</option>
                    <option value="0">未处理</option>
                    <option value="1">已处理</option>
                    <option value="2">未修改</option>
                </select>
            </li>
            <li>
                <label style="width: 100px;">
                    创建时间：</label>
                <input type="text" maxlength="10" style="width: 84px;" class="w190" id="txtCreateBeginTime"
                    name="txtCreateBeginTime"><span>至</span>
                <input type="text" maxlength="10" style="width: 84px;" class="w190" id="txtCreateEndTime"
                    name="txtCreateEndTime">
            </li>
            <li>
                <label style="width: 100px;">
                    坐席姓名：</label>
                <input type="text" id="txtSeatTrueName" name="txtSeatTrueName" class="w190" style="width: 155px;
                    *width: 160px; width: 160px\9;" />
            </li>
        </ul>
        <ul class="clear">
            <li>
                <label style="width: 100px;">
                    会员地区：</label>
                <select id="ddlSearchMemberProvince" style="width: 66px" name="ddlSearchMemberProvince"
                    class="kProvince" runat="server">
                </select>
                <select id="ddlSearchMemberCity" style="width: 66px" name="ddlSearchMemberCity" runat="server">
                    <option>城市</option>
                </select>
                <select id="ddlSearchMemberCounty" name="ddlSearchMemberCounty" class="kArea" style="width: 66px;"
                    runat="server">
                    <option>区县</option>
                </select></li>
            <li class="btnsearch">
                <input style="float: right" name="" type="button" value="查 询" onclick="javascript:ExportChangedInfoSearchHelper.search();" />
            </li>
        </ul>
    </div>
    <div class="optionBtn clearfix">
        <%if (right_btnExport)
          { %>
        <input type="button" id="btnCategory" value="导出变更详情" class="newBtn" onclick="ExportExcel()" />
        <%} %>
    </div>
    <div id="divQueryResultContent">
    </div>
</asp:Content>
