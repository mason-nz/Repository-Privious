//首页显示隐藏
$(function(){
	$('.top_open').hover(function(){
		$(this).children("a").addClass('top_on').removeClass('csbg');
		$(this).children("ul").stop(true,true).show()
	},function(){
		$(this).children("a").removeClass('top_on').addClass('csbg');
		$(this).children("ul").hide()
	});
})
$(function(){
	$('.top_open').hover(function(){
		$(this).children("a").addClass('top_on').removeClass('csbg2');
		$(this).children("ul").stop(true,true).show()
	},function(){
		$(this).children("a").removeClass('top_on').addClass('csbg2');
		$(this).children("ul").hide()
	});
})

$(function(){
	$('.top_open').hover(function(){
		$(this).children("a").addClass('top_on').removeClass('csbg3');
		$(this).children("ul").stop(true,true).show()
	},function(){
		$(this).children("a").removeClass('top_on').addClass('csbg3');
		$(this).children("ul").hide()
	});
})



















