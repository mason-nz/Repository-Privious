/*
* Written by:     zhanglp
* function:       APP列表页效果js库
* Created Date:   2017-06-15
* Modified Date:  2017-06-30
*/
$(function(){
	console.log(CTLogin);
		// 初始化页面数据
		showStageList(set);
		//全局条件
		var condition = {},
				availableTags,
				MediaID,
				TemplateID;
		//整体搜索的功能
		$('#search-hd .search-input').on('input',function(){
			var val = $(this).val();
			if(val.length > 0&&val!=" "){
				$('#search-hd .pholder').hide();
				setAjax({
					url:"/api/ADOrderInfo/QueryAPPByName?v=1_1",
					type:'get',
					data:{
						Name:val,
						AuditStatus:-4
					},
					dataType:'json'
				},function(data){
					console.log(data);
					if(data.Status==0){
						_hmt.push(['_setAutoPageview', false]);
						_hmt.push(['_trackPageview', '/OrderManager/wx_list.html']);
						availableTags = [];
						for(var i = 0;i<data.Result.length;i++){
								if(data.Result[i].Name.indexOf(val)!=-1){
									availableTags.push(data.Result[i].Name);
								}
							}
							console.log(availableTags);
						$($("#search-hd .search-input")[0]).autocomplete({
							source: availableTags
						})
					}
				})
			}
		})
		//点击搜索
		$('.btn-search').on('click',function(){
			// 点击搜索四个条件全部置为初始值
			condition = {};
			// 上四行
			$('.wx_key').each(function(index,obj){
				if(index !=4){
					$('.wx_value').eq(index).find('li a').removeClass('active')
					$('.wx_value').eq(index).find('li:first a').addClass('active');
					set.data[$('.wx_value').eq(index).prev('.wx_key').attr('data-name')]=$('.wx_value').eq(index).find('li:first :hidden').val();
				}
			});
			var Inpvalue = $("#search-hd .search-input").val();
			//添加输入框搜索的名称并显示到已选列表
			if(Inpvalue){
				var condition_media = "关键字:&nbsp;";
				condition[condition_media] = Inpvalue;
			}
			//设置查询参数
			set.data.Key = Inpvalue;
			//还原翻页展示页码
			set.data.PageIndex=1;
			showSelected(condition);
			showStageList(set);
		})
		// 回车键事件
	$('#search-hd .search-input')
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
		url:"/api/ADOrderInfo/QueryAPPByName?v=1_1",
		type:'get',
		data:{
			Name:"正",
			AuditStatus:-4
		},
		dataType:'json'
		},function(data){
			if(data.Status==0){
			_hmt.push(['_setAutoPageview', false]);
			_hmt.push(['_trackPageview', '/OrderManager/wx_list.html']);
			availableTags = [];
			$("#search-hd .search-input").autocomplete({
				source: availableTags
				})
			}
		})
	})
	.on('blur',function(){
		if($(this).val()==""){
			$('#search-hd .pholder').show();
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


	//处理城市部分的逻辑
	//点击常用城市部分的逻辑
	var city_select;
	$("#common").off("click").on("click",function(){
		$(".city").html("");
		setAjax({
			url:"/api/Media/SelectTopTenCitys?v=1_1",
			data:{

			},
			type:"get",
			dataType:"json"
		},function(data){
			console.log(data);
			for(var i=0;i<data.Result.length;i++){
				$(".city").append('<a style="padding:5px 10px;border-radius: 5px;" id="'+data.Result[i].CityID+'">'+data.Result[i].CityName+'</a><input type="hidden" name="" value="'+data.Result[i].CityID+'">');
			}
			$(".city").show();
			//点击城市显现样式并保存
			$(".city a").off("click").on("click",function(){
				// $(".city a").css('color','#666');
				// $(this).css('color','#FF9100');
				$(".city a").removeClass('active');
				$(this).addClass('active');
				// city_select= "定向投放:&nbsp;";
				// condition[city_select] = $(this).html();
				// showSelected(condition);
				// set.data.CityID = $(this).attr("id");
				// showStageList(set);

				opration();
			})
		})
	})
	//点击字母出现
	// console.log(JSonData.masterArea);
	setAjax({
		url:"http://www.chitunion.com/api/Media/SelectPublishCitys?v=1_1",
		type:"get"
	},function(data){
		console.log(data);
		var LetterObject = [];
		// LetterObject.push("常用");
		for(var i=0;i<data.Result.length;i++){
			if(LetterObject.indexOf(data.Result[i].FirstLetter)==-1){
				LetterObject.push(data.Result[i].FirstLetter);
			}
		}
		//每次渲染清空容器
		// $(".etter").html("");
		for(var i=0;i<LetterObject.length;i++){
			//对字母进行去重
			for(var j=0;j<$(".etter span").length;j++){
				if($($(".etter span")[j]).html()==LetterObject[i]){
					$($(".etter span")[j]).remove();
				}
			}
			$(".etter").append('<span>'+LetterObject[i]+'</span>');
		}

		//点击字母
		var flag = true;
		$(".etter span:gt(0)").on("click",function(){
			$(".city").html("");
			for(var i=0;i<data.Result.length;i++){
				//去重显示城市
			if($(this).html()==data.Result[i].FirstLetter){
			// 	flag = true;
			// 	for(var j=0;j<$(".city ul li").length;j++){
			// 		if($($(".city ul li")[j]).html()==SaleArea[i].City){
			// 			flag = false;
			// 		}
			// 	}
				// if(flag){
					$(".city").append('<a style="padding:5px 10px;border-radius: 5px;" id="'+data.Result[i].CityID+'">'+data.Result[i].CityName+'</a><input type="hidden" name="" value="'+data.Result[i].CityID+'">');
				// }
			}

			}
			$(".city").show();
			//点击城市显现样式并保存
			$(".city a").off("click").on("click",function(){
				$(".city a").removeClass('active');
				$(this).addClass('active');
				opration();
			})
		})
		$(".etter span").on("click",function(){
			$(".etter span").removeClass('current');
			$(this).addClass('current');
		})

	})

	opration();
	//整体搜索条件部分的操作
	function opration(){

		// 点击部分查询条件，下部显示条件并刷新页面
		$('.wx_value').each(function(index,obj){
			// 点击条件
			$(obj).unbind('click','a');
			$(obj).on('click','a',function(){
				// 控制条件的样式显示
				if($(obj).attr("city")=="city"){
					$(this).addClass('active').parents('li').siblings('li').find('a').removeClass('active');
				}else{
					$(this).addClass('active').parent('li').siblings('li').find('a').removeClass('active');
				}
				$('.wx_already_wrap').show();
				// 保存选中的条件
				condition[$(this).parents('.wx_value').prev('.wx_key').text()] = $(this).text();
				var id = $(this).next(':hidden').val();
				set.data[$(this).parents('.wx_value').prev().attr('data-name')] = id;
				showSelected(condition);
				set.data.PageIndex=1;
				showStageList(set);
			});
		});
		// 清空按钮
		$('.wx_already_value').on('click','.clearAll',function(){
			// 清空输入框
			$('#search-hd .search-input').val("");
			$(".wx_already_value ul").empty();
			set.data.Key = "";
			condition = {};
			// 上三行
			$('.wx_key').each(function(index,obj){
				if(index !=4){
					$('.wx_value').eq(index).find('li a').removeClass('active');
					$(".etter span").removeClass('current');
					$('.wx_value').eq(index).find('li:first a').addClass('active');
					set.data[$('.wx_value').eq(index).prev('.wx_key').attr('data-name')]=$('.wx_value').eq(index).find('li:first :hidden').val();
				}
			});
			set.data.PageIndex=1;
			showSelected(condition);
			showStageList(set);
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
				set.data.Key = "";
				$("#search-hd .search-input").val("");
			}
			var key = $(this).siblings('b').text();
			// 上三行
			$('.wx_key').each(function(index,obj){
				if($(obj).text() == key){
					$('.wx_value').eq(index).find('li a').removeClass('active');
					if($(obj).attr("city")=="city"){
						$(".etter span").removeClass('current');
					}
					$('.wx_value').eq(index).find('li:first a').addClass('active');
					set.data[$('.wx_value').eq(index).prev('.wx_key').attr('data-name')]=$('.wx_value').eq(index).find('li:first :hidden').val();
				}
			});
			showSelected(condition);
			set.data.PageIndex=1;
			showStageList(set);
		});
	}
	// 排序按钮
	$('.wx_table1 .fanscount').unbind('click');
	$('.wx_table1 .fanscount').on('click',function(){
		var img = $(this).find('img').attr('src');
		$(this).addClass('yellow');
		if(img && img=='/images/icon16_c.png'){
			$(this).find('img').attr('src','/images/icon16_b.png');
			set.data.Orderby = 2;
			set.data.PageIndex=1;
			showStageList(set);
		}else if(img && img=='/images/icon16_a.png'){ //倒叙排列
			$(this).find('img').attr('src','/images/icon16_b.png');
			set.data.Orderby = 2;
			set.data.PageIndex=1;
			showStageList(set);
		}else if(img && img=='/images/icon16_b.png'){
			$(this).find('img').attr('src','/images/icon16_a.png');
			set.data.Orderby = -2;
			set.data.PageIndex=1;
			showStageList(set);
		}
	});
	//请求购物车中的数量
	setAjax({
		url:"/api/ShoppingCart/GetInfo_ShoppingCart?v=1_1",
		type:"get",
		data:{
			MediaType:set.data.businesstype
		}
	},function(data){
		if(data.Status==0){
			_hmt.push(['_setAutoPageview', false]);
			_hmt.push(['_trackPageview', '/OrderManager/wx_list.html']);
			console.log(data);
			if(data.Result){
				var shopcount1 = 0;
				var shopcount2 = 0;
				for(var i=0;i<data.Result.SelfMedia.length;i++){
					for(var j=0;j<data.Result.SelfMedia[i].Medias.length;j++){
						shopcount1++;
					}
				}
				for(var i=0;i<data.Result.APP.length;i++){
          for(var j=0;j<data.Result.APP[i].Medias.length;j++){
            shopcount2++;
          }
        }
				$('.cart_num').html(shopcount1+shopcount2);

			}
		}
	})
	//跳转到购物车页面
	$("#quick_links").on("click",function(){
		if($('.cart_num').html()>0){
			window.location = "/OrderManager/shopcartForMedia01.html";
		}else{
			layer.msg("请添加广告位！",{time:2000});
		}
	})
	// //筛选是否可接单
	// $(".wx_check input").on("change",function(){
	// 	if($(".wx_check input").get(0).checked){
	// 		set.data.CanReceive=true;
	// 		set.data.PageIndex=1;
	// 		showStageList(set);
	// 	}else{
	// 		set.data.CanReceive=false;
	// 		set.data.PageIndex=1;
	// 		showStageList(set);
	// 	}
	// })

	//处理大小
	$(".mui-mbar-tabs").css("width","40px");

	// 回到顶部
	$(document).on('scroll',function(){
		if($(window).scrollTop() > $(window).height()*2/3){
			$('.top').show();
			$(".quick_toggle").addClass('quick_links_allow_gotop');
		}else{
			$('.top').css('display','none');
			$(".quick_toggle").removeClass('quick_links_allow_gotop');
		}
	});
	$('.top').on('click',function(){
		$('body,html').animate({scrollTop:0},500);
	});





})
/*--------------------------------------------公共方法*/
/*获取url的参数*/
function GetUserId() {
    var url = location.search; //获取url中"?"符后的字串
    var theRequest = {};
    if (url.indexOf("?") != -1) {
        var str = url.substr(1);
        strs = str.split("&");
        for (var i = 0; i < strs.length; i++) {
            theRequest[strs[i].split("=")[0]] = strs[i].split("=")[1];
        }
    }
    return theRequest;
}
// // 获取url参数
// var search = window.location.href.split('?')[1];
// var search_obj={};
// if(search){
// 	$(search.split('&')).each(function(index,obj){
// 		search_obj[obj.split('=')[0]] = obj.split('=')[1];
// 	});
// 	var orderID = search_obj.orderID // 获得订单id
// 	var userID =  search_obj.userID;
// 	var OrderState = search_obj.OrderState;
// }else{
// 	var orderID // 获得订单id
// 	var userID;
// 	var OrderState;
// }
// 排序函数
function sort(){
	$(this).parent('li').siblings('li').find('a').removeClass('active');
	$(this).parent('li').siblings('li:gt(2)').find('img').attr('src','../../images/icon16_c.png');
	$(this).parent('li').siblings('li').find('a').attr('is_click','');
	$(this).addClass('active');
	var Orderby = $(this).attr('data-Orderby');
	var is_click = $(this).attr('is_click');
	if($(this).text() != '默认排序'){
		if(!is_click){
			set.data.Orderby = 2;
			$(this).attr('is_click','1');
		}else{
			set.data.Orderby = -2;
			$(this).attr('is_click','');
		}
	}else{
		set.data.Orderby = -2;
	}
	//切换选中的状态
	if($(this).attr('is_click')){
		$(this).find('img').attr('src','../../images/icon16_b.png');
	}else{
		$(this).find('img').attr('src','../../images/icon16_a.png');
	}
	set.data.PageIndex=1;
	showStageList(set);
}

