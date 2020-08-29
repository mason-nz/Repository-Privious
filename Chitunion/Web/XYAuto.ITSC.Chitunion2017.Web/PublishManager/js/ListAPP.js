/*
*/
	setAjax({url:'/api/Periodication/SelectAppAdvListByPubID',
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
			status();
			anniu();
		}
	);

	function bindHTML(data){
		var str = $("#user").html();
		var result = ejs.render(str,{data:data});
		$('table').html(result);
	};
	//添加广告位
	$(".mb15 .but_add").eq(0).click(function(){
		alert(1)
		window.location.href="https://www.baidu.com"
	})
	//上架
	$(".such li").eq(5).click(function(){
		alert(2)
		window.location.href=""
	})
	//查询
	$('#but_add2').click(function(){
		var adposition = $('#adposition').val();
		var adstyle = $('#adstyle').val();
		var styles = $('#style').val();
		if(adposition=='' && adstyle=='' && styles==''){
			alert('条件必须填写一个')
			return;
		}
		//发送数据请求
		setAjax({url:'js/data.json',
				type:'get',
				data:{
					PubID:1,
					AdPosition:"adposition",
					AdForm:"adstyle",
					Style:"styles",
					PublishStatus:0,
					pagesize:20,
					PageIndex:1
				}},
				function(data){
//					console.log(data);
					var res = searchData(data);
//					console.log(res)
					var str = $("#users").html();
					var results = ejs.render(str,{data:data});
					$('table').html(results);
				}
		);
		function searchData(obj){
			var keywords = {},rules = '',searchArr = [];
			//读取要查询的字段
			for(var i=0;i<$('.such li input').length;i++){
//				console.log($('.such li input').eq(i).val())
				if($('.such li input').eq(i).val() == '');
				keywords[$('.such li input').eq(i).attr('id')] = $('.such li input').eq(i).val();
			}

			//多个条件之间为且的关系
			for(var attr in keywords){
				rules += 'obj[attr].'+attr+'=='+'keywords.'+attr+' && ';
			}

			//遍历匹配
			for(var attr in obj){
				if(eval(rules.slice(0,-4))){
					searchArr.push(obj[attr]);
				}
			}

			return searchArr;
		}
	})

	function anniu(){
		$('.editor').click(function(){
			alert(4)
		})
		$('.shangjia').click(function(){
			alert(9)
			$('.updown').html("待审核")
		})
	}

	//分页
//	var NewPage = new PaginationController({
//      WrapContainer:"#pageContainer",
//      MaxPage:150,
//      PageItemCount:20,
//      ControllerCount:7,
//      CallBack:function(currentPageIndex,callback){
//      	setAjax({url:'js/data.json',
//				type:'get',
//				data:{
//					PubID:1,
//					AdPosition:"",
//					AdForm:"",
//					Style:"",
//					PublishStatus:0,
//					pagesize:20,
//					PageIndex:currentPageIndex
//				}},
//				function(data){
//					bindHTML(data);
//					one();
//					anniu();
//					callback(true);
//				}
//			)
//      }
//  });
//  NewPage.createPageItemFu(1)

	//status
	//id:媒体名称‘
	//statusid:状态
	status();
	function status(statusid){
		var statusid={
			one:"状态：新建",
			two:"状态：待审核",
			three:"状态：驳回",
			four:"状态：审核通过"
		}
		if($('.order_r .such li').eq(4).html() == statusid.one){
			$('.order_r .such').eq(1).find('li').eq(3).css('visibility','hidden');
			$('.assign').hide();
			$('tr .tdfirst').hide();
			$('tr .updown').hide();
		}else if($('.order_r .such li').eq(4).html() == statusid.two){
			$('.order_r .such').eq(1).find('li').eq(3).css('visibility','hidden');
			$('tr .last .lasta').siblings().hide();
			$('.assign').hide();
			$('tr .tdfirst').hide();
			$('tr .updown').hide();
		}else if($('.order_r .such li').eq(4).html() == statusid.three){
			$('.order_r .such').eq(1).find('li').eq(3).css('visibility','hidden');
			$('tr .last .shangjia').hide();
			 $('.assign').hide();
			$('tr .tdfirst').hide();
			$('tr .updown').hide();
		}else if($('.order_r .such li').eq(4).html() == statusid.four){
			$('tr .updown').show();
			if($('tr .updown').html()=="上架"){
				$('td .shangjia').html("下架")
			}else if($('tr .updown').html()=="下架"){
				$('td .shangjia').html("上架")
			}
		}
	}
	//接受刊例列表传过来的参数
	function GetUrlParms()
		{
		 var args=new Object();
		 var query=location.search.substring(1);//获取查询串
		 var pairs=query.split("&");//在逗号处断开
		 for(var i=0;i<pairs.length;i++)
		 {
		  var pos=pairs[i].indexOf('=');//查找name=value
		   if(pos==-1) continue;//如果没有找到就跳过
		   var argname=pairs[i].substring(0,pos);//提取name
		   var value=pairs[i].substring(pos+1);//提取value
		   args[argname]=unescape(value);//存为属性
		 }
		 return args;
		}
		var args = new Object();
		args = GetUrlParms();
		//如果要查找参数key:
		if(args["id"]!=undefined)
		{
		//如果要查找参数key:
		var value1 = args["id"] ;
		alert(value1);
		}
