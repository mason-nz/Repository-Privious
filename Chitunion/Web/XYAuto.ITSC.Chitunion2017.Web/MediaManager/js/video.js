/*
 * Written by:     liyr
 * function:       video
 * Created Date:   2017-02-28
 * Modified Date:
 */
$(function () {
    userId(); //判断身份
    var bussinesstype = 14004;//业务类型bussinesstype（微信：14001 APP：14002 微博：14003 视频：14004 直播：14005）
    var operatetype = parseInt($.getUrlParam('operatetype')); //对应业务类型bussinesstype的操作(1:add 2:edit)
    if(operatetype == 1){
        /*保存验证信息*/
        $(".keep .button").on("click", function () {
            var save = 0;
            ValidateInfo(save);
        });
        /*保存并新增*/
        $(".but_keep_add").on("click",function () {
            var save = 1;
            ValidateInfo(save);
        });
        /*保存并创建刊例*/
        $(".but_keep_establish").on("click",function () {
            var save = 2;
            ValidateInfo(save);
        });
    }else{
        $("title").text("编辑视频账号-媒体管理-我的赤兔");
        $("h2").html("编辑视频媒体");
        $("h3").eq(0).css("display","none");
        var MediaID = parseInt($.getUrlParam('MediaID'));
        var obj = {MediaType:MediaType,MediaID:MediaID};  //MediaID列表项某一条的id
        videoData(obj);
        /*保存验证信息*/
        $(".keep .button").on("click", function () {
            var save = 0;
            ValidateInfo(save);
        });
        /*保存并新增*/
        $(".but_keep_add").on("click",function () {
            var save = 1;
            ValidateInfo(save);
        });
        /*保存并创建刊例*/
        $(".but_keep_establish").on("click",function () {
            var save = 2;
            ValidateInfo(save);
        });
    }

});