// 已选条件的显示
function showSelected(condition){
	var condition_str = '';
	// 设置已选条件的显示
	for(var i in condition){
		condition_str+= '<li><b style="font-weight:normal;">'+i+'</b><span>'+condition[i]+'</span><img src="../images/close.png"><input type="hidden" name="" value=""></li>';
	}
	if(!$.isEmptyObject(condition)){
		condition_str +='<li class="clearAll"><a href="javascript:void(0)">清空条件</a></li>';
		$('.wx_already_value ul').html(condition_str);
		$(".wx_already_value ul li").last().prev().find("img").attr("src","../images/close_h.png");
		$(".wx_already_wrap").show();
	}else{
		$(".wx_already_wrap").hide();
	}
}
// 列表数据渲染
function showStageList(set){
	$(' .sorting li a').unbind('click');
	if(set.data.FansCount==-2&&set.data.Price==-2){
		set.data.FansCount="";
		set.data.Price="";
	}
	setAjax({
		selector:'.data',
		url:set.url,
		type:'get',
		data:set.data
	},
	function(data){
		console.log(data);
		$('.sorting li:gt(1) a').on('click',sort);
		if(data.Status == 0){
			_hmt.push(['_setAutoPageview', false]);
			_hmt.push(['_trackPageview', '/OrderManager/wx_list.html']);
			$('.totalCount').text(data.Result.Total);
			if(data.Result.Total != 0){
				//展示不同的视图
				if($(' .sorting ul li').eq(0).find("img").attr("src")=="../images/icon69.png"){
					$('.data').html(ejs.render($('#wx_grid').html(),{data:data.Result.List}));
					$(".data .box").on("click",function(){
						 MediaID = $(this).find(".hideinfo .MediaID").val();
						 TemplateID = $(this).find(".hideinfo .TemplateID").val();
						 jumpdetail();
					})
				}else if($(' .sorting ul li').eq(1).find("img").attr("src")=="../images/icon70.png"){
					$('.data').html(ejs.render($('#wx_list').html(),{data:data.Result.List}));
						$(".wx_table2").on("click",function(){
							MediaID = $(this).find(".hideinfo .MediaID").val();
						 TemplateID = $(this).find(".hideinfo .TemplateID").val();
							 jumpdetail();
					})
				}
				//点击切换展示样式
				$("#list").on('click',function(){
					$("#grid").find("img").attr("src","../images/icon68.png")
					$(this).find("img").attr("src","../images/icon70.png");
					$('.data').html(ejs.render($('#wx_list').html(),{data:data.Result.List}));
						$(".wx_table2").on("click",function(){
							MediaID = $(this).find(".hideinfo .MediaID").val();
						 TemplateID = $(this).find(".hideinfo .TemplateID").val();
							 jumpdetail();
					})
				});
				$("#grid").on('click',function(){
					$("#list").find("img").attr("src","../images/icon71.png")
					$(this).find("img").attr("src","../images/icon69.png");
					$('.data').html(ejs.render($('#wx_grid').html(),{data:data.Result.List}));
					$(".data .box").on("click",function(){
						MediaID = $(this).find(".hideinfo .MediaID").val();
					  TemplateID = $(this).find(".hideinfo .TemplateID").val();
						 jumpdetail();
					})
				});
				//分页部分
				$("#pageContainer").pagination(
					data.Result.Total,
					{
              // items_per_page: 2, //每页显示多少条记录（默认为20条）
              	callback: function (currPage, jg) {
              	set.data.PageIndex = currPage;
              	$(' .sorting li a').unbind('click');
              	setAjax({
									selector:'.data',
									url:set.url,
									type:'get',
									data:set.data
								},
	           		function(data){  //成功
	           			$(' .sorting li:gt(1) a').on('click',sort);
									if(data.Status==0){
										_hmt.push(['_setAutoPageview', false]);
										_hmt.push(['_trackPageview', '/OrderManager/wx_list.html']);
		           			if(data.Result.List){
											//展示不同的视图
											if($('.sorting ul li').eq(0).find("img").attr("src")=="../images/icon69.png"){
												$('.data').html(ejs.render($('#wx_grid').html(),{data:data.Result.List}));
												$(".data .box").on("click",function(){
													MediaID = $(this).find(".hideinfo .MediaID").val();
												 TemplateID = $(this).find(".hideinfo .TemplateID").val();
													jumpdetail();
												})
											}else if($(' .sorting ul li').eq(1).find("img").attr("src")=="../images/icon70.png"){
												$('.data').html(ejs.render($('#wx_list').html(),{data:data.Result.List}));
												$(".wx_table2").on("click",function(){
													MediaID = $(this).find(".hideinfo .MediaID").val();
												 TemplateID = $(this).find(".hideinfo .TemplateID").val();
													jumpdetail();
											})
											}
											//点击切换展示样式
											$("#list").on('click',function(){
												$("#grid").find("img").attr("src","../images/icon68.png")
												$(this).find("img").attr("src","../images/icon70.png");
												$('.data').html(ejs.render($('#wx_list').html(),{data:data.Result.List}));
													$(".wx_table2").on("click",function(){
														MediaID = $(this).find(".hideinfo .MediaID").val();
													 TemplateID = $(this).find(".hideinfo .TemplateID").val();
														 jumpdetail();
												})
											});
											$("#grid").on('click',function(){
												$("#list").find("img").attr("src","../images/icon71.png")
												$(this).find("img").attr("src","../images/icon69.png");
												$('.data').html(ejs.render($('#wx_grid').html(),{data:data.Result.List}));
												$(".data .box").on("click",function(){
													MediaID = $(this).find(".hideinfo .MediaID").val();
												 TemplateID = $(this).find(".hideinfo .TemplateID").val();
													 jumpdetail();
												})
											});
		           			}
									}
	           		});
              }
            });
			}else{
				//点击切换展示样式
				$("#list").on('click',function(){
					$("#grid").find("img").attr("src","../images/icon68.png")
					$(this).find("img").attr("src","../images/icon70.png");
				});
				$("#grid").on('click',function(){
					$("#list").find("img").attr("src","../images/icon71.png")
					$(this).find("img").attr("src","../images/icon69.png");
				});
				$('#pageContainer').html('');
				$('.data').html('<img src="../images/no_data.png" style="display:block;margin:20px auto;">');
			}
		}
	});
}
// 枚举信息的渲染
function GetDictInfo(container,dom){
	setAjax({
		url:'/api/DictInfo/GetDictInfoByAPP',
		type:'get'
	},
	function(data){
		if(data.Status==0){
			_hmt.push(['_setAutoPageview', false]);
			_hmt.push(['_trackPageview', '/OrderManager/wx_list.html']);
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
			$(container).html(str);	//如果长度不够隐藏展开按钮
				setTimeout(function(){
					if($(".catagory li").length<16){
						$(".wx_ext").hide();
					}
				},0)
		}
	});
}
//跳转详情页面
function jumpdetail(){
	var userData = GetUserId();
	// var isAdd = userData.hasOwnProperty("isAdd") ? 1 : "";
	// var orderID = userData.hasOwnProperty("orderID") ? userData["orderID"] : "";
	// var userID =  userData.hasOwnProperty("userID") ? userData["userID"] : "";
	window.location ="/OrderManager/app_detail.html?MediaID="+MediaID+"&TemplateID="+TemplateID;
}
//跳转购物车页面
function jumpshop(){
	var userData = GetUserId();
	var isAdd = userData.hasOwnProperty("isAdd") ? 1 : "";
	var orderID = userData.hasOwnProperty("orderID") ? userData["orderID"] : "";
	var userID =  userData.hasOwnProperty("userID") ? userData["userID"] : "";
	window.location ="/OrderManager/shopcartForWeixin_new01.html?isAdd="+isAdd+"&orderID="+orderID+"&userID="+userID+"&MediaType="+set.data.businesstype;
}
