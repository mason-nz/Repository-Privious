/**
 * Created by fengb on 2017/4/27.
 */

$(function () {

    var suborderid = GetQueryString('suborderid')!=null&&GetQueryString('suborderid')!='undefined'?GetQueryString('suborderid'):null;
    var isSelected = GetQueryString('isSelected')!=null&&GetQueryString('isSelected')!='undefined'?GetQueryString('isSelected'):null;


    var config = {};
    var InitFeedBackData = {};

    var DetailOfOrder = {
        constructor : DetailOfOrder,
        init : function () {
            var url = 'http://www.chitunion.com/api/ADOrderInfo/GetBySubOrderID_ADOrderInfo?v=1_1';
            //var url = 'js/OrderDetail.json';
            setAjax({
                url : url,
                type : 'get',
                data : {
                    suborderid  : suborderid
                }
            },function (data) {
                var Result = data.Result;
                config = data.Result;
                config.MediaType = config.SubADInfo.MediaType;

                if(data.Status == 0){
                    //订单基本信息
                    $('#OrderInfo').html(ejs.render($('#wisdom-OrderInfo').html(),Result));

                    setAjax({
                        url : 'http://www.chitunion.com/api/ADOrderInfo/SelectPostingAddress?v=1_1',
                        type : 'get',
                        data : {
                            suborderid  : suborderid
                        }
                    },function(data){
                        if(data.Status == 0){
                            //发文地址渲染
                            $('.post_address').html(ejs.render($('#post_address').html(),data));
                        }else{
                            layer.msg(data.Message,{'time':1000});
                        }
                    })

                    //初始化打开上传数据的页面
                    if(isSelected == 1){
                        $('#tab .tab_menu li').eq(0).addClass('selected').siblings().removeClass('selected');
                        $('.tab_box .box').eq(0).show().siblings().hide();
                    }else if(isSelected == 2){
                        $('#tab .tab_menu li').eq(1).addClass('selected').siblings().removeClass('selected');
                        $('.tab_box .box').eq(1).show().siblings().hide();

                        var addetailID = config.SubADInfo.SelfDetails[0].PublishDetailID;//广告位ID
                        var CreateUserID = config.ADOrderInfo.CreateUserID;//创建人ID
                        var SubOrderID = suborderid;//订单号
                        $.openPopupLayer({
                            name: "popLayerDemo",
                            url: "./myWeixin_popup.html",
                            error: function (dd) { layer.alert(dd.status); },
                            success:function () {
                                uploadFile();
                                DetailOfOrder.closeLayer();//关闭弹窗

                                $(".layer_data").find("input[name=Username]").each(function(){
                                    $(this).on("input",function(){
                                        replaceAndSetPos(this,/[^0-9]/g,'');
                                    })
                                });
                                //提交信息
                                $("#submitMessage").click(function (e) {
                                    e.preventDefault();
                                    var ReadCount = $.trim($(".readCount").val()),//阅读数
                                        ClickCount = $.trim($(".clickCount").val()),//点赞数，
                                        TransmitCount = $.trim($(".transmitCount").val()),//转发数
                                        LinkCount = $.trim($(".linkCount").val()),//原文阅读点击数
                                        PVCount = $.trim($(".pvCount").val()),//PV数
                                        UVCount = $.trim($(".uvCount").val()),//UV数
                                        OrderCount = $.trim($(".orderCount").val()),//订单数变为线索量
                                        //上传附件
                                        FilePath = $(".DataCapture").attr("FilePathList").split(','),//上传附件  string
                                        FeedbackEndDate = $.trim($(".feedbackEndDate").val());//结束日期 string

                                    //提示信息
                                    var str = "";
                                    if(ReadCount == ""){
                                        str += "阅读数不能为空";
                                    }else if(FeedbackEndDate == ""){
                                        str += "截止时间不能为空";
                                    }else if(FilePath == ""){
                                        str += "请上传附件";
                                    }else{
                                        var obj ={MediaType:14001,SubOrderID:SubOrderID,ADDetailID:addetailID,CreateUserID:CreateUserID,ReadCount:ReadCount,ClickCount:ClickCount,TransmitCount:TransmitCount,LinkCount:LinkCount,PVCount:PVCount,UVCount:UVCount,OrderCount:OrderCount,FilePathList:FilePath,FeedbackEndDate:FeedbackEndDate};
                                        DetailOfOrder.getCheckAlreday(obj,addetailID);//判断数据日期是否存在
                                    }
                                    $(".toast").html(str);
                                });
                            }
                        });
                    }

                    DetailOfOrder.returnStateText(Result.SubADInfo.Status);
                    //广告位列表
                    $('#ADInfo').html(ejs.render($('#wisdom-order').html(),Result));
                    DetailOfOrder.ModifyAmount();
                    DetailOfOrder.LayoutChanges();
                    DetailOfOrder.showTemplate();//渲染不同的模板
                    DetailOfOrder.PostAddress();//发文地址
                }else{
                    layer.msg(data.Message,{'time':1000});
                }
            })
        },
        showTemplate : function () {//反馈数据
            var url = 'http://www.chitunion.com/api/FeedbackData/SelectFeedbackData';
            setAjax({
                url : url,
                type : 'get',
                data : {
                    SubOrderID : suborderid,
                    MediaType : config.MediaType
                }
            },function(data){
                if(data.Status == 0){
                    var FeedBackData = data.Result;
                    InitFeedBackData = FeedBackData;
                    DetailOfOrder.getTotal();//总计
                    switch(config.MediaType){
                        case 14001:
                            $('#FeedBackData').html(ejs.render($('#FeedBackDataTemplate-weixin').html(),{data:FeedBackData}));
                            break;
                        case 14002:
                            $('#FeedBackData').html(ejs.render($('#FeedBackDataTemplate-app').html(),{data:FeedBackData}));
                            break;
                        case 14003:
                            $('#FeedBackData').html(ejs.render($('#FeedBackDataTemplate-weibo').html(),{data:FeedBackData}));
                            break;
                        case 14004:
                            $('#FeedBackData').html(ejs.render($('#FeedBackDataTemplate-shipin').html(),{data:FeedBackData}));
                            break;
                        case 14005:
                            $('#FeedBackData').html(ejs.render($('#FeedBackDataTemplate-zhibo').html(),{data:FeedBackData}));
                            break;
                        default:break;
                    }
                    DetailOfOrder.clickLayer(config.MediaType);//加载弹窗
                    DetailOfOrder.deleteData(config.MediaType);//删除数据
                    DetailOfOrder.operatingBtn();//按钮操作
                    DetailOfOrder.FeedBackShowPhotos();//查看图片
                }else{
                    layer.msg(data.Message,{'time':1000});
                }
            })
        },
        getTotal : function(){//总计
            var data = InitFeedBackData;
            data.forEach(function(item){
                var total = {
                    "ClickCount":0,
                    "CommentCount":0,
                    "DeliveredCount":0,
                    "LinkCount":0,
                    "OrderCount":0,
                    "PVCount":0,
                    "ReadCount":0,
                    "TransmitCount":0,
                    "UVCount":0,
                    "Value":0,
                    "FeedbackBeginDate":"总计",
                    "FeedbackEndDate":"总计"
                }
                item.DataList.forEach(function(items){
                    total.ClickCount += items.ClickCount;
                    total.CommentCount += items.CommentCount;
                    total.DeliveredCount += items.DeliveredCount;
                    total.LinkCount += items.LinkCount;
                    total.OrderCount += items.OrderCount;
                    total.PVCount += items.PVCount;
                    total.ReadCount += items.ReadCount;
                    total.TransmitCount += items.TransmitCount;
                    total.UVCount += items.UVCount;
                    total.Value += items.Value;
                })
                var ClickRate = total.LinkCount/total.PVCount;
                total.ClickRate = formatMoney(ClickRate,2,"");
                item.DataList.push(total);
            })
        },
        clickLayer : function(type){//加载弹层

            var addetailID = config.SubADInfo.SelfDetails[0].PublishDetailID;//广告位ID
            var CreateUserID = config.ADOrderInfo.CreateUserID;//创建人ID
            var SubOrderID = suborderid;//订单号

            $(".upload_btn").off("click").on("click",function(e){
                e.preventDefault();
                if(type == 14001){//微信
                    $.openPopupLayer({
                        name: "popLayerDemo",
                        url: "./myWeixin_popup.html",
                        error: function (dd) { layer.alert(dd.status); },
                        success:function () {
                            uploadFile();
                            DetailOfOrder.closeLayer();//关闭弹窗

                            $(".layer_data").find("input[name=Username]").each(function(){
                                $(this).on("input",function(){
                                    replaceAndSetPos(this,/[^0-9]/g,'');
                                })
                            });

                            //提交信息
                            $("#submitMessage").click(function (e) {
                                e.preventDefault();
                                var ReadCount = $.trim($(".readCount").val()),//阅读数
                                    ClickCount = $.trim($(".clickCount").val()),//点赞数，
                                    TransmitCount = $.trim($(".transmitCount").val()),//转发数
                                    LinkCount = $.trim($(".linkCount").val()),//原文阅读点击数
                                    PVCount = $.trim($(".pvCount").val()),//PV数
                                    UVCount = $.trim($(".uvCount").val()),//UV数
                                    OrderCount = $.trim($(".orderCount").val()),//订单数变为线索量
                                    //上传附件
                                    FilePath = $(".DataCapture").attr("FilePathList").split(','),//上传附件  string
                                    FeedbackEndDate = $.trim($(".feedbackEndDate").val());//结束日期 string

                                //提示信息
                                var str = "";
                                if(ReadCount == ""){
                                    str += "阅读数不能为空";
                                }else if(FeedbackEndDate == ""){
                                    str += "截止时间不能为空";
                                }else if(FilePath == ""){
                                    str += "请上传附件";
                                }else{
                                    var obj ={MediaType:type,SubOrderID:SubOrderID,ADDetailID:addetailID,CreateUserID:CreateUserID,ReadCount:ReadCount,ClickCount:ClickCount,TransmitCount:TransmitCount,LinkCount:LinkCount,PVCount:PVCount,UVCount:UVCount,OrderCount:OrderCount,FilePathList:FilePath,FeedbackEndDate:FeedbackEndDate};
                                    DetailOfOrder.getCheckAlreday(obj,addetailID);//判断数据日期是否存在
                                }
                                $(".toast").html(str);
                            });
                        }
                    });
                }else if(type == 14002){//app
                    $.openPopupLayer({
                        name: "popLayerDemo",
                        url: "./myApp_popup.html",
                        error: function (dd) { layer.alert(dd.status); },
                        success:function () {
                            uploadFile();
                            DetailOfOrder.closeLayer();//关闭弹窗
                            $(".layer_data").find("input[name=Username]").each(function(){
                                $(this).on("input",function(){
                                    replaceAndSetPos(this,/[^0-9]/g,'');
                                })
                            });
                            //点击率  点击数/pv  保留两位小数
                            $(".linkCount").on("blur",function(){
                                var PVCount = $.trim($(".pvCount").val()),//PV数
                                    LinkCount = $.trim($(".linkCount").val()),//点击数
                                    ClickRate = formatMoney(LinkCount/PVCount*100,2,"");
                                $(".clickRate").html(ClickRate);
                            });

                            //提交信息
                            $("#submitMessage").click(function (e) {
                                e.preventDefault();
                                var PVCount = $.trim($(".pvCount").val()),//PV数
                                    UVCount = $.trim($(".uvCount").val()),//UV数
                                    LinkCount = $.trim($(".linkCount").val()),//点击数
                                    ClickRate = formatMoney(LinkCount/PVCount,2,"");
                                //上传附件
                                FilePath = $.trim($("#downloadFile").attr("href")),//上传附件  string
                                    FeedbackBeginDate = $.trim($(".feedbackBeginDate").val()),//开始日期 string
                                    FeedbackEndDate = $.trim($(".feedbackEndDate").val());//结束日期 string
                                //执行周期
                                //var beiginTime = $(".beiginTime").text().split(" ")[0];
                                //var endTime = $(".endTime").text().split(" ")[0];

                                //提示信息
                                var str = "";
                                if(PVCount == ""){
                                    str += "PV数不能为空";
                                }else if(UVCount == ""){
                                    str += "UV数不能为空";
                                }else if(FilePath == ""){
                                    str += "请上传附件";
                                }else if(FeedbackBeginDate == "" || FeedbackEndDate == ""){
                                    str += "数据日期不能为空";
                                }else if(FeedbackBeginDate > FeedbackEndDate){
                                    str += "开始日期不能大于结束日期";
                                }/*else if(FeedbackBeginDate < beiginTime || FeedbackEndDate > endTime){
                                 str += "数据日期必须在执行周期内";
                                 }*/else{
                                    var obj ={MediaType:type,SubOrderID:SubOrderID,ADDetailID:addetailID,CreateUserID:CreateUserID,PVCount:PVCount,UVCount:UVCount,LinkCount:LinkCount,ClickRate:ClickRate,FilePath:FilePath,FeedbackBeginDate:FeedbackBeginDate,FeedbackEndDate:FeedbackEndDate};
                                    DetailOfOrder.getCheckAlreday(obj,addetailID);//判断数据日期是否存在
                                }
                                $(".toast").html(str);
                            });
                        }
                    });
                }else if(type == 14003){//微博
                    $.openPopupLayer({
                        name: "popLayerDemo",
                        url: "./myWeibo_popup.html",
                        error: function (dd) { layer.alert(dd.status); },
                        success:function () {
                            uploadFile();
                            DetailOfOrder.closeLayer();//关闭弹窗
                            $(".layer_data").find("input[name=Username]").each(function(){
                                $(this).on("input",function(){
                                    replaceAndSetPos(this,/[^0-9]/g,'');
                                })
                            });
                            //提交信息
                            $("#submitMessage").click(function (e) {
                                e.preventDefault();
                                var ReadCount = $.trim($(".readCount").val()),//阅读数
                                    TransmitCount = $.trim($(".transmitCount").val()),//转发数
                                    ClickCount = $.trim($(".clickCount").val()),//点赞数
                                    CommentCount = $.trim($(".commentCount").val()),//评论数
                                    LinkCount = $.trim($(".linkCount").val()),//链接点击数
                                    PVCount = $.trim($(".pvCount").val()),//PV数
                                    UVCount = $.trim($(".uvCount").val()),//UV数
                                    OrderCount = $.trim($(".orderCount").val()),//订单数
                                    FilePath = $.trim($("#downloadFile").attr("href")),//上传附件  string
                                    FeedbackBeginDate = $.trim($(".feedbackBeginDate").val()),//开始日期 string
                                    FeedbackEndDate = $.trim($(".feedbackEndDate").val());//结束日期 string
                                var CreateUserID = $("#creatUser").attr("creat-id");//创建人ID
                                //执行时间
                                //var beiginTime = $(".beiginTime").text().split(" ")[0];
                                //提示信息  阅读数 送达数 数据日期，必填写项
                                var str = "";
                                if(ReadCount == ""){
                                    str += "阅读数不能为空";
                                }else if(FilePath == ""){
                                    str += "请上传附件";
                                }else if(FeedbackBeginDate == "" || FeedbackEndDate == ""){
                                    str += "数据日期不能为空";
                                }else if(FeedbackBeginDate > FeedbackEndDate ){
                                    str += "开始日期不能大于结束日期";
                                }/*else if(FeedbackBeginDate < beiginTime){
                                 str += "数据日期必须大于等于执行时间";
                                 }*/else{
                                    var obj ={MediaType:type,SubOrderID:SubOrderID,ADDetailID:addetailID,CreateUserID:CreateUserID,ReadCount:ReadCount,TransmitCount:TransmitCount,ClickCount:ClickCount,CommentCount:CommentCount,LinkCount:LinkCount,PVCount:PVCount,UVCount:UVCount,OrderCount:OrderCount,FilePath:FilePath,FeedbackBeginDate:FeedbackBeginDate,FeedbackEndDate:FeedbackEndDate};

                                    DetailOfOrder.getCheckAlreday(obj,addetailID);//判断数据日期是否存在
                                }
                                $(".toast").html(str);
                            });
                        }
                    });
                }else if(type == 14004){//视频
                    $.openPopupLayer({
                        name: "popLayerDemo",
                        url: "./myShipin_popup.html",
                        error: function (dd) { layer.alert(dd.status); },
                        success:function () {
                            uploadFile();
                            DetailOfOrder.closeLayer();//关闭弹窗
                            $(".layer_data").find("input[name=Username]").each(function(){
                                $(this).on("input",function(){
                                    replaceAndSetPos(this,/[^0-9]/g,'');
                                })
                            });
                            $("#submitMessage").click(function (e) {
                                e.preventDefault();
                                var ReadCount = $.trim($(".readCount").val()),//观看数
                                    TransmitCount = $.trim($(".transmitCount").val()),//曝光数
                                    FilePath = $.trim($("#downloadFile").attr("href")),//上传附件  string
                                    FeedbackBeginDate = $.trim($(".feedbackBeginDate").val()),//开始日期 string
                                    FeedbackEndDate = $.trim($(".feedbackEndDate").val());//结束日期 string
                                var CreateUserID = $("#creatUser").attr("creat-id");//创建人ID
                                //执行时间
                                //var beiginTime = $(".beiginTime").text().split(" ")[0];
                                //提示信息  阅读数 送达数 数据日期，必填写项
                                var str = "";
                                if(ReadCount == ""){
                                    str += "观看数不能为空";
                                }else if(FilePath == ""){
                                    str += "请上传附件";
                                }else if(FeedbackBeginDate == "" || FeedbackEndDate == ""){
                                    str += "数据日期不能为空";
                                }else if(FeedbackBeginDate > FeedbackEndDate){
                                    str += "开始日期不能大于结束日期";
                                }/*else if(FeedbackBeginDate < beiginTime){
                                 str += "数据日期必须大于等于执行时间";
                                 }*/else{
                                    var obj ={MediaType:type,SubOrderID:SubOrderID,ADDetailID:addetailID,CreateUserID:CreateUserID,ReadCount:ReadCount,TransmitCount:TransmitCount,FilePath:FilePath,FeedbackBeginDate:FeedbackBeginDate,FeedbackEndDate:FeedbackEndDate};
                                    DetailOfOrder.getCheckAlreday(obj,addetailID);//判断数据日期是否存在
                                }
                                $(".toast").html(str);

                            });
                        }
                    });
                }else if(type == 14005){//直播
                    $.openPopupLayer({
                        name: "popLayerDemo",
                        url: "./myZhibo_popup.html",
                        error: function (dd) { layer.alert(dd.status); },
                        success:function () {
                            uploadFile();
                            DetailOfOrder.closeLayer();//关闭弹窗
                            $(".layer_data").find("input[name=Username]").each(function(){
                                $(this).on("input",function(){
                                    replaceAndSetPos(this,/[^0-9]/g,'');
                                })
                            });
                            $("#submitMessage").click(function (e) {
                                e.preventDefault();
                                var ReadCount = $.trim($(".readCount").val()),//总观看人数
                                    TransmitCount = $.trim($(".transmitCount").val()),//峰值
                                    Value = $.trim($(".value").val()),//虚拟礼物价值
                                    ClickCount = $.trim($(".clickCount").val()),//题及数
                                    FilePath = $.trim($("#downloadFile").attr("href")),//上传附件  string
                                    FeedbackBeginDate = $.trim($(".feedbackBeginDate").val()),//开始日期 string
                                    FeedbackEndDate = $.trim($(".feedbackEndDate").val());//结束日期 string
                                var CreateUserID = $("#creatUser").attr("creat-id");//创建人ID
                                //执行时间
                                //var beiginTime = $(".beiginTime").text().split(" ")[0];
                                //提示信息  阅读数 送达数 数据日期，必填写项
                                var str = "";
                                if(ReadCount == ""){
                                    str += "总观看人数不能为空";
                                }else if(FilePath == ""){
                                    str += "请上传附件";
                                }else if(FeedbackBeginDate == "" || FeedbackEndDate == ""){
                                    str += "数据日期不能为空";
                                }else if(FeedbackBeginDate > FeedbackEndDate){
                                    str += "开始日期不能大于结束日期";
                                }/*else if(FeedbackBeginDate < beiginTime){
                                 str += "数据日期必须大于等于执行时间";
                                 }*/else{
                                    var obj ={MediaType:type,SubOrderID:SubOrderID,ADDetailID:addetailID,CreateUserID:CreateUserID,ReadCount:ReadCount,TransmitCount:TransmitCount,Value:Value,ClickCount:ClickCount,FilePath:FilePath,FeedbackBeginDate:FeedbackBeginDate,FeedbackEndDate:FeedbackEndDate};

                                    DetailOfOrder.getCheckAlreday(obj,addetailID);//判断数据日期是否存在
                                }
                                $(".toast").html(str);
                            })
                        }
                    });
                }
            })
        },
        getCheckAlreday : function(obj,addetailID){//检查日期是否覆盖交叉

            var arr = [];//列表里面已经有的日期数据
            var FeedbackBeginDate = obj.FeedbackBeginDate;//当前输入的开始日期 c
            var FeedbackEndDate = obj.FeedbackEndDate;//当前输入的结束日期 d

            /*获取列表里面有的日期  然后与现在输入的日期循环判断*/
            var mediaType = config.SubADInfo.MediaType;
            var SubOrderID = config.SubADInfo.SubOrderID;//订单号
            var url = "http://www.chitunion.com/api/FeedbackData/SelectFeedbackData?suborderid="+SubOrderID+"&MediaType="+mediaType;

            setAjax({
                url : url,
                type : "get"
            },function(data){
                var DetailData = data.Result;
                if(status == 0){
                    DetailData.forEach(function(item){
                        if(item.ADDetailID == addetailID){
                            item.DataList.forEach(function(items){
                                var obj = {};
                                obj.beginDate = items.FeedbackBeginDate;
                                obj.endDate = items.FeedbackEndDate;
                                arr.push(obj);
                            })
                        }
                    })
                    /*当列表里面什么都没有的情况下  arr就是空数组*/
                    if(arr.length < 1){
                        DetailOfOrder.safeData(obj,addetailID);
                    }else{
                        for(var i = 0; i < arr.length; i++){
                            //列表里面已经有的数据日期
                            var bt =  arr[i].beginDate; //a
                            var et =  arr[i].endDate; //b

                            var btLast = arr[arr.length-1];
                            var etLast = arr[arr.length-1];
                            if(FeedbackEndDate<bt || FeedbackBeginDate>et){
                                if(i==(arr.length-1)){
                                    if(FeedbackEndDate<btLast || FeedbackBeginDate>etLast) {
                                        DetailOfOrder.safeData(obj,addetailID);
                                    }
                                }
                                continue;
                            }else{
                                if(FeedbackBeginDate==bt && FeedbackEndDate==et){
                                    layer.confirm('你确认要覆盖之前的数据吗', {
                                        btn: ['确认','取消'] //按钮
                                    }, function(){
                                        DetailOfOrder.safeData(obj,addetailID);
                                        layer.closeAll();
                                    },function(){
                                        layer.closeAll();
                                    });
                                    break;
                                }else{
                                    DetailOfOrder.safeData(obj,addetailID);//交叉的情况
                                    break;
                                }
                            }
                        }
                    }
                }else{
                    layer.msg(data.Message,{'time':1000});
                }
            })
        },
        safeData : function(obj){//弹窗数据保存
            var url = 'http://www.chitunion.com/api/FeedbackData/InserFeedbackData';
            setAjax({
                url:url,
                type:'post',
                data: obj
            },function(data){
                if(data.Status == 0){
                    DetailOfOrder.showTemplate();//渲染数据
                    DetailOfOrder.FeedBackShowPhotos();//查看图片
                    $.closePopupLayer('popLayerDemo');

                    $('#tab .tab_menu li').eq(1).addClass('selected').siblings().removeClass('selected');
                    $('.tab_box .box').eq(1).show().siblings().hide();

                }else{
                    layer.msg(data.Message,{'time':1000});
                }
            });
        },
        deleteData : function(type){

            $(".delete").each(function(){
                var FeedbackID = $(this).attr("data-id"),//id
                    FileUrl = $(this).attr("data-url");//url
                $(this).on("click",function(e){
                    e.preventDefault();
                    layer.confirm('您是否确认删除此数据吗', {
                        btn: ['确认','取消'] //按钮
                    }, function(){
                        var url = "http://www.chitunion.com/api/FeedbackData/DeleteFeedbackData?MediaType="+type+"&FeedbackID="+FeedbackID+"&FileUrl="+FileUrl;
                        setAjax({url:url,type:'get'},
                            function(data){
                                if(data.Status == 0){
                                    DetailOfOrder.showTemplate();//渲染数据
                                }else{
                                    layer.msg(data.Message,{'time':1000});
                                }
                            }
                        );
                        layer.closeAll();
                    });
                })
            })
        },
        closeLayer : function(){//关闭弹层
            //取消
            $("#cancleMessage").off("click").on("click",function(e){
                e.preventDefault();
                $.closePopupLayer('popLayerDemo');
            });

            //关闭弹窗
            $("#closebt").off("click").on("click",function(e){
                console.log(1)
                e.preventDefault();
                $.closePopupLayer('popLayerDemo');
            });
        },
        returnStateText : function(status) {//状态码
            switch (status) {
                case 16001:
                    $('.subOrderStatus').text('草稿');
                    break;
                case 16002:
                    $('.subOrderStatus').text('待审核');
                    break;
                case 16003:
                    $('.subOrderStatus').text('待执行');
                    break;
                case 16004:
                    $('.subOrderStatus').text('执行中');
                    break;
                case 16005:
                    $('.subOrderStatus').text('已取消');
                    break;
                case 16006:
                    $('.subOrderStatus').text('已驳回');
                    break;
                case 16007:
                    $('.subOrderStatus').text('已删除');
                    break;
                case 16008:
                    $('.subOrderStatus').text('执行完毕');
                    break;
                case 16009:
                    $('.subOrderStatus').text('订单完成');
                    break;
                default:
                    break;
            }
        },
        ModifyAmount : function() {// 计算总额和个数

            var SalePrice = 0;//销售
            var OriginalReferencePrice = 0;//原创
            var CostReferencePrice = 0;//成本
            var FinalCostPrice = 0;//最终成本价

            //媒体个数
            var allPositionCount = $('.to_b_list_box .Tr').length;
            $('.allPositionCount').text(allPositionCount);

            $('.to_b_list_box .Tr').each(function () {
                var SalesVal = $(this).attr('SalePrice')*1;
                var OriginVal = $(this).attr('OriginalReferencePrice')*1;
                var coastVal = $(this).attr('CostReferencePrice')*1;
                var FinalVal = $(this).attr('FinalCostPrice')*1;
                SalePrice += SalesVal;
                OriginalReferencePrice += OriginVal;
                CostReferencePrice += coastVal;
                FinalCostPrice += FinalVal;
            })
            //销售参考价
            $('.SalePrice').text(formatMoney(SalePrice,2));
            //原创参考价
            $('.OriginalReferencePrice').text(formatMoney(OriginalReferencePrice,2));
            //成本参考价
            $('.CostReferencePrice').text(formatMoney(CostReferencePrice,2));
            //最终成本价
            $('.FinalCostPrice').text(formatMoney(FinalCostPrice,2));

            //销售参考价总计
            $('.SalePriceTotal').text(formatMoney(SalePrice+OriginalReferencePrice,2));
            //成本参考价总计
            $('.CostReferencePriceTotal').text(formatMoney(CostReferencePrice+OriginalReferencePrice,2));
        },
        StateOfOperation : function () {
            $('.executed').find('span').each(function () {
                switch ($(this).text()){
                    case "取消订单":
                        DetailOfOrder.ChangeStatusHandle($(this),16005);
                        break;
                    case "删除订单":
                        DetailOfOrder.ChangeStatusHandle($(this),16007);
                        break;
                    case "执行订单":
                        DetailOfOrder.ChangeStatusHandle($(this),16004);
                        break;
                    case "执行完毕":
                        DetailOfOrder.ChangeStatusHandle($(this),16008);
                        break;
                    case "订单完成":
                        DetailOfOrder.ChangeStatusHandle($(this),16009);
                        break;
                }
            })
        },
        ChangeStatusHandle : function (DOMTarget,status) {
            DOMTarget.on('click',function () {
                var url = "http://www.chitunion.com/api/ADOrderInfo/CancelOrDelete_SubADInfo?suborderid="+suborderid+"&status="+status;
                switch(status){
                    case 16004://执行订单
                        tipInfo = "确认执行此订单？";
                        DetailOfOrder.TipsInfo(url,tipInfo);
                        break;
                    case 16008://执行完毕
                        tipInfo = "确认将此订单标记为执行完毕？";
                        DetailOfOrder.TipsInfo(url,tipInfo);
                        break;
                    case 16009://订单完成
                        tipInfo = "确认将此订单标记为已完成？";
                        DetailOfOrder.TipsInfo(url,tipInfo);
                        break;
                    case 16005://取消订单
                        tipInfo = "确认取消此订单？";
                        DetailOfOrder.TipsInfo(url,tipInfo);
                        break;
                    case 16007://删除订单
                        tipInfo = "确认删除此订单？";
                        DetailOfOrder.TipsInfo(url,tipInfo);
                        break;
                    default:break;
                }
            })
        },
        TipsInfo : function (url,tipInfo) {
            layer.open({
                type: 1,
                offset: 'c' ,//具体配置参考：offset参数项
                content: '<div style="padding: 20px 50px;">'+tipInfo+'</div>',
                btn: ["确定","取消"],
                btnAlign: 'c', //按钮居中
                yes: function(){
                    layer.closeAll();
                    setAjax({
                        url:url,
                        type:"get"
                    },function(data){
                        if(data.Status == 0){
                            alert('执行成功');
                            layer.closeAll();
                        }else{
                            alert(data.Message);
                            layer.closeAll();
                        }
                    });
                }
            });
        },
        operatingBtn : function(){
            //执行
            $('.execution_order').on('click',function(){
                var OrderID = $(this).attr('OrderID');
                var status = $(this).attr('status')*1;
                var url = "http://www.chitunion.com/api/ADOrderInfo/CancelOrDelete_SubADInfo?"+"suborderid="+OrderID+"&status="+status;
                var tipInfo = "";

                tipInfo = "确认执行此订单？";
                TipsInfo(url,tipInfo);
            })

            //cancel_order 取消
            $('.cancel_order').on('click',function(){
                var OrderID = $(this).attr('OrderID');
                var status = $(this).attr('status')*1;

                var url = "http://www.chitunion.com/api/ADOrderInfo/CancelOrDelete_SubADInfo?"+"suborderid="+OrderID+"&status="+status;
                var tipInfo = "";

                tipInfo = "确认取消此订单？";
                TipsInfo(url,tipInfo);
            })

            //已完成
            $('.alreday_order').on('click',function(){
                var OrderID = $(this).attr('OrderID');
                var status = $(this).attr('status')*1;
                var url = "http://www.chitunion.com/api/ADOrderInfo/CancelOrDelete_SubADInfo?"+"suborderid="+OrderID+"&status="+status;
                var tipInfo = "";

                tipInfo = "确认将此订单标记为已完成？";
                TipsInfo(url,tipInfo);
            })

            // 提示框函数
            function TipsInfo(url,tipInfo){
                layer.open({
                    type: 1,
                    offset: 'c' ,//具体配置参考：offset参数项
                    content: '<div style="padding: 20px 50px;">'+tipInfo+'</div>',
                    btn: ["确定","取消"],
                    btnAlign: 'c', //按钮居中
                    yes: function(){
                        layer.closeAll();
                        setAjax({
                            url:url,
                            type:"get"
                        },function(data){
                            if(data.Status == 0){
                                DetailOfOrder.init();//初始化 再刷新渲染
                                layer.closeAll();
                            }else{
                                layer.msg(data.Message,{'time':'1000'});
                            }
                        });
                    }
                });
            }
        },
        LayoutChanges : function () {//展开收起
            var flag = true;
            $('#layoutChanges').off('click').on('click',function () {
                var that = $(this);
                if(flag == true) {
                    that.parents('.demand').find('ul').hide();
                    that.find('img').attr('src','../images/collection08.png');
                    flag = false;
                }else{
                    that.parents('.demand').find('ul').show();
                    that.find('img').attr('src','../images/collection09.png');
                    flag = true;
                }
            })
        },
        PostAddress : function(){//发文地址
            $('#PostAddress').off('click').on('click',function(){

                $('.channel_box').show();
                $('.channel_box .layer').show();

                var _width = document.documentElement.clientWidth;
                var _height = document.documentElement.clientHeight;
                var layer_height = $('.channel_box .layer').height();
                var _left = (_width-390)/2;
                var _top = (_height-layer_height)/2;
                
                $('.channel_box').css({'width':_width,'height':_height,'position':'fixed','left':0,'top':0,'background':'rgba(0,0,0,0.7)'});
                $('.channel_box .layer').css({'position':'absolute','left':_left,'top':_top});

                //提交
                $('#submitAddress').on('click',function(){
                    setAjax({
                        url : 'http://www.chitunion.com/api/ADOrderInfo/UpdatePostingAddressBySubID?v=1_1',
                        type : 'post',
                        data : {
                            SubOrderID : suborderid,
                            PostingAddress : $.trim($('#area_address').val())
                        }
                    },function (data) {
                        if(data.Status == 0){
                            $('.channel_box').hide();
                            $('.channel_box .layer').hide();
                        }else{
                            layer.msg(data.Message,{'time':1000});
                        }
                    })
                })

                //关闭弹窗
                $("#closebt2").off("click").on("click",function(e){
                    $('.channel_box').hide();
                    $('.channel_box .layer').hide();
                });

            })
        },
        FeedBackShowPhotos : function () {//上传反馈数据的展示
            //查看图片
            $('.look_photos').off('click').on('click',function () {
                var that = $(this);
                var idx = $(that).parents('.Tr').index()-2;
                var cur_FileInfoList = InitFeedBackData[0].DataList[idx].FileInfoList;
                $.openPopupLayer({
                    name: "popLayerDemoPhotos",
                    url: "./SeePhotos_Popup.html",
                    success:function () {
                        var str = '';
                        for(var i=0;i<=cur_FileInfoList.length-1;i++){
                            str += '<li> '+ cur_FileInfoList[i].FileName +' <img src='+ cur_FileInfoList[i].FilePath +' style="width:400px;height:auto"></li>';
                        }
                        $('#Content_Photos').html(str);
                        //关闭弹窗
                        $("#closebt3").off("click").on("click",function(e){
                            e.preventDefault();
                            $.closePopupLayer('popLayerDemoPhotos');
                        });
                    }
                })
            })
        }
    };

    DetailOfOrder.init();

})

