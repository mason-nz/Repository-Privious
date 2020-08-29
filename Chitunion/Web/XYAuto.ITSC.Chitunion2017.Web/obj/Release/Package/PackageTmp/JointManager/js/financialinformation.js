$(function () {
    function financialinformation(obj) {
        this.Rendering(obj);
    }
    financialinformation.prototype={
        constructor: financialinformation,
        Rendering:function (obj) {
            var _this=this;
            $('.but_query').off('click').on('click',function () {
                setAjax({
                    url:obj.url,
                    type:'get',
                    data:_this.parameter(obj.val,1)
                },function (data) {
                    console.log(data);
                    $('#tablelist-a').html(ejs.render($('#list'+obj.val).html(), data));
                    _this.operation()
                    // 如果数据为0显示图片
                    if (data.Result.TotalCount != 0) {
                        //分页
                        $("#pageContainer").pagination(
                            data.Result.TotalCount,
                            {
                                items_per_page: 20, //每页显示多少条记录（默认为20条）
                                callback: function (currPage, jg) {
                                    setAjax({
                                        url: obj.url,
                                        type: 'get',
                                        data: _this.parameter(obj.val,currPage)
                                    }, function (data) {
                                        $('#tablelist-a').html(ejs.render($('#list'+obj.val).html(), data));
                                        _this.operation()
                                    })
                                }
                            });
                    } else {
                        $('#pageContainer').html('<img src="/images/no_data.png" style="display: block;margin: 70px auto;">');
                    }
                })
            })
            $('.but_query').click()
        },
        parameter:function (val,i) {
            if(val==1){
                var yonghuming=$('#yonghuming input').val();
                var gdtdanhao=$('#gdtdanhao input').val();
                var zjzhleixing=$('#zjzhleixing select option:selected').val();
                var jyleixing=$('#jyleixing select option:selected').val();
                var createDate=$('#createDate').val();
                var createDate1=$('#createDate1').val();
                return {
                    UserName:yonghuming,
                    GdtNum:gdtdanhao,
                    AccountType:zjzhleixing,
                    TradeType:jyleixing,
                    StarDate:createDate,
                    EndDate:createDate1,
                    PageIndex:i,
                    pageSize:20
                }
            }else if(val==2){
                var zjzhleixing=$('#zjzhleixing select option:selected').val();
                var jyleixing=$('#jyleixing select option:selected').val();
                var createDate=$('#createDate').val();
                var createDate1=$('#createDate1').val();
                return {
                    AccountType:zjzhleixing,
                    TradeType:jyleixing,
                    StarDate:createDate,
                    EndDate:createDate1,
                    PageIndex:i,
                    pageSize:20
                }
            }else if(val==3){
                var xqdingdanhao=$('#xqdingdanhao input').val();
                var czdanhao=$('#czdanhao input').val();
                var gdtdanhao=$('#gdtdanhao input').val();
                var czyonghu=$('#czyonghu input').val();
                var zhuangtai1=$('#zhuangtai1 select option:selected').val();
                return {
                    RechargeNumber:czdanhao,
                    DemandBillNo:xqdingdanhao,
                    ExternalBillNo:gdtdanhao,
                    UserName:czyonghu,
                    RechargeStatus:zhuangtai1,
                    PageIndex:i,
                    pageSize:20
                }
            }else {
                var yonghuming=$('#yonghuming input').val();
                var xqdingdanhao=$('#xqdingdanhao input').val();
                var czdanhao=$('#czdanhao input').val();
                var gdtdanhao=$('#gdtdanhao input').val();
                var zhuangtai2=$('#zhuangtai2 select option:selected').val();
                return {
                    RechargeNumber:czdanhao,
                    DemandBillNo:xqdingdanhao,
                    ExternalBillNo:gdtdanhao,
                    UserName:yonghuming,
                    HandleStatus:zhuangtai2,
                    PageIndex:i,
                    pageSize:20
                }
            }
        },
        operation:function () {
            $('.Recharge').off('click').on('click',function () {
                var RechargeNumber=$(this).attr('RechargeNumber');
                var DemandBillNo=$(this).attr('DemandBillNo');
                var UserName=$(this).attr('UserName');
                var RechargeAmount=$(this).attr('RechargeAmount');
                location.hash='#'+DemandBillNo;
                $.openPopupLayer({
                    name: "Recharge",
                    url: "Recharge.html",
                    error: function (dd) {
                        alert(dd.status);
                    },
                    success: function (data) {
                        $('#Username3').html(RechargeNumber)
                        $('#Username2').html(DemandBillNo)

                        $('#Corporatename2').html(UserName)

                        $('#phone12').html(formatMoney(RechargeAmount,2,'¥ ')).attr('RechargeAmount',RechargeAmount)

                        $('#closebt1').off('click').on('click',function () {
                            $.closePopupLayer('Recharge')
                        })
                        // 点击提交
                        $('#submitMessage1').off('click').on('click',function () {
                            var arr=[],nub=0;

                            $('.ChooseChannel input').each(function () {
                               if($(this).prop('checked')){
                                   arr.push($(this).attr('GdtNum'));
                                   nub+=($(this).attr('Amount')-0)
                               }
                            })
                            if(arr.length==0||$('#phone12').attr('RechargeAmount')!=nub){
                                $('#czje').show();
                                return false;
                            }else {
                                $('#czje').hide();
                            }
                            setAjax({
                                url:'http://www.chitunion.com/api/ZhyInfo/BingdingTradeRelation',
                                type:'post',
                                data: {
                                    TradeType:84001,
                                    DemandBillNo:DemandBillNo,
                                    GdtNumList:arr
                                }
                            },function (data) {
                                if(data.Status==0){
                                    $.closePopupLayer('Recharge');
                                    $('.but_query').click()
                                }else {
                                    layer.msg(data.Message)
                                }
                            })
                        })
                    }
                });
            });

            $('.Backrow').off('click').on('click',function () {
                var DemandBillNo=$(this).attr('DemandBillNo');
                var UserName=$(this).attr('UserName');
                var DemandBackAmount=$(this).attr('DemandBackAmount');
                location.hash='#'+DemandBillNo;
                $.openPopupLayer({
                    name: "Backrow",
                    url: "Backrow.html",
                    error: function (dd) {
                        alert(dd.status);
                    },
                    success: function (data) {
                        $('#Username1').html(DemandBillNo);
                        $('#Corporatename1').html(UserName);
                        $('#phone11').html(formatMoney(DemandBackAmount,2,'¥ ')).attr('DemandBackAmount',DemandBackAmount)
                        $('#closebt').off('click').on('click',function () {
                            $.closePopupLayer('Backrow')
                        })
                        // 点击提交
                        $('#submitMessage2').off('click').on('click',function () {
                            var arr=[],nub=0;

                            $('.ChooseChannel1 input').each(function () {
                                if($(this).prop('checked')){
                                    arr.push($(this).attr('GdtNum'));
                                    nub+=($(this).attr('Amount')-0)
                                }
                            })
                            if(arr.length==0||$('#phone11').attr('DemandBackAmount')!=nub){
                                $('#czje1').show();
                                return false;
                            }else {
                                $('#czje1').hide();
                            }
                            setAjax({
                                url:'http://www.chitunion.com/api/ZhyInfo/BingdingTradeRelation',
                                type:'post',
                                data: {
                                    TradeType:84003,
                                    DemandBillNo:DemandBillNo,
                                    GdtNumList:arr
                                }
                            },function (data) {
                                if(data.Status==0){
                                    $.closePopupLayer('Backrow');
                                    $('.but_query').click()
                                }else {
                                    layer.msg(data.Message)
                                }
                            })
                        })
                    }
                });
            });
        }
    }

    // 切换
    $('.tab_menu li').off('click').on('click', function () {
        // 初始化参数
        $('#yonghuming input').val('');//用户名
        $('#xqdingdanhao input').val('');//需求订单号
        $('#czdanhao input').val('');//充值单号
        $('#gdtdanhao input').val('');//GDT单号
        $('#czyonghu input').val('');//充值用户
        $('#zhuangtai1 select option').eq(0).prop("selected",true);//状态-充值对账
        $('#zhuangtai2 select option').eq(0).prop("selected",true);//状态-资金回划
        $('#zjzhleixing select option').eq(0).prop("selected",true);//资金账户类型
        $('#jyleixing select option').eq(0).prop("selected",true);//交易类型
        $('#createDate').val('');//提交时间
        $('#createDate1').val('');//提交时间
        console.log(1);
        $(this).addClass('selected').siblings('li').removeClass('selected');
        var val=$(this).val();
        var url='';
        switch(val){
            case 1:
                url='http://www.chitunion.com/api/ZhyInfo/SelectGdtFlowDetail';
                $('#yonghuming').show();//用户名
                $('#xqdingdanhao').hide();//需求订单号
                $('#czdanhao').hide();//充值单号
                $('#gdtdanhao').show();//GDT单号
                $('#czyonghu').hide();//充值用户
                $('#zhuangtai1').hide();//状态-充值对账
                $('#zhuangtai2').hide();//状态-资金回划
                $('#zjzhleixing').show();//资金账户类型
                $('#jyleixing').show();//交易类型
                $('#tijiaosj').show();//提交时间
                break;
            case 2:
                url='http://www.chitunion.com/api/ZhyInfo/SelectGdtDateSummaryInfo';
                $('#yonghuming').hide();//用户名
                $('#xqdingdanhao').hide();//需求订单号
                $('#czdanhao').hide();//充值单号
                $('#gdtdanhao').hide();//GDT单号
                $('#czyonghu').hide();//充值用户
                $('#zhuangtai1').hide();//状态-充值对账
                $('#zhuangtai2').hide();//状态-资金回划
                $('#zjzhleixing').show();//资金账户类型
                $('#jyleixing').show();//交易类型
                $('#tijiaosj').show();//提交时间
                break;
            case 3:
                url='http://www.chitunion.com/api/ZhyInfo/SelectGdtRechargeInfo';
                $('#yonghuming').hide();//用户名
                $('#xqdingdanhao').show();//需求订单号
                $('#czdanhao').show();//充值单号
                $('#gdtdanhao').show();//GDT单号
                $('#czyonghu').show();//充值用户
                $('#zhuangtai1').show();//状态-充值对账
                $('#zhuangtai2').hide();//状态-资金回划
                $('#zjzhleixing').hide();//资金账户类型
                $('#jyleixing').hide();//交易类型
                $('#tijiaosj').hide();//提交时间
                break;
            case 4:
                url='http://www.chitunion.com/api/ZhyInfo/SelectBackAmountInfo';
                $('#yonghuming').show();//用户名
                $('#xqdingdanhao').show();//需求订单号
                $('#czdanhao').show();//充值单号
                $('#gdtdanhao').show();//GDT单号
                $('#czyonghu').hide();//充值用户
                $('#zhuangtai1').hide();//状态-充值对账
                $('#zhuangtai2').show();//状态-资金回划
                $('#zjzhleixing').hide();//资金账户类型
                $('#jyleixing').hide();//交易类型
                $('#tijiaosj').hide();//提交时间
                break;
        }
        var obj={
            url:url,
            val:val
        }
        new financialinformation(obj);
    })
    $('.tab_menu li').eq(0).click()
})