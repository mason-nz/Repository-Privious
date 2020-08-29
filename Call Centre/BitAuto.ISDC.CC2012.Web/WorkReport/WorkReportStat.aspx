<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/Controls/Top.Master"
    Title="工作报告统计" CodeBehind="WorkReportStat.aspx.cs" Inherits="BitAuto.ISDC.CC2012.Web.WorkReport.WorkReportStat" %>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <link href="/Css/style.css" rel="stylesheet" type="text/css" />
    <script type="text/javascript">
        document.writeln("<s" + "cript type='text/javascript' src='/Js/config.js?r=" + Math.random() + "'></scr" + "ipt>");
        loadJS("common");
    </script>
    <script type="text/javascript">
        //load方法
        $(document).ready(function () {
            //敲回车键执行方法
            enterSearch(search);
            search();
        });
        //获取参数
        function _params(refresh) {
            //姓名
            var txtName = $.trim($("#txtName").val());
            var pody = {
                txtName: txtName,
                r: Math.random()//随机数
            }
            if (refresh == "refresh") {
                pody.page = $("#input_page").val();
            }
            return pody;
        }
        //查询
        function search(refresh) {
            //获取查询条件
            var pody = _params(refresh);
            var podyStr = JsonObjToParStr(pody);

            LoadingAnimation("ajaxTable");
            $("#ajaxTable").load("/AjaxServers/WorkReport/WorkReportStat.aspx", podyStr, null);
        }
        //分页操作
        function ShowDataByPost1(pody) {
            LoadingAnimation("ajaxTable");
            $("#ajaxTable").load("/AjaxServers/WorkReport/WorkReportStat.aspx?r=" + Math.random(), pody, null);
        }
    </script>
    <div class="search clearfix">
        <ul class="clear">
            <li>
                <label>
                    姓名：</label>
                <input type="text" id="txtName" class="w190" />
            </li>
            <li class="btnsearch">
                <input style="float: right;" name="" type="button" value="查 询" onclick="javascript:search()" />
                <input type="button" value="刷 新" onclick="javascript:search('refresh')" id="Button1"
                    style="display: none" />
            </li>
        </ul>
    </div>
    <div class="bit_table" id="ajaxTable">
    </div>
</asp:Content>
