/*
* Written by:     wangcan
* function:       媒体列表——微信
* Created Date:   2017-04-26
* Modified Date:
*/
    $(function(){
        //推荐到首页的行业分类：
        var curCategoryId = -2;
        if(GetRequest().categoryId){
            curCategoryId = GetRequest().categoryId;
        }
        /*默认列表渲染时的传递参数：根据角色传不同的值
        运营下：微信名称/账号、授权类型、媒体级别、常见分类、授权异常
        AE、媒体主和其他：微信名称/账号、常见分类、、创建日期
        */
        /*获取媒体类型*/
        var mediaType = $('.media_add_btn').attr('type-id');

        /*根据角色判断搜索条件显示*/
        //审核状态传值问题：AE下传43002，其他情况下传-2；如果是媒体主，有table切换，审核状态是什么就传什么
        //如果不是媒体主，table切换移除，
        //判断url上是否带参数categoryId,如果有，说明是从选择媒体进入，有排序。否则移除。

        //如果不是媒体主，移除table切换
        if(CTLogin.RoleIDs != 'SYS001RL00003'){
            $('.media_show').remove();
        }
        //判断角色，显示对应的筛选条件
        if(CTLogin.RoleIDs == 'SYS001RL00004' || CTLogin.RoleIDs == 'SYS001RL00001'){
            $('.query_AE').remove();
            $('.query_operate').show();
        }else{
            $('.query_operate').remove();
        }
        Recommend_wechat(curCategoryId);

        /*微信号、微信名称模糊查询*/
        $('.order_r #curAccount').off('keyup').on('keyup',function(){
            var val = $.trim($(this).val());

            //审核状态传值：AE下传43002，媒体主下，审核状态是几传几,运营下传-2；
            var AuditStatus;
            //AE
            if(CTLogin.RoleIDs == 'SYS001RL00005'){
                AuditStatus = 43002;
            //媒体主
            }else if(CTLogin.RoleIDs == 'SYS001RL00003'){
                AuditStatus = $('.media_show .selected').attr('dictid');
            //运营或超管
            }else if(CTLogin.RoleIDs == 'SYS001RL00001' || CTLogin.RoleIDs == 'SYS001RL00004'){
                AuditStatus = -2;
            }

            if(val != ''){
                setAjax({
                    url:"/api/ADOrderInfo/QueryWeChat_NumerOrName?v=1_1",
                    type:'get',
                    data:{
                        NumberORName:val,
                        AuditStatus:AuditStatus
                    },
                    dataType:'json'
                },function(data){
                    if(data.Status == 0){
                        var availableArr=[];
                        for(var i = 0;i<data.Result.length;i++){
                            if(data.Result[i].Name.toUpperCase().indexOf(val.toUpperCase())!=-1 ||data.Result[i].Number.toUpperCase().indexOf(val.toUpperCase())!=-1){
                                availableArr.push(data.Result[i].Number+' '+data.Result[i].Name);
                            }
                        }
                        /*将availableArr去重*/
                        var result = [];
                        for (var i = 0; i < availableArr.length; i++) {
                            if (result.indexOf(availableArr[i]) == -1) {
                                result.push(availableArr[i]);
                            }
                        }
                        $('.order_r #curAccount').autocomplete({
                            source: availableArr
                        })
                    }
                });
            }
        })

        /*添加文本框聚焦事件，使其默认请求一次*/
        $('.order_r #curAccount').off('focus').on('focus',function(){
            var val = $.trim($(this).val());
            //审核状态传值：AE下传43002，媒体主下，审核状态是几传几,运营下传-2；
            var AuditStatus;
            //AE
            if(CTLogin.RoleIDs == 'SYS001RL00005'){
                AuditStatus = 43002;
            //媒体主
            }else if(CTLogin.RoleIDs == 'SYS001RL00003'){
                AuditStatus = $('.media_show .selected').attr('dictid');
            //运营或超管
            }else if(CTLogin.RoleIDs == 'SYS001RL00001' || CTLogin.RoleIDs == 'SYS001RL00004'){
                AuditStatus = -2;
            }
            if(val == ''){
                setAjax({
                    url:"/api/ADOrderInfo/QueryWeChat_NumerOrName?v=1_1",
                    type:'get',
                    data:{
                        NumberORName:val,
                        AuditStatus:AuditStatus
                    },
                    dataType:'json'
                },function(data){
                    if(data.Status != 0){
                        var availableArr = [];
                        $('.order_r #curAccount').autocomplete({
                            source: availableArr
                        })
                    }
                });
            }
        })

        /*创建日期点击事件*/
        $('#createDate').off('click').on('click', function () {
            laydate({
                fixed: false,
                elem: '#createDate',
                max:laydate.now(+1),
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
                max: laydate.now(+1),
                choose: function (date) {
                    if (date < $('#createDate').val() && $('#createDate').val()) {
                        layer.alert('结束时间不能小于起始时间！');
                        $('#createDate1').val('')
                    }
                }
            });
        });

        /*切换审核状态*/
        $('.tab_menu').off('click').on('click','li',function(){
            $(this).addClass('selected').siblings().removeClass('selected');
            switch($(this).index()){
                //通过、待审核、已驳回
                //通过是创建日期，其他是提交日期
                case 0 :
                    $('#dateCategory_list').html('创建日期');
                    break;
                case 1 :
                    $('#dateCategory_list').html('提交日期');
                case 2 :
                default :
                    $('#dateCategory_list').html('提交日期');
                    break;
            }
            //清空搜索条件
            $('#curAccount').val('');
            $('#authType').find('option:first').prop('selected',true);
            $('#mediaLevel').find('option:first').prop('selected',true);
            $('.category').find('option:first').prop('selected',true);
            $('#AuthStatus').prop('checked',false);
            $('.searchList').click();
        })

        //区域媒体为是时，才能选择城市
        
        $('#IsAreaMedia').off('change').on('change',function(){
            if($('#IsAreaMedia option:checked').attr('value') == 1){
                $('#fgdq_1').find('option:eq(0)').prop('selected',true);
                $('#fgdq_2').find('option:eq(0)').prop('selected',true);
                $('.hidden_area').show();
            }else{
                $('#fgdq_1').find('option:eq(0)').prop('selected',true);
                $('#fgdq_2').find('option:eq(0)').prop('selected',true);
                $('.hidden_area').hide();
            }
        })
        //城市选择
        $(JSonData.masterArea).each(function (i) {
            $('#fgdq_1').append('<option i=' + i + ' value=' + this.id + '>' + this.name + '</option>')
        })
        $('#fgdq_1').off('change').on('change', function () {
            var City1 = '', City2 = '<option value="-2">城市</option>';
            $($(JSonData.masterArea)[$('#fgdq_1 option:checked').attr('i')].subArea).each(function (i) {
                City1 += '<option i=' + i + ' value=' + this.id + '>' + this.name + '</option>'
            })
            $('#fgdq_2').html(City2 + City1);
        })

        /*搜索*/
        $('.searchList').off('click').on('click',function(){
            function getObj(){
                /*验证账号/名称是否存在,存在则查询，不存在提示用户*/
                var key = $.trim($('#curAccount').val()).split(' ')[0]; 
                var AuditStatus;//审核状态 待审核：43001 已通过：43002 驳回：43003 默认：-2
                if(!$('.tab_menu').find('.selected').attr('dictid')){
                    //若非媒体主，无Table切换，审核状态传-2
                    AuditStatus = -2;
                }else{
                    //如果有table切换，审核状态传对应的值
                    AuditStatus = $('.tab_menu').find('.selected').attr('dictid')-0;
                }
                //如果是AE。审核状态传43002
                if(CTLogin.RoleIDs == 'SYS001RL00005'){
                    AuditStatus = 43002;
                }
                var CategoryID = $('.category option:checked').attr('dictid') == undefined ? -2 : $('.category option:checked').attr('dictid')-0,//分类ID 默认：-2
                    LevelType = $('#mediaLevel option:checked').attr('value') == undefined ? -2 : $('#mediaLevel option:checked').attr('value')-0,//媒体级别ID 意见领袖:4001 普通:4002 默认:-2
                    OAuthType = $('#authType option:checked').attr('value') == undefined ? -2 : $('#authType option:checked').attr('value') -0,//授权类型 扫码授权:38001 验证码：38002 手工：38003 默认：-2
                    OAuthStatus = $('#AuthStatus').prop('checked') == true ? 39002 : -2,//授权状态 异常：39002 默认：-2
                    StartDate = $('#createDate').val() == undefined ? "":$('#createDate').val(),
                    EndDate = $('#createDate1').val() == undefined ? "":$('#createDate1').val(),
                    IsAreaMedia = $('#IsAreaMedia option:checked').attr('value'),
                    AreaProvniceId = $('#fgdq_1 option:checked').attr('value'),//媒体省份ID
                    AreaCityId = $('#fgdq_2 option:checked').attr('value')//媒体城市ID
                var OrderBy;
                if((CTLogin.RoleIDs == 'SYS001RL00004' || CTLogin.RoleIDs == 'SYS001RL00001') && GetRequest().categoryId != undefined){
                    OrderBy =$('.sorting_h').find('a.active').attr('data-orderby');
                }else{
                    OrderBy = -2;
                }
                return {
                    MediaType:mediaType,
                    AuditStatus:AuditStatus,
                    key:key,
                    CategoryID:CategoryID,
                    LevelType:LevelType,
                    OAuthType:OAuthType,
                    OAuthStatus:OAuthStatus,
                    StartDate:StartDate,
                    EndDate:EndDate,
                    IsAreaMedia:IsAreaMedia,//是否是区域媒体
                    AreaProvniceId:AreaProvniceId,
                    AreaCityId:AreaCityId,
                    OrderBy:OrderBy,
                    PageSize:20
                }
            }
            //点击搜索时，修改当前所选行业分类的值
            curCategoryId = $('.category option:checked').attr('dictid');
            var objData = getObj();
            //console.log(objData);
            //调用接口得到各审核状态下的数量
            setAjax({
                url:'/api/Media/GetMediaAuditStatusCount?v=1_1',
                type:'get'
            },function(data){
                if(data.Status == 0){
                    $('#through_num').html(data.Result.Pass);
                    $('#waitAudit_num').html(data.Result.Waitting);
                    $('#reject_num').html(data.Result.Reject);
                }
            });
            var sysID = CTLogin.RoleIDs;
            var temp = '#tmpl1';//默认渲染第一个模板
            setAjax({
                url:'http://www.chitunion.com/api/Media/GetMediaListB?v=1_1',
                type:'get',
                data:objData
            },function(data){
                if(data.Status == 0){
                    //判断角色，显示对应的筛选条件
                    if(sysID == 'SYS001RL00004' || sysID == 'SYS001RL00001'){
                        temp = '#tmpl2';
                    }else if(sysID == 'SYS001RL00005'){
                        temp = '#tmplAE';
                    }

                    if(data.Result.List.length == 0){
                        $('#pageContainer').hide();
                    }else{
                        $('#pageContainer').show();
                        var counts = data.Result.Total;
                        $("#pageContainer").pagination(counts, {
                            items_per_page: 20, //每页显示多少条记录（默认为10条）
                            callback: function (currPage) {
                                objData.PageIndex = currPage;
                                //调用列表渲染接口，返回数据
                                setAjax({
                                    url:'http://www.chitunion.com/api/Media/GetMediaListB?v=1_1',
                                    type:'get',
                                    data:objData
                                },function(data){
                                    if(data.Status ==0){
                                        var str = $(temp).html();
                                        var html = ejs.render(str, {list: data.Result.List});
                                        $('.table').html(html);
                                        switch($('.tab_menu .selected').attr('dictid')){
                                            case '43002':
                                                $('.timeCategory_list').html('创建时间');
                                                break;
                                            case '43001':
                                                $('.timeCategory_list').html('提交时间');
                                                break;
                                            case '43003':
                                                $('.timeCategory_list').html('提交时间');
                                                break;
                                        }
                                        //如果有参数，显示“推荐到首页”和“排序”
                                        if(GetRequest().categoryId != undefined){
                                            $('.recommend').show();
                                            $('.sorting_h').show();
                                        }else{
                                            $('.recommend').remove();
                                            $('.sorting_h').remove();
                                        }
                                        lookRejectReason();
                                        deleteMedia();
                                        Recommend_wechat();
                                    }
                                })
                                
                            }
                        });
                    }
                    //渲染页面
                    var str = $(temp).html();
                    var html = ejs.render(str, {list: data.Result.List});
                    $('.table').html(html);
                    //鼠标滑过效果
                    //museoverToIcon();
                    //素材管理
                    sourceManger();
                    //查看权限
                    lookpersion();

                    switch($('.tab_menu .selected').attr('dictid')){
                        case '43002':
                            $('.timeCategory_list').html('创建时间');
                            break;
                        case '43001':
                            $('.timeCategory_list').html('提交时间');
                            break;
                        case '43003':
                            $('.timeCategory_list').html('提交时间');
                            break;
                    }
                    //如果有参数，显示“推荐到首页”和“排序”
                    if(GetRequest().categoryId != undefined){
                        $('.recommend').show();
                        $('.sorting_h').show();
                    }else{
                        $('.recommend').remove();
                        $('.sorting_h').remove();
                    }
                    lookRejectReason();
                    deleteMedia();
                    Recommend_wechat(curCategoryId);
                    
                }
            });
        })
        /*获取行业分类*/
        if(mediaType == '14001'){
            IndustryCategory(47);
        }

        /*排序开始*/
        var i=0;
        $('.sorting_h li a').off('click').on('click',function () {
            if($(this).attr('dataorderby')!=-1){
                i = 0;
            }
            if(i){
                $('.sorting_h li a').attr('class','').attr('dataOrderby','').children().attr('src','/ImagesNew/icon16_c.png')
                $(this).addClass('active').attr('dataOrderby','-1').children().attr('src','/ImagesNew/icon16_a.png')
                if($(this).attr('data-orderby')!=-2){
                    $(this).attr('data-orderby',$(this).attr('data-orderby').slice(0, -1)+'2')
                }
                i=0;
                var OrderBy=$(this).attr('data-orderby')-0;
                $('.searchList').click();
            }else {
                $('.sorting_h li a').attr('class','').attr('dataOrderby','').children().attr('src','/ImagesNew/icon16_c.png')
                $(this).addClass('active').attr('dataOrderby','-1').children().attr('src','/ImagesNew/icon16_b.png')
                if($(this).attr('data-orderby')!=-2) {
                    $(this).attr('data-orderby',$(this).attr('data-orderby').slice(0, -1)+'1')
                }
                i=1;
                var OrderBy=$(this).attr('data-orderby')-0
                $('.searchList').click();
            }
        })
        /*排序结束*/

    })


