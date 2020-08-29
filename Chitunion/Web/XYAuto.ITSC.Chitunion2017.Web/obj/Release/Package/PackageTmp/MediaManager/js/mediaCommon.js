
$(function () {
    console.log(CTLogin);//返回登陆Json对象
    $(".right").children(":first-child").find("a").html(CTLogin.UserName);
    existValidate();   //媒体账号名称存在性验证
    BindProvince('ddlProvince'); //绑定覆盖区域省份
    BindProvince('ddlProvince1'); //绑定所在地区省份
    $("#ddlCity1").children().eq(0).text("所在地区-市");
    $("#ddlProvince").children().eq(1).before("<option value='0'>全国</option>");
    $("#ddlProvince1").children().eq(0).text("所在地区 -- 省");

    $("#ddlProvince1").change(function () {   //改变所在地区的默认值
        $("#ddlCity1").children().eq(0).text("所在地区 -- 市");
    });

    $("#ddlProvince").change(function () {   //选择全国的时候不能选择城市
            $(".sameArea").css("display","none");
            $("#overlay").css("display","none");
        if($("#ddlProvince").val() == '0'){
            $(".sameArea").css("display","none");
            $("#ddlCity").find('option').remove();
        }
    });
    addArea();  //添加地区
    industrySort();  //行业分类/
    mediaArea();  //媒体领域
    profession();  //职业分类
    platForm();   //所属平台
    FansCount();  //粉丝数验证
    uploadImg("headUploadify","headimgUploadFile","headimgErr","headBigImg");  //头像
    uploadImg("codeUploadify","codeimgUploadFile","codeimgErr","codeBigImg");  //二维码
    uploadImg("fansUploadify","fansimgUploadify","fansimgErr","fansBigImg");  //粉丝截图
    showImg();  //显示大图
    Validate();
    alertInfo();
});
/*获取url*/
$(function () {
    $.getUrlParam = function(name){
        var reg = new RegExp("(^|&)"+ name +"=([^&]*)(&|$)");
        var r = window.location.search.substr(1).match(reg);
        if (r!=null) return unescape(r[2]); return null;
    }
});

/*省市信息*/
var crmCustCheckHelper = (function () {
    var triggerProvince = function () {//选中省份
            BindCity('ddlProvince', 'ddlCity');
            // BindCounty('ddlProvince', 'ddlCity', 'ddlCounty');
        },

        triggerCity = function () {//选中城市
            // BindCounty('ddlProvince', 'ddlCity', 'ddlCounty');
        };
    var  triggerProvince1 = function () {
        BindCity('ddlProvince1', 'ddlCity1');
    };

    return {
        triggerProvince: triggerProvince,
        triggerProvince1: triggerProvince1,
        triggerCity: triggerCity
    };
})();

/*获取当前媒体类型*/
function changeType(mediaType) {
    MediaType = mediaType;
}

/*（APP）广告终端的显示*/
function inputShowHide(name,className) {
    $("."+name).on("click",function (){
        if($(this).prop("checked")){
            $('.'+className).css("display","block");
        }else{
            $('.'+className).css("display","none");
        }
    })
}

/*验证当前用户的身份*/
function userId() {
    /*不同角色按钮的显示*/
    var operatetype1 = $.getUrlParam('operatetype');
    if(CTLogin.RoleIDs == $.trim("SYS001RL00003")){  //媒体主
        if(operatetype1 == 1){
            $(".but_keep_establish").css("display","block");
            $(".save").css("display","none");
            $(".but_keep_add").css("display","none");
            $(".but_keep_establish").css({background:"#FF9100",color:"#fff"});
        }else{
            $(".but_keep_establish").css("display","none");
            $(".save").css("display","block");
            $(".but_keep_add").css("display","none");
        }

    }else if(CTLogin.RoleIDs == $.trim("SYS001RL00005")){   //AE
        if(operatetype1 == 1){
            $(".keep").children("a").css("display","block");
        }else{
            $(".but_keep_establish").css("display","none");
            $(".save").css("display","block");
            $(".but_keep_add").css("display","none");
        }
    }else{  //其他角色
        if(operatetype1 == 1){
            $(".keep").children("a").css("display","block");
        }else{
            $(".but_keep_establish").css("display","none");
            $(".save").css("display","block");
            $(".but_keep_add").css("display","none");
        }
    }
    /*粉丝截图的显示*/
    if(CTLogin.RoleIDs == $.trim("SYS001RL00003")){  //媒体主
        $(".fansCountImg").css("display","block");
    }else{
        $(".fansCountImg").css("display","none");
    }
}

