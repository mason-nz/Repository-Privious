function getNextMonth(date) {
    var arr = date.split('-');
    var year = arr[0]; //获取当前日期的年份
    var month = arr[1]; //获取当前日期的月份
    var day = arr[2]; //获取当前日期的日
    var days = new Date(year, month, 0);
    days = days.getDate(); //获取当前日期中的月的天数
    var year2 = year;
    var month2 = parseInt(month) + 1;
    if (month2 == 13) {
        year2 = parseInt(year2) + 1;
        month2 = 1;
    }
    var day2 = day;
    var days2 = new Date(year2, month2, 0);
    days2 = days2.getDate();
    if (day2 > days2) {
        day2 = days2;
    }
    if (month2 < 10) {
        month2 = '0' + month2;
    }

    var t2 = year2 + '-' + month2 + '-' + day2;
    return t2;
}
$(function () {
    $.ajax({
        url: public_url+'/api/chitu/GetChannel',
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
                var option=''
                data.Result.forEach(function (e) {
                    option+='<option value="'+e.ChannelID+'">'+e.ChannelName+'</option>'
                })
                $('#OrderChannel').html(option)
            }else {
                layer.msg(data.Message)
            }
        }
    })
    function ClearingInformation() {
        this.init();
    }

    ClearingInformation.prototype = {
        constructor: ClearingInformation,
        // 获取条件
        ObtainConditions: function (i) {
            // 开始时间
            var CreateTime = $('#CreateTime').val();
            // 结束时间
            var endTime = $('#endTime').val();
            // 订单渠道
            var OrderChannel = $('#OrderChannel option:selected').val();
            // 结算状态
            var SettleAccounts = $('#SettleAccounts option:selected').val();
            var obj={
                BeginTime:CreateTime,
                EndTime:endTime,
                ChannelID:OrderChannel,
                StateOfSettlement:SettleAccounts,
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
                _this.query(_this.ObtainConditions(1))
            })
            // 导出数据
            $('#ExportData').off('click').on('click',function () {
                _this.ExportData(_this.ObtainConditions())
            })
            $('#Inquiry').click()
        },
        query:function (parameter) {
            setAjax({
                url:public_url+'/api/chitu/ChannelSummaryByMonth',
                // url:'json/ChannelSummaryByMonth.json',
                type:'get',
                data:parameter
            },function (data) {
                if(data.Status==0){
                    $('.Settlement').html(ejs.render($('#Settlement').html(), data));
                    if (data.Result.TotalCount != 0) {
                        var counts = data.Result.TotalCount;
                        //分页
                        $("#pageContainer").pagination(
                            counts,
                            {
                                items_per_page: 20, //每页显示多少条记录（默认为20条）
                                callback: function (currPage, jg) {
                                    parameter.PageIndex=currPage
                                    // ajax请求
                                    setAjax({
                                        url: public_url+'/api/chitu/ChannelSummaryByMonth',
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
                }else {
                    layer.msg(data.Message)
                }
            })
        },
        ExportData:function (parameter) {
            var i=1;
            if(i!=1){
                return false;
            }
            $.ajax({
                url:public_url+'/api/chitu/ChannelSummaryMonthExcel',
                type:'GET',
                data:parameter,
                dataType: 'json',
                xhrFields: {
                    withCredentials: true
                },
                crossDomain: true,
                success: function (data) {
                    if (data.Status==0){
                        layer.msg('成功',{time:1000},function () {
                            window.location=data.Result
                        });
                    }else {
                        layer.msg(data.Message)
                    }
                    i=1;
                },
                beforeSend: function () {
                   i=0;
                }
            });
        },
    }
    new ClearingInformation()
})