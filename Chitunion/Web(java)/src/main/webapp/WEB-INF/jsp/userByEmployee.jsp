<%@ page language="java" contentType="text/html; charset=utf-8"
	pageEncoding="utf-8"%>
<%@ taglib uri="http://java.sun.com/jsp/jstl/core" prefix="c"%>
<%@ taglib uri="http://java.sun.com/jsp/jstl/functions" prefix="fn" %>
<!doctype html>
<html>
<head>
<meta charset="utf-8">
<meta name="description" content="描述文字，字数200内？">
<meta name="keywords" content="网站相关关键字，字数200内？">
<title>添加用户- 用户管理-我的赤兔</title>
	<link rel="icon" href="${pageContext.request.contextPath }/images/favicon.ico" type="image/x-icon">
    <link rel="stylesheet" type="text/css" href="/css/reset.css"/>
    <link rel="stylesheet" type="text/css" href="/css/layout.css"/>
    <script type="text/javascript" src="/js/jquery.1.11.3.min.js"></script>
    <script type="text/javascript" src="/js/vmc.slider.full.min.js"></script>
    <script type="text/javascript" src="/js/tab.js"></script>
    <script type="text/javascript">
        $(function(){
            var role = '${ userExt.role }';
            if(role != null && role != '' ){
                $("#userRole").val(role);
            }

            var mobile = '${ userExt.mobile }';
            if(mobile != null && mobile != '' ){
                var phone = mobile.replace(/(\d{3})\d{4}(\d{4})/, '$1****$2');
                $("#phone").val(phone);
            }

        });
        function getEmployee(){
            var employeeNumber = $("#employeeNumber").val();
            if(employeeNumber != null && employeeNumber != ''){
                $.get("/userInfo/user/getEmployee/" + employeeNumber,function(data){
                    if(data.status == 1){
                        var res = data.result;
                        $("#employeeID").val(res.employeeID);
                        $("#checkNumber").val(res.employeeNumber);
                        $("#cnName").val(res.cnName);
                        $("#mobile").val(res.mobile);
                        $("#email").val(res.email);
                        var phone = res.mobile.replace(/(\d{3})\d{4}(\d{4})/, '$1****$2');
                        $("#phone").val(phone);

                        
                    }else{
                        alert(data.message);
                    }
                });
            }else{
                alert("请输入员工编号");
            }
        }

        function submitBtn(){
            var data = {};
            data.sysUserID = $("#employeeID").val();
            data.employeeNumber = $("#employeeNumber").val();
            data.trueName = $("#cnName").val();
            data.mobile = $("#mobile").val();
            data.email = $("#email").val();
            data.role = $("#userRole").val();
            data.userID = $("#userID").val();
            data.oldRoleID = $("#oldRoleID").val();
            if(data.employeeNumber == ''){
                alert("请输入员工编号");
                return;
            }

            if(data.role == '-1'){
                $("#checkRole").show();
                return;
            }
            data.isNew = $("#isNew").val();
            if(data.isNew == 'true'){
                if(data.employeeNumber != $("#checkNumber").val()){
                    alert("请点击查询");
                    return;
                }
            }
            $.ajax({
                type : "POST",
                url : "/userInfo/user/setEmployeeInfo",
                contentType : 'application/json',
                data : JSON.stringify(data),
                success : function(data) {
                	alert(data.message);
                	if(data.status == "1"){
                		window.location.href="/userInfo/toUserOperateList";
                	}
                }
            });
        }
        function ck(val){
            if($(val).val() == '-1'){
                $("#checkRole").show();
                return;
            }else{
                $("#checkRole").hide();
            }
        }
        function returnBtn(){
        	window.location.href="/userInfo/toUserOperateList";
        }
    </script>
</head>

<body>
<!--顶部logo 导航-->
<jsp:include page="header.jsp"></jsp:include>

<div class="list_main">
<!--中间内容-->
<div class="order">
    <jsp:include page="Menu.jsp"></jsp:include>
    <div class="order_r">
        <div class="install">
            <input id="employeeID" type="hidden">
            <input id="isNew" type="hidden" value="${ userExt.isNew }">
            <input id="userID" type="hidden" value="${ userExt.userID }">
            <input id="oldRoleID" type="hidden" value="${ userExt.role }">
            <ul>
                <li class="ins_a"><span class="red f12">*</span> 员工编号：</li>
                <li><input id="employeeNumber" type="text" value="${ userExt.employeeNumber }" 
                maxlength="6" <c:if test="${ userExt.isNew == false }">disabled</c:if>  
                onkeyup="value=value.replace(/[^\d]/g,'')" 
                style="width:315px;"><input id="checkNumber" type="hidden"></li>
                <c:if test="${  (userExt.roleDetail.SYS001MOD120010301 || SYS001MOD120010601) && userExt.isNew == true }">
                    <li><a class="blue3" onclick="getEmployee()">查询</a></li>
                </c:if>
                <div class="clear"></div>
            </ul>
            <ul>
                <li class="ins_a"><span class="red f12">*</span> 真实姓名：</li>
                <li><input id="cnName" type="text" value="${ userExt.trueName }" disabled style="width:315px;"></li>
                <li class="red"><!-- <img src="/images/icon21.png"> 请输入真实姓名--></li>
                <div class="clear"></div>
            </ul>
            <ul>
                <li class="ins_a"><span class="red f12">*</span> 手机号：</li>
                <li><input id="phone" type="text" disabled style="width:315px;">
                <input id="mobile" type="hidden" value="${ userExt.mobile }">
                </li>
                <li class="red"><!--<img src="/images/icon21.png"> 请输入手机号--></li>
                <div class="clear"></div>
            </ul>
            <ul>
                <li class="ins_a"><span class="red f12">*</span> 邮箱：</li>
                <li><input id="email" type="text" value="${ userExt.email }" disabled style="width:315px;"></li>
                <li class="red"><!--<img src="/images/icon21.png"> 请输入邮箱--></li>
                <div class="clear"></div>
            </ul>
            <ul>
                <li class="ins_a"><span class="red f12">*</span> 角色：</li>
                <li>
                    <select id="userRole" onchange="ck(this)" style="width:327px;line-height: 30px">
                        <option value="-1">请选择角色</option>
                        <option value="SYS001RL00001">超级管理员</option>
                        <option value="SYS001RL00004">运营</option>
                        <option value="SYS001RL00005">AE</option>
                        <option value="SYS001RL00006">策划</option>
                    </select>
                </li>
                <li id="checkRole" class="red" style="display: none;"><img src="/images/icon21.png"> 请选择角色</li>
                <div class="clear"></div>
            </ul>
            <ul>
                <li class="ins_a">&nbsp;</li>
                <c:if test="${ userExt.roleDetail.SYS001MOD120010302 || userExt.roleDetail.SYS001MOD120010602 }">
                    <li><a class="button" style="width:150px" onclick="submitBtn()">提交</a></li>
                </c:if>
                <li><a class="but_keep" style="width:150px;margin-top:15px" onclick="returnBtn()">返回</a></li>
                <div class="clear"></div>
            </ul>



            <div class="clear"></div>
        </div>
    </div>
    <div class="clear"></div>
</div>

</div>


<!--底部-->
<jsp:include page="footer.jsp"></jsp:include>



</body>
</html>
