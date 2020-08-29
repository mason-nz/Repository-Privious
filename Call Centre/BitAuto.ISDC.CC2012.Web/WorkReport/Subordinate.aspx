<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/Controls/Top.Master"
    Title="下属工作报告" CodeBehind="Subordinate.aspx.cs" Inherits="BitAuto.ISDC.CC2012.Web.WorkReport.Subordinate" %>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
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
            //类型
            var SearchType = getCheckBoxVal("SearchType");
            //是否审阅
            var HasRead = getCheckBoxVal("HasRead");
            //是否回复
            var HasReply = getCheckBoxVal("HasReply");

            var pody = {
                txtName: txtName,
                SearchType: SearchType,
                HasRead: HasRead,
                HasReply: HasReply,
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
            $("#ajaxTable").load("/AjaxServers/WorkReport/Subordinate.aspx", podyStr, null);
        }
        //分页操作
        function ShowDataByPost1(pody) {
            LoadingAnimation("ajaxTable");
            $("#ajaxTable").load("/AjaxServers/WorkReport/Subordinate.aspx?r=" + Math.random(), pody, null);
        }
    </script>
    <div class="search clearfix">
        <ul class="clear">
            <li>
                <label>
                    姓名：</label>
                <input type="text" id="txtName" class="w190" />
            </li>
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
            <li>
                <label>
                    审阅状态：
                </label>
                <span>
                    <input type="checkbox" name="HasRead" value="0" checked="checked" /><em onclick="emChkIsChoose(this);">未审阅</em></span>
                <span>
                    <input type="checkbox" name="HasRead" value="1" /><em onclick="emChkIsChoose(this);">已审阅</em></span>
            </li>
        </ul>
        <ul class="clear">
            <li>
                <label>
                    有无回复：
                </label>
                <span>
                    <input type="checkbox" name="HasReply" value="1" /><em onclick="emChkIsChoose(this);">有</em></span>
                <span>
                    <input type="checkbox" name="HasReply" value="0" /><em onclick="emChkIsChoose(this);">无</em></span>
            </li>
            <li class="btnsearch" style="width: 214px; *width: 247px;">
                <input style="float: right;" name="" type="button" value="查 询" onclick="javascript:search()" />
                <input type="button" value="刷 新" onclick="javascript:search('refresh')" id="Button1"
                    style="display: none" />
            </li>
        </ul>
    </div>
    <div class="optionBtn clearfix">
    </div>
    <div class="bit_table" id="ajaxTable">
    </div>
</asp:Content>
