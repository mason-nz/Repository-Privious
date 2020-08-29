/*
* Written by:     wangcan
* function:       购物车第一步
* Created Date:   2017-04-26
* Modified Date:  2017-07-17
*/
$(function(){
    var userData = GetRequest();
    var orderID = userData.orderID || "";
    getInitalData();
    var flag = true;

    //点击“继续选择广告位”，跳转到首页
    $('#goToList').off('click').on('click',function(){
        location.href='/OrderManager/wx_list.html';
    })

    /*1、添加到项目*/
    /*超级管理员和运营、AE是一样的权限*/
    /*判断是否是AE，如果不是，移除“添加到项目”，否则
        如果是从审核页面的，url上会带着orderID,要把OrderiD显示在“添加到项目”下拉框中，
        如果不是审核页面过来的，要调接口获取对应的项目ID,显示在下拉框选项中

        再修改：如果是从审核页面过来的，url上带着orderID,此时隐藏“添加到项目”。之前的操作还保留
    */
    if(CTLogin.RoleIDs == 'SYS001RL00005' || CTLogin.RoleIDs == 'SYS001RL00001' || CTLogin.RoleIDs == 'SYS001RL00004'){
        //不管orderID是否存在，调接口获取orderID列表
        setAjax({
            url:'http://www.chitunion.com/api/ShoppingCart/OrderIDName_FuzzyQuery?v=1_1',
            type:'get',
            data:{}
        },function(data){
            if(data.Status == 0){
                var str = '<option>请选择项目ID+项目名称</option>';
                for(var i=0;i<data.Result.length;i++){
                    str += '<option>'+data.Result[i].OrderID+" "+data.Result[i].OrderName+'</option>'
                }
                $('.add_project').find('select').html(str);
                if(!orderID){
                    //如果不存在，不做任何操作
                }else{
                    //如果存在，OrderID默认选中,AE添加到项目默认选中
                    $('.add_project').find('select').val(orderID);
                    $('.add_project').find('input[type=checkbox]').prop('checked',true);
                    $('.add_project').hide();
                }
            }
        });
    }else{
        $('.add_project').remove();
    }
    operateSchedule();
    delAdv();
    lookDetail();
    /*2、排期操作*/
    /*排期操作函数——修改和删除，修改后排序,并验证日期是否正确*/
    function operateSchedule() {
        //鼠标移入修改时，更换修改排期的图片
        $('.validAdv').on('mouseover','.modify',function () {
            $(this).children().attr('src','../images/icon54_h.png')
        }).on('mouseout','.modify',function () {
            $(this).children().attr('src','../images/icon54.png')
        })
        //鼠标移入“查看节假日，显示节假日的具体信息”
        $('.tr').on('mouseover','.lookFestival',function(){
            if($(this).next('.festivals').html()){
                $(this).next('.festivals').show();
            }
        }).on('mouseout','.lookFestival',function(){
            $(this).next('.festivals').hide();
        })
        //修改微信排期
        $('.forWeixin .Time').on('click','.modify',function(){
            var _this = $(this);
            //获取刊例执行周期
            var beginTime = _this.parents('ul').attr('PubBeginTime'),
                endTime = _this.parents('ul').attr('PubEndTime');
            var curOldTime = $.trim(_this.parents('li').children('span:first-child').html());
            var curId = $(this).prev().attr('id');
            //获取除此之外的所有排期
            var scheduleArr = [],selectedPoint = [];
            _this.parents('.orderInfo').find('.forWeixin').each(function(i,v){
                if(_this.parents('.tr').attr('publishdetailid') == $(v).attr('publishdetailid') && _this.parents('.tr').attr('CartID') != $(v).attr('CartID')){
                    var timeObj = {
                        'S': $.trim($(v).find('.Time span').html()).substr(0,10),
                        'E': $.trim($(v).find('.Time span').html()).substr(0,10)
                    }
                    scheduleArr.push(timeObj);
                    var curBEtime = $.trim($(v).find('.Time span').html()).substr(0,10);
                    selectedPoint = selectedPoint.concat(curBEtime);
                }
            });
            laydate({
                elem: '#'+curId,
                format: 'YYYY-MM-DD hh:mm',
                fixed: false,
                istime: true,
                isclear: false,
                istoday:false,
                isInitCheck:false,
                isShowHoliday: true,//是否显示节假日
                voidDateRange:scheduleArr,
                selectedPoint:selectedPoint,
                min: getAvailableDate(beginTime,endTime)[0], //执行周期开始日期
                max: getAvailableDate(beginTime,endTime)[1], //执行周期结束日期
                choose: function (date) {
                    /*OptType 是   int 新增：1，修改：2，删除：3
                    CartID  是   int 购物车广告位ID
                    RecID   否   int 新增时不用传，修改删除：排期ID
                    BeginTime   是   datetime    排期时间*/
                    //修改排期调接口
                    var data = {
                        OptType:2,
                        MediaType: 14001,
                        CartID:_this.parents('.tr').attr('CartID')-0,
                        RecID:curId-0,
                        BeginTime:date,
                        EndTime:date
                    }
                    if(upScheduleInfo(data) == true){
                        allTime = _this.parents('ul').find('li span:first-child');
                        sortSchedule(allTime);
                    }else{
                        _this.parents('li').children('span:first-child').html(curOldTime);
                    }
                    //判断：如果此广告位为选中状态，对排期进行正确性验证
                    if(_this.parents('.tr').attr('isselected') == 1){
                        checkSchedule(_this);
                    }
                    
                }
            });

        });
        //APP排期修改
        $('.forAPP').on('click','.modify',function(e){
            e.stopPropagation();
            var _this = $(this);
            var isCPD = _this.parents('td').attr('isCPD');
            var PubBeginTime = _this.parents('td').attr('PubBeginTime').substr(0,10),
                PubEndTime = _this.parents('td').attr('PubEndTime').substr(0,10);
            switch(isCPD){
                // CPD
                case '11001':
                    _this.parents('.addBorder').find('.date_input').css({border:"1px solid #CBCBCB"});
                    _this.parents('.addBorder').find('.date_input').removeAttr('disabled');
                    //input的点击事件
                    _this.parents('.addBorder').find('.date_input').off('click').on('click',function(e){
                        e.stopPropagation();
                        var that = $(this);
                        var curId = that.attr('id');
                        // 获取开始和结束日期，方便后续使用
                        var oldStartTime = $.trim(_this.parents('.addBorder').find('.date_input').eq(0).val()),
                            oldEndTime = $.trim(_this.parents('.addBorder').find('.date_input').eq(1).val());
                        //获取开始或结束日期，置为不可选日期
                        var voidDateRange = [];

                        //已选日期数组(此数组是显示的具体的时间点)
                        var selectedPoint =[],selectDateArr = [];//selectDateArr每一项是一个对象，对象内包含开始日期和结束日期
                        // _this.parents('.addBorder').siblings('.addBorder').each(function(){
                        //     var timeObj = {
                        //         'S': $.trim($(this).find('input').eq(0).val()).substr(0,10),
                        //         'E': $.trim($(this).find('input').eq(1).val()).substr(0,10)
                        //     }
                        //     voidDateRange.push(timeObj);
                        // });
                        //获取其他开始和结束日期，置为不可选日期
                        // _this.parents('.addBorder').siblings('.addBorder').each(function(){
                        //     //获取此排期的开始日期和结束日期之间的日期
                        //     var curBEtime = getall($.trim($(this).find('input').eq(0).val()).substr(0,10),$.trim($(this).find('input').eq(1).val()).substr(0,10));
                        //     selectedPoint = selectedPoint.concat(curBEtime);
                        //     var selectSE = {
                        //         S:$.trim($(this).find('input').eq(0).val()).substr(0,10),
                        //         E:$.trim($(this).find('input').eq(1).val()).substr(0,10)
                        //     }
                        //     selectDateArr.push(selectSE);
                        // });

                        _this.parents('.orderInfo').find('.forAPP').each(function(i,v){
                            if(_this.parents('.tr').attr('publishdetailid') == $(v).attr('publishdetailid') && _this.parents('.tr').attr('CartID') != $(v).attr('CartID')){
                                var timeObj = {
                                    'S': $.trim($(v).find('.date_input').eq(0).val()).substr(0,10),
                                    'E': $.trim($(v).find('.date_input').eq(1).val()).substr(0,10)
                                }
                                voidDateRange.push(timeObj);
                                //获取此排期的开始日期和结束日期之间的日期
                                var curBEtime = getall($.trim($(this).find('.date_input').eq(0).val()).substr(0,10),$.trim($(v).find('.date_input').eq(1).val()).substr(0,10));
                                selectedPoint = selectedPoint.concat(curBEtime);
                                var selectSE = {
                                    S:$.trim($(v).find('.date_input').eq(0).val()).substr(0,10),
                                    E:$.trim($(v).find('.date_input').eq(1).val()).substr(0,10)
                                }
                                selectDateArr.push(selectSE);
                            }
                            
                        });
                        laydate({
                            elem: '#'+curId,
                            format: 'YYYY-MM-DD',
                            fixed: false,
                            istime: true,
                            isclear: false,
                            istoday:false,
                            isInitCheck:false,
                            isShowHoliday: true,//是否显示节假日
                            voidDateRange:voidDateRange,
                            selectedPoint: selectedPoint,
                            min: getAvailableDate(PubBeginTime,PubEndTime)[0], //执行周期开始日期
                            max: getAvailableDate(PubBeginTime,PubEndTime)[1], //执行周期结束日期
                            choose: function (date) {
                                var index = that.index();
                                //修改排期调接口
                                var updata = {
                                   "OptType" : 2,
                                   "MediaType" : 14002,
                                   "CartID" : _this.parents('.tr').attr('CartID')-0,
                                   "RecID" : _this.parents('.addBorder').attr('RecID')-0,
                                   "BeginTime" : oldStartTime,
                                   "EndTime" : oldEndTime
                                }
                                //修改开始日期
                                if(index == 0){
                                    updata.BeginTime = date.substr(0,10);
                                }else{
                                    updata.EndTime = date.substr(0,10);
                                }
                                //定义变量，使click事件只执行一次
                                var clickOn = 1;
                                //如果点击空白处，去掉Border
                                $(document).not(that).off('click').on('click',function(e){
                                    if(clickOn == 0){
                                        return false;
                                    }
                                    e.stopPropagation();
                                    that.css('border','none');
                                    that.attr('disabled','disabled');
                                    that.siblings('.date_input').css('border','none');
                                    that.siblings('.date_input').attr('disabled','disabled');
                                    //判断，修改后的排期和已选排期是否有冲突，若有冲突，还原成未修改前的排期
                                    for(var i=0;i<selectDateArr.length;i++){
                                        if(updata.BeginTime<selectDateArr[i].S && updata.EndTime>selectDateArr[i].E){
                                            layer.msg('同一时间段的排期不可重复选择');
                                            //修改开始日期
                                            if(index == 0){
                                                that.val(oldStartTime);
                                                that.siblings('.date_input').val(oldEndTime);
                                            }else{
                                                that.val(oldEndTime);
                                                that.siblings('.date_input').val(oldStartTime);
                                            }
                                            clickOn = 0;
                                            return false;
                                        }
                                    }
                                    //如果修改排期失败：开始日期和结束日期比较，开始不能大于结束，两者之间相差不能超过半年。则，恢复成未修改的排期。
                                    if(upScheduleInfo(updata) == false){
                                        //修改开始日期
                                        if(index == 0){
                                            that.val(oldStartTime);
                                            that.siblings('.date_input').val(oldEndTime);
                                        }else{
                                            that.val(oldEndTime);
                                            that.siblings('.date_input').val(oldStartTime);
                                        }
                                    }else{
                                        //上传排期信息成功后，调查看节假期接口，并修改行属性和行价格、总价
                                        setAjax({
                                            url:'http://www.chitunion.com/api/ShoppingCart/QueryHolidays?v=1_1',
                                            type:'get',
                                            data:{
                                                beginDate:updata.BeginTime.substr(0,10),
                                                endDate:updata.EndTime.substr(0,10)
                                            }
                                        },function(data){
                                            //如果请求过来有节假日信息，则追加节假日
                                            if(data.Status == 0 && data.Result.Holiday.length != 0){
                                                _this.parents('.addBorder').find('.festival').remove();
                                                for(var i=0;i<data.Result.Holiday.length;i++){
                                                    _this.parents('.addBorder').find('.festivals').append(
                                                        '<div class="festival">'+
                                                            '<span>'+data.Result.Holiday[i].BeginData.substr(0,10)+'</span>至'+
                                                            '<span>'+data.Result.Holiday[i].EndData.substr(0,10)+'</span>'+
                                                            '<span class="festivalName">'+data.Result.Holiday[i].Name+'</span>'+
                                                        '</div>'
                                                    )
                                                }
                                                //右侧如果初始状态没有“查看节假日”，则显示出来
                                                if(data.Result.Holiday.length){
                                                    _this.parents('.addBorder').find('.lookFestival').show();
                                                }
                                                //单价处，节假日价格显示
                                                _this.parents('td').next().find('p:eq(1)').show();
                                                var curIndex = _this.parents('.addBorder').index();
                                                var totalDays = getall(updata.BeginTime.substr(0,10),updata.EndTime.substr(0,10)).length;
                                                //设置总天数
                                                _this.parents('td').prev().find('.addBorder').eq(curIndex).find('.totalDays').html(totalDays+'天').attr('totalDays',totalDays);
                                                //左侧如果没有显示假期几天，显示出来,并设置假期天数
                                                _this.parents('td').prev().find('.addBorder').eq(curIndex).find('.Holidays').show().html('假期'+data.Result.Days+'天').attr('Holidays',data.Result.Days);

                                                //设置行总天数
                                                var scheduleDays = 0;
                                                _this.parents('td').prev().find('.totalDays').each(function(){
                                                    scheduleDays += ($(this).attr('totalDays')-0);
                                                });
                                                _this.parents('.tr').attr('scheduleDays',scheduleDays);

                                                //设置行总节假日天数
                                                var Holidays = 0;
                                                _this.parents('td').prev().find('.Holidays').each(function(){
                                                    Holidays += ($(this).attr('Holidays')-0);
                                                });
                                                _this.parents('.tr').attr('Holidays',Holidays);

                                                //修改行总价和总价格
                                                var TotalAmmount;
                                                //需要判断，是否区分节假日，如果区分节假日，不需要节假日的价格，全部按照非节假日价格计算。
                                                if(_this.parents('.tr').attr('HasHoliday') == 1){
                                                    TotalAmmount = Holidays*(_this.parents('.tr').attr('PriceHoliday'))+(scheduleDays-Holidays)*(_this.parents('.tr').attr('Price'));
                                                }else{
                                                    TotalAmmount = scheduleDays*(_this.parents('.tr').attr('Price'));
                                                }
                                                _this.parents('.tr').attr('TotalAmmount',TotalAmmount).find('.totalPrice').html(formatMoney(TotalAmmount));
                                                getAllAccount();
                                            //如果没有节假日信息，则把“查看节假日”隐藏，原来的节假日删除，左侧显示的假期天数，整体隐藏。
                                            }else if(data.Status == 0 && data.Result.Days == 0){
                                                _this.parents('.addBorder').find('.lookFestival').hide(); 
                                                _this.parents('.addBorder').find('.festivals').html('');
                                                //没有节假日，不显示节假日价格
                                                _this.parents('td').next().find('p:eq(1)').hide();
                                                _this.parents('td').prev().find('.Holidays').attr('holidays',0).html('').hide();
                                                var curIndex = _this.parents('.addBorder').index();
                                                var totalDays = getall(updata.BeginTime.substr(0,10),updata.EndTime.substr(0,10)).length;
                                                //设置总天数
                                                _this.parents('td').prev().find('.addBorder').eq(curIndex).find('.totalDays').html(totalDays+'天').attr('totalDays',totalDays);
                                                //设置行总天数
                                                var scheduleDays = 0;
                                                _this.parents('td').prev().find('.totalDays').each(function(){
                                                    scheduleDays += ($(this).attr('totalDays')-0);
                                                });
                                                _this.parents('.tr').attr('scheduleDays',scheduleDays);

                                                //设置行总节假日天数
                                                var Holidays = 0;
                                                _this.parents('td').prev().find('.Holidays').each(function(){
                                                    Holidays += ($(this).attr('Holidays')-0);
                                                });
                                                _this.parents('.tr').attr('Holidays',Holidays);

                                                //修改行总价和总价格
                                                var TotalAmmount;
                                                //需要判断，是否区分节假日，如果区分节假日，不需要节假日的价格，全部按照非节假日价格计算。
                                                if(_this.parents('.tr').attr('HasHoliday') == 1){
                                                    TotalAmmount = Holidays*(_this.parents('.tr').attr('PriceHoliday'))+(scheduleDays-Holidays)*(_this.parents('.tr').attr('Price'));
                                                }else{
                                                    TotalAmmount = scheduleDays*(_this.parents('.tr').attr('Price'));
                                                }
                                                _this.parents('.tr').attr('TotalAmmount',TotalAmmount).find('.totalPrice').html(formatMoney(TotalAmmount));
                                                getAllAccount();
                                            }
                                        })
                                        //判断：如果此广告位为选中状态，对排期进行正确性验证
                                        if(_this.parents('.tr').attr('isselected') == 1){
                                            checkSchedule(_this);
                                        }
                                    }
                                    clickOn = 0;
                                })
                            }
                        });
                    })
                    break;
                //CPM
                case '11002':
                    //使input可修改并加上边框
                    _this.parents('td').find('.date_input').css({border:"1px solid #CBCBCB"});
                    _this.parents('td').find('.date_input').removeAttr('disabled');
                    //input的点击事件
                    _this.parent('div').find('.date_input').off('click').on('click',function(e){
                        e.stopPropagation();
                        var that = $(this);
                        var curId = that.attr('id');
                        //获取开始或结束日期，置为不可选日期
                        var oldStartTime = $.trim(_this.parent('div').find('.date_input').eq(0).val()),
                            oldEndTime = $.trim(_this.parent('div').find('.date_input').eq(1).val());
                        //获取开始或结束日期，置为不可选日期
                        var cpmVoidDateRange = [];
                        //已选日期数组(此数组是显示的具体的时间点)
                        var cpmSelectedPoint =[],cpmSelectDateArr = [];//selectDateArr每一项是一个对象，对象内包含开始日期和结束日期
                        _this.parents('.orderInfo').find('.forAPP').each(function(i,v){
                            if(_this.parents('.tr').attr('publishdetailid') == $(v).attr('publishdetailid') && _this.parents('.tr').attr('CartID') != $(v).attr('CartID')){
                                var timeObj = {
                                    'S': $.trim($(v).find('.date_input').eq(0).val()).substr(0,10),
                                    'E': $.trim($(v).find('.date_input').eq(1).val()).substr(0,10)
                                }
                                cpmVoidDateRange.push(timeObj);
                                //获取此排期的开始日期和结束日期之间的日期
                                var curBEtime = getall($.trim($(this).find('.date_input').eq(0).val()).substr(0,10),$.trim($(v).find('.date_input').eq(1).val()).substr(0,10));
                                cpmSelectedPoint = cpmSelectedPoint.concat(curBEtime);
                                var selectSE = {
                                    S:$.trim($(v).find('.date_input').eq(0).val()).substr(0,10),
                                    E:$.trim($(v).find('.date_input').eq(1).val()).substr(0,10)
                                }
                                cpmSelectDateArr.push(selectSE);
                            }
                            
                        });
                        laydate({
                            elem: '#'+curId,
                            format: 'YYYY-MM-DD',
                            fixed: false,
                            istime: true,
                            isclear: false,
                            istoday:false,
                            isInitCheck:false,
                            isShowHoliday: true,//是否显示节假日
                            voidDateRange:cpmVoidDateRange,
                            selectedPoint:cpmSelectedPoint,
                            min: getAvailableDate(PubBeginTime,PubEndTime)[0], //执行周期开始日期
                            max: getAvailableDate(PubBeginTime,PubEndTime)[1], //执行周期结束日期
                            choose: function (date) {
                                var index = that.index();
                                //修改排期调接口
                                var updata = {
                                   "OptType" : 2,
                                   "MediaType" : 14002,
                                   "CartID" : _this.parents('.tr').attr('CartID'),
                                   "RecID" : _this.parent('div').attr('RecID'),
                                   "BeginTime" : oldStartTime,
                                   "EndTime" : oldEndTime
                                }
                                //修改开始日期
                                if(index == 0){
                                    updata.BeginTime = date.substr(0,10);
                                }else{
                                    updata.EndTime = date.substr(0,10);
                                }
                                var cpmClickOn = 1;
                                $(document).not(that).off('click').on('click',function(e){
                                    if(cpmClickOn == 0){
                                        return false;
                                    }
                                    e.stopPropagation();
                                    that.css('border','none');
                                    that.attr('disabled','disabled');
                                    that.siblings('.date_input').css('border','none');
                                    that.siblings('.date_input').attr('disabled','disabled');
                                    //判断，修改后的排期和已选排期是否有冲突，若有冲突，还原成未修改前的排期
                                    for(var i=0;i<cpmSelectDateArr.length;i++){
                                        if(updata.BeginTime<cpmSelectDateArr[i].S && updata.EndTime>cpmSelectDateArr[i].E){
                                            layer.msg('同一时间段的排期不可重复选择',{time:1500});
                                            //修改开始日期
                                            if(index == 0){
                                                that.val(oldStartTime);
                                                that.siblings('.date_input').val(oldEndTime);
                                            }else{
                                                that.val(oldEndTime);
                                                that.siblings('.date_input').val(oldStartTime);
                                            }
                                            cpmClickOn = 0;
                                            return false;
                                        }
                                    }
                                    if(upScheduleInfo(updata) == false){
                                        //修改开始日期
                                        if(index == 0){
                                            that.val(oldStartTime);
                                            that.siblings('.date_input').val(oldEndTime);
                                        }else{
                                            that.val(oldEndTime);
                                            that.siblings('.date_input').val(oldStartTime);
                                        }
                                    }
                                    //判断：如果此广告位为选中状态，对排期进行正确性验证,后期再做
                                    if(_this.parents('.tr').attr('isselected') == 1){
                                        checkSchedule(_this);
                                    }
                                    cpmClickOn = 0;
                                })
                            }
                        });
                    })
                    break;
                default:
                    break;
            }

        })
        /*CPM修改排期天次，对应改变tr上的属性及页面显示的价格(行价格和总价，需判断是否选中)*/
        $('.to_b_list .cpmADLaunchDays').each(function(){
            var that = $(this);
            var Price = that.parents('.tr').attr('Price')*1;
            that.on("input",function(){
                replaceAndSetPos(this,/[^0-9]/g,'');
            }).on('change',function () {
                var val = $(this).val();
                that.parents('.tr').attr('scheduleDays',val);
                that.parents('.tr').attr('TotalAmmount',val*Price);
                that.parents('.tr').find('.totalPrice').html(formatMoney(val*Price));
                getAllAccount();
            })
        })
    }
    /*
    * 修改微信排期后对所有排期进行排序显示
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
    }

    /*3、删除广告位(单行&删除所选)
        删除时需要判断对应媒体下是否还有广告位，若无，移除媒体名称
    */
    function delAdv(){
        $('.to_b_list').off('click').on('click','.delAdv',function(){
            var _this = $(this);
            layer.confirm('您确认删除此广告位吗', {
                time: 0 //不自动关闭
                , btn: ['确认', '取消']
                , yes: function (index) {
                    //调接口上传删除信息
                    setAjax({
                        url:'http://www.chitunion.com/api/ShoppingCart/DeleteShoppingCart?v=1_1',
                        type:'get',
                        data:{
                            cartids:_this.parents('.tr').attr('cartid')
                        }
                    },function(data){
                        if(data.Status == 0){
                            _this.parents('.tr').remove();
                            layer.close(index);
                            layer.msg('广告位删除成功',{time:3000});
                            getAllAccount();
                            $('.MediaOwner').each(function(){
                                if($(this).find('.tr').length == 0){
                                    $(this).remove();
                                }
                            });
                        }else{
                            layer.msg(data.Message,{time:3000});
                        }
                    });
                }
            });
        })
        
        /*删除所选广告位*/
        $('#delCheckedAdv').off('click').on('click',function(){
            var allAdv = $('.tr');
            //存储要删除的购物车广告位ID
            var cartids = [];
            allAdv.each(function(){
                if($(this).attr('isselected') == 1 || $(this).parents('table').prev().find('.twoCheck').prop('checked')){
                    cartids.push($(this).attr('cartid'));
                    $(this).attr('gotoDel',1);;
                }
            });
            if(cartids.length == 0){
                layer.confirm('请选择要删除的广告位',{
                    time:0,
                    btn:['确认','取消'],
                    yes:function(index){
                        layer.close(index);
                    }
                })
            }else{
                layer.confirm('您确认删除所选广告位吗', {
                    time: 0 //不自动关闭
                    , btn: ['确认', '取消']
                    , yes: function (index) {
                        //调接口上传删除信息
                        setAjax({
                            url:'http://www.chitunion.com/api/ShoppingCart/DeleteShoppingCart?v=1_1',
                            type:'get',
                            data:{
                                cartids:cartids.join(',')
                            }
                        },function(data){
                            if(data.Status == 0){
                                $('.orderInfo').find('.tr').each(function(){
                                    if($(this).attr('gotoDel') == 1){
                                        $(this).remove();
                                    }
                                });
                                $('.orderInfo').find('.tr').each(function(){
                                    if($(this).attr('gotoDel') == 1){
                                        $(this).remove();
                                    }
                                });
                                layer.msg('广告位删除成功',{time:3000});
                                getAllAccount();
                                $('.MediaOwner').each(function(){
                                    if($(this).find('.tr').length == 0){
                                        $(this).remove();
                                    }
                                });
                            }else{
                                layer.msg(data.Message,{time:3000});
                            }
                        });
                    }
                });
            }
        })
    }

    /*4、确认投放时获取页面订单信息*/
    $('#nextBtn').off('click').on('click',function(){
        if($('.validAdv[isselected=1]').length<1){
            layer.msg('请选择要投放的广告位',{time:3000});
            return
        }
        var cpmADLaunchDaysFlag = true;
        $('.validAdv[isselected=1]').each(function(){
            if($(this).find('.cpmADLaunchDays').val() <=0 || $(this).find('.cpmADLaunchDays').val() >365){
                cpmADLaunchDaysFlag = false;
            }
        })
        if(cpmADLaunchDaysFlag == false){
            layer.msg('CPM投放天次应在1-365之间，请修改');
            return
        }
        checkSchedule();

        //判断是否是AE，如果是AE，判断是从哪进入（审核进入/一级菜单进入）
        //若是审核进入，url上有orderID,会显示在select中，直接获取select的内容，调转到审核页
        //若是一级菜单进入，若选择添加到项目，带着orderID,跳转到审核页，若未勾选，不带任何id，跳转到购物车第二步
        //如果不是AE，跳转到购物车第二步
        var data = {
            "OrderID" : "",
            "IDs" : []
        }
        if(flag == true){
            switch(CTLogin.RoleIDs){
                //超级管理员、运营和AE是一样的权限
                case 'SYS001RL00001':
                case 'SYS001RL00004':
                case 'SYS001RL00005':
                    //如果AE不添加到项目，直接跳转到第二步，会生成一个orderID,跳转到第三步
                    //如果AE勾选添加到项目，1、选择项目ID,需要验证，然后跳转到审核页面，
                    //2、如果不选项目ID，跳转到第二步,此时需要把orderID带到第二步，以便第二步点击“返回购物车”时，第一步还有orderID
                    //验证是否和所选项目有冲突
                    if($('.add_project').find('input').prop('checked') && $('.add_project').find('select').val() != '请选择项目ID+项目名称'){
                        if(PubDetailVertify() == false){
                            return
                        }else{
                            data.OrderID = $.trim($('.add_project').find('select').val()).substr(0,13);
                            getInfoAndSubmit(data,'/OrderManager/AuditProject.html?orderID='+data.OrderID);
                        }
                    }else{
                        //判断url上是否有orderID,如果有，带着orderID跳转到第二步
                        if(!orderID){
                            if(GetUserId().userID){
                                getInfoAndSubmit(data,'/OrderManager/shopcartForMedia02.html?userID=' + GetUserId().userID);
                            }else{
                                getInfoAndSubmit(data,'/OrderManager/shopcartForMedia02.html');
                            }
                        }else{
                            getInfoAndSubmit(data,'/OrderManager/shopcartForMedia02.html?orderID='+orderID);
                        }
                    }
                    break;
                default:
                    //非AE时跳转到购物车第二步
                    getInfoAndSubmit(data,'/OrderManager/shopcartForMedia02.html');
                    break;
            }
        }else{
            layer.msg('排期信息有误',{time:3000});
        }   
    })


    /*5、查看详情
        1 已下架：表示选定投放时间所属的刊例已下架

         a)若该广告单元所属广告有其他已上架的刊例：点击跳转到该广告的详情页

         b)该广告单元所属广告下没有已上架的刊例：点击弹框提示“很抱歉，该广告已被下架或删除”

        2 已失效：表示添加到购物车的广告所属媒体/刊例已删除，点击弹框提示“很抱歉，该广告已下架或被删除”

    */
    function lookDetail(){
        $('.orderInfo').off('click').on('click','.toDetail',function(){
            var _this = $(this);
            var curTr = _this.parents('tr');
            var PublishStatus = curTr.attr('PublishStatus');
            var expired = curTr.attr('expired');
            var HasOtherPublish = curTr.attr('HasOtherPublish');
            var MediaType = curTr.attr('MediaType');
            var MediaID = curTr.attr('MediaID');
            var TemplateID = curTr.attr('TemplateID');
            switch(MediaType){
                case '14001':
                    if(expired == -1){
                        layer.confirm('很抱歉，该广告已下架或被删除', {
                            time: 0 //不自动关闭
                            , btn: ['从购物车中删除', '知道了']
                            , yes: function (index) {
                                //调接口上传删除信息
                                setAjax({
                                    url:'http://www.chitunion.com/api/ShoppingCart/DeleteShoppingCart?v=1_1',
                                    type:'get',
                                    data:{
                                        cartids:_this.parents('.tr').attr('cartid')
                                    }
                                },function(data){
                                    if(data.Status == 0){
                                        _this.parents('.tr').remove();
                                        layer.close(index);
                                        layer.msg('广告位删除成功',{time:3000});
                                        getAllAccount();
                                        $('.MediaOwner').each(function(){
                                            if($(this).find('.tr').length == 0){
                                                $(this).remove();
                                            }
                                        });
                                    }else{
                                        layer.msg(data.Message,{time:3000});
                                    }
                                });
                            }
                        });
                    }else{
                        if(PublishStatus == 42011){
                            window.location = '/OrderManager/wx_detail.html?MediaID='+MediaID;
                        }else{
                            if(HasOtherPublish == 1){
                                window.location = '/OrderManager/wx_detail.html?MediaID='+MediaID;
                            }else{
                                layer.confirm('很抱歉，该广告已下架或被删除', {
                                    time: 0 //不自动关闭
                                    , btn: ['从购物车中删除', '知道了']
                                    , yes: function (index) {
                                        //调接口上传删除信息
                                        setAjax({
                                            url:'http://www.chitunion.com/api/ShoppingCart/DeleteShoppingCart?v=1_1',
                                            type:'get',
                                            data:{
                                                cartids:_this.parents('.tr').attr('cartid')
                                            }
                                        },function(data){
                                            if(data.Status == 0){
                                                _this.parents('.tr').remove();
                                                layer.close(index);
                                                layer.msg('广告位删除成功',{time:2000});
                                                getAllAccount();
                                                $('.MediaOwner').each(function(){
                                                    if($(this).find('.tr').length == 0){
                                                        $(this).remove();
                                                    }
                                                });
                                            }else{
                                                layer.msg(data.Message,{time:2000});
                                            }
                                        });
                                    }
                                });
                            }
                        }
                    }
                    break;
                case '14002':
                    if(expired == -1){
                        layer.confirm('很抱歉，该广告已下架或被删除', {
                            time: 0 //不自动关闭
                            , btn: ['从购物车中删除', '知道了']
                            , yes: function (index) {
                                //调接口上传删除信息
                                setAjax({
                                    url:'http://www.chitunion.com/api/ShoppingCart/DeleteShoppingCart?v=1_1',
                                    type:'get',
                                    data:{
                                        cartids:_this.parents('.tr').attr('cartid')
                                    }
                                },function(data){
                                    if(data.Status == 0){
                                        _this.parents('.tr').remove();
                                        layer.close(index);
                                        layer.msg('广告位删除成功',{time:2000});
                                        getAllAccount();
                                        $('.MediaOwner').each(function(){
                                            if($(this).find('.tr').length == 0){
                                                $(this).remove();
                                            }
                                        });
                                    }else{
                                        layer.msg(data.Message,{time:2000});
                                    }
                                });
                            }
                        });
                    }else{
                        if(PublishStatus == 49004){
                            window.location = '/OrderManager/app_detail.html?MediaID='+MediaID+'&TemplateID='+TemplateID;
                        }else{
                            if(HasOtherPublish == 1){
                                window.location = '/OrderManager/app_detail.html?MediaID='+MediaID+'&TemplateID='+TemplateID;
                            }else{
                                layer.confirm('很抱歉，该广告已下架或被删除', {
                                    time: 0 //不自动关闭
                                    , btn: ['从购物车中删除', '知道了']
                                    , yes: function (index) {
                                        //调接口上传删除信息
                                        setAjax({
                                            url:'http://www.chitunion.com/api/ShoppingCart/DeleteShoppingCart?v=1_1',
                                            type:'get',
                                            data:{
                                                cartids:_this.parents('.tr').attr('cartid')
                                            }
                                        },function(data){
                                            if(data.Status == 0){
                                                _this.parents('.tr').remove();
                                                layer.close(index);
                                                layer.msg('广告位删除成功',{time:3000});
                                                getAllAccount();
                                                $('.MediaOwner').each(function(){
                                                    if($(this).find('.tr').length == 0){
                                                        $(this).remove();
                                                    }
                                                });
                                            }else{
                                                layer.msg(data.Message,{time:3000});
                                            }
                                        });
                                    }
                                });
                            }
                        }
                    }
                    break;
                default:
                    break;
            }
        })
    }

    
    // 只能输入数字 
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
    /*只能输入数字，结束*/
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
                //获取总价格
                getAllAccount();
                //排期操作
                operateSchedule();
                //删除广告位操作
                delAdv();
                //查看详情
                lookDetail();

                //判断，是否显示节假日单价
                $('.orderInfo .tr').each(function(){
                    if($(this).find('.lookFestival:hidden').length == $(this).find('.lookFestival').length){
                        $(this).children().eq(4).find('p:eq(1)').hide();
                    }else{
                        $(this).children().eq(4).find('p:eq(1)').show();
                    }
                });
                $('.MediaOwner').each(function() {
                    if($(this).find('.onecheck:checked').length == $(this).find('.onecheck').length){
                        $(this).find('.twoCheck').prop('checked',true);
                    }
                });
                if($('.twoCheck:checked').length == $('.twoCheck').length){
                    $('.allChecked').prop('checked',true);
                }else{
                    $('.allChecked').prop('checked',false);
                }
                /*判断是否选择广告位，若无，提示请选择要添加到项目的广告位，单选框不可选。
                    若有，用户可选择对应的项目ID
                */
                $('.add_project').off('change').on('change','input',function(){
                    if($('.orderInfo').find('.validAdv[isselected=1]').length<1){
                        layer.msg('请选择要添加到项目的广告位',{time:3000});
                        $(this).attr('checked',false);
                    }else{
                        if($('.add_project').find('select').val() == '请选择项目ID'){
                            $('.add_project').find('.message').show();
                        }else{
                            $('.add_project').find('.message').hide();
                        }
                    }
                })

                /*当某一广告位切换选中状态时，对应tr改变isselected属性*/
                $('.orderInfo .validAdv').off('change').on('change','.onecheck',function(){
                    var _this = $(this);
                    //改变对应行的isselected属性
                    if(_this.parents('.tr').attr('isselected') == 1){
                        _this.parents('.tr').attr('isselected',0);
                    }else{
                        _this.parents('.tr').attr('isselected',1);
                    }
                    //判断，若媒体下所有广告位选中，媒体主选中
                    if(_this.parents('.MediaOwner').find('.validAdv .onecheck:checked').length == _this.parents('.MediaOwner').find('.validAdv').length){
                        _this.parents('.MediaOwner').find('.twoCheck').prop('checked',true);
                    }else{
                        _this.parents('.MediaOwner').find('.twoCheck').prop('checked',false);
                    }

                    //判断，若所有可选广告位选中，全选选中
                    if($('.validAdv .onecheck:checked').length ==$('.MediaOwner').find('.validAdv').length){
                        $('.allChecked').prop('checked',true);
                    }else{
                        $('.allChecked').prop('checked',false);
                    }
                    //如果没有广告位选中，AE添加到项目选择框不可选
                    if($('.validAdv .onecheck:checked').length == 0){
                        $('.add_project').find('input').prop('checked',false);
                    }
                    //修改总价格
                    getAllAccount();
                })

                /*选中媒体时，其下广告位选中*/
                $('.MediaOwner').off('change').on('change','.twoCheck',function(){
                    if ($(this).prop('checked')) {
                        //修改选中状态
                        $(this).parents('.MediaOwner').find('.onecheck').prop('checked', true);
                        //修改tr属性isselected
                        $(this).parents('.MediaOwner').find('.tr').attr('isselected',1);
                    }else{
                        $(this).parents('.MediaOwner').find('.onecheck').prop('checked', false);
                        $(this).parents('.MediaOwner').find('.tr').attr('isselected',0);
                        //判断，如果取消选择，同时，没有广告位被选择，添加到项目选择框为未选状态
                        if($('.validAdv .onecheck:checked').length == 0){
                            $('.add_project').find('input').prop('checked',false);
                        }
                    }
                    if($('.twoCheck:checked').length == $('.twoCheck').length){
                        $('.allChecked').prop('checked',true);
                    }else{
                        $('.allChecked').prop('checked',false);
                    }
                    getAllAccount();
                })

                /*全选*/
                $('.allChecked').off('change').on('change',function(){
                    if($(this).prop('checked')){
                        $('.wx_box').find('.onecheck,.twoCheck,.allChecked').prop('checked',true);
                        $('.tr').attr('isselected',1);
                    }else{
                        $('.wx_box').find('.onecheck,.twoCheck,.allChecked').prop('checked',false);
                        $('.tr').attr('isselected',0);
                        $('.add_project').find('input').prop('checked',false);
                    }
                    getAllAccount();
                })
                
            }
        })
    }

    /*7、确认投放时，获取订单信息*/
    function getInfoAndSubmit(data,url){
       /* MediaType   是   int 媒体类型，微信：14001，APP：14002，微博：14003，视频：14004，直播：14005
        OrderID 是   string  项目号，不为空时修改对应项目
        CartID  是   int 购物车ID
        MediaID 是   int 媒体ID
        PublishDetailID 是   int 广告位ID*/
        var allAdv = $('.orderInfo').find('.tr[IsSelected]');
        allAdv.each(function(){
            var _this = $(this);
            data.IDs.push({
                "MediaType":_this.attr('MediaType'),
                "CartID" : _this.attr('cartid'),
                "MediaID" : _this.attr('mediaid'),
                "PublishDetailID" : _this.attr('publishdetailid'),
                "SaleAreaID" : _this.attr('SaleAreaID')== undefined ? -2:_this.attr('SaleAreaID'),
                "ADLaunchDays" : _this.attr('scheduledays'),
                "IsSelected" : _this.attr('IsSelected')

            });
        });
        setAjax({
            url:'http://www.chitunion.com/api/ShoppingCart/Delivery_ShoppingCart?v=1_1',
            type:'post',
            data:data
        },function(data){
            if(data.Status == 0){
                window.location = url;
            }else{
                layer.msg(data.Message,{time:2000});
            }
        })
    }

    /**
     * 修改、删除、添加排期时调用接口
     * @param data{
            OptType 是   int 新增：1，修改：2，删除：3
            CartID  是   int 购物车广告位ID
            RecID   否   int 新增时不用传，修改删除：排期ID
            BeginTime   是   datetime    排期时间
        } 
     * @param Schedule   修改、删除或添加的排期
     * 
     */
    /*上传排期信息*/
    function upScheduleInfo(data){
        var isUpData = 1;
        $.ajax({
            url: 'http://www.chitunion.com/api/ShoppingCart/ADScheduleOpt_ShoppingCart?v=1_1',
            type: 'post',
            async: false,
            data: data,
            dataType: 'json',
            xhrFields: {
                withCredentials: true
            },
            crossDomain: true,
            success: function (data) {
                if(data.Status != 0){
                    isUpData = 0;
                    layer.msg(data.Message,{time:3000});
                }
            }
        });
        return isUpData
    }


    /*验证购物车中当前选择的广告位在项目中是否已存在。*/
    function PubDetailVertify(){
        var flag = true;
        var allAdv = $('.orderInfo').find('.tr[IsSelected=1]');
        var Media = [];
        allAdv.each(function(){
            var obj = {
                MediaType:$(this).attr('MediaType'),
                CartID:$(this).attr('CartID'),
                PublishDetailID:$(this).attr('PublishDetailID'),
                SaleAreaID:$(this).attr('SaleAreaID') == undefined ? -2 : $(this).attr('SaleAreaID')
            }
            Media.push(obj);
        });
        $.ajax({
            url: 'http://www.chitunion.com/api/ShoppingCart/PubDetailVertify_ADOrderOrCart?v=1_1',
            type: 'post',
            async : false,
            data: {
                "OrderID":$.trim($('.add_project').find('select').val()).substr(0,13),
                "Media":Media
            },
            dataType: 'json',
            xhrFields: {
                withCredentials: true
            },
            crossDomain: true,
            success: function(data){
                if(data.Status == 0){
                }else{
                    //获取后台返回的冲突信息，内容为冲突的cartID，只返回第一个冲突的广告位
                    allAdv.each(function(){
                        //如果当前的广告位在冲突的广告位之中，显示提示信息
                        if(data.Message.indexOf($(this).attr('cartid')) != -1){
                            $(this).find('.PubDetailVertify').show();
                        }else{
                            $(this).find('.PubDetailVertify').hide();
                        }
                    });
                    flag =false;
                }
            }
        });
        return flag;
    }

    /*获取行价格和总价，传入操作对象（删除排期时传值，其他情况不传）*/
    function getAllAccount(event){
        if(event){
            var totalPrice = formatMoney(event.parents('.tr').attr('price')*event.parents('.tr').attr('scheduledays'));
            event.parents('.tr').children().eq(5).children().html(totalPrice);
        }
        var allSelectTr = $('.orderInfo').find('.validAdv[IsSelected=1]');
        var mediaCount = 0;
        var account = 0;
        $('.orderInfo').find('.validAdv[IsSelected=1]').each(function(i,v){
            mediaCount++;
            account += ($(v).attr('TotalAmmount')-0);
        });
        //媒体个数
        $('.tableNum').html(mediaCount);
        //总价
        $('.OrderPrice').html(formatMoney(account));
    }

    
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
    }

    /**
     * 微信刊例执行周期和当前日期比较，获取最新可用执行周期，作为laydate的可选日期
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

    /*获取两个时间之间的日期,调用函数getAll,注：包括开始日期和结束日期*/
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

    /*验证广告位的排期是否都大于今天(传当前对象验证当前行，不传值验证所有的排期)*/
    function checkSchedule(event){
        var curTime = new Date().format('yyyy-MM-dd');
        flag = true;
        //点击立即投放时判断广告位排期
        if(!event){
            $('.forWeixin[isselected=1] .Time').find('span:first-child').each(function(){
                if($.trim($(this).text()).substr(0,10) < curTime ){
                    flag = false;
                    $(this).css('color','red').parents('.tr').find('.message').show();
                 }
            });
            $('.forAPP[isselected=1]').find('.date_input:first-child').each(function(){
                if($.trim($(this).val()).substr(0,10) < curTime){
                    flag = false;
                    $(this).parents('td').find('.grey:first').show();
                    $(this).parent('div').find('.date_input').css('color','red');
                }
            });
        //修改排期时，判断排期是否修改正确
        }else{
            switch(event.parents('.validAdv').attr('MediaType')){
                case '14001':
                    event.parents('.validAdv').find('span:first-child').each(function(){
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
                    event.parents('.validAdv').find('span:first-child').each(function(){
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
                    break;
                case '14002':
                    var count = 0;
                    event.parent('div').find('.date_input').each(function(){
                        if($(this).val() > curTime){
                            count ++;
                        }
                    })
                    if(count == 2){
                        event.parents('td').find('.grey:first').hide();
                        event.parent('div').find('.date_input').css('color','#666');
                    }else{
                        event.parents('td').find('.grey:first').show();
                        event.parent('div').find('.date_input').css('color','red')
                    }
                    break;
                default:
                    break;

            }
        }
        return flag;
    }

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



