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
<title>个人信息-账号管理-我的赤兔</title>
	<link rel="icon" href="${pageContext.request.contextPath }/images/favicon.ico" type="image/x-icon">
	<script type="text/javascript" src="/js/jquery.1.11.3.min.js"></script>
    <script type="text/javascript" src="/js/jquery.form.js"></script>
    <script type="text/javascript" src="/js/Common.js"></script>
    <script type="text/javascript" src="/js/Area2.js"></script>
    <script type="text/javascript">
        $(function() {
            BindProvince('ddlProvince'); //绑定省份
            //1-超级管理员、2-广告主、3-媒体主、4-运营、5-AE、6-策划
            var role = '${ userExt.role }';
            var type = true;
            //暂时表示 true 修改 false 新增
            var isNew = '${ userExt.isNew }'  == 'false';
            if(isNew){
                var typeVal = '${ userExt.type }';
                //判断类型
                if (typeVal == '1001') {
                    type = true;
                    // $("#type_1").prop("checked", true);
                } else if (typeVal == '1002') {
                    type = false;
                    // $("#type_2").prop("checked", true);
                }
                var ddlProvince = '${ userExt.provinceID }';
                if(ddlProvince != ''){
                    $("#ddlProvince").val(ddlProvince);
                }
                crmCustCheckHelper.triggerProvince();
                var ddlCity = '${ userExt.cityID }';
                if(ddlCity != ''){
                    $("#ddlCity").val(ddlCity);
                }
                crmCustCheckHelper.triggerCity();
                var ddlCounty = '${ userExt.counntyID }';
                if(ddlCounty != ''){
                    $("#ddlCounty").val(ddlCounty);
                }
            }else{
                crmCustCheckHelper.triggerProvince();
                crmCustCheckHelper.triggerCity();
            }
            var domainName = '${ userExt.domainName }';
            if(type){ 
                var bLicenceURL = '${ userExt.bLicenceURL }';
                if(bLicenceURL != ''){
                    $("#bLicenceIMG").prop('src', domainName + bLicenceURL);
                    $("#bLicenceURL").val(bLicenceURL);
                    //调整大小
                    modifyImgSize("bLicence");
                    //清除未上传提示
                    // $("#bLicenceTEXT").text('');
                }
                var organizationURL = '${ userExt.organizationURL }';
                if(organizationURL != ''){
                    //如果上传了组织机构代码证就显示
                    $("#c_organization").show();
                    $("#organizationIMG").prop('src', domainName + organizationURL);
                    $("#organizationURL").val(organizationURL);
                    //调整大小
                    modifyImgSize("organization");
                    //清除未上传提示
                    // $("#organizationTEXT").text('');
                }
                // var idCardBackURL = '${ userExt.idCardBackURL }';
                // if(idCardBackURL != ''){
                //     $("#idCardBackIMG").prop('src', idCardBackURL);
                //     $("#idCardBackURL").val(idCardBackURL);
                //     //调整大小
                //     modifyImgSize("idCardBack");
                //     //清除未上传提示
                //     // $("#idCardBackTEXT").text('');
                // }
            }
            var idCardFrontURL = '${ userExt.idCardFrontURL }';
            if(idCardFrontURL != ''){
                $("#idCardFrontIMG").prop('src', domainName + idCardFrontURL);
                $("#idCardFrontURL").val(idCardFrontURL);
                //调整大小
                modifyImgSize("idCardFront");
                //清除未上传提示
                // $("#idCardFrontTEXT").text('');
            }
            //切换页面
            pageControl(type,false);
            // //绑定提交
            $("#btn").click(function() {
                var data = {};
                data.userID = $("#userID").val();
                data.trueName = $("#trueName").val();
                var roleDetail = "${ userExt.roleDetail.SYS001BUT300104 }" == "true" && "${ userExt.isLoginDetail }" == "false";
                if ($("#type_1").prop("checked")) {
                    data.type = 1001;
                    if(data.trueName == ''){
                        alert("请填写公司名称");
                        return;
                    }
                    data.contact = $("#contact").val();
                    if(data.contact == ''){
                        alert("请填写联系人");
                        return;
                    }
                    if(roleDetail){
                        data.mobile = $("#mobile").val();
                        if(data.mobile == ''){
                            alert("请填写联系电话");
                            return;
                        }
                        if(data.mobile.length != 11){
                            alert("联系电话格式不正确");
                            return;
                        }
                    }
                    // if(!isNew){
                    //    data.userName = $("#loginName").val();
                    //     if(data.userName == ''){
                    //         alert("请填写用户名");
                    //         return;
                    //     }
                    // } 
                    if(roleDetail){
                        data.pwd = $("#pwd").val();
                        if(data.pwd == ''){
                            alert("请填写密码");
                            return;
                        }
                    }
                    
                    data.bLicenceURL = $("#bLicenceURL").val();
                    if(data.bLicenceURL == ''){
                        alert("请上传营业执照");
                        return;
                    }
                    data.idCardFrontURL = $("#idCardFrontURL").val();
                    if(data.idCardFrontURL == ''){
                        alert("请上传法人身份证");
                        return;
                    }
                    // data.idCardBackURL = $("#idCardBackURL").val();
                    // if(data.idCardBackURL == ''){
                    //     alert("请上传法人身份证(反面)");
                    //     return;
                    // }
                } else if ($("#type_2").prop("checked")) {
                    data.type = 1002;
                    if(data.trueName == ''){
                        alert("请填写真实姓名");
                        return;
                    }
                    if(roleDetail){
                        data.mobile = $("#mobile").val();
                        if(data.mobile == ''){
                            alert("请填写联系电话");
                            return;
                        }
                    }
                    // if(!isNew){
                    //    data.userName = $("#loginName").val();
                    //     if(data.userName == ''){
                    //         alert("请填写用户名");
                    //         return;
                    //     }
                    // } 
                    if(roleDetail){
                        data.pwd = $("#pwd").val();
                        if(data.pwd == ''){
                            alert("请填写密码");
                            return;
                        }
                    }
                    
                    data.idCardFrontURL = $("#idCardFrontURL").val();
                    if(data.idCardFrontURL == ''){
                        alert("请上传手持身份证");
                        return;
                    }
                } else {
                    data.type = '';
                    alert("请选择类别！");
                    return;
                }
                data.isAuthMTZ = $("#isAuthMTZ").prop("checked");
                data.businessID = $("#businessID").val();
                data.provinceID = $("#ddlProvince").val();
                data.cityID = $("#ddlCity").val();
                data.counntyID = $("#ddlCounty").val();
                data.category = '${ userExt.category }';
                data.address = $("#address").val();
                data.organizationURL = $("#organizationURL").val();
                data.isLoginDetail = "${ userExt.isLoginDetail }" == "true";
                data.bLicenceFileSize = $("#bLicenceFileSize").val();
                data.organizationFileSize = $("#organizationFileSize").val();
                data.idCardFrontFileSize = $("#idCardFrontFileSize").val();

                $.ajax({
                    type : "POST",
                    url : "/userInfo/user/setInfoAndDetailSenior",
                    contentType : 'application/json',
                    data : JSON.stringify(data),
                    success : function(data) {
                        alert(data.message);
                        var category = '${ userExt.category }';
                        if("${ userExt.isLoginDetail }" == "false"){
                            if(role == 'SYS001RL00004' && category == '29001'){
                                window.location.href="/userInfo/toUserOperateAdd";
                            }else if(role == 'SYS001RL00004' && category == '29002'){
                                window.location.href="/userInfo/toUserOperateMedia";
                            }else if(role == 'SYS001RL00005'){
                                window.location.href="/userInfo/toUserOperateAdd";
                            }
                        }
                        $("#type_1").prop("disabled", true);
                        $("#type_2").prop("disabled", true);
                    }
                });
            })
        });

        //页面切换
        function pageControl(flag, f){
            if(flag){
                $("#c_trueName").html("<span class='red f12'>*</span> 公司名称：");
                $("#trueName").attr("maxlength",25);
                if(f){
                    $("#contact").val($("#trueName").val());
                    $("#trueName").val('');
                }
                $("#bLicenceFORM").show();
                if($("#organizationURL").val() != ''){
                    $("#c_organization").show();
                } 
                $("#organizationFORM").show();
                $("#c_idCardFront").html("<span class='red f12'>*</span> 法人身份证(正反)：");
                // $("#idCardBackFORM").show();
                $("#c_contact").show();
            }else{
                $("#c_trueName").html("<span class='red f12'>*</span> 真实姓名：");
                $("#trueName").attr("maxlength",5);
                if(f){
                    $("#trueName").val($("#contact").val());
                    $("#contact").val('');
                }
                $("#bLicenceFORM").hide();
                $("#c_organization").hide();
                $("#organizationFORM").hide();
                $("#c_idCardFront").html("<span class='red f12'>*</span> 手持身份证：");
                // $("#idCardBackFORM").hide();
                $("#c_contact").hide();
            }
        }

        //打开上传
        function openUpload(option){ 
            $("#" + option + "FILE").click(); 
        } 

        //上传图片
        function uploadPic(option) {
            // var filePath = $("#" + option + "FILE").val();
            // var lastLen = filePath.lastIndexOf('\\');
            // var fileName = filePath.substr(lastLen+1);
            // $("#" + option + "TEXT").text(fileName);

            var parameter = {
                url : '/uploadImg',
                contentType : 'multipart/form-data',
                dataType : 'json',
                type : 'post',
                success : function(data) {
                    if(data.status == 1){
                        var domainName = data.result.domainName;
                        var finaPath = data.result.finaPath;
                        var fileSize = data.result.fileSize;
                        //调整大小
                        modifyImgSize(option);
                        $("#" + option + "IMG").prop('src', domainName + finaPath);
                        $("#" + option + "URL").val(finaPath);
                        $("#" + option + "FileSize").val(fileSize);
                        //如果上传了组织机构代码证就显示
                        if(option == 'organization'){
                            $("#c_organization").show();
                        }
                    }else{
                        alert(data.message);
                    }
                }
            }
            $("#" + option + "FORM").ajaxSubmit(parameter);
        }
        //调整返回图片大小
        function modifyImgSize(option){
            $("#" + option + "IMG").css('width', "80px");
            $("#" + option + "IMG").css('height', "80px");
        }

        //省市区
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

        //显示弹层
        function onLayer(img){
            $("#big").html("<img id='bigImg' onload='if (this.width>400 || this.height>300) if (this.width/this.height>400/300) this.width=400; else this.height=300;'>");
            $("#bigImg").attr("src",$(img).attr("src"));
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
                'margin-top': -300 / 2 + 'px',
                'z-index': '10',
                'display': 'block'
            };
            //显示弹出层
            $('.layer').css(showPopup);
            //显示遮挡层
            $('#occlusion').css(showShade);
            
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
            // 清除内容
            $("#big").attr("src","");
            $("#big").html();
        }
        //自动填充用户名
        // function fillingAutomatic (){
        //     var name = $("#loginName").val();
        //     if(name == ''){
        //         $("#loginName").val($("#mobile").val());
        //     }
        // }
            
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
        <input id="userID" type="hidden" value="${ userExt.userID }">
            <ul>
                <li class="ins_a">类型：</li>
                <li><input id="type_1" name="type" type="radio" checked <c:if test="${userExt.type == 1001 || userExt.type == 1002}">disabled</c:if> onclick="pageControl(true,true)" />企业</li>
                <li><input id="type_2" name="type" type="radio" <c:if test="${ !userExt.isNew && userExt.type == 1002 }">checked</c:if> <c:if test="${userExt.type == 1001 || userExt.type == 1002}">disabled</c:if> onclick="pageControl(false,true)" />个人</li>
                <div class="clear"></div>
            </ul>
            <c:if test="${ userExt.roleDetail.SYS001BUT300102 && userExt.category == '29001' && !userExt.isLoginDetail}">
                <ul>
                    <li class="ins_a">&nbsp;</li>
                    <li><input id="isAuthMTZ" type="checkbox" <c:if test="${ userExt.isAuthMTZ }">checked</c:if> /> 允许广告主授权给我</li>
                    <div class="clear"></div>
                </ul>
            </c:if>
            <ul>
                <li id="c_trueName" class="ins_a"><span class="red f12">*</span> 公司名称：</li>
                <li><input id="trueName" type="text" value="${ userExt.trueName }" style="width:315px;"></li>
                <div class="clear"></div>
            </ul>
            <c:if test="${ userExt.roleDetail.SYS001BUT300103 && userExt.category == '29001' }">
                <ul> 
                    <li class="ins_a">所在行业：</li>
                    <li>
                        <select id="businessID" style="width:220px;line-height: 30px">
                           <option selected value="-1">请选择</option>
                            <c:forEach items="${ userExt.dictType_2 }" var="item">
                                <option value="${ item.key }" <c:if test="${ item.key == userExt.businessID }">selected</c:if> >${ item.value }</option>
                            </c:forEach>
                        </select>
                    </li>
                    <div class="clear"></div>
                </ul>
            </c:if>
            <ul>
                <li class="ins_a">所在地区：</li>
                <li>
                    <select id="ddlProvince" name="ddlProvince" style="width:100px;line-height: 30px" onchange="crmCustCheckHelper.triggerProvince();">
                    </select>
                </li>
                <li>
                    <select id="ddlCity" name="ddlCity" style="width:100px;line-height: 30px" onchange="crmCustCheckHelper.triggerCity();">
                    </select>
                </li>
                <li>
                    <select id="ddlCounty" name="ddlCounty" style="width:100px;line-height: 30px">
                    </select>
                </li>
                <div class="clear"></div>
            </ul>
            <ul id="c_contact" style="display:none">
                <li class="ins_a"><span class="red f12">*</span> 联系人：</li>
                <li><input id="contact" type="text" value="${ userExt.contact }" maxlength="5" style="width:315px;"></li>
                <div class="clear"></div>
            </ul>
            <c:if test="${ userExt.roleDetail.SYS001BUT300104 && !userExt.isLoginDetail}">
                <ul>
                    <li class="ins_a"><span class="red f12">*</span> 联系电话：</li>
                    <li><input id="mobile" type="text" maxlength="11" onkeyup="value=value.replace(/[^\d]/g,'')" value="${ userExt.mobile }" style="width:315px;"></li>
                    <li class="red">(手机号可用于登录和接收信息)</li>
                    <div class="clear"></div>
                </ul>
            </c:if>  
            <!-- <c:if test="${ userExt.isNew }">
                <ul>
                    <li class="ins_a"><span class="red f12">*</span> 登录名：</li>
                    <li><input id="loginName" type="text" maxlength="20" style="width:315px;"></li>
                    <div class="clear"></div>
                </ul>
            </c:if> -->
            <c:if test="${ userExt.roleDetail.SYS001BUT300104 && !userExt.isLoginDetail }">
                <ul>
                    <li class="ins_a"><span class="red f12">*</span> 密码：</li>
                    <li><input id="pwd" type="password" maxlength="20" value="${ userExt.pwd }" style="width:315px;"></li>
                    <div class="clear"></div>
                </ul>
            </c:if>  
            <ul>
                <li class="ins_a">联系地址：</li>
                <li><input id="address" type="text" value="${ userExt.address }" maxlength="50" style="width:500px;"></li>
                <div class="clear"></div>
            </ul>
            <form id="bLicenceFORM">
                <ul>
                    <li class="ins_a"><span class="red f12">*</span> 营业执照：</li>
                    <!-- <li><a class="but_upload" onclick="openUpload('bLicence')" style="width:200px">+ 上传</a></li> -->
                    <input id="bLicenceFILE" type="file" name="pic" onchange="uploadPic('bLicence')" style="visibility:hidden">
                    <input id="bLicenceURL" type="hidden">
                    <input id="bLicenceFileSize" type="hidden">
                    <div class="clear"></div>
                </ul>
                <ul>
                    <li class="ins_a">&nbsp;</li>
                    <li class="frame"><img id="bLicenceIMG" src="http://www.chitunion.com/ImagesNew/uploadimg.png" width="80" height="80" onclick="onLayer(this)"/><span class="set"><img src="/images/icon20.png" onclick="openUpload('bLicence')" /></span></li>
                    <div class="clear"></div>
                </ul>
            </form>
            <form id="organizationFORM">
                <ul>
                    <li class="ins_a">&nbsp;</li>
                    <li>如未三证合一，<a class="blue3" onclick="openUpload('organization')">请上传组织机构代码证</a></li>
                    <input id="organizationFILE" type="file" name="pic" onchange="uploadPic('organization')" style="visibility:hidden">
                    <input id="organizationURL" type="hidden">
                    <input id="organizationFileSize" type="hidden">
                    <div class="clear"></div>
                </ul>
            </form>
            <ul id="c_organization" style="display:none">
                <li class="ins_a">&nbsp;</li>
                <li class="frame"><img id="organizationIMG" src="" width="80" height="80" onclick="onLayer(this)"/><span class="set"><img src="/images/icon20.png" onclick="openUpload('organization')" /></span></li>
                <div class="clear"></div>
            </ul>
            <form id="idCardFrontFORM">
                <ul>
                    <li id="c_idCardFront" class="ins_a"><span class="red f12">*</span> 法人身份证(正反)：</li>
                    <!-- <li><a class="but_upload" onclick="openUpload('idCardFront')" style="width:200px">+ 上传</a></li> -->
                    <input id="idCardFrontFILE" type="file" name="pic" onchange="uploadPic('idCardFront')" style="visibility:hidden">
                    <input id="idCardFrontURL" type="hidden">
                    <input id="idCardFrontFileSize" type="hidden">
                    <div class="clear"></div>
                </ul>
                <ul>
                    <li class="ins_a">&nbsp;</li>
                    <li class="frame"><img id="idCardFrontIMG" src="http://www.chitunion.com/ImagesNew/uploadimg.png" width="80" height="80" onclick="onLayer(this)" /><span class="set"><img src="/images/icon20.png" onclick="openUpload('idCardFront')" /></span></li>
                    <div class="clear"></div>
                </ul>
            </form>
            <!--<form id="idCardBackFORM">
                <ul>
                    <li class="ins_a"><span class="red f12">*</span> 法人身份证(反面)：</li>
                    <li><a class="but_upload" onclick="openUpload('idCardBack')" style="width:200px">+ 上传</a></li>
                    <input id="idCardBackFILE" type="file" name="pic" onchange="uploadPic('idCardBack')" style="display:none">
                    <input id="idCardBackURL" type="hidden">
                    <div class="clear"></div>
                </ul>
                <ul>
                    <li class="ins_a">&nbsp;</li>
                    <li class="frame"><img id="idCardBackIMG" src="/images/default.png" width="80" height="80"/></li>
                    <div class="clear"></div>
                </ul>
            </form>-->
            <c:if test="${ userExt.roleDetail.SYS001BUT300101 }">
                <ul>
                    <li class="ins_a">&nbsp;</li>
                    <li><a id="btn" class="button">保存</a></li>
                    <div class="clear"></div>
                </ul>
            </c:if>


            <div class="clear"></div>
        </div>
    </div>
    <div class="clear"></div>
</div>

</div>
<!--遮挡层开始-->
<div id="occlusion"></div>
<!--遮挡层结束-->
<!--大图开始-->
<div class="layer" style="display: none;">
    <div class="fr"><img src="/images/icon13.png" width="16" height="16"  onMouseOver="this.src='/images/icon14.png'" onMouseOut="this.src='/images/icon13.png'" onclick="offLayer()">&nbsp;</div>
    <div id="big" class="layer_con">
        
    </div>
</div>
<!--大图结束-->

<!--底部-->
<jsp:include page="footer.jsp"></jsp:include>

</body>
</html>
