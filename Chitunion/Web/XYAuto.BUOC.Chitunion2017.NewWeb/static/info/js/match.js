/*
* Written by:     fengb
* function:       二级搜索页面
* Created Date:   2017-12-19
* Modify  Date:   2017-02-06
*/
$(function (){
	//更换icon
	$('.match_con1 .SearchIcon li').on('mouseover',function(){
		var that = $(this);
		var idx = that.index();
		var _img = that.find('img');
		if(idx == 0){
			_img.attr('src','/images/icons/icon_wx1.png');
		}else if(idx == 1){
			_img.attr('src','/images/icons/icon_wb1.png');
			$('.match_con1 .SearchIcon li').eq(0).find('img').attr('src','/images/icons/icon_wx.png');
		}else if(idx == 2){
			_img.attr('src','/images/icons/icon_app1.png');
			$('.match_con1 .SearchIcon li').eq(0).find('img').attr('src','/images/icons/icon_wx.png');
		}
	}).on('mouseout',function(){
		var that = $(this);
		var idx = that.index();
		var _img = that.find('img');
		if(idx == 0){
			_img.attr('src','/images/icons/icon_wx.png');
		}else if(idx == 1){
			_img.attr('src','/images/icons/icon_wb.png');
		}else if(idx == 2){
			_img.attr('src','/images/icons/icon_app.png');
		}
		$('.match_con1 .SearchIcon li').eq(0).find('img').attr('src','/images/icons/icon_wx1.png');
	})


	//icon搜索
	$('.SearchIcon li').off('click').on('click',function(){
		var that = $(this);
		var idx = that.index();
		var val = encodeURI($.trim($('._input').val()));
		window.location = '/static/advertister/sort_list.html?type=' + idx + '&keyword=' + val;
		$('._input').val('');//清空
	})

	//按钮搜索
	$('#btn_search').off('click').on('click',function(){
		var val = encodeURI($.trim($('._input').val()));
		window.location = '/static/advertister/sort_list.html?type=0' + '&keyword=' + val;
	})

	//enter搜索
	$(".search_input ._input").keydown(function(evt) {
		evt = evt = (evt) ? evt : ((window.event) ? window.event : "");
    	if(evt.keyCode == "13") {//keyCode=13是回车键
        	$('#btn_search').click();
    	}
    });


	//立即体验
	$('.go_deliery').off('click').on('click',function(){
		var that = $(this);
		window.location = '/static/advertister/sort_list.html';
	})	

})