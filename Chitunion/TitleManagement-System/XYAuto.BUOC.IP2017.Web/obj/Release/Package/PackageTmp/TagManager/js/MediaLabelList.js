/*
* Written by:     wangc
* function:       标签录入列表
* Created Date:   2017-10-18
* Modified Date:  2017-11-28  wangc 微信下列表排序调接口
*
*1、待打
    媒体类型为微信时，显示“文章”查询条件，查询出待打列表后可排序（调接口）
    排序注意点：由于待打、审核中、已审表头不同，所以表头之前也放在模板中，而这次，待打列表排序，
        点击排序后需要保存现在的排序条件及页面显示（降序或升序，对应Img不同）。
        所以，表头不能放在模板中，这次把两个模板的表头都提出来，切换表格隐藏显示。
2、审核中和已审，在点击查看时，需要判断当前媒体批次下，是否有文章，若有，跳转到查看文章页面，若无，跳转到查看媒体页面。
3、打标签跳转：
    区分媒体类型，主要按照媒体下是否有文章区分。
    若媒体类型为14002或14003或14004或14005，则跳转到媒体打标签页面：/TagManager/addTagForMedia.html?NumberOrName=柚宝宝&MediaType=14002
    若媒体类型为14001或14006或14007，则点击打标签时，需调接口判断，该媒体下是否有已领取的文章，若有，直接跳转到已领文章列表，用户可以直接
            若无，跳转到媒体文章列表。
*/
$(function () {
    var LabelUrl = {
        CommonlyClass:labelApi.tag+'/api/MediaLabel/GetCommonlyClass',//常见分类
        InputListMedia:labelApi.tag+'/api/MediaLabel/InputListMedia',//媒体标签录入列表，查询待打
        BatchListMedia:labelApi.tag+'/api/MediaLabel/BatchListMedia',//打标签批次列表，查询审核中和已审
        QueryArticleCount:labelApi.tag+'/api/MediaLabel/QueryArticleCount'//判断媒体下是否有文章
    }
    var searchInfo,//存储查询信息
        flag = 0;//标志是否已查询
    function MediaLabelList(){
        this.init();
    }
    MediaLabelList.prototype = {
        constructor:MediaLabelList,
        init:function(){
            var _self = this;
            /*创建日期点击事件*/
            $('#StartDate').off('click').on('click', function () {
                laydate({
                    fixed: false,
                    elem: '#StartDate',
                    choose: function (date) {
                        if (date > $('#EndDate').val() && $('#EndDate').val()) {
                            layer.alert('起始时间不能大于结束时间！');
                            $('#StartDate').val('')
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
                var LabelStatus = $('.tab_menu .selected').attr('value');
                var upData = _self.getParams();
                searchInfo = upData;
                //如果媒体类型不是微信，且用户点击头条阅读量排序，则OrderBy=-2
                if(searchInfo.MediaType!='14001' && $('.ad_table thead a.active').length && $('.ad_table thead a.active').attr('data-orderby').substr(0,1) == 2){
                    upData.OrderBy = '-2';
                    return
                }
                var requestUrl = LabelUrl.InputListMedia;//查询待打
                var temp = '#tagList0';
                switch(LabelStatus){
                    case '-2':
                        break;
                    case '1003':
                    case '1004':
                        temp = '#tagList1';
                        requestUrl = LabelUrl.BatchListMedia;//查询审核中和已审列表
                        break;
                }
                var tableTemp = temp.substr(temp.length-1,temp.length);
                $('.for'+tableTemp).show();
                if(tableTemp == 1){
                    $('.for0').hide();
                }else{
                    $('.for1').hide();
                }
                _self.searchList(requestUrl,temp,upData,'#for'+tableTemp);//递归查询列表
            })
            $('#searchList').click();
            $('.tab_menu').off('click').on('click','li',function(){
                $(this).addClass('selected').siblings('li').removeClass('selected');
                $('table thead a').attr('class', '').children().attr('src', '/ImagesNew/icon16_c.png')
                _self.judgeDisplay();
                $('#searchList').click();
            })
            //渲染常见分类
            if($('#MediaType').find('option:checked').attr('value') != '-2'){
                _self.GetCommonlyClass();
            }
            //改变媒体类型时，分类重新渲染.常见分类，文章对应显示隐藏
            $('#MediaType').off('change').on('change',function(){
                if($('.tab_menu .selected').attr('value') != '-2'){
                    return
                }

                $('table thead a').attr({'class':'','is_click':'0'}).children().attr('src', '/ImagesNew/icon16_c.png')
                //文章范围，默认选中全部
                $('#HasArticleType').find('option:first').prop('selected',true);
                var mediaType = $(this).find('option:checked').attr('value');
                switch(mediaType){
                    case '-2':
                        $('.HasArticleType').hide();
                        $('#projectType option:first').prop('selected',true);
                        break;
                    case '14001':
                    case '14006':
                    case '14007':
                        _self.GetCommonlyClass();
                        $('.HasArticleType').show();
                        break;
                    case '14002':
                    case '14003':
                    case '14004':
                    case '14005':
                        _self.GetCommonlyClass();
                        $('.HasArticleType').hide();
                        break;
                    default:
                        break;
                }
            })
            $('#reset').off('click').on('click',function (){
                _self.judgeDisplay();
            })
        },
        GetCommonlyClass:function(){//渲染常见分类
            var _self = this;
            $.ajax({
                url:LabelUrl.CommonlyClass,
                type:'get',
                async: false,
                dataType: 'json',
                xhrFields: {
                    withCredentials: true
                },
                crossDomain: true,
                data: {
                    mediaType:$('#MediaType option:checked').attr('value')-0
                },
                success:function(data){
                    if(data.Status == 0){
                        var str = '<option DictId="-2">全部</option>';
                        for(var i = 0;i<data.Result.length;i++){
                            str+='<option DictId='+data.Result[i].DictId+'>'+data.Result[i].DictName+'</option>'
                        }
                        $('#projectType').html(str);
                        if(_self.GetRequest().isSearch && localStorage.SearchOption && flag==0){
                            var SearchOption = JSON.parse(localStorage.SearchOption);
                            //常见分类
                            $('#projectType option').each(function(){
                                if($(this).attr('DictId') == SearchOption.DictId){
                                    $(this).prop('selected',true);
                                }
                            })
                        }

                    }
                }
            })
        },
        searchList:function(requestUrl,temp,upData,ele){
            var _self = this;
            $("#pageContainer").hide();
            var LabelStatus = $('.tab_menu .selected').attr('value');
            setAjax({
                url:requestUrl,
                type:'get',
                data:upData
            },function (data) {
                if(data.Status==0){
                    // $('.ad_table').html('');
                    if(data.Result.TotalCount != 0){
                        $("#pageContainer").show();
                        if(data.Result.List.length){//如果有返回数据
                            $(ele).html(ejs.render($(temp).html(), {list:data.Result.List}));
                            // _self.sorting(data.Result.List);
                            _self.sortByBackstage();
                            switch(LabelStatus){
                                case '-2':
                                    break;
                                case '1003':
                                    $('.AuditTime').hide();
                                    break;
                                case '1004':
                                    $('.AuditTime').show();
                                    break;
                            }
                            _self.operate();
                            //分页
                            $("#pageContainer").pagination(data.Result.TotalCount,{
                                items_per_page: 20, //每页显示多少条记录（默认为20条）
                                current_page:upData.PageIndex,
                                callback: function (currPage, jg) {
                                    var dataObj = _self.getParams();
                                    dataObj.PageIndex = currPage;
                                    setAjax({
                                        url: requestUrl,
                                        type: 'get',
                                        data: dataObj
                                    }, function (data) {
                                        // 渲染数据
                                        $(ele).html(ejs.render($(temp).html(), {list:data.Result.List}));
                                        switch(LabelStatus){
                                            case '-2':
                                                break;
                                            case '1003':
                                                $('.AuditTime').hide();
                                                break;
                                            case '1004':
                                                $('.AuditTime').show();
                                                break;
                                        }
                                        _self.operate();
                                        // _self.sorting(data.Result.List);
                                        _self.sortByBackstage();
                                    })
                                }
                            });
                            
                        }else{//没有返回数据
                            $("#pageContainer").hide();
                            if(upData.PageIndex != 1){
                                upData.PageIndex = upData.PageIndex-1;
                                _self.searchList(requestUrl,temp,upData,'.for'+temp.substr(temp.length-1,temp.length));
                            }
                        }
                    }else {
                        $(ele).html(ejs.render($(temp).html(), {list:data.Result.List}));
                        $('#pageContainer').show().html('<img src="/images/no_data.png" style="display: block;margin: 70px auto;">')
                    }
                }else {
                    layer.msg(data.Message,{time:1000})
                }
            })
        },
        judgeDisplay:function(){//查询条件显示隐藏、重置操作
        	var LabelStatus = $('.tab_menu .selected').attr('value');
            switch(LabelStatus){
                case '-2':
                    if($('#MediaType').find('option').length != 7){
                        $('#MediaType').find('option:first').remove();
                    }
                    //显示标签状态和文章，不显示提交日期
                    $('.HasArticleType').show();
                    $('.LabelSatus').show();
                    $('.commitDate').hide();

                    $('.commonCategory').show();
                    $('.isSelfDoBusiness').show();
                    break;
                case '1003':
                case '1004':
                    if($('#MediaType').find('option').length == 7){
                        $('#MediaType').find('option:first').before('<option value="-2">全部</option>');
                    }
                    //审核中和已审时，不显示常见分类和是否自营
                    $('.commonCategory').hide();
                    $('.isSelfDoBusiness').hide();
                    $('.HasArticleType').hide();
                    $('.LabelSatus').hide();
                    $('.commitDate').show();
                    break;
            }
            //清空原来选择
            $('#MediaType').find('option:first').prop('selected',true);
            $('#projectType').find('option:first').prop('selected',true);
            $('.keyWord').val('');
            $('#tagStatus').find('option:first').prop('selected',true);
            $('#StartDate,#EndDate').val('');
            $('#SelfDoBusiness').find('option:first').prop('selected',true);
            $('.HasArticleType').find('option:first').prop('selected',true);
        },
        getParams:function(){//获取查询参数
            var _self = this;
            if(_self.GetRequest().isSearch && localStorage.SearchOption && flag==0){
                flag = 1;
                var SearchOption = JSON.parse(localStorage.SearchOption);
                //渲染切换项
                if(SearchOption.Status == 1003){
                    $('.LabelSatus').hide();
                    $('.commitDate').show();
                   $('.tab_menu').find('li').eq(1).addClass('selected').siblings('li').removeClass('selected');
                }else if(SearchOption.Status == 1004){
                    $('.LabelSatus').hide();
                    $('.commitDate').show();
                   $('.tab_menu').find('li').eq(2).addClass('selected').siblings('li').removeClass('selected');
                }else{
                    $('.LabelSatus').show();
                    $('.commitDate').hide();
                    $('.tab_menu').find('li').eq(0).addClass('selected').siblings('li').removeClass('selected');
                }
                
                //媒体类型
                $('#MediaType option').each(function(){
                    if($(this).attr('value') == SearchOption.MediaType){
                        $(this).prop('selected',true);
                    }
                })
                if(SearchOption.MediaType != '14001' && SearchOption.MediaType != '14006' && SearchOption.MediaType != '14007'){
                    $('.HasArticleType').hide();
                    $('#HasArticleType').find('option:first').prop('selected',true);
                }else{
                    //文章范围
                    $('.HasArticleType option').each(function(){
                        if($(this).attr('value') == SearchOption.HasArticleType){
                            $(this).prop('selected',true);
                        }
                    })
                }
                //账号名称
                $('.keyWord').val(SearchOption.Name);
                //标签状态
                $('#tagStatus option').each(function(){
                    if($(this).attr('value') == SearchOption.LabelStatus){
                        $(this).prop('selected',true);
                    }
                })
                //是否自营
                $('#SelfDoBusiness option').each(function(){
                    if($(this).attr('value') == SearchOption.SelfDoBusiness){
                        $(this).prop('selected',true);
                    }
                })
                //提交日期
                $('#StartDate').val(SearchOption.StartDate);
                $('#EndDate').val(SearchOption.EndDate);
                
                return SearchOption;
            }else{
                var OrderBy = -2;
                //如果是待审且存在排序
                if($('.tab_menu .selected').attr('value') == '-2' && $('.ad_table thead a.active').length){
                    OrderBy = $('.ad_table thead a.active').attr('data-orderby');
                }
                return {
                    Status:$('.tab_menu .selected').attr('value'),//打标签批次状态
                    MediaType:$('#MediaType option:checked').attr('value')-0,
                    DictId:$('#projectType option:checked').attr('DictId')-0,
                    Name:$.trim($('.keyWord').val()),
                    LabelStatus:$('#tagStatus option:checked').attr('value')-0,//标签状态
                    SelfDoBusiness:$('#SelfDoBusiness option:checked').attr('value')-0,//是否自营
                    StartDate:$('#StartDate').val(),
                    EndDate:$('#EndDate').val(),
                    HasArticleType:$('#HasArticleType option:checked').attr('value')-0,
                    OrderBy:OrderBy,
                    PageIndex:1,
                    PageSize:20
                }
            }
        },
        operate:function(){//显示历史录入信息及打标签跳转
            var _self = this;
        	$('.viewHistory').off('mouseover').on('mouseover',function(){
        		$(this).attr('src','../ImagesNew/btn_history1.png');
        		$(this).next().show();
        	}).off('mouseout').on('mouseout',function(){
        		$(this).attr('src','../ImagesNew/btn_history.png');
        		$(this).next().hide();
        	})
            //打标签
            $('.addLabel').off('click').on('click',function(){
                _self.saveInfo();//存储查询信息
                //判断，若是微信、头条或搜狐号，则跳转至列表，否则，跳转到打标签页
                var _this = $(this);
                var mediaNumber = _this.attr('Number'),
                    MediaType = _this.attr('MediaType'),
                    mediaName = encodeURI(_this.attr('Name')),
                    BatchMediaID = _this.attr('BatchMediaID');
                switch(MediaType){
                    //有文章的媒体，如微信，头条，搜狐，若之前有领取过的文章（存在BatchMediaID），直接跳转到已领取文章列表（可点击打标签）
                    //若未领取过文章，跳转到媒体文章列表，领取文章后，才会跳转到已领取文章列表
                    //无文章的媒体，若微博。视频，直播等，直接跳转到打标签页面
                    //Number目前只有微信有，其他媒体只有Name
                    case '14001':
                        if(BatchMediaID && BatchMediaID != 'undefined' && BatchMediaID != 'null' && BatchMediaID !='-2'){
                            window.location = '/TagManager/MediaCheckedArticleList.html?BatchMediaID='+BatchMediaID+'&MediaType='+MediaType+'&NumberOrName='+mediaNumber;
                        }else{
                            window.location = '/TagManager/MediaArticleList.html?NumberOrName='+mediaNumber+'&MediaType='+MediaType;
                        }
                        break;
                    case '14006':
                    case '14007':
                        if(BatchMediaID && BatchMediaID != 'undefined' && BatchMediaID != 'null' && BatchMediaID !='-2'){
                            window.location = '/TagManager/MediaCheckedArticleList.html?BatchMediaID='+BatchMediaID+'&MediaType='+MediaType+'&NumberOrName='+mediaName;
                        }else{
                            window.location = '/TagManager/MediaArticleList.html?NumberOrName='+mediaName+'&MediaType='+MediaType;
                        }
                        break;
                    case '14002':
                    case '14003':
                    case '14004':
                    case '14005':
                        window.location = '/TagManager/addTagForMedia.html?NumberOrName='+mediaName+'&MediaType='+MediaType;//打标签-媒体
                        break;
                }
            })

            $('.seeLabel').off('click').on('click',function(){
                var BatchMediaID = $(this).attr('BatchMediaID'),
                    mediaType = $(this).attr('MediaType');
                if(mediaType == '14001' || mediaType == '14006' || mediaType == '14007'){
                    //查看媒体当前批次下是否有文章，若无，跳转到查看媒体页面，若有，跳转到查看文章页面
                    $.ajax({
                        url:LabelUrl.QueryArticleCount,
                        type:'get',
                        async: false,
                        dataType: 'json',
                        xhrFields: {
                            withCredentials: true
                        },
                        crossDomain: true,
                        data: {
                            BatchType:1,
                            BatchMediaID:BatchMediaID
                        },
                        success:function(data){
                            if(data.Status != 0){
                                layer.msg(data.Message);
                            }else{
                                if(data.Result.ArticleCount > 0){
                                    window.open('/TagManager/ViewArticle.html?BatchMediaID='+BatchMediaID+'&mediaType='+mediaType)
                                }else{
                                    window.open('/TagManager/ViewMedia.html?BatchMediaID='+BatchMediaID)
                               }
                            }
                        }
                    })
                }else{
                    window.open('/TagManager/ViewMedia.html?BatchMediaID='+BatchMediaID)
                }
            })

        },
        saveInfo:function(){//存储查询条件信息
            var curPage = $('#pageContainer').find('.current:last').html();
            var SearchOption = searchInfo;
            if(curPage == '下一页'){
                curPage = $('#pageContainer').find('.current:last').prev().html();
            }
            SearchOption.PageIndex = curPage;
            localStorage.SearchOption = JSON.stringify(SearchOption);
        },
        sorting:function(data){//排序操作
            var _self = this;
            /*排序开始*/
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
            /*排序结束*/
        },
        Sortrender: function (OrderBy, i, data) {// 排序渲染
            var _self = this;
            function time(de) {
                var stringTime = de;
                var timestamp2 = Date.parse(new Date(stringTime));
                timestamp2 = timestamp2 / 1000;
                return timestamp2
            }
            $('tbody').html('')

            if (i == 1) {// 降序
                if (OrderBy == 1001) {
                    var a = data.sort(function (x, y) {
                        return y.StatisticsCount - x.StatisticsCount
                    })
                    $('tbody').html(ejs.render($('#sortResult').html(), {list:a}));
                }
                if (OrderBy == 2001) {
                    var a = data.sort(function (x, y) {
                        return y.ReadCount - x.ReadCount
                    })
                    $('tbody').html(ejs.render($('#sortResult').html(), {list:a}));
                }
                if (OrderBy == 3001) {
                    var a = data.sort(function (x, y) {
                        return time(y.CreateTime) - time(x.CreateTime)
                    })
                    $('tbody').html(ejs.render($('#sortResult').html(), {list:a}));
                }
            } else {// 升序
                if (OrderBy == 1002) {
                    var a = data.sort(function (x, y) {
                        return x.StatisticsCount - y.StatisticsCount
                    })
                    $('tbody').html(ejs.render($('#sortResult').html(), {list:a}));
                }
                if (OrderBy == 2002) {
                    var a = data.sort(function (x, y) {
                        return x.ReadCount - y.ReadCount
                    })
                    $('tbody').html(ejs.render($('#sortResult').html(), {list:a}));
                }
                if (OrderBy == 3002) {
                    var a = data.sort(function (x, y) {
                        return time(x.CreateTime) - time(y.CreateTime)
                    })
                    $('tbody').html(ejs.render($('#sortResult').html(), {list:a}));
                }
            }
            _self.operate();
        },
        sortByBackstage:function(){
            var _self = this;
            $('table thead a').off('click').on('click', function () {
                var isClick = $(this).attr('is_click');
                //升序
                if (isClick%2 != 0) {
                    $('table thead a').attr('class', '').children().attr('src', '/ImagesNew/icon16_c.png')
                    $(this).addClass('active').children().attr('src', '/ImagesNew/icon16_a.png')
                    if ($(this).attr('data-orderby') != -2) {
                        $(this).attr('data-orderby', $(this).attr('data-orderby').slice(0, -1) + '2')
                    }
                    $('#searchList').click();
                } else {//降序
                    $('table thead a').attr('class', '').children().attr('src', '/ImagesNew/icon16_c.png')
                    $(this).addClass('active').children().attr('src', '/ImagesNew/icon16_b.png')
                    if ($(this).attr('data-orderby') != -2) {
                        $(this).attr('data-orderby', $(this).attr('data-orderby').slice(0, -1) + '1')
                    }
                    $('#searchList').click();
                }
                $(this).attr('is_click',$(this).attr('is_click')-0+1);
            })
        },
        GetRequest:function(){
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
    }
    new MediaLabelList();
})