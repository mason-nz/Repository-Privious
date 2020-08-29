$(function () {
// 获取url？后面的参数
    function GetRequest() {
        var url = location.search; //获取url中"?"符后的字串
        var theRequest = new Object();
        if (url.indexOf("?") != -1) {
            var str = url.substr(1);
            strs = str.split("&");
            for (var i = 0; i < strs.length; i++) {
                theRequest[strs[i].split("=")[0]] = decodeURI(strs[i].split("=")[1]);
            }
        }
        return theRequest;
    }
    // 场景
    // $.ajax({
    //     url: public_url+'/api/chitu/GetChannel',
    //     type: 'get',
    //     data: {},
    //     dataType: 'json',
    //     xhrFields: {
    //         withCredentials: true
    //     },
    //     crossDomain: true,
    //     async:false,
    //     success: function (data) {
    //         if(data.Status==0){
    //             var option=''
    //             data.Result.forEach(function (e) {
    //                 option+='<option value="'+e.ChannelID+'">'+e.ChannelName+'</option>'
    //             })
    //             $('#OrderChannel').html(option)
    //         }else {
    //             layer.msg(data.Message)
    //         }
    //     }
    // })
    function IncomeManagement() {
        this.init();
    }

    IncomeManagement.prototype = {
        constructor: IncomeManagement,
        // 获取条件
        ObtainConditions: function (i) {
            // 开始时间
            var CreateTime = $('#CreateTime').val();
            // 结束时间
            var endTime = $('#endTime').val();
            // 订单渠道
            var OrderChannel = $('#OrderChannel option:selected').val();
            // 结算状态
            var OrderType = $('#OrderType option:selected').val();
            // 订单id
            var OrderId=$('#OrderId').val();
            // 任务id
            var TaskId=$('#TaskId').val();
            // 物料id
            var MaterielId=$('#MaterielId').val()
            // 媒体主
            var UserName=$('#UserName').val()
            var obj={
                StartDate:CreateTime,
                EndDate:endTime,
                ChannelId:OrderChannel,
                OrderType:OrderType,
                OrderId:OrderId,
                TaskId:TaskId,
                MaterielId:MaterielId,
                UserName:UserName,
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
            if(GetRequest().TrueName){
                $('#UserName').val(GetRequest().TrueName)
            }
            $('#Inquiry').click()
        },
        query:function (parameter) {
            setAjax({
                url:public_url+'/api/order/GetIncomeList',
                // url:'json/IncomeManagement.json',
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
                                        url: public_url+'/api/order/GetIncomeList',
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
                    if(data.Result.Extend){
                        $('#Cumulative').text('累计金额：'+removePoint0(formatMoney(data.Result.Extend.TotalMoney,2,''),2)+'元')
                    }else {
                        $('#Cumulative').text('')
                    }
                }else {
                    layer.msg(data.Message)
                }
            })
        }
    }
    new IncomeManagement()
})