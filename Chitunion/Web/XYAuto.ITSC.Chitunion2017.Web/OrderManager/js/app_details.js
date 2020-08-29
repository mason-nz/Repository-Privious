/*
* Written by:     zhanglp
* function:       APP详情页效果js库
* Created Date:   2017-06-18
* Modified Date:  2017-07-2
*/
$(function(){
  // console.log(CTLogin);
  //页面中需要的参数
  var userData = GetUserId();
	var MediaType = userData.hasOwnProperty("MediaType") ? userData["MediaType"] : "";
	var MediaID =  userData.hasOwnProperty("MediaID") ? userData["MediaID"] : "";
  var TemplateID =  userData.hasOwnProperty("TemplateID") ? userData["TemplateID"] : "";
  var dataALL;

  //每个维度的选中样式,从上往下
  var select_style1,
      select_style2,
      select_style3,
      select_style4,
      select_style5;
    //每个维度包含的所有值，从上往下
  var advertising_style,
      shuffling_location,
      selling_platform,
      selling_way,
      selling_area;
    //每个维度做筛时操作存放数据的集合
  var opration1=[],
      opration2=[],
      opration3=[],
      opration4=[],
      opration5=[];
    //选定广告单元之后确定的集合（有可能是一个也有可能是多个对应不同的时间段）
    var select_time,
    //选定的精确的时间段
        sure_time,
    //加入购物车的开始时间
        shop_begintime,
    //加入购物车的结束时间
        shop_endtime;
    //广告单元集合
    var PublishDetail;
    //渲染定向城市需要的集合
    var SaleArea;
    //售卖区域的城市id
    var SaleAreaID;
    //投放次数的变量
    var ADLaunchDays;
    //无效时间段数组
    var voidDateRange;
    //取无效时间的两个中间状态的集合
    var begainarr = [];
        endarr = [];
    //购物车存在时间的时间集合
    var selectedPoint=[];
    //定义无效时间最大值和最小值
    var datamin = '2015-01-01',
        datamax = '2019-01-01';
    //定义时间跟城市的关系
    var city_time=[];
    //加入购物车中的信息（最后加入购物车的信息集合）
    var IDs = [];
    //查询加入购物车中的信息（处于中间状态）
    var ADDetailID=[];
    //定向城市相对应的addetailid变量（此处取得是每个广告单元的SaleArea）
    var addetailid_city;
    var start,
        end;
    //针对全国跟其他城市
    var ADDetailID_city = [];
  //首次页面加载先判断之前用户有没有操作过页面
  //获取链接的地址信息
  var locationHref = window.location.href;
  var  originalhref = locationHref.split("#")[0];
  //如果存在则获取选中的信息直接渲染
  if(locationHref.indexOf("#")!=-1){
    //说明：选中的之前的链接是按#position1-position2-position3
    var select_content = locationHref.split("#")[1].split("_");
    $.ajax({
      url:"/api/Periodication/SelectShoppingAppPublish",
      type:"get",
      data:{
        MediaID:MediaID,
        TemplateID:TemplateID,
      },
      dataType: 'json',
      xhrFields:{
        withCredentials:true
      },
      crossDomain:true,
      success:function(data){
        console.log(data);

        dataAll = data;

        // //渲染上部分信息
        // $(".fr").html(ejs.render($("#info_top").html(), {data: data.Result}));
        // //点击购物车图标跳转购物车页面
        // $(".details_cart").on("click",function(){
        //   if($('.cart_num').html()>0){
        //     window.location = "/OrderManager/shopcartForMedia01.html";
        //   }else{
        //     layer.msg("请添加广告位！",{time:2000});
        //   }
        // })
        //渲染详情页面
        $(".details_text").html(ejs.render($("#details_text").html(), {data: data.Result}));
        //页面加载完，出现城市列表点击别处隐藏
        $(".details_text").on("click",function(e){
          var e = e||window.event;
          var _con = $("#ding");
          var _con2 = $(".current");
          if(!_con.is(e.target) && !_con2.is(e.target)){
            $(".add_city").hide();
          }
        })
        //放大镜操作
        $('.jqzoom').jqzoom({
                  zoomType: 'standard',
                  lens: true,
                  preloadImages: true,
                  xOffset: 6,
                  title: false,
                  zoomWidth: 360,
                  zoomHeight: 360,
                  alwaysOn: false
              });
        //取出每个维度下所有信息
        advertising_style = data.Result.AdStyleList;
        shuffling_location = data.Result.CarouselCount;
        selling_platform = data.Result.SellingPlatform;
        selling_way = data.Result.SellingMode;
        selling_area = data.Result.CityGroupList;
        //广告单元集合
        PublishDetail = data.Result.PublishDetail;
        //展示记录之前操作的信息
        select_style1 = advertising_style[select_content[0]].AdStyle;
        select_style2 = select_content[1];
        select_style3 = select_content[2];
        select_style4 = select_content[3];
        select_style5 = select_content[4];

        //渲染基本信息部分的信息
        $(".details_right").html(ejs.render($("#basic_info").html(), {data: data.Result}));
        $("#material").html($("#material").html().trim());
        $("#show").html($("#show").html().trim());
        if($("#remark").html()){
          $("#remark").html($("#remark").html().trim());
        }
        //渲染案例展示页面
        show();
        //公用操作
        public_opration();
      }
    })
  }else{
    //首次加载页面默认加载选中返回的第一组数据,并把所有广告的信息存储
    $.ajax({
      url:"/api/Periodication/SelectShoppingAppPublish",
      type:"get",
      data:{
        MediaID:MediaID,
        TemplateID:TemplateID,
      },
      dataType: 'json',
      xhrFields:{
        withCredentials:true
      },
      crossDomain:true,
      success:function(data){
        console.log(data);
        dataAll = data;
        // //渲染上部分信息
        // $(".fr").html(ejs.render($("#info_top").html(), {data: data.Result}));

        //渲染详情页面
        $(".details_text").html(ejs.render($("#details_text").html(), {data: data.Result}));

        // //点击购物车图标跳转购物车页面
        // $(".details_cart").on("click",function(){
        //   if($('.cart_num').html()>0){
        //     window.location = "/OrderManager/shopcartForMedia01.html";
        //   }else{
        //     layer.msg("请添加广告位！",{time:2000});
        //   }
        // })

        //页面加载完，出现城市列表点击别处隐藏
        $(".details_text").on("click",function(e){
          var e = e||window.event;
          var _con = $("#ding");
          var _con2 = $(".current");
          if(!_con.is(e.target) && !_con2.is(e.target)){
            $(".add_city").hide();
          }
        })
        //放大镜操作
        $('.jqzoom').jqzoom({
                  zoomType: 'standard',
                  lens: true,
                  preloadImages: true,
                  xOffset: 6,
                  title: false,
                  zoomWidth: 360,
                  zoomHeight: 360,
                  alwaysOn: false
              });
        //取出每个维度下所有信息
        advertising_style = data.Result.AdStyleList;
        shuffling_location = data.Result.CarouselCount;
        selling_platform = data.Result.SellingPlatform;
        selling_way = data.Result.SellingMode;
        selling_area = data.Result.CityGroupList;
        //广告单元集合
        PublishDetail = data.Result.PublishDetail;
        //默认展示返回的第一个信息
        select_style1 = PublishDetail[0].ADStyle;
        select_style2 = PublishDetail[0].CarouselNumber;
        select_style3 = PublishDetail[0].SalePlatform;
        select_style4 = PublishDetail[0].SaleType;
        select_style5 = PublishDetail[0].GroupType;

        //渲染基本信息部分的信息
        $(".details_right").html(ejs.render($("#basic_info").html(), {data: data.Result}));
        $("#material").html($("#material").html().trim());
        $("#show").html($("#show").html().trim());
        if($("#remark").html()){
          $("#remark").html($("#remark").html().trim());
        }
        //渲染案例展示页面
        show();
        //公用操作
        public_opration();
      }

    })
  }
  //基本公用的集体操作函数
  function public_opration(){
    add_shopcart()
    //渲染收藏拉黑的操作
    collection();
    //操作的样式变化
    put_style();
    //点击切换每个维度
    click_switch();
    //点击定向投放的操作
    //渲染城市组
    render_city();
    //对售卖方式的操作
    // //对cpm,cpd的操作
    if($("#count")){
        if($("#way .active").attr("Mode")=="11001"){
          $("#count").hide();
        }else{
          $("#count").show();
        }
    }
    // 当轮播位置只有1或者0的时候隐藏轮播位置（数据库里面默认的是0）
    if($("#location a").length==1 && ($($("#location a")).attr("location")==1 ||$($("#location a")).attr("location")==0 )){
      $("#location").hide();
    }
    //如果页面渲染进来没有进行操作则直接查询当前的选中信息（SaleAreaID，此处针对全国跟其他城市）
    for(var i=0;i<$("#area a").length;i++){
      if($($("#area a")[i]).attr("class")=="active" && $($("#area a")[i]).attr("city")!=1){
        SaleAreaID = parseInt($($("#area a")[i]).attr("City"));
      }
    }
    console.log(SaleAreaID);
    //验证投放次数的输入正确性
    var reg = /^\+?[1-9]\d*$/;
    $("#put_number").on("input",function(){
      var val = $("#put_number").val();
      if(reg.test(val)&&val<366){

      }else{
        // layer.msg("只能输入0-365的正整数");
        $("#put_number").val("");
      }
    })
    //渲染日历插件中的时间（包括其中的有效时间，无效时间，购物车中已经存在的时间）
    render_time();
  }
  function render_time(){
    console.log(selectedPoint);
    //渲染日历插件中的时间（包括其中的有效时间，无效时间，购物车中已经存在的时间）
    //开始时间渲染
      start = {
        elem: "#startTime",
        fixed: false,
        // istime: true,
        // issure: true,
        istoday:false,
        voidDateRange:voidDateRange,
        selectedPoint: selectedPoint,
        // isNeedConfirm: true,
        min: add_date(laydate.now()),
        max: '',
        format: 'YYYY-MM-DD',
        isShowHoliday: true,//是否显示节假日(新调整的)
        choose: function (date,val) {
          //注：此处防止插件出现问题所以做了一个限制
          if(date>shop_endtime && shop_begintime!="" && shop_endtime!=""){
            layer.msg("开始时间不能大于结束时间",{time:2000});
            $("#startTime").val("");
            return false;
          }
          console.log(selectedPoint);
          console.log(voidDateRange);
          end.min = date; //开始日选好后，重置结束日的最小日期（注：防止插件存在问题，做第一步的保证）
          //注：此处防止插件出现问题所以做了一个限制（防止渲染无效时间失败）
          for(var i=0;i<voidDateRange.length;i++){
            if(date>=voidDateRange[i]["S"]&&date<=voidDateRange[i]["E"]||date<laydate.now().substring(0,10)){
                $("#startTime").val("");
                shop_begintime="";
                layer.msg("不在有效时间段内，请重新选择！",{time:2000});
                return false;
            }
          }
            //加入购物车的是时间（选中之后做一下其他的验证信息）
            shop_begintime = date;

            //购物车中已经有的时间不能再选择
            for(var i=0;i<selectedPoint.length;i++){
              if(date==selectedPoint[i]){
                layer.msg("购物车已经存在该时间,请勿重复选择",{time:2000});
                $("#startTime").val("");
                shop_begintime="";
                return false;
              }
              if(shop_begintime!="" && shop_endtime!="" && shop_begintime<selectedPoint[i] && shop_endtime>selectedPoint[i]){
                layer.msg("购物车已经存在中间部分时间段,请勿重复选择",{time:2000});
                $("#startTime").val("");
                shop_begintime="";
                return false;
              }
            }
            //开始结束时间全部符合之后，去查询选择到相应的价格信息，渲染到页面上
            render_city();
            //加入购物车
            add_shopcart();
        }
    }
    //结束时间渲染
     end = {
        elem: "#endTime",
        fixed: false,
        // istime: true,
        // issure: true,
        istoday:false,
        // isInitCheck: false,
        voidDateRange: voidDateRange,
        selectedPoint: selectedPoint,
        // isNeedConfirm: true,//需要确认的操作
        format: 'YYYY-MM-DD',
        min: add_date(laydate.now()),
        max: '',
        isShowHoliday: true,
        choose: function (date) {
          if(date<shop_begintime && shop_begintime!="" && shop_endtime!=""){
            layer.msg("结束时间不能小于开始时间",{time:2000});
            $("#endTime").val("");
            return false;
          }
          console.log(1);
          start.max = date; //结束日选好后，重置开始日的最大日期
          console.log(voidDateRange);
          for(var i=0;i<voidDateRange.length;i++){
            if(date>=voidDateRange[i]["S"]&&date<=voidDateRange[i]["E"]||date<laydate.now().substring(0,10)){
                $("#endTime").val("");
                shop_endtime="";
                layer.msg("不在有效时间段内，请重新选择！",{time:2000});
                return false;
            }
          }
          //加入购物车的结束时间
          shop_endtime = date;
          //购物车中已经有的时间不能再选择
          for(var i=0;i<selectedPoint.length;i++){
            if(date==selectedPoint[i]){
              layer.msg("购物车已经存在该时间,请勿重复选择",{time:2000});
              $("#endTime").val("");
              shop_endtime="";
              return false;
            }
            if(shop_begintime!="" && shop_endtime!="" && shop_begintime<selectedPoint[i] && shop_endtime>selectedPoint[i]){
              layer.msg("购物车已经存在中间部分时间段,请勿重复选择",{time:2000});
              $("#endTime").val("");
              shop_endtime="";
              return false;
            }
          }
          //查询渲染价格
          render_city();

          //加入购物车
          add_shopcart();
        }
    }
    //给开始时间跟结束时间绑定点击的事件
    $("#startTime").off("click").on("click", function () {
      laydate(start);
    })
    $("#endTime").off("click").on("click", function () {
      laydate(end);
    })
  }
  //加入购物车操作
  function add_shopcart(){
      $(".button").off("click").on("click",function(){
        //做几个必填的验证信息
        if(SaleAreaID==undefined){
          layer.msg("售卖区域为空,请输入！",{time:2000});
          return false;
        }
        if($("#way .active").attr("mode")==11002){
          if($("#put_number").val()==""){
            layer.msg("投放数量不能为空，请输入！",{time:2000});
            return false;
          }
        }
        if($("#startTime").val()=="" && $("#endTime").val()==""){
          layer.msg("开始投放时间或者结束投放时间不能为空，请输入！",{time:2000});
          return false;
        }

        if($("#count input").length!=0 && $("#count input").val()!=""){
          ADLaunchDays = parseInt($("#count input").val());
        }else{
          ADLaunchDays = 0;
        }

        console.log(select_time);
        console.log(shop_begintime.split(" ")[0]);
        console.log(shop_endtime.split(" ")[0]);
        //渲染加入购物车中的所有的信息
        for(var i=0;i<ADDetailID.length;i++){
          IDs.push(
            {
              "MediaID" : MediaID,
              "PublishDetailID" : ADDetailID[i].ADDetailID,
              "SaleAreaID" : SaleAreaID,
              "ADLaunchDays" : ADLaunchDays,
              "ADSchedule" : [
                {
                  "BeginData" : ADDetailID[i].shop_begintime,
                  "EndData" : ADDetailID[i].shop_endtime
                }
              ]
            }
          )
        }
          console.log(IDs);
          //加入购物车请求
          setAjax({
            url:"/api/ShoppingCart/AddShoppingCartAPP?v=1_1",
            type:"post",
            dataType:"json",
            data:{
              "MediaType" : 14002,
              "IDs" : IDs
            }
          },function(data){
            console.log(data);
            if(data.Status==0){
              layer.msg("加入购物车成功",{time:2000});
              //加入成功后渲染购物车的数量到最右侧的购物车数量
              setAjax({
                url:"/api/ShoppingCart/GetInfo_ShoppingCart?v=1_1",
                type:"get"
              },function(data){
                if(data.Status==0){
                  var shopcount1 = 0;
          				var shopcount2 = 0;
          				for(var i=0;i<data.Result.SelfMedia.length;i++){
          					for(var j=0;j<data.Result.SelfMedia[i].Medias.length;j++){
          						shopcount1++;
          					}
          				}
          				for(var i=0;i<data.Result.APP.length;i++){
                    for(var j=0;j<data.Result.APP[i].Medias.length;j++){
                      shopcount2++;
                    }
                  }
          				$('.cart_num').html(shopcount1+shopcount2);

                }
              })
            }else{
              layer.msg(data.Message,{time:2000});
            }
          })

      })
  }
  //操作时样式的变化（包含选中跟置灰的样式变化）
  function put_style(){
    //每次变化置空选择信息的集合
    opration1=[];
    opration2=[];
    opration3=[];
    opration4=[];
    opration5=[];
    select_time=[];
    //处理全部数据跟当前选中的位置做对比
    for(var i=0;i<PublishDetail.length;i++){
        if(PublishDetail[i].CarouselNumber==select_style2 && PublishDetail[i].SalePlatform==select_style3 && PublishDetail[i].SaleType==select_style4 && PublishDetail[i].GroupType==select_style5){
          opration1.push(PublishDetail[i].ADStyle);
        }
        if(PublishDetail[i].ADStyle==select_style1 && PublishDetail[i].SalePlatform==select_style3 && PublishDetail[i].SaleType==select_style4 && PublishDetail[i].GroupType==select_style5){
          opration2.push(PublishDetail[i].CarouselNumber);
        }
        if(PublishDetail[i].ADStyle==select_style1 && PublishDetail[i].CarouselNumber==select_style2 && PublishDetail[i].SaleType==select_style4 && PublishDetail[i].GroupType==select_style5){
          opration3.push(PublishDetail[i].SalePlatform);
        }
        if(PublishDetail[i].ADStyle==select_style1 && PublishDetail[i].CarouselNumber==select_style2 && PublishDetail[i].SalePlatform==select_style3 && PublishDetail[i].GroupType==select_style5){
          opration4.push(PublishDetail[i].SaleType);
        }
        if(PublishDetail[i].ADStyle==select_style1 && PublishDetail[i].CarouselNumber==select_style2 && PublishDetail[i].SalePlatform==select_style3 && PublishDetail[i].SaleType==select_style4){
          opration5.push(PublishDetail[i].GroupType);
        }
        if(PublishDetail[i].ADStyle==select_style1 && PublishDetail[i].CarouselNumber==select_style2 && PublishDetail[i].SalePlatform==select_style3 && PublishDetail[i].SaleType==select_style4 && PublishDetail[i].GroupType==select_style5){
          select_time.push({
            PubID:PublishDetail[i].PubID,
            ADDetailID:PublishDetail[i].ADDetailID,
            GroupType:PublishDetail[i].GroupType,
            SaleArea:PublishDetail[i].SaleArea,
            DifferDay:PublishDetail[i].DifferDay,
            BeginTime:PublishDetail[i].BeginTime.split(" ")[0],
            EndTime:PublishDetail[i].EndTime.split(" ")[0],
            HySalePrice:PublishDetail[i].HySalePrice,
            SalePrice:PublishDetail[i].SalePrice,
            ADStyle:PublishDetail[i].ADStyle,
            CarouselNumber:PublishDetail[i].CarouselNumber,
            SalePlatform:PublishDetail[i].SalePlatform,
            SaleType:PublishDetail[i].SaleType,
            GroupType:PublishDetail[i].GroupType
          });
        }
        //所有符合维度的集合
        console.log(select_time);
        // console.log(opration5)
    }
    //每个维度依次查询
    //number1
    for(var i=0;i<advertising_style.length;i++){
      //每个所有广告单元类型的值跟已经存在的组合做判断
      var flag = false;
      for(var j=0;j<opration1.length;j++){
        if(advertising_style[i].AdStyle==opration1[j]){
          flag = true;
        }
        if(flag){
          $("#style a").eq(i).removeClass('set_grey');
        }else{
          $("#style a").eq(i).addClass('set_grey');
        }
      }
      //如果跟当前选中值一样给一个选中样式
      if(select_style1==advertising_style[i].AdStyle){
        $("#style a").removeClass('active');
        $("#style a").eq(i).addClass('active');
      }
    }
    //number2
    for(var i=0;i<$("#location a").length;i++){
      var flag = false;
      for(var j=0;j<opration2.length;j++){
        if(opration2[j] == $("#location a").eq(i).attr("location")){
          flag = true;
        }
        if(flag){
          $("#location a").eq(i).removeClass('set_grey');
        }else{
          $("#location a").eq(i).addClass('set_grey');
        }
      }
      if(select_style2==$("#location a").eq(i).attr("location")){
        $("#location a").removeClass('active');
        $("#location a").eq(i).addClass('active');
      }
    }
    //number3
    console.log(select_style3);
    for(var k=0;k<$("#platform a").length;k++){
      var flag = false;
      for(var i=0;i<opration3.length;i++){
        if(opration3[i] == $("#platform a").eq(k).attr("Platform")){
          flag = true;
        }
        if(flag){
          $("#platform a").eq(k).removeClass('set_grey');
        }else{
          $("#platform a").eq(k).addClass('set_grey');
        }
      }
      if(select_style3==$("#platform a").eq(k).attr("Platform")){
        $("#platform a").removeClass('active');
        $("#platform a").eq(k).addClass('active');
      }
    }
    //number4
    for(var i=0;i<$("#way a").length;i++){
      var flag = false;
      for(var j=0;j<opration4.length;j++){
        if(opration4[j]==$("#way a").eq(i).attr("Mode")){
          flag = true;
        }
        if(flag){
          $("#way a").eq(i).removeClass('set_grey');
        }else{
          $("#way a").eq(i).addClass('set_grey');
        }
      }
      if(select_style4==$("#way a").eq(i).attr("Mode")){
        $("#way a").removeClass('active');
        $("#way a").eq(i).addClass('active');
      }
    }
    //number5
    console.log(select_style5);
    for(var i=0;i<$("#area a").length;i++){
      var flag = false;
      for(var j=0;j<opration5.length;j++){
        if(opration5[j]==$("#area a").eq(i).attr("City")){
          flag = true;
        }
        if(flag){
          $("#area a").eq(i).removeClass('set_grey');
        }else{
          $("#area a").eq(i).addClass('set_grey');
        }
      }
      if(select_style5==$("#area a").eq(i).attr("City")){
        $("#area a").removeClass('active');
        $("#area a").eq(i).addClass('active');
      }
    }
  }

  //点击每个维度的切换,并赋值锚点记录操作页面的状态
  function click_switch(){
    $("#style a").not(".set_grey").off("click").on("click",function(){
      select_style1 = $(this).html();
      shop_begintime="";
      shop_endtime="";
      $("#startTime").val("");
      $("#endTime").val("");
      public_opration();
      render_shopcount();

      // put_style();
      history.pushState(null, null, originalhref+"#"+$(this).attr("style")+"_"+select_style2+"_"+select_style3+"_"+select_style4+"_"+select_style5);
    })
    $("#location a").not(".set_grey").off("click").on("click",function(){
      select_style2 = $(this).html();
      shop_begintime="";
      shop_endtime="";
      $("#startTime").val("");
      $("#endTime").val("");

      public_opration();
      render_shopcount();

      history.pushState(null, null, originalhref+"#"+$("#style .active").attr("style")+"_"+select_style2+"_"+select_style3+"_"+select_style4+"_"+select_style5);
    })

    $("#platform a").not(".set_grey").off("click").on("click",function(){
      select_style3 = $(this).attr("Platform");
      shop_begintime="";
      shop_endtime="";
      $("#startTime").val("");
      $("#endTime").val("");

        public_opration();
        render_shopcount();

      history.pushState(null, null, originalhref+"#"+$("#style .active").attr("style")+"_"+select_style2+"_"+select_style3+"_"+select_style4+"_"+select_style5);
    })
    $("#way a").not(".set_grey").off("click").on("click",function(){
      //对cpm,cpd的操作
      if($("#count")){
          if($(this).attr("Mode")=="11001"){
            $("#count").hide();
          }else{
            $("#count").show();
          }
      }
      select_style4 = $(this).attr("Mode");
      shop_begintime="";
      shop_endtime="";
      $("#startTime").val("");
      $("#endTime").val("");

        public_opration();
        render_shopcount();

      history.pushState(null, null, originalhref+"#"+$("#style .active").attr("style")+"_"+select_style2+"_"+select_style3+"_"+select_style4+"_"+select_style5);
    })
    $("#area a").not(".set_grey").off("click").on("click",function(){
      select_style5 = $(this).attr("City");
      console.log(1);
      // render_city();
      if($(this).attr("City")==1){
        var flag = true;
        for(var i=0;i<$("#area a").length;i++){
          if($($("#area a")[i]).attr("city")==0){
            flag = false;
          }
        }
        if(flag){
          $(".add_city").css("left","-55px");
          $(".add_city").show();
        }else{
          $(".add_city").css("left","15px");
          $(".add_city").show();
        }
      }else{
        $(".add_city").hide();
      }
      if($(this).attr("City")==-1){
        SaleAreaID = -1;
        render_shopcount();
        $("#qita").show();
      }else{
        SaleAreaID = 0;
        render_shopcount();
        $("#qita").hide();
      }
      // render_city();
      // console.log(SaleAreaID);
      shop_begintime="";
      shop_endtime="";
      $("#startTime").val("");
      $("#endTime").val("");
        public_opration();
      history.pushState(null, null, originalhref+"#"+$("#style .active").attr("style")+"_"+select_style2+"_"+select_style3+"_"+select_style4+"_"+select_style5);
    })

  }

  //把需要的城市跟单价价格渲染到页面上()
  function render_city(){
    ADDetailID_city=[];
    //找到对应的全国的跟其他的SaleArea
    if($("#area .active").html()=="全国"){
      for(var i=0;i<select_time.length;i++){
        if(select_time[i].GroupType==0){
          addetailid_city=select_time[i].SaleArea;
          ADDetailID_city.push(select_time[i].ADDetailID);
        }
      }
    }
    if($("#area .active").html()=="其他城市"){
      for(var i=0;i<select_time.length;i++){
        if(select_time[i].GroupType==-1){
          addetailid_city=select_time[i].SaleArea;
          ADDetailID_city.push(select_time[i].ADDetailID);
        }
      }
    }
    // console.log(laydate.now()+1);
    //每次渲染重置初始无效时间段条件
     voidDateRange=[];
      SaleArea = [];
      city_time = [];
      //排序时间数组，按顺序加入无效时间数组
      console.log(select_time);
      begainarr=[];
      endarr=[];
      for(var m=0;m<select_time.length;m++){
        if(begainarr.indexOf(select_time[m].BeginTime)==-1){
          begainarr.push(select_time[m].BeginTime);
        }
        if(endarr.indexOf(select_time[m].EndTime)==-1){
          endarr.push(select_time[m].EndTime);
        }
      }
      begainarr.sort();
      endarr.sort();
      for(var i=0;i<begainarr.length-1;i++){
        for(var j=0;j<select_time.length;j++){
          if(begainarr[i]==select_time[j].BeginTime){
            if(add_date(select_time[j].EndTime)!=begainarr[i+1]){
              voidDateRange.push({'S': add_date(select_time[j].EndTime), 'E': reduce_date(begainarr[i+1])});
            }
          }
        }
      }
      //渲染无效时间段数组
      voidDateRange.unshift({ 'S':datamin, 'E': reduce_date(begainarr[0]) });
      voidDateRange.push({ 'S': add_date(endarr[endarr.length-1]), 'E': datamax });
      for(var i=0;i<voidDateRange.length;i++){
        if(compare(voidDateRange[i].E,laydate.now())>1){
          voidDateRange[i].E = laydate.now().split(" ")[0];
        }
        if(compare(voidDateRange[i].S,laydate.now())>1 && i!=0){
          voidDateRange[i].S = laydate.now().split(" ")[0];
        }
      }
      console.log(voidDateRange);
      //查找所有符合该组合下面的城市信息跟价格信息（注：这个地方其实多加了一步，用select_time就可以）
      for(var i=0;i<select_time.length;i++){
          city_time.push({
            ADDetailID:select_time[i].ADDetailID,
            SaleArea:select_time[i].SaleArea,
            HySalePrice:select_time[i].HySalePrice,
            SalePrice:select_time[i].SalePrice,
            BeginTime:select_time[i].BeginTime,
            EndTime: select_time[i].EndTime,
            ADStyle:select_time[i].ADStyle,
            CarouselNumber:select_time[i].CarouselNumber,
            SalePlatform:select_time[i].SalePlatform,
            SaleType:select_time[i].SaleType,
            GroupType:select_time[i].GroupType
          })
      }
      console.log(city_time);
      //找出所有符合该广告单元组合下的所有的价格最小的价格
      var min1=9999999;
      var min2=9999999;
      for(var n=0;n<city_time.length;n++){
        if(city_time[n].HySalePrice<min1){
          min1 = city_time[n].HySalePrice;
        }
        if(city_time[n].SalePrice<min2){
          min2 = city_time[n].SalePrice;
        }
      }
      //渲染页面的价格变化（SalePrice这个价格是肯定有的，HySalePrice可有可无）
      if(min1!=0 && min1<min2){
        $("#price em").html("￥"+min1);
      }else if(min1!=0 && min1>min2){
          $("#price em").html("￥"+min2);
      }else {
        $("#price em").html("￥"+min2);
      }
      //页面首次渲染的该广告单元下面的所有的城市
      // 找出对应的所有的SaleArea集合（包含每个SaleArea下面的所有的城市的信息），注：SaleArea下的城市是不可能重复的
      for(var j=0;j<dataAll.Result.CityAreaList.length;j++){
        for(var k=0;k<city_time.length;k++){
          if(dataAll.Result.CityAreaList[j].SaleArea==city_time[k].SaleArea){
              SaleArea.push({
                ADDetailID:city_time[k].ADDetailID,
                SaleArea:city_time[k].SaleArea,
                FirstLetter:dataAll.Result.CityAreaList[j].FirstLetter,
                CityID:dataAll.Result.CityAreaList[j].CityID,
                City:dataAll.Result.CityAreaList[j].City
              })

          }
        }
      }
      console.log(SaleArea);
      //城市弹窗的上部分字母表部分
      var LetterObject = [];
      //后期加的全部
          LetterObject.push("全部");
      //渲染字母表
      for(var i=0;i<SaleArea.length;i++){
        if(LetterObject.indexOf(SaleArea[i].FirstLetter)==-1){
          LetterObject.push(SaleArea[i].FirstLetter);
        }
      }
      console.log(LetterObject);
      //每次渲染清空容器
      $(".letter").html("");
      for(var i=0;i<LetterObject.length;i++){
        //对字母进行去重
        for(var j=0;j<$(".letter span").length;j++){
          if($($(".letter span")[j]).html()==LetterObject[i]){
            $($(".letter span")[j]).remove();
          }
        }
        $(".letter").append('<span>'+LetterObject[i]+'</span>');
      }
      console.log(SaleArea);
      //点击展现城市列表
      var flag = true;
        $(".letter span").off("click").on("click",function(){
          $(".letter span").removeClass('current');
          $(this).addClass('current');
          //每次点击之前先清空存放城市的容器
          $(".add_city ul").html("");
          for(var i=0;i<SaleArea.length;i++){
            //去重显示城市
          if($(this).html()==SaleArea[i].FirstLetter){
            flag = true;
            for(var j=0;j<$(".add_city ul li").length;j++){
              if($($(".add_city ul li")[j]).html()==SaleArea[i].City){
                flag = false;
              }
            }
            if(flag){
              $(".add_city ul").append('<li style="width:60px" id="'+SaleArea[i].CityID+'" ADDetailID="'+SaleArea[i].ADDetailID+'" SaleArea="'+SaleArea[i].SaleArea+'">'+SaleArea[i].City+'</li>');
            }
          }
          //去重显示城市
            if($(this).html()=="全部"){
              flag = true;
              for(var j=0;j<$(".add_city ul li").length;j++){
                if($($(".add_city ul li")[j]).html()==SaleArea[i].City){
                  flag = false;
                }
              }
              if(flag){
                $(".add_city ul").append('<li style="width:60px" id="'+SaleArea[i].CityID+'" ADDetailID="'+SaleArea[i].ADDetailID+'" SaleArea="'+SaleArea[i].SaleArea+'">'+SaleArea[i].City+'</li>');
              }
            }
          }
          //当不点击的时候获取当前选中的城市的一些信息
          for(var i=0;i<$(".add_city ul li").length;i++){
            if($("#ding").html()==$($(".add_city ul li")[i]).html()){
              addetailid_city = $($(".add_city ul li")[i]).attr("SaleArea");
              ADDetailID_city.push($($(".add_city ul li")[i]).attr("ADDetailID"));
              SaleAreaID = $($(".add_city ul li")[i]).attr("id");
            }
          }
          //点击单个城市的交互
          $(".add_city ul li").off("click").on("click",function(){
            $(".add_city ul li").removeClass('current');
            $(this).addClass('current');
            SaleAreaID = $(this).attr("id");
            addetailid_city = $(this).attr("SaleArea");
            ADDetailID_city.push($(this).attr("ADDetailID"));
            console.log(SaleAreaID);
            console.log(addetailid_city);

            render_shopcount();


            //点击完城市按钮赋值该城市的信息
            for(var i=0;i<$("#area a").length;i++){
              if($($("#area a")[i]).html()!="全国" && $($("#area a")[i]).html()!="其他城市"){
                $("#area a").eq(i).html($(this).html());
              }
            }

            //操作完隐藏城市的列表
            $(".add_city").hide(500);
          })


        })

        //全部加入后清浮动
        $(".add_city ul").append('<div class="clear"></div>');
        $(".letter span").eq(0).trigger('click');

        //当选定一个确定的时间组合之后展示该时间段下的相应的价格
      if(shop_begintime && shop_endtime){
        for(var i=0;i<voidDateRange.length;i++){
          if(shop_begintime<=voidDateRange[i].S && shop_endtime>=voidDateRange[i].E){
            layer.msg("请选择连续的时间段",{time:2000});
            $("#startTime").val("");
            $("#endTime").val("");
            shop_begintime="";
            shop_endtime="";
            return false;
          }
        }
        //当时间段为连续的情况取得相对应的连续时间段的集合
        var begin_arry=[],
            end_arry=[];
        //定义的销售价跟节假日价格
        var SalePrice,
            HySalePrice;
      //选择确定时间之后去选择确定的城市集合
      SaleArea=[];
      //每次置空购物车中的数据
      IDs=[];
      //加入购物车的相对应的中间转换集合
      ADDetailID=[];
      //此时分为两种情况（1.正常的不跨刊例的情况，此时直接把信息加入购物车即可 2.连续时间段跨刊例的情况，截取相对应的时间段信息加入购物车）
      for(var i=0;i<city_time.length;i++){
        //不跨时间段的情况
        if(shop_begintime>=city_time[i].BeginTime && shop_endtime<=city_time[i].EndTime && city_time[i].SaleArea==addetailid_city){
          SalePrice = city_time[i].SalePrice;
          HySalePrice = city_time[i].HySalePrice;
          ADDetailID.push({
            ADDetailID:city_time[i].ADDetailID,
            shop_begintime:shop_begintime,
            shop_endtime:shop_endtime
          });
          //选择投放时间变换广告单价
          $.ajax({
            url:"/api/ShoppingCart/QueryHolidays?v=1_1",
            type:"get",
            data:{
              beginDate:shop_begintime,
              endDate:shop_endtime
            },
            dataType: 'json',
            xhrFields: {
              withCredentials: true
            },
            crossDomain: true,
            success:function(data){
              console.log(data);
              var time_between = compare(shop_begintime,shop_endtime);
              console.log(time_between);
              if(data.Result.Days==0){
                // $("#price em").html(sure_time.HySalePrice+"~"+sure_time.SalePrice);
                $("#price em").html("￥"+SalePrice);
              }else{
                if(time_between>data.Result.Days){
                  if(HySalePrice!=0 && SalePrice>HySalePrice){
                    $("#price em").html("￥"+HySalePrice+"~"+"￥"+SalePrice);
                  }else if(HySalePrice!=0 && SalePrice<HySalePrice){
                    $("#price em").html("￥"+SalePrice+"~"+"￥"+HySalePrice);
                  }else{
                    $("#price em").html("￥"+SalePrice);
                  }
                }else{
                  if(HySalePrice!=0){
                    $("#price em").html("￥"+HySalePrice);
                  }else{
                    $("#price em").html("￥"+SalePrice);
                  }
                }
              }
            }
          })
        }else{
          //跨刊例的连续时间段的情况，（如果是跨时间段但是不连续直接提示）
          if(shop_begintime>=city_time[i].BeginTime && shop_begintime<=city_time[i].EndTime && city_time[i].SaleArea==addetailid_city){
            begin_arry = {
              ADDetailID:city_time[i].ADDetailID,
              SaleArea:city_time[i].SaleArea,
              HySalePrice:city_time[i].HySalePrice,
              SalePrice:city_time[i].SalePrice,
              BeginTime:city_time[i].BeginTime,
              EndTime: city_time[i].EndTime,
              ADStyle:city_time[i].ADStyle,
              CarouselNumber:city_time[i].CarouselNumber,
              SalePlatform:city_time[i].SalePlatform,
              SaleType:city_time[i].SaleType,
              GroupType:city_time[i].GroupType
            }
          }
          if(shop_endtime>=city_time[i].BeginTime && shop_endtime<=city_time[i].EndTime && city_time[i].SaleArea==addetailid_city){
            end_arry = {
              ADDetailID:city_time[i].ADDetailID,
              SaleArea:city_time[i].SaleArea,
              HySalePrice:city_time[i].HySalePrice,
              SalePrice:city_time[i].SalePrice,
              BeginTime:city_time[i].BeginTime,
              EndTime: city_time[i].EndTime,
              ADStyle:city_time[i].ADStyle,
              CarouselNumber:city_time[i].CarouselNumber,
              SalePlatform:city_time[i].SalePlatform,
              SaleType:city_time[i].SaleType,
              GroupType:city_time[i].GroupType
            }
          }
        }
      }
      //选择时间后重新选择样式
      // put_style2();
      console.log(SaleArea);
      //跨刊例的情况改变价格部分
      if(!$.isEmptyObject(begin_arry) && !$.isEmptyObject(end_arry)){
         if(compare(begin_arry.EndTime,end_arry.BeginTime)>=1){//跨多个刊例
              ADDetailID.push(
                {
                  ADDetailID:begin_arry.ADDetailID,
                  shop_begintime:shop_begintime,
                  shop_endtime:begin_arry.EndTime,
                  HySalePrice:begin_arry.HySalePrice,
                  SalePrice:begin_arry.SalePrice,
                }
              );
              ADDetailID.push(
                {
                  ADDetailID:end_arry.ADDetailID,
                  shop_begintime:end_arry.BeginTime,
                  shop_endtime: shop_endtime,
                  HySalePrice:end_arry.HySalePrice,
                  SalePrice:end_arry.SalePrice,
                }
              )
              for(var i=0;i<city_time.length;i++){
                if(add_date(begin_arry.EndTime)==city_time[i].BeginTime && city_time[i].EndTime<shop_endtime && city_time[i].SaleArea==addetailid_city){
                  var begin_arry={
                    ADDetailID:city_time[i].ADDetailID,
                    SaleArea:city_time[i].SaleArea,
                    HySalePrice:city_time[i].HySalePrice,
                    SalePrice:city_time[i].SalePrice,
                    BeginTime:city_time[i].BeginTime,
                    EndTime: city_time[i].EndTime,
                  }
                  ADDetailID.push({
                    ADDetailID:city_time[i].ADDetailID,
                    shop_begintime:city_time[i].BeginTime,
                    shop_endtime: city_time[i].EndTime,
                    HySalePrice:city_time[i].HySalePrice,
                    SalePrice:city_time[i].SalePrice
                  })

                }
              }
              //取所有的时间段里面价格最小的
              for(var n=0;n<ADDetailID.length;n++){
                if(ADDetailID[n].HySalePrice<min1){
                  min1 = ADDetailID[n].HySalePrice;
                }
                if(ADDetailID[n].SalePrice<min2){
                  min2 = ADDetailID[n].SalePrice;
                }
              }
              //渲染页面的价格变化
              if(min1!=0 && min1<min2){
                $("#price em").html("￥"+min1);
              }else if(min1!=0 && min1>min2){
                  $("#price em").html("￥"+min2);
              }else {
                $("#price em").html("￥"+min2);
              }
         }
       }
      }
  }
  //渲染购物车中的数量问题
  function render_shopcount(){
    //页面加载首先取得购物车中已经存在的数量，渲染到日历控件中去
    $.ajax({
      url:"/api/ShoppingCart/GetInfo_ShoppingCart?v=1_1",
      type:"get",
      async: true,
      dataType: 'json',
      xhrFields:{
        withCredentials:true
      },
      crossDomain:true,
      success:function(data){
      selectedPoint.splice(0,selectedPoint.length);
      console.log(data);
      if(data.Result!=null){
        for(var i=0;i<data.Result.APP.length;i++){
          for(var j=0;j<data.Result.APP[i].Medias.length;j++){
            for(var k=0;k<ADDetailID_city.length;k++){
              if(data.Result.APP[i].Medias[j].MediaID==MediaID && data.Result.APP[i].Medias[j].TemplateID == TemplateID &&data.Result.APP[i].Medias[j].PublishDetailID == ADDetailID_city[k]){
                for(var k=0;k<data.Result.APP[i].Medias[j].ADSchedule.length;k++){
                  selectedPoint = selectedPoint.concat(getall(data.Result.APP[i].Medias[j].ADSchedule[k].BeginData,data.Result.APP[i].Medias[j].ADSchedule[k].EndData));
                }
              }
            }

          }
        }
      }
      console.log(selectedPoint);
      render_time();
    }
  })
  }
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
  //将时间戳转换为日期格式2017-05-06
    Date.prototype.format = function (fmt) {
        var o = {
            "M+": this.getMonth() + 1,                 //月份
            "d+": this.getDate(),                    //日
            "h+": this.getHours(),                   //小时
            "m+": this.getMinutes(),                 //分
            "s+": this.getSeconds(),                 //秒
            "q+": Math.floor((this.getMonth() + 3) / 3), //季度
            "S": this.getMilliseconds()             //毫秒
        };
        if (/(y+)/.test(fmt)) {
            fmt = fmt.replace(RegExp.$1, (this.getFullYear() + "").substr(4 - RegExp.$1.length));
        }
        for (var k in o) {
            if (new RegExp("(" + k + ")").test(fmt)) {
                fmt = fmt.replace(RegExp.$1, (RegExp.$1.length == 1) ? (o[k]) : (("00" + o[k]).substr(("" + o[k]).length)));
            }
        }
        return fmt;
    }
    //比较时间大小
    function compare(timeEnd, timeNext) {
        var date1 = new Date(timeEnd)
        var date2 = new Date(timeNext)

        var s1 = date1.getTime(), s2 = date2.getTime();
        var total = (s2 - s1) / 1000;
        var day = parseInt(total / (24 * 60 * 60));//计算整数天数
        return day;
    }
    //时间减一天
    function reduce_date(time) {
        time=time.split(' ')[0]
        var date1 = new Date(time);
        var s1 = date1.getTime();
        var day = parseInt(s1 - (24 * 60 * 60 * 1000));//计算整数天数
        var str1 = new Date(day).format("yyyy-MM-dd");
        return str1;
    }

    //时间加一天
    function add_date(time) {
        time=time.split(' ')[0]
        var date1 = new Date(time);
        var s1 = date1.getTime();
        var day = parseInt(s1 + (24 * 60 * 60 * 1000));//计算整数天数
        var str1 = new Date(day).format("yyyy-MM-dd");
        return str1;
    }
    //渲染并操作收藏拉黑的操作
    function collection(){
      if($(".collection").length != 0){
            if ($(".collection").html().trim() == "收藏") {
                $(".collection").css("background", "url(/images/icon72_h.png) no-repeat 0 0px");
            } else {
                $(".collection").css("background", "url(/images/icon72.png) no-repeat 0 0px");
            }
            if ($(".pull").html().trim() == "拉黑") {
                $(".pull").css("background", "url(/images/icon73_h.png) no-repeat 0 0px");
            } else {
                $(".pull").css("background", "url(/images/icon73.png) no-repeat 0 0px");
                $("#zhezhao").show();
                $(".box_cart span").eq(0).css("background-color", "grey");
            }
            // 操作收藏拉黑按钮
            if($(".pull").html().trim() == "拉黑"){
              $(".collection").off("click").on("click", function () {
                if ($(".collection").html().trim() == "收藏") {
                  $(".collection").html("已收藏");
                  $(".collection").css("background", "url(/images/icon72.png) no-repeat 0 0px");
                  setAjax({
                    url: "/api/CollectPullBack/add",
                    type: "post",
                    data: {
                      businesstype: 14002,
                      operatetype: 1,
                      mediaId: MediaID
                    }
                  }, function (data) {
                    console.log(data);
                    $(".pull").off("click");
                  })
                } else {
                  $(".collection").html("收藏");
                  $(".collection").css("background", "url(/images/icon72_h.png) no-repeat 0 0px");
                  setAjax({
                    url:"/api/CollectPullBack/Remove",
                    type:"post",
                    data:{
                      businesstype:14002,
                      mediaId:MediaID,
                      operatetype:1
                    }
                  }, function (data) {
                    console.log(data);
                    $(".pull").off("click").on("click", function () {
                      if ($(".pull").html().trim() == "拉黑") {
                        layer.confirm("确定拉黑吗？", {
                          time: 0,
                          btn: ["确认", "取消"],
                          yes: function () {
                            $(".pull").css("background", "url(/images/icon73.png) no-repeat 0 0px");
                            setAjax({
                              url: "/api/CollectPullBack/add",
                              type: "post",
                              data: {
                                businesstype: 14002,
                                operatetype: 2,
                                mediaId: MediaID
                              }
                            }, function (data) {
                              console.log(data);
                              $(".pull").html("已拉黑");
                              layer.msg("已拉黑", {time: 1000});
                              $("#zhezhao").show();
                              $(".box_cart span").eq(0).css("background-color", "grey");
                              $(".collection").off("click");
                            })
                          }
                        });

                      }
                    })
                  })
                }
              })
            }
            if($(".collection").html().trim() == "收藏"){
              $(".pull").off("click").on("click", function () {
                if ($(".pull").html().trim() == "拉黑") {
                  layer.confirm("确定拉黑吗？", {
                    time: 0,
                    btn: ["确认", "取消"],
                    yes: function () {
                      $(".pull").css("background", "url(/images/icon73.png) no-repeat 0 0px");
                      setAjax({
                        url: "/api/CollectPullBack/add",
                        type: "post",
                        data: {
                          businesstype: 14002,
                          operatetype: 2,
                          mediaId: MediaID
                        }
                      }, function (data) {
                        console.log(data);
                        $(".pull").html("已拉黑");
                        layer.msg("已拉黑", {time: 1000});
                        $("#zhezhao").show();
                        $(".box_cart span").eq(0).css("background-color", "grey");
                        $(".collection").off("click");
                      })
                    }
                  });

                }
              })
            }
          }
    }
    //渲染案例展示页面
    function show(){
      //案例展示页面
        $("#case").on("click", function () {
            $("#case").addClass('active');
            $("#basic").removeClass('active');
            $("#case_div").show();
            $("#basic_div").hide();
            setAjax({
                url: "/api/Media/SelectMediaCaseInfo?v=1_1&MediaType=14002&MediaID=" + MediaID + "&CaseStatus=1"
            }, function (data) {
                console.log(data);
                //填充案例页面
                if (data.Result.length!=0&&data.Result[0].CaseContent) {
                    $("#case_div").html(data.Result[0].CaseContent);
                    if($("#case_div").html().trim().length==0){
                        $("#case").hide();
                    }
                }
            })
        });
        setAjax({
            url: "/api/Media/SelectMediaCaseInfo?v=1_1&MediaType=14002&MediaID=" + MediaID + "&CaseStatus=1"
        }, function (data) {
            console.log(data);
            //填充案例页面
            if (data.Result.length==0||data.Result[0].CaseContent=="") {
                    $("#case").hide();
            }
        })
        $("#basic").on("click", function () {
            $("#basic").addClass('active');
            $("#case").removeClass('active');
            $("#case_div").hide();
            $("#basic_div").show();
        });
    }



    //渲染相似推荐部分
    $.ajax({
          url: "/api/Publish/GetRecommendAD?v=1_1",
          type: "get",
          data: {
              MediaType:14002,
              TemplateID:TemplateID,
              MediaID: MediaID
          },
          dataType: 'json',
          xhrFields: {
              withCredentials: true
          },
          crossDomain: true,
          success:function (data) {
          console.log(data);
          if(data.Result&&data.Result.length!=0){
              $(".recommend").html(ejs.render($("#similar").html(), {data: data.Result}));
              $(".recommend div").on("click", function () {
                  MediaID = $(this).find(".MediaID").val();
                  TemplateID = $(this).find(".TemplateID").val();

                      window.location = "/OrderManager/app_detail.html" + "?MediaID=" + MediaID+"&TemplateID="+TemplateID;

              })
          }
      }
    })

    //处理大小
  	$(".mui-mbar-tabs").css("width","40px");
    //渲染购物车的数量
    setAjax({
      url:"/api/ShoppingCart/GetInfo_ShoppingCart?v=1_1",
      type:"get"
    },function(data){
      console.log(data);
      if(data.Status==0){
        if(data.Result){
        var shopcount1 = 0;
				var shopcount2 = 0;
				for(var i=0;i<data.Result.SelfMedia.length;i++){
					for(var j=0;j<data.Result.SelfMedia[i].Medias.length;j++){
						shopcount1++;
					}
				}
				for(var i=0;i<data.Result.APP.length;i++){
          for(var j=0;j<data.Result.APP[i].Medias.length;j++){
            shopcount2++;
          }
        }
				$('.cart_num').html(shopcount1+shopcount2);
      }
    }
    })
    //跳转到购物车页面
    $("#quick_links").on("click",function(){
      if($('.cart_num').html()>0){
        window.location = "/OrderManager/shopcartForMedia01.html";
      }else{
        layer.msg("请添加广告位！",{time:2000});
      }
    })






})