//渲染完数据之后  鼠标放上面显示icon
function museoverToIcon(){
    //鼠标滑过
    $('.table a').off('mousemove').on('mousemove',function () {
        var that = $(this);
        that.css('position','relative');
        that.find('span').css({'position':'absolute','right':'-66px','top':0,'width':'50px','z-inedx':'220'});
        that.find('span').show();
    })
    $('.table a').off('mouseleave').on('mouseleave',function () {
        var that = $(this);
        that.css('position','relative');
        that.find('span').css({'position':'absolute','right':'-66px','top':0,'width':'50px','z-inedx':'220'});
        that.find('span').hide();
    })
}


//素材管理   
function sourceManger(){
    $('.materialManage_btn').on('click',function(){
        $(this).attr('target','_blank');
        $(this).attr('href','/soucemanager/teletextmessage.html');
    })
}


/*添加广告*/
function addSearch(event) {
    var mediaID = $(event).parents('tr').attr('media-id');//附表
    var AuditStatus = $(event).parents('tr').attr('AuditStatus')*1;
    var OAuthType = $(event).parents('tr').attr('OAuthType')*1;
    var _Number = $(event).parents('tr').attr('Number');
    var WxID = $(event).parents('tr').attr('WxID');//基表
    if(AuditStatus == 43004){//假通过  补充资料页面 
        window.open('/MediaManager/addWeChatmedia.html?WxID=' + mediaID +'&AuthType=38001'+'&WxNumber='+_Number + '&NotRelly='+WxID + '&AuditStatus='+AuditStatus);
    }else if(AuditStatus == 43002){//真通过  添加广告页面
        window.open('/PublishManager/addEditPublishForWeiChat.html?MediaID=' + mediaID +'&entrance=1');
    }
}


