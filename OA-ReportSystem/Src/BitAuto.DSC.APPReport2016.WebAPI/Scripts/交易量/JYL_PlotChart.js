var AppReport = AppReport || {};
AppReport.JYL = (function () {

    //调用接口，获取交易量——系列数据
    var GetJYL_SeriesData = function () {
        AjaxGet("/api/jyl/GetData?r=" + Math.random(), null, null, function (data) {
            var jsonDataResult = ProcessRequestJsonData(data);
            jsonData = jsonDataResult.RetData;
            $("#sDate").html(jsonData.RetDate.RetDateVal + "&nbsp;&nbsp;");
            $("#sWeek").html(jsonData.RetDate.WeekDay);
            $('#spanHMC').MagicNumber(jsonData.huiMaiChe.Count);
            $('#spanYX').MagicNumber(jsonData.yiXin.Count);
            var hmcTB = jsonData.huiMaiChe.WeekBasis;
            var hmcHB = jsonData.huiMaiChe.DayBasis;
            var yxTB = jsonData.yiXin.WeekBasis;
            var yxHB = jsonData.yiXin.DayBasis;


            _setPanel("hmcTB", hmcTB);
            _setPanel("hmcHB", hmcHB);
            _setPanel("yxTB", yxTB);
            _setPanel("yxHB", yxHB);

        });
        var _setPanel = function (id, value) {
            if (value > 0) {
                $("#" + id + "").attr("class", "highColor");
                $("#" + id + "2").addClass("highColorText");
            }
            else if (value < 0) {
                $("#" + id + "").attr("class", "lowColor");
                $("#" + id + "2").addClass("lowColorText");
            }
            else {
                $("#" + id + "").attr("class", "normalColor");
                $("#" + id + "2").addClass("normalColor");
            }
            $("#" + id + "2").text(Number(Math.abs(value)).toPercent(1));
        };
    };

    //初始化交易量折线图
    var InitJYLChart = function () {
        // 基于准备好的dom，初始化echarts实例        
        myChart.showLoading(defaultEChartsLoadingStyle);
        AjaxGet("/api/jyl/GetDataTrend?r=" + Math.random(), null, null, function (data) {
            var jsonDataResult = ProcessRequestJsonData(data)
            var jsonData = jsonDataResult.RetData;
            //x轴初始数据索引
            var startXindex = 0;
            if (jsonData.xAxisdata.length >= 14) {
                startXindex = jsonData.xAxisdata.length - 14;
            }
            var endXindex = jsonData.xAxisdata.length - 1;

            //获取y轴最大
            var max1 = GetMaxNumber(jsonData.seriesdata[0]);
            var max2 = GetMaxNumber(jsonData.seriesdata[1]);
            var array = [max1, max2];
            var maxvalue = GetMaxNumber(array);
            maxvalue = Number(maxvalue).toMaxRound();

            var option = {
                tooltip: {
                    trigger: 'axis',
                    triggerOn: 'click',
                    backgroundColor: 'rgba(0, 0, 0, 0.7)', //提示框背景颜色
                    textStyle: {
                        color: 'rgba(255, 255, 255, 1)',
                        fontFamily: '微软雅黑',
                        fontWeight: 400,
                        fontSize: 12
                    },
                    formatter: function (params) {
                        var html = "";
                        if (params.length > 0) {
                            var date = DateFormat.StringToDate(jsonData.xAxisdata[params[0].dataIndex]);
                            var number1 = Number(params[0].value);
                            if (isNaN(number1)) {
                                number1 = "--";
                            }
                            html = "<span style='font-weight:normal;color:rgba(200, 200, 200, 1);'>" + DateFormat.DateToString(date, "yyyy-MM-dd") + "</span><br/>";
                            html += "<span style='font-weight:normal;color:rgba(200, 200, 200, 1);'>" + params[0].seriesName + "：</span>" + number1 + "<br/>";

                            if (params.length >= 2) {
                                var number2 = Number(params[1].value);
                                if (isNaN(number2)) {
                                    number2 = "--";
                                }
                                html += "<span style='font-weight:normal;color:rgba(200, 200, 200, 1);'>" + params[1].seriesName + "：</span>" + number2 + "<br/>";
                            }
                        }
                        return html;
                    }
                },
                legend: {
                    data: [{ name: '惠买车', icon: 'circle' }, { name: '易鑫', icon: 'circle'}],
                    top: 12,
                    right: 25,
                    textStyle: {
                        color: '#7781a5',
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
                    left: 50,
                    right: 30,
                    bottom: 43,
                    top: 42,
                    containLabel: false,
                    show: false
                },
                xAxis: {
                    type: 'category',
                    boundaryGap: false,
                    data: jsonData.xAxisdata,
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
                        }
                    },
                    max: maxvalue,
                    interval: maxvalue / 4
                },
                series: [{
                    name: '惠买车',
                    type: 'line',
                    symbol: 'circle',
                    symbolSize: 6,
                    showSymbol: false,
                    connectNulls: true,
                    itemStyle: {
                        normal: {
                            color: 'RGB(62,133,255)'
                        }
                    },
                    lineStyle: {
                        normal: {
                            color: 'RGB(62,133,255)',
                            width: '2',
                            type: 'solid'
                        }
                    },
                    data: jsonData.seriesdata[0]
                }, {
                    name: '易鑫',
                    type: 'line',
                    symbol: 'circle',
                    symbolSize: 6,
                    showSymbol: false,
                    connectNulls: true,
                    itemStyle: {
                        normal: {
                            color: 'RGB(253,97,95)'
                        }
                    },
                    lineStyle: {
                        normal: {
                            color: 'RGB(253,97,95)',
                            width: '2',
                            type: 'solid'
                        }
                    },
                    data: jsonData.seriesdata[1]
                }]
            };
            myChart.hideLoading();
            // 使用刚指定的配置项和数据显示图表。
            myChart.setOption(option);
        });
    };

    return {
        GetJYL_SeriesData: GetJYL_SeriesData,
        InitJYLChart: InitJYLChart
    };
})();