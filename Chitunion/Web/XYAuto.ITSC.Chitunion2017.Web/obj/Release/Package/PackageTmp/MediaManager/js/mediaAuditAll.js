/*
* Written by:     wangcan
* function:       媒体审核——微信&APP
* Created Date:   2017-06-01
* Modified Date:
*/
/*
    1、媒体列表还使用原来的接口,APP使用新接口，我还没有修改接口url
    2、统计审核数量的接口是哪个
    3、页面不同了，原来是切换审核状态，现在是切换媒体类型，所以选择审核状态时会存在“全部”选项，此时列表应该是三种审核状态下所有的媒体
    4、
*/
$(function () {
  
    /*创建日期点击事件*/
    $('#startDate').off('click').on('click', function () {
        laydate({
            fixed: false,
            elem: '#startDate',
            choose: function (date) {
                if (date > $('#endDate').val() && $('#endDate').val()) {
                    layer.alert('起始时间不能大于结束时间！');
                    $('#startDate').val('')
                }
            }
        });
    });
    $('#endDate').off('click').on('click', function () {
        laydate({
            fixed: false,
            elem: '#endDate',
            choose: function (date) {
                if (date < $('#startDate').val() && $('#startDate').val()) {
                    layer.alert('结束时间不能小于起始时间！');
                    $('#endDate').val('')
                }
            }
        });
    });
    // 切换
    $('.tab_menu li').off('click').on('click',function () {
        $(this).addClass('selected').siblings('li').removeClass('selected');
        $('.but_query').click();
    })
    /*微信账号、名称模糊查询*/
    $('#searchForWeixin').off('keyup').on('keyup','.mediaAccount',function(){
        var val = $.trim($(this).val());
        if(val != ''){
            setAjax({
                url:"/api/ADOrderInfo/QueryWeChat_NumerOrName?v=1_1",
                type:'get',
                data:{
                    NumberORName:val,
                    AuditStatus:$('#AuditStatus option:checked').attr('value')
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
                    $('#searchForWeixin .mediaAccount').autocomplete({
                        source: result
                    })
                }
            });
        }
    })
    /*微信添加文本框聚焦事件，使其默认请求一次*/
    $('#searchForWeixin').off('focus').on('focus','.mediaAccount',function(){
       var val = $.trim($(this).val());
        if(val == ''){
            setAjax({
                url:"/api/ADOrderInfo/QueryWeChat_NumerOrName?v=1_1",
                type:'get',
                data:{
                    NumberORName:val,
                    AuditStatus:$('#AuditStatus option:checked').attr('value')
                },
                dataType:'json'
            },function(data){
                if(data.Status != 0){
                    var availableArr = [];
                    $('#searchForWeixin .mediaAccount').autocomplete({
                        source: availableArr
                    })
                }
            });
        }
    })

    /*APP账号、名称模糊查询*/
    $('#searchForApp').off('keyup').on('keyup','.mediaAccount',function(){
        var val = $.trim($(this).val());
        if(val != ''){
            setAjax({
                url:"/api/ADOrderInfo/QueryAPPByName?v=1_1",
                type:'get',
                data:{
                    Name:val,
                    AuditStatus:$('#AuditStatus1 option:checked').attr('value')
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
                    $('#searchForApp .mediaAccount').autocomplete({
                        source: result
                    })
                }
            });
        }
    })
    /*APP添加文本框聚焦事件，使其默认请求一次*/
    $('#searchForApp').off('focus').on('focus','.mediaAccount',function(){
       var val = $.trim($(this).val());
        if(val == ''){
            setAjax({
                url:"/api/ADOrderInfo/QueryAPPByName?v=1_1",
                type:'get',
                data:{
                    Name:val,
                    AuditStatus:$('#AuditStatus1 option:checked').attr('value')
                },
                dataType:'json'
            },function(data){
                if(data.Status != 0){
                    var availableArr = [];
                    $('#searchForApp .mediaAccount').autocomplete({
                        source: availableArr
                    })
                }
            });
        }
    })
    
    $('.but_query').off('click').on('click', function () {
        switch ($('.tab_menu .selected').attr('value')){
            case '14002':
                $('#searchForWeixin').hide();
                $('#searchForApp').show();
                function obj() {
                    // 媒体账号/名称
                    var MediaName=$('.mediaAccount').val() == undefined ? "":$.trim($('#searchForApp .mediaAccount').val()).split(' ')[0]; ;
                    // 媒体主名称/手机号
                    var mediaMaster=$.trim($('#mediaMaster').val());
                    // 运营者类型
                    var OperatingType=$.trim($('#OperatorType option:checked').attr('value'));
                    //媒体关系
                    var MediaRelations = $('#MediaRelations option:checked').attr('value');

                    // 审核状态
                    var AuditStatus = $('#AuditStatus1 option:checked').attr('value');
                    var IsPassed = $('#AuditStatus1 option:checked').attr('value') == 43002 ? true:false;
                    return {
                        businesstype:14002,
                        MediaName:MediaName,
                        SubmitUser:mediaMaster,
                        OperatingType:OperatingType,
                        MediaRelations:MediaRelations,
                        IsPassed:IsPassed,
                        AuditStatus:AuditStatus,
                        pageIndex:1,
                        PageSize:20,
                        IsAuditView:true
                    }
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
                                callback: function (currPage, jg) {
                                    var dataObj = obj();
                                    dataObj.pageIndex = currPage
                                    setAjax({
                                        url: '/api/media/backGQuery?v=1_1',
                                        type: 'get',
                                        data: dataObj
                                    }, function (data) {
                                        // 渲染数据
                                        $('.ad_table table').html(ejs.render($('#mediaAudit_APP').html(), data));
                                        mediaOperate();
                                    })
                                }
                            });
                    } else {
                        $('#pageContainer').html('<img src="/images/no_data.png" style="display: block;margin: 70px auto;">')
                    }
                    $('.ad_table table').html(ejs.render($('#mediaAudit_APP').html(), data));
                    mediaOperate();
                })
                break;
            case '14001':
                $('#searchForWeixin').show();
                $('#searchForApp').hide();
                function obj1() {
                    // 账号/名称
                    var Key=$('.mediaAccount').val() == undefined ? "":$.trim($('.mediaAccount').val()).split(' ')[0]; ;
                    // 提交人
                    var SubmitUserName=$.trim($('#submitName').val());
                    // 时间
                    var startDate=$.trim($('#startDate').val());
                    var endDate=$.trim($('#endDate').val());
                    // 审核状态
                    var state = $('#AuditStatus option:checked').attr('value');
                    return {
                        MediaType:14001,
                        Key:Key,
                        SubmitUserName:SubmitUserName,
                        StartDate:startDate,
                        EndDate:endDate,
                        AuditStatus:state,
                        PageSize:20
                    }
                }
                // 请求数据
                setAjax({
                    url: '/api/Media/GetMediaListB?v=1_1',
                    type: 'get',
                    data: obj1(),
                }, function (data) {
                    // 如果数据为0显示图片
                    if (data.Result.Total != 0) {
                        //分页
                        $("#pageContainer").pagination(
                            data.Result.Total,
                            {
                                items_per_page: 20, //每页显示多少条记录（默认为20条）
                                callback: function (currPage, jg) {
                                    var dataObj = obj1();
                                    dataObj.pageIndex = currPage
                                    setAjax({
                                        url: '/api/Media/GetMediaListB?v=1_1',
                                        type: 'get',
                                        data: dataObj
                                    }, function (data) {
                                        // 渲染数据
                                        $('.ad_table table').html(ejs.render($('#mediaAudit_Weixin').html(), data));
                                        mediaOperate();
                                    })
                                }
                            });
                    } else {
                        $('#pageContainer').html('<img src="/images/no_data.png" style="display: block;margin: 70px auto;">')
                    }
                    $('.ad_table table').html(ejs.render($('#mediaAudit_Weixin').html(), data));
                    mediaOperate();
                })
                break;
            default:
                $('#searchForWeixin').show();
                $('#searchForApp').hide();
                break;
        }

    })
    $('.but_query').click();
    function mediaOperate(){
        /*查看媒体*/
        $('.detailSearch').off('click').on('click',function(){
            var mediaType = $('.tab_menu .selected').attr('value');
            var mediaID = $(this).attr('MediaID');
            var Wx_Status = $('#AuditStatus option:checked').attr('value');
            switch(mediaType){
                case '14001':
                    window.open('/MediaManager/mediaview.html?MediaType='+ mediaType + '&MediaID=' + mediaID + '&Wx_Status=' + Wx_Status);
                    break;
                case '14002':
                    var BaseMediaID = $(this).attr('BaseMediaID');
                    window.open('/MediaManager/appSee.html?MediaType='+ mediaType + '&MediaId=' + mediaID + '&BaseMediaId=' + BaseMediaID+'&See=1');
                    break;
                default:
                    break;
            }
        })
        /*审核媒体*/
        $('.AuditSearch').off('click').on('click',function(){
            var mediaID = $(this).attr('mediaID');
            var mediaType = $('.tab_menu .selected').attr('value');
            switch(mediaType){
                case '14001':
                    window.open('/MediaManager/mediaaudit.html?MediaType='+ mediaType + '&MediaID=' + mediaID+"&Wx_Status=43001");
                    break;
                case '14002':
                    var BaseMediaID = $(this).attr('BaseMediaID');
                    window.open('/MediaManager/appAuditing.html?MediaType='+ mediaType + '&MediaId=' + mediaID);
                    break;
                default:
                    break;
            }
        })
    }
})