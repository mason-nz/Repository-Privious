$(function () {
    //取出链接中需要的参数
    function GetUserId() {
        var url = location.search; //获取url中"?"符后的字串
        var theRequest = {};
        if (url.indexOf("?") != -1) {
            var str = url.substr(1);
            strs = str.split("&");
            for (var i = 0; i < strs.length; i++) {
                theRequest[strs[i].split("=")[0]] = strs[i].split("=")[1];
            }
        }
        return theRequest;
    }

    var userData = GetUserId();
    if (userData.hasOwnProperty("MediaID")) {
        var MediaID = userData.MediaID;
    }
//渲染相似推荐信息
    setAjax({
        url: "/api/media/GetRecommendClass?v=1_1",
        type: "get",
        data: {
            mediaId: MediaID
        }
    }, function (data) {
        console.log(data);
        $(".recommend").html(ejs.render($("#similar").html(), {data: data.Result}));
        $(".recommend div").on("click", function () {
            MediaID = $(this).find("input").val();
            if (userData.hasOwnProperty("isAdd")) {
                window.location = "/OrderManager/wx_details_charts.html?isAdd=1&OrderID=" + userData[OrderID] + "&MediaID=" + MediaID;
            } else {
                window.location = "/OrderManager/wx_details_charts.html" + "?MediaID=" + MediaID;
            }
        })
    })
//基本信息部分的数据渲染
    setAjax({
        url: "/api/media/GetItem?v=1_1",
        type: "get",
        data: {
            bussinesstype: 14001,
            mediaId: MediaID
        }
    }, function (data) {
        console.log(data);
        $(".details_info").html(ejs.render($("#basicinfo").html(), {data: data.Result}));
        $("#creat").html(ejs.render($("#creattime").html(), {data: data.Result}));
//不同接口饼图数据渲染
//粉丝性别分布扇形图
        var p_sex = $('#sex').get(0);
        var myChart_sex = echarts.init(p_sex);
        var option_sex = {
            title: {
                text: '粉丝性别分布',
                // subtext: '纯属虚构',
                x: 'center',
                y: "bottom",
                textStyle: {
                    fontWeight: 'normal',
                    fontSize: 13
                }
            },
            tooltip: {
                trigger: 'item',
                formatter: "{a} <br/>{b} : {c} ({d}%)"//显示数据的类型
            },
            legend: {
                orient: 'vertical',//图例列表的布局朝向
                // left: 'left',
                right: 0,
                top: 'middle',
                // top:"50%",  //百分比跟middle不一样，后期看一下
                data: [
                    {name: '男'},
                    {name: '女'},
                    {name: '其他'}
                ]
            },
            series: [
                {
                    name: '粉丝性别比例',
                    type: 'pie',
                    hoverAnimation: true,//是否启用鼠标放上放大效果
                    radius: ["34.5%", '65.5%'],//可以设置内外半径，第一个是内半径，第二个是外半径
                    center: ['50%', '50%'],
                    label: {
                        normal: {
                            show: false,
                            position: "inside"
                        },//正常状态下饼图图形上的文本标签
                        emphasis: {
                            show: true//有交互的时候显示图形上的文本标签
                        }
                    },
                    data: [
                        {value: data.Result.FansMalePer, name: '男'},
                        {value: data.Result.ansFemalePer, name: '女'},
                        {value: (100 - data.Result.FansMalePer - data.Result.ansFemalePer), name: '其他'}
                    ],
                    // itemStyle: {
                    //     emphasis: {
                    //       color:["yellow"],
                    //         shadowBlur: 10,
                    //         shadowOffsetX: 0,
                    //         shadowColor: 'rgba(0, 0, 0, 0.5)'
                    //     }
                    // }//emphasis 是图形在高亮状态下的样式
                }
            ],
            color: ["#39bbdb", "#099b88", "#11b89c"]//通过修改全局的默认样式来得到我们自己想要的颜色
        };
        // 显示图表。
        myChart_sex.setOption(option_sex);
        // 取出粉丝数分布区域前五的城市
        var data_FansCount = [],
            data_ProvinceName = [],
            UserScale = 0;
        if (data.Result.FansArea.length > 5) {
            for (var i = 0; i < 5; i++) {
                data_FansCount.push({
                    value: data.Result.FansArea[i].UserScale,
                    name: data.Result.FansArea[i].ProvinceName
                });
                data_ProvinceName.push({name: data.Result.FansArea[i].ProvinceName});
                UserScale += data.Result.FansArea[i].UserScale;
            }
            data_FansCount.push({value: (100 - UserScale), name: '其他'});
            data_ProvinceName.push({name: '其他'});
        } else {
            for (var i = 0; i < data.Result.FansArea.length; i++) {
                data_FansCount.push({
                    value: data.Result.FansArea[i].UserScale,
                    name: data.Result.FansArea[i].ProvinceName
                });
                data_ProvinceName.push({name: data.Result.FansArea[i].ProvinceName});
            }
        }

//粉丝分布区域图表
        var p_FansCount = $('#FansCount').get(0);
        var myChart_FansCount = echarts.init(p_FansCount);
        var option_FansCount = {
            title: {
                text: '粉丝分布区域',
                // subtext: '纯属虚构',
                x: 'center',
                y: "bottom",
                textStyle: {
                    fontWeight: 'normal',
                    fontSize: 13
                }
            },
            tooltip: {
                trigger: 'item',//数据项图形触发
                formatter: "{a} <br/>{b} : {c} ({d}%)"//显示数据的类型
            },
            legend: {
                orient: 'vertical',//图例列表的布局朝向
                // left: 'left',
                right: 0,
                top: 'middle',
                // top:"50%",  //百分比跟middle不一样，后期看一下
                data: data_ProvinceName
            },
            series: [
                {
                    name: '粉丝分布区域',
                    type: 'pie',
                    hoverAnimation: true,//是否启用鼠标放上放大效果
                    radius: ["34.5%", '65.5%'],//可以设置内外半径，第一个是内半径，第二个是外半径
                    center: ['50%', '50%'],
                    label: {
                        normal: {
                            show: false,
                            position: "inside"
                        },//正常状态下饼图图形上的文本标签
                        emphasis: {
                            show: true//有交互的时候显示图形上的文本标签
                        }
                    },
                    data: data_FansCount,
                    // itemStyle: {
                    //     emphasis: {
                    //       color:["yellow"],
                    //         shadowBlur: 10,
                    //         shadowOffsetX: 0,
                    //         shadowColor: 'rgba(0, 0, 0, 0.5)'
                    //     }
                    // }//emphasis 是图形在高亮状态下的样式
                }
            ],
            color: ["#3bbcdc", "#099b88", "#33cb98", "#e55948", "#faa018", "#f9e44b"]//通过修改全局的默认样式来得到我们自己想要的颜色
        };
        // 显示图表。
        myChart_FansCount.setOption(option_FansCount);
    })
    //发文饼图及柱状图
    setAjax({
        url: "/api/Media/GetMediaDetailStatistic?v=1_1",
        type: "get",
        data: {
            MediaType: 14001,
            MediaID: MediaID
        }
    }, function (data) {
        console.log(data);
        if (data.Result.ReadForWeb != null) {
            var wxid = data.Result.ReadForWeb.WxId;
        }
        var url = "http://spiderapi.xyauto.com/API/WeChat/GetArticleWordCloudsByWxId?callback=getwords&wxid=" + wxid;

        var p_post = $('#publish_article').get(0);
        var myChart_post = echarts.init(p_post);
        var option_post = {
            title: {
                text: '发文总量',
                // subtext: '纯属虚构',
                x: 'center',
                y: "bottom",
                textStyle: {
                    fontWeight: 'normal',
                    fontSize: 13
                }
            },
            tooltip: {
                trigger: 'item',
                formatter: "{a} <br/>{b} : {c} ({d}%)"//显示数据的类型
            },
            legend: {
                orient: 'vertical',//图例列表的布局朝向
                // left: 'left',
                right: 0,
                top: 'middle',
                // top:"50%",  //百分比跟middle不一样，后期看一下
                data: [
                    {name: '原创'},
                    {name: '发布'}
                ]
            },
            series: [
                {
                    name: '发文总量',
                    type: 'pie',
                    hoverAnimation: true,//是否启用鼠标放上放大效果
                    radius: ["34.5%", '65.5%'],//可以设置内外半径，第一个是内半径，第二个是外半径
                    center: ['50%', '50%'],
                    label: {
                        normal: {
                            show: false,
                            position: "inside"
                        },//正常状态下饼图图形上的文本标签
                        emphasis: {
                            show: true//有交互的时候显示图形上的文本标签
                        }
                    },
                    data: [
                        {value: data.Result.ReadForWeb.Original, name: '原创'},
                        {value: data.Result.ReadForWeb.NonOriginal, name: '发布'}
                    ],
                    // itemStyle: {
                    //     emphasis: {
                    //       color:["yellow"],
                    //         shadowBlur: 10,
                    //         shadowOffsetX: 0,
                    //         shadowColor: 'rgba(0, 0, 0, 0.5)'
                    //     }
                    // }//emphasis 是图形在高亮状态下的样式
                }
            ],
            color: ["#39bbdb", "#099b88"]//通过修改全局的默认样式来得到我们自己想要的颜色
        };
        myChart_post.setOption(option_post);

        //阅读数量柱状图
        var data_readavgcount = [data.Result.ReadForWeb.ReadAvgSingleCount, data.Result.ReadForWeb.ReadAvgFirstCount, data.Result.ReadForWeb.ReadAvgSencondCount, data.Result.ReadForWeb.ReadAvgThirdCount],
            data_readmaxcount = [data.Result.ReadForWeb.ReadHighestSingleCount, data.Result.ReadForWeb.ReadHighestFirstCount, data.Result.ReadForWeb.ReadHighestSencondCount, data.Result.ReadForWeb.ReadHighestThirdCount],
            MaxData = Math.ceil(Math.max.apply(null, data_readmaxcount));
        var b_readcount = $('#readcount').get(0);
        var myChart_readcount = echarts.init(b_readcount);
        var option_readcount = {
            title: {
                // text: '近30天，广告位阅读数量分布',
                // subtext: 'dom',
                x: "center",
                y: "bottom"
            },
            tooltip: {
                // trigger: 'axis',
                formatter: "{a}<br/>{b}:{c}个",//显示数据的方式
                showContent: true  //是否显示提示框浮层,也就是说提示性的文字
            },

            legend: {
                right: 0,
                data: ['最高', '平均'],
                orient: 'vertical',
                align: "left" //图例的图形跟文字的位置
            },
            calculable: true,//还不太懂什么意思
            xAxis: [
                {
                    type: 'category',
                    data: ['单图文', '多图文头条', '多图文第二条', '多图文3—N条'],
                    axisTick: {
                        show: false  // 是否显示刻度
                    }
                }
            ],
            yAxis: [
                {
                    type: 'value',
                    max: MaxData,
                    // max:'dataMax',//设置该类目中的最大值显示到坐标轴上
                    // triggerEvent:true,
                    axisTick: {
                        show: true,//刻度
                        inside: true
                    },
                    axisLabel: {
                        show: true//刻度标签
                    },
                    name: "阅读数量",
                    splitLine: {show: false}//删除坐标轴在网格区域中的分隔线。
                }
            ],
            series: [
                {
                    name: '最高',
                    type: 'bar',
                    data: data_readmaxcount,
                    barWidth: 50//设置柱形条的宽度
                },
                {
                    name: '平均',
                    type: 'bar',
                    data: data_readavgcount,
                    barGap: "0%",//让不同系列的柱状图中间没有分隔
                    // barWidth:50,//设置柱形条的宽度
                }
            ],
            color: ["#01afec", "#87d0f3"]
        };
        myChart_readcount.setOption(option_readcount);
        //发文次数柱状图
        var count_x = [],
            count_y = [];
        for (var i = 0; i < data.Result.DayUpdateForWeb.length; i++) {
            count_x.push(data.Result.DayUpdateForWeb[i].Key);
            count_y.push(data.Result.DayUpdateForWeb[i].Value);
        }
        //最大数向上取整
        var count_datamax = Math.ceil(Math.max.apply(null, count_y));
        console.log(count_datamax);
        var b_publishcount = $('#publishcount').get(0);
        var myChart_publishcount = echarts.init(b_publishcount);
        var option_publishcount = {
            title: {
                // text: '近30天更新次数分布',
                x: 'center',//水平居中
                y: "bottom",
                // bottom:"bottom",
                // textBaseline: 'bottom',
                // textAlign:"center",
                textStyle: {
                    color: "red"
                }
            },
            color: ['#00b0ec'],
            tooltip: {
                // trigger: 'axis',
                // axisPointer : {            // 坐标轴指示器，坐标轴触发有效
                //     type : 'line'        // 默认为直线，可选为：'line' | 'shadow'
                // }
            },
            grid: {
                show: false,
                left: '3%',
                right: '4%',
                bottom: '3%',
                containLabel: true
            },
            xAxis: [
                {
                    type: 'category',
                    // splitNumber:30,
                    // // minInterval:5,
                    // max:30,
                    // min:0,
                    data: count_x,
                    axisTick: {
                        show: false, //是否显示坐标轴的刻度
                        // alignWithLabel: true

                    },
                    axisLine: {
                        show: true
                    },
                    splitLine: {
                        show: false  //不显示分割线
                    }

                }
            ],
            yAxis: [
                {
                    name: "发文次数",//坐标的名字
                    type: 'value',
                    max: count_datamax,
                    axisTick: {
                        show: true,//是否显示刻度
                        inside: true//刻度的朝向，默认朝外false
                    },
                    axisLine: {
                        show: true//是否显示坐标轴
                    },
                    axisLabel: {
                        show: true//是否显示坐标轴刻度标签
                    },
                    splitLine: {
                        show: false//坐标轴的分隔线
                    }
                }

            ],
            series: [
                {
                    name: '直接访问',
                    type: 'bar',
                    // barWidth: 10,//柱条的宽度
                    data: count_y,
                    // animation: false
                }
            ]
        };
        myChart_publishcount.setOption(option_publishcount);
        //发文时间柱状图
        var time_y = [];
        for (var i = 0; i < data.Result.HourUpdateForWeb.length; i++) {
            time_y.push(data.Result.HourUpdateForWeb[i].Value);
        }
        time_y = time_y.sort();
        var time_datamax = Math.ceil(time_y[time_y.length - 1]);
        var b_publishtime = $('#publishtime').get(0);
        var myChart_publishtime = echarts.init(b_publishtime);
        var option_publishtime = {
            title: {
                // text: '近30天更新次数分布',
                x: 'center',//水平居中
                y: "bottom",
                // bottom:"bottom",
                // textBaseline: 'bottom',
                // textAlign:"center",
                textStyle: {
                    color: "red"
                }
            },
            color: ['#00b0ec'],
            tooltip: {
                // trigger: 'axis',
                // axisPointer : {            // 坐标轴指示器，坐标轴触发有效
                //     type : 'line'        // 默认为直线，可选为：'line' | 'shadow'
                // }
            },
            grid: {
                show: false,
                left: '3%',
                right: '4%',
                bottom: '3%',
                containLabel: true
            },
            xAxis: [
                {
                    type: 'category',
                    // splitNumber:30,
                    // // minInterval:5,
                    // max:30,
                    // min:0,
                    data: ['00:00', '01:00', '02:00', '03:00', '04:00', '05:00', '06:00', '07:00', '08:00', '09:00', '10:00', '11:00', '12:00', '13:00', '14:00', '15:00', '16:00', '17:00', '18:00', '19:00', '20:00', '21:00', '22:00', '23:00', '24:00'],
                    axisTick: {
                        show: false, //是否显示坐标轴的刻度
                        // alignWithLabel: true
                    },
                    axisLine: {
                        show: true
                    },
                    splitLine: {
                        show: false  //不显示分割线
                    }

                }
            ],
            yAxis: [
                {
                    name: "发文次数",//坐标的名字
                    type: 'value',
                    max: time_datamax,
                    axisTick: {
                        show: true,//是否显示刻度
                        inside: true//刻度的朝向，默认朝外false
                    },
                    axisLine: {
                        show: true//是否显示坐标轴
                    },
                    axisLabel: {
                        show: true//是否显示坐标轴刻度标签
                    },
                    splitLine: {
                        show: false//坐标轴的分隔线
                    }
                }

            ],
            series: [
                {
                    name: '不同时间段的发文量',
                    type: 'bar',
                    barWidth: 10,//柱条的宽度
                    data: time_y,
                    // animation: false
                }
            ]
        };
        myChart_publishtime.setOption(option_publishtime);


        //词云图热词部分
        var cloud_hot = [],
            cloud_brand = [];
        $.ajax({
            type: "GET",
            cache: false,
            url: url,
            dataType: "jsonp",
            jsonp: "callback",
            jsonpCallback: "getwords",
            success: function (data) {
                console.log(data);
                for (var i = 0; i < data.HotWord.length; i++) {
                    cloud_hot.push({text: data.HotWord[i].Word, weight: data.HotWord[i].Weight});
                }
                for (var i = 0; i < data.BrandWord.length; i++) {
                    cloud_brand.push({text: data.BrandWord[i].Word, weight: data.BrandWord[i].Weight});
                }
                //根据不同权重赋值不同的颜色
                for (var j = 0; j < cloud_hot.length; j++) {
                    if (cloud_hot[j].weight == 1 || cloud_hot[j].weight == 2) {
                        cloud_hot[j].html = {"style": "color:blue"}
                    } else if (cloud_hot[j].weight == 3 || cloud_hot[j].weight == 4) {
                        cloud_hot[j].html = {"style": "color:green"}
                    } else if (cloud_hot[j].weight == 5 || cloud_hot[j].weight == 6) {
                        cloud_hot[j].html = {"style": "color:red"}
                    } else if (cloud_hot[j].weight == 7 || cloud_hot[j].weight == 8) {
                        cloud_hot[j].html = {"style": "color:purple"}
                    } else if (cloud_hot[j].weight == 9 || cloud_hot[j].weight == 10) {
                        cloud_hot[j].html = {"style": "color:yellow"}
                    }
                }
                //品牌词
                for (var j = 0; j < cloud_brand.length; j++) {
                    if (cloud_brand[j].weight == 1 || cloud_brand[j].weight == 2) {
                        cloud_brand[j].html = {"style": "color:blue"}
                    } else if (cloud_brand[j].weight == 3 || cloud_brand[j].weight == 4) {
                        cloud_brand[j].html = {"style": "color:green"}
                    } else if (cloud_brand[j].weight == 5 || cloud_brand[j].weight == 6) {
                        cloud_brand[j].html = {"style": "color:red"}
                    } else if (cloud_brand[j].weight == 7 || cloud_brand[j].weight == 8) {
                        cloud_brand[j].html = {"style": "color:purple"}
                    } else if (cloud_brand[j].weight == 9 || cloud_brand[j].weight == 10) {
                        cloud_brand[j].html = {"style": "color:yellow"}
                    }
                }

                $("#hotword").jQCloud(cloud_hot, {
                    removeOverflowing: true,//如果一个word超出了cloud元素的大小，则自动剔除
                    width: 340,
                    height: 299,
                    //shape: "rectangular"
                });
                $("#brandword").jQCloud(cloud_brand, {
                    removeOverflowing: true,//如果一个word超出了cloud元素的大小，则自动剔除
                    width: 340,
                    height: 299,
                    //shape: "rectangular"
                });
            }
        });
    })


    $(".mui-mbar-tabs").css("width", "40px");
});
//全局声明jonp的回调函数
function getwords() {

}
