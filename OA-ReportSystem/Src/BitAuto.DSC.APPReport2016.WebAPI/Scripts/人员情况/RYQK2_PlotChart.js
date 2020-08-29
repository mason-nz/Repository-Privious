/**
*===============================================================
*获取“人员情况（2）”图表的相关数据逻辑
*@version: 1.0
*@author:  masj
*@date:    2016-12-09
*@modifyuser:
*@modifydate:
*===============================================================
*/
var AppReport = AppReport || {};
AppReport.RYQK2 = (function () {
    //员工司龄，拼图中图例样式数据
    var EmployeeAgeStyleData = [{ id: 40001, name: '1年以下', itemStyle: { normal: { color: '#3E85FF'} }, icon: 'image:///Images/pt_yicheAPP.png' },
                            { id: 40002, name: '1-3年', itemStyle: { normal: { color: '#17EDE5'} }, icon: 'image:///Images/pt_yichePC.png' },
                            { id: 40003, name: '3-5年', itemStyle: { normal: { color: '#15B5E9'} }, icon: 'image:///Images/pt_baojiaAPP.png' },
                            { id: 40004, name: '5年以上', itemStyle: { normal: { color: '#FCE162'} }, icon: 'image:///Images/pt_yicheWAP.png'}];
    //员工职位，拼图中图例样式数据
    var EmployeePostTitleStyleData = [{ id: 60001, name: '管理类', itemStyle: { normal: { color: '#4750FC'}} },
                            { id: 60002, name: '销售客服类', itemStyle: { normal: { color: '#17EDE5'}} },
                            { id: 60003, name: '产品开发类', itemStyle: { normal: { color: '#FCE162'}} },
                            { id: 60004, name: '支持类', itemStyle: { normal: { color: '#F9944A'}} },
                            { id: 60005, name: '编辑类', itemStyle: { normal: { color: '#16A4D4'}} },
                            { id: 60006, name: '市场类', itemStyle: { normal: { color: '#3E85FF'}}}];
    //根据员工司龄ID，获取员工司龄名称
    var GetEmployeeAgeNameByID = function (id) {
        for (var i = 0; i < EmployeeAgeStyleData.length; i++) {
            if (EmployeeAgeStyleData[i].id == id) {
                return EmployeeAgeStyleData[i].name;
            }
        }
        return '';
    };

    //调用接口，获取饼图——系列数据（司龄）
    var GetEmployeeAge_SeriesData = function (callback) {
        var url = '/api/Empolyee/GetEmpolyeeForType';
        var dataParas = {
            ItemType: 4,
            r: Math.random()
        };
        AjaxGet(url, dataParas, null, function (data) {
            //console.log(data);
            var json = ProcessRequestJsonData(data);
            var chartSeriesData = [];
            for (var i = 0; i < EmployeeAgeStyleData.length; i++) {
                for (var j = 0; j < json.Data.length; j++) {
                    if (json.Data[j].ItemId == EmployeeAgeStyleData[i].id) {
                        var obj = {};
                        obj.value = json.Data[j].Count;
                        obj.name = EmployeeAgeStyleData[i].name;
                        obj.itemStyle = EmployeeAgeStyleData[i].itemStyle;
                        obj.id = EmployeeAgeStyleData[i].id;
                        chartSeriesData.push(obj);
                    }
                }
            }
            if (callback) {
                callback(chartSeriesData, json.Data);
            }
        });
    };

    //获取饼图——图例数据
    var GetLeads_LegendData = function (dataJson) {
        var chartLegendData = [];
        if (dataJson) {
            for (var i = 0; i < EmployeeAgeStyleData.length; i++) {
                for (var j = 0; j < dataJson.length; j++) {
                    if (dataJson[j].id == EmployeeAgeStyleData[i].id) {
                        var obj = {};
                        obj.name = EmployeeAgeStyleData[i].name;
                        obj.icon = EmployeeAgeStyleData[i].icon;
                        //obj.id = BusinessStyleData[i].id;
                        chartLegendData.push(obj);
                    }
                }
            }
        }
        return chartLegendData;
    };

    //初始化拼图（司龄）
    var InitEmployeeAgePieChart = function (dataJson) {
        //console.log(dataJson);
        defaultEChartsTextStyle.color = '#B1BDE5';
        myChartSL.hideLoading();
        // 指定图表的配置项和数据
        var option = {

            legend: {
                right: '5%',
                top: '23%',
                itemHeight: 11,
                itemWidth: 11,
                itemGap: 25,
                selectedMode: false,
                //backgroundColor: 'white',
                orient: 'vertical',
                textStyle: defaultEChartsTextStyle,
                data: GetLeads_LegendData(dataJson)
            },
            series: [
                {
                    radius: ['45%', '65%'],
                    center: ['40%', '50%'],
                    name: '司龄分布',
                    //selectedMode: 'multiple',
                    startAngle: 90,
                    minAngle: 20,
                    type: 'pie',
                    hoverAnimation: false,
                    //roseType:'radius',
                    data: dataJson,
                    label: {
                        normal: {
                            position: 'outside',
                            formatter: function (params) {
                                return params.value + '人\n(' + (params.percent).toFixed(1) + '%' + ')';
                            },
                            textStyle: {
                                fontFamily: 'Avenir-Medium',
                                color: '#B0BEE5',
                                fontSize: 12
                            }
                        }
                    }
                }]
        };
        //屏幕宽度小于iphone6的尺寸时(375px)
        if ($(window).width() < 375) {
            option.series[0].radius = ['25%', '45%'];
        }
        // 使用刚指定的配置项和数据显示图表。
        myChartSL.setOption(option);
    };

    //调用接口，获取饼图——系列数据（职位）
    var GetEmployeePostTitle_SeriesData = function (callback) {
        var url = '/api/Empolyee/GetEmpolyeeForType';
        var dataParas = {
            ItemType: 6,
            r: Math.random()
        };
        AjaxGet(url, dataParas, null, function (data) {
            //console.log(data);
            var json = ProcessRequestJsonData(data);
            var chartSeriesData = [];
            for (var i = 0; i < EmployeePostTitleStyleData.length; i++) {
                for (var j = 0; j < json.Data.length; j++) {
                    if (json.Data[j].ItemId == EmployeePostTitleStyleData[i].id) {
                        var obj = {};
                        obj.value = json.Data[j].Count;
                        obj.name = EmployeePostTitleStyleData[i].name;
                        obj.itemStyle = EmployeePostTitleStyleData[i].itemStyle;
                        obj.id = EmployeePostTitleStyleData[i].id;
                        chartSeriesData.push(obj);
                    }
                }
            }
            if (callback) {
                callback(chartSeriesData, json.Data);
            }
        });
    };

    //调用接口，获取人员男女数量以及比例
    var InitPersonNum = function () {
        AjaxGet("/api/Empolyee/GetEmpolyeeCount?r=" + Math.random(), null, null, function (data) {
            var jsonDataResult = ProcessRequestJsonData(data);

            $("#male_num").text(jsonDataResult.Data.male + "人");
            $("#male_per").text(Number(Math.abs(jsonDataResult.Data.malepercent)).toPercent(1));
            $("#female_num").text(jsonDataResult.Data.female + "人");
            $("#female_per").text(Number(Math.abs(jsonDataResult.Data.femalepercent)).toPercent(1));

            InitMp("#ul_mp", Number(Math.abs(jsonDataResult.Data.malepercent)) * 100);
            InitMp("#ul_mp2", Number(Math.abs(jsonDataResult.Data.femalepercent)) * 100);
        });
        //人员进度,（控件id，value：百分比）
        var InitMp = function (id, value) {
            var mpmax = 100; //最大百分比
            var mpmin = 0; //最小百分比
            var mpstep = 0; //单个图片所占百分比
            var imgwidth = 0; //图片尺寸
            var mpvalue = 0; //显示百分比

            if (value >= mpmax) {
                value = mpmax;
            }
            else if (value <= mpmin) {
                value = 0;
            }

            mpvalue = value;
            imgwidth = $(id + ".mp .mp_down").eq(0).width();

            var mplist = $(id + ".mp .mp_up");
            mplist.css("width", "0px");
            var length = mplist.length;
            mpstep = mpmax / length;

            //动画-图片位置
            if ($(id).width() > 200) {
                var _width = ($(id).width() - 150) / 5;
                $(id + ".mp li").animate({ marginRight: "5%" });
            }
            //
            _MpMove(mplist, 0);

            //进度
            function _MpMove(list, index) {
                if (mpvalue <= 0) {
                    return;
                }
                var wz = 0;
                if (mpvalue >= mpstep) {
                    mpvalue = mpvalue - mpstep;
                    wz = imgwidth;
                }
                else {
                    wz = (mpvalue / mpstep) * imgwidth;
                    mpvalue = 0;
                }

                if (index < list.length) {

                    $(list[index]).animate({ width: wz + "px" }, "fast", "linear", function () {
                        _MpMove(list, index + 1);
                    });
                }
            }
        }
    }

    //初始化拼图（职位）
    var InitEmployeePostTitlePieChart = function (dataJson) {
        myChartZW.hideLoading();
        // 指定图表的配置项和数据
        var option = {
            legend: {
                show: false
            },
            series: [
            {
                name: '虚线环',
                type: 'pie',
                selectedOffset: 0,
                hoverAnimation: false,
                silent: true,
                z: 3,
                radius: ['65%', '65%'],
                center: ['50%', '50%'],
                label: {
                    show: false
                },
                labelLine: {
                    normal: {
                        show: false
                    }
                },
                itemStyle: {
                    normal: {
                        borderColor: '#B0BEE5',
                        borderType: 'dashed',
                        borderWidth: 1
                    }
                },
                data: [{ value: 0, name: '', itemStyle: {
                    normal: {
                        color: '#B0BEE5',
                        borderType: 'dashed',
                        borderColor: '#B0BEE5',
                        borderWidth: 1
                    }
                }
                }]
            }, {
                radius: ['35%', '55%'],
                center: ['50%', '50%'],
                name: '职位统计',
                //selectedMode: 'multiple',
                startAngle: 90,
                minAngle: 20,
                type: 'pie',
                hoverAnimation: false,
                animationDuration: 1500,
                //roseType:'radius',

                data: dataJson,
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
                        formatter: function (params) {
                            return params.name + '\n' + params.value + '(' + (params.percent).toFixed(1) + '%)';
                        },
                        //lineStyle: {
                        //    color: '#B1BDE5'
                        //},
                        textStyle: {
                            color: '#B0BEE5',
                            fontFamily: 'Avenir-Medium',
                            fontSize: 12
                        }
                    }
                },
                labelLine: {
                    normal: {
                        show: true//,
                        //length: 16,
                        //length2: 85,
                        //lineStyle: {
                        //    color: 'white',
                        //    type: 'dashed'
                        //}
                    }
                }
            }]
        };
        // 使用刚指定的配置项和数据显示图表。
        myChartZW.setOption(option);
    };

    //初始化柱图-年龄
    var InitChartNL = function () {
        myChartNL.showLoading(defaultEChartsLoadingStyle);
        AjaxGet("/api/Empolyee/GetEmpolyeeForType?ItemType=" + 3 + "&r=" + Math.random(),
        null, null, function (data) {
            //console.log(data);
            if (data.Status == 0) {
                var jsondata = data.Result.Data;
                //获取服务器数据
                var datakey = jsondata.datakey;
                var data1 = jsondata.dataval[0];
                var data2 = jsondata.dataval[1];
                var color = ['#16a4d4', '#18ece5', '#3f85ff', '#fde162', '#fa944a'];
                //构造数据
                var datavalue = new Array();
                for (var i = 0; i < data1.length; i++) {
                    var item = { value: data1[i] };
                    item.itemStyle = {};
                    item.itemStyle.normal = {};
                    item.itemStyle.normal.color = color[i];
                    datavalue.push(item);
                }
                //画图
                var option = {
                    tooltip: {
                        show: false
                    },
                    legend: {
                        show: false
                    },
                    dataZoom: [{
                        type: 'inside',
                        start: 0,
                        end: 100,
                        zoomLock: true
                    }],
                    grid: {
                        left: 0,
                        right: 0,
                        top: 40,
                        bottom: 40,
                        containLabel: false,
                        show: false
                    },
                    xAxis: {
                        type: 'category',
                        boundaryGap: true,
                        data: datakey,
                        axisLine: {
                            show: true,
                            lineStyle: {
                                color: '#36415d'
                            }
                        },
                        axisTick: {
                            show: false
                        },
                        splitLine: {
                            show: false
                        },
                        axisLabel: {
                            show: true,
                            interval: 0,
                            textStyle: {
                                color: '#36415d',
                                fontFamily: '微软雅黑',
                                fontWeight: 400,
                                fontStyle: 'normal',
                                fontSize: 12
                            }
                        }
                    },
                    yAxis: {
                        type: 'value',
                        axisLine: {
                            show: false
                        },
                        splitLine: {
                            show: false
                        },
                        axisLabel: {
                            show: false
                        },
                        axisTick: {
                            show: false
                        }
                    },
                    series: [{
                        name: '年龄分布',
                        type: 'bar',
                        data: datavalue,
                        label: {
                            normal: {
                                show: true,
                                position: 'top',
                                formatter: function (params) {
                                    //console.log(params);
                                    var index = params.dataIndex;
                                    var value = data1[index];
                                    var rate = Number(data2[index]).toPercent(1);

                                    var html = "" + value + "人\n\n" + rate;
                                    return html;
                                },
                                textStyle: {
                                    color: '#fff',
                                    fontFamily: '微软雅黑',
                                    fontWeight: 400,
                                    fontStyle: 'normal',
                                    fontSize: 12
                                }
                            }
                        }
                    }]
                };
                myChartNL.hideLoading();
                // 使用刚指定的配置项和数据显示图表。
                myChartNL.setOption(option);
            }
            else {
                myChartNL.clear();
            }
        });
    }

    //初始化柱图-职级
    var InitChartZJ = function () {
        myChartZJ.showLoading(defaultEChartsLoadingStyle);
        AjaxGet("/api/Empolyee/GetEmpolyeeForType?ItemType=" + 5 + "&r=" + Math.random(),
        null, null, function (data) {
            //console.log(data);
            if (data.Status == 0) {
                var jsondata = data.Result.Data;
                //获取数据
                var datakey = jsondata.datakey;
                var data1 = jsondata.dataval[0];
                var data2 = jsondata.dataval[1];
                var data3 = new Array();

                //计算总数
                var data3item = 0;
                for (var i = 0; i < data1.length; i++) {
                    if (!isNaN(parseInt(data1[i]))) {
                        data3item += parseInt(data1[i]);
                    }
                    else {
                        data1[i] = 0;
                    }
                }
                for (var i = 0; i < data1.length; i++) {
                    data3.push(data3item);
                }
                //画图
                var option = {
                    tooltip: {
                        show: false
                    },
                    legend: {
                        show: false
                    },
                    dataZoom: [{
                        type: 'inside',
                        start: 0,
                        end: 100,
                        zoomLock: true
                    }],
                    grid: {
                        left: 24,
                        right: 48,
                        top: 24,
                        bottom: 0,
                        containLabel: true
                    },
                    yAxis: [{
                        type: 'category',
                        boundaryGap: true, //留白策略
                        data: datakey,
                        inverse: true,
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
                        boundaryGap: true, //留白策略
                        data: datakey,
                        inverse: true,
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
                    xAxis: {
                        type: 'value',
                        max: data3item,
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
                    },
                    series: [{
                        name: '职级分布',
                        z: 2,
                        yAxisIndex: 0,
                        type: 'bar',
                        data: data1,
                        barWidth: 10,
                        itemStyle: {
                            normal: {
                                color: '#18ece5',
                                barBorderRadius: 5
                            }
                        },
                        label: {
                            normal: {
                                show: true,
                                position: ['105%', -3],
                                formatter: function (params) {
                                    //console.log(params);
                                    var index = params.dataIndex;
                                    var value = data1[index];
                                    var rate = Number(data2[index]).toPercent(1);
                                    return rate;
                                },
                                textStyle: {
                                    color: '#ffffff',
                                    fontFamily: '微软雅黑',
                                    fontWeight: 400,
                                    fontStyle: 'normal',
                                    fontSize: 12
                                }
                            }
                        }
                    }, {
                        name: '补全',
                        z: 1,
                        yAxisIndex: 1,
                        type: 'bar',
                        data: data3,
                        barWidth: 10,
                        itemStyle: {
                            normal: {
                                color: '#212540',
                                barBorderRadius: 5
                            }
                        },
                        label: {
                            normal: {
                                show: true,
                                position: ['101%', -3],
                                formatter: function (params) {
                                    //console.log(params);
                                    var index = params.dataIndex;
                                    var value = data1[index];
                                    var rate = Number(data2[index]).toPercent();
                                    return value + "人";
                                },
                                textStyle: {
                                    color: '#7881ba',
                                    fontFamily: '微软雅黑',
                                    fontWeight: 400,
                                    fontStyle: 'normal',
                                    fontSize: 12
                                }
                            }
                        },
                        silent: true
                    }]
                };
                myChartZJ.hideLoading();
                // 使用刚指定的配置项和数据显示图表。
                myChartZJ.setOption(option);
            }
            else {
                myChartZJ.clear();
            }
        });
    }

    return {
        GetEmployeeAge_SeriesData: GetEmployeeAge_SeriesData,
        InitEmployeeAgePieChart: InitEmployeeAgePieChart,
        GetEmployeePostTitle_SeriesData: GetEmployeePostTitle_SeriesData,
        InitEmployeePostTitlePieChart: InitEmployeePostTitlePieChart,
        InitChartNL: InitChartNL,
        InitChartZJ: InitChartZJ,
        InitPersonNum: InitPersonNum
    };
})();