/*查看*/
function detailSearch(event) {

    var mediaID = $(event).attr('media-id');
    var mediaType = $('.media_add_btn').attr('type-id');
    var AuditStatus = -2;

    //媒体主传审核状态  多个假通过状态
    if(CTLogin.RoleIDs == 'SYS001RL00003'){
        //AuditStatus = $('.media_show .selected').attr('dictid');
        AuditStatus = $(event).parent('tr').attr('AuditStatus');
    //AE传已通过43002
    }else if( CTLogin.RoleIDs == 'SYS001RL00005'){
        AuditStatus = 43002;
    }
    window.open('/MediaManager/mediaview.html?MediaType='+ mediaType + '&MediaID=' + mediaID +'&Wx_Status=' +AuditStatus);
}

//运营角色  查看权限
function lookpersion(){
    $('.look_permission').on('click',function(){
        var that = $(this);
        var WxID = that.parents('tr').attr('MediaID')*1;
        $.openPopupLayer({
            name: "popLayerDemo",
            url: "/MediaManager/look_popup.html",
            success:function(){
                setAjax({
                    url : '/api/Media/GetWeixinAuthorityList?v=1_1',
                    type : 'get',
                    data : {
                        'WxID' : WxID
                    }
                },function(data){
                    var Result = data.Result;
                    if(data.Status == 0){
                        var str = '<ul>';
                        Result.forEach(function(item){
                            str += '<li>'+ item + '</li>';
                        })
                        str += '</ul>';
                        $(".mb25").html(str);
                    }else{
                        layer.msg(data.Message,{'time':1000});
                    }
                })
                $('#closet').on('click',function(){
                    $.closePopupLayer('popLayerDemo');
                })
            }
        })
    })
}


