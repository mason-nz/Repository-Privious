$(function () {

    var config = {};
    config.status = '42002';

    function weChatnews() {
        this.queryparameters();
        $('#searchBtn').click();
        this.dropsearch();
        // 时间控件
        $('#createDate').off('click').on('click', function () {
            laydate({
                fixed: false,
                elem: '#createDate',
                choose: function (date) {
                    if (date > $('#createDate1').val() && $('#createDate1').val()) {
                        layer.alert('起始时间不能大于结束时间！');
                        $('#createDate').val('');
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
                        $('#createDate1').val('');
                    }
                }
            });
        });
    }

    weChatnews.prototype = {
        constructor: weChatnews,
        // 获取查询参数
        queryparameters: function () {
            var _this = this;
            // 切换
            $('.tab_menu li').off('click').on('click', function () {
                $(this).addClass('selected').siblings('li').removeClass('selected');
                $('#searchBtn').click();
                config.status = $(this).attr('name');//获取状态
            })
            if(CTLogin.RoleIDs=='SYS001RL00004'){
                $('#Newssources1').show();//广告来源
                $('#statenews').html("<option value='42011,42012'>全部</option><option value='42011'>已上架</option><option value='42012'>已下架</option>");
            }else if(CTLogin.RoleIDs=='SYS001RL00005'){
                $('.numberAndName').find('span').text('所属媒体：');
            }
            // 搜索
            $('#searchBtn').off('click').on('click', function () {
                // 微信名称/账号
                var wechataccount = $.trim($('#wechataccount').val());
                // 广告名称
                var printname = $.trim($('#printname').val());
                //所属媒体
                //var belongMedia = $.trim($('#belongMedia').val());
                // 广告状态
                var statenews = $('#statenews option:checked').attr('value');
                // 广告来源
                var Newssources= $('#Newssources option:checked').attr('value');
                // 过期时间
                var expirationtime = $('#expirationtime option:checked').attr('value');
                //提交日期
                var StartDateTime = $.trim($('#createDate').val());
                var EndDateTime = $.trim($('#createDate1').val());

                // 获取通过、待审、驳回 的状态
                var state = '';
                $('.tab_menu li').each(function () {
                    if ($(this).attr('class') == 'selected') {
                        state = $(this).attr('name');
                    }
                })

                //初始化的所有的变量
                var obj = {};

                //默认已通过
                obj = {
                    pageIndex: 1,
                    pageSize: 20,
                    businesstype: 14001,
                    ADName: printname,
                    Keyword: wechataccount,
                    EndTime: expirationtime,
                    wx_Status: '-2' + ',' + state +',' + statenews,
                    IsPassed:true,
                    IsAuditView:false
                }

                if(expirationtime != -2){
                    obj.wx_Status = 42011;
                }

                $('.adStatus').show();//广告状态
                $('.expirationTime').show();//过期时间
                $('.submitTime').hide();//提交时间

                if(state==42001){//待审核
                    obj = {
                        pageIndex: 1,
                        pageSize: 20,
                        businesstype: 14001,
                        wx_Status: state,
                        ADName: printname,
                        Keyword: wechataccount,
                        IsPassed:false,
                        IsAuditView:false,
                        StartDateTime:StartDateTime,
                        EndDateTime:EndDateTime
                    }
                    $('.adStatus').hide();//广告状态
                    $('.expirationTime').hide();//过期时间
                    $('.submitTime').show();//提交时间
                }
                if(state==42003){//已驳回
                    obj = {
                        pageIndex: 1,
                        pageSize: 20,
                        businesstype: 14001,
                        wx_Status: state,
                        ADName: printname,
                        Keyword: wechataccount,
                        IsPassed:false,
                        IsAuditView:false,
                        StartDateTime:StartDateTime,
                        EndDateTime:EndDateTime
                    }
                    $('.adStatus').hide();//广告状态
                    $('.expirationTime').hide();//过期时间
                    $('.submitTime').show();//提交时间
                }
                if(CTLogin.RoleIDs=='SYS001RL00001'||CTLogin.RoleIDs=='SYS001RL00004'){
                    obj = {
                        pageIndex: 1,
                        pageSize: 20,
                        businesstype: 14001,
                        wx_Status: statenews,
                        Source: Newssources,
                        Keyword: wechataccount,
                        ADName: printname,
                        EndTime: expirationtime,
                        IsPassed:true,
                        IsAuditView:false
                    }
                    $('.tab_menu').hide();
                };
                if(CTLogin.RoleIDs=='SYS001RL00003'){
                    obj.wx_Status= statenews;
                    $('.tab_menu').hide();
                }
                if(CTLogin.RoleIDs=='SYS001RL00005'){
                    $('.tab_menu').hide();
                    $("#printname").parent().hide();
                    $("#addAd").hide();
                }
                obj.IsAreaMedia=$('#IsAreaMedia option:checked').attr('value');
                var AreaProvniceId = $('#fgdq_1 option:checked').attr('value');//媒体省份ID
                var AreaCityId = $('#fgdq_2 option:checked').attr('value');//媒体城市ID
                if($('#IsAreaMedia option:checked').attr('value')==1){
                    obj.AreaProvniceId=AreaProvniceId;
                    obj.AreaCityId=AreaCityId;
                }else {
                    obj.AreaProvniceId=-2;
                    obj.AreaCityId=-2;
                }
                _this.requestdata(obj);
            })
        },
        // 请求数据
        requestdata: function (obj) {
            var _this = this;
            setAjax({
                url:'http://www.chitunion.com/api/Publish/GetPublishStatisticsCount?v=1_1',
                type:'get',
                data:{}
            },function (data) {
                $('#Pending').html('待审核 '+data.Result.AppendAuditCount);
                $('#Reject').html('已驳回 '+data.Result.RejectNotPassCount);
            })

            var url = 'http://www.chitunion.com/api/Publish/AdQuery?v=1_1';
            setAjax({
                url: url,
                type: 'get',
                data: obj,
            }, function (data) {

                var Result = data.Result;
                config.Result = data.Result;

                // 渲染数据
                _this.renderdata(_this, config);
                _this.operation();

                // 如果数据为0显示图片
                if (data.Result.TotleCount != 0) {
                    //分页
                    $("#pageContainer").pagination(
                        data.Result.TotleCount,
                        {
                            items_per_page: 20, //每页显示多少条记录（默认为20条）
                            callback: function (currPage, jg) {
                                obj.pageIndex = currPage;
                                setAjax({
                                    url: 'http://www.chitunion.com/api/Publish/AdQuery?v=1_1',
                                    type: 'get',
                                    data: obj
                                }, function (data) {
                                    var Result = data.Result;
                                    // 渲染数据
                                    _this.renderdata(_this, data);
                                    _this.operation();
                                })
                            }
                        });
                } else {
                    $('#pageContainer').html('<img src="/images/no_data.png" style="display: block;margin: 70px auto;">')
                }
            })
        },
        // 渲染数据
        renderdata: function (_this, data) {
            if(CTLogin.RoleIDs=='SYS001RL00005'){
                $('.table table').html(ejs.render($('#Pricelist-ae').html(), data));
            }
            if(CTLogin.RoleIDs=='SYS001RL00001'||CTLogin.RoleIDs=='SYS001RL00004'){
                $('.table table').html(ejs.render($('#Pricelist-yy').html(), data));
            }
            if(CTLogin.RoleIDs=='SYS001RL00003'){
                $('.table table').html(ejs.render($('#Pricelist-mt').html(), data));
            }
        },
        // 操作
        operation: function () {

            $('.increase').hide();

            //默认收起  只展示两个
            showOrSlide();

            // 按钮添加广告
            $('#addAd').off('click').on('click', function () {
                window.location = '/PublishManager/addEditPublishForWeiChat.html?entrance=2'
            })

            //查看
            $('.look').off('click').on('click',function () {
                var _name = $(this).attr('name');
                $(this).attr('target','_blank');
                if($(this).attr("proprietary")){
                    $(this).attr('href','/ChannelManager/periodication-AeSee.html?CostID='  +_name+'&MediaType=14001');
                }else{
                    $(this).attr('href','/PublishManager/periodication-see.html?PubID='  +_name+'&MediaType=14001');
                }

            })

            // 删除
            $('.delete').off('click').on('click', function () {

                var that = $(this);
                var _this = that.attr("name");
                var idx =  that.parent().index();
                var len = that.parents('td').find('span').length;

                layer.confirm('确定要删除吗', {
                    btn: ['确定','取消'] //按钮
                }, function(){
                    setAjax({
                        url:'http://www.chitunion.com/api/Publish/DeletePublish?v=1_1',
                        type:'post',
                        data:{PubID:_this}
                    },function (data) {
                        layer.closeAll();
                        if(data.Status == 0){
                            if(len <= 1){
                                that.parents('tr').remove();//整行删除
                                that.parent('span').remove();
                            }else{
                                //删除对应的参考价 有效期 状态
                                that.parents('td').siblings('.ReferencePrice').find('span').eq(idx).remove();
                                that.parents('td').siblings('.usefulLife').find('span').eq(idx).remove();
                                that.parents('td').siblings('.allStatus').find('span').eq(idx).remove();
                                that.parent('span').remove();
                            }

                        }else {
                            layer.msg(data.Message);
                        }
                    })
                });
            })


            //上下架
            $('.Disableenable').each(function () {
                var that = $(this);
                var idx = that.parent('span').index();//索引
                var PubID = that.attr('name');//刊例ID
                var url = 'http://www.chitunion.com/api/Publish/AuditPublish?v=1_1';
                var selfTit = that.attr('title');//title
                that.off('click').on('click',function () {
                    if(selfTit == '下架'){
                        setAjax({
                            url: url,
                            type: 'post',
                            data: {
                                PubID : PubID,
                                OpType : 42012
                            }
                        }, function (data) {
                            if (data.Status == 0) {
                                layer.msg(data.Message, {time: 800});
                                that.parents('td').prev('.allStatus').find('span').eq(idx).html('已下架');
                                that.attr('title','上架');
                                //按钮切换
                                that.find('img').attr('src','/ImagesNew/up.png');
                                that.parent().find('a').each(function () {
                                    if($(this).css('display') === 'none'){
                                        $(this).css('display','inline-block');
                                    }
                                })
                                selfTit = that.attr('title');//从新获取
                            }else{
                                layer.msg(data.Message, {time: 800});
                            }
                        })
                    }else{//上架
                        setAjax({
                            url: url,
                            type: 'post',
                            data: {
                                PubID : PubID,
                                OpType : 42011
                            }
                        }, function (data) {
                            if (data.Status == 0) {
                                layer.msg(data.Message, {time: 800});
                                that.parents('td').prev('.allStatus').find('span').eq(idx).html('已上架');
                                that.attr('title','下架');
                                //按钮切换
                                that.find('img').attr('src','/ImagesNew/down.png');
                                that.parent().find('a').each(function () {
                                    if($(this).css('display') === 'inline-block'){
                                        $(this).css('display','none');
                                    }
                                })
                                selfTit = that.attr('title');//从新获取
                            }else{
                                layer.msg(data.Message, {time: 800});
                            }
                        })
                    }
                })
            })

            // 编辑1
            $('.edit').off('click').on('click',function () {
                //编辑的时候还要传媒体ID
                var MediaID = $(this).parents('tr').attr('name');
                window.location = '/PublishManager/addEditPublishForWeiChat.html?isAdd=1&PubID=' + $(this).attr('name') + '&entrance=2&MediaID='+MediaID;
            })

            // 复制2
            $('.copy').off('click').on('click', function () {
                window.location = '/PublishManager/addEditPublishForWeiChat.html?isAdd=2&PubID=' + $(this).attr('name') + '&entrance=2';
            })

            //展开  收起
            $('.allSpread').each(function () {
                var that = $(this);
                var showSlide = $('.allSpread').attr('title');
                that.off('click').on('click',function () {
                    if(showSlide == '展开'){
                        $(this).parents('tr').find('span').show();
                        $(this).attr('title','收起');
                        $(this).find('img').attr('src','/ImagesNew/icon88.png');
                        showSlide = $(this).attr('title');
                    }else{
                        showOrSlide();
                        $(this).attr('title','展开');
                        $(this).find('img').attr('src','/ImagesNew/icon87.png');
                        showSlide = $(this).attr('title');
                    }
                })
            })

            //查看一条媒体下所有的刊例信息
            $('.seeAllPub').off('click').on('click',function () {
                var name = $(this).parent().attr('name');
                $(this).attr('target','_blank');
                $(this).attr('href','/PublishManager/pricelist-wechatAllSee.html?MediaID=' + name + '&MediaType=14001');
            })

            //鼠标滑过显示添加价格  并且跳转
            $('.table>table>tbody>tr').each(function () {
                var that = $(this);//代表的是每一行
                that.find('td:eq(0)')
                    .on('mouseover',function () {
                        var _this = $(this);//代表的是第一个td
                        _this.find('.increase').show();
                        _this.find('.increase').on('click',function () {
                            //添加广告的页面
                            window.location = '/PublishManager/addEditPublishForWeiChat.html?MediaID=' + _this.attr('name') + '&entrance=2&increase=1';
                        })
                    })
                    .on('mouseleave',function () {
                        var _this = $(this);//代表的是第一个td
                        _this.find('.increase').hide();
                    })

            })

        },

        // 下拉搜索
        dropsearch: function () {
            /*微信号、微信名称模糊查询*/
            $('#wechataccount').off('keyup').on('keyup', function () {
                var val = $.trim($(this).val());
                if (val != '') {
                    setAjax({
                        url: "http://www.chitunion.com/api/Publish/SearchAutoComplete?v=1_1",
                        type: 'get',
                        data: {
                            Keyword: val,
                            businesstype: 14001
                        },
                        dataType: 'json'
                    }, function (data) {
                        if (data.Status == 0) {
                            var availableArr = [];
                            for (var i = 0; i < data.Result.length; i++) {
                                if (data.Result[i].Name.toUpperCase().indexOf(val.toUpperCase()) != -1 || data.Result[i].Number.toUpperCase().indexOf(val.toUpperCase()) != -1) {
                                    availableArr.push(data.Result[i].Name);
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

                            $('#wechataccount').autocomplete({
                                source: result
                            })
                        }
                    });
                }
            })
            /*添加文本框聚焦事件，使其默认请求一次*/
            $('#wechataccount').off('focus').on('focus', function () {
                var val = $.trim($(this).val());
                if (val == '') {
                    setAjax({
                        url: "http://www.chitunion.com/api/Publish/SearchAutoComplete?v=1_1",
                        type: 'get',
                        data: {
                            Keyword: val,
                            businesstype: 14001
                        },
                        dataType: 'json'
                    }, function (data) {
                        if (data.Status != 0) {
                            var availableArr = [];
                            $('#wechataccount').autocomplete({
                                source: availableArr
                            })
                        }
                    });
                }
            })
        }

    }
    var wechatnews = new weChatnews();


    //默认收起  只展开两个
    function showOrSlide () {
        $('table>tbody>tr').each(function () {
            var AdItemLen = $(this).attr('AdItemLen');
            if(AdItemLen > 2){
                //参考价
                $('.ReferencePrice').each(function () {
                    for(var i = 2;i< AdItemLen;i++){
                        $(this).find('span').eq(i).hide();
                    }
                })
                //有效期
                $('.usefulLife').each(function () {
                    for(var i = 2;i< AdItemLen;i++){
                        $(this).find('span').eq(i).hide();
                    }
                })
                //状态
                $('.allStatus').each(function () {
                    for(var i = 2;i< AdItemLen;i++){
                        $(this).find('span').eq(i).hide();
                    }
                })
                //操作
                $('.operation').each(function () {
                    for(var i = 2;i< AdItemLen;i++){
                        $(this).find('span').eq(i).hide();
                    }
                })
            }else{//小于2的时候隐藏折叠按钮
                $(this).find('.allSpread').hide();
            }
        })
    }

})
