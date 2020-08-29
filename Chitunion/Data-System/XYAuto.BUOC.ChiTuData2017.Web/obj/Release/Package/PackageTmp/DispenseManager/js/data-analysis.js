/*
* Written by:     yangyakun
* function:       趋势分析
* Created Date:   2017-11-16
*/

$(function () {
	// var public_url = 'http://data1.chitunion.com'
	//柱形叠加图
	function get_baroveroption (dom,name,value,x_data,color) {
		options = {
				    tooltip : {
				        trigger: 'axis',
				        axisPointer : {            // 坐标轴指示器，坐标轴触发有效
				            type : 'shadow'        // 默认为直线，可选为：'line' | 'shadow'
				        }
				    },
				    legend: {
				    	bottom:4,
				        data:name,
				        selectedMode:false
				    },
				    grid: {
        				left: 50,
        				top:20,
        				right:25
    				},
				    color:color,
				    xAxis : [
				        {
				            type : 'category',
				            data : x_data,
				            axisLabel: { // x轴的字体样式       
		                        textStyle: {
		                            color: '#5d6a6f'
		                        }
		                    },
				            axisLine:{// x轴的颜色和宽度
		                        lineStyle:{
		                            color:'#dfd9d9',
		                        }
		                	},
						}
				    ],
				    yAxis : [
				        {
				            type : 'value',
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
		                	// 控制网格线样式
					        splitLine: {
				                lineStyle: {
				                     color: '#dfd9d9'   // 修改网格线颜色     
				                }                            
					        }
						        }
						  ],
				    series : value
				}
		echarts.init(document.querySelector(dom)).setOption(options);
	}
	//环形图
	function get_pieoption (dom,name,value,total) {
		options = {
				    tooltip: {
				        trigger: 'item',
				        formatter: "{a} <br/>{b}: {c} ({d}%)"
				    },
				    legend: {
				        bottom:0,
				        data: name,
				        selectedMode:false
				    },
				    grid:{
				    	top:0
				    },
				    graphic:{
				            type:'text',
				            left:'center',
				            top:'center',
				            style:{
				                text:total?total:'',
				                textAlign:'center',
				                fill:'#5d6a6f',
				                width:30,
				                height:30,
				                font: 'bolder 14px Microsoft YaHei'
				            }
				        },
				    series: [
				        {
					            name: '',
					            type: 'pie',
					            label: {
					                normal: {
					                    formatter: '{d}%\n{c}篇 ',
					                }
				            	},
					            radius: ['30%', '60%'],
					            center: ['50%', '50%'],
					            data:value,
					            textStyle:{
					            	fontSize:12
					            },
					            itemStyle: {
					                emphasis: {
					                    shadowBlur: 10,
					                    shadowOffsetX: 0,
					                    shadowColor: 'rgba(0, 0, 0, 0.5)'
					                }
					            }
				        	}
		    		]
				}
		echarts.init(document.querySelector(dom)).setOption(options);
	}
	//扇形图
	function get_sectoroption (dom,name,value,unit) {
		options = {
				    tooltip : {
				        trigger: 'item',
				        formatter: "{a} <br/>{b}: {c} ({d}%)"
				    },
				    legend: {
				    	type: 'scroll',
				        bottom: 4,
				        data: name,
				        selectedMode:false
				    },
				    color:['#fca3cd','#d39fea','#fdcd82','#c8e8b8','#77c5ef'],
				    series : [
				        {
				            name: '',
				            type: 'pie',
				            radius : '55%',
				            center: ['50%', '46%'],
				            label: {
					                normal: {
					                    formatter:unit,
					                }
				            	},
				            data:value,
				            // textStyle:{
				            // 	fontSize:12
				            // },
				            itemStyle: {
				                emphasis: {
				                    shadowBlur: 10,
				                    shadowOffsetX: 0,
				                    shadowColor: 'rgba(0, 0, 0, 0.5)'
				                }
				            }
				        }
				    ]
				}
		echarts.init(document.querySelector(dom)).setOption(options);
	}
	//两个图表一个canvas   包含饼图和柱形图
	function twoOption (dom,name,x_data,value,total) {
		options  =  {
	        tooltip  :  {
	                trigger:  'item',
	                formatter:  "{a}  <br/>{b}  :  {c}  "
	        },
	        legend:  {
	        	type:'scroll',
                x  :  'center',
                y  :  'bottom',
                left:100,
                right:100,
                data:name,
                selectedMode:false
	        },
	        // graphic:{//圆环内的总计
	        //     type: 'text',
	        //     left: '13.5%',
	        //     top: '50%',
	        //     style:{
	        //         text: total?total:'',
	        //         textAlign: 'center',
	        //         fill: '#333',
	        //         font:'bolder 14px Microsoft Yahei'
	        //     }
	        // },
	        grid:  [
	            {top:'20%',x: '35%',  y: '17%',  width:  '63%',  height:  '60%'}
	        ],
	        xAxis : [
		        {
		            type : 'category',
		            data : x_data,
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
                	// 控制网格线样式
			        splitLine: {
		                lineStyle: {
		                     color: '#dfd9d9'   // 修改网格线颜色     
		                }                            
			        }
				}
			],
	        series  :  value
		};
		echarts.init(document.querySelector(dom)).setOption(options);
	}
	//存储图表类型
	var grabcategory = ['grab_head','grab_body','grab_head_qudao','grab_body_qudao','grab_head_wzcj','grab_body_wzfl','grab_head_zhcj'],
		jxcategory = ['jx_head','jx_body','jx_head_qudao','jx_body_qudao','jx_head_zh','jx_body_zh','jx_head_wzcj','jx_body_wzlb','jx_head_zhcj','jx_head_wz_wzfz','jx_head_zh_zhfz'],
		cxppgory =  ['cxpp_pie_nest','cxpp_bar_brush','cxpp_yes','cxpp_no'],
		csgory = ['cs_pie_nest','cs_bar_brush','cs_head_cj','cs_head_zhcj','cs_head_wzfz','cs_head_zhfz','cs_head_wzzh'],
		rgqxgroy = ['rgqx_head_pie_nest','rgqx_head_bar_brush','rgqx_body_pie_nest','rgqx_body_bar_brush','rgqx_head_wzcj','rgqx_head_zhcj','rgqx_head_wzfz','rgqx_head_zhfz','rgqx_body_wzlb'],
		fzgroy = ['fz_head_pie','fz_head_bar','fz_scenc','fz_account','fz_essay','fz_condition'],
		fenfgroy = ['fenf_head','fenf_materia','fenf_scenc','fenf_account','fenf_essay'],
		zfgroy = ['zf_head_pie','zf_head_bar','zf_materia','zf_scenc','zf_account','zf_essay'],
		xsgroy = ['xs_head_pie','xs_head_bar','xs_materia','xs_scenc','xs_account','xs_essay'];

	//默认进入显示抓取头部
	get_param(grabcategory[0]+','+grabcategory[1])
	get_Grabdata ()
	grabScroll()
	//封装滚动传参加载图表
	function grabScroll () {
		var grabflag1 = 1,
			grabflag2 = 1,
			grabflag3 = 1,
			grabflag4 = 1;
		$(window).scroll(function () {
			var scrollTop = $(this).scrollTop();
			if(scrollTop+900>$('.echarts .list1>li').eq(6).offset().top && grabflag4){
				get_param(grabcategory[6])
			 	get_Grabdata ()
			 	grabflag4 = 0
			}else if(scrollTop+800>$('.echarts .list1>li').eq(4).offset().top && grabflag3){
				get_param(grabcategory[4]+','+grabcategory[5])
			 	get_Grabdata ()
			 	grabflag3 = 0
			}else if(scrollTop+700>$('.echarts .list1>li').eq(2).offset().top && grabflag2){
				get_param(grabcategory[2]+','+grabcategory[3])
			 	get_Grabdata ()
			 	grabflag2 = 0
			}
		})
	}
	function jxScroll () {
		var jxflag1 = 1,
			jxflag2 = 1,
			jxflag3 = 1,
			jxflag4 = 1,
			jxflag5 = 1,
			jxflag6 = 1;
		$(window).scroll(function () {
			var scrollTop = $(this).scrollTop();
			if(scrollTop+1100>$('.echarts .list2>li').eq(10).offset().top && jxflag1){
				get_param(jxcategory[10])
			 	get_jxdata ()
			 	jxflag1 = 0
			}else if(scrollTop+1000>$('.echarts .list2>li').eq(8).offset().top && jxflag2){
				get_param(jxcategory[8]+','+jxcategory[9])
			 	get_jxdata ()
			 	jxflag2 = 0
			}else if(scrollTop+900>$('.echarts .list2>li').eq(6).offset().top && jxflag3){
				get_param(jxcategory[6]+','+jxcategory[7])
			 	get_jxdata ()
			 	jxflag3 = 0
			}else if(scrollTop+800>$('.echarts .list2>li').eq(4).offset().top && jxflag4){
				get_param(jxcategory[4]+','+jxcategory[5])
			 	get_jxdata ()
			 	jxflag4 = 0
			}else if(scrollTop+700>$('.echarts .list2>li').eq(2).offset().top &&jxflag5){
				get_param(jxcategory[2]+','+jxcategory[3])
			 	get_jxdata ()
			 	jxflag5 = 0
			}
		})
	}
	function cxppScroll () {
		var cxppflag1 = 1,
			cxppflag2 = 1;
		$(window).scroll(function () {
			var scrollTop = $(this).scrollTop();
			if(scrollTop+700>$('.echarts .list3>li').eq(2).offset().top && cxppflag1){
				get_param(cxppgory[2]+','+cxppgory[3])
			 	get_cxppdata ()
			 	cxppflag1 = 0
			}
		})
	}
	function csScroll () {
		var csflag1 = 1,
			csflag2 = 1,
			csflag3 = 1,
			csflag4 = 1;
		$(window).scroll(function () {
			var scrollTop = $(this).scrollTop();
			if(scrollTop+900>$('.echarts .list4>li').eq(5).offset().top && csflag1){
				get_param(csgory[6])
			 	get_csdata ()
			 	csflag1 = 0
			}else if(scrollTop+800>$('.echarts .list4>li').eq(3).offset().top && csflag2){
				get_param(csgory[4]+','+csgory[5])
			 	get_csdata ()
			 	csflag2 = 0
			}else if(scrollTop+700>$('.echarts .list4>li').eq(1).offset().top && csflag3){
				get_param(csgory[2]+','+csgory[3])
			 	get_csdata ()
			 	csflag3 = 0
			}

		})
	}
	function rgqxScroll () {
		var rgqxflag1 = 1,
			rgqxflag2 = 1,
			rgqxflag3 = 1,
			rgqxflag4 = 1,
			rgqxflag5 = 1;
		$(window).scroll(function () {
			var scrollTop = $(this).scrollTop();
			if(scrollTop+1000>$('.echarts .list5>li').eq(6).offset().top && rgqxflag1){
				get_param(rgqxgroy[8])
			 	get_rgqxdata ()
			 	rgqxflag1 = 0
			}else if(scrollTop+900>$('.echarts .list5>li').eq(4).offset().top && rgqxflag2){
				get_param(rgqxgroy[6]+','+rgqxgroy[7])
			 	get_rgqxdata ()
			 	rgqxflag2 = 0
			}else if(scrollTop+800>$('.echarts .list5>li').eq(2).offset().top && rgqxflag3){
				get_param(rgqxgroy[4]+','+rgqxgroy[5])
			 	get_rgqxdata ()
			 	rgqxflag3 = 0
			}else if(scrollTop+700>$('.echarts .list5>li').eq(1).offset().top && rgqxflag4){
				get_param(rgqxgroy[2]+','+rgqxgroy[3])
			 	get_rgqxdata ()
			 	rgqxflag4 = 0
			}

		})
	}
	function fzScroll () {
		var fzflag1 = 1,
			fzflag2 = 1,
			fzflag3 = 1;
		$(window).scroll(function () {
			var scrollTop = $(this).scrollTop();
			if(scrollTop+800>$('.echarts .list6>li').eq(3).offset().top && fzflag1){
				get_paramL(fzgroy[4]+','+fzgroy[5])
			 	get_fzdata ()
			 	fzflag1 = 0
			}else if(scrollTop+700>$('.echarts .list6>li').eq(1).offset().top && fzflag2){
				get_paramL(fzgroy[2]+','+fzgroy[3])
			 	get_fzdata ()
			 	fzflag2 = 0
			}
		})
	}
	function fenfScroll () {
		var fenfflag1 = 1,
			fenfflag2 = 1,
			fenfflag3 = 1;
		$(window).scroll(function () {
			var scrollTop = $(this).scrollTop();
			if(scrollTop+800>$('.echarts .list7>li').eq(3).offset().top && fenfflag1){
				get_paramL(fenfgroy[3]+','+fenfgroy[4])
			 	get_distributedata ()
			 	fenfflag1 = 0
			}else if(scrollTop+700>$('.echarts .list7>li').eq(1).offset().top && fenfflag2){
				get_paramL(fenfgroy[1]+','+fenfgroy[2])
			 	get_distributedata ()
			 	fenfflag2 = 0
			}
		})
	}
	function zfScroll () {
		var zfflag1 = 1,
			zfflag2 = 1,
			zfflag3 = 1;
		$(window).scroll(function () {
			var scrollTop = $(this).scrollTop();
			if(scrollTop+800>$('.echarts .list8>li').eq(3).offset().top && zfflag1){
				get_paramL(zfgroy[4]+','+zfgroy[5])
			 	get_transmitdata ()
			 	zfflag1 = 0
			}else if(scrollTop+700>$('.echarts .list8>li').eq(1).offset().top && zfflag2){
				get_paramL(zfgroy[2]+','+zfgroy[3])
			 	get_transmitdata ()
			 	zfflag2 = 0
			}
		})
	}
	function xsScroll () {
		var xsflag1 = 1,
			xsflag2 = 1,
			xsflag3 = 1;
		$(window).scroll(function () {
			var scrollTop = $(this).scrollTop();
			if(scrollTop+800>$('.echarts .list9>li').eq(3).offset().top && xsflag1){
				get_paramL(xsgroy[4]+','+xsgroy[5])
			 	get_threaddata ()
			 	xsflag1 = 0
			}else if(scrollTop+700>$('.echarts .list9>li').eq(1).offset().top && xsflag2){
				get_paramL(xsgroy[2]+','+xsgroy[3])
			 	get_threaddata ()
			 	xsflag2 =0
			}
		})
	}
	//图表交互（默认加载7天、文章）
	var param,
		echartsColors = [
				{'name':'微信公众号','color':'#F5758C'},
				{'name':'微信公众号可用','color':'#F791A3'},
				{'name':'微信公众号作废','color':'#FABAC6'},
				{'name':'微信公众号置为腰','color':'#FCD6DD'},
				{'name':'搜狐自媒体','color':'#BFE7B5'},
				{'name':'搜狐自媒体可用','color':'#e8f7c1'},
				{'name':'搜狐自媒体作废','color':'#D9F6D1'},
				{'name':'搜狐自媒体已匹配车型','color':'#d1e2cd'},
				{'name':'搜狐自媒体未匹配车型','color':'#e4f6df'},
				{'name':'汽车之家','color':'#FDCD82'},
				{'name':'汽车之家可用','color':'#fdae82'},
				{'name':'汽车之家作废','color':'#fdd79b'},
				{'name':'汽车之家已匹配车型','color':'#e4bc8c'},
				{'name':'汽车之家未匹配车型','color':'#fed2b1'},
				{'name':'今日头条','color':'#FF5050'},
				{'name':'今日头条可用','color':'#ff7373'},
				{'name':'今日头条作废','color':'#ff9696'},
				{'name':'今日头条已匹配车型','color':'#dc9f9f'},
				{'name':'今日头条未匹配车型','color':'#ffb9b9'},
				{'name':'网易汽车','color':'#8EB2E2'},
				{'name':'网易汽车可用','color':'#a5c1e8'},
				{'name':'网易汽车作废','color':'#bbd1ee'},
				{'name':'网易汽车已匹配车型','color':'#b3c1e1'},
				{'name':'网易汽车未匹配车型','color':'#89a8ec'},
				{'name':'行圆新闻后台','color':'#91D4F5'},
				{'name':'行圆新闻后台可用','color':'#7cbdd0'},
				{'name':'行圆新闻后台作废','color':'#bde5f9'},
				{'name':'行圆新闻后台已匹配车型','color':'#91c0dc'},
				{'name':'行圆新闻后台未匹配车型','color':'#95c3f4'},
				{'name':'全分发渠道总计','color':'#F0A5B9'},
				{'name':'汽车大全','color':'#FF9593'},
				{'name':'汽车大全询价','color':'#d67c7e'},
				{'name':'汽车大全会话','color':'#ffeae9'},
				{'name':'汽车大全电话接通','color':'#ffcac9'},
				{'name':'车顾问','color':'#CEB6D8'},
				{'name':'车顾问询价','color':'#c3bada'},
				{'name':'车顾问会话','color':'#f0e9f3'},
				{'name':'车顾问电话接通','color':'#e2d3e8'},
				{'name':'赤兔联盟','color':'#60B199'},
				{'name':'赤兔联盟询价','color':'#9be2cc'},
				{'name':'赤兔联盟会话','color':'#dfefeb'},
				{'name':'赤兔联盟电话接通','color':'#bfe0d6'},
				{'name':'微博易','color':'#9FC379'},
				{'name':'微博易询价','color':'#cbe9ac'},
				{'name':'微博易会话','color':'#ecf3e4'},
				{'name':'微博易电话接通','color':'#d9e7c9'},
				{'name':'全网域','color':'#B3D0EE'},
				{'name':'全网域询价','color':'#a0bbd6'},
				{'name':'全网域会话','color':'#f0f6fc'},
				{'name':'全网域电话接通','color':'#d1e3f5'},
				{'name':'头腿','color':'#96b2e1'},
				{'name':'头腰腿','color':'#98ccda'},
				{'name':'头腰','color':'#99e1c9'},
				{'name':'腰腿','color':'#c1edba'},
				{'name':'头','color':'#f8cda4'},
				{'name':'腿','color':'#f6e7c6'}
				],
		sencLbColor = ['#fca3cd','#d39fea','#fdcd82','#c8e8b8','#77c5ef'];

	//定义一个取参数的公共函数
    function get_param(type){
        // TypeId = parseInt($(".select option:selected").attr("TypeId"));
        LatelyDays = parseInt($(".echarts_data .effect_select").attr("LatelyDays"));
        Category = '';
        param = {
            LatelyDays : LatelyDays,
            Category : type, 
            };
        return param;
        }
   	//封装 分发 转发 线索 定义一个取参数的公共函数
   	function　get_paramL(type){
   		DateType = parseInt($(".echarts_data .effect_select").attr("LatelyDays"));
        ChartType = '';
        param = {
            DateType : DateType,
            ChartType : type, 
            };
        return param;
   	}
	//定义一个抓取请求的函数
	function get_Grabdata () {
		var grab_flag1=1,
			grab_flag2=1,
			grab_flag3=1,
			grab_flag4=1;
			$.ajax({
					url : public_url+'/api/TrendAnalysis/GetGrabList',
			        // url : './json/DataAnalysis.json',
			        type : "get",
			        cache: true,
			        xhrFields : {
			            withCredentials : true
			        },
			        data:param,
			        crossDomain:true,
			        success:function(data){	
			            if(data.Status == 0){
			            	if(data.Result.grab_head){
			            	 //头部文章抓取
						     var name = [],
						        series=[],
						        width;
						        
						        for(var i = 0;i<data.Result.grab_head.Data.length;i++){
						            if(data.Result.grab_head.Data[0].data.length==7){
						               	width=20
						            }else if(data.Result.grab_head.Data[0].data.length==30){
						               	width=10
						            }
									series.push({
									    name: data.Result.grab_head.Data[i].name,
									    type: 'bar',
									    stack: '总量',
									    barWidth:width,
									    itemStyle:'',
									    data: data.Result.grab_head.Data[i].data
										});
									for(var j = 0;j<echartsColors.length;j++){
										if(series[i].name==echartsColors[j].name){
											series[i].itemStyle={normal:{color:echartsColors[j].color}}
										}
									}
									
								}
						        $('.grabHead').html(ejs.render($('#grabHead').html(),{data:data.Result.grab_head}));
						        $('.grabHead ol').width((98/data.Result.grab_head.Info.length)+'%')
						        $('.space_date').html($('.grabHead .space_date').html())
						        for(var i = 0;i < data.Result.grab_head.Data.length;i++){
						            name.push(data.Result.grab_head.Data[i].name)
						        }
						        if(data.Result.grab_head.Data.length){
						        	get_baroveroption('#echart1',name,series,data.Result.grab_head.DataLegend)
						        }else{
						        	$('#echart1').html('暂无数据……')
						        }
						        //腰部文章抓取
							    var name = [],
							        series=[],
							        width;
							        for(var i = 0;i<data.Result.grab_body.Data.length;i++){
							           if(data.Result.grab_body.Data[0].data.length==7){
							               	width=20
							            }else if(data.Result.grab_body.Data[0].data.length==30){
							               	width=10
							       	 	}
										series.push({
										    name: data.Result.grab_body.Data[i].name,
										    type: 'bar',
										    stack: '总量',
										   	barWidth:width,
										   	itemStyle:'',
										    data: data.Result.grab_body.Data[i].data
											});
										for(var j = 0;j<echartsColors.length;j++){
											if(series[i].name==echartsColors[j].name){
												series[i].itemStyle={normal:{color:echartsColors[j].color}}
											}
										}
									}
						            $('.grabBody').html(ejs.render($('#grabBody').html(),{data:data.Result.grab_body}));
						            $('.grabBody ol').width(($('.grabBody').width()-1)/data.Result.grab_body.Info.length)
						            for(var i = 0;i < data.Result.grab_body.Data.length;i++){
						                name.push(data.Result.grab_body.Data[i].name)
						            }
						            if(data.Result.grab_body.Data.length>0){
						            	get_baroveroption('#echart2',name,series,data.Result.grab_body.DataLegend)
						            }else {
						            	$('#echart2').html('暂无数据……')
						            }
						            
						        }else if(data.Result.grab_head_qudao){
				        			//抓取的头部文章在渠道上的分布
				        			var total=0;
				        			var value=[],
				        				series=[];
				        			for(var i = 0;i < data.Result.grab_head_qudao.Data.length;i++){
						                value.push(data.Result.grab_head_qudao.Data[i].value)
						                total+=value[i]
						                if(data.Result.grab_head_qudao.Data[i].value!=0){
						                	series.push({
							                	value:data.Result.grab_head_qudao.Data[i].value,
							                	name:data.Result.grab_head_qudao.Data[i].name,
							                	itemStyle:''
						                	})
						                }
						                
						                for(var j = 0;j<echartsColors.length;j++){
											if(series[i].name==echartsColors[j].name){
												series[i].itemStyle={normal:{color:echartsColors[j].color}}
											}
										}
						            }

						            if(data.Result.grab_head_qudao.Data.length){
										get_pieoption('#echart7',data.Result.grab_head_qudao.DataLegend,series,total)
									}else{
										$('#echart7').html('暂无数据……')
									}
									//抓取的腰部文章在渠道上的分布
									var total = 0;
				        			var value = [],
				        				series = [];
						            for(var i = 0;i < data.Result.grab_body_qudao.Data.length;i++){
						                value.push(data.Result.grab_body_qudao.Data[i].value)
						                total+=value[i]
						                if(data.Result.grab_body_qudao.Data[i].value!=0){
						                	series.push({
							                	value:data.Result.grab_body_qudao.Data[i].value,
							                	name:data.Result.grab_body_qudao.Data[i].name,
							                	itemStyle:''
						                	})
						                }
						                
						                for(var j = 0;j<echartsColors.length;j++){
											if(series[i].name==echartsColors[j].name){
												series[i].itemStyle={normal:{color:echartsColors[j].color}}
											}
										}
						            }
						            if(data.Result.grab_body_qudao.Data.length){
										get_pieoption('#echart8',data.Result.grab_body_qudao.DataLegend,series,total)
				        			}else{
				        				$('#echart8').html('暂无数据……')
				        			}
				        		}else if(data.Result.grab_head_wzcj){
				        			// 抓取的头部文章在场景上的分布
				        			var wzlbData = [],
										valData=0;
									for (var i = 0;i<data.Result.grab_head_wzcj.Data.length;i++) {
										if(i<4&&data.Result.grab_head_wzcj.Data[i].value!=0){
											wzlbData.push(data.Result.grab_head_wzcj.Data[i])
										}else {
											valData+=data.Result.grab_head_wzcj.Data[i].value
										}
									}
									if(data.Result.grab_head_wzcj.Data.length>=4 && valData!=0){
										wzlbData.push({TypeId:0,value:valData,name:'其它'})
										data.Result.grab_head_wzcj.DataLegend.push('其它')
									}
				        			if(data.Result.grab_head_wzcj.Data.length){
				        				get_sectoroption('#echart13',data.Result.grab_head_wzcj.DataLegend,wzlbData,'{d}%\n{c}篇 ')
									}else{
										$('#echart13').html('暂无数据……')
									}
									//抓取的腰部文章在文章类别上的分布
									var wzlbData = [],
										valData=0;
									for (var i = 0;i<data.Result.grab_body_wzfl.Data.length;i++) {
										if(i<4&&data.Result.grab_body_wzfl.Data[i].value!=0){
											wzlbData.push(data.Result.grab_body_wzfl.Data[i])
										}else {
											valData+=data.Result.grab_body_wzfl.Data[i].value
										}
									}
									if(data.Result.grab_body_wzfl.Data.length>=4 && valData!=0){
										wzlbData.push({TypeId:0,value:valData,name:'其它'})
										data.Result.grab_body_wzfl.DataLegend.push('其它')
									}
									if(data.Result.grab_body_wzfl.Data.length){
										get_sectoroption('#echart14',data.Result.grab_body_wzfl.DataLegend,wzlbData,'{d}%\n{c}篇 ')
				        			}else {
				        				$('#echart14').html('暂无数据……')
				        			}
				        		}else if(data.Result.grab_head_zhcj){
									//抓取的头部账号在场景上的分布
									var wzlbData = [],
										valData=0;
									for (var i = 0;i<data.Result.grab_head_zhcj.Data.length;i++) {
										if(i<4&&data.Result.grab_head_zhcj.Data[i].value!=0){
											wzlbData.push(data.Result.grab_head_zhcj.Data[i])
										}else {
											valData+=data.Result.grab_head_zhcj.Data[i].value
										}
									}
									if(data.Result.grab_head_zhcj.Data.length>=4 && valData!=0 && valData!=0){
										wzlbData.push({TypeId:0,value:valData,name:'其它'})
										data.Result.grab_head_zhcj.DataLegend.push('其它')
									}
									if(data.Result.grab_head_zhcj.Data.length){
										get_sectoroption('#echart60',data.Result.grab_head_zhcj.DataLegend,wzlbData,'{d}%\n{c}篇 ')
									}else {
										$('#echart60').html('暂无数据……')
									}
								}
						           
				           
							
			        	}
			        }
			    })
		}
	//定义一个机洗入库的请求函数
	function get_jxdata () {
		$.ajax({
				url : public_url+'/api/TrendAnalysis/GetJxList',
		        // url : './json/jxData.json',
		        type : "get",
		        xhrFields : {
		            withCredentials : true
		        },
		        data:param,
		        crossDomain:true,
		        success:function(data){
		        	if(data.Status == 0){
		        		if(data.Result.jx_head){
			        		// 头部文章机洗入库
			        		var name=[],
			        			series=[],
			        			width;
						    for(var i = 0;i<data.Result.jx_head.Data.length;i++){
						    	if(data.Result.jx_head.Data[0].data.length==7){
						            width=20
						        }else if(data.Result.jx_head.Data[0].data.length==30){
						            width=10
						        }
								series.push({
										    name: data.Result.jx_head.Data[i].name,
										    type: 'bar',
										    stack: '总量',
										    barWidth:width,
										    itemStyle:'',
										    data: data.Result.jx_head.Data[i].data
									    });
								for(var j = 0;j<echartsColors.length;j++){
										if(series[i].name==echartsColors[j].name){
											series[i].itemStyle={normal:{color:echartsColors[j].color}}
										}
									}
								}
			        		for(var i = 0;i < data.Result.jx_head.Data.length;i++){
						        name.push(data.Result.jx_head.Data[i].name)
						    }
				            $('.jxHead').html(ejs.render($('#jxHead').html(),{data:data.Result.jx_head}));
				            $('.jxHead ol').width((98/data.Result.jx_head.Info.length)+'%')
				            $('.space_date').html($('.jxHead .space_date').html())	
				            if(data.Result.jx_head.Data.length){	            
				            	get_baroveroption('#echart3',name,series,data.Result.jx_head.DataLegend)
				            }else{
				            	$('#echart3').html('暂无数据……')
				            }
				            // 腰部文章机洗入库
				            var name=[],
				            	series=[],
				            	width;
						    for(var i = 0;i<data.Result.jx_body.Data.length;i++){
						    	if(data.Result.jx_body.Data[0].data.length==7){
						            width=20
						        }else if(data.Result.jx_body.Data[0].data.length==30){
						            width=10
						        }
								series.push({
									name: data.Result.jx_body.Data[i].name,
									type: 'bar',
									stack: '总量',
									barWidth:width,
									itemStyle:'',
									data: data.Result.jx_body.Data[i].data
									});
								for(var j = 0;j<echartsColors.length;j++){
										if(series[i].name==echartsColors[j].name){
											series[i].itemStyle={normal:{color:echartsColors[j].color}}
										}
									}
								}
			        		for(var i = 0;i < data.Result.jx_body.Data.length;i++){
						        name.push(data.Result.jx_body.Data[i].name)
						    }
				            $('.jxBody').html(ejs.render($('#jxBody').html(),{data:data.Result.jx_body}));
				            $('.jxBody ol').width(($('.jxBody').width()-1)/data.Result.jx_body.Info.length)
				            if(data.Result.jx_body.Data.length){	
				            	get_baroveroption('#echart4',name,series,data.Result.jx_body.DataLegend)
			        		}else{
			        			$('#echart4').html('暂无数据……')
			        		}
			        	}else if(data.Result.jx_head_qudao){
				            //机洗入库的头部文章在渠道上的分布
				            var total = 0;
							var value = [],
								series = [];
						   	for(var i = 0;i < data.Result.jx_head_qudao.Data.length;i++){
						        value.push(data.Result.jx_head_qudao.Data[i].value)
						        total+=value[i]
						        if(data.Result.jx_head_qudao.Data[i].value!=0){
							        series.push({
							            value:data.Result.jx_head_qudao.Data[i].value,
							            name:data.Result.jx_head_qudao.Data[i].name,
							            itemStyle:''
							            })
							     }
						        for(var j = 0;j<echartsColors.length;j++){
									if(series[i].name==echartsColors[j].name){
										series[i].itemStyle={normal:{color:echartsColors[j].color}}
									}
								}
						    }
						    if(data.Result.jx_head_qudao.Data.length){
								get_pieoption('#echart9',data.Result.jx_head_qudao.DataLegend,series,total)
				            }else{
				            	$('#echart9').html('暂无数据……')
				            }
				            //机洗入库的腰部文章在渠道上的分布
				            var total = 0;
							var value = [],
								series = [];
						   	for(var i = 0;i < data.Result.jx_body_qudao.Data.length;i++){
						        value.push(data.Result.jx_body_qudao.Data[i].value)
						        total+=value[i]
						        if(data.Result.jx_body_qudao.Data[i].value!=0){
							        series.push({
							            value:data.Result.jx_body_qudao.Data[i].value,
							            name:data.Result.jx_body_qudao.Data[i].name,
							            itemStyle:''
							            })
							     }
						        for(var j = 0;j<echartsColors.length;j++){
									if(series[i].name==echartsColors[j].name){
										series[i].itemStyle={normal:{color:echartsColors[j].color}}
									}
								}
						    }
						    if(data.Result.jx_body_qudao.Data.length){
								get_pieoption('#echart10',data.Result.jx_body_qudao.DataLegend,series,total)
							}else {
								$('#echart10').html('暂无数据……')
							}
						}else if(data.Result.jx_head_zh){
							//头部文章机洗入库的转化
							$('.jxHeadzh').html(ejs.render($('#jxHeadzh').html(),{data:data.Result.jx_head_zh}));
							//腰部文章机洗入库的转化
							$('.jxBodyzh').html(ejs.render($('#jxBodyzh').html(),{data:data.Result.jx_body_zh}));
						}else if(data.Result.jx_head_wzcj){
			        		//机洗入库的头部文章在场景上的分布
			        		var wzlbData = [],
								valData=0;
								for (var i = 0;i<data.Result.jx_head_wzcj.Data.length;i++) {
									if(i<4&&data.Result.jx_head_wzcj.Data[i].value!=0){
										wzlbData.push(data.Result.jx_head_wzcj.Data[i])
									}else {
										valData+=data.Result.jx_head_wzcj.Data[i].value
									}
								}
							if(data.Result.jx_head_wzcj.Data.length>=4 && valData!=0){
								wzlbData.push({TypeId:0,value:valData,name:'其它'})
								data.Result.jx_head_wzcj.DataLegend.push('其它')
							}
			        		if(data.Result.jx_head_wzcj.Data.length){
			        			get_sectoroption('#echart15',data.Result.jx_head_wzcj.DataLegend,wzlbData,'{d}%\n{c}篇 ')
							}else{
								$('#echart15').html('暂无数据……')
							}
							//机洗入库的腰部文章在文章类别上的分布
							var wzlbData = [],
								valData=0;
							for (var i = 0;i<data.Result.jx_body_wzlb.Data.length;i++) {
								if(i<4&&data.Result.jx_body_wzlb.Data[i].value!=0){
									wzlbData.push(data.Result.jx_body_wzlb.Data[i])
								}else {
									valData+=data.Result.jx_body_wzlb.Data[i].value
								}
							}
							if(data.Result.jx_body_wzlb.Data.length>=4 && valData!=0){
								wzlbData.push({TypeId:0,value:valData,name:'其它'})
								data.Result.jx_body_wzlb.DataLegend.push('其它')
							}
							if(data.Result.jx_body_wzlb.Data.length){
								get_sectoroption('#echart16',data.Result.jx_body_wzlb.DataLegend,wzlbData,'{d}%\n{c}篇 ')
							}else {
								$('#echart16').html('暂无数据……')
							}
						}else if(data.Result.jx_head_zhcj){
							//机洗入库的头部账号在场景上的分布
							var wzlbData = [],
								valData=0;
								for (var i = 0;i<data.Result.jx_head_zhcj.Data.length;i++) {
									if(i<4&&data.Result.jx_head_zhcj.Data[i].value!=0){
										wzlbData.push(data.Result.jx_head_zhcj.Data[i])
									}else {
										valData+=data.Result.jx_head_zhcj.Data[i].value
									}
								}
							if(data.Result.jx_head_zhcj.Data.length>=4 && valData!=0){
								wzlbData.push({TypeId:0,value:valData,name:'其它'})
								data.Result.jx_head_zhcj.DataLegend.push('其它')
							}
							if(data.Result.jx_head_zhcj.Data.length){
								get_sectoroption('#echart61',data.Result.jx_head_zhcj.DataLegend,wzlbData,'{d}%\n{c}篇 ')
							}else {
								$('#echart61').html('暂无数据……')	
							}
							//机洗入库的头部文章在文章分值上的分布
							var series=[];
							for(var i=0;i<data.Result.jx_head_wz_wzfz.Data.length;i++){
				        		if(data.Result.jx_head_wz_wzfz.Data[i].value!=0){
				        			series.push(data.Result.jx_head_wz_wzfz.Data[i])
				        		}
				        	}
							if(data.Result.jx_head_wz_wzfz.Data.length){
								get_sectoroption('#echart17',data.Result.jx_head_wz_wzfz.DataLegend,series,'{d}%\n{c}篇 ')
							}else {
								$('#echart17').html('暂无数据……')
							}
						}else if(data.Result.jx_head_zh_zhfz){	
							//机洗入库的头部账号在文章分值上的分布
							var series=[];
							for(var i=0;i<data.Result.jx_head_zh_zhfz.Data.length;i++){
				        		if(data.Result.jx_head_zh_zhfz.Data[i].value!=0){
				        			series.push(data.Result.jx_head_zh_zhfz.Data[i])
				        		}
				        	}
							if(data.Result.jx_head_zh_zhfz.Data.length){
								get_sectoroption('#echart18',data.Result.jx_head_zh_zhfz.DataLegend,series,'{d}%\n{c}篇 ')
			        		}else {
			        			$('#echart18').html('暂无数据……')
			        		}
			        	}
		        }
		    }
		})
	}
	//定义一个车型匹配的请求函数
	function get_cxppdata () {
		$.ajax({
				url : public_url+'/api/TrendAnalysis/GetCxppList',
		        type : "get",
		        xhrFields : {
		            withCredentials : true
		        },
		        data:param,
		        crossDomain:true,
		        success:function(data){
		    		if(data.Status == 0){
		    			var cxpp_bar_brush = data.Result.cxpp_bar_brush;
		    			if(data.Result.cxpp_pie_nest){
			    			//车型匹配内嵌
			    			$('.cxpp').html(ejs.render($('#cxpp').html(),{data:data.Result.cxpp_pie_nest}));
			    			$('.cxpp ol').width((98/data.Result.cxpp_pie_nest.Info.length)+'%')
				            $('.space_date').html($('.cxpp .space_date').html())	
				            var inside = [],
				            	outer=[],
				            	name1=[],
				            	name2=[],
				            	Nname=[],
				            	width,
				            	position='',
				            	series=[];
				            	//遍历内圆数据
				            for(var i = 0;i<data.Result.cxpp_pie_nest.Data.length;i++){
				            	if(data.Result.cxpp_pie_nest.Data[i].TypeId==0 && data.Result.cxpp_pie_nest.Data[i].value!=0){
				            		inside.push({
				            			value:data.Result.cxpp_pie_nest.Data[i].value,
				            			name:data.Result.cxpp_pie_nest.Data[i].name,
				            			itemStyle:''
				            		})
				            		//对应的内环添加颜色
				            		for(var j = 0;j<echartsColors.length;j++){
										if(inside[i].name==echartsColors[j].name){
											inside[i].itemStyle={normal:{color:echartsColors[j].color}}
										}
									}
				            	}
				            }
				            //当内圆只有一个数据的时候，lengend居中显示
				            if(inside.length==1){
				            	position = 'center'
				            }else{
				            	position = 'inner'
				            }
				            for(var i = 0;i<data.Result.cxpp_pie_nest.Data.length;i++){
				            	if(data.Result.cxpp_pie_nest.Data[i].TypeId!=0 && data.Result.cxpp_pie_nest.Data[i].value!=0){
									outer.push({
										value:data.Result.cxpp_pie_nest.Data[i].value,
				            			name:data.Result.cxpp_pie_nest.Data[i].name,
				            			itemStyle:'',
				            			TypeMatchId:data.Result.cxpp_pie_nest.Data[i].TypeMatchId
									})
				            	}
				            }
				            for(var i= 0;i<outer.length;i++){
				            	if(outer[i].TypeMatchId==1){
				            		outer[i].name=outer[i].name+"已匹配车型"
				            	}else if(outer[i].TypeMatchId==2){
				            		outer[i].name=outer[i].name+"未匹配车型"
				            	}
				            	//对应的外环添加颜色
				            	for(var j = 0;j<echartsColors.length;j++){
										if(outer[i].name==echartsColors[j].name){
											outer[i].itemStyle={normal:{color:echartsColors[j].color}}
										}
									}
				            }
				            
				            //遍历图例
				            for(var i = 0;i<data.Result.cxpp_pie_nest.Data.length;i++){
				            	if(data.Result.cxpp_pie_nest.Data[i].TypeId==0){
				            		name1.push(data.Result.cxpp_pie_nest.Data[i].name,data.Result.cxpp_pie_nest.Data[i].name+"已匹配车型",data.Result.cxpp_pie_nest.Data[i].name+"未匹配车型")
				            	}
				            }
				            Nname = name1.join(",").split(",");
				            series=[{
					            name:'',
					            type:'pie',
					            selectedMode: 'single',
					            radius: [0, '30%'],
					            center: ['15%', '50%'],
					            label: {
					                normal: {
					                    position: position,
					                    show:false,
					                    textStyle:{
					                    	color:'#fff'
					                    }

					                }
					            },
					            labelLine: {
					                normal: {
					                    show: false
					                }
					            },
					            data:inside
					        },
					        {
					            name:'',
					            type:'pie',
					            radius: ['40%', '55%'],
					            center: ['15%', '50%'],
					            // startAngle:100,
					            label: {
					                normal: {
					                    formatter: '{c}篇\n{d}%  ',
					                    // backgroundColor: '#eee',
					                    // borderColor: '#aaa',
					                    // borderWidth: 1,
					                    // borderRadius: 4,
					                    rich: {
					                        // a: {
					                        //     color: '#999',
					                        //     lineHeight: 22,
					                        //     align: 'center'
					                        // },
					                        // hr: {
					                        //     borderColor: '#aaa',
					                        //     width: '100%',
					                        //     borderWidth: 0.5,
					                        //     height: 0
					                        // },
					                        // b: {
					                        //     fontSize: 16,
					                        //     lineHeight: 33
					                        // },
					                        // per: {
					                        //     color: '#eee',
					                        //     backgroundColor: '#334455',
					                        //     padding: [2, 4],
					                        //     borderRadius: 2
					                        // }
					                    }
					                }
					            },
					            data:outer
					        }]
				        	var name=[],
								n=1;
							//把某些数据变为负
							for(var m = 0;m<data.Result.cxpp_bar_brush.Data.length;m++){
								if(data.Result.cxpp_bar_brush.Data[m].TypeMatchId==77002 || data.Result.cxpp_bar_brush.Data[m].TypeMatchId==2){	
									for(var n=0;n<data.Result.cxpp_bar_brush.Data[m].data.length;n++){
										data.Result.cxpp_bar_brush.Data[m].data[n] = -data.Result.cxpp_bar_brush.Data[m].data[n]
									}
								}
							}
							//遍历图例
							for(var p=0;p<data.Result.cxpp_bar_brush.DicInfoMatch.length;p++){
								for(var q = 0;q<data.Result.cxpp_bar_brush.Data.length;q++){
									if(data.Result.cxpp_bar_brush.DicInfoMatch[p].TypeMatchId==data.Result.cxpp_bar_brush.Data[q].TypeMatchId){
										data.Result.cxpp_bar_brush.Data[q].name=data.Result.cxpp_bar_brush.Data[q].name+data.Result.cxpp_bar_brush.DicInfoMatch[p].Name
											
									}
								}
							}
							//生成新的series
							for(var i = 0;i<data.Result.cxpp_bar_brush.DicInfo.length;i++){
								for(var j = 0;j<data.Result.cxpp_bar_brush.Data.length;j++){			
										if(data.Result.cxpp_bar_brush.DicInfo[i].TypeId==data.Result.cxpp_bar_brush.Data[j].TypeId){
											if(j%data.Result.cxpp_bar_brush.DicInfoMatch.length==0){
											    n++
											}
											if(data.Result.cxpp_bar_brush.Data[0].data.length==7){
									            width=20
									        }else if(data.Result.cxpp_bar_brush.Data[0].data.length==30){
									            width=10
									        }
											series.push({
												name: data.Result.cxpp_bar_brush.Data[j].name,
												type: 'bar',
												stack:n,
												barWidth:width,
												center : ['15%','55%'],
												itemStyle:'',
												data: data.Result.cxpp_bar_brush.Data[j].data  
											});
										
									}
									
								}
							}
							//对应的柱状图添加颜色
							for(var i = 0 ;i<series.length;i++){
								 for(var j = 0;j<echartsColors.length;j++){
									if(series[i].name==echartsColors[j].name){
										series[i].itemStyle={normal:{color:echartsColors[j].color}}
										}
									}
							}
							if(data.Result.cxpp_bar_brush.Data.length>0 && data.Result.cxpp_pie_nest.Data.length>0 ){
								twoOption('#echart49',Nname,data.Result.cxpp_bar_brush.DataLegend,series)
							}else{
								$('#echart49').html('暂无数据……')
							}
							
				        }else if(data.Result.cxpp_yes){
			    			//已匹配车型文章类别分布
			    			var wzlbData = [],
								valData=0;
								for (var i = 0;i<data.Result.cxpp_yes.Data.length;i++) {
									if(i<4&&data.Result.cxpp_yes.Data[i].value!=0){
										wzlbData.push(data.Result.cxpp_yes.Data[i])
									}else {
										valData+=data.Result.cxpp_yes.Data[i].value
									}
								}
								if(data.Result.cxpp_yes.Data.length>=4 && valData!=0){
									wzlbData.push({TypeId:0,value:valData,name:'其它'})
									data.Result.cxpp_yes.DataLegend.push('其它')
								}
							if(data.Result.cxpp_yes.Data.length){
			    				get_sectoroption('#echart19',data.Result.cxpp_yes.DataLegend,wzlbData,'{d}%\n{c}篇 ')
			    			}else {
			    				$('#echart19').html('暂无数据……')
			    			}
			    			//未匹配车型文章类别分布
			    			var wzlbData = [],
								valData=0;
								for (var i = 0;i<data.Result.cxpp_no.Data.length;i++) {
									if(i<4&&data.Result.cxpp_no.Data[i].value!=0){
										wzlbData.push(data.Result.cxpp_no.Data[i])
									}else if(data.Result.cxpp_no.Data[i].value!=0){
										valData+=data.Result.cxpp_no.Data[i].value
									}
								}
								if(data.Result.cxpp_no.Data.length>=4 && valData!=0){
									wzlbData.push({TypeId:0,value:valData,name:'其它'})
									data.Result.cxpp_no.DataLegend.push('其它')
								}
							if(data.Result.cxpp_no.Data.length){
			    				get_sectoroption('#echart20',data.Result.cxpp_no.DataLegend,wzlbData,'{d}%\n{c}篇 ')
		    				}else{
		    					$('#echart20').html('暂无数据……')
		    				}
		    			}

		    		}
		        }
		    })
	}
	//定义一个初筛的请求函数
	function get_csdata () {
		$.ajax({
				url : public_url+'/api/TrendAnalysis/GetCsList',
		        // url : './json/csChart.json',
		        type : "get",
		        xhrFields : {
		            withCredentials : true
		        },
		        data:param,
		        crossDomain:true,
		        success:function(data){
		    		if(data.Status == 0){
		    			if(data.Result.cs_pie_nest){
			    			//车型匹配内嵌
			    			$('.cs').html(ejs.render($('#cs').html(),{data:data.Result.cs_pie_nest}));
			    			$('.cs ol').width((98/data.Result.cs_pie_nest.Info.length)+'%')
				            $('.space_date').html($('.cs .space_date').html())	
				            var inside = [],
				            	outer=[],
				            	name1=[],
				            	name2=[],
				            	Nname=[],
				            	position ='',
				            	series=[];
				            	//遍历内圆数据
				            for(var i = 0;i<data.Result.cs_pie_nest.Data.length;i++){
				            	if(data.Result.cs_pie_nest.Data[i].TypeId==0){
				            		inside.push({
				            			value:data.Result.cs_pie_nest.Data[i].value,
				            			name:data.Result.cs_pie_nest.Data[i].name,
				            			itemStyle:''
				            		})
				            		//对应的内环添加颜色
				            		for(var j = 0;j<echartsColors.length;j++){
										if(inside[i].name==echartsColors[j].name){
											inside[i].itemStyle={normal:{color:echartsColors[j].color}}
										}
									}
				            	}
				            }
				            //当内圆只有一个数据的时候，lengend居中显示
				            if(inside.length==1){
				            	position = 'center'
				            }else{
				            	position = 'inner'
				            }
				            for(var i = 0;i<data.Result.cs_pie_nest.Data.length;i++){
				            	if(data.Result.cs_pie_nest.Data[i].TypeId!=0){
									outer.push({
										value:data.Result.cs_pie_nest.Data[i].value,
				            			name:data.Result.cs_pie_nest.Data[i].name,
				            			itemStyle:'',
				            			TypeMatchId:data.Result.cs_pie_nest.Data[i].TypeMatchId
									})
				            	}
				            }
				            for(var i= 0;i<outer.length;i++){
				            	if(outer[i].TypeMatchId==77001){
				            		outer[i].name=outer[i].name+"可用"
				            	}else if(outer[i].TypeMatchId==77002){
				            		outer[i].name=outer[i].name+"作废"
				            	}else if (outer[i].TypeMatchId==77003){
				            		outer[i].name=outer[i].name+"置为腰"
				            	}
				            	//对应的外环添加颜色
				            	for(var j = 0;j<echartsColors.length;j++){
									if(outer[i].name==echartsColors[j].name){
										outer[i].itemStyle={normal:{color:echartsColors[j].color}}
										}
									}
				            }
				            //遍历图例
				            for(var i = 0;i<data.Result.cs_pie_nest.Data.length;i++){
				            	if(data.Result.cs_pie_nest.Data[i].TypeId==0){
				            		name1.push(data.Result.cs_pie_nest.Data[i].name,data.Result.cs_pie_nest.Data[i].name+"可用",data.Result.cs_pie_nest.Data[i].name+"作废",data.Result.cs_pie_nest.Data[i].name+"置为腰")
				            	}
				            }
				            Nname = name1.join(",").split(",");
				            series=[{
					            name:'',
					            type:'pie',
					            selectedMode: 'single',
					            radius: [0, '30%'],
					            center: ['15%', '50%'],
					            label: {
					                normal: {
					                    position: position,
					                    show:false,
					                    textStyle:{
					                    	color:'#fff'
					                    }
					                }
					            },
					            labelLine: {
					                normal: {
					                    // show: false,
					                    length:20
					                }
					            },
					            data:inside
					        },
					        {
					            name:'',
					            type:'pie',
					            radius: ['40%', '55%'],
					            center: ['15%', '50%'],
					            label: {
					                normal: {
					                    formatter: '{c}篇\n{d}%   ',
					                    // backgroundColor: '#eee',
					                    // borderColor: '#aaa',
					                    // borderWidth: 1,
					                    // borderRadius: 4,
					                    rich: {
					                        // a: {
					                        //     color: '#999',
					                        //     lineHeight: 22,
					                        //     align: 'center'
					                        // },
					                        // hr: {
					                        //     borderColor: '#aaa',
					                        //     width: '100%',
					                        //     borderWidth: 0.5,
					                        //     height: 0
					                        // },
					                        // b: {
					                        //     fontSize: 16,
					                        //     lineHeight: 33
					                        // },
					                        // per: {
					                        //     color: '#eee',
					                        //     backgroundColor: '#334455',
					                        //     padding: [2, 4],
					                        //     borderRadius: 2
					                        // }
					                    }
					                }
					            },
					            data:outer
					        }]
				            var width,
				            	name=[],
								n=1;
							//把某些数据变为负
							for(var m = 0;m<data.Result.cs_bar_brush.Data.length;m++){
								if(data.Result.cs_bar_brush.Data[m].TypeMatchId==77002 || data.Result.cs_bar_brush.Data[m].TypeMatchId==2){	
									for(var n=0;n<data.Result.cs_bar_brush.Data[m].data.length;n++){
										data.Result.cs_bar_brush.Data[m].data[n] = -data.Result.cs_bar_brush.Data[m].data[n]
									}
								}
							}
							//遍历图例
							for(var p=0;p<data.Result.cs_bar_brush.DicInfoMatch.length;p++){
								for(var q = 0;q<data.Result.cs_bar_brush.Data.length;q++){
									if(data.Result.cs_bar_brush.DicInfoMatch[p].TypeMatchId==data.Result.cs_bar_brush.Data[q].TypeMatchId){
										data.Result.cs_bar_brush.Data[q].name=data.Result.cs_bar_brush.Data[q].name+data.Result.cs_bar_brush.DicInfoMatch[p].Name
											
									}
								}
							}
							//生成新的series
							for(var i = 0;i<data.Result.cs_bar_brush.DicInfo.length;i++){
								for(var j = 0;j<data.Result.cs_bar_brush.Data.length;j++){			
										if(data.Result.cs_bar_brush.DicInfo[i].TypeId==data.Result.cs_bar_brush.Data[j].TypeId){
											if(j%data.Result.cs_bar_brush.DicInfoMatch.length==0){
											    n++
											}
											if(data.Result.cs_bar_brush.Data[0].data.length==7){
								               	width=20
								            }else if(data.Result.cs_bar_brush.Data[0].data.length==30){
								               	width=10
								            }
											series.push({
												name: data.Result.cs_bar_brush.Data[j].name,
												type: 'bar',
												stack:n,
												itemStyle:'',
											    barWidth:width,
												data: data.Result.cs_bar_brush.Data[j].data  
												});
										
									}
									
								}
							}
							//对应的柱状图添加颜色
							for(var i = 0 ;i<series.length;i++){
								 for(var j = 0;j<echartsColors.length;j++){
									if(series[i].name==echartsColors[j].name){
										series[i].itemStyle={normal:{color:echartsColors[j].color}}
										}
									}
							}
							if(data.Result.cs_bar_brush.Data.length>0 && data.Result.cs_pie_nest.Data.length>0){
								twoOption('#echart50',Nname,data.Result.cs_bar_brush.DataLegend,series)
							}else{
								$('#echart50').html('暂无数据……')
							}
				            
			             }else if(data.Result.cs_head_cj){
				            //初筛头部文章在场景上的分布
				            var cs_cj_str = '';
			                for(var i=0;i<data.Result.cs_head_cj.DicInfo.length;i++){
			                    cs_cj_str += '<option TypeId='+data.Result.cs_head_cj.DicInfo[i].TypeId+'>'+data.Result.cs_head_cj.DicInfo[i].Name+'</option>'
			                }	
			                $('.cs_cj_select').html(cs_cj_str)
				            get_param()
							get_selectData("#echart21",data.Result.cs_head_cj,'.cs_cj_select option:selected','{d}%\n{c}篇 ')
							//select部分选择变化的时候
							$(".cs_cj_select").on("change",function(){
								// 获取参数值
								get_param()
								get_selectData("#echart21",data.Result.cs_head_cj,'.cs_cj_select option:selected','{d}%\n{c}篇 ');
							})
							// 初筛头部账号在场景上的分布
							var cs_zhcj_str = '';
			                for(var i=0;i<data.Result.cs_head_zhcj.DicInfo.length;i++){
			                    cs_zhcj_str += '<option TypeId='+data.Result.cs_head_zhcj.DicInfo[i].TypeId+'>'+data.Result.cs_head_zhcj.DicInfo[i].Name+'</option>'
			                }	
			                $('.cs_zhcj_select').html(cs_zhcj_str)
							get_param()
							get_selectData("#echart22",data.Result.cs_head_zhcj,'.cs_zhcj_select option:selected','{d}%\n{c}篇 ')
							//select部分选择变化的时候
							$(".cs_zhcj_select").on("change",function(){
								// 获取参数值
								get_param()
								get_selectData("#echart22",data.Result.cs_head_zhcj,'.cs_zhcj_select option:selected','{d}%\n{c}篇 ');
							})
						}else if(data.Result.cs_head_wzfz){
							//初筛头部文章在文章分值上的分布
							var cs_wzfz_str = '';
			                for(var i=0;i<data.Result.cs_head_wzfz.DicInfo.length;i++){
			                    cs_wzfz_str += '<option TypeId='+data.Result.cs_head_wzfz.DicInfo[i].TypeId+'>'+data.Result.cs_head_wzfz.DicInfo[i].Name+'</option>'
			                }	
			                $('.cs_wzfz_select').html(cs_wzfz_str)
							get_param()
							get_selectData("#echart23",data.Result.cs_head_wzfz,'.cs_wzfz_select option:selected','{d}%\n{c}篇 ')
							//select部分选择变化的时候
							$(".cs_wzfz_select").on("change",function(){
								// 获取参数值
								get_param()
								get_selectData("#echart23",data.Result.cs_head_wzfz,'.cs_wzfz_select option:selected','{d}%\n{c}篇 ');
							})
							//初筛头部账号在账号分值上的分布
							var cs_zhfz_str = '';
			                for(var i=0;i<data.Result.cs_head_zhfz.DicInfo.length;i++){
			                    cs_zhfz_str += '<option TypeId='+data.Result.cs_head_zhfz.DicInfo[i].TypeId+'>'+data.Result.cs_head_zhfz.DicInfo[i].Name+'</option>'
			                }	
			                $('.cs_zhfz_select').html(cs_zhfz_str)
							get_param()
							get_selectData("#echart24",data.Result.cs_head_zhfz,'.cs_zhfz_select option:selected','{d}%\n{c}篇 ')

							//select部分选择变化的时候
							$(".cs_zhfz_select").on("change",function(){
								// 获取参数值
							get_param()
							get_selectData("#echart24",data.Result.cs_head_zhfz,'.cs_zhfz_select option:selected','{d}%\n{c}篇 ')
							})
						}else if(data.Result.cs_head_wzzh){
							//文章初筛可用转化
							$('.cszh').html(ejs.render($('#cszh').html(),{data:data.Result.cs_head_wzzh}));
						}
		    		}
		    	}
		    })
	}
	//定义一个人工清洗的请求函数
	function get_rgqxdata () {
		$.ajax({
				url : public_url+'/api/TrendAnalysis/GetArtificialList',
		        // url : './json/rgqxEchart.json',
		        type : "get",
		        xhrFields : {
		            withCredentials : true
		        },
		        data:param,
		        crossDomain:true,
		        success:function(data){
		        	if(data.Status == 0){
		        		if(data.Result.rgqx_head_pie_nest){
			        		//头部文章清洗入库
			    			$('.rgqxHead').html(ejs.render($('#rgqxHead').html(),{data:data.Result.rgqx_head_pie_nest}));
				            $('.rgqxHead ol').width((98/data.Result.rgqx_head_pie_nest.Info.length)+'%')
				            $('.space_date').html($('.rgqxHead .space_date').html())
				             var inside = [],
				            	outer=[],
				            	name1=[],
				            	name2=[],
				            	Nname=[],
				            	position='',
				            	series=[];
				            	//遍历内圆数据
				            for(var i = 0;i<data.Result.rgqx_head_pie_nest.Data.length;i++){
				            	if(data.Result.rgqx_head_pie_nest.Data[i].TypeId==0){
				            		inside.push({
				            			value:data.Result.rgqx_head_pie_nest.Data[i].value,
				            			name:data.Result.rgqx_head_pie_nest.Data[i].name,
				            			itemStyle:''
				            		})
				            		//对应的内环添加颜色
				            		for(var j = 0;j<echartsColors.length;j++){
										if(inside[i].name==echartsColors[j].name){
											inside[i].itemStyle={normal:{color:echartsColors[j].color}}
										}
									}	
				            	}
				            }
				            //当内圆只有一个数据的时候，lengend居中显示
				            if(inside.length==1){
				            	position = 'center'
				            }else{
				            	position = 'inner'
				            }

				            for(var i = 0;i<data.Result.rgqx_head_pie_nest.Data.length;i++){
				            	if(data.Result.rgqx_head_pie_nest.Data[i].TypeId!=0){
									outer.push({
										value:data.Result.rgqx_head_pie_nest.Data[i].value,
				            			name:data.Result.rgqx_head_pie_nest.Data[i].name,
				            			itemStyle:'',
				            			TypeMatchId:data.Result.rgqx_head_pie_nest.Data[i].TypeMatchId
									})
				            	}
				            }

				            for(var i= 0;i<outer.length;i++){
				            	if(outer[i].TypeMatchId==77001){
				            		outer[i].name=outer[i].name+"可用"
				            	}else if(outer[i].TypeMatchId==77002){
				            		outer[i].name=outer[i].name+"作废"
				            	}else if (outer[i].TypeMatchId==77003){
				            		outer[i].name=outer[i].name+"置为腰"
				            	}
				            	//对应的外环添加颜色
				            	for(var j = 0;j<echartsColors.length;j++){
									if(outer[i].name==echartsColors[j].name){
										outer[i].itemStyle={normal:{color:echartsColors[j].color}}
									}
								}
				            }
				            //遍历图例
				            for(var i = 0;i<data.Result.rgqx_head_pie_nest.Data.length;i++){
				            	if(data.Result.rgqx_head_pie_nest.Data[i].TypeId==0){
				            		name1.push(data.Result.rgqx_head_pie_nest.Data[i].name,data.Result.rgqx_head_pie_nest.Data[i].name+"可用",data.Result.rgqx_head_pie_nest.Data[i].name+"作废",data.Result.rgqx_head_pie_nest.Data[i].name+"置为腰")
				            	}
				            }
				            Nname = name1.join(",").split(",");
				             series=[{
					            name:'',
					            type:'pie',
					            selectedMode: 'single',
					            radius: [0, '30%'],
					            center: ['15%', '50%'],
					            label: {
					                normal: {
					                    position: position,
					                    show:false,
					                    textStyle:{
					                    	color:'#fff'
					                    }
					                }
					            },
					            labelLine: {
					                normal: {
					                    show: false
					                }
					            },
					            data:inside
					        },
					        {
					            name:'',
					            type:'pie',
					            radius: ['40%', '55%'],
					            center: ['15%', '50%'],
					            label: {
					                normal: {
					                     formatter: '{c}篇\n{d}%  ',
					                   
					     //                formatter:function(val){
										//     return val.name.split("已").join("\n");
										// },
					                    // backgroundColor: '#eee',
					                    // borderColor: '#aaa',
					                    // borderWidth: 1,
					                    // borderRadius: 4,
					                    rich: {
					                        // a: {
					                        //     color: '#999',
					                        //     lineHeight: 22,
					                        //     align: 'center'
					                        // },
					                        // hr: {
					                        //     borderColor: '#aaa',
					                        //     width: '100%',
					                        //     borderWidth: 0.5,
					                        //     height: 0
					                        // },
					                        // b: {
					                        //     fontSize: 16,
					                        //     lineHeight: 33
					                        // },
					                        // per: {
					                        //     color: '#eee',
					                        //     backgroundColor: '#334455',
					                        //     padding: [2, 4],
					                        //     borderRadius: 2
					                        // }
					                    }
					                }
					            },
					            data:outer
					        }]
				            var name=[],
								n=1,
								width;
							//把某些数据变为负
							for(var m = 0;m<data.Result.rgqx_head_bar_brush.Data.length;m++){
								if(data.Result.rgqx_head_bar_brush.Data[m].TypeMatchId==77002 || data.Result.rgqx_head_bar_brush.Data[m].TypeMatchId==2){	
									for(var n=0;n<data.Result.rgqx_head_bar_brush.Data[m].data.length;n++){
										data.Result.rgqx_head_bar_brush.Data[m].data[n] = -data.Result.rgqx_head_bar_brush.Data[m].data[n]
									}
								}
							}
							//遍历图例
							for(var p=0;p<data.Result.rgqx_head_bar_brush.DicInfoMatch.length;p++){
								for(var q = 0;q<data.Result.rgqx_head_bar_brush.Data.length;q++){
									if(data.Result.rgqx_head_bar_brush.DicInfoMatch[p].TypeMatchId==data.Result.rgqx_head_bar_brush.Data[q].TypeMatchId){
										data.Result.rgqx_head_bar_brush.Data[q].name=data.Result.rgqx_head_bar_brush.Data[q].name+data.Result.rgqx_head_bar_brush.DicInfoMatch[p].Name
											
									}
								}
							}
							//生成新的series
							for(var i = 0;i<data.Result.rgqx_head_bar_brush.DicInfo.length;i++){
								for(var j = 0;j<data.Result.rgqx_head_bar_brush.Data.length;j++){			
										if(data.Result.rgqx_head_bar_brush.DicInfo[i].TypeId==data.Result.rgqx_head_bar_brush.Data[j].TypeId){
											if(j%data.Result.rgqx_head_bar_brush.DicInfoMatch.length==0){
											    n++
											}
											if(data.Result.rgqx_head_bar_brush.Data[0].data.length==7){
								               	width=20
								            }else if(data.Result.rgqx_head_bar_brush.Data[0].data.length==30){
								               	width=10
								            }
											series.push({
												name: data.Result.rgqx_head_bar_brush.Data[j].name,
												type: 'bar',
												stack:n,
												itemStyle:'',
												barWidth:width,
												data: data.Result.rgqx_head_bar_brush.Data[j].data  
												});
										
									}
									
								}
							}
							//对应的柱状图添加颜色
							for(var i = 0 ;i<series.length;i++){
								 for(var j = 0;j<echartsColors.length;j++){
									if(series[i].name==echartsColors[j].name){
										series[i].itemStyle={normal:{color:echartsColors[j].color}}
										}
									}
							}
							if(data.Result.rgqx_head_bar_brush.Data.length>0 && data.Result.rgqx_head_pie_nest.Data.length>0){
								twoOption('#echart51',Nname,data.Result.rgqx_head_bar_brush.DataLegend,series)
							}else{
								$('#echart51').html('暂无数据……')
							}
				            
			       		}else if(data.Result.rgqx_body_pie_nest){
				            //腰部文章清洗入库
				            $('.rgqxBody').html(ejs.render($('#rgqxBody').html(),{data:data.Result.rgqx_body_pie_nest}));
				            $('.rgqxBody ol').width(($('.rgqxBody').width()-1)/data.Result.rgqx_body_pie_nest.Info.length)
				            var inside = [],
				            	outer=[],
				            	name1=[],
				            	name2=[],
				            	position='',
				            	Nname=[];
				            	//遍历内圆数据
				            for(var i = 0;i<data.Result.rgqx_body_pie_nest.Data.length;i++){
				            	if(data.Result.rgqx_body_pie_nest.Data[i].TypeId==0){
				            		inside.push({
				            			value:data.Result.rgqx_body_pie_nest.Data[i].value,
				            			name:data.Result.rgqx_body_pie_nest.Data[i].name,
				            			itemStyle:''
				            		})
				            		//对应的内环添加颜色
				            		for(var j = 0;j<echartsColors.length;j++){
										if(inside[i].name==echartsColors[j].name){
											inside[i].itemStyle={normal:{color:echartsColors[j].color}}
										}
									}
				            	}
				            }
				            //当内圆只有一个数据的时候，lengend居中显示
				            if(inside.length==1){
				            	position = 'center'
				            }else{
				            	position = 'inner'
				            }

				            for(var i = 0;i<data.Result.rgqx_body_pie_nest.Data.length;i++){
				            	if(data.Result.rgqx_body_pie_nest.Data[i].TypeId!=0){
									outer.push({
										value:data.Result.rgqx_body_pie_nest.Data[i].value,
				            			name:data.Result.rgqx_body_pie_nest.Data[i].name,
				            			itemStyle:'',
				            			TypeMatchId:data.Result.rgqx_body_pie_nest.Data[i].TypeMatchId
									})
				            	}
				            }

				            for(var i= 0;i<outer.length;i++){
				            	if(outer[i].TypeMatchId==77001){
				            		outer[i].name=outer[i].name+"可用"
				            	}else if(outer[i].TypeMatchId==77002){
				            		outer[i].name=outer[i].name+"作废"
				            	}else if (outer[i].TypeMatchId==77003){
				            		outer[i].name=outer[i].name+"置为腰"
				            	}
				            	//对应的外环添加颜色
				            	for(var j = 0;j<echartsColors.length;j++){
									if(outer[i].name==echartsColors[j].name){
										outer[i].itemStyle={normal:{color:echartsColors[j].color}}
									}
								}
				            }
				            //遍历图例
				            for(var i = 0;i<data.Result.rgqx_body_pie_nest.Data.length;i++){
				            	if(data.Result.rgqx_body_pie_nest.Data[i].TypeId==0){
				            		name1.push(data.Result.rgqx_body_pie_nest.Data[i].name,data.Result.rgqx_body_pie_nest.Data[i].name+"可用",data.Result.rgqx_body_pie_nest.Data[i].name+"作废",data.Result.rgqx_body_pie_nest.Data[i].name+"置为腰")
				            	}
				            }
				            Nname = name1.join(",").split(",");
				            series=[{
					            name:'',
					            type:'pie',
					            selectedMode: 'single',
					            radius: [0, '30%'],
					            center: ['15%', '50%'],
					            label: {
					                normal: {
					                    position: position,
					                    show:false,
					                    textStyle:{
					                    	color:'#fff'
					                    }
					                }
					            },
					            labelLine: {
					                normal: {
					                    show: false
					                }
					            },
					            data:inside
					        },
					        {
					            name:'',
					            type:'pie',
					            radius: ['40%', '55%'],
					            center: ['15%', '50%'],
					            label: {
					                normal: {
					                    formatter: '{c}篇\n{d}%   ',
					                    // backgroundColor: '#eee',
					                    // borderColor: '#aaa',
					                    // borderWidth: 1,
					                    // borderRadius: 4,
					                    rich: {
					                        // a: {
					                        //     color: '#999',
					                        //     lineHeight: 22,
					                        //     align: 'center'
					                        // },
					                        // hr: {
					                        //     borderColor: '#aaa',
					                        //     width: '100%',
					                        //     borderWidth: 0.5,
					                        //     height: 0
					                        // },
					                        // b: {
					                        //     fontSize: 16,
					                        //     lineHeight: 33
					                        // },
					                        // per: {
					                        //     color: '#eee',
					                        //     backgroundColor: '#334455',
					                        //     padding: [2, 4],
					                        //     borderRadius: 2
					                        // }
					                    }
					                }
					            },
					            data:outer
					        }]
				            var name=[],
								n=1;
							//把某些数据变为负
							for(var m = 0;m<data.Result.rgqx_body_bar_brush.Data.length;m++){
								if(data.Result.rgqx_body_bar_brush.Data[m].TypeMatchId==77002 || data.Result.rgqx_body_bar_brush.Data[m].TypeMatchId==2){	
									for(var n=0;n<data.Result.rgqx_body_bar_brush.Data[m].data.length;n++){
										data.Result.rgqx_body_bar_brush.Data[m].data[n] = -data.Result.rgqx_body_bar_brush.Data[m].data[n]
									}
								}
							}
							//遍历图例
							for(var p=0;p<data.Result.rgqx_body_bar_brush.DicInfoMatch.length;p++){
								for(var q = 0;q<data.Result.rgqx_body_bar_brush.Data.length;q++){
									if(data.Result.rgqx_body_bar_brush.DicInfoMatch[p].TypeMatchId==data.Result.rgqx_body_bar_brush.Data[q].TypeMatchId){
										data.Result.rgqx_body_bar_brush.Data[q].name=data.Result.rgqx_body_bar_brush.Data[q].name+data.Result.rgqx_body_bar_brush.DicInfoMatch[p].Name
											
									}
								}
							}
							//生成新的series
							for(var i = 0;i<data.Result.rgqx_body_bar_brush.DicInfo.length;i++){
								for(var j = 0;j<data.Result.rgqx_body_bar_brush.Data.length;j++){			
										if(data.Result.rgqx_body_bar_brush.DicInfo[i].TypeId==data.Result.rgqx_body_bar_brush.Data[j].TypeId){
											if(j%data.Result.rgqx_body_bar_brush.DicInfoMatch.length==0){
											    n++
											}
											if(data.Result.rgqx_body_bar_brush.Data[0].data.length==7){
								               	width=20
								            }else if(data.Result.rgqx_body_bar_brush.Data[0].data.length==30){
								               	width=10
								            }
											series.push({
												name: data.Result.rgqx_body_bar_brush.Data[j].name,
												type: 'bar',
												stack:n,
												itemStyle:'',
												data: data.Result.rgqx_body_bar_brush.Data[j].data  
												});
										
									}
									
								}
							}
							//对应的柱状图添加颜色
							for(var i = 0 ;i<series.length;i++){
								 for(var j = 0;j<echartsColors.length;j++){
									if(series[i].name==echartsColors[j].name){
										series[i].itemStyle={normal:{color:echartsColors[j].color}}
										}
									}
							}
							if(data.Result.rgqx_body_bar_brush.Data.length>0 && data.Result.rgqx_body_pie_nest.Data.length>0){
				            	twoOption('#echart52',Nname,data.Result.rgqx_body_bar_brush.DataLegend,series)
				            }else{
				            	$('#echart52').html('暂无数据……')
				            }
			        	}else if (data.Result.rgqx_head_wzcj){
			        		var rgqx_wzcj_str = '';
			                for(var i=0;i<data.Result.rgqx_head_wzcj.DicInfo.length;i++){
			                    rgqx_wzcj_str += '<option TypeId='+data.Result.rgqx_head_wzcj.DicInfo[i].TypeId+'>'+data.Result.rgqx_head_wzcj.DicInfo[i].Name+'</option>'
			                }	
			                $('.rgqx_cj_select').html(rgqx_wzcj_str)
			        		//清洗头部文章在场景上的分布
			        		get_param()
			        		get_selectData("#echart27",data.Result.rgqx_head_wzcj,'.rgqx_cj_select option:selected','{d}%\n{c}篇 ')
							//select部分选择变化的时候
							$(".rgqx_cj_select").on("change",function(){
								// 获取参数值
								get_param()
								get_selectData("#echart27",data.Result.rgqx_head_wzcj,'.rgqx_cj_select option:selected','{d}%\n{c}篇 ');
							})
							//清洗头部帐号在场景上的分布
							var rgqx_zhcj_str = '';
			                for(var i=0;i<data.Result.rgqx_head_zhcj.DicInfo.length;i++){
			                    rgqx_zhcj_str += '<option TypeId='+data.Result.rgqx_head_zhcj.DicInfo[i].TypeId+'>'+data.Result.rgqx_head_zhcj.DicInfo[i].Name+'</option>'
			                }	
			                $('.rgqx_zhcj_select').html(rgqx_zhcj_str)
							get_param()
							get_selectData("#echart28",data.Result.rgqx_head_zhcj,'.rgqx_zhcj_select option:selected','{d}%\n{c}篇 ')
							//select部分选择变化的时候
							$(".rgqx_zhcj_select").on("change",function(){
								// 获取参数值
								get_param()
								get_selectData("#echart28",data.Result.rgqx_head_zhcj,'.rgqx_zhcj_select option:selected','{d}%\n{c}篇 ');
							})
						}else if(data.Result.rgqx_head_wzfz){
							//清洗头部文章在文章分值上的分布
							var rgqx_wzfz_str = '';
			                for(var i=0;i<data.Result.rgqx_head_wzfz.DicInfo.length;i++){
			                    rgqx_wzfz_str += '<option TypeId='+data.Result.rgqx_head_wzfz.DicInfo[i].TypeId+'>'+data.Result.rgqx_head_wzfz.DicInfo[i].Name+'</option>'
			                }	
			                $('.rgqx_wzfz_select').html(rgqx_wzfz_str)
							get_param()
							get_selectData("#echart29",data.Result.rgqx_head_wzfz,'.rgqx_wzfz_select option:selected','{d}%\n{c}篇 ')
							//select部分选择变化的时候
							$(".rgqx_wzfz_select").on("change",function(){
								// 获取参数值
								get_param()
								get_selectData("#echart29",data.Result.rgqx_head_wzfz,'.rgqx_wzfz_select option:selected','{d}%\n{c}篇 ');
							})
							//清洗头部账号在账号分值上的分布
							var rgqx_zhfz_str = '';
			                for(var i=0;i<data.Result.rgqx_head_zhfz.DicInfo.length;i++){
			                    rgqx_zhfz_str += '<option TypeId='+data.Result.rgqx_head_zhfz.DicInfo[i].TypeId+'>'+data.Result.rgqx_head_zhfz.DicInfo[i].Name+'</option>'
			                }	
			                $('.rgqx_zhfz_select').html(rgqx_zhfz_str)
							get_param()
							get_selectData("#echart30",data.Result.rgqx_head_zhfz,'.rgqx_zhfz_select option:selected','{d}%\n{c}篇 ')
							//select部分选择变化的时候
							$(".rgqx_zhfz_select").on("change",function(){
								// 获取参数值
								get_param()
								get_selectData("#echart30",data.Result.rgqx_head_zhfz,'.rgqx_zhfz_select option:selected','{d}%\n{c}篇 ')
							})
						}else if(data.Result.rgqx_body_wzlb){
							//清洗腰部文章在文类别上的分布
							if(data.Result.rgqx_body_wzlb.Data.length){
								get_sectoroption("#echart31",data.Result.rgqx_body_wzlb.DataLegend,data.Result.rgqx_body_wzlb.Data,'{d}%\n{c}篇 ');
							}else{
								$('#echart31').html('暂无数据……')
							}
						}	
		        	}
		        }
		    })
	}
	//定义一个封装的请求函数
	function get_fzdata () {
		$.ajax({
				url : public_url+"/api/Encapsulate/RenderEncapsulate",
		        // url : './json/fzEchart.json',
		        type : "get",
		        xhrFields : {
		            withCredentials : true
		        },
		        data:param,
		        crossDomain:true,
		        success:function(data){
		        	if(data.Status == 0){
		        		if(data.Result.Encapsulate_HeadPie){
			        		//物料封装扇形图
			        		var total = 0,
				        		value = [],
				        		series = [],
				        		name = [];
				        	for(var i = 0;i < data.Result.Encapsulate_HeadPie.Data.length;i++){
						        value.push(data.Result.Encapsulate_HeadPie.Data[i].value)
						        total+=value[i]
						        series.push({
						            value:data.Result.Encapsulate_HeadPie.Data[i].value,
						            name:data.Result.Encapsulate_HeadPie.Data[i].name,
						            itemStyle:''
						            })
						        for(var j = 0;j<echartsColors.length;j++){
									if(series[i].name==echartsColors[j].name){
										series[i].itemStyle={normal:{color:echartsColors[j].color}}
									}
								}
						    }
						    console.log(series)
						    for(var i = 0;i<data.Result.Encapsulate_HeadPie.Data.length;i++){
						    	name.push(data.Result.Encapsulate_HeadPie.Data[i].name)
						    }
						   	series = [{
					            name: '',
					            type: 'pie',
					            label: {
					                normal: {
					                    formatter: '{c}篇\n{d}%   ',
					                }
				            	},
				            	markPoint:{
					                symbol:'rect',
					                symbolSize:0.1,
					                //symbolOffset:[0, '50%'],
					                label:{
					                    normal:{
					                        color:'black',
					                        fontWeight:'bold'
					                    }
					                },
					                data:[{
					                    //name:'122',
					                    x:'15%',
					                    y:'50%',
					                    value:total
					                }]
					            },
					            radius: ['30%', '60%'],
					            center: ['15%', '50%'],
					            data:series,
					            textStyle:{
					            	fontSize:12
					            },
					            itemStyle: {
					                emphasis: {
					                    shadowBlur: 10,
					                    shadowOffsetX: 0,
					                    shadowColor: 'rgba(0, 0, 0, 0.5)'
					                }
					            }
				        	}]
							//物料封装柱形图
							var width;
						    for(var i = 0;i<data.Result.Encapsulate_HeadBar.Series.length;i++){
						    	if(data.Result.Encapsulate_HeadBar.Series[0].Data.length==7){
						            width=20
						        }else if(data.Result.Encapsulate_HeadBar.Series[0].Data.length==30){
						            width=10
						        }
								series.push({
									name: data.Result.Encapsulate_HeadBar.Series[i].Name,
									type: 'bar',
									stack: '总量',
									barWidth:width,
									itemStyle:'',
									data: data.Result.Encapsulate_HeadBar.Series[i].Data
								});
							}
							//对应的柱状图添加颜色
							for(var i = 0 ;i<series.length;i++){
								 for(var j = 0;j<echartsColors.length;j++){
									if(series[i].name==echartsColors[j].name){
										series[i].itemStyle={normal:{color:echartsColors[j].color}}
										}
									}
							}
						    $('.fz').html(ejs.render($('#fz').html(),{data:data.Result.Encapsulate_HeadPie}));
						    $('.fz ol').width((98/data.Result.Encapsulate_HeadPie.DataLegend.length)+'%')
							if(data.Result.Encapsulate_HeadBar.Series.length>0 && data.Result.Encapsulate_HeadPie.Data.length>0 ){
								twoOption('#echart11',name,data.Result.Encapsulate_HeadBar.DateList,series,total)
							}else{
								$('#echart11').html('暂无数据……')
							}
						}else if(data.Result.Encapsulate_Scene){
						    //封装物料在场景上的分布
						    var fz_wzcj_str = '<option typeid="0">全部</option>';;
			                for(var i=0;i<data.Result.Encapsulate_Scene.DicInfo.length;i++){
			                    fz_wzcj_str += '<option TypeId='+data.Result.Encapsulate_Scene.DicInfo[i].TypeId+'>'+data.Result.Encapsulate_Scene.DicInfo[i].Name+'</option>'
			                }	
			                $('.fz_wzcj_select').html(fz_wzcj_str)
						    get_param()
							get_selectData("#echart34",data.Result.Encapsulate_Scene,'.fz_wzcj_select option:selected','{d}%\n{c}篇 ')
							//select部分选择变化的时候
							$(".fz_wzcj_select").on("change",function(){
							// 获取参数值
								get_param()
								get_selectData("#echart34",data.Result.Encapsulate_Scene,'.fz_wzcj_select option:selected','{d}%\n{c}篇 ');

							})
							//封装物料在账号分值上的分布
							var fz_zhcj_str = '<option typeid="0">全部</option>';
			                for(var i=0;i<data.Result.Encapsulate_Account.DicInfo.length;i++){
			                    fz_zhcj_str += '<option TypeId='+data.Result.Encapsulate_Account.DicInfo[i].TypeId+'>'+data.Result.Encapsulate_Account.DicInfo[i].Name+'</option>'
			                }	
			                $('.fz_zhcj_select').html(fz_zhcj_str)
							get_param()
							get_selectData("#echart35",data.Result.Encapsulate_Account,'.fz_zhcj_select option:selected','{d}%\n{c}篇 ')
							//select部分选择变化的时候
							$(".fz_zhcj_select").on("change",function(){
							// 获取参数值
								get_param()
								get_selectData("#echart35",data.Result.Encapsulate_Account,'.fz_zhcj_select option:selected','{d}%\n{c}篇 ')
							})
						}else if(data.Result.Encapsulate_Essay){
							//封装物料在头部文章分值上的分布
							var fz_wzfz_str = '<option typeid="0">全部</option>';
			                for(var i=0;i<data.Result.Encapsulate_Essay.DicInfo.length;i++){
			                    fz_wzfz_str += '<option TypeId='+data.Result.Encapsulate_Essay.DicInfo[i].TypeId+'>'+data.Result.Encapsulate_Essay.DicInfo[i].Name+'</option>'
			                }	
			                $('.fz_wzfz_select').html(fz_wzfz_str)
							get_param()
							get_selectData("#echart36",data.Result.Encapsulate_Essay,'.fz_wzfz_select option:selected','{d}%\n{c}篇 ')
							//select部分选择变化的时候
							$(".fz_wzfz_select").on("change",function(){
							// 获取参数值
								get_param()
								get_selectData("#echart36",data.Result.Encapsulate_Essay,'.fz_wzfz_select option:selected','{d}%\n{c}篇 ');
							})
							//封装物料在物料状态上的分布
							var fz_wlzt_str = '<option typeid="0">全部</option>';
			                for(var i=0;i<data.Result.Encapsulate_Condition.DicInfo.length;i++){
			                    fz_wlzt_str += '<option TypeId='+data.Result.Encapsulate_Condition.DicInfo[i].TypeId+'>'+data.Result.Encapsulate_Condition.DicInfo[i].Name+'</option>'
			                }	
			                $('.fz_wlzt_select').html(fz_wlzt_str)
							get_param()
							get_selectData("#echart32",data.Result.Encapsulate_Condition,'.fz_wlzt_select option:selected','{d}%\n{c}篇 ')
							//select部分选择变化的时候
							$(".fz_wlzt_select").on("change",function(){
							// 获取参数值
								get_param()
								get_selectData("#echart32",data.Result.Encapsulate_Condition,'.fz_wlzt_select option:selected','{d}%\n{c}篇 ');
							})
						}
		        	}
		        }
		    })
	}
	//定义一个分发的请求函数
	function get_distributedata () {
		$.ajax({
				url : public_url+'/api/Distribute/RenderDistribute',
		        // url : './json/distributeEchart.json',
		        type : "get",
		        xhrFields : {
		            withCredentials : true
		        },
		        data:param,
		        crossDomain:true,
		        success:function(data){
		        	if(data.Status == 0){
		        		if(data.Result.Distribute_Head){
			        		//物料分发
			        		$('.distribute').html(ejs.render($('#distribute').html(),{data:data.Result.Distribute_Head}));
			        		$('.distribute ol').width((98/data.Result.Distribute_Head.DataLegend.length)+'%')
			        		var series=[],
			        			width;
						    for(var i = 0;i<data.Result.Distribute_Head.Series.length;i++){
						    	if(data.Result.Distribute_Head.Series[0].Data.length==7){
						               	width=20
						            }else if(data.Result.Distribute_Head.Series[0].Data.length==30){
						               	width=10
						            }
								series.push({
									name: data.Result.Distribute_Head.Series[i].Name,
									type: 'bar',
									barGap:'10%',
									barWidth:width,
									itemStyle:'',
									data: data.Result.Distribute_Head.Series[i].Data
								});
								for(var j = 0;j<echartsColors.length;j++){
										if(series[i].name==echartsColors[j].name){
											series[i].itemStyle={normal:{color:echartsColors[j].color}}
										}
									}
							}
							if(data.Result.Distribute_Head.Series.length){
						   		get_baroveroption('#echart58',data.Result.Distribute_Head.NameList,series,data.Result.Distribute_Head.DateList)
					   		}else{
					   			$('#echart58').html('暂无数据……')
					   		}
					   	}else if(data.Result.Distribute_Materia){
						   	//分发物料在物料类型上的分布
						   	var fenf_wllx_str = '<option typeid="0">全部</option>';;
			                for(var i=0;i<data.Result.Distribute_Materia.DicInfo.length;i++){
			                    fenf_wllx_str += '<option TypeId='+data.Result.Distribute_Materia.DicInfo[i].TypeId+'>'+data.Result.Distribute_Materia.DicInfo[i].Name+'</option>'
			                }	
			                $('.distribute_wllx_select').html(fenf_wllx_str)
						   	get_param()
							get_selectData("#echart37",data.Result.Distribute_Materia,'.distribute_wllx_select option:selected','{d}%\n{c}篇 ')
							//select部分选择变化的时候
							$(".distribute_wllx_select").on("change",function(){
							 // 获取参数值
								get_param()
								get_selectData("#echart37",data.Result.Distribute_Materia,'.distribute_wllx_select option:selected','{d}%\n{c}篇 ');
							})
						   	//分发物料在场景上的分布
						   	var fenf_cj_str = '<option typeid="0">全部</option>';;
			                for(var i=0;i<data.Result.Distribute_Scene.DicInfo.length;i++){
			                    fenf_cj_str += '<option TypeId='+data.Result.Distribute_Scene.DicInfo[i].TypeId+'>'+data.Result.Distribute_Scene.DicInfo[i].Name+'</option>'
			                }	
			                $('.distribute_cj_select').html(fenf_cj_str)
						   	get_param()
							get_selectData("#echart38",data.Result.Distribute_Scene,'.distribute_cj_select option:selected','{d}%\n{c}篇 ')
							//select部分选择变化的时候
							$(".distribute_cj_select").on("change",function(){
							 // 获取参数值
								get_param()
								get_selectData("#echart38",data.Result.Distribute_Scene,'.distribute_cj_select option:selected','{d}%\n{c}篇 ');
							})
						}else if(data.Result.Distribute_Account){
						   	//分发物料在账号分值上的分布
						   	var fenf_zhfz_str = '<option typeid="0">全部</option>';;
			                for(var i=0;i<data.Result.Distribute_Account.DicInfo.length;i++){
			                    fenf_zhfz_str += '<option TypeId='+data.Result.Distribute_Account.DicInfo[i].TypeId+'>'+data.Result.Distribute_Account.DicInfo[i].Name+'</option>'
			                }	
			                $('.distribute_zhfz_select').html(fenf_zhfz_str)
						   	get_param()
						   	get_selectData("#echart39",data.Result.Distribute_Account,'.distribute_zhfz_select option:selected','{d}%\n{c}篇 ')
							//select部分选择变化的时候
							$(".distribute_zhfz_select").on("change",function(){
							 // 获取参数值
								get_param()
								get_selectData("#echart39",data.Result.Distribute_Account,'.distribute_zhfz_select option:selected','{d}%\n{c}篇 ');
							})
						   	//分发物料在头部文章分值上的分布
						   	var fenf_wfz_str = '<option typeid="0">全部</option>';;
			                for(var i=0;i<data.Result.Distribute_Essay.DicInfo.length;i++){
			                    fenf_wfz_str += '<option TypeId='+data.Result.Distribute_Essay.DicInfo[i].TypeId+'>'+data.Result.Distribute_Essay.DicInfo[i].Name+'</option>'
			                }	
			                $('.distribute_wzfz_select').html(fenf_wfz_str)
						   	get_param()
						   	get_selectData("#echart40",data.Result.Distribute_Essay,'.distribute_wzfz_select option:selected','{d}%\n{c}篇 ')
							//select部分选择变化的时候
							$(".distribute_wzfz_select").on("change",function(){
							 // 获取参数值
								get_param()
								get_selectData("#echart40",data.Result.Distribute_Essay,'.distribute_wzfz_select option:selected','{d}%\n{c}篇 ');
							})
						}
		        	}
		        }
		    })
	}
	//定义一个转发的请求函数
	function get_transmitdata () {
		$.ajax({
				url : public_url+'/api/Forward/RenderForward',
		        // url : './json/transmitEchart.json',
		        type : "get",
		        xhrFields : {
		            withCredentials : true
		        },
		        data:param,
		        crossDomain:true,
		        success:function(data){
		        	if(data.Status == 0){
						if(data.Result.Forward_HeadPie){
			        		//物料封装扇形图
			        		var total = 0,
				        		value = [],
				        		series = [],
				        		name = [],
				        		pieseries =[];
				        	for(var i = 0;i < data.Result.Forward_HeadPie.Data.length;i++){
						        value.push(data.Result.Forward_HeadPie.Data[i].value)
						        total+=value[i]
						        series.push({
						            value:data.Result.Forward_HeadPie.Data[i].value,
						            name:data.Result.Forward_HeadPie.Data[i].name,
						            itemStyle:''
						            })
						        for(var j = 0;j<echartsColors.length;j++){
									if(series[i].name==echartsColors[j].name){
										series[i].itemStyle={normal:{color:echartsColors[j].color}}
									}
								}

						    }
						    for(var i = 0;i<data.Result.Forward_HeadPie.Data.length;i++){
						    	name.push(data.Result.Forward_HeadPie.Data[i].name)
						    }
							
							series = [{
					            name: '',
					            type: 'pie',
					            radius: ['30%', '60%'],
					            center: ['15%', '50%'],
					            data:series,
					            markPoint:{
					                symbol:'rect',
					                symbolSize:0.1,
					                //symbolOffset:[0, '50%'],
					                label:{
					                    normal:{
					                        color:'black',
					                        fontWeight:'bold'
					                    }
					                },
					                data:[{
					                    //name:'122',
					                    x:'15%',
					                    y:'50%',
					                    value:total
					                }]
					            },
					            label: {
					                normal: {
					                    formatter: '{c}次\n{d}% ',
					                }
				            	},
					            textStyle:{
					            	fontSize:12
					            },
					            itemStyle: {
					                emphasis: {
					                    shadowBlur: 10,
					                    shadowOffsetX: 0,
					                    shadowColor: 'rgba(0, 0, 0, 0.5)'
					                }
					            }
				        	}]
							//物料封装柱形图
							var width;
						    for(var i = 0;i<data.Result.Forward_HeadBar.Series.length;i++){
						    	if(data.Result.Forward_HeadBar.Series[0].Data.length==7){
						            width=20
						        }else if(data.Result.Forward_HeadBar.Series[0].Data.length==30){
						            width=10
						        }
								series.push({
									name: data.Result.Forward_HeadBar.Series[i].Name,
									type: 'bar',
									stack: '总量',
									barWidth:width,
									itemStyle:'',
									data: data.Result.Forward_HeadBar.Series[i].Data
								});
							}
							//对应的柱状图添加颜色
							for(var i = 0 ;i<series.length;i++){
								 for(var j = 0;j<echartsColors.length;j++){
									if(series[i].name==echartsColors[j].name){
										series[i].itemStyle={normal:{color:echartsColors[j].color}}
										}
									}
							}

						    $('.transmit').html(ejs.render($('#transmit').html(),{data:data.Result.Forward_HeadPie}));
						   	$('.transmit ol').width((98/data.Result.Forward_HeadPie.DataLegend.length)+'%')
				
							if(data.Result.Forward_HeadPie.Data.length>0 && data.Result.Forward_HeadBar.Series.length){
								twoOption('#echart12',name,data.Result.Forward_HeadBar.DateList,series,total)
							}else{
								$('#echart12').html('暂无数据……')
							}
							
						}else if(data.Result.Forward_Materia){
						    //物料的转发次数在物料类型上的分布
						    var zf_wfz_str = '<option typeid="0">全部</option>';;
			                for(var i=0;i<data.Result.Forward_Materia.DicInfo.length;i++){
			                    zf_wfz_str += '<option TypeId='+data.Result.Forward_Materia.DicInfo[i].TypeId+'>'+data.Result.Forward_Materia.DicInfo[i].Name+'</option>'
			                }	
			                $('.transmit_wllx_select').html(zf_wfz_str)
						    get_param()
							get_selectData("#echart41",data.Result.Forward_Materia,'.transmit_wllx_select option:selected','{c}次\n{d}% ')
							//select部分选择变化的时候
							$(".transmit_wllx_select").on("change",function(){
							// 获取参数值
								get_param()
								get_selectData("#echart41",data.Result.Forward_Materia,'.transmit_wllx_select option:selected','{c}次\n{d}% ');
							})
							//物料的转发次数在场景上的分布
							var zf_cj_str = '<option typeid="0">全部</option>';;
			                for(var i=0;i<data.Result.Forward_Scene.DicInfo.length;i++){
			                    zf_cj_str += '<option TypeId='+data.Result.Forward_Scene.DicInfo[i].TypeId+'>'+data.Result.Forward_Scene.DicInfo[i].Name+'</option>'
			                }	
			                $('.transmit_cj_select').html(zf_cj_str)
							get_param()
							get_selectData("#echart42",data.Result.Forward_Scene,'.transmit_cj_select option:selected','{c}次\n{d}% ')
							//select部分选择变化的时候
							$(".transmit_cj_select").on("change",function(){
							// 获取参数值
								get_param()
								get_selectData("#echart42",data.Result.Forward_Scene,'.transmit_cj_select option:selected','{c}次\n{d}% ');
							})
						}else if(data.Result.Forward_Essay){
							//物料的转发次数在头部账号分值上的分布
							var zf_zhfz_str = '<option typeid="0">全部</option>';;
			                for(var i=0;i<data.Result.Forward_Account.DicInfo.length;i++){
			                    zf_zhfz_str += '<option TypeId='+data.Result.Forward_Account.DicInfo[i].TypeId+'>'+data.Result.Forward_Account.DicInfo[i].Name+'</option>'
			                }	
			                $('.transmit_zhfz_select').html(zf_zhfz_str)
							get_param()
							get_selectData("#echart43",data.Result.Forward_Account,'.transmit_zhfz_select option:selected','{c}次\n{d}% ')
							//select部分选择变化的时候
							$(".transmit_zhfz_select").on("change",function(){
							// 获取参数值
								get_param()
								get_selectData("#echart43",data.Result.Forward_Account,'.transmit_zhfz_select option:selected','{c}次\n{d}% ');
							})
							//物料的转发次数在头部文章分值上的分布
							var zf_wzfz_str = '<option typeid="0">全部</option>';;
			                for(var i=0;i<data.Result.Forward_Account.DicInfo.length;i++){
			                    zf_wzfz_str += '<option TypeId='+data.Result.Forward_Account.DicInfo[i].TypeId+'>'+data.Result.Forward_Account.DicInfo[i].Name+'</option>'
			                }	
			                $('.transmit_wzfz_select').html(zf_wzfz_str)
							get_param()
							get_selectData("#echart44",data.Result.Forward_Essay,'.transmit_wzfz_select option:selected','{c}次\n{d}% ')
							//select部分选择变化的时候
							$(".transmit_wzfz_select").on("change",function(){
							// 获取参数值
								get_param()
								get_selectData("#echart44",data.Result.Forward_Essay,'.transmit_wzfz_select option:selected','{c}次\n{d}% ');
							})
						}
		        	}
		        }
		    })
	}
	//定义一个线索的函数
	function get_threaddata () {
		$.ajax({
				url : public_url+'/api/Clue/RenderClue',
		        // url : './json/threadEchart.json',
		        type : "get",
		        xhrFields : {
		            withCredentials : true
		        },
		        data:param,
		        crossDomain:true,
		        success:function(data){
		        	if(data.Status == 0){
		        		if(data.Result.Clue_HeadPie){
			        		 $('.thread').html(ejs.render($('#thread').html(),{data:data.Result.Clue_HeadPie}));
			        		 $('.thread ol').width((98/data.Result.Clue_HeadPie.DataLegend.length)+'%')
			        		 //线索内嵌图
			        		 var name=[],
			        		 	Nname=[],
			        		 	series=[],
			        		 	inside=[],
			        		 	outer=[],
			        		 	position='',
			        		 	width;
			        		 	//内圆的数据
			        		 	for(var i = 0;i<data.Result.Clue_HeadPie.InsideData.length;i++){
			        		 		inside.push({
			        		 			value:data.Result.Clue_HeadPie.InsideData[i].value,
				            			name:data.Result.Clue_HeadPie.InsideData[i].name,
				            			itemStyle:'',
				            			TypeId:data.Result.Clue_HeadPie.InsideData[i].TypeId
			        		 		})
			        		 		//对应的内环添加颜色
				            		for(var j = 0;j<echartsColors.length;j++){
										if(inside[i].name==echartsColors[j].name){
											inside[i].itemStyle={normal:{color:echartsColors[j].color}}
										}
									}
			        		 	}
			        		 	//当内圆只有一个数据的时候，lengend居中显示
					            if(inside.length==1){
					            	position = 'center'
					            }else{
					            	position = 'inner'
					            }
				            	//外圆的数据
				            	for(var i = 0;i<data.Result.Clue_HeadPie.OutsideData.length;i++){
				            		if(data.Result.Clue_HeadPie.OutsideData[i].value!=0){
				            			outer.push({
										value:data.Result.Clue_HeadPie.OutsideData[i].value,
				            			name:data.Result.Clue_HeadPie.OutsideData[i].name,
				            			itemStyle:'',
				            			TypeId:data.Result.Clue_HeadPie.OutsideData[i].TypeId
									})
				            		}
									
					            }
					            for(var i=0;i<outer.length;i++){
					            	for(var j=0;j<data.Result.Clue_HeadPie.InsideData.length;j++){
					            		if(outer[i].TypeId==data.Result.Clue_HeadPie.InsideData[j].TypeId){
					            			outer[i].name=data.Result.Clue_HeadPie.InsideData[j].name+outer[i].name
					            			Nname.push(data.Result.Clue_HeadPie.InsideData[j].name,outer[i].name)
					            		}
					            	}
					            	//对应的外环添加颜色
					            	for(var j = 0;j<echartsColors.length;j++){
											if(outer[i].name==echartsColors[j].name){
												outer[i].itemStyle={normal:{color:echartsColors[j].color}}
											}
									}
					            }

			        		 	series=[{
						            name:'',
						            type:'pie',
						            selectedMode: 'single',
						            radius: [0, '30%'],
						            center: ['15%', '50%'],
						            label: {
						                normal: {
						                    position: position,
						                    show:false,
						                    textStyle:{
						                    	color:'#fff'
						                    }
						                }
						            },
						            labelLine: {
						                normal: {
						                    show: false
						                }
						            },
						            data:inside
						        },
						        {
						            name:'',
						            type:'pie',
						            radius: ['40%', '55%'],
						            center: ['15%', '50%'],
						            label: {
						                normal: {
						                    formatter: '{c}条\n{d}% ',
						                    // backgroundColor: '#eee',
						                    // borderColor: '#aaa',
						                    // borderWidth: 1,
						                    // borderRadius: 4,
						                    rich: {
						                        // a: {
						                        //     color: '#999',
						                        //     lineHeight: 22,
						                        //     align: 'center'
						                        // },
						                        // hr: {
						                        //     borderColor: '#aaa',
						                        //     width: '100%',
						                        //     borderWidth: 0.5,
						                        //     height: 0
						                        // },
						                        // b: {
						                        //     fontSize: 16,
						                        //     lineHeight: 33
						                        // },
						                        // per: {
						                        //     color: '#eee',
						                        //     backgroundColor: '#334455',
						                        //     padding: [2, 4],
						                        //     borderRadius: 2
						                        // }
						                    }
						                }
						            },
						            data:outer
					       	 	}]
	
				           //线索柱形图
				           var name=[],
								n=1;	
							
							for(var i = 0;i<data.Result.Clue_HeadBar.Series.length;i++){
								if(i%data.Result.Clue_HeadBar.NameList.length==0 ){
								    n++
								}
								if(data.Result.Clue_HeadBar.Series[0].Data.length==7){
						               	width=20
						        }else if(data.Result.Clue_HeadBar.Series[0].Data.length==30){
						               	width=10
						        }
								series.push({
									name: data.Result.Clue_HeadBar.Series[i].Name,
									type: 'bar',
									stack:n,
									barWidth:width,
									itemStyle:'',
									TypeId:data.Result.Clue_HeadBar.Series[i].TypeId, 
									data: data.Result.Clue_HeadBar.Series[i].Data  
									});
								}
								
							//对应的柱状图添加颜色
							for(var i = 0 ;i<series.length;i++){
								for(var m = 0;m < data.Result.Clue_HeadPie.InsideData.length;m++ ){
									if(series[i].TypeId==data.Result.Clue_HeadPie.InsideData[m].TypeId){
										series[i].name=data.Result.Clue_HeadPie.InsideData[m].name+series[i].name
									}
								}
								 for(var j = 0;j<echartsColors.length;j++){
									if(series[i].name==echartsColors[j].name){
										series[i].itemStyle={normal:{color:echartsColors[j].color}}
										}
									}
							}
							if(data.Result.Clue_HeadPie.InsideData.length && data.Result.Clue_HeadPie.InsideData.length>0){
								twoOption('#echart53',Nname,data.Result.Clue_HeadBar.DateList,series)
							}else{
								$('#echart53').html('暂无数据……')
							}
							
						}else if(data.Result.Clue_Materia){
							//产生的线索在物料类型上的分布
							var xs_wllx_str = '<option typeid="0">全部</option>';;
			                for(var i=0;i<data.Result.Clue_Materia.DicInfo.length;i++){
			                    xs_wllx_str += '<option TypeId='+data.Result.Clue_Materia.DicInfo[i].TypeId+'>'+data.Result.Clue_Materia.DicInfo[i].Name+'</option>'
			                }	
			                $('.xs_wllx_select').html(xs_wllx_str)
							get_param()
							get_selectData("#echart45",data.Result.Clue_Materia,'.xs_wllx_select option:selected','{c}条\n{d}% ')
							//select部分选择变化的时候
							$(".xs_wllx_select").on("change",function(){
								// 获取参数值
								get_param()
								get_selectData("#echart45",data.Result.Clue_Materia,'.xs_wllx_select option:selected','{c}条\n{d}% ');
							})
							//产生的线索在场景上的分布
		
							var xs_cj_str = '<option typeid="0">全部</option>';;
			                for(var i=0;i<data.Result.Clue_Scene.DicInfo.length;i++){
			                    xs_cj_str += '<option TypeId='+data.Result.Clue_Scene.DicInfo[i].TypeId+'>'+data.Result.Clue_Scene.DicInfo[i].Name+'</option>'
			                }	
			                $('.xs_cj_select').html(xs_cj_str)
							get_param()
				        	get_selectData("#echart46",data.Result.Clue_Scene,'.xs_cj_select option:selected','{c}条\n{d}% ')
							//select部分选择变化的时候
							$(".xs_cj_select").on("change",function(){
								// 获取参数值
								get_param()
								get_selectData("#echart46",data.Result.Clue_Scene,'.xs_cj_select option:selected','{c}条\n{d}% ');
		
							})
						}else if(data.Result.Clue_Account){
							//产生的线索在账号分值上的分布
							var xs_zhfz_str = '<option typeid="0">全部</option>';;
			                for(var i=0;i<data.Result.Clue_Account.DicInfo.length;i++){
			                    xs_zhfz_str += '<option TypeId='+data.Result.Clue_Account.DicInfo[i].TypeId+'>'+data.Result.Clue_Account.DicInfo[i].Name+'</option>'
			                }	
			                $('.xs_zhfz_select').html(xs_zhfz_str)
							get_param()
				        	get_selectData("#echart47",data.Result.Clue_Account,'.xs_zhfz_select option:selected','{c}条\n{d}% ')
							//select部分选择变化的时候
							$(".xs_zhfz_select").on("change",function(){
								// 获取参数值
								get_param()
								get_selectData("#echart47",data.Result.Clue_Account,'.xs_zhfz_select option:selected','{c}条\n{d}% ');
							})
							//产生的线索在头部文章分值上的分布
							var xs_wzfz_str = '<option typeid="0">全部</option>';;
			                for(var i=0;i<data.Result.Clue_Essay.DicInfo.length;i++){
			                    xs_wzfz_str += '<option TypeId='+data.Result.Clue_Essay.DicInfo[i].TypeId+'>'+data.Result.Clue_Essay.DicInfo[i].Name+'</option>'
			                }	
			                $('.xs_wzfz_select').html(xs_wzfz_str)
							get_param()
				        	get_selectData("#echart48",data.Result.Clue_Essay,'.xs_wzfz_select option:selected','{c}条\n{d}% ')
							//select部分选择变化的时候
							$(".xs_wzfz_select").on("change",function(){
								// 获取参数值
								get_param()
								get_selectData("#echart48",data.Result.Clue_Essay,'.xs_wzfz_select option:selected','{c}条\n{d}% ');
							})
						}
		        	}
		        }
		    })
	}
	//定义一个方法获取下拉的数据
function get_selectData (dom,selData,sel,unit) {
		var name = [];
		var value = [];
		var TypeId = parseInt($(sel).attr("typeid"));
		var wzlbData = [],
			valData=0;
		for(var i = 0;i < selData.Data.length;i++){
			if (selData.Data[i].TypeId == TypeId) {
				name.push(selData.Data[i].name)
				}      
			}
		for(var i = 0;i < selData.Data.length;i++){
			if (selData.Data[i].TypeId == TypeId && selData.Data[i].value!=0) {
			     value.push({
			     	value:selData.Data[i].value,
			     	name:selData.Data[i].name,
			     	itemStyle:''
			     })
			    }
			}
		for(var i = 0;i < value.length;i++){
			 for(var j = 0;j<echartsColors.length;j++){
				if(value[i].name==echartsColors[j].name){
					value[i].itemStyle={normal:{color:echartsColors[j].color}}
				}
			}
		}
		console.log(value)
		for(var i=0;i<value.length;i++){
			if(i<4){
				wzlbData.push(value[i])
				
			}else{
				valData+=value[i].value
			}
		}
		console.log(TypeId)
		console.log(wzlbData)
		if(value.length>=4 && valData!=0 ){
			wzlbData.push({TypeId:-1,value:valData,name:'其它'})
			selData.DataLegend.push('其它')
		}
		get_sectoroption(dom,selData.DataLegend,wzlbData,unit)
	}
	$('.echarts ul').eq(0).show().siblings().hide()
	var tab = GetUserId().Tab;
	//跳转对应tab
	if(tab=='0'){
		$(".tab_menu li").eq(0).addClass('selected').siblings('li').removeClass('selected');
		setTimeout(function(){
			$(".tab_menu li").eq(0).click();
		},50)
		get_param(grabcategory[0]+','+grabcategory[1])
		get_Grabdata ()
		grabScroll()	
	}else if(tab=='1'){
		$(".tab_menu li").eq(1).addClass('selected').siblings('li').removeClass('selected');
		setTimeout(function(){
			$(".tab_menu li").eq(1).click();
		},50)
		get_param(jxcategory[0]+','+jxcategory[1])
		get_jxdata ()
		jxScroll()
	}else if(tab=='2'){
		$(".tab_menu li").eq(2).addClass('selected').siblings('li').removeClass('selected');
		setTimeout(function(){
			$(".tab_menu li").eq(2).click();
		},50)
		cxppScroll()
	}else if(tab=='3'){
		$(".tab_menu li").eq(3).addClass('selected').siblings('li').removeClass('selected');
		setTimeout(function(){
			$(".tab_menu li").eq(3).click();
		},50)
		get_param(cxppgory[0]+','+cxppgory[1])
		get_cxppdata ()
		csScroll()
	}else if(tab=='4'){
		$(".tab_menu li").eq(4).addClass('selected').siblings('li').removeClass('selected');
		setTimeout(function(){
			$(".tab_menu li").eq(4).click();
		},50)
		rgqxScroll()
	}else if(tab=='5'){
		$(".tab_menu li").eq(5).addClass('selected').siblings('li').removeClass('selected');
		setTimeout(function(){
			$(".tab_menu li").eq(5).click();
		},50)
		fzScroll()
	}else if(tab=='6'){
		$(".tab_menu li").eq(6).addClass('selected').siblings('li').removeClass('selected');
		setTimeout(function(){
			$(".tab_menu li").eq(6).click();
		},50)
		fenfScroll()
	}else if(tab=='7'){
		$(".tab_menu li").eq(7).addClass('selected').siblings('li').removeClass('selected');
		setTimeout(function(){
			$(".tab_menu li").eq(7).click();
		},50)
		zfScroll()
	}else if(tab=='8'){
		$(".tab_menu li").eq(8).addClass('selected').siblings('li').removeClass('selected');
		setTimeout(function(){
			$(".tab_menu li").eq(8).click();
		},50)
		xsScroll()
	}
	//点击上面的tab进行切换
	$('.tab_menu li').off('click').on('click', function () {
		$(this).addClass('selected').siblings('li').removeClass('selected');
		var val = $(this).val();
        var ind = $(this).index();
        $('.echarts ul').eq(ind).show().siblings().hide()
        switch (val) {
        	case 1 :
        		$(".echarts_data .special").off("click").on("click",function () {
			        $(".echarts_data .special").removeClass('effect_select');
			        $(this).addClass('effect_select');
			        get_param(grabcategory[0]+','+grabcategory[1])
			 		get_Grabdata ()
			 		grabScroll ()
				})
        		grabScroll()
		        break;
		    case 2 :
		    	get_param(jxcategory[0]+','+jxcategory[1])
			 	get_jxdata ()
			    jxScroll()
		    	$(".echarts_data .special").off("click").on("click",function () {
			        $(".echarts_data .special").removeClass('effect_select');
			        $(this).addClass('effect_select');
			        get_param(jxcategory[0]+','+jxcategory[1])
			 		get_jxdata ()
			        jxScroll()
				})
		    	break;
		    case 3 :
		    	get_param(cxppgory[0]+','+cxppgory[1])
			 	get_cxppdata ()
		    	cxppScroll()
		    	$(".echarts_data .special").off("click").on("click",function () {
			        $(".echarts_data .special").removeClass('effect_select');
			        $(this).addClass('effect_select');
			        get_param(cxppgory[0]+','+cxppgory[1])
			 		get_cxppdata ()
		    		cxppScroll()
			     })  
		    	break;
		   	case 4 :
		   		get_param(csgory[0]+','+csgory[1])
			 	get_csdata ()
		    	csScroll()
		   		$(".echarts_data .special").off("click").on("click",function () {
			        $(".echarts_data .special").removeClass('effect_select');
			        $(this).addClass('effect_select');
			        get_param(csgory[0]+','+csgory[1])
			 		get_csdata ()
		    		csScroll()
			     })
		    	break;
		    case 5 :
		    	get_param(rgqxgroy[0]+','+rgqxgroy[1])
			 	get_rgqxdata ()
		    	rgqxScroll()
		    	$(".echarts_data .special").off("click").on("click",function () {
			        $(".echarts_data .special").removeClass('effect_select');
			        $(this).addClass('effect_select');
			        get_param(rgqxgroy[0]+','+rgqxgroy[1])
			 		get_rgqxdata ()
		    		rgqxScroll()
			     })
		    	break;
		    case 6 :
		    	get_paramL(fzgroy[0]+','+fzgroy[1])
			 	get_fzdata ()
		    	fzScroll()
		    	$(".echarts_data .special").off("click").on("click",function () {
			        $(".echarts_data .special").removeClass('effect_select');
			        $(this).addClass('effect_select');
			        get_paramL(fzgroy[0]+','+fzgroy[1])
			 		get_fzdata ()
		    		fzScroll()
			     })
		    	break;
		    case 7 :
		    	get_paramL(fenfgroy[0])
			 	get_distributedata ()
		    	fenfScroll()
		    	$(".echarts_data .special").off("click").on("click",function () {
			        $(".echarts_data .special").removeClass('effect_select');
			        $(this).addClass('effect_select');
			        get_paramL(fenfgroy[0])
			 		get_distributedata ()
		    		fenfScroll()
			     })
		    	break;
		    case 8 :
		    	get_paramL(zfgroy[0]+','+zfgroy[1])
			 	get_transmitdata ()
		    	zfScroll()
		    	$(".echarts_data .special").off("click").on("click",function () {
			        $(".echarts_data .special").removeClass('effect_select');
			        $(this).addClass('effect_select');
			        get_paramL(zfgroy[0]+','+zfgroy[1])
			 		get_transmitdata ()
		    		zfScroll()
			     })
		    	break;
		   	case 9 :
		   		get_paramL(xsgroy[0]+','+xsgroy[1])
			 	get_threaddata ()
		    	xsScroll()
		    	$(".echarts_data .special").off("click").on("click",function () {
			        $(".echarts_data .special").removeClass('effect_select');
			        $(this).addClass('effect_select');
			        get_paramL(xsgroy[0]+','+xsgroy[1])
			 		get_threaddata ()
		   			xsScroll()
			     })
        }	
	})
	//明细数据
	$('#goIntoDetail').on('click',function(event){
		var idx = $('.tab_menu li.selected').index();
    	window.open('/DispenseManager/DataListDetail.html?Status=' + idx );
    })
	//点击天数选择切换
    $(".echarts_data li").off("click").on("click",function () {
    	var that = $(this);
    	that.addClass('effect_select').siblings('li').removeClass('effect_select');
    	get_param(grabcategory[0]+','+grabcategory[1])
		get_Grabdata ()
        grabScroll()
	})
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

})