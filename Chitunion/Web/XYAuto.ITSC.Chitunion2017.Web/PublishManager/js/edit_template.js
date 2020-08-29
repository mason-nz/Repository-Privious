/*
* Written by:     zhanglp
* function:       修改模版效果js库
* Created Date:   2017-06-13
* Modified Date:  2017-06-29
* 说明：模板的修改分为两个入口：
* 1.单纯的在添加价格的页面进来的修改（本质上是添加，渲染页面用AdTempId），此时传的OperateType应该是1
* 2.在模板审核页面进来的修改（本质上是修改，渲染页面用AdBaseTempId），此时的传递的OperateType应该是2
*/
$(function(){
  //获取页面的参数
  var userData = GetUserId();
  var MediaID = parseInt(userData.hasOwnProperty("MediaID") ? userData["MediaID"] : -2);
  var BaseMediaId = parseInt(userData.hasOwnProperty("BaseMediaID") ? userData["BaseMediaID"] : -2);
  var AdTempId = parseInt(userData.hasOwnProperty("AdTempId") ? userData["AdTempId"] : -2);
  var TemplateID = parseInt(userData.hasOwnProperty("TemplateID") ? userData["TemplateID"] : -2);//模板id
  var AdBaseTempId = parseInt(userData.hasOwnProperty("AdBaseTempId") ? userData["AdBaseTempId"] : -2);//父类模板id
  var PubID = parseInt(userData.hasOwnProperty("PubID") ? userData["PubID"] : 0);
  var that;
  //定义传参变量
  var AdTemplateName,
      AdForm,
      AdTempStyle=[],
      AdTempStyleNew=[],
      CarouselCount = -2,
      SellingPlatform,
      SellingMode,
      AdSaleAreaGroup,
      OriginalFile,
      AdLegendURL,
      AdDisplay="",
      AdDescription="",
      Remarks="",
      AdDisplayLength;
  //公共模板中已经存在的样式
  var existing_style=[],
  //公共模板中已经存在的城市组
      add_city,
  //新增城市组集合
      add_city;
  //确定在最后要传的城市
    var city_selecting=[];
    //其他城市字符串
    var qita_val = "";
    //保存删除的城市的变量
    var delete_city = [];
  //渲染页面结构
  setAjax({
    url:"/api/Template/GetInfo",
    type:"get",
    data:{
      //默认值是15000
        "BusinessType":15000,
        "AdBaseTempId":AdBaseTempId,
        "AdTempId": TemplateID
    }
  },function(data){
    if(data.Result==null){
      layer.msg("URL地址不正确!",{time:2000});
      window.location = "/publishmanager/advertisinglist_app.html";
    }
    console.log(data);
    //渲染页面信息
    $(".order_r").html(ejs.render($("#info_basic").html(),{data: data.Result}));
    AdTemplateName = data.Result.AdTemplateName;
    //判断广告形式样式
    //这部分调取接口显示数据
     AdForm = data.Result.AdForm;
    setAjax({
      url:"/api/DictInfo/GetDictInfoByTypeID",
      type:"get",
      data:{
        typeID:51
      }
    },function(data){
        console.log(data);
        $("#radio").html(ejs.render($("#adtype").html(),{data:data.Result}));

        for(var i=0;i<$("#radio input").length;i++){
          if($($("#radio input")[i]).attr("id")==AdForm){
            $("#radio input")[i].checked = true;
          }else{
            $("#radio input")[i].disabled = "disabled";
          }
        }
    })

    //广告样式
    //首先把已经存在的值保存下来

    if($("#style_checkbox li").length==1){
      $("#style_checkbox").hide();
    }
    $(".close_click").on("click",function(){
      $(this).parents("li").remove();
      if($("#style_checkbox li").length==1){
        $("#style_checkbox").hide();
      }else{
        $("#style_checkbox").show();
      }
      checked();
    })
    //判断输入框的状态
  $("#style")
  .on("blur",function(){
      AdTempStyle = data.Result.AdTempStyle;
      // AdTempStyle
      //点击添加
      $("#button_style").off("click").on("click",function(){
        var style_val = $("#style").val();
        if(style_val==""){
          layer.msg("广告样式不能为空",{time:2000});
          return false;
        }
        for(var i=0;i<$(".ad_close1 span").length;i++){
          if(style_val==$($(".ad_close1 span")[i]).html()){
            layer.msg("广告样式不能重复",{time:2000});
            return false;
          }
        }
        var style_new = '<li ><input style="visibility:hidden;width:0px" class="add_style" name="2" type="checkbox" value="'+style_val+'" checked="checked"><div class="ad_close1"><span >'+style_val+'</span><img src="/ImagesNew/close_h1.png" class="close_click"></div></li>';
        $("#style_checkbox li").eq($("#style_checkbox li").length-1).after(style_new);
        if($(".add_style").length==0){
          $("#style_checkbox").hide();
        }else{
          $("#style_checkbox").show();
        }
        //鼠标移入标红
        $(".ad_close1").on("mouseenter",function(){
          $(".ad_close1").removeClass('red');
          $(this).addClass('red');
        })
        //离开时移除样式
        .on("mouseleave",function(){
          $(this).removeClass('red');
        });
        $("#style").val("");
        $(".close_click").on("click",function(){
          $(this).parents("li").remove();
          if($("#style_checkbox li").length==1){
            $("#style_checkbox").hide();
          }else{
            $("#style_checkbox").show();
          }
          checked();
        })
        checked();
        console.log(AdTempStyleNew);
        // tan();
      })
  })
  checked();
  // tan();
  //提取选中的复选框部分
function checked(){
  var style_input = $(".add_style");
  AdTempStyleNew=[];
  //建立一个标志
  var flag = true;
  for(var i=0;i<$(".add_style").length;i++){
      var val = $($(".add_style")[i]).val();
      if($($(".add_style")[i]).attr("BaseMediaID")){
        AdTempStyleNew.push({
          "BaseMediaID": $($(".add_style")[i]).attr("BaseMediaID"),
          "AdTemplateID": $($(".add_style")[i]).attr("AdTemplateID"),
          "AdStyleId": $($(".add_style")[i]).attr("AdStyleId"),
          "IsPublic": 0,
          "AdStyle": val
        })
      }else{
        AdTempStyleNew.push({
          "BaseMediaID": -2,
          "AdTemplateID": -2,
          "AdStyleId": -2,
          "IsPublic": 0,
          "AdStyle": val
        })
      }
  }
 }
    //点击复选框后面的span弹出一个框，可以修改内容
    function tan(){
      $("#style_checkbox span").not("public_style").off("click").on("click",function(){
        $(".layer_tan").show();
        that = $(this);
        console.log(that);
        //点击确定的时候(有点问题，后期看一下)
        $(".keepon").on("click",function(){
          that.html($(".tc input").val());
          // $(".tc input").val("");
          $(".layer_tan").hide();
        })
        //点击关闭的时候
        $(".close_style").on("click",function(){
          $(".tc input").val("");
          $(".layer_tan").hide();
        })
      })
     }
    //轮播数
    CarouselCount = data.Result.CarouselCount;
    var input_val = $("#location").val();
    $("#location")
    .on("focus",function(){
      // $("#location").val("");
      $(this).parent().next().html("");
    })
    .on("blur",function(){
      if($("#location").val()==""){
        // $("#location").val("请输入轮播的数字");
        $(this).parent().next().html("“1”表示没有轮播");
      }else{
        var reg = /^\+?[1-9]\d*$/;
        var val = $("#location").val();
        console.log(reg.test(val));
        //检测输入的内容的合法性
        if(val>20){
            $(this).parent().next().html("只能输入大于0小于20的数字并且比原来的数量大");
        }
        if(!reg.test(val) && input_val>val){
          $(this).parent().next().html("只能输入大于0小于20的数字并且比原来的数量大");
          // $("#location").val("");
        }else{
          CarouselCount = parseInt($("#location").val());
          console.log(CarouselCount);
        }
      }
    })
    if(AdBaseTempId==-2){
      $("#number").hide();
    }
    //售卖平台,没有值为undefined
    if(AdBaseTempId!=-2){
      for(var i=0;i<$("#platfrom_checkbox input").length;i++){
        if($("#platfrom_checkbox input")[i].checked == true){
          $($("#platfrom_checkbox input")[i]).attr("disabled","disabled");
        }
      }
    }
    SellingPlatform = data.Result.SellingPlatform;
    var platfrom_checkbox = $("#platfrom_checkbox input");
    platfrom_checkbox.on("change",function(){
      var array = [];
      for(var i=0;i<platfrom_checkbox.length;i++){
        if(platfrom_checkbox[i].checked==true){
          array.push(parseInt($(platfrom_checkbox[i]).val()));
        }
      }
      //有值得话做运算
      if(array.length){
      console.log(array);
      SellingPlatform = array[0]|array[1]|array[2]|array[3]|array[4]|array[5];
      console.log(SellingPlatform);
      }
    })
    //售卖方式
    if(AdBaseTempId!=-2){
      for(var i=0;i<$("#way_checkbox input").length;i++){
        if($("#way_checkbox input")[i].checked == true){
          $($("#way_checkbox input")[i]).attr("disabled","disabled");
        }
      }
    }
    SellingMode = data.Result.SellingMode;
    var way_checkbox = $("#way_checkbox input");
    way_checkbox.on("change",function(){
      var array = [];
      for(var i=0;i<way_checkbox.length;i++){
        if(way_checkbox[i].checked==true){
          array.push(parseInt(platfrom_checkbox[i].value));
        }
      }
      //有值得话做运算
      if(array.length){
      console.log(array);
      SellingMode = array[0]|array[1]|array[2];
      console.log(SellingMode);
      }
    })
    // console.log(SellingMode);
    //售卖区域
    //售卖区域
    //添加城市组
    //被驳回可以删除的城市组
    // var arry_city = [];

     //总的新增或者编辑的城市组集合
     AdSaleAreaGroup = [];
     //存放公共模板中已经存在的城市组
     existing_city=[];
     var existing_cityName = [];
     //新增城市组集合
     add_city=[];
     for(var i=0;i<data.Result.AdSaleAreaGroup.length;i++){
       if(data.Result.AdSaleAreaGroup[i].IsPublic==1){
         for(var j=0;j<data.Result.AdSaleAreaGroup[i].DetailArea.length;j++){
           if(data.Result.AdSaleAreaGroup[i].DetailArea[j].IsPublic==1){
             existing_cityName.push(data.Result.AdSaleAreaGroup[i].DetailArea[j].CityName);
           }
         }

       }
     }

     for(var i=0;i<data.Result.AdSaleAreaGroup.length;i++){
        if(data.Result.AdSaleAreaGroup[i].GroupName!="其他城市"&&data.Result.AdSaleAreaGroup[i].GroupName!="全国"){

          add_city.push(data.Result.AdSaleAreaGroup[i]);
        }

     }
    console.log(AdSaleAreaGroup)
    //首先把已经存在的数值放进页面中
    //城市加id集合
    var DetailArea = [];

    //点击删除城市的操作
    //点击删除新增加的城市的操作
    $(".close_img").on("click",function(){
      for(var i=0;i<add_city.length;i++){
        if($(this).prev().html()==add_city[i].GroupName){
          add_city.splice(i,1);
        }
      }
      $(this).parents("li").remove();
      opration_city();
      city_checked();
    })
    //点击删除被驳回的城市的时候
    //添加城市
    $('#location_add').off('click').on('click',function(){
      //城市列表的渲染跟操作
      opration_city();
      $(".layer_city").show();
      $("#zhezhao").show();
      //如果是公共的城市组则不修改名字
      $("#name").prop("disabled",false);

      $(".close").on("click",function(){
        $(".layer_city").hide();
        $("#zhezhao").hide();
      })
      $(".selectdCity ul").html("");
      $(".mt10 input").val("");
      //所有的城市集合
      console.log(JSonData.masterArea);
      // //城市列表的渲染跟操作
      // opration_city();
      //判断输入框的名字合法性
      $("#name").on("blur",function(){
        var val = $("#name").val();
        //城市组命名的时候不能重复
        for(var i=0;i<$(".insert_citygroup span").length;i++){
          if(val==$($(".insert_citygroup span")[i]).html()){
            layer.msg("城市组名字不能重复",{time:2000});
            $("#name").val("");
            $("#name").trigger("focus");
            return false;
          }
        }
      })
      //点击保存按钮
      $('.keep').off('click').on('click',function(){
          //每次新增的时候重置集合
          DetailArea = [];
          //点击保存操作
          //城市组命名部分
          var val = $("#name").val();
          if(val==""){
            layer.msg("请填写城市组名称",{time:2000});
            return false;
          }
          //城市组命名的判断
          var reg_location = /^([\u4e00-\u9fa5]|[0-9]|[a-z]|[A-Z]|\-|\_|\/){1,6}$/;
          if(reg_location.test(val)){
              // var DetailAreaObject = {};
              $('.selectdCity ul .num_name').each(function(index,item){
                  // console.log($(this));
                  DetailArea.push({
                      "IsPublic":0,
                      "ProvinceId": -2,
                      "ProvinceName": "",
                      "CityId": $(item).attr("city_id"),
                      "CityName": $(item).html()
                  })
              })
              console.log(DetailArea);
              //在页面中插入城市组名
              $("#location_checkbox li").eq($("#location_checkbox li").length-2).after('<li class="insert_citygroup"><input style="visibility:hidden" name="5" type="checkbox" value="'+val+'" checked="checked"><div class="ad_close1"><span class="city_group" style="cursor:pointer" >'+val+'</span><img src="/ImagesNew/close_h1.png" class="close_img"></div></li>');
              //删除重复命名
              for(var i=0;i<add_city.length;i++){
                if(val==add_city[i].GroupName){
                  add_city.splice(i,1);
                }
              }
              //加入大的城市组集合
              add_city.push({
                  "GroupId": -2,
                  "GroupType":1,
                  "IsPublic":0,
                  "GroupName": val,
                  "DetailArea": DetailArea
              })
              city_checked();
              console.log(add_city);
              console.log(city_selecting);
              console.log(existing_city);
                  // AdSaleAreaGroup = AdSaleAreaGroup.concat(add_city);
                  // console.log(AdSaleAreaGroup);
            }else{
                layer.msg("请输入汉字、数字、字母，特殊符号：“-”“_”“/”组成的名字，长度不超过6",{time:2000});
                return false;
              }
            //城市列表关闭
            $(".layer_city").hide();
            $("#zhezhao").hide();
            //点击删除城市的操作
            $(".close_img").on("click",function(){
              $(this).parents("li").remove();
              city_checked();
            })
            //点击再次编辑
            editor();
      })

  })
  city_checked();
  editor();
  //编辑城市
  //如果点击的公共模板中存在的城市组
  var GroupId,
      difference;
  function editor(){
    var that;
    $(".city_group").on("click",function(){
       that = $(this);
       if(that.attr("GroupId")){
         GroupId = that.attr("GroupId");
       }
      $(".layer_city").show();
      $("#zhezhao").show();
      $(".close").on("click",function(){
        $(".layer_city").hide();
        $("#zhezhao").hide();
      })
      $(".selectdCity ul").html("");
      $(".mt10 input").val(that.prev().val());
      // that.prev().val(val);
      // console.log(DetailArea);
      //对城市的渲染跟操作
      opration_city();
      console.log(add_city);
      //当点击编辑的时候把改城市组的信息带过来
      for(var i=0;i<add_city.length;i++){
              if(that.html()==add_city[i]["GroupName"]){
                for(var j=0;j<add_city[i]["DetailArea"].length;j++){
                  if(add_city[i]["DetailArea"][j].IsPublic==1){
                    $('.selectdCity ul').append('<li style="cursor:pointer;color:grey" class="name_public" city_id="'+add_city[i]["DetailArea"][j]["CityId"]+'">'+add_city[i]["DetailArea"][j]["CityName"]+'</li>');
                  }else{
                    $('.selectdCity ul').append('<li style="cursor:pointer;color:red" class="num_name" title="点击移除" city_id="'+add_city[i]["DetailArea"][j]["CityId"]+'">'+add_city[i]["DetailArea"][j]["CityName"]+'</li>');
                  }
                }
                //并给输入框赋值
                $("#name").val(add_city[i]["GroupName"]);
              }
          }

          //如果是公共的城市组则不修改名字
          if(that.attr("cityName")){
            $("#name").prop("disabled","disabled");
          }else{
            $("#name").prop("disabled",false);
          }
          // //判断输入框的名字合法性
          $("#name").on("blur",function(){
            var val = $("#name").val();
            //城市组命名的时候不能重复
            for(var i=0;i<$(".insert_citygroup span").length;i++){
              if(val==$($(".insert_citygroup span")[i]).html()){
                layer.msg("城市组名字不能重复",{time:2000});
                $("#name").val("");
                $("#name").trigger("focus");
                return false;
              }
            }
          })
            //编辑点击保存时的操作
            $('.keep').off('click').on('click',function(){
              //点击保存时覆盖原先数据
              //城市组命名部分
              var val = $("#name").val();
              if(val==""){
                layer.msg("请填写城市组名称",{time:2000});
                return false;
              }
              var reg_location = /^([\u4e00-\u9fa5]|[0-9]|[a-z]|[A-Z]|\-|\_|\/){1,6}$/;
              //城市组命名的判断
              if(reg_location.test(val)){
                //每次置空条件展现最新选择数据
                  DetailArea=[];
                  for(var i=0;i<add_city.length;i++){
                    if(val==add_city[i].GroupName && add_city[i].IsPublic==1){
                      for(var j=0;j<add_city[i].DetailArea.length;j++){
                        if(add_city[i].DetailArea[j].IsPublic==0){
                          add_city[i].DetailArea.splice(j,1);
                        }
                      }
                    }
                    if(val==add_city[i].GroupName && add_city[i].IsPublic!=1){
                      add_city.splice(i,1);
                    }
                  }
                  // AdSaleAreaGroup=[];
                  $('.selectdCity ul .num_name').each(function(index,item){
                      DetailArea.push({
                           "IsPublic":0,
                           "ProvinceId": -2,
                           "ProvinceName": "",
                           "CityId": $(item).attr("city_id"),
                           "CityName": $(item).html()
                      })
                  })
                  //加入大的城市组集合
                  if(GroupId){
                    add_city.push({
                      "GroupId": GroupId,
                      "GroupType":1,
                      "IsPublic":0,
                      "GroupName": val,
                      "DetailArea": DetailArea
                    })
                  }else{
                    add_city.push({
                      "GroupId": -2,
                      "GroupType":1,
                      "IsPublic":0,
                      "GroupName": val,
                      "DetailArea": DetailArea
                    })
                  }

                  console.log(add_city);
                  console.log(DetailArea);
                  //在页面中修改城市组名
                  that.parent().prev().val(val);
                  that.html(val);
              }else{
                layer.msg("请输入汉字、数字、字母，特殊符号：“-”“_”“/”组成的名字，长度不超过6",{time:2000});
                return false;
              }
              // AdSaleAreaGroup = AdSaleAreaGroup.concat(add_city);
              $(".layer_city").hide();
              $("#zhezhao").hide();
              //改变其中的city_selecting值
              city_checked();
            })

          })
  }
  //渲染跟操作城市
  function opration_city(){
    //已经选择的城市不显示
    console.log(add_city);
    $(".num_name_left").remove();
      for(var i=0;i<JSonData.masterArea.length;i++){
            for(var j=0;j<JSonData.masterArea[i].subArea.length;j++){
                var str = '<div style="cursor:pointer" class="sort_list" title="点击添加" szm="'+JSonData.masterArea[i].subArea[j].szm+'">'+'<div class="num_name_left" id="'+JSonData.masterArea[i].subArea[j].id+'">'+JSonData.masterArea[i].subArea[j].name+'</div>'+'</div>';
                $('.'+JSonData.masterArea[i].subArea[j].szm).after(str);
            }
      }
      // 移除相应的已经存在的城市
      for(var i=0;i<add_city.length;i++){
        for(var j=0;j<add_city[i].DetailArea.length;j++){
          for(var k=0;k<$(".num_name_left").length;k++){
            if(add_city[i].DetailArea[j].CityName==$($(".num_name_left")[k]).html()){
              $($(".num_name_left")[k]).remove();
            }
          }
        }
      }
      for(var i=0;i<existing_cityName.length;i++){
          for(var k=0;k<$(".num_name_left").length;k++){
            if(existing_cityName[i]==$($(".num_name_left")[k]).html()){
              $($(".num_name_left")[k]).remove();
            }
          }
      }
    //点击跳转到对应字母
    $('.word').off('click').on('click',function () {
        var id = $.trim($(this).attr('id'));
        $('.sort_box').animate({scrollTop: $("."+id).position().top+$(".sort_box").scrollTop()-183}, 200);
    })

    //点击左侧城市逻辑
    $('.sort_box').off('click').on('click','.num_name_left',function () {
        var that = $(this);
        that.addClass('red');
        var content = $.trim(that.html());
        var cityArr = [],cityStr = '';
        $('.selectdCity .num_name').each(function () {
            cityArr.push($.trim($(this).html()));
        });
        cityStr = cityArr.toString();
        if(cityStr.indexOf(content) == -1){
            $('.selectdCity ul').append('<li style="cursor:pointer;color:red" class="num_name" city_id="'+that.attr('id')+'" title="点击移除">'+content+'</li>')
        }
    });
    //点击右侧城市删除
    $('.selectdCity').off('click').on('click','.num_name',function () {
        var cityName = $.trim($(this).html());
        $(this).remove();
        $('.sort_box .num_name').each(function () {
            if($.trim($(this).html()) == cityName){
                $(this).removeClass('red');
            }
        });
    })
  }

  //判断城市组的选中的城市情况
  function city_checked(){
    city_selecting=[];
    qita_val="";
    var zhong = [];
    //判断新增城市组的名字长度，控制其他城市的显示隐藏
    if($(".insert_citygroup").length==0){
      $("#qita_city").hide();
      console.log($("#qita_city").not(":hidden").length);
    }else{
      $("#qita_city").show();
    }
    //检查页面上的所有选项
    for(var i=0;i<$("#location_checkbox input").not(":hidden").length;i++){
      for(var j=0;j<add_city.length;j++){
        if($($("#location_checkbox input")[i]).val()==add_city[j].GroupName && add_city[j].IsPublic!=1 && add_city[j].GroupName!="全国" && JSON.stringify(city_selecting).indexOf(JSON.stringify(add_city[j]))==-1){
          for(var z=0;z<city_selecting.length;z++){
            if(city_selecting[z].GroupName==add_city[j].GroupName){
              city_selecting.splice(z,1);
            }
          }
          city_selecting.push(add_city[j]);
        }else if($($("#location_checkbox input")[i]).val()==add_city[j].GroupName && add_city[j].IsPublic==1 && add_city[j].GroupName!="全国" && JSON.stringify(city_selecting).indexOf(JSON.stringify(add_city[j]))==-1){
          zhong=[];
          for(var k=0;k<add_city[j].DetailArea.length;k++){
            if(add_city[j].DetailArea[k].IsPublic!=1){
              zhong.push(add_city[j].DetailArea[k]);
            }
          }
          city_selecting.push({
            "GroupId": add_city[j].GroupId,
            "GroupType":1,
            "IsPublic":0,
            "GroupName": add_city[j].GroupName,
            "DetailArea": zhong
          });
        }
      }
      //显示其他城市
      if($($("#location_checkbox input")[i]).val()!="全国" && $($("#location_checkbox input")[i]).val()!="其他城市" && qita_val.indexOf($($("#location_checkbox input")[i]).val()) ==-1){
        qita_val = qita_val + $($("#location_checkbox input")[i]).val()+",";
        $("#qita_city span").eq(1).html('（除“'+qita_val.substring(0,qita_val.length-1)+'”以外的其他城市）');
      }
    }
      console.log(city_selecting);
      console.log(qita_val);
      //控制其他城市的显示隐藏
      if(qita_val!=""){
        $("#qita_city").show();
      }else{
        $("#qita_city").hide();
      }
  }
  //刊例上传原件
  //刊例原件和广告示例图上传
  var original_k = data.Result.OriginalFile,
      sample_img;
      var data1="",
          data2="",
          data3="";

  if(data.Result.AdLegendURL==null){
    sample_img="";
  }else{
    sample_img = data.Result.AdLegendURL+",";
    if(data.Result.AdLegendURL.split(",")[0]){
      data1 = data.Result.AdLegendURL.split(",")[0]+",";
    }else{
      data1="";
    }
    if(data.Result.AdLegendURL.split(",")[1]){
      data2 = data.Result.AdLegendURL.split(",")[1]+",";
    }else{
      data2="";
    }
    if(data.Result.AdLegendURL.split(",")[2]){
      data3 = data.Result.AdLegendURL.split(",")[2]+",";
    }else{
      data3="";
    }
  }
  $('#input1').parents('a').next().next().show().attr("href","" + data.Result.OriginalFile);
    if(data.Result.AdLegendURL){
      for(var i=0;i<data.Result.AdLegendURL.split(",").length;i++){
        $("#input"+(i+2)).parent().prev().attr("src",""+ data.Result.AdLegendURL.split(",")[i]);
      }
    }
  var input2=data1,
      input3=data2,
      input4=data3;
  uploadFile1("input1");
  //修改已经通过的模板，刊例原件项不显示
  if(CTLogin.RoleIDs=="SYS001RL00003" || CTLogin.RoleIDs=="SYS001RL00005"){
    if(AdBaseTempId>0){
      $("#original_name").html("");
      $("#success").hide();
      original_k = "";
    }
  }
  if(CTLogin.RoleIDs=="SYS001RL00004" || CTLogin.RoleIDs=="SYS001RL00001"){
    $("#kanli").css("visibility","hidden");
    $("#shili").css("visibility","visible");
    if(AdBaseTempId>0){
      $("#original").hide();
    }
    if(data.Result.OriginalFile==null){
      $("#success").hide();
    }
  }
  $(".uploadify").eq(0).css("height","20px");
  $(".swfupload").eq(0).css({"left":"0px","height":"20px","top":"-20px"})
  $(".uploadify-button").eq(0).css("height","20px");
  for(var i=2;i<$(".new_upload span").length+2;i++){
    uploadFile("input"+i);
    // console.log($(".new_upload").length+1);
    console.log(i);
  }
  $(".uploadify:gt(0)").css("height","68px");
  $(".swfupload:gt(0)").css({"left":"0px","height":"68px"})
  $(".uploadify-button:gt(0)").css("height","68px");

  /*上传原件*/
  function uploadFile1(id) {
     jQuery.extend({
         evalJSON: function (strJson) {
             if ($.trim(strJson) == '')
                 return '';
             else
                 return eval("(" + strJson + ")");
         }
     });
     function getCookie(name) {
         var arr, reg = new RegExp("(^| )" + name + "=([^;]*)(;|$)");
         if (arr = document.cookie.match(reg))
             return unescape(arr[2]);
         else
             return null;
     };
     function escapeStr(str) {
         return escape(str).replace(/\+/g, '%2B').replace(/\"/g, '%22').replace(/\'/g, '%27').replace(/\//g, '%2F');
     };


     $('#'+id).uploadify({
         'buttonText': '',
        //  'buttonClass': 'but_upload',
         'swf': '/Js/uploadify.swf?_=' + Math.random(),
         'uploader': '/AjaxServers/UploadFile.ashx',
         'auto': true,
         'multi': false,
         'width': 100,
         'height': 20,
         'formData': { Action: 'BatchImport', CarType: '', LoginCookiesContent: escapeStr(getCookie('ct-uinfo')) },
         'fileTypeDesc': '支持格式:xls,xlsx,jpg,jpeg,png.gif,zip,pdf,ppt,pptx,mp4',
         'fileTypeExts': '*.xls;*.xlsx;*.jpg;*.jpeg;*.png;*.gif;*.zip;*.pdf;*.ppt;*.pptx;*.mp4',
         'fileSizeLimit':'10MB',
         'queueSizeLimit': 1,
         'scriptAccess': 'always',
          queueID:'imgShow',
         'onQueueComplete': function (event, data) {
             //enableConfirmBtn();
         },
         'onQueueFull': function () {
             layer.alert('您最多只能上传1个文件！');
             return false;
         },
         'onUploadSuccess': function (file, data, response) {
             if (response == true) {
                 var json = $.evalJSON(data);
                 original_k = json.Msg;
                //  console.log(json.Msg);
                //  $('#'+id).parents('ul').next().show();
                 $('#'+id).parents('a').next().text(json.FileName);
                 $('#'+id).parents('a').next().next().show().attr("href","" + json.Msg);
                //  $("#tishi").show();
             }
         },
         'onProgress': function (event, queueID, fileObj, data) {},
         'onUploadError': function (event, queueID, fileObj, errorObj) {
             //enableConfirmBtn();
         },
         'onSelectError':function(file, errorCode, errorMsg){
             console.log(errorCode);
         }
     });

  };
  //图片部分上传
  function uploadFile(id) {
      jQuery.extend({
          evalJSON: function (strJson) {
              if ($.trim(strJson) == '')
                  return '';
              else
                  return eval("(" + strJson + ")");
          }
      });
      function getCookie(name) {
          var arr, reg = new RegExp("(^| )" + name + "=([^;]*)(;|$)");
          if (arr = document.cookie.match(reg))
              return unescape(arr[2]);
          else
              return null;
      };
      function escapeStr(str) {
          return escape(str).replace(/\+/g, '%2B').replace(/\"/g, '%22').replace(/\'/g, '%27').replace(/\//g, '%2F');
      };


      $('#'+id).uploadify({
          'buttonText': '',
          // 'buttonClass': 'button_add',
          'swf': '/Js/uploadify.swf?_=' + Math.random(),
          'uploader': '/AjaxServers/UploadFile.ashx',
          'auto': true,
          'multi': false,
          'width': 80,
          'height': 18,
          'formData': { Action: 'BatchImport', LoginCookiesContent: escapeStr(getCookie('ct-uinfo')), IsGenSmallImage: 1 }, //存在缩略图大图的格式
          // 'formData': { Action: 'BatchImport', CarType: '', LoginCookiesContent: escapeStr(getCookie('ct-uinfo')) },
          'fileTypeDesc': '支持格式:xls,xlsx,jpg,jpeg,png.gif',
          'fileTypeExts': '*.xls;*.xlsx;*.jpg;*.jpeg;*.png;*.gif;',
          'fileSizeLimit':'10MB',
          'queueSizeLimit': 1,
          'scriptAccess': 'always',
          'onQueueComplete': function (event, data) {},
          'fileCount':1,
          queueID:'imgShow',
          'scriptAccess': 'always',
          'overrideEvents' : [ 'onDialogClose'],
          'onQueueComplete': function (event, data) {},
          'onQueueFull': function () {
              layer.alert('您最多只能上传1个文件！');
              return false;
          },
          'onUploadSuccess': function (file, data, response) {
              if (response == true) {
                  var json = $.evalJSON(data);
                  if(id=="input2"){
                    input2 = json.Msg.split("|")[0]+",";
                  }
                  if(id=="input3"){

                    input3 = json.Msg.split("|")[0]+",";
                  }
                  if(id=="input4"){
                    input4 = json.Msg.split("|")[0]+",";
                  }
                  sample_img = input2+input3+input4;
                  // sample_img+=json.Msg+",";
                  var uploadShow = $("#"+id).parent().prev();
                  console.log(json);
                  // uploadShow.show();
                  // uploadShow.find(".uploadName").text(json.FileName);
                  // uploadShow.find(".uploadFile").attr("href","" + json.Msg);
                  uploadShow.attr("src",""+json.Msg.split("|")[0]);
              }
          },
          'onProgress': function (event, queueID, fileObj, data) {},
          'onUploadError': function (event, queueID, fileObj, errorObj) {},
          'onSelectError':function(file, errorCode, errorMsg){}
      });

  };
  //广告展示逻辑
  $("#show_logic").on("change",function(){
    if($("#show_logic").val().length>500){
      AdDisplay = $("#show_logic").val().substr(0,500);
      $("#show_span").show();
    }else{
      $("#show_span").hide();
      AdDisplay = $("#show_logic").val();
    }
  })
  AdDisplay = $("#show_logic").val();
  //素材说明
  $("#material").on("change",function(){
    if($("#material").val().length>500){
      AdDescription = $("#material").val().substr(0,500);
      $("#material_span").show();
    }else{
      $("#material_span").hide();
      AdDescription = $("#material").val();
    }
  })
  AdDescription = $("#material").val();
  //备注
  $("#mark").on("change",function(){
    if($("#mark").val().length>500){
      Remarks = $("#mark").val().substr(0,500);
      $("#mark_span").show();
    }else{
      Remarks = $("#mark").val();
      $("#mark_span").hide();
    }
  })
  Remarks = $("#mark").val();
  //起投天数
  var reg2 = /^[1-9]\d*|0$/;
  $("#count").on("change",function(){
    if(reg2.test($("#count").val()) && $("#count").val()<=365){
      $("#count_span").hide();
      AdDisplayLength = parseInt($("#count").val());
    }else{
      $("#count").val("");
      $("#count_span").show();
    }
  })
  if($("#count").val()==0){
    $("#count").val("");
  }
  AdDisplayLength = parseInt($("#count").val());

  // console.log(param);
  //判断所有必填项都填之后才能提交成功，否则提交不成功
  $("#add_price").on("click",function(){
    // console.log(param);
    console.log(1);
    console.log(AdTemplateName);
    console.log(AdForm);
    console.log(AdTempStyleNew);
    console.log(CarouselCount);
    console.log(SellingPlatform);
    console.log(SellingMode);
    console.log(city_selecting);
    console.log(original_k);
    console.log(sample_img.substring(0,sample_img.length-1));
    if(AdBaseTempId>0 && CarouselCount<data.Result.CarouselCount){
      layer.msg("只能修改大于当前的轮播数",{time:2000});
      $("#location").parent().next().html("只能修改大于当前的轮播数").show();
      return false;
    }
    if(AdTemplateName || AdForm ||CarouselCount||SellingPlatform||SellingMode||original_k){
      //掉接口传输json对象
      console.log(BaseMediaId);
      console.log(TemplateID);
      console.log(AdBaseTempId);
      //参数集合
      var param = {
            "BusinessType":15000,
            "OperateType": 1,
            "Temp": {
                "TemplateId":TemplateID, // 模板id
                "BaseAdId":AdBaseTempId, //套用的模板id
                "BaseMediaId":BaseMediaId, //基表媒体ID
                "MediaID": MediaID,//附表媒体ID
                "AdTemplateName": AdTemplateName, //模板名称
                "OriginalFile": original_k,
                "AdForm": AdForm,
                "CarouselCount": CarouselCount,
                "SellingPlatform": SellingPlatform,
                "SellingMode": SellingMode,
                "AdLegendURL": sample_img.substring(0,sample_img.length-1),
                "AdDisplay": AdDisplay,
                "AdDescription": AdDescription,
                "Remarks": Remarks,
                "AdDisplayLength": AdDisplayLength,
                "AdTempStyle": AdTempStyleNew,
                "AdSaleAreaGroup": city_selecting
            }
        }
      //说明中的第一种情况
      if(TemplateID==-2){
        param.OperateType = 1;
      }
      //说明中的第二种情况
      if(AdBaseTempId!=-2){
        param.Temp.TemplateId = -2;
        param.Temp.AdBaseTempId = AdBaseTempId;
        param.OperateType = 1;
      }
      if(TemplateID!=-2){
        param.Temp.TemplateId = TemplateID;
        param.Temp.AdBaseTempId = AdBaseTempId;
        param.OperateType = 2;
      }
      console.log(param);
      setAjax({
        url:"/api/Template/curd",
        type:"post",
        data:param
      },function(data){
        console.log(data);
        if(data.Status==0){
          layer.msg("保存成功",{time:2000});
          var TemplateID = data.Result.AdTemplateId;
          if(CTLogin.RoleIDs=="SYS001RL00003" || CTLogin.RoleIDs=="SYS001RL00005"){
            // window.location= "/PublishManager/addPublishForApp.html?MediaID="+MediaID+"&PubID="+PubID+"&TemplateID="+TemplateID;
            window.location = "/publishmanager/advertisinglist_app.html";
          }else if(CTLogin.RoleIDs=="SYS001RL00001" || CTLogin.RoleIDs=="SYS001RL00004"){
            window.location="/publishmanager/advtempauditlist.html";
          }

        }else{
          layer.msg(data.Message,{time:2000});
        }
      })
    }else{
      layer.msg("请修改之后提交",{time:2000});
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
// 通过url获取文件名称
function getFileName(o){
    if(o == null) return '';
    var pos=o.lastIndexOf("/");
    var str = o.substring(pos+1);
    var pos1 = str.indexOf('$');
    return str.substr(0,pos1);
}
