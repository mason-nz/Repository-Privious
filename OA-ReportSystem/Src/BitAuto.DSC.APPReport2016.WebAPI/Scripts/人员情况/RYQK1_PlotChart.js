
var AppReport = AppReport || {};
AppReport.RYQKHZ = (function () {
    var InitPersonSum = function () {
        AjaxGet("/api/Empolyee/GetEmpolyeeCount?r=" + Math.random(), null, null, function (data) {
            var jsonDataResult = ProcessRequestJsonData(data);
            $("#spPersonSum").text(jsonDataResult.Data.total);
            $("#spAddPersons").MagicNumber(jsonDataResult.Data.entry);
            $("#spLeavePersons").MagicNumber(jsonDataResult.Data.dimission);
        });
    }
    var InitLineChart = function () {
        myChart_line.showLoading(defaultEChartsLoadingStyle);
        AjaxGet("/api/Empolyee/GetEmpolyeeForMonth?r=" + Math.random(), null, null, function (data) {
            var jsonDataResult = ProcessRequestJsonData(data);
            //x轴初始数据索引
            var startXindex = 0;
            var endXindex = jsonDataResult.Data.datakey.length - 1;
            //柱图最大值
            var array = jsonDataResult.Data.dataval[0];
            var maxbarvalue = GetMaxNumber(array);
            maxbarvalue = maxbarvalue * 2;
            maxbarvalue = Number(maxbarvalue).toMaxRound();
            //标注颜色
            var markcolor = ["#fc6061", "#16ede5"];
            var mincolor = jsonDataResult.Data.markPoint[0] >= 0 ? markcolor[0] : markcolor[1];
            var maxcolor = jsonDataResult.Data.markPoint[1] >= 0 ? markcolor[0] : markcolor[1];
            var lastcolor = jsonDataResult.Data.markPoint[2] >= 0 ? markcolor[0] : markcolor[1];

            // 基于准备好的dom，初始化echarts实例
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
                        //console.log(params);
                        var res = "";
                        if (params.length > 0) {
                            //索引
                            var dataIndex = params[0].dataIndex;
                            var name = params[0].name.substring(0, 4) + "年" + parseInt(params[0].name.substring(5)) + "月";
                            var yuangongshu = jsonDataResult.Data.dataval[0][dataIndex];
                            var huanbivalue = Number(jsonDataResult.Data.dataval[1][dataIndex]);
                            var huanbistr = Number(jsonDataResult.Data.dataval[1][dataIndex]).toPercent(1);

                            res = name;
                            res += "<br/><span style='font-weight:normal;color:rgba(200, 200, 200, 1);'>" + "员工数" + '</span>：'
                            + yuangongshu;
                            res += "<br/><span style='font-weight:normal;color:rgba(200, 200, 200, 1);'>" + "增长率" + '</span>：环比上月'
                            + (huanbivalue < 0 ? huanbistr : '+' + huanbistr);
                        }
                        return res;
                    }
                },
                legend: {
                    data: [{ name: '员工数', icon: 'circle' }, { name: '增长率', icon: 'circle'}],
                    top: 9,
                    right: 25,
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
                    startValue: startXindex, //数据范围开始结束
                    endValue: endXindex,
                    xAxisIndex: 0,
                    zoomLock: true
                }],
                grid: [{
                    left: 40,
                    right: 10,
                    bottom: 43,
                    top: 42,
                    containLabel: false,
                    show: false
                }, {
                    left: 40,
                    right: 10,
                    bottom: '65%',
                    top: 60,
                    containLabel: false,
                    show: false
                }],
                xAxis: [{
                    gridIndex: 0,
                    type: 'category',
                    boundaryGap: true,
                    data: jsonDataResult.Data.datakey,
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
                            fontSize: 12,
                            interval: 2
                        },
                        formatter: function (value, index) {
                            // 格式化成月/日，只在第一个刻度显示年份 yyyy-MM
                            var yearMonth = value.substring(5);
                            if (yearMonth == "01" || yearMonth == "02") {
                                return value.substring(2, 4) + "年\n" + parseInt(value.substring(5)) + "月";
                            }
                            else {
                                return parseInt(value.substring(5)) + "月";
                            }
                        }
                    }
                }, {
                    gridIndex: 1,
                    type: 'category',
                    boundaryGap: true,
                    data: jsonDataResult.Data.datakey,
                    axisLine: {
                        show: false
                    },
                    axisTick: {
                        show: false
                    },
                    axisLabel: {
                        show: false
                    }
                }],
                yAxis: [{
                    gridIndex: 0,
                    type: 'value',
                    max: maxbarvalue,
                    interval: maxbarvalue / 4,
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
                        formatter: function (value, index) { //去掉千分位
                            return value;
                        }
                    }
                }, {
                    gridIndex: 1,
                    type: 'value',
                    scale: true,
                    name: '增长率',
                    show: false,
                    axisLine: {
                        show: false,
                        lineStyle: {
                            color: 'rgba(51, 51, 51, 1)'
                        }
                    },
                    axisTick: {
                        show: false
                    },
                    splitLine: {
                        show: false
                    },
                    axisLabel: {
                        show: false
                    }
                }],
                series: [{
                    xAxisIndex: 0,
                    yAxisIndex: 0,
                    name: '员工数',
                    type: 'bar',
                    barWidth: 8,
                    itemStyle: {
                        normal: {
                            color: '#18ece5',
                            barBorderRadius: 4
                        }
                    },
                    data: jsonDataResult.Data.dataval[0]
                }, {
                    xAxisIndex: 1,
                    yAxisIndex: 1,
                    name: '增长率',
                    type: 'line',
                    symbol: 'circle',
                    symbolSize: 6,
                    showSymbol: false,
                    itemStyle: {
                        normal: {
                            color: '#fbe262'
                        }
                    },
                    lineStyle: {
                        normal: {
                            color: '#fbe262',
                            width: 2,
                            type: 'solid'
                        }
                    },
                    data: jsonDataResult.Data.dataval[1],
                    markPoint: {
                        symbol: 'circle',
                        symbolSize: 6,
                        label: {
                            normal: {
                                show: true,
                                position: 'top',
                                formatter: function (params) {
                                    var res = params.value;
                                    var floatValue = parseFloat(res);
                                    if (floatValue >= 0) {
                                        return "+" + floatValue.toPercent(1);
                                    }
                                    else {
                                        return floatValue.toPercent(1);
                                    }
                                }
                            }
                        },
                        data: [{
                            type: 'max', name: '最高值', label: {
                                normal: {
                                    show: true,
                                    textStyle: { color: maxcolor }
                                }
                            }
                        }, {
                            type: 'min', name: '最低值', label: {
                                normal: {
                                    show: true,
                                    position: 'bottom',
                                    textStyle: { color: mincolor }
                                }
                            }
                        }, {
                            name: '最新值',
                            xAxis: jsonDataResult.Data.datakey[jsonDataResult.Data.datakey.length - 1],
                            yAxis: jsonDataResult.Data.dataval[1][jsonDataResult.Data.dataval[1].length - 1],
                            value: jsonDataResult.Data.dataval[1][jsonDataResult.Data.dataval[1].length - 1],
                            label: {
                                normal: {
                                    show: true,
                                    textStyle: { color: lastcolor }
                                }
                            }
                        }],
                        silent: true
                    }
                }]
            };
            // 使用刚指定的配置项和数据显示图表。
            myChart_line.hideLoading();
            myChart_line.setOption(option);
        });
    };
    return {
        InitPersonSum: InitPersonSum,
        InitLineChart: InitLineChart
    };
})();