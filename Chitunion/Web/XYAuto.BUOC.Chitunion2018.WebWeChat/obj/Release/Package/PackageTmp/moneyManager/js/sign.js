/*
* Written by:     fengbo
* function:       签到页面
* Created Date:   2018-01-19
* Modified Date:
*/
$(function(){
	
	function EachSign(){

		var SignYear = new Date().Format("yyyy") * 1;
		var SignMonth = new Date().Format("M") * 1;

		//初始化参数
		this.cur_obj = {
			SignYear: SignYear,
			SignMonth: SignMonth,
			r: Math.random()
		}
		
		this.init();
	    this.sign();
	    this.into();
	}

	//初始化日期
	EachSign.prototype.init = function() {
		var _this = this;
		
		//判断活动是否结束
		$.ajax({
			url: public_url + '/api/Sign/IsValidActivity',
			type: 'get',
			xhrFields: {
		        withCredentials: true
		    },
		    crossDomain: true,
			data: {
				ActivityType: 2,
				r: Math.random()
			},
			success: function(data){
				if(data.Status == 0){
					var Result = data.Result;
					if(Result){
						$('.sign_container').show();
						$('.noTime').hide();
					}else{
						$('.sign_container').hide();
						$('.noTime').show();
					}
				}else{
					layer.open({
					    content: data.Message,
					    skin: 'msg',
					    time: 2 //2秒后自动关闭
					});
				}
			}
		})

		//初始化layer
		laydate.render({
		  	elem: '#sign_con',
		  	position: 'static',
		  	theme: 'person_date',
		  	showBottom: false,
		  	istoday: false,
		  	ready: function(){
				_this.reloadMsg(_this.cur_obj);	
			},
		  	change: function(value, date){//监听日期月份被切换
			    var obj = {
			    	SignYear: date.year,
					SignMonth: date.month,
					r: Math.random()
			    }
			    _this.reloadMsg(obj);
			}
		})
		
	}

	//点击签到
	EachSign.prototype.sign = function(){
		var _this = this;

		$('#go_sign').off('click').on('click',function(){
			var that = $(this);
			var isSign = that.attr('isSign');
			
			wx.ready(function () {
				wx.getNetworkType({
					success: function (res) {
						var networkType = res.networkType; // 返回网络类型2g，3g，4g，wifi
						
						//没有签到的时候
						if(isSign == 'false'){
							that.attr('isSign','true');
							that.addClass('no_sign');
							$.ajax({
								url: public_url + '/api/Sign/DaySign',
								type: 'post',
								xhrFields: {
							        withCredentials: true
							    },
							    crossDomain: true,
								data: {
									r: Math.random()
								},
                                async:false,
								success: function(data){
									if(data.Status == 0){
										var Result = data.Result;
										_this.reloadMsg(_this.cur_obj);
										layer.open({
										    content: '签到成功，+' + Result + '元',
										    skin: 'msg',
										    time: 2 //2秒后自动关闭
										});
									}else{
										layer.open({
										    content: data.Message,
										    skin: 'msg',
										    time: 2 //2秒后自动关闭
										});
									}
								}
							})
						}else{
							return;
						}						
					},
					fail: function (res) {
				        layer.open({
						    content: '没有网络不可以签到哦',
						    skin: 'msg',
						    time: 2 //2秒后自动关闭
						});
				    }
				})
			})
		})
	}


	//查询基本信息 余额 今天是否签到
	EachSign.prototype.reloadMsg = function(obj){
		var _this = this;
		$.ajax({
			url: public_url + '/api/Sign/DaySignList',
			type: 'get',
			xhrFields: {
		        withCredentials: true
		    },
		    crossDomain: true,
			data: obj,
			async: false,
			success : function(data){
				
				if(data.Status == 0){

					var today = new Date().Format("yyyy-M-d");//今天
					var Result = data.Result;
					var SignDayList = Result.SignDayList;
					var IsSign = Result.IsSign;
					var TotalPrice = Result.TotalPrice;

					//SignDayList不为null的时候 代表已经有签到的时候
					if(SignDayList){

						//对日历里面已经签到的进行初始化样式设置 
						for(var i = 0; i <= SignDayList.length - 1; i++){

							var al_date = SignDayList[i];

							$('.layui-laydate-content table tbody td').each(function(){
								var that = $(this);
								var date = that.attr('lay-ymd');
								var number = that.text();
								var _class = that.attr('class');
								that.removeClass('layui-this');

								//已经签到的样式
								if(al_date == date){
									that.html("<span class='alreday_sign'>"+ number +"</span>");
								}
							})

						}

					}

					//判断今天是否签到  已经签到了
					if(IsSign == true){
						$('#go_sign').addClass('no_sign');
						$('#go_sign').removeClass('sign_icon');
						$('#go_sign').attr('isSign','true');
						$('#go_sign').html('已签到');
					}else{
						$('#go_sign').addClass('sign_icon');
						$('#go_sign').removeClass('no_sign');
						$('#go_sign').attr('isSign','false');
						$('#go_sign').html('签到');
					}

					//永远初始化今天的日期 选中样式
					$('.layui-laydate-content table tbody td').each(function(){
						var that = $(this);
						var date = that.attr('lay-ymd');
						var number = that.text();
						that.removeClass('layui-this');
						
						//今天
						if(today == date){
							that.html("<span class='cur_this'>"+ number +"</span>");
						}
					})

					$('#TotalPrice').html(formatMoney(TotalPrice,2,''));
				}else{
					layer.open({
					    content: data.Message,
					    skin: 'msg',
					    time: 2 //2秒后自动关闭
					});
				}
			}
		})
	}


	//跳转页面
	EachSign.prototype.into = function(){

		$('#tit a').eq(0).on('click',function(){
			window.location = public_pre + '/cashManager/rule.html';
		})

		$('#tit a').eq(1).on('click',function(){
			window.location = public_pre + '/cashManager/profitList.html';
		})

	}

	var sign = new EachSign();

})


