<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Knowledge.aspx.cs" Inherits="BitAuto.ISDC.CC2012.Web.AjaxServers.KnowledgeLib.Knowledge" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <script type="text/javascript">
        $(function () {
            //鼠标事件
            $(".addtion>ul>li:gt(0)").hover(function (e) {
                var this$ = $(this);
                var w = this$.find('ul>li').length * (105) + 20;
                w = w > 880 ? 880 : w;
                var lft = -90;
                if (this$.find('ul>li').length == 1) {
                    lft = -20;
                }
                this$.siblings().removeClass("yp").children('a').removeClass('active').end().children('.arrow').hide().end().children('ul').hide();
                this$.addClass("yp").children('a').addClass("active").end().children('.arrow').show().end().children('ul').css({ "width": w, "left": lft }).show(); //.slideDown(80);
            }, function () {
                $(this).removeClass("yp").children('a').removeClass('active').end().children('.arrow').hide().end().children('ul').hide();
            });
            $('#btnMore').click(function () {
                $(".addtion>ul:gt(0)").toggle("hidden");
                if ($(this).hasClass("dnowup")) {
                    $(this).removeClass("dnowup").addClass("updnow");
                } else {
                    $(this).removeClass("updnow").addClass("dnowup");
                }
            });
            //点击子菜单 class='aC'
            $(".addtion .aC").click(function () {
                var this$ = $(this);
                if (this$.attr("lev") == "1") {
                    $('#divChoose').empty().append('<li lev="1" did="' + this$.attr("did") + '"><a href="javascript:void(0)"><strong>' + this$.text() + '</strong><b></b></a></li>');
                } else {
                    var pText = this$.closest('ul').closest('li').children('a[lev=1]').text();
                    var pid = this$.closest('ul').closest('li').children('a[lev=1]').attr("did");
                    $('#divChoose').empty().append('<li lev="1" did="' + pid + '"><a href="javascript:void(0)"><strong>' + pText + '</strong><b></b></a></li>');
                    $('#divChoose').append('<li lev="2" did="' + this$.attr("did") + '"><a href="javascript:void(0)"><strong>' + this$.text() + '</strong><b></b></a></li>');
                }
                search();
            });
            $('#orderbyCT').click(function () {
                if ($('#hidOrder').val() == "CreateTime") {
                    if ($('#hidAsds').val() == "0") {
                        $('#hidAsds').val("1");
                    } else {
                        $('#hidAsds').val("0");
                    }
                } else {
                    //设置为默认安装时间降序
                    $('#hidOrder').val("CreateTime");
                    $('#hidAsds').val("0");
                }
                search();
            });

            $('#orderbyClickC').click(function () {
                if ($('#hidOrder').val() == "ClickCount") {
                    if ($('#hidAsds').val() == "0") {
                        $('#hidAsds').val("1");
                    } else {
                        $('#hidAsds').val("0");
                    }
                } else {
                    //设置为默认安装时间降序
                    $('#hidOrder').val("ClickCount");
                    $('#hidAsds').val("0");
                }
                search();
            });
            $('#aUnread').click(function () {
                if ($('#hidUnRead').val() == "1") {
                    $('#hidUnRead').val("0");
                    if ($('#spanUnread').length > 0) {
                        $('#spanUnread').remove();
                    }
                } else {
                    if ($('#spanUnread').length == 0) {
                        $('#divChoose').append($('<li  id="spanUnread" ><a href=""><strong>' + $('#aUnread').text() + '</strong><b></b></a></li>'));
                    }
                    $('#hidUnRead').val("1");
                }
                search();
            });

        });
    </script>
</head>
<body>
    <form id="form1" runat="server">
    <div class="addtion">
        <asp:Literal runat="server" ID="lbKLC"></asp:Literal>
    </div>
    <!--查询条件 end-->
    <!--查询结束-->
    <div class="optionBtn  clearfix">
        <div>
            显示： <a href="#" id="aUnread">未读（<asp:Literal runat="server" ID="ltUnreadCount"></asp:Literal>）</a>
            | <a href="#" onclick="MarkRead();">全部标记为已读</a> <span style="margin-left: 150px;">排序：<a
                href="#" id="orderbyCT">更新时间</a> | <a href="#" id="orderbyClickC">点击率</a></span>
        </div>
    </div>
    <!--列表开始-->
    <div class="bit_table" id="divList">
        <asp:Repeater ID="repeaterTableList" runat="server">
            <ItemTemplate>
                <div class="zskList">
                    <div class="bt">
                        <%#BindImg(Eval("unRead").ToString(), Eval("KLID").ToString())%>
                        <b id="bBold<%#Eval("KLID") %>"><a onclick="aHrefTitleClick(<%#Eval("KLID") %>,this)"
                            style="cursor: pointer;" title="<%#Eval("Title") %>" target='_blank' id="aHrefTitle">
                            <%#getTitle(Eval("Title").ToString()) %></a></b><em>
                            <%# BitAuto.ISDC.CC2012.Entities.CommonFunction.GetDateTimeStrForPage(Eval("CreateTime").ToString())%>
                            </em><span
                                class="right">所属分类：<%#Eval("kcname")%>
                                &nbsp;&nbsp;点击：<span class="ckcount" style="padding: 0px;" v='<%#Eval("ClickCount") %>'><%#Eval("ClickCount") %></span>
                                次&nbsp; 下载：<%#Eval("DownLoadCount") %>次 </span>
                    </div>
                    <p>
                        <%#getContent(Eval("Abstract").ToString())%>
                        <a onclick="aHrefTitleClick(<%#Eval("KLID") %>,this)" target="_blank" style="cursor: pointer;">
                            查阅全文</a></p>
                </div>
            </ItemTemplate>
        </asp:Repeater>
        <!--列表结束-->
        <!--分页开始-->
        <!--分页-->
        <div class="pageTurn mr10">
            <p>
                <asp:Literal runat="server" ID="litPagerDown"></asp:Literal>
            </p>
        </div>
        <input type="hidden" runat="server" id="hidRecordCount" value="0" />
        <input type="hidden" runat="server" id="hidImgID" />
    </div>
    </form>
</body>
</html>
