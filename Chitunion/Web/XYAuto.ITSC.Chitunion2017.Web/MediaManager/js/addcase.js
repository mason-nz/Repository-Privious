$(function () {
  var ue = UE.getEditor('editor');
    //媒体ID
    var MediaID = GetQueryString('MediaID')!=null&&GetQueryString('MediaID')!='undefined'?parseFloat(GetQueryString('MediaID')):null;
    //媒体类型
    var MediaType = GetQueryString('MediaType')!=null&&GetQueryString('MediaType')!='undefined'?parseFloat(GetQueryString('MediaType')):null;
    // 页面渲染
    setAjax({
        url:'/api/Media/GetMediaDetail',
        type:'get',
        data:{
            MediaType:MediaType,
            MediaID:MediaID
        },
        dataType:'json'
    },
        function (data) {
            if(data.Status==0){
              $('.install ul').html(ejs.render($('#orderList').html(),{data:data.Result.MediaInfo}))
            }
        }
    )
    //编辑器渲染
    setAjax({
        url:'/api/Media/SelectMediaCaseInfo?v=1_1',
              type:'get',
              data:{
                MediaType:MediaType,
                MediaID:MediaID,
                CaseStatus:1,
              },
      },function(data){
          if(data.Result.length>0){
               ue.ready(function() {//编辑器初始化完成再赋值
                 ue.setContent(data.Result[0].CaseContent);  //赋值给UEditor
            });
          }
      })
  //预览功能
    // $('.keep .button').eq(0).attr({'href':'/MediaManager/preseeAddCase.html?MediaType='+MediaType+'&MediaID='+MediaID});
    $('.keep .button').eq(0).on('mouseover',function(){
        $(this).attr({'href':'/MediaManager/preseeAddCase.html?MediaType='+MediaType+'&MediaID='+MediaID});
    })
      .on('click',function () {
          var _this = $(this);
          setAjax({
              url:'/api/Media/InsertMediaCaseInfo?v=1_1',
              type:'POST',
              data:{
                MediaType:MediaType,
                MediaID:MediaID,
                CaseStatus:0,
                CaseContent:ue.getContent()
              },
              dataType:'json'
          },function (data) {
              if(data.Status==0){
                  _this.attr({'href':'/MediaManager/preseeAddCase.html?MediaType='+MediaType+'&MediaID='+MediaID});
              }else{
                  alert(data.Message);
              }
          }
        )
    })
    //提交功能
  $('.keep .button').eq(1).off('click').on('click',function () {
        var reg = /.{20,}/g;
        if(reg.test(ue.getContentTxt())){
              setAjax({
                  url:'/api/Media/InsertMediaCaseInfo?v=1_1',
                  type:'POST',
                  data:{
                    MediaType:MediaType,
                    MediaID:MediaID,
                    CaseStatus:1,
                    CaseContent:ue.getContent()
                  },
                  dataType:'json'
              },function (data) {
                console.log(data);
                  if(data.Status==0){
                        $('#error_prompt').hide();
                        $('#prompt').show(200,function(){
                            setTimeout(function(){
                              if(MediaType==14001){
                                window.location.href='/MediaManager/mediaWeChatList_new.html';
                              }else if(MediaType==14002){
                                window.location.href='/MediaManager/mediaAPP.html';
                              }

                            },1000);
                        });

                 }else{
                     alert(data.Message);
                }
              }
            )

        }else{
            $('#error_prompt').show();
        }
    })
    //获取url 地址参数方法
    function GetQueryString(name) {
        var reg = new RegExp("(^|&)"+ name +"=([^&]*)(&|$)");
        var r = window.location.search.substr(1).match(reg);
        if(r!=null)return unescape(r[2]); return null;
    }
})
