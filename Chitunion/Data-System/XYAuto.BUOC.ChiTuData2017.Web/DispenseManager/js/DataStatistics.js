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
                $('#OrderChannel').html(option);
            }else {
                layer.msg(data.Message)
            }
        }
    })

    function DataStatistics() {
        this.init();
    }
    DataStatistics.prototype={
        constructor:DataStatistics,
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
                BeginDate:CreateTime,
                EndDate:endTime,
                ChannelID:OrderChannel,
                StateOfSettlement:SettleAccounts,
                PageSize:20
            }
            if(i!=undefined){
                obj.PageIndex=i
            }
            return obj
        },
        init:function(){
            var _this=this
            // 查询
            $('#Inquiry').off('click').on('click',function () {
                _this.AddUpList(_this.ObtainConditions(1))
            })
            _this.AddUp();
            $('#Inquiry').click();
            _this.judge()
        },
        judge:function () {
            if(CTLogin.RoleIDs=='SYS004RL00018'||CTLogin.RoleIDs=='SYS004RL00027'){
                $('#OrderChannel').parent().show()
            }else {
                $('#OrderChannel').parent().hide()
            }
        },
        AddUp:function () {
            setAjax({
                url:public_url+'/api/chitu/ChannelSummary',
                // url:'json/ChannelSummary.json',
                type:'get',
                data:{}
            },function (data) {
                if(data.Status==0) {
                    $('.AddUp').html(ejs.render($('#AddUp').html(), data));
                    $('#Arrow_img').off('click').on('click',function () {
                       if($('.Tab_Show').eq(0).css('display')=='none'){
                           $(this).attr('class','')
                           $('.Tab_Show').show()
                       }else {
                           $(this).attr('class','Arrow_Icon')
                           $('.Tab_Show').hide()
                       }
                    })
                }else {
                    layer.msg(data.Message)
                }
            })
        },
        AddUpList:function (parameter) {
            setAjax({
                url:public_url+'/api/chitu/ChannelSummaryByDay',
                // url:'json/ChannelSummaryByDay.json',
                type:'get',
                data:parameter
            },function (data) {
                if(data.Status==0){
                    $('.AddUpList').html(ejs.render($('#AddUpList').html(), data));
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
                                        url: public_url+'/api/chitu/ChannelSummaryByDay',
                                        type: "GET",
                                        data: parameter
                                    }, function (data) {
                                        $('.AddUpList').html(ejs.render($('#AddUpList').html(), data));
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
    }
    new DataStatistics()
})