$( function () {
  var userData = GetUserId();
	var CostID = userData.hasOwnProperty("CostID") ? userData["CostID"] : "";
  //根据CostID去获取成本信息
  setAjax({
    url:"http://www.chitunion.com/api/Channel/GetCostDetail",
    type:"get",
    dataType:"json",
    data:{
      CostID: CostID
    }
  },function (data) {
    console.log(data);
    //渲染信息
    $(".install_box").html(ejs.render($("#rendering").html(),{data:data.Result}));
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
