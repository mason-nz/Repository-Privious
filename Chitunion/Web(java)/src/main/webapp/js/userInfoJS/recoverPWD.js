		//三秒后清除短信发送提示信息
		var mesTimeOut;
		var countdown = 60;
		$(document).ready(function() {
			flushValidateCode();//进入页面就刷新生成验证码
			$('#getImgCode').attr("disabled",false);
			$('#add').prop('checked',true);
		});
		 document.onkeydown=function(event){
	         var e = event || window.event || arguments.callee.caller.arguments[0];
	         if(e && e.keyCode==27){ // 按 Esc 
	             //要做的事情
	           }
	         if(e && e.keyCode==113){ // 按 F2 
	              //要做的事情
	            }            
	          if(e && e.keyCode==116){ // F5 键
	        	  $('#add').prop('checked',true);
	        	  $('#getImgCode').attr('disabled',false);
	        	  clearData();
	         }
	    };
	    function mobileChange(){
			var phoneMessage=$("#phoneMessage").text();
			var message=$("#message").text();
			if(phoneMessage==''&&message==''&&phoneMessage!=null&&message!=null){
				$("#img01").hide();
				$("#getImgCode").removeClass("obtain_no").addClass("obtain");
			}else{
				$("#getImgCode").removeClass("obtain").addClass("obtain_no");
			}
		}
		function tijiao(){
			var pwd = $("#pwd").val();
			var pwdMessage=$("#pwdMessage").text();
			var pwdTwo = $("#pwdTwo").val();
			var pwdTwoMessage=$("#pwdTwoMessage").text();
			var phoneMessage=$("#phoneMessage").text();
			var userID=$("#userID").val();
			var mobile=$("#mobiles").val();
			var user = {};
			user.userID=userID;
			user.mobile = mobile;
			user.pwd = pwd;
			user.pwdTwo = pwdTwo;
			user.category = $("input:radio[name='roleId']:checked").val();
			if(user!=null&&user!=''){
				if (userID == '' || userID == null) {
					$("#nextStep01").show();
					$("#nextStep02").hide();
					$("#nextStep03").hide();
					return;
				} 
				if (mobile == '' || mobile == null) {
					$("#img01").show();
					$("#phoneMessage").text("请输入手机号");
					$("#nextStep01").show();
					$("#nextStep02").hide();
					$("#nextStep03").hide();
					return;
				} 
				if(phoneMessage=='请输入正确的手机号'||phoneMessage=='请输入手机号'||phoneMessage=='手机号未注册，立即注册'){
					$("#img01").show();
					$("#nextStep01").show();
					$("#nextStep02").hide();
					$("#nextStep03").hide();
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
					$("#pwdMessage").text("密码长度应为6〜20位字符");
					return;
				}
				if(pwdMessage=='密码长度应为6〜20位字符'){
					return;
				}
				if (pwdTwo == '' || pwdTwo == null) {
					$("#pwdTwoMessage").text("请再次输入密码");
					return;
				} 
				if (pwdTwo != pwd) {
					$("#pwdTwoMessage").text("两次密码不一致");
					return;
				} 
				if(pwdTwoMessage=='请再次输入密码'||pwdTwoMessage=='两次密码不一致'){
					return;
				}
				if(pwdTwo.length>20||pwdTwo.length<6){
					$("#pwdTwoMessage").text("密码长度应为6〜20位字符");
					return;
				}
				if(pwdTwoMessage=='密码长度应为6〜20位字符'){
					return;
				}else{
					$.ajax({
						type : "post",
						url : "/userInfo/recoverPWDForm",
						data : JSON.stringify(user),
						success : function(data) {
							if (data.code == 1) {
								if(data.message=='请输入手机号'){
									$("#img01").show();
									$("#phoneMessage").text(data.message);
									$("#nextStep01").show();
									$("#nextStep02").hide();
									$("#nextStep03").hide();
								}
								if(data.message=='请设置密码'||data.message=='密码长度应为6~20位字符'){
									$("#pwdMessage").text(data.message);
									$("#nextStep02").show();
									$("#nextStep01").hide();
									$("#nextStep03").hide();
								}
								if(data.message=='两次密码不一致'||data.message=='请再次输入密码'){
									$("#nextStep02").show();
									$("#nextStep01").hide();
									$("#nextStep03").hide();
									$("#pwdTwoMessage" + roleId).text(data.message);
								}
								flushValidateCode();
								$("#message").text("验证码错误，请输入新的验证码");
							}
							if (data.code == 6) {
								$("#img01").show();
								$("#phoneMessage").html("手机号未注册，<a href='http://j.chitunion.com/userInfo/toRegister'>立即注册</a>");
								$("#getImgCode").removeClass("obtain").addClass("obtain_no");
								flushValidateCode();
								$("#message").text("验证码错误，请输入新的验证码");
								$("#nextStep01").show();
								$("#nextStep02").hide();
								$("#nextStep03").hide();
							}
							if (data.code== 0) {
								$("#nextStep03").show();
								$("#nextStep01").hide();
								$("#nextStep02").hide();
							}
						},
						dataType : "json",
						contentType : 'application/json'
					});
				}
			}else{
				alert("请填写内容");
			}
		}
		 //清除数据	
		 function clearData(){
			 $("#mobile").val("");
			 $("#img01").hide();
			 $("#phoneMessage").text("");
			 $("#validateCode").val("");
			 $("#message").text("");
			 $("#mobileCode").val("");
			 $("#mesMessage").text("");
			 $("#pwd").val("");
			 $("#pwdMessage").text("");
			 $("#pwdTwo").val("");
			 $("#pwdTwoMessage").text("");
			 countdown = 60;
			 return;
		 };
		//换图
		function changeImage(){
			flushValidateCode();
			var message=$("#message").text();
			var validateCode=$("#validateCode").val();
			if(validateCode==''||validateCode==null){
				$("#message").text("请输入验证码");
				$("#getImgCode").removeClass("obtain").addClass("obtain_no");
				return;
			}
			if(validateCode!=''&&message==''){
				$("#message").text("验证码错误，请输入新的验证码");
				$("#getImgCode").removeClass("obtain").addClass("obtain_no");
				return;
			}
		}
		function addvice() {
			clearData();
			flushValidateCode();
		}
		function media() {
			clearData();
			flushValidateCode();
		}
		//校验手机号是否注册
		function checkMobileReg() {
			var mobile = $("#mobile").val();
			if (null != mobile && mobile != '') {
				var user = {};
				user.mobile = mobile;
				if(mobile.length==11){
					user.category = $("input:radio[name='roleId']:checked").val();
					$.ajax({
								type : "post",
								url : "/userInfo/checkPhoneRegister",
								data : JSON.stringify(user),
								success : function(data) {
									if (data.code == 0) {
										$("#img01").show();
										$("#phoneMessage").html("手机号未注册，<a href='http://j.chitunion.com/userInfo/toRegister'>立即注册</a>");
										$("#getImgCode").removeClass("obtain").addClass("obtain_no");
									}else if(data.code ==3){
										$("#img01").hide();
										$("#phoneMessage").text("");
										$("#getImgCode").removeClass("obtain_no").addClass("obtain");
									}else {
										$("#getImgCode").removeClass("obtain").addClass("obtain_no");
										$("#phoneMessage").text("" + data.message);
									}
								},
								dataType : "json",
								contentType : 'application/json'
							});
				}else{
					$("#getImgCode").removeClass("obtain").addClass("obtain_no");
					$("#phoneMessage").text("请输入正确的手机号");
					$("#img01").show();
				}
			} else {
				$("#getImgCode").removeClass("obtain").addClass("obtain_no");
				$("#phoneMessage").text("请输入手机号");
				$("#img01").show();
			}
		}
		//验证密码非空 正则
		function checkPwd() {
			var pwd = $("#pwd").val();
			if (pwd == '' || pwd == null) {
				$("#pwdMessage").text("请设置密码");
				return;
			}else if(pwd.length>20||pwd.length<6){
				$("#pwdMessage").text("密码长度应为6〜20位字符");
				return;
			}else {
					$("#pwdMessage").text("");
			}
		}
		function checkPwdTwo() {
			var pwd = $("#pwdTwo").val();
			if (pwd == '' || pwd == null) {
				$("#pwdTwoMessage").text("请再次输入密码");
			} else {
				var pwd1 = $("#pwd").val();
				var pwd2 = $("#pwdTwo").val();
				if (pwd1 == pwd2) {
					$("#pwdTwoMessage").text("");
				} else {
					$("#pwdTwoMessage").text("两次密码不一致");
				}
			}
		}
		//验证短信验证码非空 正则
		function checkPhone() {
			var mobileCode = $("#mobileCode").val();
			if (mobileCode == '' || mobileCode == null) {
				$("#mesMessage").text("请填写短信验证码");
			} else {
				$("#mesMessage").text("");
			}
		}
		//下一步验证
		function nextStep() {
			var mobile = $("#mobile").val();
			var phoneMessage=$("#phoneMessage").text();
			var validateCode = $("#validateCode").val();
			var message=$("#message").text();
			var mobileCode = $("#mobileCode").val();
			var mesMessage=$("#mesMessage").text();
			var user = {};
			user.mobile = mobile;
			user.category = $("input:radio[name='roleId']:checked").val();
			user.mobileCode = mobileCode;
				if (mobile == '' || mobile == null) {
					$("#phoneMessage").text("请输入手机号");
					$("#img01").show();
					return;
				} 
				if(phoneMessage=='请输入正确的手机号'||phoneMessage=='请输入手机号'||phoneMessage=='手机号未注册，立即注册'){
					$("#img01").show();
					return;
				}
				if(validateCode==''|| validateCode == null){
					$("#message").text("请输入验证码");
					return;
				}
				if(message=='请输入验证码'||message=='验证码不正确'||message=='验证码错误，请输入新的验证码'){
					flushValidateCode();
					return;
				}
				if(mesMessage=='请填写短信验证码'||mesMessage=='请获取短信验证码'||mesMessage=='手机验证码不正确，请重新输入'){
					return;
				}
				if(mobileCode==''|| mobileCode == null){
					$("#mesMessage").text("请填写短信验证码");
					return;
				}
				if(mesMessage=='请填写短信验证码'){
					return;
				}else{
					$.ajax({
						type : "post",
						url : "/userInfo/checkMobileCode",
						data : JSON.stringify(user),
						success : function(data) {
							console.info(data);
							if (data.code == 1) {
								if(data.message=='请输入手机号'){
									$("#img01").show();
									$("#phoneMessage").text(data.message);
								}
								if(data.message=='手机验证码不正确，请重新输入'||data.message=='请获取短信验证码'||data.message=='手机验证码已过期,请重新获取'){
									$("#mesMessage").text(data.message);
								}
								$("#message").text("验证码错误，请输入新的验证码");
								flushValidateCode();
							}
							if (data.code == 6) {
								$("#img01").show();
								$("#phoneMessage").html("手机号未注册，<a href='http://j.chitunion.com/userInfo/toRegister'>立即注册</a>");
								$("#getImgCode").removeClass("obtain").addClass("obtain_no");
								flushValidateCode();
							}
							if (data.code== 0) {
								$("#nextStep02").show();
								$("#nextStep01").hide();
								$("#mobiles").val(data.mobile);
								$("#userID").val(data.userID);
							}
						},
						dataType : "json",
						contentType : 'application/json'
					});
				}
		}
		/* 刷新生成验证码 */
		function flushValidateCode() {
			var validateImgObject = document.getElementById("codeValidateImg");
			validateImgObject.src = "/userInfo/getSysManageLoginCode?time="
					+ new Date().getTime();
		}
		/*校验验证码输入是否正确*/
		function checkImg(code) {
			var validateCode = $("#validateCode").val();
			if (null != validateCode && validateCode != '') {
				var url = "/userInfo/checkimagecode";
				$.get(url, {
					"validateCode" : code
				}, function(data) {
					if (data.code == 0) {
						$("#message").text("");
						var phoneMessage=$("#phoneMessage").text();
						if(phoneMessage==''&&phoneMessage!=null){
							$("#img01").hide();
							$("#getImgCode").removeClass("obtain_no").addClass("obtain");
						}
					} else {
						$("#getImgCode").removeClass("obtain").addClass("obtain_no");
						flushValidateCode();
						$("#message").text(data.message);
					}
				})
			} else {
				$("#getImgCode").removeClass("obtain").addClass("obtain_no");
				$("#message").text("请输入验证码");
			}
		}
		function clearMes(){
			var mes1=$("#mesMessage").text();
			if(mes1=='短信已发送 请注意查收'){
				$("#mesMessage").text("");
				window.clearTimeout(mesTimeOut); 
			}
		}
		//获取短信验证码
		function getMessageCode(obj) {
				if (countdown == 60) {
						var message=$("#message").text();
						var mobile = $("#mobile").val();
						var phoneMessage = $("#phoneMessage").text();
						var validateCode = $("#validateCode").val();
						var user = {};
						user.mobile = mobile;
						user.validateCode = validateCode;
						if (null != mobile && '' != mobile) {
							if(mobile.length<11){
								$("#img01").show();
								$("#phoneMessage").text("请输入正确的手机号");
								obj.removeAttribute("disabled");
								$("#getImgCode").removeClass("obtain").addClass("obtain_no");
								obj.value = "获取验证码";
								countdown = 60;
								return;
							}
							if(validateCode.length<=0){
								$("#message").text("请输入验证码");
								obj.removeAttribute("disabled");
								$("#getImgCode").removeClass("obtain").addClass("obtain_no");
								obj.value = "获取验证码";
								countdown = 60;
								return;
							}
							if(phoneMessage!='手机号未注册，立即注册'){
								if(message!='请输入验证码'&&message!=null){
									if(message!='验证码不正确'&&message!='验证码错误，请输入新的验证码'){
										$.ajax({
											type : "post",
											url : "/userInfo/findMobileCode",
											data : JSON.stringify(user),
											success : function(data) {
												if (data.code == 1) {
													flushValidateCode();
													$("#message").text(data.message);
													$("#getImgCode").removeClass("obtain").addClass("obtain_no");
												}
												if (data.code == 0) {
													$("#mesMessage").text("短信已发送 请注意查收");
													mesTimeOut=setTimeout(function() {
														clearMes()
													},3000);
												}
											},
											dataType : "json",
											contentType : 'application/json'
										});
										$("#phoneMessage").text("");
									}else{
										flushValidateCode();
										$("#message").text("验证码错误，请输入新的验证码");
										obj.removeAttribute("disabled");
										$("#getImgCode").removeClass("obtain").addClass("obtain_no");
										obj.value = "获取验证码";
										countdown = 60;
										return;
									}
								}else{
									$("#message").text("请输入验证码");
									$("#getImgCode").removeClass("obtain").addClass("obtain_no");
									obj.removeAttribute("disabled");
									obj.value = "获取验证码";
									countdown = 60;
									return;
								}	
							} else {
								$("#img01").show();
								$("#phoneMessage").html("手机号未注册，<a href='http://j.chitunion.com/userInfo/toRegister'>立即注册</a>");
								obj.removeAttribute("disabled");
								$("#getImgCode").removeClass("obtain").addClass("obtain_no");
								obj.value = "获取验证码";
								countdown = 60;
								return;
							}
						} else {
							$("#img01").show();
							$("#phoneMessage").text("请输入手机号");
							obj.removeAttribute("disabled");
							$("#getImgCode").removeClass("obtain").addClass("obtain_no");
							obj.value = "获取验证码";
							countdown = 60;
							return;
						}
					}
					if (countdown == 0) {
						obj.removeAttribute("disabled");
						$("#getImgCode").removeClass("obtain").addClass("obtain_no");
						obj.value = "获取验证码";
						countdown = 60;
						return;
					} else {
						$("#getImgCode").removeClass("obtain").addClass("obtain_no");
						obj.setAttribute("disabled", true);
						obj.value = countdown+"秒后可重新获取";
						countdown--;
					}
					setTimeout(function() {
						getMessageCode(obj);
					}, 1000)
		}