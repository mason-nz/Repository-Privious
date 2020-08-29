wx.ready(function () {
	/*标题：赤兔联盟—页面title

	描述：【

	抢单赚钱：加入赤兔联盟，分享文章赚零花

	我的订单：来赤兔联盟，让您轻松赚点零花钱

	收益记录：来赤兔联盟，让您轻松赚点零花钱

	提现（我的余额）：来赤兔联盟，让您轻松赚点零花钱

	提现记录：来赤兔联盟，让您轻松赚点零花钱

	签到有礼：每日签到，天天都有小惊喜

	提现规则：关注赤兔联盟，赚钱秘籍都在这里哟！

	个人信息：来赤兔联盟，让您轻松赚点零花钱

	】*/
	var jsUrlHelper = {
		   getUrlParam : function(url, ref) {
			var str = "";

			// 如果不包括此参数
			if (url.indexOf(ref) == -1)
				return "";

			str = url.substr(url.indexOf('?') + 1);

			arr = str.split('&');
			for (i in arr) {
				var paired = arr[i].split('=');

				if (paired[0] == ref) {
					return paired[1];
				}
			}
			return "";
		   },
		   putUrlParam : function(url, ref, value) {

			// 如果没有参数
			if (url.indexOf('?') == -1)
				return url + "?" + ref + "=" + value;

			// 如果不包括此参数
			if (url.indexOf(ref) == -1)
				return url + "&" + ref + "=" + value;

			var arr_url = url.split('?');

			var base = arr_url[0];

			var arr_param = arr_url[1].split('&');

			for (i = 0; i < arr_param.length; i++) {

				var paired = arr_param[i].split('=');

				if (paired[0] == ref) {
					paired[1] = value;
					arr_param[i] = paired.join('=');
					break;
				}
			}

			return base + "?" + arr_param.join('&');
		   },
		   delUrlParam : function(url, ref) {

			// 如果不包括此参数
			if (url.indexOf(ref) == -1)
				return url;

			var arr_url = url.split('?');

			var base = arr_url[0];

			var arr_param = arr_url[1].split('&');

			var index = -1;

			for (i = 0; i < arr_param.length; i++) {

				var paired = arr_param[i].split('=');

				if (paired[0] == ref) {

					index = i;
					break;
				}
			}

			if (index == -1) {
				return url;
			} else {
				arr_param.splice(index, 1);
				return base + "?" + arr_param.join('&');
			}
		}
	};
	var url = window.location.href.toLowerCase();
	var url1 = jsUrlHelper.delUrlParam(url,'isauth'),
		// imgurl = public_url+'/images/demoIcon.jpg',
		option = {
			title: '赤兔联盟-'+document.title,//分享标题
			desc: '来赤兔联盟，让您轻松赚点零花钱',//分享描述
			link: url,
			imgUrl: public_url+'/images/default_share.jpg',//分享图标
			type: 'link', // 分享类型,music、video或link，不填默认为link
			dataUrl: '', // 如果type是music或video，则要提供数据链接，默认为空
			trigger: function (res) {
				// alert('用户点击分享');
			},
			success: function (res) {
				// alert('已分享');
				//分享保存接口
				$.ajax({
					url:public_url+'/api/WechatShare/AddWechatShare',
					type:'post',
					xhrFields: {
		                withCredentials: true
		            },
		            crossDomain: true,
					data:{
						ShareUrl:url
					},
					success:function(){
						//保存成功
					}
				})
			},
			cancel: function (res) {
				// alert('已取消');
			},
			fail: function (res) {
				alert(JSON.stringify(res));
			}
		}

	if(url.indexOf('make_money') != '-1'){//抢单赚钱
		option.desc = '加入赤兔联盟，分享文章赚零花';
	}else if(url.indexOf('sign') != '-1'){//签到
		option.desc = '每日签到，天天都有小惊喜';
	}else if(url.indexOf('rule') != '-1'){//提现规则
		option.desc = '关注赤兔联盟，赚钱秘籍都在这里哟！';
	}
	wx.onMenuShareAppMessage(option);
	wx.onMenuShareTimeline(option);
	wx.onMenuShareQQ(option);
	wx.onMenuShareQZone(option);
	wx.onMenuShareWeibo(option);

	//个人信息所有二级页  隐藏所有非基础菜单
	if(url.indexOf('usermanager') != '-1' && url.indexOf('userinfo.html') == '-1'){
		wx.hideMenuItems({
          menuList: ['menuItem:share:appMessage',
          			'menuItem:share:timeline',
          			'menuItem:share:qq',
          			'menuItem:share:weiboApp',
          			'menuItem:favorite',
          			'menuItem:share:facebook',
          			'menuItem:share:QZone',
          			'menuItem:editTag',
          			'menuItem:delete',
          			'menuItem:copyUrl',
          			'menuItem:originPage',
          			'menuItem:readMode',
          			'menuItem:openWithQQBrowser',
          			'menuItem:openWithSafari',
          			'menuItem:share:email',
          			'menuItem:share:brand'
          ] // 要隐藏的菜单项，只能隐藏“传播类”和“保护类”按钮
        });
	}

})