/*获取行业分类,获取微信行业分类传20*/
function IndustryCategory(typeId) {
    setAjax({
        url: '/api/DictInfo/GetDictInfoByTypeID',
        type: 'get',
        data: {
            'TypeID': typeId
        },
    }, function (data) {
        var str = '';
        var res = data.Result;
        str += '<option DictID="-2">不限</option>';
        for (var i = 0; i < res.length; i++) {
            str += ' <option DictID="' + res[i].DictId + '">' + res[i].DictName + '</option>'
        }
        $('.category').html(str);
        if((CTLogin.RoleIDs == 'SYS001RL00004' || CTLogin.RoleIDs == 'SYS001RL00001') && GetRequest().categoryId != undefined){
            var categoryid = GetRequest().categoryId;
            var categoryOption = $('.category').find('option');
            categoryOption.each(function(){
                if($(this).attr('dictid') == categoryid){
                    $(this).prop('selected',true);
                }
            });
        }
        $('.searchList').click();
    })
}

/*查看驳回原因*/
function lookRejectReason(){
    $('.lookRejectMsg').off('click').on('click',function(){
        var RejectMsg = $(this).attr('RejectMsg');
        $.openPopupLayer({
            name: "popLayerDemo",
            url: "/MediaManager/resultPopup.html",
            success:function(){
                $(".mb25").html(RejectMsg);
                $('#closebt').off('click').on('click',function(){
                    $.closePopupLayer('popLayerDemo');
                })
                $('.button').off('click').on('click',function(){
                    $.closePopupLayer('popLayerDemo');
                })
            }
        })
    })
}


