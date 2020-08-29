/**
 * Created by fengb on 2017/4/27.
 */

$(function () {

    var suborderid = GetQueryString('suborderid')!=null&&GetQueryString('suborderid')!='undefined'?GetQueryString('suborderid'):null;

    //获取url 地址参数方法
    function GetQueryString(name) {
        var reg = new RegExp("(^|&)"+ name +"=([^&]*)(&|$)");
        var r = window.location.search.substr(1).match(reg);
        if(r!=null)return r[2]; return null;
    }

    var config = {};

    var InitFeedBackData = {};

    var DetailOfOrder = {
        constructor : DetailOfOrder,
        init : function () {
            var url = 'http://www.chitunion.com/api/ADOrderInfo/GetBySubOrderID_ADOrderInfo?v=1_1';
            setAjax({
                url : url,
                type : 'get',
                data : {
                    suborderid : suborderid
                }
            },function (data) {
                config = data.Result;
                config.MediaType = config.MediaOrderInfo.MediaType;
                
                //订单基本信息
                $('#OrderInfo').html(ejs.render($('#DetailOfOrderForWeChat').html(),{data:config}));
                //广告位列表
                $('#ADInfo').html(ejs.render($('#ADInfoTemplate-other').html(),{data:config}));

                lookDetailHolidays();//查看节假日 以及链接跳转

                DetailOfOrder.returnStateText(config.SubADInfo.Status);
                DetailOfOrder.ModifyAmount();
                //DetailOfOrder.initMoney();
                DetailOfOrder.showTemplate();//渲染不同的模板
                //DetailOfOrder.StateOfOperation();
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
                    DetailOfOrder.FeedBackShowPhotos();//渲染图片
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
           
            var addetailID = InitFeedBackData[0].ADDetailID;//广告位ID
            var CreateUserID = config.ADOrderInfo.CreateUserID;//创建人ID
            var SubOrderID = config.SubADInfo.SubOrderID;//订单号

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
                                    /*DeliveredCount = $.trim($(".deliverCount").val()),//送达数*/
                                    ClickCount = $.trim($(".clickCount").val()),//点赞数，
                                    TransmitCount = $.trim($(".transmitCount").val()),//转发数
                                    LinkCount = $.trim($(".linkCount").val()),//原文阅读点击数
                                    PVCount = $.trim($(".pvCount").val()),//PV数
                                    UVCount = $.trim($(".uvCount").val()),//UV数
                                    OrderCount = $.trim($(".orderCount").val()),//订单数变为线索量
                                    FilePath = $(".DataCapture").attr("FilePathList").split(','),//上传附件
                                    FeedbackEndDate = $.trim($(".feedbackEndDate").val());//结束日期 string

                                //提示信息
                                var str = "";
                                if(ReadCount == ""){
                                    str += "阅读数不能为空";
                                }else if(FeedbackEndDate == ""){
                                    str += "截止时间不能为空";
                                }else if(FilePath == ""){
                                    str += "请上传数据截图";
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
                data:obj
            },function(data){
                if(data.Status == 0){
                    DetailOfOrder.showTemplate();//渲染数据
                    $.closePopupLayer('popLayerDemo');
                    DetailOfOrder.FeedBackShowPhotos();
                }else{
                    layer.msg(data.Message,{'time':1000});
                }
            });
        },
        deleteData : function(type){
            //删除
            $(".delete").each(function(){
                var FeedbackID = $(this).attr("data-id"),//id
                FileUrl = $(this).attr("data-url");//url
                $(this).on("click",function(e){
                    e.preventDefault();
                    layer.confirm('您是否确认删除此数据吗', {
                            btn: ['确认','取消'] //按钮
                    }, function(){
                        var url = "/api/FeedbackData/DeleteFeedbackData?MediaType="+type+"&FeedbackID="+FeedbackID+"&FileUrl="+FileUrl;
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
                e.preventDefault();
                $.closePopupLayer('popLayerDemo');
            });
        },
        returnStateText : function(status) {
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

            var AllMoney = 0;
            var allPositionCount = 0;

            //总价
            $('.to_b_list .totalPrices').each(function () {
                var val = $(this).parent().attr('lastMoney')*1;
                AllMoney += val;
            })
            $('.allMoney').text(formatMoney(AllMoney,2));

            if(config.SubADInfo.SelfDetails != null){
                var SelfDetailsLen = config.SubADInfo.SelfDetails.length;
                $('.allPositionCount').text(SelfDetailsLen);
            }
            if(config.SubADInfo.APPDetails != null){
                var APPDetailsLen = config.SubADInfo.APPDetails.length;
                $('.allPositionCount').text(APPDetailsLen);
            }
        },
        initMoney : function(){//计算appCPD的 初始化总额

            if(config.SubADInfo.APPDetails != null){
                config.SubADInfo.APPDetails.forEach(function (item,i) {
                    if(item.CPDCPM == '11001'){//CPD
                        var allDays = 0;
                        var Holidays = 0;
                        var workingDays = 0;
                        item.ADSchedules.forEach(function (each,e) {
                            allDays += each.AllDays;
                            Holidays += each.Holidays;
                            workingDays = allDays-Holidays;
                        })

                        var cur = $('.to_b_list').find('.cpdApp').eq(i);
                        var originalprice = cur.attr('originalprice')*1;
                        var priceHoliday = cur.attr('priceholiday')*1;
                        //赋给初始化变量  到每一行里面
                        cur.attr('allDays',allDays);//总时间
                        cur.attr('Holidays',Holidays);//假期时间
                        cur.attr('workingDays',workingDays);//工作日时间
                        //每一行的总价  然后赋值给tr
                        var total = Holidays*priceHoliday + workingDays*originalprice;
                        cur.find('.totalPrices').val(total);
                        cur.find('.totalPrice').attr('totalprice',total);
                        cur.attr('AdjustPrice',total);
                        //已失效已过期的情况下不算总额   那就默认渲染
                        cur.find('.init_total').html(total);
                        DetailOfOrder.ModifyAmount();
                        //只有一个排期的时候 初始化把删除按钮隐藏
                        if(item.ADSchedules.length == 1){
                            cur.find('.app_delete').hide();
                        }
                    }
                })
            }

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
        FeedBackShowPhotos : function () {//上传反馈数据的展示
            //查看图片
            $('.look_photos').off('click').on('click',function () {
                var that = $(this);
                var idx = $(that).parents('.Tr').index()-2;
                var cur_FileInfoList = InitFeedBackData[0].DataList[idx].FileInfoList;
                //console.log(cur_FileInfoList);
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
                        $("#closebt2").off("click").on("click",function(e){
                            e.preventDefault();
                            $.closePopupLayer('popLayerDemoPhotos');
                        });
                    }
                })
            })
        }
    };

    DetailOfOrder.init();

    //鼠标悬浮查看节假日 以及添加链接
    function lookDetailHolidays() {

        $('.look_holidays').each(function () {

            var that = $(this);
            that.off('mouseenter').on('mouseenter',function (e) {
                e.preventDefault();
                e.stopPropagation();
                var detailDays = that.next().html();
                that.parents('.appTime').css('position','relative');
                that.parents('.appTime').find('.SuspensionElements').show();
                that.parents('.appTime').find('.SuspensionElements').html(detailDays);
            })
            that.off('mouseleave').on('mouseleave',function (e) {
                e.preventDefault();
                e.stopPropagation();
                that.parents('.appTime').find('.SuspensionElements').hide();
            })

        })

        //无用的tr链接
        $('.uselessTr').each(function () {
            var that = $(this);
            var HasOtherPublish = that.attr('HasOtherPublish')*1;
            var expired = that.attr('expired')*1;
            var MediaType = that.attr('MediaType')*1;
            var MediaID = that.attr('MediaID')*1;
            var TemplateID = that.attr('TemplateID')*1;

            if(MediaType == 14001){//微信
                if(HasOtherPublish == 1){//跳转到详情页
                    that.find('.content a').eq(0).on('click',function () {
                        $(this).attr('target','_blank');
                        $(this).attr('href','/OrderManager/wx_detail.html?MediaType='+MediaType+'&MediaID='+MediaID);
                    })
                }else if(HasOtherPublish == 0){//提示
                    that.find('.content a').eq(0).on('click',function () {
                        alert('很抱歉，该广告已下架或被删除');
                    })
                }else if(expired == -1){//已失效
                    that.find('.content a').eq(0).on('click',function () {
                        alert('很抱歉，该广告已下架或被删除');
                    })
                }
            }else{//app
                if(HasOtherPublish == 1){//跳转到详情页
                    that.find('.content a').eq(0).on('click',function () {
                        $(this).attr('target','_blank');
                        $(this).attr('href','/OrderManager/app_detail.html?MediaID='+MediaID+'&TemplateID='+TemplateID);
                    })
                }else if(HasOtherPublish == 0){//提示
                    that.find('.content a').eq(0).on('click',function () {
                        alert('很抱歉，该广告已下架或被删除');
                    })
                }else if(expired == -1){//已失效
                    that.find('.content a').eq(0).on('click',function () {
                        alert('很抱歉，该广告已下架或被删除');
                    })
                }
            }
        })

        //有用的tr链接
        $('.usefulTr').each(function () {
            var that = $(this);
            var MediaType = that.attr('MediaType')*1;
            var MediaID = that.attr('MediaID')*1;
            var TemplateID = that.attr('TemplateID')*1;
            if(MediaType == 14001){
                that.find('.content a').eq(0).on('click',function () {
                    $(this).attr('target','_blank');
                    $(this).attr('href','/OrderManager/wx_detail.html?MediaType='+MediaType+'&MediaID='+MediaID);
                })
            }else{
                that.find('.content a').eq(0).on('click',function () {
                    $(this).attr('target','_blank');
                    $(this).attr('href','/OrderManager/app_detail.html?MediaID='+MediaID+'&TemplateID='+TemplateID);
                })
            }
        })

    }

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
        'fileTypeDesc': '支持格式:jpg,jpeg,png,zip',
        'fileTypeExts': '*.jpg;*.jpeg;*.png;*.zip;',
        'fileSizeLimit':'10MB',
        /*'queueSizeLimit': 1,*/
        'scriptAccess': 'always',
        'onQueueComplete': function (event, data) {},
        'onQueueFull': function () {
            layer.alert('您最多只能上传1个文件！');
            return false;
        },
        'onUploadSuccess': function (file, data, response) {
            if (response == true) {
                var json = $.evalJSON(data);
                /*$(".fileBox").show();
                $("#FileName").text(json.FileName);
                $("#downloadFile").attr("href",json.Msg);*/
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



