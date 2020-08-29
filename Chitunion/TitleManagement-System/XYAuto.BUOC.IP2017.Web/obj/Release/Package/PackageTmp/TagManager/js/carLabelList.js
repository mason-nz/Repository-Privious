/*
* Written by:     wangcan
* function:       车型标签录入列表
* Created Date:   2017-10-18
* Modified Date:
*/
$(function () {
    var LabelUrl = {
        QueryBrand:labelApi.tag+'/api/CarSerial/QueryBrand',//获取品牌
        InputListCar:labelApi.tag+'/api/CarSerialLabel/InputListCar',//车型标签接口，查询待打
        BatchListCar:labelApi.tag+'/api/CarSerialLabel/BatchListCar'//车型批次列表，查询审核中和已审
    }
    var searchInfo,//存储查询信息
        flag = 0;//标志是否已查询
    function CarLabelList(){
        this.init();
    }
    CarLabelList.prototype = {
        constructor:CarLabelList,
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
            // 品牌车型
            _self.displayCarInfo();
            $('#searchList').off('click').on('click',function () {
                $('.ad_table').html('');
                var LabelStatus = $('.tab_menu .selected').attr('value');
                var upData = _self.getParams();
                // console.log(upData)
                searchInfo = upData;
                var requestUrl = LabelUrl.InputListCar;//查询待打
                var temp = '#tagList';
                switch(LabelStatus){
                    case '-2':
                        break;
                    case '1003':
                        temp = '#tagList1';
                        requestUrl = LabelUrl.BatchListCar;//查询审核中和已审列表
                        break;
                    case '1004':
                        temp = '#tagList2';
                        requestUrl = LabelUrl.BatchListCar;//查询审核中和已审列表
                        break;
                }
                _self.searchList(requestUrl,temp,upData);//递归查询列表
            })
            $('#searchList').click();
            $('.tab_menu').off('click').on('click','li',function(){
                $(this).addClass('selected').siblings('li').removeClass('selected');
                _self.judgeDisplay();
                $('#searchList').click();

            })
            $('#reset').off('click').on('click',function (){
                _self.judgeDisplay();
            })
        },
        displayCarInfo:function(){//渲染车型数据
            var _self = this;
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
                        if($('.tab_menu .selected').attr('value') == '-2'){
                            str = '';
                        }
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
                        if(_self.GetRequest().isSearch && localStorage.SearchOptionForCar && flag==0){
                            var SearchOptionForCar = JSON.parse(localStorage.SearchOptionForCar);
                            //品牌和子品牌
                            if(SearchOptionForCar.MasterId){
                                $('.brand option').each(function(){
                                    if($(this).attr('MasterId') == SearchOptionForCar.MasterId){
                                        $(this).prop('selected',true);
                                    }
                                })
                                $('.brand').change();
                                $('.series option').each(function(){
                                    if($(this).attr('BrandId') == SearchOptionForCar.BrandID){
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
        },
        searchList:function(requestUrl,temp,upData){//查询并渲染列表
            var _self = this;
            var LabelStatus = $('.tab_menu .selected').attr('value');
            setAjax({
                url:requestUrl,
                type:'get',
                data:upData
            },function (data) {
                // console.log(data)
                if(data.Status==0){
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
                                    })
                                }
                            });
                            $('.ad_table').html(ejs.render($(temp).html(), {list:data.Result.List}));
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
            var _self = this;
            var LabelStatus = $('.tab_menu .selected').attr('value');
            switch(LabelStatus){
                case '-2':
                    //显示标签状态，不显示提交日期
                    $('.tagStatus').show();
                    $('.commitDate').hide();
                    break;
                case '1003':
                case '1004':
                    $('.tagStatus').hide();
                    $('.commitDate').show();
                    break;
            }
            //清空原来选择
            $('.brand').find('option:first').prop('selected',true);
            _self.displayCarInfo();
            $('.series').find('option:first').prop('selected',true);
            $('#tagStatus').find('option:first').prop('selected',true);
            $('#StartDate,#EndDate').val('');
        },
        getParams:function(){//获取查询参数
            var _self = this;
            if(_self.GetRequest().isSearch && localStorage.SearchOptionForCar && flag==0){
                flag = 1;
                var SearchOptionForCar = localStorage.SearchOptionForCar;
                //渲染切换项
                if(SearchOptionForCar.Status == 1003){
                    $('.tagStatus').hide();
                    $('.commitDate').show();
                   $('.tab_menu').find('li').eq(1).addClass('selected').siblings('li').removeClass('selected');
                }else if(SearchOptionForCar.Status == 1004){
                    $('.tagStatus').hide();
                    $('.commitDate').show();
                   $('.tab_menu').find('li').eq(2).addClass('selected').siblings('li').removeClass('selected');
                }else{
                    $('.tagStatus').show();
                    $('.commitDate').hide();
                   $('.tab_menu').find('li').eq(0).addClass('selected').siblings('li').removeClass('selected');
                }
                //品牌和子品牌
                if(SearchOptionForCar.MasterId){
                    $('.brand option').each(function(){
                        if($(this).attr('MasterId') == SearchOptionForCar.MasterId){
                            $(this).prop('selected',true);
                        }
                    })
                    $('.series option').each(function(){
                        if($(this).attr('BrandId') == SearchOptionForCar.BrandID){
                            $(this).prop('selected',true);
                        }
                    })
                }
                //账号名称
                $('.keyWord').val(SearchOptionForCar.Name);
                //标签状态
                $('#tagStatus option').each(function(){
                    if($(this).attr('value') == SearchOptionForCar.LabelStatus){
                        $(this).prop('selected',true);
                    }
                })
                //提交日期
                $('#StartDate').val(SearchOptionForCar.StartDate);
                $('#EndDate').val(SearchOptionForCar.EndDate);
                return JSON.parse(localStorage.SearchOptionForCar);

            }else{
                var BrandType = 1,BrandId = -2;
                if($('.brand option:checked').attr('MasterId') != '-2' && $('.series option:checked').attr('BrandId') != '-2'){
                    BrandType = 2;
                    BrandId = $('.series option:checked').attr('BrandId');
                }else{
                    BrandId = $('.brand option:checked').attr('MasterId');
                }
                return {
                    BrandType:BrandType,
                    MasterId:$('.brand option:checked').attr('MasterId'),
                    BrandID:$('.series option:checked').attr('BrandId'),
                    BrandId:BrandId,
                    LabelStatus:$('#tagStatus option:checked').attr('value')-0,
                    Status:$('.tab_menu .selected').attr('value')-0,
                    StartDate:$('#StartDate').val(),
                    EndDate:$('#EndDate').val(),
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
                _self.saveInfo();
                var BrandID = $(this).attr('BrandID'),
                    SerialID = $(this).attr('SerialID');
                window.location = '/TagManager/addTagForcar.html?BrandID='+BrandID+'&SerialID='+SerialID;
            })
            //重置
            $('#reset').off('click').on('click',function(){
                _self.judgeDisplay();
            })
        },
        saveInfo:function(){//存储查询条件信息
            var curPage = $('#pageContainer').find('.current:last').html();
            if(curPage == '下一页'){
                curPage = $('#pageContainer').find('.current:last').prev().html();
            }
            var SearchOptionForCar = searchInfo;
            SearchOptionForCar.PageIndex = curPage;
            localStorage.SearchOptionForCar = JSON.stringify(SearchOptionForCar);
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
    new CarLabelList();
})