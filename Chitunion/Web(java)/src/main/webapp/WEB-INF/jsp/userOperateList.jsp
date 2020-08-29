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
</head>

<body>
	<!--顶部logo 导航-->
	<jsp:include page="header.jsp"></jsp:include>
	<script type="text/javascript" src="${pageContext.request.contextPath }/js/vmc.slider.full.min.js"></script>
	<script type="text/javascript" src="${pageContext.request.contextPath }/js/tab.js"></script>
	<script src="${pageContext.request.contextPath }/js/jquery.pagination.js" type="text/javascript"></script>
	<div class="list_main">
		<!--中间内容-->
		<div class="order">
			<jsp:include page="Menu.jsp"></jsp:include>
			<div class="order_r">
				<div class="mb15">
				<c:if test="${ map.SYS001MOD1200103}">
					<a
						href="${pageContext.request.contextPath }/userInfo/user/getEmployeeInfo/0"><span
						class="but_add" style="width: 145px;">新建用户</span></a>
				</c:if>
				</div>
				<ul class="state">
					<li>手机号：<input maxlength="11" onkeyup="value=value.replace(/[^\d]/g,'')" name="mobile" type="text" style="width: 132px;"></li>
					<li>真实姓名：<input name="trueName" type="text"
						style="width: 132px;"></li>
					<li>角 色： <select name="role" id="role"
						style="width: 120px; line-height: 30px">
							<option value="-1">全部</option>
					</select>
					</li>
					<li>状 态： <select name="status" id="status"
						style="width: 120px; line-height: 30px">
							<option value="-1">全部</option>
							<option value="0">启用</option>
							<option value="1">禁用</option>
					</select>
					</li>
					<li>
					<a class="but_query" onclick="caXun()" id="cx"
						style="width: 70px; margin-right: 10px">查询</a>
					</li>
					<div class="clear"></div>
				</ul>
				<div class="clear"></div>

				<div class="table">
					<div class="assign">
						<span><input type="checkbox" id="selectAll"/> 选择</span> 
						<c:if test="${map.SYS001MOD1200104}">
						<span>
						<a onclick="status(0)">启用</a>
						</span> 
							<span><a
							onclick="status(1)">禁用</a></span> 
						</c:if>
						<c:if test="${map.SYS001MOD1200105}">
							<span>
							<a
							onclick="initPWD()">重置密码</a></span>
						</c:if>
					</div>
					<table id="userList" width="100%" border="0" cellspacing="1"
						cellpadding="0" bgcolor="#ddd">
						<tr>
							<th width="5%">&nbsp;</th>
							<th width="10%">编号</th>
							<th width="15%">手机号</th>
							<th width="15%">真实姓名</th>
							<th width="10%">角色</th>
							<th width="15%">注册时间</th>
							<th width="10%">状态</th>
							<th width="15%">操作</th>
						</tr>
					</table>
					<div class="no_data"></div>
					<!--分页-->
					<div class="green-black" id="pageContainer"></div>
				</div>
			</div>
			<div class="clear"></div>
		</div>
	</div>
		<!--底部-->
		<jsp:include page="footer.jsp"></jsp:include>
	
	<script type="text/javascript">
		function singleStatus(userID, status, i) {
			var userArray = [];
			var array = {};
			var user = {};
			user.userID = userID;
			if (status == 1) {
				user.status = 0;
				statusMessage='启用';
			}
			if (status == 0) {
				user.status = 1;
				statusMessage='禁用';
			}
			userArray.push(user);
			array.array = userArray;
			$.ajax({
				url : "/userInfo/upUserInfoByRquest",
				type : "post",
				dataType : "json",
				data : JSON.stringify(array),
				success : function(data) {
					if (data.code == 0) {
						alert(statusMessage+"成功");
						caXun();
					} else {
						alert(statusMessage+"失败");
					}
				},
				contentType : 'application/json'
			});
		}
		function status(param) {
			var userArray = [];
			var array = {};
			var statusMessage='';
			if(param==0){
				statusMessage='启用';
			}
			if(param==1){
				statusMessage='禁用';
			}
			$(".cks").each(function() {
				var user = {};
				if ($(this).prop("checked")) {
					user.userID = $(this).val();
					user.status = param;
					userArray.push(user);
				}
			});
			array.array = userArray;
			if (userArray != null && userArray.length != 0) {
				$.ajax({
					url : "/userInfo/upUserInfoByRquest",
					type : "post",
					dataType : "json",
					data : JSON.stringify(array),
					success : function(data) {
						if (data.code == 0) {
							caXun();
							alert(statusMessage+"成功");
						} else {
							alert(statusMessage+"失败");
						}
					},
					contentType : 'application/json'
				});
			} else {
				if (param == 1) {
					alert("请选择要禁用的用户");
				}
				if (param == 0) {
					alert("请选择要启用的用户");
				}
			}
		};
		function initPWD() {
			var userArray = [];
			var userArr = {};
			$(".cks").each(function() {
				var user = {};
				if ($(this).prop("checked")) {
					user.userID = $(this).val();
					user.category = $(this).next().text();
					user.userName = $(this).next().next().text();
					user.mobile=$(this).next().next().next().text();
					user.mark = 1;
					userArray.push(user);
				}
			});
			userArr.array = userArray;
			if (userArray != null && userArray.length != 0) {
				$.ajax({
					url : "/userInfo/upUserInfoByRquest",
					type : "post",
					dataType : "json",
					data : JSON.stringify(userArr),
					success : function(data) {
						if (data.code == 0) {
							alert("密码重置成功");
							caXun();
						} else {
							alert("密码重置失败");
						}
					},
					contentType : 'application/json'
				});
			} else {
				alert("请选择重置密码对象");
			}
		}
	</script>
	<script type="text/javascript">
		category = 29001;
		function initUserList(data) {
			var role=$("#role").val();
			$(".tr").remove();
			$(".roleID").remove();
			var map = data.roleList;
			var user = data.userInfo;
			$("[name='role'].roleID").remove();
			for ( var j in map) {
				$("[name='role']").append(
						"<option class='roleID' value='"+map[j].roleID+"'>" + map[j].roleName
								+ "</option>");
			}
			for ( var i in user) {
				$("#userList")
						.append(
								"<tr class='tr'><td><input class='cks' type='checkbox' value='"+user[i].userID+"'><span style='display: none;'>"
										+ user[i].category
										+ "</span><span style='display: none;'>"
										+ user[i].userName
										+ "</span><span style='display: none;'>"
										+ user[i].mobile
										+ "</span></td><td id='userID"+i+"'>"
										+ user[i].userID
										+ "</td><td id='mobile"+i+"' name='mobile' value='"+user[i].mobile+"'>"
										+ user[i].mobile
										+ "</td><td id='trueName"+i+"' namem='trueName' value='"+user[i].trueName+"'>"
										+ user[i].trueName
										+ "</td><td id='roleID"+i+"' value='"+user[i].roleID+"'>"
										+ user[i].roleName
										+ "</td><td id='createTime"+i+"'>"
										+ user[i].createStartTime
										+ "</td><td id='status"+i+"'>"
										+ user[i].status
										+ "</td><td><a id='power"+i+"' href='${pageContext.request.contextPath }/userInfo/user/getEmployeeInfo/"+user[i].userID+"'>设置权限</a>  <a><span id='aStatus"
										+ i
										+ "' onclick='singleStatus("
										+ user[i].userID
										+ ","
										+ user[i].status
										+ "," + i + ")'></span></a></td></tr>");
				if (user[i].status == 1) {
					if('${map.SYS001MOD1200104}'=='true'){
						$("#aStatus" + i).text("启用");
					}
					$("#status" + i).text("禁用");
				} else{
					if('${map.SYS001MOD1200104}'=='true'){
						$("#aStatus" + i).text("禁用");
					}
					$("#status" + i).text("启用");
				}
				if('${map.SYS001MOD1200106}'=='false'){
					$("#power"+i).text("");
				}
				if (user[i].roleID == null) {
					$("#roleID" + i).text("");
				}
				if (user[i].roleName == undefined) {
					$("#roleID" + i).text("");
				}
				if (user[i].trueName == null || user[i].trueName == '') {
					$("#trueName" + i).text("");
				}
			}
			$("#role").val(role);
			$("#userList").find(".cks").click(function() {
				var HasCheckedNum = $("#userList").find(".cks:checked").length;
				if (HasCheckedNum == $("#userList").find(".cks").length) {
					$("#selectAll")[0].checked = true;
				} else {
					$("#selectAll")[0].checked = false;
				}
			});
		};
		function page(count, pageRows, url, user) {
			//分页----begin----
			$("#pageContainer").pagination(
            		count,
                        {
                            items_per_page: pageRows, //每页显示多少条记录（默认为20条）
                            callback: function (currPage, jg) {
								console.info(jg);
                            	user.curPage = currPage;
            					$.ajax({
            						url : "/userInfo/" + url,
            						type : "post",
            						dataType : "json",
            						data : JSON.stringify(user),
            						success : function(data) {
            							$("#selectAll")[0].checked = false;
            							initUserList(data);
            						},
            						contentType : 'application/json'
            					});
                            } //回调函数
                        });
			if(count==0){
				$(".no_data").html("<img src='${pageContext.request.contextPath }/images/no_data.png'>");
				$("#pageContainer").html("");
			}else{
				$(".no_data").html("");
			}
			//分页----end----
		}
		function caXun() {
			var user = {};
			user.trueName = $("[name='trueName']").val();
			user.status = $("[name='status']").val();
			user.mobile = $("[name='mobile']").val();
			user.roleID = $("[name='role']").val();
			user.curPage = 1;
			$.ajax({
				url : "/userInfo/selectAddUserList",
				type : "post",
				dataType : "json",
				data : JSON.stringify(user),
				success : function(data) {
					$("#selectAll")[0].checked = false;
					var datas = [];
					if(data!=null){
						datas = data.userInfo;
						if (datas != null && datas.length != 0) {
							count = datas[0].count;
							pageRows = datas[0].pageRows;
						} else {
							count = 0;
							pageRows = 5;
						}
						page(count, pageRows, 'selectAddUserList', user);
						initUserList(data);
					}
				},
				contentType : 'application/json'
			});
		}
		$(function() {
			//全选
			$("#selectAll").click(function() {
				if ($(this).is(":checked")) {
					$("#userList").find(".cks").each(function() {
						$(this)[0].checked = true;
					});
				} else {
					$("#userList").find(".cks").each(function() {
						$(this)[0].checked = false;
					});
				}
			});
			var user = {};
			user.curPage = 1;
			user.category = category;
			//获取角色   
			$.ajax({
				url : "/userInfo/getUserOperationList",
				type : "post",
				dataType : "json",
				data : JSON.stringify(user),
				success : function(data) {
					var datas = [];
					if(data!=null){
						datas = data.userInfo;
						if (datas != null && datas.length != 0) {
							count = datas[0].count;
							pageRows = datas[0].pageRows;
						} else {
							count = 0;
							pageRows = 5;
						}
						page(count, pageRows, 'getUserOperationList', user);
						initUserList(data);
					}
				},
				contentType : 'application/json'
			});
		});
	</script>
</body>
</html>
