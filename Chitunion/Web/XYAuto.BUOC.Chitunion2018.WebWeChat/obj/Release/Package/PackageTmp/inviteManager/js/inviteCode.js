/*
* Written by:     wangc
* function:       邀请好友二维码
* Created Date:   2018-01-18
* Modified Date:
*/
$(function(){
	function ShareDetail(){
		this.init();
	}
	//专属二维码，默认是赤兔无专属
	var ownImgUrl = public_url+'/images/demoIcon.png',
		ownImg_mediaId = '';
	ShareDetail.prototype = {
		constructor:ShareDetail,
		init:function(){
			var _self = this;
			//判断活动是否在有效期内
			$.ajax({
				url:public_url+'/api/Sign/IsValidActivity',
				type:'get',
				xhrFields: {
	                withCredentials: true
	            },
	            crossDomain: true,
				data:{
					ActivityType:1
				},
				success:function(data){
					if(data.Status == 0){
						if(data.Result == true){
							$('.main_container:last').remove();
						}else{//活动过期
							$('.main_container:last').show().siblings('.main_container').remove();
						}
					}
				}
			})
			wx.ready(function(){
				wx.hideMenuItems({
					menuList: ['menuItem:copyUrl']  // 要隐藏的菜单项，只能隐藏“传播类”和“保护类”按钮，所有menu项见附录3
				});
			})
			$.ajax({
				url: '/api/WeixinJSSDK/GetInfo?url=' + encodeURIComponent(window.location.href),
				type: 'get',
				xhrFields: {
				  withCredentials: true
				},
				crossDomain: true,
				success: function (data) {
					var configInfo = data.Result;
					wx.config({
						debug: false,
						appId: configInfo.AppId,
						nonceStr: configInfo.NonceStr,
						timestamp: configInfo.Timestamp,
						signature: configInfo.Signature,
						jsApiList: [
							'checkJsApi',
							'onMenuShareTimeline',
							'onMenuShareAppMessage',
							'onMenuShareQQ',
							'onMenuShareWeibo',
							'onMenuShareQZone',
							'downloadImage'
						]
				 	});
				}
			})
			$('.main_container').css('height',$(window).height());
			//邀请人能看到按钮，被邀请人看不到按钮
			if( GetRequest().newYear ){
				//获取专属二维码的url
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
			            	ownImgUrl = data.Result.InvitationQR+'?r='+Math.random();
			            	ownImg_mediaId = data.Result.MediaID;
			                $('#code').attr('src',ownImgUrl);
			                $('#code').on('error',function(){
			                	$(this).attr('src',public_url+'/images/demoIcon.jpg');
			                	ownImgUrl = public_url+'/images/demoIcon.jpg';
			                })
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
			}else{//被邀请人,会有一个imgurl,直接取此imgurl的地址，赋给图片
				ownImgUrl = decodeURI(GetRequest().imgurl);
				$('#code').attr('src',ownImgUrl);
				// $('#share').remove();
				_self.operate();
			}
		},
		operate:function(){//保存图片和分享
			var imgurl = encodeURI(ownImgUrl);
			// $('#save_pic').off('click').on('click',function(){
			// 	// alert('下载');
			// 	//获取邀请图片serverId并下载图片到手机
			// 	wx.downloadImage({
			// 		serverId: ownImg_mediaId, 
			// 		isShowProgressTips: 1, // 默认为1，显示进度提示
			// 		success: function (res) {
			// 			// alert(JSON.stringify(res));
			// 			var localId = res.localId; // 返回图片下载后的本地ID
			// 			layer.open({
			// 		        content: '已将图片下载到本地，请自行前往分享！'   //设置弹层内容
			// 		        ,btn:'好的'
			// 		    });
			// 		},error:function(){
			// 			layer.open({
			// 		        content: '下载失败'   
			// 		        ,btn:['重试','取消']
			// 		        ,shadeClose:false                      
			// 		        ,yes: function(index){         
			// 		            layer.close(index);
			// 		            $('#save_pic').click();
			// 		        },no:function(index){                   
			// 		            layer.close(index)
			// 		        }
			// 		    });
			// 		}
			// 	});
			// })
			$('#share').off('click').on('click',function(){
				$('.layer_pop').show();
				$('.layer_pop').off('click').on('click',function(){
					$(this).hide();
				})
			})
			wx.ready(function () {
				var option = {
					title: '送你一个赚钱机会',//分享标题
					desc: '加入赤兔联盟，赚钱就是这么简单！',//分享描述
					link: invite_url+'/inviteManager/inviteCode.html?imgurl='+imgurl,
					imgUrl: public_url+'/images/share.jpg',//分享图标
					type: 'link', // 分享类型,music、video或link，不填默认为link
					dataUrl: '', // 如果type是music或video，则要提供数据链接，默认为空
					trigger: function (res) {
						// alert('用户点击分享');
					},
					success: function (res) {
						// alert('已分享');
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
						$('.layer_pop').hide();
						alert(JSON.stringify(res));
					}
				}
				wx.onMenuShareAppMessage(option);
				wx.onMenuShareTimeline(option);
				wx.onMenuShareQQ(option);
				wx.onMenuShareQZone(option);
				wx.onMenuShareWeibo(option);
			})
		}

	}
	new ShareDetail();
})