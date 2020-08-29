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
<title>个人信息-账号管理-我的赤兔
</title>
	<link rel="icon" href="${pageContext.request.contextPath }/images/favicon.ico" type="image/x-icon">
    <link rel="stylesheet" type="text/css" href="/css/reset.css"/>
    <link rel="stylesheet" type="text/css" href="/css/layout.css"/>
    <script type="text/javascript" src="/js/jquery.1.11.3.min.js"></script>
    <script type="text/javascript" src="/js/vmc.slider.full.min.js"></script>
    <script type="text/javascript" src="/js/tab.js"></script>
    <script type="text/javascript">
        $(function() {
            var role = '${ userExt.role }';
            var SYS001BUT300203 = '${ userExt.roleDetail.SYS001BUT300203 }' == 'true';
            var userID = '${ userExt.userID }';
            var isAuthMTZ = '${ userExt.isAuthMTZ }';
            $("#userID").val(userID);
            if(SYS001BUT300203 && isAuthMTZ){
                var isAuthAE = '${ userExt.isAuthAE }';
                $("#isAuthAE").prop("checked",isAuthAE == 'true');
            }
            var mobile = '${ userExt.mobile }';
            $("#mobile").val(mobile);
            //绑定提交
            $("#btnSub").click(function() {
                var data = {};
                data.userID = $("#userID").val();
                var roleIsAuthMTZ = SYS001BUT300203 && isAuthMTZ;
                if(roleIsAuthMTZ){
                    data.isAuthAE = $("#isAuthAE").prop("checked");
                }
                data.isAuthMTZ = '${ userExt.isAuthMTZ }';
                data.pwd = $("#pwd").val();
                data.newPwd = $("#newPwd").val();
                reNewPwd = $("#reNewPwd").val();

                var authorization = data.pwd == '' && data.newPwd == '' && reNewPwd == '' && roleIsAuthMTZ == 'true';
                if(data.pwd == '' && !authorization){
                    $("#checkPwd").show();
                    return;
                }
                
                if(data.newPwd == '' && !authorization){
                    $("#checkNewPwd").show();
                    return;
                }
                
                if(reNewPwd == '' && !authorization){
                    $("#checkReNewPwd").show();
                    return;
                }
                if(data.newPwd == reNewPwd){
                    $.ajax({
                        type : "POST",
                        url : "/userInfo/user/setInfoPassAndAEName",
                        contentType : 'application/json',
                        data : JSON.stringify(data),
                        success : function(data) {
                            if(data.status == 1)
                                alert("保存成功");
                            else{
                                alert(data.message);
                            }
                        }
                    });
                }else{
                    alert("两次密码输入不一致");
                    return;
                }
            });
        });
        //获取验证码
        var countdown=60;
        function getCode(obj){
            if(countdown == 60){
                var newMobile = $("#newMobile").val();
                if(newMobile == ''){
                    alert("请输入手机号");
                    return;
                }
                $.get("getMessage/" + newMobile,function(data){
                    alert(data.message);
                    if(data.status == 1){
                        $("#hidMobile").val($("#newMobile").val());
                    }else{
                        countdown = 0;
                    }
                });
            }
            if (countdown == 0) { 
                $(obj).prop("disabled",false);    
                $(obj).val("获取验证码");
                countdown = 60; 
                return;
            } else {
                $(obj).prop("disabled", true); 
                $(obj).val("重新获取" +countdown + "秒");
                countdown--; 
            } 
            setTimeout(function() { 
                getCode(obj) 
            },1000)      
        }
        //提交验证
        function subCode(){
            var newMobile = $("#newMobile").val();
            if(newMobile == ''){
                alert("请输入手机号");
                return;
            }
            var code = $("#code").val();
            if(code == ''){
                alert("请输入验证码");
                return;
            }
            var hidMobile = $("#hidMobile").val();
            if(hidMobile != newMobile){
                alert("请获取验证码");
                return;
            }
            $.ajax({
                type : "POST",
                url : "/userInfo/user/setMobile",
                contentType : 'application/json',
                data : code,
                success : function(data) {
                    if(data.status == 1){
                    alert("修改成功");
                    window.location.href="getInfoPassAndAEName";
                    }else{
                        alert(data.message);
                    }
                }
            });
            // $.get("setMobile/" + code,function(data){
            //     if(data.status == 1){
            //         alert("修改成功");
            //         window.location.href="getInfoPassAndAEName";
            //     }else{
            //         alert(data.message);
            //     }
            // });
        }

        function clearCheck(item){
            $("#check" + item).hide();
        }
        
        //显示弹层
        function onLayer(){
            // 显示遮挡层样式
            var showShade = {
                'position': 'fixed',
                'left': '0',
                'top': '0',
                'width': '100%',
                'height': '100%',
                'z-index': '9',
                'background-color': 'rgba(0,0,0,0.5)',
                'display': 'block'
            };
            // 显示弹出层样式
            var showPopup = {
                'position': 'fixed',
                'left': '50%',
                'top': '50%',
                'margin-left': -$('.layer').width() / 2 + 'px',
                'margin-top': -$('.layer').height() / 2 + 'px',
                'z-index': '10',
                'display': 'block'
            };
            //显示弹出层
            $('.layer').css(showPopup);
            //显示遮挡层
            $('#occlusion').css(showShade);
            //清除数据
            $("#newMobile").val('');
            $("#code").val('');
        }
        //关闭弹层
        function offLayer(){
            // 清除并隐藏遮挡层样式
            var hideShade = {
                'position': '',
                'left': '',
                'top': '',
                'width': '',
                'height': '',
                'z-index': '',
                'background-color': '',
                'display': 'none'
            };
            
            // 清除并隐藏
            var hidePopup = {
                'position': '',
                'left': '',
                'top': '',
                'margin-left': '',
                'margin-top': '',
                'z-index': '',
                'display': 'none'
            };
            // 隐藏弹出层
            $('.layer').css(hidePopup);
            // 隐藏遮挡层
            $('#occlusion').css(hideShade);
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
        <input id="userID" type="hidden">
        <c:if test="${ userExt.roleDetail.SYS001BUT300203 && userExt.isAuthMTZ && userExt.authAETrueName != null}">
            <h2>授权设置</h2>
            <div><input id="isAuthAE" type="checkbox" />授权给赤兔联盟客服   ${ userExt.authAETrueName } ，允许 ${ userExt.authAETrueName } 登录我的账号进行操作</div>
        </c:if>
            <h2 class="mt20">联系方式</h2>
            <ul>
                <li class="ins_a">绑定手机：</li>
                <li><input id="mobile" type="text" disabled style="width:315px;"></li>
                <c:if test="${ userExt.roleDetail.SYS001BUT300201 }">
                    <li><a href="#" class="blue3" onclick="onLayer()">修改</a></li><li class="red">(换绑之后即可使用新手机号登录和接收提示信息)</li>
                </c:if>
                <div class="clear"></div>
            </ul>
            <div class="clear"></div>
                <h2 class="mt20">修改密码</h2>
            <ul>
                <li class="ins_a">旧密码：</li>
                <li><input id="pwd" type="password" maxlength="20" onchange="clearCheck('Pwd')" style="width:315px;"></li>
                <li id="checkPwd" class="red" style="display: none;"><img src="/images/icon21.png"> 旧密码不能为空</span></li>
                <div class="clear"></div>
            </ul>
            <ul>
                <li class="ins_a">新密码：</li>
                <li><input id="newPwd" type="password" maxlength="20" onchange="clearCheck('NewPwd')" style="width:315px;"></li>
                <li id="checkNewPwd" class="red" style="display: none;"><img src="/images/icon21.png"> 密码不能为空</span></li>
                <div class="clear"></div>
            </ul>
            <ul>
                <li class="ins_a">确认密码：</li>
                <li><input id="reNewPwd" type="password" maxlength="20" onchange="clearCheck('ReNewPwd')" style="width:315px;"></li>
                <li id="checkReNewPwd" class="red" style="display: none;"><img src="/images/icon21.png"> 新密码不能为空</span></li>
                <div class="clear"></div>
            </ul>
            <ul style="margin-top:0px;">
                <li class="ins_a">&nbsp;</li>
                <c:if test="${ userExt.roleDetail.SYS001BUT300202 }">
                    <li><a id="btnSub" class="button" onMouseOut="clearCheck()">保存</a></li>
                </c:if>
                <div class="clear"></div>
            </ul>

            <div class="clear"></div>
        </div>
    </div>
    <div class="clear"></div>
</div>

</div>

<!--遮挡层开始-->
<div id="occlusion"></div>
<!--遮挡层结束-->
<!--验证新手机开始-->
<div class="layer" style="display: none;">
    <div class="title">
        <div class="fl">请验证新手机</div>
        <div class="fr"><a href="javascript:void(0)" id="closebt"><img src="/images/icon13.png" width="16" height="16"  onMouseOver="this.src='/images/icon14.png'" onMouseOut="this.src='/images/icon13.png'" onclick="offLayer()"></a></div>
        <div class="clear"></div>
    </div>
    <div class="layer_con">
        <div class="laycont">
            <ul>
                <li><input id="newMobile" type="text" maxlength="11" onkeyup="value=value.replace(/[^\d]/g,'')" style="width:230px;" placeholder="请输入手机号">
                <input id="hidMobile" type="hidden">
                </li>
                <li><input type="button" value="获取验证码" onclick="getCode(this)" class="obtain"></li>
                <div class="clear"></div>
            </ul>
            <ul>
                <li><input id="code" type="text"  style="width:330px;" placeholder="请输入验证码"></li>
                <div class="clear"></div>
            </ul>
        </div>
        <div class="keep">
            <span><a class="button" onclick="subCode()" disabled style="width:150px">确认</a></span> 
            <span><a class="but_keep" onclick="offLayer()" style="width:150px">取消</a></span>
        </div>
    </div>
</div>
<!--验证新手机结束-->

<!--底部-->
<jsp:include page="footer.jsp"></jsp:include>


</body>
</html>
