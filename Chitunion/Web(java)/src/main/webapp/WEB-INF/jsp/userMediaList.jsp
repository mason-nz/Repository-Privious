<%@ page language="java" contentType="text/html; charset=UTF-8"
	pageEncoding="UTF-8"%>
	<%@page import="org.apache.jasper.tagplugins.jstl.core.Import"%>
	<%@ taglib uri="http://java.sun.com/jsp/jstl/functions" prefix="fn"  %>
	<%@ taglib uri="http://java.sun.com/jsp/jstl/core" prefix="c"%>
<!DOCTYPE html PUBLIC "-//W3C//DTD HTML 4.01 Transitional//EN" "http://www.w3.org/TR/html4/loose.dtd">
<html>
<head>
<meta charset="utf-8">
<meta name="description" content="描述文字，字数200内？">
<meta name="keywords" content="网站相关关键字，字数200内？">
<title>赤兔联盟平台</title>
<link rel="icon" href="${pageContext.request.contextPath }/images/favicon.ico" type="image/x-icon">
<script type="text/javascript"
	src="${pageContext.request.contextPath }/js/jquery.1.11.3.min.js"></script>
<script type="text/javascript"
	src="${pageContext.request.contextPath }/js/vmc.slider.full.min.js"></script>
<script type="text/javascript"
	src="${pageContext.request.contextPath }/js/tab.js"></script>
</head>
<body>
	<!--顶部logo 导航-->
	<jsp:include page="header.jsp"></jsp:include>
	<div class="list_main">
		<!--中间内容-->
		<div class="order">
			<jsp:include page="Menu.jsp"></jsp:include>
			<div class="order_r">
				<div class="mb15 f18">
					欢迎 <span class="yellow" id="loginName" >${CTLogin.UserName}</span> 使用行圆变现系统
					<c:if test="${ map.SYS001BUT1001}">
					 <a
						href='${pageContext.request.contextPath }/userInfo/user/getLoginDetail'
						class="yellow">完善资料</a>，将享有特权
					</c:if>
				</div>
				<h2 class="blue">
					<img src="${pageContext.request.contextPath }/images/line_01.png">
					我的媒体资源
				</h2>
				<c:if test="${ map.SYS001BUT1003}">
				<div class="grey"></div>
				<ul class="media">
					<li><a
						href="http://www.chitunion.com/MediaManager/weChat_addEdit.html?operatetype=1"><img
							src='${pageContext.request.contextPath }/images/media_01.png'
							onmouseover='src="${pageContext.request.contextPath }/images/media_h.png"'
							onmouseout='src="${pageContext.request.contextPath }/images/media_01.png"'></a></li>
					<li><a
						href="http://www.chitunion.com/MediaManager/video_addEdit.html?operatetype=1"><img
							src='${pageContext.request.contextPath }/images/media_02.png'
							onmouseover='src="${pageContext.request.contextPath }/images/media_h.png"'
							onmouseout='src="${pageContext.request.contextPath }/images/media_02.png"'></a></li>
					<li><a
						href="http://www.chitunion.com/MediaManager/weibo_addEdit.html?operatetype=1"><img
							src='${pageContext.request.contextPath }/images/media_03.png'
							onmouseover='src="${pageContext.request.contextPath }/images/media_h.png"'
							onmouseout='src="${pageContext.request.contextPath }/images/media_03.png"'></a></li>
					<li><a
						href="http://www.chitunion.com/MediaManager/zhibo_addEdit.html?operatetype=1"><img
							src='${pageContext.request.contextPath }/images/media_04.png'
							onmouseover='src="${pageContext.request.contextPath }/images/media_h.png"'
							onmouseout='src="${pageContext.request.contextPath }/images/media_04.png"'></a></li>
					<li><a
						href="http://www.chitunion.com/MediaManager/platform_addEdit.html?operatetype=1"><img
							src='${pageContext.request.contextPath }/images/media_05.png'
							onmouseover='src="${pageContext.request.contextPath }/images/media_h.png"'
							onmouseout='src="${pageContext.request.contextPath }/images/media_05.png"'></a></li>
					<div class="clear"></div>
				</ul>
				</c:if>
				<div class="table">
					<div class="mb15">
						<span class="fl f16 blue"><img
							src="${pageContext.request.contextPath }/images/line_01.png">
							我的订单</span> <span class="fr f14">
							<c:if test="${ map.SYS001BUT1002}">
							<a
							href="http://www.chitunion.com/OrderManager/ListOfOrder.html"
							class="blue2">查看全部订单</a>
							</c:if>
							</span>
						<div class="clear"></div>
					</div>
					<table id="userList" width="100%" border="0" cellspacing="1"
						cellpadding="0" bgcolor="#ddd">
						<tr>
							<th width="14%">订单编号</th>
							<th width="20%">订单名称</th>
							<th width="20%">执行周期</th>
							<th width="13%">订单金额(元)</th>
							<th width="11%">资源类型</th>
							<th width="20%">下单时间</th>
						</tr>
					</table>
					<div class="no_data"></div>
				</div>
			</div>
			<div class="clear"></div>
		</div>
	</div>
	<!--底部-->
	<jsp:include page="footer.jsp"></jsp:include>
	<script type="text/javascript">
	$(function() {
		$("#loginName").text(CTLogin.UserName);
		var user = {};
		user.curPage = 1;
		//获取角色   
		$.ajax({
			url : "/userInfo/getUserInfoOrder",
			type : "post",
			dataType : "json",
			data : JSON.stringify(user),
			success : function(data) {
				var datas = [];
				datas = data;
				if (datas != null && datas.length != 0) {
					$(".no_data").html("");
				} else {
					$(".no_data").html("<img src='${pageContext.request.contextPath }/images/no_data.png'>");
				}
				initUserList(datas);
			},
			contentType : 'application/json'
		});
	});
		function initUserList(data) {
			$(".tr").remove();
			var user = data;
			for ( var i in user) {
				$("#userList")
						.append(
								"<tr class='tr'><td id='subOrderID"+i+"'><a href='http://www.chitunion.com/OrderManager/ListDetaileOfOrder.html?suborderid="
										+ user[i].subOrderID
										+ "'>"
										+ user[i].subOrderID
										+ "</a></td><td id='orderName"+i+"' namem='orderName'>"
										+ user[i].orderName
										+ "</td><td id='beginEndTime"+i+"' value='"+user[i].beginEndTime+"'>"
										+ user[i].beginEndTime
										+ "</td><td id='totalAmount"+i+"'>"
										+ user[i].totalAmount
										+ "</td><td id='mediaType"+i+"' name='mediaType' value='"+user[i].mediaType+"'>"
										+ user[i].mediaTypeName
										+ "</td><td id='createTime"+i+"'>"
										+ user[i].createTime + "</td></tr>");
				if (user[i].orderName == null || user[i].orderName == '') {
					$("#orderName" + i).text("");
				}
			}
		};
	</script>
</body>
</html>
