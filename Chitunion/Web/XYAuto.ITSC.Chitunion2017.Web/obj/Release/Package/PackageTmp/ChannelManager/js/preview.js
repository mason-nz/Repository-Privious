$(function () {
  var userData = GetUserId();
  var ChannelID = userData.hasOwnProperty("ChannelID") ? decodeURIComponent(userData["ChannelID"]) : "";
  $.ajax({
    url: "http://www.chitunion.com/api/Channel/GetUploadPreview",
    type: "get",
    dataType: "json",
    xhrFields: {
      withCredentials: true
    },
    crossDomain: true,
    beforeSend: function (data) {
      $(".install_box").html('<div style="margin:150px auto; width:32px;"><img src="/images/loading.gif"></div>');
    },
    success: function (data) {
      if(data.Status == 0){
        console.log(data);
        //渲染页面信息
        // $("#table").html(ejs.render($("#preview").html(),{data:data.Result}));
        $(".install_box").html(ejs.render($("#preview").html(),{data:data.Result}));
        //点击提交按钮跳转到成本列表
        $("#submit").off("click").on("click",function () {
          // console.log(data);
          $.ajax({
            url: "http://www.chitunion.com/api/Channel/SubmitUploadResource",
            type:"post",
            dataType: "json",
            xhrFields: {
              withCredentials: true
            },
            crossDomain: true,
            beforeSend: function () {
              $(".install_box").html('<div style="margin:150px auto; width:32px;"><img src="/images/loading.gif"></div>');
            },
            success: function (data) {
              console.log(data);
              if(data.Status == 0){
                layer.msg("上传成功",{time:2000});
                //跳转到成本列表
                window.location = "/ChannelManager/CostList.html?ChannelID="+ChannelID;
              }else{
                layer.msg(data.Message,{time:2000});

              }
            }
          })
        })


      }else{
        layer.msg(data.Message,{time:5000});
        // $("#rendering1").html('<div style="margin:150px auto; width:32px;"><img src="/images/loading.gif"></div>');
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
