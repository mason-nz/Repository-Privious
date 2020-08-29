<%@ Page Language="C#" MasterPageFile="~/Controls/Top.Master" AutoEventWireup="true"
    CodeBehind="MemberCheckList.aspx.cs" Title="会员二次核实" Inherits="BitAuto.ISDC.CC2012.Web.CustCheck.MemberCheckList" %>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <script src="../Js/json2.js" type="text/javascript"></script>
    <script type="text/javascript">
        $(document).ready(function () {



            $('#txtApplyStartTime').bind('click focus', function () { WdatePicker({ maxDate: '#F{$dp.$D(\'txtApplyEndTime\')}', onpicked: function () { document.getElementById("txtApplyEndTime").focus(); } }); });
            $('#txtApplyEndTime').bind('click focus', function () { WdatePicker({ minDate: '#F{$dp.$D(\'txtApplyStartTime\')}' }); });


            $('#txtMemberOptStartTime').bind('click focus', function () { WdatePicker({ maxDate: '#F{$dp.$D(\'txtMemberOptEndTime\')}', onpicked: function () { document.getElementById("txtMemberOptEndTime").focus(); } }); });
            $('#txtMemberOptEndTime').bind('click focus', function () { WdatePicker({ minDate: '#F{$dp.$D(\'txtMemberOptStartTime\')}' }); });

        });

        //分页操作
        function ShowDataByPost1(pody) {
            LoadingAnimation('divQueryResultContent');
            $('#divQueryResultContent').load('/AjaxServers/MemberCheck/MemberCheckList.aspx', pody, LoadDivSuccess);
        }
        var memberCheckHelper = (function () {
            //默认查询参数（全局）
            var defaultPostData = {},
            //列表容器ID
            contentElementId = 'divQueryResultContent',
            //配置分页控件
            pagerConfig = {
                ContentElementId: contentElementId
            },

            verifyLogic = function () {
                var txtMemberName = $.trim($('#txtMemberName').val());
                var txtMemberAddr = $.trim($('#txtMemberAddr').val());
                var txtCustName = $.trim($('#txtCustName').val());
                var txtCustID = $.trim($('#txtCustID').val());
                var txtApplyStartTime = $.trim($('#txtApplyStartTime').val());
                var txtApplyEndTime = $.trim($('#txtApplyEndTime').val());
                var txtApplyUserName = $.trim($('#txtApplyUserName').val());
                var txtMemberOptStartTime = $.trim($('#txtMemberOptStartTime').val());
                var txtMemberOptEndTime = $.trim($('#txtMemberOptEndTime').val());

                if (GetStringRealLength(txtMemberName) > 100) {
                    $.jAlert("会员名称长度不能超过100个字符");
                    return false;
                }
                if (GetStringRealLength(txtMemberAddr) > 50) {
                    $.jAlert("会员简称长度不能超过50个字符");
                    return false;
                }
                if (GetStringRealLength(txtCustName) > 100) {
                    $.jAlert("客户名称长度不能超过100个字符");
                    return false;
                }
                if (GetStringRealLength(txtCustID) > 50) {
                    $.jAlert("客户简称长度不能超过50个字符");
                    return false;
                }
                if (GetStringRealLength(txtApplyUserName) > 50) {
                    $.jAlert("申请人长度不能超过50个字符");
                    return false;
                }
                if ($.trim(txtApplyStartTime).length > 0) {
                    if (!($.trim(txtApplyStartTime).isDate())) {
                        $.jAlert("申请开始时间格式不正确", function () {
                            $('#txtApplyStartTime').val('');
                            $('#txtApplyStartTime').focus();
                            return false;
                        });
                    }
                }
                if ($.trim(txtApplyEndTime).length > 0) {
                    if (!($.trim(txtApplyEndTime).isDate())) {
                        $.jAlert("申请结束时间格式不正确", function () {
                            $('#txtApplyEndTime').val('');
                            $('#txtApplyEndTime').focus();
                            return false;
                        });
                    }
                }
                if ($.trim(txtMemberOptStartTime).length > 0) {
                    if (!($.trim(txtMemberOptStartTime).isDate())) {
                        $.jAlert("审批开始时间格式不正确", function () {
                            $('#txtMemberOptStartTime').val('');
                            $('#txtMemberOptStartTime').focus();
                            return false;
                        });
                    }
                }
                if ($.trim(txtMemberOptEndTime).length > 0) {
                    if (!($.trim(txtMemberOptEndTime).isDate())) {
                        $.jAlert("审批结束时间格式不正确", function () {
                            $('#txtMemberOptEndTime').val('');
                            $('#txtMemberOptEndTime').focus();
                            return false;
                        });
                    }
                }
                return true;
            },

            search = function (params) {
                if (verifyLogic()) {
                    var msg = '';
                    //--------生成查询参数并进行校验--------
                    var txtMemberName = $.trim($('#txtMemberName').val());
                    var txtMemberAddr = $.trim($('#txtMemberAddr').val());
                    var txtCustName = $.trim($('#txtCustName').val());
                    var txtCustID = $.trim($('#txtCustID').val());
                    var txtApplyStartTime = $.trim($('#txtApplyStartTime').val());
                    var txtApplyEndTime = $.trim($('#txtApplyEndTime').val());
                    var txtApplyUserName = $.trim($('#txtApplyUserName').val());
                    var txtMemberOptStartTime = $.trim($('#txtMemberOptStartTime').val());
                    var txtMemberOptEndTime = $.trim($('#txtMemberOptEndTime').val());

                    var ckxDMSSyncStatus = $(':checkbox[id^="ckxDMSSyncStatus_"][checked]').map(function () { return $(this).val(); }).get().join(",");
                    var chkDMSStatus = $(':checkbox[id^="chkDMSStatus"][checked]').map(function () { return $(this).val(); }).get().join(",");

                    var radioType = $("input[name='radioType']:checked").val();

                    var pageSize = $("#hidSelectPageSize").val();

                    var _params = {
                        MemberName: encodeURIComponent(txtMemberName),
                        MemberAddr: encodeURIComponent(txtMemberAddr),
                        CustName: encodeURIComponent(txtCustName),
                        CustID: encodeURIComponent(txtCustID),
                        ApplyStartTime: encodeURIComponent(txtApplyStartTime),
                        ApplyEndTime: encodeURIComponent(txtApplyEndTime),
                        ApplyUserName: encodeURIComponent(txtApplyUserName),
                        MemberOptStartTime: encodeURIComponent(txtMemberOptStartTime),
                        MemberOptEndTime: encodeURIComponent(txtMemberOptEndTime),
                        DMSSyncStatus: encodeURIComponent(ckxDMSSyncStatus),
                        DMSStatus: encodeURIComponent(chkDMSStatus),
                        Type: encodeURIComponent(radioType),
                        pageSize: pageSize,
                        r: Math.random()
                    };
                    //var innerParams = _params;
                    //innerParams.QueryParams = encodeURIComponent(JSON.stringify(_params));

                    var podyStr = JsonObjToParStr($.extend({}, pagerConfig, defaultPostData, _params, params));

                    //加载查询结果
                    LoadingAnimation(contentElementId);
                    $('#' + contentElementId).load(
                    '/AjaxServers/MemberCheck/MemberCheckList.aspx', podyStr
                    );
                }
            };

            return {
                search: search
            };
        })();
    </script>
    <script type="text/javascript">
        $(function () {

            //敲回车键执行方法
            enterSearch(memberCheckHelper.search);
            memberCheckHelper.search();

            //填充空格，以使checkbox对齐，按照3个字对齐，少一个字填充3个&nbsp;
            $("em").each(function () {
                var em_val = $.trim($(this).html());
                var add_nbsp = "";
                for (var i = em_val.length; i < 3; i++) {
                    add_nbsp += "&nbsp;&nbsp;&nbsp;";
                }
                $(this).html(em_val + add_nbsp);
            });

            //            memberCheckHelper.search({
            //                Page: '<%= BitAuto.ISDC.CC2012.Web.Util.PagerHelper.GetCurrentPage() %>',
            //                PageSize: '<%= BitAuto.ISDC.CC2012.Web.Util.PagerHelper.GetPageSize() %>'
            //            });
        });
    </script>
    <style type="text/css">
        .pageP
        {
            width: 200px;
            float: left;
            text-align: left;
            padding-left: 20px;
        }
        
        .pageP a.selectA
        {
            color: Red;
        }
        .pageP a
        {
            height: 50px;
        }
        .pageP a:hover
        {
            font-size: 16px;
        }
    </style>
    <div class="search clearfix">
        <ul>
            <li>
                <label>
                    会员名称：</label>
                <input type="text" id="txtMemberName" name="txtMemberName" class="w190" />
            </li>
            <li>
                <label>
                    会员简称：</label>
                <input type="text" id="txtMemberAddr" name="txtMemberAddr" class="w190" />
            </li>
            <li>
                <label>
                    客户名称：</label>
                <input type="text" id="txtCustName" name="txtCustName" class="w190" />
            </li>
        </ul>
        <ul class="clear">
            <li>
                <label>
                    客户ID：</label>
                <input type="text" id="txtCustID" name="txtCustID" class="w190" />
            </li>
            <li>
                <label>
                    申请时间：</label>
                <input type="text" name="txtApplyStartTime" id="txtApplyStartTime" class="w85" style="width: 84px;
                    *width: 83px; width: 83px\9;" />
                至
                <input type="text" name="txtApplyEndTime" id="txtApplyEndTime" class="w85" style="width: 84px;
                    *width: 83px; width: 83px\9;" />
            </li>
            <li>
                <label>
                    申请人：</label>
                <input type="text" id="txtApplyUserName" name="txtApplyUserName" class="w190" />
            </li>
        </ul>
        <ul class="clear">
            <li style="width: 282px; *width: 281px; width: 284px\9">
                <label>
                    CRM会员状态：</label>
                <span>
                    <input type="checkbox" id="chkDMSStatusNormal" value="0" /><em onclick="emChkIsChoose(this)">正常</em></span>
                <span>
                    <input type="checkbox" id="chkDMSStatusDel" value="-1" /><em onclick="emChkIsChoose(this)">删除</em></span>
            </li>
            <li style="width: 282px; *width: 281px; width: 284px\9">
                <label>
                    会员类型：</label>
                <span>
                    <input type="radio" checked="checked" id="radioDMSType" value="0" name="radioType" /><em
                        onclick="emChkIsChoose(this)">易湃</em></span> <span>
                            <input type="radio" id="radioCSTType" value="1" name="radioType" /><em onclick="emChkIsChoose(this)">车商通</em></span>
            </li>
            <li>
                <label>
                    审批时间：</label>
                <input type="text" name="txtMemberOptStartTime" id="txtMemberOptStartTime" class="w85"
                    style="width: 84px; *width: 83px; width: 83px\9;" />
                至
                <input type="text" name="txtMemberOptEndTime" id="txtMemberOptEndTime" class="w85"
                    style="width: 84px; *width: 83px; width: 83px\9;" />
            </li>
        </ul>
        <ul class="clear">
            <li>
                <label>
                    申请状态：</label>
                <span>
                    <input type="checkbox" id="ckxDMSSyncStatus_170001" value="170001" name="ckxDMSSyncStatus_170001" /><em
                        onclick="emChkIsChoose(this)">审批中</em></span> <span>
                            <input type="checkbox" id="ckxDMSSyncStatus_170002" value="170002" name="ckxDMSSyncStatus_170002" /><em
                                onclick="emChkIsChoose(this)">通过</em></span><span>
                                    <input type="checkbox" id="ckxDMSSyncStatus_170003" value="170003" name="ckxDMSSyncStatus_170003" /><em
                                        onclick="emChkIsChoose(this)">失败</em></span><span>
                                            <input type="checkbox" id="ckxDMSSyncStatus_170008" value="170008" name="ckxDMSSyncStatus_170008" /><em
                                                onclick="emChkIsChoose(this)">打回</em></span>
            </li>
            <li class="btnsearch">
                <input style="float: right;" name="" id="btnSearch" type="button" value="查 询" onclick="javascript:memberCheckHelper.search();" />
            </li>
        </ul>
        <input type="hidden" id="hidSelectPageSize" value="" />
    </div>
    <div id="divQueryResultContent">
    </div>
</asp:Content>
