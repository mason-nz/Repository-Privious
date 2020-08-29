//  whitten by deng_x
//  2017.5.2
$(function () {
    //左边渲染数据集合
    var dataLeft = {};
    //右边渲染数据集合
    var dataRight = {};
    //日历控件最小时间
    var datemin = '2016-01-01';
    //日历控件最大时间
    var datemax = '2019-12-31';
    //选中的时间
    var timeInput = "";
    //点击二级维度对应一级维度的组合
    var adArr = [];
    //点击一级维度对应二级维度的组合
    var crArr = [];
    //一级维度选中状态的属性值
    var adposi1 = "";
    //二级维度选中状态的属性值
    var adposi2 = "";
    //通过一级和二级维度找出的不同日期的集合
    var timeArr = [];
    //日历控件无效区间渲染的状态
    var voidArr = [];
    //保存的广告位id
    var PubDetaildID = "";
    //日期id
    var pubId = "";
    //检测添加购物车广告位
    var checkArr = [];
    //保存选中的日期
    var selectArr = [];
    var dataPublishDetail = "";
    var dataPublishInfo = "";
    var rightDom1 = "";
    var rightDom = "";
    var dataArr = [];
    //刊例的状态
    var Wx_Status = [];
    //媒体ID
    var MediaID = GetQueryString('MediaID') != null && GetQueryString('MediaID') != 'undefined' ? parseFloat(GetQueryString('MediaID')) : null;
    //媒体类型
    var MediaType = GetQueryString('MediaType') != null && GetQueryString('MediaType') != 'undefined' ? parseFloat(GetQueryString('MediaType')) : null;
    //订单类型
    var orderID = GetQueryString('orderID') != null && GetQueryString('orderID') != 'undefined' ? GetQueryString('orderID') : null;
    //获取锚点之前的地址信息
    var locationHref = window.location.href.split("#")[0];
    //锚点与无锚点区分
    if (window.location.href.indexOf("#") != -1) {
        var anchorStr = window.location.href.split('#')[1];
        var anchorArr = anchorStr.split("_");
        //锚点为如：#6001_41001_30592进行解析
        if (anchorArr.length == 3) {
            setAjax({
                    url: set.url,
                    type: 'get',
                    data: {
                        MediaType: 14001,
                        MediaID: Getrequest().MediaID
                    },
                    dataType: 'json'
                },
                function (data) {
                    console.log(data);
                    if (data.Status == 0) {
                        data.Result.disabled = '';
                        dataPublishDetail = data.Result.PublishDetail;
                        dataPublishInfo = data.Result.PublishInfo;
                        dataArr = [];
                        adposi1 = anchorArr[0];
                        adposi2 = anchorArr[1];
                        crArr = [];
                        adArr = [];
                        for (var j = 0; j < dataPublishInfo.length; j++) {
                            console.log(anchorArr[2] == dataPublishInfo[j].PubID);
                            if (anchorArr[2] == dataPublishInfo[j].PubID) {
                                dataRight['PublishInfo'] = data.Result.PublishInfo[j];
                            }
                        }
                        console.log(data.Result);
                        for (var i = 0; i < dataPublishDetail.length; i++) {
                            if (dataPublishDetail[i].ADPosition1 == anchorArr[0] && dataPublishDetail[i].ADPosition2 == anchorArr[1] && dataPublishDetail[i].PubID == anchorArr[2]) {
                                dataRight['SalePrice'] = dataPublishDetail[i].SalePrice;
                            }
                        }
                        //左边渲染数据
                        dataLeft['PublishDetail'] = dataPublishDetail[0];
                        //右边渲染数据
                        // dataRight['PublishInfo'] = data.Result.PublishInfo[0];
                        // dataRight['SalePrice'] = dataPublishDetail[0].SalePrice;
                        //
                        // dataRight['AllADPosition1'] = data.Result.AllADPosition1;
                        // dataRight['AllADPosition2'] = data.Result.AllADPosition2;

                        dataRight['PublishInfo'] = data.Result.PublishInfo[0];
                        dataRight['SalePrice'] = dataPublishDetail[0].SalePrice;
                        dataRight['AllADPosition1'] = data.Result.AllADPosition1;
                        dataRight['AllADPosition2'] = data.Result.AllADPosition2;
                        adposi1 = dataPublishDetail[0].ADPosition1;
                        adposi2 = dataPublishDetail[0].ADPosition2;
                        PubDetaildID = dataPublishDetail[0].ADDetailID;
                        // console.log(dataRight);
                        $('.details_text .right #right1').html(ejs.render($("#wx_right_list").html(), {
                            data: dataRight,
                            disabled: data.Result.disable
                        }));
                        $('.details_text .left').html(ejs.render($("#wx_left_list").html(), {data: dataLeft}));

                        showlist_main();
                        $('.jqzoom').jqzoom({
                            zoomType: 'standard',
                            lens: true,
                            preloadImages: true,
                            xOffset: 6,
                            title: false,
                            zoomWidth: 360,
                            zoomHeight: 360,
                            alwaysOn: false
                        });
                    }
                })
        } else {
            showTopList();
        }

    } else {
        showTopList();
    }
    function showTopList() {
        //顶部数据渲染
        setAjax({
                url: set.url,
                type: 'get',
                data: {
                    MediaType: 14001,
                    MediaID: Getrequest().MediaID
                },
                dataType: 'json'
            },
            function (data) {
                if (data.Status == 0) {
                    var dingyi = 0;
                    data.Result.disabled = '';
                    if (data.Result.PublishInfo.length == 0) {
                        dingyi = 1;
                        data = {
                            "Status": 0,
                            "Message": "Success",
                            "Result": {
                                "PublishInfo": [
                                    {
                                        "PubID": 26201,
                                        "MainTitle": data.Result.Subtitle,
                                        "Subtitle": data.Result.Subtitle,
                                        "BeginTime": "2017-04-07",
                                        "EndTime": "2017-06-30",
                                        "SaleStyle": "自营",
                                        "ServiceType": "",
                                        "IsVerify": "未认证",
                                        "HeadImg": data.Result.HeadImg,
                                        "QrCodeUrl": data.Result.QrCodeUrl,
                                        "DifferDay": 1,
                                        "IsAppointment": "不需预约",
                                        "RemarkName": null,
                                        "HeadImg-sl": data.Result['HeadImg-sl'],
                                        "QrCodeUrl-sl": data.Result['QrCodeUrl-sl']
                                    }
                                ],
                                "PublishDetail": [
                                    {
                                        "ADDetailID": 169178,
                                        "PubID": 26201,
                                        "ADPosition1": 0,
                                        "ADPosition2": 0,
                                        "SalePrice": 268000,
                                        "ImageUrl1": "",
                                        "ImageUrl2": "",
                                        "ImageUrl3": "",
                                        "ImgUrl1-sl": "",
                                        "ImgUrl2-sl": "",
                                        "ImgUrl3-sl": "",
                                        "HeadImg": data.Result.HeadImg,
                                        "QrCodeUrl": data.Result.QrCodeUrl,
                                        "HeadImg-sl": data.Result['HeadImg-sl'],
                                        "QrCodeUrl-sl": data.Result['QrCodeUrl-sl']
                                    }
                                ],
                                "AllADPosition1": data.Result.AllADPosition1,
                                "AllADPosition2": data.Result.AllADPosition2,
                                "disabled": "disabled"
                            },
                            "IsOverdue": null
                        };
                    }
                    dataPublishDetail = data.Result.PublishDetail;
                    dataPublishInfo = data.Result.PublishInfo;
                    voidArr = [];
                    timeArr = [];
                    //左边渲染数据
                    dataLeft['PublishDetail'] = dataPublishDetail[0];
                    //右边渲染数据
                    dataRight['PublishInfo'] = data.Result.PublishInfo[0];
                    dataRight['SalePrice'] = dataPublishDetail[0].SalePrice;
                    dataRight['AllADPosition1'] = data.Result.AllADPosition1;
                    dataRight['AllADPosition2'] = data.Result.AllADPosition2;
                    adposi1 = dataPublishDetail[0].ADPosition1;
                    adposi2 = dataPublishDetail[0].ADPosition2;
                    PubDetaildID = dataPublishDetail[0].ADDetailID;

                    $('.details_text .left').html(ejs.render($("#wx_left_list").html(), {data: dataLeft}));
                    $('.details_text .right #right1').html(ejs.render($("#wx_right_list").html(), {
                        data: dataRight,
                        disabled: data.Result.disabled
                    }));
                    if (dingyi) {
                        console.log(1);
                        $('.box_cart').remove();
                        $('#right1>p').remove();
                        $('#right1>ul').remove();
                        $('#right1').append('<div class="not_created"><img src="/ImagesNew/icon21.png"> 该媒体主还未创建刊例</div>')
                    }
                    ;
                    if (dingyi == 0 && data.Result.PublishInfo[0].Wx_Status != 42011) {
                        $('#right1>ul').eq(0).html('刊例已停用/刊例已过期').addClass('f6');
                        $('#zhezhao').show();
                        $('.button').css({backgroundColor: 'grey'})
                    }
                    showlist_main();
                    $('.jqzoom').jqzoom({
                        zoomType: 'standard',
                        lens: true,
                        preloadImages: true,
                        xOffset: 6,
                        title: false,
                        zoomWidth: 360,
                        zoomHeight: 360,
                        alwaysOn: false
                    });
                }
            }
        )
    }

    function showlist_main() {
        //选中日历日期赋值
        $("[name=Username]").val(timeInput);
        rightDom1 = $('.right ul li').eq(1).children();
        rightDom = $('.right ul li').eq(2).children();
        eachother_begrey();

        showlist_one();

        //点击选择日期
        $("[name=Username]").off('click').on("click", promotion_date);
        function promotion_date() {
// //日历控件最小时间
//             var datemin = '2016-01-01';
//             //日历控件最大时间
//             var datemax = '2019-12-31';
            laydate({
                // elem:"#startTime",
                fixed: false,
                format: 'YYYY-MM-DD hh:mm', //日期格式
                istime: true, //是否开启时间选择
                issure: true,
                // isv: true,
                init: false,
                istoday:false,
                // min: laydate.now(-500),
                // max: ,
                voidDateRange: voidArr,//渲染无效时间
                selectedPoint: selectArr,//渲染已选择时间
                isNeedConfirm: true,//是否点击确认
                //选择时间之后的回调函数
                choose: function (date) {
                  console.log(voidArr);

                //   $("#Promotiondate").val("");
                    console.log(date.substring(0,10));
                    //一级维度选中获取的值
                    adposi1 = $('.right ul li:nth-child(2) .active').attr("data-dicid");
                    //二级维度选中获取的值
                    adposi2 = $('.right ul li:nth-child(3) .active').attr("data-dicid");
                    timeInput = date;
                    //点击时间重新渲染
                    for (var i = 0; i < timeArr.length; i++) {
                        if (date+':00' >= timeArr[i]['BeginTime'] && date+':00' <= timeArr[i]["EndTime"]) {
                        for (var j = 0; j < dataPublishInfo.length; j++) {
                            if (timeArr[i]['pubid'] == dataPublishInfo[j].PubID) {
                                dataRight['PublishInfo'] = dataPublishInfo[j];
                                pubId = dataPublishInfo[j].PubID;
                            }
                        }
                        for (var k = 0; k < dataPublishDetail.length; k++) {
                            if (dataPublishDetail[k].PubID == timeArr[i]['pubid'] && adposi1 == dataPublishDetail[k].ADPosition1 && adposi2 == dataPublishDetail[k].ADPosition2) {
                                dataLeft['PublishDetail'] = dataPublishDetail[k];
                                dataRight['SalePrice'] = dataPublishDetail[k].SalePrice;
                                PubDetaildID = dataPublishDetail[k].ADDetailID;
                            }
                        }
                        }
                    }
                    ;
                    console.log(PubDetaildID);
                    //渲染右边数据
                    $('.details_text .right #right1').html(ejs.render($("#wx_right_list").html(), {
                        data: dataRight,
                        disabled: ''
                    }));
                    $('.details_text .left').html(ejs.render($("#wx_left_list").html(), {data: dataLeft}));
                    rightDom1 = $('.right ul li').eq(1).children();
                    rightDom = $('.right ul li').eq(2).children();
                    //添加锚点
                    window.location.href = locationHref + "#" + adposi1 + "_" + adposi2 + "_" + pubId;
                    //一级与二级维度置灰判断
                    for (var i = 0; i < dataPublishDetail.length; i++) {
                        if (dataPublishDetail[i].ADPosition1 == adposi1) {
                            crArr.push(dataPublishDetail[i]);
                        }
                        if (dataPublishDetail[i].ADPosition2 == adposi2) {
                            adArr.push(dataPublishDetail[i]);
                        }
                    }
                    showlist_one();
                    $("[name=Username]").val(timeInput);
                    $('.right ul li').eq(2).children().not(".set_grey").on('click', fun2);
                    $('.right ul li').eq(1).children().not(".set_grey").on('click', fun1);
                    $("[name=Username]").off('click').on("click", promotion_date);
                    $(".box_cart .button").off("click").on('click', addShopCart);
                    $('.jqzoom').jqzoom({
                        zoomType: 'standard',
                        lens: true,
                        preloadImages: true,
                        xOffset: 6,
                        title: false,
                        zoomWidth: 360,
                        zoomHeight: 360,
                        alwaysOn: false
                    });
                    for(var i=0;i<voidArr.length;i++){
                      if(date.substring(0,10)>=voidArr[i]["S"]&&date.substring(0,10)<=voidArr[i]["E"]){
                          $("#Promotiondate").val("");
                      }
                    }
                }
            });
        }

        //点击一级维度
        $('.right ul li').eq(1).children().not(".set_grey").on('click', fun1);
        function fun1() {
            dataArr = [];
            pubId = "";
            adArr = [];
            crArr = [];
            timeArr = [];
            voidArr = [];
            adposi1 = $(this).attr("data-dicid");
            adposi2 = $('.right ul li:nth-child(3) .active').attr("data-dicid");
            eachother_begrey();

            dataRight['SalePrice'] = dataArr[0].SalePrice;
            pubId = dataArr[0].PubID;
            for (var i = 0; i < dataPublishInfo.length; i++) {
                if (dataPublishInfo[i].PubID == pubId) {
                    dataRight['PublishInfo'] = dataPublishInfo[i];
                }
            }
            //添加锚点
            window.location.href = locationHref + "#" + adposi1 + "_" + adposi2 + "_" + pubId;
            //左边渲染
            $('.details_text .left').html(ejs.render($("#wx_left_list").html(), {data: dataLeft}));
            //右边渲染
            $('.details_text .right #right1').html(ejs.render($("#wx_right_list").html(), {
                data: dataRight,
                disabled: ''
            }));
            var flag = true;
            rightDom1 = $('.right ul li').eq(1).children();
            rightDom = $('.right ul li').eq(2).children();
            showlist_one();
            $('.right ul li').eq(2).children().not(".set_grey").on('click', fun2);
            $('.right ul li').eq(1).children().not(".set_grey").on('click', fun1);
            $("[name=Username]").off('click').on("click", promotion_date);
            $(".box_cart .button").off("click").on('click', addShopCart);
            $('.jqzoom').jqzoom({
                zoomType: 'standard',
                lens: true,
                preloadImages: true,
                xOffset: 6,
                title: false,
                zoomWidth: 360,
                zoomHeight: 360,
                alwaysOn: false
            });
        }

        //点击二级维度
        $(".right ul li").eq(2).children().on('click', fun2);
        function fun2() {
            dataArr = [];
            pubId = "";
            adArr = [];
            crArr = [];
            timeArr = [];
            voidArr = [];
            adposi1 = $('.right ul li:nth-child(2) .active').attr("data-dicid");
            adposi2 = $(this).attr("data-dicid");
            eachother_begrey();
            dataRight['SalePrice'] = dataArr[0].SalePrice;
            pubId = dataArr[0].PubID;
            for (var i = 0; i < dataPublishInfo.length; i++) {
                if (dataPublishInfo[i].PubID == pubId) {
                    dataRight['PublishInfo'] = dataPublishInfo[i];
                }
            }
            //添加锚点
            window.location.href = locationHref + "#" + adposi1 + "_" + adposi2 + "_" + pubId;
            //左边渲染
            $('.details_text .left').html(ejs.render($("#wx_left_list").html(), {data: dataLeft}));
            //右边渲染
            $('.details_text .right #right1').html(ejs.render($("#wx_right_list").html(), {
                data: dataRight,
                disabled: ''
            }));
            var flag;
            rightDom1 = $('.right ul li').eq(1).children();
            rightDom = $('.right ul li').eq(2).children();
            showlist_one();
            $('.right ul li').eq(1).children().not(".set_grey").on('click', fun1);
            $('.right ul li').eq(2).children().not(".set_grey").on('click', fun2);
            $("[name=Username]").off('click').on("click", promotion_date);
            $(".box_cart .button").off("click").on('click', addShopCart);
            $('.jqzoom').jqzoom({
                zoomType: 'standard',
                lens: true,
                preloadImages: true,
                xOffset: 6,
                title: false,
                zoomWidth: 360,
                zoomHeight: 360,
                alwaysOn: false
            });
        }

        //添加购物车
        $(".box_cart .button").off("click").on('click', addShopCart);
        function addShopCart() {
            var date=$('#Promotiondate').val();
            console.log(date);
            //一级维度选中获取的值
            adposi1 = $('.right ul li:nth-child(2) .active').attr("data-dicid");
            //二级维度选中获取的值
            adposi2 = $('.right ul li:nth-child(3) .active').attr("data-dicid");
            timeInput = date;
            //点击时间重新渲染
            for (var i = 0; i < timeArr.length; i++) {
                if (date+':00' >= timeArr[i]['BeginTime'] && date+':00' <= timeArr[i]["EndTime"]) {
                    for (var j = 0; j < dataPublishInfo.length; j++) {
                        if (timeArr[i]['pubid'] == dataPublishInfo[j].PubID) {
                            dataRight['PublishInfo'] = dataPublishInfo[j];
                            pubId = dataPublishInfo[j].PubID;
                        }
                    }
                    for (var k = 0; k < dataPublishDetail.length; k++) {
                        if (dataPublishDetail[k].PubID == timeArr[i]['pubid'] && adposi1 == dataPublishDetail[k].ADPosition1 && adposi2 == dataPublishDetail[k].ADPosition2) {
                            dataLeft['PublishDetail'] = dataPublishDetail[k];
                            dataRight['SalePrice'] = dataPublishDetail[k].SalePrice;
                            PubDetaildID = dataPublishDetail[k].ADDetailID;
                        }
                    }
                }
            }
            ;
            console.log(PubDetaildID);
            console.log(timeInput);
            console.log(checkArr);//保存的广告位id
            if ($("[name=Username]").val() == "") {
                alert('推广日期不能为空');
            } else {
                setAjax({
                    url: set.addShopCart,
                    type: 'POST',
                    data: {
                        "MediaType": 14001,
                        "IDs": [
                            {
                                "MediaID": Getrequest().MediaID,
                                "PublishDetailID": PubDetaildID,
                                "ADSchedule": $('#Promotiondate').val()
                            }
                        ]
                    },
                    dataType: 'json'
                }, function (data) {
                    if (data.Status == 0) {

                        checkArr.push(PubDetaildID);
                        var checkArrNew = [];
                        //对数组中重复广告位PubDetaildID去重
                        for (var i = 0; i < checkArr.length; i++) {
                            if (checkArrNew.indexOf(checkArr[i]) == -1) {
                                checkArrNew.push(checkArr[i]);
                            }
                        }
                        checkArr = checkArrNew;
                        //储存有效时间段
                        if (timeInput != "") {
                            selectArr.push(getDay(timeInput));
                        }
                        //请求购物车中的数量
                        setAjax({
                            url: "/api/ShoppingCart/GetInfo_ShoppingCart?v=1_1",
                            type: "get",
                            data: {
                                MediaType: 14001
                            }
                        }, function (data) {
                            console.log(data);
                            if (data.Result) {
                              var shopcount1 = 0;
                              var shopcount2 = 0;
                              for(var i=0;i<data.Result.SelfMedia.length;i++){
                                for(var j=0;j<data.Result.SelfMedia[i].Medias.length;j++){
                                  shopcount1++;
                                }
                              }
                              for(var i=0;i<data.Result.APP.length;i++){
                                for(var j=0;j<data.Result.APP[i].Medias.length;j++){
                                  shopcount2++;
                                }
                              }
                              $('.cart_num').html(shopcount1+shopcount2);
                            }
                        })
                        // $('.cart_num').html(checkArr.length);
                        console.log(data);
                        _hmt.push(['_trackEvent', 'button', 'click', 'addcart']);
                        layer.msg('成功加入购物车！')
                    } else {
                        layer.msg(data.Message);
                        // 显示您的购物车中已有三个该广告位，请前去下单
                        if (data.Message == '您的购物车中已有三个该广告位，请前去下单') {
                            $('#advnum').show();
                        }
                    }

                })
            }
            ;

        };
        //请求购物车中的数量
        setAjax({
            url: "/api/ShoppingCart/GetInfo_ShoppingCart?v=1_1",
            type: "get",
            data: {
                MediaType: 14001
            }
        }, function (data) {

            if (data.Result) {
              var shopcount1 = 0;
              var shopcount2 = 0;
              for(var i=0;i<data.Result.SelfMedia.length;i++){
                for(var j=0;j<data.Result.SelfMedia[i].Medias.length;j++){
                  shopcount1++;
                }
              }
              for(var i=0;i<data.Result.APP.length;i++){
                for(var j=0;j<data.Result.APP[i].Medias.length;j++){
                  shopcount2++;
                }
              }
              $('.cart_num').html(shopcount1+shopcount2);
            }
        })
    }

    //一二级维度对应置灰
    function showlist_one() {
        //默认选中广告位置与置灰
        for (var j = 0; j < rightDom1.length; j++) {
            flag = true;
            for (var i = 0; i < adArr.length; i++) {
                if (adArr[i].ADPosition1 == rightDom1[j].getAttribute("data-dicid")) {
                    flag = false;
                }
                if (flag) {
                    rightDom1[j].className = "set_grey";
                } else {
                    rightDom1[j].className = "";
                }
            }
            if (rightDom1[j].getAttribute("data-dicid") == adposi1) {
                rightDom1[j].className = "active";
            }
        }
        //创作类型置灰
        for (var j = 0; j < rightDom.length; j++) {
            flag = true;
            for (var i = 0; i < crArr.length; i++) {
                if (crArr[i].ADPosition2 == rightDom[j].getAttribute("data-dicid")) {
                    flag = false;
                }
                if (flag) {
                    rightDom[j].className = "set_grey";
                } else {
                    rightDom[j].className = "";
                }
            }
            if (rightDom[j].getAttribute("data-dicid") == adposi2) {
                rightDom[j].className = "active";
            }
        }
    }

    function eachother_begrey() {
        //互相判断置灰
        for (var i = 0; i < dataPublishDetail.length; i++) {
            if (dataPublishDetail[i].ADPosition1 == adposi1 && dataPublishDetail[i].Wx_Status == 42011) {
                crArr.push(dataPublishDetail[i]);
            }
            if (dataPublishDetail[i].ADPosition2 == adposi2 && dataPublishDetail[i].Wx_Status == 42011) {
                adArr.push(dataPublishDetail[i]);
            }
            if (dataPublishDetail[i].ADPosition2 == adposi2 && dataPublishDetail[i].ADPosition1 == adposi1 && dataPublishDetail[i].Wx_Status == 42011) {
                dataArr.push(dataPublishDetail[i]);
                dataLeft['PublishDetail'] = dataPublishDetail[i];
                //渲染时间
                for (var l = 0; l < dataPublishInfo.length; l++) {
                    if (dataPublishDetail[i].PubID == dataPublishInfo[l].PubID && dataPublishDetail[i].Wx_Status == 42011) {
                        timeArr.push({
                            'pubid': dataPublishDetail[i].PubID,
                            "BeginTime": dataPublishInfo[l].BeginTime,
                            "EndTime": dataPublishInfo[l].EndTime
                        });
                    }
                }
            }
        }
        if (timeArr.length > 1) {
            for (var t = 0; t < timeArr.length; t++) {
                if (t + 1 < timeArr.length && compare(timeArr[t]['EndTime'], timeArr[t + 1]['BeginTime']) > 1) {
                    voidArr.push({"S": add_date(timeArr[t]['EndTime']), "E": reduce_date(timeArr[t + 1]['BeginTime'])});
                }
            }
            //将时间进行对比，将无效时间放入数组,开始时间减1，结束时间加1
            voidArr.push({"S": add_date(timeArr[timeArr.length - 1]['EndTime']), "E": datemax});
            voidArr.unshift({"S": datemin, "E": reduce_date(timeArr[0]['BeginTime'])});
        } else {
            voidArr = [{"S": datemin, "E": reduce_date(timeArr[0]['BeginTime'])}, {
                "S": add_date(timeArr[0]['EndTime']),
                "E": datemax
            }];
        }
    }


    //比较时间大小
    function compare(timeEnd, timeNext) {
        var date1 = new Date(timeEnd)
        var date2 = new Date(timeNext)

        var s1 = date1.getTime(), s2 = date2.getTime();
        var total = (s2 - s1) / 1000;
        var day = parseInt(total / (24 * 60 * 60));//计算整数天数
        return day;
    }

    //获取url 地址参数方法
    function GetQueryString(name) {
        var reg = new RegExp("(^|&)" + name + "=([^&]*)(&|$)");
        var r = window.location.search.substr(1).match(reg);
        if (r != null)return unescape(r[2]);
        return null;
    }

    //将时间戳转换为日期格式2017-05-06
    Date.prototype.format = function (fmt) {
        var o = {
            "M+": this.getMonth() + 1,                 //月份
            "d+": this.getDate(),                    //日
            "h+": this.getHours(),                   //小时
            "m+": this.getMinutes(),                 //分
            "s+": this.getSeconds(),                 //秒
            "q+": Math.floor((this.getMonth() + 3) / 3), //季度
            "S": this.getMilliseconds()             //毫秒
        };
        if (/(y+)/.test(fmt)) {
            fmt = fmt.replace(RegExp.$1, (this.getFullYear() + "").substr(4 - RegExp.$1.length));
        }
        for (var k in o) {
            if (new RegExp("(" + k + ")").test(fmt)) {
                fmt = fmt.replace(RegExp.$1, (RegExp.$1.length == 1) ? (o[k]) : (("00" + o[k]).substr(("" + o[k]).length)));
            }
        }
        return fmt;
    }
    //时间减一天
    function reduce_date(time) {
        time=time.split(' ')[0]
        var date1 = new Date(time);
        var s1 = date1.getTime();
        var day = parseInt(s1 - (24 * 60 * 60 * 1000));//计算整数天数
        var str1 = new Date(day).format("yyyy-MM-dd");
        return str1;
    }

    //时间加一天
    function add_date(time) {
        time=time.split(' ')[0]
        var date1 = new Date(time);
        var s1 = date1.getTime();
        var day = parseInt(s1 + (24 * 60 * 60 * 1000));//计算整数天数
        var str1 = new Date(day).format("yyyy-MM-dd");
        return str1;
    }

    //截取到天
    function getDay(str) {
        return str.split(" ")[0];
    }

    /*获取解码后的用户ID*/
    function Getrequest() {
        var url = location.search; //获取url中"?"符后的字串
        var theRequest = new Object();
        if (url.indexOf("?") != -1) {
            var str = url.substr(1);
            strs = str.split("&");
            for (var i = 0; i < strs.length; i++) {
                theRequest[strs[i].split("=")[0]] = strs[i].split("=")[1];
            }
        }
        return theRequest;
    };
    gotoShopCart()
    // 点击购物车跳转
    function gotoShopCart() {

        // 参数
        function parameter() {
            if (Getrequest().userID != undefined) {
                if (Getrequest().isAdd != undefined) {
                    return '?isAdd=' + Getrequest().isAdd + '&orderID=' + Getrequest().orderID + '&MediaType=' + Getrequest().MediaType+'&userID='+Getrequest().userID
                };
                if (Getrequest().orderID != undefined) {
                    return '?orderID=' + Getrequest().orderID + '&MediaType=' + Getrequest().MediaType+'&userID='+Getrequest().userID
                };
                if (Getrequest().orderID == undefined && Getrequest().isAdd == undefined) {
                    return '' + '?MediaType=' + Getrequest().MediaType+'&userID='+Getrequest().userID
                };
            }else {
                if (Getrequest().isAdd != undefined) {
                    return '?isAdd=' + Getrequest().isAdd + '&orderID=' + Getrequest().orderID + '&MediaType=' + Getrequest().MediaType
                };
                if (Getrequest().orderID != undefined) {
                    return '?orderID=' + Getrequest().orderID + '&MediaType=' + Getrequest().MediaType
                };
                if (Getrequest().orderID == undefined && Getrequest().isAdd == undefined) {
                    return '' + '?MediaType=' + Getrequest().MediaType
                };
            }
        }

        $('#shopCart').off('click').on('click', function () {
            if ($('.cart_num').html() > 0) {
                window.location = '/OrderManager/shopcartForMedia01.html';
                _hmt.push(['_trackEvent', 'button', 'click', 'entercart']);
            } else {
                layer.msg('请添加广告位！')
                return false
            }
        })

    }

    //详情页下面部分
    //取出链接中需要的参数
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

    var userData = GetUserId();
    if (userData.hasOwnProperty("MediaID") && userData.hasOwnProperty("MediaType")) {
        var MediaID = parseInt(userData.MediaID);
        var MediaType = parseInt(userData.MediaType);
        console.log(typeof MediaID);
        console.log(typeof MediaType);
    }
    //渲染相似推荐信息
    $.when($.ajax({
        url: "/api/media/GetRecommendClass?v=1_1",
        type: "get",
        data: {
            mediaId: MediaID
        },
        dataType: 'json',
        xhrFields: {
            withCredentials: true
        },
        crossDomain: true,
        success:function (data) {
        console.log(data);
        if(data.Result&&data.Result.length!=0){
            $(".recommend").html(ejs.render($("#similar").html(), {data: data.Result}));
            $(".recommend div").on("click", function () {
                MediaID = $(this).find("input").val();
                if (userData.hasOwnProperty("isAdd")) {
                    window.location = "/OrderManager/wx_detail.html?isAdd=1&orderID=" + userData[orderID] + "&MediaID=" + MediaID+"&MediaType="+MediaType;
                } else {
                    window.location = "/OrderManager/wx_detail.html" + "?MediaID=" + MediaID+"&MediaType="+MediaType;
                }
            })
        }
    }
    })
    //基本信息部分的数据渲染
    ,$.ajax({
        url: "/api/media/GetItem?v=1_1",
        type: "get",
        data: {
            businesstype: MediaType,
            mediaId: MediaID
        },
        dataType: 'json',
        xhrFields: {
            withCredentials: true
            },
        crossDomain: true,
        success:function (data) {
        console.log(data);
        if(data.Result&&!$.isEmptyObject(data.Result)){
            //渲染基本信息部分
            $(".details_info").html(ejs.render($("#basicinfo").html(), {data: data.Result}));
            //渲染图表部分的创建日期
            $("#creat").html(ejs.render($("#creattime").html(), {data: data.Result}));
            if($("#data").html().trim().length==0){
                $("#creat").hide();
            }
            //渲染上部分粉丝数
            $("#fr").html(ejs.render($("#fanscount").html(), {data: data.Result}));
            // 给粉丝数旁边的购物车绑定点击事件
            $('.details_cart').off('click').on('click', function () {
                console.log(1);
                $('#shopCart').click();
            })
            //渲染收藏拉黑状态当有刊例的状态下
              $("#operation").html(ejs.render($("#cp").html(), {data: data.Result}));
              if($(".collection").length != 0){
              if ($(".collection").html().trim() == "收藏") {
                  $(".collection").css("background", "url(/images/icon72_h.png) no-repeat 0 4px");
              } else {
                  $(".collection").css("background", "url(/images/icon72.png) no-repeat 0 4px");
              }
              if ($(".pull").html().trim() == "拉黑") {
                  $(".pull").css("background", "url(/images/icon73_h.png) no-repeat 0 4px");
              } else {
                  $(".pull").css("background", "url(/images/icon73.png) no-repeat 0 4px");
                  $("#zhezhao").show();
                  $(".box_cart span").eq(0).css("background-color", "grey");
              }
              // 操作收藏拉黑按钮
                 if($(".pull").html().trim() == "拉黑"){
                  $(".collection").off("click").on("click", function () {
                      if ($(".collection").html().trim() == "收藏") {
                          $(".collection").html("已收藏");
                          $(".collection").css("background", "url(/images/icon72.png) no-repeat 0 4px");
                          setAjax({
                              url: "/api/CollectPullBack/add",
                              type: "post",
                              data: {
                                  businesstype: MediaType,
                                  operatetype: 1,
                                  mediaId: MediaID
                              }
                          }, function (data) {
                              console.log(data);
                              $(".pull").off("click");
                          })
                      } else {
                          $(".collection").html("收藏");
                          $(".collection").css("background", "url(/images/icon72_h.png) no-repeat 0 4px");
                          setAjax({
                              url:"/api/CollectPullBack/Remove",
                              type:"post",
                              data:{
                                  businesstype:MediaType,
                                  mediaId:MediaID,
                                  operatetype:1
                              }
                          }, function (data) {
                              console.log(data);
                              $(".pull").off("click").on("click", function () {
                                  if ($(".pull").html().trim() == "拉黑") {
                                      layer.confirm("确定拉黑吗？", {
                                          time: 0,
                                          btn: ["确认", "取消"],
                                          yes: function () {
                                              $(".pull").css("background", "url(/images/icon73.png) no-repeat 0 4px");
                                              setAjax({
                                                  url: "/api/CollectPullBack/add",
                                                  type: "post",
                                                  data: {
                                                      businesstype: MediaType,
                                                      operatetype: 2,
                                                      mediaId: MediaID
                                                  }
                                              }, function (data) {
                                                  console.log(data);
                                                  $(".pull").html("已拉黑");
                                                  layer.msg("已拉黑", {time: 1000});
                                                  $("#zhezhao").show();
                                                  $(".box_cart span").eq(0).css("background-color", "grey");
                                                  $(".collection").off("click");
                                              })
                                          }
                                      });

                                  }
                              })
                          })
                      }
                  })
              }
              if($(".collection").html().trim() == "收藏"){
                  $(".pull").off("click").on("click", function () {
                      if ($(".pull").html().trim() == "拉黑") {
                          layer.confirm("确定拉黑吗？", {
                              time: 0,
                              btn: ["确认", "取消"],
                              yes: function () {
                                  $(".pull").css("background", "url(/images/icon73.png) no-repeat 0 4px");
                                  setAjax({
                                      url: "/api/CollectPullBack/add",
                                      type: "post",
                                      data: {
                                          businesstype: MediaType,
                                          operatetype: 2,
                                          mediaId: MediaID
                                      }
                                  }, function (data) {
                                      console.log(data);
                                      $(".pull").html("已拉黑");
                                      layer.msg("已拉黑", {time: 1000});
                                      $("#zhezhao").show();
                                      $(".box_cart span").eq(0).css("background-color", "grey");
                                      $(".collection").off("click");
                                  })
                              }
                          });

                      }
                  })
              }
            }
            //不同接口饼图数据渲染
            //粉丝性别分布扇形图
            var p_sex = $('#sex').get(0);
            var myChart_sex = echarts.init(p_sex);
            var data_sex = [];
            var other = 100 - data.Result.FansMalePer - data.Result.FansFemalePer;
            if(data.Result.FansMalePer){
                data_sex.push({value: data.Result.FansMalePer, name: '男'})
            }
            if(data.Result.FansFemalePer){
                data_sex.push({value: data.Result.FansFemalePer, name: '女'})
            }
            if(100 - data.Result.FansMalePer - data.Result.FansFemalePer){
                data_sex.push({value: other, name: '其他'})
            }
            var option_sex = {
                title: {
                    text: '粉丝性别分布',
                    // subtext: '纯属虚构',
                    x: 'center',
                    y: "bottom",
                    textStyle: {
                        fontWeight: 'normal',
                        fontSize: 13
                    }
                },
                tooltip: {
                    trigger: 'item',
                    formatter: "{a} <br/>{b} : {d}%"//显示数据的类型
                },
                legend: {
                    orient: 'vertical',//图例列表的布局朝向
                    // left: 'left',
                    right: 0,
                    top: '33%',
                    // top:"50%",  //百分比跟middle不一样，后期看一下
                    data: [
                        {name: '男'},
                        {name: '女'},
                        {name: '其他'}
                    ]
                },
                series: [
                    {
                        name: '粉丝性别比例',
                        type: 'pie',
                        hoverAnimation: true,//是否启用鼠标放上放大效果
                        radius: ["34.5%", '65.5%'],//可以设置内外半径，第一个是内半径，第二个是外半径
                        center: ['50%', '40%'],
                        label: {
                            normal: {
                                show: false,
                                position: "inside"
                            },//正常状态下饼图图形上的文本标签
                            emphasis: {
                                show: true//有交互的时候显示图形上的文本标签
                            }
                        },
                        data: data_sex,
                        // itemStyle: {
                        //     emphasis: {
                        //       color:["yellow"],
                        //         shadowBlur: 10,
                        //         shadowOffsetX: 0,
                        //         shadowColor: 'rgba(0, 0, 0, 0.5)'
                        //     }
                        // }//emphasis 是图形在高亮状态下的样式
                    }
                ],
                color: ["#39bbdb", "#099b88", "#11b89c"]//通过修改全局的默认样式来得到我们自己想要的颜色
            };
            // 显示图表。
            myChart_sex.setOption(option_sex);
            if(data_sex.length==0||other==100){
                $("#sex").hide();
            }
            // 取出粉丝数分布区域前五的城市
            var data_FansCount = [],
                data_ProvinceName = [],
                UserScale = 0;
            // if (data.Result.FansArea.length > 5) {
            //     for (var i = 0; i < 5; i++) {
            //         if(data.Result.FansArea[i].UserScale&&data.Result.FansArea[i].ProvinceName){
            //             data_FansCount.push({
            //                 value: data.Result.FansArea[i].UserScale,
            //                 name: data.Result.FansArea[i].ProvinceName
            //             });
            //         }
            //         data_ProvinceName.push({name: data.Result.FansArea[i].ProvinceName});
            //         UserScale += data.Result.FansArea[i].UserScale;
            //     }
            //     data_FansCount.push({value: (100 - UserScale), name: '其他'});
            //     data_ProvinceName.push({name: '其他'});
            // } else {
            //     for (var i = 0; i < data.Result.FansArea.length; i++) {
            //         if(data.Result.FansArea[i].UserScale&&data.Result.FansArea[i].ProvinceName){
            //             data_FansCount.push({
            //                 value: data.Result.FansArea[i].UserScale,
            //                 name: data.Result.FansArea[i].ProvinceName
            //             });
            //         }
            //         data_ProvinceName.push({name: data.Result.FansArea[i].ProvinceName});
            //     }
            // }

                if(data.Result.FansArea!=null && data.Result.FansArea.length!=0){

                  for (var i = 0; i < data.Result.FansArea.length; i++) {
                    if(data.Result.FansArea[i].UserScale&&data.Result.FansArea[i].ProvinceName){
                      data_FansCount.push({
                        value: data.Result.FansArea[i].UserScale,
                        name: data.Result.FansArea[i].ProvinceName
                      });
                    }
                    data_ProvinceName.push({name: data.Result.FansArea[i].ProvinceName});
                    UserScale += data.Result.FansArea[i].UserScale;
                  }
                  data_FansCount.push({value: (100 - UserScale), name: '其他'});
                  data_ProvinceName.push({name: '其他'});
                }
            //粉丝分布区域图表
            var p_FansCount = $('#FansCount').get(0);
            var myChart_FansCount = echarts.init(p_FansCount);
            var option_FansCount = {
                title: {
                    text: '粉丝分布区域',
                    // subtext: '纯属虚构',
                    padding:[
                      0,
                      0,
                      0,
                      140
                    ],
                    y: "bottom",
                    textStyle: {
                        fontWeight: 'normal',
                        fontSize: 13
                    }
                },
                tooltip: {
                    trigger: 'item',//数据项图形触发
                    formatter: "{a} <br/>{b} : {d}%"//显示数据的类型
                },
                legend: {
                    orient: 'vertical',//图例列表的布局朝向
                    // left: 'left',
                    right: 0,
                    top: 'middle',
                    // top:"50%",  //百分比跟middle不一样，后期看一下
                    data: data_ProvinceName
                },
                series: [
                    {
                        name: '粉丝分布区域',
                        type: 'pie',
                        hoverAnimation: true,//是否启用鼠标放上放大效果
                        radius: ["34.5%", '65.5%'],//可以设置内外半径，第一个是内半径，第二个是外半径
                        center: ['40%', '40%'],
                        label: {
                            normal: {
                                show: false,
                                position: "inside"
                            },//正常状态下饼图图形上的文本标签
                            emphasis: {
                                show: true//有交互的时候显示图形上的文本标签
                            }
                        },
                        data: data_FansCount,
                        // itemStyle: {
                        //     emphasis: {
                        //       color:["yellow"],
                        //         shadowBlur: 10,
                        //         shadowOffsetX: 0,
                        //         shadowColor: 'rgba(0, 0, 0, 0.5)'
                        //     }
                        // }//emphasis 是图形在高亮状态下的样式
                    }
                ],
                color: ["#3bbcdc", "#099b88", "#33cb98", "#e55948", "#faa018", "#f9e44b"]//通过修改全局的默认样式来得到我们自己想要的颜色
            };
            // 显示图表。
            myChart_FansCount.setOption(option_FansCount);
            if(data_FansCount.length==0||data_ProvinceName.length==0){
                $("#FansCount").hide();
            }


            var wxid = data.Result.Number;
            var url = "http://spiderapi.chitunion.com/API/WeChat/GetArticleWordCloudsByWxId?&wxid=" + wxid;
            //词云图热词部分
            if(wxid){
                var cloud_hot = [],
                    cloud_brand = [];
                $.ajax({
                    type: "GET",
                    cache: false,
                    url: url,
                    dataType: "jsonp",
                    jsonp: "callback",
                    jsonpCallback: "getwords",
                    success: function (data) {
                        console.log(data);
                        if(data.HotWord || data.BrandWord){
                            for (var i = 0; i < data.HotWord.length; i++) {
                                cloud_hot.push({text: data.HotWord[i].Word, weight: data.HotWord[i].Weight});
                            }
                            for (var i = 0; i < data.BrandWord.length; i++) {
                                cloud_brand.push({text: data.BrandWord[i].Word, weight: data.BrandWord[i].Weight});
                            }
                            // //根据不同权重赋值不同的颜色
                            // for (var j = 0; j < cloud_hot.length; j++) {
                            //     if (cloud_hot[j].weight == 1 || cloud_hot[j].weight == 2) {
                            //         cloud_hot[j].html = {"style": "color:blue"}
                            //     } else if (cloud_hot[j].weight == 3 || cloud_hot[j].weight == 4) {
                            //         cloud_hot[j].html = {"style": "color:green"}
                            //     } else if (cloud_hot[j].weight == 5 || cloud_hot[j].weight == 6) {
                            //         cloud_hot[j].html = {"style": "color:red"}
                            //     } else if (cloud_hot[j].weight == 7 || cloud_hot[j].weight == 8) {
                            //         cloud_hot[j].html = {"style": "color:purple"}
                            //     } else if (cloud_hot[j].weight == 9 || cloud_hot[j].weight == 10) {
                            //         cloud_hot[j].html = {"style": "color:yellow"}
                            //     }
                            // }
                            // //品牌词
                            // for (var j = 0; j < cloud_brand.length; j++) {
                            //     if (cloud_brand[j].weight == 1 || cloud_brand[j].weight == 2) {
                            //         cloud_brand[j].html = {"style": "color:blue"}
                            //     } else if (cloud_brand[j].weight == 3 || cloud_brand[j].weight == 4) {
                            //         cloud_brand[j].html = {"style": "color:green"}
                            //     } else if (cloud_brand[j].weight == 5 || cloud_brand[j].weight == 6) {
                            //         cloud_brand[j].html = {"style": "color:red"}
                            //     } else if (cloud_brand[j].weight == 7 || cloud_brand[j].weight == 8) {
                            //         cloud_brand[j].html = {"style": "color:purple"}
                            //     } else if (cloud_brand[j].weight == 9 || cloud_brand[j].weight == 10) {
                            //         cloud_brand[j].html = {"style": "color:yellow"}
                            //     }
                            // }
                            $("#hotword").jQCloud(cloud_hot, {
                                removeOverflowing: true,//如果一个word超出了cloud元素的大小，则自动剔除
                                width: 340,
                                height: 299,
                                shape: "elliptic"
                            });
                            $("#brandword").jQCloud(cloud_brand, {
                                removeOverflowing: true,//如果一个word超出了cloud元素的大小，则自动剔除
                                width: 340,
                                height: 299,
                                shape: "elliptic"
                            });
                            if(cloud_hot.length==0){
                                $("#hotword").hide();
                            }
                            if(cloud_brand.length==0){
                                $("#brandword").hide();
                            }
                            if(cloud_hot.length==0&&cloud_brand.length==0){
                                $("#article_h2").hide();
                            }
                        }else{
                            $("#article_h2").hide();
                        }
                    }
                });
            }else{
                $("#article_h2").hide();
            }


        }else{
            $("#basic_image").hide();
            $(".line3").hide();
        }
    }
    })

    //发文饼图及柱状图
    ,$.ajax({
        url: "/api/Media/GetMediaDetailStatistic?v=1_1",
        type: "get",
        data: {
            MediaType: MediaType,
            MediaID: MediaID
        },
        dataType: 'json',
        xhrFields: {
            withCredentials: true
            },
        crossDomain: true,
        success:function (data) {
        console.log(data);
        if(data.Result&&data.Result.length!=0){
            if (data.Result.ReadForWeb || data.Result.DayUpdateForWeb || data.Result.HourUpdateForWeb) {
                if(data.Result.ReadForWeb){

                    var data_publish = [];
                    if(data.Result.ReadForWeb.Original){
                        data_publish.push({value: data.Result.ReadForWeb.Original, name: '原创'});
                    }
                    if(data.Result.ReadForWeb.NonOriginal){
                        data_publish.push({value: data.Result.ReadForWeb.NonOriginal, name: '发布'});
                    }
                    var p_post = $('#publish_article').get(0);
                    var myChart_post = echarts.init(p_post);
                    var option_post = {
                        title: {
                            text: '发文总量',
                            // subtext: '纯属虚构',
                            x: 'center',
                            y: "bottom",
                            textStyle: {
                                fontWeight: 'normal',
                                fontSize: 13
                            }
                        },
                        tooltip: {
                            trigger: 'item',
                            formatter: "{a} <br/>{b} : {c} ({d}%)"//显示数据的类型
                        },
                        legend: {
                            orient: 'vertical',//图例列表的布局朝向
                            // left: 'left',
                            right: 0,
                            top: 'middle',
                            // top:"50%",  //百分比跟middle不一样，后期看一下
                            data: [
                                {name: '原创'},
                                {name: '发布'}
                            ]
                        },
                        series: [
                            {
                                name: '发文总量',
                                type: 'pie',
                                hoverAnimation: true,//是否启用鼠标放上放大效果
                                radius: ["34.5%", '65.5%'],//可以设置内外半径，第一个是内半径，第二个是外半径
                                center: ['50%', '50%'],
                                label: {
                                    normal: {
                                        show: false,
                                        position: "inside"
                                    },//正常状态下饼图图形上的文本标签
                                    emphasis: {
                                        show: true//有交互的时候显示图形上的文本标签
                                    }
                                },
                                data: [
                                    {value: data.Result.ReadForWeb.Original, name: '原创'},
                                    {value: data.Result.ReadForWeb.NonOriginal, name: '发布'}
                                ],
                                // itemStyle: {
                                //     emphasis: {
                                //       color:["yellow"],
                                //         shadowBlur: 10,
                                //         shadowOffsetX: 0,
                                //         shadowColor: 'rgba(0, 0, 0, 0.5)'
                                //     }
                                // }//emphasis 是图形在高亮状态下的样式
                            }
                        ],
                        color: ["#39bbdb", "#099b88"]//通过修改全局的默认样式来得到我们自己想要的颜色
                    };
                    myChart_post.setOption(option_post);
                    if(data_publish.length==0){
                        $('#publish_article').hide();
                    }

                    //阅读数量柱状图
                    var data_readavgcount = [data.Result.ReadForWeb.ReadAvgSingleCount, data.Result.ReadForWeb.ReadAvgFirstCount, data.Result.ReadForWeb.ReadAvgSencondCount, data.Result.ReadForWeb.ReadAvgThirdCount],
                        data_readmaxcount = [data.Result.ReadForWeb.ReadHighestSingleCount, data.Result.ReadForWeb.ReadHighestFirstCount, data.Result.ReadForWeb.ReadHighestSencondCount, data.Result.ReadForWeb.ReadHighestThirdCount],
                        MaxData = Math.ceil(Math.max.apply(null, data_readmaxcount));
                        MaxData = Math.ceil(MaxData*(4/3));
                    var b_readcount = $('#readcount').get(0);
                    var myChart_readcount = echarts.init(b_readcount);
                    var option_readcount = {
                        title: {
                            // text: '近30天，广告位阅读数量分布',
                            // subtext: 'dom',
                            x: "center",
                            y: "bottom"
                        },
                        tooltip: {
                            // trigger: 'axis',
                            formatter: "{a}<br/>{b}:{c}个",//显示数据的方式
                            showContent: true  //是否显示提示框浮层,也就是说提示性的文字
                        },

                        legend: {
                            right: 0,
                            data: ['最高', '平均'],
                            orient: 'vertical',
                            align: "left" //图例的图形跟文字的位置
                        },
                        calculable: true,//还不太懂什么意思
                        xAxis: [
                            {
                                type: 'category',
                                data: ['单图文', '多图文头条', '多图文第二条', '多图文3—N条'],
                                axisTick: {
                                    show: false  // 是否显示刻度
                                }
                            }
                        ],
                        yAxis: [
                            {
                                type: 'value',
                                max: MaxData,
                                // max:'dataMax',//设置该类目中的最大值显示到坐标轴上
                                // triggerEvent:true,
                                axisTick: {
                                    show: true,//刻度
                                    inside: true
                                },
                                axisLabel: {
                                    show: true//刻度标签
                                },
                                name: "阅读数量",
                                splitLine: {show: false}//删除坐标轴在网格区域中的分隔线。
                            }
                        ],
                        series: [
                            {
                                name: '最高',
                                type: 'bar',
                                data: data_readmaxcount,
                                barWidth: 50//设置柱形条的宽度
                            },
                            {
                                name: '平均',
                                type: 'bar',
                                data: data_readavgcount,
                                barGap: "0%",//让不同系列的柱状图中间没有分隔
                                // barWidth:50,//设置柱形条的宽度
                            }
                        ],
                        color: ["#01afec", "#87d0f3"]
                    };
                    //防止出现小数的情况
                    if(MaxData>5){

                        }else{
                            option_readcount.yAxis[0].interval = MaxData;
                            }
                    myChart_readcount.setOption(option_readcount);
                    if(data_readavgcount.length==0&&data_readmaxcount.length==0){
                        $("#read_h3").hide();
                        $("#readcount").hide();
                    }
                }else{
                    $('#publish_article').hide();
                    $("#read_h3").hide();
                    $("#readcount").hide();
                }
                //发文次数柱状图
                if(data.Result.DayUpdateForWeb){
                    var count_x = [],
                        count_y = [];
                    for (var i = 0; i < data.Result.DayUpdateForWeb.length; i++) {
                        count_x.push(data.Result.DayUpdateForWeb[i].Key);
                        count_y.push(data.Result.DayUpdateForWeb[i].Value);
                    }
                    //最大数向上取整
                    var count_datamax = Math.ceil(Math.max.apply(null, count_y));
                    count_datamax = Math.ceil(count_datamax*(4/3));
                    console.log(count_datamax);
                    var b_publishcount = $('#publishcount').get(0);
                    var myChart_publishcount = echarts.init(b_publishcount);
                    var option_publishcount = {
                        title: {
                            // text: '近30天更新次数分布',
                            x: 'center',//水平居中
                            y: "bottom",
                            // bottom:"bottom",
                            // textBaseline: 'bottom',
                            // textAlign:"center",
                            textStyle: {
                                color: "red"
                            }
                        },
                        color: ['#00b0ec'],
                        tooltip: {
                            // trigger: 'axis',
                            // axisPointer : {            // 坐标轴指示器，坐标轴触发有效
                            //     type : 'line'        // 默认为直线，可选为：'line' | 'shadow'
                            // }
                        },
                        grid: {
                            show: false,
                            left: '3%',
                            right: '4%',
                            bottom: '3%',
                            containLabel: true
                        },
                        xAxis: [
                            {
                                type: 'category',
                                // splitNumber:30,
                                // // minInterval:5,
                                // max:30,
                                // min:0,
                                data: count_x,
                                axisTick: {
                                    show: false, //是否显示坐标轴的刻度
                                    // alignWithLabel: true

                                },
                                axisLine: {
                                    show: true
                                },
                                splitLine: {
                                    show: false  //不显示分割线
                                }

                            }
                        ],
                        yAxis: [
                            {
                                name: "发文次数",//坐标的名字
                                type: 'value',
                                max: count_datamax,
                                axisTick: {
                                    show: true,//是否显示刻度
                                    inside: true//刻度的朝向，默认朝外false
                                },
                                axisLine: {
                                    show: true//是否显示坐标轴
                                },
                                axisLabel: {
                                    show: true//是否显示坐标轴刻度标签
                                },
                                splitLine: {
                                    show: false//坐标轴的分隔线
                                }
                            }

                        ],
                        series: [
                            {
                                name: '发文次数',
                                type: 'bar',
                                // barWidth: 10,//柱条的宽度
                                data: count_y,
                                // animation: false
                            }
                        ]
                    };
                    if(count_datamax>5){

                              }else{
                                  option_publishcount.yAxis[0].interval = count_datamax;
                              }
                    myChart_publishcount.setOption(option_publishcount);
                    if(count_x.length==0&&count_y.length==0){
                        $("#count_h3").hide();
                        $("#publishcount").hide();
                    }
                }else{
                    $("#count_h3").hide();
                    $("#publishcount").hide();
                }
                //发文时间柱状图
                if(data.Result.HourUpdateForWeb){
                    var time_y = [];
                    for (var i = 0; i < data.Result.HourUpdateForWeb.length; i++) {
                        time_y.push(data.Result.HourUpdateForWeb[i].Value);
                    }
                    time_y = time_y.sort();
                    var time_datamax = Math.ceil(time_y[time_y.length - 1]);
                    time_datamax = Math.ceil(time_datamax*(4/3));
                    var b_publishtime = $('#publishtime').get(0);
                    var myChart_publishtime = echarts.init(b_publishtime);
                    var option_publishtime = {
                        title: {
                            // text: '近30天更新次数分布',
                            x: 'center',//水平居中
                            y: "bottom",
                            // bottom:"bottom",
                            // textBaseline: 'bottom',
                            // textAlign:"center",
                            textStyle: {
                                color: "red"
                            }
                        },
                        color: ['#00b0ec'],
                        tooltip: {
                            // trigger: 'axis',
                            // axisPointer : {            // 坐标轴指示器，坐标轴触发有效
                            //     type : 'line'        // 默认为直线，可选为：'line' | 'shadow'
                            // }
                        },
                        grid: {
                            show: false,
                            left: '3%',
                            right: '4%',
                            bottom: '3%',
                            containLabel: true
                        },
                        xAxis: [
                            {
                                type: 'category',
                                // splitNumber:30,
                                // // minInterval:5,
                                // max:30,
                                // min:0,
                                data: ['00:00', '01:00', '02:00', '03:00', '04:00', '05:00', '06:00', '07:00', '08:00', '09:00', '10:00', '11:00', '12:00', '13:00', '14:00', '15:00', '16:00', '17:00', '18:00', '19:00', '20:00', '21:00', '22:00', '23:00', '24:00'],
                                axisTick: {
                                    show: false, //是否显示坐标轴的刻度
                                    // alignWithLabel: true
                                },
                                axisLine: {
                                    show: true
                                },
                                splitLine: {
                                    show: false  //不显示分割线
                                }

                            }
                        ],
                        yAxis: [
                            {
                                name: "发文次数",//坐标的名字
                                type: 'value',
                                max: time_datamax,
                                axisTick: {
                                    show: true,//是否显示刻度
                                    inside: true//刻度的朝向，默认朝外false
                                },
                                axisLine: {
                                    show: true//是否显示坐标轴
                                },
                                axisLabel: {
                                    show: true//是否显示坐标轴刻度标签
                                },
                                splitLine: {
                                    show: false//坐标轴的分隔线
                                }
                            }

                        ],
                        series: [
                            {
                                name: '不同时间段的发文量',
                                type: 'bar',
                                barWidth: 10,//柱条的宽度
                                data: time_y,
                                // animation: false
                            }
                        ]
                    };
                    if(time_datamax>5){

                    }else{
                        option_publishtime.yAxis[0].interval = time_datamax;
                    }
                    myChart_publishtime.setOption(option_publishtime);
                    if(time_y.length==0){
                        $("#time_h3").hide();
                        $("#publishtime").hide();
                    }
                }else{
                    $("#time_h3").hide();
                    $("#publishtime").hide();
                }

            }else{
                $('#publish_article').hide();
                $("#read_h3").hide();
                $("#count_h3").hide();
                $("#time_h3").hide();
                // $("#article_h2").hide();
            }
        }else{
            // $("#relative_data").hide();
            $('#publish_article').hide();
            $("#read_h3").hide();
            $("#readcount").hide();
            $("#count_h3").hide();
            $("#publishcount").hide();
            $("#time_h3").hide();
            $("#publishtime").hide();
            // $("#article_h2").hide();
        }
    }
    })).done(function(){
        if($("#data").html().trim().length==0&&$("canvas").length==0){
            $("#relative_div").hide();
        }
    })
    //案例展示页面
    $("#case").on("click", function () {
        $("#case").addClass('active');
        $("#basic").removeClass('active');
        $("#case_div").show();
        $("#basic_div").hide();
        setAjax({
            url: "/api/Media/SelectMediaCaseInfo?v=1_1&MediaType=14001&MediaID=" + MediaID + "&CaseStatus=1"
        }, function (data) {
            console.log(data);
            //填充案例页面
            if (data.Result.length!=0&&data.Result[0].CaseContent) {
                $("#case_div").html(data.Result[0].CaseContent);
                if($("#case_div").html().trim().length==0){
                    $("#case").hide();
                }
            }
        })
    });
    setAjax({
        url: "/api/Media/SelectMediaCaseInfo?v=1_1&MediaType=14001&MediaID=" + MediaID + "&CaseStatus=1"
    }, function (data) {
        console.log(data);
        //填充案例页面
        if (data.Result.length==0||data.Result[0].CaseContent=="") {
                $("#case").hide();
        }
    })
    $("#basic").on("click", function () {
        $("#basic").addClass('active');
        $("#case").removeClass('active');
        $("#case_div").hide();
        $("#basic_div").show();
    });
    //改变右侧形状大小
    $(".mui-mbar-tabs").css("width", "40px");

})
//全局声明jonp的回调函数
function getwords() {

}