/*删除媒体*/
function deleteMedia(){
    $('.table').off('click').on('click','.media_delete_btn',function(){
        var _this = $(this);
        var mediaType =$('.media_add_btn').attr('type-id');
        var mediaID = _this.attr('media-id');
        var PublishState = _this.parents('tr').attr('PublishStatus');
        var hasOnPub = _this.parents('tr').attr('hasOnPub');
        var data = {
            MediaType:mediaType,
            MediaID:mediaID
        }
        /*判断媒体下是否有启用的刊例*/
        if(hasOnPub == 'false'){
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
                            $('.searchList').click();
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


/*编辑*/
function modifySearch(event) {
    //超管和AE传1
    if(CTLogin.RoleIDs == 'SYS001RL00005' || CTLogin.RoleIDs == 'SYS001RL00001' || CTLogin.RoleIDs == 'SYS001RL00004'){
        var wxae = 1;
    }
    if(CTLogin.RoleIDs == 'SYS001RL00004' || CTLogin.RoleIDs == 'SYS001RL00001'){
        var q = 1;
    }else{
        var q = 0;
    }
    var mediaID = $(event).attr('media-id');
    var mediaType = $('.media_add_btn').attr('type-id');
    var OAuthType = $(event).parents('tr').attr('OAuthType');
    var AuditStatus = $(event).attr('AuditStatus');
    window.open('/MediaManager/addWeChatmedia.html?MediaType=' + mediaType + '&wxae=' + wxae + '&WxID=' + mediaID +'&OAuthType=' + OAuthType +'&IsAuditPass=' +AuditStatus +'&q='+q);
}
/*编辑案例*/
function modifyCase(event) {
    var mediaID = $(event).attr('media-id');
    var mediaType = $('.media_add_btn').attr('type-id');
    window.open('/MediaManager/addcase.html?MediaType=' + mediaType + '&MediaID=' + mediaID);
}



/*审核*/
function auditSearch(event){
    var mediaID = $(event).attr('media-id');
    var mediaType = $('.media_add_btn').attr('type-id');
    window.location('/MediaManager/mediaaudit?MediaType='+ mediaType + '&MediaID=' + mediaID);
}
/*微信推荐到首页*/
function Recommend_wechat(curCategoryId) {
    $('.recommend').off('click').on('click',function () {
        var _this=$(this)
        setAjax({
            url:'/api/recommend/add',
            type:'POST',
            data: {
                MediaId:_this.attr('name'),
                BusinessType:14001,
                CategoryId:curCategoryId
            }
        },function (data) {
            if(data.Status!=0){
                layer.msg(data.Message)
                return
            }
            _this.hide();
            layer.msg('推荐成功',{time:500});
        })
    })
}
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