/*添加所在地区*/
function addArea() {
    $(".saveArea").off("click").on("click",function () {
        var ddlProvince = $("#ddlProvince option:checked");
        var ddlCity = $("#ddlCity option:checked");
        if(ddlProvince.val() == -1 && $("#checkArea").find("li").not(":first").length == 0){
            $("#checkArea").css("display","none");
        }
        else{
                $("#checkArea").css("display","block");
                $("#checkArea").find("li").each(function (i) {
                    if(($.trim(ddlProvince.text()) == $.trim($(this).text().split("-")[0]) )&& (ddlCity.val() == -1) ){  
                        //城市没有选择时 省份不能相同
                        $(".sameArea").css("display","block");                       
                        return false;
                    }
                    else  if(($.trim(ddlProvince.text()) == $.trim($(this).text().split("-")[0]) )&&($.trim(ddlCity.text()) == $.trim($(this).text().split("-")[1]) )){  
                        //选择的省份、城市 都相同时
                        $(".sameArea").css("display","block");                       
                        return false;
                    }
                    else if($.trim(ddlProvince.text()) == $.trim($(this).text().split("-")[0])  && $.trim($(this).text().split("-")[1]) ==""){ 
                        //已经选择了省份，再次选相同省份下的城市时
                        $(".sameArea").css("display","block");                       
                        return false;
                    }
                    else if(ddlProvince.val() == -1){
                        $("#overlay").css("display","block");
                        return false;
                    }else if($(this).attr("data-Provinceid") == 0){
                        $(".sameArea").css("display","block");
                        return false;
                    }else if(ddlProvince.val() == 0){
                        if($("#checkArea").find("li").not(':first').length != 0){
                            $(".sameArea").css("display","block");
                            return false;
                        }else{
                            $(".sameArea").css("display","none");
                            $(".area").before('<li data-Cityid="0" data-Provinceid="'+ddlProvince.val()+'">' + ddlProvince.text() + '<img src="/images/icon22.png" onclick="deleteArea()"/></li>');
                        }
                    }


                    if(i == $("#checkArea").find("li").length-1){
                        if(ddlCity.val() == -1){
                            $(".area").before('<li data-Cityid="0" data-Provinceid="'+ddlProvince.val()+'">' + ddlProvince.text() + '<img src="/images/icon22.png" onclick="deleteArea()"/></li>');
                        }else{
                            $(".area").before('<li data-Cityid="'+ddlCity.val()+'" data-Provinceid="'+ddlProvince.val()+'">' + ddlProvince.text() + '-'+ddlCity.text() +'<img src="/images/icon22.png" onclick="deleteArea()"/></li>');
                        }
                    }
                })
            }
    })
}


/*删除地区*/
function deleteArea() {
    $('#checkArea').delegate('img', 'click', function () {
        var $tr = $(this).parent();
        $tr.remove();
        if( $("#checkArea").find("img").length == 0){
            $("#checkArea").css("display","none");
        }
        if($("#checkArea").find("li").not(":first").length == 0){
            $(".sameArea").css("display","none");
            $("#overlay").css("display","none");
        }
    });
}

