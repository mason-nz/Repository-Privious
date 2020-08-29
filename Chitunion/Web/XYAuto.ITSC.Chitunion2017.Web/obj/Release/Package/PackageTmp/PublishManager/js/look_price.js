/*
* Written by:     zhangliangpeng
* function:       查看价格页效果js库
* Created Date:   2017-06-7
* Modified Date:  2017-05-17
*/
$(function(){
  var userData = GetUserId();
  var PubID = userData.hasOwnProperty("PubID") ? userData["PubID"] : 0;
  var TemplateID = userData.hasOwnProperty("TemplateID") ? userData["TemplateID"] : 0;
  var CreateUserRole = decodeURIComponent(userData["CreateUserRole"]);
  console.log(CreateUserRole);
  //请求页面参数
  setAjax({
    url:"/api/Publish/GetADDetail?v=1_1",
    data:{
      MediaType:14002,
      PubID:PubID
    },
    dataType:"json",
    type:"get"
  },function(data){
    console.log(CTLogin);
    console.log(data);
    if(CTLogin.RoleIDs=="SYS001RL00005"){
      $('.macro_box').html(ejs.render($('#look_AE').html(),{data:data.Result}));
      change_color();
      $(".hc_r2 a").on("click",function(){
        window.location = "/publishmanager/advTempDetail.html?AdTempId="+TemplateID;
      })
    }else if(CTLogin.RoleIDs=="SYS001RL00003"){
      $('.macro_box').html(ejs.render($('#look_media').html(),{data:data.Result}));
      change_color();
      $(".hc_r2 a").on("click",function(){
        window.location = "/publishmanager/advTempDetail.html?AdTempId="+TemplateID;
      })
    }else if(CTLogin.RoleIDs=="SYS001RL00001" || CTLogin.RoleIDs=="SYS001RL00004"){
      if(CreateUserRole == "AE" ){
        $('.macro_box').html(ejs.render($('#look_AE').html(),{data:data.Result}));
        change_color();
        $(".hc_r2 a").on("click",function(){
          window.location = "/publishmanager/advTempDetail.html?AdTempId="+TemplateID;
        })
      }else if(CreateUserRole == "媒体主"){
        console.log(1);
        $('.macro_box').html(ejs.render($('#look_media').html(),{data:data.Result}));
        change_color();
        $(".hc_r2 a").on("click",function(){
          window.location = "/publishmanager/advTempDetail.html?AdTempId="+TemplateID;
        })
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
  // 改变tr，td颜色
  function change_color(){
    for(var i=0;i<$("tr").length;i++){
      if($($("tr")[i]).attr("RowState")==1){
        $($("tr")[i]).children('td').css("background","#e8f9e0");
      }
       else if($($("tr")[i]).attr("RowState")==-1){
        $($("tr")[i]).children('td').css("background","#f9dcdc");
      }else if($($("tr")[i]).attr("RowState")!=null){
        for(var j=0;j<$($("tr")[i]).attr("RowState").split(",").length;j++){
          for(var k=0;k<$($("tr")[i]).children('td').length;k++){
            if($($("tr")[i]).attr("RowState").split(",")[j]==$($($("tr")[i]).children('td')[k]).attr("sync")){
                $($($("tr")[i]).children('td')[k]).css("color","red");
            }
          }
        }
      }
    }
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
