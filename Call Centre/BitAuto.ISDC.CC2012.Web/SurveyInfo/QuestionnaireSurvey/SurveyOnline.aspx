<%@ Page Language="C#" Title="问卷调查" MasterPageFile="~/Controls/Top.Master" AutoEventWireup="true"
    CodeBehind="SurveyOnline.aspx.cs" Inherits="BitAuto.ISDC.CC2012.Web.SurveyInfo.QuestionnaireSurvey.SurveyOnline" %>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <script src="../../Js/jquery.uploadify.v2.1.4.min.js" type="text/javascript"></script>
    <script src="../../Js/json2.js" type="text/javascript"></script>
    <script type="text/javascript" src="../../Js/swfobject.js"></script>
    <script language="javascript" type="text/javascript">
        $(document).ready(function () {
            //敲回车键执行方法
            enterSearch(search);

            $("#Surveykeywords").css({ 'background': 'url("../CSS/img/inputbg01.jpg") repeat-x scroll 0 0 transparent', 'color': '#CCCCCC' });

            $("#Surveykeywords").focus(function () {
                if ($("#Surveykeywords").val() == "请输入关键字查询") {
                    $("#Surveykeywords").val('');
                }
                $("#Surveykeywords").css({ 'color': '#000', 'background': '#FFFFCC' });
            }).blur(function () {
                if ($.trim($("#Surveykeywords").val()) == "") {
                    $("#Surveykeywords").val("请输入关键字查询");
                }
                $("#Surveykeywords").css({ 'background': 'url("../CSS/img/inputbg01.jpg") repeat-x scroll 0 0 transparent', 'color': '#CCCCCC' });
            });

            search();

        });

        //查询
        function search() {
            var pody = _params();
            var podyStr = JsonObjToParStr(pody);

            LoadingAnimation("ajaxTable");
            $("#ajaxTable").load("../../AjaxServers/SurveyInfo/QuestionnaireSurvey/SurveyOnline.aspx", podyStr, function () {
                isOver();
            });
        }

        //判断小图标是否显示
        function isOver() {
            $("#spanSurveyAll").html($("#hidSurveyAll").val());
            $("#spanSurveying").html($("#hidSurveying").val());
            $("input[type='hidden'][name='ajaxPage_isSubmit']").each(function () {
                if ($(this).val().toLowerCase() == "false") {
                    $(this).parent().find("img").hide();
                    $(this).parent().find("b").attr("style", "font-weight: normal;");
                }
            });
        }

        //获取参数
        function _params() {
            var category = $(":checkbox[name='examCategory']:checked").map(function () {
                return $(this).val();
            }).get().join(',');

            var name = "";
            if ($.trim($("#Surveykeywords").val()) != "请输入关键字查询") {
                name = encodeURIComponent($.trim($("#Surveykeywords").val()));
            }

            var pody = { Category: category, Name: name, r: Math.random() };
            return pody;
        }

        //分页操作
        function ShowDataByPost1(pody) {
            LoadingAnimation("tableList");
            $('#ajaxTable').load('../../AjaxServers/SurveyInfo/QuestionnaireSurvey/SurveyOnline.aspx', pody, function () {
                isOver();
            });
        } 
    </script>
    <!--查询开始-->
    <div class="search zsk">
        <ul>
            <li>
                <label>
                    分类：</label>
                <%=BindCategory() %>
                <span class="keywords">
                    <input style="" type="text" value="请输入关键字查询" class="w220" name="Surveykeywords" id="Surveykeywords" /></span>
                <span class="btnsearch">
                    <input name="" type="button" onclick="search()" value="搜 索" id="btnsearch" /></span>
            </li>
        </ul>
    </div>
    <!--查询结束-->
    <div class="optionBtn  clearfix">
        <div>
            显示： 全部<span style="color: Red">（<span id="spanSurveyAll" style="color: Red"></span>）</span>&nbsp;当前还有<span
                style="color: Red">（<span id="spanSurveying" style="color: Red"></span>）</span> 个调查正在进行！
        </div>
    </div>
    <!--列表开始-->
    <div id="ajaxTable">
    </div>
</asp:Content>
