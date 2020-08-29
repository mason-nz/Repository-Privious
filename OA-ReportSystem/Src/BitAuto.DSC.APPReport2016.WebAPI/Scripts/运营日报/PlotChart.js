var AppReport = AppReport || {};
AppReport.YYRB = (function () {
    var InitPage = function () {
        AjaxGet("/api/yyrb/GetPageData?r=" + Math.random(),
       null, null, function (data) {
           //console.log(data);
           if (data.Status == 0) {
               var RetDate = data.Result.RetDate;
               var Traffic = data.Result.Traffic;
               var Leads = data.Result.Leads;
               var BuyUser = data.Result.BuyUser;
               var Trades = data.Result.Trades;

               if (RetDate) {
                   $("#spanTitleDate").text(RetDate.RetDateVal);
                   $("#spanTitleWeek").text(RetDate.WeekDay);
               }
               if (Traffic) {
                   SetPanel("span_zfgs", Traffic);
               }
               if (Leads) {
                   SetPanel("span_zleads", Leads);
               }
               if (BuyUser) {
                   SetPanel("span_zxds", BuyUser);
               }
               if (Trades) {
                   SetPanel("span_jyl", Trades);
               }
           }
           else {
           }
       });
    };
    var SetPanel = function (id, obj) {
        $("#" + id).MagicNumber(Number(obj.Count));
        //$("#" + id + "_tb").MagicFloat(Number(obj.WeekBasis) * 100, 2, "%");
        //$("#" + id + "_hb").MagicFloat(Number(obj.DayBasis) * 100, 2, "%");

        $("#" + id + "_tb2").text(Number(Math.abs(obj.WeekBasis)).toPercent(1));
        $("#" + id + "_hb2").text(Number(Math.abs(obj.DayBasis)).toPercent(1));

        if (obj.WeekBasis > 0) {

            $("#" + id + "_tb").attr("class", "highColor");
            $("#" + id + "_tb2").addClass("highColorText");
        }
        else if (obj.WeekBasis < 0) {
            $("#" + id + "_tb").attr("class", "lowColor");
            $("#" + id + "_tb2").addClass("lowColorText");
        }
        else {
            $("#" + id + "_tb").attr("class", "normalColor");
            $("#" + id + "_tb2").addClass("normalColor");
        }

        if (obj.DayBasis > 0) {
            $("#" + id + "_hb").attr("class", "highColor");
            $("#" + id + "_hb2").addClass("highColorText");
        }
        else if (obj.DayBasis < 0) {
            $("#" + id + "_hb").attr("class", "lowColor");
            $("#" + id + "_hb2").addClass("lowColorText");
        }
        else {
            $("#" + id + "_hb").attr("class", "normalColor");
            $("#" + id + "_hb2").addClass("normalColor");
        }
    };

    var InitLine = function () {
        myChart.showLoading(defaultEChartsLoadingStyle);
        AjaxGet("/api/yyrb/GetDataTrend?r=" + Math.random(),
       null, null, function (data) {
           console.log(data);
           if (data.Status == 0) {
               //获取数据
               var seriesname = data.Result.seriesname;
               var datakey = data.Result.xAxisdata;

               //总覆盖用户数
               var datavalue1 = data.Result.seriesdata[0];
               //总Leads数
               var datavalue2 = data.Result.seriesdata[1];
               //总下单用户数
               var datavalue3 = data.Result.seriesdata[2];

               //处理数据
               //蓝，绿，黄
               var seriescolor = ['#3f85ff', '#19ede6', '#fde162'];

               //坐标轴-图表
               var datakey_new = new Array();
               for (var i = 0; i < datakey.length; i++) {
                   var date = DateFormat.StringToDate(datakey[i]);
                   if (date.getMonth() == 0 && date.getDate() == 1 && i > 0) {
                       //1月1号不在第一位置
                       datakey_new.push(DateFormat.DateToString(date, "yy-MM-dd"));
                   }
                   else {
                       datakey_new.push(DateFormat.DateToString(date, "MM-dd"));
                   }
               }

               //缩放
               var zoomkey = 14;
               var zoomindex = new Array();
               var count = datakey.length;
               if (count <= zoomkey) {
                   zoomindex.push(0);
                   zoomindex.push(count - 1);
               }
               else {
                   zoomindex.push(count - zoomkey);
                   zoomindex.push(count - 1);
               }

               //y2轴最大值
               var maxvalue1 = GetMaxNumber(DanweiZhuanHuan(datavalue1, "万"));
               //console.log(maxvalue1);
               maxvalue1 = Number(maxvalue1).toMaxRound();
               //console.log(maxvalue1);
               //y1轴最大值
               var maxvalue2 = GetMaxNumber(DanweiZhuanHuan(datavalue2, "万"));
               var maxvalue3 = GetMaxNumber(DanweiZhuanHuan(datavalue3, "万"));
               maxvalue2 = GetMaxNumber([maxvalue2, maxvalue3]);
               maxvalue2 = Number(maxvalue2 * 1.2).toMaxRound();

               //画图
               var option = {
                   grid: {
                       left: 20,
                       right: 20,
                       top: 60,
                       bottom: 0,
                       containLabel: true
                   },
                   legend: {
                       data: [{
                           name: seriesname[0],
                           icon: 'circle'
                       }, {
                           name: seriesname[1],
                           icon: 'circle'
                       }, {
                           name: seriesname[2],
                           icon: 'circle'
                       }],
                       top: 0,
                       right: 15,
                       textStyle: {
                           color: '#99a8d1',
                           fontFamily: '微软雅黑',
                           fontWeight: 400,
                           fontStyle: 'normal',
                           fontSize: 14
                       },
                       itemGap: 10,
                       itemWidth: 8,
                       itemHeight: 8,
                       selectedMode: false
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
                           if (params.length > 0) {
                               var dataIndex = params[0].dataIndex;
                               var date = DateFormat.StringToDate(datakey[dataIndex]);
                               var value1 = datavalue1[dataIndex] ? Number(datavalue1[dataIndex]).toMoney("万", 0) + "万" : "--";
                               var value2 = datavalue2[dataIndex] ? Number(datavalue2[dataIndex]).addComma() : "--";
                               var value3 = datavalue3[dataIndex] ? Number(datavalue3[dataIndex]).addComma() : "--";

                               var html = "<span style='font-weight:normal;color:rgba(200, 200, 200, 1);'></span>" + DateFormat.DateToString(date, "yyyy-MM-dd") + "<br/>";
                               html += "<span style='font-weight:normal;color:rgba(200, 200, 200, 1);'>" + params[0].seriesName + "：</span>" + value1 + "<br/>";
                               html += "<span style='font-weight:normal;color:rgba(200, 200, 200, 1);'>" + params[1].seriesName + "：</span>" + value2 + "<br/>";
                               html += "<span style='font-weight:normal;color:rgba(200, 200, 200, 1);'>" + params[2].seriesName + "：</span>" + value3 + "<br/>";
                               return html;
                           }
                           else {
                               return "";
                           }
                       }
                   },
                   dataZoom: {
                       type: 'inside',
                       startValue: zoomindex[0],
                       endValue: zoomindex[1]
                   },
                   xAxis: {
                       type: 'category',
                       boundaryGap: false, //留白策略
                       data: datakey_new,
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
                   },
                   yAxis: [{
                       type: 'value',
                       name: '单位/万',
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
                           formatter: function (value, index) {
                               //去掉千分位
                               return value;
                           }
                       },
                       max: maxvalue2,
                       interval: maxvalue2 / 4
                   }, {
                       type: 'value',
                       name: '总覆盖用户数',
                       nameTextStyle: {
                           color: '#36415d',
                           fontFamily: '微软雅黑',
                           fontWeight: 400,
                           fontStyle: 'normal',
                           fontSize: 12
                       },
                       nameLocation: 'end',
                       position: 'right',
                       axisLine: {
                           show: false
                       },
                       splitLine: {
                           show: false
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
                               //去掉千分位
                               return value;
                           }
                       },
                       max: maxvalue1,
                       interval: maxvalue1 / 4
                   }],
                   series: [{
                       name: seriesname[0],
                       yAxisIndex: 1,
                       type: 'line',
                       showSymbol: false,
                       symbol: 'circle',
                       symbolSize: 6,
                       itemStyle: {
                           normal: {
                               color: seriescolor[2]
                           }
                       },
                       lineStyle: {
                           normal: {
                               color: seriescolor[2],
                               width: 2,
                               type: 'solid'
                           }
                       },
                       data: DanweiZhuanHuan(datavalue1, "万")
                   }, {
                       name: seriesname[1],
                       yAxisIndex: 0,
                       type: 'line',
                       showSymbol: false,
                       symbol: 'circle',
                       symbolSize: 6,
                       itemStyle: {
                           normal: {
                               color: seriescolor[1]
                           }
                       },
                       lineStyle: {
                           normal: {
                               color: seriescolor[1],
                               width: 2,
                               type: 'solid'
                           }
                       },
                       data: DanweiZhuanHuan(datavalue2, "万")
                   }, {
                       name: seriesname[2],
                       yAxisIndex: 0,
                       type: 'line',
                       showSymbol: false,
                       symbol: 'circle',
                       symbolSize: 6,
                       itemStyle: {
                           normal: {
                               color: seriescolor[0]
                           }
                       },
                       lineStyle: {
                           normal: {
                               color: seriescolor[0],
                               width: 2,
                               type: 'solid'
                           }
                       },
                       data: DanweiZhuanHuan(datavalue3, "万")
                   }]
               };
               myChart.hideLoading();
               myChart.setOption(option);
               //事件
               myChart.off("datazoom");
               myChart.on("datazoom", function (params) {
                   //console.log(params);
                   var count = datakey.length;
                   if (params.batch && params.batch.length > 0 && count > zoomkey) {
                       var start = Math.round(params.batch[0].start / 100 * count);
                       var end = Math.round(params.batch[0].end / 100 * count);
                       if (end - start < zoomkey - 1) {
                           start = end - (zoomkey - 1);
                           if (start < 0) start = 0;
                           end = start + (zoomkey - 1);

                           var option = {
                               dataZoom: {
                                   type: 'inside',
                                   startValue: start,
                                   endValue: end
                               }
                           };
                           myChart.setOption(option);
                       }
                   }
               });
           }
           else {
               myChart.clear();
           }
       });
    };
    //单位转换，万or亿
    var DanweiZhuanHuan = function (array, danwei) {
        var array_new = new Array();
        for (var i = 0; i < array.length; i++) {
            array_new.push(Number(array[i]).toMoney(danwei, 0));
        }
        return array_new;
    }

    return {
        InitPage: InitPage,
        InitLine: InitLine
    }
})();