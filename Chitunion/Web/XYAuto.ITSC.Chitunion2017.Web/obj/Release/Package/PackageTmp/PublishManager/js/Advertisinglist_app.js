//获取Url上的参数
function GetRequest() {
    var url = location.search; //获取url中"?"符后的字串
    var theRequest = new Object();
    if (url.indexOf("?") != -1) {
        var str = url.substr(1);
        strs = str.split("&");
        for (var i = 0; i < strs.length; i++) {

            theRequest[strs[i].split("=")[0]] = strs[i].split("=")[1];
        }
    }
    return theRequest;
};
// 获取当前时间
Date.prototype.format = function (fmt) {
    var o = {
        "M+": this.getMonth() + 1,                 //月份
        "d+": this.getDate(),                    //日
        "h+": this.getHours(),                   //小时
        "m+": this.getMinutes(),                 //分
        "s+": this.getSeconds(),                 //秒
        "q+": Math.floor((this.getMonth() + 3) / 3), //季度
        "S": this.getMilliseconds()             //毫秒
    };
    if (/(y+)/.test(fmt)) {
        fmt = fmt.replace(RegExp.$1, (this.getFullYear() + "").substr(4 - RegExp.$1.length));
    }
    for (var k in o) {
        if (new RegExp("(" + k + ")").test(fmt)) {
            fmt = fmt.replace(RegExp.$1, (RegExp.$1.length == 1) ? (o[k]) : (("00" + o[k]).substr(("" + o[k]).length)));
        }
    }
    return fmt;
};
$(function () {
    // 媒体列表跳转传值
    if (GetRequest().mediaName != undefined) {
        $('#MediaName').val(decodeURIComponent(GetRequest().mediaName));
    }

    /*=======================start判断不同角色渲染的搜索条件不同===========================*/
    // 运营角色
    if (CTLogin.RoleIDs == 'SYS001RL00001' || CTLogin.RoleIDs == 'SYS001RL00004') {
        // 状态
        $('#Searchstatus').html('<option PubStatus="-2">全部</option><option PubStatus="49002">待审核</option><option PubStatus="49004">已上架</option><option PubStatus="49005">已下架</option><option PubStatus="49003">已驳回</option>');
        // 隐藏添加app
        $('#addAppAdverty').hide();
        // 隐藏有效期
        $('#validity').hide();
    }
    // ae角色
    if (CTLogin.RoleIDs == 'SYS001RL00005') {
        // 状态
        $('#Searchstatus').html('<option PubStatus="-2">全部</option><option PubStatus="49002">待审核</option><option PubStatus="49004">已上架</option><option PubStatus="49005">已下架</option><option PubStatus="49003">已驳回</option>');
        // 显示添加app
        $('#addAppAdverty').show();
        // 隐藏媒体主名称/手机号
        $('#UserName').parent().hide();
    }
    // 媒体主角色
    if (CTLogin.RoleIDs == 'SYS001RL00003') {
        // 状态
        $('#Searchstatus').html('<option PubStatus="-2">全部</option><option PubStatus="49004">已上架</option><option PubStatus="49005">已下架</option>');
        // 显示添加app
        $('#addAppAdverty').show();
        // 隐藏媒体主名称/手机号
        $('#UserName').parent().hide();
    }
    /*========================end判断不同角色渲染的搜索条件不同==========================*/

    /*======================start联想搜索=========================*/
    // 运营广告列表传-2，ae和媒体广告列表传43002
    var AuditStatus;
    if (CTLogin.RoleIDs == 'SYS001RL00001' || CTLogin.RoleIDs == 'SYS001RL00004') {
        AuditStatus = -2;
    }
    if (CTLogin.RoleIDs == 'SYS001RL00005' || CTLogin.RoleIDs == 'SYS001RL00003') {
        AuditStatus = 43002;
    }
    // 媒体名称联想搜索
    $('#MediaName').off('keyup').on('keyup', function () {
        var val = $.trim($(this).val());

        if (val != '') {
            $.ajax({
                url: '/api/ADOrderInfo/QueryAPPByName?v=1_1',
                type: 'get',
                data: {
                    Name: val,
                    AuditStatus: AuditStatus
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
                            if (data.Result[i].Name.toUpperCase().indexOf(val.toUpperCase()) != -1) {
                                // availableArr.push(data.Result[i].Name+' '+data.Result[i].Number);
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
                url: '/api/ADOrderInfo/QueryAPPByName?v=1_1',
                type: 'get',
                data: {
                    Name: val?val:'-',
                    AuditStatus: AuditStatus
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
                            if (data.Result[i].Name.toUpperCase().indexOf(val.toUpperCase()) != -1) {
                                // availableArr.push(data.Result[i].Name+' '+data.Result[i].Number);
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
                            source: availableArr
                        });
                    }
                }
            });
        }

    });

    /*======================end联想搜索=========================*/
    // 封装获取的参数
    function parameter(i) {
        var data = {};
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
        var Searchstatus = $('#Searchstatus option:selected').attr('PubStatus') - 0;
        // 运营
        if (CTLogin.RoleIDs == 'SYS001RL00001' || CTLogin.RoleIDs == 'SYS001RL00004') {
            data = {
                MediaType: 14002,
                MediaName: MediaName,
                ADName: ADName,
                UserName: UserName,
                PubStatus: Searchstatus,
                IsAE: false,
                PageSize: 20,
                PageIndex: i
            };
            return data;
        }
        // ae或媒体
        if (CTLogin.RoleIDs == 'SYS001RL00005' || CTLogin.RoleIDs == 'SYS001RL00003') {
            data = {
                MediaType: 14002,
                MediaName: MediaName,
                ADName: ADName,
                PubStatus: Searchstatus,
                BeginTime: BeginTime,
                EndTime: EndTime,
                PageSize: 20,
                PageIndex: i
            };
            return data;
        }
    };
    // 点击搜索渲染页面
    $('#search').off('click').on('click', function () {
        // 运营(不同接口)
        if (CTLogin.RoleIDs == 'SYS001RL00001' || CTLogin.RoleIDs == 'SYS001RL00004') {

            // 请求数据
            setAjax({
                url: '/api/Publish/GetPublishListB?v=1_1',
                type: 'get',
                data: parameter(1)
            }, function (data) {
                $('#box').html(ejs.render($('#yunying').html(), data));

                // 如果数据为0显示图片
                if (data.Result.Total != 0) {
                    //分页
                    $("#pageContainer").pagination(
                        data.Result.Total,
                        {
                            items_per_page: 20, //每页显示多少条记录（默认为20条）
                            callback: function (currPage, jg) {
                                setAjax({
                                    url: '/api/Publish/GetPublishListB?v=1_1',
                                    type: 'get',
                                    data: parameter(currPage)
                                }, function (data) {
                                    $('#box').html(ejs.render($('#yunying').html(), data));
                                    // 调用操作
                                    Operation();
                                })
                            }
                        });

                } else {
                    $('#pageContainer').html('<img src="/images/no_data.png" style="display: block;margin: 70px auto;">')
                }
                // 调用操作
                Operation();
            });


            // 操作
            function Operation() {
                // 上下架
                $('.Upanddown').off('click').on('click', 'img', function () {


                        // 下架
                        if ($(this).attr('name') == 4) {
                            var PubIDs = $(this).attr('PubIDs') - 0;
                            var _this = $(this);
                            setAjax({
                                url: '/api/Publish/BatchAuditPublish?v=1_1',
                                type: 'post',
                                data: {
                                    "PubIDs": [PubIDs],
                                    "MediaType": 14002,
                                    "OpType": 4
                                }
                            }, function (data) {
                                if(data.Status==0) {
                                    _this.attr('name', 3).attr('src', '/ImagesNew/up.png').parent().parent().prev().html('已下架');
                                    _this.parent().siblings('.recommend').hide();
                                    _this.parent().nextAll('.show').show();
                                }else {
                                    layer.msg(data.Message,{time:400});
                                }
                            })

                            return false;
                        };
                    // 判断是否在有效期
                    if ($(this).attr('EndDate').split(' ')[0] > new Date().format('yyyy-MM-dd')) {
                        // 上架
                        if ($(this).attr('name') == 3) {
                            var PubIDs = $(this).attr('PubIDs') - 0;
                            var _this = $(this);
                            setAjax({
                                url: '/api/Publish/BatchAuditPublish?v=1_1',
                                type: 'post',
                                data: {
                                    "PubIDs": [PubIDs],
                                    "MediaType": 14002,
                                    "OpType": 3
                                }
                            }, function (data) {
                                if(data.Status==0) {
                                    _this.attr('name', 4).attr('src', '/ImagesNew/down.png').parent().parent().prev().html('已上架');
                                    _this.parent().siblings('.recommend').show();
                                    _this.parent().nextAll('.show').hide();
                                }else {
                                    layer.msg(data.Message,{time:400});
                                }
                            })
                            return false;
                        }
                    } else {
                        layer.msg('请操作有效的执行时间', {time: 1000});
                        return false;
                    }

                });
                // 删除
                $('.delete').each(function () {
                    $(this).off('click').on('click', function () {
                        var _this = $(this);

                        layer.confirm('您确认要删除该条排期及价格吗？删除后不可恢复？', {
                            time: 0 //不自动关闭
                            , btn: ['删除', '取消']
                            , yes: function (index) {
                                setAjax({
                                    url: '/api/Publish/DeletePublish?v=1_1',
                                    type: 'post',
                                    data: {
                                        PubID: _this.attr('pubids')-0
                                    }
                                }, function (data) {
                                    if (data.Status == 0) {
                                        _this.parents('tr').remove();
                                    } else {
                                        layer.msg(data.Message);
                                    }
                                })
                                layer.msg('操作成功', {time: 400});
                                layer.close(index);
                            }
                        });


                    })
                });
                // 价格查看
                $('.Priceview').each(function () {
                    $(this).off('click').on('click',function () {
                        window.open('/PublishManager/look_price.html?PubID='+$(this).attr('PubIDs')+'&CreateUserRole='+$(this).attr('createuserrole')+'&TemplateID='+$(this).attr('templateid'))
                    })
                });
                // 广告查看
                $('.Adview').each(function () {
                    $(this).off('click').on('click',function () {
                        window.open('/publishmanager/advTempDetail.html?AdTempId='+$(this).attr('templateid'))
                    })
                });

                // 推荐到首页
                $('.recommend').each(function () {
                    $(this).off('click').on('click',function () {
                        var _this=$(this);
                        setAjax({
                            url:'/api/recommend/add',
                            type:'post',
                            data:{
                                BusinessType:14002,
                                ADDetailID:_this.attr('name'),
                                TemplateID:_this.attr('templateid')
                            }
                        },function (data) {
                            if(data.Status==0){
                                _this.hide();
                            }else {
                                layer.msg('推荐到首页失败')
                            }
                        })
                    })
                });
                // 广告编辑
                $('.Advertisingeditor').each(function () {
                    $(this).off('click').on('click',function () {
                        window.location='/PublishManager/editPublishApp.html?PubID='+$(this).attr('pubids')
                    })
                });
            }
        };

        // 媒体主ae(不同接口)
        if (CTLogin.RoleIDs == 'SYS001RL00003') {
            // 媒体
            setAjax({
                url: '/api/Publish/GetADListB?v=1_1',
                type: 'get',
                data: parameter(1)
            }, function (data) {
                $('#box').html(ejs.render($('#meiti').html(), data));
                // 调用操作
                Mediaoperation();
                // 如果数据为0显示图片
                if (data.Result.Total != 0) {
                    //分页
                    $("#pageContainer").pagination(
                        data.Result.Total,
                        {
                            items_per_page: 20, //每页显示多少条记录（默认为20条）
                            callback: function (currPage, jg) {
                                setAjax({
                                    url: '/api/Publish/GetADListB?v=1_1',
                                    type: 'get',
                                    data: parameter(currPage)
                                }, function (data) {
                                    $('#box').html(ejs.render($('#meiti').html(), data));
                                    // 调用操作
                                    Mediaoperation();
                                })
                            }
                        });
                } else {
                    $('#pageContainer').html('<img src="/images/no_data.png" style="display: block;margin: 70px auto;">')
                }
            })
            // 操作
            function Mediaoperation() {
                // 显示添加价格和隐藏添加价格
                $('.increase_price').parent().each(function () {
                    $(this).mousemove(function () {
                        $(this).find('.increase_price').show();
                    }).mouseout(function () {
                        $(this).find('.increase_price').hide();
                    })
                });
                // 点击操作下面的上下架
                $('.Upanddown').off('click').on('click','img',function () {
                    var PubIDs = $(this).attr('PubIDs') - 0;
                    var _this = $(this);


                        // 下架
                        if ($(this).attr('name') == 4) {
                            setAjax({
                                url: '/api/Publish/BatchAuditPublish?v=1_1',
                                type: 'post',
                                data: {
                                    "PubIDs": [PubIDs],
                                    "MediaType": 14002,
                                    "OpType": 4
                                }
                            }, function (data) {
                                if(data.Status==0) {
                                    _this.attr('name', 3).attr('src', '/ImagesNew/up.png');
                                    var i = _this.attr('i');
                                    _this.parents('td').prev().find('div:eq(' + i + ')').html('已下架');
                                    _this.parent().nextAll('.show').show();
                                }else {
                                    layer.msg(data.Message,{time:400});
                                }
                            })

                            return false;
                        };
                    // 判断是否在有效期
                    if ($(this).attr('EndDate').split(' ')[0] > new Date().format('yyyy-MM-dd')) {
                        // 上架
                        if ($(this).attr('name') == 3) {
                            setAjax({
                                url: '/api/Publish/BatchAuditPublish?v=1_1',
                                type: 'post',
                                data: {
                                    "PubIDs": [PubIDs],
                                    "MediaType": 14002,
                                    "OpType": 3
                                }
                            }, function (data) {
                                if(data.Status==0) {
                                _this.attr('name', 4).attr('src', '/ImagesNew/down.png');
                                var i=_this.attr('i');
                                _this.parents('td').prev().find('div:eq('+i+')').html('已上架');
                                    _this.parent().nextAll('.show').hide();
                                }else {
                                    layer.msg(data.Message,{time:400});
                                }
                            })
                            return false;
                        }
                    }else {
                        layer.msg('不在执行周期内')
                    }

                });
                // 修改广告模板
                $('.increase_price div').each(function () {
                    $(this).off('click').on('click',function () {
                        var TemplateID=$(this).attr('name')-0;
                        var MediaID=$(this).attr('MediaID')-0;
                        window.location='/publishmanager/edit_template.html?MediaID='+MediaID+'&TemplateID='+TemplateID
                    })
                })
                // 删除广告模板
                $('.increase_price span').each(function () {
                    $(this).off('click').on('click',function () {
                        var TemplateID=$(this).attr('name')-0;
                        var _this=$(this);
                        layer.confirm('是否要删除该广告？删除后该广告下广告价格也将被删除', {
                            time: 0 //不自动关闭
                            , btn: ['删除', '取消']
                            , yes: function (index) {
                                setAjax({
                                    url:'/api/Template/ToDeleteAppTemplate',
                                    type:'get',
                                    data:{
                                        TemplateID:TemplateID
                                    }
                                },function (data) {
                                    if(data.Status==0){
                                        _this.parents('tr').remove();
                                    }
                                })
                                layer.close(index);
                                layer.msg('操作成功', {time: 400});
                            }
                        });
                    })
                });
                // 删除广告价格
                $('.delete').off('click').on('click',function () {
                    // 在当前td处于第几个
                    var tds=$(this).attr('i')-0;
                    var pubidleng=$(this).attr('pubidleng')
                    // 父元素td下的div有几个
                    var fudiv=$(this).parents('td').find('div').length;
                    // 获取广告状态
                    var adverts=$(this).parents('tr').find('td:eq(1)').attr('name') - 0;
                    var _this=$(this);
                    layer.confirm('您确认要删除该条排期及价格吗？删除后不可恢复？', {
                        time: 0 //不自动关闭
                        , btn: ['删除', '取消']
                        , yes: function (index) {
                           setAjax({
                               url:'/api/Publish/DeletePublish?v=1_1',
                               type:'post',
                               data:{
                                   PubID:_this.attr('PubIDs')-0
                               }
                           },function (data) {
                               if(data.Status==0) {
                                   if (fudiv > 1) {
                                       $('.removeor' + pubidleng).each(function () {
                                           $(this).children().each(function () {
                                               if ($(this).attr('i') == tds) {
                                                   $(this).remove();
                                               }
                                           })
                                       })
                                   } else {
                                       if (adverts == 48002) {
                                           _this.parents('tr').remove();
                                       } else {
                                           $('.removeor' + pubidleng).each(function () {
                                               $(this).children().html('--');
                                           })
                                       }
                                   }
                               }else {
                                   layer.msg(data.Message, {time: 400});
                               }
                           })
                            layer.msg('操作成功', {time: 400});
                            layer.close(index);

                        }
                    });

                });
                /*全选*/
                $('.box').on('change', '#Select', function () {
                    if ($(this).prop('checked')) {
                        $('.Radio').prop('checked', true);
                    } else {
                        $('.Radio').prop('checked', false);
                    }
                });
                $('.box').on('change', '.Radio', function () {
                    if ($('.Radio').length == $("input[name='checkbox']:checked").length) {
                        $('#Select').prop('checked', true);
                    } else {
                        $('#Select').prop('checked', false);
                    }
                });

                // 点击批量操作
                // 上架
                $('#batchUp').off('click').on('click',function () {
                    console.log($(this));
                    // 49004
                    var arr=[],error=false;
                    $('.Radio').each(function () {
                        if($(this).prop('checked')==true){
                            if($(this).attr('name')==49005){
                                arr.push($(this).attr('pubids')-0);
                            }else {
                                error=true;
                            }
                        }
                    });
                    if (error){
                        layer.msg('请选择状态为“已下架”的广告');
                        return false;
                    }else {
                        if(arr.length<=0){
                            layer.msg('请选择已下架的广告价格');
                            return;
                        }
                        setAjax({
                            url:'/api/Publish/BatchAuditPublish?v=1_1',
                            type:'post',
                            data:{
                                PubIDs:arr,
                                MediaType:14002,
                                OpType:3
                            }
                        },function (data) {
                            if(data.Status==0) {
                                $('#search').click();
                            }else {
                                layer.msg(data.Message,{time:400});
                                $('#search').click();
                            }
                        })
                    }
                });
                // 下架
                $('#batchdown').off('click').on('click',function () {
                    // 49004
                    var arr=[],error=false;
                    $('.Radio').each(function () {
                        if($(this).prop('checked')==true){
                            if($(this).attr('name')==49004){
                                arr.push($(this).attr('pubids')-0);
                            }else {
                                error=true;
                            }
                        }
                    });
                    if (error){
                        layer.msg('请选择状态为“已上架”的广告');
                        return false;
                    }else {
                        if(arr.length<=0){
                            layer.msg('请选择已上架的广告价格');
                            return
                        }
                        setAjax({
                            url:'/api/Publish/BatchAuditPublish?v=1_1',
                            type:'post',
                            data:{
                                PubIDs:arr,
                                MediaType:14002,
                                OpType:4
                            }
                        },function (data) {
                            if(data.Status==0) {
                                $('#search').click();
                            }else {
                                layer.msg(data.Message,{time:400});
                                $('#search').click();
                            }
                        })
                    }
                });

                // 价格查看
                $('.Priceview').each(function () {
                    $(this).off('click').on('click',function () {
                        window.open('/PublishManager/look_price.html?PubID='+$(this).attr('PubIDs')+'&TemplateID='+$(this).attr('templateid'))
                    })
                });

                // 广告编辑
                $('.Advertisingeditor').each(function () {
                    $(this).off('click').on('click',function () {
                        window.location='/PublishManager/editPublishApp.html?PubID='+$(this).attr('pubids')
                    })
                });
                // 增加价格
                $('.increase_price label').each(function () {
                    $(this).off('click').on('click',function () {
                        window.location='/PublishManager/addPublishForApp.html?MediaID='+$(this).parent().attr('mediaid')+'&PubID=0&TemplateID='+$(this).parent().attr('name')
                    })
                });

                // 广告查看
                $('.Adview').each(function () {
                    $(this).off('click').on('click',function () {
                        window.open('/publishmanager/advTempDetail.html?AdTempId='+$(this).attr('templateid'))
                    })
                })
            }
        }
        if(CTLogin.RoleIDs == 'SYS001RL00005'){
            // ae
            setAjax({
                url: '/api/Publish/GetADListB?v=1_1',
                type: 'get',
                data: parameter(1)
            }, function (data) {
                $('#box').html(ejs.render($('#ae').html(), data));
                // 调用操作
                Mediaoperation1();
                // 如果数据为0显示图片
                if (data.Result.Total != 0) {
                    //分页
                    $("#pageContainer").pagination(
                        data.Result.Total,
                        {
                            items_per_page: 20, //每页显示多少条记录（默认为20条）
                            callback: function (currPage, jg) {
                                setAjax({
                                    url: '/api/Publish/GetADListB?v=1_1',
                                    type: 'get',
                                    data: parameter(currPage)
                                }, function (data) {
                                    $('#box').html(ejs.render($('#ae').html(), data));
                                    // 调用操作
                                    Mediaoperation1();
                                })
                            }
                        });
                } else {
                    $('#pageContainer').html('<img src="/images/no_data.png" style="display: block;margin: 70px auto;">')
                }
            })
            // 操作
            function Mediaoperation1() {
                // 显示添加价格和隐藏添加价格
                $('.increase_price').parent().each(function () {
                    $(this).mousemove(function () {
                        $(this).find('.increase_price').show();
                    }).mouseout(function () {
                        $(this).find('.increase_price').hide();
                    })
                });
                // 点击操作下面的上下架
                $('.Upanddown').off('click').on('click','img',function () {
                    var PubIDs = $(this).attr('PubIDs') - 0;
                    var _this = $(this);

                        // 下架
                        if ($(this).attr('name') == 4) {
                            setAjax({
                                url: '/api/Publish/BatchAuditPublish?v=1_1',
                                type: 'post',
                                data: {
                                    "PubIDs": [PubIDs],
                                    "MediaType": 14002,
                                    "OpType": 4
                                }
                            }, function (data) {
                                if(data.Status==0){
                                    _this.attr('name', 3).attr('src', '/ImagesNew/up.png');
                                    var i=_this.attr('i');
                                    _this.parents('td').prev().find('div:eq('+i+')').html('已下架');
                                    _this.parent().nextAll('.show').show();
                                }else {
                                    layer.msg(data.Message,{time:400});
                                }

                            })

                            return false;
                        };
                    // 判断是否在有效期
                    if ($(this).attr('EndDate').split(' ')[0] > new Date().format('yyyy-MM-dd')) {
                        // 上架
                        if ($(this).attr('name') == 3) {
                            setAjax({
                                url: '/api/Publish/BatchAuditPublish?v=1_1',
                                type: 'post',
                                data: {
                                    "PubIDs": [PubIDs],
                                    "MediaType": 14002,
                                    "OpType": 3
                                }
                            }, function (data) {
                                if(data.Status==0){
                                    _this.attr('name', 4).attr('src', '/ImagesNew/down.png');
                                    var i=_this.attr('i');
                                    _this.parents('td').prev().find('div:eq('+i+')').html('已上架');
                                    _this.parent().nextAll('.show').hide();
                                }else {
                                    layer.msg(data.Message,{time:400});
                                }


                            })
                            return false;
                        }
                    }else {
                        layer.msg('不在执行周期内')
                    }

                });
                // 修改广告模板
                $('.increase_price div').each(function () {
                    $(this).off('click').on('click',function () {
                        var TemplateID=$(this).attr('name')-0;
                        var MediaID=$(this).attr('MediaID')-0;
                        window.location='/publishmanager/edit_template.html?MediaID='+MediaID+'&TemplateID='+TemplateID
                    })
                })
                // 删除广告模板
                $('.increase_price span').each(function () {
                    $(this).off('click').on('click',function () {
                        var TemplateID=$(this).attr('name')-0;
                        var _this=$(this);
                        layer.confirm('是否要删除该广告？删除后该广告下广告价格也将被删除', {
                            time: 0 //不自动关闭
                            , btn: ['删除', '取消']
                            , yes: function (index) {
                                setAjax({
                                    url:'/api/Template/ToDeleteAppTemplate',
                                    type:'get',
                                    data:{
                                        TemplateID:TemplateID
                                    }
                                },function (data) {
                                    if(data.Status==0){
                                        _this.parents('tr').remove();
                                    }
                                })
                                layer.msg('操作成功', {time: 400});
                                layer.close(index);

                            }
                        });
                    })
                });
                // 删除广告价格
                $('.delete').off('click').on('click',function () {
                    console.log(1);
                    // 在当前td处于第几个
                    var tds=$(this).attr('i')-0;
                    var pubidleng=$(this).attr('pubidleng')
                    // 父元素td下的div有几个
                    var fudiv=$(this).parents('td').find('div').length;
                    // 获取广告状态
                    var adverts=$(this).parents('tr').find('td:eq(1)').attr('name') - 0;
                    var _this=$(this);
                    layer.confirm('您确认要删除该条排期及价格吗？删除后不可恢复？', {
                        time: 0 //不自动关闭
                        , btn: ['删除', '取消']
                        , yes: function (index) {
                            setAjax({
                                url:'/api/Publish/DeletePublish?v=1_1',
                                type:'post',
                                data:{
                                    PubID:_this.attr('pubids')-0
                                }
                            },function (data) {
                                if(data.Status==0){
                                    if(fudiv>1){
                                        $('.removeor' + pubidleng).each(function () {
                                            $(this).children().each(function () {
                                                if($(this).attr('i')==tds){
                                                    $(this).remove();
                                                }
                                            })
                                        })
                                    }else {
                                        if(adverts==48002){
                                            _this.parents('tr').remove();
                                        }else {
                                            $('.removeor' + pubidleng).each(function () {
                                                $(this).children().html('--');
                                            })
                                        }
                                    }
                                }else {
                                    layer.msg(data.Message, {time: 400});
                                }
                            })
                            layer.msg('操作成功', {time: 400});
                            layer.close(index);
                        }
                    });

                });
                /*全选*/
                $('.box').on('change', '#Select', function () {
                    if ($(this).prop('checked')) {
                        $('.Radio').prop('checked', true);
                    } else {
                        $('.Radio').prop('checked', false);
                    }
                });
                $('.box').on('change', '.Radio', function () {
                    if ($('.Radio').length == $("input[name='checkbox']:checked").length) {
                        $('#Select').prop('checked', true);
                    } else {
                        $('#Select').prop('checked', false);
                    }
                });

                // 点击批量操作
                // 上架
                $('#batchUp').off('click').on('click',function () {
                    console.log($(this));
                    // 49004
                    var arr=[],error=false;
                    $('.Radio').each(function () {
                        if($(this).prop('checked')==true){
                            if($(this).attr('name')==49005){
                                arr.push($(this).attr('pubids')-0);
                            }else {
                                error=true;
                            }
                        }
                    });
                    if (error){
                        layer.msg('请选择状态为“已下架”的广告');
                        return false;
                    }else {
                        if(arr.length<=0){
                            layer.msg('请选择已下架的广告价格');
                            return
                        }
                        setAjax({
                            url:'/api/Publish/BatchAuditPublish?v=1_1',
                            type:'post',
                            data:{
                                PubIDs:arr,
                                MediaType:14002,
                                OpType:3
                            }
                        },function (data) {
                            if(data.Status==0) {
                                $('#search').click();
                            }else {
                                layer.msg(data.Message,{time:400});
                                $('#search').click();
                            }
                        })
                    }
                });
                // 下架
                $('#batchdown').off('click').on('click',function () {
                    // 49004
                    var arr=[],error=false;
                    $('.Radio').each(function () {
                        if($(this).prop('checked')==true){
                            if($(this).attr('name')==49004){
                                arr.push($(this).attr('pubids')-0);
                            }else {
                                error=true;
                            }
                        }
                    });
                    if (error){
                        layer.msg('请选择状态为“已上架”的广告');
                        return false;
                    }else {
                        if(arr.length<=0){
                            layer.msg('请选择已上架的广告价格');
                            return
                        }
                        setAjax({
                            url:'/api/Publish/BatchAuditPublish?v=1_1',
                            type:'post',
                            data:{
                                PubIDs:arr,
                                MediaType:14002,
                                OpType:4
                            }
                        },function (data) {
                            if(data.Status==0) {
                                $('#search').click();
                            }else {
                                layer.msg(data.Message,{time:400});
                                $('#search').click();
                            }
                        })
                    }
                });

                // 价格查看
                $('.Priceview').each(function () {
                    $(this).off('click').on('click',function () {

                        window.open('/PublishManager/look_price.html?PubID='+$(this).attr('PubIDs')+'&TemplateID='+$(this).attr('templateid'))
                    })
                });
                // 广告编辑
                $('.Advertisingeditor').each(function () {
                    $(this).off('click').on('click',function () {
                        window.location='/PublishManager/editPublishApp.html?PubID='+$(this).attr('pubids')
                    })
                })
                // 增加价格
                $('.increase_price label').each(function () {
                    $(this).off('click').on('click',function () {
                        window.location='/PublishManager/addPublishForApp.html?MediaID='+$(this).parent().attr('mediaid')+'&PubID=0&TemplateID='+$(this).parent().attr('name')
                    })
                });
                // 广告查看
                $('.Adview').each(function () {
                    $(this).off('click').on('click',function () {
                        window.open('/publishmanager/advTempDetail.html?AdTempId='+$(this).attr('templateid'))
                    })
                })
            }
        }
    });
    $('#search').click();
    // 添加广告
    $('#addAppAdverty').off('click').on('click',function () {
        $.openPopupLayer({
            name: "Reject",
            url: "addAppadvert.html",
            error: function (dd) {
                alert(dd.status);
            },
            success: function () {
                var id = null;
                var arr=[],arrid=[];
                /*微信号、微信名称模糊查询*/
                $('.mt10 input').off('keyup').on('keyup', function () {
                    $('.already_add').hide();
                    var val = $.trim($(this).val());

                    if (val != '') {
                        $.ajax({
                            url: '/api/ADOrderInfo/QueryAPPByName?v=1_1',
                            type: 'get',
                            data: {
                                Name: val,
                                AuditStatus:-5
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
                                        if (data.Result[i].Name.toUpperCase().indexOf(val.toUpperCase()) != -1) {
                                            // availableArr.push(data.Result[i].Name+' '+data.Result[i].Number);
                                            availableArr.push(data.Result[i].Name);
                                        };
                                        arr.push(data.Result[i].Name);
                                        arrid.push(data.Result[i].MediaID)
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
                                    $('#ui-id-1').css('z-index', 999999999999999);
                                }
                            }
                        });
                    }
                });
                /*添加文本框聚焦事件，使其默认请求一次*/
                $('.mt10 input').off('focus').on('focus', function () {
                    $('.already_add').hide();
                    var val = $.trim($(this).val());

                    if (val == '') {
                        $.ajax({
                            url: '/api/ADOrderInfo/QueryAPPByName?v=1_1',
                            type: 'get',
                            data: {
                                Name: val?val:'-',
                                AuditStatus: -5
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
                                        if (data.Result[i].Name.toUpperCase().indexOf(val.toUpperCase()) != -1) {
                                            // availableArr.push(data.Result[i].Name+' '+data.Result[i].Number);
                                            availableArr.push(data.Result[i].Name);
                                        };
                                     arr.push(data.Result[i].Name);
                                     arrid.push(data.Result[i].MediaID)
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
                                    $('#ui-id-1').css('z-index', 999999999999999);
                                }
                            }
                        });
                    }

                });
                // 关闭
                $('.close').off('click').on('click', function () {
                    $.closePopupLayer('Reject');
                });
                // 添加验证
                $('.but_data').off('click').on('click', function () {

                    if(arr.indexOf($('.mt10 input').val())!=-1){
                        var a=arr.indexOf($('.mt10 input').val());
                        id=arrid[a]
                    }

                    if (!id) {
                        $('.already_add').html('<img src="/ImagesNew/icon21.png" > 您没有添加该媒体，不可添加广告').show();
                        return false;
                    }

                    setAjax({
                        url: '/api/media/VerfiyOfAppTemplate?v=1_1',
                        type: 'POST',
                        data: {
                            MediaId: id
                        }
                    }, function (data) {

                        if (data.Status != 0) {
                            $('.already_add').html('<img src="/ImagesNew/icon21.png" > 该媒体暂未审核通过，不可添加广告').show();
                            return false;
                        } else {

                            if (data.Result.AdTemplateId > 0) {
                                window.location = '/PublishManager/addPublishForApp.html?MediaID=' + id + '&PubID=0&TemplateID=' + data.Result.AdTemplateId
                            } else {
                                // console.log('/PublishManager/add_template.html?BaseMediaID=' + data.Result.BaseMediaId + '&AppName=' + $.trim($('.mt10 input').val()) + '&MediaID=' + id);
                                // return false;
                                window.location = '/PublishManager/add_template.html?BaseMediaID=' + (data.Result.BaseMediaId?data.Result.BaseMediaId:-2) + '&AppName=' + $.trim($('.mt10 input').val())+'&MediaID='+id
                            }

                        }
                    })

                });
            }
        })
    })
})
