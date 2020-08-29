<%@ Page Title="历史记录" Language="C#" AutoEventWireup="true" CodeBehind="ConversationHistoryForCCDetail.aspx.cs"
    Inherits="BitAuto.DSC.IM_2015.Web.ConversationHistoryForCCDetail" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <meta http-equiv="X-UA-Compatible" content="IE=8">
    <title></title>
    <link href="css/cc/base.css" rel="stylesheet" type="text/css" />
    <link href="css/cc/style.css" rel="stylesheet" type="text/css" />
    <script src="/Scripts/jquery-1.4.1.min.js" language="javascript" type="text/javascript"></script>
    <script type="text/javascript" charset="utf-8" src="/Scripts/jquery.jmpopups-0.5.1.pack.js"></script>
    <script src="Scripts/common.js" type="text/javascript"></script>
    <style type="text/css">
        .highlight
        {
            background-color: yellow;
        }
        #dialogue img
        {
            width: 20px;
            height: 20px;
        }
    </style>
    <script type="text/javascript">
        document.domain = "bitauto.com";

        $(document).ready(function () {
            ShowImg();
            AutoHeight();
            //回车查询
            enterSearch(highlight);

            // setTimeout("ImgAfter()", 2000);
        });
        window.onload = function () {
            ImgAfter()
        };

        $(window).resize(function () {
            AutoHeight();
        });
        //图片
        function ShowImg() {
            $(".cktp").live("mouseenter", function () {
                var imgurl = $(this).attr("src");
                var ImgObj = new Image();
                ImgObj.src = imgurl;
                //验证图片是否正确
                if (ImgObj.fileSize > 0 || (ImgObj.width > 0 && ImgObj.height > 0)) {

                    var left = $(this).offset().left + 20; //大图与小图间隔
                    var top = $(this).offset().top;
                    var smallWidth = $(this).width(); //小图宽度
                    var smallHeight = $(this).height(); //小图高度
                    var width = ImgObj.width; //原图宽度
                    var height = ImgObj.height; //原图高度

                    //==================================================================================
                    //父级iframe的top

                    var topParent = top - $(document).scrollTop();
                    //父级iframe的left
                    var leftParent = left - $(document).scrollLeft();
                    for (var i = 0; i < $("iframe", window.parent.document).length; i++) {
                        try {
                            var frame = $("iframe", window.parent.document)[i];
                            if ($(frame).contents().find("#formdetail").length > 0) {
                                leftParent += $(frame).offset().left - $(window.parent.document).scrollLeft();
                                topParent += $(frame).offset().top - $(window.parent.document).scrollTop();
                                break;
                            }
                        } catch (e) { }
                    }

                    //==================================================================================
                    var avalWidth = $(window.parent.window).width() - leftParent - smallWidth - 20; //最大宽度
                    var avalHeight = $(window.parent.window).height() - 20; //最大高度

                    //==================================================================================
                    //图片最终尺寸
                    if (height > avalHeight) {
                        height = avalHeight;
                    }
                    if (width > avalWidth) {
                        width = avalWidth;
                    }
                    //=================================================================================
                    //图片最终位置
                    var endleft = leftParent + smallWidth; //左边
                    var endtop = topParent - height / 2; //小图所在位置
                    if (endtop < 20) {
                        endtop = 20;
                    }
                    if (endtop + height > avalHeight) {
                        endtop = avalHeight - height;
                    }
                    if (endtop <= 0) {
                        endtop = 10;
                    }

                    window.parent.window.ShowImg(escape(imgurl), width + "px", height + "px", endleft + "px", endtop + "px", "");
                }
            })
            $(".cktp").live("mouseleave", function () {
                window.parent.window.HiddenImg();
            })
        }

        function ImgAfter() {

            $("#dialogue img").each(function () {
                //判断是不是表情,Emotions表情文件夹
                if ($(this).attr("src").toString().indexOf("smilies") < 0) {
                    $(this).after("<span class='cktp' style='cursor:pointer;' src='" + $(this).attr("src") + "'>查看图片</span>");
                    $(this).attr("src", "Images/view_img.png");
                    $(this).attr("class", "icon_view");
                    $(this).removeAttr("width");
                    $(this).removeAttr("height");
                    $(this).removeAttr("style");
                }

            });

        }

        function ShowDataByPost1(podyStr) {
            LoadingAnimation("page");
            $('#page').load("/ConversationHistoryForCCDetail.aspx #page > *", podyStr, function () {
                AutoHeight();
                highlight();
                setTimeout("ImgAfter()", 2000);
            });
        }

        function highlight() {
            clearSelection(); //先清空一下上次高亮显示的内容；
            var searchText = $.trim($('#txtKeyWord').val()); //获取你输入的关键字；
            if (searchText.length > 0) {
                var regExp = new RegExp(searchText, 'g'); //创建正则表达式，g表示全局的，如果不用g，则查找到第一个就不会继续向下查找了；

                $('#dialogue .dhc').each(function ()//遍历文章；
                {
                    var html = $(this).html();
                    var newHtml = html.replace(regExp, "<span class='highlight'>" + searchText + '</span>'); //将找到的关键字替换，加上highlight属性；

                    $(this).html(newHtml); //更新文章；
                });
            }
        }
        function clearSelection() {
            $('#dialogue .dhc').each(function ()//遍历
            {
                $(this).find('.highlight').each(function ()//找到所有highlight属性的元素；
                {
                    var thishtml = $(this).html();
                    $(this)[0].outerHTML = thishtml;
                    // $(this).replaceWith(thishtml); //将他们的属性去掉；
                });
            });
        }

        function AutoHeight() {
            var height = $(window).height();
            if (height > $(document).height()) {
                height = $(document).height();
            }
            height -= 313;
            $("#dialogue").css("height", height + "px");
        }
      
    
     
    </script>