function ValidateInfo(save) {
    var MediaID = parseInt($.getUrlParam('MediaID') || -1);
    var bussinesstype = 14004;//业务类型bussinesstype（微信：14001 APP：14002 微博：14003 视频：14004 直播：14005）
    var operatetype = parseInt($.getUrlParam('operatetype')); //对应业务类型bussinesstype的操作(1:add 2:edit)
    var Platform = $('select[name="plateForm"]').find("option:selected").val(); //int 所属平台
    var Number =  $.trim($(".plateFormID").val());//string 账号
    var Name = $.trim($(".plateFormName").val()); //string 昵称
    var HeadIconURL = $(".headImg").attr("src");  //string 头像图片
    var Sex = $('input:radio[name="sex"]:checked').val(); //int 性别（未知：-1，男：0，女：1）
    var FansCount = $.trim($(".fansCount").val());//int 粉丝数
    if(CTLogin.RoleIDs == $.trim("SYS001RL00003")){  //媒体主
        FansCountURL = $(".fansImg").attr("src");//粉丝数截图
    }else{  //其他
        var FansCountURL = "-1";//粉丝数截图
    }
    var CategoryID =  $.trim($('select[name="industryClassify"]').find("option:selected").val());//int 行业分类枚举
    var Profession =  $.trim($('select[name="profession"]').find("option:selected").val()); //int 职业枚举
    /*覆盖区域string*/
    var CoverageArea = "";
    $("#checkArea").find("li").not(":first").each(function () {
        CoverageArea +=','+ $(this).attr('data-Provinceid')+'-'+ $(this).attr('data-Cityid');
    });
    CoverageArea = CoverageArea.substring(1);
    /*所在地区*/
    var ProvinceID =  $.trim($("#ddlProvince1").find("option:selected").val()); //省份int
    var CityID =  $.trim($("#ddlCity1").find("option:selected").val()); //市int
    var IsReserve = $('input:radio[name="appointment"]:checked').val(); //int 预约情况
    var LevelType = $('.mediaLevel input:radio[name="level"]:checked').val(); //媒体级别（意见领袖或普通）
    var IsAuth	= $('input:radio[name="identification"]:checked').val();//认证枚举
    /*互动参数*/
    var AveragePlayCount = $.trim($(".averagePlayCount").val());//int  平均播放数
    var AveragePointCount =  $.trim($(".averagePointCount").val()); //int  平均点赞数
    var AverageCommentCount =  $.trim($(".averageCommentCount").val()); //int  平均评论数
    var AverageBarrageCount =  $.trim($(".averageBarrageCount").val()); //int  平均弹幕数
    if(Platform == "-1" || Number == "" || Name == "" || HeadIconURL == "" || Sex == "" || FansCount == "" || FansCount <= 500 || FansCountURL == "" || CategoryID == "-1" || Profession == "-1" || CoverageArea == "" || IsAuth == undefined || LevelType == undefined || IsReserve == undefined || AveragePlayCount == "" || AveragePointCount == "" || AverageCommentCount == "" || AverageBarrageCount == ""){
        ValidateType();
    }else{
        ValidateType();    //请除提示信息
        Platform = parseInt(Platform);
        FansCount = parseInt(FansCount);
        Sex = parseInt($('input:radio[name="sex"]:checked').attr("data-id")); //int 性别（未知：-1，男：0，女：1）
        CategoryID = parseInt(CategoryID);
        Profession = parseInt(Profession);
        ProvinceID = parseInt(ProvinceID);
        CityID = parseInt(CityID);
        IsReserve = Boolean(parseInt($('input:radio[name="appointment"]:checked').attr("data-id")));
        LevelType = parseInt(LevelType);
        IsAuth = Boolean(parseInt($('input:radio[name="identification"]:checked').attr("data-id")));
        AveragePlayCount = parseInt(AveragePlayCount);
        AveragePointCount = parseInt(AveragePointCount);
        AverageCommentCount = parseInt(AverageCommentCount);
        AverageBarrageCount = parseInt(AverageBarrageCount);
        var obj = {businesstype:bussinesstype,operatetype:operatetype,Platform:Platform,Number:Number,Name:Name,HeadIconURL:HeadIconURL,Sex:Sex,FansCount:FansCount,FansCountURL:FansCountURL,CategoryID:CategoryID,Profession:Profession,CoverageArea:CoverageArea,ProvinceID:ProvinceID,CityID:CityID,IsReserve:IsReserve,IsAuth	:IsAuth,LevelType:LevelType,AveragePlayCount:AveragePlayCount,AveragePointCount:AveragePointCount,AverageCommentCount:AverageCommentCount,AverageBarrageCount:AverageBarrageCount,MediaID:MediaID};
        videoAjax(obj,save);
    }
}

function videoAjax(obj,save) {
    setAjax({url:'/api/media/curd',type:'post',data:obj},
        function(data){
            if(save == 0){  //保存按钮
                if(data.Status != 0){
                    layer.alert(data.Message, function(index){
                        layer.close(index);
                    })
                }else{
                    window.location.href='/MediaManager/mediaVideoList.html';
                }

            }else if(save == 1){  //保存并新增按钮
                if(data.Status != 0){
                    layer.alert(data.Message, function(index){
                        layer.close(index);
                    })
                }else{
                    window.location.href='/MediaManager/video_addEdit.html?operatetype=1';
                }

            }else if(save == 2){  //保存并添加刊例
                var operatetype = parseInt($.getUrlParam('operatetype'));  //int对应业务类型bussinesstype的操作(1:add 2:edit)
                if(operatetype == 1){
                    var MediaID = data.Result.MediaId;
                }else if(operatetype == 2){
                    MediaID = parseInt($.getUrlParam('MediaID'));
                }
                if(data.Status != 0){
                    layer.alert(data.Message, function(index){
                        layer.close(index);
                    })
                }else{
                    window.location.href='/PublishManager/addEditPublish-video.html?isAdd=0&MediaID='+MediaID;
                }

            }
        },
        function(data){
        }
    );
}

