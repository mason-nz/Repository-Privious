/**
 * Written by:     zhengxh
 * Created Date:   2017/7/25
 */
// 获取当前时间
Date.prototype.format = function(fmt) {
    var o = {
        "M+" : this.getMonth()+1,                 //月份
        "d+" : this.getDate(),                    //日
        "h+" : this.getHours(),                   //小时
        "m+" : this.getMinutes(),                 //分
        "s+" : this.getSeconds(),                 //秒
        "q+" : Math.floor((this.getMonth()+3)/3), //季度
        "S"  : this.getMilliseconds()             //毫秒
    };
    if(/(y+)/.test(fmt)) {
        fmt=fmt.replace(RegExp.$1, (this.getFullYear()+"").substr(4 - RegExp.$1.length));
    }
    for(var k in o) {
        if(new RegExp("("+ k +")").test(fmt)){
            fmt = fmt.replace(RegExp.$1, (RegExp.$1.length==1) ? (o[k]) : (("00"+ o[k]).substr((""+ o[k]).length)));
        }
    }
    return fmt;
};
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
};
$(function () {
    $.ajax({
        url: "http://www.chitunion.com/api/Channel/GetCost_ChannelList",
        type: "get",
        data: {

        },
        dataType: 'json',
        async: false,
        xhrFields: {
            withCredentials: true
        },
        crossDomain: true,
        success: function (data) {
            if(data.Status==0){
                for(var i=0;i<data.Result.List.length;i++){
                    $('#channel').append('<option value="'+data.Result.List[i].ChannelID+'">'+data.Result.List[i].ChannelName+'</option>');
                }
            }
        }
    });
    var meideid;
    // 媒体名称联想搜索
    $('#meideName').off('keyup').on('keyup', function () {
        meideid=-2;
        var val = $.trim($(this).val());
        if (val != '') {
            $.ajax({
                url: 'http://www.chitunion.com/api/Channel/GetCost_MediaList',
                type: 'get',
                data: {
                    MediaName:val?val:'-'
                },
                dataType: 'json',
                async: false,
                xhrFields: {
                    withCredentials: true
                },
                crossDomain: true,
                success: function (data) {
                    if (data.Status == 0) {
                        var availableArr = [],MediaID=[];
                        for (var i = 0; i < data.Result.List.length; i++) {
                            if (data.Result.List[i].MediaName.toUpperCase().indexOf(val.toUpperCase()) != -1) {
                                // availableArr.push(data.Result[i].Name+' '+data.Result[i].Number);
                                MediaID.push(data.Result.List[i].MediaID);
                                availableArr.push(data.Result.List[i].MediaName);
                            }
                        }
                        if(availableArr.indexOf($.trim($('#meideName').val()))!=-1){
                            console.log(meideid);
                            meideid=MediaID[availableArr.indexOf($.trim($('#meideName').val()))];
                        }
                        /*将availableArr去重*/
                        var result = [];
                        for (var i = 0; i < availableArr.length; i++) {
                            if (result.indexOf(availableArr[i]) == -1) {
                                result.push(availableArr[i]);
                            }
                        }
                        $('#meideName').autocomplete({
                            source: availableArr,
                            select:function (event, ui) {
                                console.log(event);
                                console.log(ui);
                                console.log(availableArr);
                                console.log($.trim($('#meideName').val()));
                                if(availableArr.indexOf(ui.item.value)!=-1){

                                    meideid=MediaID[availableArr.indexOf(ui.item.value)];
                                    console.log(meideid);
                                }
                            }
                        })
                    }
                }
            });
        }
    });
    /*添加文本框聚焦事件，使其默认请求一次*/
    $('#meideName').off('focus').on('focus', function () {
        var val = $.trim($(this).val());
        if (val == '') {
            $.ajax({
                url: 'http://www.chitunion.com/api/Channel/GetCost_MediaList',
                type: 'get',
                data: {
                    MediaName:val?val:'-'
                },
                dataType: 'json',
                async: false,
                xhrFields: {
                    withCredentials: true
                },
                crossDomain: true,
                success: function (data) {
                    if (data.Status == 0) {
                        var availableArr = [];
                        for (var i = 0; i < data.Result.List.length; i++) {
                            if (data.Result.List[i].MediaName.toUpperCase().indexOf(val.toUpperCase()) != -1) {
                                // availableArr.push(data.Result[i].Name+' '+data.Result[i].Number);
                                availableArr.push(data.Result.List[i].MediaName);
                            }
                        }
                        /*将availableArr去重*/
                        var result = [];
                        for (var i = 0; i < availableArr.length; i++) {
                            if (result.indexOf(availableArr[i]) == -1) {
                                result.push(availableArr[i]);
                            }
                        }
                        $('#meideName').autocomplete({
                            source: availableArr
                        });
                    }
                }
            });
        }

    });
    if(GetRequest().ChannelID!=undefined){
        $('#channel option').each(function () {
            if($(this).val()==GetRequest().ChannelID){
                $(this).prop('selected', true);
                return false;
            }
        })
    }
    $('#seach').off('click').on('click',function () {
        function parameter(i) {
            var meideName=meideid;
            var channel=$('#channel option:selected').val();
            var ggstart=$('#ggstart option:selected').val();
            return {
                MediaID:meideName,
                ChannelID:channel,
                SaleStatus:ggstart,
                PageSize:20,
                PageIndex:i
            }
        };

        setAjax({
            url:'http://www.chitunion.com/api/Channel/GetCostList',
            type:'get',
            data:parameter(1)
        },function (data) {
            if(data.Status==0){
                // data.Result.List.push({
                //     "CostID": 111,
                //     "HeadIconUrl":"头像地址",
                //     "WxName": "微信名称",
                //     "WxNumber": "微信账号",
                //     "ChannelName": "渠道名称",
                //     "CostPriceRange": "",
                //     "SalePriceRange": "",
                //     "OriginalPrice": 0,
                //     "CooperateDate": "",
                //     "SaleStatus": 1,
                //     "SaleStatusName": "上架"
                // })
                $('table').html(ejs.render($('#Cost').html(), data));
                operation();
                // 如果数据为0显示图片
                if (data.Result.TotalCount != 0) {
                    //分页
                    $("#pageContainer").pagination(
                        data.Result.TotalCount,
                        {
                            items_per_page: 20, //每页显示多少条记录（默认为20条）
                            callback: function (currPage, jg) {
                                setAjax({
                                    url:'http://www.chitunion.com/api/Channel/GetCostList',
                                    type: 'get',
                                    data: parameter(currPage)
                                }, function (data) {
                                    $('table').html(ejs.render($('#Cost').html(), data));
                                    operation();
                                })
                            }
                        });

                } else {
                    $('#pageContainer').html('<img src="/images/no_data.png" style="display: block;margin: 70px auto;">')
                }
            }
        })

    });
    $('#seach').click();

    function operation() {
        /*全选 全不选按钮*/
        $('.box').on('change', '#allbox', function () {
            if ($(this).prop('checked')) {
                $('.onebox').prop('checked', true);
            } else {
                $('.onebox').prop('checked', false);
            }
        });
        /*单选按钮*/
        $('.box').on('change', '.onebox', function () {
            if ($('.onebox').length == $("input[name='checkbox']:checked").length) {
                $('#allbox').prop('checked', true);
            } else {
                $('#allbox').prop('checked', false);
            }
        });

        $('.delete').off('click').on('click',function () {
            var _this=$(this);
            layer.confirm('是否删除此项？', {
                time: 0 //不自动关闭
                , btn: ['删除', '取消']
                , yes: function (index) {
                    setAjax({
                        url:'http://www.chitunion.com/api/Channel/DeleteCost',
                        type:'post',
                        data:{
                            CostID: _this.attr('CostID')
                        }
                    },function (data) {
                        if(data.Status==0){
                            layer.msg('删除成功');
                            $('#seach').click();
                        }else {
                            layer.msg(data.Message)
                        }
                    })
                    layer.close(index);
                }
            });

        });

        $('.Offtheshelf').off('click').on('click',function () {
            var _this=$(this);
                setAjax({
                    url:'http://www.chitunion.com/api/Channel/BatchCostOperate',
                    type:'post',
                    data:{
                        CostIDList:[_this.attr('CostID')-0],
                        OpType:44002
                    }
                },function (data) {
                    if(data.Status==0){
                        _this.hide().siblings().show();
                        _this.parents('td').prev().find('div').html('停用');
                    }else {
                        layer.msg(data.Message)
                    }
                })
        });
        $('.Theshelves').off('click').on('click',function () {
            var _this=$(this);
            if(new Date().format("yyyy-MM-dd")<=$(this).attr('endtime')){
                setAjax({
                    url:'http://www.chitunion.com/api/Channel/BatchCostOperate',
                    type:'post',
                    data:{
                        CostIDList:[_this.attr('CostID')-0],
                        OpType:44001
                    }
                },function (data) {
                    if(data.Status==0){
                        _this.hide().siblings('.delete').hide().siblings('.Offtheshelf').show();
                        _this.parents('td').prev().find('div').html('启用');
                    }else {
                        layer.msg(data.Message)
                    }
                })
            }else {
                layer.msg('不在执行周期内');
            }
        });

        $('.Batchshelves').off('click').on('click',function () {
            var arrid=[],date=[];
            $('.onebox').each(function () {
                var _this=$(this);
                if($(this).prop('checked')){
                    arrid.push(_this.attr('CostID')-0);
                    date.push(_this.attr('date'))
                }
            });
            console.log(arrid);
            console.log(date);
            var date_1=[],screen=[];
            for(var i=0;i<date.length;i++){
                if(date[i]==0){
                    date_1.push(date[i]);
                    screen.push(i);
                }
            }
            console.log(screen);
            for (var i=0;i<screen.length;i++){
                arrid[screen[i]] = ''
            }
            var arrid1=[];
            for(var i=0;i<arrid.length;i++){
                if(arrid[i]!=''){
                    arrid1.push(arrid[i])
                }
            }
            if(arrid.length>0){
                if(date.length==date_1.length){
                    layer.msg('不在执行周期内');
                    return false;
                }
                setAjax({
                    url:'http://www.chitunion.com/api/Channel/BatchCostOperate',
                    type:'post',
                    data:{
                        CostIDList:arrid1,
                        OpType:44001
                    }
                },function (data) {
                    if(data.Status==0){
                        $('#seach').click();
                        $('#allbox').prop('checked', false);
                    }else {
                        layer.msg(data.Message)
                    }
                })
            }else {
                layer.msg('请选择媒体');
            }
        });
        $('.Batchrelease').off('click').on('click',function () {
            var arrid=[],date=[];
            $('.onebox').each(function () {
                var _this=$(this);
                if($(this).prop('checked')){
                    arrid.push(_this.attr('CostID')-0);
                    date.push(_this.attr('date'))
                }
            });
            console.log(arrid);
            console.log(date);
            var date_1=[],screen=[];
            for(var i=0;i<date.length;i++){
                if(date[i]==0){
                    date_1.push(date[i]);
                    screen.push(i);
                }
            }
            console.log(screen);
            // for (var i=0;i<screen.length;i++){
            //     arrid[screen[i]] = ''
            // }
            // var arrid1=[];
            // for(var i=0;i<arrid.length;i++){
            //     if(arrid[i]!=''){
            //         arrid1.push(arrid[i])
            //     }
            // }

            if(arrid.length>0){
                setAjax({
                    url:'http://www.chitunion.com/api/Channel/BatchCostOperate',
                    type:'post',
                    data:{
                        CostIDList:arrid,
                        OpType:44002
                    }
                },function (data) {
                    if(data.Status==0){
                        $('#seach').click();
                        $('#allbox').prop('checked', false);
                    }else {
                        layer.msg(data.Message)
                    }
                })
            }else {
                layer.msg('请选择媒体');
            }
        });

        $('.See').off('click').on('click',function () {
            window.open('/ChannelManager/look_CostPrice.html?CostID='+$(this).attr('CostID'))
        })
    }
})