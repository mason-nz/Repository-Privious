$(function(){
	// 初始化页面
	// showStageList(set);
	showShopcar(set.data.businesstype);
	//全局条件
	var condition = {};
	var availableTags,
			mediaIdArr,
			val;

/*获取解码后的用户ID*/
function GetUserId() {
    var url = location.search; //获取url中"?"符后的字串
    var theRequest = new Object();
    if (url.indexOf("?") != -1) {
        var str = url.substr(1);
        strs = str.split("&");
        for (var i = 0; i < strs.length; i++) {
            theRequest[strs[i].split("=")[0]] = strs[i].split("=")[1];
        }
    }
    return theRequest;
}
var userData = GetUserId();
var userID = userData.userID?userData.userID:'gt86ZRCRjng%3d';
function userID1(userID){

	/*若由AE代客户下单进入，flag=0*/
	if(userID != "gt86ZRCRjng%3d" && userID != undefined){
		if(GetUserId().flag){
			return '&userID='+ userID + '&flag=' +GetUserId().flag;
		}else{
	    	return '&userID='+ userID + '&flag=0';
		}
	/*若从一级菜单进入，flag=1*/
	}else{
		if(GetUserId().flag){
			return '&userID='+ userID + '&flag=' +GetUserId().flag;
		}else{
	    	return '&userID='+ userID + '&flag=1';
		}
	}
};
//整体搜索的功能
$($('#search-hd .search-input')[0]).on('input',function(){
	val = $(this).val();
	if(val.length > 0&&val!=" "){
		$('#search-hd .pholder').hide();
		setAjax({
			url:"/api/Publish/SearchAuto",
			type:'get',
			data:{
				pageSize:10,
				KeyWord:val,
				businesstype:set.data.businesstype
			},
			dataType:'json'
		},function(data){
			if(data.Status==0){
				if(set.data.businesstype==14003){
					_hmt.push(['_setAutoPageview', false]);
					_hmt.push(['_trackPageview', '/OrderManager/wb_list.html']);
				}else if(set.data.businesstype==14004){
					_hmt.push(['_setAutoPageview', false]);
					_hmt.push(['_trackPageview', '/OrderManager/sp_list.html']);
				}else if(set.data.businesstype==14005){
					_hmt.push(['_setAutoPageview', false]);
					_hmt.push(['_trackPageview', '/OrderManager/zb_list.html']);
				}else{
					_hmt.push(['_setAutoPageview', false]);
					_hmt.push(['_trackPageview', '/OrderManager/app_list.html']);
				}
				availableTags = [];
				mediaIdArr = [];
				for(var i = 0;i<data.Result.length;i++){
						if(data.Result[i].Name.toLowerCase().indexOf(val.toLowerCase())!=-1||data.Result[i].Name.indexOf(val)!=-1){
							availableTags.push(data.Result[i].Name);
							mediaIdArr.push(data.Result[i].MediaId);
						}else if((data.Result[i].Name.toLowerCase().indexOf(val.toLowerCase())==-1||data.Result[i].Name.indexOf(val)==-1)&&(data.Result[i].Number.toLowerCase().indexOf(val.toLowerCase())!=-1||data.Result[i].Number.indexOf(val)!=-1)){
							availableTags.push(data.Result[i].Number);
							mediaIdArr.push(data.Result[i].MediaId);
						}

					}
				$($("#search-hd .search-input")[0]).autocomplete({
					source: availableTags
				})
			}
		})
	}
})
//点击搜索
$('.btn-search').eq(0).on('click',function(){
	// 条件清空
	$(".wx_already_value ul").empty();
	condition = {};
	// 上三行
	$('.wx_key').each(function(index,obj){
		if(index !=3){
			$('.wx_value').eq(index).find('li a').removeClass('active')
			$('.wx_value').eq(index).find('li:first a').addClass('active');
			set.data[$('.wx_value').eq(index).prev('.wx_key').attr('data-name')]=$('.wx_value').eq(index).find('li:first :hidden').val();
		}
	});
	// 下拉框
	$('.wx_value select').each(function(index,obj){
		$(obj).val($(this).find('option:first').val());
		set.data.CoverageArea='';
		if($(obj).attr('name') != 'ProvinceID' && $(obj).attr('name') != 'CityID'){
			set.data[$(obj).attr('name')]=$(obj).val();
		}
	});
	if($('.wx_already_value ul li').length>0){
		$('.wx_already_wrap').show();
	}else{
		$('.wx_already_wrap').hide();
	}

	var Inpvalue = $("#search-hd .search-input").val();
	//添加输入框搜索的名称并显示到已选列表
	var condition_media = "关键字:&nbsp;";
	condition[condition_media] = Inpvalue;
//精确查找部分
	if(availableTags.indexOf(Inpvalue)!=-1){
		var index = availableTags.indexOf(Inpvalue);
		 mediaId = mediaIdArr[index];
		 //区分精确跟模糊重叠的情况
		if(availableTags.length==1){
			var SearchMediaId = "SearchMediaId";
			set.data[SearchMediaId] = mediaId;
		}else{
			set.data.SearchMediaId = -2;
			var MediaName = "MediaName";
			set.data[MediaName] = Inpvalue;
		}
			showSelected(condition);
			showStageList(set);
	}//模糊查找部分
	else if(Inpvalue!=""){
		var MediaName = "MediaName";
		//重置精确选择的条件
		set.data.SearchMediaId = -2;
		set.data[MediaName] = Inpvalue;
		//调用显示条件的函数
		showSelected(condition);
		showStageList(set);
	}//其他情况
	else{
			set.data.SearchMediaId = -2;
			set.data.MediaName = "";
			showStageList(set);
		}
		//点击下拉框显示的条件显示数据
				$(document).on('click', '.ui-menu-item', function () {
					var index = $(this).index();
					 mediaId = mediaIdArr[index];
					//添加输入框搜索的名称并显示到已选列表
					var condition_media = "关键字:&nbsp;";
					condition[condition_media] = Inpvalue;
					var SearchMediaId = "SearchMediaId";
					set.data[SearchMediaId] = mediaId;
					showSelected(condition);
					// console.log(set);
					// showStageList(set);
				});
})
// 回车键事件
$($('#search-hd .search-input')[0])
.on('keydown',function(e) {
	var e = e||window.event;
	if(e.keyCode==13){
			$('.btn-search').eq(0).trigger('click');
			$(this).trigger('blur');
	}
})
//提前请求状态
.on("focus",function(){
	setAjax({
		url:"/api/Publish/SearchAuto",
		type:'get',
		data:{
			pageSize:10,
			businesstype:set.data.businesstype
		},
		dataType:'json'
	},function(data){
		if(data.Status==0){
			if(set.data.businesstype==14003){
				_hmt.push(['_setAutoPageview', false]);
				_hmt.push(['_trackPageview', '/OrderManager/wb_list.html']);
			}else if(set.data.businesstype==14004){
				_hmt.push(['_setAutoPageview', false]);
				_hmt.push(['_trackPageview', '/OrderManager/sp_list.html']);
			}else if(set.data.businesstype==14005){
				_hmt.push(['_setAutoPageview', false]);
				_hmt.push(['_trackPageview', '/OrderManager/zb_list.html']);
			}else{
				_hmt.push(['_setAutoPageview', false]);
				_hmt.push(['_trackPageview', '/OrderManager/app_list.html']);
			}
			availableTags = [];
			mediaIdArr = [];
			$($("#search-hd .search-input")[0]).autocomplete({
				source: availableTags
			})
		}
	})
})
.on('blur',function(){
	if($(this).val()==""){
		$('#search-hd .pholder').show(500);
	}
});
//针对IE不兼容input事件的兼容
if(document.all){
    $('input[type="text"]').each(function() {
        var that=this;
        if(this.attachEvent) {
            this.attachEvent('onpropertychange',function(e) {
							  var e = e||window.event;
                if(e.propertyName!='value') return;
                $(that).trigger('input');
            });
        }
    })
}
	// 条件展开隐藏
	$('.wx_ext').on('click',function(){
		if(!$(this).attr('data-open')){
			$(this).parents('.wx_wrap').css('height','auto');
			$(this).attr('data-open','1');
			$('.wx_ext').html('收起 <img src="/images/arrow_02.png">');
		}else{
			$(this).parents('.wx_wrap').css('height','35px');
			$(this).attr('data-open','');
			$('.wx_ext').html('展开 <img src="/images/arrow_01.png">');
		}
	});

	// 点击部分查询条件，下部显示条件并刷新页面
	$('.wx_value').each(function(index,obj){
		// 点击条件
		$(obj).unbind('click','a');
		$(obj).on('click','a',function(){
			// 控制条件的样式显示
			$(this).addClass('active').parent('li').siblings('li').find('a').removeClass('active');
			$('.wx_already_wrap').show();

			// 保存选中的条件
			condition[$(this).parents('.wx_value').prev('.wx_key').text()] =$(this).text();
			var id = $(this).next(':hidden').val();
			set.data[$(this).parents('.wx_value').prev().attr('data-name')] = id;
			showSelected(condition);
			var arr = [];
			if(set.data.businesstype == 14002){
				$('.shopcartList .PublishDetailID').each(function(i,o){
					arr.push($(o).val());
				});
			}else{
				$('.shopcartList .MediaID').each(function(i,o){
					arr.push($(o).val());
				});
			}
			showStageList(set,arr);
		});
	});

// $('.catagory a').each(function(i,o){
// 	$(o).trigger('click');
// 	if($('.list').html()=='<img src="/images/no_data.png" style="display:block;margin:20px auto;">'){
// 		$(this).parent().remove();
// 	}
// })

	// 下拉框修改查询条件
	$('.wx_value select').on('change',function(){
		// 获取下拉框的值并修改查询条件，刷新页面
		var name = $(this).attr('name');
		if(name=='ProvinceID' || name=='CityID'){
			var Province = parseInt($('#ddlProvince').val()) ? parseInt($('#ddlProvince').val()):0;
			var City = parseInt($('#ddlCity').val()) ? parseInt($('#ddlCity').val()):0;
			var Province_str =$('#ddlProvince').find('option:selected').text() == '覆盖区域 -- 省'? '':$('#ddlProvince').find('option:selected').text();
			var City_str =$('#ddlCity').find('option:selected').text()== '覆盖区域 -- 市'? '':$('#ddlCity').find('option:selected').text();
			if(Province == 0){
				condition['覆盖区域：'] = Province_str;
				set.data.CoverageArea = Province+'-'+0;
			}else{
				condition['覆盖区域：'] = Province_str+' '+ City_str;
				set.data.CoverageArea = Province+'-'+City;
			}

			showSelected(condition);
		}else{
			if($(this).find('option:selected')[0] == $(this).find('option:first')[0]){
				delete condition[$(this).attr('data-name')+'：'];
			}else{
				condition[$(this).attr('data-name')+'：'] = $(this).find('option:selected').text();
			}
			set.data[$(this).attr('name')] = $(this).val();
			showSelected(condition);
			if($('.wx_already_value ul li').length>1){
				$('.wx_already_wrap').show();
			}else{
				$('.wx_already_wrap').hide();
			}
		}
		var arr = [];
		if(set.data.businesstype == 14002){
			$('.shopcartList .PublishDetailID').each(function(i,o){
				arr.push($(o).val());
			});
		}else{
			$('.shopcartList .MediaID').each(function(i,o){
				arr.push($(o).val());
			});
		}
		showStageList(set,arr);
	});
	// 搜索框修改查询条件
	$('.wx_value .submit').on('click',function(){
		var val = $.trim($(this).siblings('.input_search').val());
		if(val){
			condition[$(this).prev('.input_search').attr('data-name')+'：'] = val;
			set.data[$(this).prev('.input_search').attr('name')] = val;
			showSelected(condition);
			var arr = [];
			if(set.data.businesstype == 14002){
				$('.shopcartList .PublishDetailID').each(function(i,o){
					arr.push($(o).val());
				});
			}else{
				$('.shopcartList .MediaID').each(function(i,o){
					arr.push($(o).val());
				});
			}
			showStageList(set,arr);
		}else{
			layer.msg($(this).siblings('.input_search').attr('data-name')+'不能为空');
			$(this).siblings('.input_search').val('');
		}
	});
	$('.input_search').on('focus',function(){
		$(this).addClass('hover');
	}).on('blur',function(){
		$(this).removeClass('hover');
	});

	// 省市二级联动
	BindProvince('ddlProvince',true); //绑定省份
	$('#ddlProvince').on('change',function(){
		crmCustCheckHelper.triggerProvince();
		if($(this).val()>0)
		{
			$('#ddlCity').css('display','inline-block');
		}
		else
		{
			$('#ddlCity').hide();
		}
	});
	// 清空按钮
	$('.wx_already_value').on('click','.clearAll',function(){
		// 清空输入框
		$('#search-hd .search-input').val("");
		$(".wx_already_value ul").empty();
		condition = {};
		// 上三行
		$('.wx_key').each(function(index,obj){
			if(index !=3){
				$('.wx_value').eq(index).find('li a').removeClass('active')
				$('.wx_value').eq(index).find('li:first a').addClass('active');
				set.data[$('.wx_value').eq(index).prev('.wx_key').attr('data-name')]=$('.wx_value').eq(index).find('li:first :hidden').val();
			}
		});
		// 下拉框
		$('.wx_value select').each(function(index,obj){
			$(obj).val($(this).find('option:first').val());
			set.data.CoverageArea='';
			if($(obj).attr('name') != 'ProvinceID' && $(obj).attr('name') != 'CityID'){
				set.data[$(obj).attr('name')]=$(obj).val();
			}
		});
		// 搜索框
		$('.input_search').val('');
		set.data.MediaName = '';
		set.data.SearchMediaId = "";
		set.data.AdForm = '';
		var arr = [];
		if(set.data.businesstype == 14002){
			$('.shopcartList .PublishDetailID').each(function(i,o){
				arr.push($(o).val());
			});
		}else{
			$('.shopcartList .MediaID').each(function(i,o){
				arr.push($(o).val());
			});
		}
		showStageList(set,arr);
		if($('.wx_already_value ul li').length>0){
			$('.wx_already_wrap').show();
		}else{
			$('.wx_already_wrap').hide();
		}
	});
	// 单个删除按钮
	$('.wx_already_value').on('click','img',function(){
		// 隐藏本条件，并使本条件恢复默认值，刷新页面
		$(this).parent('li').remove();
		delete condition[$(this).siblings('b').text()]; //删除条件

		var Inpvalue = $("#search-hd .search-input").val();
		//删除模糊搜索条件
		if($(this).prev().html()==Inpvalue){
			var condition_media = "关键字:&nbsp;";
			delete condition[condition_media];
			set.data.MediaName = "";
			set.data.SearchMediaId = -2;
			$("#search-hd .search-input").val("");
		}
		var key = $(this).siblings('b').text();
		// 上三行
		$('.wx_key').each(function(index,obj){
			if($(obj).text() == key){
				$('.wx_value').eq(index).find('li a').removeClass('active')
				$('.wx_value').eq(index).find('li:first a').addClass('active');
				set.data[$('.wx_value').eq(index).prev('.wx_key').attr('data-name')]=$('.wx_value').eq(index).find('li:first :hidden').val();

			}
		});
		// 下拉框
		$('.wx_value select').each(function(index,obj){
			if($(obj).attr('data-name')+'：' == key){
				$(obj).val($(this).find('option:first').val());
				if($(obj).attr('name') == 'ProvinceID' || $(obj).attr('name') == 'CityID'){
					set.data.CoverageArea='';
				}else{
					set.data[$(obj).attr('name')]=$(obj).val();
				}
			}
		});

		$('.wx_value .input_search').each(function(index,obj){
			set.data[$(obj).attr('name')] = '';
			if($(obj).attr('data-name')+'：' == key){
				$(obj).val('');
			}
		});

		if($('.wx_already_value ul li').length>1){
			$('.wx_already_wrap').show();
		}else{
			$('.wx_already_wrap').hide();
		}

		var arr = [];
		if(set.data.businesstype == 14002){
			$('.shopcartList .PublishDetailID').each(function(i,o){
				arr.push($(o).val());
			});
		}else{
			$('.shopcartList .MediaID').each(function(i,o){
				arr.push($(o).val());
			});
		}
		showStageList(set,arr);
	});
	// 排序按钮
	$('.wx_table1 .fanscount').unbind('click');
	$('.wx_table1 .fanscount').on('click',function(){
		var img = $(this).find('img').attr('src');
		$(this).addClass('yellow');
		if(img && img=='/images/icon16_c.png'){
			$(this).find('img').attr('src','/images/icon16_b.png');
			set.data.orderBy = 1001;
			var arr1 = [];
			if(set.data.businesstype == 14002){
				$('.shopcartList .PublishDetailID').each(function(i,o){
					arr1.push($(o).val());
				});
			}else{
				$('.shopcartList .MediaID').each(function(i,o){
					arr1.push($(o).val());
				});
			}
			showStageList(set,arr1);
		}else if(img && img=='/images/icon16_a.png'){ //倒叙排列
			$(this).find('img').attr('src','/images/icon16_b.png');
			set.data.orderBy = 1001;
			var arr2 = [];
			if(set.data.businesstype == 14002){
				$('.shopcartList .PublishDetailID').each(function(i,o){
					arr2.push($(o).val());
				});
			}else{
				$('.shopcartList .MediaID').each(function(i,o){
					arr2.push($(o).val());
				});
			}
			showStageList(set,arr2);
		}else if(img && img=='/images/icon16_b.png'){
			$(this).find('img').attr('src','/images/icon16_a.png');
			set.data.orderBy = 1002;
			var arr3 = [];
			if(set.data.businesstype == 14002){
				$('.shopcartList .PublishDetailID').each(function(i,o){
					arr3.push($(o).val());
				});
			}else{
				$('.shopcartList .MediaID').each(function(i,o){
					arr3.push($(o).val());
				});
			}
			showStageList(set,arr3);
		}
	});
	// $('.sorting select').on('change',function(){
	// 	$('.sorting li a').removeClass('active');
	// 	$('.sorting li img').attr('src','/images/icon16_c.png');
	// 	$(this).addClass('hover');
	// 	set.data.OrderByReference = $(this).val();
	// 	var arr = [];
	// 	if(set.data.businesstype == 14002){
	// 		$('.shopcartList .PublishDetailID').each(function(i,o){
	// 			arr.push($(o).val());
	// 		});
	// 	}else{
	// 		$('.shopcartList .MediaID').each(function(i,o){
	// 			arr.push($(o).val());
	// 		});
	// 	}
	// 	showStageList(set,arr);
	// });


	/*----------------------------------------------------------------------------------购物车*/
	// 购物车移入移出效果
	$('.shop_cart_btn').on('mouseenter',function(e){
		e.stopPropagation();
		$(this).parents('.shop_cart').addClass('hover');
	}).on('mouseleave',function(e){
		e.stopPropagation();
		$(this).parents('.shop_cart').removeClass('hover');
	});
	$(".shop_cart_btn").unbind('click');
	// 点击购物车按钮效果
	$(".shop_cart_btn").on("click", function(e){
		if(!$(this).attr('data-isClick')){
			$('.cart_content').css({width:'280px',border:'solid #E4E4E4 1px'});
			$(this).parents('.shop_cart').toggleClass('clicked');
			$(document).one("click", function(){
				$('.cart_content').css({width:'0px',border:'0 none'});
				$('.shop_cart').removeClass('clicked');
				$(".shop_cart_btn").attr('data-isClick','');
			});
			e.stopPropagation();
			$(this).attr('data-isClick','1');
		}else{
			$('.cart_content').css('width','0px');
			$('.shop_cart').removeClass('clicked');
			$(this).attr('data-isClick','');
		}
	});
	$(".cart_content").unbind('click');
	$(".cart_content").on("click", function(e){
		e.stopPropagation();
	});

	// 添加购物车功能
	$('input.selectAll').on('change',function(){
		var index_load = layer.load(0, {shade: false});
		if(orderId){
			if($(this).prop('checked') == true){
				var arr1 = [];
				var str1 = '';
				$('.list input:checkbox').prop('checked',true);
				if(set.data.businesstype == 14002){
					$('.list input:checkbox').each(function(index,obj){
						if(index<=10 - $('.cart_num').text()){
							var flag = true;
							$('.shopcartList .PublishDetailID').each(function(i,o){
								if($(obj).siblings('.PublishDetailID').val() == $(o).val()){
									flag = false;
								}
							});
							if(flag){
								var o = {
									MediaID: $(obj).siblings('.MediaID').val(),
									PublishDetailID: $(obj).siblings('.PublishDetailID').val(),
									AdForm : $(this).parents('.wx_table2').find('.AdForm').text(),
									Style   : $(this).parents('.wx_table2').find('.Style').text(),
									Name : $(this).parents('.wx_table2').find('.Name').text(),
									AdPosition: $(this).parents('.wx_table2').find('.AdPosition').text()
								}
								arr1.push(o);
							}
						}
					});
					if(flag){
						var o = {
							MediaID: $(obj).siblings('.MediaID').val(),
							PublishDetailID: $(obj).siblings('.PublishDetailID').val(),
							AdForm : $(this).parents('.wx_table2').find('.AdForm').text(),
							Style   : $(this).parents('.wx_table2').find('.Style').text(),
							Name : $(this).parents('.wx_table2').find('.Name').text(),
							AdPosition: $(this).parents('.wx_table2').find('.AdPosition').text()
						}
						arr1.push(o);
					}
					$(arr1).each(function(index,obj){
						str1+= '<div class="cart_shop"><div class="ping01"><input name="" type="checkbox" checked="checked" value=""><input class="MediaID" type="hidden" value="'+obj.MediaID+'"><input class="PublishDetailID" type="hidden" value="'+obj.PublishDetailID+'"></div>'
						+'<div class="ping04"><div class="h2">'+obj.AdForm+'</div><div class="p">'+obj.Style+'</div><div class="clear"></div><div class="h2">'+obj.Name+'</div><div class="p">'+obj.AdPosition+'</div><div class="clear"></div></div>'
						+'<div class="ping03"><a href="javascript:void(0)" class="red del">删除</a></div><div class="clear"></div></div>';;
					});
					$('.shopcartList').append(str1);
					$('.cart_num').text($('.shopcartList .cart_shop').length);
					if($('.cart_num').text() !=0){
						$('.cart_not').remove();
					}
					layer.close(index_load);
					var left = $('.shop_cart_btn').offset().left;
					var top = $('.shop_cart_btn').offset().top;
					$('.flyer').eq(0).show().siblings('.flyer').hide();
					$('.wx_table2').eq(0).css('position','static')
					$('.flyer').eq(0).animate({left:left,top:top},800,function(){
						$('.wx_table2').eq(0).css('position','relative')
						$(this).css({
							display:'none',
							top:$(this).parents('.wx_table2').offset().top,
							left:$(this).parents('.wx_table2').offset().left
						});
					});
				}else{
					$('.list input:checkbox').each(function(index,obj){
						var flag = true;
						$('.shopcartList .MediaID').each(function(i,o){
							if($(obj).siblings('.MediaID').val() == $(o).val()){
								flag = false;
							}
						});
						if(flag){
							var o = {
								MediaID: $(obj).siblings('.MediaID').val(),
								imgSrc : $(this).parents('.wx_table2').find('.portrait img').attr('src'),
								name   : $(this).parents('.wx_table2').find('.wx_content1 h2>span').text(),
								number : $(this).parents('.wx_table2').find('.wx_content1 h2 p span').text()
							}
							arr1.push(o);
						}
					});
					$(arr1).each(function(index,obj){
						str1+= '<div class="cart_shop"><div class="ping01"><input name="" type="checkbox" checked="checked" value=""><input class="MediaID" type="hidden" value="'+obj.MediaID+'"></div><div class="ping02"><div class="img"><img src="'+obj.imgSrc+'" width="42" height="42"></div><div class="h2">'+obj.name+'</div><div class="p">'+obj.number+'</div></div><div class="ping03"><a href="javascript:void(0)" class="red del">删除</a></div><div class="clear"></div></div>';
					});
					$('.shopcartList').append(str1);
					$('.cart_num').text($('.shopcartList .cart_shop').length);
					if($('.cart_num').text() !=0){
						$('.cart_not').remove();
					}
					layer.close(index_load);
					var left = $('.shop_cart_btn').offset().left;
					var top = $('.shop_cart_btn').offset().top;
					$('.flyer').eq(0).show().siblings('.flyer').hide();
					$('.wx_table2').eq(0).css('position','static')
					$('.flyer').eq(0).animate({left:left,top:top},800,function(){
						$('.wx_table2').eq(0).css('position','relative')
						$(this).css({
							display:'none',
							top:$(this).parents('.wx_table2').offset().top,
							left:$(this).parents('.wx_table2').offset().left
						});
					});
				}
			}else{
				$('.list input:checkbox').prop('checked',false);
				$('.list input:checkbox').not(':checked').each(function(index,obj){
					$('.list input:checkbox').each(function(index,obj){
						if(set.data.businesstype == 14002){
							$('.cart_shop .PublishDetailID').each(function(i,o){
								if($(obj).siblings('.PublishDetailID').val() == $(o).val()){
									$(o).parents('.cart_shop').remove();
									$('.cart_num').text($('.shopcartList .cart_shop').length);
								}
							});
						}else{
							$('.cart_shop .MediaID').each(function(i,o){
								if($(obj).siblings('.MediaID').val() == $(o).val()){
									$(o).parents('.cart_shop').remove();
									$('.cart_num').text($('.shopcartList .cart_shop').length);
								}
							});
						}

					});
				});
				layer.close(index_load);
			}
		}else{
			var arr= [];
			if(set.data.businesstype == 14002){
				$('.list input:checkbox').each(function(index,obj){
					var o = {
						MediaID: $(obj).siblings('.MediaID').val(),
						PublishDetailID:$(obj).siblings('.PublishDetailID').val()
					}
					arr.push(o);
				});
			}else{
				$('.list input:checkbox').each(function(index,obj){
					var o = {
						MediaID: $(obj).siblings('.MediaID').val(),
						PublishDetailID:null
					}
					arr.push(o);
				});
			}
			if($(this).prop('checked')){
				addShopcar(1,arr,set.data.businesstype);
				$('.list input:checkbox').prop('checked',true);
				var left = $('.shop_cart_btn').offset().left;
				var top = $('.shop_cart_btn').offset().top;
				$('.flyer').eq(0).show().siblings('.flyer').hide();
				$('.wx_table2').eq(0).css('position','static');
				$('.flyer').eq(0).animate({left:left,top:top},800,function(){
					$('.wx_table2').eq(0).css('position','relative');
					$(this).css({
						display:'none',
						top:$(this).parents('.wx_table2').offset().top,
						left:$(this).parents('.wx_table2').offset().left
					});
				});
			}else{
				$('.list input:checkbox').prop('checked',false);
				addShopcar(2,arr,set.data.businesstype);
				showShopcar(set.data.businesstype,true);
			}
		}
	});
	$('.list').on('change','input:checkbox',function(){
		if(orderId){
			var index_load = layer.load(0, {shade: false});
			if($(this).prop('checked') == true){
				if(set.data.businesstype == 14002){
					var MediaID = $(this).siblings('.MediaID').val();
					var PublishDetailID = $(this).siblings('.PublishDetailID').val();
					var AdForm = $(this).parents('.wx_table2').find('.AdForm').text();
					var Style = $(this).parents('.wx_table2').find('.Style').text();
					var Name = $(this).parents('.wx_table2').find('.Name').text();
					var AdPosition = $(this).parents('.wx_table2').find('.AdPosition').text();
					var flag = true;   //默认可添加
					$('.shopcartList .MediaID').each(function(index,obj){
						if($(obj).val() == PublishDetailID){
							flag =  false;
						}
					});
					if(flag){
						var str = '<div class="cart_shop"><div class="ping01"><input name="" type="checkbox" checked="checked" value=""><input class="MediaID" type="hidden" value="'+MediaID+'"><input class="PublishDetailID" type="hidden" value="'+PublishDetailID+'"></div>'
							+'<div class="ping04"><div class="h2">'+AdForm+'</div><div class="p">'+Style+'</div><div class="clear"></div><div class="h2">'+Name+'</div><div class="p">'+AdPosition+'</div><div class="clear"></div></div>'
							+'<div class="ping03"><a href="javasctipt:void(0)" class="red del">删除</a></div><div class="clear"></div></div>';
						$('.shopcartList').append(str);
						$('.cart_num').text($('.shopcartList .cart_shop').length);
						if($('.cart_num').text() !=0){
							$('.cart_not').remove();
						}
						layer.close(index_load);
						var left = $('.shop_cart_btn').offset().left;
						var top = $('.shop_cart_btn').offset().top;
						var i = $(this).attr('name');
						$('.flyer').eq(parseInt(i)).show().siblings('.flyer').hide();
						$('.wx_table2').eq(parseInt(i)).css('position','static')
						$('.flyer').eq(parseInt(i)).animate({left:left,top:top},800,function(){
							$('.wx_table2').eq(parseInt(i)).css('position','relative')
							$(this).css({
								display:'none',
								top:$(this).parents('.wx_table2').offset().top,
								left:$(this).parents('.wx_table2').offset().left
							});
						});
					}
				}else{
					var MediaID = $(this).siblings('.MediaID').val();
					var PubID = $(this).siblings('.PubID').val();
					var imgSrc = $(this).parents('.wx_table2').find('.portrait img').attr('src');
					var name = $(this).parents('.wx_table2').find('.name').text();
					var number = $(this).parents('.wx_table2').find('.number').text()? $(this).parents('.wx_table2').find('.number').text(): '';
					var flag = true;   //默认可添加
					$('.shopcartList .MediaID').each(function(index,obj){
						if($(obj).val() == MediaID){
							flag =  false;
						}
					});
					if(flag){
						var str = '<div class="cart_shop">'
						+'<div class="ping01"><input name="" type="checkbox" checked="checked" value=""><input class="MediaID" type="hidden" value="'+MediaID+'"><input class="PubID" type="hidden" value="'+PubID+'"></div>'
						+'<div class="ping02"><div class="img"><img src="'+imgSrc+'" width="42" +height="42"></div><div class="h2">'+name+'</div><div class="p">'+number+'</div></div>'
						+'<div class="ping03"><a href="javascript:void(0)" class="red del">删除</a></div>'
						+'<div class="clear"></div>'
						+' </div>'
						$('.shopcartList').append(str);
						$('.cart_num').text($('.shopcartList .cart_shop').length);
						if($('.cart_num').text() !=0){
							$('.cart_not').remove();
						}
						layer.close(index_load);
						var left = $('.shop_cart_btn').offset().left;
						var top = $('.shop_cart_btn').offset().top;
						var i = $(this).attr('name');
						$('.flyer').eq(parseInt(i)).show().siblings('.flyer').hide();
						$('.wx_table2').eq(parseInt(i)).css('position','static')
						$('.flyer').eq(parseInt(i)).animate({left:left,top:top},800,function(){
							$('.wx_table2').eq(parseInt(i)).css('position','relative')
							$(this).css({
								display:'none',
								top:$(this).parents('.wx_table2').offset().top,
								left:$(this).parents('.wx_table2').offset().left
							});
						});
					}
				}
			}else{
				layer.close(index_load);
				if(set.data.businesstype == 14002){
					var PublishDetailID = $(this).siblings('.PublishDetailID').val();
					$('.shopcartList .PublishDetailID[value='+PublishDetailID+']').parents('.cart_shop').remove();
					$('.cart_num').text($('.shopcartList .cart_shop').length);
				}else{
					var MediaID = $(this).siblings('.MediaID').val();
					$(':hidden[value='+MediaID+']').parents('.cart_shop').remove();
					$('.cart_num').text($('.shopcartList .cart_shop').length);
				}
			}
		}else{
			if(set.data.businesstype == 14002){
				var o = [{
					MediaID: $(this).siblings('.MediaID').val(),
					PublishDetailID:$(this).siblings('.PublishDetailID').val()
				}];
			}else{
				var o = [{
					MediaID: $(this).siblings('.MediaID').val(),
					PublishDetailID:null
				}];
			}
			if($(this).prop('checked')){
				addShopcar(1,o,set.data.businesstype,$(this).attr('name'));
			}else{
				addShopcar(2,o,set.data.businesstype,$(this).attr('name'));
			}
		}

		if($('.list :checkbox').length == $('.list :checked').length && $('.list :checkbox').length !=0){
			$('.selectAll').prop('checked',true);
		}else{
			$('.selectAll').prop('checked',false);
		}
		checkShopcar();
	});
	// 删除
	$('.shopcartList').unbind('click');
	$('.shopcartList').on('click','.del',function(){
		if(orderId){
			if(set.data.businesstype == 14002){
				var PublishDetailID = $(this).parents('.cart_shop').find('.PublishDetailID').val();
				$('.list .PublishDetailID').each(function(i,o){
					if($(o).val() == PublishDetailID){
						$(o).siblings(':checkbox').prop('checked',false);
					}
				});
			}else{
				var MediaID = $(this).parents('.cart_shop').find('.MediaID').val();
				$('.list .MediaID').each(function(i,o){
					if($(o).val() == MediaID){
						$(o).siblings(':checkbox').prop('checked',false);
					}
				});
			}

			$(this).parents('.cart_shop').remove();
			$('.cart_num').text($('.shopcartList .cart_shop').length);
			$('.cart_price').text($('.shopcartList :checked').length);
			if($('.cart_num').text() == 0){
				$('.shopcartList').html('<div class="cart_not"><img src="/images/cart_02.png"</div>');
				$('.ibar_cart_group_title :checkbox').prop('checked',false);
			}
		}else{
			var id1 = $(this).parents('.cart_shop').find('.MediaID').val();
			var id2 = $(this).parents('.cart_shop').find('.PublishDetailID').val()? $(this).parents('.cart_shop').find('.PublishDetailID').val():null;
			setAjax(
				{   url:'/api/ShoppingCart/Operate_ShoppingCart',
				type:'post',
				data:{
					"OptType":2,
					"MediaType": set.data.businesstype,
					IDs:[{
						"MediaID":id1,
						"PublishDetailID":id2
					}]
				}
			},
			function(data){
				if(data.Status == 0){
					if(set.data.businesstype==14003){
						_hmt.push(['_setAutoPageview', false]);
						_hmt.push(['_trackPageview', '/OrderManager/wb_list.html']);
					}else if(set.data.businesstype==14004){
						_hmt.push(['_setAutoPageview', false]);
						_hmt.push(['_trackPageview', '/OrderManager/sp_list.html']);
					}else if(set.data.businesstype==14005){
						_hmt.push(['_setAutoPageview', false]);
						_hmt.push(['_trackPageview', '/OrderManager/zb_list.html']);
					}else{
						_hmt.push(['_setAutoPageview', false]);
						_hmt.push(['_trackPageview', '/OrderManager/app_list.html']);
					}
					showShopcar(set.data.businesstype);
					$('.list .MediaID[value='+id1+']').siblings(':checkbox').prop('checked',false);
				}else{
					return ;
				}
			}
			);
		}
		checkShopcar();
	});
	// 一键清除
	$('.cart_box .clearAll').unbind('click');
	$('.cart_box .clearAll').on('click',function(){
		if($('.shopcartList .cart_shop').length){
			var _this = this;
			layer.confirm('确定清空吗？',{btn:['确认','取消']},
				function(index){
					layer.close(index);
					$('.list :checkbox').prop('checked',false);
					$('.selectAll').prop('checked',false);
					if(orderId){
						$('.shopcartList').empty();
						$('.cart_num').text(0);
						$('.cart_price').text(0);
						if($('.cart_num').text() == 0){
							$('.shopcartList').html('<div class="cart_not"><img src="/images/cart_02.png"</div>');
						}
						$('.ibar_cart_group_title :checkbox').prop('checked',false);
						checkShopcar();
					}else{
						var IDs = [];
						$('.shopcartList .cart_shop').each(function(index,obj){
							var obj = {
								MediaID:$(obj).find('.MediaID').val(),
								PublishDetailID:$(obj).find('.PublishDetailID')?$(obj).siblings('.PublishDetailID').val() : null
							}
							IDs.push(obj);
						});
						setAjax(
							{   url:'/api/ShoppingCart/Operate_ShoppingCart',
							type:'post',
							data:{
								OptType:3,
								MediaType: set.data.businesstype,
								IDs:IDs
							}
						},
						function(data){
							if(data.Status == 0){
								if(set.data.businesstype==14003){
									_hmt.push(['_setAutoPageview', false]);
									_hmt.push(['_trackPageview', '/OrderManager/wb_list.html']);
								}else if(set.data.businesstype==14004){
									_hmt.push(['_setAutoPageview', false]);
									_hmt.push(['_trackPageview', '/OrderManager/sp_list.html']);
								}else if(set.data.businesstype==14005){
									_hmt.push(['_setAutoPageview', false]);
									_hmt.push(['_trackPageview', '/OrderManager/zb_list.html']);
								}else{
									_hmt.push(['_setAutoPageview', false]);
									_hmt.push(['_trackPageview', '/OrderManager/app_list.html']);
								}
								showShopcar(set.data.businesstype,true);
							}else{
								return ;
							}
						}
						);
						checkShopcar();
					}
				}
				);
		}else{
			return ;
		}
	});

	// 选中/反选
	$('.ibar_cart_group_title input').unbind('change');
	$('.ibar_cart_group_title input').on('change',function(){
		if($(this).prop('checked')){
			$('.shopcartList :checkbox').each(function(index,object){
				if($(this).attr('disabled')){
					$(this).prop('checked',false)
				}else{
					$(this).prop('checked',true);
				}
			})
		}else{
			$('.shopcartList :checkbox').prop('checked',false);
		}
		checkShopcar();
	});
	// 控制购物车按钮
	$('.shopcartList').on('change',':checkbox',function(){
		checkSelecteAll();
		checkShopcar();
	});
  	// 立即投放
  	$('.cart_go_btn').unbind('click');
  	$('.cart_go_btn').on('click',function(){
  		if(orderId){
  			var ADDetails = [];
  			var ADOrderInfo =  {
  				"OrderID"   : $('.ADOrderInfo .OrderID').val() ? $('.ADOrderInfo .OrderID').val() : '',
  				"MediaType" : $('.ADOrderInfo .MediaType').val() ? $('.ADOrderInfo .MediaType').val() : '',
  				"OrderName" : $('.ADOrderInfo .OrderName').val() ? $('.ADOrderInfo .OrderName').val() : '',
  				"Status"    : $('.ADOrderInfo .Status').val() ?  $('.ADOrderInfo .Status').val(): '',
  				"BeginTime" : $('.ADOrderInfo .BeginTime').val() ? $('.ADOrderInfo .BeginTime').val() : '',
  				"EndTime"   : $('.ADOrderInfo .EndTime').val() ? $('.ADOrderInfo .EndTime').val() : '',
  				"Note"      : $('.ADOrderInfo .Note').val() ? $('.ADOrderInfo .Note').val(): '',
  				"CustomerID": $('.ADOrderInfo .CustomerID').val() ? $('.ADOrderInfo .CustomerID').val()+'' : '',
  				"UploadFileName":$('.ADOrderInfo .UploadFileName').val(),
  				"UploadFileURL" : $('.ADOrderInfo .UploadFileURL').val() ? $('.ADOrderInfo .UploadFileURL').val() : ''
  			};
  			if(ADOrderInfo.CustomerID == '0'){
  				ADOrderInfo.CustomerID='';
  			}
  			if(set.data.businesstype == 14002){
  				$('.shopcartList :checked').each(function(index,obj){
  					if($('.ADScheduleInfos').eq(index).text()){
  						var ADScheduleInfos = JSON.parse($('.ADScheduleInfos').eq(index).text());

  					}else{
  						var ADScheduleInfos = null;
  					}
  					var o = {
  						"MediaType":set.data.businesstype,
  						"MediaID":$(obj).siblings('.MediaID').val(),
  						"PubDetailID"  : $(obj).siblings('.PublishDetailID').val(),
  						"AdjustPrice"  : $(obj).siblings('.AdjustPrice').val(),
  						"ADLaunchDays"  : $(obj).siblings('.ADLaunchDays').val(),
  						"ADScheduleInfos":ADScheduleInfos
  					}

  					ADDetails.push(o);
  				});
  			}else{
  				$('.shopcartList :checked').each(function(index,obj){
  					var o = {
  						"MediaType":set.data.businesstype,
  						"MediaID":$(obj).siblings('.MediaID').val(),
  						"PubDetailID"  : $(obj).siblings('.PublishDetailID').val(),
  						"AdjustPrice"  : $(obj).siblings('.AdjustPrice').val(),
  						"AdjustDiscount"  : $(obj).siblings('.AdjustDiscount').val(),
  						"ADLaunchDays"  : $(obj).siblings('.ADLaunchDays').val(),
  					}
  					// console.log("Push对象是"+JSON.stringify(o));
  					ADDetails.push(o);
  				});
  			}
  			if(checkShopcar()){
  				setAjax(
  					{   url:'/api/ADOrderInfo/AddOrUpdate_ADOrderInfo',
  					type:'post',
  					data:{
  						optType: 2,
  						ADOrderInfo:ADOrderInfo,
  						ADDetails:ADDetails
  					}
  				},
  				function(data){
  					// console.log("data是"+JSON.stringify(data));
  					if(data.Status == 0){
							if(set.data.businesstype==14003){
								_hmt.push(['_setAutoPageview', false]);
								_hmt.push(['_trackPageview', '/OrderManager/wb_list.html']);
							}else if(set.data.businesstype==14004){
								_hmt.push(['_setAutoPageview', false]);
								_hmt.push(['_trackPageview', '/OrderManager/sp_list.html']);
							}else if(set.data.businesstype==14005){
								_hmt.push(['_setAutoPageview', false]);
								_hmt.push(['_trackPageview', '/OrderManager/zb_list.html']);
							}else{
								_hmt.push(['_setAutoPageview', false]);
								_hmt.push(['_trackPageview', '/OrderManager/app_list.html']);
							}
							// 跳转页面
							if(set.data.businesstype == 14002){
								if(OrderState){
									window.location='/OrderManager/requirementApp.html?MediaType=14002&orderID='+orderId+'&OrderState='+OrderState + userID1(userID);
									_hmt.push(['_trackEvent', 'button', 'click', 'entercart']);
								}else{
									window.location='/OrderManager/AuditingOfApp.html?MediaType=14002&orderID='+orderId+ userID1(userID);
									_hmt.push(['_trackEvent', 'button', 'click', 'entercart']);
								}
							}else{
								if(OrderState){
									window.location='/OrderManager/requirementSelf01.html?MediaType='+set.data.businesstype+'&orderID='+orderId+ userID1(userID);
									_hmt.push(['_trackEvent', 'button', 'click', 'entercart']);
								}else{
									window.location='/OrderManager/AuditOrderSelf.html?MediaType='+set.data.businesstype+'&orderID='+orderId+ userID1(userID);
									_hmt.push(['_trackEvent', 'button', 'click', 'entercart']);
								}
							}
						}else{
							// console.log(data.Message);
							var index = layer.alert(data.Message,function(){
								layer.close(index);
								return ;
							});

						}
					}
					);
  			}else{
  				return ;
  			}
  		}else{
  			var IDs = [];
  			$('.shopcartList :checkbox').each(function(i,obj){
  				var o = {
  					'MediaID':$(obj).siblings('.MediaID').val() ? $(obj).siblings('.MediaID').val():null,
  					'PublishDetailID':$(obj).siblings('.PublishDetailID').val() ? $(obj).siblings('.PublishDetailID').val() : null,
  					'IsSelected': $(obj).prop('checked')
  				}
  				IDs.push(o);
  			});
  			if(checkShopcar()){
  				setAjax(
  					{   url:'/api/ShoppingCart/Delivery_ShoppingCart',
  					type:'post',
  					data:{
  						MediaType: set.data.businesstype,
  						IDs:IDs
  					}
  				},
  				function(data){
  					if(data.Status == 0){
							if(set.data.businesstype==14003){
								_hmt.push(['_setAutoPageview', false]);
								_hmt.push(['_trackPageview', '/OrderManager/wb_list.html']);
							}else if(set.data.businesstype==14004){
								_hmt.push(['_setAutoPageview', false]);
								_hmt.push(['_trackPageview', '/OrderManager/sp_list.html']);
							}else if(set.data.businesstype==14005){
								_hmt.push(['_setAutoPageview', false]);
								_hmt.push(['_trackPageview', '/OrderManager/zb_list.html']);
							}else{
								_hmt.push(['_setAutoPageview', false]);
								_hmt.push(['_trackPageview', '/OrderManager/app_list.html']);
							}
							// 跳转页面
							if(userID){
								if(set.data.businesstype == 14002){
									window.location='/OrderManager/requirementApp.html?MediaType='+set.data.businesstype+userID1(userID);
									_hmt.push(['_trackEvent', 'button', 'click', 'entercart']);
								}else{
									window.location='/OrderManager/requirementSelf01.html?MediaType='+set.data.businesstype+userID1(userID);
									_hmt.push(['_trackEvent', 'button', 'click', 'entercart']);
								}
							}else{
								if(set.data.businesstype == 14002){
									window.location='/OrderManager/requirementApp.html?MediaType='+set.data.businesstype+userID1(userID);
									_hmt.push(['_trackEvent', 'button', 'click', 'entercart']);
								}else{
									window.location='/OrderManager/requirementSelf01.html?MediaType='+set.data.businesstype+userID1(userID);
									_hmt.push(['_trackEvent', 'button', 'click', 'entercart']);
								}
							}

						}else{
							// console.log(data.Message);
							layer.mgs(data.Message,function(){
								return ;
							});

						}
					}
					);
  			}else{
  				return ;
  			}
  		}
  	});

  	// 回到顶部
  	$(document).on('scroll',function(){
  		if($(window).scrollTop() > $(window).height()*2/3){
  			$('.top').show();
  		}else{
  			$('.top').css('display','none');
  		}
  	});
  	$('.top').on('click',function(){
  		$('body,html').animate({scrollTop:0},500);
  	});
  	// 控制窗口大小改变时，购物车高度的控制
  	$(window).resize(function(){
  		var height = document.documentElement.clientHeight-150;
  		$('.shopcartList').css({maxHeight:height+'px',overflowY:'auto'});
  	});
  });
