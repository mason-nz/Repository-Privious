/*
* Written by:     wangc
* function:       标签已审核列表
* Created Date:   2017-10-20
* Modified Date:
*/
/*
1、审核列表：品牌名称和所属品牌均由后台返回，前端不做判断
*/
$(function () {
    var LabelUrl = {
        QueryBrand:labelApi.tag+'/api/CarSerial/QueryBrand',//获取品牌
        CommonlyClass:labelApi.tag+'/api/MediaLabel/GetCommonlyClass',//常见分类
        InputListMedia:labelApi.tag+'/api/ExamineLabel/QueryAuditedMediaList',//查询媒体已审
        BatchListCar:labelApi.tag+'/api/ExamineLabel/QueryAuditedBrandList',//查询车型已审
        QueryArticleCount:labelApi.tag+'/api/MediaLabel/QueryArticleCount'//判断媒体下是否有文章
    }
    var searchInfo,//存储查询信息
        flag = 0;//标志是否已查询
    function LabelAuditList(){
        this.init();
    }
    LabelAuditList.prototype = {
        constructor:LabelAuditList,
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
            // 品牌车型
            $.ajax({
                url:LabelUrl.QueryBrand,
                type:'get',
                async: false,
                dataType: 'json',
                xhrFields: {
                    withCredentials: true
                },
                crossDomain: true,
                data: {},
                success:function(data){
                    if(data.Status == 0){
                        var str = '<option MasterId="-2">请选择品牌</option>';
                        for(var i=0;i<data.Result.length;i++){
                            str += '<option MasterId='+data.Result[i].MasterId+'>'+data.Result[i].Name+'</option>'
                        }
                        $('.brand').html(str);

                        $('.brand').off('change').on('change',function(){
                            var MasterBrandId = $(this).find('option:checked').attr('MasterId');
                            console.log(MasterBrandId)
                            if(MasterBrandId == '-2'){
                                $('.series').html('<option BrandId="-2">请选择子品牌</option>');
                            }else{
                                $.ajax({
                                    url:LabelUrl.QueryBrand,
                                    type:'get',
                                    async: false,
                                    dataType: 'json',
                                    xhrFields: {
                                        withCredentials: true
                                    },
                                    crossDomain: true,
                                    data: {
                                        MasterBrandId:MasterBrandId
                                    },
                                    success:function(data){
                                        if(data.Status == 0){
                                            var str1 = '<option BrandId="-2">请选择子品牌</option>';
                                            for(var j=0;j<data.Result.length;j++){
                                                str1 += '<option BrandId='+data.Result[j].BrandId+'>'+data.Result[j].Name+'</option>'
                                            }
                                            $('.series').html(str1);
                                        }
                                    }
                                });
                            }
                        })
                        $('.brand').change();
                    }else{
                        layer.msg(data.Message,{time:1000})
                    }
                }
            })

            $('.searchList').off('click').on('click',function () {
                // $('.ad_table').html('');
                var type = $('.tab_menu .selected').attr('value');
                var upData = _self.getParams();
                searchInfo = upData;
                var requestUrl = LabelUrl.InputListMedia;//查询媒体待审
                var temp = '#tagList';
                switch(type){
                    case '0':
                        break;
                    case '1':
                        temp = '#tagList1';
                        requestUrl = LabelUrl.BatchListCar;//查询车型待审
                        break;
                }
                _self.searchList(requestUrl,temp,upData);//递归查询列表
            })
            $('.searchList').click();
            $('.tab_menu').off('click').on('click','li',function(){
                $(this).addClass('selected').siblings('li').removeClass('selected');
                _self.judgeDisplay();
                //切换后，如果媒体类型为全部，不显示常见分类和是否自营
                if($('#MediaType').find('option:checked').attr('value') == '-2'){
                    $('.commonCategory,.isSelfDoBusiness').hide();
                }else{
                    $('.commonCategory,.isSelfDoBusiness').show();
                }
                $('.searchList').click();

            })
            $('.reset').off('click').on('click',function (){
                _self.judgeDisplay();
            })
        },
        GetCommonlyClass:function(){//渲染常见分类
            setAjax({
                url:LabelUrl.CommonlyClass,
                type:'get',
                data:{
                    mediaType:$('#MediaType option:checked').attr('value')-0
                }
            },function(data){
                if(data.Status == 0){
                    var str = '<option DictId="-2">全部</option>';
                    for(var i = 0;i<data.Result.length;i++){
                        str+='<option DictId='+data.Result[i].DictId+'>'+data.Result[i].DictName+'</option>'
                    }
                    $('#projectType').html(str)
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
        judgeDisplay:function(){//查询条件显示隐藏、重置操作
        	var LabelStatus = $('.tab_menu .selected').attr('value');
            switch(LabelStatus){
                case '0':
                    $('.forMedia').show();
                    $('.forCar').hide();
                    break;
                case '1':
                    $('.forMedia').hide();
                    $('.forCar').show();
                    break;
            }
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
            var BrandType,BrandId;
            if($('.brand option:checked').attr('MasterId') != -2 && $('.series option:checked').attr('brandid') != -2){
                BrandType = 2;
                BrandId = $('.series option:checked').attr('BrandID')-0;
            }else{
                BrandType = 1;
                BrandId = $('.brand option:checked').attr('MasterId')-0;
            }
            return {
                MediaType:$('#MediaType option:checked').attr('value')-0,
                DictId:$('#projectType option:checked').attr('DictId')-0,
                Name:$.trim($('.keyWord').val()),
                SelfDoBusiness:$('#SelfDoBusiness option:checked').attr('value')-0,
                StartDate:$('#StartDate').val(),
                EndDate:$('#EndDate').val(),
                BrandType:BrandType,
                MasterId:$('.brand option:checked').attr('MasterId')-0,
                BrandId:BrandId,
                PageIndex:1,
                PageSize:20
            }
        },
        operate:function(){//查看
            $('.seeLabel').off('click').on('click',function(){

                ///TagManager/ViewAuditArticle.html?BatchAuditID=<%= item.BatchID %>&mediaType=<%= item.MediaType 
                ///TagManager/ViewAuditMedia.html?BatchAuditID=<%= item.BatchID %>
                var BatchAuditID = $(this).attr('BatchAuditID'),
                    mediaType = $(this).attr('mediaType');
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
                            BatchType:2,
                            BatchMediaID:BatchAuditID
                        },
                        success:function(data){
                            if(data.Status != 0){
                                layer.msg(data.Message);
                            }else{
                                if(data.Result.ArticleCount > 0){
                                    window.open('/TagManager/ViewAuditArticle.html?BatchAuditID='+BatchAuditID+'&mediaType='+mediaType)
                                }else{
                                    window.open('/TagManager/ViewAuditMedia.html?BatchAuditID='+BatchAuditID)
                               }
                            }
                        }
                    })
                }else{
                    window.open('/TagManager/ViewAuditMedia.html?BatchAuditID='+BatchAuditID)
                }
            })
        }
    }
    new LabelAuditList();
})