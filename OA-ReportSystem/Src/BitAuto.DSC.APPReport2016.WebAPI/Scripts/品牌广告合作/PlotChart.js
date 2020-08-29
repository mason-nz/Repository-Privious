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
//设置柱状图事件
function BarChartEvent() {
    //设置事件           
    var downposition = { clientX: 0, clientY: 0 };
    $("#ChartBar").mousedown(function (params) {
        downposition.clientX = params.clientX;
        downposition.clientY = params.clientY;
    });
    $("#ChartBar").mouseup(function (params) {
        var cha_X = params.clientX - downposition.clientX;
        if (cha_X > 50) {
            alert("向右");
        }
        else if (cha_X < -50) {
            alert("向左");
        }
    });

    $("#ChartBar")[0].addEventListener('touchstart', function (params) {
        downposition.clientX = params.touches[0].pageX;
        downposition.clientY = params.touches[0].pageY;
    }, false);
    $("#ChartBar")[0].addEventListener('touchmove', function (params) {
        var cha_X = params.touches[0].pageX - downposition.clientX;
        if (cha_X > 50) {
            alert("向右");
        }
        else if (cha_X < -50) {
            alert("向左");
        }
    }, false);
}
//设置表格数据
function TableEvent() {
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
}
//设置排序样式
function SetTdStyle(clicktd, othertd) {
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
}
//设置数据等待样式
function LoadingStyle(isshow) {
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
}

//---------------------------------------------------api------------------------------------------------//
//设置品牌数和品牌额
function QueryPinPai() {
    //异步api获取数据-获取文字中的数据
    AjaxGet("/api/ppgghz/GetBrandOverViewData?r=" + Math.random(), null, null, function (data) {
        //console.log(data);
        if (data.Status == 0) {
            var summaryInfo = data.Result.summaryInfo;
            var rankInfo = data.Result.rankInfo;
            if (summaryInfo) {
                $("#span_year").text(summaryInfo.Year);
                $("#pinpaihezuoshu").MagicNumber(summaryInfo.Count);
                $("#pinpaihezuojine").MagicFloat(summaryInfo.Amount.toMoney("亿"), 2, "");
            }
            if (rankInfo) {
                $("#top10jine").MagicNumber(rankInfo.Top10Amount.toMoney("万", 0));
                $("#top10zhanbi").MagicFloat(rankInfo.Top10Percent * 100, 1, "%");
                $("#top40zhanbi").MagicFloat(rankInfo.Top40Percent * 100, 1, "%");
            }
        }
        else {
        }
    });
}
//设置图表
function QuerymyChart() {
    myChart.showLoading(defaultEChartsLoadingStyle);
    //异步api获取数据-获取柱图中的数据
    AjaxGet("/api/ppgghz/GetCooperateBarChartData?r=" + Math.random(), null, null, function (data) {
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
                    containLabel: false,
                    show: false
                }, {
                    left: 55.5,
                    right: 80 - 55.5,
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
                            var pervalue = params[0].value;
                            var nowvalue = params[1].value;
                            var linevalue = barData.GetSeriesData(2)[dataIndex];
                            var rate = (linevalue >= 0 ? "+" : "") + linevalue.toPercent(1);
                            var nowdate = DateFormat.StringToDate(barData.xAxisdata[dataIndex]);
                            var perdate = DateFormat.AddYears(nowdate, -1);

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
                    data: barData.GetSeriesData(0),
                    silent: false
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
                    data: barData.GetSeriesData(1),
                    silent: false
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
                    data: barData.GetSeriesData(2),
                    silent: false
                }]
            };
            myChart.hideLoading();
            myChart.setOption(option);
        }
        else {
            myChart.clear();
        }
    });
}
//查询数据
function QueryTable(append) {
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
        orderBy = "Amount " + no_sort;
    }
    else if (rate_sort != "none") {
        orderBy = "YearBasis " + rate_sort;
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

    //异步api获取数据-获取表格中的数据
    AjaxGet("/api/ppgghz/GetCooperateBrandRanklist?orderBy=" + orderBy + "&pageIndex=" + pageIndex + "&pageSize=" + pageSize + "&lastno=" + lastno + "&r=" + Math.random(),
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
                            tr += "<td name='lie1'  class='left first'>" + item.No + "</td>";
                            tr += "<td name='lie2' class='left b'>" + item.Name + "</td>";
                            tr += "<td name='lie3'  class='b'>" + Number(Number(item.Amount).toMoney("万", 0)).addComma() + "</td>";
                            tr += "<td name='lie4' class='b'>" + Number(item.Percent).toPercent(1) + "</td>";
                            tr += "<td name='lie5'  class='right "
                                + (!isNaN(parseFloat(item.YearBasis)) ? (Number(item.YearBasis) >= 0 ? "tdhighColor" : "tdlowColor") : "")
                                + " b'>"
                                + (!isNaN(parseFloat(item.YearBasis)) ? ((Number(item.YearBasis) >= 0 ? "+" : "") + Number(item.YearBasis).toPercent(1)) : "-")
                                + "</td>";
                            tr += "</tr>";

                            $("#pingpaiTable tbody").append(tr);
                        }
                        else {
                            //该行存在，修改
                            $("#pingpaiTable tbody tr[data-no=" + item.RowNumber + "]").removeAttr("del");
                            $("#pingpaiTable tbody tr[data-no=" + item.RowNumber + "] td[name='lie1']").html(item.No);
                            $("#pingpaiTable tbody tr[data-no=" + item.RowNumber + "] td[name='lie2']").html(item.Name);
                            $("#pingpaiTable tbody tr[data-no=" + item.RowNumber + "] td[name='lie3']").html(Number(Number(item.Amount).toMoney("万", 0)).addComma());
                            $("#pingpaiTable tbody tr[data-no=" + item.RowNumber + "] td[name='lie4']").html(Number(item.Percent).toPercent(1));
                            $("#pingpaiTable tbody tr[data-no=" + item.RowNumber + "] td[name='lie5']").attr("class", "right "
                                + (!isNaN(parseFloat(item.YearBasis)) ? (Number(item.YearBasis) >= 0 ? "tdhighColor" : "tdlowColor") : "")
                                + " b").html(
                                   (!isNaN(parseFloat(item.YearBasis)) ? ((Number(item.YearBasis) >= 0 ? "+" : "") + Number(item.YearBasis).toPercent(1)) : "-")
                                );
                        }
                        isend = item.RowNumber == data.Result.totalCount;
                    }
                    //删除之前的数据
                    $("tr[data-no][del]").remove();
                }
                else {
                }
                LoadingStyle(isend ? null : false);
                myChart.resize();
            });
}
//---------------------------------------------------api------------------------------------------------//


function InitPage() {
    //设置品牌数和品牌额
    //设置排名信息
    QueryPinPai();
    //设置柱状图
    //BarChartEvent();
    QuerymyChart();
    //设置数据列表
    TableEvent();
    QueryTable(false);
}