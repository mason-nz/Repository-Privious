/*
* Written by:     fengb
* function:       订单管理-全网域订单列表
* Created Date:   2017-12-18
*/

$(function () {

	var OrderId = GetQueryString('OrderId')!=null&&GetQueryString('OrderId')!='undefined'?GetQueryString('OrderId'):null;

    function OrderDetail() {
    	this.init();//查询
    }

    console.log(CTLogin);
   
    OrderDetail.prototype = {
        constructor: OrderDetail,
        init: function (obj) {// 请求数据
            var _this = this;
            //基本信息
            $.ajax({
                url: public_url + '/api/order/GetInfo',
                type: 'get',
                xhrFields: {
                    withCredentials: true
                },
                crossDomain: true,
                data: {
                    OrderId : OrderId
                },
                async : false,
                beforeSend: function(){
                    $('#listLoading').html('<img src="/images/loading.gif" style="display: block;margin: 30px auto;">');
                },
                success : function (data) {
                    if(data.Status == 0){
                        $('#listLoading').html('');
                        var Result = data.Result;
                        $('.basic_info').html(ejs.render($('#order-info').html(), Result));
                    }else{
                        layer.msg(data.Message,{'time':2000});
                    }
                }
            })

            //列表
            $.ajax({
                url: public_url + '/api/order/GetIncomeDetails',
                type: 'get',
                xhrFields: {
		            withCredentials: true
		        },
		        crossDomain: true,
                data: {
                    OrderId : OrderId
                },
                async : false,
                beforeSend: function(){
                    $('#listLoading').html('<img src="/images/loading.gif" style="display: block;margin: 30px auto;">');
                },
                success : function (data) {
                	if(data.Status == 0){
                		$('#listLoading').html('');
	                    var Result = data.Result;
	                    if(Result.List.length > 0) {
                            var TotalMoney = Result.Extend.TotalMoney;
                            $('.last_money').show();
                            $('.OrderList').html(ejs.render($('#order-list').html(), Result));
                            $('.last_money .totalmoney').html(TotalMoney + '元');
	                    }else{
	                        $('#listLoading').html('<img src="/images/no_data.png" style="display: block;margin: 30px auto;">');
                            $('.last_money').hide();
	                    }
                	}else{
                		layer.msg(data.Message,{'time':2000});
                	}
                }
            })
            _this.operation();
        },
        operation: function () {
            // 订单图片：鼠标移入时可下载
            $('.display_img').off('mouseover').on('mouseover',function(){
                $(this).find('.download_img').show();
            }).off('mouseout').on('mouseout',function(){
                $(this).find('.download_img').hide();
            })
        }
    }
    var OrderDetail = new OrderDetail();
})