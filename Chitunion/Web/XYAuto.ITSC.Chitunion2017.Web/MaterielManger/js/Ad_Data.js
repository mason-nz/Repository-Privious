$(function () {
  //定义页面中的交换数据
  var ChannelID,
      RecID,
      DataDate,
      ReadCount,
      LikeCount,
      CommentCount,
      DataAll,
      mediatype,
      flag = true;
      var userData = GetUserId();
      var MaterielID = userData.hasOwnProperty("MaterielID") ? decodeURIComponent(userData["MaterielID"]) : "";
  //渲染页面上部分信息
  $.ajax({
    url:"http://www.chitunion.com/api/Materiel/getInfo",
    type:"get",
    data:{
      MaterielId: MaterielID,
      IsGetChannel: true
    },
    dataType:"json",
    xhrFields:{
      withCredentials: true
    },
    crossDomain: true,
    success: function (data) {
      if(data.Status ==0){
        $(".searchOption").html(ejs.render($("#Top_Tmp").html(),{data:data.Result}));
      }
    }
  })

  render_info();
  //渲染下部分的信息
  function render_info() {
    $.ajax({
      url:"http://www.chitunion.com/api/MaterielChannelData/GetDataList",
      type:"get",
      data:{
        MaterielID: MaterielID
      },
      xhrFields:{
        withCredentials: true
      },
      crossDomain:true,
      success: function (data) {
      DataAll = data;

        if(data.Status == 0){
          //渲染中间页面信息
          $("#Ad_media").html(ejs.render($("#Media_Temp").html(),{data:data.Result}));
          //渲染下部分总计的信息
          // $("#Bt_info").html(ejs.render($("#Bt_Tmp").html(),{data:data.Result}));

          //对页面中媒体的添加操作
          $(".add_btn").off("click").on("click",function () {

            $("#startTime").val('');
            $("#ReadCount").val('');
            $("#LikeCount").val('');
            $("#CommentCount").val('');
            $("#Time_info").hide();
            $("#Time_info_two").hide();
            $("#Read_info").hide();
            //昨天加操作先把内容跟提示信息清除

            //赋值渠道名称部分
            $("#channel_name span").eq(0).html($(this).prev().children('span').eq(1).html());
            $("#channel_name span").eq(1).html($(this).prev().children('span').eq(0).html());
            if(!$(this).prev().children('span').eq(1).html()){
              $("#channel_name b").hide();
            }
            //计算高度
            calculate();
            $(".layer").show();
            verify();
            ChannelID = $(this).attr("ChannelID");
            mediatype = $(this).attr("mediatype");
            //点击提交
            $("#add_z").off("click").on("click",function(){
              //进行弹层信息的验证并取值
              validation_data();
              if(flag){
                $.ajax({
                  url:"http://www.chitunion.com/api/MaterielChannelData/SaveData",
                  type:"post",
                  data:{
                    "ChannelID":ChannelID,
                    "RecID": 0,
                    "DataDate": DataDate,
                    "ReadCount": ReadCount,
                    "LikeCount": LikeCount,
                    "CommentCount": CommentCount
                  },
                  dataType:"json",
                  xhrFields:{
                    withCredentials:true
                  },
                  crossDomain:true,
                  success: function (data) {
                    if(data.Status == 0){
                      $("#Time_info_two").hide();
                      $(".layer").hide();
                      console.log(data);
                      //每次操作完重新渲染页面
                      render_info();
                    }else{
                      $("#Time_info_two").show();
                    }
                  }
                })
              }
            })
            //点击删除
            $(".close").on("click",function(){
              $(".layer").hide();
            })
          })

          //对页面中的编辑操作
          $(".channel_edit_btn").off("click").on("click",function(){
            //赋值渠道名称部分
            $("#channel_name span").eq(0).html($(this).prev().children('span').eq(1).html());
            $("#channel_name span").eq(1).html($(this).prev().children('span').eq(0).html());
            if(!$(this).prev().children('span').eq(1).html()){
              $("#channel_name b").hide();
            }
            calculate();
            $(".layer").show();

            //每个维度依次赋值
            var tds = $(this).parents("tr").children('td');
            RecID =  $(this).parents("tr").attr("RecID");
            console.log(RecID);
            $("#startTime").val(tds.eq(0).html());
            $("#ReadCount").val(tds.eq(1).html());


            if(tds.eq(2).html() == 0){
              $("#LikeCount").val("");
            }else{
              $("#LikeCount").val(tds.eq(2).html());
            }
            if(tds.eq(3).html() == 0){
              $("#CommentCount").val("");
            }else{
              $("#CommentCount").val(tds.eq(3).html());
            }
            verify();
            ChannelID = $(this).parents(".ad_table").attr("ChannelID");
            mediatype = $(this).parents(".ad_table").attr("mediatype");
            //点击提交
            $("#add_z").off("click").on("click",function(){
              //进行弹层信息的验证并取值
              validation_data();
              if(flag){
                $.ajax({
                  url:"http://www.chitunion.com/api/MaterielChannelData/SaveData",
                  type:"post",
                  data:{
                    "ChannelID":ChannelID,
                    "RecID": RecID,
                    "DataDate": DataDate,
                    "ReadCount": ReadCount,
                    "LikeCount": LikeCount,
                    "CommentCount": CommentCount
                  },
                  dataType:"json",
                  xhrFields:{
                    withCredentials:true
                  },
                  crossDomain:true,
                  success: function (data) {
                    if(data.Status == 0){
                      $("#Time_info_two").hide();
                      $(".layer").hide();
                      console.log(data);
                      //每次操作完重新渲染页面
                      render_info();
                    }else{
                      $("#Time_info_two").show();
                    }
                  }
                })
              }
            })
            //点击删除
            $(".close").on("click",function(){
              $(".layer").hide();
            })
          })

          //对页面中的删除操作
          $(".channel_delete_btn").off("click").on("click",function(){
            var that = $(this);
            RecID =  that.parents("tr").attr("RecID");
            console.log(RecID);
            layer.confirm("确定要删除吗？",{
              time:0,
              btn:["确认","取消"],
              yes: function(index){
                that.parents("tr").remove();
                //调用yes，需要手动关闭
                layer.close(index);

                $.ajax({
                  url:"http://www.chitunion.com/api/MaterielChannelData/DeleteData",
                  type:"post",
                  data:{
                    RecID:RecID
                  },
                  dataType:"json",
                  xhrFields:{
                    withCredentials: true
                  },
                  crossDomain: true,
                  success: function(data){
                    console.log(data);
                    //每次操作完重新渲染页面
                    render_info();
                  }

                })
              }
            })

          })

          var start = {
            elem: "#startTime",
            fixed: false,
            // min: add_date(laydate.now()),
            // min: laydate.now(),
            // istime: true,
            // issure: true,
            istoday:false,
            // isNeedConfirm: true,
            format: 'YYYY-MM-DD',
            choose: function (date) {
              DataDate = date;
            }
          }
          $("#startTime").off("click").on("click", function () {
            laydate(start);
            // console.log(CooperateBeginDate);
          })
        }
        if(data.Result.TypeList.length==0){
          $("#Ad_media").html('<img src="../images/no_data.png" style="display:block;margin:20px auto;">');
        }

      }
    })
  }

  //打包下载
  $("#download").off("click").on("click",function(){
    console.log(1);
    if(DataAll.Result.TypeList.length == 0 || DataAll.Result.Total.length == 0 ){
      layer.msg("暂无数据下载",{time:2000});
    }else{
      $.ajax({
        url:"http://www.chitunion.com/api/MaterielChannelData/ExportToExcel",
        type:"post",
        data:{
          "MaterielID": MaterielID
        },
        dataType:"json",
        xhrFields:{
          withCredentials:true
        },
        crossDomain: true,
        success: function(data){
          console.log(data);
          if(data.Status == 0){
            // $("#download").attr("target","_blank");
            // $("#download").attr("href",data.Result);
            // 下载文件
            window.location.href = "http://www.chitunion.com" + data.Result;

          }
        }
      })
    }

  })
  function verify(){
    var reg = /^\+?[1-9]\d*$/;
    //阅读数只能填入数字
    $("#ReadCount").on("input",function(){
      if(!reg.test($("#ReadCount").val())){
        $("#ReadCount").val("");
      }
    })
    $("#LikeCount").on("input",function(){
      if(!reg.test($("#LikeCount").val())){
        $("#LikeCount").val("");
      }
    })
    $("#CommentCount").on("input",function(){
      if(!reg.test($("#CommentCount").val())){
        $("#CommentCount").val("");
      }
    })
  }
  //弹层数据验证并取值
  function validation_data() {
    flag = true;
    DataDate = $("#startTime").val();
    ReadCount = $("#ReadCount").val();
    LikeCount = $("#LikeCount").val();
    CommentCount = $("#CommentCount").val();

    //验证数据日期跟阅读数的必填项
    if(!DataDate){
      $("#Time_info").show();
      flag = false;
    }else{
      $("#Time_info").hide();
    }
    if(!ReadCount){
      $("#Read_info").show();
      flag = false;
    }else{
      $("#Read_info").hide();
    }
    //验证日期不能重复
    // for(var i=0;i<DataAll.Result.TypeList.length;i++){
    //   if(mediatype == DataAll.Result.TypeList[i].MediaTypeName){
    //     for(var j=0;j<DataAll.Result.TypeList[i].MediaList.length;j++){
    //       if(ChannelID == DataAll.Result.TypeList[i].MediaList[j].ChannelID){
    //         for(var k=0;k<DataAll.Result.TypeList[i].MediaList[j].DataList.length;k++){
    //           if(DataDate == DataAll.Result.TypeList[i].MediaList[j].DataList[k].DataDate.split(' ')[0]){
    //             $("#Time_info_two").show();
    //             flag = false;
    //           }else{
    //             $("#Time_info_two").hide();
    //           }
    //         }
    //       }
    //     }
    //   }
    // }
  }
  //实时的获取距离可视化顶部的距离
  function calculate() {
    var scro = $(document).scrollTop();
    $('.layer').css("top",scro+100+"px");
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
})
