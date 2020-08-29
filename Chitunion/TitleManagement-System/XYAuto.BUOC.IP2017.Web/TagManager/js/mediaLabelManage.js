/*
* Written by:     wangc
* function:       媒体标签管理
* Created Date:   2017-10-23
* Modified Date:  2017-11-28 增加媒体删除功能
*/
$(function () {
    var LabelUrl = {
        CommonlyClass:labelApi.tag+'/api/MediaLabel/GetCommonlyClass',//常见分类
        InputListMedia:labelApi.tag+'/api/ResultLabel/QueryResultMediaList',//查询媒体最终结果
        DeleteMediaLabel:labelApi.tag+'/api/ResultLabel/DeleteMediaLabel'//删除媒体
    }
    var searchInfo,//存储查询信息
        flag = 0;//标志是否已查询
    function LabelManageList(){
        this.init();
    }
    LabelManageList.prototype = {
        constructor:LabelManageList,
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

            //渲染常见分类
            if($('#MediaType').find('option:checked').attr('value') != '-2'){
                _self.GetCommonlyClass();
            }
            //改变媒体类型时，分类重新渲染,常见分类和是否自营在媒体类型为全部时不显示
            $('#MediaType').off('change').on('change',function(){
                var mediaType = $(this).find('option:checked').attr('value');
                if(mediaType == '-2'){
                    $('#projectType option:first').prop('selected',true);
                    $('.commonCategory,.isSelfDoBusiness').hide();
                    $('#SelfDoBusiness').find('option:first').prop('selected',true);
                }else{
                    _self.GetCommonlyClass();
                    $('#SelfDoBusiness').find('option:first').prop('selected',true);
                    $('.commonCategory,.isSelfDoBusiness').show();
                }
            })
            $('#MediaType').change();

            $('.searchList').off('click').on('click',function () {
                var type = $('.tab_menu .selected').attr('value');
                var upData = _self.getParams();
                searchInfo = upData;
                var requestUrl = LabelUrl.InputListMedia;
                _self.searchList(requestUrl,'#tagList',upData);//递归查询列表
            })
            $('.searchList').click();
            $('.tab_menu').off('click').on('click','li',function(){
                $(this).addClass('selected').siblings('li').removeClass('selected');
                _self.judgeDisplay();
                $('.searchList').click();

            })
            $('.reset').off('click').on('click',function (){
                _self.judgeDisplay();
                $('#MediaType').change();
            })
        },
        GetCommonlyClass:function(){
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
                        if(_self.GetRequest().isSearch && localStorage.searchForMedia && flag==0){
                            var searchForMedia = JSON.parse(localStorage.searchForMedia);
                            //常见分类
                            $('#projectType option').each(function(){
                                if($(this).attr('DictId') == searchForMedia.DictId){
                                    $(this).prop('selected',true);
                                }
                            })
                        }

                    }
                }
            })
        },
        searchList:function(requestUrl,temp,upData){
            var _self = this;
            var LabelStatus = $('.tab_menu .selected').attr('value');
            setAjax({
                url:requestUrl,
                type:'get',
                data:upData
            },function (data) {
                if(data.Status == 0){
                    // console.log(data)
                    $('.ad_table').html('');
                    if(data.Result.TotalCount != 0){
                        $("#pageContainer").show();
                        if(data.Result.List.length){//如果有返回数据
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
                                        $('.ad_table').html(ejs.render($(temp).html(), {list:data.Result.List}));
                                        _self.operate();
                                    })
                                }
                            });
                            $('.ad_table').html(ejs.render($(temp).html(), {list:data.Result.List}));
                            _self.operate();
                        }else{//没有返回数据
                            $("#pageContainer").hide();
                            if(upData.PageIndex != 1){
                                upData.PageIndex = upData.PageIndex-1;
                                _self.searchList(requestUrl,temp,upData);
                            }
                        }
                    }else {
                        $('.ad_table').html(ejs.render($(temp).html(), {list:data.Result.List}));
                        $('#pageContainer').show().html('<img src="/images/no_data.png" style="display: block;margin: 70px auto;">')
                    }
                }else {
                    layer.msg(data.Message,{time:1000})
                }
            })
        },
        judgeDisplay:function(){//重置操作
            //清空原来选择
            $('#MediaType').find('option:first').prop('selected',true);
            $('#projectType').find('option:first').prop('selected',true);
            $('.keyWord').val('');
            $('#tagStatus').find('option:first').prop('selected',true);
            $('#StartDate,#EndDate').val('');
            $('#SelfDoBusiness').find('option:first').prop('selected',true);
            $('.brand').find('option:first').prop('selected',true);
            $('.series').find('option:first').prop('selected',true);
        },
        getParams:function(){//获取查询参数
            var _self = this;
            if(_self.GetRequest().isSearch && localStorage.searchForMedia && flag==0){
                flag = 1;
                var searchForMedia = JSON.parse(localStorage.searchForMedia);
                //媒体类型
                $('#MediaType option').each(function(){
                    if($(this).attr('value') == searchForMedia.MediaType){
                        $(this).prop('selected',true);
                    }
                })
                console.log(searchForMedia.MediaType)
                //如果媒体类型为全部，不显示常见分类和是否自营，是否自营，默认显示全部
                if(searchForMedia.MediaType == '-2'){
                    $('.commonCategory,.isSelfDoBusiness').hide();
                    $('#SelfDoBusiness').find('option:first').prop('selected',true);
                }else{
                    //否则，根据存储渲染是否自营和常见分类
                    //是否自营
                    $('#SelfDoBusiness option').each(function(){
                        if($(this).attr('value') == searchForMedia.SelfDoBusiness){
                            $(this).prop('selected',true);
                        }
                    })
                    //常见分类
                    $('#projectType option').each(function(){
                        if($(this).attr('DictId') == searchForMedia.DictId){
                            $(this).prop('selected',true);
                        }
                    })
                }
                //账号名称
                $('.keyWord').val(searchForMedia.Name);

                //提交日期
                $('#StartDate').val(searchForMedia.StartDate);
                $('#EndDate').val(searchForMedia.EndDate);
                return searchForMedia;
            }else{
                return {
                    MediaType:$('#MediaType option:checked').attr('value')-0,
                    DictId:$('#projectType option:checked').attr('DictId')-0,
                    Name:$.trim($('.keyWord').val()),
                    SelfDoBusiness:$('#SelfDoBusiness option:checked').attr('value')-0,
                    StartDate:$('#StartDate').val(),
                    EndDate:$('#EndDate').val(),
                    PageIndex:1,
                    PageSize:20
                }
            }
        },
        operate:function(){//查看标签
            var _self = this;
            //修改标签
            $('.modifyLabel').off('click').on('click',function(){
                _self.saveInfo();
                var BatchID = $(this).attr('BatchID');//批次ID
                var MediaType = $(this).attr('MediaType');
                // 修改媒体
                window.location = '/TagManager/Modifymedia.html?TaskType=2001&BatchID='+BatchID+'&SelectType=2';
            })
            //删除标签
            $('.deleteLabel').off('click').on('click',function(){
                var BatchID = $(this).attr('BatchID')-0;
                layer.confirm('确认要删除吗',{
                    btn:['确认','取消']
                },function(index){
                    setAjax({
                        url:LabelUrl.DeleteMediaLabel,
                        type:'post',
                        data:{
                            MediaResultID:BatchID
                        }
                    },function(data){
                        if(data.Status == 0){
                            layer.msg('成功');
                            $('.searchList').click();
                        }else{
                            layer.msg(data.Message);
                        }
                    })
                    layer.close(index);
                })
            })
        },
        saveInfo:function(){//存储查询条件信息
            var curPage = $('#pageContainer').find('.current:last').html();
            if(curPage == '下一页'){
                curPage = $('#pageContainer').find('.current:last').prev().html();
            }
            var searchForMedia = searchInfo;
            searchForMedia.PageIndex = curPage;
            localStorage.searchForMedia = JSON.stringify(searchForMedia);
        },
        GetRequest:function(){// 获取url？后面的参数
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
    new LabelManageList();
})