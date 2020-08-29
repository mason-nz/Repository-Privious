/*
* Written by:     zhanglp
* function:       新增模版效果js库
* Created Date:   2017-06-5
* Modified Date:  2017-06-29
*/
$(function(){
  //获取页面的参数
  var userData = GetUserId();
  var MediaID = userData.hasOwnProperty("MediaID") ? userData["MediaID"] : -2;
  var BaseMediaID = userData.hasOwnProperty("BaseMediaID") ? userData["BaseMediaID"] : -2;
  var AppName = userData.hasOwnProperty("AppName") ? decodeURIComponent(userData["AppName"]) : "";
  var PubID = userData.hasOwnProperty("PubID") ? userData["PubID"] : 0;
  var TemplateID = userData.hasOwnProperty("TemplateID") ? userData["TemplateID"] : 0;
  $("#AppName").html(AppName);
  console.log(BaseMediaID);
  console.log(AppName);
  if(BaseMediaID==-2 || BaseMediaID=="" || BaseMediaID == undefined || AppName=="" || AppName=="undefined" ){
    layer.msg("URL地址不正确!",{time:2000});
    window.location = "/publishmanager/advertisinglist_app.html";
  }

  //定义传参变量
  var AdTemplateName,
      AdForm,
      AdTempStyle=[],
      CarouselCount,
      SellingPlatform,
      SellingMode,
      AdSaleAreaGroup,
      OriginalFile,
      AdLegendURL="",
      AdDisplay="",
      AdDescription="",
      Remarks="",
      AdDisplayLength=0;
    //确定最后要传的集合
    var city_selecting=[];
    //其他城市字符串
    var qita_val = "";
    //验证广告名称
    var reg_name = /^([\u4e00-\u9fa5]|[0-9]|[a-z]|[A-Z]|\-|\_|\/){1,20}$/;
    $("#uname")
    .blur(function(){
      if(reg_name.test($.trim($("#uname").val()))){
        $("#name2").hide();
        AdTemplateName = $.trim($("#uname").val());
        console.log(AdTemplateName)
        //调取接口验证名称是否已经存在
        setAjax({
          url:"/api/Template/VerifyAdTemplateName",
          type:"post",
          data:{
            BaseMediaId:BaseMediaID,
            MediaId:MediaID,
            OperateType:1,
            AdTempName:AdTemplateName
          }
        },function(data){
          console.log(data);
          // var AdTempId = ;
          if(data.Status){
            var AdTemplateId = data.Result.AdTemplateId;
            if(AdTemplateId && CTLogin.RoleIDs!="SYS001RL00004"&&CTLogin.RoleIDs!="SYS001RL00001"){
              //如果已经存在显示信息，点击跳转到添加价格页面
              $('#alert').show();
              $('#alert a').attr("href","/PublishManager/addPublishForApp.html?MediaID="+MediaID+"&PubID=0&TemplateID="+AdTemplateId);
            }else if(CTLogin.RoleIDs=="SYS001RL00004"||CTLogin.RoleIDs=="SYS001RL00001"){
              $("#yunying").show();
            }
          }else{
            $('#alert').hide();
            $("#yunying").hide();
          }
        })
      }else{
        $("#name2").show();
      }
    })
    //广告形式(单选)
    //这部分调取接口显示数据
    setAjax({
      url:"/api/DictInfo/GetDictInfoByTypeID",
      type:"get",
      data:{
        typeID:51
      }
    },function(data){
        console.log(data);
        $("#radio").html(ejs.render($("#adtype").html(),{data:data.Result}));

        $("#radio input").on("click",function(){
          $('#alert_form').hide();
          for(var i=0;i<$("#radio input").length;i++){
            if($("#radio input")[i].checked==true){
              AdForm = parseInt($("#radio input").eq(i).attr("id"));
              console.log(AdForm);
            }
          }
        })
    })
    //广告样式，看有没有选中的复选框
      //首先判断输入框的状态
        // AdTempStyle
        //点击添加
        $("#button_style").off("click").on("click",function(){
          var style_val = $("#style").val();
          if(style_val==""){
            $("#alert_style").show();
            return false;
          }
          for(var i=0;i<$("#style_checkbox input").length;i++){
            if($($("#style_checkbox input")[i]).val()==style_val){
              layer.msg("广告样式的名字不能重复",{time:2000});
              return false;
            }
          }
          $("#alert_style").hide();
          var style_new = '<li><input name="2" class="add_style" type="checkbox" value="'+style_val+'" checked="checked" style="visibility:hidden;width:0px"><div class="ad_close1"><span>'+style_val+'</span><img src="/ImagesNew/close_h1.png" class="close_click"></div></li>';
          $("#style_checkbox").show();
          $("#style_checkbox li").eq($("#style_checkbox li").length-1).after(style_new);
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
            if($(".add_style").length==0){
              $("#style_checkbox").hide();
            }
            checked();
          })

          checked();
          console.log(AdTempStyle);
        })
    checked();
    //提取选中的复选框部分
  function checked(){
    var style_input = $("#style_checkbox input");
    style_input.on("change",function(){
      AdTempStyle=[];
      //建立一个标志
      var flag = true;
      for(var i=0;i<$(".add_style").length;i++){
        if($(".add_style")[i].checked==true){
          var val = $($(".add_style")[i]).val();
          for(var j=0;j<AdTempStyle.length;j++){
            if(AdTempStyle[j].AdStyle==val){
              flag = false
            }
          }
          if(flag){
            AdTempStyle.push({
                   "BaseMediaID": -2,
                   "AdTemplateID": 1,
                   "AdStyleId": 0,
                   "IsPublic": 0,
                   "AdStyle": val
                 })
            }
        }
      }
      console.log(AdTempStyle);
    })
    //建立一个标志
    var flag = true;
    AdTempStyle=[];
    for(var i=0;i<$(".add_style").length;i++){
      flag = true;
      if($(".add_style")[i].checked==true){
        var val = $($(".add_style")[i]).val();
        for(var j=0;j<AdTempStyle.length;j++){
          if(AdTempStyle[j].AdStyle==val){
            flag = false
          }
        }
        if(flag){
          AdTempStyle.push({
                 "BaseMediaID": -2,
                 "AdTemplateID": -2,
                 "AdStyleId": -2,
                 "IsPublic": 0,
                 "AdStyle": val
            })
          }
        }
      }
  }

  //轮播数位置,没有值为空

  $("#location")
  .on("input",function(){
    var reg = /^\+?[1-9]\d*$/;
    var val = $("#location").val();
    if(!reg.test(val)){
      $("#location").val(val.substr(0,0));
    }else if(val>20){
      $("#alert_shuffling").html("数量不能大于20");
      $("#alert_shuffling").show();
    }
  })
  .on("blur",function(){
  //   if($("#location").val()==""){
  //     $("#location").val("请输入轮播的数字");
  //     $(this).parent().next().html("“1”表示没有轮播");
  //   }else{
      var reg = /^\+?[1-9]\d*$/;
      var val = $("#location").val();
      console.log(reg.test(val));
      //检测输入的内容的合法性
      if(!reg.test(val)){
        $("#alert_shuffling").show();
        // $("#location").trigger('focus');
        // $("#location").val("");
      }else{
        $("#alert_shuffling").hide();
        CarouselCount = parseInt($("#location").val());
        console.log(CarouselCount);
      }
  //   }
  })
  //售卖平台,没有值为undefined
  var platfrom_checkbox = $("#platfrom_checkbox input");
  platfrom_checkbox.on("change",function(){
    $("#alert_platform").hide();
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

  //售卖区域
  //添加城市组
  //总的城市组集合
  AdSaleAreaGroup = [];
  //城市加id集合
  var DetailArea = [];
        // AdSaleAreaGroup.push({
        //     "GroupId": -2,
        //     "GroupType":-1,
        //     "IsPublic":0,
        //     "GroupName": "其他城市",
        //     "DetailArea": [
        //         {
        //                 "ProvinceId": -2,
        //                 "ProvinceName": "",
        //                 "CityId": -2,
        //                 "CityName": ""
        //         }
        //     ]
        // })
    //添加城市
    $('#location_add').off('click').on('click',function(){
      $(document).scrollTop();
      $(".layer_city").css({"top":$(document).scrollTop()+50+"px","left":"400px"});
      $(".layer_city").show();
      $("#zhezhao").show();

      $(".close").on("click",function(){
        $(".layer_city").hide();
        $("#zhezhao").hide();
      })
      $(".selectdCity ul").html("");
      $(".mt10 input").val("");
      //所有的城市集合
      console.log(JSonData.masterArea);
      //城市列表的渲染跟操作
      opration_city();
      //判断输入框的名字合法性
      $("#name").on("blur",function(){
        var val = $("#name").val();
        //城市组命名的时候不能重复
        for(var i=0;i<city_selecting.length;i++){
          if(val==city_selecting[i].GroupName){
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
              if($('.selectdCity ul li').length==0){
                layer.msg("城市不能为空",{time:2000});
                return false;
              }
              $('.selectdCity ul li').each(function(index,item){
                  // console.log($(this));
                  DetailArea.push({
                      "IsPublic":0,
                      "ProvinceId": -2,
                      "ProvinceName": "",
                      "CityId": $(item).attr("city_id"),
                      "CityName": $(item).html()
                  })
              })
              // console.log(DetailArea);
              //在页面中插入城市组名
              $("#location_checkbox li").eq($("#location_checkbox li").length-2).after('<li class="insert_citygroup"><input name="5" type="checkbox" value="'+val+'" checked="checked" style="visibility:hidden"><div class="ad_close1"><span class="city_group" style="cursor:pointer">'+val+'</span><img src="/ImagesNew/close_h1.png" class="close_img" style="cursor:pointer"></div></li>');
              //鼠标移入标红
              $(".ad_close1").on("mouseenter",function(){
                $(".ad_close1").removeClass('red');
                $(this).addClass('red');
              })
              //离开时移除样式
              .on("mouseleave",function(){
                $(this).removeClass('red');
              });
              for(var i=0;i<AdSaleAreaGroup.length;i++){
                if(val==AdSaleAreaGroup[i].GroupName){
                  AdSaleAreaGroup.splice(i,1);
                }
              }
              //加入大的城市组集合
              AdSaleAreaGroup.push({
                  "GroupId": -2,
                  "GroupType":1,
                  "IsPublic":0,
                  "GroupName": val,
                  "DetailArea": DetailArea
              })
          }else{
            layer.msg("请输入汉字、数字、字母，特殊符号：“-”“_”“/”组成的城市组名字，长度不超过6",{time:2000});
            // layer.msg("请填写城市组名称",{time:2000});
            return false;
          }
          //城市列表关闭
          $(".layer_city").hide();
          $("#zhezhao").hide();
          //点击再次编辑
          editor();
          //点击删除城市的操作
          $(".close_img").on("click",function(){
            $(this).parents("li").remove();
            city_checked();
          })
          city_checked();
          console.log(AdSaleAreaGroup);
          console.log(city_selecting);
      })
    })
    editor();
    city_checked();
    //编辑城市
    function editor(){
      var that;
      //点击添加的城市组
      $(".city_group").on("click",function(){
         that = $(this);
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
            // console.log(AdSaleAreaGroup);
            // //当点击编辑的时候把改城市组的信息带过来
            for(var i=0;i<city_selecting.length;i++){
              if(that.html()==city_selecting[i]["GroupName"]){
                for(var j=0;j<city_selecting[i]["DetailArea"].length;j++){
                  $('.selectdCity ul').append('<li class="num_name" style="cursor:pointer" title="点击移除" city_id="'+city_selecting[i]["DetailArea"][j]["CityId"]+'">'+city_selecting[i]["DetailArea"][j]["CityName"]+'</li>');
                }
                //并给输入框赋值
                $("#name").val(city_selecting[i]["GroupName"]);
              }
            }
            //判断输入框的名字合法性
            $("#name").on("blur",function(){
              var val = $("#name").val();
              //城市组命名的时候不能重复
              for(var i=0;i<city_selecting.length;i++){
                if(val==city_selecting[i].GroupName){
                  layer.msg("城市组名字不能重复",{time:2000});
                  $("#name").val("");
                  $("#name").trigger("focus");
                  return flase;
                }
              }
              var reg_location = /^([\u4e00-\u9fa5]|[0-9]|[a-z]|[A-Z]|\-|\_|\/){1,6}$/;
              // if(!reg_location.test(val)){
              //   layer.msg("请输入汉字、数字、字母，特殊符号：“-”“_”“/”组成的名字，长度不超过6",{time:2000});
              // }
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
                  for(var i=0;i<AdSaleAreaGroup.length;i++){
                    if(val==AdSaleAreaGroup[i].GroupName){
                      AdSaleAreaGroup.splice(i,1);
                    }
                  }
                  if($('.selectdCity ul li').length==0){
                    layer.msg("城市不能为空",{time:2000});
                    return false;
                  }
                  // AdSaleAreaGroup=[];
                  $('.selectdCity ul li').each(function(index,item){
                    DetailArea.push({
                         "IsPublic":0,
                         "ProvinceId": -2,
                         "ProvinceName": "",
                         "CityId": $(item).attr("city_id"),
                         "CityName": $(item).html()
                       })
                  })
                  //加入大的城市组集合
                  AdSaleAreaGroup.push({
                    "GroupId": -2,
                    "GroupType":1,
                    "IsPublic":0,
                    "GroupName": val,
                    "DetailArea": DetailArea
                  })
                  console.log(AdSaleAreaGroup);
                  console.log(DetailArea);
                  //在页面中修改城市组名
                  that.parent().prev().val(val);
                  that.html(val);
                  //改变其中city_selecting的值
                  city_checked();
              }else{
                layer.msg("请输入汉字、数字、字母，特殊符号：“-”“_”“/”组成的名字，长度不超过6",{time:2000});
                // layer.msg("请填写城市组名称",{time:2000});
                return false;
              }
              $(".layer_city").hide();
              $("#zhezhao").hide();
            })
        })
    }
    //渲染跟操作城市
    function opration_city(){
      //已经选择的城市不显示
      console.log(AdSaleAreaGroup);
      $(".num_name_left").remove();
      for(var i=0;i<JSonData.masterArea.length;i++){
            for(var j=0;j<JSonData.masterArea[i].subArea.length;j++){
                var str = '<div style="cursor:pointer" class="sort_list" title="点击添加" szm="'+JSonData.masterArea[i].subArea[j].szm+'">'+'<div class="num_name_left" id="'+JSonData.masterArea[i].subArea[j].id+'">'+JSonData.masterArea[i].subArea[j].name+'</div>'+'</div>';
                $('.'+JSonData.masterArea[i].subArea[j].szm).after(str);
            }
        }
        for(var i=0;i<city_selecting.length;i++){
          for(var j=0;j<city_selecting[i].DetailArea.length;j++){
            for(var k=0;k<$(".num_name_left").length;k++){
              if(city_selecting[i].DetailArea[j].CityName==$($(".num_name_left")[k]).html()){
                $($(".num_name_left")[k]).remove();
              }
            }
          }
        }
      //点击跳转到对应字母
      $('.word').off('click').on('click',function () {
          var id = $.trim($(this).attr('id'));
          console.log(id);
          $('.sort_box').animate({scrollTop: $("."+id).position().top+$(".sort_box").scrollTop()-183}, 200);
          console.log($("."+id).position().top);
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
              $('.selectdCity ul').append('<li style="cursor:pointer" class="num_name" city_id="'+that.attr('id')+'" title="点击移除">'+content+'</li>')
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
    //判断城市组的选中的城市复选框
    function city_checked(){
      city_selecting = [];
      qita_val="";
      $("#location_checkbox input").on("change",function(){
        city_selecting = [];
        qita_val="";
        for(var i=0;i<$("#location_checkbox input").length;i++){

          for(var j=0;j<AdSaleAreaGroup.length;j++){
            if($("#location_checkbox input")[i].checked==true && $($("#location_checkbox input")[i]).val()==AdSaleAreaGroup[j].GroupName && JSON.stringify(city_selecting).indexOf(JSON.stringify(AdSaleAreaGroup[j]))==-1){
              city_selecting.push(AdSaleAreaGroup[j]);
              //显示其他城市
              if($($("#location_checkbox input")[i]).val()!="全国" && $($("#location_checkbox input")[i]).val()!="其他城市" && qita_val.indexOf($($("#location_checkbox input")[i]).val()) ==-1){
                qita_val = qita_val + $($("#location_checkbox input")[i]).val()+",";
                $("#qita_city span").eq(1).html('（除“'+qita_val.substring(0,qita_val.length-1)+'”以外的其他城市）');
              }
            }
          }
        }
        console.log(city_selecting);
        // opration_city();
        console.log(qita_val);
      })

      //如果页面上的所有城市组复选框都取消勾选
      if($(".insert_citygroup input").length==0){
        $("#qita_city").hide();
        console.log($("#qita_city").not(":hidden").length);
        }else{
          $("#qita_city").show();
        }
      //做删除操作时的验证
      for(var i=0;i<$("#location_checkbox input").not(":hidden").length;i++){
        for(var j=0;j<AdSaleAreaGroup.length;j++){
          if($("#location_checkbox input")[i].checked==true && $($("#location_checkbox input")[i]).val()==AdSaleAreaGroup[j].GroupName){
            city_selecting.push(AdSaleAreaGroup[j]);
            //显示其他城市
            if($($("#location_checkbox input")[i]).val()!="全国" && $($("#location_checkbox input")[i]).val()!="其他城市" && qita_val.indexOf($($("#location_checkbox input")[i]).val()) ==-1 ){
              qita_val = qita_val + $($("#location_checkbox input")[i]).val()+",";
              $("#qita_city span").eq(1).html('（除“'+qita_val.substring(0,qita_val.length-1)+'”以外的其他城市）');
            }
          }
        }
      }
      console.log(city_selecting);
      console.log(qita_val);
      if(qita_val!=""){
        $("#qita_city").show();
      }else{
        $("#qita_city").hide();
      }
      // opration_city();
    }
    //刊例原件和广告示例图上传
    var original_k="",
        sample_img="",
        input2="",
        input3="",
        input4="";

    uploadFile1("input1");
    if(CTLogin.RoleIDs=="SYS001RL00001" || CTLogin.RoleIDs=="SYS001RL00004"){
      $("#kanli").css("visibility","hidden");
      $("#shili").css("visibility","visible");
    }
    $(".uploadify").eq(0).css("height","20px");
    $(".swfupload").eq(0).css({"left":"0px","height":"20px","top":"-20px"});
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
           'fileCount':1,
           queueID:'imgShow',
           'overrideEvents' : [ 'onDialogClose'],
           'onQueueComplete': function (event, data) {
               //enableConfirmBtn();
           },
           'onQueueFull': function () {
               layer.alert('您最多只能上传1个文件！');
               return false;
           },
           'onUploadSuccess': function (file, data, response) {
                $("#alert_file").hide();
               if (response == true) {
                   var json = $.evalJSON(data);
                   original_k = json.Msg;
                   console.log(original_k);
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
            'fileTypeDesc': '支持格式:xls,xlsx,jpg,jpeg,png.gif,zip,pdf,ppt,pptx,mp4',
            'fileTypeExts': '*.xls;*.xlsx;*.jpg;*.jpeg;*.png;*.gif;*.zip;*.pdf;*.ppt;*.pptx;*.mp4',
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
                      input2 = "";
                      input2 = json.Msg.split("|")[0]+",";
                    }
                    if(id=="input3"){
                      input3 = "";
                      input3 = json.Msg.split("|")[0]+",";
                    }
                    if(id=="input4"){
                      input4 = "";
                      input4 = json.Msg.split("|")[0]+",";
                    }
                    sample_img = input2+input3+input4;
                    console.log(sample_img.substring(0,sample_img.length-1));
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

      //判断所有必填项都填之后才能提交成功，否则提交不成功
      $("#add_price").on("click",function(){
        console.log(AdTemplateName);
        console.log(AdForm);
        console.log(AdTempStyle);
        console.log(CarouselCount);
        console.log(SellingPlatform);
        console.log(SellingMode);
        console.log(city_selecting);
        console.log(original_k);
        console.log(sample_img);
        //信息提示
        if(!!AdTemplateName==false){
          $("#name2").show();
        }
        if(!!AdForm==false){
          $('#alert_form').show();
        }
        if(AdTempStyle.length==0){
          $("#alert_style").show();
        }
        if(!!CarouselCount==false){
          $("#alert_shuffling").show();
        }
        if(!!SellingPlatform==false){
          $("#alert_platform").show();
        }
        if(!!SellingMode==false){
          $("#alert_Mode").show();
        }
        if(original_k=="" && (CTLogin.RoleIDs!="SYS001RL00004" && CTLogin.RoleIDs!="SYS001RL00001")){
          $("#alert_file").show();
        }
        if(CTLogin.RoleIDs!="SYS001RL00004" && CTLogin.RoleIDs!="SYS001RL00001"){
          if(AdTemplateName && AdForm && AdTempStyle.length!=0 && CarouselCount && SellingPlatform && SellingMode && original_k){
            //掉接口传输json对象
            setAjax({
              url:"/api/Template/curd",
              type:"post",
              data:{
                    "BusinessType":15000,
                    "OperateType": 1,
                    "Temp": {
                        "TemplateId":-2,
                        "BaseAdID":-2, //模板id
                        "BaseMediaId":BaseMediaID, //基表媒体ID
                        "MediaID": MediaID,             //副表媒体id
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
                        "AdTempStyle": AdTempStyle,
                        "AdSaleAreaGroup": city_selecting
                    }
                }
            },function(data){
              console.log(data);
              if(data.Status==0){
                layer.msg("提交成功",{time:2000});
                var AdTemplateId = data.Result.AdTemplateId;
                console.log(MediaID);
                console.log(AdTemplateId);
                if(CTLogin.RoleIDs=="SYS001RL00003" || CTLogin.RoleIDs=="SYS001RL00005"){
                  // window.location= "/PublishManager/addPublishForApp.html?MediaID="+MediaID+"&PubID="+PubID+"&TemplateID="+AdTemplateId;
                  window.location = "/publishmanager/advertisinglist_app.html";
                }else if(CTLogin.RoleIDs=="SYS001RL00001" || CTLogin.RoleIDs=="SYS001RL00004"){
                  window.location="/publishmanager/advtempauditlist.html";
                }
              }else{
                layer.msg(data.Message,{time:2000});
              }
            })
          }else{
            layer.msg("必填项不能为空",{time:2000});
          }
        }
          if(CTLogin.RoleIDs=="SYS001RL00004" || CTLogin.RoleIDs=="SYS001RL00001"){
            if(AdTemplateName && AdForm && AdTempStyle.length!=0 && CarouselCount && SellingPlatform && SellingMode){
              //掉接口传输json对象
              setAjax({
                url:"/api/Template/curd",
                type:"post",
                data:{
                      "BusinessType":15000,
                      "OperateType": 1,
                      "Temp": {
                          "TemplateId":-2,
                          "BaseAdID":-2, //模板id
                          "BaseMediaId":BaseMediaID, //基表媒体ID
                          "MediaID": MediaID,             //副表媒体id
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
                          "AdTempStyle": AdTempStyle,
                          "AdSaleAreaGroup": city_selecting
                      }
                  }
              },function(data){
                console.log(data);
                if(data.Status==0){
                  layer.msg("提交成功",{time:2000});
                  var AdTemplateId = data.Result.AdTemplateId;
                  console.log(MediaID);
                  console.log(AdTemplateId);
                  if(CTLogin.RoleIDs=="SYS001RL00003" || CTLogin.RoleIDs=="SYS001RL00005"){
                    // window.location= "/PublishManager/addPublishForApp.html?MediaID="+MediaID+"&PubID="+PubID+"&TemplateID="+AdTemplateId;
                    window.location = "/publishmanager/advertisinglist_app.html";
                  }else if(CTLogin.RoleIDs=="SYS001RL00001" || CTLogin.RoleIDs=="SYS001RL00004"){
                    window.location="/publishmanager/advtempauditlist.html";
                  }
                }else{
                  layer.msg(data.Message,{time:2000});
                }
              })
            }else{
              layer.msg("必填项不能为空",{time:2000});
            }
          }
      })

      //点击保存并继续添加跳转到新增模板页面
      $("#continue_add").on("click",function(){
        console.log(AdTemplateName);
        console.log(AdForm);
        console.log(AdTempStyle);
        console.log(CarouselCount);
        console.log(SellingPlatform);
        console.log(SellingMode);
        console.log(city_selecting);
        console.log(original_k);
        //信息提示
        if(!!AdTemplateName==false){
          $("#name2").show();
        }
        if(!!AdForm==false){
          $('#alert_form').show();
        }
        if(AdTempStyle.length==0){
          $("#alert_style").show();
        }
        if(!!CarouselCount==false){
          $("#alert_shuffling").show();
        }
        if(!!SellingPlatform==false){
          $("#alert_platform").show();
        }
        if(!!SellingMode==false){
          $("#alert_Mode").show();
        }
        if(original_k==""){
          $("#alert_file").show();
        }
          if(AdTemplateName && AdForm && AdTempStyle.length!=0 && CarouselCount && SellingPlatform && SellingMode && original_k && CTLogin.RoleIDs!="SYS001RL00004"){
            //掉接口传输json对象
            setAjax({
              url:"/api/Template/curd",
              type:"post",
              data:{
                    "BusinessType":15000,
                    "OperateType": 1,
                    "Temp": {
                        "TemplateId":-2,
                        "BaseAdID":-2, //模板id
                        "BaseMediaId":BaseMediaID, //基表媒体ID
                        "MediaID": MediaID,//附表媒体id
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
                        "AdTempStyle": AdTempStyle,
                        "AdSaleAreaGroup": city_selecting
                    }
                }
            },function(data){
              console.log(data);
              if(data.Status==0){
                layer.msg("提交成功",{time:2000});
                if(CTLogin.RoleIDs=="SYS001RL00003" || CTLogin.RoleIDs=="SYS001RL00005"){
                  window.location= "/PublishManager/add_template.html?MediaID="+MediaID+"&AppName="+AppName;
                }else if(CTLogin.RoleIDs=="SYS001RL00001" || CTLogin.RoleIDs=="SYS001RL00004"){
                  window.location="/publishmanager/advtempauditlist.html";
                }
              }else{
                layer.msg(data.Message,{time:2000});
              }
            })
          }else{
            layer.msg("必填项不能为空",{time:2000});
          }

          if(CTLogin.RoleIDs=="SYS001RL00004"){
            if(AdTemplateName && AdForm && AdTempStyle.length!=0 && CarouselCount && SellingPlatform && SellingMode){
              //掉接口传输json对象
              setAjax({
                url:"/api/Template/curd",
                type:"post",
                data:{
                      "BusinessType":15000,
                      "OperateType": 1,
                      "Temp": {
                          "TemplateId":-2,
                          "BaseAdID":-2, //模板id
                          "BaseMediaId":BaseMediaID, //基表媒体ID
                          "MediaID": MediaID,//附表媒体id
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
                          "AdTempStyle": AdTempStyle,
                          "AdSaleAreaGroup": city_selecting
                      }
                  }
              },function(data){
                console.log(data);
                if(data.Status==0){
                  layer.msg("提交成功",{time:2000});
                  if(CTLogin.RoleIDs=="SYS001RL00003" || CTLogin.RoleIDs=="SYS001RL00005"){
                    window.location= "/PublishManager/add_template.html?MediaID="+MediaID+"&AppName="+AppName;
                  }else if(CTLogin.RoleIDs=="SYS001RL00001" || CTLogin.RoleIDs=="SYS001RL00004"){
                    window.location="/publishmanager/advtempauditlist.html";
                  }
                }else{
                  layer.msg(data.Message,{time:2000});
                }
              })
            }else{
              layer.msg("必填项不能为空",{time:2000});
            }
          }

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