</head>
<body>
    <form id="formdetail" runat="server">
    <div class="lsjl_left">
        <div class="online_kf">
            <div class="title_kf titles">
                历史记录</div>
            <div class="content_kf">
                <div class="his_jl his_jl_zj">
                    <!--查询开始-->
                    <div class="searchTj searchTj_zj clearfix">
                        <ul>
                            <li>
                                <label>
                                    关键字：</label><input name="txtKeyWord" id="txtKeyWord" type="text" class="w200" />
                                <input type="text" value="" style="display: none;" />
                            </li>
                            <li class="tjBtn">
                                <input type="button" value="查询" onclick="highlight()" class="w60" /></li>
                        </ul>
                    </div>
                    <!--查询结束-->
                    <!--历史标签-->
                    <div class="searchTj searchTj_zj  searchTj_zj2 clearfix">
                        <ul>
                            <li>
                                <label>
                                    发起页面：</label><span style="display: block; width: 240px; height: 23px; overflow: hidden;">
                                        <a href="<%=UserReferURL%>" target="_blank" title="<%=UserReferTitle%>">
                                            <%=UserReferTitle%></a></span></li>
                            <li>
                                <label>
                                    对话开始时间：</label><span><%=CreateTime.Contains("1900") ? "" : CreateTime%></span></li>
                            <li>
                                <label>
                                    应答时间：</label><span><%=AgentStartTime.Contains("1900") ? "" :AgentStartTime  %></span></li>
                            <li>
                                <label>
                                    结束时间：</label><span><%=EndTime.Contains("1900") ? "" : EndTime %></span></li>
                            <li>
                                <label>
                                    结束方式：</label><span><%=CloseType%></span></li>
                        </ul>
                    </div>
                    <!--历史标签-->
                    <!--历史记录-->
                    <h2>
                        历史记录</h2>
                    <div id="page">
                        <div id="dialogue" class="dialogue">
                            <asp:Repeater runat="server" ID="Rt_CSHistoryData">
                                <ItemTemplate>
                                    <div class="dh2">
                                        <div class="title">
                                            <%#Eval("newName")%>
                                        </div>
                                        <div class="dhc dhc2 dhc3">
                                            <%#Eval("Content")%>
                                        </div>
                                    </div>
                                </ItemTemplate>
                            </asp:Repeater>
                        </div>
                        <!--历史记录-->
                        <div class="clearfix">
                        </div>
                        <!--分页开始-->
                        <div class="searchTj searchTj2 searchTj_zj">
                            <div class="pagesnew">
                                <asp:Literal runat="server" ID="litPagerDown_History"></asp:Literal>
                            </div>
                            <input type="hidden" value="<%=RecordCount %>" id="hidHistoryTotalCount" />
                        </div>
                        <div class="clearfix">
                        </div>
                        <!--分页结束-->
                    </div>
                    <!--历史记录-->
                </div>
            </div>
        </div>
    </div>
    </form>
</body>
</html>