/*输入框有值时，提示信息隐藏*/
function Validate() {
    /*文本输入框*/
    $(".editText").each(function () {
        $(this).focus(function () {
            $(this).parent().parent().children(":last-child").prev().css("display", "none");
        }).blur(function () {
            if ($.trim($(this).val()) == "") {
                $(this).parent().parent().children(":last-child").prev().css("display", "block");
            }else{
                $(this).parent().parent().children(":last-child").prev().css("display", "none");
            }
        })
    });
    /*互动参数*/
    $(".parameter input").each(function () {
        $(this).focus(function () {
            $(".parameterInfo").css("display","none");
        }).blur(function () {
            $(".parameterInfo").text("");
            var str = $(".parameterInfo").text();
            $(".parameter input").each(function () {
                if($.trim($(this).val()) == ""){
                    str += $(this).parent().parent().children(":first-child").text().substring(1) + "不能为空/";
                }
            });
            str = str.substring(0,str.length - 1);
            $(".parameterInfo").text(str).css("display","block");

        })
    });
    /*下拉列表*/
    $("select").each(function () {
        $(this).change(function () {
            if($("#ddlCity1").val() == "-1" && $("#ddlProvince1").val() != "-1"){
                $(this).parent().parent().children(":last-child").prev().css("display", "none");
            }

            if ($(this).val() != "-1"){
                    $(this).parent().parent().children(":last-child").prev().css("display", "none");
            }else {
                $(this).parent().parent().children(":last-child").prev().css("display", "block");
            }

            if($('#checkArea').find("li").not(":first").length != 0){
                $(this).parent().parent().children(":last-child").prev().css("display", "none");
            }

    });


    });
    /*单选框*/
    $('input[type="radio"]').each(function () {
       $(this).on("click",function () {
           $(this).parent().parent().children(":last-child").prev().css("display", "none");
       })

    });
    /*复选框*/
    $('input[type="checkbox"]').change(function () {
        if($('input:checkbox:checked').length != 0){
            $(".ad").css("display", "none");
        }else{
            $(".ad").css("display", "block");
        }
    });
}

/*表单验证信息*/
function ValidateType() {
    /*所有的输入框验证*/
    $(".editText").each(function () {
        if ($.trim($(this).val()) == "") {
            $(this).parent().parent().children(":last-child").prev().css("display", "block");
        }else {
            $(this).parent().parent().children(":last-child").prev().css("display", "none");
        }
    });
    /*互动参数数据验证*/
    $(".parameterInfo").text("");
    var str = $(".parameterInfo").text();
    $(".parameter input").each(function () {
        if($.trim($(this).val()) == ""){
            str += $(this).parent().parent().children(":first-child").text().substring(1) + "不能为空/";
        }
    });
    str = str.substring(0,str.length - 1);
    $(".parameterInfo").text(str).css("display","block");
    /**************/
    /*上传图片验证*/
    $(".uploadImg").each(function () {
        if ($.trim($(this).attr("src")) == "") {
            $(this).parent().parent().children(":last-child").prev().css("display", "block");
        } else {
            $(this).parent().parent().children(":last-child").prev().css("display", "none");
        }
    });
    /*下拉列表验证*/
    $("select").each(function () {
        if($("#ddlCity1").val() == "-1" && $("#ddlProvince1").val() != "-1"){
            $(this).parent().parent().children(":last-child").prev().css("display", "none");
        }
        if ($(this).val() == "-1"){
            $(this).parent().parent().children(":last-child").prev().css("display", "block");
        }else {
           $(this).parent().parent().children(":last-child").prev().css("display", "none");
       }

    });
    /*覆盖区域验证*/
    var areas = $("#checkArea li").eq(0).nextAll().not("div").length;
    if(areas != 0){
        $("#overlay").css("display","none");
    }else{
        $("#overlay").css("display","block");
    }
    radioVerify("level");  //媒体级别 微信  微博 直播
    radioVerify("identification");  //认证 微信  微博  直播
    radioVerify("appointment");  //预约情况  除了APP
    radioVerify("sex");  //性别  直播 视频  微博
    radioVerify("fansSex");  //粉丝性别  微博  视频
    /*复选框的验证*/
    if($('input:checkbox:checked').length != 0){
        $(".ad").css("display", "none");
    }else{
        $(".ad").css("display", "block");
    }
}