//----------------只能输入数字 Start
/*调用    '/限制的正则表达式/g'   必须以/开头，/g结尾
 *  onkeyup="replaceAndSetPos(this,/[^0-9]/g,'')" oninput ="replaceAndSetPos(this,/[^0-9]/g,'')"
 */
//获取光标位置
function getCursorPos(obj) {
    var CaretPos = 0;
    // IE Support
    if (document.selection) {
        obj.focus (); //获取光标位置函数
        var Sel = document.selection.createRange ();
        Sel.moveStart ('character', -obj.value.length);
        CaretPos = Sel.text.length;
    }
    // Firefox/Safari/Chrome/Opera support
    else if (obj.selectionStart || obj.selectionStart == '0')
        CaretPos = obj.selectionEnd;
    return (CaretPos);
};
//定位光标
function setCursorPos(obj,pos){
    if (obj.setSelectionRange) { //Firefox/Safari/Chrome/Opera
        obj.focus(); //
        obj.setSelectionRange(pos,pos);
    } else if (obj.createTextRange) { // IE
        var range = obj.createTextRange();
        range.collapse(true);
        range.moveEnd('character', pos);
        range.moveStart('character', pos);
        range.select();
    }
};
//替换后定位光标在原处,可以这样调用onkeyup=replaceAndSetPos(this,/[^/d]/g,'');
function replaceAndSetPos(obj,pattern,text){
    if ($(obj).val() == "" || $(obj).val() == null) {
        return;
    }
    var pos=getCursorPos(obj);//保存原始光标位置
    var temp=$(obj).val(); //保存原始值
    obj.value=temp.replace(pattern,text);//替换掉非法值
    //截掉超过长度限制的字串（此方法要求已设定元素的maxlength属性值）
    var max_length = obj.getAttribute? parseInt(obj.getAttribute("maxlength")) : "";
    if( obj.value.length > max_length){
        var str1 = obj.value.substring( 0,pos-1 );
        var str2 = obj.value.substring( pos,max_length+1 );
        obj.value = str1 + str2;
    }
    pos=pos-(temp.length-obj.value.length);//当前光标位置
    setCursorPos(obj,pos);//设置光标
    //el.onkeydown = null;
};
//-----------------只能输入数字 end


