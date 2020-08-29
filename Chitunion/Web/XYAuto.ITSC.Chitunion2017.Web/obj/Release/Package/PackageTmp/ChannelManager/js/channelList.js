/*
* Written by:     wangcan
* function:       渠道列表
* Created Date:   2017-07-17
* Modified Date:
*/
$(function () {
    //判断权限，显示“添加账号”
    if(CTLogin.BUTIDs.indexOf('SYS001BUT1600101') == -1){
        $('.channel_add_btn').remove();
    }
    $('#searchList').off('click').on('click', function () {
        function obj() {
            // 渠道名称
            var ChannelName=$('#ChannelName').val() == undefined ? "":$.trim($('#ChannelName').val());
            //状态 0-有效, 1-已过期 ,-2-不限
            var Status = $('.ChannelStatus option:checked').attr('value');
            return {
                ChannelName:ChannelName,
                Status:Status,
                pageIndex:1,
                PageSize:20
            }
        }
        var tmp = '#channelTemp';
        // 请求数据
        setAjax({
            url: 'http://www.chitunion.com/api/Channel/GetChannelList',
            type: 'get',
            data: obj(),
        }, function (data) {
            if(data.Status == 0){
                // 如果数据为0显示图片
                if (data.Result.TotalCount != 0) {
                    //分页
                    $("#pageContainer").pagination(
                        data.Result.TotalCount,
                        {
                            items_per_page: 20, //每页显示多少条记录（默认为20条）
                            callback: function (curPage, jg) {
                                var dataObj = obj();
                                dataObj.pageIndex = curPage
                                setAjax({
                                    url: 'http://www.chitunion.com/api/Channel/GetChannelList',
                                    type: 'get',
                                    data: dataObj
                                }, function (data) {
                                    // 渲染数据
                                    $('.ad_table table tbody').html(ejs.render($(tmp).html(), {list: data.Result.List}));
                                    channelOperate();
                                })
                            }
                        });
                } else {
                    $('#pageContainer').html('<img src="/images/no_data.png" style="display: block;margin: 70px auto;">')
                }
                $('.ad_table table tbody').html(ejs.render($(tmp).html(), {list: data.Result.List}));
                channelOperate();

            }
        })
    })
    $('#searchList').click();
    function channelOperate(){
        /*查看媒体*/
        $('.detailSearch').off('click').on('click',function(){
            var ChannelID = $(this).attr('ChannelID');
            window.open('/ChannelManager/look_channel.html?ChannelID='+ChannelID);
        })
        //编辑，在当前页打开
        $('.channel_edit_btn').off('click').on('click',function(){
            var ChannelID = $(this).parents('td').attr('ChannelID');
            window.location = '/ChannelManager/edit_channel.html?ChannelID='+ChannelID;
        })
        //删除
        $('.channel_delete_btn').off('click').on('click',function(){
            var _this = $(this);
            var ChannelID = _this.parents('td').attr('ChannelID');
            var WxCount = _this.parents('td').attr('WxCount');
            if(WxCount > 0){
                layer.msg('该渠道下有媒体，不能删除！',{time:2000});
            }else{
                layer.confirm('您确认删除此渠道吗', {
                    time: 0 //不自动关闭
                    , btn: ['确认', '取消']
                    , yes: function (index) {
                        setAjax({
                            url:'http://www.chitunion.com/api/Channel/DeleteChannel',
                            type: 'post',
                            data: {
                               ChannelID : ChannelID 
                            }
                        },function(data){
                            if(data.Status == 0){
                                _this.parents('tr').remove();
                                layer.close(index);
                                layer.msg('渠道删除成功', {time: 1500});
                                $('#searchList').click();
                            }else{
                                layer.msg(data.Message);
                            }
                        });
                    }
                });
            }
        })
    }
    //添加渠道
    $('.channel_add_btn').off('click').on('click',function(){
        window.location = '/ChannelManager/add_channel.html'
    })
})
