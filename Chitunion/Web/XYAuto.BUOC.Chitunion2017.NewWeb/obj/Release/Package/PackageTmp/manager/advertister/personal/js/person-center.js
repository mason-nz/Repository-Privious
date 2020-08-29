/*
* Written by:     fengb
* function:       广告主个人中心
* Created Date:   2017-12-21
*/

$(function () {

    console.log(CTLogin);

    if(CTLogin.Category != 29001){//如果角色不对 需要跳转到媒体主的个人中心
        window.location = '/manager/media/personal/personalCenter.html';
    }

	var urlArrs = ['/api/SmartSearch/GetSmartSearchList','/api/ContentDistribute/GetContentDistributeList','/api/MediaPromotion/GetMediaPromotionList'];
    function PersonalCenter() {
    	this.queryparameters();//查询
        $('#TabSelect li').eq(0).click();
        $('#UserName').html(CTLogin.UserName.substr(0,15));
    }
   
    PersonalCenter.prototype = {
        constructor: PersonalCenter,
        // 获取查询参数
        queryparameters: function () {
            var _this = this;
            // 切换显示数据
            $('#TabSelect li').off('click').on('click', function () {
                var that = $(this);
                var idx = that.index();
                that.addClass('selected').siblings().removeClass('selected');
                var obj = {
                    PageSize : 10,
                    PageIndex : 1,
                    r: Math.random()
                }
                $('.list-table').html('');
                _this.requestdata(obj,idx);
            })
        },
        requestdata: function (obj,idx) {// 请求数据
            var _this = this;
            $.ajax({
                url: public_url + urlArrs[idx],
                type: 'get',
                xhrFields: {
		            withCredentials: true
		        },
		        crossDomain: true,
                data: obj,
                beforeSend: function(){
                    $('#listLoading').html('<img src="/images/loading.gif" style="display: block;margin: 70px auto;">');
                },
                success : function (data) {
                	if(data.Status == 0){
                		$('#listLoading').html('');
	                    var Result = data.Result;
	                    if(Result.TotalCount <= 0) {
                            $('.list-table').html('');
	                        $('#listLoading').html('<img src="/images/no_data.png" style="display: block;margin: 70px auto;">');
	                    }
                        _this.getoperation(Result,idx);
                	}else{
                		layer.msg(data.Message,{'time':2000});
                	}
                }
            })

            $.ajax({
                url: public_url + '/api/SmartSearch/GetSmartSearchCount',
                type: 'get',
                xhrFields: {
                    withCredentials: true
                },
                crossDomain: true,
                data: {
                    r: Math.random()
                },
                success : function (data) {
                    if(data.Status == 0){
                        var PromotionCount = data.Result.PromotionCount;
                        $('#PromotionCount').html(PromotionCount);
                    }else{
                        layer.msg(data.Message,{'time':2000});
                    }
                }
            })
        },
        getoperation: function (data,idx) {// 渲染数据
        	var _this = this;
            $('.list-table').html(ejs.render($('#personal-order').html(), data));
            $('.Modele').hide();
            $('.Modele' + idx).show();
            //查看更多
            $('#more').off('click').on('click',function(){
                var that = $(this);
                if(idx == 0){
                    that.attr('href','/manager/advertister/extension/IntelligentSearchList.html');
                }else if(idx == 1){
                    that.attr('href','/manager/advertister/extension/distributeGeneralizelist.html');
                }else if(idx == 2){
                    that.attr('href','/manager/advertister/extension/mediaRealizationList.html');
                }
            })

            //查看
            $('.list-table .goIntoDetail').off('click').on('click',function(){
                var that = $(this);
                var RecID = that.parents('tr').attr('RecID');
                if(idx == 0){
                    that.attr('href','/manager/advertister/extension/viewOrderDetail.html?RecID=' + RecID);
                }else if(idx == 1){
                    that.attr('href','/manager/advertister/extension/distributeGeneralizeDetail.html?RecID=' + RecID);
                }else if(idx == 2){
                    that.attr('href','/manager/advertister/extension/mediaRealizationDetail.html?RecID=' + RecID);
                }
            })

            $('.list-table tr').off('click').on('click',function(){
                var that = $(this);
                var RecID = that.attr('RecID');
                if(idx == 0){
                    window.location = '/manager/advertister/extension/viewOrderDetail.html?RecID=' + RecID;
                }else if(idx == 1){
                    window.location = '/manager/advertister/extension/distributeGeneralizeDetail.html?RecID=' + RecID;
                }else if(idx == 2){
                    window.location = '/manager/advertister/extension/mediaRealizationDetail.html?RecID=' + RecID;
                }
            })

        }
    }
    var PersonalCenter = new PersonalCenter();
})