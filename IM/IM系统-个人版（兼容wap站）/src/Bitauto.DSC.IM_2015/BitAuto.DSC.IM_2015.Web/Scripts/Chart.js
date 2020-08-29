function ShowChart(id, chartType, result, selectType) {
    chartOption = {
        chart: {
            type: chartType
        },
        title: {
            text: ''
        },
        xAxis: {
            labels: {
                step: result.step == null ? "" : result.step,
                align: 'left',
                enabled: true  //是否显示x轴刻度值
            },
            categories: result.categories == null ? "" : result.categories
        },
        credits: {
            enabled: false
        },
        legend: {
            backgroundColor: '#FFFFFF',
            align: 'center',
            verticalAlign: 'top',
            itemStyle: {
                cursor: 'default'
            }
        },
        colors: ['#90D8F8', '#E7CBC8', '#4F81BD', '#f7a35c', '#8085e9', '#f15c80', '#e4d354', '#8085e8', '#8d4653', '#91e8e1'],
        plotOptions: {
            line: {
                marker: {
                    radius: 1,
                    lineColor: '#666666',
                    lineWidth: 1
                }
            },
            areaspline: {
                fillOpacity: 0.2
            },
            pie: {
                allowPointSelect: true,
                cursor: 'pointer',
                size: 160,
                dataLabels:
               {
                   enabled: true,
                   distance: 8,
                   color: '#000000',
                   connectorColor: '#000000',
                   format: '<b>{point.name}</b>: {point.percentage:.1f} %'
               }, showInLegend: true
            }
        },
        yAxis: { min: 0, title: { text: "" },
            allowDecimals: false //是否允许刻度有小数
        },
        exporting: { enabled: false },
        tooltip: {
            crosshairs: true,
            shared: true,
            useHTML: true,
            headerFormat: '<lager>{point.key}</lager>',
            pointFormat: '<div><span style=" font-weight:bolder;color: {series.color}">{series.name}: </span>' +
                '<span style="text-align: right"><b>{point.y}</span>',
            footerFormat: '</div>'
        }
    };
    chartOption.chart.renderTo = id;
    if (chartOption.chart.type == "pie") {

        if (selectType == 1) {
            var pieTooltip = {
                pointFormat: '<b>{point.y}</b>'
            }
            chartOption.tooltip = pieTooltip;


            var piedataLabels = {
                enabled: true,
                distance: 8,
                color: '#000000',
                connectorColor: '#000000',
                format: '<b>{point.name}</b>: {point.y}'
            }
            chartOption.plotOptions.pie.dataLabels = piedataLabels;
        }
        else {
            var pieTooltip = {
                pointFormat: '<b>{point.percentage:.1f}%</b>'
            }
            chartOption.tooltip = pieTooltip;
        }
        var pieLegend = {
            verticalAlign: 'bottom',
            itemStyle: {
                cursor: 'default'
            }
        }

        chartOption.chart.height = 260;
        chartOption.legend = pieLegend;
        chartOption.series = [result];

    }
    else {
        if (selectType == 0) {
            var hourTooltip = {
                crosshairs: true,
                shared: true,
                useHTML: true,
                headerFormat: '<lager>{point.key}:00-{point.key}:59</lager>',
                pointFormat: '<div><span style=" font-weight:bolder;color: {series.color}">{series.name}: </span>' +
                '<span style="text-align: right"><b>{point.y}</span>',
                footerFormat: '</div>'
            }
            chartOption.tooltip = hourTooltip;
        }

        chartOption.series = result.series
    }

    var c = new Highcharts.Chart(chartOption);
}

/**
* 将数值四舍五入后格式化成金额形式
*
* @param num 数值(Number或者String)
* @return 金额格式的字符串,如'1,234,567'
* @type String
*/
function formatCurrencyInt(num) {
    num = num.toString().replace(/\$|\,/g, '');
    if (isNaN(num))
        num = "0";
    sign = (num == (num = Math.abs(num)));
    num = Math.floor(num * 1 + 0.50000000001);
    cents = num % 1;
    num = Math.floor(num / 1).toString();
    for (var i = 0; i < Math.floor((num.length - (1 + i)) / 3); i++)
        num = num.substring(0, num.length - (4 * i + 3)) + ',' +
    num.substring(num.length - (4 * i + 3));
    return (((sign) ? '' : '-') + num);
}