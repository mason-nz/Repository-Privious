/*
 * Written by:     liyr
 * function:       新增微信公众号
 * Created Date:   2017-02-23
 * Modified Date:
 */
$(function () {
    /*验证当前用户身份*/
    userId();
    var bussinesstype = 14001;//业务类型bussinesstype（微信：14001 APP：14002 微博：14003 视频：14004 直播：14005）
    var operatetype = parseInt($.getUrlParam('operatetype'));  //对应业务类型bussinesstype的操作(1:add 2:edit)
    /*男女比例*/
    sexProportion("male");
    sexProportion("female");
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
        $("title").text("编辑微信公众号-媒体管理-我的赤兔");
        $("h2").html("编辑微信公众号");
        $("h3").eq(0).css("display","none");
        var MediaID = parseInt($.getUrlParam('MediaID'));
        var obj = {MediaType:MediaType,MediaID:MediaID};  //MediaID列表项某一条的id
        weChatData(obj);
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

/*传输的字段*/
function ValidateInfo(save) {
    var MediaID = parseInt($.getUrlParam('MediaID') || -1);
    var bussinesstype = 14001;//int业务类型bussinesstype（微信：14001 APP：14002 微博：14003 视频：14004 直播：14005）
    var operatetype = parseInt($.getUrlParam('operatetype'));  //int对应业务类型bussinesstype的操作(1:add 2:edit)
    /*公共参数*/
    var Number = $.trim($(".weChatId").val());  //微信号string
    var Name = $.trim($(".wechatName").val());  //微信名称string
    var HeadIconURL = $.trim($(".headImg").attr("src"));  //头像地址string
    var FansCount =  $.trim($(".fansCount").val());//粉丝数 int
    var LevelType =  $.trim($('.mediaLevel input:radio[name="level"]:checked').val()); //媒体级别（意见领袖或普通) int
    var CategoryID =  $.trim($('select[name="industryClassify"]').find("option:selected").val()); //行业分类 int
    /*所在地区*/
    var ProvinceID =  $.trim($("#ddlProvince1").find("option:selected").val()); //省份int
    var CityID =  $.trim($("#ddlCity1").find("option:selected").val()); //市int
    /*覆盖区域string*/
    var CoverageArea = "";
    $("#checkArea").find("li").not(":first").each(function () {
        CoverageArea +=','+ $(this).attr('data-Provinceid')+'-'+ $(this).attr('data-Cityid');
    });
    CoverageArea = CoverageArea.substring(1);

    var Sign = $.trim($("textarea").val()); //描述/签名
    /*微信参数*/
    var TwoCodeURL = $(".Tcode").attr("src");//二维码截图
    if(CTLogin.RoleIDs == $.trim("SYS001RL00003")){  //媒体主
        FansCountURL = $(".fansImg").attr("src");//粉丝数截图
    }else{  //其他
        var FansCountURL = "-1";//粉丝数截图
    }

    var FansMalePer = "" + Math.round(($("#male").val())*100)/100;//粉丝男比例(保留2位小数)
    var FansFemalePer ="" + Math.round(($("#female").val())*100)/100;//粉丝女比例(保留2位小数)
    var count = parseInt(FansFemalePer) + parseInt(FansMalePer);

    var AreaID = $.trim($('select[name="mediaArea"]').find("option:selected").val()); //	媒体领域枚举
    var OrderRemark = "";
    $('input:checkbox[name="orderRemark"]').each(function () {
        if($(this).prop("checked")){
            OrderRemark +=","+ $(this).attr("data-id");//下单备注（枚举）
        }
    });
    OrderRemark = OrderRemark.substring(1);
    var IsAuth = $('input:radio[name="identification"]:checked').val(); //是否微信认证
    var IsReserve =$('input:radio[name="appointment"]:checked').val(); //是否预约
    /*互动参数*/
    var ReferReadCount =  $.trim($(".referReadCount").val());//参考阅读数
    var AveragePointCount =  $.trim($(".averagePointCount").val()); //平均点赞数
    var MoreReadCount =  $.trim($(".moreReadCount").val());  //10W+阅读文章数
    var OrigArticleCount =  $.trim($(".origArticleCount").val());  //原创文章数
    var UpdateCount =  $.trim($(".updateCount").val()); //更新次数
    var MaxinumReading =  $.trim($(".maxinumReading").val());  //最高阅读数
    if(Number == "" || Name == "" || HeadIconURL == "" || TwoCodeURL == "" ||  FansCount == "" || FansCount <= 500 || FansCountURL == "" || count > 100 || FansMalePer == "" || FansFemalePer == "" || CategoryID == "-1" ||  AreaID == "-1" || CoverageArea == "" || LevelType == undefined || IsAuth == undefined || Sign == "" || IsReserve == undefined || ReferReadCount == "" || AveragePointCount == "" || MoreReadCount == "" || OrigArticleCount == "" || UpdateCount == "" || MaxinumReading == ""){
        ValidateType();
    }else{
        ValidateType();  //清除所有的提示信息
        FansCount = parseInt(FansCount);
        LevelType = parseInt(LevelType);
        CategoryID = parseInt(CategoryID);
        ProvinceID = parseInt(ProvinceID);
        CityID = parseInt(CityID);
        FansMalePer = parseFloat(FansMalePer);
        FansFemalePer = parseFloat(FansFemalePer);
        AreaID = parseInt(AreaID);
        IsAuth = Boolean(parseInt($('input:radio[name="identification"]:checked').attr("data-id")));//是否微信认证
        IsReserve = Boolean(parseInt($('input:radio[name="appointment"]:checked').attr("data-id"))); //是否预约
        ReferReadCount = parseInt(ReferReadCount);
        AveragePointCount = parseInt(AveragePointCount);
        MoreReadCount = parseInt(MoreReadCount);
        OrigArticleCount = parseInt(OrigArticleCount);
        UpdateCount = parseInt(UpdateCount);
        MaxinumReading = parseInt(MaxinumReading);
        var obj  = {businesstype:bussinesstype,operatetype:operatetype,Number:Number,Name:Name,HeadIconURL:HeadIconURL,FansCount:FansCount,LevelType:LevelType,CategoryID:CategoryID,ProvinceID:ProvinceID,CityID:CityID,CoverageArea:CoverageArea,Sign:Sign,TwoCodeURL:TwoCodeURL,FansCountURL:FansCountURL,FansMalePer:FansMalePer,FansFemalePer:FansFemalePer,AreaID:AreaID,IsAuth:IsAuth,IsReserve:IsReserve,ReferReadCount:ReferReadCount,AveragePointCount:AveragePointCount,MoreReadCount:MoreReadCount,OrigArticleCount:OrigArticleCount,OrderRemark:OrderRemark,UpdateCount:UpdateCount,MaxinumReading:MaxinumReading,MediaID:MediaID};
        weChatAjax(obj,save);
    }
}

/*保存信息的接口*/
function weChatAjax(obj,save) {
    setAjax({url:'/api/media/curd',type:'post',data:obj},
        function(data){
            if(save == 0){  //保存按钮
                if(data.Status != 0){
                    layer.alert(data.Message, function(index){
                        layer.close(index);
                    })
                }else{
                    window.location.href='/MediaManager/mediaWeChatList.html';
                }

            }else if(save == 1){  //保存并新增按钮
                if(data.Status != 0){
                    layer.alert(data.Message, function(index){
                        layer.close(index);
                    })
                }else{
                    window.location.href='/MediaManager/weChat_addEdit.html?operatetype=1';
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
                    window.location.href='/PublishManager/addEditPublish-wechat.html?isAdd=0&MediaID='+MediaID;
                }

            }
        },
        function(data){
        }
    );
}

/*编辑时页面数据的渲染*/
function weChatData(obj) {
    setAjax({url:'/api/Media/GetMediaDetail',type:'get',data:obj},
        function(data){
            var dataMediaInfo = data.Result.MediaInfo;
            $('.weChatId').val(dataMediaInfo.Number);  //微信号
            $('.weChatId').attr("disabled",true); 
            $('.wechatName').val(dataMediaInfo.Name);  //微信名称
            $('#headimgUploadFile').attr("src",dataMediaInfo.HeadIconURL);  //头像
            $('#headBigImg').attr("src",dataMediaInfo.HeadIconURL);
            $('#codeimgUploadFile').attr("src",dataMediaInfo.TwoCodeURL);  //二维码
            $('#codeBigImg').attr("src",dataMediaInfo.TwoCodeURL);

            $('#fansimgUploadify').attr("src",dataMediaInfo.FansCountURL);  //粉丝截图
            $('#fansBigImg').attr("src",dataMediaInfo.FansCountURL);  //粉丝截图
            $('.fansCount').val(dataMediaInfo.FansCount);  //粉丝数
            $('#male').val(dataMediaInfo.FansMalePer);  //男粉比例
            $('#female').val(dataMediaInfo.FansFemalePer);  //女粉比例
            $('select[name="industryClassify"]').val(dataMediaInfo.CategoryID);  //行业分类
            $('#mediaArea').val(dataMediaInfo.AreaID);  //媒体领域
            $('select[name="ddlProvince1"]').val(dataMediaInfo.ProvinceID);  //省份
            BindCity('ddlProvince1', 'ddlCity1');  //需要重新调用二级联动函数，进行数据渲染
            $('select[name="ddlCity1"]').val(dataMediaInfo.CityID);  //城市

            $('input[name="level"]').each(function () {
                if($(this).attr("value") == dataMediaInfo.LevelType){
                    $(this).attr('checked',"checked");  //媒体级别
                }
            });

            $('input[name="identification"]').each(function () {
                if(Boolean(parseInt($(this).attr("data-id"))) == dataMediaInfo.IsAuth){
                    $(this).attr('checked',"checked");  //微信认证
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
            
            /*互动参数*/
            var dataInteractionInfo = data.Result.InteractionInfo;
            $(".referReadCount").val(dataInteractionInfo.ReferReadCount);
            $(".averagePointCount").val(dataInteractionInfo.AveragePointCount);
            $(".moreReadCount").val(dataInteractionInfo.MoreReadCount);
            $(".origArticleCount").val(dataInteractionInfo.OrigArticleCount);
            $(".updateCount").val(dataInteractionInfo.UpdateCount);
            $(".maxinumReading").val(dataInteractionInfo.MaxinumReading);
        },
        function(data){
        }
    );
}

/*男女粉丝性别比*/
function sexProportion(id) {
    $('#'+id).focus(function () {
        $(".fansProp").css("display","none");
        $('#fans').css("display", "none");
        $(".FanssexCount").css("display","none");
    }).blur(function () {
        var malePro = parseFloat($("#male").val());
        var femalePro = parseFloat($("#female").val());
        var count = malePro + femalePro;
        if($(this).val() > 100){
            $('#fans').css("display", "block");
        }
        if(malePro === "" && femalePro === ""){
            $(".fansProp").css("display","block");
        }else if(count <= 0){
            $('.FanssexCount').css("display", "block");
        }else if(count > 100){
            $('#fans').css("display", "block");
        }
    });
}


