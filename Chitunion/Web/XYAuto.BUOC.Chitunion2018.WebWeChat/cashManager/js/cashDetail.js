/*
* Written by:     wangc
* function:       提现详情
* Created Date:   2018-01-18
* Modified Date:
*/
$(function(){
	var app = new Vue({
		el: '#content',
		data:{
			detail: [],
			success_status:false,
			fail_status:false
		},
		created:function(){
			this.checkNet();
			// this.initDetail();
		},
		methods:{
			checkNet:function(){//根据网络状态显示内容
				var that = this;
				wx.ready(function () {
					//隐藏所有传播类和复制链接菜单
					wx.hideMenuItems({
						menuList: ['menuItem:copyUrl','menuItem:share:appMessage','menuItem:share:timeline','menuItem:share:qq','menuItem:share:weiboApp','menuItem:share:QZone','menuItem:share:facebook']  // 要隐藏的菜单项，只能隐藏“传播类”和“保护类”按钮，所有menu项见附录3
					});
					wx.getNetworkType({
						success: function (res) {
							var networkType = res.networkType; // 返回网络类型2g，3g，4g，wifi
							that.initDetail();
							$('.netOk').show().siblings('.netNo').remove();
						},
						fail:function(){
							$('.netNo').show().siblings('.netOk').remove();
						}
					});
				})
			},
			initDetail:function(){//获取提现详情
				var that = this;
				$.ajax({
					url:public_url+'/api/Withdrawals/GetInfo',
					type:'get',
					xhrFields: {
		                withCredentials: true
		            },
		            crossDomain: true,
					data:{
						WithdrawalsId:GetRequest().WithdrawalsId,
						r:Math.random()
					},
					success:function(response){
						if(response.Status == 0){
							that.detail = response.Result.WithdrawalsInfo;
							if(response.Result.WithdrawalsInfo.PayStatus == 195002){
								that.fail_status = true;
							}else if(response.Result.WithdrawalsInfo.PayStatus == 195003){
								that.success_status = true;
							}
						}
					}
				})
			}
		}
	})
})