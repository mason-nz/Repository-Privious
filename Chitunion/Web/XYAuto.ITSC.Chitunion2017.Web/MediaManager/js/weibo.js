/*
 * Written by:     liyr
 * function:       weibo
 * Created Date:   2017-02-27
 * Modified Date:
 */
$(function () {
    userId(); //判断身份
    var bussinesstype = 14003;//业务类型bussinesstype（微信：14001 APP：14002 微博：14003 视频：14004 直播：14005）
    var operatetype = parseInt($.getUrlParam('operatetype'));  //对应业务类型bussinesstype的操作(1:add 2:edit)
    // var MediaType = 14003;//媒体分类（除PC,APP外）
    if(operatetype == 1){
        /*保存验证信息*/
        $(".keep .save").on("click", function () {
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
        $("title").text("编辑新浪微博账号-媒体管理-我的赤兔");
        $("h2").html("编辑微博媒体");
        $("h3").eq(0).css("display","none");
        var MediaID = parseInt($.getUrlParam('MediaID'));
        var obj = {MediaType:MediaType,MediaID:MediaID};  //MediaID列表项某一条的id
        weiboData(obj);
        /*保存验证信息*/
        $(".keep .save").on("click", function () {
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
    /*公共参数*/
    var bussinesstype = 14003;//业务类型bussinesstype（微信：14001 APP：14002 微博：14003 视频：14004 直播：14005）
    var operatetype = parseInt($.getUrlParam('operatetype'));  //对应业务类型bussinesstype的操作(1:add 2:edit)
    var Number = $.trim($(".weiBoId").val());  //微博号
    var Name = $.trim($(".weiBoName").val());  //微博名称
    if ($(".mediaNameAlert").attr("isrepeat") == "true") {
        Name = "";
    }
    var HeadIconURL = $(".headImg").attr("src");  //头像地址
    var FansCount = $.trim($(".fansCount").val());//粉丝数
    var LevelType = $.trim($('.mediaLevel input:radio[name="level"]:checked').val()); //媒体级别（意见领袖或普通）
    var CategoryID = $.trim($('select[name="industryClassify"]').find("option:selected").val()); //行业分类
    var ProvinceID = $.trim($("#ddlProvince1").find("option:selected").val()); //所在地区省份
    var CityID = $.trim($("#ddlCity1").find("option:selected").val()); //所在地区市
    var Sign = $.trim($("textarea").val()); //描述/签名
    /*覆盖区域*/
    var CoverageArea = "";
    $("#checkArea").find("li").not(":first").each(function () {
        CoverageArea +=','+ $(this).attr('data-Provinceid')+'-'+ $(this).attr('data-Cityid');
    });
    CoverageArea = CoverageArea.substring(1);
    var Sex = $.trim($('input:radio[name="sex"]:checked').attr("data-id"));//性别 int
    if(CTLogin.RoleIDs == $.trim("SYS001RL00003")){  //媒体主
        FansCountURL = $(".fansImg").attr("src");//粉丝数截图
    }else{  //其他
        var FansCountURL = "-1";//粉丝数截图
    }
    var FansSex = $.trim($('input:radio[name="fansSex"]:checked').attr("data-id"));//粉丝性别 int
    var AreaID = $.trim($('select[name="mediaArea"]').find("option:selected").val()); //	媒体领域枚举
    var Profession =  $.trim($('select[name="profession"]').find("option:selected").val());//职业
    var AuthType =  $.trim($('input:radio[name="identification"]:checked').val());//认证枚举
    var OrderRemark = "";
    $('input:checkbox[name="orderRemark"]').each(function () {
        if($(this).prop("checked")){
            OrderRemark +=","+ $(this).attr("data-id");//下单备注（枚举）
        }
    });
    OrderRemark = OrderRemark.substring(1);
    var IsReserve = $('input:radio[name="appointment"]:checked').val();//预约情况
    var Source = 1;//来源枚举
    /*互动参数*/
    var MediaType = 14003;//媒体分类（除PC,APP外）
    var AverageForwardCount = $.trim($(".averageForwardCount").val());//平均转发数
    var AveragePointCount = $.trim($(".averagePointCount").val()); //平均点赞数
    var AverageCommentCount = $.trim($(".averageCommentCount").val());  //平均评论数
    // var ScreenShotURL = 1; //互动参数截图/
    if(Number == "" || Name == "" || HeadIconURL == "" || FansCount == "" || FansCount <= 500 || FansCountURL == "" || FansSex == "" || CategoryID == "-1" || AreaID == "-1" || Profession == "-1" || CoverageArea == "" || LevelType == undefined || AuthType == undefined || Sign == "" || IsReserve == undefined  || AverageForwardCount == "" || AveragePointCount == ""  || AverageCommentCount == ""){
        ValidateType();
    }else{
        ValidateType(); //清除所有的提示信息
        FansCount = parseInt(FansCount);
        LevelType = parseInt(LevelType);
        CategoryID = parseInt(CategoryID);
        ProvinceID = parseInt(ProvinceID);
        CityID = parseInt(CityID);
        Sex = parseInt(Sex);
        FansSex = parseInt(FansSex);
        AreaID = parseInt(AreaID);
        Profession = parseInt(Profession);
        AuthType = parseInt(AuthType);
        AreaID = parseInt(AreaID);
        IsReserve = Boolean(parseInt($('input:radio[name="appointment"]:checked').attr("data-id")));
        var obj = {businesstype:bussinesstype,operatetype:operatetype,Number:Number,Name:Name,Sex:Sex,HeadIconURL:HeadIconURL,FansCount:FansCount,FansCountURL:FansCountURL,FansSex:FansSex,CategoryID:CategoryID,AreaID:AreaID,Profession:Profession,ProvinceID:ProvinceID,CityID:CityID,CoverageArea:CoverageArea,LevelType:LevelType,AuthType:AuthType,Sign:Sign,OrderRemark:OrderRemark,IsReserve:IsReserve,AverageForwardCount:AverageForwardCount,AveragePointCount:AveragePointCount,AverageCommentCount:AverageCommentCount,MediaID:MediaID};
        weiboAjax(obj,save);
    }
}

function weiboAjax(obj,save) {
    setAjax({url:'/api/media/curd',type:'post',data:obj},
        function(data){
            if(save == 0){  //保存按钮
                if(data.Status != 0){
                    layer.alert(data.Message, function(index){
                        layer.close(index);
                    })
                }else{
                    window.location.href='/MediaManager/mediaBlogList.html';
                }
            }else if(save == 1){  //保存并新增按钮
                if(data.Status != 0){
                    layer.alert(data.Message, function(index){
                        layer.close(index);
                    })
                }else{
                    window.location.href='/MediaManager/weibo_addEdit.html?operatetype=1';
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
                    window.location.href='/PublishManager/addEditPublish-blog.html?isAdd=0&MediaID='+MediaID;
                }
            }
        },
        function(data){
        }
    );
}

function weiboData(obj) {
    setAjax({url:'/api/Media/GetMediaDetail',type:'get',data:obj},
        function(data){
            var dataMediaInfo = data.Result.MediaInfo;
            $(".weiBoId").val(dataMediaInfo.Number);  //微博号
            $(".weiBoId").attr("disabled",true);
            $(".weiBoName").val(dataMediaInfo.Name);  //微博昵称
            $('#headimgUploadFile').attr("src",dataMediaInfo.HeadIconURL);  //头像
            $('#headBigImg').attr("src",dataMediaInfo.HeadIconURL);  //头像
            $('input[name="sex"]').each(function () {//性别
                if($.trim($(this).attr("data-id")) == $.trim(dataMediaInfo.Sex)){
                    $(this).attr('checked',"checked");
                }
            });

            $(".fansCount").val(dataMediaInfo.FansCount);
            $('#fansimgUploadify').attr("src",dataMediaInfo.FansCountURL);
            $('#fansBigImg').attr("src",dataMediaInfo.FansCountURL);
            $('input[name="fansSex"]').each(function () {
                if($.trim($(this).attr("data-id")) == $.trim(dataMediaInfo.FansSex)){
                    $(this).attr('checked',"checked");
                }
            });
            $('select[name="industryClassify"]').val(dataMediaInfo.CategoryID);  //行业分类
            $('#mediaArea').val(dataMediaInfo.AreaID);  //媒体领域
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
                if($(this).attr("value") == dataMediaInfo.AuthType){
                    $(this).attr('checked',"checked");  //微博认证
                }
            });
            $(".sign").val(dataMediaInfo.Sign); //描述/签名

            $('input:checkbox[name="orderRemark"]').each(function () {  //下单备注
                if(dataMediaInfo.OrderRemark != null){
                    var  OrderRemark = dataMediaInfo.OrderRemark.split(",");
                    for(var i=0;i<OrderRemark.length;i++){
                        if($.trim($(this).attr("data-id")) == $.trim(OrderRemark[i])){
                            $(this).attr('checked',"checked");
                        }
                    }
                }
                
            });

            $('input[name="appointment"]').each(function () {
                if(Boolean(parseInt($(this).attr("data-id"))) == dataMediaInfo.IsReserve){
                    $(this).attr('checked',"checked");  //预约情况
                }
            });
            /*互动参数*/
            var dataInteractionInfo = data.Result.InteractionInfo;
            $(".averageForwardCount").val(dataInteractionInfo.AverageForwardCount);
            $(".averageCommentCount").val(dataInteractionInfo.AverageCommentCount);
            $(".averagePointCount").val(dataInteractionInfo.AveragePointCount);
        },
        function(data){
        }
    );
}
