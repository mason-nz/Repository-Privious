$(function(){
  var currPage = 1;
  var currPageSize = 20;
  var recordCount = 0;
  //初始渲染页面
  var param = {};
  dataload(param);
  //滚动从第二页开始
  $(window).scroll(function(){
     var scrollTop = $(this).scrollTop();
     var scrollHeight = $(document).height();
     var windowHeight = $(this).height();
     if(scrollTop + windowHeight == scrollHeight && (currPage*currPageSize < recordCount)){
          dataload(param);
    }
   })
  function dataload(param){
    param.currentPage = currPage;
      setAjax({
          url:"/api/Media/SelectCollectionPullBlack?v=1_1",
          type:"get",
          data:{
              // businesstype:14001,
              SelectType:1,
              pageSize:20,
              pageIndex:param.currentPage
          }
      },function(data){
          console.log(data);
          if(data.Result){
          recordCount = data.Result.TotleCount;
          $(".blacklist").append(ejs.render($("#favorite").html(),{data:data.Result.listDetail}));
          for(var i=0;i<$(".off_shelves2").length;i++){
            $($(".off_shelves2")[i]).prev().children('.cancel_favorites').remove();
          }
          $(".portrait").on("click",function(){
              var MediaID = $(this).parent().find(".MediaID").val();
              var MediaType = $(this).parent().find(".MediaType").val();
              var TemplateID = $(this).parent().find(".TemplateID").val();
              console.log(MediaID);
              if(MediaType==14001){
                if(TemplateID>0){
                  window.location ="/OrderManager/wx_detail.html"+"?MediaID="+MediaID+"&MediaType="+MediaType;
                }else{
                  layer.msg("暂无可售卖广告",{time:2000});
                }
              }else if(MediaType==14002){
                if(TemplateID>0){
                  window.location ="/OrderManager/app_detail.html"+"?MediaID="+MediaID+"&MediaType="+MediaType+"&TemplateID="+TemplateID;
                }else{
                  layer.msg("暂无可售卖广告",{time:2000});
                }
              }
					})
          //对收藏商品进行操作
          $(".cancel_favorites").on("click",function(){
                console.log($(this));
                var MediaID = parseInt($(this).find("input").eq(0).val());
                var MediaType = parseInt($(this).find(".MediaType").val());
                console.log(MediaID);
                $(this).parent().remove();
                //取消收藏操作
                setAjax({
                url:"/api/CollectPullBack/Remove",
                type:"post",
                data:{
                    businesstype:MediaType,
                    operatetype:1,
                    mediaId:MediaID
                }
                },function(data){
                console.log(data);
                //删除后刷新页面
                location.reload();
                })
          })
          //页面增加
          currPage++;
        }else{
          $(".collection2").html('<img src="/images/no_data.png" style="display:block;margin:20px auto;">');
        }
      })
    }
})
