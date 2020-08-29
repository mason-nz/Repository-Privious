$( function () {
  var FilePath = '';
  var userData = GetUserId();
  var ChannelName = userData.hasOwnProperty("ChannelName") ? decodeURIComponent(userData["ChannelName"]) : "";
  var CooperateDate = userData.hasOwnProperty("CooperateDate") ? decodeURIComponent(userData["CooperateDate"]) : "";
  var ChannelID = userData.hasOwnProperty("ChannelID") ? decodeURIComponent(userData["ChannelID"]) : "";
  //上传资源页面
  uploadFile("input1");
  $(".uploadify").eq(0).css("height","20px");
  $(".swfupload").eq(0).css({"left":"0px","height":"20px","top":"8px"});
  $(".uploadify-button").eq(0).css("height","20px");
  $("#ChannelName").html(ChannelName);
  $("#CooperateDate").html(CooperateDate);
  //点击确定进行请求
  $("#determine").off("click").on("click",function () {
    if(!FilePath){
      layer.msg("请先上传表格",{time:2000});
      return false;
    }
    //传成功之后进行请求
    $.ajax({
      url:"http://www.chitunion.com/api/Channel/UploadChannelResource",
      type:"post",
      data: {
        FilePath: FilePath,
        ChannelID: ChannelID
      },
      xhrFields: {
        withCredentials: true
      },
      crossDomain: true,
      dataType:"json",
      beforeSend: function () {
        $("#material_two").html('<div style="margin:150px auto; width:32px;"><img src="/images/loading.gif"></div>');
        $("#material_two").show();
      },
      success: function (data){
        console.log(data);
        if(data.Status == 0){
          if(data.Result.ErrorList.length == 0 ){
            $("#material").hide();
            //刷新提示信息
            window.onbeforeunload = function (e) {
                e = e || window.event;
                  // 兼容IE8和Firefox 4之前的版本
                if (e) {
                   e.returnValue = '关闭提示';
                }
                // Chrome, Safari, Firefox 4+, Opera 12+ , IE 9+
                return '关闭提示';
            };
            layer.msg("上传成功",{time:2000});
            $("#box_one").hide();
            $("#box_two").show();
            //渲染页面信息
            $("#tab").html(ejs.render($("#up_success").html(),{data:data.Result.Data}));
            console.log(ChannelName);
            console.log(CooperateDate);
            $("#ChannelName_two").html(ChannelName);
            $("#CooperateDate span").html(CooperateDate);
            //点击确定验证通过之后会出现通过的页面信息
            //点击切换不同维度的信息
            $(".tab_menu li").off("click").on("click",function () {
              var tmp = $(this).attr("tmp");
              $(".tab_menu li").removeClass('selected');
              $(this).addClass('selected');
              $("#table").html(ejs.render($("#"+tmp).html(),{data:data.Result.Data}));
            })
            for(var i=0;i<$(".tab_menu span").length;i++){
              if($(".tab_menu span")[i].innerHTML != 0){
                $(".tab_menu li").eq(i).trigger('click');
                break;
              }
            }

            //点击预览进入预览页面
            $("#preview").on("click", function () {
              window.onbeforeunload = null;
              // $("#preview").attr("target","_blank");
              // $("#preview").attr("href","/ChannelManager/preview.html?ChannelID="+ChannelID);
              window.location = "/ChannelManager/preview.html?ChannelID="+ChannelID;
            });

          }else{
            // layer.msg(data.Message,{time:2000});
            // $(".specification").show();
            $("#question").html("");
            var str = "";
            for(var i=0;i<data.Result.ErrorList.length;i++){
              str += data.Result.ErrorList[i]+"</br>"
            }
            $("#question").html(str);
            $("#material").show();
            $("#material_two").hide();
          }
        }else{
          layer.msg(data.Message,{time:2000});
          $("#material_two").hide();
        }
      }
    })
  })
  $("#cancel").off("click").on("click",function () {
    // $("#cancel").attr("target","_blank");
    $("#cancel").attr("href","/ChannelManager/channelList.html");
  })



  //上传资源函数
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
         //  'buttonClass': 'but_upload',
          'swf': '/Js/uploadify.swf?_=' + Math.random(),
          'uploader': 'http://www.chitunion.com/AjaxServers/UploadFile.ashx', //请求保存路径
          'auto': true,
          'multi': false,
          'width': 100,
          'height': 20,
          'formData': { Action: 'BatchImport', CarType: '', LoginCookiesContent: escapeStr(getCookie('ct-uinfo')) },
          'fileTypeDesc': '支持格式:xls,xlsx',
          'fileTypeExts': '*.xls;*.xlsx',
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
               $("#material").hide();
               $("#alert_file").hide();
              if (response == true) {
                  var json = $.evalJSON(data);
                  FilePath = json.Msg;
                  console.log(FilePath);
                  $('#'+id).parents('a').next().text(json.FileName);
                  $('#'+id).parents('a').next().next().show().attr("href", json.Msg);
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
