var flag = false;
$(function () {
    //获取app中的数据
    setAjax({
        url:set.urlSecond,
        type:'get',
        data:{
            TopCount:4,
            MediaTypeID:14002,
            CategoryID:0
        },
        dataType: 'json'
      },
      function (data) {
        if(data.Status == 0) {
            $(".app_box").html(ejs.render($("#app_list").html(),{data:data.Result}));
            checkPrice($(".app_box .offer a"),url.app);
        }
    });
	//表单的表头(开始数据)
    setAjax({
        url:set.url,
        type:'get',
        data:set.data,
        dataType: 'json'
    },
    function (data) {
        if(data.Status == 0) {
            //表头数据拉取
            $('.wechat .nav').html(ejs.render($("#nav_list").html(),{data:data.Result[0].listCategory}));
            $('.sina_blog .nav').html(ejs.render($("#nav_list").html(),{data:data.Result[1].listCategory}));
            $('.video .nav').html(ejs.render($("#nav_list").html(),{data:data.Result[2].listCategory}));
            $('.seeding .nav').html(ejs.render($("#nav_list").html(),{data:data.Result[3].listCategory}));
                var MediaTypeID = data.Result[0].MediaTypeID;
                var wxArr = data.Result[0].listCategory;
                var wbArr = data.Result[1].listCategory;
                var spArr = data.Result[2].listCategory;
                var zbArr = data.Result[3].listCategory;
            //首屏渲染微信
            setAjax({
                // selector : '#wx_content',
                url:set.urlSecond,
                type:'get',
                data:{
                    TopCount:6,
                    MediaTypeID:14001,
                    CategoryID:wxArr[0].CategoryID
                },
                dataType: 'json'
              },
               function (data) {
                //  console.log(data);
                if(data.Status == 0) {
                    if(data.Result[0]){
                        $(".wechat_nr .wechat_nr_left").eq(0).html(ejs.render($("#wechat_list").html(),{data:data.Result[0]}));
                    }
                    if(data.Result[1]){
                        $(".wechat_nr .wechat_nr_left").eq(1).html(ejs.render($("#wechat_list").html(),{data:data.Result[1]}));
                    }
                    $(".wechat_nr .wechat_box").html(ejs.render($("#wechat_list2").html(),{data:data.Result}));
                    //雷达图
                    charts(data.Result);
                      // 查看报价
                    checkPrice($(".wechat_see"),url.wx);
                    checkPrice($(".wechat_see1"),url.wx);
                    //渲染点击样式
                    $(".wechat .nav li a").eq(0).addClass('active');
                }
          });
            //首屏渲染直播
            setAjax({
                url:set.urlSecond,
                type:'get',
                data:{
                    TopCount:4,
                    MediaTypeID:14005,
                    CategoryID:zbArr[0].CategoryID
                },
                dataType: 'json'
            },
                function (data) {
                    if(data.Status == 0) {
                        $(".seeding_box").html(ejs.render($("#seeding_list").html(),{data:data.Result}));
                        //点击查看报价
                        checkPrice($(".seeding_box .offer a"),url.zb);
                        //滚动显示
                        window.onscroll=function (){
                            if(($(document).scrollTop() + $(window).height() - 130) >= $(".seeding_nr").offset().top&&!flag) {
                                //进度条
                                progressBar($(".progress_fans"),$(".progress_gifts"),data.Result);
                            }
                        };
                        //渲染点击样式
                        $(".seeding .nav li a").eq(0).addClass('active');
                    }
              })
            //微信点击切换数据
            $(".wechat .nav li").off('click');
            $(".wechat .nav li").on('click',function () {
                var index = $(this).index();
                $(".wechat_nr .wechat_nr_left").eq(0).html("");
                $(".wechat_nr .wechat_nr_left").eq(1).html("");
                $(".wechat .nav li a").removeClass('active');
                $(this).children().addClass('active');
                setAjax({
                // selector : '#wx_content',
                url:set.urlSecond,
                type:'get',
                data:{
                    TopCount:6,
                    MediaTypeID:14001,
                    CategoryID:wxArr[index].CategoryID
                },
                dataType: 'json'
              },
               function (data) {
                //    console.log(data);
                if(data.Status == 0) {
                    if(data.Result[0]){
                        $(".wechat_nr .wechat_nr_left").eq(0).html(ejs.render($("#wechat_list").html(),{data:data.Result[0]}));
                    }
                    if(data.Result[1]){
                        $(".wechat_nr .wechat_nr_left").eq(1).html(ejs.render($("#wechat_list").html(),{data:data.Result[1]}));
                    }
                    $(".wechat_nr .wechat_box").html(ejs.render($("#wechat_list2").html(),{data:data.Result}));
                    var dataArr = data.Result;
                    //渲染雷达图
                    charts(dataArr);
                      // 查看报价
                     checkPrice($(".wechat_see"),url.wx);
                     checkPrice($(".wechat_see1"),url.wx);
                    }
                });
            })
            //点击切换微博数据
            $(".sina_blog .nav li").off('click');
            $('.sina_blog .nav li').on('click',function () {
                var index = $(this).index();
                $(".sina_blog .nav li a").removeClass('active');
                $(this).children().addClass('active');
                setAjax({
                    url:set.urlSecond,
                    type:'get',
                    data:{
                        TopCount:7,
                        MediaTypeID:14003,
                        CategoryID:wbArr[index].CategoryID
                    },
                    dataType: 'json'
                  },
                    function (data) {
                        if(data.Status == 0) {
                        $(".sina_nr").html(ejs.render($("#template_sina").html(),{data:data.Result}));
                        if($(".sina_ang ul").length>2){
                            $(".sina_ang ul").eq(2).css({"border-right":"0"});
                        }
                        //点击查看报价
                        checkPrice($(".sina_nr .wechat_see"),url.wb);
                        $(".sina_nr .sina_ang_h ul").eq(3).css('border-right','0');
                    }
                  })
            })
            //点击切换视频数据
            $(".video .nav li").off('click');
            $('.video .nav li').on('click',function () {
                var index = $(this).index();
                $(".video .nav li a").removeClass('active');
                $(this).children().addClass('active');
                setAjax({
                    selector:'.video_ang',
                    url:set.urlSecond,
                    type:'get',
                    data:{
                        TopCount:7,
                        MediaTypeID:14004,
                        CategoryID:spArr[index].CategoryID
                    },
                    dataType: 'json'
                  },
                    function (data) {
                        if(data.Status == 0) {
                            // if(scriptIe&&script&&script_h5){
                            //     document.getElementsByTagName("head")[0].removeChild(scriptIe);
                            //     document.getElementsByTagName("head")[0].removeChild(script);
                            //     document.getElementsByTagName("head")[0].removeChild(script_h5);
                            // }
                            var scriptIe = document.createElement("script");
                            var script = document.createElement("script");
                            var script_h5 = document.createElement("script");
                            scriptIe.src = "js/Videojs/videojs-ie8.min.js";
                            script.src = "js/Videojs/video.min.js";
                            script_h5.src = "//cdn.bootcss.com/html5shiv/3.6/html5shiv.min.js";
                            document.getElementsByTagName("head")[0].appendChild(scriptIe);
                            document.getElementsByTagName("head")[0].appendChild(script);
                            document.getElementsByTagName("head")[0].appendChild(script_h5);
                          $(".video_nr").html(ejs.render($("#template_video").html(),{data:data.Result}));
                          //点击查看报价
                          checkPrice($(".video_nr .offer a"),url.sp);
                          checkPrice($(".video_ang_h .wechat_see"),url.sp);
                          $(".video_ang .box").eq(2).css("border-right","0");
                          $(".video_nr .video_ang_h ul").eq(3).css("border-right","0");
                    }
                  })
            });
            //点击切换直播
            $(".seeding .nav li").off('click');
            $('.seeding .nav li').on('click',function () {
                flag = true;
                var index = $(this).index();
                $(".seeding .nav li a").removeClass('active');
                $(this).children().addClass('active');
                setAjax({
                    url:set.urlSecond,
                    type:'get',
                    data:{
                        TopCount:4,
                        MediaTypeID:14005,
                        CategoryID:zbArr[index].CategoryID,
                    },
                    dataType: 'json'
                  },
                    function (data) {
                        if(data.Status == 0) {
                            $(".seeding_box").html(ejs.render($("#seeding_list").html(),{data:data.Result}));
                            //点击查看报价
                            checkPrice($(".seeding_box .offer a"),url.zb);
                            progressBar($(".progress_fans"),$(".progress_gifts"),data.Result);
                        }
                  })
            })
            //微博首屏渲染
            $('.sina_blog .nav li').eq(0).trigger('click');
            //视频首屏渲染
            $('.video .nav li').eq(0).trigger('click');
        }
    });
    //点击更多推荐
    checkPrice($('.wechat_more a'),url.wx);
    checkPrice($('.sina_more a'),url.wb);
    checkPrice($('.video_more a'),url.sp);
    checkPrice($('.seeding_more a'),url.zb);
    checkPrice($('.app_more a'),url.app);

    //查看报价
    function checkPrice(element,url) {
      // 查看报价
      element.click(function () {
          if(CTLogin.IsLogin) {
              $(this).attr({href:url});
          }else{
              $(this).attr({href:'/login.aspx'});
          }
      })
    }
    //进度条
    function progressBar(element,element1,number) {
        element.each(function(index,ele){
           element.eq(index).animate({width:(number[index].FansCount/number[index].maxFansCount)*100+'%'},2200);
           element1.eq(index).animate({width:(number[index].CumulateIncome/number[index].maxCumulateIncome)*100+'%'},2200);
        })
    }
    //f5刷新返回顶部
    // $(document).keydown(function (event) {
    //    if (event.keyCode == 116) {
    //           $('body,html').scrollTop(0);
    //       }
    // });
})
//echarts图表函数
function charts(dataArr){
      for(var i=0;i<dataArr.length;i++){
        if(i==2){
          break;
        }
          //初始化ECharts实例
        var myChart = echarts.init($(".echarts_img")[i]);
        var option = {
          title: {
        //  text: '基础雷达图'
          },
          // tooltip: {
          // //  trigger: 'axis'
          // },
          legend: {
            //  data: ['预算分配（Allocated Budget）', '实际开销（Actual Spending）']
            // data:['某软件']
          },
          radar: [
           {
              shape: 'circle',
               indicator: [
                   {text: '粉丝数', max: dataArr[i].maxFansCount},
                   {text: '参考阅读数', max: dataArr[i].maxReferReadCount},
                   {text: '最高阅读数', max: dataArr[i].maxMaxinumReading},
                   {text: '平均点赞数', max: dataArr[i].maxAveragePointCount},
                   {text: '周更新频率', max: dataArr[i].maxUpdateCount}
               ],
               center: ['50%','50%'],
               radius: 80,
               splitArea: {//背景分割区域
                   areaStyle: {
                       color: ['#FEFEFE', '#FEFEFE', '#FEFEFE', '#FEFEFE','#FEFEFE']
                   }
               }
           }
          ],
          series: [
               {
                   type: 'radar',
                    tooltip: {
                       trigger: 'item'
                   },
                   itemStyle: {normal: {areaStyle: {type: 'default'}}},
                   data: [
                       {
                           value: [dataArr[i].FansCount,dataArr[i].ReferReadCount,dataArr[i].maxMaxinumReading,dataArr[i].AveragePointCount,dataArr[i].UpdateCount>7?7:dataArr[i].UpdateCount],
                          //  name: '某软件'
                          //阴影部分
                          itemStyle: {normal: {
                              areaStyle: {type: 'default'},
                              color:'#FCC648',
                              lineStyle:{  
                                  color:'#CEA086',
                                          }  
                          }
                        }
                       }
                   ]
               }
          ]
        };
        // 显示图表。
        myChart.setOption(option);
      }
  function loadScript(url, callback) {
    var script = document.createElement("script");
    script.type = "text/javascript";
    // IE
    if (script.readyState) {
        script.onreadystatechange = function () {
            if (script.readyState == "loaded" || script.readyState == "complete") {
                script.onreadystatechange = null;
                callback();
            }
        };
    } else { // others
        script.onload = function () {
            callback();
        };
    }
    script.src = url;
    document.body.appendChild(script);
  }
}
