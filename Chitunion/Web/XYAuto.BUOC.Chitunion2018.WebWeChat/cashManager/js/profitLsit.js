/*
* Written by:     wangc
* function:       收益记录列表
* Created Date:   2018-02-05
* Modified Date:
*/
$(function(){
	var parameter = {
		RowNum:0,
		TopCount:10,
		r:Math.random()
	};
	var app = new Vue({
		el: '#app',
		data:{
			list: []
		},
		created:function(){
			this.checkNet();
		},
		methods:{
			checkNet:function(){//根据网络状态显示内容
				var that = this;
				wx.ready(function () {
					wx.getNetworkType({
						success: function (res) {
							var networkType = res.networkType; // 返回网络类型2g，3g，4g，wifi
							that.initList(parameter);
							$('.netOk').show().siblings('.netNo').remove();
						},
						fail:function(){
							$('.netNo').show().siblings('.netOk').remove();
						}

					});
				})
			},
			initList:function(parameter){//获取收益列表
				var that = this;
				$.ajax({
					// url:'json/list1.json',
					url:public_url+'/api/Profit/GetProfitList',
					type:'get',
					xhrFields: {
		                withCredentials: true
		            },
		            crossDomain: true,
					data:{
						RowNum:0,
						TopCount:10,
						r:Math.random()
					},
					beforesend:function(){
						layer.open({
						    type: 2
						    ,content: '加载中',
						    shadeClose:false
						});
					},
					success:function(response){
						layer.closeAll();
						if(response.Status == 0 && response.Result.TotalCount){
							if(response.Result.TotalCount == 0){
								$('.netOk .no_data').show();
								$('.no_data .text_blue').off('click').on('click',function(){
									window.location = public_pre+'/moneyManager/make_money.html';
								})
							}else{
								if(response.Result.TotalCount > 10){
									that.ReloadList();
								}else{
									that.list = response.Result.ProfitList;
									that.$nextTick(function () {
										$('.profit_list').find('.single_row:last').removeClass('border-bottom');
									})
								}
							}
						}else{
							$('.netOk .no_data').show();
							$('.no_data .text_blue').off('click').on('click',function(){
								window.location = public_pre+'/moneyManager/make_money.html';
							})
						}
					},
					error:function(response) {
						layer.closeAll();
						layer.open({
							content:JSON.stringify(response),
							time:2,
							skin: 'msg'
						})
					}
				})
			},
			ReloadList:function(){// 列表下拉上拉刷新 加载
				var dropload = $('.netOk').dropload({
					scrollArea : window,
			        domUp : {//上方dom
			            domClass   : 'dropload-up',
			            domRefresh : '<div class="dropload-refresh">↓下拉刷新</div>',
			            domUpdate  : '<div class="dropload-update">↑释放更新</div>',
			            domLoad    : '<div class="dropload-load"><span class="loading"></span>加载中...</div>'
			        },
			        domDown : {//下方dom
			            domClass   : 'dropload-down',
			            domRefresh : '<div class="dropload-refresh">↑上拉加载更多</div>',
			            domLoad    : '<div class="dropload-load"><span class="loading"></span>加载中...</div>',
			            domNoData  : '<div class="dropload-noData">-- END --</div>'
			        },
			        loadDownFn : function(me){//下滑动作
					    parameter.RowNum = $('.profit_list').find('.single_row:last').attr('RowNum') || 0;
			            // 拼接HTML
			            var result = '';
			            $.ajax({
			            	// url:'json/list1.json',
			            	url:public_url+'/api/Profit/GetProfitList',
			                type: 'get',
			                xhrFields: {
				                withCredentials: true
				            },
				            crossDomain: true,
			                data: parameter,
			                success: function(data){
			                    if(data.Status == 0 && data.Result.ProfitList){
			                    	var arrLen = data.Result.ProfitList;
				                    if(arrLen.length > 0){
				                        for(var i = 0; i< arrLen.length; i++){
											result += '<div>';
												var length = $('.profit_list').find('.year').length;
												$('.profit_list').find('.year').each(function(index,one){
													console.log(index,one);
													//如果不存在  length--
													if($.trim($(one).find('span').html()) != arrLen[i].ProfitDate.replace(/-/g,'.')){
														length -- ;
													}
												});
												//如果日期不存在，追加日期，否则，直接追加内容
												if(length == 0){
													result += 
														'<div class="year">'+
															'<img src="../images/year.png">'+
		                    								'<span>'+arrLen[i].ProfitDate.replace(/-/g,'.')+'</span>'+
		                    							'</div>';
												}
                    							arrLen[i].DateList.forEach(function(item){
                    								result += 
													'<div class="single_row border-bottom profit" RowNum='+item.RowNum+'>'+
				                    					'<figure>'+
				                    						'<figcaption>'+
				                    							'<p class="pay_status">';
					                    							if(item.ProfitType == 103001){
					                    								result+='订单收益</p><p class="pay_time">'+item.ProfitDescribe+'</p>';
					                    							}else if(item.ProfitType == 103003){
					                    								result+='签到</p><p class="pay_time">'+item.ProfitDescribe+'</p>';
					                    							}else{
					                    								if(!item.Headimgurl){
					                    									item.Headimgurl = '/images/pic_default.png';
					                    								}
					                    								result+='邀请好友</p><p class="pay_time"><img src='+item.Headimgurl+'><span>'+item.Nickname+'</span></p>';
					                    							}
				                    							result+='</p>'+
							                    				'<div class="clear"></div>'+
							                    			'</figcaption>'+
							                    		'</figure>'+
							                    		'<div class="money_date">'+
							                    			'<p class="receive_money red">'+
							                    				'<span class="money">'+ '+'+formatMoney(item.ProfitPrice,2,'') + '</span>元'+
							                    			'</p>'+
							                    			'<p class="date">';
							                    			if(item.ProfitType == 103001){
							                    				result += '阅读次数：'+	item.ReadCount+'次';
							                    			}else if(item.ProfitType == 103003){
							                    				result += '签到时间：'+item.IncomeTime.substr(11,5);
							                    			}else{
							                    				result += '领取红包时间：'+item.IncomeTime.substr(11,5);
							                    			}
							                    			result += '</p>'+
							                    		'</div><div class="clear"></div>'+
							                    	'</div>';
                    								
                    							})
						                    result += '</div>';
				                        }
				                    }else{// 如果没有数据
				                        // 锁定
				                        me.lock();
				                        // 无数据
				                        me.noData(true);
				                    }
				                    // 为了测试，延迟1秒加载
				                    setTimeout(function(){
				                        // 插入数据到页面，放到最后面
				                        $('.profit_list').append(result);

				                        // .find('.single_row:last').removeClass('border-bottom');
				                        // 每次数据插入，必须重置
				                        me.resetload();
				                    },1000);
			                    }
			                },
			                error: function(xhr, type){
			                    alert('Ajax error!');
			                    // 即使加载出错，也得重置
			                    me.resetload();
			                }
			            });
			        },
			        //threshold : 150
			    });
				
			}
		}
	})
})