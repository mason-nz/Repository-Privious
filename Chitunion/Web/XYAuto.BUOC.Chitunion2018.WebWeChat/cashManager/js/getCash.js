/*
* Written by:     wangc
* function:       余额提现
* Created Date:   2018-01-19
* Modified Date:
*/
function GetCash(){
	this.initData();
}
var userInfo = {
	account_number:'',//支付宝账号
	type:'1001',//用户类型
	canCash:0,//可提现金额
	cashLimit:50
}
GetCash.prototype = {
	constructor:GetCash,
	initData:function(){
		var _self = this;
		if($.trim($('#waitCashMoney').val())){
			$('#getCach').removeClass('button_grey').addClass('button_blue');
		}
		wx.ready(function () {
			//隐藏所有传播类和复制链接菜单
			wx.hideMenuItems({
				menuList: ['menuItem:copyUrl','menuItem:share:appMessage','menuItem:share:timeline','menuItem:share:qq','menuItem:share:weiboApp','menuItem:share:QZone','menuItem:share:facebook']  // 要隐藏的菜单项，只能隐藏“传播类”和“保护类”按钮，所有menu项见附录3
			});
			wx.getNetworkType({
				success: function (res) {
					var networkType = res.networkType; // 返回网络类型2g，3g，4g，wifi
					$('.netOk').show().siblings('.netNo').remove();
				},
				fail:function(){
					$('.netNo').show().siblings('.netOk').remove();
				}
			});
		})
		//获取用户基本信息
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
			success:function(data){
				if(data.Status == 0){
					if(data.Result.BankAccountInfo && data.Result.BankAccountInfo.AccountName){
						userInfo.account_number = data.Result.BankAccountInfo.AccountName;
						userInfo.type = data.Result.AuthenticationInfo.Type;
						$('#account_number').html(userInfo.account_number);
					}
				}else{
					layer.open({
						content:data.Message,
						time:2,
						skin: 'msg'
					})
				}
			}
		})
		//获取用户账户余额等信息--收入管理--详情
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
				userInfo.canCash = data.Result.CanWithdrawalsMoney;//余额
				$('#canCash').html(formatMoney(userInfo.canCash,2,''));
				if(userInfo.canCash > userInfo.cashLimit && userInfo.type == 1002){//个人可提现
					// $('.tipInfo').remove();
				}else{
					$('#getCach').removeClass('button_blue').addClass('button_grey')
				}
				_self.operate();
			}
		})
	},
	operate:function(){
		var _self = this;
		$('#cashAll').off('click').on('click',function(){
			$('#waitCashMoney').val(userInfo.canCash);
			$('#getCach').removeClass('button_grey').addClass('button_blue');
			if(userInfo.type == 1002){
			    $.ajax({
			    	url:public_url+'/api/Withdrawals/WithdrawalsPriceCalc',
					type:'post',
					xhrFields: {
		                withCredentials: true
		            },
		            crossDomain: true,
					data:{
						WithdrawalsPrice:userInfo.canCash,
						r:Math.random()
					},
					success:function(data){
						if(data.Status == 0){
							if(data.Result.IndividualTaxPeice){
								var TaxPeice = data.Result.IndividualTaxPeice,
									PracticalPrice = data.Result.PracticalPrice;
								$('.pageChange').html('<li>代缴个税：'+
					                '<a href="javascript:void(0)" class="red">'+TaxPeice+'元 </a>'+
					                '&nbsp;&nbsp;到账金额：<a href="javascript:void(0)" class="red">'+PracticalPrice+' 元</a>'+
					            '</li>').removeClass('hide').addClass('show');
							}else{
								$('.pageChange').removeClass('show').addClass('hide');
							}
						}
					}
				})
	    	}
		})
		$('#getCach').off('click').on('click',function(){
			if($(this).attr('class') == 'button_grey'){
				return
			}
			var cash = $('#waitCashMoney').val();
			if(cash > userInfo.canCash ){
				layer.open({
			        content: '最多提现金额为'+userInfo.canCash+'元'   //设置弹层内容
			        ,btn:'知道了'
			    });
			}else{
				if(userInfo.type == 1001){//企业
					$('.pageChange').html('<li>请将发票快递到赤兔联盟，地址详情见<a href="/cashManager/rule.html" class="blue_link">提现规则</a></li>').removeClass('hide').addClass('show');
				}else{
					//提现操作
					$.ajax({
						url:public_url+'/api/Withdrawals/WithdrawalsOpt',
						type:'post',
						dataType: 'json',
						xhrFields: {
			                withCredentials: true
			            },
			            crossDomain: true,
						data: {
							WithdrawalsPrice:cash,
							r:Math.random()
						},
						beforeSend:function(){
							$('#getCach').removeClass('button_blue').addClass('button_grey');
						},
						success:function(data){
							$('#getCach').removeClass('button_grey').addClass('button_blue')
							if(data.Status == 0){
								layer.open({
							        content: '提现成功<br/>提现金额会在申请后的7个工作日内到账'   
							        ,btn:['完成','查看详情']
							        ,shadeClose:false                       
							        ,yes: function(index){          
							        	//跳转到提现列表
							            window.location = public_pre+'/cashManager/withdrawalsList.html'
							        },no:function(index){                   
							            //跳转到提现详情
							            window.location = public_pre+'/cashManager/cashDetail.html?WithdrawalsId='+data.Result;
							        }
							    });
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
								        content: '提现金额不足'+userInfo.cashLimit+'元<br/>无法提现！！'
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
		})
	}
}
var getCash = new GetCash();
function onlyNumber(obj){
    //得到第一个字符是否为负号
    var t = obj.value.charAt(0);
    //先把非数字的都替换掉，除了数字和.
    obj.value = obj.value.replace(/[^\d\.]/g,'');
    //必须保证第一个为数字而不是.
    obj.value = obj.value.replace(/^\./g,'');
    //保证只有出现一个.而没有多个.
    obj.value = obj.value.replace(/\.{2,}/g,'.');
    //保证.只出现一次，而不能出现两次以上
    obj.value = obj.value.replace('.','$#$').replace(/\./g,'').replace('$#$','.');
  	//只能输入两位小数
    obj.value = obj.value.replace(/^(\-)*(\d+)\.(\d\d).*$/,'$1$2.$3');
    //如果第一位是负号，则允许添加
    if(t == '-'){
        return;
    }
    if(obj.value.length >=8){
    	obj.value = obj.value.substr(0,8);
    }
    if( !parseFloat(obj.value)){
    	$('#getCach').removeClass('button_blue').addClass('button_grey');
    	$('.pageChange').removeClass('show').addClass('hide');
    }else{
    	$('#getCach').removeClass('button_grey').addClass('button_blue');
    	if(userInfo.type == 1002){
		    $.ajax({
		    	url:public_url+'/api/Withdrawals/WithdrawalsPriceCalc',
				// url:'./json/money.json',
				// type:'get',
				type:'post',
				xhrFields: {
	                withCredentials: true
	            },
	            crossDomain: true,
				data:{
					WithdrawalsPrice:obj.value,
					r:Math.random()
				},
				success:function(data){
					if(data.Status == 0){
						if(data.Result.IndividualTaxPeice){
							var TaxPeice = data.Result.IndividualTaxPeice,
								PracticalPrice = data.Result.PracticalPrice;
							$('.pageChange').html('<li>代缴个税：'+
				                '<a href="javascript:void(0)" class="red">'+TaxPeice+'元 </a>'+
				                '&nbsp;&nbsp;到账金额：<a href="javascript:void(0)" class="red">'+PracticalPrice+' 元</a>'+
				            '</li>').removeClass('hide').addClass('show');
						}else{
							$('.pageChange').removeClass('show').addClass('hide');
						}
					}
				}
			})
    	}
    }

}
