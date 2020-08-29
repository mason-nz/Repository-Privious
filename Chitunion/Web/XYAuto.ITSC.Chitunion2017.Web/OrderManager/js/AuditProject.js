/**
 * Created by fengb on 2017/4/28.
 */

$(function () {


    var orderID = GetQueryString('orderID')!=null&&GetQueryString('orderID')!='undefined'?GetQueryString('orderID'):null;
    var config = {};
    var newADDetailsArr = [];

    var DetailOfOrder = {
        constructor : DetailOfOrder,
        init : function () {
            //根据项目号查看项目的接口  取信息
            var url = 'http://www.chitunion.com/api/ADOrderInfo/GetByOrderID_ADOrderInfo?v=1_1';
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
                //console.log(config);
                DetailOfOrder.showTemplate(config,'#AuditProjecrForWeChat');

                //区别父元素的 类型  微信  CPD CPM
                $('.usefulTr').each(function (i,v) {
                    var MediaType =  $(v).attr('MediaType')*1;
                    var CPDCPM = $(v).attr('CPDCPM')*1;
                    var PubDetailID = $(v).attr('PubDetailID')*1;

                    if(MediaType == 14001){//微信
                        $(v).parents('.to_b_list').addClass('we_order');
                    }else if(MediaType == 14002){
                        $(v).parents('.to_b_list').addClass('app_order');
                        $(v).parents('.to_b_list').attr('PubDetailID',PubDetailID);
                    } 
                })

                DetailOfOrder.initMoney();//初始化金钱
                DetailOfOrder.ModifyAmount();
                DetailOfOrder.returnStateText(config.ADOrderInfo.Status);//状态码
                DetailOfOrder.theScheduleWeChat();//微信排期
                DetailOfOrder.theScheduleApp();//app排期
                DetailOfOrder.VerificationInformation();//验证价格和金钱
                DetailOfOrder.shoppingCar();//继续选择广告位
                DetailOfOrder.removePosition();//删除广告位
                DetailOfOrder.uploadFileAll();//上传
            })
        },
        uploadFileAll : function () {
            for(var i=0;i<=config.MediaOrderInfos.length-1;i++){
                uploadFile('fileUpload'+i);
            }
        },
        showTemplate : function (data,id) {
            var _this = this;
            //->首先把页面中模板的innerHTML获取到
            var str=$(id).html();
            //->然后把str和data交给EJS解析处理，得到我们最终想要的字符串
            var result = ejs.render(str, {
                data: data
            });
            //->最后把获取的HTML放入到MENU
            $(".install_box").html(result);

            //订单状态
            $('.subOrderStatus').each(function () {
                var subStatus = $(this).attr('status')*1;
                DetailOfOrder.returnStateText(subStatus);//状态码
            })

            lookDetailHolidays();//鼠标悬浮效果 并加上已经失效的或者下架的链接和提示
        },
        returnStateText : function(status) {
            switch (status) {
                case 16001:
                    $('.orderStatus').text('草稿');
                    $('.subOrderStatus').text('草稿');
                    break;
                case 16002:
                    $('.orderStatus').text('待审核');
                    $('.subOrderStatus').text('待审核');
                    break;
                case 16003:
                    $('.orderStatus').text('待执行');
                    $('.subOrderStatus').text('待执行');
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
        theScheduleWeChat : function () {//微信排期操作  
            //修改
            $('.validAdv .Time').on('click','.modify',function(){
                var d = [];
                var _this = $(this);
                var voidDateRangeArr = [];//无效数组
                var beginTime = _this.parents('ul').attr('BeginTime');//获取刊例执行周期
                var endTime = _this.parents('ul').attr('EndTime');
                var curId = $(this).prev().attr('id');

                _this.parents('ul').find('.Time span:first-child').each(function () {
                    d.push($.trim($(this).html().substr(0,11)));
                })

                for(var i = 0;i <= d.length - 1;i++ ){
                    var obj = {'S':$.trim(d[i]),'E':$.trim(d[i])};
                    voidDateRangeArr.push(obj);
                }

                laydate({
                    elem: '#'+curId,
                    format: 'YYYY-MM-DD hh:mm',
                    fixed: false,
                    istoday:false,
                    istime: true,
                    isclear: false,
                    voidDateRange:voidDateRangeArr,//无效日期s
                    isInitCheck : false,//解决初始化日期消失的问题
                    min: getAvailableDate(beginTime,endTime)[0], //执行周期开始日期
                    max: getAvailableDate(beginTime,endTime)[1], //执行周期结束日期
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
                            }
                        });
                        if(flag == true){
                            allTime = _this.parents('ul').find('li span:first-child');
                            sortSchedule(allTime);
                        }
                        checkSchedule(_this);
                        newADDetails();//对象赋值
                    }
                });
            });

            //微信赤兔自营可以选择渠道
            $('.choose_channel').off('click').on('click',function () {
                var that = $(this);
                var _width = document.documentElement.clientWidth;
                var _height = document.documentElement.clientHeight;

                var AdPosition1 = that.parents('.tr').attr('ADPositionID')*1;
                var AdPosition2 = 7002;
                var AdPosition3 = that.parents('.tr').attr('CreateTypeID')*1;
                var ChannelID = that.parents('.tr').attr('ChannelID')*1;
                var MediaID = that.parents('.tr').attr('MediaID')*1;
                var CooperateDate = that.parents('.tr').find('input').eq(0).val(); 

                var url = 'http://www.chitunion.com/api/ADOrderInfo/GetChannelList?v=1_1';
                setAjax({
                    url : url,
                    type : 'get',
                    data : {
                        /*ChannelId : 6,
                        MediaId : 16717,*/
                        /*ChannelId : ChannelID,*/
                        MediaId : MediaID,
                        AdPosition1 : AdPosition1,
                        AdPosition2 : AdPosition2,
                        AdPosition3 : AdPosition3,
                        CooperateDate : CooperateDate
                    }
                },function(data){
                    var Result = data.Result;
                    if(data.Status == 0){
                        $('.ChooseChannel').html(ejs.render($('#choose-channel').html(),Result));

                        $('.channel_box').show();
                        $('.channel_box .layer').show();
                        var layer_height = $('.channel_box .layer').height();
                        var _left = (_width-550)/2;
                        var _top = (_height-layer_height)/2;
                        $('.channel_box').css({'width':_width,'height':_height,'position':'fixed','left':0,'top':0,'background':'rgba(0,0,0,0.7)'});
                        $('.channel_box .layer').css({'position':'absolute','left':_left,'top':_top});

                        //关闭弹层
                        $('#closebt1').on('click',function () {
                            $('.channel_box').hide();
                            $('.channel_box .layer').hide();
                        })

                        //成本参考价默认为文本 鼠标点击还可以输入修改价格
                        $('.channel_box .tdcosting_price').on('click',function () {
                            var that = $(this);
                            that.find('.costing_price').removeAttr('disabled');
                        })
                        $('.channel_box .tdcosting_price').on('blur',function () {
                            var that = $(this);
                            that.find('.costing_price').prop('disabled',true);
                        })
                        //成本参考价只能输入数字
                        $('.channel_box .costing_price').on("input",function(){
                            replaceAndSetPos(this,/[^0-9]/g,'');
                            var val = $.trim($(this).val());
                            $(this).parents('tr').attr('CostReferencePrice',val);
                        }).on('blur',function () {
                            var val = $.trim($(this).val());
                            if(val == ''){
                                layer.msg('请输入成本参考价',{'time':1000});
                            }
                        })
                        //选择按钮
                        $('.channel_box .choose_btn').on('click',function () {
                            var ChannelName = $(this).parents('tr').attr('ChannelName');
                            var CostReferencePrice = $(this).parents('tr').attr('CostReferencePrice');
                            var ChannelID = $(this).parents('tr').attr('ChannelID');

                            if(CostReferencePrice == ''){
                                layer.msg('请输入成本参考价',{'time':1000});
                            }else{
                                var curTr = that.parents('.tr');
                                var curTd = curTr.find('td').eq(5);

                                $('.channel_box').hide();
                                $('.channel_box .layer').hide();
                                curTd.find('p').eq(0).text(CostReferencePrice);
                                curTd.find('p').eq(1).text(ChannelName);
                                //同时应该将每一行绑定的数据也修改 成本参考价  渠道ID
                                curTr.attr('CostReferencePrice',CostReferencePrice);
                                curTr.attr('ChannelID',ChannelID);
                                DetailOfOrder.ModifyAmount();//金钱重新计算
                                newADDetails();
                            }
                        })

                        //查看渠道
                        $('.look_channel').on('click',function () {
                            var ChannelID = $(this).parents('tr').attr('ChannelID');
                            window.open('/ChannelManager/look_channel.html?ChannelID='+ChannelID);
                        })
                    }else{
                        layer.msg(data.message,{'time':'1000'});
                    }
                })
            })
            
        },
        theScheduleApp : function(){//app排期
            //修改
            $('.each_app .appTime').on('click','.app_modify',function(e){
                e.stopPropagation();
                var that = $(this);
                var curId = [];
                var idx = that.parents('.appTime').index();
                var beginTime = that.parent('p').attr('BeginTime');//获取刊例执行周期
                var endTime = that.parent('p').attr('EndTime');

                that.parent('p').find('input').each(function(){
                   curId.push($(this).attr('id'));
                });
                that.parent('p').find('input').css('border','1px solid #ccc');
                that.parent('p').find('input').removeAttr('disabled');

                chooseDetailDate(curId[0],curId[1],that,idx);

                //开始日期   结束日期
                function chooseDetailDate(element1,element2,del,idx) {
                    var ele1 = '#'+element1;
                    var ele2 = '#'+element2;
                    var idx = idx;
                    var beginDate = $(ele1).val();
                    var endDate = $(ele2).val();

                    $(ele1).off('click').on('click',function (e) {
                        e.stopPropagation();
                        var that = $(this);
                        var CPDORCPM = $(ele1).parents('.each_app').attr('CPDCPM')*1;

                        //要判断的是   在广告位ID一样的情况下  但是订单ID不一样的情况下才判断
                        
                        //获取开始或结束日期，置为不可选日期  //已选日期数组   获取其他开始和结束日期，置为不可选日期
                        var voidDateRange = [];
                        var selectedPoint = [];

                        var cur_PubDetailID = $(ele1).parents('.app_order').attr('PubDetailID');
                        var cur_SubOrderID = $(ele1).parents('.app_order').attr('SubOrderID');

                        /*$(ele1).parents('.cpd_order').siblings('.cpd_order').each(function(){
                            var timeObj = {
                                'S': $.trim($(this).find('input').eq(0).val()),
                                'E': $.trim($(this).find('input').eq(1).val())
                            }
                            voidDateRange.push(timeObj);
                        });
                        $(ele1).parents('.cpd_order').siblings('.cpd_order').each(function(){
                            //获取此排期的开始日期和结束日期之间的日期
                            var curBEtime = getall($.trim($(this).find('input').eq(0).val()).substr(0,10),$.trim($(this).find('input').eq(1).val()).substr(0,10));
                            selectedPoint = selectedPoint.concat(curBEtime);
                        });*/

                        //标黄
                        $(ele1).parents('.install_box').find('.app_order').each(function(i,v){
                            var PubDetailID = $(v).attr('PubDetailID');
                            var SubOrderID = $(v).attr('SubOrderID');

                            var cur = $(this).find('.appTime');
                            if(cur_PubDetailID == PubDetailID && cur_SubOrderID != SubOrderID){
                                var curBEtime = getall($.trim(cur.find('input').eq(0).val()).substr(0,10),$.trim(cur.find('input').eq(1).val()).substr(0,10));
                                selectedPoint = selectedPoint.concat(curBEtime);
                            }
                        });

                        //置灰
                        $(ele1).parents('.app_order').siblings('.app_order').each(function(i,v){
                            var PubDetailID = $(v).attr('PubDetailID');
                            var SubOrderID = $(v).attr('SubOrderID');

                            var cur = $(this).find('.appTime');
                            if(cur_PubDetailID == PubDetailID && cur_SubOrderID != SubOrderID){
                                var timeObj = {
                                    'S': $.trim(cur.find('input').eq(0).val()),
                                    'E': $.trim(cur.find('input').eq(1).val())
                                }
                                voidDateRange.push(timeObj);
                            }
                        });
                        
                        var Cur_EndTime = that.parents('.appTime').find('input:eq(1)').val();
                        var Cur_BeginTime = that.parents('.appTime').find('input:eq(0)').val();
                        laydate({
                            elem: ele1,
                            format: 'YYYY-MM-DD',
                            fixed: false,
                            istoday:false,
                            istime: true,
                            isclear: false,
                            isInitCheck : false,//解决初始化日期消失的问题
                            isShowHoliday: true,//是否显示节假日
                            voidDateRange : voidDateRange,
                            selectedPoint: selectedPoint,
                            min: getAvailableDate(beginTime,endTime)[0], //执行周期开始日期
                            max: getAvailableDate(beginTime,endTime)[1], //执行周期结束日期
                            choose: function (date) {

                                for(var i=0;i<voidDateRange.length;i++){
                                    if(date<voidDateRange[i].S && Cur_EndTime>voidDateRange[i].E){
                                        layer.msg('同一时间段的排期不可重复选择');
                                        //修改开始日期
                                        $(ele1).val(Cur_BeginTime);
                                        return false;
                                    }
                                }

                                if(date > Cur_EndTime){
                                    layer.msg('起始时间不能大于结束时间',{'time':1000});
                                    $(ele1).val(Cur_BeginTime);
                                    $(ele1).css('border','1px solid #ccc');
                                    $(ele1).removeAttr('disabled');
                                }else{
                                    beginDate = date;
                                    sendHolidayDate(beginDate,endDate,ele1,idx);
                                }

                            }
                        })
                    })
                    $(ele2).off('click').on('click',function (e) {
                        e.stopPropagation();
                        var that = $(this);
                        var CPDORCPM = $(ele1).parents('.each_app').attr('CPDCPM')*1;

                        /*if(CPDORCPM == 11001){
                            //获取开始或结束日期，置为不可选日期
                            var voidDateRange = [];
                            //已选日期数组 //获取其他开始和结束日期，置为不可选日期
                            var selectedPoint = [];
                            $(ele2).parents('.cpd_order').siblings('.cpd_order').each(function(){
                                var timeObj = {
                                    'S': $.trim($(this).find('input').eq(0).val()),
                                    'E': $.trim($(this).find('input').eq(1).val())
                                }
                                voidDateRange.push(timeObj);
                            });
                            $(ele2).parents('.cpd_order').siblings('.cpd_order').each(function(){
                                //获取此排期的开始日期和结束日期之间的日期
                                var curBEtime = getall($.trim($(this).find('input').eq(0).val()).substr(0,10),$.trim($(this).find('input').eq(1).val()).substr(0,10));
                                selectedPoint = selectedPoint.concat(curBEtime);
                            });
                        }else if(CPDORCPM == 11002){
                            //获取开始或结束日期，置为不可选日期
                            var voidDateRange = [];
                            //已选日期数组 //获取其他开始和结束日期，置为不可选日期
                            var selectedPoint = [];

                            $(ele2).parents('.cpm_order').siblings('.cpm_order').each(function(){
                                var cur = $(this).find('.appTime');
                                var timeObj = {
                                    'S': $.trim(cur.find('input').eq(0).val()),
                                    'E': $.trim(cur.find('input').eq(1).val())
                                }
                                voidDateRange.push(timeObj);
                            })
                            $(ele2).parents('.cpm_order').siblings('.cpm_order').each(function(){
                                var cur = $(this).find('.appTime');
                                //获取此排期的开始日期和结束日期之间的日期
                                var curBEtime = getall($.trim(cur.find('input').eq(0).val()).substr(0,10),$.trim(cur.find('input').eq(1).val()).substr(0,10));
                                selectedPoint = selectedPoint.concat(curBEtime);
                            });

                        }*/

                        //获取开始或结束日期，置为不可选日期  //已选日期数组   获取其他开始和结束日期，置为不可选日期
                        var voidDateRange = [];
                        var selectedPoint = [];

                        var cur_PubDetailID = $(ele2).parents('.app_order').attr('PubDetailID');
                        var cur_SubOrderID = $(ele2).parents('.app_order').attr('SubOrderID');


                        //标黄
                        $(ele2).parents('.install_box').find('.app_order').each(function(i,v){
                            var PubDetailID = $(v).attr('PubDetailID');
                            var SubOrderID = $(v).attr('SubOrderID');

                            var cur = $(this).find('.appTime');
                            if(cur_PubDetailID == PubDetailID && cur_SubOrderID != SubOrderID){
                                var curBEtime = getall($.trim(cur.find('input').eq(0).val()).substr(0,10),$.trim(cur.find('input').eq(1).val()).substr(0,10));
                                selectedPoint = selectedPoint.concat(curBEtime);
                            }
                        });

                        //置灰
                        $(ele2).parents('.app_order').siblings('.app_order').each(function(i,v){
                            var PubDetailID = $(v).attr('PubDetailID');
                            var SubOrderID = $(v).attr('SubOrderID');

                            var cur = $(this).find('.appTime');
                            if(cur_PubDetailID == PubDetailID && cur_SubOrderID != SubOrderID){
                                var timeObj = {
                                    'S': $.trim(cur.find('input').eq(0).val()),
                                    'E': $.trim(cur.find('input').eq(1).val())
                                }
                                voidDateRange.push(timeObj);
                            }
                        });
                        
                        var Cur_BeginTime = that.parents('.appTime').find('input:eq(0)').val();
                        var Cur_EndTime = that.parents('.appTime').find('input:eq(1)').val();
                        laydate({
                            elem: ele2,
                            format: 'YYYY-MM-DD',
                            fixed: false,
                            istoday:false,
                            istime: true,
                            isclear: false,
                            isInitCheck : false,//解决初始化日期消失的问题,
                            isShowHoliday: true,//是否显示节假日
                            voidDateRange : voidDateRange,
                            selectedPoint: selectedPoint,
                            min: getAvailableDate(beginTime,endTime)[0], //执行周期开始日期
                            max: getAvailableDate(beginTime,endTime)[1], //执行周期结束日期
                            choose: function (date) {

                                for(var i=0;i<voidDateRange.length;i++){
                                    if(Cur_BeginTime<voidDateRange[i].S && date>voidDateRange[i].E){
                                        layer.msg('同一时间段的排期不可重复选择');
                                        //修改结束日期
                                        $(ele2).val(Cur_EndTime);
                                        return false;
                                    }
                                }

                                if(date < Cur_BeginTime){
                                    layer.msg('结束时间不能小于起始时间',{'time':1000});
                                    $(ele2).val(Cur_EndTime);
                                    $(ele2).css('border','1px solid #ccc');
                                    $(ele2).removeAttr('disabled');
                                }else{
                                    endDate = date;
                                    sendHolidayDate(beginDate,endDate,ele2,idx);
                                }
                            }
                        })
                    })
                    $(document.body).not(del).on('click',function (e) {
                        e.stopPropagation();
                        $(ele1).css('border','none');
                        $(ele1).attr('disabled',true);
                        $(ele2).css('border','none');
                        $(ele2).attr('disabled',true);
                    })
                }
            });

            //修改完日期以后从新更新节假日信息
            function sendHolidayDate(beginDate,endDate,ele,idx) {

                var idx = idx;//索引
                var AllDays = getall(beginDate,endDate).length;//改变后的总时间
                var prev = $(ele).parents('.appTime').parent().prev().find('.day_bottom').eq(idx);//当前元素对应的之前的tr的索引
                prev.find('span').eq(0).text(AllDays);//页面的总时间改变

                var url = '/api/ShoppingCart/QueryHolidays?v=1_1';
                setAjax({
                    url : url,
                    type : 'get',
                    data : {
                        'beginDate' : beginDate,
                        'endDate' : endDate
                    }
                },function (data) {
                    var Result = data.Result;
                    var renderHoliday = Result.Holiday;

                    var holidayAllDays = Result.Days;//获取到的假期天数
                    var workingdays = AllDays-holidayAllDays;//工作日的天数
                    var originalprice = $(ele).parents('.cpdApp').attr('OriginalPrice')*1;
                    var priceholiday = $(ele).parents('.cpdApp').attr('PriceHoliday')*1;
                    var HasHoliday = $(ele).parents('.cpdApp').attr('HasHoliday')*1;//是否应该节假日的价格

                    prev.attr('alldays',AllDays);
                    prev.attr('holidays',holidayAllDays);
                    prev.attr('workingdays',workingdays);
                    
                    if(HasHoliday == 1){
                        var total = workingdays*originalprice + holidayAllDays*priceholiday;
                        prev.attr('adjustprice',total);
                        $(ele).parents('.appTime').parent().next().find('span').eq(1).show();
                    }else{
                        var total = AllDays*originalprice;
                        prev.attr('adjustprice',total);
                    }

                    var adjustprice = 0;
                    var ADLaunchDays = 0;
                    var Holidays = 0;

                    $(ele).parents('.appTime').parent().prev().find('.day_bottom').each(function(){
                        adjustprice += $(this).attr('adjustprice')*1;
                        ADLaunchDays += $(this).attr('alldays')*1;
                        Holidays += $(this).attr('holidays')*1;
                    })
                    //console.log(adjustprice);

                    //总价钱发生相应的变化  并且对应绑的总价的属性也会发生变化
                    $(ele).parents('.cpdApp').find('.totalPrices').val(adjustprice);
                    $(ele).parents('.cpdApp').find('.totalPrice').attr('totalprice',adjustprice);
                    $(ele).parents('.cpdApp').attr('AdjustPrice',adjustprice);

                    //每一行的发生变化
                    $(ele).parents('.cpdApp').attr('ADLaunchDays',ADLaunchDays);
                    $(ele).parents('.cpdApp').attr('Holidays',Holidays);


                    DetailOfOrder.ModifyAmount();//总价钱也发生相应的变化
                    newADDetails();

                    if(holidayAllDays > 0){//有节假日的情况

                        prev.find('span').eq(1).show();//把节假日的天数显示出来
                        //prev.find('span').eq(1).find('label').text();
                        $(ele).parent('p').next().show();//把查看节假日显示

                        if(HasHoliday == 1){
                            $(ele).parents('td').next().find('span').eq(1).show();//节假日的价格显示出来
                        }

                        var str = '';
                        for(var i=0;i<renderHoliday.length;i++){
                            str += '<p class="everyHoliday"><span>'+renderHoliday[i].BeginData.substr(0,10)+'</span>至<span>'+renderHoliday[i].EndData.substr(0,10)+'</span><span class="holiday_icon">'+renderHoliday[i].Name+'</span></p>';
                        }
                        $(ele).parents('.appTime').find('.SuspensionElements').html(str);
                        prev.find('label').text(holidayAllDays);//假日的天数统计

                    }else{//没有节假日

                        prev.find('span').eq(1).hide();//把节假日的天数隐藏
                        $(ele).parent('p').next().hide();//把查看节假日隐藏掉
                        //$(ele).parents('td').next().find('span').eq(1).hide();//节假日的价格
                    }
                })
            }

            
            //CPM修改排期次数 每一行的金额和最下面的总额会发生变化
            $('.each_app .qianCount').each(function () {
                var that = $(this);
                var originalprice = that.parents('.each_app').attr('originalprice')*1;
                that.on("input",function(){
                    replaceAndSetPos(this,/[^0-9]/g,'');
                })
                //验证投放次数的输入正确性
                var reg = /^\+?[1-9]\d*$/;
                that.on("input",function(){
                    var val = $(this).val();
                    if(reg.test(val)&&val<366){
                        console.log('正常');
                    }else{
                        layer.msg("只能输入0-365的正整数",{'time':1000});
                        $(this).val("");
                    }
                })
                that.on('change',function () {
                    var val = $(this).val();
                    that.parents('.each_app').find('.totalPrice').attr('totalprice',val*originalprice);
                    that.parents('.each_app').find('.totalPrices').val(val*originalprice);
                    that.parents('.each_app').attr('ADLaunchDays',val);
                    that.parents('.each_app').attr('AdjustPrice',val*originalprice);
                    DetailOfOrder.ModifyAmount();
                })
            });
        },
        initMoney : function(){//计算appCPD的 初始化总额
            config.SubADInfos.forEach(function (items,j){
                if(items.APPDetails != null){
                    var curTable = $('.to_b_list').eq(j);
                    items.APPDetails.forEach(function (item,i) {
                        if(item.CPDCPM == 11001){//CPD
                            var ADLaunchDays = 0;
                            var Holidays = 0;

                            var idx = i;
                            var cur = curTable.find('.each_app').eq(idx);
                            var OriginalPrice = cur.attr('OriginalPrice')*1;
                            var priceHoliday = cur.attr('PriceHoliday')*1;
                            var HasHoliday = cur.attr('HasHoliday')*1;//是否应该节假日的价格

                            item.ADSchedules.forEach(function (each,e) {
                                var allDays = each.AllDays;
                                var Holidays = each.Holidays;
                                var workingDays = allDays-Holidays;

                                ADLaunchDays += each.AllDays;
                                Holidays += each.Holidays;
                                
                                if(HasHoliday == 1){
                                    var total = workingDays*OriginalPrice + Holidays*priceHoliday;
                                    cur.find('.day_bottom').eq(e).attr('adjustprice',total);
                                }else{
                                    var total = allDays*OriginalPrice;
                                    cur.find('.day_bottom').eq(e).attr('adjustprice',total);
                                }
                            })
                            cur.attr('ADLaunchDays',ADLaunchDays);//总时间
                            cur.attr('Holidays',Holidays);//假期时间

                            /*//赋给初始化变量  到每一行里面
                            cur.attr('allDays',allDays);//总时间
                            cur.attr('Holidays',Holidays);//假期时间
                            cur.attr('workingDays',workingDays);//工作日时间
                            //每一行的总价  然后赋值给tr
                            var total = Holidays*priceHoliday + workingDays*originalprice;

                            cur.find('.totalPrices').val(total);
                            cur.find('.totalPrice').attr('totalPrice',total);
                            cur.attr('AdjustPrice',total);*/
                            DetailOfOrder.ModifyAmount();
                        }
                    })
                }
            })
        },
        ModifyAmount : function () {//计算广告位个数和金额

            var AllMoney = 0;
            var allPositionCount = 0;
            var SelfDetailsLen = 0;
            var APPDetailsLen = 0;
            var TotalOriginalPrice = 0;//销售参考价
            var TotalCostReferencePrice = 0;//成本参考价

            //总价
            $('.totalPrices').each(function () {
                var val = $(this).parent().attr('totalPrice')*1;
                AllMoney += val;
            })
            $('.allMoney').text(formatMoney(AllMoney,2));
            //广告位个数
            allPositionCount = $('.tr').length;
            $('.allPositionCount').text(allPositionCount);


            //销售参考价
            $('.each_OriginalPrice').each(function(){
                var val = $(this).attr('OriginalPrice');
                TotalOriginalPrice += val;
            })
            //成本参考价
            $('.each_CostReferencePrice').each(function(){
                var val = $(this).attr('CostReferencePrice');
                TotalCostReferencePrice += val;
            })
            $('.SalePrice').text(formatMoney(TotalOriginalPrice,2));
            $('.CostReferencePrice').text(formatMoney(TotalCostReferencePrice,2));

        },
        VerificationInformation : function () {
            //需求名称
            $('.NameOfDemand').on('blur',function () {
                var NameOfDemand = $(this).val();
                $(this).parent().attr('orderName',NameOfDemand);
                if(NameOfDemand == ''){
                    $(this).next('span').show();
                }else{
                    $(this).next('span').hide();
                }
            })
            //需求说明
            $('.FileInDemand').on('blur',function () {
                var FileInDemand = $(this).val();
                $(this).parents('.satements').attr('Note',FileInDemand);
                if(FileInDemand == ''){
                    $(this).next('span').show();
                }else{
                    $(this).next('span').hide();
                }
            })

            //每一行的成交价手动改变的时候   都要从新算一下总金额  而且把改完的价钱放在每一行的属性AdjustPrice
            $('.usefulTr .totalPrices').each(function (i,v) {
                var reg = /^([1-9]\d*|0)(\.\d{1,2})?$/;//整数或者保留两位小数的正则
                var that = $(this);
                that.on('change',function () {
                    var val = $(this).val();
                    if(!reg.test(val)){
                        layer.alert('请输入正确的金额');
                        $(this).val('');
                    }
                    that.parent('p').attr('totalprice',val);
                    that.parents('.usefulTr').attr('AdjustPrice',val);
                    DetailOfOrder.ModifyAmount();//从新修改价格
                    newADDetails();
                })
            })

            //点击提交的时候
            $('#lastResult').on('click',function (event) {
                event.preventDefault();
                newADDetails();


                var FileInDemandArr = [];//需求说明
                var fileUrlArr = [];//上传文件

                //需求说明
                $('.FileInDemand').each(function(){
                    var val = $(this).val();
                    FileInDemandArr.push(val);
                })
                for(var i=0;i<FileInDemandArr.length;i++){
                    if(FileInDemandArr[i] == ""){
                        FileInDemandArr.splice(i,1);
                    }
                }
                //上传文件
                $('.uploadFile').each(function(){
                    var val = $(this).attr('href');
                    fileUrlArr.push(val);
                })
                for(var i=0;i<fileUrlArr.length;i++){
                    if(fileUrlArr[i] == ""){
                        fileUrlArr.splice(i,1);
                    }
                }

                var NameOfDemand = $('.AlOrderName').attr('orderName');//需求名称
                //获取 MediaOrderInfos对象
                var MediaOrderInfos = [];
                $('.demand .satements').each(function () {
                    var that = $(this);
                    var MediaType = that.attr('MediaType');
                    var Note = that.attr('Note');
                    var UploadFileURL = that.find('.uploadFile').attr('href');
                    var obj = {
                        'MediaType' : MediaType,
                        'Note' : Note,
                        'UploadFileURL' : UploadFileURL
                    }
                    MediaOrderInfos.push(obj);
                })

                //console.log(fileUrlArr)

                if(FileInDemandArr.length < config.MediaOrderInfos.length){
                    layer.msg('请补充需求',{'time':1000});
                }/*else if(fileUrlArr.length < config.MediaOrderInfos.length){
                    layer.msg('请补充上传资料',{'time':1000});
                }*/else if(!$('#auditPass').prop('checked') && !$('#auditReject').prop('checked')){
                    layer.msg('请选择通过或者驳回',{'time':1000});
                }else if($('#auditPass').prop('checked')){//通过
                    
                    var url = '/api/ADOrderInfo/AddOrUpdate_ADOrderInfo?v=1_1';
                    setAjax({
                        url : url,
                        type : 'post',
                        data : {
                            optType : 2,
                            ADOrderInfo : {
                                OrderID : config.ADOrderInfo.OrderID,
                                OrderName : NameOfDemand,//不能直接从接口里面取出  可以修改的
                                Status : 16003,
                                CustomerID : config.ADOrderInfo.CustomerID
                            },
                            MediaOrderInfos : MediaOrderInfos,
                            ADDetails : newADDetailsArr
                        }
                    },function (data) {
                        if(data.Status == 0){
                            DetailOfOrder.PassOreject();
                            window.location = '/ordermanager/listofproject.html';
                        }else{
                            alert(data.Message);
                        }
                    })
                }else if($('#auditReject').prop('checked')) {//驳回
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
                }
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
                    window.location = '/ordermanager/listofproject.html';
                }
            })
        },
        shoppingCar : function () {//继续选择广告位 首页
            $('#goShopCar').on('click',function () {
                $(this).attr('href','/OrderManager/wx_list.html');
            })
        },
        removePosition : function () {//删除广告位
            $('.removePosition').each(function () {
                $(this).on('click',function () {
                    var that = $(this);
                    var len = that.parents('tbody').find('tr').length;
                    if(len <= 1){
                        layer.alert('至少要保留一个广告位');
                    }else{
                        layer.confirm('确认要删除数据吗', {
                            btn: ['确认','取消'] //按钮
                        }, function(){
                            that.parents('tr').remove();
                            DetailOfOrder.ModifyAmount();//从新修改价格
                            newADDetails();//删除之后从新 传数据
                            layer.closeAll();
                        })
                    }
                })
            })
        }
    };

    DetailOfOrder.init();

    //传入后台的数据
    function newADDetails () {//默认的数组数据
        var ADDetails = [];
        //ADDetails
        $('.usefulTr').each(function (i,v) {
            var obj = {};
            obj.MediaType = $(v).attr('MediaType')*1;
            obj.MediaID = $(v).attr('MediaID')*1;
            obj.PubDetailID = $(v).attr('PubDetailID')*1;
            obj.SaleAreaID = $(v).attr('SaleAreaID')*1;
            obj.AdjustPrice = $(v).attr('AdjustPrice')*1;
            obj.AdjustDiscount = 1;
            obj.PriceHoliday = $(v).attr('PriceHoliday')*1;
            obj.CostReferencePrice = $(v).attr('CostReferencePrice')*1;
            obj.ChannelID = $(v).attr('ChannelID')*1;

            if(obj.MediaType == 14001){//微信
                obj.ADLaunchDays = $(v).attr('scheduleDays')*1;
                obj.Holidays = 0;

                var ADScheduleInfos = [];//排期数组
                $(v).find('.Time').each(function () {
                    var that = $(this);
                    var tim = {
                        BeginData : that.find('span').eq(0).text(),
                        EndData : that.find('span').eq(0).text()
                    };
                    ADScheduleInfos.push(tim);
                })
                obj.ADScheduleInfos = ADScheduleInfos;
            }else{
                
                var CPDCPM = $(v).attr('CPDCPM')*1;
                if(CPDCPM == 11001){//app 总天数
                    obj.ADLaunchDays = $(v).attr('ADLaunchDays')*1;
                    obj.Holidays = $(v).attr('Holidays')*1;
                }else{//次数
                    obj.ADLaunchDays = $(v).attr('ADLaunchDays')*1;
                    obj.Holidays = 0;
                }

                var ADScheduleInfos = [];//排期数组
                $(v).find('.appTime').each(function () {
                    var that = $(this);
                    var tim = {
                        BeginData : that.find('input').eq(0).val(),
                        EndData : that.find('input').eq(1).val()
                    };
                    ADScheduleInfos.push(tim);
                })
                obj.ADScheduleInfos = ADScheduleInfos;
            }
            ADDetails.push(obj);
        })
        newADDetailsArr = ADDetails;
        //检测数组中的空元素  直接从数组中删除掉
        /*for(var i = 0; i < newADDetailsArr.length; i++) {
            if(newADDetailsArr[i] == null) {
                newADDetailsArr.splice(i,1);
                i = i - 1;// i - 1 ,因为空元素在数组下标 2 位置，删除空之后，后面的元素要向前补位，
            }
        }*/
    };

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


    /*------------------------公共的方法----------------------------*/

    // 1 上传文件
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
            'buttonText': '上传文件',
            'buttonClass': 'button_add',
            'swf': '/Js/uploadify.swf?_=' + Math.random(),
            'uploader': '/AjaxServers/UploadFile.ashx',
            'auto': true,
            'multi': false,
            'width': 80,
            'height': 18,
            /*'formData': { Action: 'BatchImport', LoginCookiesContent: escapeStr(getCookie('ct-uinfo')), IsGenSmallImage: 1 }, 存在缩略图大图的格式 */
            'formData': { Action: 'BatchImport', CarType: '', LoginCookiesContent: escapeStr(getCookie('ct-uinfo')) },
            'fileTypeDesc': '支持格式:xls,xlsx,jpg,jpeg,png.gif,zip',
            'fileTypeExts': '*.xls;*.xlsx;*.jpg;*.jpeg;*.png;*.gif;*.zip;',
            'fileSizeLimit':'5MB',
            'queueSizeLimit': 1,
            'scriptAccess': 'always',
            'onQueueComplete': function (event, data) {},
            'fileCount':1,
            queueID:'imgShow',
            'scriptAccess': 'always',
            'overrideEvents' : [ 'onDialogClose'],
            'onQueueComplete': function (event, data) {},
            'onQueueFull': function () {
                layer.alert('您最多只能上传1个文件！');
                return false;
            },
            'onUploadSuccess': function (file, data, response) {
                if (response == true) {
                    var json = $.evalJSON(data);
                    $("#"+id).parent().next().show();
                    $("#"+id).parents('.satements').find('.uploadName').text(json.FileName);
                    $("#"+id).parents('.satements').find('.uploadFile').attr("href",json.Msg);
                }
            },
            'onProgress': function (event, queueID, fileObj, data) {},
            'onUploadError': function (event, queueID, fileObj, errorObj) {},
            'onSelectError':function(file, errorCode, errorMsg){}
        });

    };


    /* 2.将当前时间格式变为2017-04-21*/
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


    /** 3
     * 刊例执行周期和明天比较比较，获取最新可用执行周期，作为laydate的可选日期
     * @param beginTime 从页面上获取的刊例执行周期开始日期
     * @param endTime   从页面上获取的刊例执行周期结束日期
     * @returns {Array} 数组[0]是执行周期开始日期， 数组[1]是执行周期结束日期
     */
    function getAvailableDate(beginTime,endTime){
        var availableBeginTime,availableEndTime,arr = [];
        var curTime = new Date();
        var tommorow = new Date((+curTime)+1*24*3600*1000).format('yyyy-MM-dd');;
        /*如果当前日期小于开始日期*/
        if(tommorow<beginTime){
            availableBeginTime = beginTime;
            availableEndTime = endTime;
        }else if(tommorow > beginTime && tommorow < endTime){
            /*当前日期在开始日期和结束日期之间*/
            availableBeginTime = tommorow;
            availableEndTime = endTime;
        }else if(tommorow >endTime){
            /*当前日期在结束日期之后——停用啦，修改添加按钮不可点击。其实不用管*/
            availableBeginTime = tommorow;
            availableEndTime = tommorow;
        }
        arr.push(availableBeginTime,availableEndTime);
        return arr;
    }

    /* 4
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


    /* 5 验证广告位的排期是否都大于今天(传当前对象验证当前行，不传值验证所有的排期)*/
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


    /* 6 获取两个时间之间的日期,调用函数getAll,注：包括开始日期和结束日期*/
    function getall(start_time,end_time) {
        var bd = new Date(start_time),be = new Date(end_time);
        var bd_time = bd.getTime(), be_time = be.getTime(),time_diff = be_time - bd_time;
        var d_arr = [];
        for(var i=0; i<= time_diff; i+=86400000){
            var ds = new Date(bd_time+i).format('yyyy-MM-dd');
            d_arr.push(ds);
        }
        return d_arr;
    }


    // 7 获取url 地址参数方法
    function GetQueryString(name) {
        var reg = new RegExp("(^|&)"+ name +"=([^&]*)(&|$)");
        var r = window.location.search.substr(1).match(reg);
        if(r!=null)return r[2]; return null;
    }


    // 8--------------只能输入数字 Start
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

})


