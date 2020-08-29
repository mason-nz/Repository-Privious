/*
* Written by:     wangc
* function:       账户余额
* Created Date:   2018-01-18
* Modified Date:
*/
function GetCash(){
	this.initData();
}
var userInfo = {
		Status : 0,//认证状态，默认未认证
        Type : 1002,//用户类型，默认是个人
        mobile:'',//用户手机号
        AccountName : '',//支付宝账号，默认空
        cashLimit:50//可提现金额限制，满50可提现
	},
	account_info = {};//账户财产信息
GetCash.prototype = {
	constructor:GetCash,
	initData:function(){//初始化页面数据及样式
		var _self = this;
		wx.ready(function () {
			wx.getNetworkType({
				success: function (res) {
					var networkType = res.networkType; // 返回网络类型2g，3g，4g，wifi
					$('.netOk').show().siblings('.netNo').remove();
				},
				fail:function(){
					$('.netNo').show().siblings('.netOk').remove();
					return false;
				}
			});
		})
		//获取收入详情--统计信息
		$.ajax({
			url:public_url+'/api/Withdrawals/GetIncomeInfo',
			type:'get',
			xhrFields: {
                withCredentials: true
            },
            crossDomain: true,
			data:{
				r:Math.random()
			},
			success:function(data){
				account_info = {
					canCash:data.Result.CanWithdrawalsMoney,		    //余额
					totalEarning:data.Result.EarningsPrice,             //累计收益
					alreadyMoney:data.Result.AlreadyWithdrawalsMoney,	//已提现
					withdrawalsMoney:data.Result.WithdrawalsMoneyIng    //提现中
				};
				$('.total_count').html(formatMoney(account_info.canCash,2,''));
				$('#total_receive').html(formatMoney(account_info.totalEarning,2,''));
				$('.already_money').html(formatMoney(account_info.alreadyMoney,2,''));
				$('.precess_money').html(formatMoney(account_info.withdrawalsMoney,2,''));
			}
		})
		_self.getBaseInfo();
		//查询用户基本信息，验证用户是否可提现
		
	},
	operate:function(){
		var _self = this;
		$('#getCach').off('click').on('click',function(){
			var _this = $(this);
			if(_this.attr('class') == 'button_grey'){
				return
			}
			if(userInfo.Status == 0){//未认证
            	layer.open({
		            content: '为了保证提现安全<br/>请先完善个人信息'
		            ,btn:['去完善','取消']
		            ,shade: 'background-color: rgba(0,0,0,.3)'
		            ,shadeClose:false                      
		            ,yes: function(index){  
		            	//跳转到个人信息页面        
		                window.location = public_pre+'/userManager/userInfo.html';
		            },no:function(index){                   
		                layer.close(index)
		            }
		        });
		        return
            }else if(userInfo.Status == 1){//待审核
            	layer.open({
		            content: '您的个人信息还在审核中<br/>审核通过后才可提现'
		            ,btn:'好的'
		        });
		        return
            }else if(userInfo.Status == 3){//认证未通过
		        layer.open({
		            content: '您的个人信息未审核通过<br/>不可提现'
		            ,btn:['修改个人信息','取消']
		            ,shade: 'background-color: rgba(0,0,0,.3)'
		            ,shadeClose:false                      
		            ,yes: function(index){          
		                //跳转到个人信息页面，页面顶部显示审核未通过原因      
		                window.location = public_pre+'/userManager/userInfo.html';
		            },no:function(index){                   
		                layer.close(index);
		            }
		        });
		        return
            }

            //认证成功--验证提现账号
            if( !userInfo.AccountName ){
                layer.open({
		            content: '请先完善提现账号'
		            ,btn:['去完善','取消']
		            ,shade: 'background-color: rgba(0,0,0,.3)'
		            ,shadeClose:false                      
		            ,yes: function(index){          
		                //跳转到个人信息页面        
		                window.location = public_pre+'/userManager/userInfo.html';
		            },no:function(index){                   
		                layer.close(index);
		            }
		        });
		        return
            }else{
            	_self.judgeFormReturn();
            }

	    })
	    //页面跳转
	    //提现列表
	    $('.withdrawalList').off('click').on('click',function(){
	    	window.location = public_pre+'/cashManager/withdrawalsList.html'
	    })
	    //收益列表
	    $('.profitList').off('click').on('click',function(){
	    	window.location = public_pre+'/cashManager/profitList.html'
	    })
	    //提现规则
	    $('.rule').off('click').on('click',function(){
	    	window.location = public_pre+'/cashManager/rule.html'
	    })
	},
	getBaseInfo:function(){//根据基本信息验证用户是否可提现
		var _self = this;
		$.ajax({
			url:public_url+'/api/UserManage/QueryUserBasicInfo',
			type:'get',
			xhrFields: {
                withCredentials: true
            },
            crossDomain: true,
			data:{
				r:Math.random()
			},
			beforesend:function(){
				$('#getCach').removeClass('button_blue').addClass('button_grey');
			},
			success:function(data){
				// $('#getCach').removeClass('button_grey').addClass('button_blue');
				if(data.Status == 0){
					if(data.Result.AuthenticationInfo){
                        if(data.Result.AuthenticationInfo.Status){
                            userInfo.Status = data.Result.AuthenticationInfo.Status;//保存认证状态
                        }
                        if(data.Result.AuthenticationInfo.Type){
                            userInfo.Type = data.Result.AuthenticationInfo.Type;//保存用户类型
                        }
                    }
                    if(data.Result.BankAccountInfo && data.Result.BankAccountInfo.AccountName){
                        userInfo.AccountName = data.Result.BankAccountInfo.AccountName;//保存支付宝账号
                    }
                    if(data.Result.BasicInfo && data.Result.BasicInfo.Mobile){
                    	userInfo.mobile = data.Result.BasicInfo.Mobile;//保存手机号
                    }

                    //企业不可提现
                    if(userInfo.Type == 1001){
						$('.tipInfo').html('<li>企业用户提现需联系客服 <a href="tel:400-0100000" class="blue_link">400-0100000</a></li>').show();
						$('#getCach').removeClass('button_blue').addClass('button_grey');
					}else{
						//如果信息完善
						if(userInfo.mobile && userInfo.AccountName && userInfo.Status==2){
							//个人可提现金额需>=50
							if(account_info.canCash < userInfo.cashLimit){
								$('.tipInfo').html('<li>账户余额满'+userInfo.cashLimit+'元方可申请提现</li>').show();
								$('#getCach').removeClass('button_blue').addClass('button_grey');
							}else{
								$('#getCach').removeClass('button_grey').addClass('button_blue');
								$('.tipInfo').remove();
							}
						}else{//信息不完善
							$('.tipInfo').html('<li>个人信息完善且审核通过后，才能提现哦！查看<a href="/userManager/userInfo.html" class="blue">个人信息</a></li>').show();
							$('#getCach').removeClass('button_blue').addClass('button_grey');
						}
					}
                    _self.operate();
 				}else{
					layer.open({
						content:data.Message,
						time:2,
						skin: 'msg'
					})
				}
			}
		})
	},
	judgeFormReturn:function(){//根据后台返回结果验证用户是否可提现，若不可提现，不跳页面
		var _self = this;
		if($('#getCach').attr('class')!= 'button_blue'){
			return
		}
        $.ajax({
        	url:public_url+'/api/Withdrawals/VerifyWithdrawalsClick',
            type:'post',
            xhrFields: {
                withCredentials: true
            },
            crossDomain: true,
            data:{
            	r:Math.random()
            },
            beforesend:function(){
				$('#getCach').removeClass('button_blue').addClass('button_grey');
			},
            success:function(data){
            	$('#getCach').removeClass('button_grey').addClass('button_blue');
            	if(data.Status == 0){
                	window.location = public_pre+'/cashManager/getCash.html';
	            }else{
	            	if(data.Status == 1015){
	            		layer.open({
					        content: '可提现金额不足'+userInfo.cashLimit+'元<br/>无法提现！'
					        ,btn:'好的'
					    });
	            	}else if(data.Status == 1011){
					    layer.open({
				            content: '为了保证提现安全<br/>请先完善个人信息'
				            ,btn:['去完善','取消']
				            ,shade: 'background-color: rgba(0,0,0,.3)'
				            ,shadeClose:false                      
				            ,yes: function(index){   
				            	//跳转到个人信息页面       
				                window.location = public_pre+'/userManager/userInfo.html';
				            },no:function(index){                   
				                layer.close(index);
				            }
				        });
					}else if(data.Status == 1012){
					    layer.open({
				            content: '您的个人信息未审核通过<br/>不可提现'
				            ,btn:['修改个人信息','取消']
				            ,shade: 'background-color: rgba(0,0,0,.3)'
				            ,shadeClose:false                      
				            ,yes: function(index){          
				                //跳转到个人信息页面，页面顶部显示审核未通过原因      
				                window.location = public_pre+'/userManager/userInfo.html';
				            },no:function(index){                   
				                layer.close(index);
				            }
				        }); 
					}else if(data.Status == 1013){
					    layer.open({
					        content: '请先完善提现账号'
					        ,btn:['去完善','取消']
					        ,shade: 'background-color: rgba(0,0,0,.3)'
					        ,shadeClose:false                      
					        ,yes: function(index){          
					            window.location = public_pre+'/userManager/userInfo.html';
					        },no:function(index){                   
					            layer.close(index)
					        }
					    });
					}else if(data.Status == 1016){
					    layer.open({
					        content: '每天只能提现一次<br/>请明天再试！'
					        ,btn:'好的'
					    }); 
					}else if(data.Status == 1017){
					    layer.open({
					        content: '您有正在支付中的提现申请<br/>请在提现完成后再申请！'
					        ,btn:'好的'
					    }); 
					}else if(data.Status == 3020){
					    layer.open({
					        content: '可提现金额不足'+userInfo.cashLimit+'元<br/>无法提现！'
					        ,btn:'好的'
					    });
					}else{
					    layer.open({
					        content: '提交失败<br/>请稍后重试！'
					        ,btn:'好的'
					    });
					}
				}
            }
        })
	}
}
var getCash = new GetCash();