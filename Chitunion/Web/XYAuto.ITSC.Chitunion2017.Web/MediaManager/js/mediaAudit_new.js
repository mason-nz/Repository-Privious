/*
* Written by:     wangcan
* function:       媒体审核——微信
* Created Date:   2017-05-04
* Modified Date:
*/
$(function () {
    // 时间控件
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
    // 切换
    $('.tab_menu li').off('click').on('click', function () {
        $(this).addClass('selected').siblings('li').removeClass('selected');
        displayDateAndRejectMsg($('#mediatype option:checked').attr('value'));
        //清空搜索条件
        $('#mediatype').find('option:first').prop('selected',true);
        $('#wechataccountID').val('');
        $('#printname').val('');
        $('#createDate').val('');
        $('#createDate').val('');
        $('.but_query').click();
    })
    /*账号、名称模糊查询*/
    $('#wechataccountID').off('keyup').on('keyup',function(){
        var val = $.trim($(this).val());
        if(val != ''){
            setAjax({
                url:"/api/ADOrderInfo/QueryWeChat_NumerOrName?v=1_1",
                type:'get',
                data:{
                    NumberORName:val,
                    AuditStatus:$('.tab_menu .selected').attr('dictid')
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
                    $('#wechataccountID').autocomplete({
                        source: result
                    })
                }
            });
        }
    })
    /*添加文本框聚焦事件，使其默认请求一次*/
    $('#wechataccountID').off('focus').on('focus',function(){
       var val = $.trim($(this).val());
        if(val == ''){
            setAjax({
                url:"/api/ADOrderInfo/QueryWeChat_NumerOrName?v=1_1",
                type:'get',
                data:{
                    NumberORName:val,
                    AuditStatus:$('.tab_menu .selected').attr('dictid')
                },
                dataType:'json'
            },function(data){
                if(data.Status != 0){
                    var availableArr = [];
                    $('#wechataccountID').autocomplete({
                        source: availableArr
                    })
                }
            });
        }
    })
    // 统计审核数量
    setAjax({
        url:'/api/Media/GetMediaAuditStatusCount?v=1_1',
        type:'get',
        data:{}
    },function (data) {
        $('#through_num').html(data.Result.Pass)
        $('#Pending').html(data.Result.Waitting)
        $('#Reject').html(data.Result.Reject)
    })
    $('.but_query').off('click').on('click', function () {
        function obj() {
            // 媒体类型
            var mediatype=$('#mediatype option:checked').attr('value');
            // 账号/名称
            var Key=$('#wechataccountID').val() == undefined ? "":$.trim($('#wechataccountID').val()).split(' ')[0]; ;
            // 提交人
            var SubmitUserName=$.trim($('#printname').val());
            // 时间
            var createDate=$.trim($('#createDate').val());
            var createDate1=$.trim($('#createDate1').val());
            // 获取
            var state = '';
            $('.tab_menu li').each(function () {
                if ($(this).attr('class') == 'selected') {
                    state = $(this).attr('dictid')-0
                }
            })
            return {
                MediaType:mediatype,
                Key:Key,
                SubmitUserName:SubmitUserName,
                StartDate:createDate,
                EndDate:createDate1,
                AuditStatus:state,
                PageSize:20
            }
        }
        console.log(obj());
        // 请求数据
        setAjax({
            url: '/api/Media/GetMediaListB?v=1_1',
            type: 'get',
            data: obj(),
        }, function (data) {
            // 如果数据为0显示图片
            if (data.Result.Total != 0) {
                //分页
                $("#pageContainer").pagination(
                    data.Result.Total,
                    {
                        items_per_page: 20, //每页显示多少条记录（默认为20条）
                        callback: function (currPage, jg) {
                            var dataObj = obj();
                            dataObj.pageIndex = currPage
                            setAjax({
                                url: '/api/Media/GetMediaListB?v=1_1',
                                type: 'get',
                                data: dataObj
                            }, function (data) {
                                // 渲染数据
                                $('.table table').html(ejs.render($('#mediaAudit_opt').html(), data));
                                displayDateAndRejectMsg($('#mediatype option:checked').attr('value'));
                                mediaOperate();
                            })
                        }
                    });
            } else {
                $('#pageContainer').html('<img src="/images/no_data.png" style="display: block;margin: 70px auto;">')
            }
            $('.table table').html(ejs.render($('#mediaAudit_opt').html(), data));
            displayDateAndRejectMsg($('#mediatype option:checked').attr('value'));
            mediaOperate();
        })
    })
    $('.but_query').click();
    function mediaOperate(){
        /*查看媒体*/
        $('.deatilSearch').off('click').on('click',function(){
            var mediaType = 14001;
            var mediaID = $(this).attr('MediaID');
            var Wx_Status = $('.tab_menu .selected').attr('dictid');
            window.open('/MediaManager/mediaview.html?MediaType='+ mediaType + '&MediaID=' + mediaID + '&Wx_Status=' + Wx_Status);
        })
        /*审核媒体*/
        $('.AuditSearch').off('click').on('click',function(){
            var mediaID = $(this).attr('mediaID');
            var mediaType = 14001;
            window.open('/MediaManager/mediaaudit.html?MediaType='+ mediaType + '&MediaID=' + mediaID+"&Wx_Status=43001");
        })
    }
    function displayDateAndRejectMsg(mediaType){
        switch($('.tab_menu .selected').attr('dictid')){
            case '43002':
                $('#dateCategory').html('审核时间');
                $('#timeCategory').html('审核时间');
                break;
            case '43001':
                $('#dateCategory').html('提交时间');
                $('#timeCategory').html('提交时间');
                break;
            case '43003':
                $('#dateCategory').html('审核时间');
                $('#timeCategory').html('审核时间');
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
                break;
            default :
                break;
        }
    }
})