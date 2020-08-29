/*
* Written by:     wangc
* function:       媒体文章列表
* Created Date:   2017-10-19
* Modified Date:  2017-11-27 查询条件下若媒体无文章，也可打标签
* 发文日期默认是：今天减6天至今天，如：2017-11-22至2017-11-28
* 默认渲染或点击查询后，若无对应文章，显示操作“打标签”，用户可以直接给媒体打标签。
* 若存在对应文章，显示操作“领取|领取查询出的所有文章”，点击后跳转到文章打标签页面。
*/
$(function () {
    var LabelUrl = {
        ArticleQueryOrRecive:labelApi.tag+'/api/MediaLabel/ArticleQueryOrRecive'//打标签媒体文章列表查询
    }
    var searchInfo;//存储查询信息
    function MediaArticleList(){
        this.init();
    }
    MediaArticleList.prototype = {
        constructor:MediaArticleList,
        init:function(){
            var _self = this;
            var today = new Date().Format('yyyy-MM-dd');
            var today7 = new Date(new Date()-6*24*3600*1000).Format('yyyy-MM-dd');
            $('#EndDate').val(today);
            $('#StartDate').val(today7);
            /*创建日期点击事件*/
            $('#StartDate').off('click').on('click', function () {
                laydate({
                    fixed: false,
                    elem: '#StartDate',
                    choose: function (date) {
                        if (date > $('#EndDate').val() && $('#EndDate').val()) {
                            layer.alert('起始时间不能大于结束时间！');
                            $('#SatrtDate').val('')
                        }
                    }
                });
            });
            $('#EndDate').off('click').on('click', function () {
                laydate({
                    fixed: false,
                    elem: '#EndDate',
                    choose: function (date) {
                        if (date < $('#StartDate').val() && $('#StartDate').val()) {
                            layer.alert('结束时间不能小于起始时间！');
                            $('#EndDate').val('')
                        }
                    }
                });
            });
            $('#searchList').off('click').on('click',function () {
                $('.ad_table tbody').html('');
                $('#pageContainer').show().html('<img src="/images/no_data.png" style="display: block;margin: 70px auto;">');
                $('#getArticle,#getAllArticle').hide();
                $('#addTag').show();
                var upData = _self.getParams();
                if( !upData.StartDate){
                    layer.msg('请选择发文开始日期',{time:1000})
                    return
                }
                if( !upData.EndDate){
                    layer.msg('请选择发文结束日期',{time:1000})
                    return
                }
                if( !upData.ArticleCount){
                    layer.msg('请输入文章数量',{time:1000})
                    return
                }
                searchInfo = upData;
                _self.searchList(upData);
            })
            $('#searchList').click();
        },
        searchList:function(upData){//查询列表
            var _self = this;
            setAjax({
                url:LabelUrl.ArticleQueryOrRecive,
                type:'get',
                data:upData
            },function (data) {
                if(data.Status==0){
                    $('.ad_table tbody').html('');
                    //渲染媒体信息
                    if(data.Result.MediaInfo.HeadImg){
                        $('#mediaImg').attr('src',data.Result.MediaInfo.HeadImg);
                    }else{
                        $('#mediaImg').attr('src','../images/default_touxiang.png');
                    }
                    
                    if(data.Result.MediaInfo.Number){
                        $('.mediaNumber').html(data.Result.MediaInfo.Number);
                    }
                    
                    var mediaType = '微信公众号';
                    switch(data.Result.MediaInfo.MediaType){
                        case 14001:
                            break;
                        case 14002:
                            mediaType = 'APP';
                            break;
                        case 14003:
                            mediaType = '微博';
                            break;
                        case 14004:
                            mediaType = '视频';
                            break;
                        case 14005:
                            mediaType = '直播';
                            break;
                        case 14006:
                            mediaType = '头条';
                            break;
                        case 14007:
                            mediaType = '搜狐';
                            break;
                    }
                    $('.mediaName').html(data.Result.MediaInfo.Name+' [ '+mediaType+ ' ]');
                    if(data.Result.ListInfo.TotalCount != 0){
                        $("#pageContainer").show();
                        $('#getArticle,#getAllArticle').show();
                        $('#addTag').hide();
                        //分页
                        $("#pageContainer").pagination(data.Result.ListInfo.TotalCount,{
                            items_per_page: 30, //每页显示多少条记录（默认为20条）
                            callback: function (currPage, jg) {
                                var dataObj = _self.getParams();
                                dataObj.PageIndex = currPage;
                                setAjax({
                                    url: LabelUrl.ArticleQueryOrRecive,
                                    type: 'get',
                                    data: dataObj
                                }, function (data) {
                                    // 渲染数据
                                    $('.ad_table tbody').html(ejs.render($('#tagList').html(), {list:data.Result.ListInfo.List}));
                                    _self.operate();
                                    _self.sorting(data.Result);
                                })
                            }
                        });
                        $('.ad_table tbody').html(ejs.render($('#tagList').html(), {list:data.Result.ListInfo.List}));
                        _self.sorting(data.Result.ListInfo.List);
                        _self.operate();
                    }else {
                        $('#pageContainer').show().html('<img src="/images/no_data.png" style="display: block;margin: 70px auto;">');
                        $('#getArticle,#getAllArticle').hide();
                        $('#addTag').show();
                        _self.operate();
                    }
                }else {
                    layer.msg(data.Message,{time:1000})
                }
            })
        },
        getParams:function(){//获取查询参数
            var _self = this;
            var Resource;
            switch(_self.GetRequest().MediaType){
                case '14001':
                    Resource = 1;
                    break;
                case '14006':
                    Resource = 3;
                    break;
                case '14007':
                    Resource = 6;
                    break;
                default:
                    Resource = 1;
                    break;
            }
            return {
                IsRecive:false,//查询fasle ，领取true
                StartDate:$('#StartDate').val(),
                EndDate:$('#EndDate').val(),
                Resource:Resource,//渠道ID，1：微信 2：汽车之家 3：今日头条 4： 网易汽车 5：行圆新闻后台 6：搜狐
                Number:_self.GetRequest().NumberOrName,
                ArticleCount:$.trim($('#articleNumber').val()),
                ArticleIds:'',
                PageIndex:1,
                PageSize:30
            }
        },
        operate:function(){//全选、单选及领取文章、文章数量需>=1
            var _self = this;
            $('#AllCheck').off('change').on('change',function(){
                if($(this).prop('checked')){
                    $('.oneCheck').prop('checked',true);
                }else{
                     $('.oneCheck').prop('checked',false);
                }
            })
            $('.oneCheck').off('change').on('change',function(){
                if($('.oneCheck:checked').length < $('.oneCheck').length){
                    $('#AllCheck').prop('checked',false);
                }else{
                    $('#AllCheck').prop('checked',true);
                }
            })
            $('#articleNumber').off('blur').on('blur',function(){
                var reg = /^\d+$/,
                    val = $.trim($(this).val());
                if( reg.test(val)==false || val==0){
                    $('#articleNumber').val('');
                    layer.msg('请输入 >=1 的整数',{time:1000})
                }
            })
            //领取查询出的所有文章
            $('#getAllArticle').off('click').on('click',function(){
                var ArticleIdsArr = [],ArticleIds;
                if($('.ad_table tbody').find('tr').length){
                    var ReceiveArticle = searchInfo;
                    ReceiveArticle.IsRecive = true;
                    ReceiveArticle.ArticleIds = "";
                    delete ReceiveArticle.PageSize;
                    delete ReceiveArticle.PageIndex;
                    setAjax({
                        url:LabelUrl.ArticleQueryOrRecive,
                        type:'get',
                        data:ReceiveArticle
                    },function(data){
                        if(data.Status == 0){
                            //原逻辑：领取跳转到已领文章列表，现在：直接打标签
                            // var BatchMediaID = data.Result,
                            //     NumberOrName = _self.GetRequest().NumberOrName,
                            //     MediaType = _self.GetRequest().MediaType;
                            // window.location = '/TagManager/MediaCheckedArticleList.html?BatchMediaID='+BatchMediaID+'&MediaType='+MediaType+'&NumberOrName='+NumberOrName;
                            //媒体文章打标签
                            var MediaType = _self.GetRequest().MediaType,
                                NumberOrName = _self.GetRequest().NumberOrName;
                            window.location = '/TagManager/addTagForArticle.html?MediaType='+MediaType+'&NumberOrName='+NumberOrName;
                        }
                    })
                    
                }else{
                    layer.msg('没有可领取的文章',{time:1000});
                }
                
            })
            //领取选择的文章
            $('#getArticle').off('click').on('click',function(){
                var ArticleIdsArr = [],ArticleIds;
                if($('.ad_table tbody').find('input:checked').length){
                    $('.ad_table tbody').find('tr').each(function(){
                        if($(this).find('input').prop('checked')){
                            ArticleIdsArr.push($(this).find('input').attr('ArticleId'));
                        }
                        
                    })
                    ArticleIds = ArticleIdsArr.join(',');
                    var ReceiveArticle = searchInfo;
                    ReceiveArticle.IsRecive = true;
                    ReceiveArticle.ArticleIds = ArticleIds;
                    delete ReceiveArticle.PageSize;
                    delete ReceiveArticle.PageIndex;
                    setAjax({
                        url:LabelUrl.ArticleQueryOrRecive,
                        type:'get',
                        data:ReceiveArticle
                    },function(data){
                        if(data.Status == 0){
                            // var BatchMediaID = data.Result,
                            //     NumberOrName = _self.GetRequest().NumberOrName,
                            //     MediaType = _self.GetRequest().MediaType;
                            // window.location = '/TagManager/MediaCheckedArticleList.html?BatchMediaID='+BatchMediaID+'&MediaType='+MediaType+'&NumberOrName='+NumberOrName;
                            var MediaType = _self.GetRequest().MediaType,
                                NumberOrName = _self.GetRequest().NumberOrName;
                            window.location = '/TagManager/addTagForArticle.html?MediaType='+MediaType+'&NumberOrName='+NumberOrName;
                        }
                    })
                }else{
                    layer.msg('请选择要领取的文章',{time:1000});
                }

            })
            //打标签--跳转到媒体打标签页面
            $('#addTag').off('click').on('click',function(){
                var MediaType = _self.GetRequest().MediaType,
                    NumberOrName = _self.GetRequest().NumberOrName;
                window.location = '/TagManager/addTagForMedia.html?MediaType='+MediaType+'&NumberOrName='+NumberOrName;
            })
        },
        sorting:function(data){//点击排序切换正序倒序
            var _self = this;
            var i = 0;
            $('table thead a').off('click').on('click', function () {
                if ($(this).attr('dataorderby') != -1) {
                    i = 0;
                }
                //升序
                if (i) {
                    $('table thead a').attr('class', '').attr('dataOrderby', '').children().attr('src', '/ImagesNew/icon16_c.png')
                    $(this).addClass('active').attr('dataOrderby', '-1').children().attr('src', '/ImagesNew/icon16_a.png')
                    if ($(this).attr('data-orderby') != -2) {
                        $(this).attr('data-orderby', $(this).attr('data-orderby').slice(0, -1) + '2')
                    }
                    i = 0;
                    var OrderBy = $(this).attr('data-orderby') - 0;
                    if($('tbody').html() != ''){
                        _self.Sortrender(OrderBy, 2, data);
                    }
                } else {//降序
                    $('table thead a').attr('class', '').attr('dataOrderby', '').children().attr('src', '/ImagesNew/icon16_c.png')
                    $(this).addClass('active').attr('dataOrderby', '-1').children().attr('src', '/ImagesNew/icon16_b.png')
                    if ($(this).attr('data-orderby') != -2) {
                        $(this).attr('data-orderby', $(this).attr('data-orderby').slice(0, -1) + '1')
                    }
                    i = 1;
                    var OrderBy = $(this).attr('data-orderby') - 0;
                    if($('tbody').html() != ''){
                        _self.Sortrender(OrderBy, 1, data);
                    }
                }
            })
        },
        Sortrender: function (OrderBy, i, data) {// 排序渲染
            var _self = this;
            function time(de) {
                var stringTime = de;
                var timestamp2 = Date.parse(new Date(stringTime));
                timestamp2 = timestamp2 / 1000;
                return timestamp2
            }
            $('tbody').html('');

            if (i == 1) {// 降序
                if (OrderBy == 1001) {
                    var a = data.sort(function (x, y) {
                        return y.ReadNum - x.ReadNum
                    })
                    $('tbody').html(ejs.render($('#tagList').html(), {list:a}));
                }
                if (OrderBy == 2001) {
                    var a = data.sort(function (x, y) {
                        return y.LikeNum - x.LikeNum
                    })
                    $('tbody').html(ejs.render($('#tagList').html(), {list:a}));
                }
                if (OrderBy == 3001) {
                    var a = data.sort(function (x, y) {
                        return y.ComNum - x.ComNum
                    })
                    $('tbody').html(ejs.render($('#tagList').html(), {list:a}));
                }
                if (OrderBy == 4001) {
                    var a = data.sort(function (x, y) {
                        return time(y.PublishTime) - time(x.PublishTime)
                    })
                    $('tbody').html(ejs.render($('#tagList').html(), {list:a}));
                }
            } else {// 升序
                if (OrderBy == 1002) {
                    var a = data.sort(function (x, y) {
                        return x.ReadNum - y.ReadNum
                    })
                    $('tbody').html(ejs.render($('#tagList').html(), {list:a}));
                }
                if (OrderBy == 2002) {
                    var a = data.sort(function (x, y) {
                        return x.LikeNum - y.LikeNum
                    })
                    $('tbody').html(ejs.render($('#tagList').html(), {list:a}));
                }
                if (OrderBy == 3002) {
                    var a = data.sort(function (x, y) {
                        return x.ComNum - y.ComNum
                    })
                    $('tbody').html(ejs.render($('#tagList').html(), {list:a}));
                }
                if (OrderBy == 4002) {
                    var a = data.sort(function (x, y) {
                        return time(x.PublishTime) - time(y.PublishTime)
                    })
                    $('tbody').html(ejs.render($('#tagList').html(), {list:a}));
                }
            }
            _self.operate();
        },
        GetRequest:function(){
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
    }
    new MediaArticleList();
    Date.prototype.Format = function (fmt) { //author: meizz 
        var o = {
            "M+": this.getMonth() + 1, //月份 
            "d+": this.getDate(), //日 
            "h+": this.getHours(), //小时 
            "m+": this.getMinutes(), //分 
            "s+": this.getSeconds(), //秒 
            "q+": Math.floor((this.getMonth() + 3) / 3), //季度 
            "S": this.getMilliseconds() //毫秒 
        };
        if (/(y+)/.test(fmt)) fmt = fmt.replace(RegExp.$1, (this.getFullYear() + "").substr(4 - RegExp.$1.length));
        for (var k in o)
            if (new RegExp("(" + k + ")").test(fmt)) fmt = fmt.replace(RegExp.$1, (RegExp.$1.length == 1) ? (o[k]) : (("00" + o[k]).substr(("" + o[k]).length)));
        return fmt;
    }
})