/*互动参数图片鼠标移出显示隐藏*/
function alertInfo() {
    $(".parameter ul li:nth-child(2)").on("mouseover",function () {
        $(this).find("div").css("display","block");
    })
        .on("mouseout",function () {
            $(this).find("div").css("display","none");
        })
}

/*单选框验证*/
function radioVerify(name) {
    var length = $('input[name='+name+']:checked').length;
    if(length == 1){
        $("."+name+"").css("display","none")
    }else{
        $("."+name+"").css("display","block")
    }
}

/*行业分类枚举*/
function industrySort(){
    var typeid = "";
    if(MediaType == 14001){  //微信
        typeid = 20;
        industryAjax(MediaType,typeid);
        MediaType = 14001;
    }else if(MediaType == 14002){  //APP
        typeid = 22;
        industryAjax(MediaType,typeid);
    }else if(MediaType == 14003){   //微博
        typeid = 19;
        industryAjax(MediaType,typeid);
    }else if(MediaType == 14004 || MediaType == 14005){
        typeid = 25;
        industryAjax(MediaType,typeid);
    }
}

/*行业分类接口*/
function industryAjax(MediaType,typeid) {

    setAjax({url:'/api/DictInfo/GetDictInfoByTypeID',type:'get',data:{typeID:typeid}},
        function(data){
            // console.log(data);
            var result = data.Result;
            if(MediaType == 14001){
                var str1 = "";
                for(var i=0;i<result.length;i++){
                    str1+='<option value="'+result[i].DictId+'">'+result[i].DictName+'</option>';
                }
                $(".Industry").html(str1);
            }else if(MediaType == 14002){
                var str2 = "";
                for(var i=0;i<result.length;i++){
                    str2+='<option value="'+result[i].DictId+'">'+result[i].DictName+'</option>';
                }
                $(".Industry").html(str2);
            }else if(MediaType == 14003){
                var str3 = "";
                for(var i=0;i<result.length;i++){
                    str3+='<option value="'+result[i].DictId+'">'+result[i].DictName+'</option>';
                }
                $(".Industry").html(str3);
            }else if(MediaType == 14004 || MediaType == 14005){
                var str4 = "";
                for(var i=0;i<result.length;i++){
                    str4+='<option value="'+result[i].DictId+'">'+result[i].DictName+'</option>';
                }
                $(".Industry").html(str4);  //直播/视频
            }
            $(".industryEnumType").children().eq(0).before("<option value='-1' selected>请选择</option>");
        },
        function(data){
            // console.log(data)
        }
    );
}

/*媒体领域枚举*/
function mediaArea() {
    var typeid = 23;
    mediaAreaAjax(MediaType,typeid);
}

/*媒体领域接口*/
function mediaAreaAjax(MediaType,typeid){
    // 引用实例
    setAjax({url:'/api/DictInfo/GetDictInfoByTypeID',type:'get',data:{typeID:typeid}},
        function(data){
            var result = data.Result;
            var str = "";
            for(var i=0;i<result.length;i++){
                str+='<option value="'+result[i].DictId+'">'+result[i].DictName+'</option>';
            }
            $("#mediaArea").html(str);
            $(".mediaEnumType").children().eq(0).before("<option value='-1' selected>请选择</option>");
        },
        function(data){
            console.log(data)
        }
    );
}

/*职业枚举*/
function profession() {
    var typeid = "";
    if(MediaType == 14003){  //微博
        typeid = 21;
        professionAjax(MediaType,typeid);
    }else if(MediaType == 14004 || MediaType == 14005){
        typeid = 24;
        professionAjax(MediaType,typeid);
    }
}

