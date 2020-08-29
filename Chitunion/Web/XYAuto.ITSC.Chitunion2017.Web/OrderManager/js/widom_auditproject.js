/*
* 1.渲染订单基本信息
* 2.渲染广告位
* 3.选择渠道
* 4.添加广告
*
* */

$(function () {

    var orderID = GetQueryString('orderID')!=null&&GetQueryString('orderID')!='undefined'?GetQueryString('orderID'):null;
    var config = {};//广告位数据默认数组

    //获取参数向后台传输
    var ADDetails = [];

    var DetailOfOrder = {
        constructor : DetailOfOrder,
        init : function () {//初始化
            //根据项目号查看项目的接口  取信息
            var url = 'http://www.chitunion.com/api/ADOrderInfo/IntelligenceADOrderInfoQuery?v=1_1';
            //var url = 'js/ProjectDetail.json';
            setAjax({
                url : url,
                type : 'get',
                data : {
                    orderid : orderID
                }
            },function (data) {
                var Result = data.Result;
                config = Result;
                if(data.Status == 0){
                    $('.install_box').html(ejs.render($('#wisdom-project').html(),config));

                    //计算每一个地区的价格的总计
                    config.AreaInfos.forEach(function(item,i){
                        var PublishDetails = item.PublishDetails;
                        var SalePrice = 0;//销售
                        var OriginalReferencePrice = 0;//原创
                        var CostReferencePrice = 0;//成本
                        var FinalCostPrice = 0;//最终成本价
                        PublishDetails.forEach(function(each){
                            SalePrice += each.SalePrice;
                            CostReferencePrice += each.CostReferencePrice;
                            FinalCostPrice += each.FinalCostPrice;
                            if(each.EnableOriginPrice == true){
                                OriginalReferencePrice += each.OriginalReferencePrice;
                            }else{
                                OriginalReferencePrice += 0;
                            }
                        })
                        var cur_tab = $('.to_b_list_box').find('.to_b_list').eq(i);
                        cur_tab.find('.SalePrice').text(formatMoney(SalePrice,2));
                        cur_tab.find('.CostReferencePrice').text(formatMoney(CostReferencePrice,2));
                        cur_tab.find('.FinalCostPrice').text(formatMoney(FinalCostPrice,2));
                        cur_tab.find('.OriginalReferencePrice').text(formatMoney(OriginalReferencePrice,2));
                    })

                    DetailOfOrder.returnStateText(Result.ADOrderInfo.Status);
                    //项目基本信息
                    DetailOfOrder.OrderMessage();
                }else{
                    layer.msg(data.Message,{'time':1000});
                }
            })
        },
        OrderMessage : function () {//项目基本信息
            //上传
            DetailOfOrder.uploadFileAll();
            //计算媒体个数和总价
            DetailOfOrder.ModifyAmount();
            //添加渠道
            DetailOfOrder.PositionMessage();
            //添加广告
            DetailOfOrder.AddPosition();
            //展开收起
            DetailOfOrder.LayoutChanges();
            //保存
            DetailOfOrder.VerificationInformation();
        },
        PositionMessage : function () {//添加渠道 删除广告位
            var city_idx = 0;
            var channel_idx = 0;

            //选择渠道弹层
            $('.choose_channel').off('click').on('click',function () {
                var that = $(this);
                var _width = document.documentElement.clientWidth;
                var _height = document.documentElement.clientHeight;

                var AdPosition1 = that.parents('.Tr').attr('ADPositionID')*1;
                var AdPosition2 = 7002;
                var AdPosition3 = that.parents('.Tr').attr('CreateTypeID')*1;
                var ChannelID = that.parents('.Tr').attr('ChannelID')*1;
                var MediaID = that.parents('.Tr').attr('MediaID')*1;
                var CooperateDate = that.parents('.Tr').attr('LaunchTime').substr(0,10);

                var url = 'http://www.chitunion.com/api/ADOrderInfo/GetChannelList?v=1_1';
                setAjax({
                    url : url,
                    type : 'get',
                    data : {
                        /*ChannelId : ChannelID,*/
                        MediaId : MediaID,
                        /*ChannelId : 6,
                        MediaId : 16668,*/
                        AdPosition1 : AdPosition1,
                        //AdPosition1 : 6003,
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

                        city_idx = that.parents('.to_b_list').index();
                        channel_idx = that.parents('tr').index()-1;

                        //关闭弹层
                        $('#closebt1').on('click',function () {
                            $('.channel_box').hide();
                            $('.channel_box .layer').hide();
                        })

                        //最终成本价默认为文本 鼠标点击还可以输入修改价格
                        $('.channel_box .tdcosting_price').on('click',function () {
                            var that = $(this);
                            that.find('.costing_price').removeAttr('disabled');
                        })
                        $('.channel_box .tdcosting_price').on('blur',function () {
                            var that = $(this);
                            that.find('.costing_price').prop('disabled',true);
                        })

                        //最终成本价只能输入数字
                        $('.channel_box .costing_price').on("input",function(){
                            replaceAndSetPos(this,/[^0-9]/g,'');
                            var val = $.trim($(this).val());
                            $(this).parents('tr').attr('FinalCostPrice',val);
                        }).on('blur',function () {
                            var val = $.trim($(this).val());
                            if(val == ''){
                                layer.msg('请输入最终成本价',{'time':1000});
                            }
                        })
                        //选择按钮
                        $('.channel_box .choose_btn').on('click',function () {
                            var ChannelName = $(this).parents('tr').attr('ChannelName');
                            var FinalCostPrice = $(this).parents('tr').attr('FinalCostPrice');
                            var ChannelID = $(this).parents('tr').attr('ChannelID');

                            if(FinalCostPrice == ''){
                                layer.msg('请输入最终成本价',{'time':1000});
                            }else{
                                var curTable = $('.to_b_list_box').find('.to_b_list').eq(city_idx);
                                var curTr = curTable.find('table tr').eq(channel_idx+1);
                                var curTd = curTr.find('td').eq(6);

                                $('.channel_box').hide();
                                $('.channel_box .layer').hide();
                                curTd.find('span').eq(0).text(formatMoney(FinalCostPrice,2));
                                curTd.find('span').eq(1).text(ChannelName);
                                //同时应该将每一行绑定的数据也修改 成本参考价  渠道ID
                                curTr.attr('FinalCostPrice',FinalCostPrice);
                                curTr.attr('ChannelID',ChannelID);

                                //将config里面的价格也发生改变
                                config.AreaInfos[city_idx].PublishDetails[channel_idx].FinalCostPrice = FinalCostPrice*1;

                                //计算每一个地区的价格的总计
                                config.AreaInfos.forEach(function(item,i){
                                    var PublishDetails = item.PublishDetails;
                                    var SalePrice = 0;//销售
                                    var OriginalReferencePrice = 0;//原创
                                    var CostReferencePrice = 0;//成本
                                    var FinalCostPrice = 0;//最终成本价
                                    PublishDetails.forEach(function(each){
                                        SalePrice += each.SalePrice;
                                        CostReferencePrice += each.CostReferencePrice;
                                        FinalCostPrice += each.FinalCostPrice;
                                        if(each.EnableOriginPrice == true){
                                            OriginalReferencePrice += each.OriginalReferencePrice;
                                        }else{
                                            OriginalReferencePrice += 0;
                                        }
                                    })
                                    var cur_tab = $('.to_b_list_box').find('.to_b_list').eq(i);
                                    cur_tab.find('.SalePrice').text(formatMoney(SalePrice,2));
                                    cur_tab.find('.CostReferencePrice').text(formatMoney(CostReferencePrice,2));
                                    cur_tab.find('.FinalCostPrice').text(formatMoney(FinalCostPrice,2));
                                    cur_tab.find('.OriginalReferencePrice').text(formatMoney(OriginalReferencePrice,2));
                                })

                                DetailOfOrder.ModifyAmount();//金钱重新计算
                                ChangeParameter();//重新更新后台传入的参数
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

            //删除广告位数据
            $('.delete_position').on('click',function () {
                var that = $(this);
                var channel_len = that.parents('.to_b_list_box .to_b_list').find('.Tr').length;
                var tb_idx = that.parents('.to_b_list').index();
                var idx = that.parents('.Tr').index()-1;

                //console.log(tb_idx,idx)

                if(channel_len <= 1){
                    layer.msg('至少要保留一个广告位',{'time':1000});
                }else{
                    layer.confirm('确认要删除数据吗', {
                        btn: ['确认','取消'] //按钮
                    }, function(){
                        layer.closeAll();
                        that.parents('tr').remove();

                        DetailOfOrder.ModifyAmount();//从新修改价格
                        config.AreaInfos[tb_idx].PublishDetails.splice(idx,1);

                        ChangeParameter();//重新更新后台传入的参数

                        //计算每一个地区的价格的总计
                        //config.AreaInfos.forEach(function(item,i){
                            //var PublishDetails = item.PublishDetails;
                            var SalePrice = 0;//销售
                            var OriginalReferencePrice = 0;//原创
                            var CostReferencePrice = 0;//成本
                            var FinalCostPrice = 0;//最终成本价
                            config.AreaInfos[tb_idx].PublishDetails.forEach(function(each){
                                SalePrice += each.SalePrice;
                                CostReferencePrice += each.CostReferencePrice;
                                FinalCostPrice += each.FinalCostPrice;
                                if(each.EnableOriginPrice == true){
                                    OriginalReferencePrice += each.OriginalReferencePrice;
                                }else{
                                    OriginalReferencePrice += 0;
                                }
                            })
                            var cur_tab = $('.to_b_list_box').find('.to_b_list').eq(tb_idx);
                            cur_tab.find('.SalePrice').text(formatMoney(SalePrice,2));
                            cur_tab.find('.CostReferencePrice').text(formatMoney(CostReferencePrice,2));
                            cur_tab.find('.FinalCostPrice').text(formatMoney(FinalCostPrice,2));
                            cur_tab.find('.OriginalReferencePrice').text(formatMoney(OriginalReferencePrice,2));
                        })
                    //})
                }
            })
        },
        AddPosition : function () {//添加广告位
            //选择广告
            $('#AddPositionBtn').off('click').on('click',function () {
                var that = $(this);
                var _width = document.documentElement.clientWidth;
                var _height = document.documentElement.clientHeight;

                $('.AddPosition_box').show();
                $('.AddPosition_box .layer').show();

                var layer_height = $('.AddPosition_box .layer').height();
                var _left = (_width-550)/2;
                var _top = (_height-layer_height)/2;

                $('.AddPosition_box').css({'width':_width,'height':_height,'position':'fixed','left':0,'top':0,'background':'rgba(0,0,0,0.7)'});
                $('.AddPosition_box .layer').css({'position':'absolute','left':_left,'top':_top});

                //查询广告弹层里面的省份城市和预计投放日期
                var ExpectedDate = config.ADOrderInfo.LaunchTime.substr(0,10);
                var ProvinceCityArr = [];//省份或者城市的空数组
                config.AreaInfos.forEach(function (item) {
                    var obj = {
                        ProvinceID : item.ProvinceID,
                        ProvinceName : item.ProvinceName,
                        CityID : item.CityID,
                        CityName : item.CityName
                    }
                    ProvinceCityArr.push(obj);
                })

                //预计投放日期
                $('#ExpectedDate').val(ExpectedDate);//默认值
                $('#ExpectedDate').off('click').on('click', function () {
                    laydate({
                        fixed: false,
                        elem: '#ExpectedDate',
                        choose: function (date) {}
                    });
                });
                //省份城市
                var str = '';
                for(var i=0;i<=ProvinceCityArr.length-1;i++){
                    var ProvinceCityID = ProvinceCityArr[i].ProvinceID +''+ ProvinceCityArr[i].CityID;
                    str += '<option ProvinceID='+ProvinceCityArr[i].ProvinceID+' CityID='+ProvinceCityArr[i].CityID+' ProvinceCityID='+ProvinceCityID+'>'+ProvinceCityArr[i].ProvinceName +" " + ProvinceCityArr[i].CityName+'</option>';
                }
                $('#ProvinceCity').html(str);
            })

            //广告位查询
            $('#select_adposition').on('click',function () {
                var ProvinceID = $('#ProvinceCity option:checked').attr('ProvinceID');
                var CityID = $('#ProvinceCity option:checked').attr('CityID');
                var LaunchTime = $('#ExpectedDate').val();
                var MediaType = 14001;
                var MeidiaNumbers = [];
                $('.to_b_list_box .Tr').each(function () {
                    MeidiaNumbers.push($(this).attr('MediaNumber'));
                });
                MeidiaNumbers = MeidiaNumbers.join(',');
                var url = 'http://www.chitunion.com/api/ADOrderInfo/IntelligenceRecommend_PubQuery?v=1_1';
                setAjax({
                    url : url,
                    type : 'get',
                    data : {
                        ProvinceID : ProvinceID,
                        CityID : CityID,
                        LaunchTime : LaunchTime,
                        //MediaType : MediaType,
                        MeidiaNumbers : MeidiaNumbers
                    }
                },function (data) {
                    var Result = data.Result;
                    if(data.Status == 0){
                        $('#ChoosePositionContent').html(ejs.render($('#choose-position').html(),data));

                        //添加广告位到table里面
                        $('.AddEachPositionToTable').off('click').on('click',function () {
                            var that = $(this).parents('tr');
                            var ProvinceID = $('#ProvinceCity option:checked').attr('ProvinceCityID');

                            //是否应该原创参考价的复选框
                            var EnableOriginPrice = that.find('.IsUseOrigin').prop('checked');

                            //找到外面的对应的城市所在的位置
                            var ProvinceIDArr = [];
                            $('.to_b_list_box .to_b_list').each(function () {
                                ProvinceIDArr.push($(this).attr('ProvinceCityID'));
                            })
                            if(ProvinceIDArr.indexOf(ProvinceID) != -1){
                                var idx = ProvinceIDArr.indexOf(ProvinceID);
                                var obj = {
                                    HeadIconURL : that.attr('HeadIconURL'),
                                    MediaName : that.attr('MediaName'),
                                    MediaNumber : that.attr('MediaNumber'),
                                    ADPosition : that.attr('ADPosition'),
                                    CreateType : that.attr('CreateType'),
                                    LaunchTime : LaunchTime,
                                    ADLaunchDays : that.attr('ADLaunchDays'),
                                    SalePrice : that.attr('SalePrice')*1,
                                    CostReferencePrice : that.attr('CostReferencePrice')*1,
                                    ChannelName : that.attr('ChannelName'),
                                    PublishDetailID : that.attr('PublishDetailID'),
                                    MediaID : that.attr('MediaID'),
                                    ChannelID : that.attr('ChannelID'),
                                    ADPositionID : that.attr('ADPositionID'),
                                    CreateTypeID : that.attr('CreateTypeID'),
                                    MediaType : 14001,
                                    EnableOriginPrice : EnableOriginPrice,
                                    CostPrice : that.attr('CostPrice')*1,
                                    FinalCostPrice : that.attr('FinalCostPrice')*1
                                }

                                if(EnableOriginPrice == false){//原创参考价为0
                                    obj.OriginalReferencePrice = 0;  
                                }else{//应用原创参考价
                                    obj.OriginalReferencePrice = that.attr('OriginalReferencePrice');
                                }
                                config.AreaInfos[idx].PublishDetails.push(obj);
                                var alreday_trlen = $('.position_tab').eq(idx).find('tr').length-2;
                                //console.log(alreday_trlen);
                                $('.position_tab').eq(idx).find('tbody tr:eq('+alreday_trlen+')').after(ejs.render($('#FeedDataPosition').html(),obj));

                                //计算每一个地区的价格的总计
                                config.AreaInfos.forEach(function(item,i){
                                    var PublishDetails = item.PublishDetails;
                                    var SalePrice = 0;//销售
                                    var OriginalReferencePrice = 0;//原创
                                    var CostReferencePrice = 0;//成本
                                    var FinalCostPrice = 0;//最终成本价
                                    PublishDetails.forEach(function(each){
                                        SalePrice += each.SalePrice;
                                        CostReferencePrice += each.CostReferencePrice;
                                        FinalCostPrice += each.FinalCostPrice;
                                        if(each.EnableOriginPrice == true){
                                            OriginalReferencePrice += each.OriginalReferencePrice;
                                        }else{
                                            OriginalReferencePrice += 0;
                                        }
                                    })
                                    var cur_tab = $('.to_b_list_box').find('.to_b_list').eq(i);
                                    cur_tab.find('.SalePrice').text(formatMoney(SalePrice,2));
                                    cur_tab.find('.CostReferencePrice').text(formatMoney(CostReferencePrice,2));
                                    cur_tab.find('.FinalCostPrice').text(formatMoney(FinalCostPrice,2));
                                    cur_tab.find('.OriginalReferencePrice').text(formatMoney(OriginalReferencePrice,2));
                                })

                                var SalePrice = 0;//销售
                                var OriginalReferencePrice = 0;//原创
                                var CostReferencePrice = 0;//成本
                                var FinalCostPrice = 0;//最终成本价
                                config.AreaInfos[idx].PublishDetails.forEach(function(each){
                                    SalePrice += each.SalePrice;
                                    CostReferencePrice += each.CostReferencePrice;
                                    FinalCostPrice += each.FinalCostPrice;
                                    if(each.EnableOriginPrice == true){
                                        OriginalReferencePrice += each.OriginalReferencePrice;
                                    }else{
                                        OriginalReferencePrice += 0;
                                    }
                                })
                                var cur_tab = $('.to_b_list_box').find('.to_b_list').eq(idx);
                                cur_tab.find('.SalePrice').text(formatMoney(SalePrice,2));
                                cur_tab.find('.CostReferencePrice').text(formatMoney(CostReferencePrice,2));
                                cur_tab.find('.FinalCostPrice').text(formatMoney(FinalCostPrice,2));
                                cur_tab.find('.OriginalReferencePrice').text(formatMoney(OriginalReferencePrice,2));

                                DetailOfOrder.returnStateText(config.ADOrderInfo.Status);//状态
                                DetailOfOrder.ModifyAmount();//重新计算价钱
                                DetailOfOrder.PositionMessage();//添加渠道
                                ChangeParameter();//重新更新后台传入的参数
                            }
                        })
                    }else{
                        layer.msg(data.Message,{'time':1000});
                    }
                })
            })

            //关闭弹层
            $('#closebt2').on('click',function () {
                $('.AddPosition_box').hide();
                $('.AddPosition_box .layer').hide();
                $('.AddPosition_box').find('.position_table').html('');
                DetailOfOrder.AddPosition();
            })
        },
        uploadFileAll : function () {//上传
            //for(var i=0;i<=config.MediaOrderInfos.length-1;i++){
                uploadFile('UploadInfo');
            //}
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
                    $('.orderStatus').text('已完成');
                    break;
                default:
                    break;
            }
        },
        ModifyAmount : function () {//计算广告位个数和金额

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
            //销售参考价总计
            $('.SalePriceTotal').text(formatMoney(SalePrice+OriginalReferencePrice,2));
            //成本参考价总计
            $('.CostReferencePriceTotal').text(formatMoney(CostReferencePrice+OriginalReferencePrice,2));
        },
        VerificationInformation : function () {

            ChangeParameter();

            //营销政策
            $('.FileInDemand').on('blur',function () {
                var FileInDemand = $(this).val();
                $(this).parents('.demand').attr('MarketingPolices',FileInDemand);
                if(FileInDemand == ''){
                    $(this).next().show();
                }else{
                    $(this).next().hide();
                }
            })

            //驳回
            $('#auditReject').on('change',function(){
                if($('#auditReject').prop('checked')) {//驳回
                    $("#RejectedMsg").show();
                }else{
                    $("#RejectedMsg").hide();
                }
            })
            //通过
            $('#auditPass').on('change',function(){
                if($('#auditReject').prop('checked')) {//驳回
                    $("#RejectedMsg").show();
                }else{
                    $("#RejectedMsg").hide();
                }
            })


            //点击提交的时候
            $('#lastResult').off('click').on('click',function (event) {
                event.preventDefault();
                if($('.demand').attr('MarketingPolices') == ''){
                    layer.msg('请补充营销政策',{'time':1000});
                }else if(!$('#auditPass').prop('checked') && !$('#auditReject').prop('checked')){
                    layer.msg('请选择通过或者驳回',{'time':1000});
                }else if($('#auditPass').prop('checked')){//通过
                    var url = 'http://www.chitunion.com/api/ADOrderInfo/IntelligenceADOrderInfoCrud?v=1_1';
                    setAjax({
                        url : url,
                        type : 'post',
                        data : {
                            optType : 2,
                            ADOrderInfo : {
                                OrderID : config.ADOrderInfo.OrderID,
                                OrderName : config.ADOrderInfo.OrderName,
                                Status : 16003,
                                CustomerID : config.ADOrderInfo.CustomerID,
                                MarketingPolices : $('.demand').attr('MarketingPolices'),//政策可能会被改写
                                UploadFileURL : $('.demand').attr('UploadFileURL'),//上传资料也可能被改写
                                LaunchTime : config.ADOrderInfo.LaunchTime,
                                CRMCustomerID :config.ADOrderInfo.CRMCustomerID,
                                CustomerText : config.ADOrderInfo.CustomerText,
                                BudgetTotal : config.ADOrderInfo.BudgetTotal,
                                OrderRemark : config.ADOrderInfo.OrderRemark,
                                MasterID : config.ADOrderInfo.MasterID,
                                BrandID : config.ADOrderInfo.BrandID,
                                SerialID : config.ADOrderInfo.SerialID,
                                MasterName : config.ADOrderInfo.MasterName,
                                BrandName : config.ADOrderInfo.BrandName,
                                SerialName : config.ADOrderInfo.SerialName,
                                JKEntrance : config.ADOrderInfo.JKEntrance,
                                Note : config.ADOrderInfo.Note//添加需求
                            },
                            ADDetails : ADDetails
                        }
                    },function (data) {
                        if(data.Status == 0){
                            DetailOfOrder.PassOreject();
                            window.location = '/ordermanager/listofproject.html';
                        }else{
                            layer.msg(data.Message,{'time':1000});
                        }
                    })
                }else if($('#auditReject').prop('checked')) {//驳回

                    var resultReson = $('#RejectedMsg').val();
                    var url = '/api/ADOrderInfo/Review_ADOrderInfo';
                    $("#RejectedMsg").on('change',function () {
                        resultReson = $('#RejectedMsg').val();
                    })
                    $('#lastResult').off('click').on('click',function () {
                        if(resultReson == ''){
                            layer.msg('请填写驳回原因',{'time':1000});
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
                                    //$.closePopupLayer('popLayerDemo');
                                    window.location = '/ordermanager/listofproject.html';
                                }else{
                                    layer.msg(data.Message,{'time':1000});
                                }
                            })
                        }
                    })
                    /*$.openPopupLayer({
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
                                    layer.msg('请填写驳回原因',{'time':1000});
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
                                            layer.msg(data.Message,{'time':1000});
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
                    })*/
                }
            })
        },
        PassOreject : function () {//点击审核通过的时候  还得掉审核通过的接口告诉接口已经审核通过
            var url = 'http://www.chitunion.com/api/ADOrderInfo/Review_ADOrderInfo';
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
        }
    };

    DetailOfOrder.init();
    
    function ChangeParameter() {
        var newADDetails = [];
        $('.to_b_list_box .to_b_list').each(function () {
            var that = $(this);
            var OriginContain = true;
            if(that.attr('OriginContain') == 'true'){
                OriginContain = true;
            }else if(that.attr('OriginContain') == 'false'){
                OriginContain = false;
            }
            var obj1 = {
                ProvinceID : that.attr('ProvinceID')*1,
                CityID : that.attr('CityID')*1,
                Budget : that.attr('Budget')*1,
                MediaCount : that.attr('MediaCount')*1,
                OriginContain : OriginContain,
                PublishDetails : []
            };
            that.find('.Tr').each(function () {
                var _that = $(this);
                var EnableOriginPrice = true;
                if(_that.attr('EnableOriginPrice') == 'true'){
                    EnableOriginPrice = true;
                }else if(_that.attr('EnableOriginPrice') == 'false'){
                    EnableOriginPrice = false;
                }
                var obj2 = {
                    PublishDetailID : _that.attr('PublishDetailID')*1,
                    MediaType : 14001,
                    MediaID : _that.attr('MediaID')*1,
                    AdjustPrice : _that.attr('AdjustPrice')*1,
                    EnableOriginPrice : EnableOriginPrice,
                    ChannelID : _that.attr('ChannelID')*1,
                    LaunchTime : _that.attr('LaunchTime'),
                    CostPrice : _that.attr('CostPrice')*1,
                    FinalCostPrice : _that.attr('FinalCostPrice')*1,
                    CostReferencePrice : _that.attr('CostReferencePrice')
                }
                obj1.PublishDetails.push(obj2);
            })
            newADDetails.push(obj1);
        })
        ADDetails = newADDetails;
        //console.log(ADDetails);
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
                    $('.demand').attr('UploadFileURL',json.Msg);
                    $("#"+id).parent().next().show();
                    $("#"+id).parent().next().next().show();
                    $("#"+id).parent().next().text(json.FileName);
                    $("#"+id).parent().next().next().find('a').attr('href',json.Msg);
                    //$("#"+id).parents('.satements').find('.uploadFile').attr("href",json.Msg);
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