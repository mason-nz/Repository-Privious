/*
* Written by:     wangcan
* function:       购物车第一步
* Created Date:   2017-06-08
* Modified Date:  2017-07-17  
*/
$(function(){
    var userData = GetRequest();
    var orderID = userData.orderID || "";
    getInitalData();
    var flag = true;

    /*1、模糊查询广告主名称_AE、运营和超级管理员都可看到-----V1.1.8下需在此处加上销售角色的判断，销售也可以看到*/
    if(CTLogin.RoleIDs != 'SYS001RL00005' && CTLogin.RoleIDs != 'SYS001RL00004' && CTLogin.RoleIDs != 'SYS001RL00001' && CTLogin.RoleIDs != "SYS001RL00008" ){
        $('.advertiser').remove();
    }else{
        $('#adMatser').off('change').on('change',function(){
            $('.advertiser input').val('');
        })
        /*添加文本框聚焦事件，使其默认请求一次*/
        $('.advertiser input').off('focus').on('focus',function(){
           var val = $.trim($(this).val()),
                IsAEAuth = 0;
            if(val == ''){
                setAjax({
                    url:"http://www.chitunion.com/api/ADOrderInfo/GetADMaster?v=1_1",
                    type:'get',
                    data:{
                        NameOrMobile:val,
                        IsAEAuth:IsAEAuth
                    },
                    dataType:'json'
                },function(data){
                    if(data.Status == -1){
                        var availableArr = [];
                        $('.advertiser input').autocomplete({
                            source: availableArr
                        })
                    }
                });
            }
        })
        /*广告主模糊查询*/
        $('.advertiser input').off('keyup').on('keyup',function(){
            var val = $.trim($(this).val());
            if(val != ''){
                // GetADMaster(val,0);
                GetADMaster(val,$('#adMatser option:checked').attr('value'));
            }
        })

    }

    /*2、验证信息是否填写正确*/

    /*上传附件*/
    uploadFile('uploadify');
    uploadFile('uploadify1');

    /*验证需求名称*/
    $("#requireName").off('click').on('click',function(){
        $(this).siblings('.nameMsg').hide();
    }).off('blur').on('blur',function () {
        checkUsername()
    });

    /*验证需求说明*/
    $('.demand').off('click').on('click','.textArea',function(){
        $(this).parents('ul').find('.explainMsg').hide();
    }).off('blur').on('blur',function(){
        checkExplain($(this));
    })


    /*3、需求与附件共用*/
    $('.demand').off('click').on('click','.share',function(){
        var _this = $(this);
        //当前需求
        var curDemand = $.trim(_this.parents('.demand').find('.textArea').val());
        //当前已上传的文件名称
        var curUploadFileName = _this.parents('.demand').find('.ImgUp .imgName').html();
        //当前文件的url
        var curUrl = _this.parents('.demand').find('.ImgUp .imgDownURL').attr('href');
        //如果需求不为空，再执行复制操作
        if(curDemand){
            //勾选微信后的“需求与附件共用”
            if(_this.attr('id') == 'weixinShare'){
                $('#appShare').parents('.demand').find('.textArea').val(curDemand);
            }else{
                $('#weixinShare').parents('.demand').find('.textArea').val(curDemand);
            }
        }
        //如果已上传文件，再执行复制操作
        if(curUploadFileName){
            if(_this.attr('id') == 'weixinShare'){
                $('#appShare').parents('.demand').find('.ImgUp').show().end().find('.imgName').text(curUploadFileName).end().find('.imgDownURL').attr('href',curUrl);
            }else{
                 $('#weixinShare').parents('.demand').find('.ImgUp').show().end().find('.imgName').text(curUploadFileName).end().find('.imgDownURL').attr('href',curUrl);
            }
        }
    })

    /*4、提交需求*/

    $('#submitNeed').off('click').on('click',function(){
        //如果是广告主，不需要验证广告主名称。销售角色下，只验证广告主名称非空
        if(CTLogin.RoleIDs != 'SYS001RL00002'){
            if(checkAdvName($('#adMatser option:checked').attr('value')) == false){
                $(".advertiser").attr("userID","");
            }
        }
        checkUsername();
        // V1.1.8下AE/运营/超管/销售角色时 上传物料可以为空，非必填项.而广告主下，需验证附件非空
        if(CTLogin.RoleIDs == 'SYS001RL00002'){
            checkAttachment();
        }
        checkExplain();
        
        var arr = [];
        $('.tipInfo:visible').each(function(){
            arr.push($.trim($(this).html()))
        })
        for(var i=0;i<arr.length;i++){
            if(arr[i]){
                return
            }
        }
        var data = getAllInfo();
        setAjax({
            url:'http://www.chitunion.com/api/ADOrderInfo/AddOrUpdate_ADOrderInfo?v=1_1',
            type:'post',
            data:data
        },function(data){
            if(data.Status == 0){
                window.location = '/OrderManager/shopcartForWeixin_new03.html?&orderID=' + data.Message;
            }
        })
    })


    //点击“继续选择广告位”，跳转到首页
    $('#goToList').off('click').on('click',function(){
        window.location = '/index.html';
    })

    /*切换展开收起状态*/
    $('#changeDisplay').off('click').on('click','img',function(){
        var _this = $(this);
        if(_this.attr('src') == '../images/collection08.png'){
            _this.attr('src','../images/collection09.png');
            $('.orderInfo').hide();
        }else{
            _this.attr('src','../images/collection08.png');
            $('.orderInfo').show();
            //判断，是否显示节假日单价
            $('.orderInfo .tr').each(function(){
                if($(this).find('.lookFestival:hidden').length == $(this).find('.lookFestival').length){
                    $(this).children().eq(4).find('p:eq(1)').hide();
                }else{
                    $(this).children().eq(4).find('p:eq(1)').show();
                }
            });
        }
    })
    /*返回购物车*/
    $('.returnToPrev').off('click').on('click',function(){
        //AE代客下单或直接从一级菜单而来
        if(!orderID){
            //AE代客下单时，会带着userID
            if(GetUserId().userID){
                window.location = '/OrderManager/shopcartForMedia01.html&userID=' + GetUserId().userID;
                return
            }
            //一级菜单而来，只带媒体类型
            window.location = '/OrderManager/shopcartForMedia01.html';
        //从审核页面跳转而来
        }else{
            window.location = '/OrderManager/shopcartForMedia01.html&orderID='+orderID;
        }
    })

    /*调接口获取页面初始化数据*/
    function getInitalData(){
        setAjax({
            url:'http://www.chitunion.com/api/ShoppingCart/GetInfo_ShoppingCart?v=1_1',
            type:'get',
            data:{}
        },function(data){
            if(data.Status == 0){
                var str = $('#schedule').html();
                var html = ejs.render(str, {data: data.Result});
                $('.orderInfo').append(html);
                $('.orderInfo').attr('result',JSON.stringify(data.Result));

                //判断，如果没有APP，APP广告需求不显示。微信需求与附件共用不显示
                var appLen = 0,weixinLen = 0;
                $('.orderInfo').find('.tr').each(function(){
                    if($(this).attr('MediaType') == '14001'){
                        weixinLen++;
                    }else{
                        appLen++;
                    }
                });
                if(appLen == 0){
                    $('.wx_box').find('.demand:last').hide();
                    $('.wx_box').find('.demand:first #weixinShare').hide();
                }
                if(weixinLen == 0){
                    $('.wx_box').find('.demand:last #appShare').hide();
                    $('.wx_box').find('.demand:first').hide();
                }
                //所有的textarea  val=""
                $('.textArea').each(function(){
                    $(this).val('');
                });

                //如果是广告主，附件为必填项，显示“*”
                if(CTLogin.RoleIDs == 'SYS001RL00002'){
                    $('.fujian').show();
                }
                //显示媒体个数和总价
                $('.tableNum').html($('.validAdv').length);
                var TotalAmmount = 0;
                $('.validAdv').each(function(){
                    TotalAmmount += ($(this).attr('TotalAmmount')-0);
                });
                $('.OrderPrice').html(formatMoney(TotalAmmount));
                //如果媒体下没有广告位，移除媒体名称
                $('.MediaOwner').find('table').each(function(){
                    if($(this).children().length == 0){
                        $(this).parents('.MediaOwner').remove();
                    }
                });
                //鼠标移入“查看节假日，显示节假日的具体信息”
                $('.validAdv').on('mouseover','.lookFestival',function(){
                    $(this).next('.festivals').show();
                }).on('mouseout','.lookFestival',function(){
                    $(this).next('.festivals').hide();
                })
            }
        })
    }


    /*提交需求时获取页面信息（不包含未选中和已下架、过期）*/
    function getAllInfo(){
        // V1.1.18第二部分修改为：
        var ADOrderInfo = {
            "OrderID" : "",
            "OrderName" : "",
            "Status" : 16002,
            "CustomerID" : "gt86ZRCRjng%3d",
            "CRMCustomerID" : "",
            "CustomerText" : ""
        };
        ADOrderInfo.OrderName = $.trim($('#requireName').val());
        ADOrderInfo.CustomerText = $.trim($('.advertiser').find('input').val());
        // V1.1.8下如果是销售角色，取文本框输入的广告主名称，如果
        //其他角色：验证名称通过后，按照是CRM还是赤兔修改CustomerID/CRMCustomerID的值

        if($.trim($('.advertiser').attr('userid')) != ""){
            //赤兔
            if($('#adMatser option:checked').attr('value') == 0){
                ADOrderInfo.CustomerID = $.trim($('.advertiser').attr('userid'));
            //CRM
            }else{
                ADOrderInfo.CRMCustomerID = $.trim($('.advertiser').attr('userid'));
            }
        }

        var MediaOrderInfos = [];
        if($('.demandForWeixin').css('display') == 'block'){
            var demandInfo = {
                "MediaType" : 14001,
                "Note" : $.trim($('#weixinShare').next().find('.textArea').val()),
                "UploadFileURL" : $('#weixinShare').parents('.demand').find('.ImgUp .imgDownURL').attr('href')
            };
            MediaOrderInfos.push(demandInfo);
        }
        if($('.demandForAPP').css('display') == 'block'){
            var demandInfo = {
                "MediaType" : 14002,
                "Note" : $.trim($('#appShare').next().find('.textArea').val()),
                "UploadFileURL" : $('#appShare').parents('.demand').find('.ImgUp .imgDownURL').attr('href')
            };
            MediaOrderInfos.push(demandInfo);
        }

        var ADDetails = [];
        var result = JSON.parse($('.orderInfo').attr('result'));
        //APP
        for(var i=0;i<result.APP.length;i++){
            for(var j=0;j<result.APP[i].Medias.length;j++){
                //页面模板根据这些条件判断是否显示，而提交订单时，不需要向后台传这些未选择和失效的数据，进行筛选，微信一样如此
                if(result.APP[i].Medias[j].PublishStatus == 15006 || result.APP[i].Medias[j].expired == 1 || result.APP[i].Medias[j].expired == -1 || result.APP[i].Medias[j].IsSelected != 1){
                }else{
                    var ADScheduleInfos = [];
                    for(var k=0;k<result.APP[i].Medias[j].ADSchedule.length;k++){
                        var ADSchedule = {
                           "BeginData" : result.APP[i].Medias[j].ADSchedule[k].BeginData,
                           "EndData" : result.APP[i].Medias[j].ADSchedule[k].EndData
                        }
                        ADScheduleInfos.push(ADSchedule);
                    }
                    var obj = {
                        "MediaType" : 14002,
                        "MediaID" : result.APP[i].Medias[j].MediaID,
                        "CartID": result.APP[i].Medias[j].CartID,
                        "PubDetailID" : result.APP[i].Medias[j].PublishDetailID,
                        "SaleAreaID" : result.APP[i].Medias[j].SaleAreaID,
                        "AdjustPrice" : result.APP[i].Medias[j].Price,
                        "AdjustDiscount" : 1,
                        "ADLaunchDays" : result.APP[i].Medias[j].ADLaunchDays,
                        "Holidays": 0,
                        "ADScheduleInfos" : ADScheduleInfos
                    }
                    ADDetails.push(obj);
                }
            }
        }
        //微信
        for(var i=0;i<result.SelfMedia.length;i++){
            for(var j=0;j<result.SelfMedia[i].Medias.length;j++){
                if(result.SelfMedia[i].Medias[j].PublishStatus == 15006 || result.SelfMedia[i].Medias[j].expired == 1 || result.SelfMedia[i].Medias[j].expired == -1 || result.SelfMedia[i].Medias[j].IsSelected != 1){
                }else{
                    var ADScheduleInfos = [];
                    for(var k=0;k<result.SelfMedia[i].Medias[j].ADSchedule.length;k++){
                        var ADSchedule = {
                           "BeginData" : result.SelfMedia[i].Medias[j].ADSchedule[k].BeginData,
                           "EndData" : result.SelfMedia[i].Medias[j].ADSchedule[k].BeginData
                        }
                        ADScheduleInfos.push(ADSchedule);
                    }
                    var obj = {
                        "MediaType" : 14001,
                        "MediaID" : result.SelfMedia[i].Medias[j].MediaID,
                        "CartID" : result.SelfMedia[i].Medias[j].CartID,
                        "PubDetailID" : result.SelfMedia[i].Medias[j].PublishDetailID,
                        "SaleAreaID" : -2,
                        "AdjustPrice" : result.SelfMedia[i].Medias[j].Price,
                        "AdjustDiscount" : 1,
                        "ADLaunchDays" : result.SelfMedia[i].Medias[j].ADSchedule.length,
                        "Holidays": 0,
                        "ADScheduleInfos" : ADScheduleInfos
                    }
                    ADDetails.push(obj);
                }
            }

        }
        var dataObj = {
            "optType" : 1,
            "ADOrderInfo" :ADOrderInfo,
            "MediaOrderInfos" :MediaOrderInfos,
            "ADDetails" :ADDetails
        }
        return dataObj;
    }

    /*模糊查询广告主名称，传入参数为查询方式：0为查询自己创的和授权的，1为只查询授权的*/
    function GetADMaster(nameOrMobile,IsAEAuth){
        setAjax(
            {
                type: "get",
                url: "http://www.chitunion.com/api/ADOrderInfo/GetADMaster?v=1_1",
                data: {
                    NameOrMobile: nameOrMobile,
                    IsAEAuth:IsAEAuth
                }
            },
            function (data) {
                var availableAdv = [];
                if(data.Status == 0){
                    for(var i=0;i<data.Result.length;i++){
                        if(data.Result[i].Mobile.toLowerCase().indexOf(nameOrMobile.toLowerCase()) != -1){
                            if(availableAdv.length<10){
                                availableAdv.push(data.Result[i].Mobile);
                            }
                        }
                        if(data.Result[i].TrueName.toLowerCase().indexOf(nameOrMobile.toLowerCase()) != -1){
                            if(availableAdv.length<10){
                                availableAdv.push(data.Result[i].TrueName);
                            }
                        }
                    }
                    $(".advertiser input").autocomplete({
                        source: availableAdv,
                        select:function(event,ui){
                            $('.advertiser').attr('userID',ui.value)
                        }
                    })
                }

            }
        )
    }

    /*验证广告主填写是否正确，若正确，将对应的userID绑定到标签上，若不正确，显示错误提示信息*/
    function checkAdvName(IsAEAuth){
        var flag = true;
        var advName = $(".advertiser input").val();
        if(advName != ""){
            //如果是销售角色，需验证广告主名称是否为空，不需调验证是否存在。但如果用户选择了库里有的广告主名称，需要把对应的ID返给后台
            /*获取当前广告主名称之后，进行模糊查询，当前广告主是否存在*/
            $.ajax({
                type: "get",
                url: "http://www.chitunion.com/api/ADOrderInfo/GetADMaster?v=1_1",
                data: {
                    NameOrMobile: advName,
                    IsAEAuth:IsAEAuth
                },
                dataType: 'json',
                async: false,
                xhrFields: {
                    withCredentials: true
                },
                crossDomain: true,
                success:function(data){
                    /*count为广告主名称不匹配的数量*/
                    var count = 0;
                    if(data.Result == null){
                        $(".advertiser .advMsg").html("<img src='../images/icon21.png'> 广告主不存在，请重新输入！");
                        $("#nextBtn").css("background-color", "#666");
                        flag = false;
                    }else{
                        for(var i=0;i<data.Result.length;i++){
                            if(data.Result[i].Mobile != advName){
                                count++;
                            }
                            if(data.Result[i].TrueName != advName){
                                count++;
                            }

                        }
                        /*由于查询时会匹配数据中的真实名称和手机号，所以若广告主名称不匹配的数量 = 总查询的数据总量时，说明都不匹配*/
                        if(count == data.Result.length * 2){
                            $(".advertiser .advMsg").html("<img src='../images/icon21.png'> 广告主不存在，请重新输入！");
                            flag = false;
                            $("#nextBtn").css("background-color", "#666");
                        }else{
                            /*匹配时再次调用接口，把当前广告主名称对应的userID绑定到页面上*/
                            $.ajax({
                                type: "get",
                                url: "http://www.chitunion.com/api/ADOrderInfo/GetADMaster?v=1_1",
                                data: {
                                    NameOrMobile: advName,
                                    IsAEAuth:IsAEAuth
                                },
                                dataType: 'json',
                                async: false,
                                xhrFields: {
                                    withCredentials: true
                                },
                                crossDomain: true,
                                success:function(data){
                                    for(var i=0;i<data.Result.length;i++){
                                        for(j in data.Result[i]){
                                            if(advName == data.Result[i][j]){
                                                $(".advertiser").attr("userID",data.Result[i].UserID);
                                            }
                                        }
                                    }
                                }
                            });
                            $(".advertiser .advMsg").html("");
                            flag = true;
                        }
                    }
                }
            });
        }else{
            $(".advertiser .advMsg").html("<img src='../images/icon21.png'> 广告主名称不能为空");
            flag = false;
        }
        return flag;
    }

    /*验证需求说明*/
    function checkExplain(event) {
        var flag = true;
        if(event){
            var requireExplain = $.trim(event.val());
            if (requireExplain === "") {
                event.parents('ul').find('.explainMsg').show().html("<img src='../images/icon21.png'> 请填写需求说明")
                flag = false;
            } else {
                event.parents('ul').find('.explainMsg').html(" ")
            }
        }else{
            $('.textArea').each(function(){
                var requireExplain = $.trim($(this).val());
                if (requireExplain === "") {
                    $(this).parents('ul').find('.explainMsg').show().html("<img src='../images/icon21.png'> 请填写需求说明")
                    flag = false;
                } else {
                    $(this).parents('ul').find('.explainMsg').html(" ")
                }
            });
        }
        return flag;
    };

    /*验证需求名称*/
    function checkUsername() {
        var requireName = $('#requireName').val()
        if (requireName === "") {
            $('.nameMsg').html("<img src='../images/icon21.png'> 请填写需求名称")
            return false;
        } else if (requireName.length < 3 || requireName.length > 25) {
            $('.nameMsg').html("<img src='../images/icon21.png'> 字符长度为3-25")
            return false;
        } else {
            $('.nameMsg').text("")
            return true;
        }
    }
    /*验证是否上传附件*/
    function checkAttachment(event) {
        var flag = true;
        if(event){
            var imgName = event.parents('ul').next().find('.imgName').text();
            if (imgName === "") {
                event.parents('ul').find('.imgMsg').html("<img src='../images/icon21.png'> 请上传附件")
                flag = false;
            } else {
                event.parents('ul').find('.imgMsg').html(" ").hide();
            };
        }else{
            $('.imgName').each(function(){
                if( !$.trim($(this).text())){
                    $(this).parents('ul').prev().find('.imgMsg').html("<img src='../images/icon21.png'> 请上传附件");
                    flag = false;
                }
            });
        }
        return flag;
    }

    /*上传附件*/
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
            'buttonText': '+ 上传附件',
            'buttonClass': 'but_upload',
            'swf': '/Js/uploadify.swf?_=' + Math.random(),
            'uploader': 'http://www.chitunion.com/AjaxServers/UploadFile.ashx',
            'auto': true,
            'multi': false,
            'width': 200,
            'height': 35,
            'formData': { Action: 'BatchImport', CarType: '', LoginCookiesContent: escapeStr(getCookie('ct-uinfo')) },
            'fileTypeDesc': '支持格式:jpg,jpeg,png,pdf,zip ',
            'fileTypeExts': '*.jpg;*.jpeg;*.png;*.pdf;*.zip;',
            'fileSizeLimit':'10MB',
            'queueSizeLimit': 1,
            'scriptAccess': 'always',
            'onQueueComplete': function (event, data) {
                //enableConfirmBtn();
            },
            'onQueueFull': function () {
                layer.alert('您最多只能上传1个文件！');
                return false;
            },
            'onUploadSuccess': function (file, data, response) {
                if (response == true) {
                    var json = $.evalJSON(data);
                    $('#'+id).parents('ul').next().show();
                    $('#'+id).parents('ul').next().find('.imgName').text(json.FileName);
                    $('#'+id).parents('ul').next().find('.imgDownURL').attr("href","http://www.chitunion.com" + json.Msg);
                    // V1.1.8下AE/运营/超管/销售角色时 上传物料可以为空，非必填项，而广告主下需验证附件非空
                    if(CTLogin.RoleIDs == 'SYS001RL00002'){
                        checkAttachment($('#'+id));
                    }
                }
            },
            'onProgress': function (event, queueID, fileObj, data) {},
            'onUploadError': function (event, queueID, fileObj, errorObj) {
                console.log(errorObj);
                //enableConfirmBtn();
            },
            'onSelectError':function(file, errorCode, errorMsg){
                console.log(errorCode);
            }
        });

    };
    // 获取url？后面的参数
    function GetRequest() {
        var url = location.search; //获取url中"?"符后的字串
        var theRequest = new Object();
        if (url.indexOf("?") != -1) {
            var str = url.substr(1);
            strs = str.split("&");
            for (var i = 0; i < strs.length; i++) {
                theRequest[strs[i].split("=")[0]] = unescape(strs[i].split("=")[1]);
            }
        }
        return theRequest;
    }
    function GetUserId(){
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
    }
});
