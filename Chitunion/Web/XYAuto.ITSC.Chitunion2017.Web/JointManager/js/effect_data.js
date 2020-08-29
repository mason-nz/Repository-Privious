$(function(){
      //点击整体跳转整体数据
      $(".effect_left_top").off("click").on("click",function(data){
            window.location = "/JointManager/effect_data.html";
      })
      //左部分交互
      //渲染左侧信息
      $.ajax({
            url:"http://www.chitunion.com/api/Demand/MapGetLeft",
            type:"get",
            xhrFields:{
                  withCredentials:true
            },
            crossDomain:true,
            success:function(data){
                  console.log(data);
                  if(data.Status == 0){
                        //渲染页面
                        $(".effect_left_bottom").html(ejs.render($("#advertising").html(),{data:data.Result}));
                        //对页面的操作
                        //点击收起展开的按钮
                        $(".img_switch").off("click").on("click",function(){
                          if($(this).parent().next().get(0).style.display == "none"){
                                $(".effect_ad ul").hide();
                                for(var i=0;i<$(".img_switch").length;i++){
                                     $($(".img_switch")[i]).children('img').eq(0).hide();
                                     $($(".img_switch")[i]).children('img').eq(1).show();
                                }
                                $(this).children('img').eq(0).show();
                                $(this).children('img').eq(1).hide();
                                $(this).parent().next().show();
                          }else{
                                $(this).children('img').eq(0).hide();
                                $(this).children('img').eq(1).show();
                                $(this).parent().next().hide();
                          }
                        })
                        //点击具体需求数据进入需求的数据页面
                        $('.accurate_requirement').off("click").on("click",function(){

                         var DemandBillNo = $(this).attr("DemandBillNo");
                          window.location = "/JointManager/demand_data.html?DemandBillNo="+DemandBillNo;

                        })
                        //点击具体的广告数据的时候进入广告的数据页面
                        $('.accurate_advertising').off("click").on("click",function(){

                          var AdgroupId = $(this).attr("AdgroupId");
                          window.location = "/JointManager/adver_data.html?AdgroupId="+AdgroupId;
                        })
                  }


            }
      })
      //渲染右部分上部分信息
      $.ajax({
            url:"http://www.chitunion.com/api/Demand/MapGetRightOne",
            type:"get",
            xhrFields:{
                  withCredentials:true
            },
            crossDomain:true,
            dataType:"json",
            success:function(data){
                  if(data.Status == 0){
                        console.log(data);
                        //渲染上部分数据
                        $(".effect_right_top").html(ejs.render($("#right_top").html(),{data:data.Result}));
                  }

            }
      })
      //渲染右部分中间部分
      $.ajax({
            url:"http://www.chitunion.com/api/Demand/MapGetRightTwo",
            type:"get",
            dataType:"json",
            xhrFields:{
                  withCredentials:true
            },
            crossDomain:true,
            success: function(data){
                  console.log(data);
                  if(data.Status == 0){
                        $(".effect_right_bottom_one").html(ejs.render($("#center_data").html(),{data:data.Result}));
                  }
            }
      })
      //右部分图表逻辑交互（默认加载今天和点击量的数据）
      var DateType,
          DataType,
          BeginDate,
          EndDate,
          DemandBillNo,
          ADGroupID,
          //判断是不是大于两天
          flag,
          param;
          //定义图表部分需要的变量
          var echarts_name,
               x_name,
               y_name,
               x_data = [],
               y_data = [],
               interval_number,
               y_max,
               option;

         //获取初始的信息
         get_param();
         get_data();
         //定义一个取参数的公共函数
         function get_param(){
               DataType = parseInt($(".select option:selected").attr("DateType"));
               DateType = parseInt($(".echarts_data .effect_select").attr("DataType"));
               param = {
                     DemandBillNo: DemandBillNo || 0,
                     // ADGroupID: ADGroupID || 0,
                     DateType: DateType || 0,
                     DataType: DataType
               };
               console.log(param);
         }
         //定义一个请求的公共函数
         function get_data(){
               $.ajax({
                     url:"http://www.chitunion.com/api/Demand/MapGetRightThree",
                     type:"get",
                     dataType:"json",
                     xhrFields:{
                           withCredentials:true
                     },
                     crossDomain:true,
                     data:param,
                     success:function(data){
                           console.log(data);
                           if(data.Status == 0){
                                 x_data = [];
                                 y_data = [];
                                 if(DateType == 7|| DateType == 30){
                                       //天数
                                       for(var i=0;i<data.Result.length;i++){
                                             x_data.push(data.Result[i].Key.substring(5,10));
                                             y_data.push(data.Result[i].Value);
                                       }
                                 }else{
                                       //小时
                                       for(var i=0;i<data.Result.length;i++){
                                          //    x_data.push(data.Result[i].Key.substring(11,16));
                                             x_data.push(data.Result[i].Key);
                                             y_data.push(data.Result[i].Value);
                                       }
                                 }
                                 y_max = Math.max.apply(null, y_data);
                                 // y_max = Math.ceil(y_max*(1+1/4));
                                 var number_str = y_max.toString();
                                 var number_len = number_str.length;
                                 if(number_len == 1 && y_max < 4){
                                       y_max = 4;
                                 }else if(number_len > 1){
                                       y_max = y_max + Math.pow(10,number_len-1) - number_str.substring(1,number_len);
                                 }
                                 console.log(x_data);
                                 console.log(y_data);
                                 console.log(typeof y_max);
                                 //初始化数据生成表格
                                 get_option();
                                 if(x_data.length == 0 || y_data.length == 0){
                                       $("#echart").html('<img src="../images/no_data.png" style="display:block;margin:20px auto;">');
                                 }
                           }

                     }
               })
         }
         //获取图表option参数的函数
             function get_option(){
                   echarts_name = $(".select ").val();
                   if(echarts_name == "点击量" || echarts_name == "曝光量"){
                         y_name = "单位：次";
                   }else if(echarts_name == "点击率"){
                         y_name = "单位：%";
                   }else if(echarts_name == "费用"){
                         y_name = "单位：元";
                         y_data = y_data.map(function(a){
                            return a/100;
                         })
                         y_max = y_max/100;
                   }else if(echarts_name == "话单量" || echarts_name == "订单量"){
                         y_name = "单位：个";
                   }

                   if(DateType == 1 || DateType == -1){
                         interval_number = 3;
                   }else if(DateType == 7){
                         interval_number = "auto"
                   }else if(DateType == 30){
                         interval_number = 4;
                   }
                    option = {
                            title: {
                                text: echarts_name,
                                bottom:0,
                                x:"center"
                            },
                            tooltip: {
                                trigger: 'axis'
                            },
                            xAxis:  {
                                name: x_name,
                                type: 'category',
                                boundaryGap: false,
                                data: x_data,
                                axisLabel:{
                                      interval :interval_number
                                },
                            },
                            yAxis: {
                                name: y_name,
                                type: 'value',
                                max: y_max,
                           //      splitNumber:5
                            },
                            series: [
                                {
                                    name:echarts_name,
                                    type:'line',
                                    data:y_data,
                                    lineStyle:{  //线条颜色
                                          normal:{
                                                color: "#fa7674"
                                          }
                                    },
                                    areaStyle:{  //填充颜色
                                          normal:{
                                                color:"#fa7674"
                                          }
                                    }
                                },

                            ]
                        };
                        var mycharts = echarts.init($("#echart").get(0));
                        mycharts.setOption(option);
             }


      //select部分选择变化的时候
      $(".select").on("change",function(){
            //获取参数值
            get_param();
            get_data();

      })
      //点击天数选择切换
      $(".echarts_data ul .special").off("click").on("click",function() {
          $(".echarts_data ul .special").removeClass('effect_select');
          $(this).addClass('effect_select');
          //获取参数值
          get_param();
          get_data();

      })




})
