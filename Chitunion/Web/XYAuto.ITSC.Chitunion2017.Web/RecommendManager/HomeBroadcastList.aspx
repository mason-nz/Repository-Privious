<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="HomeBroadcastList.aspx.cs" Inherits="XYAuto.ITSC.Chitunion2017.Web.RecommendManager.Broadcast" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">

<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title>首页直播-推荐列表</title>
    <!--#include file="/base/js.html" -->
    <script type="text/javascript" src="/js/jquery.jmpopups-0.5.1.pack.js"></script>
    <script type="text/javascript" src="../js/jquery.pagination.js"></script>
    <script src="js/HomeCategory.js"></script>
    <script type="text/javascript" src="/RecommendManager/js/preview.js"></script>
</head>
<body>
    <!--#include file="/base/header.html" -->
    <form id="form1" runat="server">
        <!--中间内容-开始 -->
        <div class="order">
            <!--#include file="/base/Menu.html" -->
            <div class="order_r">
                <div class="list_box">
                    <div class="list">
                        <h2>
                            <div class="fl">推荐管理 &gt; 首页直播推荐</div>
                            <div id="again" class="but_query" style="float: right; width: 80px; height: 26px; line-height: 26px">重选</div>
                            <div class="clear"></div>
                        </h2>
                    </div>

                    <%if (dts != null && dts.Rows.Count > 0)
                        { %>

                    <div id="tab_list">
                        <ul class="tab_menu">
                            <%foreach (System.Data.DataRow item in dts.Rows)
                                { %>
                            <li data-catid="<%=item["CategoryID"]%>"><%=item["CategoryName"]%></li>
                            <%}%>
                        </ul>
                        <div class="clear"></div>

                        <ul class="state">
                            <li>
                                <div class="ins_c">直播昵称：</div>
                                <input id="searchKw" type="text" placeholder="请输入直播昵称" style="width: 173px;"></li>
                            <li class="but_query" id="btnSearch" style="width: 70px; margin-right: 10px">查询</li>
                            <li class="but_query" id="selectMedia" style="float: right; width: 70px; margin-right: 10px">选择媒体</li>
                            <li class="but_query" id="zhibo" style="float: right; width: 70px; margin-right: 10px">预览</li>
                            <li class="but_query" id="publishOperate" style="float: right; width: 70px; margin-right: 10px">发布</li>
                            <div class="clear"></div>
                        </ul>
                    </div>
                </div>
                <div class="list_box mt20">
                    <div class="tab_box">
                        <div class="box">

                            <div class="table">
                                <table width="100%" border="0" cellspacing="1" cellpadding="0" bgcolor="#ddd">
                                    <thead>
                                        <tr>
                                            <th width="16%">媒体</th>
                                            <th width="15%">平台账号</th>
                                            <th width="15%">性别</th>
                                            <th width="15%">粉丝数</th>
                                            <th width="15%">收礼</th>
                                            <th width="12%">排序</th>
                                            <th width="12%">操作</th>
                                        </tr>
                                    </thead>
                                    <tbody id="contentList"></tbody>
                                </table>

                                <div class="list"></div>
                                <!--分页-->
                                <div class="green-black" id="pageContainer"></div>
                                <!--分页-->
                            </div>
                        </div>

                        <div class="clear"></div>
                    </div>
                </div>
                <% }
                    else
                    {%>
                <div class="order_r" style="cursor: pointer; height: 748px; color: blue; width: 890px; margin-left: 25px"><span id="Upload">选择分类</span></div>
                <% }%>
            </div>
            <div class="clear"></div>
        </div>
    </form>
    <!--#include file="/base/footer.html" -->

    <script src="../js/layer/layer.js"></script>
    <script src="js/RecommendList.js"></script>

    <script type="text/javascript">

        $(document).ready(function () {
            $("#tab_list .tab_menu li:first").attr("class", "selected");
            $.recommend.init({
                businesstype: 14005
            });
            $("div").data("CategoryID", 25)
            $("div").data("MediaTypeID", 14005)
        });
    </script>
</body>
</html>
