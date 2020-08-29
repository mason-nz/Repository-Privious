<%@ page language="java" contentType="text/html; charset=UTF-8"
    pageEncoding="UTF-8"%>
    <link rel="stylesheet" type="text/css" href="/css/reset.css"/>
    <link rel="stylesheet" type="text/css" href="/css/layout.css"/>
    <link rel="icon" href="${pageContext.request.contextPath }/images/favicon.ico" type="image/x-icon">
  	<link rel="stylesheet" href="${pageContext.request.contextPath }/js/ztree/zTreeStyle.css" type="text/css" />
   	<script type="text/javascript" src="${pageContext.request.contextPath }/js/jquery.1.11.3.min.js"></script>
   	<script type="text/javascript" src="/js/laydate.js"></script>
   	<script language="javascript" src="http://www.chitunion.com/api/check.ashx?NotCheckModule=true" type="text/javascript"></script>
   	<script type="text/javascript" src="/js/tab.js"></script>
   	<script type="text/javascript" src="/js/Common_chitu.js"></script>
    <script type="text/javascript" src="/js/jquery.form.js"></script>
	<script src="${pageContext.request.contextPath }/js/ztree/jquery.ztree.all-3.5.js" type="text/javascript"></script>

<div id="top">
    <div class="box">
        <div class="left">
          	您好，欢迎来到赤兔联盟：一站式媒体在线交易平台</div>
        <ul class="right" id="hasLogin">
            <li id="userName">Hi， <a href=""></a></li>
  			<li>
            	<a href="http://www.chitunion.com/help.html" target="_blank" style="color: #fff">帮助中心</a>
            </li><!--             <li>|</li> -->
            <li><a href="http://www.chitunion.com/exit.aspx"><span style="color: #fff">退出</span></a></li>
        </ul>
        <div class="clear">
        </div>
    </div>
</div>
<div class="topBar">
    <div class="topBox">
        <a id="topLogo" href="http://www.chitunion.com/index.html" class="topLogo"></a>
        <ul id="topBoxlist" style="display: none;">
            <li><a href="http://www.chitunion.com/index.html" data-media="index">首页</a></li>
            <li><a href="http://www.chitunion.com/OrderManager/wx_list.html" data-media="wx_list">
                微信公众号</a></li>
            <li><a href="http://www.chitunion.com/OrderManager/wb_list.html" data-media="wb_list">
                新浪微博</a></li>
            <li><a href="http://www.chitunion.com/OrderManager/sp_list.html" data-media="sp_list">
                视频</a></li>
            <li><a href="http://www.chitunion.com/OrderManager/zb_list.html" data-media="zb_list">
                直播</a></li>
            <li><a href="http://www.chitunion.com/OrderManager/app_list.html" data-media="app_list">
                APP资源</a></li>
            <div class="clear">
            </div>
        </ul>
        <div class="clear">
        </div>
    </div>
</div>
<script>
    //头部页面加载后自调
    $(function () {
        if (CTLogin && CTLogin.IsLogin == true) {
            $('div.logon_sy_r').remove();
        }
        var userName = CTLogin.UserName; //用户姓名
        var userType = CTLogin.Category; //用户类型：媒体主——29002，AE——29001，运营。媒体主没有header的列表
        var RoleIDs = CTLogin.RoleIDs;
        if (userName) {//如果用户已登录并获取到，则显示用户姓名
            if (userType == 29002&&RoleIDs == 'SYS001RL00003') {//判断是否为媒体主
                $('#topBoxlist').hide();
            	$('#topLogo').attr('href','http://j.chitunion.com/userInfo/toUserMediaList');
            } else {
                $('#topBoxlist').show();
                var href = window.location.href.split('/') ? window.location.href.split('/') : null;
                if (href && href.length > 0) {
                    var nowHref = href ? href[href.length - 1] : null;
                    var media = nowHref ? nowHref.split('.')[0] : null; //媒体类型 14001-微信 14002-APP 14003-微博 14004-视频 14005-直播
                    if (media) {//根据页面点击的媒体类型，显示被选中状态
                        $('#topBoxlist a').removeClass('active');
                        $('#topBoxlist a[data-media=' + media + ']').addClass('active');
                    } else {
                        $('#topBoxlist a').removeClass('active');
                    }
                }
            }
            $('#userName a').html(userName);
            $('#hasLogin').show();
            $('#noLogin').hide();

            if (RoleIDs) {
                $('#userName a').click(function (e) {
                    e.preventDefault();
                    if (RoleIDs == 'SYS001RL00002' || RoleIDs == 'SYS001RL00004' || RoleIDs == 'SYS001RL00005') {//广告主、AE、运营——项目列表
                        window.location.href = 'http://www.chitunion.com/OrderManager/ListOfProject.html';
                    } else if (RoleIDs == 'SYS001RL00003') {//媒体主——个人中心
                        window.location.href = 'http://j.chitunion.com/userInfo/toUserMediaList';
                    } else if (RoleIDs == 'SYS001RL00006') {//策划——标签管理页面
                        window.location.href = 'http://j.chitunion.com/title/mediaNoTitleList';
                    } else {//其他——项目列表
                        window.location.href = 'http://www.chitunion.com/OrderManager/ListOfProject.html';
                    }
                });
            }

        } else {//如果用户未登录，则显示帮助中心
            $('#noLogin ').show();
            $('#hasLogin').hide();
        }
        var orderID = GetQueryString("orderID");
        if (orderID && orderID != 'undefined') {
            $('#topBoxlist').hide();
        }
        function GetQueryString(name) {
            var reg = new RegExp("(^|&)" + name + "=([^&]*)(&|$)");
            var r = window.location.search.substr(1).match(reg);
            if (r != null) return unescape(r[2]); return null;
        }
    });
</script>

