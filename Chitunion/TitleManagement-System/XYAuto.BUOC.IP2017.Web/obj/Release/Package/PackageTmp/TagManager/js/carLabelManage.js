/*
* Written by:     wangcan
* function:       车型标签管理
* Created Date:   2017-10-23
* Modified Date:
*/
$(function () {
    var LabelUrl = {
        QueryBrand:labelApi.tag+'/api/CarSerial/QueryBrand',//获取品牌
        BatchListCar:labelApi.tag+'/api/ResultLabel/QueryResultBrandList',//查询已审车型
        DeleteMediaLabel:labelApi.tag+'/api/ResultLabel/DeleteMediaLabel'//删除车型标签
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
                        if(GetRequest().isSearch && localStorage.SearchForCar && flag==0){
                            var SearchForCar = JSON.parse(localStorage.SearchForCar);
                            //品牌和子品牌
                            if(SearchForCar.MasterId){
                                $('.brand option').each(function(){
                                    if($(this).attr('MasterId') == SearchForCar.MasterId){
                                        $(this).prop('selected',true);
                                    }
                                })
                                $('.brand').change();
                                $('.series option').each(function(){
                                    if($(this).attr('BrandId') == SearchForCar.myBrandID){
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
            $('#searchList').off('click').on('click',function () {
                $('.ad_table').html('');
                var LabelStatus = $('.tab_menu .selected').attr('value');
                var upData = _self.getParams();
                // console.log(upData)
                searchInfo = upData;
                var requestUrl = LabelUrl.BatchListCar;//
                var temp = '#tagList';
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
        searchList:function(requestUrl,temp,upData){
            var _self = this;
            var LabelStatus = $('.tab_menu .selected').attr('value');
            setAjax({
                url:requestUrl,
                type:'get',
                data:upData
            },function (data) {
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
                                        _self.operate();
                                    })
                                }
                            });
                            $('.ad_table').html(ejs.render($(temp).html(), {list:data.Result.List}));
                            _self.operate();
                        }else{//没有返回数据
                            $('#pageContainer').hide();
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
            //清空原来选择
            $('.brand').find('option:first').prop('selected',true);
            $('.series').find('option:first').prop('selected',true);
            $('#tagStatus').find('option:first').prop('selected',true);
            $('#StartDate,#EndDate').val('');
        },
        getParams:function(){//获取查询参数
            var _self = this;
            if(GetRequest().isSearch && localStorage.SearchForCar && flag==0){
                flag = 1;
                var SearchForCar = localStorage.SearchForCar;
                //品牌和子品牌
                if(SearchForCar.MasterId){
                    $('.brand option').each(function(){
                        if($(this).attr('MasterId') == SearchForCar.MasterId){
                            $(this).prop('selected',true);
                        }
                    })
                    $('.series option').each(function(){
                        if($(this).attr('BrandId') == SearchForCar.myBrandID){
                            $(this).prop('selected',true);
                        }
                    })
                }
                //提交日期
                $('#StartDate').val(SearchForCar.StartDate);
                $('#EndDate').val(SearchForCar.EndDate);
                return JSON.parse(localStorage.SearchForCar);
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
                    BrandId:BrandId,
                    myBrandID:$('.series option:checked').attr('BrandId'),
                    StartDate:$('#StartDate').val(),
                    EndDate:$('#EndDate').val(),
                    PageIndex:1,
                    PageSize:20
                }
            }
        },
        operate:function(){//显示历史录入信息及打标签跳转
            var _self = this;
            //修改车型
            $('.modifyLabel').off('click').on('click',function(){
                _self.saveInfo();
                var BatchID = $(this).attr('BatchID');//批次ID
                var TaskType;
                if($('.brand option:checked').attr('value') != -2 && $('.series option:checked').attr('value') != -2){
                    TaskType = 2003;//有子品牌，即任务类型为车型
                }else{
                    TaskType = 2002;//任务类型为子品牌
                }
                window.location = '/TagManager/Modifycar.html?TaskType='+TaskType+'&BatchID='+BatchID+'&SelectType=2';
            })
            //重置
            $('#reset').off('click').on('click',function(){
                _self.judgeDisplay();
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
                            $('#searchList').click();
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
            var SearchForCar = searchInfo;
            SearchForCar.PageIndex = curPage;
            localStorage.SearchForCar = JSON.stringify(SearchForCar);
        }
    }
    new CarLabelList();
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
    }
})