/**
 *  上传附件
 * @desc  JQuery扩展，将json字符串转换为对象，需要引用类库JQuery
 * @param   json字符串
 * @return 返回object,array,string等对象
 * @Add=Masj, Date: 2009-12-07
 */
function uploadFile() {
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

    var FilePathList = [];//多个上传的路径集合
    var FilePathName = [];//多个文件名称

    $('#UploadifyDoc').uploadify({
        'buttonText': '+ 上传数据截图',
        'buttonClass': 'but_upload',
        'swf': '/Js/uploadify.swf?_=' + Math.random(),
        'uploader': '/AjaxServers/UploadFile.ashx',
        'auto': true,
        'multi': true,
        'width': 200,
        'height': 35,
        'formData': { Action: 'BatchImport', CarType: '', LoginCookiesContent: escapeStr(getCookie('ct-uinfo')) },
        'fileTypeDesc': '支持格式:jpg,jpeg,png',
        'fileTypeExts': '*.jpg;*.jpeg;*.png;',
        'fileSizeLimit':'10MB',
        'scriptAccess': 'always',
        'onQueueComplete': function (event, data) {},
        'onQueueFull': function () {
            layer.alert('您最多只能上传1个文件！');
            return false;
        },
        'onUploadSuccess': function (file, data, response) {
            if (response == true) {
                var json = $.evalJSON(data);
                FilePathList.push(json.Msg);
                FilePathName.push(json.FileName)
                $(".fileBox").show();
                var str = '';
                for(var i=0;i<=FilePathList.length-1;i++){
                    str += '<li style="width:100%;margin-left: 82px;"><img src="../images/icon18.png" style="padding-right: 10px;"><span style="padding-right: 10px;">'+FilePathName[i]+'</span><a href='+ FilePathList[i]+' target="_blank"><img src="/ImagesNew/icon19.png"></a></li>';
                }
                $(".fileBox").html(str);
                $(".DataCapture").attr("FilePathList",FilePathList);//数据截图 数组
            }
        },
        'onProgress': function (event, queueID, fileObj, data) {},
        'onUploadError': function (event, queueID, fileObj, errorObj) {},
        'onSelectError':function(file, errorCode, errorMsg){
            console.log(errorCode);
        }
    });

};


//获取url 地址参数方法
function GetQueryString(name) {
    var reg = new RegExp("(^|&)"+ name +"=([^&]*)(&|$)");
    var r = window.location.search.substr(1).match(reg);
    if(r!=null)return r[2]; return null;
}