/*职业枚举接口*/
function professionAjax(MediaType,typeid) {
    setAjax({url:'/api/DictInfo/GetDictInfoByTypeID',type:'get',data:{typeID:typeid}},
        function(data){
            var result = data.Result;
            if(MediaType == 14003){
                var str1 = "";
                for(var i=0;i<result.length;i++){
                    str1+='<option value="'+result[i].DictId+'">'+result[i].DictName+'</option>';
                }
                $(".weiboProfession").html(str1);
            }else{
                var str2 = "";
                for(var i=0;i<result.length;i++){
                    str2+='<option value="'+result[i].DictId+'">'+result[i].DictName+'</option>';
                }
                $(".Profession").html(str2);
            }
            $(".professionEnumType").children().eq(0).before("<option value='-1' selected>请选择</option>");

        }
    );
}

/*所属平台枚举*/
function platForm(){
    var typeid = 26;
    switch(MediaType){
            case 14002:typeid=12;
            break;      
            case 14004:typeid=26;
            break;
            case 14005:typeid=34;
            break;  
    }
    platFormAjax(typeid);
}

/*所属平台接口*/
function platFormAjax(typeid) {
    setAjax({url:'/api/DictInfo/GetDictInfoByTypeID',type:'get',data:{typeID:typeid}},
        function(data){
            // console.log(data);
            var result = data.Result;
                var str1 = "";
                for(var i=0;i<result.length;i++){
                    str1+='<option value="'+result[i].DictId+'">'+result[i].DictName+'</option>';
                }
                $("#plateForm").html(str1);
            $(".platFormEnumType").children().eq(0).before("<option value='-1' selected>请选择</option>");
        }
    );
}

/*媒体账号名称存在性验证*/
function existValidate() {
    $(".mediaId").focus(function () {
        $(this).parent().next("li").css("display", "none");
    }).blur(function () {
        var number = $.trim($(this).val());
        var mediaID = parseInt($.getUrlParam('MediaID') == null ? "0" : $.getUrlParam('MediaID'));
        if (number == "") {
            return false;
        } else {
            var obj = { MediaType: MediaType, Number: number, MediaID: mediaID };
            Validate_NameNumber(obj, "mediaIDAlert");
        }
    });
    /*验证媒体名称*/
    $(".mediaName").not(".wechatName").not(".plateFormName").focus(function () {
        $(this).parent().next("li").css("display", "none");
    }).blur(function () {
        var name = $.trim($(this).val());
        var mediaID = parseInt($.getUrlParam('MediaID') == null ? "0" : $.getUrlParam('MediaID'));
        if(name == ""){
            return false
        }else{
            var obj = { MediaType: MediaType, Name: name, MediaID: mediaID };
            Validate_NameNumber(obj,"mediaNameAlert");
        }
    });
}
function Validate_NameNumber(obj,idName) {
    setAjax({url:'/api/Media/MediaExists',type:'get',data:obj},
        function(data){
        console.log(data);
            if(data.Status == 0){  //为true时存在
                $("." + idName).css("display", "none");
                $("." + idName).attr("isrepeat", "false");
            }else if(data.Status ==1){  //false  不存在
                $("." + idName).css("display", "block");
                $("." + idName).attr("isrepeat", "true");
            }
        },
        function(data){
            console.log(data);
        }
    );
}

/*粉丝数验证*/
function FansCount() {
    $(".fansCount").focus(function () {
        $(".FansCount").css("display","none");
        $(".FansCountControl").css("display","none");
    }).blur(function () {
        if($(this).val() <= 0 && $(this).val() != ""){
            $(".FansCount").css("display","block");
            $(".FansCountControl").css("display","none");
        }else if($(this).val() > 0 && $(this).val() <= 500){
            $(".FansCount").css("display","none");
            $(".FansCountControl").css("display","block");
        }
    });
}

