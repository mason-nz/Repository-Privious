<%@ Page Title="项目查询" Language="C#" MasterPageFile="~/Controls/Top.Master" AutoEventWireup="true"
    CodeBehind="YTGActivityProjectList.aspx.cs" Inherits="BitAuto.ISDC.CC2012.Web.YTGActivityTask.YTGActivityProjectList" %>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <script src="../Js/jquery.blockUI.js" type="text/javascript"></script>
    <script type="text/javascript" language="javascript">
        $(function () {

            $('#CBeginTime').bind('click focus', function () { WdatePicker({ maxDate: '#F{$dp.$D(\'CEndTime\')}', onpicked: function () { document.getElementById("CEndTime").focus(); } }); });
            $('#CEndTime').bind('click focus', function () { WdatePicker({ minDate: '#F{$dp.$D(\'CBeginTime\')}' }); });

            $('#BBeginTime').bind('click focus', function () { WdatePicker({ maxDate: '#F{$dp.$D(\'BEndTime\')}', onpicked: function () { document.getElementById("BEndTime").focus(); } }); });
            $('#BEndTime').bind('click focus', function () { WdatePicker({ minDate: '#F{$dp.$D(\'BBeginTime\')}' }); });

            $('#HBeginTime').bind('click focus', function () { WdatePicker({ maxDate: '#F{$dp.$D(\'HEndTime\')}', onpicked: function () { document.getElementById("HEndTime").focus(); } }); });
            $('#HEndTime').bind('click focus', function () { WdatePicker({ minDate: '#F{$dp.$D(\'HBeginTime\')}' }); });

            //敲回车键执行方法
            enterSearch(search);

            search();
        });


        //查询
        function search() {
            var pody = _params();
            var podyStr = JsonObjToParStr(pody);

            LoadingAnimation("ajaxTable");
            $("#ajaxTable").load("AjaxServers/YTGActivityProjectList.aspx .bit_table > *", podyStr);
        }

        //获取参数
        function _params() {

            var cbeginTime = encodeURIComponent($.trim($("#CBeginTime").val()));
            var bbeginTime = encodeURIComponent($.trim($("#BBeginTime").val()));
            var hbeginTime = encodeURIComponent($.trim($("#HBeginTime").val()));

            var cendTime = encodeURIComponent($.trim($("#CEndTime").val()));
            var bendTime = encodeURIComponent($.trim($("#BEndTime").val()));
            var hendTime = encodeURIComponent($.trim($("#HEndTime").val()));

            //            if ((beginTime != "" && !beginTime.isDate()) || (endTime != "" && !endTime.isDate())) {
            //                $.jAlert("输入的时间格式不正确");
            //                return false;
            //            }

            var projectName = encodeURIComponent($.trim($("#projectName").val()));
            var zhuti = $.trim($('#zhuti').val());
            var status = $(":checkbox[name='chkStatus']:checked").map(function () {
                return $(this).val();
            }).get().join(',');




            var pody = {
                projectName: projectName,
                status: status,
                cbeginTime: cbeginTime,
                hbeginTime: hbeginTime,
                bbeginTime: bbeginTime,
                cendTime: cendTime,
                bendTime: bendTime,
                hendTime: hendTime,
                zhuti: zhuti,
                r: Math.random()
            }

            return pody;
        }

        //分页操作
        function ShowDataByPost1(pody) {

            LoadingAnimation("ajaxTable");
            $('#ajaxTable').load('AjaxServers/YTGActivityProjectList.aspx?r=' + Math.random() + ' .bit_table > *', pody);
        }

      

         
    </script>
    <div class="search clearfix">
        <ul class="clear">
            <li>
                <label>
                    项目名称：</label>
                <input type="text" id="projectName" class="w190" />
            </li>
            <li>
                <label>
                    创建时间：</label>
                <input type="text" name="CBeginTime" id="CBeginTime" class="w85" style="width: 84px;
                    *width: 83px; width: 83px\9;" />
                至
                <input type="text" name="CEndTime" id="CEndTime" class="w85" style="width: 84px;
                    *width: 83px; width: 83px\9;" />
            </li>
            <li>
                <label>
                    状态：</label>
                <span>
                    <input type="checkbox" id="chkStatusUnused" value="1" name="chkStatus" /><em onclick='emChkIsChoose(this);'>进行中&nbsp;</em></span>
                <span>
                    <input type="checkbox" id="chkStatusOver" value="2" name="chkStatus" /><em onclick='emChkIsChoose(this);'>已结束</em></span>
            </li>
        </ul>
        <ul class="clear">
            <li>
                <label>
                    关联活动主题：</label>
                <input type="text" id="zhuti" class="w190" />
            </li>
            <li>
                <label>
                    报名开始时间：</label>
                <input type="text" name="BBeginTime" id="BBeginTime" class="w85" style="width: 84px;
                    *width: 83px; width: 83px\9;" />
                至
                <input type="text" name="BEndTime" id="BEndTime" class="w85" style="width: 84px;
                    *width: 83px; width: 83px\9;" />
            </li>
            <li>
                <label>
                    活动开始时间：</label>
                <input type="text" name="HBeginTime" id="HBeginTime" class="w85" style="width: 84px;
                    *width: 83px; width: 83px\9;" />
                至
                <input type="text" name="HEndTime" id="HEndTime" class="w85" style="width: 84px;
                    *width: 83px; width: 83px\9;" />
            </li>
            <li class="btnsearch">
                <input style="float: right" name="" type="button" value="查 询" onclick="javascript:search();" />
            </li>
        </ul>
    </div>
    <div class="optionBtn clearfix">
    </div>
    <div id="ajaxTable">
    </div>
    <form id="formExport" action="/AjaxServers/ProjectManage/ExportProjectTask.aspx">
    <input type="hidden" name="projectid" value="" />
    <input type="hidden" id="Browser" name="Browser" value="" />
    </form>
</asp:Content>
