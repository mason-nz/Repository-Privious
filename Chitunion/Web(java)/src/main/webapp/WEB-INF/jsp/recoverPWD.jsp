<%@ page language="java" contentType="text/html; charset=UTF-8"
	pageEncoding="UTF-8"%>
<!DOCTYPE html PUBLIC "-//W3C//DTD HTML 4.01 Transitional//EN" "http://www.w3.org/TR/html4/loose.dtd">
<html>
<head>
<meta charset="utf-8">
<meta name="description" content="描述文字，字数200内？">
<meta name="keywords" content="网站相关关键字，字数200内？">
<title>赤兔联盟平台</title>
<link rel="icon" href="${pageContext.request.contextPath }/images/favicon.ico" type="image/x-icon">
<link rel="stylesheet" type="text/css"
	href="${pageContext.request.contextPath }/css/reset.css" />
<link rel="stylesheet" type="text/css"
	href="${pageContext.request.contextPath }/css/layout.css" />
<script
	src="${pageContext.request.contextPath }/js/jquery.1.11.3.min.js"></script>
<script type="text/javascript"
	src="${pageContext.request.contextPath }/js/tab.js"></script>
	<script type="text/javascript" src="${pageContext.request.contextPath }/js/userInfoJS/recoverPWD.js"></script>
</head>

