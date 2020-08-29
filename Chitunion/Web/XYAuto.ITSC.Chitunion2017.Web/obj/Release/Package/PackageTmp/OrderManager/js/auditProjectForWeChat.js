/**
 * Created by fengb on 2017/4/28.
 */

$(function () {


    var orderID = GetQueryString('orderID')!=null&&GetQueryString('orderID')!='undefined'?GetQueryString('orderID'):null;

    var config = {};
    var adscheduleArr = [];

    var DetailOfOrder = {
        constructor : DetailOfOrder,
        init : function () {
            //根据项目号查看项目的接口  取信息
            var url = 'js/listDetail.json';
            //var url = '/api/ADOrderInfo/GetByOrderID_ADOrderInfo?v=1_1';
            setAjax({
                url : url,
                type : 'get',
                data : {
                    orderid : orderID
                }
            },function (data) {
                config = data.Result;
                var Status = config.ADOrderInfo.Status;
                config.Status = Status;
                
                DetailOfOrder.showTemplate(config,'#AuditProjecrForWeChat');
                DetailOfOrder.returnAllMoney();
                DetailOfOrder.ModifyAmount();//计算金额
                DetailOfOrder.returnStateText(config.ADOrderInfo.Status);
                DetailOfOrder.uploadImg("headUploadify","","","",'');//上传
                DetailOfOrder.addSchedule();
                DetailOfOrder.operateSchedule();
                DetailOfOrder.VerificationInformation();//验证价格和金钱
                DetailOfOrder.shoppingCar();//继续添加购物车
                DetailOfOrder.removePosition();//删除广告位
            })
        },
        showTemplate : function (data,id) {
            var _this = this;
            //->首先把页面中模板的innerHTML获取到
            var str=$(id).html();
            //->然后把str和data交给EJS解析处理，得到我们最终想要的字符串
            var result = ejs.render(str, {
                data: data
            });
            //将渲染出来的排期赋值给tr  方便修改和添加之后取数据

            //->最后把获取的HTML放入到MENU
            $(".install_box").html(result);
            $('.wx_form .tr').each(function (i,v) {
                var that = $(this);

                //获取排期的具体时间  赋值给tr属性
                var newarr = '';
                newarr += ($.trim(that.find('.Time').find('span').text()));
                var detailTime = newarr.split('  ');
                that.attr('beginSchedule',detailTime);

                //获取广告位所属媒体或刊例是否已经删除  若删除则置灰 并且不能进行任何操作
                var expired = $(this).attr('expired');
                if(expired == -1){
                    $('<div style="position: absolute;width: 100%;height: 100%;background: rgba(242,242,242,0.5);left:0;top:0"></div>').appendTo(that);
                    that.find('.portrait').append('<span style="position: absolute;width: 100%;height: 100%;background: rgba(0,0,0,0.5);left:0;top:0;color: #fff;text-align: center;line-height: 80px">已失效</span>')
                }
            })
        },
        returnStateText : function(status) {
            switch (status) {
                case 16001:
                    $('.orderStatus').text('草稿');
                    break;
                case 16002:
                    $('.orderStatus').text('待审核');
                    break;
                case 16003:
                    $('.orderStatus').text('待执行');
                    break;
                case 16004:
                    $('.orderStatus').text('执行中');
                    break;
                case 16005:
                    $('.orderStatus').text('已取消');
                    break;
                case 16006:
                    $('.orderStatus').text('已驳回');
                    break;
                case 16007:
                    $('.orderStatus').text('已删除');
                    break;
                case 16008:
                    $('.orderStatus').text('执行完毕');
                    break;
                case 16009:
                    $('.orderStatus').text('订单完成');
                    break;
                default:
                    break;
            }
        },
        returnAllMoney : function() {// 计算总额和广告位个数
            var allDays = 0;
            config.SubADInfos.forEach(function (items) {
                items.SelfDetails.forEach(function (item) {
                    var allDays = item.ADSchedule.length;
                    item.allDays = allDays;
                })
            })
        },
        uploadImg : function(id,img,imgerr,bigImg) {
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
            }
            function escapeStr(str) {
                return escape(str).replace(/\+/g, '%2B').replace(/\"/g, '%22').replace(/\'/g, '%27').replace(/\//g, '%2F');
            }

            $(document).ready(function (){
                $('#'+id).uploadify({
                    'buttonText': '上传',
                    'buttonClass': 'button_add',
                    'auto': true,
                    'multi': false,
                    'swf': '/Js/uploadify.swf?_=' + Math.random(),
                    'uploader': '/AjaxServers/UploadFile.ashx',
                    'width': 80,
                    'height': 20,
                    'fileTypeDesc': '支持格式:xls,jpg,jpeg,png.gif',
                    'fileTypeExts': '*.xls;*.jpg;*.jpeg;*.png;*.gif',
                    fileSizeLimit:'2MB',
                    'fileCount':1,
                    'queueSizeLimit': 1,
                    queueID:'imgShow',
                    'scriptAccess': 'always',
                    'overrideEvents' : [ 'onDialogClose'],
                    'formData': { Action: 'BatchImport', CarType: '', LoginCookiesContent: escapeStr(getCookie('ct-uinfo')), IsGenSmallImage: 1 },
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
                            $('#'+id).parent('a').attr('src',json.Msg);
                            $('.fileName').text(json.FileName);
                            $('.fileDownLoad').attr('href',json.Msg);
                        }
                    },
                    'onProgress': function (event, queueID, fileObj, data) { },
                    'onUploadError': function (event, queueID, fileObj, errorObj) {},
                    'onSelectError':function(file, errorCode, errorMsg){
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
        },
        addSchedule : function () {//添加排期  绑定数据
            $('.validAdv .add').click(function(){
                var _this = $(this);
                //获取排期数目
                var count = _this.prev().prev().children().length;
                if(count == 3){
                    return
                }
                var prevID = _this.prev().prev().children('li:last-child').find('span:first-child').attr('id');
                //获取刊例执行周期
                var beginTime = _this.parents('.wechat_list5').find('ul').attr('BeginTime'),
                    endTime = _this.parents('.wechat_list5').find('ul').attr('EndTime');

                _this.prev().prev().append('' +
                    '<li class="Time" style="visibility: hidden;">'+ '<span id="'+prevID+count+'"></span>'+ '<span  class="modify"><img src="../images/icon54.png"></span >&nbsp;' + '<span class="delete"><img src="../images/icon57.png"></span>' + '</li>');
                var curId = '#'+prevID+count;
                laydate({
                    elem: curId,
                    format: 'YYYY-MM-DD hh:mm',
                    fixed: false,
                    istoday:false,
                    istime: true,
                    isclear: false,
                    min: DetailOfOrder.getAvailableDate(beginTime,endTime)[0], //执行周期开始日期
                    max: DetailOfOrder.getAvailableDate(beginTime,endTime)[1], //执行周期结束日期
                    choose: function (date) {
                        _this.prev().prev().find('.delete').show();
                        $('.Time').css('visibility','visible');
                        //获取排期数目
                        count = _this.prev().prev().children().length;
                        if(count == 3){
                            _this.hide();
                        }
                        //添加后判断日期是否重复，若重复，移除排期
                        var oldTime = _this.prev().prev().find('li:not(:last) span:first-child');
                        var flag = true;
                        oldTime.each(function () {
                            if($.trim($(this).text()).substr(0,10) == date.substr(0,10)){
                                flag = false;
                                layer.msg('不可添加相同日期的排期');
                                $(curId).parents('li').remove();
                                _this.show();
                            }
                        });
                        if(flag == true){
                            //获取所有排期
                            allTime = _this.prev().prev().find('li span:first-child');

                            //修改排期天数
                            var paiQiDays = _this.prev().prev().children().length;//获取排期天数
                            _this.parents('.tr').attr('scheduleDays',paiQiDays);
                            //获取单价
                            var singlePrice = _this.parents('.tr').attr('OriginalPrice');
                            //总价
                            _this.parents('.wechat_list5').next().next().find('input').val(singlePrice*paiQiDays);
                            _this.parents('.tr').attr('AdjustPrice',singlePrice*paiQiDays);

                            //获取排期的具体时间  赋值给tr属性
                            var detailTime = [];
                            _this.prev().prev().find('li').each(function (i,v) {
                                var val = $.trim($(v).find('span').text());
                                detailTime.push(val);
                            })

                            _this.parents('.tr').attr('beginSchedule',detailTime);
                            DetailOfOrder.ModifyAmount();
                            newADDetails();//对象赋值
                            //排序
                            sortSchedule(allTime);
                        }

                        //修改删除操作+排序
                        DetailOfOrder.operateSchedule();
                    }
                });
                //删除多余的Li（laydate未选择日期时，会添加隐藏的li，占位）
                $(document).click(function (e) {
                    if($(e.target).parents('#laydate_box')[0] != $('#laydate_box')[0]){
                        _this.prev().prev('ul').find('li').each(function(){
                            if($(this).css('visibility') == 'hidden'){
                                $(this).remove();
                            }
                        });
                    }
                });
            });
        },
        getAvailableDate : function (beginTime,endTime) {
            /**
             * 刊例执行周期和当前日期比较，获取最新可用执行周期，作为laydate的可选日期
             * @param beginTime 从页面上获取的刊例执行周期开始日期
             * @param endTime   从页面上获取的刊例执行周期结束日期
             * @returns {Array} 数组[0]是执行周期开始日期， 数组[1]是执行周期结束日期
             */
            var availableBeginTime,availableEndTime,arr = [];
            var curTime = new Date().format('yyyy-MM-dd');
            /*如果当前日期小于开始日期*/
            if(curTime<beginTime){
                availableBeginTime = beginTime;
                availableEndTime = endTime;
            }else if(curTime > beginTime && curTime < endTime){
                /*当前日期在开始日期和结束日期之间*/
                availableBeginTime = curTime;
                availableEndTime = endTime;
            }else if(curTime >endTime){
                /*当前日期在结束日期之后——停用啦，修改添加按钮不可点击。其实不用管*/
                availableBeginTime = curTime;
                availableEndTime = curTime;
            }
            arr.push(availableBeginTime,availableEndTime);
            return arr;
        },
        operateSchedule : function () {//修改排期
            $('.validAdv .Time').on('click','.modify',function(){
                var _this = $(this);
                //获取刊例执行周期
                var beginTime = _this.parents('ul').attr('BeginTime'),
                    endTime = _this.parents('ul').attr('EndTime');
                var curOldTime = $.trim(_this.parents('li').children('span:first-child').html());
                var curId = $(this).prev().attr('id');
                laydate({
                    elem: '#'+curId,
                    format: 'YYYY-MM-DD hh:mm',
                    fixed: false,
                    istoday:false,
                    istime: true,
                    isclear: false,
                    isInitCheck : false,//解决初始化日期消失的问题
                    min: DetailOfOrder.getAvailableDate(beginTime,endTime)[0], //执行周期开始日期
                    max: DetailOfOrder.getAvailableDate(beginTime,endTime)[1], //执行周期结束日期
                    choose: function (date) {
                        //修改后判断日期是否重复，若重复，移除重复排期
                        var oldTime = _this.parents('li').siblings().find('span:first-child');
                        var flag = true;
                        oldTime.each(function () {
                            if($.trim($(this).text()).substr(0,10) == date.substr(0,10)){
                                flag = false;
                                layer.msg('不可修改为相同日期的排期');
                                _this.parents('ul').next().show();
                                $('#'+curId).parents('li').remove();
                                console.log(_this.parents('ul').next());
                            }
                        });
                        if(flag == true){
                            allTime = _this.parents('ul').find('li span:first-child');
                            sortSchedule(allTime);
                        }
                        checkSchedule(_this);
                    }
                });
            });
            //删除排期  绑定数据
            $('.validAdv .Time').on('click','.delete',function () {
                var _this = $(this);
                var paiqiNum = _this.parents('ul').children('li').length;
                var curDelSchedule = $.trim(_this.parents('li').find('span:first-child').text());

                //获取排期的具体时间  赋值给tr属性
                var detailTime = [];
                _this.parents('ul').find('li').each(function (i,v) {
                    var val = $.trim($(v).find('span').text());
                    detailTime.push(val);
                })

                if(detailTime.indexOf(curDelSchedule) != -1){
                    var idx = detailTime.indexOf(curDelSchedule);
                    detailTime.splice(idx,1);
                }
                _this.parents('.tr').attr('beginSchedule',detailTime);

                var paiQiDays ;//获取排期天数
                var singlePrice;//单价
                switch(paiqiNum){
                    case 1:
                        break;
                    case 2:
                        checkSchedule(_this);
                        //在此处需要注意，应该先把第一个li的删除隐藏，再移除被删的li
                        //.tr上排期天数减1
                        _this.parents('.tr').attr('scheduleDays',_this.parents('.tr').attr('scheduleDays')-1);
                        paiQiDays = _this.parents('.tr').attr('scheduleDays');//天数
                        singlePrice = _this.parents('.tr').attr('OriginalPrice');;//单价
                        //总价

                        _this.parents('.wechat_list5').next().next().find('input').val(singlePrice*paiQiDays);
                        _this.parents('.tr').attr('AdjustPrice',singlePrice*paiQiDays);
                        DetailOfOrder.ModifyAmount();
                        newADDetails();//对象赋值
                        _this.parents('ul').find('li').find('.delete').hide();
                        _this.parents('li').remove();
                        layer.msg('删除成功');
                        _this.parents('ul').next().next().show();
                        break;
                    case 3:
                        checkSchedule(_this);
                        _this.parents('.tr').attr('scheduleDays',_this.parents('.tr').attr('scheduleDays')-1);
                        paiQiDays = _this.parents('.tr').attr('scheduleDays');//天数
                        singlePrice = _this.parents('.tr').attr('OriginalPrice');;//单价
                        //总价
                        _this.parents('.wechat_list5').next().next().find('input').val(singlePrice*paiQiDays);
                        _this.parents('.tr').attr('AdjustPrice',singlePrice*paiQiDays);
                        DetailOfOrder.ModifyAmount();
                        newADDetails();//对象赋值
                        _this.parents('ul').next().next().show();
                        _this.parents('li').remove();
                        layer.msg('删除成功');
                        break;
                }
            })
        },
        ModifyAmount : function () {
            var total = 0;
            $('.totalPrices').each(function () {
                var price = $(this).val()*1;
                total += price;
            })
            $('.totalPrice').text(formatMoney(total,2,''));

            var allPositionCount = 0;
            config.SubADInfos.forEach(function (items,i) {
                allPositionCount += items.SelfDetails.length;
            })
            $('.totalCount').text(allPositionCount);
        },
        VerificationInformation : function () {//验证input框信息和金额
            //需求名称和填写需求不能为空
            $('.NameOfDemand').on('blur',function () {
                var NameOfDemand = $(this).val();
                if(NameOfDemand == ''){
                    $(this).next('span').text('请填写需求名称');
                }else{
                    $(this).next('span').hide();
                }
            })

            $('.FileInDemand').on('blur',function () {
                var FileInDemand = $(this).val();
                if(FileInDemand == ''){
                    $(this).next('span').text('请填写需求名称');
                }else{
                    $(this).next('span').hide();
                }
            })

            //总价金额验证
            $('.totalPrices').each(function (i,v) {
                var reg = /^([1-9]\d*|0)(\.\d{1,2})?$/;//整数或者保留两位小数的正则
                $(this).on('change',function () {
                    var val = $(this).val();
                    if(!reg.test(val)){
                        layer.alert('请输入正确的金额');
                        $(this).val('');
                    }
                    $(this).parents('.tr').attr('AdjustPrice',val);
                    newADDetails();//修改完价格之后的数据
                    DetailOfOrder.ModifyAmount();//从新修改价格
                })
            })

            newADDetails();

            //点击审核通过的时候
            $('#auditPass').on('click',function () {

                var adPositionName = $('.adPositionName').text();//广告主名称
                var NameOfDemand = $('.NameOfDemand').val();//需求名称
                var FileInDemand = $('.FileInDemand').val();//填写需求
                var fileDownLoad = $('.fileDownLoad').attr('href');//附件的路径

                var obj = {
                    optType : 2,
                    ADOrderInfo : {
                        OrderID : config.ADOrderInfo.OrderID,
                        MediaType : 14001,
                        OrderName : config.ADOrderInfo.OrderName,//需求名称
                        Status : 16003,
                        BeginTime : config.ADOrderInfo.BeginTime,
                        EndTime : config.ADOrderInfo.EndTime,
                        Note : FileInDemand,//填写需求
                        UploadFileURL : fileDownLoad,
                        CustomerID : config.ADOrderInfo.CustomerID
                    },
                    ADDetails : newADDetailsArr
                }
                var url = '/api/ADOrderInfo/AddOrUpdate_ADOrderInfo?v=1_1';
                setAjax({
                    url : url,
                    type : 'post',
                    data : obj
                },function (data) {
                    if(data.Status == 0){
                        alert(data.Message);
                        DetailOfOrder.PassOreject();
                        window.location = '/ordermanager/listofproject.html';
                    }else{
                        alert(data.Message);
                    }
                })
            })
            //审核不通过的时候auditReject
            $('#auditReject').on('click',function (event) {
                event.preventDefault();
                $.openPopupLayer({
                    name: "popLayerDemo",
                    url: "./AuditRejected.html",
                    success: function () {
                        var resultReson = $('#RejectedMsg').val();
                        var url = '/api/ADOrderInfo/Review_ADOrderInfo';
                        $("#RejectedMsg").on('change',function () {
                            resultReson = $('#RejectedMsg').val();
                        })
                        $('.sureReject').on('click',function () {
                            if(resultReson == ''){
                                alert('请填写驳回原因');
                            }else{
                                setAjax({
                                    url : url,
                                    type : 'get',
                                    data : {
                                        orderid : config.ADOrderInfo.OrderID,
                                        optType : 27002,
                                        rejectMsg : resultReson
                                    }
                                },function (data) {
                                    if(data.Status == 0){
                                        $.closePopupLayer('popLayerDemo');
                                        window.location = '/ordermanager/listofproject.html';
                                    }else{
                                        alert(data.Message);
                                    }
                                })
                            }
                        })
                        $('.cancleReject').on('click',function () {
                            $.closePopupLayer('popLayerDemo');
                        })
                        $('#closebt').on('click',function () {
                            $.closePopupLayer('popLayerDemo');
                        })
                    }
                })
            })
        },
        PassOreject : function () {//点击审核通过的时候  还得掉审核通过的接口告诉接口已经审核通过
            var url = '/api/ADOrderInfo/Review_ADOrderInfo';
            setAjax({
                url : url,
                type : 'get',
                data : {
                    orderid : config.ADOrderInfo.OrderID,
                    optType : 27001,
                    rejectMsg : ''
                }
            },function (data) {
                if(data.Status == 0){
                    alert(data.Message);
                    window.location = '/ordermanager/listofproject.html';
                }
            })
        },
        shoppingCar : function () {//继续添加购物车
            var OrderID = config.ADOrderInfo.OrderID;
            $('#goShopCar').on('click',function () {
                $(this).attr('href', '/OrderManager/wx_list.html?isAdd=1&orderID=' + OrderID);
            })
        },
        removePosition : function () {//删除广告位
            $('.removePosition').each(function () {
                $(this).on('click',function () {
                    var that = $(this);
                    var len = $('.wx_form').find('.tr').length;
                    if(len <= 1){
                        layer.alert('至少要保留一个广告位');
                    }else{
                        layer.confirm('确认要删除数据吗', {
                            btn: ['确认','取消'] //按钮
                        }, function(){
                            that.parents('.validAdv').remove();
                            newADDetails ();//删除之后从新   传数据
                            DetailOfOrder.ModifyAmount();//从新修改价格
                            layer.closeAll();
                        })
                    }
                })
            })
        }
    };

    DetailOfOrder.init();


    /* 将当前时间格式变为2017-04-21*/
    Date.prototype.format = function(fmt) {
        var o = {
            "M+" : this.getMonth()+1,                 //月份
            "d+" : this.getDate(),                    //日
            "h+" : this.getHours(),                   //小时
            "m+" : this.getMinutes(),                 //分
            "s+" : this.getSeconds(),                 //秒
            "q+" : Math.floor((this.getMonth()+3)/3), //季度
            "S"  : this.getMilliseconds()             //毫秒
        };
        if(/(y+)/.test(fmt)) {
            fmt=fmt.replace(RegExp.$1, (this.getFullYear()+"").substr(4 - RegExp.$1.length));
        }
        for(var k in o) {
            if(new RegExp("("+ k +")").test(fmt)){
                fmt = fmt.replace(RegExp.$1, (RegExp.$1.length==1) ? (o[k]) : (("00"+ o[k]).substr((""+ o[k]).length)));
            }
        }
        return fmt;
    };

    /*
     * 添加或修改后对所有排期进行排序显示
     * @param allTime 修改排期所属广告位的所有已选排期
     * */
    function sortSchedule(allTime) {
        //对象存储排期和对应ID
        var obj={};
        //存储排期
        var arr = [];
        allTime.each(function (i) {
            obj[$.trim($(this).html())]=$(this).attr('id')
            arr.push($.trim($(this).html()));
        })
        arr.sort();
        allTime.each(function (i) {
            $(this).html(arr[i]);
            $(this).attr('id',obj[arr[i]])
        });
    };


    /*验证广告位的排期是否都大于今天(传当前对象验证当前行，不传值验证所有的排期)*/
    function checkSchedule(event){
        var curTime = new Date().format('yyyy-MM-dd');
        if(!event){
            $('.tr[isselected=1] .Time').find('span:first-child').each(function(){
                flag = true;
                if($.trim($(this).text()).substr(0,10) < curTime ){
                    flag = false;
                    $(this).css('color','red').parents('.tr').find('.message').show();
                }
            });
        }else{
            event.parents('.tr').find('span:first-child').each(function(){
                flag = true;
                if($.trim($(this).text()).substr(0,10) < curTime ){
                    flag = false;
                    $(this).css('color','red').parents('.tr').find('.message').show();
                }else{
                    if($.trim($(this).text()).length>10){
                        $(this).css('color','#666');
                    }
                }
            });
            var redColor = 0;
            event.parents('.tr').find('span:first-child').each(function(){
                if($(this).css('color') == 'rgb(255, 0, 0)'){
                    redColor++;
                }
            });
            //由于是先删除，所以找不到对应的父亲和提示信息，需要判断是否是删除按钮进入的此操作，若是，redColor为1则排期正确
            if(event.attr('class') == 'delete'){
                if(redColor == 1){
                    event.parents('.tr').find('.message').hide();
                }
            }else{
                if(redColor == 0){
                    event.parents('.tr').find('.message').hide();
                }
            }
        }
    };


    //传入后台的数据需要不断的更新   在添加删除排期的时候更新  在价格更改的时候更新
    var newADDetailsArr = [];

    function newADDetails () {//默认的数组数据
        var ADDetails = [];
        $('.validAdv').each(function (i,v) {

            //获取广告位所属媒体或刊例是否已经删除  若为-1则不能传给后台
            var expired = $(v).attr('expired');

            var obj = {};
            obj.MediaID = $(v).attr('MediaID')*1;
            obj.PubDetailID = $(v).attr('PubDetailID')*1;
            obj.AdjustPrice = $(v).attr('AdjustPrice')*1;
            obj.AdjustDiscount = $(v).attr('AdjustDiscount');
            obj.ADLaunchDays = $(v).attr('scheduleDays')*1;

            var scheduleDate = [];

            var beginschedule = $(v).attr('beginschedule').split(',');

            var beginscheduleLen = beginschedule.length;

            for(var i = 0;i<beginscheduleLen;i++){
                scheduleDate[i] = {};
                scheduleDate[i].BeginData = beginschedule[i];
                scheduleDate[i].EndData = beginschedule[i];
            }

            obj.ADScheduleInfos = scheduleDate;

            if(expired == -1){
                obj = null;//将对象置为空
            }
            ADDetails.push(obj);
        })

        newADDetailsArr = ADDetails;

        //检测数组中的空元素  直接从数组中删除掉
        for(var i = 0; i < newADDetailsArr.length; i++) {
            if(newADDetailsArr[i] == null) {
                newADDetailsArr.splice(i,1);
                i = i - 1;// i - 1 ,因为空元素在数组下标 2 位置，删除空之后，后面的元素要向前补位，
            }
        }
        //console.log(newADDetailsArr);
    }

    //获取url 地址参数方法
    function GetQueryString(name) {
        var reg = new RegExp("(^|&)"+ name +"=([^&]*)(&|$)");
        var r = window.location.search.substr(1).match(reg);
        if(r!=null)return r[2]; return null;
    }

})


