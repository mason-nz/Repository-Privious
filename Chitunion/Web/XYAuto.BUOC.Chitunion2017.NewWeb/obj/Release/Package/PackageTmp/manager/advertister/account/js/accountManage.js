/*
* Written by:     wangc
* function:       媒体主账号管理
* Created Date:   2017-12-18
* Modified Date:
*注意：基本信息提交时，如果是微信昵称，提交时，传用户名称会有问题吗？
*如果有问题，就需要判断是哪种方式，对应传不同的字段
*微信昵称显示时，更换手机号，合并信息时，具体操作咋整，不知道，乱死了


**微信用户第一次绑定手机号，且不需合并信息，提交的时候，我怎么知道对着呢。。可以提交呢？
*存在一个问题，手机验证码 失去焦点时，要验证很多东西，而点击获取验证码同样也会触发她的失焦事件
*造成的结果就是：点击获取验证码，就已经在验证手机验证码是否正确了，而不是 发送验证码到手机
*/
$(function(){
	function AccountManage(){
		this.init();
	}
	var UserInfo = new Object();
	AccountManage.prototype = {
		constructor:AccountManage,
		init:function(){
			var _self = this;
			//判断用户角色，广告主无提现账号
			if(CTLogin.Category == '29001'){
				$('.list_switch').find('li[index=2]').remove();
			}
			//城市选择
		    $(JSonData.masterArea).each(function (i) {
		        $('#province').append('<option i=' + i + ' value=' + this.id + '>' + this.name + '</option>')
		    })
		    $('#province').off('change').on('change', function () {
		    	_self.hideTipInfo('#province');
		        var City1 = '', City2 = '<option value="-2">城市</option>';
		        if($('#province option:checked').attr('value') != '-2'){
			        $($(JSonData.masterArea)[$('#province option:checked').attr('i')].subArea).each(function (i) {
			            City1 += '<option i=' + i + ' value=' + this.id + '>' + this.name + '</option>'
			        })
		        }
		        $('#city').html(City2 + City1);
		    })
		    _self.uploadImg('uploadImg');
		    //页面加载需要显示用户原有的信息，如为空或NULL则不显示
			_self.QueryUserBasicInfo();
			
			this.operate();
		},
		operate:function(){
			var _self = this;
			$('.list_switch li').off('click').on('click',function(){
		        var that = $(this);
		        var idx = that.index();
		        if( !CTLogin.Mobile ){//如果不存在手机号，不可修改认证信息和提现账号
		        	if(that.attr('index') == 2 || that.attr('index') == 1){
		        		layer.msg('请先完善手机号',function(){
		        			$('.list_switch>li').eq(0).click();
		        		})
		        	}else{
		        		if(that.attr('index') == 3){//选择修改密码
		        			that.addClass('selected').siblings().removeClass('selected');
				        	$('.Personal_table2>div').eq(3).removeClass('hide').siblings('div').addClass('hide');
				        	$('#submitInfo').hide();
				        }
				        if(that.attr('index') == 0){
				        	that.addClass('selected').siblings().removeClass('selected');
				        	$('.Personal_table2>div').eq(0).removeClass('hide').siblings('div').addClass('hide');
				        	$('#submitInfo').show();
				        }
		        	}
		        }else{//存在手机号
		        	
			        that.addClass('selected').siblings().removeClass('selected');
			        if(idx == 2 && that.attr('index') == 3){//选择修改密码
			        	$('.Personal_table2>div').eq(3).removeClass('hide').siblings('div').addClass('hide');
			        }else{
			        	$('.Personal_table2>div').eq(idx).removeClass('hide').siblings('div').addClass('hide');
			        }
			        
			        //如果是提现账号且之前有支付宝账号数据，不显示提交按钮
			        if(that.attr('index') == 2){
			        	if($('.account').find('.after:visible').length){
			        		$('#submitInfo').hide();
			        	}else{
			        		$('#submitInfo').show();
			        	}
			        //如果是认证信息：且不可编辑，不显示提交按钮
			        }else if(that.attr('index') == 1){
			        	if($('.authInfo').find('.after:visible').length){
			        		$('#submitInfo').hide();
			        	}else{
			        		$('#submitInfo').show();
			        	}
			        }else if(that.attr('index') == 3){
			        	if($('.passWordInfo .noPsd:visible').length){
			        		$('#submitInfo').hide();
			        	}else{
			        		$('#submitInfo').show();
			        	}
			        }else if(that.attr('index') == 0){
			        	$('#submitInfo').show();
			        }
		        }
		    });
		    //从收入管理跳转而来，判断对应选择table
		    if(_self.getRequest().selected){
		    	$('.list_switch>li').each(function(){
		    		if($(this).attr('index') == _self.getRequest().selected){
		    			$(this).click();
		    		}
		    	})
		    }

		    /*基本信息交互*/

		    //当用户输入或选择时，提示信息消失
		    $('#userName').off('focus').on('focus',function(){
		    	_self.hideTipInfo('#userName');
		    })
		    $('#phone_number1').off('focus').on('focus',function(){
		    	_self.hideTipInfo('#phone_number1');
		    })

		    _self.limitNumber('#phone_number1');

		    $('#codeForWeChat').off('focus').on('focus',function(){
		    	_self.hideTipInfo('#codeForWeChat');
		    })
		    $('#codeForWeChat1').off('focus').on('focus',function(){
		    	_self.hideTipInfo('#codeForWeChat1');
		    })
		    $('#address').off('focus').on('focus',function(){
		    	_self.hideTipInfo('#address');
		    })
		    $('#city').off('change').on('change',function(){
		    	_self.hideTipInfo('#city');
		    })

		    //V2.4新增部分：微信相关用户绑定手机号
		    $('#vertifyCodeYa').off('click').on('click',function(){
		    	$(this).attr('src',public_url+'/CheckCode.aspx?r='+Math.random());
		    })

		    //获取验证码--forWechat
		    $('#getCodeYa').off('click').on('click',function(){
		    	var _this = $(this),
		    		$moble = $('#phone_number1'),
		    		curMobile = $.trim($moble.val()),
		    		$code = $('#codeForWeChat'),
		    		errorMsgYa = [];
		    	//验证手机号
		    	if(!curMobile){
		    		$moble.parents('ul').find('.tipInfo').html('<img src="/images/icon7.png"> 请输入手机号！').show();
		    		errorMsgYa.push('1');
		    	}else if(curMobile.length != 11){
		    		$moble.parents('ul').find('.tipInfo').html('<img src="/images/icon7.png"> 请输入正确的手机号！').show();
		    		errorMsgYa.push('1');
		    	}
		    	//验证码
		    	if(!$.trim($code.val())){
		    		$code.parents('ul').find('.tipInfo').html('<img src="/images/icon7.png"> 请输入验证码！').show();
		    		errorMsgYa.push('1');
		    	}
		    	if(errorMsgYa.length == 0){
			    	//发送手机验证码
	        		setAjax({
	        			url:public_url+'/AjaxServers/LoginManager.ashx',
	        			type:'post',
	        			data:{
	        				action : 'sendmobilemsg_modifymobile',
	        				mobile:curMobile,
	        				checkCode:$.trim($code.val())	            				
	        			}
	        		},function(data){
	        			switch(data){
	        				case 0:
	        					layer.msg('已发送',{time:2000});
	        					var time = 60;
			            		_this.html(time+'秒后可重新获取');
			            		var t = setInterval(function(){
			            			time -- ;
			            			if(time > 0){
			            				_this.html(time+'秒后可重新获取');
			            			}else{
			            				_this.html('获取验证码');
			            				clearInterval(t);
			            			}
			            		},1000)
	        					break;
	        				case -9:
	        					$code.parents('ul').find('.tipInfo').html('<img src="/images/icon7.png"> 验证码不正确').show();
	        					$code.click();
	        					break;
	        				case -10:
	        					$('#getCodeYa').parents('ul').find('.tipInfo').html('<img src="/images/icon7.png"> 验证码获取频繁，请稍后再试').show();
	        					$code.click();
	        					break;
	        				case -11:
	        					$('#getCodeYa').parents('ul').find('.tipInfo').html('<img src="/images/icon7.png"> 发送短信失败').show();
	        					$code.click();
	        					break;
	        			}
	        		})
		    		
		    	}
		    })
		    //手机验证码失去焦点事件--各种判断--合并信息
		    $('#codeForWeChat1').off('blur').on('blur',function(e){
		    	var e = e || window.event;
		    	//防止点击在获取验证码上
		    	if(e.target.id != 'getCodeYa'){
		    		_self.confimBindUser();
		    	}
		    	
		    })

		    $('#changePhone').off('click').on('click',function(){
		    	var oldPhoneCode = "";
		        $.openPopupLayer({
		            name: "Layer",
		            url: "modifyPhoneLayer.html",
		            error: function (dd) {
		                // $.closePopupLayer('closedown1');
		            },
		            success: function () {
		            	$('#oldPhone').val(UserInfo.BasicInfo.Mobile);
		            	
		            	var oldCode = '',
		            		newCode = '',
		            		newPhone = '';
		            	//当用户输入时，提示信息消失
		            	$('#codeText').off('focus').on('focus',function(){
		            		$('#oldInfo').find('.tipInfo').html('');
		            	})
		            	$('#newPhone,#codeText1').off('focus').on('focus',function(){
		            		$('#newInfo').find('.tipInfo').html('');
		            	})
		            	$('#vertifyCode').off('click').on('click',function(){
		            		$(this).attr('src',public_url+'/CheckCode.aspx?r='+Math.random());
		            	})
		            	$('#vertifyCode').click();
		            	$('#vertifyCode1').off('click').on('click',function(){
		            		$(this).attr('src',public_url+'/CheckCode.aspx?r='+Math.random());
		            	})
		            	// $('#vertifyCode1').click();
		            	//获取旧手机号验证码
		            	$('#getOldCode').off('click').on('click',function(){
		            		var _this = $(this),
		            			html = _this.html();
		            		if(html != '获取验证码'){
		            			return
		            		}
		            		//发送手机验证码
		            		setAjax({
		            			url:public_url+'/AjaxServers/LoginManager.ashx',
		            			// url:'json/code.json',
		            			type:'post',
		            			data:{
		            				action : 'sendmobilemsg_modifymobile',
		            				mobile:$.trim($('#oldPhone').val()),
		            				checkCode:$.trim($('#codeForVertify').val())		            				
		            			}
		            		},function(data){
		            			switch(data){
		            				case 0:
		            					var time = 60;
					            		_this.html(time+'秒后可重新获取');
					            		var t = setInterval(function(){
					            			time -- ;
					            			if(time > 0){
					            				_this.html(time+'秒后可重新获取');
					            			}else{
					            				_this.html('获取验证码');
					            				clearInterval(t);
					            			}
					            		},1000)
		            					break;
		            				case -9:
		            					$('#oldInfo').find('.tipInfo').html('<img src="/images/icon7.png"> 验证码不正确').show();
		            					$('#vertifyCode').click();
		            					break;
		            				case -10:
		            					$('#oldInfo').find('.tipInfo').html('<img src="/images/icon7.png"> 验证码获取频繁，请稍后再试').show();
		            					$('#vertifyCode').click();
		            					break;
		            				case -11:
		            					$('#oldInfo').find('.tipInfo').html('<img src="/images/icon7.png"> 发送短信失败').show();
		            					$('#vertifyCode').click();
		            					break;
		            			}
		            		})
		            	})
		            	//提交旧手机号验证码
		            	$('.next_step').off('click').on('click',function(){
		            		oldPhoneCode = $.trim($('#codeText').val());
		            		$.post(public_url+'/AjaxServers/LoginManager.ashx',{
								action:'checksmscode',
								mobile:$.trim($('#oldPhone').val()),
								mobileCheckCode:oldPhoneCode
							},function(data){
								  var dataInfo = $.evalJSON(data); 
								  if(dataInfo.result == 0){
								       $('#oldInfo').hide().next().show();
								       $('#vertifyCode1').click();
								  }else{
								      $('#oldInfo').find('.tipInfo').html('<img src="/images/icon7.png"> '+dataInfo.msg).show();
								  }
							});
		            	})
		            	$('#getNewCode').off('click').on('click',function(){
		            		var _this = $(this),
		            			html = _this.html();
		            		$('#newInfo').find('.tipInfo').html('');
		            		newPhone = $.trim($('#newPhone').val());
		            		if( !newPhone ){
		            			$('#newInfo').find('.tipInfo').html('<img src="/images/icon7.png"> 请输入新手机号').show();
		            			return false;
		            		}
		            		if( /^\d{11}$/.test(newPhone) == false){
		            			$('#newInfo').find('.tipInfo').html('<img src="/images/icon7.png"> 请输入正确的新手机号').show();
		            			return false;
		            		}
		            		
		            		//发送手机验证码
		            		setAjax({
		            			url:public_url+'/AjaxServers/LoginManager.ashx',
		            			// url:'json/code.json',
		            			type:'post',
		            			data:{
		            				action : 'sendmobilemsg_modifymobile',
		            				mobile : newPhone,
		            				checkCode : $.trim($('#codeForVertify1').val())		            				
		            			}
		            		},function(data){
		            			switch(data){
		            				case 0:
		            					var time1 = 60;
					            		_this.html(time1+'秒后可重新获取');
					            		var t1 = setInterval(function(){
					            			time1 -- ;
					            			if(time1 > 0){
					            				_this.html(time1+'秒后可重新获取');
					            			}else{
					            				_this.html('获取验证码');
					            				clearInterval(t1);
					            			}
					            			
					            		},1000)
		            					break;
		            				case -9:
		            					$('#newInfo').find('.tipInfo').html('<img src="/images/icon7.png"> 验证码不正确').show();
		            					$('#vertifyCode1').click();
		            					break;
		            				case -10:
		            					$('#newInfo').find('.tipInfo').html('<img src="/images/icon7.png"> 验证码获取频繁，请稍后再试').show();
		            					$('#vertifyCode1').click();
		            					break;
		            				case -11:
		            					$('#newInfo').find('.tipInfo').html('<img src="/images/icon7.png"> 发送短信失败').show();
		            					$('#vertifyCode1').click();
		            					break;
		            			}
		            		})
		            	})
		            	//提交新手机号验证码
		            	$('#sure_submmit').off('click').on('click',function(){
		            		var codeText = $.trim($('#codeText1').val());
			            	if( !newPhone ){
		            			$('#newInfo').find('.tipInfo').html('<img src="/images/icon7.png"> 请输入新手机号').show();
		            			return false;
		            		}
		            		if( /^\d{11}$/.test(newPhone) == false){
		            			$('#newInfo').find('.tipInfo').html('<img src="/images/icon7.png"> 请输入正确的新手机号').show();
		            			return false;
		            		}
		            		if( !codeText ){
		            			$('#newInfo').find('.tipInfo').html('<img src="/images/icon7.png"> 请输入手机验证码').show();
		            			return false;
		            		}

		            		$.post(public_url+'/AjaxServers/LoginManager.ashx',{
								action:'checksmscode',
	            				mobile:newPhone,
	            				mobileCheckCode:codeText
							},function(data){
								  var dataInfo = $.evalJSON(data);
								  if(dataInfo.result == 0){
								       setAjax({
					            			url:public_url+'/api/UserMange/EditUserMobileInfo',
					            			type:'post',
					            			data:{
					            				UserID : CTLogin.UserID,
					            				Mobile : newPhone,
					            				ValidateCode : codeText
					            			}
					            		},function(data1){
					            			if(data1.Status == 0){
						            			$.closePopupLayer('Layer');
						            			var curPhone = newPhone.substr(0,3)+'****'+newPhone.substr(7,10);
							            		$('#phone_number').html(curPhone);
					            			}else{
					            				$('#newInfo').find('.tipInfo').html('<img src="/images/icon7.png"> '+data1.Message).show();
					            			}
					            		})
								  }else{
								      $('#newInfo').find('.tipInfo').html('<img src="/images/icon7.png"> '+dataInfo.msg).show();
								  }
							});
		            	})
		            	$('#closebt').off('click').on('click',function(){
		            		$.closePopupLayer('Layer');
		            	})
		            }
		        })
		    })
		    /*认证信息交互*/
		    //类型为企业时，显示为公司名称，营业执照，类型为个人时，显示为真实姓名，手持身份证。
		    $('#identityType input').off('change').on('change',function(){
		    	var type = $('#identityType input:checked').attr('value'),
		    		$nameInfo = $('#nameInfo'),
		    		$operateName = $('#nameInfo').parents('ul').find('.tr100'),
		    		$operatePic = $('.business_license .tr100');
		    	$('#identityType').attr('type',type);
		    	$('#identityType').parents('.authInfo').find('.tipInfo').hide();
		    	//清空信息
		    	$nameInfo.val('');
		    	$('#identify_number').val('');
		    	$('#loadFile').attr('src','/images/icon8.png');
		    	$('.display_file_name').hide();
		    	if(type == 1001){//企业
		    		$nameInfo.attr('placeholder','请输入公司名称');
		    		$operateName.html('<span class="red">*</span> 公司名称：');
		    		$operatePic.html('<span class="red">*</span> 营业执照：');
		    		$('.identify_number').hide();
		    	}else{//个人
		    		$nameInfo.attr('placeholder','请输入真实姓名');
		    		$operateName.html('<span class="red">*</span> 真实姓名：');
		    		$('.identify_number').show();
		    		$operatePic.html('<span class="red">*</span> 手持身份证：');
		    	}
		    })
		    //当用户输入或选择时，提示信息消失
		    $('#nameInfo').off('focus').on('focus',function(){
		    	_self.hideTipInfo('#nameInfo');
		    })
		    $('#identify_number').off('focus').on('focus',function(){
		    	_self.hideTipInfo('#identify_number');
		    })

		    //点击重新认证时，切换显示隐藏
		    $('.reAuth').off('click').on('click',function(){
		    	var authInfo_audit = UserInfo.AuthenticationInfo;
				$('#nameInfo').val(authInfo_audit.TrueName);
		    	if(authInfo_audit.Type == 1002){//个人
		    		$('.radio_type').find('input[type=radio]').eq(1).click();
		    		$('#loadFile').attr('src',authInfo_audit.IDCardFrontURL);
		    		$('#identify_number').val(authInfo_audit.IdentityNo);
		    	}else{//企业
		    		$('.radio_type').find('input[type=radio]').eq(0).click();
		    		$('#loadFile').attr('src',authInfo_audit.BLicenceURL);
		    	}
		    	$('.authInfo').find('.before').removeClass('hide').next('.after').addClass('hide');
		    	$('#submitInfo').show();
		    })
		    /*提现账号*/
		    //如果用户已经保存过支付宝账号，页面上显示支付宝账号，中间4位字符用*号显示,如果没有保存过，直接显示文本框让用户输入
		    $('#alipayAccount').off('focus').on('focus',function(){
		    	_self.hideTipInfo('#alipayAccount');
		    })
		    $('#sureAlipayAccount').off('focus').on('focus',function(){
		    	_self.hideTipInfo('#sureAlipayAccount');
		    })

		    $('#oldAlipayAccount').off('focus').on('focus',function(){
		    	_self.hideTipInfo('#oldAlipayAccount');
		    })
		    $('#newAlipayAccount').off('focus').on('focus',function(){
		    	_self.hideTipInfo('#newAlipayAccount');
		    })
		    $('#sureNewAlipayAccount').off('focus').on('focus',function(){
		    	_self.hideTipInfo('#sureNewAlipayAccount');
		    })

		    $('#modifyAlipay').off('click').on('click',function(){
		    	$('.account>div').addClass('hide');
		    	$('.account').find('.afterSubmit').removeClass('hide');
		    	$('#submitInfo').show();
		    })

		    /*修改密码*/

		    //当用户输入或选择时，提示信息消失
		    $('#oldPsw').off('focus').on('focus',function(){
		    	_self.hideTipInfo('#oldPsw');
		    })
		    $('#newPsw').off('focus').on('focus',function(){
		    	_self.hideTipInfo('#newPsw');
		    })
		    $('#sureNewPsw').off('focus').on('focus',function(){
		    	_self.hideTipInfo('#sureNewPsw');
		    })
		    $('#createNewPsw').off('focus').on('focus',function(){
		    	_self.hideTipInfo('#createNewPsw');
		    })
		    $('#createSureNewPsw').off('focus').on('focus',function(){
		    	_self.hideTipInfo('#createSureNewPsw');
		    })
		    //提交
			$('#submitInfo').off('click').on('click',function(){
        		var baseInfo = new Object(),
        			errorInfo = [];
				switch($('.list_switch .selected').attr('index')){
					case '0'://基本信息
						if($('.basicInfo .noPhone:visible').length){//用户首次绑定手机号且不需合并信息
							baseInfo = _self.getBindBaseInfo();
							var _this = $('#codeForWeChat1'),
					    		codeForWeChat1 = $.trim(_this.val()),
					    		$moble = $('#phone_number1'),
					    		curMobile = $.trim($moble.val()),
					    		$code = $('#codeForWeChat'),
					    		errorMsgYa = [];
					    	//验证手机号
					    	if(!curMobile){
					    		$moble.parents('ul').find('.tipInfo').html('<img src="/images/icon7.png"> 请输入手机号！').show();
					    		errorMsgYa.push('1');
					    	}else if(curMobile.length != 11){
					    		$moble.parents('ul').find('.tipInfo').html('<img src="/images/icon7.png"> 请输入正确的手机号！').show();
					    		errorMsgYa.push('1');
					    	}
					    	//验证码
					    	if(!$.trim($code.val())){
					    		$code.parents('ul').find('.tipInfo').html('<img src="/images/icon7.png"> 请输入验证码！').show();
					    		errorMsgYa.push('1');
					    	}
					    	//手机验证码
					    	if( !codeForWeChat1 ){
					    		_this.parents('ul').find('.tipInfo').html('<img src="/images/icon7.png"> 请输入手机验证码！').show();
					    		errorMsgYa.push('1');
					    	}else if(codeForWeChat1.length != 4){
					    		_this.parents('ul').find('.tipInfo').html('<img src="/images/icon7.png"> 请输入正确的手机验证码！').show();
					    		errorMsgYa.push('1');
					    	}
					    	
					    	console.log(errorMsgYa.length);
					    	//如果手机验证码是4位且用户输入了手机号和验证码，调接口验证手机验证码是否正确，如果正确，调接口判断是否需合并信息
					    	if( !errorMsgYa.length){
					    		//验证短信验证码
					    		$.post(public_url+'/AjaxServers/LoginManager.ashx', {
				                    action: 'checksmscode',
				                    mobile: curMobile,
				                    mobileCheckCode:codeForWeChat1, 
				                    r: Math.random()
				                }, function(data){
				                	var returnData = $.evalJSON(data);
				                	console.log($.evalJSON(data));

				                    if(returnData.result == 0){//验证成功--判断是否需要合并信息
				                    	$.ajax({
				                    		url:public_url+'/api/UserMange/VerifyMobileBind',
											type:'post',
											async: false,
									        dataType: 'json',
									        xhrFields: {
									            withCredentials: true
									        },
									        crossDomain: true,
									        data:{
							    				Mobile:curMobile,
							    				MobileCode:codeForWeChat1
							    			},
							    			success:function(data1){
							    				if(data1.Status == 0){//验证通过，可正常提交
							    					if( baseInfo.ProvinceID == '-2' ){
									        		}else if( baseInfo.CityID == '-2'){
									        			$('#province').parents('ul').find('.tipInfo').html('<img src="/images/icon7.png">请选择所在城市').show();
									        			errorMsgYa.push('请选择所在城市');
									        		} 
									        		
									        		//联系地址--非必填项
									        		if( !baseInfo.Address ){
									        		}else if( baseInfo.Address.length > 100 ){
									        			$('#address').parents('ul').find('.tipInfo').html('<img src="/images/icon7.png">联系地址最多输入100个汉字').show();
									        			errorMsgYa.push('联系地址最多输入100个汉字');
									        		}
									        		if(errorMsgYa.length == 0){
								    					setAjax({
									        				url:public_url+'/api/UserMange/firstUpdateUser',
									        				// url:'json/code.json',
									        				type:'post',
									        				data:baseInfo
									        			},function(data){
									        				if(data.Status == 0){
									        					layer.msg('保存成功',{time:2000});
									        					//重新初始化
									        					_self.QueryUserBasicInfo();
									        				}else{
									        					layer.msg(data.Message,{time:2000})
									        				}
									        			})
									        		}
								    			}else if(data1.Status == 50001){//验证码错误
								    				$code.parents('ul').find('.tipInfo').html('<img src="/images/icon7.png"> 验证码不正确，请重新输入！').show();
								    			}else if(data1.Status == 50002){//手机验证码不正确
								    				_this.parents('ul').find('.tipInfo').html('<img src="/images/icon7.png"> 手机验证码不正确，请重新输入！').show();
								    			}else if(data1.Status == 60001){//手机号已注册且已绑定微信号
								    				$moble.parents('ul').find('.tipInfo').html('<img src="/images/icon7.png"> 该手机号已注册，请重新输入！').show();
								    			}else if(data1.Status == 60002){
								    				layer.confirm('该手机号已经注册，是否做信息合并？', {
													  	btn: ['修改手机号','合并信息'] //按钮
													}, function(index){
													  	layer.close(index);
													}, function(){//合并信息
														$.ajax({
															url:public_url+'/api/UserMange/BindUser',
															type:'post',
															async: false,
													        dataType: 'json',
													        xhrFields: {
													            withCredentials: true
													        },
													        crossDomain: true,
													        data:{
																Mobile:curMobile,
																MobileCode:codeForWeChat1
															},
															success:function(response){
																if(response.Status == 0){///验证通过
																	layer.msg('合并信息成功',{time:2000});
																	location.reload();
																}else{
																	layer.msg('合并失败');
																	console.log('合并失败');
																}
															}
														})
													});
								    			}
							    			}
				                    	})
				                    }else{
				                    	errorMsgYa.push(returnData.msg);
				                    	layer.msg(returnData.msg,{time:2000});
				                    	console.log(errorMsgYa);
				                    }
				                });
					    	}else{
					    		console.log(errorMsgYa);
					    	}
						}else{//PC端注册用户，同第一版
							baseInfo = _self.getBaseInfo();
			        		//用户名---不需验证
			        		// if( !baseInfo.UserName ){
			        		// 	$('#userName').parents('ul').find('.tipInfo').html('<img src="/images/icon7.png">请输入用户名').show();
			        		// 	errorInfo.push('请输入用户名');
			        		// }
			        		//正则验证用户名
			        		// var reg = /^(?![\d]+$)(?![a-zA-Z]+$)(?![_-]+$)[\da-zA-Z_-]{4,20}$/;
			        		// if(reg.test(baseInfo.UserName) == false){
			        		// 	$('#userName').parents('ul').find('.tipInfo').html('<img src="/images/icon7.png">用户名必须为字母、数字、”_”、“-”两种及以上组合的4-20个字符').show();
			        		// 	errorInfo.push('用户名必须为字母、数字、”_”、“-”两种及以上组合的4-20个字符');
			        		// }
			        		//城市--非必填项
			        		if( baseInfo.ProvinceID == '-2' ){
			        			// $('#province').parents('ul').find('.tipInfo').html('<img src="/images/icon7.png">请选择所在省份').show();
			        			// errorInfo.push('请选择所在省份');
			        		}else if( baseInfo.CityID == '-2'){
			        			$('#province').parents('ul').find('.tipInfo').html('<img src="/images/icon7.png">请选择所在城市').show();
			        			errorInfo.push('请选择所在城市');
			        		} 
			        		
			        		//联系地址--非必填项
			        		if( !baseInfo.Address ){
			        			// $('#address').parents('ul').find('.tipInfo').html('<img src="/images/icon7.png">请输入联系地址').show();
			        			// errorInfo.push('请输入联系地址');
			        		}else if( baseInfo.Address.length > 100 ){
			        			$('#address').parents('ul').find('.tipInfo').html('<img src="/images/icon7.png">联系地址最多输入100个汉字').show();
			        			errorInfo.push('联系地址最多输入100个汉字');
			        		}
			        		if(errorInfo.length == 0){
			        			setAjax({
			        				url:public_url+'/api/UserMange/EditUserBasicInfo',
			        				// url:'json/code.json',
			        				type:'post',
			        				data:baseInfo
			        			},function(data){
			        				if(data.Status == 0){
			        					layer.msg('保存成功',{time:2000});
			        					//重新初始化
			        					_self.QueryUserBasicInfo();
			        				}else{
			        					layer.msg(data.Message,{time:2000})
			        				}
			        			})
			        		}
							
						}
		        		break;
		        	case '1'://认证信息
		        		baseInfo = _self.getAuthenticationInfo();
		        		if(baseInfo.Type == 1001){//企业
		        			var $nameTip = $('#nameInfo').parents('ul').find('.tipInfo'),
		        				$imgTip = $('#loadFile').parents('ul').find('.tipInfo'),
		        				$identiTip = $('#identify_number').parents('ul').find('.tipInfo');
		        			if( !baseInfo.TrueName ){
		        				$nameTip.html('<img src="/images/icon7.png"> 请输入公司名称').show();
		        				errorInfo.push('请输入公司名称');
		        			}else if(baseInfo.TrueName.length > 100 ){
		        				$nameTip.html('<img src="/images/icon7.png"> 公司名称最多可输入100个汉字').show();
		        				errorInfo.push('公司名称最多可输入100个汉字');
		        			}
		        			if( baseInfo.BLicenceURL == '/images/icon8.png'){
		        				$imgTip.html('<img src="/images/icon7.png"> 请上传营业执照').show();
		        				errorInfo.push('请上传营业执照');
		        			}

						}else{//个人
							var $nameTip = $('#nameInfo').parents('ul').find('.tipInfo'),
								$imgTip = $('#loadFile').parents('ul').find('.tipInfo'),
								$identiTip = $('#identify_number').parents('ul').find('.tipInfo');
		        			if( !baseInfo.TrueName ){
		        				$nameTip.html('<img src="/images/icon7.png"> 请输入真实姓名').show();
		        				errorInfo.push('请输入真实姓名');
		        			}else if(baseInfo.TrueName.length > 5 ){
		        				$nameTip.html('<img src="/images/icon7.png"> 真实姓名最多可输入5个汉字').show();
		        				errorInfo.push('真实姓名最多可输入5个汉字');
		        			}
		        			if( !baseInfo.IdentityNo ){
		        				$identiTip.html('<img src="/images/icon7.png"> 请输入身份证号').show();
		        				errorInfo.push('请输入身份证号');
		        			}else{
			        			if(/^(^[1-9]\d{7}((0\d)|(1[0-2]))(([0|1|2]\d)|3[0-1])\d{3}$)|(^[1-9]\d{5}[1-9]\d{3}((0\d)|(1[0-2]))(([0|1|2]\d)|3[0-1])((\d{4})|\d{3}[Xx])$)$/.test(baseInfo.IdentityNo) == false){
			        				$identiTip.html('<img src="/images/icon7.png"> 身份证号格式不正确').show();
			        				errorInfo.push('身份证号格式不正确');
			        			}
		        			}
		        			if( baseInfo.BLicenceURL == '/images/icon8.png'){
		        				$imgTip.html('<img src="/images/icon7.png"> 请上传手持身份证照片').show();
		        				errorInfo.push('请上传手持身份证照片');
		        			}
						}
						if(errorInfo.length == 0){
		        			setAjax({
		        				url:public_url+'/api/UserMange/EditUserAuthenticationInfo',
		        				type:'post',
		        				data:baseInfo
		        			},function(data){
		        				if(data.Status == 0){
		        					layer.msg('保存成功',{time:2000});
		        					//重新获取信息
		        					_self.QueryUserBasicInfo();
		        				}else{
		        					layer.msg(data.Message,{time:2000})
		        				}
		        			})
		        		}
						break;
					case '2'://提现
						//支付宝账号还需要验证，具体看原型：长度，格式等
						//需要判断是第一次添加支付宝账号还是修改支付宝账号
						if($('.account .afterSubmit:visible').length == 1){
							baseInfo = _self.getModifyAlipay();
							var $oldAlipayTip = $('#oldAlipayAccount').parents('ul').find('.tipInfo'),
								$accountTip = $('#newAlipayAccount').parents('ul').find('.tipInfo'),
								$accountTip1 = $('#sureNewAlipayAccount').parents('ul').find('.tipInfo');
							if( !baseInfo.OldAccountName ){
								$oldAlipayTip.html('<img src="/images/icon7.png">请输入旧支付宝账号').show();
								errorInfo.push('请输入旧支付宝账号');
							}else{
								// 如支付宝账号不是手机或邮箱格式，或者超过100个字符，提示“支付宝账号格式不正确！”
								if(/^(0|86|17951)?(13[0-9]|15[012356789]|17[678]|18[0-9]|14[57])[0-9]{8}$/.test(baseInfo.OldAccountName) == false
								 &&
									/\w+([-+.]\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*/.test(baseInfo.AccountName) == false){
									$oldAlipayTip.html('<img src="/images/icon7.png"> 支付宝账号格式不正确').show();
									errorInfo.push('支付宝账号格式不正确');
								}else if(baseInfo.AccountName.length > 100){
									$oldAlipayTip.html('<img src="/images/icon7.png"> 支付宝账号格式不正确').show();
									errorInfo.push('支付宝账号格式不正确');
								}
							}
							if( !baseInfo.AccountName ){
								$accountTip.html('<img src="/images/icon7.png">请输入新支付宝账号').show();
								errorInfo.push('请输入新支付宝账号');
							}else{
								// 如支付宝账号不是手机或邮箱格式，或者超过100个字符，提示“支付宝账号格式不正确！”
								if(/^(0|86|17951)?(13[0-9]|15[012356789]|17[678]|18[0-9]|14[57])[0-9]{8}$/.test(baseInfo.AccountName) == false
								 &&
									/\w+([-+.]\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*/.test(baseInfo.AccountName) == false){
									$accountTip.html('<img src="/images/icon7.png"> 支付宝账号格式不正确').show();
									errorInfo.push('支付宝账号格式不正确');
								}else if(baseInfo.AccountName.length > 100){
									$accountTip.html('<img src="/images/icon7.png"> 支付宝账号格式不正确').show();
									errorInfo.push('支付宝账号格式不正确');
								}
								if( baseInfo.AccountNameAgain && baseInfo.AccountNameAgain != baseInfo.AccountName ){
									$accountTip1.html('<img src="/images/icon7.png"> 两次输入的支付宝账号不一致，请重新输入').show();
									errorInfo.push('两次输入的支付宝账号不一致，请重新输入');
								}
							}
							if( !baseInfo.AccountNameAgain ){
								$accountTip1.html('<img src="/images/icon7.png"> 请再次输入新支付宝账号').show();
								errorInfo.push('请再次输入新支付宝账号');
							}
							if(errorInfo.length == 0){
								//调接口
								setAjax({
									// url:'json/code.json',
									url:public_url+'/api/UserMange/FillWithdrawalsInfo',
									type:'post',//后期改为post
									data:baseInfo
								},function(data){
									if(data.Status == 0){
										_self.QueryUserBasicInfo();
									}else{
										layer.msg(data.Message,{time:2000});
									}
								})
							}
						}else{
							baseInfo = _self.getAlipayInfo();
							var $accountTip = $('#alipayAccount').parents('ul').find('.tipInfo'),
								$accountTip1 = $('#sureAlipayAccount').parents('ul').find('.tipInfo');
							if( !baseInfo.AccountName ){
								$accountTip.html('<img src="/images/icon7.png">请输入支付宝账号').show();
								errorInfo.push('请输入支付宝账号');
							}else{
								// 如支付宝账号不是手机或邮箱格式，或者超过100个字符，提示“支付宝账号格式不正确！”
								if(/^(0|86|17951)?(13[0-9]|15[012356789]|17[678]|18[0-9]|14[57])[0-9]{8}$/.test(baseInfo.AccountName) == false
								 &&
									/\w+([-+.]\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*/.test(baseInfo.AccountName) == false){
									$accountTip.html('<img src="/images/icon7.png"> 支付宝账号格式不正确').show();
									errorInfo.push('支付宝账号格式不正确');
								}else if(baseInfo.AccountName.length > 100){
									$accountTip.html('<img src="/images/icon7.png"> 支付宝账号格式不正确').show();
									errorInfo.push('支付宝账号格式不正确');
								}
								if( baseInfo.AccountNameAgain && baseInfo.AccountNameAgain != baseInfo.AccountName ){
									$accountTip1.html('<img src="/images/icon7.png"> 两次输入的支付宝账号不一致，请重新输入').show();
									errorInfo.push('两次输入的支付宝账号不一致，请重新输入');
								}
							}
							if( !baseInfo.AccountNameAgain ){
								$accountTip1.html('<img src="/images/icon7.png"> 请再次输入支付宝账号').show();
								errorInfo.push('请再次输入支付宝账号');
							}
							if(errorInfo.length == 0){
								//调接口
								setAjax({
									// url:'json/code.json',
									url:public_url+'/api/UserMange/UpdateWithdrawalsInfo',
									type:'post',//后期改为post
									data:baseInfo
								},function(data){
									if(data.Status == 0){
										_self.QueryUserBasicInfo();
									}else{
										layer.msg(data.Message,{time:2000});
									}
								})
							}
						}
						break;
					case '3'://修改密码
						var $oldPsw = $('#oldPsw'),//原密码
							$newPsw = $('#newPsw'),//新密码
							$sureNewPsw = $('#sureNewPsw'),//确认新密码
							$createPsd = $('#createNewPsw'),//创建密码
							$createSureNewPsw = $('#createSureNewPsw');//确认创建密码
						if($('.passWordInfo .createPsd:visible').length){//说明是创建密码
							baseInfo = _self.getCreatePsd();
							if( !baseInfo.Password ){
								$createPsd.parents('ul').find('.tipInfo').html('<img src="/images/icon7.png"> 请输入密码').show();
								errorInfo.push('请输入密码');
							}else if(baseInfo.Password.length > 20){
								$createPsd.parents('ul').find('.tipInfo').html('<img src="/images/icon7.png"> 密码最多20个字符').show();
								errorInfo.push('密码最多20个字符');
							}
							if( !baseInfo.PasswordAgain ){
								$createSureNewPsw.parents('ul').find('.tipInfo').html('<img src="/images/icon7.png"> 请再次输入密码').show();
								errorInfo.push('请输入新密码');
							}else{
								if(baseInfo.PasswordAgain.length > 20){
									$createSureNewPsw.parents('ul').find('.tipInfo').html('<img src="/images/icon7.png"> 密码最多20个字符').show();
									errorInfo.push('密码最多20个字符');
								}
								if( baseInfo.PasswordAgain != baseInfo.Password){
									$createSureNewPsw.parents('ul').find('.tipInfo').html('<img src="/images/icon7.png"> 两次输入的密码不一致，请重新输入').show();
									errorInfo.push('两次输入的密码不一致，请重新输入');
								}
							}
							if(errorInfo.length == 0){
			        			setAjax({
			        				url:public_url+'/api/UserMange/addPassword',
			        				type:'post',
			        				data:baseInfo
			        			},function(data){
			        				if(data.Status == 0){
			        					layer.msg('保存成功',{time:2000});
			        					$('#createNewPsw,#createSureNewPsw').val('');
			        				}else{
			        					layer.msg(data.Message,{time:2000})
			        				}
			        			})
			        		}
						}else if($('.passWordInfo .hasPsd:visible').length){//说明是修改密码
							baseInfo = _self.getPasswordInfo();
							if( !baseInfo.OldPassword ){
								$oldPsw.parents('ul').find('.tipInfo').html('<img src="/images/icon7.png"> 请输入旧密码').show();
								errorInfo.push('请输入旧密码');
							}else if(baseInfo.OldPassword.length > 20){
								$oldPsw.parents('ul').find('.tipInfo').html('<img src="/images/icon7.png"> 密码最多20个字符').show();
								errorInfo.push('密码最多20个字符');
							}
							if( !baseInfo.NewPassword ){
								$newPsw.parents('ul').find('.tipInfo').html('<img src="/images/icon7.png"> 请输入新密码').show();
								errorInfo.push('请输入新密码');
							}else{
								if( baseInfo.OldPassword == baseInfo.NewPassword ){
									$newPsw.parents('ul').find('.tipInfo').html('<img src="/images/icon7.png"> 新密码与旧密码相同').show();
									errorInfo.push('新密码与旧密码相同');
								}else if(baseInfo.NewPassword.length > 20){
									$newPsw.parents('ul').find('.tipInfo').html('<img src="/images/icon7.png"> 密码最多20个字符').show();
									errorInfo.push('密码最多20个字符');
								}
								if( baseInfo.ConfirmPassword && baseInfo.NewPassword != baseInfo.ConfirmPassword){
									$sureNewPsw.parents('ul').find('.tipInfo').html('<img src="/images/icon7.png"> 两次输入的密码不一致，请重新输入').show();
									errorInfo.push('两次输入的密码不一致，请重新输入');
								}
							}
							if(errorInfo.length == 0){
			        			setAjax({
			        				url:public_url+'/api/UserMange/EditUserPasswordInfo',
			        				type:'post',
			        				data:baseInfo
			        			},function(data){
			        				if(data.Status == 0){
			        					layer.msg('修改成功',{time:2000});
			        					$('#oldPsw,#newPsw,#sureNewPsw').val('');
			        				}else{
			        					layer.msg(data.Message,{time:2000})
			        				}
			        			})
			        		}
						}
						break;
					default:
						break;
		        }

		    })
		    
		},
		getBaseInfo:function(){//基本信息
			return {
				UserID : CTLogin.UserID,
				UserName : $('#UserName').html() || '',
				ProvinceID : $('#province option:checked').attr('value')-0,
				CityID : $('#city option:checked').attr('value')-0,
				Address : $.trim($('#address').val())
			}
		},
		getBindBaseInfo:function(){//针对首次绑定手机号且无需合并的用户获取基本信息
			return {
				Mobile: $.trim($('#phone_number1').val()),
				MobileCode : $.trim($('#codeForWeChat1').val()),
				ProvinceID : $('#province option:checked').attr('value')-0,
				CityID : $('#city option:checked').attr('value')-0,
				Address:$.trim($('#address').val())
			}
		},
		getAuthenticationInfo:function(){//认证信息
			return {
				UserID : CTLogin.UserID,
				Type : $('#identityType').attr('type')-0,
				TrueName : $.trim($('#nameInfo').val()),
				IdentityNo:$.trim($('#identify_number').val()),//身份证字段等后台更新接口文档后需修改
				BLicenceURL : $('#loadFile').attr('src'),
				IDCardFrontURL:$('#loadFile').attr('src')
			}
		},
		getAlipayInfo:function(){//提现账号
			return {
				AccountType:96001,
				AccountName : $.trim($('#alipayAccount').val()),
				AccountNameAgain : $.trim($('#sureAlipayAccount').val())
			}
		},
		getModifyAlipay:function(){//支付宝账号保存成功后，再次修改时，获取修改后的支付宝账号信息
			return {
				AccountType:96001,
				OldAccountName:$.trim($('#oldAlipayAccount').val()),
				AccountName : $.trim($('#newAlipayAccount').val()),
				AccountNameAgain : $.trim($('#sureNewAlipayAccount').val())
			}
		},
		getPasswordInfo:function(){//修改密码
			return {
				UserID : CTLogin.UserID,
				OldPassword : $.trim($('#oldPsw').val()),
				NewPassword : $.trim($('#newPsw').val()),
				ConfirmPassword : $.trim($('#sureNewPsw').val())
			}
		},
		getCreatePsd:function(){//创建密码--针对微信无密码或其他方式下无密码，需创建密码
			return {
				Password:$.trim($('#createNewPsw').val()),
				PasswordAgain:$.trim($('#createSureNewPsw').val())
			}
		},
		QueryUserBasicInfo:function(){//调接口查询账号基本信息
			var _self = this;
			setAjax({
				// url:'json/info.json',
				url:public_url+'/api/UserMange/QueryUserBasicInfo',
				type:'get',
				data:{
					UserID:CTLogin.UserID
				}
			},function(data){
				if(data.Status == 0){
					UserInfo = data.Result;
					var BasicInfo = data.Result.BasicInfo,
						AuthenticationInfo =  data.Result.AuthenticationInfo,
						BankAccountInfo = data.Result.BankAccountInfo;
					/**基本信息**/
					//基本信息增加微信登录后基本信息显示---RegisterType注册类型
					//如果有用户名，显示用户名、手机号、
					//如果没有用户名，说明是微信登录，需显示昵称。
					//若存在手机号，显示手机号和更改手机号按钮，若不存在，显示手机号input框
					//若用户名为空，显示微信下昵称显示；如已经有用户名，则显示用户名
					if( BasicInfo.UserName && BasicInfo.RegisterType == '199001'){
						$('.basicInfo .forPc').removeClass('hide').siblings('.forWeChat').addClass('hide');
						$('#UserName').html(BasicInfo.UserName).removeClass('hide');
						//手机号
						$('#phone_number').html(BasicInfo.Mobile.substr(0,3)+'****'+BasicInfo.Mobile.substr(7,10)).parents('.hasPhone').removeClass('hide');
					}else{
						$('#UserName1').html(BasicInfo.NickName).removeClass('hide');
						$('.basicInfo .forWeChat').removeClass('hide').siblings('.forPc').addClass('hide');
						//如果手机号存在，显示手机号和更改按钮。
						if($.trim(BasicInfo.Mobile)){
							$('.basicInfo .hasPhone').removeClass('hide').find('#phone_number').html(BasicInfo.Mobile.substr(0,3)+'****'+BasicInfo.Mobile.substr(7,10));
						}else{
							$('.basicInfo .noPhone').removeClass('hide').siblings('.codeYa').removeClass('hide');
							$('#vertifyCodeYa').attr('src',public_url+'/CheckCode.aspx?r='+Math.random());
						}
					}
					if(BasicInfo.ProvinceID && BasicInfo.ProvinceID != '-2'){
						$('#province option').each(function(i,item){
							if($(item).attr('value') == BasicInfo.ProvinceID){
								$(item).prop('selected','true');
								$('#province').change();
								if(BasicInfo.CityID != '-2'){
									$('#city option').each(function(j,single){
										if($(single).attr('value') == BasicInfo.CityID){
											$(single).prop('selected',true);
										}
									})
								}
							}
						})
					}
					//地址
					if(BasicInfo.Address){
						$('#address').val(BasicInfo.Address);
					}

					/**认证信息**/
					if(AuthenticationInfo){
						var $authInfo = $('.authInfo'),
							$detailInfo = $authInfo.find('.after'),
							$beforeInfo = $authInfo.find('.before');
						if(AuthenticationInfo.Status == 0){//未认证
							$authInfo.find('.after .authRetrunInfo').addClass('hide');
						}else if(AuthenticationInfo.Status == 1){//待审核
							$authInfo.find('.after .submit_success').removeClass('hide').siblings('.authRetrunInfo').addClass('hide');
						}else if(AuthenticationInfo.Status == 2){//已认证-认证通过
							$authInfo.find('.after .success_auth').removeClass('hide').siblings('.authRetrunInfo').addClass('hide');
						}else if(AuthenticationInfo.Status == 3){//认证未通过
							$authInfo.find('#reject_reason').html(AuthenticationInfo.Reason);//显示驳回原因
							$authInfo.find('.after .fail_auth').removeClass('hide').siblings('.authRetrunInfo').addClass('hide');
							$authInfo.find('.after .fail_auth').removeClass('hide');
						}
						if(AuthenticationInfo.Type == '1001'){//企业
							// $beforeInfo.find('.tr100').html('<span class="red">*</span>公司名称：');
							// $beforeInfo.find('.identify_number .tr100').html('<span class="red">*</span>营业执照：');
							// $('#identityType').find('input').eq(0).prop('checked',true);//默认选中企业类型
							$('.radio_type').find('input[type=radio]').eq(0).click();
							if(AuthenticationInfo.BLicenceURL){//如果存在营业执照，说明认证过，不可编辑
								$detailInfo.find('.identify_number').parents('ul').addClass('hide');
								$detailInfo.find('.radio_type').html('企业');
								$detailInfo.find('.business_license li').eq(0).html('营业执照：');
								$detailInfo.find('.name_type').html('公司名称：');
								$detailInfo.find('.company_name').html(AuthenticationInfo.TrueName);
								$detailInfo.find('.img_show').attr('src',AuthenticationInfo.BLicenceURL);
								$authInfo.find('.before').addClass('hide').next().removeClass('hide');
								if($('.list_switch .selected').attr('index') == '1'){
									$('#submitInfo').hide();
								}else{
									$('#submitInfo').show();
								}
							}
						}else{
							// $('#identityType').find('input').eq(1).prop('checked',true);//默认选中个人类型
							// $('#nameInfo').parents('ul').find('.tr100').html('<span class="red">*</span>真实姓名：');
							// $('#loadFile').parents('ul').find('.tr100').html('<span class="red">*</span>手持身份证：');
							// $('#identify_number').parents('.identify_number').removeClass('hide');
							$('.radio_type').find('input[type=radio]').eq(1).click();
							if(AuthenticationInfo.IDCardFrontURL){//如果存在身份证，说明个人认证过，不可编辑
								$detailInfo.find('.radio_type').html('个人');
								$detailInfo.find('.business_license li').eq(0).html('手持身份证：');
								$detailInfo.find('.name_type').html('真实姓名：');
								$detailInfo.find('.company_name').html(AuthenticationInfo.TrueName);
								$detailInfo.find('.img_show').attr('src',AuthenticationInfo.IDCardFrontURL);
								$detailInfo.find('.identify_number').html(AuthenticationInfo.IdentityNo);
								$detailInfo.find('.identify_number').parents('ul').removeClass('hide');
								$authInfo.find('.before').addClass('hide').next().removeClass('hide');
								if($('.list_switch .selected').attr('index') == '1'){
									$('#submitInfo').hide();
								}else{
									$('#submitInfo').show();
								}
							}
						}
						$('.reAuth').off('click').on('click',function(){
					    	$('.authInfo').find('before').removeClass('hide').next('.after').addClass('hide');
					    	$('#submitInfo').show();
					    })
						
					}

					/**提现账号**/
					if(BasicInfo.Category == '29002'){//媒体主有提现账号
						if(BankAccountInfo && BankAccountInfo.AccountType == '96001'){//支付宝类型
							if(BankAccountInfo.AccountName){
								$('.account').find('.after').removeClass('hide').end().find('.before').addClass('hide');
								$('#alipayAccount_m').html(BankAccountInfo.AccountName);
								if($('.list_switch .selected').attr('index') == '2'){
									$('#submitInfo').hide();
								}else{
									$('#submitInfo').show();
								}
							}else{
								$('.account').find('.before').removeClass('hide').end().find('.after').addClass('hide');
							}
						}else{
							$('.account').find('.before').removeClass('hide').end().find('.after').addClass('hide');
						}
					}else{
						$('.list_switch').find('li[index=2]').remove();
					}

					/*修改密码*/
					//判断是否有密码，若有--正常显示
					//没有密码：判断用户注册方式，若是PC，正常显示修改密码功能，
					//若是微信，判断是否绑定手机号，未绑定，提示绑定手机号，已绑定，显示创建密码
					setAjax({
						url:public_url+'/api/UserMange/VerifyPassword',
						type:'post',
						data:{}
					},function(data){
						if(data.Status == 0){//没有密码，需要创建密码
							if(BasicInfo.RegisterType == '199001'){//PC注册
								$('.passWordInfo .hasPsd').removeClass('hide').siblings('').addClass('hide');
							}else if(BasicInfo.RegisterType == '199002'){//微信注册
								if( !$.trim(BasicInfo.Mobile)){
									$('.passWordInfo .noPsd').removeClass('hide').siblings('').addClass('hide');
									$('#bindMoble').off('click').on('click',function(){
										$('.list_switch').find('li').eq(0).click();
									})
								}else{
									$('.passWordInfo .createPsd').removeClass('hide').siblings('').addClass('hide');
								}
							}
						}else if(data.Status == '50010'){//用户信息不存在
							console.log('用户信息不存在')
						}else if(data.Status == '50011'){//可以修改密码
							$('.passWordInfo .hasPsd').removeClass('hide').siblings('').addClass('hide');
						}
					})
					_self.operate();
				}
			})
		},
		hideTipInfo:function(ele){
			$(ele).parents('ul').find('.tipInfo').html('').hide();
		},
		confimBindUser:function(callback){//验证信息并判断是否合并等
			var _this = $('#codeForWeChat1'),
	    		codeForWeChat1 = $.trim(_this.val()),
	    		$moble = $('#phone_number1'),
	    		curMobile = $.trim($moble.val()),
	    		$code = $('#codeForWeChat'),
	    		errorMsgYa = [];
	    	//验证手机号
	    	if(!curMobile){
	    		$moble.parents('ul').find('.tipInfo').html('<img src="/images/icon7.png"> 请输入手机号！').show();
	    		errorMsgYa.push('1');
	    	}else if(curMobile.length != 11){
	    		$moble.parents('ul').find('.tipInfo').html('<img src="/images/icon7.png"> 请输入正确的手机号！').show();
	    		errorMsgYa.push('1');
	    	}
	    	//验证码
	    	if(!$.trim($code.val())){
	    		$code.parents('ul').find('.tipInfo').html('<img src="/images/icon7.png"> 请输入验证码！').show();
	    		errorMsgYa.push('1');
	    	}
	    	//手机验证码
	    	if( !codeForWeChat1 ){
	    		_this.parents('ul').find('.tipInfo').html('<img src="/images/icon7.png"> 请输入手机验证码！').show();
	    		errorMsgYa.push('1');
	    	}else if(codeForWeChat1.length != 4){
	    		_this.parents('ul').find('.tipInfo').html('<img src="/images/icon7.png"> 请输入正确的手机验证码！').show();
	    		errorMsgYa.push('1');
	    	}
	    	console.log(errorMsgYa.length);
	    	//如果手机验证码是4位且用户输入了手机号和验证码，调接口验证手机验证码是否正确，如果正确，调接口判断是否需合并信息
	    	if( !errorMsgYa.length){
	    		//验证短信验证码
	    		$.post(public_url+'/AjaxServers/LoginManager.ashx', {
                    action: 'checksmscode',
                    mobile: curMobile,
                    mobileCheckCode:codeForWeChat1, 
                    r: Math.random()
                }, function(data){
                	var returnData = $.evalJSON(data);
                	console.log($.evalJSON(data));

                    if(returnData.result == 0){//验证成功--判断是否需要合并信息
                    	$.ajax({
                    		url:public_url+'/api/UserMange/VerifyMobileBind',
							type:'post',
							async: false,
					        dataType: 'json',
					        xhrFields: {
					            withCredentials: true
					        },
					        crossDomain: true,
					        data:{
			    				Mobile:curMobile,
			    				MobileCode:codeForWeChat1
			    			},
			    			success:function(data1){
			    				if(data1.Status == 0){//验证通过，可正常提交
			    					callback && callback();
				    			}else if(data1.Status == 50001){//验证码错误
				    				$code.parents('ul').find('.tipInfo').html('<img src="/images/icon7.png"> 验证码不正确，请重新输入！').show();
				    			}else if(data1.Status == 50002){//手机验证码不正确
				    				_this.parents('ul').find('.tipInfo').html('<img src="/images/icon7.png"> 手机验证码不正确，请重新输入！').show();
				    			}else if(data1.Status == 60001){//手机号已注册且已绑定微信号
				    				$moble.parents('ul').find('.tipInfo').html('<img src="/images/icon7.png"> 该手机号已注册，请重新输入！').show();
				    			}else if(data1.Status == 60002){
				    				layer.confirm('该手机号已经注册，是否做信息合并？', {
									  	btn: ['修改手机号','合并信息'] //按钮
									}, function(index){
									  	layer.close(index);
									}, function(){//合并信息
										$.ajax({
											url:public_url+'/api/UserMange/BindUser',
											type:'post',
											async: false,
									        dataType: 'json',
									        xhrFields: {
									            withCredentials: true
									        },
									        crossDomain: true,
									        data:{
												Mobile:curMobile,
												MobileCode:codeForWeChat1
											},
											success:function(response){
												if(response.Status == 0){///验证通过
													layer.msg('合并信息成功',{time:2000});
													location.reload();
												}else{
													layer.msg('合并失败');
													console.log('合并失败');
												}
											}
										})
									});
				    			}
			    			}
                    	})
                    }else{
                    	errorMsgYa.push(returnData.msg);
                    	layer.msg(returnData.msg,{time:2000});
                    	console.log(errorMsgYa);
                    }
                });
	    	}else{
	    		console.log(errorMsgYa);
	    	}
		},
		limitNumber:function(ele){//限制只能输入数字
			//获取光标位置
		    function getCursorPos(obj) {
		        var CaretPos = 0;
		        // IE Support
		        if (document.selection) {
		            obj.focus (); //获取光标位置函数
		            var Sel = document.selection.createRange ();
		            Sel.moveStart ('character', -obj.value.length);
		            CaretPos = Sel.text.length;
		        }
		        // Firefox/Safari/Chrome/Opera support
		        else if (obj.selectionStart || obj.selectionStart == '0')
		            CaretPos = obj.selectionEnd;
		        return (CaretPos);
		    };
		    //定位光标
		    function setCursorPos(obj,pos){
		        if (obj.setSelectionRange) { //Firefox/Safari/Chrome/Opera
		            obj.focus(); //
		            obj.setSelectionRange(pos,pos);
		        } else if (obj.createTextRange) { // IE
		            var range = obj.createTextRange();
		            range.collapse(true);
		            range.moveEnd('character', pos);
		            range.moveStart('character', pos);
		            range.select();
		        }
		    };
		    function replaceAndSetPos(obj,pattern,text){
		        if ($(obj).val() == "" || $(obj).val() == null) {
		            return;
		        }
		        var pos=getCursorPos(obj);//保存原始光标位置
		        var temp=$(obj).val(); //保存原始值
		        obj.value=temp.replace(pattern,text);//替换掉非法值
		        //截掉超过长度限制的字串（此方法要求已设定元素的maxlength属性值）
		        var max_length = obj.getAttribute? parseInt(obj.getAttribute("maxlength")) : "";
		        if( obj.value.length > max_length){
		            var str1 = obj.value.substring( 0,pos-1 );
		            var str2 = obj.value.substring( pos,max_length+1 );
		            obj.value = str1 + str2;
		        }
		        pos=pos-(temp.length-obj.value.length);//当前光标位置
		        setCursorPos(obj,pos);//设置光标
		        //el.onkeydown = null;
		    };
		    $(ele).on("input",function(){replaceAndSetPos(this,/[^0-9]/g,'');});
		    $(ele).on("keyup",function(){replaceAndSetPos(this,/[^0-9]/g,'');})
		},
		uploadImg:function(id) {
            jQuery.extend({
                 evalJSON: function (strJson) {
                     if ($.trim(strJson) == '')
                         return '';
                     else
                         return eval("(" + strJson + ")");
                 }
            });
            function getCookie(name) {
                var arr, reg = new RegExp("(^| )" + name + "=([^;]*)(;|$)");
                if (arr = document.cookie.match(reg))
                    return unescape(arr[2]);
                else
                    return null;
            }
            function escapeStr(str) {
                return escape(str).replace(/\+/g, '%2B').replace(/\"/g, '%22').replace(/\'/g, '%27').replace(/\//g, '%2F');
            }
            
            $(document).ready(function () {
                $('#'+id).uploadify({
                    "buttonText": "",
                    "buttonClass": "but_upload",
                    "swf": "/Js/uploadify.swf?_=" + Math.random(),
                    "uploader": "/AjaxServers/UploadFile.ashx",
                    "auto": true,
                    "multi": false,
                    // "position":"absolute",
                    // "top":-50,
                    "width":75,
                    "height": 74,
                    "formData": { Action: "BatchImport", CarType: "", LoginCookiesContent: escapeStr(getCookie("ct-ouinfo")), IsGenSmallImage: 1 },
                    'fileTypeDesc': '支持格式:jpg,jpeg,png',
                    'fileTypeExts': '*.jpg;*.jpeg;*.png;',
                    "queueSizeLimit": 1,
                    "fileSizeLimit": "2MB",
                    "scriptAccess": "always",
                    //"overrideEvents": ["onDialogClose"],
                    "queueID": "imgShow",
                    "onUploadStart": function (file) {
                        if ("*.xls;*.xlsx;*.jpg;*.jpeg;*.png;*.zip;*.mp4;*.pdf;*.ppt;*.pptx;".indexOf(file.type) <= 0) {
                            layer.alert("您选择的文件不是有效的格式!\r\n请选择以下格式jpg,jpeg,png;");
                        }
                    },
                    "onQueueComplete": function (event, data) {
                        //enableConfirmBtn();
                    },
                    "onQueueFull": function () {
                        layer.alert("您最多只能上传1个文件！");
                        return false;
                    },
                    'onFallback':function(){
                        console.log('您未安装FLASH控件，无法上传图片！');
                        return false;
                    },
                    "onUploadSuccess": function (file, data, response) {
                        if (response == true) {
                            var json = $.evalJSON(data);
                            $('#loadFile').attr('src',json.Msg);
                            $('.display_file_name').show();
                            $('#downloadIMg').attr('href',json.Msg);
                            $('.file_name').html('<img src="/images/icon9.png">&nbsp;&nbsp;'+ json.FileName);
                            $('#uploadImg').parents('ul').find('.tipInfo').html('');
                        }
                    },
                    "onProgress": function (event, queueID, fileObj, data) { },
                    "onUploadError": function (event, queueID, fileObj, errorObj) {
                        //enableConfirmBtn();
                    },
                    'onSelectError':function(file, errorCode, errorMsg){
                        if(errorCode == '-110'){
                        	layer.msg('文件大小需小于2M',{time:2000});
                        }
                        if (errorCode == SWFUpload.UPLOAD_ERROR.FILE_CANCELLED
                            || errorCode == SWFUpload.UPLOAD_ERROR.UPLOAD_STOPPED) {
                            return false;
                        }
                    }
                });
            });
        },
        getRequest:function(){
        	var url = location.search; //获取url中"?"符后的字串
	        var theRequest = new Object();
	        if (url.indexOf("?") != -1) {
	            var str = url.substr(1);
	            strs = str.split("&");
	            for (var i = 0; i < strs.length; i++) {
	                theRequest[strs[i].split("=")[0]] = unescape(strs[i].split("=")[1]);
	            }
	        }
	        return theRequest;
        }
	}
	new AccountManage();
})