/**
*===============================================================
*获取“会议覆盖率”图表的相关数据逻辑
*@version: 1.0
*@author:  强斐
*@date: 2016-12-8
*@modifyuser:
*@modifydate:
*===============================================================
*/
var AppReport = AppReport || {};
AppReport.HYFGL = (function () {
    //会员类型，仪表盘中图例样式数据
    var MemberTypeStyleData = [{ id: 90001, name: '车易通', itemStyle: { normal: { color: '#3F85FF'}} },
                            { id: 90002, name: '车盟通', itemStyle: { normal: { color: '#18EDE5'}} },
                            { id: 90003, name: '微信通', itemStyle: { normal: { color: '#FDE162'}}}];

    //根据员工司龄ID，获取员工司龄名称
    var GetMemberTypeStyleData = function (id) {
        for (var i = 0; i < MemberTypeStyleData.length; i++) {
            if (MemberTypeStyleData[i].id == id) {
                return MemberTypeStyleData[i];
            }
        }
        return null;
    };

    //设置表格数据
    var TableEvent = function () {
        //设置排序事件
        $("#orderno").click(function () {
            SetTdStyle($("#orderno"), $("#orderrate"));
            QueryTable(false);
        });
        $("#orderrate").click(function () {
            SetTdStyle($("#orderrate"), $("#orderno"));
            QueryTable(false);
        });
        //追加事件
        $("#append").click(function () {
            QueryTable(true);
        });
    };
    //设置排序样式
    var SetTdStyle = function (clicktd, othertd) {
        //切换当前字段的排序
        var nowsort = clicktd.attr("data-sort");
        clicktd.find("img").css("display", "");
        //默认：desc
        if (nowsort != "desc") {
            clicktd.attr("data-sort", "desc");
            clicktd.find("img").attr("src", "/Images/table_order_desc.png");
        }
        else {
            clicktd.attr("data-sort", "asc");
            clicktd.find("img").attr("src", "/Images/table_order_asc.png");
        }
        //情况其他字段的排序
        othertd.attr("data-sort", "none");
        othertd.find("img").attr("src", "/Images/table_order_none.png");
    };
    //设置数据等待样式
    var LoadingStyle = function (isshow) {
        var span = $("#append").find("span");
        var img = $("#append").find("img");

        if (isshow == true) {
            span.text("正在加载中...");
            img.css("display", "none");
            $("#append").css("display", "");
        }
        else if (isshow == false) {
            span.text("展开");
            img.css("display", "");
            $("#append").css("display", "");
        }
        else if (isshow == null) {
            //没有下一页数据了，隐藏
            $("#append").css("display", "none");
        }
    };
    //查询数据
    var QueryTable = function (append) {
        LoadingStyle(true);
        //添加数据

        if (append == false) {
            //标记-删除现有数据
            $("tr[data-no]").attr("del", "true");
        }

        //获取排序字段
        var no_sort = $("#orderno").attr("data-sort");
        var rate_sort = $("#orderrate").attr("data-sort");
        var orderBy = "";
        if (no_sort != "none") {
            orderBy = "fugailv " + no_sort;
        }
        else if (rate_sort != "none") {
            orderBy = "MonthBasis " + rate_sort;
        }
        //获取当前页码
        var pageIndex = $("#hid_pageIndex").val();
        var lastno = $("#hid_lastno").val();

        if (append == false) {
            //不追加数据，从第一页取
            pageIndex = 1;
            lastno = 0;
        }
        else {
            //追加数据，取下一页
            pageIndex = parseInt(pageIndex) + 1;
        }
        //获取分页大小
        var pageSize = $("#hid_pageSize").val();
        var yearmonth = $("#hid_yearmonth").val();
        var typeid = $("#hid_typeid").val();

        //异步api获取数据-获取表格中的数据
        AjaxGet("/api/xchyhz/GetMemmberDepartFGL?itemId=" + typeid + "&yearMonth=" + yearmonth
        + "&orderBy=" + orderBy + "&pageIndex=" + pageIndex + "&pageSize=" + pageSize + "&lastno=" + lastno + "&r=" + Math.random(),
            null, null, function (data) {
                //console.log(data);
                if (data.Status == 0) {
                    //存储当前页码
                    $("#hid_pageIndex").val(data.Result.pageIndex);
                    $("#hid_lastno").val(data.Result.lastno);
                    //数据是否结束
                    var isend = false;
                    //追加数据
                    for (var i = 0; i < data.Result.data.length; i++) {
                        var item = data.Result.data[i];
                        if ($("#pingpaiTable tbody tr[data-no=" + item.RowNumber + "]").length == 0) { //该行不存在，新增
                            var tr = "<tr data-no='" + item.RowNumber + "'>";
                            tr += "<td name='lie1' class='left first'>" + item.No + "</td>";
                            tr += "<td name='lie2' class='left b'>" + item.DepartName + "</td>";
                            tr += "<td name='lie3' class='b'>" + item.Count + "/" + item.Total + "</td>";
                            tr += "<td name='lie4' class='b'>" + (item.fugailv ? (Number(item.fugailv).toPercent(1)) : "-") + "</td>";
                            tr += "<td name='lie5' class='right "
                               + (!isNaN(parseFloat(item.MonthBasis)) ? (Number(item.MonthBasis) >= 0 ? "tdhighColor" : "tdlowColor") : "")
                               + " b'>"
                               + (!isNaN(parseFloat(item.MonthBasis)) ? ((Number(item.MonthBasis) >= 0 ? "+" : "") + Number(item.MonthBasis).toPercent(1)) : "-")
                               + "</td>";
                            tr += "</tr>";
                            $("#pingpaiTable tbody").append(tr);
                        }
                        else {
                            //该行存在，修改
                            $("#pingpaiTable tbody tr[data-no=" + item.RowNumber + "]").removeAttr("del");
                            $("#pingpaiTable tbody tr[data-no=" + item.RowNumber + "] td[name='lie1']").html(item.No);
                            $("#pingpaiTable tbody tr[data-no=" + item.RowNumber + "] td[name='lie2']").html(item.DepartName);
                            $("#pingpaiTable tbody tr[data-no=" + item.RowNumber + "] td[name='lie3']").html(item.Count + "/" + item.Total);
                            $("#pingpaiTable tbody tr[data-no=" + item.RowNumber + "] td[name='lie4']").html((item.fugailv ? (Number(item.fugailv).toPercent(1)) : "-"));
                            $("#pingpaiTable tbody tr[data-no=" + item.RowNumber + "] td[name='lie5']").attr("class", "right "
                                + (!isNaN(parseFloat(item.MonthBasis)) ? (Number(item.MonthBasis) >= 0 ? "tdhighColor" : "tdlowColor") : "")
                                + " b").html(
                                   (!isNaN(parseFloat(item.MonthBasis)) ? ((Number(item.MonthBasis) >= 0 ? "+" : "") + Number(item.MonthBasis).toPercent(1)) : "-")
                                );
                        }
                        isend = item.RowNumber == data.Result.totalCount;
                    }

                    //删除之前的数据
                    $("#pingpaiTable tbody tr[data-no][del]").remove();
                }
                else {
                }
                LoadingStyle(isend ? null : false);
                //myChartGauge.resize();
            });
    };
    //表格初始化
    var InitGrid = function (yearmonth, typeid, name) {
        $("#gridtitle").text(name);
        $("#hid_yearmonth").val(yearmonth);
        $("#hid_typeid").val(typeid);
        //设置数据列表
        TableEvent();
        QueryTable(false);
    }

    //调用接口，获取仪表盘——系列数据（会员覆盖率）
    var GetMemberFGL_SeriesData = function (callback) {
        var url = '/api/xchyhz/GetMemberFGL';
        $.get(url, null, function (data) {
            //console.log(data);
            var json = ProcessRequestJsonData(data);
            var chartSeriesData = [];
            for (var i = 0; i < MemberTypeStyleData.length; i++) {
                for (var j = 0; j < json.Data.length; j++) {
                    if (json.Data[j].TypeID == MemberTypeStyleData[i].id) {
                        var obj = {};
                        obj.value = parseInt(json.Data[j].Count); //当前值
                        //obj.label = MemberTypeStyleData[i].name + '\n' + parseInt(json.Data[j].Count).addComma() + '/' + parseInt(json.Data[j].Total).addComma();
                        obj.name = MemberTypeStyleData[i].name;
                        obj.id = MemberTypeStyleData[i].id;
                        obj.total = parseInt(json.Data[j].Total); //总量
                        obj.remain = obj.total - obj.value; //剩余量
                        obj.color = MemberTypeStyleData[i].itemStyle.normal.color; //当前选项颜色值
                        chartSeriesData.push(obj);
                    }
                }
            }
            if (callback) {
                callback(chartSeriesData, json.Data);
            }
        }, 'json');
    };

    //初始化仪表盘
    var InitGaugeChart = function (dataJson) {
        //console.log(dataJson);
        //defaultEChartsTextStyle.color = '#B1BDE5';
        myChartGauge.hideLoading();
        // 指定图表的配置项和数据
        var option = {
            tooltip: {
                show: false
            },
            legend: {
                show: false
            },
            label: {
                normal: {
                    show: false
                }
            },
            series: [
        {
            name: '车易通',
            type: 'pie',
            z: 2,
            hoverAnimation: false,
            radius: ['50%', '55%'],
            center: ['50%', '50%'],    // 默认全局居中
            label: {
                normal: {
                    show: false
                }
            },
            data: [
            {
                //value: 19829, name: '车易通', id: 90001, itemStyle: { normal: { color: '#306BFE'} },
                label: {
                    normal: {
                        show: true,
                        position: 'center',
                        //                        formatter: function (params) {
                        //                            return params.percent.toPercent(1) + '\n' + params.value + '/' + '1234567' + '\n' + params.name;
                        //                        },
                        textStyle: {
                            color: 'white',
                            fontFamily: 'GothamBook',
                            fontSize: 13
                        }
                    }
                }
            },
                {
                    //value: 8491, name: '车易通', id: 90001, itemStyle: { normal: { color: '#20233C'} },
                    label: {
                        normal: {
                            show: false
                        },
                        emphasis: {
                            show: false
                        }
                    }
                }


            ]
        },
        {
            name: '车盟通',
            type: 'pie',
            z: 1,
            hoverAnimation: false,
            radius: ['40%', '45%'],
            center: ['15%', '50%'],    // 默认全局居中
            label: {
                normal: {
                    show: false
                }
            },
            data: [
            {
                //value: 19829, name: '车盟通', id: 90002, itemStyle: { normal: { color: '#18EDE5'} },
                label: {
                    normal: {
                        show: true,
                        position: 'center',
                        //                        formatter: function (params) {
                        //                            return params.percent.toPercent(1) + '\n' + params.value + '/' + '1234567' + '\n' + params.name;
                        //                        },
                        textStyle: {
                            color: 'white',
                            fontFamily: 'GothamBook',
                            fontSize: 13
                        }
                    }
                }
            },
                {
                    //value: 8491, name: '车盟通', id: 90002, itemStyle: { normal: { color: '#20233C'} },
                    label: {
                        normal: {
                            show: false
                        },
                        emphasis: {
                            show: false
                        }
                    }
                }


            ]
        },
        {
            name: '微信通',
            type: 'pie',
            z: 3,
            hoverAnimation: false,
            radius: ['40%', '45%'],
            center: ['85%', '50%'],    // 默认全局居中
            label: {
                normal: {
                    show: false
                }
            },
            data: [
            {
                //value: 19829, name: '微信通', id: 90003, itemStyle: { normal: { color: '#FDE162'} },
                label: {
                    normal: {
                        show: true,
                        position: 'center',
                        //                        formatter: function (params) {
                        //                            return params.percent.toPercent(1) + '\n' + params.value + '/' + '1234567' + '\n' + params.name;
                        //                        },
                        textStyle: {
                            color: 'white',
                            fontFamily: 'GothamBook',
                            fontSize: 13
                        }
                    }
                }
            },
                {
                    //value: 8491, name: '微信通', id: 90003, itemStyle: { normal: { color: '#20233C'} },
                    label: {
                        normal: {
                            show: false
                        },
                        emphasis: {
                            show: false
                        }
                    }
                }


            ]
        }
    ]
        };
        //根据接口返回的数据，绑定图表
        var option = BindGaugeChart(option, dataJson);

        // 使用刚指定的配置项和数据显示图表。
        myChartGauge.setOption(option);

        $(window).resize(function () {
            myChartGauge.resize();
        });

        //注册事件
        myChartGauge.off('click');
        myChartGauge.on('click', function (params) {
            //console.log(params.data);
            var option = this.getOption();
            //option.animation = true;
            var seriesIndex = params.seriesIndex;
            var z = option.series[seriesIndex].z;
            //console.log(z);
            while (option.series[seriesIndex].z != 2) {
                for (var i = 0; i < option.series.length; i++) {
                    var z = option.series[i].z;
                    z = (z + 1) % 3;
                    if (z == 0) {
                        z = 3;
                    }
                    option.series[i].z = z;
                    //option.series[i].animation = true;
                }
            }
            //根据接口返回的数据，绑定图表
            var option = BindGaugeChart(option, dataJson);
            // 使用刚指定的配置项和数据显示图表。
            myChartGauge.setOption(option);

            //console.log(option.series[seriesIndex].name);
            //console.log(option.series[seriesIndex].data[0].id);
            //设置数据列表
            InitGrid("", option.series[seriesIndex].data[0].id, option.series[seriesIndex].name);
        });
    };

    // 使用刚指定的配置项和数据显示图表。
    var BindGaugeChart = function (option, dataJson) {
        //var option = myChart.getOption();
        if (dataJson && option) {
            for (var i = 0; i < dataJson.length; i++) {
                var id = dataJson[i].id; //会员类型ID
                var name = dataJson[i].name; //会员类型名称
                //var percent = (parseFloat(dataJson[i].value) * 100).toFixed(1); //占比
                //var label = dataJson[i].label; //提示文字
                //var currentDataEnum = GetMemberTypeStyleData(id);
                var val = dataJson[i].value;
                var total = dataJson[i].total;
                var remain = dataJson[i].remain;
                var color = dataJson[i].color;

                //公共配置
                if (name && name != null) {
                    //                    option.series[i].axisLine.lineStyle.color = [[parseFloat(dataJson[i].value), currentDataEnum.itemStyle.normal.color], [1, '#37405D']];
                    //                    option.series[i].itemStyle = currentDataEnum.itemStyle;
                    //                    option.series[i].data = [{ value: percent, name: label, id: id}];
                    //                    option.series[i].detail.textStyle.color = currentDataEnum.itemStyle.normal.color;
                    option.series[i].data = [
                                                {
                                                    value: val, name: name, id: id, itemStyle: { normal: { color: color} },
                                                    label: {
                                                        normal: {
                                                            show: true,
                                                            position: 'center',
                                                            formatter: function (params) {
                                                                return (params.percent / 100).toPercent(1) + '\n' + params.value + '/' + total + '\n' + params.name;
                                                            },
                                                            textStyle: {
                                                                color: 'white',
                                                                fontFamily: 'GothamBook',
                                                                fontSize: 13
                                                            }
                                                        }
                                                    }
                                                },
                                                {
                                                    value: remain, name: name, itemStyle: { normal: { color: '#20233C'} },
                                                    label: {
                                                        normal: {
                                                            show: false
                                                        },
                                                        emphasis: {
                                                            show: false
                                                        }
                                                    }
                                                }
                    ];
                }

                switch (option.series[i].z) {
                    case 2: //中间样式
                        option.series[i].radius = ['60%', '65%'];
                        option.series[i].center = ['50%', '50%'];
                        //option.series[i].pointer = { length: '70%', width: 4 };
                        //option.series[i].title.textStyle.fontSize = 15;
                        //option.series[i].title.textStyle.color = '#FFFFFF';
                        //option.series[i].detail.textStyle.fontSize = 21;
                        break;
                    case 1: //两边样式（左）
                        option.series[i].radius = ['50%', '55%'];
                        option.series[i].center = ['15%', '50%'];
                        option.series[i].data[0].label.normal.textStyle.fontSize = 12;
                        //                        option.series[i].pointer = { length: '60%', width: 3 };
                        //                        option.series[i].title.textStyle.fontSize = 12;
                        //                        option.series[i].title.textStyle.color = '#B0BEE5';
                        //                        option.series[i].detail.textStyle.fontSize = 13;
                        break;
                    case 3: //两边样式（右）
                        option.series[i].radius = ['50%', '55%'];
                        option.series[i].center = ['85%', '50%'];
                        option.series[i].data[0].label.normal.textStyle.fontSize = 12;
                        //                        option.series[i].pointer = { length: '60%', width: 3 };
                        //                        option.series[i].title.textStyle.fontSize = 12;
                        //                        option.series[i].title.textStyle.color = '#B0BEE5';
                        //                        option.series[i].detail.textStyle.fontSize = 13;
                        break;
                    default:
                }

            }
        }
        return option;
    };

    return {
        InitGrid: InitGrid,
        GetMemberFGL_SeriesData: GetMemberFGL_SeriesData,
        InitGaugeChart: InitGaugeChart,
        BindGaugeChart: BindGaugeChart
    };
})();
