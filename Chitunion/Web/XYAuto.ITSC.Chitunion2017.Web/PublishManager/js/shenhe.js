setAjax({url:'js/data.json',
		type:'get',
		data:{
			PubID:1,
			AdPosition:"",
			AdForm:"",
			Style:"",
			PublishStatus:0,
			pagesize:20,
			PageIndex:1
		}},
		function(data){
			console.log(data )
			bindHTML(data);

		}
);
//
//
function bindHTML(data){
	var str = $("#shenhe").html();
	var result = ejs.render(str,{data:data});
	$('table').html(result);
};
//
//
//$('#but_add2').click(function(){
//	var adposition = $('#adposition').val();
//	var adstyle = $('#adstyle').val();
//	var styles = $('#style').val();
//	if(adposition=='' && adstyle=='' && styles==''){
//		alert('条件必须填写一个')
//		return;
//	}
//	//发送数据请求
//	setAjax({url:'js/data.json',
//			type:'get',
//			data:{
//				PubID:1,
//				AdPosition:"adposition",
//				AdForm:"adstyle",
//				Style:"styles",
//				PublishStatus:0,
//				pagesize:20,
//				PageIndex:1
//			}},
//			function(data){
////					console.log(data);
//				var res = searchData(data);
////					console.log(res)
//				var str = $("#shenhe").html();
//				var results = ejs.render(str,{data:data});
//				$('table').html(results);
//			}
//	);
//	function searchData(obj){
//		var keywords = {},rules = '',searchArr = [];
//		//读取要查询的字段
//		for(var i=0;i<$('.such li input').length;i++){
////				console.log($('.such li input').eq(i).val())
//			if($('.such li input').eq(i).val() == '');
//			keywords[$('.such li input').eq(i).attr('id')] = $('.such li input').eq(i).val();
//		}
//		
//		//多个条件之间为且的关系
//		for(var attr in keywords){
//			rules += 'obj[attr].'+attr+'=='+'keywords.'+attr+' && ';
//		}
//		
//		//遍历匹配
//		for(var attr in obj){
//			if(eval(rules.slice(0,-4))){
//				searchArr.push(obj[attr]);
//			}
//		}
//		
//		return searchArr;
//	}
//})