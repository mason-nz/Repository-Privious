/*
* Written by:     fengb
* function:       漏斗分析
* Created Date:   2017-11-21
*/

$(function () {
	
	var funnelAnalysis = {
		init : function(){
			setTimeout(function(){
				$('#list_switching li').eq(0).click();
			},50);
			
			$('#list_switching li').off('click').on('click',function(e){
				e.preventDefault();
				var that = $(this);
				var index = that.index();
				that.addClass('selected').siblings().removeClass('selected');
				$('#LatelyDays span').eq(0).addClass('selected').siblings().removeClass('selected');

				//近7天 近30天
				$('#LatelyDays span').off('click').on('click',function(e){
					e.preventDefault();
					var that = $(this);
					var DateType = that.attr('name')*1;
					//var BeginTime = getthedate(new Date(),-7);
					//var EndTime = getthedate(new Date(),-30);
					that.addClass('selected').siblings().removeClass('selected');
					//漏斗明细
					funnelAnalysis.getAnalyData(index,DateType);
					//列表数据
					funnelAnalysis.getListData(index,DateType);
				})
				$('#LatelyDays span').eq(0).click();

				if(index == 0 || index == 1){//头腰部
					$('.analysis_con').show();
					$('.material_con').hide();
				}else if(index == 2){//物料
					$('.analysis_con').hide();
					$('.material_con').show();
				}
			})
		},
		getAnalyData : function(index,DateType){//漏斗明细
			var TabType = 'head';
			if(index == 0 || index == 1){
				if(index == 0){
					TabType = 'head';
				}else if(index == 1){
					TabType = 'body';
				}
				setAjax({
					url : public_url + '/api/Funnel/GetArticleChart',
					type : 'get',
					data : {
						LatelyDays : DateType,
						TabType : TabType
					}
				},function(data){
					if(data.Status == 0){
						var Result = data.Result;
						var Info = Result.Info;
						if(Info){
							$('.analysis_con').html(ejs.render($('#ArticleChart').html(),Result));
							if(index == 0){
								$('.AccountCount').show();
								$('.analysis_tit .tit').html('头部文章封装流程转化');
							}else if(index == 1){
								$('.analysis_tit .tit').html('腰部文章封装流程转化');
								$('.AccountCount').hide();
							}
						}else{
							$('.analysis_con').html('<img src="/ImagesNew/no_data.png" style="display: block;margin: 0 auto;">');
						}
					}else{
						layer.msg(data.Message,{'time':1000});
					}
				})
			}else if(index == 2){
				setAjax({
					url : public_url + '/api/Funnel/GetMaterielChart',
					type : 'get',
					data : {
						LatelyDays : DateType
					}
				},function(data){
					if(data.Status == 0){
						var Result = data.Result;
						var Info = Result.Info;
						if(Info){
							var Result = [];
							var arr1 = [];
							var arr2 = []; 
							for(var i = 0;i<= Info.length-1;i ++){
								if(Info[i].TypeId == 1){
									arr1.push(Info[i]);
								}
								if(Info[i].TypeId == 2){
									arr2.push(Info[i]);
								}
							}
							Result.push(arr1);
							Result.push(arr2);
							$('.material_con').html(ejs.render($('#MaterielChart').html(),{List : Result}));
						}else{
							$('.material_con').html('<img src="/ImagesNew/no_data.png" style="display: block;margin: 0 auto;">');
						}
					}else{
						layer.msg(data.Message,{'time':1000});
					}
				})
			}
		},
		getListData : function(idx,DateType){
			//默认加载场景
			var Operator = 1;

			//按钮元素和URL
			var elm = ['Head','Waist','Material'];
			var url = ['/api/Funnel/GetFunnelHeadDetailList','/api/Funnel/GetFunnelWaistDetailList','/api/Funnel/GetFunnelMaterialDetailList'];
			
			$('.TabBox').hide();
			$('.TabBox').eq(idx).show();
			funnelAnalysis.clickPublic(elm[idx],url[idx],DateType,Operator,idx);

		},
		clickPublic : function(elm,url,DateType,Operator,index){
			$('#' + elm).find('span').off('click').on('click',function(event){
				event.preventDefault();
				var that = $(this);
				var idx = that.index();
				Operator = that.attr('name')*1;
				that.addClass('selected').siblings().removeClass('selected');
				$.ajax({
					url : public_url + url,
					type : 'get',
					data : {
						DateType : DateType,
						Operator : Operator
					},
					xhrFields: {
			            withCredentials: true
			        },
					async : false,
					beforeSend: function(){
	                    $('.listLoading').html('<img src="/ImagesNew/icon_loading.gif" style="display: block;margin: 70px auto;">');
	                },
					success : function(data){
						$('.listLoading').html('');
						if(data.Status == 0){
							var Result = data.Result;
                        	if(Result.List.length > 0){
	                            $('.' + elm + 'Con').find('tbody').html(ejs.render($('#'+ elm +'DetailList').html(), Result));
								//tab条件控制
								funnelAnalysis.checkedTab(elm,index,idx);
	                        }else{
	                        	$('.' + elm + 'Con').find('tbody').html('');
	                            $('.listLoading').html('<img src="/ImagesNew/no_data.png" style="display: block;margin: 70px auto;">');
	                        }
	                        //下载数据
							funnelAnalysis.downloadData(elm,DateType,Operator,index);
						}else{
							layer.msg(data.Message);
						}
					}
				})
			})
			$('#' + elm).find('span').eq(0).click();
		},
		checkedTab : function(elm,index,idx){
			//index 表示当前头部tab的类型  idx为当前tab下面列表tab的index
			if(index == 0 || index == 2){//头部文章和物料
				if(idx == 0){//场景
					$('.' + elm + 'Con').find('.SceneName').show();
					$('.' + elm + 'Con').find('.ChannelName').hide();
					$('.' + elm + 'Con').find('.AAScoreTypeName').hide();
				}else if(idx == 1){//渠道
					$('.' + elm + 'Con').find('.SceneName').hide();
					$('.' + elm + 'Con').find('.ChannelName').show();
					$('.' + elm + 'Con').find('.AAScoreTypeName').hide();
				}else if(idx == 2){//分值
					$('.' + elm + 'Con').find('.SceneName').hide();
					$('.' + elm + 'Con').find('.ChannelName').hide();
					$('.' + elm + 'Con').find('.AAScoreTypeName').show();
				}
			}else if(index == 1){//腰部文章
				if(idx == 0){
					$('.WaistCon .ArticleTypeName').show();
					$('.WaistCon .ChannelName').hide();
				}else if(idx == 1){
					$('.WaistCon .ArticleTypeName').hide();
					$('.WaistCon .ChannelName').show();
				}
			}
		},
		downloadData(elm,DateType,Operator,index){
			var url = ['/api/Funnel/FunnelHeadExport','/api/Funnel/FunnelWaistExport','/api/Funnel/FunnelMaterialExport'];
			$('#' + elm + 'Download').off('click').on('click',function(event){
				event.preventDefault();
				setAjax({
					url : public_url + url[index],
					type : 'get',
					data : {
						DateType : DateType,
						Operator : Operator
					}
				},function(data){
					if(data.Status == 0){
						var Result = data.Result;
						window.open(Result.Url);
					}else{
						layer.msg(data.Message,{'time':1000});
					}
				})
			})
		}		
	}
	funnelAnalysis.init();	
})



/* 获取  昨天  最近七天  */
function getthedate(dd,dadd){
	//可以加上错误处理
	var a = new Date(dd)
	a = a.valueOf()
	a = a + dadd * 24 * 60 * 60 * 1000
	a = new Date(a);
	var m = a.getMonth() + 1;
	if(m.toString().length == 1){
	    m='0'+m;
	}
	var d = a.getDate();
	if(d.toString().length == 1){
	    d='0'+d;
	}
	return a.getFullYear() + "-" + m + "-" + d;
}

