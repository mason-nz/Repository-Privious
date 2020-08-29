/*
* Written by:     wangc
* function:       媒体文章已领取列表
* Created Date:   2017-10-23
* Modified Date:
*/
$(function () {
    var LabelUrl = {
        QueryArticleListByBactchID:labelApi.tag+'/api/MediaLabel/QueryArticleListByBactchID'//查询媒体已领文章列表
    }
    function MediaArticleList(){
        this.init();
    }
    MediaArticleList.prototype = {
        constructor:MediaArticleList,
        init:function(){
            var _self = this;
            $('.ad_table tbody').html('');
            var upData = _self.getParams();
            _self.searchList(upData);
        },
        searchList:function(upData){//查询列表
            var _self = this;
            setAjax({
                url:LabelUrl.QueryArticleListByBactchID,
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
                        default:
                            break;
                    }
                    $('.mediaName').html(data.Result.MediaInfo.Name+' [ '+mediaType+ ' ]');
                    if(data.Result.ListInfo.TotalCount != 0){
                        $("#pageContainer").show();
                        //分页
                        $("#pageContainer").pagination(data.Result.ListInfo.TotalCount,{
                            items_per_page: 30, //每页显示多少条记录（默认为20条）
                            callback: function (currPage, jg) {
                                var dataObj = _self.getParams();
                                dataObj.PageIndex = currPage;
                                setAjax({
                                    url: LabelUrl.QueryArticleListByBactchID,
                                    type: 'get',
                                    data: dataObj
                                }, function (data) {
                                    // 渲染数据
                                    $('.ad_table tbody').html(ejs.render($('#tagList').html(), {list:data.Result.ListInfo.List}));
                                    _self.operate();
                                })
                            }
                        });
                        $('.ad_table tbody').html(ejs.render($('#tagList').html(), {list:data.Result.ListInfo.List}));
                        _self.operate();
                    }else {
                        $('#pageContainer').show().html('<img src="/images/no_data.png" style="display: block;margin: 70px auto;">')
                        window.location = '/TagManager/MediaArticleList.html?NumberOrName='+_self.GetRequest().NumberOrName+'&MediaType='+_self.GetRequest().MediaType;
                    }
                }else {
                    layer.msg(data.Message,{time:1000})
                }
            })
        },
        getParams:function(){//获取查询参数
            var _self = this;
            return {
                BatchMediaID:_self.GetRequest().BatchMediaID,
                PageSize:30,
                PageIndex:1
            }
        },
        operate:function(){//媒体打标签
            var _self = this;
            $('#addTag').off('click').on('click',function(){
                //媒体文章打标签
                var MediaType = _self.GetRequest().MediaType,
                    NumberOrName = _self.GetRequest().NumberOrName;
                window.location = '/TagManager/addTagForArticle.html?MediaType='+MediaType+'&NumberOrName='+NumberOrName;
            })
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
})