<%@ Page Title="" Language="C#" MasterPageFile="~/Controls/Top.Master" AutoEventWireup="true"
    CodeBehind="FAQList.aspx.cs" Inherits="BitAuto.ISDC.CC2012.Web.KnowledgeLib.FAQList" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head1" runat="server">
    <title>FAQ列表</title>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <!--查询开始-->
    <%--<div class="search zsk">
        <ul>
        <li>
            <label>分类：</label>
        
            <span>
            <select id="selKCID1" class="w100" onchange="selKCIDChange(1)">
                <option value='-1'>请选择</option>
            </select>
            </span>
            
            <span>
            <select id="selKCID2" class="w100" onchange="selKCIDChange(2)">
                <option value='-1'>请选择</option>
            </select>
            </span>

            <span>
            <select id="selKCID3" class="w100" disabled="disabled" style="" >
                <option value='-1'>请选择</option>
            </select>
            </span>

            <span class="keywords"><input style=" " type="text" value="对问题或回答检索"  class="w200" name="FAQkeywords" id="FAQkeywords" /></span>
            <span class="btnsearch"><input name="" type="button" onclick="search()"  value="搜 索"/></span>
        </li>
        </ul> 
    </div>--%>
    <div class="index_cx">
        <div class="cx">
            <div class="coupon-box002">
                <input type="text" name="FAQkeywords" id="FAQkeywords" value="" class="text02"><b><a
                    href="#" id="btnSearch">查询</a></b></div>
        </div>
        <div class="clearfix">
        </div>
    </div>
    <!--查询条件 start-->
    <%-- <div class="attr" id="divChoose" style="text-align: left; vertical-align: bottom;">
        <label>
            已选择：</label>
    </div>--%>
    <div class="attr">
        <div class="a-key">
            已选条件：</div>
        <div class="a-values">
            <div class="v-fold">
                <ul class="f-list" id="divChoose">
                    <%--  <li><a href=""><strong>易湃平台</strong><b></b></a></li>
                            <li><a href=""><strong>易湃平台</strong><b></b></a></li>
                            <li><a href=""><strong>易湃平台</strong><b></b></a></li>
                            <li><a href=""><strong>易湃平台</strong><b></b></a></li>
                            <li><a href=""><strong>易湃平台</strong><b></b></a></li>--%>
                </ul>
            </div>
        </div>
    </div>
    <!--查询结束-->
    <div class="bit_table" id="bit_table">
    </div>
    <script type="text/javascript">


        $(document).ready(function () {
            //敲回车键执行方法
            enterSearch(search);
            $("#FAQkeywords").trigger("blur");
            selKCIDChange(0);
            search();

            //            $('div .attr').click(function (e) {
            //                if (e.target.nodeName.toUpperCase() == "IMG") {
            //                    var $span = $(e.target).closest('span');
            //                    if ($span[0].id == "spanUnread") {
            //                        $('#hidUnRead').val(0);
            //                    }
            //                    $(e.target).closest('span').remove();

            //                    search();
            //                }

            //            });


            $('#divChoose').click(function (e) {
                if (e.target.nodeName.toUpperCase() == "B" || e.target.nodeName.toUpperCase() == "A" || e.target.nodeName.toUpperCase() == "LI" || e.target.nodeName.toUpperCase() == "STRONG") {
                    var this$ = $(e.target).closest('li');
                    if (this$.id == "spanUnread") {
                        $('#hidUnRead').val(0);
                    }
                    this$.remove();
                    search();
                }

                return false;
            });

            $('#btnSearch').click(function () {
                search();
            });

        });

        $("#FAQkeywords").focus(function () {
            if ($("#FAQkeywords").val() == "对问题或回答检索") {
                $("#FAQkeywords").val('');
            }
            $("#FAQkeywords").css({ 'color': '#000', 'background': '#FFFFCC' });
        });

        $("#FAQkeywords").blur(function () {
            if ($("#FAQkeywords").val() == "") {
                $("#FAQkeywords").val('对问题或回答检索');
            }
            //$("#FAQkeywords").css({ 'background': 'url("../CSS/img/inputbg01.jpg") repeat-x scroll 0 0 transparent', 'color': '#CCCCCC' }); //background: 'url("../img/inputbg01.jpg")
        });

        //查询
        function search() {
            LoadingAnimation("bit_table");
            var pody = params();
            $(".bit_table").load("../AjaxServers/KnowledgeLib/FAQListFont.aspx", pody);
        }
        //参数
        function params() {

            var keywords = $.trim($("#FAQkeywords").val());
            var kcpid, kcid;
            //            kcpid = $('#divChoose >span[lev=1]').attr("did");
            //            kcid = $('#divChoose >span[lev=2]').attr("did");

            kcpid = $('#divChoose >li[lev=1]').attr("did");
            kcid = $('#divChoose >li[lev=2]').attr("did");

            //            }
            kcpid = kcpid == null ? "-1" : kcpid;
            kcid = (kcid == null) ? "-1" : kcid;


            if (keywords == "对问题或回答检索") {
                keywords = "";
            }
            else {
                keywords = encodeURI(keywords);
            }
            var pody = "Keywords=" + keywords + "&KCPID=" + escape(kcpid) + "&KCID=" + escape(kcid) + "&df=" + Math.random();

            return pody;
        }
        //分页操作
        function ShowDataByPost1(pody) {
            LoadingAnimation("bit_table");
            $('#bit_table').load('../AjaxServers/KnowledgeLib/FAQListFont.aspx', pody);
        }
        //绑定分类列表
        function selKCIDChange(n) {
            var pid = $("#selKCID" + n).val();
            if (pid == undefined) {
                pid = 0;
            }
            $("#selKCID" + (n + 1)).children().remove();
            $("#selKCID" + (n + 1)).append("<option value='-1'>请选择</option>");
            var level = n + 1;
            $.post("../AjaxServers/KnowledgeLib/KnowledgeLibList.ashx", { Action: 'bindknowledgecategoryexceptdel', Level: level, KCID: pid  ,regionid:<%=RegionID %>}, function (data) {
                if (data != "") {
                    $("#selKCID3").removeAttr("disabled");
                    var jsonData = $.evalJSON(data);
                    if (jsonData != "") {
                        $.each(jsonData.root, function (idx, item) {
                            $("#selKCID" + (n + 1)).append("<option value='" + item.kcid + "'>" + item.name + "</option>");
                        });
                    }
                }
                else if (n == 2) {
                    $("#selKCID3").attr("disabled", "disabled");
                }
            });
        }  
    </script>
</asp:Content>
