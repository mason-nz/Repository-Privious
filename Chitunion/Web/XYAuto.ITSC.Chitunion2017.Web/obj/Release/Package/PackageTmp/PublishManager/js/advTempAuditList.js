/*
* Written by:     wangcan
* function:       广告模板列表
* Created Date:   2017-06-02
* Modified Date:
*/
/*
    1、媒体名称和广告模板名称模糊查询时，显示什么*/

$(function () {
    // 切换审核状态
    $('.tab_menu li').off('click').on('click', function () {
        $(this).addClass('selected').siblings('li').removeClass('selected');
        //判断批量操作是否显示
        if($('.tab_menu .selected').attr('dictid') == 48001){
            //全选操作显示
            $('.allCheck').show();
        }else{
            $('.allCheck').hide();
        }
        $('#but_query').click();
        mediaOperate();
    })
    // 获取对应审核状态下的数量
    setAjax({
        url:'/api/Template/GetStatisticsCount',
        type:'get',
        data:{}
    },function (data) {
        if(data.Status == 0){
            $('#through_num').html(data.Result.AuditPassCount);
            $('#Pending').html(data.Result.AppendAuditCount);
            $('#Reject').html(data.Result.RejectNotPassCount);
        }
    });
    $('#but_query').off('click').on('click', function () {
        function obj() {
            // 媒体名称
            var MediaName = $.trim($('#mediaAccount').val()).split(' ')[0]; ;
            // 广告名称
            var AdTemplateName=$.trim($('#advName').val());
            // 提交人手机号
            var SubmitUser=$.trim($('#mediaMasterPhone').val());
            //审核状态
            var TemplateAuditStatus = $('.tab_menu .selected').attr('dictid');
            //是否通过
            var IsPassed = $('.tab_menu .selected').attr('dictid') == 48002 ? true:false;
            return {
                businessType:15000,
                TemplateAuditStatus:TemplateAuditStatus,
                MediaName:MediaName,
                AdTemplateName:AdTemplateName,
                SubmitUser:SubmitUser,
                IsPassed:IsPassed,
                pageIndex:1,
                PageSize:20
            }
        }
        setAjax({
            url: '/api/template/query',
            type: 'get',
            data: obj(),
        }, function (data) {
            if(data.Status ==0){
                if (data.Result.TotleCount != 0) {
                    $("#pageContainer").pagination(
                        data.Result.TotleCount,
                        {
                            items_per_page: 20,
                            callback: function (currPage, jg) {
                                var dataObj = obj();
                                dataObj.pageIndex = currPage
                                setAjax({
                                    url: '/api/template/query',
                                    type: 'get',
                                    data: dataObj
                                }, function (data) {
                                    $('.ad_table table').html(ejs.render($('#mediaAudit_opt').html(), data));
                                    mediaOperate();
                                })
                            }
                        });
                } else {
                    $('#pageContainer').html('<img src="/images/no_data.png" style="display: block;margin: 70px auto;">')
                }
                $('.ad_table table').html(ejs.render($('#mediaAudit_opt').html(), data));
                mediaOperate();
            }
        })
    })
    $('#but_query').click();
    function mediaOperate(){
        /*添加广告模板*/
        $('.but_add').off('click').on('click',function(){
            $.openPopupLayer({
                name: "addAdvTemp",
                url: "addAdvTempLayer.html",
                error: function (dd) {
                    alert(dd.Status);
                },
                success: function () {
                    // 关闭选择分类弹窗
                    $('.close').off('click').on('click', function () {
                        $.closePopupLayer('addAdvTemp');
                    })
                    //APP名称模糊查询
                    /*媒体名称模糊查询*/
                    $('#MediaName').off('keyup').on('keyup',function(){
                        var val = $.trim($(this).val());
                        if(val != ''){
                            setAjax({
                                url:"/api/ADOrderInfo/QueryAPPByName?v=1_1",
                                type:'get',
                                data:{
                                    Name:val,
                                    AuditStatus: -2
                                },
                                dataType:'json'
                            },function(data){
                                if(data.Status == 0){
                                    var availableArr=[];
                                    for(var i = 0;i<data.Result.length;i++){
                                        if(data.Result[i].Name.toUpperCase().indexOf(val.toUpperCase())!=-1){
                                            availableArr.push(data.Result[i].Name);
                                        }
                                    }
                                    /*将availableArr去重*/
                                    var result = [];
                                    for (var i = 0; i < availableArr.length; i++) {
                                        if (result.indexOf(availableArr[i]) == -1) {
                                            result.push(availableArr[i]);
                                        }
                                    }
                                    $('#MediaName').autocomplete({
                                        source: result
                                    })
                                }
                            });
                        }
                    })
                    /*添加文本框聚焦事件，使其默认请求一次*/
                    $('#MediaName').off('focus').on('focus',function(){
                       var val = $.trim($(this).val());
                        if(val == ''){
                            setAjax({
                                url:"/api/ADOrderInfo/QueryAPPByName?v=1_1",
                                type:'get',
                                data:{
                                    Name:val,
                                    AuditStatus: -2
                                },
                                dataType:'json'
                            },function(data){
                                if(data.Status != 0){
                                    var availableArr = [];
                                    $('#MediaName').autocomplete({
                                        source: availableArr
                                    })
                                }
                            });
                        }
                    })
                    // 点击提交
                    $('#allowBtn').off('click').on('click', function () {
                        var val = $.trim($('#MediaName').val());
                        if(val != ''){
                            setAjax({
                                url:"/api/ADOrderInfo/QueryAPPByName?v=1_1",
                                type:'get',
                                data:{
                                    Name:val,
                                    AuditStatus: -2
                                },
                                dataType:'json'
                            },function(data){
                                if(data.Status == 0){
                                    var availableArr=[];
                                    for(var i = 0;i<data.Result.length;i++){
                                        if(data.Result[i].Name.toUpperCase().indexOf(val.toUpperCase())!=-1){
                                            var obj = {
                                                Name:data.Result[i].Name,
                                                MediaID:data.Result[i].MediaID
                                            }
                                            availableArr.push(obj);
                                        }
                                    }
                                    var count = 0;
                                    for(var i=0;i<availableArr.length;i++){
                                        if(val != availableArr[i].Name){
                                            count++;
                                        }
                                    }
                                    if(count == availableArr.length){
                                        $('.layer').find('.tips').show().html('库中没有该媒体');
                                    }else{
                                        setAjax({
                                            url:'/api/media/VerfiyOfAppTemplate?v=1_1',
                                            type:'post',
                                            data:{
                                                MediaName:val
                                            }
                                        },function(data){
                                            if(data.Status == 0){
                                                /*if (data.Result.AdTemplateId > 0) {
                                                    window.location = '/PublishManager/addPublishForApp.html?MediaID=' + id + '&PubID=0&TemplateID=' + data.Result.AdTemplateId
                                                } else {*/
                                                    window.location = '/PublishManager/add_template.html?MediaID=-2&BaseMediaID=' + data.Result.BaseMediaId + '&AppName=' + val+'';
                                                // }
                                            }else{
                                                layer.msg(data.Message);
                                            }

                                        })
                                    }
                                }
                            });
                        }else{
                            $('.layer').find('.tips').show().html('请输入媒体名称');
                        }
                    })

                }
            });
        })
        /*查看*/
        $('.detailSearch').off('click').on('click',function(){
            var AdTempId = $(this).attr('TemplateID');
            var AdBaseTempId = $(this).attr('BaseAdID');
            window.open('/publishmanager/advTempDetail.html?AdTempId=' + AdTempId +'&AdBaseTempId=' +AdBaseTempId);
        })
        /*修改*/
        $('.media_edit_btn').off('click').on('click',function(){
            var TemplateID = $(this).attr('TemplateID');
            var AdBaseTempId = $(this).attr('BaseAdID');
            var BaseMediaId = $(this).attr('BaseMediaId');
            window.open('/PublishManager/edit_template.html?BaseMediaID='+BaseMediaId+'&TemplateID=' +TemplateID);
        })
        /*审核*/
        $('.AuditSearch').off('click').on('click',function(){
            var AdTempId = $(this).attr('TemplateID');
            var BaseMediaID = $(this).attr('BaseMediaID');
            var BaseAdID = $(this).attr('BaseAdID');
            //批量审核传1
            window.open('/publishmanager/advTempAudit.html?AdTempId='+AdTempId+'&BaseMediaId='+BaseMediaID+'&isMany=0');
        })

        /*批量审核*/
        $('#auditAll').off('click').on('click',function(){
            //判断，如果没有选择广告模板，不进行下面的操作
            if($('.onecheck:checked').length<1){
                layer.msg('请选择要审核的广告模板',{time:2000});
                return;
            }
            //判断所选的广告模板是否都在同一媒体下，如果不是，提示用户，否则，带着模板ID，跳转到审核页面
            //所选的媒体ID、模板ID
            var checkedMedia = [],checkedAdTempId = [];
            $('.onecheck').each(function(i,v){
                if($(v).prop('checked')){
                    checkedMedia.push($(v).attr('BaseMediaID'));
                    checkedAdTempId.push($(v).attr('TemplateID'));
                }
            });
            function equals(arr){
                var bool=true;
                for(var i = 1;i < arr.length; i++){
                    if(arr[i]!==arr[0]){
                        bool = false
                    }
                }
                return bool
            };
            var equals= equals(checkedMedia);
            if(equals){
                var AdTempId = checkedAdTempId.join(',');
                var BaseMediaId = checkedMedia[0];
                window.open('/publishmanager/advTempAudit.html?AdTempId='+AdTempId+'&BaseMediaId='+BaseMediaId+'&isMany=1');
            }else{
                layer.msg('选择的模板必须都在同一媒体下',{time:2000});
            }
        })

        /*全选*/
        $('.allCheck input').off('change').on('change',function(){
            //为选中状态时，判断可选input，其他input置为disable
            //全选时，只把可以选择的勾选，在点击批量审核时，再进行验证，是否都在同一媒体下，若不在，不跳转，提示
            if($(this).prop('checked')){
                $('.ad_table input[Enable="true"]').prop('checked',true);
            }else{
                $('.ad_table input[Enable="true"]').prop('checked',false);
            }
        })

        /*单选*/
        $('.onecheck').off('click').on('click',function(){
            if($('.onecheck[Enable="true"]').length == $('.onecheck:checked').length){
                $('.allCheck input').prop('checked',true);
            }else{
                $('.allCheck input').prop('checked',false);
            }
        })


    }
})
