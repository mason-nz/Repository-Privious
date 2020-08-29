/*
* Written by:     wangcan
* function:       标签管理列表
* Created Date:   2017-08-04
* Modified Date:
*/
/*
初次进入页面时，未打标签没有数据，点击领取，显示数据，再次进入之后，未打标签就可能有数据了
已打标签列表，向后台传递的数据需要调整，接口需要调整
*/
$(function () {
    getInitData();
    /*创建日期点击事件*/
    $('#createDate').off('click').on('click', function () {
        laydate({
            fixed: false,
            elem: '#createDate',
            choose: function (date) {
                if (date > $('#createDate1').val() && $('#createDate1').val()) {
                    layer.alert('起始时间不能大于结束时间！');
                    $('#createDate').val('')
                }
            }
        });
    });
    $('#createDate1').off('click').on('click', function () {
        laydate({
            fixed: false,
            elem: '#createDate1',
            choose: function (date) {
                if (date < $('#createDate').val() && $('#createDate').val()) {
                    layer.alert('结束时间不能小于起始时间！');
                    $('#createDate1').val('')
                }
            }
        });
    });
    $('#searchList').off('click').on('click', function () {
        function obj() {
            //项目类型
            var projectType = $('#projectType option:checked').attr('value');
            //关键词
            var keyWord = $.trim($('.keyWord').val());
            //审核状态
            var Status = $('.tab_menu .selected').attr('value')-0;
            //提交日期
            var submitBeginTime = $('#createDate').val();
            var submitEndTime = $('#createDate1').val();
            return {
                projectType:projectType,
                keyWord:keyWord,
                Status:Status,
                auditUserID:-2,//审核人UserID
                submitBeginTime:submitBeginTime || "1990-01-01",
                submitEndTime:submitEndTime || "1990-01-01",
                pageIndex:1,
                pageSize:20
            }
        }
        var tmp = '#tagList1';
        // 请求数据
        setAjax({
            url: 'http://www.chitunion.com/api/LabelTask/LabelListQuery?v=1_1',//标签任务列表接口json/tagList.json
            type: 'get',
            data: obj(),
        }, function (data) {
            if(data.Status == 0){
                // 如果数据为0显示图片
                if (data.Result.TotalCount != 0) {
                    $('.ad_table').html(ejs.render($(tmp).html(), {list: data.Result.List}));
                    if($('.tab_menu .selected').attr('value') == '63004' || $('.tab_menu .selected').attr('value') == '63005'){
                        $('.deleteCol').remove();
                    }
                    //分页
                    $("#pageContainer").pagination(
                        data.Result.TotalCount,
                        {
                            items_per_page: 20, //每页显示多少条记录（默认为20条）
                            callback: function (curPage, jg) {
                                var dataObj = obj();
                                dataObj.pageIndex = curPage
                                setAjax({
                                    url: 'http://www.chitunion.com/api/LabelTask/LabelListQuery?v=1_1',
                                    type: 'get',
                                    data: dataObj
                                }, function (data) {
                                    // 渲染数据
                                    $('.ad_table').html(ejs.render($(tmp).html(), {list: data.Result.List}));
                                    if($('.tab_menu .selected').attr('value') == '63004' || $('.tab_menu .selected').attr('value') == '63005'){
                                        $('.deleteCol').remove();
                                    }
                                })
                            }
                        });
                } else {
                    $('.ad_table').html('<table width="100%" border="0" cellspacing="1" cellpadding="0" bgcolor="#ddd">'+
                           '<thead>'+
                                '<tr>'+
                                    '<th width="30%">标题/媒体名称</th>'+
                                    '<th width="10%">提交时间</th>'+
                                    '<th width="10%" class="deleteCol">审核时间</th>'+
                                    '<th width="8%" class="deleteCol">审核人</th>'+
                                    '<th width="8%">操作</th>'+
                                '</tr>'+
                            '</thead>'+
                        '</table>');
                    if($('.tab_menu .selected').attr('value') == '63004' || $('.tab_menu .selected').attr('value') == '63005'){
                        $('.deleteCol').remove();
                    }
                    $('.ad_table').append('<img src="/images/no_data.png" style="display: block;margin: 70px auto;">');
                }
            }else{
                layer.msg(data.Message);
            }
        })
    })
    // 切换
    $('.tab_menu li').off('click').on('click', function () {
        $(this).addClass('selected').siblings('li').removeClass('selected');
        if($('.tab_menu .selected').attr('value') == '63003'){
            $('#search').hide();
            $('#getMask').show();
            getInitData();
        }else{
            $('#search').show();
            $('#getMask').hide();
            $('#projectType option:eq(0)').prop('selected',true);
            $('.keyWord').val('');
            $('#createDate,#createDate1').val('');
            $('#searchList').click();

        }
    })
    //领取任务
    $('#getMask').off('click').on('click',function(){
        if($('.ad_table tbody').find('tr').length == 0){
            setAjax({
                url:'http://www.chitunion.com/api/LabelTask/LabelReceiveTask',//领取任务接口json/ReceiveTask.json
                type:'get',
                data:{}
            },function(data){
                if(data.Status == 0){
                    $('.ad_table').html(ejs.render($('#tagList').html(), {list: data.Result}));
                }else{
                    layer.msg(data.Message);
                }
            })
        }else{
            layer.msg('有未完成的任务，不可领取')
        }
    })

    function getInitData() {
        setAjax({
            url:'http://www.chitunion.com/api/LabelTask/LabelListQuery?v=1_1',//标签任务列表接口json/tagList.json
            type:'get',
            data:{
                Status : 63003,
                auditUserID :-2,
                pageIndex:1,
                pageSize :20
            }
        },function(data){
            if(data.Status == 0){
                if(data.Result.List.length == 0){
                    $('.ad_table').html('<table width="100%" border="0" cellspacing="1" cellpadding="0" bgcolor="#ddd">'+
                           '<thead>'+
                                '<tr>'+
                                    '<th width="30%">标题/媒体名称</th>'+
                                    '<th width="10%">操作</th>'+
                                '</tr>'+
                            '</thead>');
                    $('.ad_table').append('<img src="/images/no_data.png" style="display: block;margin: 70px auto;">')
                }else{
                    $('.ad_table').html('');
                    $('.ad_table').html(ejs.render($('#tagList').html(), {list: data.Result.List}));
                }
            }
        })  
    }
})