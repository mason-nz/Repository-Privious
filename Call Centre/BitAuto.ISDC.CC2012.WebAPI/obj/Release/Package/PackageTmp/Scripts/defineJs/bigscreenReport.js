var BigChartOption = {};
var chartBackgroundColor = 'rgba(0,0,0,0)';
var xAxisStyle = {
    fontWeight: "bolder",
    fontFamily: 'Arial',
    fontSize: '18px',
    color: '#333333'
};
var yAxisStyle = {
    fontFamily: 'Arial',
    fontSize: '20px',
    color: '#333333'
};
var plotOptionsStyle = {
    fontWeight: "bolder",
    fontFamily: 'Arial',
    fontSize: '30px',
    color: '#333333'
};
var plotOptionsBarStyle = {
    fontWeight: "bolder",
    fontFamily: 'Arial',
    fontSize: '24px',
    color: '#333333'
};
var plotOptionsSeries = {
    borderWidth: 0,
    borderRadius: 8
};
var legendStyle = {
    fontWeight: "bolder",
    fontFamily: '微软雅黑',
    fontSize: '14px',
    color: '#000000'
};
var xAxisLineStyle = {
    fontWeight: "bolder",
    fontFamily: 'Arial',
    fontSize: '14px',
    color: '#333333'
};

//创建一个公共的图配置
BigChartOption.CreateColumnChartOption = function () {
    var chartOption = {
        chart: {
            type: "column",
            backgroundColor: chartBackgroundColor,
            marginTop: -18
        },
        title: {
            text: ''
        },
        xAxis: {
            tickLength: 0,
            labels: {
                style: xAxisStyle,
                y: 40
            }
        },
        yAxis: {
            min: 0,
            title: {
                text: ''
            },
            labels: {
                style: yAxisStyle
            }
        },
        plotOptions: {
            column: {
                dataLabels: {
                    enabled: true,
                    style: plotOptionsStyle,
                    format: '{point.name}'
                }
            },
            series: plotOptionsSeries
        },
        legend: {
            enabled: false
        },
        credits: {
            enabled: false
        }
    };
    return chartOption;
}
BigChartOption.CreateLineChartOption = function () {
    return {
        chart: {
            type: "area",
            backgroundColor: chartBackgroundColor,
            marginTop: 40
        },
        legend: {
            borderWidth: 0,
            symbolRadius: 4,
            itemStyle: legendStyle
        },
        plotOptions: {
            column: {
                dataLabels: {
                    enabled: false
                }
            },
            area: {
                fillOpacity: 0.2
            }
        },
        title: { text: "", x: 0 },
        xAxis: {
            labels: {
                style: xAxisLineStyle,
                y: 30
            },
            tickmarkPlacement: 'on',
            max: 16,
            min: 0,
            categories: ["9:30", "10:00", "10:30", "11:00", "11:30", "12:00", "12:30", "13:00", "13:30", "14:00", "14:30", "15:00", "15:30", "16:00", "16:30", "17:00", "17:30", "18:00"]
        },
        yAxis: {
            title: { text: "" },
            maxPadding: 0.1,
            minPadding: 0.1,
            floor: 0,
            labels: {
                style: yAxisStyle,
                format: "{value}"
            }
        },
        series: {
            borderWidth: 0
        },
        credits: { enabled: false }
    }
};
BigChartOption.CreateBarChartOption = function (type, categories, data) {
    var $container;
    switch (type) {
        case "1": $container = $('#container1'); break;
        case "2": $container = $('#container2'); break;
        case "3": $container = $('#container3'); break;
        case "4": $container = $('#container4'); break;
    }
    $container.highcharts({
        chart: {
            type: 'bar',
            backgroundColor: chartBackgroundColor,
            marginRight: -60
        },
        title: {
            text: '',
            enabled: false
        },
        subtitle: {
            text: '',
            enabled: false
        },
        xAxis: {
            categories: $.evalJSON(categories),
            title: {
                text: ''
            },
            tickLength: 0,
            labels: {
                y:8
            }
        },
        yAxis: {
            min: 0,
            title: {
                text: '',
                align: 'high'
            },
            labels: {
                style: yAxisStyle,
                formatter: function () {
                    return this.value == "300" ? "" : this.value;
                }
            },
            tickInterval: 50,
            max: 300
        },
        plotOptions: {
            bar: {
                cursor: 'pointer',
                dataLabels: {
                    enabled: true,
                    style: plotOptionsBarStyle,
                    //format: "{point.name}",
                    overflow: 'justify'
                    , formatter: function () {
                        return this.point.name == "-1" ? "" : this.point.name;
                    }
                },
                width: 50
            }
        },
        legend: {
            enabled: false
        },
        credits: {
            enabled: false
        },
        tooltip: {
            enabled: false
        },
        series: [{ data: $.evalJSON(data), borderWidth: 0, borderRadius: 10, pointWidth: 25}]
    });
}

