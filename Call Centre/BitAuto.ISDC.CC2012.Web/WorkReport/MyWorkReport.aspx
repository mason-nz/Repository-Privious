<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/Controls/Top.Master"
    Title="我的工作报告" CodeBehind="MyWorkReport.aspx.cs" Inherits="BitAuto.ISDC.CC2012.Web.WorkReport.MyWorkReport" %>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <script type="text/javascript">
        document.writeln("<s" + "cript type='text/javascript' src='/Js/config.js?r=" + Math.random() + "'></scr" + "ipt>");
        loadJS("common");
    </script>
    <script type="text/javascript">
        //load方法
        $(document).ready(function () {
            document.domain = "<%=SysUrl %>";
            //敲回车键执行方法
            enterSearch(search);
            search();
        });
        //打开编辑页面
        function OpenEdit(type) {
            var url = "http://<%=WpUrl %>/WorkReport/Edit.aspx?type=" + type + "&From=CC&BtnID=btnsearch&r=" + Math.random();
            OpenNewPageForURL(url);
        }
        //获取参数
        function _params(refresh) {
            //类型
            var SearchType = "";
            $("input[name='SearchType']:checked").each(function (i, n) {
                SearchType += n.value + ",";
            });
            if (SearchType.substr(SearchType.length - 1, 1) == ',') {
                SearchType = SearchType.substr(0, SearchType.length - 1);
            }

            var pody = {
                SearchType: SearchType,
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
            $("#ajaxTable").load("/AjaxServers/WorkReport/MyWorkReport.aspx", podyStr, null);
        }
        //分页操作
        function ShowDataByPost1(pody) {
            LoadingAnimation("ajaxTable");
            $("#ajaxTable").load("/AjaxServers/WorkReport/MyWorkReport.aspx?r=" + Math.random(), pody, null);
        }
        //删除工作报告
        function DeleteReport(reportid) {
            $.jConfirm('确认删除选择的工作报告?', function (result) {
                if (result) {
                    $.post("/AjaxServers/WorkReport/Handler.ashx?action=delete&recid=" + reportid + "&r=" + Math.random(), null, function (json) {
                        if (json["success"]) {
                            $.jPopMsgLayer("删除成功", function () { search('refresh'); });
                        } else {
                            $.jAlert("删除出错：" + json["message"]);
                        }
                    }, 'json');
                }
            });
        }
        //撤销工作报告
        function revokeReport(reportid) {
            $.post("/AjaxServers/WorkReport/Handler.ashx?action=isview&recid=" + reportid + "&r=" + Math.random(), null, function (json) {
                if (json["success"]) {
                    if (json["message"] == "0") {
                        $.post("/AjaxServers/WorkReport/Handler.ashx?action=updatestatus&recid=" + reportid + "&status=1&r=" + Math.random(), null, function (data) {
                            if (data["success"]) {
                                $.jPopMsgLayer("撤回成功", function () { search('refresh'); });
                            } else {
                                $.jAlert("撤回出错：" + data["message"]);
                            }
                        }, 'json');
                    } else {
                        $.jAlert("该报告已被查阅，不能撤回！", function () {
                            search('refresh');
                        });
                    }
                } else {
                    $.jAlert("撤回出错：" + json["message"]);
                }
            }, 'json');
        }
        //导出工作报告
        function ExportReport(reportid) {
            window.location = "http://<%=WpUrl %>/WorkReport/MyWorkReport.aspx?type=Export&RecId=" + reportid;
        }
    </script>
    <div class="search clearfix">
        <ul class="clear">
            <li>
                <label>
                    报告类别：
                </label>
                <span>
                    <input type="checkbox" name="SearchType" value="1" /><em onclick="emChkIsChoose(this);">日报</em></span>
                <span>
                    <input type="checkbox" name="SearchType" value="2" /><em onclick="emChkIsChoose(this);">周报</em></span>
                <span>
                    <input type="checkbox" name="SearchType" value="3" /><em onclick="emChkIsChoose(this);">月报</em></span>
                <span>
                    <input type="checkbox" name="SearchType" value="4" /><em onclick="emChkIsChoose(this);">季报</em></span>
            </li>
            <li class="btnsearch">
                <input style="float: right;" name="" type="button" value="查 询" onclick="javascript:search()" />
                <input type="button" value="刷 新" onclick="javascript:search('refresh')" id="btnsearch"
                    style="display: none" />
            </li>
        </ul>
    </div>
    <div class="optionBtn clearfix">
        <input type="button" value="新增季报" onclick="OpenEdit('4');" class="newBtn" />
        <input type="button" value="新增月报" onclick="OpenEdit('3');" class="newBtn" />
        <input type="button" value="新增周报" onclick="OpenEdit('2');" class="newBtn" />
        <input type="button" value="新增日报" onclick="OpenEdit('1');" class="newBtn" />
    </div>
    <div class="bit_table" id="ajaxTable">
    </div>
</asp:Content>
