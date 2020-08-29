/*
* Written by:     wangc
* function:       邀请好友
* Created Date:   2018-01-18
* Modified Date:
*/
$(function(){
	var parameter ={
		RecID:0,
		TopCount:50,
		r:Math.random()
	};
	
	var app = new Vue({
		el: '#app',
		data:{
			list: [],
			TotalPrice:0.00,
			TotalCount:0,
			ownImgUrl:public_url+'/images/demoIcon.png'//专属二维码地址
		},
		created:function(){
			this.judge();//判断
			// this.initList();
		},
		methods:{
			judge:function(){//判断活动是否在有效期内
			    var that = this;
				$.ajax({
					url:public_url+'/api/Sign/IsValidActivity',
					type:'get',
					data:{
						ActivityType:1
					},
					xhrFields: {
					    withCredentials: true
					},
					crossDomain: true,
					success:function(data){
						if(data.Status == 0){
							if(data.Result == true){
								that.getImg();
								that.initList();//初始化页面
							}else{//活动过期
								$('.netNo').show().siblings('.main_container').remove();
							}
						}
					}
				})
			},
			getImg:function(){
				var that = this;
				//获取专属图片地址
				var imgurl = encodeURI(that.ownImgUrl);
				$.ajax({
			        url:public_url+'/api/WeixinJSSDK/GetInvitationMediaId',
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
			            	that.ownImgUrl = data.Result.InvitationQR+'?r='+Math.random();
			            	imgurl = encodeURI(that.ownImgUrl);
							var option = {
								title: '送你一个赚钱机会',//分享标题
								desc: '加入赤兔联盟，赚钱就是这么简单！',//分享描述
								link: public_url+'/inviteManager/inviteCode.html?imgurl='+imgurl,
								imgUrl: public_url+'/images/share.jpg',//分享图标
								type: 'link', // 分享类型,music、video或link，不填默认为link
								dataUrl: '', // 如果type是music或video，则要提供数据链接，默认为空
								// trigger: function (res) {
								// 	alert('用户点击分享');
								// },
								success: function (res) {
									// alert(JSON.stringify(option));
									$('.layer_pop').hide();
									//分享保存接口
									$.ajax({
										url:public_url+'/api/WechatShare/AddWechatShare',
										type:'post',
										xhrFields: {
							                withCredentials: true
							            },
							            crossDomain: true,
										data:{
											ShareUrl:window.location.href
										},
										success:function(){
											//保存成功
										}
									})
								},
								cancel: function (res) {
									// alert('已取消');
									$('.layer_pop').hide();
								},
								fail: function (res) {
									alert(JSON.stringify(res));
								}
							}
							wx.ready(function () {
								wx.onMenuShareAppMessage(option);
								wx.onMenuShareTimeline(option);
								wx.onMenuShareQQ(option);
								wx.onMenuShareQZone(option);
								wx.onMenuShareWeibo(option);
								wx.hideMenuItems({
									menuList: ['menuItem:copyUrl'] // 要隐藏的菜单项，只能隐藏“传播类”和“保护类”按钮，所有menu项见附录3
								});
							})

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
			initList:function(){//初始化页面
				var that = this;
				$.ajax({
					url:public_url+'/api/Invite/GetBeInvitedList',
					type:'get',
					xhrFields: {
		                withCredentials: true
		            },
		            crossDomain: true,
					data:{
						RecID:0,
						TopCount:parameter.TopCount,
						r:Math.random()
					},
					success:function(response){
						if(response.Status == 0){
							that.TotalPrice = response.Result.TotalPrice;
							that.TotalCount = response.Result.TotalCount;
							if(response.Result.TotalCount > parameter.TopCount){
								$('.content_list').html('');
								that.ReloadList();
							}else{
								that.list = response.Result.BeInvitedList;
							}
							that.$nextTick(function () {
								that.operate();
							})
						}
					}
				})
			},
			operate:function(){//微信原生分享+领红包+邀请好友点击事件
				var that = this;
				//邀请好友
				$('#invite').off('click').on('click',function(){
					//绑定百度统计trackEvent事件逻辑
					var name = $(this).attr('baidu_track_name');
			        var action = $(this).attr('baidu_track_action');
			        var label = $(this).attr('baidu_track_label');
			        var val = $(this).attr('baidu_track_val');
			        if (val == null) {
			            window._hmt && window._hmt.push(['_trackEvent', name, action, label == null ? '-' : label]);
			        }
			        else {
			            window._hmt && window._hmt.push(['_trackEvent', name, action, label == null ? '-' : label, val]);
			        }
					// window.location = public_pre+'/inviteManager/inviteCode.html?newYear=2018';
					$('.guide').show();
					$('.guide').off('click').on('click',function(){
						$(this).hide();
					})
				})
				//生成邀请海报
				$('#generatePoster').off('click').on('click',function(){
					//绑定百度统计trackEvent事件逻辑
					var name = $(this).attr('baidu_track_name');
			        var action = $(this).attr('baidu_track_action');
			        var label = $(this).attr('baidu_track_label');
			        var val = $(this).attr('baidu_track_val');
			        if (val == null) {
			            window._hmt && window._hmt.push(['_trackEvent', name, action, label == null ? '-' : label]);
			        }
			        else {
			            window._hmt && window._hmt.push(['_trackEvent', name, action, label == null ? '-' : label, val]);
			        }
					$('.down_img').show().find('#down_img').attr('src',that.ownImgUrl);
					// $('#down_img').css('margin-top',($(window).height()-$('#down_img').height())/2);
					$('html').addClass('noscroll');
					$('.down_img').off('click').on('click',function(e){
						var e = e || window.event;
						if(e.target.id != 'down_img'){
							$(this).hide();
							$('html').removeClass('noscroll');
						}
					}).on('touchmove', function(e) {
						var e = e || window.event;
					    e.preventDefault();
					});
				})

				$('.get_money').off('click').on('click',function(){
					// alert('领红包');
					var _self = $(this);
					var RecID = _self.attr('RecID');
					$.ajax({
						url:public_url+'/api/Invite/ReceiveRedEves',
						type:'post',
						xhrFields: {
			                withCredentials: true
			            },
			            crossDomain: true,
						data:{
							RecID:RecID,
							r:Math.random()
						},
						success:function(data){
							if(data.Status == 0){
								that.initList();
							}else{
								layer.open({
									content:data.Message,
									time:2,
									skin: 'msg'
								})
							}
						}
					})
				})
			},
			ReloadList:function(){// 列表下拉上拉刷新 加载
				var that = this;
				var dropload = $('.center_contain').dropload({
					scrollArea : window,
			        domDown : {//下方dom
			            domClass   : 'dropload-down',
			            domRefresh : '<div class="dropload-refresh">↑上拉加载更多</div>',
			            domLoad    : '<div class="dropload-load"><span class="loading"></span>加载中...</div>',
			            domNoData  : '<div class="dropload-noData">-- END --</div>'
			        },
			        loadDownFn : function(me){//下滑动作
			            parameter.RecID = $('.content_list').find('.single_row:last').attr('RecID');
			            var dropLen1 = $('.center_contain').find('.dropload-load').length;
		                if( dropLen1 > 1){
		                	$('.center_contain').find('.dropload-load:last').remove();
		                }
			            // 拼接HTML
			            var result = '';
			            $.ajax({
			            	url:public_url+'/api/Invite/GetBeInvitedList',
			                type: 'get',
			                xhrFields: {
				                withCredentials: true
				            },
				            crossDomain: true,
			                data: parameter,
			                success: function(data){
			                    if(data.Status == 0){
			                    	var arrLen = data.Result.BeInvitedList;
				                    if(arrLen.length > 0){
				                        for(var i = 0; i< arrLen.length; i++){
											result += '<div class="single_row border-bottom" RecID='+arrLen[i].RecID+'>'+
															'<figure>';
																if( !arrLen[i].HeadImgurl){
																	arrLen[i].HeadImgurl = '/images/pic_default.png';
																}
										                        result +='<img src='+arrLen[i].HeadImgurl+'>'+
										                        '<figcaption>'+
										                            '<p class="pay_status">'+
										                               arrLen[i].Nickname+
										                            '</p>'+
										                            '<p class="pay_time">'+
										                                arrLen[i].InviteTime.substr(5,11)+
										                            ' 关注</p>'+
										                            '<div class="clear"></div>'+
										                        '</figcaption>'+
										                    '</figure>'+
								                    		'<div class="money_date">';
								                    if(arrLen[i].RedEvesStatus == '104001'){
								                    	result += '<p class="receive_money red"><span class="orange get_money" RecID='+arrLen[i].RecID+'>领红包</span>';
								                    }else if(arrLen[i].RedEvesStatus == '104002'){
								                    	result += '<p class="receive_money red">'+formatMoney(arrLen[i].RedEvesPrice,2,'');
								                    }else if(arrLen[i].RedEvesStatus == '104003'){
								                    	result += '<p class="receive_money text_grey">当日红包已领完';
								                    }else if(arrLen[i].RedEvesStatus == '104004'){
								                    	result += '<p class="receive_money text_grey">尚未完成分享';
								                    }
								                    result += '</p></div></div>';
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
				                        $('.content_list').append(result);
				                        var dropLen = $('.center_contain').find('.dropload-down').length;
				                        if( dropLen > 1){
				                        	$('.center_contain').find('.dropload-down:last').remove();
				                        }
				                        that.operate();
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
			        threshold : 150
			    });
			}
		}
	})
	
})