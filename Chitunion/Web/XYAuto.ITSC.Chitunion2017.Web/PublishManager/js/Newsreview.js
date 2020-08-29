$(function () {
    // 时间控件
    $('#createDate').off('click').on('click', function () {
        laydate({
            fixed: false,
            elem: '#createDate',
            min:laydate.now(+1),
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
            min: laydate.now(+1),
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
        $(this).addClass('selected').siblings('li').removeClass('selected')
        $('.but_query').click();
    })
    $('.but_query').off('click').on('click', function () {
        // 获取
        var state = '';
        $('.tab_menu li').each(function () {
            if ($(this).attr('class') == 'selected') {
                state = $(this).attr('name')
            }
        })
        // if(state==14001){
        //     $('#app').hide();
        //     $('#wecha').show();
        //     function obj() {
        //         // 审核状态
        //         var mediatype=$('#mediatype option:checked').attr('value');
        //         // 名称/账号
        //         var wechataccount=$('#wechataccount').val();
        //         // 提交人
        //         var printname=$('#printname').val();
        //         // 时间
        //         var createDate=$('#createDate').val()
        //         var createDate1=$('#createDate1').val();
        //         // 获取
        //         var state = '';
        //         $('.tab_menu li').each(function () {
        //             if ($(this).attr('class') == 'selected') {
        //                 state = $(this).attr('name')
        //             }
        //         })
        //         if(state==14002){
        //             return false;
        //         }
        //         var IsPassed;
        //         if(state==42002){
        //             IsPassed=true;
        //             state = state ;
        //         }else {
        //             IsPassed=false;
        //         }
        //         return {
        //             businesstype:state,
        //             Keyword:wechataccount,
        //             printname:printname,
        //             StartDateTime:createDate,
        //             EndDateTime:createDate1,
        //             wx_Status:mediatype,
        //             IsPassed:IsPassed,
        //             IsAuditView:true
        //         }
        //     }
        //     // 请求数据
        //     setAjax({
        //         url: 'http://www.chitunion.com/api/Publish/AdQuery?v=1_1',
        //         type: 'get',
        //         data: obj(),
        //     }, function (data) {
        //         // 如果数据为0显示图片
        //         if (data.Result.TotleCount != 0) {
        //             //分页
        //             $("#pageContainer").pagination(
        //                 data.Result.TotleCount,
        //                 {
        //                     items_per_page: 20, //每页显示多少条记录（默认为20条）
        //                     callback: function (currPage, jg) {
        //                         var pageIndexobj=obj()
        //                         pageIndexobj.pageIndex = currPage
        //                         setAjax({
        //                             url: '/api/Publish/backGQuery?v=1_1',
        //                             type: 'get',
        //                             data: pageIndexobj
        //                         }, function (data) {
        //                             // 渲染数据
        //                             $('.table table').html(ejs.render($('#Pricelist-yy').html(), data));
        //                         })
        //                     }
        //                 });
        //         } else {
        //             $('#pageContainer').html('<img src="/images/no_data.png" style="display: block;margin: 70px auto;">')
        //         }
        //         $('.table table').html(ejs.render($('#Pricelist-yy').html(), data));
        //     })
        // }
        // if(state==14002){
        //
        // }
        $('#app').show();
        $('#wecha').hide();
        // 状态
        var Searchstatus = $('#Searchstatus option:selected').attr('PubStatus') - 0;
        // 媒体名称联想搜索
        $('#MediaName').off('keyup').on('keyup', function () {
            var val = $.trim($(this).val());

            if (val != '') {
                $.ajax({
                    url: 'http://www.chitunion.com/api/ADOrderInfo/QueryAPPByName?v=1_1',
                    type: 'get',
                    data: {
                        Name: val,
                        AuditStatus: Searchstatus
                    },
                    dataType: 'json',
                    async: false,
                    xhrFields: {
                        withCredentials: true
                    },
                    crossDomain: true,
                    success: function (data) {
                        if (data.Status == 0) {
                            var availableArr = [];
                            for (var i = 0; i < data.Result.length; i++) {
                                if (data.Result[i].Name.toUpperCase().indexOf(val.toUpperCase()) != -1 || data.Result[i].Number.toUpperCase().indexOf(val.toUpperCase()) != -1) {
                                    // availableArr.push(data.Result[i].Name+' '+data.Result[i].Number);
                                    availableArr.push(data.Result[i].Number);
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
                                source: availableArr
                            })
                        }
                    }
                });
            }
        });
        /*添加文本框聚焦事件，使其默认请求一次*/
        $('#MediaName').off('focus').on('focus', function () {
            var val = $.trim($(this).val());
            if (val == '') {
                $.ajax({
                    url: 'http://www.chitunion.com/api/ADOrderInfo/QueryAPPByName?v=1_1',
                    type: 'get',
                    data: {
                        Name: val,
                        AuditStatus: Searchstatus
                    },
                    dataType: 'json',
                    async: false,
                    xhrFields: {
                        withCredentials: true
                    },
                    crossDomain: true,
                    success: function (data) {
                        if (data.Status == 0) {
                            var availableArr = [];
                            for (var i = 0; i < data.Result.length; i++) {
                                if (data.Result[i].Name.toUpperCase().indexOf(val.toUpperCase()) != -1 || data.Result[i].Number.toUpperCase().indexOf(val.toUpperCase()) != -1) {
                                    // availableArr.push(data.Result[i].Name+' '+data.Result[i].Number);
                                    availableArr.push(data.Result[i].Number);
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
                                source: availableArr
                            });
                        }
                    }
                });
            }

        });

        function obj() {
            // 媒体名称
            var MediaName = $('#MediaName').val();
            // 广告名称
            var ADName = $('#ADName').val();
            // 媒体主名称/手机号
            var UserName = $('#UserName').val();
            // 开始时间
            var BeginTime = $('#BeginTime').val();
            // 结束时间
            var EndTime = $('#EndTime').val();
            // 状态
            // var Searchstatus = $('#mediatype1 option:selected').attr('pubstatus') - 0;
            return {
                MediaType: 14002,
                MediaName: MediaName,
                ADName: ADName,
                UserName: UserName,
                PubStatus: state,
                IsAE: true,
                PageSize: 20,
            }
        }
        // 请求数据
        setAjax({
            url: 'http://www.chitunion.com/api/Publish/GetPublishListB?v=1_1',
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
                            var pageIndexobj=obj()
                            pageIndexobj.pageIndex = currPage
                            setAjax({
                                url: 'http://www.chitunion.com/api/Publish/GetPublishListB?v=1_1',
                                type: 'get',
                                data: pageIndexobj
                            }, function (data) {
                                // 渲染数据
                                $('.table table').html(ejs.render($('#Pricelist-app').html(), data));
                            })
                        }
                    });
            } else {
                $('#pageContainer').html('<img src="/images/no_data.png" style="display: block;margin: 70px auto;">')
            }
            $('.table table').html(ejs.render($('#Pricelist-app').html(), data));
        })
    })
    $('.but_query').click();

})
