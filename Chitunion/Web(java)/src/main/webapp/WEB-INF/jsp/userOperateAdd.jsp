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
<script type="text/javascript">
aeRoleID='SYS001RL00005';
category=29001;
</script>
</head>
<body>
	<!--顶部logo 导航-->
	<jsp:include page="header.jsp"></jsp:include>
	<script src="${pageContext.request.contextPath }/js/tab.js" type="text/javascript" ></script>
	<script src="${pageContext.request.contextPath }/js/Area2.js" type="text/javascript"></script>
	<script src="${pageContext.request.contextPath }/js/Common.js" type="text/javascript"></script>
	<script src="${pageContext.request.contextPath }/js/laydate.js" type="text/javascript" ></script>
	<script src="${pageContext.request.contextPath }/js/jquery.pagination.js" type="text/javascript"></script>
	<script src="${pageContext.request.contextPath }/js/jquery-ui.js" type="text/javascript"></script>
	<div class="list_main">
	<!--中间内容-->
	<div class="order">
		<jsp:include page="Menu.jsp"></jsp:include>
		<div class="order_r">
			<div class="mb15">
			<c:if test="${ map.SYS001BUT00110102}">
				<a href="${pageContext.request.contextPath }/userInfo/user/getInfoAndDetail/29001/0">
					<span class="but_add" style="width: 145px;">新建广告主</span>
				</a>
			</c:if>
			</div>
			<ul class="state">
				<li><div class="ins_c">手机号：</div><input maxlength="11" onkeyup="value=value.replace(/[^\d]/g,'')" name="mobile" type="text" style="width:140px;"></li>
				<li><div class="ins_c">公司名称：</div><input name="trueName" type="text" style="width:140px;"></li>
				<li><div class="ins_c">所在地区：</div>
				<select id="ddlProvince" name="ddlProvince" onchange="crmCustCheckHelper.triggerProvince();"
					style="width:115px;line-height: 30px">
						<option value="-1">省/直辖市</option>
				</select> 
				<select id="ddlCity" name="ddlCity" onchange="crmCustCheckHelper.triggerCity();" style="width:115px;line-height: 30px">
						<option value="-1">城市</option>
				</select> 
				<select id="ddlCounty" name="ddlCounty" style="width:115px;line-height: 30px">
						<option value="-1">区/县</option>
				</select>
				</li>
				<div class="clear"></div>
				<li>
					<div class="ins_c">所在行业：</div>
					<select id="businessID" name="businessID" style="width:152px;line-height: 30px">
						<option value="-1">选择行业</option>
					</select>
				</li>
				<li><div class="ins_c">来 源：</div>
					 <select id="source" name="source" style="width:152px;line-height: 30px">
							<option value="-1">全部</option>
							<option value="3001">自营</option>
							<option value="3002">自助</option>
					</select>
				</li>
				<li><div class="ins_c">注册日期：</div>
		                <input id="beginDate"   name="beginDate" type="text" readonly="readonly" style="width:153px;" class="laydate-icon" onClick="laydate({fixed:false,elem:'#beginDate'})">
		               	 至
		                <input id="endDate" name="endDate" type="text" readonly="readonly"  style="width:153px;" class="laydate-icon" onClick="laydate({fixed:false,elem:'#endDate'})">
		        </li>
		        <div class="clear"></div>
				<li><div class="ins_c">状 态：</div> 
					<select name="status" id="status" style="width: 152px; line-height: 30px">
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
				<c:if test="${map.SYS001BUT00110101}">
				<li><div class="ins_c">AE：</div><input id="tags" autocomplete="off" name="aeName" style="width: 120px;"></li>
				</c:if>
				<li><a class="but_query" onclick="caXun()" style="width: 70px;">查询</a></li>
				<div class="clear"></div>
			</ul>
			<div class="clear"></div>
			<div class="table">
				<div class="assign">
					<span><input type="checkbox" id="selectAll"></span>
						 <span>
						<c:if test="${map.SYS001BUT00110101}">
							<a href="javascript:void(0);"  target="_self" onclick="onLayer()">指定AE</a>
						</c:if>
						</span> 
						<c:if test="${map.SYS001BUT00110104}">
						<span>
							<a href="javascript:void(0);"  target="_self" onclick="status(0)">启用</a>
						</span> 
						<span><a href="javascript:void(0);"  target="_self" onclick="status(1)">禁用</a>
						</span>
						</c:if>
						<c:if test="${ map.SYS001BUT00110105}">
						 <span>
						 	<a href="javascript:void(0);"  target="_self" onclick="initPWD()">重置密码</a>
						 </span>
						</c:if>
				</div>
				<table id="userList" width="100%" border="0" cellspacing="1" cellpadding="0" bgcolor="#ddd">
					<tr>
						<th width="4%">&nbsp;</th>
						<th width="5%">编号</th>
						<th width="10%">手机号</th>
						<th width="5%">类型</th>
						<th width="15%">公司名称/真实姓名</th>
						<th width="10%">所在地区</th>
						<th width="8%">所属行业</th>
						<th width="5%">来源</th>
						<c:if test="${map.SYS001BUT00110101}">
						<th width="7%">AE</th>
						</c:if>
						<th width="15%">注册时间</th>
						<th width="6%">状态</th>
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
	<!-- 指定AE弹出层 -->
	<div id="occlusion1"></div>
	<!--指定AE开始-->
	<div class="layer" id="layer1" style="display: none;">
		<div class="title">
			<div class="fl">选择AE账号</div>
			<div class="fr">
				<a href="javascript:void(0)" id="closebt"><img
					src="${pageContext.request.contextPath }/images/icon13.png" onclick="offLayer(1)"
					width="16" height="16"
					onMouseOver="this.src='${pageContext.request.contextPath }/images/icon14.png'"
					onMouseOut="this.src='${pageContext.request.contextPath }/images/icon13.png'"></a>
			</div>
			<div class="clear"></div>
		</div>
		<div class="layer_con">
			<div class="table" style="max-height: 400px;width: 500px;overflow: auto;">
				<table id="aeTable1" width="100%" border="0" cellspacing="1"
					cellpadding="0" bgcolor="#fff">
				</table>
			</div>
			<div class="keep">
				<span><a  href="javascript:void(0);"  target="_self" onclick="zhiAE()" class="button"
					style="width: 100px">提交</a></span> <span><a
					onclick="offLayer(1)" class="but_keep" style="width: 100px">取消</a></span>
			</div>
		</div>
	</div>
	<!--指定AE结束-->
	<!--指定AE开始-->
	<!-- 单用户指定AE弹出层 -->
	<div id="occlusion2"></div>
	<div class="layer" id="layer2" style="display: none;">
		<div class="title">
			<div class="fl">选择AE账号</div>
			<div class="fr">
				<a href="javascript:void(0)"  id="closebt"><img onclick="offLayer(2)"
					src="${pageContext.request.contextPath }/images/icon13.png"
					width="16" height="16"
					onMouseOver="this.src='${pageContext.request.contextPath }/images/icon14.png'"
					onMouseOut="this.src='${pageContext.request.contextPath }/images/icon13.png'"></a>
			</div>
			<div class="clear"></div>
		</div>
		<div class="layer_con" id="layer_con2">
			<div class="table" style="max-height: 400px;width: 500px;overflow: auto;">
				<table id="aeTable2" width="100%" border="0" cellspacing="1"
					cellpadding="0" bgcolor="#fff">
				</table>
			</div>
			<div class="keep">
				<span><a href="javascript:void(0);" href="javascript:void(0);"  target="_self" onclick="zhiSingleAE()" class="button"
					style="width: 100px">提交</a></span> <input style="display: none;"
					id="aeUserID"> <span><a
					onclick="offLayer(2)" class="but_keep" style="width: 100px">取消</a></span>
			</div>
		</div>
	</div>
	<!-- 逻辑 -->
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
			var user = {};
			user.curPage = 1;
			user.category=category;
			if(CTLogin.RoleIDs==aeRoleID){
				user.authAEUserID =CTLogin.UserID;
			}
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
					page(count, pageRows, 'getUserInfo', user,'pageContainer',0);
					initUserList(data);
				},
				contentType : 'application/json'
			});
		});
		function initUserList(data) {
			$(".tr").remove();
			var businessIDs=$("#businessID").val();
			$(".businessIDs").remove();
			var map = [];
			map = data.dictValues;
			var user = data.userInfo;
			for ( var j in map) {
				if (map[j].dictType == 2) {
					$("#businessID").append(
							"<option class='businessIDs' value='"+map[j].dictId+"'>"
									+ map[j].dictName + "</option>");
				}
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
										+ "</td><td id='mobile' name='mobile' value='"+user[i].mobile+"'>"
										+ user[i].mobile
										+ "</td><td id='type"+i+"' value='"+user[i].type+"'>"
										+ user[i].typeName
										+ "</td><td style='word-break: break-all;' id='trueName"+i+"'>"
										+ user[i].trueName
										+ "</td><td id='address' value='"+user[i].address+"'>"
										+ user[i].address
										+ "</td><td id='businessID"+i+"'>"
										+ user[i].businessName
										+ "</td><td id='source"+i+"' value='"+user[i].source+"'>"
										+ user[i].sourceName
										+ "</td><td id='authAEUserID"+i+"'>"
										+ user[i].aeName
										+ "</td><td id='createTime"+i+"'>"
										+ user[i].createStartTime
										+ "</td><td id='status"+i+"'>"
										+ user[i].statusName
										+ "</td><td><a id='editor"+i+"' href='${pageContext.request.contextPath }/userInfo/user/getInfoAndDetail/29001/"+user[i].userID+"'></a> <a><span id='aStatus"
										+ i
										+ "' onclick='singleStatus("
										+ user[i].userID
										+ ","
										+ user[i].status
										+ ","
										+ i
										+ ")'></span></a>  <a href='http://www.chitunion.com/OrderManager/wx_list.html?userID="+user[i].userIDPwd+"'><span id='order"+i+"'></span></a> <a id='ae"
										+ i
										+ "' href='javascript:void(0);'  target='_self' onclick='singloZhiAE("
										+ user[i].userID
										+ ")'></a></td></tr>");
				
				if (user[i].aeName == null) {
					$("#authAEUserID" + i).text("");
				}
				if (user[i].typeName == null) {
					$("#type" + i).text("");
				}
				if (user[i].trueName == null) {
					$("#trueName" + i).text("");
				}
				if (user[i].businessName == null) {
					$("#businessID" + i).text("");
				}
				if (user[i].isAuthAE == true&&'${map.SYS001BUT00110106}'=='true') {
					$("#order" + i).text("代客户下单");
				}
				if (user[i].authAEUserID == null) {
					$("#authAEUserID" + i).text("");
				}
				if (user[i].statusName == '启用'&&'${map.SYS001BUT00110104}'=='true') {
					$("#aStatus" + i).text("禁用");
				}else  if('${map.SYS001BUT00110104}'=='true') {
					$("#aStatus" + i).text("启用");
				}
				if ('${map.SYS001BUT00110103}'=='true'){
					$("#editor" + i).text("编辑");
				}
				if ('${map.SYS001BUT00110101}'=='true'){
					$("#ae" + i).text("指定AE");
				}
				if ('${map.SYS001BUT00110101}'=='false'){
					$("#authAEUserID" + i).remove();
				}
				if ('${map.SYS001BUT00110101}'=='false'&&'${map.SYS001BUT00110104}'=='false'&&'${map.SYS001BUT00110106}'=='false'&&'${map.SYS001BUT00110103}'=='false'){
					$("#editor" + i).remove();
				}
			}
			$("#businessID").val(businessIDs);
			$("#userList").find(".cks").click(function() {
				var HasCheckedNum = $("#userList").find(".cks:checked").length;
				if (HasCheckedNum == $("#userList").find(".cks").length) {
					$("#selectAll")[0].checked = true;
				} else {
					$("#selectAll")[0].checked = false;
				}
			});
		};
		function caXun() {
			var user = {};
			user.curPage = 1;
			user.trueName = $("[name='trueName']").val();
			user.aeName = $("[name='aeName']").val();
			user.userName = $("[name='userName']").val();
			user.source = $("[name='source']").val();
			user.status = $("[name='status']").val();
			user.type = $("[name='type']").val();
			user.mobile = $("[name='mobile']").val();
			user.createStartTime = $("[name='beginDate']").val();
			user.createEndTime = $("[name='endDate']").val();
			user.cityID = $("[name='ddlCity']").val();
			user.counntyID = $("[name='ddlCounty']").val();
			user.provinceID = $("[name='ddlProvince']").val();
			user.businessID = $("#businessID").val();
			user.category=category;
			if(user.createStartTime!=''&&user.createStartTime!=null&&user.createEndTime!=''&&user.createEndTime!=null){
				if(user.createStartTime>user.createEndTime){
					alert("开始日期必须小于等于结束日期");
					return;
				}
			}
			if(CTLogin.RoleIDs==aeRoleID){
				user.authAEUserID =CTLogin.UserID;
			}
			$.ajax({
						url : "${pageContext.request.contextPath }/userInfo/getUserInfo",
						type : "post",
						dataType : "json",
						data : JSON.stringify(user),
						success : function(data) {
							$("#selectAll")[0].checked = false;
							var datas = [];
							datas = data.userInfo;
							if (datas != null && datas.length != 0) {
								count = datas[0].count;
								pageRows = datas[0].pageRows;
							} else {
								count = 0;
								pageRows = 5;
							}
							page(count, pageRows, 'getUserInfo', user,'pageContainer');
							initUserList(data);
						},
						contentType : 'application/json'
					});
		}
		function page(count, pageRows, url, user,pageID) {
				$("#"+pageID).pagination(
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
        								$("#selectAll")[0].checked = false;
        								initUserList(data);
        							},
        							contentType : 'application/json'
        						});
                            } //回调函数
                        });
				if(count==0){
					$(".no_data").html("<img src='${pageContext.request.contextPath }/images/no_data.png'>");
					$("#"+pageID).html("");
				}else{
					$(".no_data").html("");
				}
		}
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
	</script>
	<!-- 逻辑 -->
	<!--指定AE结束-->
	<script type="text/javascript">
		//单用户指定AE
		function singloZhiAE(userID) {
			$(".aeTr").remove();
			//显示弹窗，及请求所有AE角色用户
			showAEList(2);
			//将userID绑定
			$("#aeUserID").val(userID);
		}
		//显示弹层
		function onLayer() {
			var userArray = [];
			var array = {};
			$(".cks").each(function() {
				var user = {};
				if ($(this).prop("checked")) {
					user.userID = $(this).val();
					userArray.push(user);
				}
			});
			array.array = userArray;
			if (userArray != null && userArray.length != 0) {
				//显示弹窗，及请求所有AE角色用户
				showAEList(1);
			} else {
				alert("请选择需要指定AE的用户");
			}
		}
		//显示弹窗，及回显所有AE用户
		function showAEList(j) {
			// 显示遮挡层样式
			var showShade = {
				'position' : 'fixed',
				'left' : '0',
				'top' : '0',
				'width' : '100%',
				'height' : '100%',
				'z-index' : '9',
				'background-color' : 'rgba(0,0,0,0.5)',
				'display' : 'block'
			};
			// 显示弹出层样式
			var showPopup = {
				'position' : 'fixed',
				'left' : '50%',
				'top' : '50%',
				'margin-left' : -275 + 'px',//$('.layer').width() / 2
				'margin-top' :  -275 + 'px',//$('.layer').height() / 2
				'z-index' : '10',
				'display' : 'block'
			};
			//显示弹出层
			$('#layer' + j).css(showPopup);
			//显示遮挡层
			$('#occlusion' + j).css(showShade);
			var user = {};
			user.curPage = 1;
			$.ajax({
				url : "/userInfo/getAERole",
				type : "post",
				dataType : "json",
				data : JSON.stringify(user),
				success : function(data) {
					var datas = data;
					appendAE(datas,j);
				},
				contentType : 'application/json'
			});
		}
		//处理AE列表
		function appendAE(datas,j){
			$(".aeTr").remove();
			for ( var i in datas) {
				if (datas[i].trueName == null) {
					datas[i].trueName='';
				}
				$("#aeTable" + j)
						.append(
								"<tr class='aeTr'><td><input type='radio' id='roleID"+i+"' name='roleID' value='"+datas[i].userID+"'/></td><td id='trueName"+i+"'>"
										+ datas[i].trueName
										+ "</td></tr>");
				
			}
		}
		//指定AE
		function updateAE(userArr) {
			$.ajax({
				url : "/userInfo/updateUserAuthAE",
				type : "post",
				dataType : "json",
				data : JSON.stringify(userArr),
				success : function(data) {
					if (data.code == 1) {
						alert(data.message);
					} else {
						offLayer(1);
						offLayer(2);
						caXun();
					}
				},
				contentType : 'application/json'
			});
		}
		function zhiSingleAE() {
			var roleID = $("[name='roleID']:checked").val();
			var userArray = [];
			var userArr = {};
			var user = {};
			user.userID = $("#aeUserID").val();
			user.authAEUserID = roleID;
			userArray.push(user);
			userArr.array = userArray;
			if (roleID != undefined && roleID != null && userArray != null && userArray.length != 0) {
				(userArray.length);
				updateAE(userArr);
			} else {
				alert("请选择AE");
			}
		}
		function zhiAE() {
			var roleID = $("[name='roleID']:checked").val();
			var userArray = [];
			var userArr = {};
			$(".cks").each(function() {
				var user = {};
				if ($(this).prop("checked")) {
					user.userID = $(this).val();
					user.authAEUserID = roleID;
					userArray.push(user);
				}
			});
			userArr.array = userArray;
			if (roleID != undefined && roleID != null && userArray != null && userArray.length != 0) {
				updateAE(userArr);
			} else {
				alert("请选择指定AE");
			}
		}
		//关闭弹层
		function offLayer(i) {
			// 清除并隐藏遮挡层样式
			var hideShade = {
				'position' : '',
				'left' : '',
				'top' : '',
				'width' : '',
				'height' : '',
				'z-index' : '',
				'background-color' : '',
				'display' : 'none'
			};
			// 清除并隐藏
			var hidePopup = {
				'position' : '',
				'left' : '',
				'top' : '',
				'margin-left' : '',
				'margin-top' : '',
				'z-index' : '',
				'display' : 'none'
			};
			// 隐藏弹出层
			$('#layer' + i).css(hidePopup);
			// 隐藏遮挡层
			$('#occlusion' + i).css(hideShade);
		}
	</script>
</body>
</html>