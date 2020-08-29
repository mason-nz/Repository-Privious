<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="HomeAppList.aspx.cs" Inherits="XYAuto.ITSC.Chitunion2017.Web.RecommendManager.HomeAppList" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">

<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title>首页app-推荐列表</title>
    <!--#include file="/base/js.html" -->
    <script type="text/javascript" src="../js/jquery.jmpopups-0.5.1.pack.js"></script>
    <script type="text/javascript" src="../OrderManager/js/jquery.uploadify.js"></script>
    <script type="text/javascript" src="../js/jquery.pagination.js"></script>
    <script src="js/preview.js"></script>
</head>
<body>
    <!--#include file="/base/header.html" -->
    <form id="form1" runat="server">
        <!--中间内容-开始 -->
        <div class="order">
            <!--左侧菜单-->
            <!--#include file="/base/Menu.html" -->
            <div class="order_r">
                <div class="list_box">
                    <div class="list">
                        <h2>
                            <div class="fl">推荐管理 &gt; 首页APP推荐</div>
                            <%-- <div id="again" class="but_query" style="float: right; width: 80px; height: 26px; line-height: 26px">重选</div>--%>
                            <div class="clear"></div>
                        </h2>
                    </div>
                    <div id="tab_list">
                        <div class="clear"></div>
                        <ul class="state">
                            <li>
                                <div class="ins_c" style="width: inherit">平台媒体名称：</div>
                                <input id="searchKw" type="text" placeholder="请输入平台媒体名称" style="width: 173px;" /></li>
                            <li class="but_query" id="btnSearch" style="width: 70px; margin-right: 10px">查询</li>
                            <li class="but_query" id="selectMedia" style="float: right; width: 70px; margin-right: 10px">选择媒体</li>
                            <li class="but_query" id="app" style="float: right; width: 70px; margin-right: 10px">预览</li>
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
                                            <th width="15%">广告位位置</th>
                                            <th width="15%">广告位形式/名称</th>
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
            </div>
            <div class="clear">
            </div>
        </div>
        <!--中间内容-结束 -->
    </form>
    <!--#include file="/base/footer.html" -->

    <script src="../js/layer/layer.js"></script>
    <script src="js/RecommendList.js"></script>

    <script type="text/javascript">

        $(document).ready(function () {
            $.recommend.init({
                businesstype: 14002
            });
        });
    </script>
</body>
</html>
