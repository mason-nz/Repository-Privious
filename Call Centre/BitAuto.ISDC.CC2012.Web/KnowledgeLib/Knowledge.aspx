<%@ Page Title="知识点" Language="C#" AutoEventWireup="true" MasterPageFile="~/Controls/Top.Master"
    CodeBehind="Knowledge.aspx.cs" Inherits="BitAuto.ISDC.CC2012.Web.KnowledgeLib.Knowledge" %>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <script src="../Js/jquery.uploadify.v2.1.4.min.js" type="text/javascript"></script>
    <script src="../Js/json2.js" type="text/javascript"></script>
    <script type="text/javascript" src="../Js/swfobject.js"></script>
    <style type="text/css">
        
    </style>
    <script language="javascript" type="text/javascript">
        mumUnRead = 0;
        $(document).ready(function () {
            //敲回车键执行方法
            enterSearch(search);

            $("#Knowledgekeywords").focus(function () {
                if ($("#Knowledgekeywords").val() == "请输入关键字查询") {
                    $("#Knowledgekeywords").val('');
                }
                $("#Knowledgekeywords").css({ 'color': '#000', 'background': '#FFFFCC' });
            });

            $("#Knowledgekeywords").blur(function () {
                if ($("#Knowledgekeywords").val() == "") {
                    $("#Knowledgekeywords").val('请输入关键字查询');
                }
                //$("#Knowledgekeywords").css({ 'background': 'url("../CSS/img/inputbg01.jpg") repeat-x scroll 0 0 transparent', 'color': '#CCCCCC' }); //background: 'url("../img/inputbg01.jpg")
            });

            //selKCIDChange(0);

            //ShowAll();
            search();

            $('#btnSearch').click(function () {
                insertSearchLog();
                search();
            });
            $("#Knowledgekeywords").trigger("blur");

            $('#divChoose').click(function (e) {
                if (e.target.nodeName.toUpperCase() == "B" || e.target.nodeName.toUpperCase() == "A" || e.target.nodeName.toUpperCase() == "LI" || e.target.nodeName.toUpperCase() == "STRONG") {
                    var this$ = $(e.target).closest('li');
                    if (this$[0].id == "spanUnread") {
                        $('#hidUnRead').val(0);
                    }
                    this$.remove();
                    search();
                }

                return false;
            });
            /*
    
           
            $('div .attr').click(function (e) {
            if (e.target.nodeName.toUpperCase() == "IMG") {
            var $span = $(e.target).closest('span');
            if ($span[0].id == "spanUnread") {
            $('#hidUnRead').val(0);
            }
            $(e.target).closest('li').remove();

            search();
            }

            });
            */
        }); //jquery end

        function init() {
            $('#spanUnread').remove();
            if ($('#hidUnRead').val() == "1") {
                //                $('#divChoose').append($('<span id="spanUnread">' + $('#aUnread').text() + '<img alt="关闭" src="../Css/img/gb.png"></span>'));
                //                $('#divChoose').append($('<span id="spanUnread">' + $('#aUnread').text() + '<img alt="关闭" src="../Css/img/gb.png"></span>'));
                $('#divChoose').append($('<li  id="spanUnread" ><a href="#"><strong>' + $('#aUnread').text() + '</strong><b></b></a></li>'));

            }

            if ($('#hidUnRead').val() == "1") {
                $('#aUnread').addClass("redColor");
            } else {
                $('#aUnread').removeClass("redColor");
            }
            if ($('#hidOrder').val() == "CreateTime") {
                $('#orderbyCT').addClass("redColor");
            } else {
                $('#orderbyClickC').addClass("redColor");
            }
            mumUnRead = parseInt($('#aUnread').text().substr(3));
        }

        //搜索框如果输入了文字，则插入一条记录到搜索日志表
        function insertSearchLog() {
            keywords = $.trim($("#Knowledgekeywords").val());
            if (keywords == "请输入关键字查询" || keywords == "") {
                keywords = "";
            }
            else {
                keywords = encodeURI(keywords);
            }
            if (keywords != "") {
                $.post("../AjaxServers/KnowledgeLib/KnowledgeLibList.ashx", { Action: 'InsertSearchLog', SearchKey: keywords, SearchType: 1, r: Math.random() }, function () { });
            }
        }

        //查询
        function search() {
            var pody = params();
            LoadingAnimation("ajaxTable");
            $("#ajaxTable").load("../AjaxServers/KnowledgeLib/Knowledge.aspx", pody, function () {
                //$('#ajaxTable').load("../AjaxServers/KnowledgeLib/Knowledge.aspx?Keywords=&KCID=&r=0.4722482094075531&PageSize=undefined", null, function () {
                setTimeout(init, 200);
                //                $("#spanRecordCount").html($("#hidRecordCount").val());
                //                $("#spanUnRead").html($("#hidUnRead").val());
                //                var hidImgID = $("#hidImgID").val().split(',');
                //                for (var i = 0; i < hidImgID.length; i++) {
                //                    $("#img" + hidImgID[i]).hide();
                //                    $("#bBold" + hidImgID[i]).css("font-weight", "normal");
                //                }
            });
        }
        //参数
        function params() {
            var keywords = $.trim($("#Knowledgekeywords").val());
            var kcpid, kcid;
            //            if ($('#divChoose >span').length == 1) {
            //                kcpid = $('#divChoose >span[lev=1]').attr("did");
            //            } else {
            //            kcpid = $('#divChoose >span[lev=1]').attr("did");
            //            kcid = $('#divChoose >span[lev=2]').attr("did");
            kcpid = $('#divChoose >li[lev=1]').attr("did");
            kcid = $('#divChoose >li[lev=2]').attr("did");
            //            }
            kcpid = kcpid == null ? "-1" : kcpid;
            kcid = (kcid == null) ? "-1" : kcid;


            if (keywords == "请输入关键字查询") {
                keywords = "";
            }
            else {
                keywords = encodeURI(keywords);
            }
            var pody = "Keywords=" + keywords + "&KCPID=" + escape(kcpid) + "&KCID=" + escape(kcid) + "&r=" + Math.random();

            //显示 全部
            if ($("#hidPageSize").val() != "0") {
                pody += "&PageSize=" + escape($("#hidPageSize").val());
            }
            pody += "&od=" + escape($('#hidOrder').val());
            pody += "&asds=" + escape($('#hidAsds').val());

            pody += "&isUnread=" + escape($('#hidUnRead').val() == null ? 0 : $('#hidUnRead').val());
            return pody;
        }

        /*
        //绑定分类列表
        function selKCIDChange(n) {
        $("#selKCID3").hide();
        var pid = $("#selKCID" + n).val();
        if (pid == undefined) {
        pid = 0;
        }
        $("#selKCID" + (n + 1)).children().remove();
        $("#selKCID" + (n + 1)).append("<option value='-1'>请选择</option>");
        var level = n + 1;
        $.post("../AjaxServers/KnowledgeLib/KnowledgeLibList.ashx", { Action: 'bindknowledgecategoryexceptdel', Level: level, KCID: pid }, function (data) {
        if (data != "") {
        var jsonData = $.evalJSON(data);
        if (jsonData != "") {
        $.each(jsonData.root, function (idx, item) {
        $("#selKCID" + (n + 1)).append("<option value='" + item.kcid + "'>" + item.name + "</option>");
        });
        if (n == 2) {
        $("#selKCID3").show();
        }
        }
        }
        });
        }
        */
        //分页操作
        function ShowDataByPost1(pody) {
            LoadingAnimation("ajaxTable"); //http://ncc.sys1.bitauto.com/AjaxServers/KnowledgeLib/Knowledge.aspx?Keywords=&KCID=&r=0.4722482094075531&PageSize=undefined
            //            $('#ajaxTable').load("../AjaxServers/KnowledgeLib/Knowledge.aspx .bit_table > *", pody, function () {
            $('#ajaxTable').load("../AjaxServers/KnowledgeLib/Knowledge.aspx", pody, function () {
                init();
                /*
                $("#spanRecordCount").html($("#hidRecordCount").val());
                $("#spanUnRead").html($("#hidUnRead").val());
                var hidImgID = $("#hidImgID").val().split(',');
                for (var i = 0; i < hidImgID.length; i++) {
                $("#img" + hidImgID[i]).hide();
                $("#bBold" + hidImgID[i]).css("font-weight", "normal");
                }
                */
            });
        }
        //全部标记为已读
        function MarkRead() {
            $.post("../AjaxServers/KnowledgeLib/KnowledgeLibList.ashx", { Action: 'AllMarkRead', r: Math.random() }, function (data) {
                var jsonData = $.evalJSON(data);
                $.jAlert(jsonData.msg, function () {
                    window.location.reload();
                });
            });
        }
        //点击全部 时 显示全部数据
        function ShowAll() {
            $("#aShowUnRead").removeClass("redColor");
            $("#aShowAll").addClass("redColor");
            $("#hidUnRead1").val("0");
            search();
        }
        //点击未读 时 显示未读数据
        function ShowUnRead() {
            $("#aShowAll").removeClass("redColor");
            $("#aShowUnRead").addClass("redColor");
            $("#hidUnRead1").val("10000");
            search();
        }


        //点击超链接触发事件
        function aHrefTitleClick(kid, ths) {
            //增加点击次数
            try {
                var thsSpan$ = $(ths).closest(".zskList").find('.ckcount');
                var countNow = parseInt(thsSpan$.text());

                $.post("../AjaxServers/KnowledgeLib/KnowledgeLibList.ashx", { Action: 'AddKLClickAndDownloadCount', r: Math.random(), type: 0, KLID: kid }, function () {
                    countNow++;
                    thsSpan$.text(countNow);
                });
            } catch (e) {

            }
            kid = parseInt(kid);

            if ($("#ig" + kid).length > 0) {
                $("#ig" + kid).hide(); //隐藏未读小图标                
                mumUnRead = mumUnRead - 1;

                $('#aUnread').text(" 未读（" + (mumUnRead < 0 ? 0 : mumUnRead) + "）");
            }
            try {
                window.external.MethodScript('/browsercontrol/newpage?url=' + escape('<%=BitAuto.Utils.Config.ConfigurationUtil.GetAppSettingValue("ExitAddress")%>/KnowledgeLib/KnowledgeViewForUsers.aspx?kid=' + kid));
            }
            catch (e) {
                window.open("/KnowledgeLib/KnowledgeViewForUsers.aspx?kid=" + kid);
            }

        }

    </script>
    <!--分页结束-->
    <!--查询开始-->
    <div class="index_cx">
        <div class="cx">
            <div class="coupon-box002">
                <input type="text" name="Knowledgekeywords" id="Knowledgekeywords" value="" class="text02"><b><a
                    href="#" id="btnSearch">查询</a></b></div>
        </div>
        <div class="clearfix">
        </div>
    </div>
    <!--查询条件 start-->
    <%--<div class="attr" id="divChoose" style="text-align: left; vertical-align: bottom;">
        <label>
            已选择：</label><span lev="1" did="1">易湃平台<img src="../Css/img/gb.png" alt="关闭" /></span>
        <span did="141" lev="1">1(4)<img alt="关闭" src="../Css/img/gb.png"></span><span did="142"
            lev="2">11(4)<img alt="关闭" src="../Css/img/gb.png"></span><span id="spanUnread">未读（1）<img
                src="../Css/img/gb.png" alt="关闭"></span>
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
    <input type="hidden" id="Hidden1" value="0" />
    <input type="hidden" id="hidOrder" value="CreateTime" />
    <input type="hidden" id="hidPageSize" value="10" />
    <input type="hidden" id="hidUnRead" value="0" />
    <%--//0:降序--%>
    <input type="hidden" id="hidAsds" value="0" />
    <!--列表开始-->
    <div id="ajaxTable">
    </div>
</asp:Content>
