/*
* Written by:     wangc
* function:       标签审核列表
* Created Date:   2017-10-20
* Modified Date:
* 1、审核列表：品牌名称和所属品牌均由后台返回，前端不做判断
*/
$(function () {
    var LabelUrl = {
        CommonlyClass:labelApi.tag+'/api/MediaLabel/GetCommonlyClass',//常见分类
        QueryBrand:labelApi.tag+'/api/CarSerial/QueryBrand',//获取品牌
        InputListMedia:labelApi.tag+'/api/ExamineLabel/QueryPendingAuditMediaList',//查询媒体待审
        BatchListCar:labelApi.tag+'/api/ExamineLabel/QueryPendingAuditBrandList',//查询车型待审
        UpdateMediaStatus:labelApi.tag+'/api/ExamineLabel/UpdateMediaStatus'//审核中接口
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
                        if(_self.GetRequest().isSearch && localStorage.auditOption && flag==0){
                            var auditOption = JSON.parse(localStorage.auditOption);
                            //品牌和子品牌
                            if(auditOption.MasterId){
                                $('.brand option').each(function(){
                                    if($(this).attr('MasterId') == auditOption.MasterId){
                                        $(this).prop('selected',true);
                                    }
                                })
                                $('.brand').change();
                                $('.series option').each(function(){
                                    if($(this).attr('BrandId') == auditOption.myBrandID){
                                        $(this).prop('selected',true);
                                    }
                                })
                            }
                        }
                    }else{
                        layer.msg(data.Message,{time:1000})
                    }
                }
            })
            $('.searchList').off('click').on('click',function () {
                $('.ad_table').html('');
                var type = $('.tab_menu .selected').attr('value');
                if(_self.GetRequest().isSearch && localStorage.auditOption && flag==0){
                    type = JSON.parse(localStorage.auditOption).LabelType;
                }
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
        GetCommonlyClass:function(){
            var _self = this;
            //渲染常见分类
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
                        if(_self.GetRequest().isSearch && localStorage.auditOption && flag==0){
                            var auditOption = JSON.parse(localStorage.auditOption);
                            //常见分类
                            $('#projectType option').each(function(){
                                if($(this).attr('DictId') == auditOption.DictId){
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
            if(_self.GetRequest().isSearch && localStorage.auditOption && flag==0){
                flag = 1;
                var auditOption = JSON.parse(localStorage.auditOption);
                //渲染切换项
                $('.tab_menu li').each(function(){
                    if($(this).attr('value') == auditOption.LabelType){
                        $(this).addClass('selected').siblings('li').removeClass('selected');
                    }

                })
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
                //媒体类型
                $('#MediaType option').each(function(){
                    if($(this).attr('value') == auditOption.MediaType){
                        $(this).prop('selected',true);
                    }
                })
                //如果媒体类型为全部，不显示常见分类和是否自营，是否自营，默认显示全部
                if(auditOption.MediaType == '-2'){
                    $('.commonCategory,.isSelfDoBusiness').hide();
                    $('#SelfDoBusiness').find('option:first').prop('selected',true);
                }else{
                    //否则，根据存储渲染是否自营和常见分类
                    //是否自营
                    $('#SelfDoBusiness option').each(function(){
                        if($(this).attr('value') == auditOption.SelfDoBusiness){
                            $(this).prop('selected',true);
                        }
                    })
                    //常见分类
                    $('#projectType option').each(function(){
                        if($(this).attr('DictId') == auditOption.DictId){
                            $(this).prop('selected',true);
                        }
                    })
                }
                //常见分类
                $('#projectType option').each(function(){
                    if($(this).attr('DictId') == auditOption.DictId){
                        $(this).prop('selected',true);
                    }
                })
                //账号名称
                $('.keyWord').val(auditOption.Name);
                //标签状态
                $('#tagStatus option').each(function(){
                    if($(this).attr('value') == auditOption.LabelStatus){
                        $(this).prop('selected',true);
                    }
                })
                //是否自营
                $('#SelfDoBusiness option').each(function(){
                    if($(this).attr('value') == auditOption.SelfDoBusiness){
                        $(this).prop('selected',true);
                    }
                })

                //车型

                //提交日期
                $('#StartDate').val(auditOption.StartDate);
                $('#EndDate').val(auditOption.EndDate);
                return auditOption;
            }else{
                var BrandType,BrandId;
                if($('.brand option:checked').attr('masterid') != -2 && $('.series option:checked').attr('brandid') != -2){
                    BrandId = $('.series option:checked').attr('BrandID')-0;
                }else{
                    BrandId = $('.brand option:checked').attr('MasterId')-0;
                }
                return {
                    LabelType:$('.tab_menu .selected').attr('value'),
                    MediaType:$('#MediaType option:checked').attr('value')-0,
                    DictId:$('#projectType option:checked').attr('DictId')-0,
                    Name:$.trim($('.keyWord').val()),
                    SelfDoBusiness:$('#SelfDoBusiness option:checked').attr('value')-0,
                    StartDate:$('#StartDate').val(),
                    EndDate:$('#EndDate').val(),
                    MasterId:$('.brand option:checked').attr('MasterId')-0,
                    BrandId:BrandId,
                    myBrandID:$('.series option:checked').attr('BrandID')-0,
                    PageIndex:1,
                    PageSize:20
                }
            }
        },
        operate:function(){//显示历史录入信息及打标签跳转
            var _self = this;
            //审核标签
            $('.auditLabel').off('click').on('click',function(){
                _self.saveInfo();
                var LabelType = $(this).attr('LabelType');//0为媒体标签，1为车型标签
                var BatchID = $(this).attr('BatchID');//批次ID
                var MediaType = $(this).attr('MediaType');
                var TaskType = $(this).attr('TaskType');
                $.ajax({
                    url:LabelUrl.UpdateMediaStatus,
                    type:'post',
                    async: false,
                    dataType: 'json',
                    xhrFields: {
                        withCredentials: true
                    },
                    crossDomain: true,
                    data: {
                        BatchAuditID:BatchID,
                        ExamineStatus:1003
                    },
                    success:function(data){
                        if(data.Status != 0){
                            layer.msg(data.Message);
                        }else{
                            switch(LabelType){
                                case '0':
                                    // 审核文章
                                    if(MediaType == '14001' || MediaType == '14006' || MediaType == '14007'){
                                        if(data.Result > 0){
                                            window.location = '/TagManager/AuditArticle.html?TaskType=2001&BatchID='+BatchID+'&SelectType=1';
                                        }else{
                                            window.location = '/TagManager/AuditMedia.html?TaskType=2001&BatchID='+BatchID+'&SelectType=1';
                                        }
                                    }else{
                                        // 审核媒体
                                        window.location = '/TagManager/AuditMedia.html?TaskType=2001&BatchID='+BatchID+'&SelectType=1';
                                    }
                                    break;
                                    // 审核车型
                                case '1':
                                    window.location = '/TagManager/AuditCar.html?TaskType='+TaskType+'&BatchID='+BatchID+'&SelectType=1';
                                    break;
                            }
                        }
                    }
                })
            })
        },
        saveInfo:function(){//存储查询条件信息
            var curPage = $('#pageContainer').find('.current:last').html();
            if(curPage == '下一页'){
                curPage = $('#pageContainer').find('.current:last').prev().html();
            }
            var auditOption = searchInfo;
            auditOption.PageIndex = curPage;
            localStorage.auditOption = JSON.stringify(auditOption);
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
    new LabelAuditList();
})