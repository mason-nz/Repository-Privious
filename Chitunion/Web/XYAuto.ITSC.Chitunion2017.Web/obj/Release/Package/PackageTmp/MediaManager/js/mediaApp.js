/*
* Written by:     wangcan
* function:       媒体列表——APP
* Created Date:   2017-06-01
* Modified Date:
*/
/*
    1、媒体名称模糊查询调用哪个接口
    2、操作项，页面跳转及对应操作显示
    3、不同角色下显示是否正确
    4、接口给了之后，修改对应字段的显示
    5、APP列表不同审核状态下的数量显示调用哪个接口
    6、添加人也需要模糊查询
    7、列表查询接口url需要修改
*/
$(function () {
    //判断权限，显示“添加账号”
    if(CTLogin.BUTIDs.indexOf('SYS001BUT400501') == -1){
        $('.media_add_btn').remove();
    }
    //判断角色，显示搜索条件和table切换，并显示对应审核状态下的数目
    switch (CTLogin.RoleIDs){
        //AE：
        case 'SYS001RL00005' :
            $('.query_media').remove();
            $('.query_AE').show(); 
            $('.query_operate').remove();
            $('.query_MediaOwner').remove();
            $('.media_show').remove();
            break;
        //超管和运营
        case 'SYS001RL00001' :
        case 'SYS001RL00004' :
            //运营没有添加媒体
            // $('#addAppMedia').remove();
            $('.query_media').remove();
            $('.query_AE').remove(); 
            $('.query_operate').show();
            $('.query_MediaOwner').remove();
            $('.media_show').remove();
            break;
        default:
            //按媒体主：
            $('.query_media').show();
            $('.query_AE').remove(); 
            $('.query_operate').remove();
            $('.query_MediaOwner').show();
            $('.media_show').show();
            if($('.tab_menu .selected').attr('dictid') != 43002){
                $('table tr:first').find('td:last').prev().prev().remove();
            }
            // 统计审核数量
            setAjax({
                url:'/api/media/GetStatisticsCount?v=1_1',
                type:'get',
                data:{}
            },function (data) {
                if(data.Status == 0){
                    $('#through_num').html(data.Result.AuditPassCount);
                    $('#waitAudit_num').html(data.Result.AppendAuditCount);
                    $('#reject_num').html(data.Result.RejectNotPassCount);
                }
            })
            break;
    }
    // 切换
    $('.tab_menu li').off('click').on('click', function () {
        $(this).addClass('selected').siblings('li').removeClass('selected');
        //清空搜索条件
        $('#mediatype').find('option:first').prop('selected',true);
        $('#MediaName').val('');
        $('.MediaRelations').find('option:first').prop('selected',true);
        $('#AddRole').find('option:first').prop('selected',true);
        $('#addPerson').find('option:first').prop('selected',true);
        $('#searchList').click();
    })
    /*媒体名称模糊查询*/
    $('#MediaName').off('keyup').on('keyup',function(){
        var val = $.trim($(this).val());
        if(val != ''){
            setAjax({
                url:"/api/ADOrderInfo/QueryAPPByName?v=1_1",
                type:'get',
                data:{
                    Name:val,
                    AuditStatus:$('.tab_menu .selected').attr('dictid') || -2
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
                    Name:val?val:'-',
                    AuditStatus:$('.tab_menu .selected').attr('dictid') || -2
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
    
    $('#searchList').off('click').on('click', function () {
        function obj() {
            // 媒体名称
            var MediaName=$('#MediaName').val() == undefined ? "":$.trim($('#MediaName').val()).split(' ')[0]; ;
            //媒体关系
            var MediaRelations = $('.MediaRelations option:checked').attr('value')== undefined ? "" : $.trim($('.MediaRelations option:checked').attr('value'));
            //添加人角色
            var SubmitUserRole = $('#AddRole option:checked').attr('value') == undefined ? "" : $.trim($('#AddRole option:checked').attr('value'));
            //添加人
            var SubmitUser = $('#addPerson').val() == undefined ? "" : $.trim($('#addPerson').val());
            //审核状态
            var AuditStatus = $('.tab_menu .selected').attr('dictid');
            //是否通过审核
            var IsPassed = $('.tab_menu .selected').attr('dictid') == 43002 ? true:false;
            return {
                businesstype:14002,
                MediaName:MediaName,
                MediaRelations:MediaRelations,
                SubmitUserRole:SubmitUserRole,
                SubmitUser:SubmitUser,
                IsPassed:IsPassed,
                AuditStatus:AuditStatus,
                IsAuditView:false,//是否是审核页面(运营角色的情况下考虑)
                pageIndex:1,
                PageSize:20
            }
        }
        var tmp = '#tmp_mediaAE';
        if(CTLogin.RoleIDs == 'SYS001RL00001' || CTLogin.RoleIDs == 'SYS001RL00004'){
            tmp = '#tmp_opt';
        }
        // 请求数据
        setAjax({
            url: '/api/media/backGQuery?v=1_1',
            type: 'get',
            data: obj(),
        }, function (data) {
            // 如果数据为0显示图片
            if (data.Result.TotleCount != 0) {
                
                //分页
                $("#pageContainer").pagination(
                    data.Result.TotleCount,
                    {
                        items_per_page: 20, //每页显示多少条记录（默认为20条）
                        callback: function (curPage, jg) {
                            var dataObj = obj();
                            dataObj.pageIndex = curPage
                            setAjax({
                                url: '/api/media/backGQuery?v=1_1',
                                type: 'get',
                                data: dataObj
                            }, function (data) {
                                // 渲染数据
                                $('.ad_table table').html(ejs.render($(tmp).html(), {list: data.Result.List}));
                                mediaOperate();
                                //媒体主下，如果不是审核通过，不显示“上架/其余广告”
                                if(CTLogin.RoleIDs == 'SYS001RL00003' && $('.tab_menu .selected').attr('dictid') != 43002){
                                    $('table tr:first').find('th:last').prev().prev().remove();
                                }
                            })
                        }
                    });
            } else {
                $('#pageContainer').html('<img src="/images/no_data.png" style="display: block;margin: 70px auto;">')
            }
            $('.ad_table table').html(ejs.render($(tmp).html(), {list: data.Result.List}));
            mediaOperate();
            //媒体主下，如果不是审核通过，不显示“上架/其余广告”
            if(CTLogin.RoleIDs == 'SYS001RL00003' && $('.tab_menu .selected').attr('dictid') != 43002){
                $('table tr:first').find('th:last').prev().prev().remove();
            }
        })
    })
    $('#searchList').click();
    function mediaOperate(){
        /*查看媒体*/
        $('.detailSearch').off('click').on('click',function(){
            var mediaType = 14002;
            var mediaID = $(this).attr('MediaID');
            var BaseMediaID = $(this).attr('BaseMediaID');
            var AuditStatus = $(this).parents('tr').attr('AuditStatus');
            if(AuditStatus == 43003){
                window.open('/MediaManager/appSee.html?MediaId=' + mediaID + '&BaseMediaId=' + BaseMediaID+'&Reject=1');
            }else{
                window.open('/MediaManager/appSee.html?MediaId=' + mediaID + '&BaseMediaId=' + BaseMediaID);
            }
            
        })
        /*审核媒体*/
        $('.AuditSearch').off('click').on('click',function(){
            var mediaType = 14002;
            var mediaID = $(this).attr('mediaID');
            var BaseMediaID = $(this).attr('BaseMediaID');
            window.open('/MediaManager/appAuditing.html?&MediaId=' + mediaID + '&BaseMediaId=' + BaseMediaID);
        })
        //添加广告，判断当前媒体下是否有已审核通过的模板或自己添加的未通过的模板。若有：跳转到添加广告页面。若没有：跳转到添加模板页面

        $('.media_addAdv_btn').off('click').on('click',function(){
            var baseMediaid = $(this).attr('BaseMediaID');
            var MediaID = $(this).attr('MediaID');
            var mediaType = 14002;
            var AdTemplateId = $(this).attr('AdTemplateId');
            var Name=$(this).attr('Name');
            if(AdTemplateId > 0){
                //说明当前媒体下有已审核的模板
                window.open('/PublishManager/addPublishForApp.html?MediaID='+MediaID+'&PubID=0&TemplateID='+AdTemplateId);
            }else{
                //说明没有
                window.open('/PublishManager/add_template.html?MediaID='+MediaID+'&BaseMediaID='+baseMediaid+'&AppName='+encodeURIComponent(Name));
            }
            
        })
        //编辑
        $('.media_edit_btn').off('click').on('click',function(){
            var mediaType = 14002;
            var mediaID = $(this).attr('mediaID');
            var BaseMediaID = $(this).attr('BaseMediaID');
            //如果是运营、AE，直接跳
            if(CTLogin.RoleIDs == 'SYS001RL00004' || CTLogin.RoleIDs == 'SYS001RL00001' || CTLogin.RoleIDs == 'SYS001RL00005'){
                window.open('/MediaManager/addAppmedia.html?MediaId=' + mediaID + '&BaseMediaId=' + BaseMediaID+'&OperateType=2');
            }else{
                //需要判断要添加的媒体是否已在基库中存在，
                // a)已存在：跳转到补充资质页面addAppmedia02.html  
     
                // b)不存在：跳转到添加媒体、补充资质页面addAppmedia.html 
                if(BaseMediaID >0){
                    window.open('/MediaManager/addAppmedia02.html?MediaId=' + mediaID + '&BaseMediaId=' + BaseMediaID+'&OperateType=2');
                }else{
                    window.open('/MediaManager/addAppmedia.html?MediaId=' + mediaID + '&BaseMediaId=' + BaseMediaID+'&OperateType=2');
                }
            }
        })
        //编辑案例
        $('.media_editCase_btn').off('click').on('click',function(){
            var mediaType = 14002;
            var mediaID = $(this).attr('mediaID');
            var BaseMediaID = $(this).attr('BaseMediaID');
            window.open('/MediaManager/addcase_APP.html?MediaType='+ mediaType + '&MediaID=' + mediaID + '&BaseMediaID=' + BaseMediaID);
        })
        //删除
        $('.media_delete_btn').off('click').on('click',function(){
            var _this = $(this);
            var mediaID = $(this).attr('mediaID');
            var BaseMediaID = $(this).attr('BaseMediaID');
            var HasOnPub = _this.attr('HasOnPub');
            var data = {
                MediaType:14002,
                MediaID:mediaID
            }
            //如果是运营删除，传BaseMediaID
            if(CTLogin.RoleIDs == 'SYS001RL00001' || CTLogin.RoleIDs == 'SYS001RL00004'){
                data = {
                    MediaType:14002,
                    MediaID:BaseMediaID
                }
            }
            /*判断媒体下是否有启用的刊例*/
            if(HasOnPub == 0){
                layer.confirm('您确认删除此媒体吗', {
                    time: 0 //不自动关闭
                    , btn: ['确认', '取消']
                    , yes: function (index) {
                        setAjax({
                            url:'/api/Media/ToDeleteMedia?v=1_1',
                            type: 'get',
                            data: data
                        },function(data){
                            if(data.Status == 0){
                                _this.parents('tr').remove();
                                layer.close(index);
                                layer.msg('媒体删除成功', {time: 1500});
                                $('#searchList').click();
                            }else{
                                layer.msg(data.Message);
                            }
                        });
                    }
                });
            }else{
                layer.msg('该账号有已上架广告不能删除',{time:1500});
            }
        })
    }



    // 添加媒体
    $('#addAppMedia').off('click').on('click',function () {
        $.openPopupLayer({
            name: "clickApply",
            url: "addApp.html",
            error: function (dd) {
                alert(dd.status);
            },
            success: function () {
                var ad=-4;
                var arr=[];
                /*微信号、微信名称模糊查询*/
                $('.mt10 input').off('keyup').on('keyup', function () {
                    if(CTLogin.RoleIDs=="SYS001RL00001"||CTLogin.RoleIDs=="SYS001RL00004"){
                        ad=-2
                    }
                    $('.already_add').hide();
                    var val = $.trim($(this).val());
                    if (val != '') {
                            $('#ui-id-1').css('z-index',999999999999999)
                            $.ajax({
                                url: '/api/ADOrderInfo/QueryAPPByName?v=1_1',
                                type: 'get',
                                data: {
                                    Name: val?val:'-',
                                    AuditStatus:ad
                                },
                                dataType: 'json',
                                async: false,
                                xhrFields: {
                                    withCredentials: true
                                },
                                crossDomain: true,
                                success: function (data) {
                                    if (data.Status == 0) {
                                        if(data.Result==null){
                                            data.Result=[]
                                        }
                                        var availableArr = [];
                                        for (var i = 0; i < data.Result.length; i++) {
                                            if (data.Result[i].Name.toUpperCase().indexOf(val.toUpperCase()) != -1) {
                                                // availableArr.push(data.Result[i].Name+' '+data.Result[i].Number);
                                                availableArr.push(data.Result[i].Name);
                                                arr.push(data.Result[i].Name);
                                            }
                                        }
                                        /*将availableArr去重*/
                                        var result = [];
                                        for (var i = 0; i < availableArr.length; i++) {
                                            if (result.indexOf(availableArr[i]) == -1) {
                                                result.push(availableArr[i]);
                                            }
                                        }

                                        $('.mt10 input').autocomplete({
                                            source: availableArr
                                        })
                                        $('#ui-id-1').css('z-index',999999999999999)
                                    }
                                }
                            });
                        };
                });
                /*添加文本框聚焦事件，使其默认请求一次*/
                $('.mt10 input').off('focus').on('focus', function () {
                    if(CTLogin.RoleIDs=="SYS001RL00001"||CTLogin.RoleIDs=="SYS001RL00004"){
                        ad=-2
                    }
                    $('.already_add').hide();
                    var val = $.trim($(this).val());
                    if (val == '') {
                        $('#ui-id-1').css('z-index',999999999999999)
                        $.ajax({
                            url: '/api/ADOrderInfo/QueryAPPByName?v=1_1',
                            type: 'get',
                            data: {
                                Name: val?val:'-',
                                AuditStatus:ad
                            },
                            dataType: 'json',
                            async: false,
                            xhrFields: {
                                withCredentials: true
                            },
                            crossDomain: true,
                            success: function (data) {
                                if (data.Status == 0) {
                                    if(data.Result==null){
                                        data.Result=[]
                                    }
                                    var availableArr = [];
                                    for (var i = 0; i < data.Result.length; i++) {
                                        if (data.Result[i].Name.toUpperCase().indexOf(val.toUpperCase()) != -1) {
                                            // availableArr.push(data.Result[i].Name+' '+data.Result[i].Number);
                                       availableArr.push(data.Result[i].Name);
                                       arr.push(data.Result[i].Name);
                                        }
                                    }
                                    /*将availableArr去重*/
                                    var result = [];
                                    for (var i = 0; i < availableArr.length; i++) {
                                        if (result.indexOf(availableArr[i]) == -1) {
                                            result.push(availableArr[i]);
                                        }
                                    }

                                    $('.mt10 input').autocomplete({
                                        source: availableArr
                                    });
                                    $('#ui-id-1').css('z-index',999999999999999)
                                }
                            }
                        });
                    }

                });
                // 关闭
                $('.close').off('click').on('click',function () {
                    $.closePopupLayer('clickApply');
                });
                // 添加验证
                $('.keep').off('click').on('click',function () {

                    if(CTLogin.RoleIDs=="SYS001RL00001"||CTLogin.RoleIDs=="SYS001RL00004"){
                        if(arr.indexOf($('.mt10 input').val())!=-1){
                            layer.msg('该媒体已存在！')
                            return false;
                        } else {
                            window.location='/mediamanager/addAppmedia.html?MediaId='+0+'&OperateType=1&BaseMediaId='+0+'&appName='+$('.mt10 input').val()
                        }
                    }

                    setAjax({
                        url:'/api/media/VerifyOfAdd?v=1_1',
                        type:'POST',
                        data:{
                            MediaName:$.trim($('.mt10 input').val())
                        }
                    },function (data) {
                        // 不能添加
                        if(data.Result.ResultCode==1001){
                            $('.already_add').show();
                        }
                        // 可以添加资质信息
                        if(data.Result.ResultCode==1002){
                            window.location='/mediamanager/addAppmedia02.html?MediaId='+data.Result.MediaId+'&OperateType=1&BaseMediaId='+data.Result.BaseMediaId+'&appName='+$('.mt10 input').val()
                        }
                        // 添加媒体+资质信息
                        if(data.Result.ResultCode==1003){
                            window.location='/mediamanager/addAppmedia.html?MediaId='+data.Result.MediaId+'&OperateType=1&BaseMediaId='+data.Result.BaseMediaId+'&appName='+$('.mt10 input').val()
                        }
                        // 跳转编辑页面（角色是AE）
                        if(data.Result.ResultCode==1004){
                            var OperateType=2;
                            if(data.Result.BaseMediaId>0){
                                OperateType=1;
                            }
                            function appName() {
                                if (OperateType==1){
                                    return '&appName='+$('.mt10 input').val()+'&Rendering=1';
                                }else {
                                    return ' '
                                }
                            }
                            window.location='/mediamanager/addAppmedia.html?MediaId='+data.Result.MediaId+'&OperateType='+OperateType+'&BaseMediaId='+data.Result.BaseMediaId+appName();
                        }
                        // 跳转添加媒体页面
                        if(data.Result.ResultCode==1005){
                            window.location='/mediamanager/addAppmedia.html?MediaId='+data.Result.MediaId+'&OperateType=1&BaseMediaId='+data.Result.BaseMediaId+'&appName='+$('.mt10 input').val()
                        };

                    })
                })
            }
        })
    })
})