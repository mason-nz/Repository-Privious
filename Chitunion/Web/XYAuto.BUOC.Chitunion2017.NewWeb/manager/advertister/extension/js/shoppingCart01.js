/*
* Written by:     wangc
* function:       选号车
* Created Date:   2017-12-20
* Modified Date:
*/
$(function(){
	function TableList(){
		this.init();
	}
	TableList.prototype = {
		constructor:TableList,
		init:function(){
			var _self = this;
			//如果未登录或不是广告主，跳转到登录页面
			if(CTLogin.IsLogin){
	            if( CTLogin.Category != '29001'){
	                layer.confirm('您不是广告主，请登录！', {
	                    btn: ['登录'] //按钮
	                }, function(){
	                    var url = encodeURI(public_url+'/manager/advertister/extension/shoppingCart01.html');
	                    window.location = '/Exit.aspx?gourl='+url;
	                });
	            }else{
	            	var h = $(window).height();
				    $('.wx_type').off('click').on('click','li',function(){
				        var _this = $(this),
				            index = _this.index();
				        _this.addClass('selected').siblings().removeClass('selected');
				        $('.rightMain').find('.table').hide().end().find('.table').eq(''+index+'').show();
				    })
					_self.searchList();
	            }
	        }else{
	            layer.confirm('您尚未登录，请登录！', {
	                btn: ['登录'] //按钮
	            }, function(){
	                var url = encodeURI(public_url+'/manager/advertister/extension/shoppingCart01.html');
	                window.location = '/Login.aspx?gourl='+url;
	            });
	        }
		},
		searchList:function(){
			var _self = this;
			setAjax({
				// url:'json/shop1.json',
				url:public_url+'/api/MediaMatching/GetCartMediaList',
				type:'get',
				data:{}
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
					layer.msg(data.Message,{time:2000})
				}
			})
		},
		operate:function(){
			var _self = this;
			$('.noBigPic').off('mouseover').on('mouseover',function(){
		        $(this).removeClass('noBigPic');
		    }).off('mouseout').on('mouseout',function(){
		        $(this).addClass('noBigPic');
		    })
		    $('.right_body').css('height',$('#navLeft').height()+20);
		    $('.contine_choose').off('click').on('click',function(){
		    	window.location = '/static/advertister/sort_list.html';
		    })
		    $('.deleteIt').off('click').on('click',function(){
		    	var MediaID = $(this).attr('RecID'),
		    		CartInfoList = [];
		    	CartInfoList.push({
		    		MediaType:$('.wx_type .selected').attr('mediaType')-0,
		    		MediaID:MediaID
		    	})
		    	setAjax({
		    		url:public_url+'/api/MediaMatching/DelCartInfo',
		    		type:'post',
		    		data:{
		    			CartInfoList:CartInfoList
		    		}
		    	},function(data){
		    		if(data.Status == 0){
		    			layer.msg('删除成功',{time:2000});
		    			_self.searchList();
		    		}else{
		    			layer.msg(data.Message,{time:2000})
		    		}
		    	})
		    })
		    $('#sure_delever').off('click').on('click',function(){
		    	if($('.total_num').html()>0){
		    		window.location = '/manager/advertister/extension/shoppingCart02.html';
		    	}else{
		    		layer.msg('请选择广告位',{time:2000});
		    	}
		    })
		}
	}
	new TableList();
})