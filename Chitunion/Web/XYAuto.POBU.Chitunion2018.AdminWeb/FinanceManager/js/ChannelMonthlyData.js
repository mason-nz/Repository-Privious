$(function () {

    // 汇总年月
    $.ajax({
        url: public_url+'/api/order/GetStatMonthlySelect',
        type: 'get',
        data: {},
        dataType: 'json',
        xhrFields: {
            withCredentials: true
        },
        crossDomain: true,
        async:false,
        success: function (data) {
            if(data.Status==0){
                var option='<option value="">全部</option>'
                data.Result.List.forEach(function (e) {
                    option+='<option value="'+e.Date+'">'+e.Date+'</option>'
                })
                $('#SummaryYear').html(option)
            }else {
                layer.msg(data.Message)
            }
        }
    })
    function ChannelMonthlyData() {
        this.init();
    }

    ChannelMonthlyData.prototype = {
        constructor: ChannelMonthlyData,
        // 获取条件
        ObtainConditions: function (i) {
            // 订单渠道
            var OrderChannel = $('#OrderChannel option:selected').val();
            // 汇总年月
            var SummaryYear = $('#SummaryYear option:selected').val();
            // 支付状态
            var PayStatus = $('#PayStatus option:selected').val();

            var obj={
                ChannelId:OrderChannel,
                SummaryDate:SummaryYear,
                PayStatus:PayStatus,
                PageSize:20
            }
            if(i!=undefined){
                obj.PageIndex=i
            }
            return obj
        },
        init: function () {
            var _this=this
            // 查询
            $('#Inquiry').off('click').on('click',function () {
                _this.query(_this.ObtainConditions(1),_this)
            })
            $('#Inquiry').click()
        },
        query:function (parameter,_this) {
            setAjax({
                url:public_url+'/api/order/GetMonthlyList',
                // url:'json/ChannelMonthlyData.json',
                type:'get',
                data:parameter
            },function (data) {
                if(data.Status==0){
                    $('.Settlement').html(ejs.render($('#Settlement').html(), data));
                    if (data.Result.TotleCount != 0) {
                        var counts = data.Result.TotleCount;
                        //分页
                        $("#pageContainer").pagination(
                            counts,
                            {
                                items_per_page: 20, //每页显示多少条记录（默认为20条）
                                callback: function (currPage, jg) {
                                    parameter.PageIndex=currPage
                                    // ajax请求
                                    setAjax({
                                        url: public_url+'/api/order/GetMonthlyList',
                                        type: "GET",
                                        data: parameter
                                    }, function (data) {
                                        $('.Settlement').html(ejs.render($('#Settlement').html(), data));
                                    })
                                }
                            });

                    } else {
                        $('#pageContainer').html('<img src="/ImagesNew/no_data.png" style="display: block;margin: 70px auto;">')
                    }
                    $('#Cumulative').text('累计金额：'+removePoint0(formatMoney(data.Result.Extend.TotalMoney,2,''),2)+'元');
                    _this.operation()
                }else {
                    layer.msg(data.Message)
                }
            })
        },
        operation:function () {
            $('.payment').off('click').on('click',function () {
                var statisticsid=$(this).attr('statisticsid')
                layer.confirm('确认已付款吗？', function(index){
                    setAjax({
                        url:public_url+'/api/order/PayMonthly',
                        type:'post',
                        data:{
                            StatisticsId:statisticsid
                        }
                    },function (data) {
                        if (data.Status==0){
                            layer.msg('成功',{time:1000},function () {
                                location.reload()
                            })
                        }else {
                            layer.msg('失败')
                        }
                    })
                    layer.close(index);
                });
            })
        }
    }
    new ChannelMonthlyData()
})