<body>
	<!--顶部logo-->
	<div class="topBar">
	    <div class="topBox">
	        <a href="http://www.chitunion.com/index.html" class="topLogo"></a>
	        <div class="fl ml20 come">找回密码</div>
	        <div class="fr come">没有账号？ <a href="http://j.chitunion.com/userInfo/toRegister" class="yellow">请注册</a></div>
	        <div class="clear"></div>
	        <div class="clear"></div>
	    </div>
	</div>
	<!--中间内容-->
	<!--第一步-->
	<div class="getback_mh" id="nextStep01" >
	        <div class="getback">
	            <img src="${pageContext.request.contextPath }/images/login_01.png"/>
	            <ul>
	                <li class="yellow">验证身份</li>
	                <li class="tx">设置新密码</li>
	                <li>设置成功</li>
	                <div class="clear"></div>
	            </ul>
	        </div>
	        <div class="getback_box">
	            <div class="f16">
	              	  用户类型：
	                <span><input onclick="addvice()" name="roleId" type="radio" id="add" value="29001" checked="checked" /> 广告主</span>
	                <span class="ml10"><input onclick="media()" name="roleId" type="radio" value="29002" /> 媒体主</span>
	            </div>
	                <ul>
	                    <li class="hrb">手机号：</li>
	                    <li>
		                    <input id="mobile"  onchange="mobileChange()" 
		                    maxlength="11" onkeyup="value=value.replace(/[^\d]/g,'')" 
		                    onblur="checkMobileReg()" name="mobile" type="text"  
		                    style="width:325px;" placeholder="输入手机号" autocomplete="off">
	                   </li>
	                    <div class="clear"></div>
	                </ul>
	                <div class="not">
	                		<img style="display: none;" id="img01" src="${pageContext.request.contextPath }/images/icon21.png"/> 
	                		<span id="phoneMessage" class="red"></span>
	                </div>
	                <ul>
	                    <li class="hrb">验证码：</li>
	                    <li><input id="validateCode" name="validateCode" onchange="mobileChange()" onblur="checkImg(this.value)" type="text"  style="width:98px;" placeholder="输入验证码" autocomplete="off"></li>
	                    <li>
	                    	<span>
	                    		<img id="codeValidateImg" onclick="changeImage()">
	                    	</span>
	                    </li>
	                    <li>看不清？ <a onclick="changeImage()" class="blue">换张图</a></li>
	                    <div class="clear"></div>
	                </ul>
	                <div class="not">
	                		<span id="message" class="red"></span>
	                </div>
	                <ul>
	                    <li class="hrb">短信验证码：</li>
	                    <li><input id="mobileCode" name="mobileCode" onblur="checkPhone()" maxlength="6" type="text"  style="width:98px;" placeholder="输入手机验证码" autocomplete="off"></li>
	                    <li><input id="getImgCode" type="button" onclick="getMessageCode(this)" class="obtain_no"  value="获取验证码" /></li>
	                    <input id="getImgMesssage" type="button"class="obtain" style="display: none;" />
	                    <div class="clear"></div>
	                </ul>
	                <div class="not">
	                		<span id="mesMessage" class="red"></span>
	                </div>
	                <ul style="margin-bottom:25px">
	                    <li class="hrb">&nbsp;</li>
	                    <li><a class="button" id="tijiao" href="javascript:void(0);" onclick="javascript:nextStep();" style="width: 178px;">下一步</a></li>
	                    <div class="clear"></div>
	                </ul>
	            <div class="clear"></div>
	        </div>
	</div>
	<!--第二步-->
	<div class="getback_mh" id="nextStep02" style="display: none;">
	        <div class="getback">
	            <img src="${pageContext.request.contextPath }/images/login_02.png"/>
	            <ul>
	                <li>验证身份</li>
	                <li class="tx yellow">设置新密码</li>
	                <li>设置成功</li>
	                <input type="hidden" id="userID">
	                <input type="hidden" id="mobiles">
	                <div class="clear"></div>
	            </ul>
	        </div>
	        <div class="getback_box">
	                <ul>
	                    <li class="hrb">密码：</li>
	                    <li><input id="pwd" name="pwd" minlength="6" maxlength="20" onblur="checkPwd()" type="password"  style="width:325px;" placeholder="请输入密码（6-20位数字，字母）" autocomplete="off"></li>
	                    <div class="clear"></div>
	                </ul>
	                <div class="not">
	                		<span id="pwdMessage" class="red"></span>
	                </div>
	                <ul>
	                    <li class="hrb">确认密码：</li>
	                    <li><input id="pwdTwo" minlength="6" maxlength="20" name="pwdTwo" type="password" onblur="checkPwdTwo()"  style="width:325px;" placeholder="请再次输入密码" autocomplete="off"></li>
	                    <div class="clear"></div>
	                </ul>
	                <div class="not">
	                		<span id="pwdTwoMessage" class="red"></span>
	                </div>
	                <ul style="margin-bottom:25px">
	                    <li class="hrb">&nbsp;</li>
	                    <li><a onclick="tijiao()" href="javascript:void(0);" class="button" style="width: 178px;">提交</a></li>
	                    <div class="clear"></div>
	                </ul>
	            <div class="clear"></div>
	        </div>
	</div>
	<!--第三步-->
	<div class="getback_mh" id="nextStep03"  style="display: none;">
	        <div class="getback">
	            <img src="${pageContext.request.contextPath }/images/login_03.png"/>
	            <ul>
	                <li>验证身份</li>
	                <li class="tx">设置新密码</li>
	                <li class="yellow">设置成功</li>
	                <div class="clear"></div>
	            </ul>
	        </div>
	        <div class="getback_box">
	            <div class="success">
	                <div><img src="${pageContext.request.contextPath }/images/point.png"/> <span class="f18 ml10">密码设置成功！</span></div>
	                <div class="mt20"><a href="http://www.chitunion.com/Login.aspx" class="button" style="width:250px;">马上登录</a></div>
	            </div>
	        </div>
	</div>
	<div class="line2">&nbsp;</div>			
	<!--底部-->
	<div id="footer2">
	    <div class="main">
<!-- 	        <ul> -->
<!-- 	            <li><a href="#" target="_blank">关于我们</a></li> -->
<!-- 	            <li>|</li> -->
<!-- 	            <li><a href="#" target="_blank">联系我们</a></li> -->
<!-- 	            <li>|</li> -->
<!-- 	            <li><a href="#" target="_blank">案例中心</a></li> -->
<!-- 	            <div class="clear"></div> -->
<!-- 	        </ul> -->
	        <p>@2016-2017 www.chitunion.com All Rights Reserved. 北京行圆汽车信息技术有限公司所有 京ICP备17011360号-3</p>
	    </div>
	</div>
</body>
</html>