function videoData(obj) {
    setAjax({url:'/api/Media/GetMediaDetail',type:'get',data:obj},
        function(data){
            var dataMediaInfo = data.Result.MediaInfo;
            $('#plateForm').val(dataMediaInfo.Platform);  //所属平台
            $(".plateFormID").val(dataMediaInfo.Number);  //账号
            $(".plateFormID").attr("disabled",true); 
            $(".plateFormName").val(dataMediaInfo.Name);  //昵称
            $('#headimgUploadFile').attr("src",dataMediaInfo.HeadIconURL);  //头像
            $('#headBigImg').attr("src",dataMediaInfo.HeadIconURL);
            $('input[name="sex"]').each(function () {//性别
                if($.trim($(this).attr("data-id")) == $.trim(dataMediaInfo.Sex)){
                    $(this).attr('checked',"checked");
                }
            });

            $(".fansCount").val(dataMediaInfo.FansCount);
            $('#fansimgUploadify').attr("src",dataMediaInfo.FansCountURL);
            $('#fansBigImg').attr("src",dataMediaInfo.FansCountURL);

            $('select[name="industryClassify"]').val(dataMediaInfo.CategoryID);  //行业分类
            $('#profession').val(dataMediaInfo.Profession);  //职业

            var dataOverlayIDs = data.Result.OverlayIDs;  //覆盖地区
            
            if(dataOverlayIDs.length>0){
                $("#checkArea").css("display","block");
                $('select[name="ddlProvince"]').val(dataOverlayIDs[dataOverlayIDs.length-1].ProvinceID);  //省份
                BindCity('ddlProvince', 'ddlCity');  //需要重新调用二级联动函数，进行数据渲染
                $('select[name="ddlCity"]').val(dataOverlayIDs[dataOverlayIDs.length-1].CityID);  //市份
                for(var i= 0;i<dataOverlayIDs.length;i++){
                    if(dataOverlayIDs[i].CityID == '0'){
                        $(".area").before('<li data-Cityid="'+dataOverlayIDs[i].CityID+'" data-Provinceid="'+dataOverlayIDs[i].ProvinceID+'">' + dataOverlayIDs[i].ProvinceName +'<img src="/images/icon22.png" onclick="deleteArea()"/></li>');
                    }else{
                        $(".area").before('<li data-Cityid="'+dataOverlayIDs[i].CityID+'" data-Provinceid="'+dataOverlayIDs[i].ProvinceID+'">' + dataOverlayIDs[i].ProvinceName + '-'+dataOverlayIDs[i].CityName  +'<img src="/images/icon22.png" onclick="deleteArea()"/></li>');
                    }
                }
            }
            
            /*所在地区*/
            $('select[name="ddlProvince1"]').val(dataMediaInfo.ProvinceID);  //省份
            BindCity('ddlProvince1', 'ddlCity1');  //需要重新调用二级联动函数，进行数据渲染
            $('select[name="ddlCity1"]').val(dataMediaInfo.CityID);  //市份


            $('input[name="level"]').each(function () {
                if($(this).val() == dataMediaInfo.LevelType){
                    $(this).attr('checked',"checked");  //媒体级别
                }
            });
            $('input[name="identification"]').each(function () {
                if($(this).attr("data-id") == dataMediaInfo.AuthType){
                    $(this).attr('checked',"checked");  //是否认证
                }
            });

            $('input[name="appointment"]').each(function () {
                if(Boolean(parseInt($(this).attr("data-id"))) == dataMediaInfo.IsReserve){
                    $(this).attr('checked',"checked");  //预约情况
                }
            });
            /*互动参数*/
            var dataInteractionInfo = data.Result.InteractionInfo;
            $(".averagePlayCount").val(dataInteractionInfo.AveragePlayCount);
            $(".averagePointCount").val(dataInteractionInfo.AveragePointCount);
            $(".averageCommentCount").val(dataInteractionInfo.AverageCommentCount);
            $(".averageBarrageCount").val(dataInteractionInfo.AverageBarrageCount);
        },
        function(data){
        }
    );
}