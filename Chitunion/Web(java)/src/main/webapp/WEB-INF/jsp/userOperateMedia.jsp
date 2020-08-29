<%@ page language="java" contentType="text/html; charset=UTF-8"
	pageEncoding="UTF-8"%>
		<%@page import="org.apache.jasper.tagplugins.jstl.core.Import"%>
	<%@ taglib uri="http://java.sun.com/jsp/jstl/functions" prefix="fn"  %>
	<%@ taglib uri="http://java.sun.com/jsp/jstl/core" prefix="c"%>
<!DOCTYPE html PUBLIC "-//W3C//DTD HTML 4.01 Transitional//EN" "http://www.w3.org/TR/html4/loose.dtd">
<html>
<script type="text/javascript">
	category=29002;
</script>
<head>
<meta charset="utf-8">
<meta name="description" content="描述文字，字数200内？">
<meta name="keywords" content="网站相关关键字，字数200内？">
<title>赤兔联盟平台</title>
<link rel="icon" href="${pageContext.request.contextPath }/images/favicon.ico" type="image/x-icon">
<script src="${pageContext.request.contextPath }/js/Area2.js"
	type="text/javascript"></script>
<script src="${pageContext.request.contextPath }/js/Common.js"
	type="text/javascript"></script>
</head>
<body>
	<!--顶部logo 导航-->
	<jsp:include page="header.jsp"></jsp:include>
	<script src="${pageContext.request.contextPath }/js/jquery.pagination.js" type="text/javascript"></script>
        <div class="list_main">
		<!--中间内容-->
		<div class="order">
			<jsp:include page="Menu.jsp"></jsp:include>
			<div class="order_r">
				<div class="mb15">
				<c:if test="${ map.SYS001BUT00110204}">
					<a href="${pageContext.request.contextPath }/userInfo/user/getInfoAndDetail/29002/0">
					<span class="but_add" style="width: 145px;">新建媒体主</span></a>
				</c:if>
				</div>
				<ul class="state">
					<li><div class="ins_c">手机号：</div><input name="mobile" maxlength="11" onkeyup="value=value.replace(/[^\d]/g,'')" type="text" style="width: 140px;"></li>
					<li><div class="ins_c">公司名称：</div><input name="trueName" type="text" style="width:140px;"></li>
					<li><div class="ins_c">所在地区：</div>
					 <select id="ddlProvince" name="ddlProvince" onchange="crmCustCheckHelper.triggerProvince();"
						style="width:110px;line-height: 30px">
							<option value="-1">省/直辖市</option>
					</select> 
					<select id="ddlCity" name="ddlCity"
						onchange="crmCustCheckHelper.triggerCity();"
						style="width:110px;line-height: 30px">
							<option value="-1">城市</option>
					</select> <select id="ddlCounty" name="ddlCounty"
						style="width:110px;line-height: 30px">
							<option value="-1">区/县</option>
					</select>
					</li>
					<div class="clear"></div>
					<li>
						<div class="ins_c">状 态：</div>
							<select name="status" id="status" style="width:152px;line-height: 30px">
								<option value="-1">全部</option>
								<option value="0">启用</option>
								<option value="1">禁用</option>
							</select>
					</li>
					<li><div class="ins_c">类 型：</div>
							<select name="type" id="type" style="width: 152px; line-height: 30px">
								<option value="-1">全部</option>
								<option value="1001">企业</option>
								<option value="1002">个人</option>
							</select>
					</li>
					<li><div class="ins_c">注册日期：</div>
				                <input id="beginDate" name="beginDate" type="text" readonly="readonly"  style="width:102px" class="laydate-icon" onClick="laydate({fixed:false,elem:'#beginDate'})">
				                至
				                <input id="endDate" name="endDate" type="text" readonly="readonly"  style="width:102px;" class="laydate-icon" onClick="laydate({fixed:false,elem:'#endDate'})">
				     </li>
					<li><a class="but_query" onclick="caXun()" style="width:70px;margin-right:0">查询</a></li>
					<div class="clear"></div>
				</ul>
				<div class="clear"></div>
				<div class="table">
					<div class="assign">
						<span><input type="checkbox" id="selectAll"></span> 
							<c:if test="${map.SYS001BUT00110201}">
							<span><a onclick="status(0)">启用</a></span> 
							<span><a onclick="status(1)">禁用</a></span> 
							</c:if>
							<c:if test="${map.SYS001BUT00110202}">
							<span>
							<a onclick="initPWD()" href="javascript:void(0);" target="_self">重置密码</a>
							</span>
							</c:if>
					</div>
					<table id="userList" width="100%" border="0" cellspacing="1" cellpadding="0" bgcolor="#ddd">
						<tr>
		                    <th width="5%">&nbsp;</th>
		                    <th width="5%">编号</th>
		                    <th width="15%">手机号</th>
		                    <th width="8%">类型</th>
		                    <th width="15%">公司名称/真实姓名</th>
		                    <th width="17%">所在地区</th>
		                    <th width="15%">注册时间</th>
		                    <th width="8%">状态</th>
		                    <th width="12%">操作</th>
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
			BindProvince('ddlProvince'); //绑定省份
			initPage(1);
		});
		function initUserList(data) {
			$(".tr").remove();
			var aeRole = [];
			aeRole = data.aeRole;
			var user = data.userInfo;
//				console.info(user);
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
										+ "</td><td id='mobile' name='mobile' value='"+user[i].mobile+"'>"
										+ user[i].mobile
										+ "</td><td id='type"+i+"' value='"+user[i].type+"'>"
										+ user[i].typeName
										+ "</td><td style='word-break:break-all;width:15px;' id='trueName"+i+"' value='"+user[i].trueName+"'>"
										+ user[i].trueName
										+ "</td><td id='address' value='"+user[i].address+"'>"
										+ user[i].address
										+ "</td><td id='createTime"+i+"'>"
										+ user[i].createStartTime
										+ "</td><td id='status"+i+"'>"
										+ user[i].statusName
										+ "</td><td><a id='editor"+i+"' href='${pageContext.request.contextPath }/userInfo/user/getInfoAndDetail/"+category+"/"+user[i].userID+"'></a>  <a><span id='aStatus"
										+ i
										+ "' onclick='singleStatus("
										+ user[i].userID
										+ ","
										+ user[i].status
										+ ","
										+ i
										+ ")'></span></a> </td></tr>");
				if (user[i].typeName == null) {
					$("#type" + i).text("");
				}
				if (user[i].authAEUserID == null) {
					$("#authAEUserID" + i).text("");
				}
				if (user[i].statusName == '启用'&&'${map.SYS001BUT00110201}'=='true') {
					$("#aStatus" + i).text("禁用");
				}else  if('${map.SYS001BUT00110201}'=='true') {
					$("#aStatus" + i).text("启用");
				}
				if ('${map.SYS001BUT00110203}'=='true'){
					$("#editor" + i).text("编辑");
				}
				
				if (user[i].trueName == null || user[i].trueName == '') {
					$("#trueName" + i).text("");
				}
			}
			$("#userList").find(".cks").click(function() {
				var HasCheckedNum = $("#userList").find(".cks:checked").length;
				if (HasCheckedNum == $("#userList").find(".cks").length) {
					$("#selectAll")[0].checked = true;
				} else {
					$("#selectAll")[0].checked = false;
				}
			});
		};
		function initPage(curPage) {
			var user = {};
			user.curPage = curPage;
			user.category=category;
			$.ajax({
				url : "/userInfo/getUserInfo",
				type : "post",
				dataType : "json",
				data : JSON.stringify(user),
				success : function(data) {
					var datas = [];
					datas = data.userInfo;
					if (datas != null && datas.length != 0) {
						count = datas[0].count;
						pageRows = datas[0].pageRows;
					} else {
						count = 0;
						pageRows = 5;
					}
					page(count, pageRows, 'getUserInfo', user);
					initUserList(data);
				},
				contentType : 'application/json'
			});
		};
			
			function caXun() {
				var user = {};
				user.trueName = $("[name='trueName']").val();
				user.source = $("[name='source']").val();
				user.status = $("[name='status']").val();
				user.type = $("[name='type']").val();
				user.mobile = $("[name='mobile']").val();
				user.createStartTime = $("[name='beginDate']").val();
				user.createEndTime = $("[name='endDate']").val();
				user.cityID = $("[name='ddlCity']").val();
				user.counntyID = $("[name='ddlCounty']").val();
				user.provinceID = $("[name='ddlProvince']").val();
				user.category=category;
				user.curPage = 1;
				if(user.createStartTime!=''&&user.createStartTime!=null&&user.createEndTime!=''&&user.createEndTime!=null){
					if(user.createStartTime>user.createEndTime){
						alert("开始日期必须小于等于结束日期");
						return;
					}
				}
				$("#selectAll")[0].checked = false;
				$.ajax({
							url : "${pageContext.request.contextPath }/userInfo/getUserInfo",
							type : "post",
							dataType : "json",
							data : JSON.stringify(user),
							success : function(data) {
								var datas = [];
								datas = data.userInfo;
								if (datas != null && datas.length != 0) {
									count = datas[0].count;
									pageRows = datas[0].pageRows;
								} else {
									count = 0;
									pageRows = 5;
								}
								page(count, pageRows, 'getUserInfo', user);
								initUserList(data);
							},
							contentType : 'application/json'
						});
			}
			
			function singleStatus(userID, status, i) {
				var userArray = [];
				var array = {};
				var user = {};
				var statusMessage='';
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
// 				console.info(array);
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
			var crmCustCheckHelper = (function() {
				var triggerProvince = function() {//选中省份
					BindCity('ddlProvince', 'ddlCity');
					BindCounty('ddlProvince', 'ddlCity', 'ddlCounty');
				},

				triggerCity = function() {//选中城市
					BindCounty('ddlProvince', 'ddlCity', 'ddlCounty');
				};
				return {
					triggerProvince : triggerProvince,
					triggerCity : triggerCity
				};
			})();

			function page(count, pageRows, url, user) {
				  $("#pageContainer").pagination(
	                		count,
	                            {
	                                items_per_page: pageRows, //每页显示多少条记录（默认为20条）
	                                callback: function (currPage, jg) {
	                                	user.curPage = currPage;
	            						$.ajax({
	            							url : "/userInfo/" + url,
	            							type : "post",
	            							dataType : "json",
	            							data : JSON.stringify(user),
	            							success : function(data) {
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
			}//分页----end----
		</script>
</body>
</html>
