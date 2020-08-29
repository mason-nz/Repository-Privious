/*
 * Written by:     liyr
 * function:       APP
 * Created Date:   2017-02-25
 * Modified Date:
 */
$(function () {
    inputShowHide("app","appShow"); //对应广告终端的显示
    inputShowHide("wap","wapShow"); //对应广告终端的显示
    var businesstype = 14002;//业务类型businesstype（微信：14001 APP：14002 微博：14003 视频：14004 直播：14005）
    var operatetype = parseInt($.getUrlParam('operatetype'));  //对应业务类型businesstype的操作(1:add 2:edit)
    if(operatetype == 1){
        $(".keep").css("display","block");  //控制显示的按钮

        /*保存验证信息*/
        $(".keep .save").on("click", function () {
            var save = 0;
            ValidateInfo(save);
        });
        /*保存并创建刊例*/
        $(".but_keep_establish").on("click",function () {
            var save = 2;
            ValidateInfo(save);
        });
    }else{
        $("title").text("编辑平台媒体-媒体管理-我的赤兔");
        $("h2").html("编辑平台媒体");
        $("h3").eq(0).css("display","none");
        $(".save").css("display","block");
        $(".but_keep_establish").css("display","none");
        var MediaID = parseInt($.getUrlParam('MediaID'));
        var obj = {MediaType:MediaType,MediaID:MediaID};
        AppData(obj);
        /*保存并创建刊例*/
        $(".save").on("click",function () {
            var save = 0;
            ValidateInfo(save);
        });
    }
});
function ValidateInfo(save) {
    var MediaID = parseInt($.getUrlParam('MediaID') || -1);
    var businesstype = parseInt(14002);//业务类型businesstype（微信：14001 APP：14002 微博：14003 视频：14004 直播：14005）
    var operatetype = parseInt($.getUrlParam('operatetype'));  //对应业务类型businesstype的操作(1:add 2:edit)
    var Name = $.trim($(".mediaName").val());//媒体名称
    if ($(".mediaNameAlert").attr("isrepeat")=="true") {
        Name = "";
    }

    var HeadIconURL = $(".headImg").attr("src");//媒体logo
    var CategoryID = $.trim($('select[name="industryClassify"]').find("option:selected").val());//行业分类
    /*覆盖区域*/
    var CoverageArea = "";
    $("#checkArea").find("li").not(":first").each(function () {
        CoverageArea +=','+ $(this).attr('data-Provinceid')+'-'+ $(this).attr('data-Cityid');
    });
    CoverageArea = CoverageArea.substring(1);
    var ProvinceID = $.trim($("#ddlProvince1").find("option:selected").val()); //所在区域省份
    var CityID = $.trim($("#ddlCity1").find("option:selected").val()); //所在区域市
    var Terminal = "";
    $('input:checkbox[name="adTerminal"]').each(function () {
        if($(this).prop("checked")){
            Terminal +=","+ $(this).attr("data-id");//下单备注（枚举）
        }
    });
    Terminal = Terminal.substring(1);
    if($('.app').prop("checked")){
        var DailyLive = $.trim($(".appDailyLive").val());//APP日活 int
    }else{
        DailyLive = -1;  //当页面中没有此项时，true != ""
    }
    if($('.wap').prop("checked")){
        var DailyIP = $.trim($(".wapDailyIP").val());//WAP/PC日均IP int
    }else{
        DailyIP = -1;  //当页面中没有此项时，true != ""
    }
    var WebSite = $.trim($(".webSite").val());//网址
    var Remark = $.trim($(".remark").val());//媒体介绍
    if(Name == "" || HeadIconURL == "" || CategoryID == "-1" || CoverageArea == ""|| Terminal == "" || DailyLive == "" || DailyIP == "" || WebSite == "" || Remark == ""){
        ValidateType();
    }else{
        ValidateType();  //清除所有提示信息
        CategoryID = parseInt(CategoryID);
        ProvinceID = parseInt(ProvinceID);
        CityID = parseInt(CategoryID);
        DailyLive = parseInt(DailyLive);
        DailyIP = parseInt(DailyIP);
        var obj = {businesstype:businesstype,operatetype:operatetype,Name:Name,HeadIconURL:HeadIconURL,CategoryID:CategoryID,CoverageArea:CoverageArea,ProvinceID:ProvinceID,CityID:CityID,Terminal:Terminal,DailyLive:DailyLive,DailyIP:DailyIP,WebSite:WebSite,Remark:Remark,MediaID:MediaID};
        APPAjax(obj,save);
    }
}

function APPAjax(obj,save) {
    setAjax({url:'/api/media/curd',type:'post',data:obj},
        function(data){
            if(save == 0){  //保存按钮
                if(data.Status != 0){
                    layer.alert(data.Message, function(index){
                        layer.close(index);
                    })
                }else{
                    window.location.href='/MediaManager/mediaPlatform.html';
                }

            }else if(save == 2){  //保存并添加刊例
                var operatetype = parseInt($.getUrlParam('operatetype'));  //int对应业务类型businesstype的操作(1:add 2:edit)
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
                    window.location.href='/PublishManager/addEditPublish-app.html?isAdd=0&MediaID='+MediaID;
                }

            }
        },
        function(data){
        }
    );
}

function AppData(obj) {
    setAjax({url:'/api/Media/GetMediaDetail',type:'get',data:obj},
        function(data){
            var dataMediaInfo = data.Result.MediaInfo;
            $(".mediaName").val(dataMediaInfo.Name);  //媒体名称
            $(".mediaName").attr("disabled",true); 
            $("#headimgUploadFile").attr("src",dataMediaInfo.HeadIconURL); //媒体LOGO
            $("#headBigImg").attr("src",dataMediaInfo.HeadIconURL); //媒体LOGO
            $('select[name="industryClassify"]').val(dataMediaInfo.CategoryID);//所在行业

            /*覆盖地区*/
            var dataOverlayIDs = data.Result.OverlayIDs;
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
            /*广告终端*/
            var Terminal = dataMediaInfo.Terminal.split(",");
            for(var i=0;i<Terminal.length;i++){
                $('input:checkbox[name="adTerminal"]').each(function () {
                    if($(this).attr('data-id') == Terminal[i]){
                        $(this).attr("checked","checked");
                        $('.'+$(this).val()+'').css("display","block");
                        if(dataMediaInfo.DailyLive == -1){
                            $(".appDailyLive").val("");
                        }else{
                            $(".wapDailyIP").val(dataMediaInfo.DailyIP);
                        }
                        if(dataMediaInfo.DailyIP == -1  ){
                            $(".wapDailyIP").val("");
                        }else{
                            $(".wapDailyIP").val(dataMediaInfo.DailyIP);
                        }

                    }
                });
            }
            //app日活
            if(dataMediaInfo.appDailyLive == 0){
                            $(".appDailyLive").val("");
                        }else{
                            $(".appDailyLive").val(dataMediaInfo.DailyLive);
                        }
            $(".webSite").val(dataMediaInfo.WebSite);  //网址
            $(".remark").val(dataMediaInfo.Remark);  //媒体介绍
        },
        function(data){
        }
    );
}


