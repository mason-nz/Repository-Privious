﻿<%@ Page Title="历史记录" Language="C#" AutoEventWireup="true" CodeBehind="ConversationHistoryForCC.aspx.cs" Inherits="BitAuto.DSC.IM_2015.Web.ConversationHistoryForCC" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<meta http-equiv="X-UA-Compatible" content="IE=8" >
<head id="Head1" runat="server">
    <title></title>
    <link rel="stylesheet" href="/css/css.css" type="text/css" />
    <link rel="stylesheet" href="/css/style.css" type="text/css" />
    <style type="text/css">
        .highlight
        {
            background-color: yellow;
        } 
    </style>
    <script src="/Scripts/jquery-1.4.1.min.js" language="javascript" type="text/javascript"></script>
    <script type="text/javascript" charset="utf-8" src="/Scripts/jquery.jmpopups-0.5.1.pack.js"></script>
    <script src="/Scripts/My97DatePicker/WdatePicker.js" type="text/javascript"></script>
    <script type="text/javascript">
        function ShowDataByPost1(podyStr) {
            // LoadingAnimation("dialogue");
            $('#dialogue').load("/ConversationHistoryForCC.aspx #dialogue > *", podyStr, function () {
                highlight();
            });
        }
        function getDataTimeNow() {
            var nowstr = new Date();
            var datenow = nowstr.getFullYear() + "-"
                     + ((nowstr.getMonth() + 1) < 10 ? "0" : "") + (nowstr.getMonth() + 1) + "-"
                     + (nowstr.getDate() < 10 ? "0" : "") + nowstr.getDate();
            return datenow;
        }
        function getDataMonthStart() {
            var nowstr = new Date();
            var datenow = nowstr.getFullYear() + "-"
                     + ((nowstr.getMonth() + 1) < 10 ? "0" : "") + (nowstr.getMonth() + 1) + "-01";
            return datenow;

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

        function closethispage() {
            try {
                window.external.MethodScript('/browsercontrol/closepage');
            } catch (e) {
                window.opener = null; 
                window.close();
            }
        }
    </script>
</head>
<body>
    <form id="form1" runat="server">
    <div class="online_kf">
        <div class="title_kf">
            历史记录<span style="padding-top:8px;padding-top:8px \9; *padding-top:8px;"><img src="images/c_btn.png" onclick="closethispage();" border="0" alt="关闭" /></span></div>
        <div class="content_kf" style="overflow: scroll; overflow-x: hidden; background-color: White;
            margin: 10px;">
            <div class="his_jl">
                <!--查询开始-->
                <div class="searchTj">
                    <ul>
                        <li>
                            <label style=" position:static; padding-top:7px;">关键字：</label>
                            <input name="" id="txtKeyWord" type="text" class="w240" style=" position:static;" />
                        </li>
                        <li style="width: 80px;">
                            <div class="tjBtn">
                                <input type="button" value="查询" onclick="highlight()" class="w60" /></div>
                        </li>
                    </ul>
                    <!--分页开始-->
                    <div class="pagesnew">
                    </div>
                    <!--分页结束-->
                    <div class="clearfix">
                    </div>
                </div>
                <!--查询结束-->
                <!--历史记录-->
                <h2>
                    历史记录</h2>
                <div class="dialogue" id="dialogue" style="width: 99%;">
                    <asp:Repeater runat="server" ID="Rt_CSHistoryData">
                        <ItemTemplate>
                            <div class="dh1">
                                <div class="title">
                                    <%#Eval("newName")%>
                                </div>
                                <div class="dhc">
                                    <%#Eval("Content")%>
                                </div>
                            </div>
                        </ItemTemplate>
                    </asp:Repeater>
                    <br />
                    <div class="searchTj" style="margin: 0px 35px 0px 15px;">
                        &nbsp;
                    </div>
                    <!--分页-->
                    <div class="pagesnew" style="float: right; margin: 10px; position:static;" id="itPage">
                        <p>
                            <asp:Literal runat="server" ID="litPagerDown_History"></asp:Literal>
                        </p>
                    </div>
                    <input type="hidden" value="<%=RecordCount %>" id="hidHistoryTotalCount" />
                </div>
                <!--历史记录-->
                <div class="clearfix">
                </div>
            </div>
        </div>
    </div>
    </form>
</body>
</html>