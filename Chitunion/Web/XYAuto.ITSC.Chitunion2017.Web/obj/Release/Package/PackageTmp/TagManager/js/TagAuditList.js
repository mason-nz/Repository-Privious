/*
* Written by:     wangcan
* function:       标签审核列表
* Created Date:   2017-08-07
* Modified Date:
*/

$(function () {
    getInitData();
    setAjax({
        url:'http://www.chitunion.com/api/LabelTask/GetAuditUser?v=1_1',
        type:'get',
        data:{}
    },function(data){
        if(data.Status == 0){
            var str = '<option value="-2">全部</option>';
            for(var i=0;i<data.Result.length;i++){
                var AuditUser = data.Result[i].UserName || data.Result[i].TrueName || data.Result[i].Mobile;
                str += '<option UserID='+data.Result[i].UserID+'>'+AuditUser+'</option>';    
            }
            $('#AuditPerson').html(str);
        }
    })
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
            var Status = $('.tab_menu .selected').attr('value');
            //审核人USERid
            var auditUserID = $('#AuditPerson option:checked').attr('UserID')?$('#AuditPerson option:checked').attr('UserID'):-2;
            //提交日期
            var auditBeginTime = $('#createDate').val();
            var auditEndTime = $('#createDate1').val();
            return {
                projectType:projectType,
                keyWord:keyWord,
                Status:Status,
                auditUserID:auditUserID,
                auditBeginTime:auditBeginTime || "1990-01-01",
                auditEndTime:auditEndTime || "1990-01-01",
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
                                })
                            }
                        });
                } else {
                    $('.ad_table').html('<table width="100%" border="0" cellspacing="1" cellpadding="0" bgcolor="#ddd">'+
                           '<thead>'+
                                '<tr>'+
                                    '<th width="30%">标题/媒体名称</th>'+
                                    '<th width="10%">提交时间</th>'+
                                    '<th width="10%">审核时间</th>'+
                                    '<th width="8%">审核人</th>'+
                                    '<th width="8%">操作</th>'+
                                '</tr>'+
                            '</thead>'+
                        '</table>');
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
        if($('.tab_menu .selected').attr('value') == '63005'){
            $('#projectType').find('option:first').prop('selected',true);
            $('.keyWord').val('');
            $('#AuditPerson').find('option:first').prop('selected',true);
            $('#createDate,#createDate1').val('');
            $('#search').hide();

            getInitData();
        }else{
            $('#search').show();
            $('#searchList').click();

        }
    })

    function getInitData() {
        setAjax({
            url:'http://www.chitunion.com/api/LabelTask/LabelListQuery?v=1_1',//标签任务列表接口
            type:'get',
            data:{
                projectType:-2,
                keyWord:"",
                Status:63005,
                auditUserID:-2,
                submitBeginTime:"1990-01-01",
                submitEndTime:"1990-01-01",
                pageIndex:1,
                pageSize:20
            }
        },function(data){
            if(data.Status == 0){
                if(data.Result.TotalCount == 0){
                    $('.ad_table').html('<table width="100%" border="0" cellspacing="1" cellpadding="0" bgcolor="#ddd">'+
                           '<thead>'+
                                '<tr>'+
                                    '<th width="30%">标题/媒体名称</th>'+
                                    '<th width="12%">提交时间</th>'+
                                    '<th width="10%">操作</th>'+
                                '</tr>'+
                            '</thead>');
                    $('.ad_table').append('<img src="/images/no_data.png" style="display: block;margin: 70px auto;">')
                }else{
                    $('.ad_table').html('');
                    $('.ad_table').html(ejs.render($('#tagList').html(), {list: data.Result.List}));
                    auditTask();
                    //分页
                    $("#pageContainer").pagination(data.Result.TotalCount,{
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
                                $('.ad_table').html(ejs.render($('#tagList').html(), {list: data.Result.List}));
                                auditTask();
                            })
                        }
                    });

                }
            }
        })  
    }
    function auditTask(){
        $('.AuditTask').off('click').on('click',function(){
            var ProjectType = $(this).attr('ProjectType');
            var TaskID = $(this).attr('TaskID');
            setAjax({
                url:'http://www.chitunion.com/api/LabelTask/LabelTaskAuditQuery',
                type:'get',
                data:{
                    taskID : TaskID
                }
            },function(data){
                if(data.Status == 0){
                    if(ProjectType == '61001'){
                        window.location = '/TagManager/AuditMedia.html?TaskID='+TaskID+'&SelectType=1';
                    }else{
                        window.location = '/TagManager/AuditArticle.html?TaskID='+TaskID+'&SelectType=1';
                    }
                    
                }else{
                    layer.msg('当前任务已经有人在审核，请选择其他任务审核');
                }
            })
        })
    }
})