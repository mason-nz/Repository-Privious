/*
* Written by:     fengb
* function:       订单管理-全网域订单列表
* Created Date:   2017-12-18
*/

$(function () {

    function FullOrderList() {
        //订单状态 
        $.ajax({
            url: public_url + '/api/DictInfo/GetDictInfoByTypeID',
            type: 'get',
            xhrFields: {
                withCredentials: true
            },
            crossDomain: true,
            data: {
                typeID : 193 
            },
            async : false,
            success : function (data) {
                if(data.Status == 0){
                    var Result = data.Result;
                    var str = '<option DictId="-2">全部</option>';
                    for(var i = 0;i <= Result.length - 1;i ++){
                        str += "<option DictId="+ Result[i].DictId+">"+Result[i].DictName+"</option>";
                    }
                    $('#OrderStatus').html(str);
                }else{
                    layer.msg(data.Message,{'time':2000});
                }
            }
        })
    	this.queryparameters();//查询
        $('#searchBtn').click();
    }
   
    FullOrderList.prototype = {
        constructor: FullOrderList,
        // 获取查询参数
        queryparameters: function () {
            var _this = this;
            // 搜索
            $('#searchBtn').off('click').on('click', function () {
                var OrderName = $.trim($('#OrderName').val());
                var OrderStatus = $('#OrderStatus option:checked').attr('DictId');
                //对象
                var obj = {
                    PageSize : 20,
                    PageIndex : 1,
                    OrderName : OrderName,
                    OrderStatus : OrderStatus
                }
                _this.requestdata(obj);
            })
        },
        requestdata: function (obj) {// 请求数据
            var _this = this;
            
            $.ajax({
                url: public_url + '/api/order/GetDistributeList',
                type: 'get',
                xhrFields: {
		            withCredentials: true
		        },
		        crossDomain: true,
                data: obj,
                beforeSend: function(){
                    $('#listLoading').html('<img src="/images/loading.gif" style="display: block;margin: 30px auto;">');
                },
                success : function (data) {
                	if(data.Status == 0){
                		$('#listLoading').html('');
	                    var Result = data.Result;
	                    if(Result.TotleCount > 0) {
	                    	_this.renderdata(Result, obj);
	                        _this.createPageController(obj, Result);
	                    }else{
                            $('.FullOrder tbody').html('');
	                        $('#pageContainer').html('<img src="/images/no_data.png" style="display: block;margin: 30px auto;">');
	                    }
                	}else{
                		layer.msg(data.Message,{'time':2000});
                	}
                }
            })
        },
        createPageController : function(obj, Result){
        	var _this = this;
            var counts = Result.TotleCount;
            $("#pageContainer").pagination(counts, {
                current_page: (obj.PageIndex ? obj.PageIndex : 1),
                items_per_page: 20, 
                callback: function (currPage) {
                    var obj1 = obj;
                    obj1.PageIndex = currPage;
                    _this.requestdata(obj1);
                    $('#pageContainer').html('');
                }
            });
        },
        renderdata: function (data,obj) {// 渲染数据
        	var _this = this;
            $('.FullOrder tbody').html(ejs.render($('#full-order').html(), data));
            $('.FullOrder tbody tr').off('click').on('click',function(){
                var that = $(this);
                var OrderId = that.attr('OrderId');
                window.open('/manager/media/order/content-detail.html?OrderId=' + OrderId);
            })
        }
    }
    var FullOrderList = new FullOrderList();
})