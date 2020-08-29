/**
*===============================================================
*获取“商机监控-平台维度”图表的相关数据逻辑
*中间生成HTML代码结构
<div id="divSlide" class="layout swiper-container" style="width: 100%; clear: both;">
<div class="swiper-wrapper">
<div class="swiper-slide">
<ul class="ulitem">
<li>
<div class="liItem selected">
<p class="title">
<span>易车网PC</span>
</p>
<p class="total">
<span style="font-size: 12px;">9999.99</span> <span style="font-size: 13px;">万</span>
</p>
<div class="percent">
<p>
<span class="tb up">29.99%</span> <span style="color: #afbee7; font-size: 12px;">同比</span>
</p>
</div>
<div style="margin-left: 50%; margin-top: 0px;">
<p>
<span class="tb down">59.99%</span> <span style="color: #afbee7; font-size: 12px;">环比</span>
</p>
</div>
</div>
</li>
</ul>
</div>
</div>
</div>
*@version: 1.0
*@author:  masj
*@date:    2016-12-06
*@modifyuser:
*@modifydate:
*===============================================================
*/
var AppReport = AppReport || {};
AppReport.SJJK_PT = (function () {
    //商机监控-平台维度，拼图中图例样式数据(平台数据)
    var PingTaiStyleData = [{ id: 10001, name: '易车网PC', itemStyle: { normal: { color: '#18ECE5'} }, icon: 'image:///Images/pt_yichePC.png' },
                            { id: 10002, name: '易车网WAP', itemStyle: { normal: { color: '#FDE162'} }, icon: 'image:///Images/pt_yicheWAP.png' },
                            { id: 10005, name: '易车APP', itemStyle: { normal: { color: '#3F85FF'} }, icon: 'image:///Images/pt_yicheAPP.png' },
                            { id: 10004, name: '报价APP', itemStyle: { normal: { color: '#16A4D4'} }, icon: 'image:///Images/pt_baojiaAPP.png' },
                            { id: 10003, name: '二手车', itemStyle: { normal: { color: '#FA944A'} }, icon: 'image:///Images/pt_taoche.png' },
                            { id: 10006, name: '惠买车', itemStyle: { normal: { color: '#4850FD'} }, icon: 'image:///Images/pt_huimaiche.png'}];

    //商机监控-平台维度，拼图中图例样式数据(业务线数据)
    var YeWuXianStyleData = [{ id: 20001, name: '会员', itemStyle: { normal: { color: '#17EDE5'} }, icon: 'image:///Images/pt_yichePC.png' },
                            { id: 20003, name: '惠买车', itemStyle: { normal: { color: '#15B5E9'} }, icon: 'image:///Images/pt_baojiaAPP.png' },
                            { id: 20002, name: '二手车', itemStyle: { normal: { color: '#FCE162'} }, icon: 'image:///Images/pt_yicheWAP.png' },
                            { id: 20004, name: '易鑫', itemStyle: { normal: { color: '#3E85FF'} }, icon: 'image:///Images/pt_yicheAPP.png'}];
    //根据平台ID，获取平台名称
    var GetPingTaiNameByID = function (id) {
        for (var i = 0; i < PingTaiStyleData.length; i++) {
            if (PingTaiStyleData[i].id == id) {
                return PingTaiStyleData[i].name;
            }
        }
        return '';
    };

    //调用接口，获取饼图——系列数据
    var GetLeads_SeriesData = function (callback) {
        var url = '/api/sjjk/GetLeadsPieChartData';
        var dataParas = {
            type: 1,
            date: '',
            r: Math.random()
        };
        AjaxGet(url, dataParas, null, function (data) {
            //console.log(data);
            var json = ProcessRequestJsonData(data);
            var chartSeriesData = [];
            for (var i = 0; i < PingTaiStyleData.length; i++) {
                for (var j = 0; j < json.data.length; j++) {
                    if (json.data[j].id == PingTaiStyleData[i].id) {
                        var obj = {};
                        obj.value = json.data[j].value;
                        obj.name = PingTaiStyleData[i].name;
                        obj.itemStyle = PingTaiStyleData[i].itemStyle;
                        chartSeriesData.push(obj);
                    }
                }
            }
            if (callback) {
                callback(chartSeriesData, json);
            }
        });
    };

    //获取饼图——图例数据
    var GetLeads_LegendData = function (dataJson) {
        var chartLegendData = [];
        for (var i = 0; i < dataJson.length; i++) {
            for (var j = 0; j < PingTaiStyleData.length; j++) {
                if (dataJson[i].name == PingTaiStyleData[j].name) {
                    var obj = {};
                    obj.name = dataJson[i].name;
                    obj.icon = PingTaiStyleData[j].icon;
                    chartLegendData.push(obj);
                }
            }
        }
        return chartLegendData;
    };

    //初始化拼图
    var InitPieChart = function (divID, dataJson) {
        // 指定图表的配置项和数据
        var option = {
            legend: {
                right: '5%',
                top: '23%',
                itemHeight: 11,
                itemWidth: 11,
                itemGap: 25,
                selectedMode: false,
                orient: 'vertical',
                textStyle: defaultEChartsTextStyle,
                data: GetLeads_LegendData(dataJson)
            },
            series: [
            {
                center: ['35%', '50%'],
                name: '平台Leads数构成',
                startAngle: 90,
                minAngle: 20,
                type: 'pie',
                hoverAnimation: false,
                data: dataJson,
                label: {
                    normal: {
                        position: 'inside',
                        formatter: function (params) {
                            return (params.percent).toFixed(1) + '%';
                        },
                        textStyle: {
                            fontFamily: 'Avenir-Medium',
                            fontSize: 13
                        }
                    }
                }
            }]
        };
        myChartPie1.hideLoading();
        // 使用刚指定的配置项和数据显示图表。
        myChartPie1.setOption(option);
        myChartPie1.on('click', function (params) {
        });
    };

    //调用接口，获取 商机监控-平台维度 明细（以平台维度展示）
    var GetLeads_DetailData = function (date, callback) {
        var url = '/api/sjjk/GetLeadsBlockData';
        var dataParas = {
            type: 1,
            date: date,
            r: Math.random()
        };
        AjaxGet(url, dataParas, null, function (data) {
            //console.log(data);
            var json = ProcessRequestJsonData(data);
            if (callback) {
                callback(json);
            }
        });
    };

    //绑定 商机监控-平台维度 明细（以平台维度展示）
    var BindLeadsDetailData = function (jsonList) {
        if (jsonList && jsonList.length > 0) {
            var wrapper = $('<div>').addClass('swiper-wrapper');
            wrapper.prependTo($('#divSlide'));

            for (var j = 0; j < jsonList.length; j++) {

                var Name = GetPingTaiNameByID(jsonList[j].siteId);
                var Count = parseInt(jsonList[j].totalCount).addComma();
                var WeekBasis = jsonList[j].lastWeek; //同比
                var DayBasis = jsonList[j].yesterday; //环比
                var SiteId = jsonList[j].siteId;

                var divObj = $('<div>').addClass('liItem');
                divObj.attr('SiteId', SiteId);
                divObj.append($('<p>').addClass('title').append($('<span>').text(Name))); //展现平台名称
                divObj.append($('<p>').addClass('total').append($('<span>').css({ 'font-size': '14px' }).text(Count))); //展现平台下，覆盖用户数

                var spanPercent1 = $('<span>').addClass('tb');
                if (WeekBasis) {
                    if (WeekBasis > 0) {
                        spanPercent1.addClass('up').text(WeekBasis.toPercent(1));
                    }
                    else if (WeekBasis < 0) {
                        spanPercent1.addClass('down').text(Math.abs(WeekBasis).toPercent(1));
                    }
                    else {
                        spanPercent1.addClass('normalColor').text('———');
                    }
                    var pObj = $('<p>').append(spanPercent1)
                                       .append('&nbsp;')
                                       .append($("<span class='tb2'>").css({ 'color': '#afbee7', 'font-size': '12px' }).text('同比'));

                    $('<div>').addClass('percent')
                              .append(pObj)
                              .appendTo(divObj);
                }
                var spanPercent2 = $('<span>').addClass('tb');
                if (DayBasis) {
                    if (DayBasis > 0) {
                        spanPercent2.addClass('up').text(DayBasis.toPercent(1));
                    }
                    else if (DayBasis < 0) {
                        spanPercent2.addClass('down').text(Math.abs(DayBasis).toPercent(1));
                    }
                    else {
                        spanPercent2.addClass('normalColor').text('———');
                    }

                    var pObj = $('<p>').append(spanPercent2)
                                       .append('&nbsp;')
                                       .append($("<span class='tb2'>").css({ color: '#afbee7', 'font-size': '12px' }).text('环比'));

                    $("<div class='percent2' >")
                              .append(pObj)
                              .appendTo(divObj);
                }
                divObj = divObj.wrap('<li>').parent();

                if (j % 4 == 0) {
                    divObj.wrap($('<ul>').addClass('ulitem')).parent()
                          .wrap($('<div>').addClass('swiper-slide')).parent()
                          .appendTo(wrapper);
                }
                else {
                    wrapper.find('div.swiper-slide:last ul.ulitem li:last').after(divObj.wrap($('<li>')));
                }
            }
            InitSwiperAndBindClickEvent();

            //iphone5 尺寸处理
            if ($(document).width() < 350) {
                $('div.swiper-container div.swiper-wrapper div.swiper-slide ul.ulitem li div.liItem .total span').css({ 'font-size': '12px' });
                $('div.swiper-container div.swiper-wrapper div.swiper-slide ul.ulitem li div.liItem div span.tb').css({ 'font-size': '10px', 'width': '30px', 'background-size': '8px', 'margin-left': '-2px' });
                $('div.swiper-container div.swiper-wrapper div.swiper-slide ul.ulitem li div.liItem div span.tb2').css({ 'font-size': '10px', 'padding-left': '5px', 'padding-top': '0px' });
            }
            else {
                //箭头位置修改
                var spanlist = $('div.swiper-container div.swiper-wrapper div.swiper-slide ul.ulitem li div.liItem div span.tb');
                for (var i = 0; i < spanlist.length; i++) {
                    var newwidth = (6 - spanlist.eq(i).text().length) * 7;
                    if (newwidth > 0) {
                        spanlist.eq(i).css('background-position', newwidth + "px");
                    }
                }
            }
        }
    };

    //
    var InitSwiperAndBindClickEvent = function () {
        var liList = $('div.swiper-container div.swiper-wrapper div.swiper-slide ul.ulitem li div.liItem');
        //数量个数大于4时，才初始化拖动控件
        if (liList.size() > 4) {
            //初始化幻灯片区域
            var mySwiper = new Swiper('.swiper-container', {
                // 如果需要分页器
                pagination: '.swiper-pagination',
                paginationClickable: true
            });
        }

        //初始化中间li Click事件
        liList.bind('click', function () {
            $('div.swiper-container div.swiper-wrapper div.swiper-slide ul.ulitem li div.liItem').removeClass('selected');
            $(this).addClass('selected');
            var SiteId = $(this).attr('SiteId');
            //初始化页面下方图表
            var siteName = GetPingTaiNameByID(SiteId);
            $('#spanPT_Chart2Title,#spanPT_Chart3Title').text(siteName);
            GetLeads_YWX_SeriesData(SiteId);

            //初始化页面最下下方面积图图表
            InitLineChart(SiteId);
        });

        //默认选中第一个
        $('div.swiper-container div.swiper-wrapper div.swiper-slide ul.ulitem li div.liItem:first').click();
    };

    //调用接口，获取饼图——系列数据(业务线)
    var GetLeads_YWX_SeriesData = function (SiteId) {
        myChartPie2.showLoading(defaultEChartsLoadingStyle);
        var url = '/api/sjjk/GetLeadsCircularChartData';
        var dataParas = {
            siteId: SiteId,
            lineId: 0,
            date: '',
            r: Math.random()
        };
        AjaxGet(url, dataParas, null, function (data) {
            //console.log(data);
            var json = ProcessRequestJsonData(data);
            var chartSeriesData = [];
            for (var i = 0; i < YeWuXianStyleData.length; i++) {
                for (var j = 0; j < json.length; j++) {
                    if (json[j].id == YeWuXianStyleData[i].id) {
                        var obj = {};
                        obj.value = json[j].value;
                        obj.name = YeWuXianStyleData[i].name;
                        obj.itemStyle = YeWuXianStyleData[i].itemStyle;
                        obj.id = json[j].id;
                        chartSeriesData.push(obj);
                    }
                }
            }
            InitPieRingChart('ChartPie2', chartSeriesData);
        });
    };

    //获取饼图——图例数据(业务线维度)
    var GetYWX_LegendData = function (dataJson) {
        var chartLegendData = [];
        for (var i = 0; i < YeWuXianStyleData.length; i++) {
            for (var j = 0; j < dataJson.length; j++) {
                if (dataJson[j].id == YeWuXianStyleData[i].id) {
                    var obj = {};
                    obj.name = YeWuXianStyleData[i].name;
                    obj.icon = YeWuXianStyleData[i].icon;
                    chartLegendData.push(obj);
                }
            }
        }
        return chartLegendData;
    };

    //初始化拼图
    var InitPieRingChart = function (divID, dataJson) {
        // 指定图表的配置项和数据
        var option2 = {
            legend: {
                left: '5%',
                top: '23%',
                itemHeight: 11,
                itemWidth: 11,
                itemGap: 25,
                selectedMode: false,
                orient: 'vertical',
                textStyle: defaultEChartsTextStyle,
                data: GetYWX_LegendData(dataJson)
            },

            series: [
            {
                radius: ['40%', '60%'],
                center: ['60%', '50%'],
                name: '易车网PC Leads导向构成',
                startAngle: 90,
                minAngle: 20,
                type: 'pie',
                hoverAnimation: false,
                data: dataJson,
                label: {
                    normal: {
                        position: 'outside',
                        formatter: function (params) {
                            return params.value + '\n(' + (params.percent).toFixed(1) + '%)';
                        },
                        textStyle: {
                            color: 'white',
                            fontFamily: 'Avenir-Medium',
                            fontSize: 12
                        }
                    }
                },
                labelLine: {
                    normal: {
                        lineStyle: {
                            width: 2
                        }
                    }
                }
            }]
        };
        myChartPie2.hideLoading();

        //屏幕宽度小于iphone6的尺寸时(375px)
        if ($(window).width() < 375) {
            option2.series[0].radius = ['30%', '50%'];
        }

        // 使用刚指定的配置项和数据显示图表。
        myChartPie2.setOption(option2);
        //事件
        myChartPie2.on('click', function (params) {
        });
    };
    //线图
    var InitLineChart = function (ptID) {
        myChartLine.showLoading(defaultEChartsLoadingStyle);
        AjaxGet("/api/sjjk/GetLeadsLineChartData?r=" + Math.random(), { date: $("#spanTitleDate").html(), siteId: ptID, lineId: '0' }, null,
        function (data) {
            var jsonDataResult = ProcessRequestJsonData(data);
            //x轴初始数据索引
            var startXindex = 0;
            if (jsonDataResult.datekey.length >= 14) {
                startXindex = jsonDataResult.datekey.length - 14;
            }
            var endXindex = jsonDataResult.datekey.length - 1;

            //y轴最大值
            var bar1max = GetMaxNumber(jsonDataResult.dataval[0]);
            var bar2max = GetMaxNumber(jsonDataResult.dataval[1]);

            var array = [bar1max, bar2max];
            var maxvalue = GetMaxNumber(array);
            maxvalue = Number(maxvalue).toMaxRound();

            // 基于准备好的dom，初始化echarts实例
            var option = {
                tooltip: {
                    trigger: 'axis', //item 数据项图形触发，主要在散点图，饼图等无类目轴的图表中使用,axis 坐标轴触发，主要在柱状图，折线图等会使用类目轴的图表中使用。
                    triggerOn: 'click', //默认鼠标移动时触发，如果设置click则鼠标点击时触发
                    backgroundColor: 'rgba(0, 0, 0, 0.7)', //提示框背景颜色
                    textStyle: {
                        color: 'rgba(255, 255, 255, 1)',
                        fontFamily: '微软雅黑',
                        fontWeight: 400,
                        fontSize: 12
                    },
                    formatter: function (params) {
                        var date = DateFormat.StringToDate(jsonDataResult.datekey[params[0].dataIndex]);
                        var number1 = Number(params[0].value);
                        var number2 = Number(params[1].value);
                        var html = "<span style='font-weight:normal;color:rgba(200, 200, 200, 1);'>" + DateFormat.DateToString(date, "yyyy-MM-dd") + "</span><br/>";
                        html += "<span style='font-weight:normal;color:rgba(200, 200, 200, 1);'>" + params[0].seriesName + "：</span>" + Number(number1).addComma() + "<br/>";
                        html += "<span style='font-weight:normal;color:rgba(200, 200, 200, 1);'>" + params[1].seriesName + "：</span>" + Number(number2).addComma() + "<br/>";
                        return html;
                    }
                },
                legend: {
                    data: [{ name: 'Leads数', icon: 'circle' }, { name: '下单用户数', icon: 'circle'}],
                    top: 12,
                    right: 20,
                    textStyle: {
                        color: '#99a8d1',
                        fontFamily: '微软雅黑',
                        fontWeight: '400',
                        fontStyle: 'normal',
                        fontSize: 14
                    },
                    itemGap: 25,
                    itemWidth: 8,
                    itemHeight: 8,
                    selectedMode: false
                },
                dataZoom: [{
                    type: 'inside',
                    startValue: startXindex,
                    endValue: endXindex,
                    xAxisIndex: 0,
                    zoomLock: false
                }],
                grid: {
                    left: 60,
                    right: 20,
                    bottom: 43,
                    top: 42,
                    containLabel: false,
                    show: false
                },
                xAxis: {
                    type: 'category',
                    boundaryGap: false,
                    data: jsonDataResult.datekey,
                    axisLine: {
                        show: true,
                        lineStyle: {
                            color: '#272d45'
                        }
                    },
                    axisTick: {
                        show: false
                    },
                    axisLabel: {
                        show: true,
                        textStyle: {
                            color: '#36425c',
                            fontFamily: '微软雅黑',
                            fontWeight: '400',
                            fontStyle: 'normal',
                            fontSize: 12
                        },
                        formatter: function (value, index) {
                            // 格式化成月/日，只在第一个刻度显示年份
                            var date = new Date(value);
                            var texts = [(date.getMonth() + 1), date.getDate()];
                            if (date.getMonth() + 1 == 1 && date.getDate() == 1) {
                                texts.unshift(date.getFullYear());
                            }
                            return texts.join('-');
                        }
                    }
                },
                yAxis: {
                    nameTextStyle: {
                        color: '#363f5c',
                        fontStyle: 'normal',
                        fontWeight: 'normal',
                        fontFamily: '微软雅黑',
                        fontSize: 14
                    },
                    nameGap: 10,
                    type: 'value',
                    axisLine: {
                        show: true,
                        lineStyle: {
                            color: '#272d45'
                        }
                    },
                    axisTick: {
                        show: false

                    },
                    splitLine: {
                        show: true,
                        lineStyle: {
                            color: '#282a43'
                        }
                    },
                    axisLabel: {
                        show: true,
                        textStyle: {
                            color: '#363f5c',
                            fontFamily: '微软雅黑',
                            fontWeight: '400',
                            fontStyle: 'normal',
                            fontSize: 12
                        },
                        formatter: function (value, index) {
                            //去掉千分位
                            return value;
                        }
                    },
                    max: maxvalue,
                    interval: maxvalue / 4
                },
                series: [{
                    name: 'Leads数',
                    type: 'line',
                    symbol: 'circle', //数据点形状
                    symbolSize: 6, //图形大小
                    showSymbol: false,
                    areaStyle: { normal: { color: new echarts.graphic.LinearGradient(0, 0, 0, 1, [{
                        offset: 0, color: 'RGB(22,237,229)' // 0% 处的颜色
                    }, {
                        offset: 1, color: 'RGB(27,29,50)' // 100% 处的颜色
                    }], false)
                    }
                    },
                    itemStyle: {
                        normal: {
                            color: 'RGB(22,237,229)'
                        }
                    },
                    lineStyle: {
                        normal: {
                            color: 'RGB(22,237,229)',
                            width: '2',
                            type: 'solid'
                        }
                    },
                    data: jsonDataResult.dataval[0]
                }, {
                    name: '下单用户数',
                    type: 'line',
                    symbol: 'circle',
                    symbolSize: 6,
                    showSymbol: false,
                    areaStyle: { normal: { color: new echarts.graphic.LinearGradient(0, 0, 0, 1, [{
                        offset: 0, color: 'RGB(56,111,212)' // 0% 处的颜色
                    }, {
                        offset: 1, color: 'RGB(27,29,50)' // 100% 处的颜色
                    }], false)
                    }
                    },
                    itemStyle: {
                        normal: {
                            color: 'RGB(56,111,212)'
                        }
                    },
                    lineStyle: {
                        normal: {
                            color: 'RGB(56,111,212)',
                            width: '2',
                            type: 'solid'
                        }
                    },
                    data: jsonDataResult.dataval[1]
                }]
            };
            myChartLine.hideLoading();
            // 使用刚指定的配置项和数据显示图表。
            myChartLine.setOption(option);
        });
    };
    return {
        GetLeads_SeriesData: GetLeads_SeriesData,
        InitPieChart: InitPieChart,
        GetLeads_DetailData: GetLeads_DetailData,
        BindLeadsDetailData: BindLeadsDetailData,
        InitLineChart: InitLineChart
    };
})();