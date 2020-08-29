<%@ Page Title="在线考试列表" Language="C#" MasterPageFile="~/Controls/Top.Master" AutoEventWireup="true"
    CodeBehind="ExamList.aspx.cs" Inherits="BitAuto.ISDC.CC2012.Web.ExamOnline.ExamList" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head1" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <script src="../Js/jquery.uploadify.v2.1.4.min.js" type="text/javascript"></script>
    <script src="../Js/json2.js" type="text/javascript"></script>
    <script type="text/javascript" src="../Js/swfobject.js"></script>
    <script language="javascript" type="text/javascript">
        $(function () {
            //敲回车键执行方法
            enterSearch(search);

            $("#Knowledgekeywords").css({ 'background': 'url("../CSS/img/inputbg01.jpg") repeat-x scroll 0 0 transparent', 'color': '#CCCCCC' });
            $("#Knowledgekeywords").focus(function () {
                if ($(this).val() == "请输入关键字查询") {
                    $(this).val("");
                }
                $("#Knowledgekeywords").css({ 'color': '#000', 'background': '#FFFFCC' });
            });
            $("#Knowledgekeywords").blur(function () {
                if ($(this).val() == "") {
                    $(this).val("请输入关键字查询");
                }
                $("#Knowledgekeywords").css({ 'background': 'url("../CSS/img/inputbg01.jpg") repeat-x scroll 0 0 transparent', 'color': '#CCCCCC' });
            });
            search(); 
            
        });
        //查询
        function search() {
            var pody = params();
            LoadingAnimation("ajaxTable");
            $("#ajaxTable").load("/AjaxServers/ExamOnline/ExamList.aspx", pody, function () { 
                IsTimeOver();
            });
        }
        //判断每一条记录是不是考试时间已到，如果结束了则不显示 小图标 字体变细；并且将已考数目和未考数目绑定过来
        function IsTimeOver() {
            $(".zskList").each(function () {
                var hid = $(this).find(":hidden").val();
                if (hid == "false") {
                    $(this).find("img").hide().end().find("b").addClass("read");
                }
            });
            $("#spanTestOver").html($("#hidTestOver").val());
            $("#spanNoTest").html($("#hidNoTest").val());
        }
        //参数
        function params() {
            var keywords = $.trim($("#Knowledgekeywords").val());
            if (keywords == "请输入关键字查询") {
                keywords = "";
            }
            else {
                keywords = encodeURI(keywords);
            }

            var examCategory = $(":checkbox[name='examCategory']:checked").map(function () {
                return $(this).val();
            }).get().join(',');

            var pody = "Keywords=" + keywords + "&ExamCategory=" + examCategory + "&r=" + Math.random();

            return pody;
        }

        //分页操作
        function ShowDataByPost1(pody) {
            LoadingAnimation("ajaxTable");
            $('#ajaxTable').load("../AjaxServers/ExamOnline/ExamList.aspx", pody, function () { IsTimeOver(); });
        }

    </script> 
    <!--查询开始-->
    <div class="search zsk">
        <ul>
            <li>
                <label>
                    分类：</label>
                <span>
                    <input name="examCategory" type="checkbox" value="5" />
                    <em onclick="emChkIsChoose(this)">月考</em></span> <span>
                        <input name="examCategory" type="checkbox" value="6" />
                        <em onclick="emChkIsChoose(this)">季考</em></span> <span>
                            <input name="examCategory" type="checkbox" value="7" />
                            <em onclick="emChkIsChoose(this)">培训考试</em></span> <span>
                                <input name="examCategory" type="checkbox" value="8" />
                                <em onclick="emChkIsChoose(this)">新员工考试</em></span> <span class="keywords">
                                    <input style="" type="text" value="请输入关键字查询" class="w220" name="Knowledgekeywords"
                                        id="Knowledgekeywords" /></span> <span class="btnsearch">
                                            <input name="" type="button" onclick="search()" value="搜 索" id="btnsearch" /></span>
            </li>
        </ul>
    </div>
    <!--查询结束-->
    <div class="optionBtn  clearfix">
        <div>
            显示： <span style="color:Red">已考（<span id="spanTestOver" style="color: Red"></span>）</span>&nbsp;当前还有<span style="color:Red">（<span id="spanNoTest" style="color: Red"></span>）</span> 场考试正在进行！
        </div>
    </div>
    <!--列表开始-->
    <div id="ajaxTable">
    </div>
</asp:Content>
