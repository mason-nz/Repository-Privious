var AppReport = AppReport || {};
AppReport.XCHYHZ = (function () {
    //贡献标题
    var InitSumHYGX = function () {
        $.get("/api/xchyhz/GetAvgAmount?r=" + Math.random(), null, function (data) {
            var jsonDataResult = ProcessRequestJsonData(data);
            $("#spYear").text(jsonDataResult.Data.year);

            //$("#spAV").text(Number(jsonDataResult.Data.allavg).addComma(0));
            $("#spCYT").text(Number(jsonDataResult.Data.cytavg).addComma(0));
            $("#spCMT").text(Number(jsonDataResult.Data.cmtavg).addComma(0));
            $("#spWXT").text(Number(jsonDataResult.Data.wxtavg).addComma(0));
        });
    };
    //线图
    var InitLineChart = function () {
        myChart_line.showLoading(defaultEChartsLoadingStyle);
        AjaxGet("/api/xchyhz/GetMemberAmountForQuarter?r=" + Math.random(), null, null, function (data) {
            var jsonDataResult = ProcessRequestJsonData(data);
            //x轴初始数据索引
            var startXindex = 0;
            var endXindex = jsonDataResult.Data.datakey.length - 1;
            //y轴最大值
            var maxvalue = GetMaxNumber(jsonDataResult.Data.dataval[0]);
            maxvalue = Number(maxvalue).toMaxRound();

            var option = {
                tooltip: {
                    trigger: 'axis',
                    triggerOn: 'click', //默认鼠标移动时触发，如果设置click则鼠标点击时触发
                    axisPointer: {            // 坐标轴指示器，坐标轴触发有效
                        type: 'line'        // 默认为直线，可选为：'line' | 'shadow'
                    },
                    backgroundColor: 'rgba(0, 0, 0, 0.7)', //提示框背景颜色
                    textStyle: {
                        color: 'rgba(255, 255, 255, 1)',
                        fontFamily: '微软雅黑',
                        fontWeight: 400,
                        fontSize: 12
                    },
                    formatter: function (params) {
                        if (params.length > 0) {
                            var dataIndex = params[0].dataIndex;
                            var nowdate = jsonDataResult.Data.datakey[dataIndex];

                            var html = nowdate + "<br/>";
                            for (var i = 0; i < params.length; i++) {
                                var value_i = Math.round(params[i].value);
                                var name_i = params[i].seriesName;

                                if (value_i)
                                    html += "<span style='font-weight:normal;color:rgba(200, 200, 200, 1);'>" + name_i + "：</span>" + value_i + "元<br />";
                            }
                            return html;
                        }
                        else {
                            return "";
                        }
                    }
                },
                legend: {
                    data: [{ name: '车易通', icon: 'circle' }, { name: '车盟通', icon: 'circle' }, { name: '微信通', icon: 'circle'}],
                    top: -5,
                    right: 25,
                    textStyle: {
                        color: '#99a8d1',
                        fontFamily: '微软雅黑',
                        fontWeight: 400,
                        fontStyle: 'normal',
                        fontSize: 14
                    },
                    itemGap: 25,
                    itemWidth: 8,
                    itemHeight: 8,
                    selectedMode: true,
                    inactiveColor: "#36415d"
                },
                dataZoom: [{
                    type: 'inside',
                    startValue: startXindex, //数据范围开始结束
                    endValue: endXindex,
                    xAxisIndex: 0,
                    zoomLock: true
                }],
                grid: {
                    left: 70,
                    right: 30,
                    top: 28,
                    bottom: 43,
                    containLabel: false
                },
                xAxis: {
                    type: 'category',
                    boundaryGap: false,
                    data: jsonDataResult.Data.datakey,
                    axisLine: {
                        lineStyle: {
                            color: '#36415d'
                        }
                    },
                    axisTick: {
                        show: false
                    },
                    axisLabel: {
                        show: true,
                        textStyle: {
                            color: '#36415d',
                            fontFamily: '微软雅黑',
                            fontWeight: 400,
                            fontStyle: 'normal',
                            fontSize: 12
                        },
                        formatter: function (value, index) {
                            if (value.substr(4) != "Q1") {
                                return value.substr(4);
                            }
                            else {
                                return value;
                            }
                        }
                    }
                },
                yAxis: {
                    type: 'value',
                    axisLine: {
                        lineStyle: {
                            color: '#36415d'
                        }
                    },
                    axisTick: {
                        show: false
                    },
                    splitLine: {
                        show: true,
                        lineStyle: {
                            color: '#36415d'
                        }
                    },
                    axisLabel: {
                        show: true,
                        textStyle: {
                            color: '#36415d',
                            fontFamily: '微软雅黑',
                            fontWeight: 400,
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
                series: [
               
                {
                    name: '车易通',
                    type: 'line',
                    symbol: 'circle',
                    symbolSize: 6,
                    itemStyle: {
                        normal: {
                            color: '#3F85FF'
                        }
                    },
                    lineStyle: {
                        normal: {
                            color: '#3F85FF',
                            width: 2,
                            type: 'solid'
                        }
                    },
                    data: jsonDataResult.Data.dataval[0]
                },
                {
                    name: '车盟通',
                    type: 'line',
                    symbol: 'circle',
                    symbolSize: 6,
                    itemStyle: {
                        normal: {
                            color: '#18ECE5'
                        }
                    },
                    lineStyle: {
                        normal: {
                            color: '#18ECE5',
                            width: 2,
                            type: 'solid'
                        }
                    },
                    data: jsonDataResult.Data.dataval[1]
                },
                {
                    name: '微信通',
                    type: 'line',
                    symbol: 'circle',
                    symbolSize: 6,
                    itemStyle: {
                        normal: {
                            color: '#FDE061'
                        }
                    },
                    lineStyle: {
                        normal: {
                            color: '#FDE061',
                            width: 2,
                            type: 'solid'
                        }
                    },
                    data: jsonDataResult.Data.dataval[2]
                }]
            };
            // 使用刚指定的配置项和数据显示图表。
            myChart_line.hideLoading();
            myChart_line.setOption(option);
        });
    };

    //合作标题
    var InitSumHYHZ = function () {
        //查询车易通
        AjaxGet("/api/xchyhz/GetMemberHZForDay?ItemId=90001&r=" + Math.random(), null, null, function (data) {
            //console.log(data);
            if (data.Status == 0) {
                var date = DateFormat.StringToDate(data.Result.date);
                $("#bartitle_date").text(DateFormat.DateToString(date, "MM月dd日"));
                $("#bartitle_name").text(data.Result.name);
                $("#bartitle_num1").text(data.Result.count.addComma());
                $("#bartitle_year").text(DateFormat.DateToString(date, "yy年"));
                $("#bartitle_num2").text(data.Result.maxcount.addComma());
            }
            else {
            }
        });
    };
    //图表数据
    var barData = {
        //服务器获取的数据
        seriesname: [],
        xAxisdata: [],
        seriesdata: [[], []],
        zoomindex: [],
        //图表用到的颜色集合 柱1，柱2，柱3，预估1，预估2，预估3
        seriescolor: ['#3f85ff', '#19ede6', '#fde162', '#2f3660', '#4e5484', '#7b82ae'],
        //转换方法
        //把seriesname转换成图表用的数据
        GetLegendData: function () {
            var data = new Array();
            for (var i = 0; i < this.seriesname.length; i++) {
                var item = {};
                item.name = this.seriesname[i];
                item.icon = "circle";
                data.push(item);
            }
            return data;
        },
        //获取X轴显示
        GetxAxisData: function () {
            var array = barData.xAxisdata;
            var result = new Array();
            for (var i = 0; i < array.length; i++) {
                var date = DateFormat.StringToDate(array[i], "yyyy-MM");
                var item = "";
                if (date.getMonth() == 0) {
                    item = DateFormat.DateToString(date, "yy年\nM月");
                }
                else {
                    item = DateFormat.DateToString(date, "M月");
                }
                result.push(item);
            }
            return result;
        },
        //把seriesdata转换成图表用的数据
        GetSeriesData: function (index) {
            var result = new Array();
            var data = this.seriesdata[index];
            var type = this.seriesdata[3];
            for (var i = 0; i < data.length; i++) {
                var value = data[i];
                //中间包含预估数据
                var item = {};
                item.value = value;
                //1是0否
                if (type[i] == 1) {
                    //是预估数据
                    item.itemStyle = {};
                    item.itemStyle.normal = {};
                    item.itemStyle.normal.color = this.seriescolor[index + 3];
                }
                result.push(item);
            }
            return result;
        }
    };
    //柱图
    var InitBarChart = function () {
        myChart_bar.showLoading(defaultEChartsLoadingStyle);
        AjaxGet("/api/xchyhz/GetMemberHZForMonth?r=" + Math.random(), null, null, function (data) {
            //console.log(data);
            if (data.Status == 0) {
                //设置数据
                barData.seriesname = data.Result.seriesname;
                barData.xAxisdata = data.Result.datakey;
                barData.seriesdata = data.Result.dataval;
                barData.zoomindex = data.Result.zoomindex;

                //y轴最大值
                var bar1max = GetMaxNumber(barData.seriesdata[0]);
                var bar2max = GetMaxNumber(barData.seriesdata[1]);
                var bar3max = GetMaxNumber(barData.seriesdata[2]);

                var array = [bar1max, bar2max, bar3max];
                var maxvalue = GetMaxNumber(array);
                maxvalue = Number(maxvalue).toMaxRound();

                //加载柱图
                var option = {
                    grid: [{
                        left: 70,
                        right: 30,
                        top: 28,
                        bottom: 43,
                        containLabel: false
                    }],
                    dataZoom: [{
                        type: 'inside',
                        startValue: barData.zoomindex[0],
                        endValue: barData.zoomindex[1],
                        zoomLock: true
                    }],
                    legend: {
                        data: barData.GetLegendData(),
                        top: 0,
                        right: 25,
                        textStyle: {
                            color: '#99a8d1',
                            fontFamily: '微软雅黑',
                            fontWeight: 400,
                            fontStyle: 'normal',
                            fontSize: 14
                        },
                        itemGap: 25,
                        itemWidth: 8,
                        itemHeight: 8,
                        selectedMode: true,
                        inactiveColor: "#36415d",
                        backgroundColor: '#323755'
                    },
                    tooltip: {
                        trigger: 'axis',
                        triggerOn: 'click', //默认鼠标移动时触发，如果设置click则鼠标点击时触发
                        axisPointer: {            // 坐标轴指示器，坐标轴触发有效
                            type: 'line'        // 默认为直线，可选为：'line' | 'shadow'
                        },
                        backgroundColor: 'rgba(0, 0, 0, 0.7)', //提示框背景颜色
                        textStyle: {
                            color: 'rgba(255, 255, 255, 1)',
                            fontFamily: '微软雅黑',
                            fontWeight: 400,
                            fontSize: 12
                        },
                        formatter: function (params) {
                            //console.log(params);
                            if (params.length > 0) {
                                var dataIndex = params[0].dataIndex;
                                var nowdate = DateFormat.StringToDate(barData.xAxisdata[dataIndex]);

                                var html = DateFormat.DateToString(nowdate, "yyyy年M月") + "<br/>";
                                for (var i = 0; i < params.length; i++) {
                                    var value_i = params[i].value;
                                    var name_i = params[i].seriesName;

                                    if (value_i)
                                        html += "<span style='font-weight:normal;color:rgba(200, 200, 200, 1);'>" + name_i + "：</span>" + value_i + "<br />";
                                }
                                return html;
                            }
                            else {
                                return "";
                            }
                        }
                    },
                    xAxis: [{
                        type: 'category',
                        position: 'bottom',
                        boundaryGap: true,
                        data: barData.GetxAxisData(),
                        axisLine: {
                            lineStyle: {
                                color: '#36415d'
                            }
                        },
                        axisTick: {
                            show: false
                        },
                        axisLabel: {
                            show: true,
                            textStyle: {
                                color: '#36415d',
                                fontFamily: '微软雅黑',
                                fontWeight: 400,
                                fontStyle: 'normal',
                                fontSize: 12
                            }
                        }
                    }],
                    yAxis: [{
                        type: 'value',
                        nameTextStyle: {
                            color: '#36415d',
                            fontFamily: '微软雅黑',
                            fontWeight: 400,
                            fontStyle: 'normal',
                            fontSize: 12
                        },
                        axisLine: {
                            lineStyle: {
                                color: '#36415d'
                            }
                        },
                        axisTick: {
                            show: false
                        },
                        splitLine: {
                            show: true,
                            lineStyle: {
                                color: '#36415d'
                            }
                        },
                        axisLabel: {
                            show: true,
                            textStyle: {
                                color: '#36415d',
                                fontFamily: '微软雅黑',
                                fontWeight: 400,
                                fontStyle: 'normal',
                                fontSize: 12
                            },
                            formatter: function (value, index) { //去掉千分位
                                return value;
                            }
                        },
                        max: maxvalue,
                        interval: maxvalue / 4
                    }],
                    series: [{
                        name: barData.seriesname[0],
                        xAxisIndex: 0,
                        yAxisIndex: 0,
                        type: 'bar',
                        barWidth: 8,
                        itemStyle: {
                            normal: {
                                color: barData.seriescolor[0],
                                barBorderRadius: 4
                            }
                        },
                        data: barData.GetSeriesData(0)
                    }, {
                        name: barData.seriesname[1],
                        xAxisIndex: 0,
                        yAxisIndex: 0,
                        type: 'bar',
                        barWidth: 8,
                        itemStyle: {
                            normal: {
                                color: barData.seriescolor[1],
                                barBorderRadius: 4
                            }
                        },
                        data: barData.GetSeriesData(1)
                    }, {
                        name: barData.seriesname[2],
                        xAxisIndex: 0,
                        yAxisIndex: 0,
                        type: 'bar',
                        barWidth: 8,
                        itemStyle: {
                            normal: {
                                color: barData.seriescolor[2],
                                barBorderRadius: 4
                            }
                        },
                        data: barData.GetSeriesData(2)
                    }]
                };
                myChart_bar.hideLoading();
                myChart_bar.setOption(option);
                //注册事件
                myChart_bar.off("legendselectchanged");
                myChart_bar.on("legendselectchanged", function (params) {
                    //console.log(params);
                    if (params.selected) {
                        var count = 0;
                        for (var item in params.selected) {
                            if (params.selected[item]) {
                                count++;
                            }
                        }
                        var option = myChart_bar.getOption();
                        if (option.dataZoom && option.dataZoom.length > 0) {
                            var zoom = option.dataZoom[0];
                            var startValue = zoom.startValue;
                            var endValue = zoom.endValue;
                            var total = barData.xAxisdata.length;

                            var result = null;
                            if (count == 1) {
                                //显示12个index
                                result = CalcStartAndEndIndex(startValue, endValue, total, 12);
                            }
                            else if (count > 1) {
                                //显示6个index
                                result = CalcStartAndEndIndex(startValue, endValue, total, 6);
                            }

                            //设置
                            if (result && result.length == 2) {
                                var option = {
                                    dataZoom: {
                                        type: 'inside',
                                        startValue: result[0],
                                        endValue: result[1],
                                        zoomLock: true
                                    }
                                };
                                myChart_bar.setOption(option);
                            }
                        }
                    }
                });
            }
            else {
                myChart_bar.clear();
            }
        });
    };
    //计算起始和终止索引
    var CalcStartAndEndIndex = function (start, end, total, count) {
        var result = new Array();
        if (total <= count) {
            result.push(0);
            result.push(total - 1);
        }
        else {
            var mid = Math.round((start + end) / 2);
            var half = Math.round(count / 2);
            //先计算newstart
            var newstart = mid - half;
            if (newstart < 0) newstart = 0;
            var newend = newstart + count - 1;
            result.push(newstart);
            result.push(newend);
        }
        return result;
    }
    return {
        InitSumHYGX: InitSumHYGX,
        InitLineChart: InitLineChart,
        InitSumHYHZ: InitSumHYHZ,
        InitBarChart: InitBarChart
    };
})();