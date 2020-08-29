/*
* Written by:     wangcan
* function:       智投提交需求
* Created Date:   2017-07-24
* Modified Date:  2017-08-21 增加导出媒体
*/
$(function(){
    function SubmmitDemand(){
        this.initData(GetRequest().orderID);
        this.operation();
    }
    SubmmitDemand.prototype = {
        constructor:SubmmitDemand,
        initData:function(orderID){//初始化数据
            setAjax({
                url:'http://www.chitunion.com/api/ADOrderInfo/IntelligenceADOrderInfoQuery?v=1_1',
                type:'get',
                data:{
                    orderid : orderID
                }
            },function(data){
                if(data.Status == 0){
                    var str = $('#orderInfo').html();
                    var html = ejs.render(str, {list: data.Result});
                    $('.orderInfo').append(html);
                    $('.orderInfo').attr('ADOrderInfo',JSON.stringify(data.Result.ADOrderInfo));

                    //显示媒体个数和总价
                    getAllCount();
                    //所有的textarea  val=""
                    $('textArea').each(function(){
                        $(this).val('');
                    });
                    //品牌需求渲染：
                    $('#projectDemnd').val(data.Result.ADOrderInfo.Note);
                    //渲染项目名称：城市+品牌车型
                    var projectName;
                    var cityNames = [];
                    data.Result.AreaInfos.forEach(function(v,i){
                        if(v.CityName != ''){
                            cityNames.push(v.CityName)
                        }else{
                            cityNames.push(v.ProvinceName)
                        }
                    });
                    $('#requireName').val(cityNames.join('/')+'+'+data.Result.ADOrderInfo.MasterName+'/'+data.Result.ADOrderInfo.BrandName+'/'+data.Result.ADOrderInfo.SerialName);
                    $('#requireName').attr('OrderName',$('#requireName').val());
                }else{
                    layer.msg(data.Message);
                }
            })
            //获取媒体信息：媒体个数、销售参考价、成本参考价、原创参考价
            function getAllCount(){
                //媒体数目
                var mediaNumber = $('.orderInfo .mediaInfo').length;
                $('.bottom').find('.tableNum').html(mediaNumber);
                //销售价、成本价、原创价
                var SalePrice = 0,CostReferencePrice = 0,OriginaReferencePrice = 0;
                $('.orderInfo').find('.mediaInfo').each(function(i,item) {
                    SalePrice += ($(item).attr('SalePrice')-0);
                    CostReferencePrice += ($(item).attr('CostReferencePrice')-0);
                    if($(item).attr('EnableOriginPrice') == 'true'){
                        OriginaReferencePrice += ($(item).attr('OriginaReferencePrice')-0);
                        SalePrice += ($(item).attr('OriginaReferencePrice')-0);
                        CostReferencePrice += ($(item).attr('OriginaReferencePrice')-0);
                    }
                });
                $('.SalePrice').html(formatMoney(SalePrice));
                $('.CostReferencePrice').html(formatMoney(CostReferencePrice));
                $('.OriginaReferencePrice').html(formatMoney(OriginaReferencePrice));
            }
        },
        operation:function(){
            /*1、模糊查询广告主名称*/
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
                    GetADMaster(val,$('#adMatser option:checked').attr('value'));
                }
            })
            /*上传附件*/
            uploadFile('uploadify');

            /*2、验证信息是否填写正确*/
            /*验证项目名称*/
            $("#requireName").off('blur').on('blur',function () {
                checkUsername()
            });

            /*验证营销政策*/
            $('.textArea').off('focus').on('focus',function(){
                $(this).parents('ul').find('.explainMsg').hide();
            }).off('blur').on('blur',function(){
                checkExplain($(this));
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
                }
            })

            /*导出媒体*/
            $('#outPut').off('click').on('click',function () {
                setAjax({
                    url:'http://www.chitunion.com/api/ADOrderInfo/IntelligenceRecommendADOrderExport?v=1_1',
                    type:'get',
                    data:{
                        orderID : GetRequest().orderID,
                        orderName : $.trim($('#requireName').val())?$.trim($('#requireName').val()):$('#requireName').attr('OrderName')
                    }
                },function(data){
                    if(data.Status == 0){
                        window.location = decodeURIComponent(data.Result);
                    }
                })
            })
            /*4、提交需求*/
            $('#submitNeed').off('click').on('click',function(){
                //销售角色下，只验证广告主名称非空，无论什么角色，如果广告主名称验证失败，把USERid置为“”
                if(checkAdvName($('#adMatser option:checked').attr('value')) == false){
                    $(".advertiser").attr("userID","");
                }

                checkUsername();

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
                var data = getAllInfo(GetRequest().orderID);
                setAjax({
                    url:'http://www.chitunion.com/api/ADOrderInfo/IntelligenceADOrderInfoCrud?v=1_1',
                    type:'post',
                    data:data
                },function(data){
                    if(data.Status == 0){
                        window.location = '/OrderManager/shopcartForWisdomDelivery03.html?&orderID=' + data.Message;
                    }else{
                        layer.msg(data.Message);
                    }
                })
            })
            //获取媒体信息：媒体个数、销售参考价、成本参考价、原创参考价
            function getAllCount(){
                //媒体数目
                var mediaNumber = $('.orderInfo .mediaInfo').length;
                $('.bottom').find('.tableNum').html(mediaNumber);
                //销售价、成本价、原创价
                var SalePrice = 0,CostReferencePrice = 0,OriginaReferencePrice = 0;
                $('.orderInfo').find('.mediaInfo').each(function(i,item) {
                    SalePrice += ($(item).attr('SalePrice')-0);
                    CostReferencePrice += ($(item).attr('CostReferencePrice')-0);
                    if($(item).attr('EnableOriginPrice') == 'true'){
                        OriginaReferencePrice += ($(item).attr('OriginaReferencePrice')-0);
                        SalePrice += ($(item).attr('OriginaReferencePrice')-0);
                        CostReferencePrice += ($(item).attr('OriginaReferencePrice')-0);
                    }
                });
                $('.SalePrice').html(formatMoney(SalePrice));
                $('.CostReferencePrice').html(formatMoney(CostReferencePrice));
                $('.OriginaReferencePrice').html(formatMoney(OriginaReferencePrice));
            }
            /*提交需求时获取页面信息*/
            function getAllInfo(orderID){
                var returnADOrderInfo = JSON.parse($('.orderInfo').attr('ADOrderInfo'));
                var dataObj = {
                   "optType" : 3,
                   "ADOrderInfo" : {
                        "OrderID" : orderID,
                        "OrderName" : $.trim($('#requireName').val()),
                        "Status" : 16002,
                        "CustomerID" : "gt86ZRCRjng%3d",
                        "CRMCustomerID" : "",
                        "CustomerText" : $.trim($('.advertiser').find('input').val()),
                        "MarketingPolices" : $.trim($('.textArea').val()),
                        "UploadFileURL" : $('.imgDownURL').attr('href')=='javascript:;'?'':$('.imgDownURL').attr('href'),
                        "Note":$.trim($('#projectDemnd').val()),
                        "LaunchTime" : returnADOrderInfo.LaunchTime,
                        "BudgetTotal":returnADOrderInfo.BudgetTotal,
                        "OrderRemark" : returnADOrderInfo.OrderRemark,
                        "MasterID" : returnADOrderInfo.MasterID,
                        "BrandID" : returnADOrderInfo.BrandID,
                        "SerialID" : returnADOrderInfo.SerialID,
                        "MasterName" : returnADOrderInfo.MasterName,
                        "BrandName" : returnADOrderInfo.BrandName,
                        "SerialName" : returnADOrderInfo.SerialName,
                        "JKEntrance" : returnADOrderInfo.JKEntrance 

                   },
                   "ADDetails" : []
                }
                // V1.1.8下如果是销售角色，取文本框输入的广告主名称即可，如果用户选择了广告主，把对应ID传给后台，其他角色：验证名称通过后，按照是CRM还是赤兔修改CustomerID/CRMCustomerID的值

                if($.trim($('.advertiser').attr('userid')) != ""){
                    //赤兔
                    if($('#adMatser option:checked').attr('value') == 0){
                        dataObj.ADOrderInfo.CustomerID = $.trim($('.advertiser').attr('userid'));
                    //CRM
                    }else{
                        dataObj.ADOrderInfo.CRMCustomerID = $.trim($('.advertiser').attr('userid'));
                    }
                }
                return dataObj;
            }

            /*模糊查询广告主名称，传入参数为查询方式：1为查询赤兔用户，2为查询CRM用户*/
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
                                    $('.advertiser').attr('userID',ui.item.value)
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

            /*验证营销政策说明*/
            function checkExplain() {
                var flag = true;
                $('.textArea').each(function(){
                    var requireExplain = $.trim($(this).val());
                    if (requireExplain === "") {
                        $(this).parents('ul').find('.explainMsg').show().html("<img src='../images/icon21.png'> 请填写营销政策")
                        flag = false;
                    } else {
                        $(this).parents('ul').find('.explainMsg').html(" ")
                    }
                });
                return flag;
            };

            /*验证项目名称*/
            function checkUsername() {
                var requireName = $('#requireName').val()
                if (requireName === "") {
                    $('.nameMsg').html("<img src='../images/icon21.png'> 请填写项目名称")
                    return false;
                } else if (requireName.length < 3 || requireName.length > 25) {
                    $('.nameMsg').html("<img src='../images/icon21.png'> 字符长度为3-25")
                    return false;
                } else {
                    $('.nameMsg').text("")
                    return true;
                }
            }

        }
    }
    new SubmmitDemand();
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
            'fileTypeDesc': '支持格式:jpg,jpeg,png,pdf,zip',
            'fileTypeExts': '*.jpg;*.jpeg;*.png;*.pdf;*.zip',
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

});
