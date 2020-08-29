		 addRoleId = 29001;
		 medieRoleId = 29002;
		 var time1;
		 var time2;
		 $("#roles").val(addRoleId);
		 document.onkeydown=function(event){
	            var e = event || window.event || arguments.callee.caller.arguments[0];
	            if(e && e.keyCode==27){ // 按 Esc 
	                //要做的事情
	              }
	            if(e && e.keyCode==113){ // 按 F2 
	                 //要做的事情
	               }            
	             if(e && e.keyCode==116){ // F5 键
		        	  clearData();
		        	  $('#getImgCode29001').attr("disabled",false);
		        	  $('#getImgCode29002').attr("disabled",false);
		         }
	      };
	    $(document).ready(function() {
    		flushValidateCode(29001);//进入页面就刷新生成验证码
    	  	$('#getImgCode29001').attr("disabled",false);
    	  	$('#getImgCode29002').attr("disabled",false);
    		clearData();
    	});
		 function clearMesMessage(roleId){
			var mesMessage=$("#mesMessage"+roleId).text();
			 if(mesMessage=='短信已发送 请注意查收'){
				 $("#mesMessage"+roleId).text("");
			 }
		 }
		 //检查获取验证码样式改变
		 function mobileChange(roleId){
				var phoneMessage=$("#phoneMessage"+roleId).text();
				var message=$("#message"+roleId).text();
				if(phoneMessage==''&&message==''&&phoneMessage!=null&&message!=null){
					$("#getImgCode"+roleId).removeClass("obtain_no").addClass("obtain");
				}else{
					$("#getImgCode"+roleId).removeClass("obtain").addClass("obtain_no");
				}
		}
		//换图
		function changeImage(roleId){
			flushValidateCode(roleId);
			var message=$("#message"+roleId).text();
			var validateCode=$("#validateCode"+roleId).val();
			if(validateCode==''||validateCode==null){
				$("#message"+roleId).text("请输入验证码");
				$("#getImgCode"+roleId).removeClass("obtain").addClass("obtain_no");
				return;
			}
			if(validateCode!=''&&message==''){
				$("#message"+roleId).text("验证码错误，请输入新的验证码");
				$("#getImgCode"+roleId).removeClass("obtain").addClass("obtain_no");
				return;
			}
		}
		function addvice() {
			var roleId = 29001;
			clearData();
			flushValidateCode(roleId);
			$("#roles").val(roleId);
		}
		function media() {
			var roleId = 29002;
			clearData();
			flushValidateCode(roleId);
			$("#roles").val(roleId);
		}
		//校验手机号是否注册
		function checkMobileReg(roleId) {
			var mobile = $("#mobile" + roleId).val();
			if (null != mobile && mobile != '') {
				var user = {};
				user.mobile = mobile;
				if(mobile.length==11){
					user.category = roleId;
					$.ajax({
								type : "post",
								url : "/userInfo/checkPhoneRegister",
								data : JSON.stringify(user),
								success : function(data) {
									if (data.code == 3) {
										$("#phoneMessage" + roleId).html("手机号已注册，请<a href='http://www.chitunion.com/'>登陆</a>");
										$("#getImgCode"+roleId).removeClass("obtain").addClass("obtain_no");
									}else if(data.code ==1){
										$("#phoneMessage" + roleId).text("");
										$("#getImgCode"+roleId).removeClass("obtain_no").addClass("obtain");
									}else {
										$("#getImgCode"+roleId).removeClass("obtain").addClass("obtain_no");
										$("#phoneMessage" + roleId).text(
												"" + data.message);
									}
								},
								dataType : "json",
								contentType : 'application/json'
							});
				}else{
					$("#getImgCode"+roleId).removeClass("obtain").addClass("obtain_no");
					$("#phoneMessage" + roleId).text("请输入正确的手机号");
				}
			} else {
				$("#getImgCode"+roleId).removeClass("obtain").addClass("obtain_no");
				$("#phoneMessage" + roleId).text("请输入手机号");
			}
		}
		//验证密码非空 正则
		function checkPwd(roleId) {
			var pwd = $("#pwd" + roleId).val();
			if (pwd == '' || pwd == null) {
				$("#pwdMessage" + roleId).text("请设置密码");
				return;
			}else if(pwd.length>20||pwd.length<6){
				$("#pwdMessage" + roleId).text("密码长度应为6〜20位字符");
				return;
			}else {
					$("#pwdMessage" + roleId).text("");
			}
		}
		function checkPwdTwo(roleId) {
			var pwd = $("#pwdTwo" + roleId).val();
			if (pwd == '' || pwd == null) {
				$("#pwdTwoMessage" + roleId).text("请再次输入密码");
			} else {
				var pwd1 = $("#pwd" + roleId).val();
				var pwd2 = $("#pwdTwo" + roleId).val();
				if (pwd1 == pwd2) {
					$("#pwdTwoMessage" + roleId).text("");
				} else {
					$("#pwdTwoMessage" + roleId).text("两次密码不一致");
				}
			}
		}
		//验证短信验证码非空 正则
		function checkPhone(roleId) {
			var mobileCode = $("#mobileCode" + roleId).val();
			if (mobileCode == '' || mobileCode == null) {
				$("#mesMessage" + roleId).text("请填写短信验证码");
			} else {
				$("#mesMessage" + roleId).text("");
			}
		}
		//注册
		function submitForm(roleId) {
			var mobile = $("#mobile" + roleId).val();
			var phoneMessage=$("#phoneMessage" + roleId).text();
			var validateCode = $("#validateCode" + roleId).val();
			var message=$("#message" + roleId).text();
			var mobileCode = $("#mobileCode" + roleId).val();
			var mesMessage=$("#mesMessage" + roleId).text();
			var pwd = $("#pwd" + roleId).val();
			var pwdMessage=$("#pwdMessage" + roleId).text();
			var pwdTwo = $("#pwdTwo" + roleId).val();
			var pwdTwoMessage=$("#pwdTwoMessage" + roleId).text();
			var user = {};
			user.mobile = mobile;
			user.pwd = pwd;
			user.pwdTwo = pwdTwo;
			user.category = roleId;
			user.mobileCode = mobileCode;
			if(user!=null&&user!=''){
				if (mobile == '' || mobile == null) {
					$("#phoneMessage" + roleId).text("请输入手机号");
					return;
				} 
				if(phoneMessage=='请输入正确的手机号'||phoneMessage=='请输入手机号'||phoneMessage=='手机号已注册，请登陆'){
					return;
				}
				if(validateCode==''|| validateCode == null){
					$("#message" + roleId).text("请输入验证码");
					return;
				}
				if(message=='请输入验证码'||message=='验证码不正确'||message=='验证码错误，请输入新的验证码'){
					flushValidateCode(roleId);
					return;
				}
				if(mobileCode==''|| mobileCode == null){
					$("#mesMessage" + roleId).text("请填写短信验证码");
					return;
				}
				if(mesMessage=='请填写短信验证码'||mesMessage=='请获取短信验证码'||mesMessage=='手机验证码不正确，请重新输入'){
					return;
				}
				if (pwd == '' || pwd == null) {
					$("#pwdMessage" + roleId).text("请设置密码");
					return;
				} 
				if(pwdMessage=='请设置密码'){
					return;
				}
				if(pwd.length>20||pwd.length<6){
					$("#pwdMessage" + roleId).text("密码长度应为6〜20位字符");
					return;
				}
				if(pwdMessage=='密码长度应为6〜20位字符'){
					return;
				}
				if (pwdTwo == '' || pwdTwo == null) {
					$("#pwdTwoMessage" + roleId).text("请再次输入密码");
					return;
				} 
				if (pwdTwo != pwd) {
					$("#pwdTwoMessage" + roleId).text("两次密码不一致");
					return;
				} 
				if(pwdTwoMessage=='请再次输入密码'||pwdTwoMessage=='两次密码不一致'){
					return;
				}
				if(pwdTwo.length>20||pwdTwo.length<6){
					$("#pwdTwoMessage" + roleId).text("密码长度应为6〜20位字符");
					return;
				}
				if(pwdTwoMessage=='密码长度应为6〜20位字符'){
					return;
				}else{
					$.ajax({
						type : "post",
						url : "/userInfo/registerForm",
						data : JSON.stringify(user),
						success : function(data) {
							if (data.code == 1) {
								if(data.message=='请输入手机号'){
									$("#phoneMessage" + roleId).text(data.message);
								}
								if(data.message=='手机验证码不正确，请重新输入'||data.message=='手机验证码已过期,请重新获取'||data.message=='请获取短信验证码'){
									$("#mesMessage" + roleId).text(data.message);
								}
								if(data.message=='请设置密码'||data.message=='密码长度应为6~20位字符'){
									$("#pwdMessage" + roleId).text(data.message);
								}
								if(data.message=='两次密码不一致'||data.message=='请再次输入密码'){
									$("#pwdTwoMessage" + roleId).text(data.message);
								}
								flushValidateCode(roleId);
								$("#message"+roleId).text("验证码错误，请输入新的验证码");
							}
							if (data.code == 3) {
								$("#phoneMessage" + roleId).html("手机号已注册，请<a href='http://www.chitunion.com/'>登陆</a>");
								flushValidateCode(roleId);
								$("#message"+roleId).text("验证码错误，请输入新的验证码");
							}
							if (data.code== 0) {
								alert("注册成功，请登录");
								location.href = "http://www.chitunion.com/";
							}
						},
						dataType : "json",
						contentType : 'application/json'
					});
				}
			}else{
				alert("请填写注册内容");
			}
		}
		/* 刷新生成验证码 */
		function flushValidateCode(roleId) {
			var validateImgObject = document.getElementById("codeValidateImg"
					+ roleId);
			validateImgObject.src = "/userInfo/getSysManageLoginCode?time="
					+ new Date().getTime();
		}
		/*校验验证码输入是否正确*/
		function checkImg(code, roleId) {
			var validateCode = $("#validateCode" + roleId).val();
			if (null != validateCode && validateCode != '') {
				var url = "/userInfo/checkimagecode";
				$.get(url, {
					"validateCode" : validateCode
				}, function(data) {
					if (data.code == 0) {
						$("#message" + roleId).text("");
						var phoneMessage=$("#phoneMessage" + roleId).text();
						if(phoneMessage==''&&phoneMessage!=null){
							$("#getImgCode"+roleId).removeClass("obtain_no").addClass("obtain");
						}
					} else {
						$("#getImgCode"+roleId).removeClass("obtain").addClass("obtain_no");
						flushValidateCode(roleId);
						$("#message" + roleId).text(data.message);
					}
				})
			} else {
				$("#getImgCode"+roleId).removeClass("obtain").addClass("obtain_no");
				$("#message" + roleId).text("请输入验证码");
			}
		}
		//三秒后清除短信发送提示信息
		var mesTimeOut;
		function clearMes(){
			var mes1=$("#mesMessage29001").text();
			var mes2=$("#mesMessage29002").text();
			if(mes1=='短信已发送 请注意查收'||mes2=='短信已发送 请注意查收'){
				$("#mesMessage29001").text("");
				$("#mesMessage29002").text("");
				window.clearTimeout(mesTimeOut); 
			}
		}
		var countdown = 60;
		//获取短信验证码
		function getMessageCode(obj, roleId) {
				if (countdown == 60) {
						var message=$("#message" + roleId).text();
						var mobile = $("#mobile" + roleId).val();
						var phoneMessage = $("#phoneMessage" + roleId).text();
						var validateCode = $("#validateCode" + roleId).val();
						var user = {};
						user.mobile = mobile;
						user.validateCode = validateCode;
						if (null != mobile && '' != mobile) {
							if(mobile.length<11){
								$("#phoneMessage" + roleId).text("请输入正确的手机号");
								obj.removeAttribute("disabled");
								$("#getImgCode"+roleId).removeClass("obtain").addClass("obtain_no");
								obj.value = "获取验证码";
								countdown = 60;
								return;
							}
							if(validateCode.length<=0){
								$("#message" + roleId).text("请输入验证码");
								obj.removeAttribute("disabled");
								$("#getImgCode"+roleId).removeClass("obtain").addClass("obtain_no");
								obj.value = "获取验证码";
								countdown = 60;
								return;
							}
							if(phoneMessage!='手机号已注册，请登陆'){
								if(message!='请输入验证码'&&message!=null){
									if(message!='验证码不正确'&&message!='验证码错误，请输入新的验证码'){
										$.ajax({
											type : "post",
											url : "/userInfo/getMobileCode",
											data : JSON.stringify(user),
											success : function(data) {
												if (data.code == 1) {
													flushValidateCode(roleId);
													$("#message" + roleId).text(data.message);
													$("#getImgCode"+roleId).removeClass("obtain").addClass("obtain_no");
												}
												if (data.code == 0) {
													$("#mesMessage" + roleId).text("短信已发送 请注意查收");
													mesTimeOut=setTimeout(function() {
														clearMes()
													},3000);
												}
											},
											dataType : "json",
											contentType : 'application/json'
										});
										$("#phoneMessage" + roleId).text("");
									}else{
										flushValidateCode(roleId);
										$("#message" + roleId).text("验证码错误，请输入新的验证码");
										obj.removeAttribute("disabled");
										$("#getImgCode"+roleId).removeClass("obtain").addClass("obtain_no");
										obj.value = "获取验证码";
										countdown = 60;
										return;
									}
								}else{
									$("#message" + roleId).text("请输入验证码");
									$("#getImgCode"+roleId).removeClass("obtain").addClass("obtain_no");
									obj.removeAttribute("disabled");
									obj.value = "获取验证码";
									countdown = 60;
									return;
								}	
							} else {
								$("#phoneMessage" + roleId).html("手机号已注册，请<a href='http://www.chitunion.com/'>登陆</a>");
								obj.removeAttribute("disabled");
								$("#getImgCode"+roleId).removeClass("obtain").addClass("obtain_no");
								obj.value = "获取验证码";
								countdown = 60;
								return;
							}
						} else {
							$("#phoneMessage" + roleId).text("请输入手机号");
							obj.removeAttribute("disabled");
							$("#getImgCode"+roleId).removeClass("obtain").addClass("obtain_no");
							obj.value = "获取验证码";
							countdown = 60;
							return;
						}
					}
					if (countdown == 0) {
						obj.removeAttribute("disabled");
						$("#getImgCode"+roleId).removeClass("obtain").addClass("obtain_no");
						obj.value = "获取验证码";
						countdown = 60;
						return;
					} else {
						$("#getImgCode"+roleId).removeClass("obtain").addClass("obtain_no");
						obj.setAttribute("disabled", true);
						obj.value = countdown+"秒后可重新获取";
						countdown--;
					}
					time1=setTimeout(function() {
						getMessageCode(obj, roleId);
					}, 1000)
		}
		var countdown2=60;
		function getMessageCode2(obj, roleId) {
				if (countdown2 == 60) {
						var message=$("#message" + roleId).text();
						var mobile = $("#mobile" + roleId).val();
						var validateCode = $("#validateCode" + roleId).val();
						var phoneMessage = $("#phoneMessage" + roleId).text();
						var user = {};
						user.mobile = mobile;
						user.validateCode = validateCode;
						if (null != mobile && '' != mobile) {
							if(mobile.length<11){
								$("#phoneMessage" + roleId).text("请输入正确的手机号");
								obj.removeAttribute("disabled");
								$("#getImgCode"+roleId).removeClass("obtain").addClass("obtain_no");
								obj.value = "获取验证码";
								countdown = 60;
								return;
							}
							if(validateCode.length<=0){
								$("#message" + roleId).text("请输入验证码");
								obj.removeAttribute("disabled");
								$("#getImgCode"+roleId).removeClass("obtain").addClass("obtain_no");
								obj.value = "获取验证码";
								countdown = 60;
								return;
							}
							if(phoneMessage!='手机号已注册，请登陆'){
								if(message!='请输入验证码'&&message!=null){
									if(message!='验证码不正确'&&message!='验证码错误，请输入新的验证码'){
										$.ajax({
											type : "post",
											url : "/userInfo/getMobileCode",
											data : JSON.stringify(user),
											success : function(data) {
												if (data.code == 1) {
													flushValidateCode(roleId);
													$("#message" + roleId).text(data.message);
													$("#getImgCode"+roleId).removeClass("obtain").addClass("obtain_no");
												}
												if (data.code == 0) {
													$("#mesMessage" + roleId).text("短信已发送 请注意查收");
													mesTimeOut=setTimeout(function() {
														clearMes()
													},3000);
												}
											},
											dataType : "json",
											contentType : 'application/json'
										});
										$("#phoneMessage" + roleId).text("");
									}else{
										flushValidateCode(roleId);
										$("#message" + roleId).text("验证码错误，请输入新的验证码");
										obj.removeAttribute("disabled");
										$("#getImgCode"+roleId).removeClass("obtain").addClass("obtain_no");
										obj.value = "获取验证码";
										countdown2 = 60;
										return;
									}
								}else{
									$("#message" + roleId).text("请输入验证码");
									$("#getImgCode"+roleId).removeClass("obtain").addClass("obtain_no");
									obj.removeAttribute("disabled");
									obj.value = "获取验证码";
									countdown2 = 60;
									return;
								}
							}else {
								$("#phoneMessage" + roleId).html("手机号已注册，请<a href='http://www.chitunion.com/'>登陆</a>");
								obj.removeAttribute("disabled");
								$("#getImgCode"+roleId).removeClass("obtain").addClass("obtain_no");
								obj.value = "获取验证码";
								countdown = 60;
								return;
							}
						} else {
							$("#phoneMessage" + roleId).text("请输入手机号");
							obj.removeAttribute("disabled");
							$("#getImgCode"+roleId).removeClass("obtain").addClass("obtain_no");
							obj.value = "获取验证码";
							countdown2 = 60;
							return;
						}
					}
					if (countdown2 == 0) {
						obj.removeAttribute("disabled");
						$("#getImgCode"+roleId).removeClass("obtain").addClass("obtain_no");
						obj.value = "获取验证码";
						countdown2 = 60;
						return;
					} else {
						$("#getImgCode"+roleId).removeClass("obtain").addClass("obtain_no");
						obj.setAttribute("disabled", true);
						obj.value = countdown2+"秒后可重新获取";
						countdown2--;
					}
					time1=setTimeout(function() {
						getMessageCode2(obj, roleId);
					}, 1000)
		}
		 //清除数据	
		 function clearData(){
			 $("#mobile29001").val("");
			 $("#phoneMessage29001").text("");
			 $("#validateCode29001").val("");
			 $("#message29001").text("");
			 $("#mobileCode29001").val("");
			 $("#mesMessage29001").text("");
			 $("#pwd29001").val("");
			 $("#pwdMessage29001").text("");
			 $("#pwdTwo29001").val("");
			 $("#pwdTwoMessage29001").text("");
			 
			 $("#mobile29002").val("");
			 $("#phoneMessage29002").text("");
			 $("#validateCode29002").val("");
			 $("#message29002").text("");
			 $("#mobileCode29002").val("");
			 $("#mesMessage29002").text("");
			 $("#pwd29002").val("");
			 $("#pwdMessage29002").text("");
			 $("#pwdTwo29002").val("");
			 $("#pwdTwoMessage29002").text("");
			 
			 $("#getImgCode29001").attr("disabled",false);
			 $("#getImgCode29001").removeClass("obtain").addClass("obtain_no");
			 $("#getImgCode29001").val("获取验证码");
			 $("#getImgCode29002").attr("disabled",false);
			 $("#getImgCode29002").removeClass("obtain").addClass("obtain_no");
			 $("#getImgCode29002").val("获取验证码");
				countdown = 60;
				countdown2= 60;
				return;
		 };
		function registerService(){
			showAEList()
			/*  $.openPopupLayer({
                name: "Reject",
                url: "${pageContext.request.contextPath }/registerService.jsp",
                error: function (dd) {
                    alert(dd.status);
                },
                success: function () {
                   $('.mb25').html(data)
                    //点击取消
                    $('.button').off('click').on('click', function () {
                        $.closePopupLayer('Reject');
                    });
                    $('.fr>a').off('click').on('click', function () {
                        $.closePopupLayer('Reject');
                    }); 
                }
            });*/
		}
		//显示弹窗，及回显所有AE用户
		function showAEList() {
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
			$('#layer').css(showPopup);
			//显示遮罩层
			$('#occlusion').css(showShade);
		}
		//关闭弹层
		function offLayer() {
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
			$('#layer').css(hidePopup);
			// 隐藏遮挡层
			$('#occlusion').css(hideShade);
		}
