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
                $('#Finished .OrderChannel').html(option);
                $('#UnderWay .OrderChannel').html(option);
            }else {
                layer.msg(data.Message)
            }
        }
    })
    function ChannelOrder() {
        this.init()
    }
    ChannelOrder.prototype={
        constructor:ChannelOrder,
        // 获取条件
        ObtainConditions:function (i) {
            var CreateTime,endTime,OrderChannel,OrderType,OrderId,TaskId;
            if($('.selected').attr('value')==1){
                // 进行中start
                // 开始时间
                CreateTime = $('#CreateTime').val();
                // 结束时间
                endTime = $('#endTime').val();
                // 订单渠道
                OrderChannel = $('#UnderWay .OrderChannel option:selected').val();
                // 订单类型
                OrderType = $('#UnderWay .OrderType option:selected').val();
                // 订单id
                OrderId= $('#UnderWay .OrderId').val();
                // 任务id
                TaskId= $('#UnderWay .TaskId').val();
                // 进行中end
            }else {
                // 已结束start
                // 开始时间
                CreateTime = $('#CreateTime1').val();
                // 结束时间
                endTime = $('#endTime1').val();
                // 订单渠道
                OrderChannel = $('#Finished .OrderChannel option:selected').val();
                // 订单类型
                OrderType = $('#Finished .OrderType option:selected').val();
                // 订单id
                OrderId= $('#Finished .OrderId').val();
                // 任务id
                TaskId= $('#Finished .TaskId').val();
                // 已结束end
            }
            var obj={
                Status:$('.selected').attr('status'),
                BeginTime:CreateTime,
                EndTime:endTime,
                ChannelID:OrderChannel,
                OrderType:OrderType,
                OrderID:OrderId,
                TaskID:TaskId,
                PageSize:20
            }
            if(i!=undefined){
                obj.PageIndex=i
            }
            return obj
        },
        init:function () {
            var _this=this;
            // 点击查询
            $('#Inquiry').off('click').on('click',function () {
                _this.query(_this.ObtainConditions(1),_this)
            })
            // 切换
            $('.tab_menu li').off('click').on('click',function () {
                $(this).attr('class','selected').siblings().attr('class','');
                if($(this).attr('value')==1){
                    $('#UnderWay').show();
                    $('#Finished').hide()
                }else {
                    $('#Finished').show();
                    $('#UnderWay').hide()
                }
                $('#Inquiry').click();
            })
            // 导出数据
            $('#ExportData').off('click').on('click',function () {
                _this.ExportData(_this.ObtainConditions())
            });
            if(GetRequest().finish==1){
                $('#Finished .OrderChannel option').each(function () {
                    if($(this).val()==GetRequest().ChannelID){
                        $(this).prop('selected',true)
                    }
                })
                $('#CreateTime1').val(GetRequest().start)
                $('#endTime1').val(GetRequest().end);
                $('.tab_menu li').eq(1).click();
            }else {
                $('#Inquiry').click();
            }
        },
        query:function (parameter,_this) {
            setAjax({
                url:public_url+'/api/chitu/Order',
                // url:'json/Order.json',
                type:'get',
                data:parameter
            },function (data) {
                if(data.Status==0){
                    $('.channel').html(ejs.render($('#channel').html(), data));
                    _this.operation()
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
                                        url: public_url+'/api/chitu/Order',
                                        type: "GET",
                                        data: parameter
                                    }, function (data) {
                                        $('.channel').html(ejs.render($('#channel').html(), data));
                                        _this.operation()
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
                url:public_url+'/api/chitu/OrderExcel',
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
        operation:function () {
          $('.Order_Id').off('click').on('click',function () {
              window.open('/DispenseManager/OrderDetails.html?orderid='+$(this).attr('orderid'))
          })
        }
    }
    new ChannelOrder()
})