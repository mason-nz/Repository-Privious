/*
* Written by:     fengb
* function:       数据首页
* Created Date:   2017-11-16
*/

$(function () {
	//默认所有的颜色
	var TheChartsColorArr = [{name : '微信公众号' ,itemStyle : {normal : {color : '#F5758C'} }},
		{name : '汽车之家' ,itemStyle : {normal : {color : '#FDCD82'} }},	
		{name : '今日头条' ,itemStyle : {normal : {color : '#FF5050'} }},	
		{name : '网易汽车' ,itemStyle : {normal : {color : '#8EB2E2'} }},
		{name : '行圆新闻后台' ,itemStyle : {normal : {color : '#91D4F5'} }}, 
		{name : '搜狐自媒体' ,itemStyle : {normal : {color : '#BFE7B5'} }},
		{name : '车顾问' ,itemStyle : {normal : {color : '#CEB6D8'} }},
		{name : '全网域' ,itemStyle : {normal : {color : '#B3D0EE'} }},
		{name : '汽车大全m站' ,itemStyle : {normal : {color : '#FF9593'} }},
		{name : '赤兔联盟' ,itemStyle : {normal : {color : '#60B199'} }},
		{name : '微博易' ,itemStyle : {normal : {color : '#9FC379'} }},
		{name : '头腿' ,itemStyle : {normal : {color : '#96b2e1'} }},
		{name : '头腰腿' ,itemStyle : {normal : {color : '#98ccda'} }},
		{name : '头腰' ,itemStyle : {normal : {color : '#99e1c9'} }},
		{name : '腰腿' ,itemStyle : {normal : {color : '#c1edba'} }},
		{name : '头' ,itemStyle : {normal : {color : '#f8cda4'} }},
		{name : '腿' ,itemStyle : {normal : {color : '#f6e7c6'} }},
		{name : '总计' ,itemStyle : {normal : {color : '#F0A5B9'} }} 
	]
									
										
	//柱状图和饼图
	function getOptionsAll(elem,name,TotalCount,XLine,Data,positionBar){
		options = {
			tooltip : {
                trigger:  'axis',
                axisPointer : {            // 坐标轴指示器，坐标轴触发有效
			        type : 'shadow'        // 默认为直线，可选为：'line' | 'shadow'
			    }
		    },
		    legend : {
		        x  :  'center',
                y  :  'bottom',
                data : name,
                selectedMode : false
		    },
		    /*grid : [//柱状图的坐标
	            {  top : '20%',x: '35%',  y: '7%',  width: '64%',  height:  '60%'}
	        ],*/
	        grid : positionBar,
		    /*graphic:{//圆环内的总计
	            type: 'text',
	            left: '14%',
	            top: '50%',
	            style:{
	                text: TotalCount?TotalCount:'',
	                textAlign: 'center',
	                fill: '#333',
	                font: 'bolder 14px Microsoft YaHei'
	            }
	        },*/
	        xAxis : [
		        {
		            type : 'category',
		            data : XLine,
		            axisLabel: { // x轴的字体样式       
                        textStyle: {
                            color: '#5d6a6f'
                        }
                    },
		            axisLine:{// x轴的颜色和宽度
                        lineStyle:{
                            color:'#dfd9d9',
                        }
                	}
		        }
		    ],
		    yAxis : [
		        {
		            type : 'value',
		            axisLine:{
                        lineStyle:{
                            color:'#333',
                        }
                	},
                	axisLabel: { // y轴的字体样式       
                        textStyle: {
                            color: '#5d6a6f'
                        }
                    }, 
                    axisLine:{// y轴的颜色和宽度
                        lineStyle:{
                            color:'#dfd9d9',
                        }
                	},
                	
			        splitLine: {// 控制网格线样式
		                lineStyle: {
		                     color: '#dfd9d9'// 修改网格线颜色     
		                }                            
			        }
		        }
		    ],
		   	series : Data
	    }
	    if(Data.length != 0 ){
 			echarts.init(elem.get(0)).clear();
			echarts.init(elem.get(0)).setOption(options);
 		}else{
 			$(elem).html('<p style="text-align:center;line-height:300px;">暂无数据~</p>');
 		}
	}


	//定义图表类型 参数
	var Category = ['grab_head','jx_head','grab_body','jx_body','cxpp_body','wlfz','wlff','zf','xshq'];

	var myDataChart = {
		originInit : function(){
			//昨日数据统计
			myDataChart.historyDate();
			//近7天  近30天
			$('.data_files .data_tab>span').off('click').on('click',function(e){
				e.preventDefault();
				var that = $(this);
				var LatelyDays = that.attr('LatelyDays')*1;
				that.addClass('selected').siblings().removeClass('selected');

				//图表加载
				myDataChart.initChart(LatelyDays,0);
			})
			//默认点击
			setTimeout(function(){
				$('.data_files .data_tab>span').eq(0).click();
			},50);
		},
		historyDate : function(){
			//昨日数据统计
			setAjax({
				url : public_url + '/api/DataView/History',
				type : 'get',
				data : {
					LatelyDays : -1
				}
			},function(data){
				if(data.Status == 0){
					var Result = data.Result;
					if(!Result){
						Result = {
							"HeadArticle":0,
					        "HeadArticleAccount":0,
					        "HeadAutoClean":0,
					        "HeadAutoCleanAccount":0,
					        "WaistArticle":0,
					        "WaistArticleClean":0,
					        "WaistArticleMatched":0,
					        "WaistArticleUnmatched":0,
					        "MaterialPackaged":0,
					        "MaterialDistribute":0,
					        "MaterialForward":0,
					        "Clues":0
						}
					}
					$('#HistoryCon').html(ejs.render($('#History').html(), Result));
				}
			})
			//tip展示
			$('.data_files .ad_table thead tr img').on('mouseover',function(){
				var that = $(this);
				var idx = that.parent().index();
				that.css({'cursor':'pointer'});
				that.attr('src','/ImagesNew/tip.png');
				$('.data_files .ad_table tbody tr td').eq(idx).find('.tip').show();
			}).on('mouseout',function(){
				var that = $(this);
				var idx = that.parent().index();
				that.attr('src','/ImagesNew/tip2.png');
				$('.data_files .ad_table tbody tr td').eq(idx).find('.tip').hide();
			})
		},
		initChart : function(days,typ){

			var a = 0;

			//切换日期的时候控制下拉框
			$('._option').find('option').eq(0).prop('selected',true);
			//默认加载头部文章抓取
			myDataChart.loadDate(days,Category[typ],0);

			var flag1 = 1;
			var flag2 = 1;
			var flag3 = 1;
			var flag4 = 1;
			var flag5 = 1;
			var flag6 = 1;
			var flag7 = 1;
			var flag8 = 1;

			//滚动条
			$(window).unbind('scroll');
			$(window).scroll(function () {
			    var scrollTop = $(this).scrollTop();
			    if(scrollTop + 600*1 > $('.data_item').eq(8).offset().top && flag8){
			    	myDataChart.loadDate(days,Category[8],8);
			    	flag8 = 0;
			    }else if(scrollTop + 600*1 > $('.data_item').eq(7).offset().top && flag7){
			    	myDataChart.loadDate(days,Category[7],7);
			    	flag7 = 0;
			    }else if(scrollTop + 600*1 > $('.data_item').eq(6).offset().top && flag6){
			    	myDataChart.loadDate(days,Category[6],6);
			    	flag6 = 0;
			    }else if(scrollTop + 600*1 > $('.data_item').eq(5).offset().top && flag5){
			    	myDataChart.loadDate(days,Category[5],5);
			    	flag5 = 0;
			    }else if(scrollTop + 600*1 > $('.data_item').eq(4).offset().top && flag4){
			    	myDataChart.loadDate(days,Category[4],4);
			    	flag4 = 0;
			    }else if(scrollTop + 600*1 > $('.data_item').eq(3).offset().top && flag3){
			    	myDataChart.loadDate(days,Category[3],3);
			    	flag3 = 0;
			    }else if(scrollTop + 600*1 > $('.data_item').eq(2).offset().top && flag2){
			    	myDataChart.loadDate(days,Category[2],2);
			    	flag2 = 0;
			    }else if(scrollTop + 600*1 > $('.data_item').eq(1).offset().top && flag1){
			    	myDataChart.loadDate(days,Category[1],1);
			    	flag1 = 0;
			    }
			    return;
			    if(scrollTop > 400){
			    	if(a < scrollTop){
		                var b = Math.max(a,scrollTop);
		              	a = scrollTop;
		            	var size = Math.floor(scrollTop/418);
		            	console.log(size);
		            	if(size >= 8){
		            		size = 8;
		            	}		            	
		            	//加载数据
		            	myDataChart.loadDate(days,Category[size],size);
		            }
			    }	
			})
		},
		loadDate : function(days,typ,idx){//天数  类型  索引

			$.ajax({
				url : public_url + '/api/DataView/Charts',
				type : 'get',
				data : {
					Category : typ,
					LatelyDays : days
				},
				async : false,
				xhrFields: {
		            withCredentials: true
		        },
				success :function(data){
					if(data.Status == 0){
						var Result = data.Result;
						//类型 
						var ChartItem = Result[Category[idx]];
						var ChartItem_Pie = ChartItem.DataPie;
						var ChartItem_Bar = ChartItem.DataBar;
						//时间
						var ChartItem_date = ChartItem.Date.BeginTime.split('-').join('.') + '-' + ChartItem.Date.EndTime.split('-').join('.');
						$('.item_date').html('(' + ChartItem_date + ')');
						//标题
						var TitInfo = ChartItem.Info;
						//柱状图Data区分数据  分为两种情况 
						var BarDataArr = [];
						//饼图的总数
						var TotalCount = 0;
						//将饼图和柱状图的数据合并
						var AllDataInfo = [];
						//legend
						var legend = [];
						//位置
						var positionBar = [];

						if(ChartItem_Bar.Data.length != 0 || ChartItem_Pie.Data.length != 0 || TitInfo.length != 0){
							$('#ChangeOption'+ idx).show();
							if(idx != 6){
								//饼图的数据  过滤掉0的数据
								var PieSeries = [];
								for(var i = 0;i <= ChartItem_Pie.Data.length - 1;i++){
									if(ChartItem_Pie.Data[i].value != 0){
										//计算饼图的总数
										TotalCount += ChartItem_Pie.Data[i].value;
										var obj = {
											name : ChartItem_Pie.Data[i].name,
											value : ChartItem_Pie.Data[i].value,
										}
										for(var j = 0;j <= TheChartsColorArr.length - 1;j ++){
											if(ChartItem_Pie.Data[i].name == TheChartsColorArr[j].name){
												obj.itemStyle = TheChartsColorArr[j].itemStyle;
											}
										}
										PieSeries.push(obj);
									}
								}
								var PieDataArr = [{		
									name: '',					
					                type:'pie',
						            radius: ['40%', '66%'],
						            center: ['15%', '50%'],
						            avoidLabelOverlap: false,
						            labelLine: {
						                normal: {
						                    length: 20,
						                    length2: 40
						                }
						            },
						            label: {
						                normal: {
						                	show: true,
						                    formatter: '{c}\n{d}%',
						                },
						                emphasis: {
						                    show: true,
						                    textStyle: {
						                        fontSize: '14'
						                    }
						                }
						            },
						            textStyle:{
						            	fontSize:12
						            },
						            markPoint:{//圆环统计
						                symbol : 'rect',
						                symbolSize : 0.1,
						                label:{
						                    normal:{
						                        color : 'black',
						                        fontWeight : 'bold'
						                    }
						                },
						                data:[{
						                    x : '15%',
						                    y : '50%',
						                    value:TotalCount?TotalCount:''
						                }]
						            },
						            data : PieSeries,
								}];
								var obj = {
									name : ChartItem_Bar.Data[0].name,
									type : 'bar',
									barWidth: 20,
									data : ChartItem_Bar.Data[0].data,							
								}
								//图例
								for(var i = 0;i<= PieSeries.length-1;i ++){
									legend.push(PieSeries[i].name);
								}
								if(days == 7){
									obj.barWidth = 20;
								}else if(days == 30){
									obj.barWidth = 10;
								}
								BarDataArr.push(obj);
								AllDataInfo = PieDataArr.concat(BarDataArr);
								positionBar = [];
								positionBar.push({top : '20%',x: '35%',  y: '7%',  width: '64%',  height:  '60%'});
							}else{//物料分发
								var arr = [];
								for(var j = 0;j<= ChartItem_Bar.Data.length - 1;j ++){
									var obj = {
										name : ChartItem_Bar.Data[j].name,
										type : 'bar',
										barWidth : 20,
										data : ChartItem_Bar.Data[j].data
									};
									if(days == 7){
										obj.barWidth = 20;
									}else if(days == 30){
										obj.barWidth = 10;
									}
									arr.push(obj);
								}
								BarDataArr = arr;
								AllDataInfo = BarDataArr;
								positionBar = [];
								positionBar.push({  top : '20%',x: '5%',  y: '7%',  width: '94%',  height:  '60%'});
							}
							//图例
							for(var i = 0;i<= BarDataArr.length-1;i ++){
								legend.push(BarDataArr[i].name);

								for(var j = 0;j <= TheChartsColorArr.length - 1;j ++){
									if(BarDataArr[i].name == TheChartsColorArr[j].name ){
										BarDataArr[i].itemStyle = TheChartsColorArr[j].itemStyle;
									}else if(BarDataArr[i].name.indexOf('总计') != -1){
										BarDataArr[i].itemStyle = TheChartsColorArr[TheChartsColorArr.length - 1].itemStyle;
									}
								}
							}
							getOptionsAll($('#echartsCon_'+ idx),legend,TotalCount,ChartItem_Bar.DataLegend,AllDataInfo,positionBar);
						}

						//计算标题的宽度
						$('.sort' + idx).html(ejs.render($('#EachTit').html(), {List : TitInfo}));
						var len = TitInfo.length;
						$('.sort' + idx).find('li').css({'width':98/len + '%'});
						//下拉框
						myDataChart.changeOption($('#echartsCon_'+ idx),legend,TotalCount,ChartItem_Bar.DataLegend,AllDataInfo,idx,days,ChartItem_Bar,positionBar);
					}
				} 
			})	
		},
		changeOption : function(elem,legend,TotalCount,XLine,AllData,idx,days,data,positionBar){//更改切换数据
			
			var newData = AllData.splice(0,1);//只留住饼图
			var DicInfo = data.DicInfo;
			var Data = data.Data;
			var str = '';
			for(var i = 0;i <= DicInfo.length-1;i ++){
				str += "<option TypeId="+ DicInfo[i].TypeId +">" + DicInfo[i].Name + "</option>";
			} 
			$('#ChangeOption'+ idx).html(str);

			if(data.length != 0){
				//下拉更改
				$('#ChangeOption'+ idx).on('change',function(){
					var indx = $(this).find('option:selected').index();
					var that = $(this).find('option:selected');
					var TypeId = that.attr('TypeId');
					var getArr = [];
					var nameArr = [];
					for(var i = 0;i<= Data.length-1;i ++){
						if(Data[i].TypeId == TypeId){
							getArr = Data[i].data;
							nameArr.push(Data[i].name);
						}
					}
					var arr = [];
					var obj = {
						name : nameArr.join(""),
						type : 'bar',
						barWidth : 20,
						data : getArr
					};
					for(var j = 0;j <= TheChartsColorArr.length - 1;j ++){
						if(obj.name == TheChartsColorArr[j].name){
							obj.itemStyle = TheChartsColorArr[j].itemStyle;
						}else if(obj.name.indexOf('总计') != -1){
							obj.itemStyle = TheChartsColorArr[TheChartsColorArr.length - 1].itemStyle;
						}
					}
					if(days == 7){
						obj.barWidth = 20;
					}else if(days == 30){
						obj.barWidth = 10;
					}					
					arr.push(obj);
					var AllDataArr = newData.concat(arr);
					getOptionsAll($('#echartsCon_'+ idx),legend,TotalCount,XLine,AllDataArr,positionBar);
				})
			}			
		}
	}
	myDataChart.originInit();
})