/*上传图片*/
function uploadImg(id,img,imgerr,bigImg) {
    jQuery.extend(
        {
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
    }
    function escapeStr(str) {
        return escape(str).replace(/\+/g, '%2B').replace(/\"/g, '%22').replace(/\'/g, '%27').replace(/\//g, '%2F');
    }

    function postTest() {
               // setAjax({
               //     url: '/api/Authorize/GetMenuInfo',
               //     type: 'get'
               // }, null);
        $.ajax({
            type: "get",
            url: "/api/Authorize/GetMenuInfo",
            //contentType: 'application/json',
            dataType: 'json',
            xhrFields: {
                withCredentials: true
            },
            crossDomain: true,
            //data: JSON.stringify({ ModuleIDs: "Jim,dafd,a,asd,fasd" }),
            success: function (data, status) { }
        });
    }

    function disableConfirmBtn() { $('#btnConfirm').attr('disabled', 'disabled'); }
    function enableConfirmBtn() { $('#btnConfirm').removeAttr('disabled'); }

    var uploadSuccess = true;
    $(document).ready(function () {

        $('#divPopLayer').unbind('click').bind('click', function () {
            $.openPopupLayer({
                name: "popLayerDemo",
                url: "/弹出层页面.html?r=" + Math.random(),
                error: function (dd) { alert(dd.status); }
            });

        });

        $('#'+id).uploadify({
            'auto': true,
            'multi': false,
            'swf': '/Js/uploadify.swf?_=' + Math.random(),
            'uploader': '/AjaxServers/UploadFile.ashx',
            'buttonImage':'/images/icon20.png',
            'width': 27,
            'height': 25,
            'fileTypeDesc': '支持格式:xls,jpg,jpeg,png.gif',
            'fileTypeExts': '*.xls;*.jpg;*.jpeg;*.png;*.gif',
            fileSizeLimit:'2MB',
            'fileCount':1,
            'queueSizeLimit': 1,
            queueID:'imgShow',
            'scriptAccess': 'always',
            'overrideEvents' : [ 'onDialogClose'],
            'formData': { Action: 'BatchImport', CarType: '', LoginCookiesContent: escapeStr(getCookie('ct-uinfo')) },
            'onQueueComplete': function (event, data) {
                //enableConfirmBtn();
            },
            'onQueueFull': function () {
                alert('您最多只能上传1个文件！');
                return false;
            },
            //检测FLASH失败调用
            'onFallback':function(){
                $('#'+imgerr).html('您未安装FLASH控件，无法上传图片！请安装FLASH控件后再试。');
            },
            //上传成功后返回的信息
            'onUploadSuccess': function (file, data, response) {
                if (response == true) {
                    var json = $.evalJSON(data);
                    console.log(json);
                    $('#'+img).attr('src',json.Msg);
                    $("#"+bigImg).attr("src",json.Msg);
                    $("#"+imgerr).html('上传成功！').css('color',"#666");
                    $("#"+imgerr).next().css("display","none")
                }
            },
            'onProgress': function (event, queueID, fileObj, data) { },
            'onUploadError': function (event, queueID, fileObj, errorObj) {
                //enableConfirmBtn();
            },
            'onSelectError':function(file, errorCode, errorMsg){
                //if(this.queueData.filesErrored>0){alert(this.queueData.errorMsg);}
                if (errorCode == SWFUpload.UPLOAD_ERROR.FILE_CANCELLED
                    || errorCode == SWFUpload.UPLOAD_ERROR.UPLOAD_STOPPED) {
                    return false;
                }
                switch(errorCode) {
                    case -100:
                        $('#'+imgerr).html('<img src="/images/icon21.png">上传图片数量超过1个').css('color',"red");
                        break;
                    case -110:
                        $('#'+imgerr).html('<img src="/images/icon21.png">上传图片大小应小于2MB').css('color',"red");
                        break;
                    case -120:

                        break;
                    case -130:
                        $('#'+imgerr).html('<img src="/images/icon21.png">上传图片类型不正确').css('color',"red");
                        break;
                }
            }
        });
    });
}

/*鼠标移入显示大图*/
function showImg() {
    $(".uploadBigimg").each(function () {
        $(this).mousemove(function () {
            if($.trim($(this).attr("src")) != ""){
                $(this).next().css("display","inline");
            }
        }).mouseout(function () {
            $(this).next().css("display","none");
        })
    })
}