//创建第一屏左图
BigChartOption.CreateChart_1_1_2 = function () {
    var chart1_2 = {};
    chart1_2.chartOption = BigChartOption.CreateColumnChartOption();
    chart1_2.chartOption.xAxis.categories = ['Waiting', 'Call', 'Free', 'Busy', 'Acw', 'Online CSR'];
    chart1_2.chartOption.yAxis.max = 15 + 5;
    chart1_2.chartOption.yAxis.tickInterval = 5;
    return chart1_2;
}
//创建第一屏右图
BigChartOption.CreateChart_1_3_4 = function () {
    var chart3_4 = {};
    chart3_4.chartOption = BigChartOption.CreateColumnChartOption();
    chart3_4.chartOption.xAxis.categories = ['接通率', '30s服务水平', '问题解决率', '客户满意率'];
    chart3_4.chartOption.xAxis.labels.style.fontFamily = "微软雅黑";
    chart3_4.chartOption.yAxis.max = 100 + 20;
    chart3_4.chartOption.yAxis.tickInterval = 20;
    chart3_4.chartOption.yAxis.labels.formatter = function () {
        return this.value + "%";
    };
    return chart3_4;
}

//创建2,3屏右侧图
BigChartOption.CreateChart_2_3 = function () {

    var chart = BigChartOption.CreateLineChartOption();
    chart.yAxis.max = 1500;
    chart.yAxis.tickInterval = 300;
    chart.yAxis.minorTickInterval = 150;
    return chart;
};
BigChartOption.CreateChart_2_4 = function () {

    var chart = BigChartOption.CreateLineChartOption();
    chart.yAxis.max = 800;
    chart.yAxis.tickInterval = 160;
    chart.yAxis.minorTickInterval = 80;
    return chart;
};
BigChartOption.CreateChart_3_3 = function () {

    var chart = BigChartOption.CreateLineChartOption();
    chart.yAxis.max = 7000;
    chart.yAxis.tickInterval = 1400;
    chart.yAxis.minorTickInterval = 700;
    return chart;
};
BigChartOption.CreateChart_3_4 = function () {

    var chart = BigChartOption.CreateLineChartOption();
    chart.yAxis.max = 2100;
    chart.yAxis.tickInterval = 420;
    chart.yAxis.minorTickInterval = 120;
    return chart;
};

//创建2,3屏左侧图
BigChartOption.CreateChart_2_1 = function () {
    var chart = {};
    chart.chartOption = BigChartOption.CreateColumnChartOption();
    chart.chartOption.xAxis.categories = ['Call', 'Free', 'Busy', 'Online CSR'];
    chart.chartOption.yAxis.max = 60 + 10;
    chart.chartOption.yAxis.tickInterval = 10;
    return chart;
};
BigChartOption.CreateChart_2_2 = function () {
    var chart = BigChartOption.CreateChart_2_1();
    chart.chartOption.yAxis.max = 20 + 5;
    chart.chartOption.yAxis.tickInterval = 5;
    return chart;
};
BigChartOption.CreateChart_3_1 = function () {
    var chart = BigChartOption.CreateChart_2_1();
    chart.chartOption.yAxis.max = 80 + 20;
    chart.chartOption.yAxis.tickInterval = 20;
    return chart;
};
BigChartOption.CreateChart_3_2 = function () {
    var chart = BigChartOption.CreateChart_2_1();
    chart.chartOption.yAxis.max = 20 + 5;
    chart.chartOption.yAxis.tickInterval = 5;
    return chart;
};


