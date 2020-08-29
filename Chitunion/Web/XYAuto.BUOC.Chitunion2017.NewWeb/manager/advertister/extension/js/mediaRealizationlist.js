/*
* Written by:     yangyakun
* function:       媒体变现
* Created Date:   2017-12-15
*/
$(function(){
    //内容分发推广列表
    function mediarealization() {
        this.Rendering()
    }
    mediarealization.prototype = {
        constructor: mediarealization,
        Rendering: function () {//渲染
            var _this = this;
            $('.but_global').off('click').on('click', function () {
                setAjax({
                    url :public_url+'/api/MediaPromotion/GetMediaPromotionList',
                    // url: './json/mediaRealizationList.json',
                    type: 'get',
                    data:_this.getParams(1)
                }, function (data) {
                    console.log(data);
                    $('.data_body').html(ejs.render($('#media_realization').html(), {data:data.Result}));
                    _this.operation();
                    // 如果数据为0显示图片
                    if (data.Result.TotalCount != 0) {
                        if(data.Result.TotalCount>20){
                            $('#pageContainer').show()
                        } else{
                            $('#pageContainer').hide()
                        }
                        //分页
                        $("#pageContainer").pagination(
                            data.Result.TotalCount,
                            {
                                items_per_page: 20, //每页显示多少条记录（默认为20条）
                                callback: function (currPage, jg) {
                                    setAjax({
                                        url :public_url+'/api/MediaPromotion/GetMediaPromotionList',
                                        // url: './json/mediarealization.json',
                                        type: 'get',
                                        data: _this.getParams(currPage)
                                    }, function (data) {
                                        $('.data_body').html(ejs.render($('#media_realization').html(), {data:data.Result}));
                                        _this.operation();
                                    })
                                }
                            });
                    } else {
                        $('#no_data').removeClass('hide');
                        $('#pageContainer').hide()
                    }
                })
            })
            $('.but_global').click()
        },
        operation:function(){// 删除
            $('.delete').off('click').on('click',function(){
                var that = $(this);
                var RecID = that.parents('tr').attr('RecID')
                var param = {
                    RecID:RecID
                }
                layer.confirm('您确认删除吗', {
                    time: 0 //不自动关闭
                    , btn: ['确认', '取消']
                    , yes: function (index) {
                        setAjax({
                            url:public_url+'/MediaPromotion/GetMediaPromotionList',
                            url:'./json/mediaRealizationList.json',
                            type: 'get',
                            data: param
                        },function(data){
                            if(data.Status == 0){
                                that.parents('tr').remove();
                                layer.close();
                                layer.msg('媒体删除成功', {time: 2000});
                            }else{
                                layer.msg(data.Message);
                            }
                        });
                    }
                });
            })
            //点击添加智能匹配推广
            $('.extension').off('click').on('click',function(){
                 window.location=public_url+'/manager/advertister/extension/mediaRealizationCreate.html';
            })
            $('.table_a tr').off('click').on('click',function(){
                var $this = $(this),
                    RecID = $this.attr('RecID');
                window.open('/manager/advertister/extension/mediaRealizationDetail.html?RecID='+RecID);
            })
        },
        getParams:function(i){//获取参数
            return {
                PageIndex : i,
                PageSize : 20,
                Name : $.trim($('#generalizename').val()),
                Status : $('.state_sel option:checked').attr('value')-0
            }
        }  
    }
    var mediarealization = new mediarealization();

})