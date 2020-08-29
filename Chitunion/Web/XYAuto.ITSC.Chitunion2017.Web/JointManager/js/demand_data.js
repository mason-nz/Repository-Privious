$(function() {
      var userData = GetUserId();
	var DemandBillNo = userData.hasOwnProperty("DemandBillNo") ? userData["DemandBillNo"] : "";
      var AdgroupId = userData.hasOwnProperty("AdgroupId") ? userData["AdgroupId"] : "";
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
                  if(data.Status == 0){
                        console.log(data);
                        //渲染页面
                        $(".effect_left_bottom").html(ejs.render($("#advertising").html(),{data:data.Result}));
                        //渲染标红需求
                        for(var i=0;i<$('.accurate_requirement').length;i++){
                             if($($('.accurate_requirement')[i]).attr("DemandBillNo") == DemandBillNo){
                                  $($('.accurate_requirement')[i]).css("color","red");
                             }
                        }


                        //对页面的操作
                        //点击收起展开的按钮
                        // $(".img_switch").off("click").on("click",function(){
                        //   $(this).children('img').eq(0).toggle();
                        //   $(this).children('img').eq(1).toggle();
                        //   $(this).parent().next().toggle();
                        //
                        // })
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

      //右上部分数据
      $.ajax({
            url:"http://www.chitunion.com/api/Demand/MapGetRightTwo",
            type:"get",
            dataType:"json",
            data:{
                  DemandBillNo: DemandBillNo
            },
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

      //中间的需求详情
      $.ajax({
            url:"http://www.chitunion.com/api/Demand/GetDemandDetail",
            type:"get",
            dataType:"json",
            xhrFields:{
                  withCredentials:true
            },
            data:{
              DemandBillNo: DemandBillNo
            },
            crossDomain:true,
            success: function(data) {
                  console.log(data);
                  if(data.Status == 0){
                        $(".effect_right_bottom_two_left").html(ejs.render($("#demand_tmp").html(),{data:data.Result}));
                        //品牌车型部分
                        var str1 = '';
                        for(var i=0;i<data.Result.Demand.BrandAndCarSerialList.length;i++){
                              str1 += data.Result.Demand.BrandAndCarSerialList[i] + "、";
                        }
                        $("#brand_model").html(str1.substring(0,str1.length-1));
                        //省份城市
                        var str2 = '';
                        for(var i=0;i<data.Result.Demand.ProvinceAndCityList.length;i++){
                              str2 += data.Result.Demand.ProvinceAndCityList[i] + "、";
                        }
                        $("#city").html(str2.substring(0,str2.length-1));
                        //经销商列表
                        var str3 = '';
                        for(var i=0;i<data.Result.Demand.DistributorList.length;i++){
                              str3 += data.Result.Demand.DistributorList[i] + "、";
                        }
                        $("#dealers").html(str3.substring(0,str3.length-1));

                        //关联广告部分
                        var str4 = '';
                        for(var i=0;i<data.Result.ADGroupList.length;i++){
                              str4 += data.Result.ADGroupList[i].AdgroupName+ "、";
                        }
                        $("#correlate_ad").html(str4.substring(0,str4.length-1));
                  }


            }
      })

      //中间的图表部分



      var DateType,
          DataType,
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
                             max: y_max
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






            //右边下部分交互

            //点击切换数据
            $(".detail_data_tab span").off("click").on("click",function(){
                  $(".detail_data_tab span").removeClass('demand_select');
                  $(this).addClass('demand_select');
                  if($(".detail_data_tab span").eq(0).hasClass("demand_select")){
                        $(".detail_data_ultwo").hide();
                        $(".table").eq(0).show();
                        $(".table").eq(1).hide();
                        //广告数据排行
                        get_data_ad();
                  }else{
                        $(".detail_data_ultwo").show();
                        $(".table").eq(0).hide();
                        $(".table").eq(1).show();
                        //需求数据明细
                        get_data_dem();
                  }
            })

            //广告排行数据部分

            // 曝光量降序为默认值
            var OrderBy = 1,
                IsDesc = 1,
                data_param = {
                      DemandBillNo: DemandBillNo,
                      OrderBy: OrderBy,
                      IsDesc: IsDesc,
                      PageIndex:1,
                      PageSize :20
                };
               //默认值渲染
               get_data_ad();
            //广告排行数据
            $(".OrderBy_status").off("click").on("click",function(){
                  data_param.OrderBy = $(this).attr("OrderBy");
                  if($(this).children('img').attr("src") == "/ImagesNew/btn_default.png"){
                        $(".OrderBy_status").children('img').attr("src","/ImagesNew/btn_default.png");
                        $(this).children('img').attr("src","/ImagesNew/btn_down.png");
                        data_param.IsDesc = 1;
                  }else if($(this).children('img').attr("src") == "/ImagesNew/btn_down.png"){
                        $(this).children('img').attr("src","/ImagesNew/btn_up.png");
                        data_param.IsDesc = 0;
                  }else{
                        $(this).children('img').attr("src","/ImagesNew/btn_down.png");
                        data_param.IsDesc = 1;
                  }
                  //每次点击取得最新的值
                  get_data_ad();
            })

            //请求数据函数
            function get_data_ad(){
                  $.ajax({
                        url:"http://www.chitunion.com/api/Demand/MapGetRightFour",
                        dataType:"json",
                        type:"get",
                        data:data_param,
                        xhrFields:{
                              withCredentials:true
                        },
                        crossDomain:true,
                        success: function(data){
                              console.log(data);
                              if(data.Status == 0){
                                    if(data.Result.TotalCount){
                                    //渲染页面
                                    $(".Weirdo").html(ejs.render($("#Weirdo_tmp").html(),{data:data.Result.List}));
                                    // $("#number").html(data.Result.TotalCount);
                                    //翻页逻辑
                                    $("#pageContainer").pagination(
                                          data.Result.TotalCount,
                                          {
                                                // items_per_page: 2, //每页显示多少条记录（默认为20条）
                                                callback: function (currPage, jg) {
                                                      data_param.PageIndex = currPage;
                                                      $.ajax({
                                                            url:"http://www.chitunion.com/api/Demand/MapGetRightFour",
                                                            dataType:"json",
                                                            type:"get",
                                                            data:data_param,
                                                            xhrFields:{
                                                                  withCredentials:true
                                                            },
                                                            crossDomain:true,
                                                            success: function(data){
                                                                  console.log(data);
                                                                  $(".Weirdo").html(ejs.render($("#Weirdo_tmp").html(),{data:data.Result.List}));
                                                            }
                                                      })

                                                }
                                          });
                                    }else{
                                          $('#pageContainer').html('');
                                          $('.Weirdo').html('<tr><td><img src="../images/no_data.png" style="display:block;margin:20px auto;"></td></tr>');
                                    }
                              }

                        }
                  })
            }

            //需求数据明细
            var BeginDate = "",
                EndDate = "",
                dem_param = {
                      DemandBillNo: DemandBillNo,
                      BeginDate: "",
                      EndDate: "",
                      PageIndex:1,
                      PageSize :20
                };

             //取得页面上的时间

             var start = {
               elem: "#startTime",
               fixed: false,
               // min: add_date(laydate.now()),
              //  min: laydate.now(),
               // istime: true,
               // issure: true,
               istoday:false,
               // isNeedConfirm: true,
               format: 'YYYY-MM-DD',
               choose: function (date) {
                   BeginDate = date;
                   if(EndDate != "" && BeginDate != ""){
                        if(EndDate < BeginDate){
                              layer.msg("结束时间不能小于开始时间",{time:2000});
                              $("#startTime").val("");
                        }else{
                              dem_param.EndDate = EndDate;
                              dem_param.BeginDate = BeginDate;
                        }
                   }else{
                     dem_param.BeginDate = BeginDate;
                   }
                   console.log(dem_param);
               }
             }
             var end = {
               elem: "#endTime",
               fixed: false,
               // min: add_date(laydate.now()),
              //  min: laydate.now(),
               // istime: true,
               // issure: true,
               istoday:false,
               // isNeedConfirm: true,
               format: 'YYYY-MM-DD',
               choose: function (date) {
                     EndDate = date;
                     if(EndDate != "" && BeginDate != ""){
                           if(EndDate < BeginDate){
                                 layer.msg("结束时间不能小于开始时间",{time:2000});
                                 $("#endTime").val("");
                           }else{
                                 dem_param.EndDate = EndDate;
                                 dem_param.BeginDate = BeginDate;
                           }
                     }else{
                           dem_param.EndDate = EndDate;
                     }
                     console.log(dem_param);

               }
             }
             //给开始时间跟结束时间绑定点击的事件
             $("#startTime").off("click").on("click", function () {

               laydate(start);
               if(!BeginDate){
                  $("#startTime").val("");
               }

               console.log(BeginDate);
             })
             $("#endTime").off("click").on("click", function () {
               laydate(end);
               if(!EndDate){
                  $("#endTime").val("");
               }
               console.log(EndDate);
             })


             //请求需求数据明细函数
             function get_data_dem(){
                  $.ajax({
                        url:"http://www.chitunion.com/api/Demand/MapGetRightFive",
                        dataType:"json",
                        type:"get",
                        data:dem_param,
                        xhrFields:{
                              withCredentials:true
                        },
                        crossDomain:true,
                        success: function(data){
                              console.log(data);
                              if(data.Status == 0){
                                    if(data.Result.TotalCount){
                                    //渲染页面
                                    $(".Weirdo_two").html(ejs.render($("#Weirdo_tmp_two").html(),{data:data.Result.List}));
                                    // $("#number").html(data.Result.TotalCount);
                                    //翻页逻辑
                                    $("#pageContainer").pagination(
                                          data.Result.TotalCount,
                                          {
                                                // items_per_page: 2, //每页显示多少条记录（默认为20条）
                                                callback: function (currPage, jg) {
                                                      dem_param.PageIndex = currPage;
                                                      $.ajax({
                                                            url:"http://www.chitunion.com/api/Demand/MapGetRightFive",
                                                            dataType:"json",
                                                            type:"get",
                                                            data:dem_param,
                                                            xhrFields:{
                                                                  withCredentials:true
                                                            },
                                                            crossDomain:true,
                                                            success: function(data){
                                                                  console.log(data);
                                                                  $(".Weirdo_two").html(ejs.render($("#Weirdo_tmp_two").html(),{data:data.Result.List}));
                                                                  // $("#number").html(data.Result.TotalCount);
                                                            }
                                                      })

                                                }
                                          });
                                    }else{
                                          $('#pageContainer').html('');
                                          $('.Weirdo_two').html('<tr><td><img src="../images/no_data.png" style="display:block;margin:20px auto;"></td></tr>');
                                    }
                              }

                        }
                  })
            }

            //点击查询搜索
            $("#search").off("click").on("click",function(){

                        //请求数据
                        get_data_dem();


            })

            //点击下载报表
            $("#down_data").off("click").on("click",function(){

                        //请求数据
                        $.ajax({
                              url:"http://www.chitunion.com/api/Demand/MapExportToExcel",
                              type:"post",
                              xhrFields:{
                                    withCredentials:true
                              },
                              crossDomain:true,
                              data:{
                                    DemandBillNo: DemandBillNo,
                                    BeginDate: BeginDate,
                                    EndDate: EndDate
                              },
                              success: function(data){
                                    console.log(data);
                                    if(data.Status == 0){
                                          //下载报表
                                          // $("#down_data a").attr("href","http://www.chitunion.com" + data.Result);
                                          window.location = "http://www.chitunion.com" + data.Result
                                    }

                              }

                        })

            })







            /*获取url的参数*/
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
})
