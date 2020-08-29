$(function () {
  //页面中需要的参数
  var userData = GetUserId();
	var ChannelID = userData["ChannelID"];
  //页面进来渲染信息
  setAjax({
    url: "http://www.chitunion.com/api/Channel/GetChannelInfo",
    type: "get",
    dataType: "json",
    data: {
      ChannelID: ChannelID
    }
  },function (data) {
    if(data.Status == 0){
      //渲染页面
      $(".install_box").html(ejs.render($("#channel").html(),{data:data.Result}));
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