//将时间戳转换为日期格式2017-05-06
  Date.prototype.format = function (fmt) {
      var o = {
          "M+": this.getMonth() + 1,                 //月份
          "d+": this.getDate(),                    //日
          "h+": this.getHours(),                   //小时
          "m+": this.getMinutes(),                 //分
          "s+": this.getSeconds(),                 //秒
          "q+": Math.floor((this.getMonth() + 3) / 3), //季度
          "S": this.getMilliseconds()             //毫秒
      };
      if (/(y+)/.test(fmt)) {
          fmt = fmt.replace(RegExp.$1, (this.getFullYear() + "").substr(4 - RegExp.$1.length));
      }
      for (var k in o) {
          if (new RegExp("(" + k + ")").test(fmt)) {
              fmt = fmt.replace(RegExp.$1, (RegExp.$1.length == 1) ? (o[k]) : (("00" + o[k]).substr(("" + o[k]).length)));
          }
      }
      return fmt;
  }
/*获取两个时间之间的日期,调用函数getAll,注：包括开始日期和结束日期*/
function getall(start_time,end_time) {
  var bd = new Date(start_time),be = new Date(end_time);
  var bd_time = bd.getTime(), be_time = be.getTime(),time_diff = be_time - bd_time;
  var d_arr = [];
  for(var i=0; i<= time_diff; i+=86400000){
      var ds = new Date(bd_time+i).format('yyyy-MM-dd');
      d_arr.push(ds);
  }
  return d_arr;
}
