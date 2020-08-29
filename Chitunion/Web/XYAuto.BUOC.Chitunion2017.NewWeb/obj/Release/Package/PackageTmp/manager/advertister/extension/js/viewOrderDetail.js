/*
* Written by:     wangc
* function:       查看订单详情
* Created Date:   2017-12-22
* Modified Date:
*/
$(function(){
	function OrderDetail(){
		this.init();
	}
	OrderDetail.prototype = {
		constructor:OrderDetail,
		init:function(){
			var _self = this;
		    $('.wx_type').off('click').on('click','li',function(){
		        var _this = $(this),
		            index = _this.index();
		        _this.addClass('selected').siblings().removeClass('selected');
		        $('.wx_form').find('.table').hide().end().find('.table').eq(''+index+'').show();
		    })
		    _self.queryOrderInfo();
			_self.searchList();
		},
		searchList:function(){//查看订单下媒体信息
			var _self = this;
			setAjax({
				// url:'json/shop1.json',
				url:public_url+'/api/SmartSearch/GetSmartSearchMediaList',
				type:'get',
				data:{
					SmartSearchID:_self.GetRequest().RecID
				}
			},function(data){
				if(data.Status == 0){
     				$('#wechat_num').html(data.Result.CartWeiXin.TotalCount);
     				$('#weibo_num').html(data.Result.CartWeiBo.TotalCount);
     				$('#app_num').html(data.Result.CartApp.TotalCount);
     				$('.total_num').html(data.Result.CartWeiXin.TotalCount+data.Result.CartWeiBo.TotalCount+data.Result.CartApp.TotalCount);
					$('#table_wechat').html(ejs.render($('#weixin_list').html(),{list:data.Result.CartWeiXin.List}));
					$('#table_weibo').html(ejs.render($('#weibo_list').html(),{list:data.Result.CartWeiBo.List}));
					$('#table_app').html(ejs.render($('#app_list').html(),{list:data.Result.CartApp.List}));
					_self.operate();
				}else{
					layer.msg(data.Message,{time:1000})
				}
			})
		},
		queryOrderInfo:function(){//查看订单详情--明细
			var _self = this;
			setAjax({
				// url:'json/info.json',
				url:public_url+'/api/SmartSearch/GetSmartSearchDetailInfo',
				type:'get',
				data:{
					RecID:_self.GetRequest().RecID
				}
			},function(data){
				if(data.Status == 0){
					$('#orderStatus').html(data.Result.StatusName);
					$('.generalize').html(ejs.render($('#order_detail').html(),{info:data.Result}));
					setAjax({
						url:'json/purpose.json',
						type:'get',
						data:{
							typeID:97
						}
					},function(data1){
						if(data1.Status == 0){
							var purposeStr = '';
							data1.Result.forEach(function(el,i) {
								if((Math.pow(2,i) & data.Result.Purposes) >0){
									purposeStr = purposeStr.concat(el.DictName+'、');
								}
							});
							$('#promotePurpose').html(purposeStr.substr(0,purposeStr.length-1));
						}
					})
				}else{
					layer.msg(data.Message,{time:2000});
				}
			})
		},
		operate:function(){
			$('.noBigPic').off('mouseover').on('mouseover',function(){
		        $(this).removeClass('noBigPic');
		    }).off('mouseout').on('mouseout',function(){
		        $(this).addClass('noBigPic');
		    })
		    $('#sure_delever').off('click').on('click',function(){
		    	window.location = '/manager/advertister/extension/shoppingCart02.html';
		    })
		    //切换收起展开
		    $('.scale').off('click').on('click',function(){
		        var curImg = $(this).find('img').attr('src');
		        if(curImg == '/images/arrow_02.png'){//收起
		            $(this).html('<span>查看</span> <img src="/images/arrow_01.png">')
		            $('.wx_form').addClass('hide');
		            // $('#navLeft').css('height','800px');
		        }else{
		            $(this).html('<span>收起</span> <img src="/images/arrow_02.png">')
		            $('.wx_form').removeClass('hide');
		            // $('#navLeft').css('height',$('.contaner_body').height());
		        }
		        $('.wx_type li').eq(0).click();
		        
		    })
		},
		GetRequest:function() {// 获取url？后面的参数
	        var url = location.search;
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
	new OrderDetail();
})