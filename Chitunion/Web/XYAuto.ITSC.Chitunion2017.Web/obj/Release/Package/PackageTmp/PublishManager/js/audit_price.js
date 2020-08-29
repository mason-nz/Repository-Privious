/*
* Written by:     zhangliangpeng
* function:       审核价格页效果js库
* Created Date:   2017-06-07
* Modified Date:  2017-05-17
*/
$(function(){
  var userData = GetUserId();
  var PubID = userData.hasOwnProperty("PubID") ? userData["PubID"] : -2;
  var that,
      PubIDs=[];
  again();
  function again(){
    setAjax({
      url:"/api/Publish/GetAuditADPriceList?v=1_1",
      data:{
        MediaType:14002,
        PubID:PubID
      },
      dataType:"json",
      type:"get"
    },function(data){

      console.log(data);
      if(data.Status==0){

        if(data.Result.List==null){
          window.location = "/publishmanager/newsreview.html";
        }
        $('.h_content').html(ejs.render($('#info').html(),{data:data.Result}));
        $('#data').html(ejs.render($('#publish').html(),{data:data.Result}));
        $(".ad_select input[type=radio]").prop("checked",false);
        $("#reason input").val("");
        $("#reason").hide();
        //变换新增或者修改的颜色
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
        //查看选中的复选框
        $('.audit_info2 input').on("change",function(){
          PubIDs=[];
          //检查全选状态
          change();
          for(var i=0;i<$('.audit_info2 input').length;i++){
            if($('.audit_info2 input')[i].checked==true && PubIDs.indexOf($($('.audit_info2 input')[i]).attr("PubID"))==-1){
              PubIDs.push($($('.audit_info2 input')[i]).attr("PubID"));
            }
            if($('.audit_info2 input')[i].checked==false && PubIDs.indexOf($($('.audit_info2 input')[i]).attr("PubID"))!=-1){
              //如果在没选中的状态下数组里面有值得话就去删除
                PubIDs.splice(PubIDs.indexOf($($('.audit_info2 input')[i]).attr("PubID")),1)
            }

          }
        })

        change();
        //点击确认按钮时的操作，掉接口传审核结果
        $(".put_in").off("click").on("click",function(){
          console.log(PubIDs);

              $("input[type=radio]").each(function(index, el) {
                if(el.checked == true && $(el).val()=="通过"){
                  console.log(PubIDs);
                  setAjax({
                    url:"/api/Publish/BatchAuditPublish?v=1_1",
                    type:"post",
                    data:{
                      PubIDs: PubIDs,
                      MediaType:14002,
                      RejectReason:"",
                      OpType:1
                    }
                  },function(data){
                    if(data.Status==0){
                      var NextPubID = data.Result.NextPubID;
                      layer.msg("审核成功",{time:2000});
                      console.log(data);
                      $('.audit_info2 input').each(function(index, el) {
                        if(el.checked == true){
                          that = $(el);
                          console.log(that);
                          console.log(that.parent().parent());
                          //删除该行
                          console.log(that.parent());
                          that.parent().parent().remove();
                        }
                      })
                      if($(".audit_info2").length==0){
                        PubID = NextPubID;
                        //再次请求
                        again();
                      }
                    }
                  })

                }else if(el.checked == true && $(el).val()=="驳回"){
                  if($("#reason input").val()==""){
                    layer.msg("驳回原因不能为空",{time:2000});
                    return false;
                  }
                  setAjax({
                    url:"/api/Publish/BatchAuditPublish?v=1_1",
                    type:"post",
                    data:{
                      PubIDs: PubIDs,
                      MediaType:14002,
                      RejectReason:$("#reason input").val(),
                      OpType:2
                    }
                  },function(data){
                    if(data.Status==0){
                      var NextPubID = data.Result.NextPubID;
                      layer.msg("审核成功",{time:2000});
                      console.log(data);
                      $('.audit_info2 input').each(function(index, el) {
                        if(el.checked == true){
                          that = $(el);
                          console.log(that);
                          console.log(that.parent().parent());
                          //删除该行
                          console.log(that.parent());
                          that.parent().parent().remove();
                        }
                      })
                      if($(".audit_info2").length==0){
                        PubID = NextPubID;
                        //再次请求
                        again();
                      }
                    }
                  })
                }
              })


        })
        //全选按钮的样式变化
        function change(){
          if($(".audit_info2 input:checked").length == $(".audit_info2 input").length){
            $(".audit_info input").prop("checked",true);
            $(".ad_select input").eq(0).prop("checked",true);
          }else{
            $(".audit_info input").prop("checked",false);
            $(".ad_select input").eq(0).prop("checked",false);
          }
        //选中通过或者驳回按钮的操作
        $("input[type=radio]").on("change",function(){
          if($(".audit_info2 input:checked").length==0){
            $(this).prop("checked",false);
            return false;
          }
            $("input[type=radio]").each(function(index, el) {
              if(el.checked == true && $(el).val()=="驳回"){
                $("#reason").show();
                // $(".keep").off("click").on("click",function(){
                //   $(".layer").hide();
                // })
              }
            })
            $("input[type=radio]").each(function(index, el) {
              if(el.checked == true && $(el).val()=="通过"){
                $("#reason").hide();
                // $(".keep").off("click").on("click",function(){
                //   $(".layer").hide();
                // })
              }
            })
        })
        // //检查页面上的将要审核的单元，没有的话再次请求
        // $(".audit_info2 input").on("change",function(){
        //   if($(".audit_info2").length==0){
        //     //再次请求
        //     again();
        //   }
        // })
      }
      //全选按钮的操作
      $(".audit_info input").on("change",function(){
        PubIDs=[];
        // console.log(1);
        if($(this).prop("checked")){
          $(".audit_info2 input").prop("checked",true);
          $(".ad_select input").eq(0).prop("checked",true);
          for(var i=0;i<$('.audit_info2 input').length;i++){
            if($('.audit_info2 input')[i].checked==true && PubIDs.indexOf($($('.audit_info2 input')[i]).attr("PubID"))==-1){
              PubIDs.push($($('.audit_info2 input')[i]).attr("PubID"));
            }
            if($('.audit_info2 input')[i].checked==false && PubIDs.indexOf($($('.audit_info2 input')[i]).attr("PubID"))!=-1){
              //如果在没选中的状态下数组里面有值得话就去删除
                PubIDs.splice(PubIDs.indexOf($($('.audit_info2 input')[i]).attr("PubID")),1)
            }

          }
        }else{
          $(".audit_info2 input").prop("checked",false);
          $(".ad_select input").eq(0).prop("checked",false);
        }
      })
      $(".ad_select input").eq(0).on("change",function(){
        PubIDs=[];
        if($(this).prop("checked")){
          $(".audit_info2 input").prop("checked",true);
          $(".audit_info input").prop("checked",true);
          for(var i=0;i<$('.audit_info2 input').length;i++){
            if($('.audit_info2 input')[i].checked==true && PubIDs.indexOf($($('.audit_info2 input')[i]).attr("PubID"))==-1){
              PubIDs.push($($('.audit_info2 input')[i]).attr("PubID"));
            }
            if($('.audit_info2 input')[i].checked==false && PubIDs.indexOf($($('.audit_info2 input')[i]).attr("PubID"))!=-1){
              //如果在没选中的状态下数组里面有值得话就去删除
                PubIDs.splice(PubIDs.indexOf($($('.audit_info2 input')[i]).attr("PubID")),1)
            }

          }
        }else{
          $(".audit_info2 input").prop("checked",false);
          $(".audit_info input").prop("checked",false);
        }
      })
      //点击弹窗关闭按钮
      // $(".close").on("click",function(){
      //   $(".layer").hide();
      // })
    }else{
      $('#data').html('<img src="/images/no_data.png" style="display:block;margin:20px auto;">');
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


})
// 通过url获取文件名称
function getFileName(o){
    if(o == null) return '';
    var pos=o.lastIndexOf("/");
    var str = o.substring(pos+1);
    var pos1 = str.indexOf('$');
    return str.substr(0,pos1);
}
