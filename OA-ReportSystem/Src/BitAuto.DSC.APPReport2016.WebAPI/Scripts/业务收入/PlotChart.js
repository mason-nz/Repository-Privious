/**
*===============================================================
*获取“业务收入”图表的相关数据逻辑
*@version: 1.0
*@author:  masj
*@date:    2016-12-07
*@modifyuser:
*@modifydate:
*===============================================================
*/
var AppReport = AppReport || {};
AppReport.YWSR = (function () {
    //业务收入，拼图中图例样式数据
    var BusinessStyleData = [{ id: 80001, name: '新车广告', itemStyle: { normal: { color: '#17EDE5'}} },
                            { id: 80002, name: '新车会员', itemStyle: { normal: { color: '#3E85FF'}} },
                            { id: 80003, name: '二手车', itemStyle: { normal: { color: '#FCE162'}}}];

    //    //商机监控-平台维度，拼图中图例样式数据(业务线数据)
    //    var YeWuXianStyleData = [{ id: 20001, name: '会员', itemStyle: { normal: { color: '#17EDE5'} }, icon: 'image:///Images/pt_yichePC.png' },
    //                            { id: 20002, name: '二手车', itemStyle: { normal: { color: '#FCE162'} }, icon: 'image:///Images/pt_yicheWAP.png' },
    //                            { id: 20003, name: '惠买车', itemStyle: { normal: { color: '#15B5E9'} }, icon: 'image:///Images/pt_baojiaAPP.png' },
    //                            { id: 20004, name: '易鑫', itemStyle: { normal: { color: '#3E85FF'} }, icon: 'image:///Images/pt_yicheAPP.png'}];
    //根据平台ID，获取平台名称
    var GetBusinessNameByID = function (id) {
        for (var i = 0; i < BusinessStyleData.length; i++) {
            if (BusinessStyleData[i].id == id) {
                return BusinessStyleData[i].name;
            }
        }
        return '';
    };

    //调用接口，获取饼图——系列数据
    var GetLeads_SeriesData = function (year, callback) {
        var url = '/api/ywsr/GetDataByYear';
        var dataParas = {
            operationYear: year,
            r: Math.random()
        };
        $.get(url, dataParas, function (data) {
            //console.log(data);
            var json = ProcessRequestJsonData(data);
            var chartSeriesData = [];
            for (var i = 0; i < BusinessStyleData.length; i++) {
                for (var j = 0; j < json.RetData.details.length; j++) {
                    if (json.RetData.details[j].DataType == BusinessStyleData[i].id) {
                        var obj = {};
                        obj.value = json.RetData.details[j].Count;
                        obj.name = BusinessStyleData[i].name;
                        obj.itemStyle = BusinessStyleData[i].itemStyle;
                        obj.id = BusinessStyleData[i].id;
                        chartSeriesData.push(obj);
                    }
                }
            }
            if (callback) {
                callback(chartSeriesData, json.RetData);
            }
        }, 'json');
    };

    //初始化拼图
    var InitPieChart = function (dataJson, total) {
        //console.log(dataJson); console.log(total);
        myChart1.hideLoading();
        // 指定图表的配置项和数据
        var option = {
            legend: {
                show: false
            },
            series: [{
                name: '',
                type: 'pie',
                selectedOffset: 0,
                hoverAnimation: false,
                silent: false,
                //z: 3,
                radius: [0, '35%'],
                center: ['50%', '50%'],
                label: {
                    normal: {
                        position: 'center',
                        formatter: function (params) {
                            return '总收入\n\n' + params.value.toMoney('亿') + '亿';
                        },
                        textStyle: {
                            color: 'white',
                            fontSize: 14
                        }
                    }
                },
                labelLine: {
                    normal: {
                        show: false
                    }
                },
                data: [
                    { value: total, name: '总收入', id: 0, itemStyle: { normal: { color: '#15B5E9'}} }
                ]
            },
            {
                radius: ['50%', '70%'],
                center: ['50%', '50%'],
                name: '2016年业务收入情况',
                //selectedMode: 'single',
                selectedOffset: 5,
                startAngle: 90,
                minAngle: 20,
                type: 'pie',
                hoverAnimation: false,
                //roseType: 'radius',
                data: dataJson,

                //                [
                //                { value: 18.51, name: '新车广告', itemStyle: { normal: { color: '#17EDE5'}} },
                //                { value: 0.38, name: '二手车', itemStyle: { normal: { color: '#FCE162'}} },
                //                { value: 12.95, name: '新车会员', itemStyle: { normal: { color: '#3E85FF'}}}],
                //                itemStyle: {
                //                    emphasis: {
                //                        shadowBlur: 10,
                //                        shadowOffsetX: 0,
                //                        shadowColor: 'white'
                //                    }
                //                },
                label: {
                    normal: {
                        position: 'outside',
                        //formatter: '{c}亿\n占比{d}%\n{b}',
                        formatter: function (params) {
                            //return params.value + '\n' + parseInt(params.percent) + '%';
                            return params.name + '\n' + params.value.toMoney('亿') + '亿\n(' + params.percent.toFixed(2) + '%)';
                        },
                        textStyle: {
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
        //屏幕宽度小于iphone6的尺寸时(375px)
        if ($(window).width() < 375) {
            option.series[0].radius = [0, '25%'];
            option.series[1].radius = ['40%', '60%'];
        }

        // 使用刚指定的配置项和数据显示图表。
        myChart1.setOption(option);
        //注册事件
        myChart1.off('click');
        myChart1.on('click', function (params) {
            var option = myChart1.getOption();
            var id = option.series[params.seriesIndex].data[params.dataIndex].id;
            var name = option.series[params.seriesIndex].data[params.dataIndex].name;
            if (id == 0) {
                name = '总';
            }
            var year = $('#spanTitleYear1').text();
            //取消之前选择的扇形
            for (var i = 0; i < option.series[1].data.length; i++) {
                option.series[1].data[i].selected = false;
            }
            //选中当前的扇形
            if (params.seriesIndex == 1) {
                option.series[params.seriesIndex].data[params.dataIndex].selected = true;
            }

            //            //取消之前高亮的图形
            //            for (var i = 0; i < option.series[params.seriesIndex].data.length; i++) {
            //                myChart1.dispatchAction({
            //                    type: 'downplay',
            //                    seriesIndex: params.seriesIndex,
            //                    dataIndex: i
            //                });
            //            }
            //            //高亮当前图形
            //            this.dispatchAction({
            //                type: 'highlight',
            //                seriesIndex: params.seriesIndex,
            //                dataIndex: params.dataIndex
            //            });
            this.setOption(option);
            //加载下方图表
            //console.log('id=' + id + ',name=' + name);
            InitBarChart(year, id, name);
        });
        var year = $('#spanTitleYear1').text();
        InitBarChart(year, 0, '总');
        //        //初始化加载下面图表逻辑
        //        if (option.series[1].data && option.series[1].data.length > 0) {
        //            var id = option.series[1].data[0].id;
        //            var name = option.series[1].data[0].name;
        //            var year = $('#spanTitleYear1').text();
        //            if (id && name) {
        //                //加载下方图表               
        //                //console.log('id=' + id + ',name=' + name);
        //                InitBarChart(year, id, name);
        //            }
        //        }
    };

    //绑定页面标题，导航事件
    var BindTitleChangeEvent = function (obj) {
        obj.unbind('click').bind('click', function () {
            //先设置图片为“不可用”状态
            $('#imgPre').attr('src', '/images/004.png').unbind('click');
            //BindTitleChangeEvent($('#imgPre'));
            $('#imgNext').attr('src', '/images/001.png').unbind('click');
            //BindTitleChangeEvent($('#imgNext'));

            var year = $('#spanTitleYear1').text();
            var imgObj = $(this);
            if (!isNaN(year)) {
                year = parseInt(year);
                var controlID = $(this).attr('id');

                if (controlID == 'imgPre') {
                    year = year - 1;
                }
                else if (controlID == 'imgNext') {
                    year = year + 1;
                }

                //                var date = new Date;
                //                if (date.getFullYear() < year) {
                //                    $('#imgNext').attr('src', '/images/001.png');
                //                    return;
                //                }
                //                else {
                //                    $('#imgNext').attr('src', '/images/002.png');
                //                    BindTitleChangeEvent($('#imgNext'));
                //                }



                //                if (date.getFullYear() <= year) {
                //                    $('#imgNext').attr('src', '/images/001.png');
                //                }

                //获取饼图数据源
                AppReport.YWSR.GetLeads_SeriesData(year, function (chartSeriesData, json) {
                    //初始化页面标题日期内容
                    if (json && json.retYear) {
                        //console.log(json.total);
                        if (chartSeriesData.length > 0 || json.total > 0) {
                            $('#spanTitleYear1,#spanTitleYear2').text(json.retYear);
                            //$('#spanTitleYear1').text(year);
                            //BindTitleChangeEvent(imgObj);
                            //绘制饼图
                            AppReport.YWSR.InitPieChart(chartSeriesData, json.total);
                        }
                        else {
                            var imgObjID = imgObj.attr('id');
                            imgObj.unbind('click');
                            if (imgObjID == 'imgNext') {
                                imgObj.attr('src', '/images/001.png');
                            }
                            else if (imgObjID == 'imgPre') {
                                imgObj.attr('src', '/images/004.png');
                            }
                        }
                        if (json && json.preTotal && json.preTotal == true) {
                            $('#imgPre').attr('src', '/images/003.png');
                            BindTitleChangeEvent($('#imgPre'));
                        }
                        if (json && json.nextTotal && json.nextTotal == true) {
                            $('#imgNext').attr('src', '/images/002.png');
                            BindTitleChangeEvent($('#imgNext'));
                        }
                    }
                });
            }
        });
    };

    //柱图数据
    var barData = {
        //服务器获取的数据
        seriesname: [],
        xAxisdata: [],
        seriesdata: [[], []],
        //图表用到的颜色集合
        seriescolor: ['#3f85ff', '#19ede6', '#323761', '#fde162'],
        //转换方法
        //把seriesname转换成图表用的数据
        GetLegendData: function () {
            var data = new Array();
            for (var i = 0; i < this.seriesname.length; i++) {
                var item = {};
                item.name = this.GetxSeriesnameData()[i];
                item.icon = "circle";
                data.push(item);
            }
            return data;
        },
        //获取系列名称
        GetxSeriesnameData: function () {
            var array = barData.seriesname;
            var result = new Array();
            for (var i = 0; i < array.length; i++) {
                var date = DateFormat.StringToDate(array[i], "yyyy");
                var item = DateFormat.DateToString(date, "yyyy年");
                result.push(item);
            }
            return result;
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
                if (index == 0) {
                    result.push(value ? value.toMoney("亿") : null);
                }
                else if (index == 1) {
                    //中间包含预估数据
                    var item = {};
                    item.value = value ? value.toMoney("亿") : null;
                    //1是0否
                    if (type[i] == 1) {
                        //是预估数据
                        item.itemStyle = {};
                        item.itemStyle.normal = {};
                        item.itemStyle.normal.color = this.seriescolor[2];
                    }
                    result.push(item);
                }
                else if (index == 2) {
                    result.push(value);
                }
            }
            return result;
        },
        //取2个柱子的最大值
        GetBarMaxData: function () {
            var result = new Array();
            var data0 = this.seriesdata[0];
            var data1 = this.seriesdata[1];
            for (var i = 0; i < data0.length; i++) {
                if (data0[i]) {
                    result.push(data0[i].toMoney("亿"));
                }
            }
            for (var i = 0; i < data1.length; i++) {
                if (data1[i]) {
                    result.push(data1[i].toMoney("亿"));
                }
            }
            var max = GetMaxNumber(result);
            return max;
        }
    };
    //初始化柱图
    var InitBarChart = function (year, typeid, name) {
        $("#bartitle").text(name);
        myChart2.showLoading(defaultEChartsLoadingStyle);
        //异步api获取数据-获取柱图中的数据
        AjaxGet("/api/ywsr/GetDataByYearAndDataType?operationYear=" + year + "&dataType=" + typeid + "&r=" + Math.random(), null, null, function (data) {
            //console.log(data);
            if (data.Status == 0) {
                //设置数据
                barData.seriesname = data.Result.seriesname;
                barData.xAxisdata = data.Result.datakey;
                barData.seriesdata = data.Result.dataval;
                barData.zoomindex = data.Result.zoomindex;
                barData.isonlyoneyear = data.Result.isonlyoneyear;
                //y轴最大值
                var maxvalue = barData.GetBarMaxData();
                maxvalue = Number(maxvalue).toMaxRound();

                //加载柱图
                var option = {
                    grid: [{
                        left: 50,
                        right: 30,
                        top: 28,
                        bottom: 43,
                        containLabel: false
                    }, {
                        left: barData.isonlyoneyear == 1 ? 45 : 55.5,
                        right: barData.isonlyoneyear == 1 ? 80 - 45 : 80 - 55.5,
                        top: '55%',
                        bottom: '25%',
                        containLabel: false,
                        show: false
                    }],
                    dataZoom: [{
                        type: 'inside',
                        xAxisIndex: [0, 1],
                        startValue: barData.zoomindex[0],
                        endValue: barData.zoomindex[1],
                        zoomLock: true
                    }],
                    legend: {
                        data: barData.GetLegendData(),
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
                        selectedMode: false
                    },
                    tooltip: [{
                        trigger: 'axis',
                        triggerOn: 'click', //默认鼠标移动时触发，如果设置click则鼠标点击时触发
                        axisPointer: {            // 坐标轴指示器，坐标轴触发有效
                            type: 'line'        // 默认为直线，可选为：'line' | 'shadow'
                        },
                        //formatter: 折线（区域）图、柱状（条形）图、K线图 : {a}（系列名称），{b}（类目值），{c}（数值）, {d}（无）
                        backgroundColor: 'rgba(0, 0, 0, 0.7)', //提示框背景颜色
                        textStyle: {
                            color: 'rgba(255, 255, 255, 1)',
                            fontFamily: '微软雅黑',
                            fontWeight: 400,
                            fontSize: 12
                        },
                        //formatter: '{a0}{b}<br/>执行额：{c0}<br />{a2}: 环比上月+{c2}%'
                        formatter: function (params) {
                            //两个坐标轴，所以只取dataIndex
                            //console.log(params);
                            if (params.length == 2 && params[0] && params[1]) {
                                var dataIndex = params[0].dataIndex;
                                var pervalue = null;
                                var nowvalue = null;
                                var linevalue = barData.GetSeriesData(2)[dataIndex];
                                var rate = (linevalue >= 0 ? "+" : "") + linevalue.toPercent(1);
                                var nowdate = DateFormat.StringToDate(barData.xAxisdata[dataIndex]);
                                var perdate = DateFormat.AddYears(nowdate, -1);

                                if (barData.isonlyoneyear == 1) {
                                    pervalue = null;
                                    nowvalue = params[0].value;
                                }
                                else {
                                    pervalue = params[0].value;
                                    nowvalue = params[1].value;
                                }
                                var html = DateFormat.DateToString(nowdate, "yyyy年M月") + "<br/>"
                                        + "<span style='font-weight:normal;color:rgba(200, 200, 200, 1);'>执行额：</span>" + nowvalue + "亿元<br />"
                                        + "<span style='font-weight:normal;color:rgba(200, 200, 200, 1);'>增长率：</span>环比上月" + rate + "<br/>";
                                if (pervalue) {
                                    html += "<div style='clear:both;margin: 5px 0px;height:1px;background-color: rgba(255, 255, 255, 1);'></div>"
                                            + DateFormat.DateToString(perdate, "yyyy年M月") + "<br/>"
                                            + "<span style='font-weight:normal;color:rgba(200, 200, 200, 1);'>执行额：</span>" + pervalue + "亿元<br />";
                                }
                                return html;
                            }
                            else {
                                return "";
                            }
                        }
                    }, {
                        show: false,
                        showContent: false,
                        trigger: 'axis',
                        triggerOn: 'click'//默认鼠标移动时触发，如果设置click则鼠标点击时触发
                    }],
                    xAxis: [{
                        type: 'category',
                        gridIndex: 0,
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
                    }, {
                        type: 'category',
                        gridIndex: 1,
                        position: 'bottom',
                        boundaryGap: true,
                        data: barData.GetxAxisData(),
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
                        type: 'value',
                        gridIndex: 0,
                        name: '单位/亿',
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
                            formatter: '{value}',
                            textStyle: {
                                color: '#36415d',
                                fontFamily: '微软雅黑',
                                fontWeight: 400,
                                fontStyle: 'normal',
                                fontSize: 12
                            }
                        },
                        max: maxvalue,
                        interval: maxvalue / 4
                    }, {
                        type: 'value',
                        gridIndex: 1,
                        scale: true,
                        axisLine: {
                            show: false
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
                        name: barData.GetxSeriesnameData()[0],
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
                        name: barData.GetxSeriesnameData()[1],
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
                        name: '增长率',
                        type: 'line',
                        xAxisIndex: 1,
                        yAxisIndex: 1,
                        symbol: 'circle',
                        symbolSize: 6,
                        itemStyle: {
                            normal: {
                                color: barData.seriescolor[3]
                            }
                        },
                        lineStyle: {
                            normal: {
                                color: barData.seriescolor[3],
                                width: 2
                            }
                        },
                        data: barData.GetSeriesData(2)
                    }]
                };
                myChart2.hideLoading();
                myChart2.setOption(option);
            }
            else {
                myChart2.clear();
            }
        });
    };

    return {
        GetLeads_SeriesData: GetLeads_SeriesData,
        InitPieChart: InitPieChart,
        BindTitleChangeEvent: BindTitleChangeEvent
    };
})();
