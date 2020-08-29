/*
* Written by:     wangcan
* function:       任务列表
* Created Date:   2017-08-04
* Modified Date:
*/
/*
    1、任务停止之后需要删除此行吗？任务停止操作需要提示用户吗？


*/
$(function () {
    
    getInitData();
    if(CTLogin.BUTIDs.indexOf('SYS001BUT6000402') == -1){
        $('.set_project').remove();
    }
    $('.set_project').off('click').on('click',function(){
        window.location = '/TagManager/SetTask.html';
    })

    function getInitData(){
        var url = 'http://www.chitunion.com/api/LabelTask/ProjectListQuery';//  json/task.json
        // 请求数据
        setAjax({
            url: url,
            type: 'get',
            data: {
                pageIndex : 1,
                pageSize : 20
            }
        }, function (data) {
            $('.ad_table tbody').html(ejs.render($('#taskList').html(), {list: data.Result.ProjectInfo}));
            // 如果数据为0显示图片
            if (data.Result.TotalCount != 0) {
                //分页
                $("#pageContainer").pagination(
                    data.Result.TotalCount,
                    {
                        items_per_page: 20, //每页显示多少条记录（默认为20条）
                        callback: function (curPage, jg) {
                            var dataObj = obj();
                            dataObj.pageIndex = curPage
                            setAjax({
                                url: url,
                                type: 'get',
                                data: {
                                    pageIndex : curPage,
                                    pageSize : 20
                                }
                            }, function (data) {
                                // 渲染数据
                                $('.ad_table tbody').html(ejs.render($('#taskList').html(), {list: data.Result.ProjectInfo}));
                                mediaOperate();
                            })
                        }
                    });
            } else {
                $('#pageContainer').html('<img src="/images/no_data.png" style="display: block;margin: 70px auto;">')
            }
            mediaOperate();
        })
    }

    function mediaOperate(){
        /*查看任务*/
        $('.detailSearch').off('click').on('click',function(){
            var curTr = $(this).parents('tr');
            var projectID = curTr.attr('projectID');
            var ProjectType = curTr.attr('ProjectType');
            if(ProjectType == 61001){//媒体
                window.open('/TagManager/ViewTasks.html?projectID=' + projectID);
            }else{//文章
                window.open('/TagManager/ViewTasks.html?projectID=' + projectID);
            }
        })
        $('.deleteTask').off('click').on('click',function(){
            var curTr = $(this).parents('tr');
            var projectID = curTr.attr('projectID');
            layer.confirm('确认删除此任务吗', {
                time: 0 //不自动关闭
                , btn: ['确认', '取消']
                , yes: function (index) {
                    setAjax({
                        url:'http://www.chitunion.com/api/LabelTask/LabelProjectStatus',//  json/info.json
                        type:'get',
                        data:{
                            projectID : projectID,
                            status : -1
                        }
                    },function(data){
                        if(data.Status == 0){
                            curTr.remove();
                            layer.msg('任务删除成功');
                            layer.close(index);
                            getInitData();
                        }else{
                            layer.msg('任务删除失败，请稍后重试');
                            layer.close(index);
                        }
                    })
                }
            });
            
        })
        $('.stopTask').off('click').on('click',function(){
            var curTr = $(this).parents('tr');
            var projectID = curTr.attr('projectID');
            layer.confirm('确认停止此任务吗', {
                time: 0 //不自动关闭
                , btn: ['确认', '取消']
                , yes: function (index) {
                    setAjax({
                        url:'http://www.chitunion.com/api/LabelTask/LabelProjectStatus',//json/info.json
                        type:'get',
                        data:{
                            projectID : projectID,
                            status : 62004
                        }
                    },function(data){
                        layer.close(index);
                        if(data.Status == 0){
                            //状态变为已停止之后，重新调数据，已停止的可删除
                            getInitData();
                        }else{
                            layer.msg('任务停止失败，请稍后重试');
                        }
                    })
                }
            });
        })
    }
})