/*--------------------------------------------公共方法*/
// 获取url参数
var search = window.location.href.split('?')[1];
var search_obj={};
if(search){
	$(search.split('&')).each(function(index,obj){
		search_obj[obj.split('=')[0]] = obj.split('=')[1];
	});
	var orderId = search_obj.orderID // 获得订单id
	var userID =  search_obj.userID;
	var OrderState = search_obj.OrderState;
}else{
	var orderId // 获得订单id
	var userID;
	var OrderState;
}
// 排序函数
function sort(){
	$(this).parent('li').siblings('li').find('a').removeClass('active');
	$(this).parents('li').siblings('li').find('img').attr('src','/images/icon16_c.png');
	$(this).parent('li').siblings('li').find('a').attr('is_click','');
	$('.sorting select').removeClass('hover');
	$('.sorting select option:first').prop('selected',true);
	set.data.OrderByReference = -1;
	$(this).addClass('active');

	var orderBy = $(this).attr('data-orderBy');
	var is_click = $(this).attr('is_click');
	if($(this).text() != '默认排序'){
		if(is_click){
			set.data.orderBy = parseInt(orderBy)+1;
			$(this).attr('is_click','');
		}else{
			set.data.orderBy = orderBy;
			$(this).attr('is_click','1');
		}
	}else{
		set.data.orderBy = orderBy;
	}
	var arr = [];
	if(set.data.businesstype == 14002){
		$('.shopcartList .PublishDetailID').each(function(i,o){
			arr.push($(o).val());
		});
	}else{
		$('.shopcartList .MediaID').each(function(i,o){
			arr.push($(o).val());
		});
	}
	//切换选中的状态
	if($(this).attr('is_click')){
		$(this).find('img').attr('src','/images/icon16_b.png');
	}else{
		$(this).find('img').attr('src','/images/icon16_a.png');
	}
	showStageList(set,arr);
}
// 检查购物车选中状态，控制按钮样式
function checkShopcar(){
	var num = $('.shopcartList :checked').length;
	$('.cart_price').text(num);
	if(num !=0){
		$('.cart_handler a').addClass('cart_go_btn').removeClass('cart_go_buk');
		return true;
	}else{
		$('.cart_handler a').addClass('cart_go_buk').removeClass('cart_go_btn');
		return  false;
	}
}
/*---------------------------------------购物车*/
// 检查购物车中已存在的
function checkShopSelected(arr){
	if(set.data.businesstype == 14002){
		$(arr).each(function(index,obj){
			$('.list .wx_table2 .PublishDetailID').each(function(i,o){
				if(obj == $(o).val()){
					$('.list .wx_table2 :checkbox').eq(i).prop('checked',true);
				}
			});
		});
	}else{
		$(arr).each(function(index,obj){
			$('.list .wx_table2 .MediaID').each(function(i,o){
				if(obj == $(o).val()){
					$('.list .wx_table2 :checkbox').eq(i).prop('checked',true);
				}
			});
		});
	}
}
// 请求购物车数据
function showShopcar(businesstype,showStage){
	if(orderId){
		setAjax(
			{   url:'/api/ADOrderInfo/GetByOrderID_ADOrderInfo',
			type:'get',
			data:{
				orderid: orderId
			},
			selector:'.shopcartList',
		},
		function(data){
			if(data.Status == 0 ){
				if(set.data.businesstype==14003){
					_hmt.push(['_setAutoPageview', false]);
					_hmt.push(['_trackPageview', '/OrderManager/wb_list.html']);
				}else if(set.data.businesstype==14004){
					_hmt.push(['_setAutoPageview', false]);
					_hmt.push(['_trackPageview', '/OrderManager/sp_list.html']);
				}else if(set.data.businesstype==14005){
					_hmt.push(['_setAutoPageview', false]);
					_hmt.push(['_trackPageview', '/OrderManager/zb_list.html']);
				}else{
					_hmt.push(['_setAutoPageview', false]);
					_hmt.push(['_trackPageview', '/OrderManager/app_list.html']);
				}
				if(data.Result){
					$('.ADOrderInfo').html(ejs.render($('#ADOrderInfo').html(),{data: data.Result.ADOrderInfo}));
					if(data.Result.SubADInfos){
						if(set.data.businesstype == 14002){
							var s1=0;
							for(var i=0;i<data.Result.SubADInfos.length;i++){
								for(var j=0;j<data.Result.SubADInfos[i].APPDetails.length;j++){
									s1++;
								}
							}
							if(s1== 0){
								$('.shopcartList').html('<div class="cart_not"><img src="/images/cart_02.png"</div>');
								$('.cart_num').text(0);
								checkShopcar();
							}else{
								$('.shopcartList').html(ejs.render($('#shopcart_temp').html(),{list:data.Result,type:set.data.businesstype}));
								$('.cart_num').text($('.cart_box .cart_shop').length);
								checkSelecteAll();
								checkShopcar();
								$('.shopcartList .MediaID').each(function(index,object){
									$('.list .MediaId').each(function(i,o){
										if($(object).val() == $(o).val()){
											$(object).siblings(':checkbox').prop('checked',true);
										}
									})
								});
								if(!showStage){
									var shop_arr1=[];
									$('.shopcartList .PublishDetailID').each(function(i,o){
										shop_arr1.push($(o).val());
									});
									showStageList(set,shop_arr1);
								}
							}
						}else{
							var s2=0;
							for(var i=0;i<data.Result.SubADInfos.length;i++){
								for(var j=0;j<data.Result.SubADInfos[i].SelfDetails.length;j++){
									s2++;
								}
							}
							if(s2== 0){
								$('.shopcartList').html('<div class="cart_not"><img src="/images/cart_02.png"</div>');
								$('.cart_num').text(0);
								checkShopcar();
							}else{
								$('.shopcartList').html(ejs.render($('#shopcart_temp').html(),{list:data.Result,type:set.data.businesstype}));
								$('.cart_num').text($('.cart_box .cart_shop').length);
								checkSelecteAll();
								checkShopcar();
								if(!showStage){
									var shop_arr2=[];
									$('.shopcartList .MediaID').each(function(i,o){
										shop_arr2.push($(o).val());
									});
									showStageList(set,shop_arr2);
								}

							}
						}
					}else{
						$('.shopcartList').html('<div class="cart_not"><img src="/images/cart_02.png"</div>');
						$('.cart_num').text(0);
						checkShopcar();
					}
				}
			}else{
				$('.shopcartList').html('<div class="cart_not"><img src="/images/cart_02.png"</div>');
				$('.cart_num').text(0);
				checkShopcar();
			}
			var height = document.documentElement.clientHeight-150;
			$('.shopcartList').css({maxHeight:height+'px',overflowY:'auto'});

		});
	}else{
		setAjax(
			{   url:'/api/ShoppingCart/GetInfo_ShoppingCart',
			type:'get',
			data:{
				MediaType: businesstype
			},
			selector:'.shopcartList',
		},
		function(data){
			if(data.Status == 0){
				if(set.data.businesstype==14003){
					_hmt.push(['_setAutoPageview', false]);
					_hmt.push(['_trackPageview', '/OrderManager/wb_list.html']);
				}else if(set.data.businesstype==14004){
					_hmt.push(['_setAutoPageview', false]);
					_hmt.push(['_trackPageview', '/OrderManager/sp_list.html']);
				}else if(set.data.businesstype==14005){
					_hmt.push(['_setAutoPageview', false]);
					_hmt.push(['_trackPageview', '/OrderManager/zb_list.html']);
				}else{
					_hmt.push(['_setAutoPageview', false]);
					_hmt.push(['_trackPageview', '/OrderManager/app_list.html']);
				}
				if(data.Result){
					if(businesstype == 14002){
						if(data.Result.APP){
							$('.cart_num').text(data.Result.APP.length);
							$('.shopcartList').html(ejs.render($('#shopcart_temp').html(),{list:data.Result.APP,type:set.data.businesstype}));
							if(!showStage){
								var arr1 =[];
								for(var j=0;j<data.Result.APP.length;j++){
									arr1.push(data.Result.APP[j].PublishDetailID);
								}
								showStageList(set,arr1);
							}
						}
					}else{
						if(data.Result.SelfMedia){
							$('.cart_num').text(data.Result.SelfMedia.length);
							$('.shopcartList').html(ejs.render($('#shopcart_temp').html(),{list:data.Result.SelfMedia,type:set.data.businesstype}));
							if(!showStage){
								var arr2 =[];
								for(var i=0;i<data.Result.SelfMedia.length;i++){
									arr2.push(data.Result.SelfMedia[i].MediaID);
								}
								showStageList(set,arr2);
							}
						}
					}
				}
			}else if(data.Status==2){
				$('.shopcartList').html('<div class="cart_not"><img src="/images/cart_02.png"</div>');
				$('.cart_num').text(0);
				$('.cart_price').text(0);
				if(!showStage){
					showStageList(set);
				}
			}
			var height = document.documentElement.clientHeight-150;
			$('.shopcartList').css({maxHeight:height+'px',overflowY:'auto'});

				//检验多选
				checkSelecteAll();
				checkShopcar();
			}
			);
	}
}
// 添加购物车
function addShopcar(type,id,businesstype,i){
	var index = layer.load(0, {shade: false});
	setAjax(
		{   url:'/api/ShoppingCart/Operate_ShoppingCart',
		type:'post',
		data:{
			"OptType": type,
			"MediaType": businesstype,
			"IDs":id
		},
	},
	function(data){
			if(data.Status == 0){ //成功
				if(set.data.businesstype==14003){
					_hmt.push(['_setAutoPageview', false]);
					_hmt.push(['_trackPageview', '/OrderManager/wb_list.html']);
				}else if(set.data.businesstype==14004){
					_hmt.push(['_setAutoPageview', false]);
					_hmt.push(['_trackPageview', '/OrderManager/sp_list.html']);
				}else if(set.data.businesstype==14005){
					_hmt.push(['_setAutoPageview', false]);
					_hmt.push(['_trackPageview', '/OrderManager/zb_list.html']);
				}else{
					_hmt.push(['_setAutoPageview', false]);
					_hmt.push(['_trackPageview', '/OrderManager/app_list.html']);
				}
				layer.close(index);
				if(type == 1){
					shopAnimate(i);
					if($('.selectAll').prop('checked')){
						var left = $('.shop_cart_btn').offset().left;
						var top = $('.shop_cart_btn').offset().top;
						$('.flyer').eq(0).show().siblings('.flyer').hide();
						$('.wx_table2').eq(0).css('position','static')
						$('.flyer').eq(0).animate({left:left,top:top},800,function(){
							$('.wx_table2').eq(0).css('position','relative')
							$(this).css({
								display:'none',
								top:$(this).parents('.wx_table2').offset().top,
								left:$(this).parents('.wx_table2').offset().left
							});
							showShopcar(set.data.businesstype,true);
						});
					}
				}else if(type == 2){
					showShopcar(set.data.businesstype,true);
				}else if(type == 3){
					showShopcar(set.data.businesstype,true);
				}
			}else{
				return ;
			}
		}
		);
}
// 购物车动画
function shopAnimate(i){
	if($('.wx_table2 :checkbox').eq(parseInt(i)).prop('checked')){
		var left = $('.shop_cart_btn').offset().left;
		var top = $('.shop_cart_btn').offset().top;
		$('.flyer').eq(parseInt(i)).show().siblings('.flyer').hide();
		$('.wx_table2').eq(parseInt(i)).css('position','static')
		$('.flyer').eq(parseInt(i)).animate({left:left,top:top},800,function(){
			showShopcar(set.data.businesstype,true);
			$(this).css({
				display:'none',
				top:$(this).parents('.wx_table2').offset().top,
				left:$(this).parents('.wx_table2').offset().left
			});
		});
	}
}
// 购物车全选反选的判断
function checkSelecteAll(){
	if($('.shopcartList :checkbox').length == $('.shopcartList :checked').length && $('.shopcartList :checkbox').length !=0){
		$('.ibar_cart_group_title input').prop('checked',true);
	}else{
		$('.ibar_cart_group_title input').prop('checked',false);
	}
}
// 已选条件的显示
function showSelected(condition){
	var condition_str = '';
	// 设置已选条件的显示
	for(var i in condition){
		condition_str+= '<li><b style="font-weight:normal;">'+i+'</b><span>'+condition[i]+'</span><img src="/images/close.png"><input type="hidden" name="" value=""></li>';
	}
	condition_str +='<li class="clearAll"><a href="javascript:void(0)">清空条件</a></li>';
	$('.wx_already_value ul').html(condition_str);

	if($('.wx_already_value ul li').length>0){
		$('.wx_already_wrap').show();
	}else{
		$('.wx_already_wrap').hide();
	}
}
function layerImg(){
	/*图例弹层*/
	$(".list").on("click",".picDemo",function () {
		var url  = $(this).attr('picurl');
		$.openPopupLayer({
			name:'popLayerDemo',
			url:'./layer.html',
			error:function (dd) {
				alert(dd.status)
			},
			success:function () {
				$('.layer_con2 img').attr('src',url);
				$('.layer_con2').html('<img src="'+url+'" width="350" height="420">');

				$('#popupLayerScreenLocker').click(function () {
					$.closePopupLayer('popLayerDemo')
				})

			}
		});
	});
}
// 列表数据渲染
function showStageList(set,array){
	$(' .sorting li a').unbind('click');
	if(set.data.FansCount==-2&&set.data.Price==-2){
		set.data.FansCount="";
		set.data.Price="";
	}
	setAjax({
		selector:'.list',
		url:set.url,
		type:'get',
		data:set.data
	},
	function(data){
		console.log(data);
		$(' .sorting li a').on('click',sort);
		if(data.Status == 0){
			if(set.data.businesstype==14003){
				_hmt.push(['_setAutoPageview', false]);
				_hmt.push(['_trackPageview', '/OrderManager/wb_list.html']);
			}else if(set.data.businesstype==14004){
				_hmt.push(['_setAutoPageview', false]);
				_hmt.push(['_trackPageview', '/OrderManager/sp_list.html']);
			}else if(set.data.businesstype==14005){
				_hmt.push(['_setAutoPageview', false]);
				_hmt.push(['_trackPageview', '/OrderManager/zb_list.html']);
			}else{
				_hmt.push(['_setAutoPageview', false]);
				_hmt.push(['_trackPageview', '/OrderManager/app_list.html']);
			}
			$('.totalCount').text(data.Result.TotleCount);
			if(data.Result.TotleCount != 0){
				$('.list').html(ejs.render($('#wx_list').html(),{data:data.Result.List}));
				if(array){
					checkShopSelected(array)
				}
				if($('.list :checkbox').length == $('.list :checked').length && $('.list :checkbox').length !=0){
					$('.selectAll').prop('checked',true);
				}else{
					$('.selectAll').prop('checked',false);
				}

				$('.flyer').each(function(index,object){
					$(object).css({
						left:$(object).parents('.wx_table2').offset().left,
						top:$(object).parents('.wx_table2').offset().top
					})
				});
				/*图例弹层*/
				layerImg();
				$("#pageContainer").pagination(
					data.Result.TotleCount,
					{
                            // items_per_page: 2, //每页显示多少条记录（默认为20条）
                            callback: function (currPage, jg) {
                            	set.data.pageIndex = currPage;
                            	$(' .sorting li a').unbind('click');
                            	setAjax({selector:'.list',url:set.url,type:'get',data:set.data},
				           		function(data){  //成功
												if(data.Status == 0){
													if(set.data.businesstype==14003){
														_hmt.push(['_setAutoPageview', false]);
														_hmt.push(['_trackPageview', '/OrderManager/wb_list.html']);
													}else if(set.data.businesstype==14004){
														_hmt.push(['_setAutoPageview', false]);
														_hmt.push(['_trackPageview', '/OrderManager/sp_list.html']);
													}else if(set.data.businesstype==14005){
														_hmt.push(['_setAutoPageview', false]);
														_hmt.push(['_trackPageview', '/OrderManager/zb_list.html']);
													}else{
														_hmt.push(['_setAutoPageview', false]);
														_hmt.push(['_trackPageview', '/OrderManager/app_list.html']);
													}
					           			$(' .sorting li a').on('click',sort);
					           			if(data.Result.List !=null){
					           				$('.list').html(ejs.render($('#wx_list').html(),{data:data.Result.List}));
					           				layerImg();
					           				if(array){
					           					checkShopSelected(array)
					           				}
					           				if($('.list input:checkbox').length == $('.list input:checked').length && $('.list input:checkbox').length !=0){
					           					$('input.selectAll').prop('checked',true);
					           				}else{
					           					$('input.selectAll').prop('checked',false);
					           				}
					           				$('.flyer').each(function(index,object){
					           					$(object).css({
					           						left:$(object).parents('.wx_table2').offset().left,
					           						top:$(object).parents('.wx_table2').offset().top
					           					})
					           				})
					           			}
												}
				           		});
                            }
                        });
			}else{
				$('#pageContainer').html('');
				$('.selectAll').prop('checked',false);
				$('.list').html('<img src="/images/no_data.png" style="display:block;margin:20px auto;">');
			}
		}
	}
	);
}
// 枚举信息的渲染
function GetDictInfo(id,container,dom){
	setAjax({
		url:'/api/DictInfo/GetDictInfoByTypeID',
		type:'get',
		data:{typeID:id}
	},
	function(data){
		if(data.Status == 0){
			if(set.data.businesstype==14003){
				_hmt.push(['_setAutoPageview', false]);
				_hmt.push(['_trackPageview', '/OrderManager/wb_list.html']);
			}else if(set.data.businesstype==14004){
				_hmt.push(['_setAutoPageview', false]);
				_hmt.push(['_trackPageview', '/OrderManager/sp_list.html']);
			}else if(set.data.businesstype==14005){
				_hmt.push(['_setAutoPageview', false]);
				_hmt.push(['_trackPageview', '/OrderManager/zb_list.html']);
			}else{
				_hmt.push(['_setAutoPageview', false]);
				_hmt.push(['_trackPageview', '/OrderManager/app_list.html']);
			}
			if(dom=='li'){
				var str='<li style="float:left;margin-right: 10px;"><a href="javascript:void(0)" class="active">不限</a><input type="hidden" name="" value="-2"></li>';
				$(data.Result).each(function(i,o){
					str+='<li style="float:left;margin-right: 10px;"><a href="javascript:void(0)">'+o.DictName+'</a><input type="hidden" name="" value='+o.DictId+'></li>';

				});
				str+= '<div class="clear"></div>';
			}else if(dom=='option'){
				var str='<option value="-2">'+$(container).attr('data-name')+'</option>';
				$(data.Result).each(function(i,o){
					str+='<option value='+o.DictId+'>'+o.DictName+'</option>'
				});
				str+= '<div class="clear"></div>';
			}

			$(container).html(str);
		}
	});
}
//专门针对APP的枚举信息
function GetDictInfoAPP(id,container,dom){
		if(dom=='li'){
			setAjax({
				url:'/api/DictInfo/GetDictInfoByAPP',
				type:'get',
				data:{typeID:id}
			},
			function(data){
				if(data.Status == 0){
					if(set.data.businesstype==14003){
						_hmt.push(['_setAutoPageview', false]);
						_hmt.push(['_trackPageview', '/OrderManager/wb_list.html']);
					}else if(set.data.businesstype==14004){
						_hmt.push(['_setAutoPageview', false]);
						_hmt.push(['_trackPageview', '/OrderManager/sp_list.html']);
					}else if(set.data.businesstype==14005){
						_hmt.push(['_setAutoPageview', false]);
						_hmt.push(['_trackPageview', '/OrderManager/zb_list.html']);
					}else{
						_hmt.push(['_setAutoPageview', false]);
						_hmt.push(['_trackPageview', '/OrderManager/app_list.html']);
					}
					var str='<li style="float:left;margin-right: 10px;"><a href="javascript:void(0)" class="active">不限</a><input type="hidden" name="" value="-2"></li>';
					$(data.Result).each(function(i,o){
						str+='<li style="float:left;margin-right: 10px;"><a href="javascript:void(0)">'+o.DictName+'</a><input type="hidden" name="" value='+o.DictId+'></li>';
					});
					str+= '<div class="clear"></div>';
					$(container).html(str);
					//如果长度不够隐藏展开按钮
					setTimeout(function(){
						if($(".catagory li").length<12){
							$(".wx_ext").hide();
						}
					},0)
				}
			});

		}else if(dom=='option'){
			setAjax({
				url:'/api/DictInfo/GetDictInfoByTypeID',
				type:'get',
				data:{typeID:id}
			},
			function(data){
				if(data.Status == 0){
					if(set.data.businesstype==14003){
						_hmt.push(['_setAutoPageview', false]);
						_hmt.push(['_trackPageview', '/OrderManager/wb_list.html']);
					}else if(set.data.businesstype==14004){
						_hmt.push(['_setAutoPageview', false]);
						_hmt.push(['_trackPageview', '/OrderManager/sp_list.html']);
					}else if(set.data.businesstype==14005){
						_hmt.push(['_setAutoPageview', false]);
						_hmt.push(['_trackPageview', '/OrderManager/zb_list.html']);
					}else{
						_hmt.push(['_setAutoPageview', false]);
						_hmt.push(['_trackPageview', '/OrderManager/app_list.html']);
					}
					var str='<option value="-2">'+$(container).attr('data-name')+'</option>';
					$(data.Result).each(function(i,o){
						str+='<option value='+o.DictId+'>'+o.DictName+'</option>'
					});
					str+= '<div class="clear"></div>';
					$(container).html(str);
				}
			});

		}

}
// 省市二级联动
var crmCustCheckHelper = (function () {
    var triggerProvince = function () {//选中省份
    	BindCity('ddlProvince', 'ddlCity');
    }
    return {
    	triggerProvince: triggerProvince
    };
})();
