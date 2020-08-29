<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="FAQListFont.aspx.cs" Inherits="BitAuto.ISDC.CC2012.Web.AjaxServers.KnowledgeLib.FAQListFont" %>

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
            $(".addtion .aC").click(function () {
                var this$ = $(this);
                if (this$.attr("lev") == "1") {
                    $('#divChoose').empty().append($('<li lev="1" did="' + this$.attr("did") + '"><a href="javascript:void(0)"><strong>' + this$.text() + '</strong><b></b></a></li>'));
                } else {
                    var pText = this$.closest('ul').closest('li').children('a[lev=1]').text();
                    var pid = this$.closest('ul').closest('li').children('a[lev=1]').attr("did");
                    $('#divChoose').empty().append($('<li lev="1" did="' + pid + '"><a href="javascript:void(0)"><strong>' + pText + '</strong><b></b></a></li>'));
                    $('#divChoose').append($('<li lev="2" did="' + this$.attr("did") + '"><a href="javascript:void(0)"><strong>' + this$.text() + '</strong><b></b></a></li>'));
                }
                search();
            });
            //记录点击次数
            $('.faqList li a[name="kl"]').click(function () {
                var this$ = $(this);
                $.post("../AjaxServers/KnowledgeLib/KnowledgeLibList.ashx", { Action: 'AddKLClickAndDownloadCount', r: Math.random(), type: 0, KLID: $(this).closest(".lb").attr("klid") }, function () {
                    var nT$ = this$.closest(".lb").find('em.clkCount');
                    if (nT$.length > 0) {
                        var nC = parseInt(nT$.text());
                        nC++;
                        nT$.text(nC);
                    }
                });
            });

            //收藏提问
            $('.faqList li a[name="tw"]').click(function (eve) {
                eve.preventDefault();
                $.openPopupLayer({
                    name: "AddNewQuestionAjaxPopup",
                    parameters: {},
                    url: "/AjaxServers/KnowledgeLib/AddQuestion.aspx?KLType=1&KLID=" + $(this).closest(".lb").attr("did") + "&r=" + Math.random()
                });
                return false;
            });

            //记录FAQ收藏次数
            $('.faqList li a[name="sc"]').click(function (eve) {
                eve.preventDefault();
                $.post("/KnowledgeLib/Personalization/PersonalizationHandler.ashx", { Action: "addcollection", CollectRefId: $(this).closest(".lb").attr("did"), CollectType: 1, r: Math.random() }, function (data) {
                    if (data == "success") {
                        //$.jAlert("收藏成功！");
                        $.jPopMsgLayer("收藏成功！");
                        $.closePopupLayer('AddNewQuestionAjaxPopup');
                    }
                    else {
                        $.jAlert(data);
                    }
                });
                return false;
            });
        });
    </script>
</head>
<body>
    <form id="form1" runat="server">
    <div class="addtion">
        <asp:Literal runat="server" ID="lbKLC"></asp:Literal>
    </div>
    <div class="clearfix" style="border: 1px solid #eee;">
    </div>
    <div class="bit_table">
        <!--列表开始-->
        <div class="faqList">
            <asp:Repeater runat="server" ID="Rt_FAQ" OnItemDataBound="rpt_FAQItemBound">
                <ItemTemplate>
                    <ul>
                        <li class="bt"><b>Q：</b> <span>
                            <%#Eval("Question") %></span> </li>
                        <li><b>A：</b> <span>
                            <%#Eval("Ask") %></span> </li>
                        <li class="lb" did="<%#Eval("KLFAQID") %>" klid="<%#Eval("KLID") %>" style="overflow: hidden;">
                            <%#CheckGlobleHidden(Eval("libStatus").ToString(), Eval("KLID").ToString(), Eval("title").ToString(), Eval("ClickCount").ToString(), Eval("DownLoadCount").ToString())%>
                            <span class="right"><a href="#" name="sc">收藏
                                <img src="../../Images/sc.png" /></a> <a href="#" name="tw">提问
                                    <img src="../../Images/tw.png" /></a></span> </li>
                    </ul>
                </ItemTemplate>
            </asp:Repeater>
        </div>
        <!--列表结束-->
        <br />
        <!--分页-->
        <div class="pageTurn mr10">
            <p>
                <asp:Literal runat="server" ID="litPagerDown"></asp:Literal>
            </p>
        </div>
    </div>
    </form>
</body>
</html>
