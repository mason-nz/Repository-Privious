/**
 * Written by:     zhengxh
 * Created Date:   2017/3/2
 */
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


$(function () {
    if (CTLogin.RoleIDs == 'SYS001RL00005' || CTLogin.RoleIDs == 'SYS001RL00003') {
        $('div[name="operate-wechat"]').html(' ')
        $('div[name="operate-video"]').html(' ')
        $('div[name="operate-sina"]').html(' ')
        $('div[name="operate-zhibo"]').html(' ')
        $('div[name="operate-app"]').html(' ')
    }
    if (CTLogin.RoleIDs == 'SYS001RL00004' || CTLogin.RoleIDs == 'SYS001RL00001') {
        $('div[name="AE-wechat"]').html(' ')
        $('div[name="AE-video"]').html(' ')
        $('div[name="AE-sina"]').html(' ')
        $('div[name="AE-zhibo"]').html(' ')
        $('div[name="AE-app"]').html(' ')
    }
})
$(function () {
    // 申请上架url地址
    var Ajaxurl = "/api/Publish/ModifyPublishStatus";
    // 查询url地址
    var seleceurl = "/api/Publish/backGQuery";
    // 获取当前时间
    Date.prototype.format = function(fmt) {
        var o = {
            "M+" : this.getMonth()+1,                 //月份
            "d+" : this.getDate(),                    //日
            "h+" : this.getHours(),                   //小时
            "m+" : this.getMinutes(),                 //分
            "s+" : this.getSeconds(),                 //秒
            "q+" : Math.floor((this.getMonth()+3)/3), //季度
            "S"  : this.getMilliseconds()             //毫秒
        };
        if(/(y+)/.test(fmt)) {
            fmt=fmt.replace(RegExp.$1, (this.getFullYear()+"").substr(4 - RegExp.$1.length));
        }
        for(var k in o) {
            if(new RegExp("("+ k +")").test(fmt)){
                fmt = fmt.replace(RegExp.$1, (RegExp.$1.length==1) ? (o[k]) : (("00"+ o[k]).substr((""+ o[k]).length)));
            }
        }
        return fmt;
    }
    // AE
    if (CTLogin.RoleIDs == 'SYS001RL00005' || CTLogin.RoleIDs == 'SYS001RL00003') {
        // 微信公共号刊例列表
        if ($('div[name="AE-wechat"]').attr('name') == 'AE-wechat') {
            $('div[name="AE-wechat"]').css({display: 'block'})
            function wxpublicFunction() {
                judgeAuthority(1)
                // 点击申请上架
                $('tbody tr').find('td:last a[name^=15002]').off('click').on('click', function () {
                    var data = $(this).attr('name').split(',')
                    if(Date.parse(new Date().format("yyyy/MM/dd")) - Date.parse($(this).parent().attr('name').split(' ')[0].split('-').join('/')) < 0 ){
                    $.openPopupLayer({
                        name: "clickApply",
                        url: "pricelist-application.html",
                        error: function (dd) {
                            alert(dd.status);
                        },
                        success: function () {
                            // 点击确定
                            $('.layer_con').find('.button').off('click').on('click', function () {
                                setAjax({
                                    url: Ajaxurl,
                                    type: "POST",
                                    data: {
                                        "PubID": data[1],
                                        "Status": data[0]
                                    }
                                }, function (data) {
                                    $.closePopupLayer('clickApply');
                                    if (data.Result) {
                                        window.location.reload();
                                    }
                                })
                            })                 // 点击取消
                                .end().find('.but_keep').off('click').on('click', function () {
                                $.closePopupLayer('clickApply')
                            })

                        }
                    });
                    }else {
                        alert('请调整刊例时间！')
                    }
                })
                // 上下架切换
                $('tbody tr').find('td:last span a').off('click').on('click', function (e) {
                    if(e.target.innerHTML=='编辑'){
                        return
                    }
                    var $this = $(this);
                    var data1 = $this.attr('name').split(',');
                    if($this.text()=='下架'){
                        setAjax({
                            url: Ajaxurl,
                            type: "POST",
                            data: {
                                "PubID": data1[1],
                                "Status": data1[0]
                            }
                        }, function (data) {
                            if (data.Result) {
                                // window.location.reload();
                                if (data1[0] == 15005) {
                                    $this.attr('name', '15006,' + data1[1]).text('下架').parents('td').prev().text('上架');
                                    $this.attr('name', '15006,' + data1[1]).next().css('display','none')
                                } else {
                                    $this.attr('name', '15005,' + data1[1]).text('上架').parents('td').prev().text('下架');
                                    $this.attr('name', '15005,' + data1[1]).next().css('display','inline-block')
                                }

                            }
                        })
                        return
                    }

                    if (Date.parse(new Date().format("yyyy/MM/dd")) - Date.parse($(this).parent().parent().attr('name').split(' ')[0].split('-').join('/')) < 0 ) {
                        setAjax({
                            url: Ajaxurl,
                            type: "POST",
                            data: {
                                "PubID": data1[1],
                                "Status": data1[0]
                            }
                        }, function (data) {
                            if (data.Result) {
                                // window.location.reload();
                                if (data1[0] == 15005) {
                                    $this.attr('name', '15006,' + data1[1]).text('下架').parents('td').prev().text('上架');
                                    $this.attr('name', '15006,' + data1[1]).next().css('display','none')
                                } else {
                                    $this.attr('name', '15005,' + data1[1]).text('上架').parents('td').prev().text('下架');
                                    $this.attr('name', '15005,' + data1[1]).next().css('display','inline-block')
                                }

                            }
                        })
                    } else {
                        alert('请调整刊例时间！')
                    }
                })

                // 点击驳回原因
                $('tbody tr').find('td[name=15004]').find('a').off('click').on('click', function () {
                    // 驳回原因的内容
                    var data = $(this).next().text();
                    $.openPopupLayer({
                        name: "Reject",
                        url: "pricelist-reason.html",
                        error: function (dd) {
                            alert(dd.status);
                        },
                        success: function () {
                            $('.mb25').html(data)
                            //点击取消
                            $('.button').off('click').on('click', function () {
                                $.closePopupLayer('Reject');
                            });
                            $('.fr>a').off('click').on('click', function () {
                                $.closePopupLayer('Reject');
                            });
                        }
                    });
                })


            }

            setAjax({

                url: seleceurl,
                type: "GET",
                data: {'pageIndex': 1, 'pageSize': 20, 'BusinessType': 14001}
                //"pageIndex=1&pageSize=20&BusinessType=14001",
            }, function (data) {
                if (data.Result.List != null) {
                    // 后台返回数据处理
                    for (var i = 0; i < data.Result.List.length; i++) {
                        if (data.Result.List[i].FistName == null) {
                            data.Result.List[i].FistName = [];
                        }
                        if (data.Result.List[i].SecondName == null) {
                            data.Result.List[i].SecondName = []
                        }
                        if (data.Result.List[i].ThridName == null) {
                            data.Result.List[i].ThridName = []
                        }
                        if (data.Result.List[i].FourthName == null) {
                            data.Result.List[i].FourthName = []
                        }

                    }
                }
                $('tbody').html(ejs.render($('#weixin').html(), data));

                //查询
                $('.state li:last').off('click').on('click', function () {
                    // 获取微信号
                    var wechat = $('.state li:eq(0) input').val();
                    // 获取审核状态
                    var auditStatus = $('.state li:eq(1) select option:selected').attr('name');
                    // 获取上下架状态
                    var updownStatus = $('.state li:eq(2) select option:selected').attr('name');
                    // 获取到期时间
                    var dueTime = $('.state li:eq(3) select option:selected').attr('name');
                    setAjax({

                        url: seleceurl,
                        type: "GET",
                        data: {
                            'pageIndex': 1,
                            'pageSize': 20,
                            'BusinessType': 14001,
                            'Number': wechat,
                            'Status': auditStatus,
                            'PublishStatus': updownStatus,
                            'EndTime': dueTime
                        }
                        //"pageIndex=1&pageSize=20&BusinessType=14001&Number=" + wechat + "&Status=" + auditStatus + "&PublishStatus=" + updownStatus + "&EndTime=" + dueTime,
                    }, function (data) {
                        $('tbody').html(ejs.render($('#weixin').html(), data));
                        if (data.Result.TotleCount != 0) {
                            var counts = data.Result.TotleCount;
                            //分页
                            $("#pageContainer").pagination(
                                counts,
                                {
                                    items_per_page: 20, //每页显示多少条记录（默认为20条）
                                    callback: function (currPage, jg) {
                                        // ajax请求
                                        setAjax({

                                            url: seleceurl,
                                            type: "GET",
                                            data: {
                                                'pageIndex': currPage,
                                                'pageSize': 20,
                                                'BusinessType': 14001,
                                                'Number': wechat,
                                                'Status': auditStatus,
                                                'PublishStatus': updownStatus,
                                                'EndTime': dueTime
                                            }
                                            //"pageIndex="+currPage +"&pageSize=20&BusinessType=14001&Number=" + wechat + "&Status=" + auditStatus + "&PublishStatus=" + updownStatus + "&EndTime=" + dueTime,
                                        }, function (data) {

                                            if (data.Result.List != null) {
                                                // 后台返回数据处理
                                                for (var i = 0; i < data.Result.List.length; i++) {
                                                    if (data.Result.List[i].FistName == null) {
                                                        data.Result.List[i].FistName = [];
                                                    }
                                                    if (data.Result.List[i].SecondName == null) {
                                                        data.Result.List[i].SecondName = []
                                                    }
                                                    if (data.Result.List[i].ThridName == null) {
                                                        data.Result.List[i].ThridName = []
                                                    }
                                                    if (data.Result.List[i].FourthName == null) {
                                                        data.Result.List[i].FourthName = []
                                                    }

                                                }
                                            }
                                            $('tbody').html(ejs.render($('#weixin').html(), data));
                                            // 公共功能
                                            wxpublicFunction()

                                        })
                                    }
                                });

                        } else {
                            $('#pageContainer').html('<img src="/images/no_data.png" style="display: block;margin: 70px auto;">')
                        }
                        // 公共功能
                        wxpublicFunction()
                    })
                })
                if (data.Result.TotleCount != 0) {
                    var counts = data.Result.TotleCount;
                    //分页
                    $("#pageContainer").pagination(
                        counts,
                        {
                            items_per_page: 20, //每页显示多少条记录（默认为20条）
                            callback: function (currPage, jg) {
                                // ajax请求
                                setAjax({

                                    url: seleceurl,
                                    type: "GET",
                                    data: {'pageIndex': currPage, 'pageSize': 20, 'BusinessType': 14001}
                                    //"pageIndex=" + currPage + "&pageSize=20&BusinessType=14001"
                                }, function (data) {
                                    $('tbody').html(ejs.render($('#weixin').html(), data));
                                    // 公共功能
                                    wxpublicFunction();
                                })
                            } //回调函数
                        });


                } else {
                    $('#pageContainer').html('<img src="/images/no_data.png" style="display: block;margin: 70px auto;">')
                }
                // 公共功能
                wxpublicFunction();
            })
        }
        // 新浪微博刊例列表
        if ($('div[name="AE-sina"]').attr('name') == 'AE-sina') {
            $('div[name="AE-sina"]').css({display: 'block'})
            function wbpublicFunction() {
                judgeAuthority(2)
                // 点击申请上架
                $('tbody tr').find('td:last a[name^=15002]').off('click').on('click', function () {
                    var data = $(this).attr('name').split(',')
                    if(Date.parse(new Date().format("yyyy/MM/dd")) - Date.parse($(this).parent().attr('name').split(' ')[0].split('-').join('/')) < 0 ) {
                        $.openPopupLayer({
                            name: "shipingclickApply",
                            url: "pricelist-application.html",
                            error: function (dd) {
                                alert(dd.status);
                            },
                            success: function () {
                                // 点击确定
                                $('.layer_con').find('.button').off('click').on('click', function () {
                                    setAjax({
                                        url: Ajaxurl,
                                        type: "POST",
                                        data: {
                                            "PubID": data[1],
                                            "Status": data[0]
                                        }
                                    }, function (data) {
                                        $.closePopupLayer('shipingclickApply');
                                        if (data.Result) {
                                            window.location.reload();
                                        }
                                    })
                                })                 // 点击取消
                                    .end().find('.but_keep').off('click').on('click', function () {
                                    $.closePopupLayer('shipingclickApply')
                                })

                            }
                        });
                    }else {
                        alert('请调整刊例时间！')
                    }

                })
                // 上下架切换
                $('tbody tr').find('td:last span a').off('click').on('click', function (e) {
                    if(e.target.innerHTML=='编辑'){
                        return
                    }
                    var $this = $(this);
                    var data1 = $this.attr('name').split(',');
                    if($this.text()=='下架'){
                        setAjax({
                            url: Ajaxurl,
                            type: "POST",
                            data: {
                                "PubID": data1[1],
                                "Status": data1[0]
                            }
                        }, function (data) {
                            if (data.Result) {
                                // window.location.reload();
                                if (data1[0] == 15005) {
                                    $this.attr('name', '15006,' + data1[1]).text('下架').parents('td').prev().text('上架');
                                    $this.attr('name', '15006,' + data1[1]).next().css('display','none')
                                } else {
                                    $this.attr('name', '15005,' + data1[1]).text('上架').parents('td').prev().text('下架');
                                    $this.attr('name', '15005,' + data1[1]).next().css('display','inline-block')
                                }

                            }
                        })
                        return
                    }
                    if (Date.parse(new Date().format("yyyy/MM/dd")) - Date.parse($(this).parent().parent().attr('name').split(' ')[0].split('-').join('/')) < 0 ) {
                        setAjax({
                            url: Ajaxurl,
                            type: "POST",
                            data: {
                                "PubID": data1[1],
                                "Status": data1[0]
                            }
                        }, function (data) {
                            if (data.Result) {
                                // window.location.reload();
                                if (data1[0] == 15005) {
                                    $this.attr('name', '15006,' + data1[1]).text('下架').parents('td').prev().text('上架');
                                    $this.attr('name', '15006,' + data1[1]).next().css('display','none')
                                } else {
                                    $this.attr('name', '15005,' + data1[1]).text('上架').parents('td').prev().text('下架');
                                    $this.attr('name', '15005,' + data1[1]).next().css('display','inline-block')
                                }
                            }
                        })
                    } else {
                        alert('请调整刊例时间！');
                    }
                })

                // 点击驳回原因
                $('tbody tr').find('td[name=15004]').find('a').off('click').on('click', function () {
                    // 驳回原因的内容
                    var data = $(this).next().text();
                    $.openPopupLayer({
                        name: "Reject",
                        url: "pricelist-reason.html",
                        error: function (dd) {
                            alert(dd.status);
                        },
                        success: function () {
                            $('.mb25').html(data)
                            //点击取消
                            $('.button').off('click').on('click', function () {
                                $.closePopupLayer('Reject');
                            });
                            $('.fr>a').off('click').on('click', function () {
                                $.closePopupLayer('Reject');
                            });
                        }
                    });
                })

                // 点击跳转
                $('tbody tr').find('td:last').find('a').each(function () {
                    $(this).off('cilck').on('click', function () {
                        if ($(this).html() == '编辑') {
                            window.location = "/PublishManager/addEditPublish-blog.html?isAdd=1&PubID=" + $(this).attr('name');
                        }
                    })
                })
            }

            setAjax({

                url: seleceurl,
                type: "GET",
                data: {'pageIndex': 1, 'pageSize': 20, 'BusinessType': 14003}
                //"pageIndex=1&pageSize=20&BusinessType=14003"
            }, function (data) {
                if (data.Result.List != null) {
                    // 后台返回数据处理
                    for (var i = 0; i < data.Result.List.length; i++) {
                        if (data.Result.List[i].BeginTime) {
                            data.Result.List[i].BeginTime = data.Result.List[i].BeginTime.split(" ")[0];
                        }
                        if (data.Result.List[i].EndTime) {
                            data.Result.List[i].EndTime = data.Result.List[i].EndTime.split(" ")[0];
                        }
                        if (data.Result.List[i].FistName == null) {
                            data.Result.List[i].FistName = [];
                        }
                        if (data.Result.List[i].SecondName == null) {
                            data.Result.List[i].SecondName = []
                        }
                        if (data.Result.List[i].ThridName == null) {
                            data.Result.List[i].ThridName = []
                        }
                        if (data.Result.List[i].FourthName == null) {
                            data.Result.List[i].FourthName = []
                        }

                    }
                }
                $('tbody').html(ejs.render($('#xinlang').html(), data));
                // 公共功能
                wbpublicFunction();

                //查询
                $('.state li:last').off('click').on('click', function () {
                    // 获取微博号
                    var wechat = $('.state li:eq(0) input').val();
                    // 获取微博名称
                    var wechatz = $('.state li:eq(1) input').val();
                    // 获取审核状态
                    var auditStatus = $('.state li:eq(2) select option:selected').attr('name');
                    // 获取上下架状态
                    var updownStatus = $('.state li:eq(3) select option:selected').attr('name');
                    // 获取到期时间
                    var dueTime = $('.state li:eq(4) select option:selected').attr('name');
                    setAjax({

                        url: seleceurl,
                        type: "GET",
                        data: {
                            'pageIndex': 1,
                            'pageSize': 20,
                            'BusinessType': 14003,
                            'Number': wechat,
                            'Name': wechatz,
                            'Status': auditStatus,
                            'PublishStatus': updownStatus,
                            'EndTime': dueTime
                        }
                        //"pageIndex=1&pageSize=20&BusinessType=14003&Number=" + wechat + "&Name=" + wechatz + "&Status=" + auditStatus + "&PublishStatus=" + updownStatus + "&EndTime=" + dueTime ,
                    }, function (data) {
                        if (data.Result.List != null) {
                            // 后台返回数据处理
                            for (var i = 0; i < data.Result.List.length; i++) {
                                if (data.Result.List[i].BeginTime) {
                                    data.Result.List[i].BeginTime = data.Result.List[i].BeginTime.split(" ")[0];
                                }
                                if (data.Result.List[i].EndTime) {
                                    data.Result.List[i].EndTime = data.Result.List[i].EndTime.split(" ")[0];
                                }
                                if (data.Result.List[i].FistName == null) {
                                    data.Result.List[i].FistName = [];
                                }
                                if (data.Result.List[i].SecondName == null) {
                                    data.Result.List[i].SecondName = []
                                }
                                if (data.Result.List[i].ThridName == null) {
                                    data.Result.List[i].ThridName = []
                                }
                                if (data.Result.List[i].FourthName == null) {
                                    data.Result.List[i].FourthName = []
                                }

                            }
                        }
                        $('tbody').html(ejs.render($('#xinlang').html(), data));
                        if (data.Result.TotleCount != 0) {
                            var counts = data.Result.TotleCount;
                            //分页
                            $("#pageContainer").pagination(
                                counts,
                                {
                                    items_per_page: 20, //每页显示多少条记录（默认为20条）
                                    callback: function (currPage, jg) {
                                        // ajax请求
                                        setAjax({

                                            url: seleceurl,
                                            type: "GET",
                                            data: {
                                                'pageIndex': currPage,
                                                'pageSize': 20,
                                                'BusinessType': 14003,
                                                'Number': wechat,
                                                'Name': wechatz,
                                                'Status': auditStatus,
                                                'PublishStatus': updownStatus,
                                                'EndTime': dueTime
                                            }
                                            //"pageIndex="+currPage +"&pageSize=20&BusinessType=14003&Number=" + wechat + "&Name=" + wechatz + "&Status=" + auditStatus + "&PublishStatus=" + updownStatus + "&EndTime=" + dueTime ,
                                        }, function (data) {

                                            if (data.Result.List != null) {
                                                // 后台返回数据处理
                                                for (var i = 0; i < data.Result.List.length; i++) {
                                                    if (data.Result.List[i].BeginTime) {
                                                        data.Result.List[i].BeginTime = data.Result.List[i].BeginTime.split(" ")[0];
                                                    }
                                                    if (data.Result.List[i].EndTime) {
                                                        data.Result.List[i].EndTime = data.Result.List[i].EndTime.split(" ")[0];
                                                    }
                                                    if (data.Result.List[i].FistName == null) {
                                                        data.Result.List[i].FistName = [];
                                                    }
                                                    if (data.Result.List[i].SecondName == null) {
                                                        data.Result.List[i].SecondName = []
                                                    }
                                                    if (data.Result.List[i].ThridName == null) {
                                                        data.Result.List[i].ThridName = []
                                                    }
                                                    if (data.Result.List[i].FourthName == null) {
                                                        data.Result.List[i].FourthName = []
                                                    }

                                                }
                                            }
                                            $('tbody').html(ejs.render($('#xinlang').html(), data));
                                            // 公共功能
                                            wbpublicFunction();

                                        })
                                    }
                                });

                        } else {
                            $('#pageContainer').html('<img src="/images/no_data.png" style="display: block;margin: 70px auto;">')
                        }
                        wbpublicFunction();
                    })
                })
                if (data.Result.TotleCount != 0) {
                    var counts = data.Result.TotleCount;
                    //分页
                    $("#pageContainer").pagination(
                        counts,
                        {
                            items_per_page: 20, //每页显示多少条记录（默认为20条）
                            callback: function (currPage, jg) {
                                // ajax请求
                                setAjax({

                                    url: seleceurl,
                                    type: "GET",
                                    data: {'pageIndex': currPage, 'pageSize': 20, 'BusinessType': 14003}
                                    //"pageIndex=" + currPage + "&pageSize=20&BusinessType=14003"
                                }, function (data) {

                                    if (data.Result.List != null) {
                                        // 后台返回数据处理
                                        for (var i = 0; i < data.Result.List.length; i++) {
                                            if (data.Result.List[i].BeginTime) {
                                                data.Result.List[i].BeginTime = data.Result.List[i].BeginTime.split(" ")[0];
                                            }
                                            if (data.Result.List[i].EndTime) {
                                                data.Result.List[i].EndTime = data.Result.List[i].EndTime.split(" ")[0];
                                            }
                                            if (data.Result.List[i].FistName == null) {
                                                data.Result.List[i].FistName = [];
                                            }
                                            if (data.Result.List[i].SecondName == null) {
                                                data.Result.List[i].SecondName = []
                                            }
                                            if (data.Result.List[i].ThridName == null) {
                                                data.Result.List[i].ThridName = []
                                            }
                                            if (data.Result.List[i].FourthName == null) {
                                                data.Result.List[i].FourthName = []
                                            }

                                        }
                                    }
                                    $('tbody').html(ejs.render($('#xinlang').html(), data));
                                    // 公共功能
                                    wbpublicFunction()

                                })
                            }
                        });

                } else {
                    $('#pageContainer').html('<img src="/images/no_data.png" style="display: block;margin: 70px auto;">')
                }
            })
        }
        // 视频刊例列表
        if ($('div[name="AE-video"]').attr('name') == 'AE-video') {
            $('div[name="AE-video"]').css({display: 'block'})
            function sppublicFunction() {
                judgeAuthority(3)
                // 点击申请上架
                $('tbody tr').find('td:last a[name^=15002]').off('click').on('click', function () {
                    var data = $(this).attr('name').split(',')
                    if(Date.parse(new Date().format("yyyy/MM/dd")) - Date.parse($(this).parent().attr('name').split(' ')[0].split('-').join('/')) < 0 ) {
                        $.openPopupLayer({
                            name: "shipingclickApply",
                            url: "pricelist-application.html",
                            error: function (dd) {
                                alert(dd.status);
                            },
                            success: function () {
                                // 点击确定
                                $('.layer_con').find('.button').off('click').on('click', function () {
                                    setAjax({
                                        url: Ajaxurl,
                                        type: "POST",
                                        data: {
                                            "PubID": data[1],
                                            "Status": data[0]
                                        }
                                    }, function (data) {
                                        $.closePopupLayer('shipingclickApply')
                                        if (data.Result) {
                                            window.location.reload();
                                        }
                                    })
                                })                 // 点击取消
                                    .end().find('.but_keep').off('click').on('click', function () {
                                    $.closePopupLayer('shipingclickApply')
                                })

                            }
                        });
                    }else {
                        alert('请调整刊例时间！')
                    }

                })
                // 上下架切换
                $('tbody tr').find('td:last span a').off('click').on('click', function (e) {
                    if(e.target.innerHTML=='编辑'){
                        return
                    }
                    var $this = $(this);
                    var data1 = $this.attr('name').split(',');
                    if($this.text()=='下架'){
                        setAjax({
                            url: Ajaxurl,
                            type: "POST",
                            data: {
                                "PubID": data1[1],
                                "Status": data1[0]
                            }
                        }, function (data) {
                            if (data.Result) {
                                // window.location.reload();
                                if (data1[0] == 15005) {
                                    $this.attr('name', '15006,' + data1[1]).text('下架').parents('td').prev().text('上架');
                                    $this.attr('name', '15006,' + data1[1]).next().css('display','none')
                                } else {
                                    $this.attr('name', '15005,' + data1[1]).text('上架').parents('td').prev().text('下架');
                                    $this.attr('name', '15005,' + data1[1]).next().css('display','inline-block')
                                }

                            }
                        })
                        return
                    }
                    if (Date.parse(new Date().format("yyyy/MM/dd")) - Date.parse($(this).parent().parent().attr('name').split(' ')[0].split('-').join('/')) < 0 ) {
                        setAjax({
                            url: Ajaxurl,
                            type: "POST",
                            data: {
                                "PubID": data1[1],
                                "Status": data1[0]
                            }
                        }, function (data) {
                            if (data.Result) {
                                // window.location.reload();
                                if (data1[0] == 15005) {
                                    $this.attr('name', '15006,' + data1[1]).text('下架').parents('td').prev().text('上架');
                                    $this.attr('name', '15006,' + data1[1]).next().css('display','none')
                                } else {
                                    $this.attr('name', '15005,' + data1[1]).text('上架').parents('td').prev().text('下架');
                                    $this.attr('name', '15005,' + data1[1]).next().css('display','inline-block')
                                }
                            }
                        })
                    } else {
                        alert('请调整刊例时间！')
                    }
                })

                // 点击驳回原因
                $('tbody tr').find('td[name=15004]').find('a').off('click').on('click', function () {
                    // 驳回原因的内容
                    var data = $(this).next().text();
                    $.openPopupLayer({
                        name: "Reject",
                        url: "pricelist-reason.html",
                        error: function (dd) {
                            alert(dd.status);
                        },
                        success: function () {
                            $('.mb25').html(data)
                            //点击取消
                            $('.button').off('click').on('click', function () {
                                $.closePopupLayer('Reject');
                            });
                            $('.fr>a').off('click').on('click', function () {
                                $.closePopupLayer('Reject');
                            });
                        }
                    });
                })
                // 点击跳转
                $('tbody tr').find('td:last').find('a').each(function () {
                    $(this).off('cilck').on('click', function () {
                        if ($(this).html() == '编辑') {
                            window.location = "/PublishManager/addEditPublish-video.html?isAdd=1&PubID=" + $(this).attr('name');
                        }
                    })
                })
            }

            setAjax({
                  url: seleceurl,
                type: "GET",
                data: {'pageIndex': 1, 'pageSize': 20, 'BusinessType': 14004}
                //"pageIndex=1&pageSize=20&BusinessType=14004"
            }, function (data) {
                if (data.Result.List != null) {
                    // 后台返回数据处理
                    for (var i = 0; i < data.Result.List.length; i++) {
                        if (data.Result.List[i].BeginTime) {
                            data.Result.List[i].BeginTime = data.Result.List[i].BeginTime.split(" ")[0];
                        }
                        if (data.Result.List[i].EndTime) {
                            data.Result.List[i].EndTime = data.Result.List[i].EndTime.split(" ")[0];
                        }
                        if (data.Result.List[i].FistName == null) {
                            data.Result.List[i].FistName = [];
                        }
                        if (data.Result.List[i].SecondName == null) {
                            data.Result.List[i].SecondName = []
                        }
                        if (data.Result.List[i].ThridName == null) {
                            data.Result.List[i].ThridName = []
                        }
                        if (data.Result.List[i].FourthName == null) {
                            data.Result.List[i].FourthName = []
                        }

                    }
                }
                $('tbody').html(ejs.render($('#shiping').html(), data));
                // 公共功能
                sppublicFunction()
                //查询
                $('.state li:last').off('click').on('click', function () {
                    // 获取微博号
                    var wechat = $('.state li:eq(0) input').val();
                    // 获取微博名称
                    var wechatz = $('.state li:eq(1) input').val();
                    // 获取审核状态
                    var auditStatus = $('.state li:eq(2) select option:selected').attr('name');
                    // 获取上下架状态
                    var updownStatus = $('.state li:eq(3) select option:selected').attr('name');
                    // 获取到期时间
                    var dueTime = $('.state li:eq(4) select option:selected').attr('name');
                    setAjax({
                          url: seleceurl,
                        type: "GET",
                        data: {
                            'pageIndex': 1,
                            'pageSize': 20,
                            'BusinessType': 14004,
                            'Number': wechat,
                            'Name': wechatz,
                            'Status': auditStatus,
                            'PublishStatus': updownStatus,
                            'EndTime': dueTime
                        }
                        //"pageIndex=1&pageSize=20&BusinessType=14004&Number=" + wechat + "&Name=" + wechatz + "&Status=" + auditStatus + "&PublishStatus=" + updownStatus + "&EndTime=" + dueTime ,
                    }, function (data) {

                        if (data.Result.List != null) {
                            // 后台返回数据处理
                            for (var i = 0; i < data.Result.List.length; i++) {
                                if (data.Result.List[i].BeginTime) {
                                    data.Result.List[i].BeginTime = data.Result.List[i].BeginTime.split(" ")[0];
                                }
                                if (data.Result.List[i].EndTime) {
                                    data.Result.List[i].EndTime = data.Result.List[i].EndTime.split(" ")[0];
                                }
                                if (data.Result.List[i].FistName == null) {
                                    data.Result.List[i].FistName = [];
                                }
                                if (data.Result.List[i].SecondName == null) {
                                    data.Result.List[i].SecondName = []
                                }
                                if (data.Result.List[i].ThridName == null) {
                                    data.Result.List[i].ThridName = []
                                }
                                if (data.Result.List[i].FourthName == null) {
                                    data.Result.List[i].FourthName = []
                                }

                            }
                        }
                        $('tbody').html(ejs.render($('#shiping').html(), data));
                        if (data.Result.TotleCount != 0) {
                            var counts = data.Result.TotleCount;
                            //分页
                            $("#pageContainer").pagination(
                                counts,
                                {
                                    items_per_page: 20, //每页显示多少条记录（默认为20条）
                                    callback: function (currPage, jg) {
                                        // ajax请求
                                        setAjax({
                                              url: seleceurl,
                                            type: "GET",
                                            data: {
                                                'pageIndex': currPage,
                                                'pageSize': 20,
                                                'BusinessType': 14004,
                                                'Number': wechat,
                                                'Name': wechatz,
                                                'Status': auditStatus,
                                                'PublishStatus': updownStatus,
                                                'EndTime': dueTime
                                            }
                                            //"pageIndex="+currPage +"&pageSize=20&BusinessType=14004&Number=" + wechat + "&Name=" + wechatz + "&Status=" + auditStatus + "&PublishStatus=" + updownStatus + "&EndTime=" + dueTime
                                        }, function (data) {

                                            if (data.Result.List != null) {
                                                // 后台返回数据处理
                                                for (var i = 0; i < data.Result.List.length; i++) {
                                                    if (data.Result.List[i].BeginTime) {
                                                        data.Result.List[i].BeginTime = data.Result.List[i].BeginTime.split(" ")[0];
                                                    }
                                                    if (data.Result.List[i].EndTime) {
                                                        data.Result.List[i].EndTime = data.Result.List[i].EndTime.split(" ")[0];
                                                    }
                                                    if (data.Result.List[i].FistName == null) {
                                                        data.Result.List[i].FistName = [];
                                                    }
                                                    if (data.Result.List[i].SecondName == null) {
                                                        data.Result.List[i].SecondName = []
                                                    }
                                                    if (data.Result.List[i].ThridName == null) {
                                                        data.Result.List[i].ThridName = []
                                                    }
                                                    if (data.Result.List[i].FourthName == null) {
                                                        data.Result.List[i].FourthName = []
                                                    }

                                                }
                                            }
                                            $('tbody').html(ejs.render($('#shiping').html(), data));
                                            // 公共功能
                                            sppublicFunction();

                                        })
                                    }
                                });

                        } else {
                            $('#pageContainer').html('<img src="/images/no_data.png" style="display: block;margin: 70px auto;">')
                        }
                        sppublicFunction();
                    })
                })
                if (data.Result.TotleCount != 0) {
                    var counts = data.Result.TotleCount;
                    //分页
                    $("#pageContainer").pagination(
                        counts,
                        {
                            items_per_page: 20, //每页显示多少条记录（默认为20条）
                            callback: function (currPage, jg) {
                                // ajax请求
                                setAjax({
                                      url: seleceurl,
                                    type: "GET",
                                    data: {'pageIndex': currPage, 'pageSize': 20, 'BusinessType': 14004}
                                    //"pageIndex=" + currPage + "&pageSize=20&BusinessType=14004"
                                }, function (data) {

                                    if (data.Result.List != null) {
                                        // 后台返回数据处理
                                        for (var i = 0; i < data.Result.List.length; i++) {
                                            if (data.Result.List[i].BeginTime) {
                                                data.Result.List[i].BeginTime = data.Result.List[i].BeginTime.split(" ")[0];
                                            }
                                            if (data.Result.List[i].EndTime) {
                                                data.Result.List[i].EndTime = data.Result.List[i].EndTime.split(" ")[0];
                                            }
                                            if (data.Result.List[i].FistName == null) {
                                                data.Result.List[i].FistName = [];
                                            }
                                            if (data.Result.List[i].SecondName == null) {
                                                data.Result.List[i].SecondName = []
                                            }
                                            if (data.Result.List[i].ThridName == null) {
                                                data.Result.List[i].ThridName = []
                                            }
                                            if (data.Result.List[i].FourthName == null) {
                                                data.Result.List[i].FourthName = []
                                            }

                                        }
                                    }
                                    $('tbody').html(ejs.render($('#shiping').html(), data));
                                    // 公共功能
                                    sppublicFunction()

                                })
                            }
                        });

                } else {
                    $('#pageContainer').html('<img src="/images/no_data.png" style="display: block;margin: 70px auto;">')
                }
            })
        }
        // 直播刊例列表
        if ($('div[name="AE-zhibo"]').attr('name') == 'AE-zhibo') {
            $('div[name="AE-zhibo"]').css({display: 'block'})
            function zbpublicFunction() {
                judgeAuthority(4)
                // 点击申请上架
                $('tbody tr').find('td:last a[name^=15002]').off('click').on('click', function () {
                    var data = $(this).attr('name').split(',')
                    if(Date.parse(new Date().format("yyyy/MM/dd")) - Date.parse($(this).parent().attr('name').split(' ')[0].split('-').join('/')) < 0 ) {
                        $.openPopupLayer({
                            name: "shipingclickApply",
                            url: "pricelist-application.html",
                            error: function (dd) {
                                alert(dd.status);
                            },
                            success: function () {
                                // 点击确定
                                $('.layer_con').find('.button').off('click').on('click', function () {
                                    setAjax({
                                        url: Ajaxurl,
                                        type: "POST",
                                        data: {
                                            "PubID": data[1],
                                            "Status": data[0]
                                        }
                                    }, function (data) {
                                        $.closePopupLayer('shipingclickApply');
                                        if (data.Result) {
                                            window.location.reload();
                                        }
                                    })
                                })                 // 点击取消
                                    .end().find('.but_keep').off('click').on('click', function () {
                                    $.closePopupLayer('shipingclickApply')
                                })

                            }
                        });
                    }else {
                        alert('请调整刊例时间！')
                    }

                })
                // 上下架切换
                $('tbody tr').find('td:last span a').off('click').on('click', function (e) {
                    if(e.target.innerHTML=='编辑'){
                        return
                    }
                    var $this = $(this);
                    var data1 = $this.attr('name').split(',');
                    if($this.text()=='下架'){
                        setAjax({
                            url: Ajaxurl,
                            type: "POST",
                            data: {
                                "PubID": data1[1],
                                "Status": data1[0]
                            }
                        }, function (data) {
                            if (data.Result) {
                                // window.location.reload();
                                if (data1[0] == 15005) {
                                    $this.attr('name', '15006,' + data1[1]).text('下架').parents('td').prev().text('上架');
                                    $this.attr('name', '15006,' + data1[1]).next().css('display','none')
                                } else {
                                    $this.attr('name', '15005,' + data1[1]).text('上架').parents('td').prev().text('下架');
                                    $this.attr('name', '15005,' + data1[1]).next().css('display','inline-block')
                                }

                            }
                        })
                        return
                    }
                    if (Date.parse(new Date().format("yyyy/MM/dd")) - Date.parse($(this).parent().parent().attr('name').split(' ')[0].split('-').join('/')) < 0 ) {
                        setAjax({
                            url: Ajaxurl,
                            type: "POST",
                            data: {
                                "PubID": data1[1],
                                "Status": data1[0]
                            }
                        }, function (data) {
                            if (data.Result) {
                                // window.location.reload();
                                if (data1[0] == 15005) {
                                    $this.attr('name', '15006,' + data1[1]).text('下架').parents('td').prev().text('上架');
                                    $this.attr('name', '15006,' + data1[1]).next().css('display','none')
                                } else {
                                    $this.attr('name', '15005,' + data1[1]).text('上架').parents('td').prev().text('下架');
                                    $this.attr('name', '15005,' + data1[1]).next().css('display','inline-block')
                                }
                            }
                        })
                    } else {
                        alert('请调整刊例时间！');
                    }
                })

                // 点击驳回原因
                $('tbody tr').find('td[name=15004]').find('a').off('click').on('click', function () {
                    // 驳回原因的内容
                    var data = $(this).next().text();
                    $.openPopupLayer({
                        name: "Reject",
                        url: "pricelist-reason.html",
                        error: function (dd) {
                            alert(dd.status);
                        },
                        success: function () {
                            $('.mb25').html(data)
                            //点击取消
                            $('.button').off('click').on('click', function () {
                                $.closePopupLayer('Reject');
                            });
                            $('.fr>a').off('click').on('click', function () {
                                $.closePopupLayer('Reject');
                            });
                        }
                    });
                })

                // 点击跳转
                $('tbody tr').find('td:last').find('a').each(function () {
                    $(this).off('cilck').on('click', function () {
                        if ($(this).html() == '编辑') {
                            window.location = "/PublishManager/addEditPublish-livevideo.html?isAdd=1&PubID=" + $(this).attr('name');
                        }
                    })
                })
            }

            setAjax({
                  url: seleceurl,
                type: "GET",
                data: {'pageIndex': 1, 'pageSize': 20, 'BusinessType': 14005}
                //"pageIndex=1&pageSize=20&BusinessType=14005"
            }, function (data) {

                if (data.Result.List != null) {
                    // 后台返回数据处理
                    for (var i = 0; i < data.Result.List.length; i++) {
                        if (data.Result.List[i].FistName == null) {
                            data.Result.List[i].FistName = [];
                        }
                        if (data.Result.List[i].SecondName == null) {
                            data.Result.List[i].SecondName = []
                        }
                        if (data.Result.List[i].ThridName == null) {
                            data.Result.List[i].ThridName = []
                        }
                        if (data.Result.List[i].FourthName == null) {
                            data.Result.List[i].FourthName = []
                        }

                    }
                }
                $('tbody').html(ejs.render($('#zhibo').html(), data));
                // 公共功能
                zbpublicFunction()
                //查询
                $('.state li:last').off('click').on('click', function () {
                    // 获取微博号
                    var wechat = $('.state li:eq(0) input').val();
                    // 获取微博名称
                    var wechatz = $('.state li:eq(1) input').val();
                    // 获取审核状态
                    var auditStatus = $('.state li:eq(2) select option:selected').attr('name');
                    // 获取上下架状态
                    var updownStatus = $('.state li:eq(3) select option:selected').attr('name');
                    // 获取到期时间
                    var dueTime = $('.state li:eq(4) select option:selected').attr('name');
                    setAjax({
                          url: seleceurl,
                        type: "GET",
                        data: {
                            'pageIndex': 1,
                            'pageSize': 20,
                            'BusinessType': 14005,
                            'Number': wechat,
                            'Name': wechatz,
                            'Status': auditStatus,
                            'PublishStatus': updownStatus,
                            'EndTime': dueTime
                        }
                    }, function (data) {

                        if (data.Result.List != null) {
                            // 后台返回数据处理
                            for (var i = 0; i < data.Result.List.length; i++) {
                                if (data.Result.List[i].FistName == null) {
                                    data.Result.List[i].FistName = [];
                                }
                                if (data.Result.List[i].SecondName == null) {
                                    data.Result.List[i].SecondName = []
                                }
                                if (data.Result.List[i].ThridName == null) {
                                    data.Result.List[i].ThridName = []
                                }
                                if (data.Result.List[i].FourthName == null) {
                                    data.Result.List[i].FourthName = []
                                }

                            }
                        }
                        $('tbody').html(ejs.render($('#zhibo').html(), data));
                        if (data.Result.TotleCount != 0) {
                            var counts = data.Result.TotleCount;
                            //分页
                            $("#pageContainer").pagination(
                                counts,
                                {
                                    items_per_page: 20, //每页显示多少条记录（默认为20条）
                                    callback: function (currPage, jg) {
                                        // ajax请求
                                        setAjax({
                                              url: seleceurl,
                                            type: "GET",
                                            data: {
                                                'pageIndex': currPage,
                                                'pageSize': 20,
                                                'BusinessType': 14005,
                                                'Number': wechat,
                                                'Name': wechatz,
                                                'Status': auditStatus,
                                                'PublishStatus': updownStatus,
                                                'EndTime': dueTime
                                            }
                                        }, function (data) {

                                            if (data.Result.List != null) {
                                                // 后台返回数据处理
                                                for (var i = 0; i < data.Result.List.length; i++) {
                                                    if (data.Result.List[i].FistName == null) {
                                                        data.Result.List[i].FistName = [];
                                                    }
                                                    if (data.Result.List[i].SecondName == null) {
                                                        data.Result.List[i].SecondName = []
                                                    }
                                                    if (data.Result.List[i].ThridName == null) {
                                                        data.Result.List[i].ThridName = []
                                                    }
                                                    if (data.Result.List[i].FourthName == null) {
                                                        data.Result.List[i].FourthName = []
                                                    }

                                                }
                                            }
                                            $('tbody').html(ejs.render($('#zhibo').html(), data));
                                            // 公共功能
                                            zbpublicFunction();

                                        })
                                    }
                                });

                        } else {
                            $('#pageContainer').html('<img src="/images/no_data.png" style="display: block;margin: 70px auto;">')
                        }
                        zbpublicFunction();
                    })
                })
                if (data.Result.TotleCount != 0) {
                    var counts = data.Result.TotleCount;
                    //分页
                    $("#pageContainer").pagination(
                        counts,
                        {
                            items_per_page: 20, //每页显示多少条记录（默认为20条）
                            callback: function (currPage, jg) {
                                // ajax请求
                                setAjax({
                                      url: seleceurl,
                                    type: "GET",
                                    data: {'pageIndex': currPage, 'pageSize': 20, 'BusinessType': 14005}
                                    //"pageIndex=" + currPage + "&pageSize=20&BusinessType=14005"
                                }, function (data) {

                                    if (data.Result.List != null) {
                                        // 后台返回数据处理
                                        for (var i = 0; i < data.Result.List.length; i++) {
                                            if (data.Result.List[i].FistName == null) {
                                                data.Result.List[i].FistName = [];
                                            }
                                            if (data.Result.List[i].SecondName == null) {
                                                data.Result.List[i].SecondName = []
                                            }
                                            if (data.Result.List[i].ThridName == null) {
                                                data.Result.List[i].ThridName = []
                                            }
                                            if (data.Result.List[i].FourthName == null) {
                                                data.Result.List[i].FourthName = []
                                            }

                                        }
                                    }
                                    $('tbody').html(ejs.render($('#zhibo').html(), data));
                                    // 公共功能
                                    zbpublicFunction()

                                })
                            }
                        });

                } else {
                    $('#pageContainer').html('<img src="/images/no_data.png" style="display: block;margin: 70px auto;">')
                }
            })
        }
        // app刊例列表
        if ($('div[name="AE-app"]').attr('name') == 'AE-app') {
            $('div[name="AE-app"]').css({display: 'block'})
            function apppublicFunction() {
                judgeAuthority(5)
                // 点击申请上架
                $('tbody tr').find('td:last a[name^=15002]').off('click').on('click', function () {
                    var data = $(this).attr('name').split(',')
                    if(Date.parse(new Date().format("yyyy/MM/dd")) - Date.parse($(this).parent().attr('name').split(' ')[0].split('-').join('/')) < 0 ) {
                        if ($(this).parent().prev().prev().prev().html() == '0') {
                            alert("广告位数量为0，不能申请上架！");
                        } else {
                            $.openPopupLayer({
                                name: "shipingclickApply",
                                url: "pricelist-application.html",
                                error: function (dd) {
                                    alert(dd.status);
                                },
                                success: function () {
                                    // 点击确定
                                    $('.layer_con').find('.button').off('click').on('click', function () {
                                        setAjax({
                                            url: Ajaxurl,
                                            type: "POST",
                                            data: {
                                                "PubID": data[1],
                                                "Status": data[0]
                                            }
                                        }, function (data) {
                                            $.closePopupLayer('shipingclickApply')
                                            if (data.Result) {
                                                window.location.reload();
                                            }
                                        })
                                    })                 // 点击取消
                                        .end().find('.but_keep').off('click').on('click', function () {
                                        $.closePopupLayer('shipingclickApply')
                                    })

                                }
                            });
                        }
                    }else {
                        alert('请调整刊例时间！')
                    }
                })
                // 点击驳回原因
                $('tbody tr').find('td[name=15004]').find('a').off('click').on('click', function () {
                    // 驳回原因的内容
                    var data = $(this).next().text();
                    $.openPopupLayer({
                        name: "Reject",
                        url: "pricelist-reason.html",
                        error: function (dd) {
                            alert(dd.status);
                        },
                        success: function () {
                            $('.mb25').html(data)
                            //点击取消
                            $('.button').off('click').on('click', function () {
                                $.closePopupLayer('Reject');
                            });
                            $('.fr>a').off('click').on('click', function () {
                                $.closePopupLayer('Reject');
                            });
                        }
                    });
                })
                // 点击跳转
                $('tbody tr').find('td:last').find('a').each(function () {
                    $(this).off('cilck').on('click', function () {
                        if ($(this).html() == '查看') {
                            window.open("/PublishManager/auditPublishAPP.html?MediaID=" + $(this).attr('name'))
                        }
                        if ($(this).html() == '编辑') {
                            window.location = "/PublishManager/addEditPublish-app.html?isAdd=1&PubID=" + $(this).attr('name');
                        }
                    })
                })
            }

            setAjax({
                  url: seleceurl,
                type: "GET",
                data: {'pageIndex': 1, 'pageSize': 20, 'BusinessType': 14002}
                //"pageIndex=1&pageSize=20&BusinessType=14002"
            }, function (data) {
                if (data.Result.List != null) {
                    // 后台返回数据处理
                    for (var i = 0; i < data.Result.List.length; i++) {
                        if (data.Result.List[i].BeginTime) {
                            data.Result.List[i].BeginTime = data.Result.List[i].BeginTime.split(" ")[0];
                        }
                        if (data.Result.List[i].EndTime) {
                            data.Result.List[i].EndTime = data.Result.List[i].EndTime.split(" ")[0];
                        }
                    }
                }
                $('tbody').html(ejs.render($('#app').html(), data));
                // 公共功能
                apppublicFunction()
                // 查询
                $('.state li:last').off('click').on('click', function () {
                    // 获取媒体名称
                    var mediaName = $('.state li:eq(0) input').val();
                    // 获取审核状态
                    var auditStatus = $('.state li:eq(1) option:selected').attr('name');

                    setAjax({
                          url: seleceurl,
                        type: "GET",
                        data: {
                            'pageIndex': 1,
                            'pageSize': 20,
                            'BusinessType': 14002,
                            'Name': mediaName,
                            'Status': auditStatus
                        }
                        //"pageIndex=1&pageSize=20&BusinessType=14002&Name=" + mediaName + "&Status=" + auditStatus
                    }, function (data) {
                        if (data.Result.List != null) {
                            // 后台返回数据处理
                            for (var i = 0; i < data.Result.List.length; i++) {
                                if (data.Result.List[i].BeginTime) {
                                    data.Result.List[i].BeginTime = data.Result.List[i].BeginTime.split(" ")[0];
                                }
                                if (data.Result.List[i].EndTime) {
                                    data.Result.List[i].EndTime = data.Result.List[i].EndTime.split(" ")[0];
                                }
                            }
                        }
                        $('tbody').html(ejs.render($('#app').html(), data));
                        if (data.Result.TotleCount != 0) {
                            var counts = data.Result.TotleCount;
                            //分页
                            $("#pageContainer").pagination(
                                counts,
                                {
                                    items_per_page: 20, //每页显示多少条记录（默认为20条）
                                    callback: function (currPage, jg) {
                                        // ajax请求
                                        setAjax({
                                              url: seleceurl,
                                            type: "GET",
                                            data: {
                                                'pageIndex': currPage,
                                                'pageSize': 20,
                                                'BusinessType': 14002,
                                                'Name': mediaName,
                                                'Status': auditStatus
                                            }
                                            //"pageIndex="+currPage +"&pageSize=20&BusinessType=14002&Name=" + mediaName + "&Status=" + auditStatus
                                        }, function (data) {
                                            if (data.Result.List != null) {
                                                // 后台返回数据处理
                                                for (var i = 0; i < data.Result.List.length; i++) {
                                                    if (data.Result.List[i].BeginTime) {
                                                        data.Result.List[i].BeginTime = data.Result.List[i].BeginTime.split(" ")[0];
                                                    }
                                                    if (data.Result.List[i].EndTime) {
                                                        data.Result.List[i].EndTime = data.Result.List[i].EndTime.split(" ")[0];
                                                    }
                                                }
                                            }
                                            $('tbody').html(ejs.render($('#app').html(), data));
                                            // 公共功能
                                            apppublicFunction()

                                        })
                                    }
                                });

                        } else {
                            $('#pageContainer').html('<img src="/images/no_data.png" style="display: block;margin: 70px auto;">')
                        }
                        // 公共功能
                        apppublicFunction()
                    })
                })
                if (data.Result.TotleCount != 0) {
                    var counts = data.Result.TotleCount;
                    //分页
                    $("#pageContainer").pagination(
                        counts,
                        {
                            items_per_page: 20, //每页显示多少条记录（默认为20条）
                            callback: function (currPage, jg) {
                                // ajax请求
                                setAjax({
                                      url: seleceurl,
                                    type: "GET",
                                    data: {'pageIndex': currPage, 'pageSize': 20, 'BusinessType': 14002}
                                    //"pageIndex=" + currPage + "&pageSize=20&BusinessType=14002"
                                }, function (data) {
                                    if (data.Result.List != null) {
                                        // 后台返回数据处理
                                        for (var i = 0; i < data.Result.List.length; i++) {
                                            if (data.Result.List[i].BeginTime) {
                                                data.Result.List[i].BeginTime = data.Result.List[i].BeginTime.split(" ")[0];
                                            }
                                            if (data.Result.List[i].EndTime) {
                                                data.Result.List[i].EndTime = data.Result.List[i].EndTime.split(" ")[0];
                                            }
                                        }
                                    }
                                    $('tbody').html(ejs.render($('#app').html(), data));
                                    // 公共功能
                                    apppublicFunction()

                                })
                            }
                        });

                } else {
                    $('#pageContainer').html('<img src="/images/no_data.png" style="display: block;margin: 70px auto;">')
                }
            })
        }
    }
    // 运营
    if (CTLogin.RoleIDs == 'SYS001RL00004' || CTLogin.RoleIDs == 'SYS001RL00001') {
        //微信公共号列表
        if ($('div[name="operate-wechat"]').attr('name') == 'operate-wechat') {
            $('div[name="operate-wechat"]').css({display: 'block'})
            function wxpublicFunction(i) {
                judgeAuthority(1)
                // 点击申请上架
                $('tbody').find("tr").find('td:last a[name^=15002]').off('click').on('click', function () {
                    var data = $(this).attr('name').split(',')
                    if(Date.parse(new Date().format("yyyy/MM/dd")) - Date.parse($(this).parent().attr('name').split(' ')[0].split('-').join('/')) < 0) {
                        $.openPopupLayer({
                            name: "shipingclickApply",
                            url: "pricelist-application.html",
                            error: function (dd) {
                                alert(dd.status);
                            },
                            success: function () {
                                // 点击确定
                                $('.layer_con').find('.button').off('click').on('click', function () {
                                    setAjax({
                                        url: Ajaxurl,
                                        type: "POST",
                                        data: {
                                            "PubID": data[1],
                                            "Status": data[0]
                                        }
                                    }, function (data) {
                                        $.closePopupLayer('shipingclickApply');
                                        if (data.Result) {
                                            window.location.reload();
                                        }
                                    })
                                })                 // 点击取消
                                    .end().find('.but_keep').off('click').on('click', function () {
                                    $.closePopupLayer('shipingclickApply')
                                })

                            }
                        });
                    }else {
                        alert('请调整刊例时间！')
                    }
                })
                // 点击驳回原因
                $('tbody').eq(i).find('tr').find('td[name=15004]').find('a').off('click').on('click', function () {
                    // 驳回原因的内容
                    var data = $(this).next().text();
                    $.openPopupLayer({
                        name: "Reject",
                        url: "pricelist-reason.html",
                        error: function (dd) {
                            alert(dd.status);
                        },
                        success: function () {
                            $('.mb25').html(data)
                            //点击取消
                            $('.button').off('click').on('click', function () {
                                $.closePopupLayer('Reject');
                            });
                            $('.fr>a').off('click').on('click', function () {
                                $.closePopupLayer('Reject');
                            });
                        }
                    });
                })
                // 上下架切换
                $('tbody tr').find('td:last span a').off('click').on('click', function (e) {
                    if(e.target.innerHTML=='编辑'){
                        return
                    }

                    var $this = $(this);
                    var data1 = $this.attr('name').split(',');
                    if($this.text()=='下架'){
                        setAjax({
                            url: Ajaxurl,
                            type: "POST",
                            data: {
                                "PubID": data1[1],
                                "Status": data1[0]
                            }
                        }, function (data) {
                            if (data.Result) {
                                // window.location.reload();
                                if (data1[0] == 15005) {
                                    $this.attr('name', '15006,' + data1[1]).text('下架');
                                    $this.attr('name', '15006,' + data1[1]).next().css('display','none')
                                } else {
                                    $this.attr('name', '15005,' + data1[1]).text('上架');
                                    $this.attr('name', '15005,' + data1[1]).next().css('display','inline-block')
                                }

                            }
                        })
                        return
                    }
                    if (Date.parse(new Date().format("yyyy/MM/dd")) - Date.parse($(this).parent().parent().attr('name').split(' ')[0].split('-').join('/')) < 0 ) {
                        setAjax({
                            url: Ajaxurl,
                            type: "POST",
                            data: {
                                "PubID": data1[1],
                                "Status": data1[0]
                            }
                        }, function (data) {
                            if (data.Result) {
                                if (data1[0] == 15005) {
                                    $this.attr('name', '15006,' + data1[1]).text('下架')
                                    $this.attr('name', '15006,' + data1[1]).next().css('display','none');
                                } else {
                                    $this.attr('name', '15005,' + data1[1]).text('上架')
                                    $this.attr('name', '15005,' + data1[1]).next().css('display','inline-block');
                                }

                            }
                        })
                    } else {
                        alert('请调整刊例时间！')
                    }
                })
            }

            // 全部
            $('.tab_menu li').eq(0).on('click', function () {
                setAjax({
                      url: seleceurl,
                    type: "GET",
                    data: {'pageIndex': 1, 'pageSize': 20, 'BusinessType': 14001}
                    //"pageIndex=1&pageSize=20&BusinessType=14001",
                }, function (data) {

                    if (data.Result.List != null) {
                        // 后台返回数据处理
                        for (var i = 0; i < data.Result.List.length; i++) {
                            if (data.Result.List[i].FistName == null) {
                                data.Result.List[i].FistName = [];
                            }
                            if (data.Result.List[i].SecondName == null) {
                                data.Result.List[i].SecondName = []
                            }
                            if (data.Result.List[i].ThridName == null) {
                                data.Result.List[i].ThridName = []
                            }
                            if (data.Result.List[i].FourthName == null) {
                                data.Result.List[i].FourthName = []
                            }

                        }
                    }
                    $('tbody').eq(0).html(ejs.render($('#weixin1').html(), data));
                    if (data.Result.TotleCount != 0) {
                        var counts = data.Result.TotleCount;
                        //分页
                        $("#pageContainer1").pagination(
                            counts,
                            {
                                items_per_page: 20, //每页显示多少条记录（默认为20条）
                                callback: function (currPage, jg) {
                                    // ajax请求
                                    setAjax({
                                          url: seleceurl,
                                        type: "GET",
                                        data: {'pageIndex': currPage, 'pageSize': 20, 'BusinessType': 14001}
                                        //"pageIndex=" + currPage + "&pageSize=20&BusinessType=14001"
                                    }, function (data) {

                                        if (data.Result.List != null) {
                                            // 后台返回数据处理
                                            for (var i = 0; i < data.Result.List.length; i++) {
                                                if (data.Result.List[i].FistName == null) {
                                                    data.Result.List[i].FistName = [];
                                                }
                                                if (data.Result.List[i].SecondName == null) {
                                                    data.Result.List[i].SecondName = []
                                                }
                                                if (data.Result.List[i].ThridName == null) {
                                                    data.Result.List[i].ThridName = []
                                                }
                                                if (data.Result.List[i].FourthName == null) {
                                                    data.Result.List[i].FourthName = []
                                                }

                                            }
                                        }
                                        $('tbody').eq(0).html(ejs.render($('#weixin1').html(), data));
                                        // 公共功能
                                        wxpublicFunction(0)

                                    })
                                }
                            });

                    } else {
                        $('#pageContainer1').html('<img src="/images/no_data.png" style="display: block;margin: 70px auto;">')
                    }
                    // 公共功能
                    wxpublicFunction(0)
                })
                //查询
                $('.but_query').eq(0).off('click').on('click', function () {
                    // 获取微信号
                    var weixin = $('.state').eq(0).find('li:eq(0) input').val();
                    // 获取微信名称
                    var weixinname = $('.state').eq(0).find('li:eq(1) input').val();
                    // 获取来源
                    var origin = $('.state').eq(0).find('li:eq(2) select option:selected').attr('name');
                    // 获取到期时间
                    var expiredate = $('.state').eq(0).find('li:eq(3) select option:selected').attr('name');
                    setAjax({
                          url: seleceurl,
                        type: "GET",
                        data: {
                            'pageIndex': 1,
                            'pageSize': 20,
                            'BusinessType': 14001,
                            'Number': weixin,
                            'Name': weixinname,
                            'EndTime': expiredate,
                            'Source': origin
                        }
                        //"pageIndex=1&pageSize=20&BusinessType=14001&Number=" + weixin + "&Name=" + weixinname + "&EndTime=" + expiredate + "&Source=" + origin,
                    }, function (data) {

                        if (data.Result.List != null) {
                            // 后台返回数据处理
                            for (var i = 0; i < data.Result.List.length; i++) {
                                if (data.Result.List[i].FistName == null) {
                                    data.Result.List[i].FistName = [];
                                }
                                if (data.Result.List[i].SecondName == null) {
                                    data.Result.List[i].SecondName = []
                                }
                                if (data.Result.List[i].ThridName == null) {
                                    data.Result.List[i].ThridName = []
                                }
                                if (data.Result.List[i].FourthName == null) {
                                    data.Result.List[i].FourthName = []
                                }

                            }
                        }
                        $('tbody').eq(0).html(ejs.render($('#weixin1').html(), data));
                        if (data.Result.TotleCount != 0) {
                            var counts = data.Result.TotleCount;
                            //分页
                            $("#pageContainer1").pagination(
                                counts,
                                {
                                    items_per_page: 20, //每页显示多少条记录（默认为20条）
                                    callback: function (currPage, jg) {
                                        // ajax请求
                                        setAjax({
                                              url: seleceurl,
                                            type: "GET",
                                            data: {
                                                'pageIndex': currPage,
                                                'pageSize': 20,
                                                'BusinessType': 14001,
                                                'Number': weixin,
                                                'Name': weixinname,
                                                'EndTime': expiredate,
                                                'Source': origin
                                            }
                                            //"pageIndex="+currPage +"&pageSize=20&BusinessType=14001&Number=" + weixin + "&Name=" + weixinname + "&EndTime=" + expiredate + "&Source=" + origin,
                                        }, function (data) {

                                            if (data.Result.List != null) {
                                                // 后台返回数据处理
                                                for (var i = 0; i < data.Result.List.length; i++) {
                                                    if (data.Result.List[i].FistName == null) {
                                                        data.Result.List[i].FistName = [];
                                                    }
                                                    if (data.Result.List[i].SecondName == null) {
                                                        data.Result.List[i].SecondName = []
                                                    }
                                                    if (data.Result.List[i].ThridName == null) {
                                                        data.Result.List[i].ThridName = []
                                                    }
                                                    if (data.Result.List[i].FourthName == null) {
                                                        data.Result.List[i].FourthName = []
                                                    }

                                                }
                                            }
                                            $('tbody').eq(0).html(ejs.render($('#weixin1').html(), data));
                                            // 公共功能
                                            wxpublicFunction(0)

                                        })
                                    }
                                });

                        } else {
                            $('#pageContainer1').html('<img src="/images/no_data.png" style="display: block;margin: 70px auto;">')
                        }

                        // 公共功能
                        wxpublicFunction(0)
                    })
                })

            })
            $('.tab_menu li').eq(0).click()
            // 待审核
            $('.tab_menu li').eq(1).on('click', function () {
                setAjax({
                      url: seleceurl,
                    type: "GET",
                    data: {'pageIndex': 1, 'pageSize': 20, 'BusinessType': 14001, 'Status': 15002}
                    //"pageIndex=1&pageSize=20&BusinessType=14001&Status=15002",
                }, function (data) {

                    if (data.Result.List != null) {
                        // 后台返回数据处理
                        for (var i = 0; i < data.Result.List.length; i++) {
                            if (data.Result.List[i].FistName == null) {
                                data.Result.List[i].FistName = [];
                            }
                            if (data.Result.List[i].SecondName == null) {
                                data.Result.List[i].SecondName = []
                            }
                            if (data.Result.List[i].ThridName == null) {
                                data.Result.List[i].ThridName = []
                            }
                            if (data.Result.List[i].FourthName == null) {
                                data.Result.List[i].FourthName = []
                            }

                        }
                    }
                    $('tbody').eq(1).html(ejs.render($('#weixin2').html(), data));
                    if (data.Result.TotleCount != 0) {
                        var counts = data.Result.TotleCount;
                        //分页
                        $("#pageContainer2").pagination(
                            counts,
                            {
                                items_per_page: 20, //每页显示多少条记录（默认为20条）
                                callback: function (currPage, jg) {
                                    // ajax请求
                                    setAjax({
                                          url: seleceurl,
                                        type: "GET",
                                        data: {
                                            'pageIndex': currPage,
                                            'pageSize': 20,
                                            'BusinessType': 14001,
                                            'Status': 15002
                                        }
                                        //"pageIndex=" + currPage + "&pageSize=20&BusinessType=14001&Status=15002"
                                    }, function (data) {

                                        if (data.Result.List != null) {
                                            // 后台返回数据处理
                                            for (var i = 0; i < data.Result.List.length; i++) {
                                                if (data.Result.List[i].FistName == null) {
                                                    data.Result.List[i].FistName = [];
                                                }
                                                if (data.Result.List[i].SecondName == null) {
                                                    data.Result.List[i].SecondName = []
                                                }
                                                if (data.Result.List[i].ThridName == null) {
                                                    data.Result.List[i].ThridName = []
                                                }
                                                if (data.Result.List[i].FourthName == null) {
                                                    data.Result.List[i].FourthName = []
                                                }

                                            }
                                        }
                                        $('tbody').eq(1).html(ejs.render($('#weixin2').html(), data));
                                        // 公共功能
                                        wxpublicFunction(1)

                                    })
                                }
                            });

                    } else {
                        $('#pageContainer2').html('<img src="/images/no_data.png" style="display: block;margin: 70px auto;">')
                    }
                    // 公共功能
                    wxpublicFunction(1)
                })
                //查询
                $('.but_query').eq(1).off('click').on('click', function () {
                    // 获取微信号
                    var weixin = $('.state').eq(1).find('li:eq(0) input').val();
                    // 获取微信名称
                    var weixinname = $('.state').eq(1).find('li:eq(1) input').val();
                    // 获取来源
                    var origin = $('.state').eq(1).find('li:eq(2) select option:selected').attr('name');
                    // 获取到期时间
                    var expiredate = $('.state').eq(1).find('li:eq(3) select option:selected').attr('name');
                    setAjax({
                          url: seleceurl,
                        type: "GET",
                        data: {
                            'pageIndex': 1,
                            'pageSize': 20,
                            'BusinessType': 14001,
                            'Status': 15002,
                            'Number': weixin,
                            'Name': weixinname,
                            'EndTime': expiredate,
                            'Source': origin
                        }
                        //"pageIndex=1&pageSize=20&BusinessType=14001&Status=15002&Number=" + weixin + "&Name=" + weixinname + "&EndTime=" + expiredate + "&Source=" + origin
                    }, function (data) {

                        if (data.Result.List != null) {
                            // 后台返回数据处理
                            for (var i = 0; i < data.Result.List.length; i++) {
                                if (data.Result.List[i].FistName == null) {
                                    data.Result.List[i].FistName = [];
                                }
                                if (data.Result.List[i].SecondName == null) {
                                    data.Result.List[i].SecondName = []
                                }
                                if (data.Result.List[i].ThridName == null) {
                                    data.Result.List[i].ThridName = []
                                }
                                if (data.Result.List[i].FourthName == null) {
                                    data.Result.List[i].FourthName = []
                                }

                            }
                        }
                        $('tbody').eq(1).html(ejs.render($('#weixin2').html(), data));
                        if (data.Result.TotleCount != 0) {
                            var counts = data.Result.TotleCount;
                            //分页
                            $("#pageContainer2").pagination(
                                counts,
                                {
                                    items_per_page: 20, //每页显示多少条记录（默认为20条）
                                    callback: function (currPage, jg) {
                                        // ajax请求
                                        setAjax({
                                              url: seleceurl,
                                            type: "GET",
                                            data: {
                                                'pageIndex': currPage,
                                                'pageSize': 20,
                                                'BusinessType': 14001,
                                                'Status': 15002,
                                                'Number': weixin,
                                                'Name': weixinname,
                                                'EndTime': expiredate,
                                                'Source': origin
                                            }
                                            //"pageIndex="+currPage +"&pageSize=20&BusinessType=14001&Status=15002&Number=" + weixin + "&Name=" + weixinname + "&EndTime=" + expiredate + "&Source=" + origin
                                        }, function (data) {

                                            if (data.Result.List != null) {
                                                // 后台返回数据处理
                                                for (var i = 0; i < data.Result.List.length; i++) {
                                                    if (data.Result.List[i].FistName == null) {
                                                        data.Result.List[i].FistName = [];
                                                    }
                                                    if (data.Result.List[i].SecondName == null) {
                                                        data.Result.List[i].SecondName = []
                                                    }
                                                    if (data.Result.List[i].ThridName == null) {
                                                        data.Result.List[i].ThridName = []
                                                    }
                                                    if (data.Result.List[i].FourthName == null) {
                                                        data.Result.List[i].FourthName = []
                                                    }

                                                }
                                            }
                                            $('tbody').eq(1).html(ejs.render($('#weixin2').html(), data));
                                            // 公共功能
                                            wxpublicFunction(1)

                                        })
                                    }
                                });

                        } else {
                            $('#pageContainer2').html('<img src="/images/no_data.png" style="display: block;margin: 70px auto;">')
                        }
                        // 公共功能
                        wxpublicFunction(1)
                    })
                })

            })
            // 通过
            $('.tab_menu li').eq(2).on('click', function () {
                setAjax({
                      url: seleceurl,
                    type: "GET",
                    data: {'pageIndex': 1, 'pageSize': 20, 'BusinessType': 14001, 'Status': 15003}
                    //"pageIndex=1&pageSize=20&BusinessType=14001&Status=15003"
                }, function (data) {

                    if (data.Result.List != null) {
                        // 后台返回数据处理
                        for (var i = 0; i < data.Result.List.length; i++) {
                            if (data.Result.List[i].FistName == null) {
                                data.Result.List[i].FistName = [];
                            }
                            if (data.Result.List[i].SecondName == null) {
                                data.Result.List[i].SecondName = []
                            }
                            if (data.Result.List[i].ThridName == null) {
                                data.Result.List[i].ThridName = []
                            }
                            if (data.Result.List[i].FourthName == null) {
                                data.Result.List[i].FourthName = []
                            }

                        }
                    }
                    $('tbody').eq(2).html(ejs.render($('#weixin3').html(), data));
                    if (data.Result.TotleCount != 0) {
                        var counts = data.Result.TotleCount;
                        //分页
                        $("#pageContainer3").pagination(
                            counts,
                            {
                                items_per_page: 20, //每页显示多少条记录（默认为20条）
                                callback: function (currPage, jg) {
                                    // ajax请求
                                    setAjax({
                                          url: seleceurl,
                                        type: "GET",
                                        data: {
                                            'pageIndex': currPage,
                                            'pageSize': 20,
                                            'BusinessType': 14001,
                                            'Status': 15003
                                        }
                                        //"pageIndex=" + currPage + "&pageSize=20&BusinessType=14001&Status=15002"
                                    }, function (data) {

                                        if (data.Result.List != null) {
                                            // 后台返回数据处理
                                            for (var i = 0; i < data.Result.List.length; i++) {
                                                if (data.Result.List[i].FistName == null) {
                                                    data.Result.List[i].FistName = [];
                                                }
                                                if (data.Result.List[i].SecondName == null) {
                                                    data.Result.List[i].SecondName = []
                                                }
                                                if (data.Result.List[i].ThridName == null) {
                                                    data.Result.List[i].ThridName = []
                                                }
                                                if (data.Result.List[i].FourthName == null) {
                                                    data.Result.List[i].FourthName = []
                                                }

                                            }
                                        }
                                        $('tbody').eq(2).html(ejs.render($('#weixin3').html(), data));
                                        // 公共功能
                                        wxpublicFunction(2)

                                    })
                                }
                            });

                    } else {
                        $('#pageContainer3').html('<img src="/images/no_data.png" style="display: block;margin: 70px auto;">')
                    }
                    // 公共功能
                    wxpublicFunction(2)
                })
                //查询
                $('.but_query').eq(2).off('click').on('click', function () {
                    // 获取微信号
                    var weixin = $('.state').eq(2).find('li:eq(0) input').val();
                    // 获取微信名称
                    var weixinname = $('.state').eq(2).find('li:eq(1) input').val();
                    // 获取来源
                    var origin = $('.state').eq(2).find('li:eq(2) select option:selected').attr('name');
                    // 获取到期时间
                    var expiredate = $('.state').eq(2).find('li:eq(3) select option:selected').attr('name');
                    setAjax({
                          url: seleceurl,
                        type: "GET",
                        data: {
                            'pageIndex': 1,
                            'pageSize': 20,
                            'BusinessType': 14001,
                            'Status': 15003,
                            'Number': weixin,
                            'Name': weixinname,
                            'EndTime': expiredate,
                            'Source': origin
                        }
                        //"pageIndex=1&pageSize=20&BusinessType=14001&Status=15003&Number=" + weixin + "&Name=" + weixinname + "&EndTime=" + expiredate + "&Source=" + origin
                    }, function (data) {

                        if (data.Result.List != null) {
                            // 后台返回数据处理
                            for (var i = 0; i < data.Result.List.length; i++) {
                                if (data.Result.List[i].FistName == null) {
                                    data.Result.List[i].FistName = [];
                                }
                                if (data.Result.List[i].SecondName == null) {
                                    data.Result.List[i].SecondName = []
                                }
                                if (data.Result.List[i].ThridName == null) {
                                    data.Result.List[i].ThridName = []
                                }
                                if (data.Result.List[i].FourthName == null) {
                                    data.Result.List[i].FourthName = []
                                }

                            }
                        }
                        $('tbody').eq(2).html(ejs.render($('#weixin3').html(), data));
                        if (data.Result.TotleCount != 0) {
                            var counts = data.Result.TotleCount;
                            //分页
                            $("#pageContainer3").pagination(
                                counts,
                                {
                                    items_per_page: 20, //每页显示多少条记录（默认为20条）
                                    callback: function (currPage, jg) {
                                        // ajax请求
                                        setAjax({
                                              url: seleceurl,
                                            type: "GET",
                                            data: {
                                                'pageIndex': currPage,
                                                'pageSize': 20,
                                                'BusinessType': 14001,
                                                'Status': 15003,
                                                'Number': weixin,
                                                'Name': weixinname,
                                                'EndTime': expiredate,
                                                'Source': origin
                                            }
                                            //"pageIndex="+currPage +"&pageSize=20&BusinessType=14001&Status=15003&Number=" + weixin + "&Name=" + weixinname + "&EndTime=" + expiredate + "&Source=" + origin
                                        }, function (data) {
                                            if (data.Result.List != null) {
                                                // 后台返回数据处理
                                                for (var i = 0; i < data.Result.List.length; i++) {
                                                    if (data.Result.List[i].FistName == null) {
                                                        data.Result.List[i].FistName = [];
                                                    }
                                                    if (data.Result.List[i].SecondName == null) {
                                                        data.Result.List[i].SecondName = []
                                                    }
                                                    if (data.Result.List[i].ThridName == null) {
                                                        data.Result.List[i].ThridName = []
                                                    }
                                                    if (data.Result.List[i].FourthName == null) {
                                                        data.Result.List[i].FourthName = []
                                                    }

                                                }
                                            }
                                            $('tbody').eq(2).html(ejs.render($('#weixin3').html(), data));
                                            // 公共功能
                                            wxpublicFunction(2)

                                        })
                                    }
                                });

                        } else {
                            $('#pageContainer3').html('<img src="/images/no_data.png" style="display: block;margin: 70px auto;">')
                        }

                        // 公共功能
                        wxpublicFunction(2)
                    })
                })

            })
            // 驳回
            $('.tab_menu li').eq(3).on('click', function () {
                setAjax({
                      url: seleceurl,
                    type: "GET",
                    data: {'pageIndex': 1, 'pageSize': 20, 'BusinessType': 14001, 'Status': 15004}
                    //"pageIndex=1&pageSize=20&BusinessType=14001&Status=15004",
                }, function (data) {

                    if (data.Result.List != null) {
                        // 后台返回数据处理
                        for (var i = 0; i < data.Result.List.length; i++) {
                            if (data.Result.List[i].FistName == null) {
                                data.Result.List[i].FistName = [];
                            }
                            if (data.Result.List[i].SecondName == null) {
                                data.Result.List[i].SecondName = []
                            }
                            if (data.Result.List[i].ThridName == null) {
                                data.Result.List[i].ThridName = []
                            }
                            if (data.Result.List[i].FourthName == null) {
                                data.Result.List[i].FourthName = []
                            }

                        }
                    }
                    $('tbody').eq(3).html(ejs.render($('#weixin4').html(), data));
                    if (data.Result.TotleCount != 0) {
                        var counts = data.Result.TotleCount;
                        //分页
                        $("#pageContainer4").pagination(
                            counts,
                            {
                                items_per_page: 20, //每页显示多少条记录（默认为20条）
                                callback: function (currPage, jg) {
                                    // ajax请求
                                    setAjax({
                                          url: seleceurl,
                                        type: "GET",
                                        data: {
                                            'pageIndex': currPage,
                                            'pageSize': 20,
                                            'BusinessType': 14001,
                                            'Status': 15004
                                        }
                                        //"pageIndex=" + currPage + "&pageSize=20&BusinessType=14001&Status=15004"
                                    }, function (data) {

                                        if (data.Result.List != null) {
                                            // 后台返回数据处理
                                            for (var i = 0; i < data.Result.List.length; i++) {
                                                if (data.Result.List[i].FistName == null) {
                                                    data.Result.List[i].FistName = [];
                                                }
                                                if (data.Result.List[i].SecondName == null) {
                                                    data.Result.List[i].SecondName = []
                                                }
                                                if (data.Result.List[i].ThridName == null) {
                                                    data.Result.List[i].ThridName = []
                                                }
                                                if (data.Result.List[i].FourthName == null) {
                                                    data.Result.List[i].FourthName = []
                                                }

                                            }
                                        }
                                        $('tbody').eq(3).html(ejs.render($('#weixin4').html(), data));
                                        // 公共功能
                                        wxpublicFunction(3)

                                    })
                                }
                            });

                    } else {
                        $('#pageContainer4').html('<img src="/images/no_data.png" style="display: block;margin: 70px auto;">')
                    }
                    // 公共功能
                    wxpublicFunction(3)
                })
                //查询
                $('.but_query').eq(3).off('click').on('click', function () {
                    // 获取微信号
                    var weixin = $('.state').eq(3).find('li:eq(0) input').val();
                    // 获取微信名称
                    var weixinname = $('.state').eq(3).find('li:eq(1) input').val();
                    // 获取来源
                    var origin = $('.state').eq(3).find('li:eq(2) select option:selected').attr('name');
                    // 获取到期时间
                    var expiredate = $('.state').eq(3).find('li:eq(3) select option:selected').attr('name');
                    setAjax({
                          url: seleceurl,
                        type: "GET",
                        data: {
                            'pageIndex': 1,
                            'pageSize': 20,
                            'BusinessType': 14001,
                            'Status': 15004,
                            'Number': weixin,
                            'Name': weixinname,
                            'EndTime': expiredate,
                            'Source': origin
                        }
                        //"pageIndex=1&pageSize=20&BusinessType=14001&Status=15004&Number=" + weixin + "&Name=" + weixinname + "&EndTime=" + expiredate + "&Source=" + origin
                    }, function (data) {

                        if (data.Result.List != null) {
                            // 后台返回数据处理
                            for (var i = 0; i < data.Result.List.length; i++) {
                                if (data.Result.List[i].FistName == null) {
                                    data.Result.List[i].FistName = [];
                                }
                                if (data.Result.List[i].SecondName == null) {
                                    data.Result.List[i].SecondName = []
                                }
                                if (data.Result.List[i].ThridName == null) {
                                    data.Result.List[i].ThridName = []
                                }
                                if (data.Result.List[i].FourthName == null) {
                                    data.Result.List[i].FourthName = []
                                }

                            }
                        }
                        $('tbody').eq(3).html(ejs.render($('#weixin4').html(), data));
                        if (data.Result.TotleCount != 0) {
                            var counts = data.Result.TotleCount;
                            //分页
                            $("#pageContainer4").pagination(
                                counts,
                                {
                                    items_per_page: 20, //每页显示多少条记录（默认为20条）
                                    callback: function (currPage, jg) {
                                        // ajax请求
                                        setAjax({
                                              url: seleceurl,
                                            type: "GET",
                                            data: {
                                                'pageIndex': currPage,
                                                'pageSize': 20,
                                                'BusinessType': 14001,
                                                'Status': 15004,
                                                'Number': weixin,
                                                'Name': weixinname,
                                                'EndTime': expiredate,
                                                'Source': origin
                                            }
                                            //"pageIndex="+currPage +"&pageSize=20&BusinessType=14001&Status=15004&Number=" + weixin + "&Name=" + weixinname + "&EndTime=" + expiredate + "&Source=" + origin
                                        }, function (data) {
                                            if (data.Result.List != null) {
                                                // 后台返回数据处理
                                                for (var i = 0; i < data.Result.List.length; i++) {
                                                    if (data.Result.List[i].FistName == null) {
                                                        data.Result.List[i].FistName = [];
                                                    }
                                                    if (data.Result.List[i].SecondName == null) {
                                                        data.Result.List[i].SecondName = []
                                                    }
                                                    if (data.Result.List[i].ThridName == null) {
                                                        data.Result.List[i].ThridName = []
                                                    }
                                                    if (data.Result.List[i].FourthName == null) {
                                                        data.Result.List[i].FourthName = []
                                                    }

                                                }
                                            }
                                            $('tbody').eq(3).html(ejs.render($('#weixin4').html(), data));
                                            // 公共功能
                                            wxpublicFunction(3)

                                        })
                                    }
                                });

                        } else {
                            $('#pageContainer4').html('<img src="/images/no_data.png" style="display: block;margin: 70px auto;">')
                        }
                        // 公共功能
                        wxpublicFunction(3)
                    })
                })

            })
        }
        // 微博刊例列表
        if ($('div[name="operate-sina"]').attr('name') == 'operate-sina') {
            $('div[name="operate-sina"]').css({display: 'block'})
            function wbpublicFunction(i) {
                judgeAuthority(2)
                // 点击申请上架
                $('tbody').find("tr").find('td:last a[name^=15002]').off('click').on('click', function () {
                    var data = $(this).attr('name').split(',')
                    if(Date.parse(new Date().format("yyyy/MM/dd")) - Date.parse($(this).parent().attr('name').split(' ')[0].split('-').join('/')) < 0 ) {
                        $.openPopupLayer({
                            name: "shipingclickApply",
                            url: "pricelist-application.html",
                            error: function (dd) {
                                alert(dd.status);
                            },
                            success: function () {
                                // 点击确定
                                $('.layer_con').find('.button').off('click').on('click', function () {
                                    setAjax({
                                        url: Ajaxurl,
                                        type: "POST",
                                        data: {
                                            "PubID": data[1],
                                            "Status": data[0]
                                        }
                                    }, function (data) {
                                        $.closePopupLayer('shipingclickApply');
                                        if (data.Result) {
                                            window.location.reload();
                                        }
                                    })
                                })                 // 点击取消
                                    .end().find('.but_keep').off('click').on('click', function () {
                                    $.closePopupLayer('shipingclickApply')
                                })

                            }
                        });
                    }else {
                        alert('请调整刊例时间！')
                    }
                })
                // 点击驳回原因
                $('tbody').eq(i).find('tr').find('td[name=15004]').find('a').off('click').on('click', function () {
                    // 驳回原因的内容
                    var data = $(this).next().text();
                    $.openPopupLayer({
                        name: "Reject",
                        url: "pricelist-reason.html",
                        error: function (dd) {
                            alert(dd.status);
                        },
                        success: function () {
                            $('.mb25').html(data)
                            //点击取消
                            $('.button').off('click').on('click', function () {
                                $.closePopupLayer('Reject');
                            });
                            $('.fr>a').off('click').on('click', function () {
                                $.closePopupLayer('Reject');
                            });
                        }
                    });
                })
                // 上下架切换
                $('tbody tr').find('td:last span a').off('click').on('click', function (e) {
                    if(e.target.innerHTML=='编辑'){
                        return
                    }
                    var $this = $(this);
                    var data1 = $this.attr('name').split(',');
                    if($this.text()=='下架'){
                        setAjax({
                            url: Ajaxurl,
                            type: "POST",
                            data: {
                                "PubID": data1[1],
                                "Status": data1[0]
                            }
                        }, function (data) {
                            if (data.Result) {
                                // window.location.reload();
                                if (data1[0] == 15005) {
                                    $this.attr('name', '15006,' + data1[1]).text('下架');
                                    $this.attr('name', '15006,' + data1[1]).next().css('display','none')
                                } else {
                                    $this.attr('name', '15005,' + data1[1]).text('上架');
                                    $this.attr('name', '15005,' + data1[1]).next().css('display','inline-block')
                                }

                            }
                        })
                        return
                    }
                    if (Date.parse(new Date().format("yyyy/MM/dd")) - Date.parse($(this).parent().parent().attr('name').split(' ')[0].split('-').join('/')) < 0) {
                        setAjax({
                            url: Ajaxurl,
                            type: "POST",
                            data: {
                                "PubID": data1[1],
                                "Status": data1[0]
                            }
                        }, function (data) {
                            if (data.Result) {
                                // window.location.reload();
                                if (data1[0] == 15005) {
                                    $this.attr('name', '15006,' + data1[1]).text('下架')
                                    $this.attr('name', '15006,' + data1[1]).next().css('display','none')
                                } else {
                                    $this.attr('name', '15005,' + data1[1]).text('上架')
                                    $this.attr('name', '15005,' + data1[1]).next().css('display','inline-block')
                                }

                            }
                        })
                    } else {
                        alert('请调整刊例时间！');
                    }
                })

            };
            // 全部
            $('.tab_menu li').eq(0).on('click', function () {
                setAjax({
                      url: seleceurl,
                    type: "GET",
                    data: {'pageIndex': 1, 'pageSize': 20, 'BusinessType': 14003}
                    //"pageIndex=1&pageSize=20&BusinessType=14003"
                }, function (data) {

                    if (data.Result.List != null) {
                        // 后台返回数据处理
                        for (var i = 0; i < data.Result.List.length; i++) {
                            if (data.Result.List[i].BeginTime) {
                                data.Result.List[i].BeginTime = data.Result.List[i].BeginTime.split(" ")[0];
                            }
                            if (data.Result.List[i].EndTime) {
                                data.Result.List[i].EndTime = data.Result.List[i].EndTime.split(" ")[0];
                            }
                            if (data.Result.List[i].FistName == null) {
                                data.Result.List[i].FistName = [];
                            }
                            if (data.Result.List[i].SecondName == null) {
                                data.Result.List[i].SecondName = [];
                            }
                            if (data.Result.List[i].ThridName == null) {
                                data.Result.List[i].ThridName = [];
                            }

                        }
                    }
                    $('tbody').eq(0).html(ejs.render($('#weibo1').html(), data));
                    if (data.Result.TotleCount != 0) {
                        var counts = data.Result.TotleCount;
                        //分页
                        $("#pageContainer1").pagination(
                            counts,
                            {
                                items_per_page: 20, //每页显示多少条记录（默认为20条）
                                callback: function (currPage, jg) {
                                    // ajax请求
                                    setAjax({
                                          url: seleceurl,
                                        type: "GET",
                                        data: {'pageIndex': currPage, 'pageSize': 20, 'BusinessType': 14003}
                                        //"pageIndex=" + currPage + "&pageSize=20&BusinessType=14003"
                                    }, function (data) {
                                        if (data.Result.List != null) {
                                            // 后台返回数据处理
                                            for (var i = 0; i < data.Result.List.length; i++) {
                                                if (data.Result.List[i].BeginTime) {
                                                    data.Result.List[i].BeginTime = data.Result.List[i].BeginTime.split(" ")[0];
                                                }
                                                if (data.Result.List[i].EndTime) {
                                                    data.Result.List[i].EndTime = data.Result.List[i].EndTime.split(" ")[0];
                                                }
                                                if (data.Result.List[i].FistName == null) {
                                                    data.Result.List[i].FistName = [];
                                                }
                                                if (data.Result.List[i].SecondName == null) {
                                                    data.Result.List[i].SecondName = [];
                                                }
                                                if (data.Result.List[i].ThridName == null) {
                                                    data.Result.List[i].ThridName = [];
                                                }

                                            }
                                        }
                                        $('tbody').eq(0).html(ejs.render($('#weibo1').html(), data));
                                        // 公共功能
                                        wbpublicFunction(0)

                                    })
                                }
                            });

                    } else {
                        $('#pageContainer1').html('<img src="/images/no_data.png" style="display: block;margin: 70px auto;">')
                    }
                    // 公共功能
                    wbpublicFunction(0);
                })
                //查询
                $('.but_query').eq(0).off('click').on('click', function () {
                    // 获取账号
                    var account = $('.state').eq(0).find('li:eq(0) input').val();
                    // 获取名称
                    var accountname = $('.state').eq(0).find('li:eq(1) input').val();
                    // 获取来源
                    var origin = $('.state').eq(0).find('li:eq(2) select option:selected').attr('name');
                    // 获取到期时间
                    var expiredate = $('.state').eq(0).find('li:eq(3) select option:selected').attr('name');
                    setAjax({
                          url: seleceurl,
                        type: "GET",
                        data: {
                            'pageIndex': 1,
                            'pageSize': 20,
                            'BusinessType': 14003,
                            'Number': account,
                            'Name': accountname,
                            'EndTime': expiredate,
                            'Source': origin
                        }
                        //"pageIndex=1&pageSize=20&BusinessType=14003&Number=" + account + "&Name=" + accountname + "&EndTime=" + expiredate + "&Source=" + origin,
                    }, function (data) {

                        if (data.Result.List != null) {
                            // 后台返回数据处理
                            for (var i = 0; i < data.Result.List.length; i++) {
                                if (data.Result.List[i].BeginTime) {
                                    data.Result.List[i].BeginTime = data.Result.List[i].BeginTime.split(" ")[0];
                                }
                                if (data.Result.List[i].EndTime) {
                                    data.Result.List[i].EndTime = data.Result.List[i].EndTime.split(" ")[0];
                                }
                                if (data.Result.List[i].FistName == null) {
                                    data.Result.List[i].FistName = [];
                                }
                                if (data.Result.List[i].SecondName == null) {
                                    data.Result.List[i].SecondName = [];
                                }
                                if (data.Result.List[i].ThridName == null) {
                                    data.Result.List[i].ThridName = [];
                                }

                            }
                        }
                        $('tbody').eq(0).html(ejs.render($('#weibo1').html(), data));
                        if (data.Result.TotleCount != 0) {
                            var counts = data.Result.TotleCount;
                            //分页
                            $("#pageContainer1").pagination(
                                counts,
                                {
                                    items_per_page: 20, //每页显示多少条记录（默认为20条）
                                    callback: function (currPage, jg) {
                                        // ajax请求
                                        setAjax({
                                              url: seleceurl,
                                            type: "GET",
                                            data: {
                                                'pageIndex': currPage,
                                                'pageSize': 20,
                                                'BusinessType': 14003,
                                                'pageIndex': 1,
                                                'pageSize': 20,
                                                'BusinessType': 14004,
                                                'Number': account,
                                                'Name': accountname,
                                                '&EndTime': expiredate,
                                                'Source': origin
                                            }
                                            //"pageIndex=" + currPage + "&pageSize=20&BusinessType=14003pageIndex=1&pageSize=20&BusinessType=14004&Number=" + account + "&Name=" + accountname + "&EndTime=" + expiredate + "&Source=" + origin
                                        }, function (data) {

                                            if (data.Result.List != null) {
                                                // 后台返回数据处理
                                                for (var i = 0; i < data.Result.List.length; i++) {
                                                    if (data.Result.List[i].BeginTime) {
                                                        data.Result.List[i].BeginTime = data.Result.List[i].BeginTime.split(" ")[0];
                                                    }
                                                    if (data.Result.List[i].EndTime) {
                                                        data.Result.List[i].EndTime = data.Result.List[i].EndTime.split(" ")[0];
                                                    }
                                                    if (data.Result.List[i].FistName == null) {
                                                        data.Result.List[i].FistName = [];
                                                    }
                                                    if (data.Result.List[i].SecondName == null) {
                                                        data.Result.List[i].SecondName = [];
                                                    }
                                                    if (data.Result.List[i].ThridName == null) {
                                                        data.Result.List[i].ThridName = [];
                                                    }

                                                }
                                            }
                                            $('tbody').eq(0).html(ejs.render($('#weibo1').html(), data));
                                            // 公共功能
                                            wbpublicFunction(0)

                                        })
                                    }
                                });

                        } else {
                            $('#pageContainer1').html('<img src="/images/no_data.png" style="display: block;margin: 70px auto;">')
                        }
                        // 公共功能
                        wbpublicFunction(0)
                    })
                })
            })
            $('.tab_menu li').eq(0).click();
            // 待审核
            $('.tab_menu li').eq(1).on('click', function () {
                setAjax({
                      url: seleceurl,
                    type: "GET",
                    data: {'pageIndex': 1, 'pageSize': 20, 'BusinessType': 14003, 'Status': 15002}
                    //"pageIndex=1&pageSize=20&BusinessType=14003&Status=15002"
                }, function (data) {

                    if (data.Result.List != null) {
                        // 后台返回数据处理
                        for (var i = 0; i < data.Result.List.length; i++) {
                            if (data.Result.List[i].BeginTime) {
                                data.Result.List[i].BeginTime = data.Result.List[i].BeginTime.split(" ")[0];
                            }
                            if (data.Result.List[i].EndTime) {
                                data.Result.List[i].EndTime = data.Result.List[i].EndTime.split(" ")[0];
                            }
                            if (data.Result.List[i].FistName == null) {
                                data.Result.List[i].FistName = [];
                            }
                            if (data.Result.List[i].SecondName == null) {
                                data.Result.List[i].SecondName = [];
                            }
                            if (data.Result.List[i].ThridName == null) {
                                data.Result.List[i].ThridName = [];
                            }

                        }
                    }
                    $('tbody').eq(1).html(ejs.render($('#weibo2').html(), data));
                    if (data.Result.TotleCount != 0) {
                        var counts = data.Result.TotleCount;
                        //分页
                        $("#pageContainer2").pagination(
                            counts,
                            {
                                items_per_page: 20, //每页显示多少条记录（默认为20条）
                                callback: function (currPage, jg) {
                                    // ajax请求
                                    setAjax({
                                          url: seleceurl,
                                        type: "GET",
                                        data: {
                                            'pageIndex': currPage,
                                            'pageSize': 20,
                                            'BusinessType': 14003,
                                            'Status': 15002
                                        }
                                        //"pageIndex=" + currPage + "&pageSize=20&BusinessType=14003&Status=15002"
                                    }, function (data) {

                                        if (data.Result.List != null) {
                                            // 后台返回数据处理
                                            for (var i = 0; i < data.Result.List.length; i++) {
                                                if (data.Result.List[i].BeginTime) {
                                                    data.Result.List[i].BeginTime = data.Result.List[i].BeginTime.split(" ")[0];
                                                }
                                                if (data.Result.List[i].EndTime) {
                                                    data.Result.List[i].EndTime = data.Result.List[i].EndTime.split(" ")[0];
                                                }
                                                if (data.Result.List[i].FistName == null) {
                                                    data.Result.List[i].FistName = [];
                                                }
                                                if (data.Result.List[i].SecondName == null) {
                                                    data.Result.List[i].SecondName = [];
                                                }
                                                if (data.Result.List[i].ThridName == null) {
                                                    data.Result.List[i].ThridName = [];
                                                }

                                            }
                                        }
                                        $('tbody').eq(1).html(ejs.render($('#weibo2').html(), data));
                                        // 公共功能
                                        wbpublicFunction(1)

                                    })
                                }
                            });

                    } else {
                        $('#pageContainer2').html('<img src="/images/no_data.png" style="display: block;margin: 70px auto;">')
                    }
                    // 公共功能
                    wbpublicFunction(1)
                })
                //查询
                $('.but_query').eq(1).off('click').on('click', function () {
                    // 获取账号
                    var account = $('.state').eq(1).find('li:eq(0) input').val();
                    // 获取名称
                    var accountname = $('.state').eq(1).find('li:eq(1) input').val();
                    // 获取来源
                    var origin = $('.state').eq(1).find('li:eq(2) select option:selected').attr('name');
                    // 获取到期时间
                    var expiredate = $('.state').eq(1).find('li:eq(3) select option:selected').attr('name');
                    setAjax({
                          url: seleceurl,
                        type: "GET",
                        data: {
                            'pageIndex': 1,
                            'pageSize': 20,
                            'BusinessType': 14003,
                            'Status': 15002,
                            'Number': account,
                            'Name': accountname,
                            'EndTime': expiredate,
                            'Source': origin
                        }
                        //"pageIndex=1&pageSize=20&BusinessType=14003&Status=15002&Number=" + account + "&Name=" + accountname + "&EndTime=" + expiredate + "&Source=" + origin
                    }, function (data) {

                        if (data.Result.List != null) {
                            // 后台返回数据处理
                            for (var i = 0; i < data.Result.List.length; i++) {
                                if (data.Result.List[i].BeginTime) {
                                    data.Result.List[i].BeginTime = data.Result.List[i].BeginTime.split(" ")[0];
                                }
                                if (data.Result.List[i].EndTime) {
                                    data.Result.List[i].EndTime = data.Result.List[i].EndTime.split(" ")[0];
                                }
                                if (data.Result.List[i].FistName == null) {
                                    data.Result.List[i].FistName = [];
                                }
                                if (data.Result.List[i].SecondName == null) {
                                    data.Result.List[i].SecondName = [];
                                }
                                if (data.Result.List[i].ThridName == null) {
                                    data.Result.List[i].ThridName = [];
                                }

                            }
                        }
                        $('tbody').eq(1).html(ejs.render($('#weibo2').html(), data));
                        if (data.Result.TotleCount != 0) {
                            var counts = data.Result.TotleCount;
                            //分页
                            $("#pageContainer2").pagination(
                                counts,
                                {
                                    items_per_page: 20, //每页显示多少条记录（默认为20条）
                                    callback: function (currPage, jg) {
                                        // ajax请求
                                        setAjax({
                                              url: seleceurl,
                                            type: "GET",
                                            data: {
                                                'pageIndex': currPage,
                                                'pageSize': 20,
                                                'BusinessType': 14003,
                                                'Status': 15002,
                                                'Number': account,
                                                'Name': accountname,
                                                'EndTime': expiredate,
                                                'Source': origin
                                            }
                                            //"pageIndex=" + currPage + "&pageSize=20&BusinessType=14003&Status=15002pageIndex=1&pageSize=20&BusinessType=14004&Number=" + account + "&Name=" + accountname + "&EndTime=" + expiredate + "&Source=" + origin
                                        }, function (data) {

                                            if (data.Result.List != null) {
                                                // 后台返回数据处理
                                                for (var i = 0; i < data.Result.List.length; i++) {
                                                    if (data.Result.List[i].BeginTime) {
                                                        data.Result.List[i].BeginTime = data.Result.List[i].BeginTime.split(" ")[0];
                                                    }
                                                    if (data.Result.List[i].EndTime) {
                                                        data.Result.List[i].EndTime = data.Result.List[i].EndTime.split(" ")[0];
                                                    }
                                                    if (data.Result.List[i].FistName == null) {
                                                        data.Result.List[i].FistName = [];
                                                    }
                                                    if (data.Result.List[i].SecondName == null) {
                                                        data.Result.List[i].SecondName = [];
                                                    }
                                                    if (data.Result.List[i].ThridName == null) {
                                                        data.Result.List[i].ThridName = [];
                                                    }

                                                }
                                            }
                                            $('tbody').eq(1).html(ejs.render($('#weibo2').html(), data));
                                            // 公共功能
                                            wbpublicFunction(1)

                                        })
                                    }
                                });

                        } else {
                            $('#pageContainer2').html('<img src="/images/no_data.png" style="display: block;margin: 70px auto;">')
                        }
                        // 公共功能
                        wbpublicFunction(1)
                    })
                })
            })
            // 通过
            $('.tab_menu li').eq(2).on('click', function () {
                setAjax({
                      url: seleceurl,
                    type: "GET",
                    data: {'pageIndex': 1, 'pageSize': 20, 'BusinessType': 14003, 'Status': 15003}
                    //"pageIndex=1&pageSize=20&BusinessType=14003&Status=15003"
                }, function (data) {

                    if (data.Result.List != null) {
                        // 后台返回数据处理
                        for (var i = 0; i < data.Result.List.length; i++) {
                            if (data.Result.List[i].BeginTime) {
                                data.Result.List[i].BeginTime = data.Result.List[i].BeginTime.split(" ")[0];
                            }
                            if (data.Result.List[i].EndTime) {
                                data.Result.List[i].EndTime = data.Result.List[i].EndTime.split(" ")[0];
                            }
                            if (data.Result.List[i].FistName == null) {
                                data.Result.List[i].FistName = [];
                            }
                            if (data.Result.List[i].SecondName == null) {
                                data.Result.List[i].SecondName = [];
                            }
                            if (data.Result.List[i].ThridName == null) {
                                data.Result.List[i].ThridName = [];
                            }

                        }
                    }
                    $('tbody').eq(2).html(ejs.render($('#weibo3').html(), data));
                    if (data.Result.TotleCount != 0) {
                        var counts = data.Result.TotleCount;
                        //分页
                        $("#pageContainer3").pagination(
                            counts,
                            {
                                items_per_page: 20, //每页显示多少条记录（默认为20条）
                                callback: function (currPage, jg) {
                                    // ajax请求
                                    setAjax({
                                          url: seleceurl,
                                        type: "GET",
                                        data: {
                                            'pageIndex': currPage,
                                            'pageSize': 20,
                                            'BusinessType': 14003,
                                            'Status': 15003
                                        }
                                        //"pageIndex=" + currPage + "&pageSize=20&BusinessType=14003&Status=15003"
                                    }, function (data) {

                                        if (data.Result.List != null) {
                                            // 后台返回数据处理
                                            for (var i = 0; i < data.Result.List.length; i++) {
                                                if (data.Result.List[i].BeginTime) {
                                                    data.Result.List[i].BeginTime = data.Result.List[i].BeginTime.split(" ")[0];
                                                }
                                                if (data.Result.List[i].EndTime) {
                                                    data.Result.List[i].EndTime = data.Result.List[i].EndTime.split(" ")[0];
                                                }
                                                if (data.Result.List[i].FistName == null) {
                                                    data.Result.List[i].FistName = [];
                                                }
                                                if (data.Result.List[i].SecondName == null) {
                                                    data.Result.List[i].SecondName = [];
                                                }
                                                if (data.Result.List[i].ThridName == null) {
                                                    data.Result.List[i].ThridName = [];
                                                }

                                            }
                                        }
                                        $('tbody').eq(2).html(ejs.render($('#weibo3').html(), data));
                                        // 公共功能
                                        wbpublicFunction(2)

                                    })
                                }
                            });

                    } else {
                        $('#pageContainer3').html('<img src="/images/no_data.png" style="display: block;margin: 70px auto;">')
                    }
                    // 公共功能
                    wbpublicFunction(2)
                })
                //查询
                $('.but_query').eq(2).off('click').on('click', function () {
                    // 获取账号
                    var account = $('.state').eq(2).find('li:eq(0) input').val();
                    // 获取名称
                    var accountname = $('.state').eq(2).find('li:eq(1) input').val();
                    // 获取来源
                    var origin = $('.state').eq(2).find('li:eq(2) select option:selected').attr('name');
                    // 获取到期时间
                    var expiredate = $('.state').eq(2).find('li:eq(3) select option:selected').attr('name');
                    setAjax({
                          url: seleceurl,
                        type: "GET",
                        data: {
                            'pageIndex': 1,
                            'pageSize': 20,
                            'BusinessType': 14003,
                            'Status': 15003,
                            'Number': account,
                            'Name': accountname,
                            'EndTime': expiredate,
                            'Source': origin
                        }
                        //"pageIndex=1&pageSize=20&BusinessType=14003&Status=15003&Number=" + account + "&Name=" + accountname + "&EndTime=" + expiredate + "&Source=" + origin,
                    }, function (data) {

                        if (data.Result.List != null) {
                            // 后台返回数据处理
                            for (var i = 0; i < data.Result.List.length; i++) {
                                if (data.Result.List[i].BeginTime) {
                                    data.Result.List[i].BeginTime = data.Result.List[i].BeginTime.split(" ")[0];
                                }
                                if (data.Result.List[i].EndTime) {
                                    data.Result.List[i].EndTime = data.Result.List[i].EndTime.split(" ")[0];
                                }
                                if (data.Result.List[i].FistName == null) {
                                    data.Result.List[i].FistName = [];
                                }
                                if (data.Result.List[i].SecondName == null) {
                                    data.Result.List[i].SecondName = [];
                                }
                                if (data.Result.List[i].ThridName == null) {
                                    data.Result.List[i].ThridName = [];
                                }

                            }
                        }
                        $('tbody').eq(2).html(ejs.render($('#weibo3').html(), data));
                        if (data.Result.TotleCount != 0) {
                            var counts = data.Result.TotleCount;
                            //分页
                            $("#pageContainer3").pagination(
                                counts,
                                {
                                    items_per_page: 20, //每页显示多少条记录（默认为20条）
                                    callback: function (currPage, jg) {
                                        // ajax请求
                                        setAjax({
                                              url: seleceurl,
                                            type: "GET",
                                            data: {
                                                'pageIndex': currPage,
                                                'pageSize': 20,
                                                'BusinessType': 14003,
                                                'Status': 15003,
                                                'Number': account,
                                                'Name': accountname,
                                                'EndTime': expiredate,
                                                'Source': origin
                                            }
                                            //"pageIndex=" + currPage + "&pageSize=20&BusinessType=14003&Status=15003pageIndex=1&pageSize=20&BusinessType=14004&Number=" + account + "&Name=" + accountname + "&EndTime=" + expiredate + "&Source=" + origin
                                        }, function (data) {

                                            if (data.Result.List != null) {
                                                // 后台返回数据处理
                                                for (var i = 0; i < data.Result.List.length; i++) {
                                                    if (data.Result.List[i].BeginTime) {
                                                        data.Result.List[i].BeginTime = data.Result.List[i].BeginTime.split(" ")[0];
                                                    }
                                                    if (data.Result.List[i].EndTime) {
                                                        data.Result.List[i].EndTime = data.Result.List[i].EndTime.split(" ")[0];
                                                    }
                                                    if (data.Result.List[i].FistName == null) {
                                                        data.Result.List[i].FistName = [];
                                                    }
                                                    if (data.Result.List[i].SecondName == null) {
                                                        data.Result.List[i].SecondName = [];
                                                    }
                                                    if (data.Result.List[i].ThridName == null) {
                                                        data.Result.List[i].ThridName = [];
                                                    }

                                                }
                                            }
                                            $('tbody').eq(2).html(ejs.render($('#weibo3').html(), data));
                                            // 公共功能
                                            wbpublicFunction(2)

                                        })
                                    }
                                });

                        } else {
                            $('#pageContainer3').html('<img src="/images/no_data.png" style="display: block;margin: 70px auto;">')
                        }
                        // 公共功能
                        wbpublicFunction(2)
                    })
                })
            })
            // 驳回
            $('.tab_menu li').eq(3).on('click', function () {
                setAjax({
                      url: seleceurl,
                    type: "GET",
                    data: {'pageIndex': 1, 'pageSize': 20, 'BusinessType': 14003, 'Status': 15004}
                    //"pageIndex=1&pageSize=20&BusinessType=14003&Status=15004"
                }, function (data) {

                    if (data.Result.List != null) {
                        // 后台返回数据处理
                        for (var i = 0; i < data.Result.List.length; i++) {
                            if (data.Result.List[i].BeginTime) {
                                data.Result.List[i].BeginTime = data.Result.List[i].BeginTime.split(" ")[0];
                            }
                            if (data.Result.List[i].EndTime) {
                                data.Result.List[i].EndTime = data.Result.List[i].EndTime.split(" ")[0];
                            }
                            if (data.Result.List[i].FistName == null) {
                                data.Result.List[i].FistName = [];
                            }
                            if (data.Result.List[i].SecondName == null) {
                                data.Result.List[i].SecondName = [];
                            }
                            if (data.Result.List[i].ThridName == null) {
                                data.Result.List[i].ThridName = [];
                            }

                        }
                    }
                    $('tbody').eq(3).html(ejs.render($('#weibo4').html(), data));
                    if (data.Result.TotleCount != 0) {
                        var counts = data.Result.TotleCount;
                        //分页
                        $("#pageContainer4").pagination(
                            counts,
                            {
                                items_per_page: 20, //每页显示多少条记录（默认为20条）
                                callback: function (currPage, jg) {
                                    // ajax请求
                                    setAjax({
                                          url: seleceurl,
                                        type: "GET",
                                        data: {
                                            'pageIndex': currPage,
                                            'pageSize': 20,
                                            'BusinessType': 14003,
                                            'Status': 15004
                                        }
                                        //"pageIndex=" + currPage + "&pageSize=20&BusinessType=14003&Status=15004"
                                    }, function (data) {

                                        if (data.Result.List != null) {
                                            // 后台返回数据处理
                                            for (var i = 0; i < data.Result.List.length; i++) {
                                                if (data.Result.List[i].BeginTime) {
                                                    data.Result.List[i].BeginTime = data.Result.List[i].BeginTime.split(" ")[0];
                                                }
                                                if (data.Result.List[i].EndTime) {
                                                    data.Result.List[i].EndTime = data.Result.List[i].EndTime.split(" ")[0];
                                                }
                                                if (data.Result.List[i].FistName == null) {
                                                    data.Result.List[i].FistName = [];
                                                }
                                                if (data.Result.List[i].SecondName == null) {
                                                    data.Result.List[i].SecondName = [];
                                                }
                                                if (data.Result.List[i].ThridName == null) {
                                                    data.Result.List[i].ThridName = [];
                                                }

                                            }
                                        }
                                        $('tbody').eq(3).html(ejs.render($('#weibo4').html(), data));
                                        // 公共功能
                                        wbpublicFunction(3)

                                    })
                                }
                            });

                    } else {
                        $('#pageContainer4').html('<img src="/images/no_data.png" style="display: block;margin: 70px auto;">')
                    }
                    // 公共功能
                    wbpublicFunction(3)
                })
                //查询
                $('.but_query').eq(3).off('click').on('click', function () {
                    // 获取账号
                    var account = $('.state').eq(3).find('li:eq(0) input').val();
                    // 获取名称
                    var accountname = $('.state').eq(3).find('li:eq(1) input').val();
                    // 获取来源
                    var origin = $('.state').eq(3).find('li:eq(2) select option:selected').attr('name');
                    // 获取到期时间
                    var expiredate = $('.state').eq(3).find('li:eq(3) select option:selected').attr('name');
                    setAjax({
                          url: seleceurl,
                        type: "GET",
                        data: {
                            'pageIndex': 1,
                            'pageSize': 20,
                            'BusinessType': 14003,
                            'Status': 15004,
                            'Number': account,
                            'Name': accountname,
                            'EndTime': expiredate,
                            'Source': origin
                        }
                        //"pageIndex=1&pageSize=20&BusinessType=14003&Status=15004&Number=" + account + "&Name=" + accountname + "&EndTime=" + expiredate + "&Source=" + origin,
                    }, function (data) {

                        if (data.Result.List != null) {
                            // 后台返回数据处理
                            for (var i = 0; i < data.Result.List.length; i++) {
                                if (data.Result.List[i].BeginTime) {
                                    data.Result.List[i].BeginTime = data.Result.List[i].BeginTime.split(" ")[0];
                                }
                                if (data.Result.List[i].EndTime) {
                                    data.Result.List[i].EndTime = data.Result.List[i].EndTime.split(" ")[0];
                                }
                                if (data.Result.List[i].FistName == null) {
                                    data.Result.List[i].FistName = [];
                                }
                                if (data.Result.List[i].SecondName == null) {
                                    data.Result.List[i].SecondName = [];
                                }
                                if (data.Result.List[i].ThridName == null) {
                                    data.Result.List[i].ThridName = [];
                                }

                            }
                        }
                        $('tbody').eq(3).html(ejs.render($('#weibo4').html(), data));
                        if (data.Result.TotleCount != 0) {
                            var counts = data.Result.TotleCount;
                            //分页
                            $("#pageContainer4").pagination(
                                counts,
                                {
                                    items_per_page: 20, //每页显示多少条记录（默认为20条）
                                    callback: function (currPage, jg) {
                                        // ajax请求
                                        setAjax({
                                              url: seleceurl,
                                            type: "GET",
                                            data: {
                                                'pageIndex': currPage,
                                                'pageSize': 20,
                                                'BusinessType': 14003,
                                                'Status': 15004,
                                                'Number': account,
                                                'Name': accountname,
                                                'EndTime': expiredate,
                                                'Source': origin
                                            }
                                            //"pageIndex=" + currPage + "&pageSize=20&BusinessType=14003&Status=15004pageIndex=1&pageSize=20&BusinessType=14004&Number=" + account + "&Name=" + accountname + "&EndTime=" + expiredate + "&Source=" + origin
                                        }, function (data) {

                                            if (data.Result.List != null) {
                                                // 后台返回数据处理
                                                for (var i = 0; i < data.Result.List.length; i++) {
                                                    if (data.Result.List[i].BeginTime) {
                                                        data.Result.List[i].BeginTime = data.Result.List[i].BeginTime.split(" ")[0];
                                                    }
                                                    if (data.Result.List[i].EndTime) {
                                                        data.Result.List[i].EndTime = data.Result.List[i].EndTime.split(" ")[0];
                                                    }
                                                    if (data.Result.List[i].FistName == null) {
                                                        data.Result.List[i].FistName = [];
                                                    }
                                                    if (data.Result.List[i].SecondName == null) {
                                                        data.Result.List[i].SecondName = [];
                                                    }
                                                    if (data.Result.List[i].ThridName == null) {
                                                        data.Result.List[i].ThridName = [];
                                                    }

                                                }
                                            }
                                            $('tbody').eq(3).html(ejs.render($('#weibo4').html(), data));
                                            // 公共功能
                                            wbpublicFunction(3)

                                        })
                                    }
                                });

                        } else {
                            $('#pageContainer4').html('<img src="/images/no_data.png" style="display: block;margin: 70px auto;">')
                        }
                        // 公共功能
                        wbpublicFunction(3)
                    })
                })
            })
        }
        //视频刊例列表
        if ($('div[name="operate-video"]').attr('name') == 'operate-video') {
            $('div[name="operate-video"]').css({display: 'block'})
            function sppublicFunction(i) {
                judgeAuthority(3)
                // 点击申请上架
                $('tbody').find("tr").find('td:last a[name^=15002]').off('click').on('click', function () {

                    var data = $(this).attr('name').split(',')
                    if(Date.parse(new Date().format("yyyy/MM/dd")) - Date.parse($(this).parent().attr('name').split(' ')[0].split('-').join('/')) < 0 ) {
                        $.openPopupLayer({
                            name: "shipingclickApply",
                            url: "pricelist-application.html",
                            error: function (dd) {
                                alert(dd.status);
                            },
                            success: function () {
                                // 点击确定
                                $('.layer_con').find('.button').off('click').on('click', function () {
                                    setAjax({
                                        url: Ajaxurl,
                                        type: "POST",
                                        data: {
                                            "PubID": data[1],
                                            "Status": data[0]
                                        }
                                    }, function (data) {
                                        $.closePopupLayer('shipingclickApply');
                                        if (data.Result) {
                                            window.location.reload();
                                        }
                                    })
                                })                 // 点击取消
                                    .end().find('.but_keep').off('click').on('click', function () {
                                    $.closePopupLayer('shipingclickApply')
                                })

                            }
                        });
                    }else {
                        alert('请调整刊例时间！')
                    }
                })
                // 点击驳回原因
                $('tbody').eq(i).find('tr').find('td[name=15004]').find('a').off('click').on('click', function () {
                    // 驳回原因的内容
                    var data = $(this).next().text();
                    $.openPopupLayer({
                        name: "Reject",
                        url: "pricelist-reason.html",
                        error: function (dd) {
                            alert(dd.status);
                        },
                        success: function () {
                            $('.mb25').html(data)
                            //点击取消
                            $('.button').off('click').on('click', function () {
                                $.closePopupLayer('Reject');
                            });
                            $('.fr>a').off('click').on('click', function () {
                                $.closePopupLayer('Reject');
                            });
                        }
                    });
                })
                // 上下架切换
                $('tbody tr').find('td:last span a').off('click').on('click', function (e) {
                    if(e.target.innerHTML=='编辑'){
                        return
                    }
                    var $this = $(this);
                    var data1 = $this.attr('name').split(',');
                    if($this.text()=='下架'){
                        setAjax({
                            url: Ajaxurl,
                            type: "POST",
                            data: {
                                "PubID": data1[1],
                                "Status": data1[0]
                            }
                        }, function (data) {
                            if (data.Result) {
                                // window.location.reload();
                                if (data1[0] == 15005) {
                                    $this.attr('name', '15006,' + data1[1]).text('下架');
                                    $this.attr('name', '15006,' + data1[1]).next().css('display','none')
                                } else {
                                    $this.attr('name', '15005,' + data1[1]).text('上架');
                                    $this.attr('name', '15005,' + data1[1]).next().css('display','inline-block')
                                }

                            }
                        })
                        return
                    }
                    if (Date.parse(new Date().format("yyyy/MM/dd")) - Date.parse($(this).parent().parent().attr('name').split(' ')[0].split('-').join('/')) < 0 ) {
                        setAjax({
                            url: Ajaxurl,
                            type: "POST",
                            data: {
                                "PubID": data1[1],
                                "Status": data1[0]
                            }
                        }, function (data) {
                            if (data.Result) {
                                // window.location.reload();
                                if (data1[0] == 15005) {
                                    $this.attr('name', '15006,' + data1[1]).text('下架')
                                    $this.attr('name', '15006,' + data1[1]).next().css('display','none')
                                } else {
                                    $this.attr('name', '15005,' + data1[1]).text('上架')
                                    $this.attr('name', '15005,' + data1[1]).next().css('display','inline-block')
                                }

                            }
                        })
                    } else {
                        alert('请调整刊例时间！')
                    }
                })
            }

            // 平台
            setAjax({

                url: "/api/DictInfo/GetDictInfoByTypeID",
                type: "GET",
                data: "typeID=26"
            }, function (data) {
                $('.platform3').html(ejs.render($('#platform1').html(), data));
            })
            // 全部
            $('.tab_menu li').eq(0).on('click', function () {
                setAjax({
                      url: seleceurl,
                    type: "GET",
                    data: {'pageIndex': 1, 'pageSize': 20, 'BusinessType': 14004}
                    //"pageIndex=1&pageSize=20&BusinessType=14004"
                }, function (data) {
                    if (data.Result.List != null) {
                        // 后台返回数据处理
                        for (var i = 0; i < data.Result.List.length; i++) {
                            if (data.Result.List[i].BeginTime) {
                                data.Result.List[i].BeginTime = data.Result.List[i].BeginTime.split(" ")[0];
                            }
                            if (data.Result.List[i].EndTime) {
                                data.Result.List[i].EndTime = data.Result.List[i].EndTime.split(" ")[0];
                            }
                            if (data.Result.List[i].FistName) {
                                data.Result.List[i].FistName = data.Result.List[i].FistName.split("#");
                            } else {
                                data.Result.List[i].FistName = []
                            }
                            if (data.Result.List[i].SecondName) {
                                data.Result.List[i].SecondName = data.Result.List[i].SecondName.split("#");
                            } else {
                                data.Result.List[i].SecondName = []
                            }
                            if (data.Result.List[i].ThridName) {
                                data.Result.List[i].ThridName = data.Result.List[i].ThridName.split("#");
                            } else {
                                data.Result.List[i].ThridName = []
                            }
                            if (data.Result.List[i].FourthName) {
                                data.Result.List[i].FourthName = data.Result.List[i].FourthName.split("#");
                            } else {
                                data.Result.List[i].FourthName = []
                            }

                        }
                    }
                    $('tbody').eq(0).html(ejs.render($('#shipin1').html(), data));
                    if (data.Result.TotleCount != 0) {
                        var counts = data.Result.TotleCount;
                        //分页
                        $("#pageContainer1").pagination(
                            counts,
                            {
                                items_per_page: 20, //每页显示多少条记录（默认为20条）
                                callback: function (currPage, jg) {
                                    // ajax请求
                                    setAjax({
                                          url: seleceurl,
                                        type: "GET",
                                        data: {'pageIndex': currPage, 'pageSize': 20, 'BusinessType': 14004}
                                        //"pageIndex=" + currPage + "&pageSize=20&BusinessType=14004"
                                    }, function (data) {
                                        if (data.Result.List != null) {
                                            // 后台返回数据处理
                                            for (var i = 0; i < data.Result.List.length; i++) {
                                                if (data.Result.List[i].BeginTime) {
                                                    data.Result.List[i].BeginTime = data.Result.List[i].BeginTime.split(" ")[0];
                                                }
                                                if (data.Result.List[i].EndTime) {
                                                    data.Result.List[i].EndTime = data.Result.List[i].EndTime.split(" ")[0];
                                                }

                                            }
                                        }
                                        $('tbody').eq(0).html(ejs.render($('#shipin1').html(), data));
                                        // 公共功能
                                        sppublicFunction(0)

                                    })
                                }
                            });

                    } else {
                        $('#pageContainer1').html('<img src="/images/no_data.png" style="display: block;margin: 70px auto;">')
                    }
                    // 公共功能
                    sppublicFunction(0)
                })
                //查询
                $('.but_query').eq(0).off('click').on('click', function () {
                    // 获取平台
                    var weixin = $('.state').eq(0).find('li:eq(0) select option:selected').attr('name');
                    // 获取账号
                    var account = $('.state').eq(0).find('li:eq(1) input').val();
                    // 获取名称
                    var accountname = $('.state').eq(0).find('li:eq(2) input').val();
                    // 获取来源
                    var origin = $('.state').eq(0).find('li:eq(3) select option:selected').attr('name');
                    // 获取到期时间
                    var expiredate = $('.state').eq(0).find('li:eq(4) select option:selected').attr('name');
                    setAjax({
                          url: seleceurl,
                        type: "GET",
                        data: {
                            'pageIndex': 1,
                            'pageSize': 20,
                            'BusinessType': 14004,
                            'Number': account,
                            'Name': accountname,
                            'EndTime': expiredate,
                            'Platform': weixin,
                            'Source': origin
                        }
                        //"pageIndex=1&pageSize=20&BusinessType=14004&Number=" + account + "&Name=" + accountname + "&EndTime=" + expiredate + "&Platform=" + weixin + "&Source=" + origin,
                    }, function (data) {

                        if (data.Result.List != null) {
                            // 后台返回数据处理
                            for (var i = 0; i < data.Result.List.length; i++) {
                                if (data.Result.List[i].BeginTime) {
                                    data.Result.List[i].BeginTime = data.Result.List[i].BeginTime.split(" ")[0];
                                }
                                if (data.Result.List[i].EndTime) {
                                    data.Result.List[i].EndTime = data.Result.List[i].EndTime.split(" ")[0];
                                }
                                if (data.Result.List[i].FistName) {
                                    data.Result.List[i].FistName = data.Result.List[i].FistName.split("#");
                                } else {
                                    data.Result.List[i].FistName = []
                                }
                                if (data.Result.List[i].SecondName) {
                                    data.Result.List[i].SecondName = data.Result.List[i].SecondName.split("#");
                                } else {
                                    data.Result.List[i].SecondName = []
                                }
                                if (data.Result.List[i].ThridName) {
                                    data.Result.List[i].ThridName = data.Result.List[i].ThridName.split("#");
                                } else {
                                    data.Result.List[i].ThridName = []
                                }
                                if (data.Result.List[i].FourthName) {
                                    data.Result.List[i].FourthName = data.Result.List[i].FourthName.split("#");
                                } else {
                                    data.Result.List[i].FourthName = []
                                }

                            }
                        }
                        $('tbody').eq(0).html(ejs.render($('#shipin1').html(), data));
                        if (data.Result.TotleCount != 0) {
                            var counts = data.Result.TotleCount;
                            //分页
                            $("#pageContainer1").pagination(
                                counts,
                                {
                                    items_per_page: 20, //每页显示多少条记录（默认为20条）
                                    callback: function (currPage, jg) {
                                        // ajax请求
                                        setAjax({
                                              url: seleceurl,
                                            type: "GET",
                                            data: {
                                                'pageIndex': currPage,
                                                'pageSize': 20,
                                                'BusinessType': 14004,
                                                'Number': account,
                                                'Name': accountname,
                                                'EndTime': expiredate,
                                                'Platform': weixin,
                                                'Source': origin
                                            }
                                            //"pageIndex=" + currPage + "&pageSize=20&BusinessType=14004&Number=" + account + "&Name=" + accountname + "&EndTime=" + expiredate + "&Platform=" + weixin + "&Source=" + origin,
                                        }, function (data) {

                                            if (data.Result.List != null) {
                                                // 后台返回数据处理
                                                for (var i = 0; i < data.Result.List.length; i++) {
                                                    if (data.Result.List[i].BeginTime) {
                                                        data.Result.List[i].BeginTime = data.Result.List[i].BeginTime.split(" ")[0];
                                                    }
                                                    if (data.Result.List[i].EndTime) {
                                                        data.Result.List[i].EndTime = data.Result.List[i].EndTime.split(" ")[0];
                                                    }
                                                    if (data.Result.List[i].FistName) {
                                                        data.Result.List[i].FistName = data.Result.List[i].FistName.split("#");
                                                    } else {
                                                        data.Result.List[i].FistName = []
                                                    }
                                                    if (data.Result.List[i].SecondName) {
                                                        data.Result.List[i].SecondName = data.Result.List[i].SecondName.split("#");
                                                    } else {
                                                        data.Result.List[i].SecondName = []
                                                    }
                                                    if (data.Result.List[i].ThridName) {
                                                        data.Result.List[i].ThridName = data.Result.List[i].ThridName.split("#");
                                                    } else {
                                                        data.Result.List[i].ThridName = []
                                                    }
                                                    if (data.Result.List[i].FourthName) {
                                                        data.Result.List[i].FourthName = data.Result.List[i].FourthName.split("#");
                                                    } else {
                                                        data.Result.List[i].FourthName = []
                                                    }

                                                }
                                            }
                                            $('tbody').eq(0).html(ejs.render($('#shipin1').html(), data));
                                            // 公共功能
                                            sppublicFunction(0)

                                        })
                                    }
                                });

                        } else {
                            $('#pageContainer1').html('<img src="/images/no_data.png" style="display: block;margin: 70px auto;">')
                        }

                        // 公共功能
                        sppublicFunction(0)
                    })
                })
            })
            $('.tab_menu li').eq(0).click()
            // 待审核
            $('.tab_menu li').eq(1).on('click', function () {
                setAjax({
                      url: seleceurl,
                    type: "GET",
                    data: {'pageIndex': 1, 'pageSize': 20, 'BusinessType': 14004, 'Status': 15002}
                    //"pageIndex=1&pageSize=20&BusinessType=14004&Status=15002"
                }, function (data) {

                    if (data.Result.List != null) {
                        // 后台返回数据处理
                        for (var i = 0; i < data.Result.List.length; i++) {
                            if (data.Result.List[i].BeginTime) {
                                data.Result.List[i].BeginTime = data.Result.List[i].BeginTime.split(" ")[0];
                            }
                            if (data.Result.List[i].EndTime) {
                                data.Result.List[i].EndTime = data.Result.List[i].EndTime.split(" ")[0];
                            }
                            if (data.Result.List[i].FistName) {
                                data.Result.List[i].FistName = data.Result.List[i].FistName.split("#");
                            } else {
                                data.Result.List[i].FistName = []
                            }
                            if (data.Result.List[i].SecondName) {
                                data.Result.List[i].SecondName = data.Result.List[i].SecondName.split("#");
                            } else {
                                data.Result.List[i].SecondName = []
                            }
                            if (data.Result.List[i].ThridName) {
                                data.Result.List[i].ThridName = data.Result.List[i].ThridName.split("#");
                            } else {
                                data.Result.List[i].ThridName = []
                            }
                            if (data.Result.List[i].FourthName) {
                                data.Result.List[i].FourthName = data.Result.List[i].FourthName.split("#");
                            } else {
                                data.Result.List[i].FourthName = []
                            }

                        }
                    }
                    $('tbody').eq(1).html(ejs.render($('#shipin2').html(), data));
                    if (data.Result.TotleCount != 0) {
                        var counts = data.Result.TotleCount;
                        //分页
                        $("#pageContainer2").pagination(
                            counts,
                            {
                                items_per_page: 20, //每页显示多少条记录（默认为20条）
                                callback: function (currPage, jg) {
                                    // ajax请求
                                    setAjax({
                                          url: seleceurl,
                                        type: "GET",
                                        data: {
                                            'pageIndex': currPage,
                                            'pageSize': 20,
                                            'BusinessType': 14004,
                                            'Status': 15002
                                        }
                                        //"pageIndex=" + currPage + "&pageSize=20&BusinessType=14004&Status=15002"
                                    }, function (data) {

                                        if (data.Result.List != null) {
                                            // 后台返回数据处理
                                            for (var i = 0; i < data.Result.List.length; i++) {
                                                if (data.Result.List[i].BeginTime) {
                                                    data.Result.List[i].BeginTime = data.Result.List[i].BeginTime.split(" ")[0];
                                                }
                                                if (data.Result.List[i].EndTime) {
                                                    data.Result.List[i].EndTime = data.Result.List[i].EndTime.split(" ")[0];
                                                }
                                                if (data.Result.List[i].FistName) {
                                                    data.Result.List[i].FistName = data.Result.List[i].FistName.split("#");
                                                } else {
                                                    data.Result.List[i].FistName = []
                                                }
                                                if (data.Result.List[i].SecondName) {
                                                    data.Result.List[i].SecondName = data.Result.List[i].SecondName.split("#");
                                                } else {
                                                    data.Result.List[i].SecondName = []
                                                }
                                                if (data.Result.List[i].ThridName) {
                                                    data.Result.List[i].ThridName = data.Result.List[i].ThridName.split("#");
                                                } else {
                                                    data.Result.List[i].ThridName = []
                                                }
                                                if (data.Result.List[i].FourthName) {
                                                    data.Result.List[i].FourthName = data.Result.List[i].FourthName.split("#");
                                                } else {
                                                    data.Result.List[i].FourthName = []
                                                }

                                            }
                                        }
                                        $('tbody').eq(1).html(ejs.render($('#shipin2').html(), data));
                                        // 公共功能
                                        sppublicFunction(1)

                                    })
                                }
                            });

                    } else {
                        $('#pageContainer2').html('<img src="/images/no_data.png" style="display: block;margin: 70px auto;">')
                    }
                    // 公共功能
                    sppublicFunction(1)
                })
                //查询
                $('.but_query').eq(1).off('click').on('click', function () {
                    // 获取平台
                    var weixin = $('.state').eq(1).find('li:eq(0) select option:selected').attr('name');
                    // 获取账号
                    var account = $('.state').eq(1).find('li:eq(1) input').val();
                    // 获取名称
                    var accountname = $('.state').eq(1).find('li:eq(2) input').val();
                    // 获取来源
                    var origin = $('.state').eq(1).find('li:eq(3) select option:selected').attr('name');
                    // 获取到期时间
                    var expiredate = $('.state').eq(1).find('li:eq(4) select option:selected').attr('name');
                    setAjax({
                          url: seleceurl,
                        type: "GET",
                        data: {
                            'pageIndex': 1,
                            'pageSize': 20,
                            'BusinessType': 14004,
                            'Status': 15002,
                            'Number': account,
                            'Name': accountname,
                            'EndTime': expiredate,
                            'Platform': weixin,
                            'Source': origin
                        }
                        //"pageIndex=1&pageSize=20&BusinessType=14004&Status=15002&Number=" + account + "&Name=" + accountname + "&EndTime=" + expiredate + "&Platform=" + weixin + "&Source=" + origin,
                    }, function (data) {

                        if (data.Result.List != null) {
                            // 后台返回数据处理
                            for (var i = 0; i < data.Result.List.length; i++) {
                                if (data.Result.List[i].BeginTime) {
                                    data.Result.List[i].BeginTime = data.Result.List[i].BeginTime.split(" ")[0];
                                }
                                if (data.Result.List[i].EndTime) {
                                    data.Result.List[i].EndTime = data.Result.List[i].EndTime.split(" ")[0];
                                }
                                if (data.Result.List[i].FistName) {
                                    data.Result.List[i].FistName = data.Result.List[i].FistName.split("#");
                                } else {
                                    data.Result.List[i].FistName = []
                                }
                                if (data.Result.List[i].SecondName) {
                                    data.Result.List[i].SecondName = data.Result.List[i].SecondName.split("#");
                                } else {
                                    data.Result.List[i].SecondName = []
                                }
                                if (data.Result.List[i].ThridName) {
                                    data.Result.List[i].ThridName = data.Result.List[i].ThridName.split("#");
                                } else {
                                    data.Result.List[i].ThridName = []
                                }
                                if (data.Result.List[i].FourthName) {
                                    data.Result.List[i].FourthName = data.Result.List[i].FourthName.split("#");
                                } else {
                                    data.Result.List[i].FourthName = []
                                }

                            }
                        }
                        $('tbody').eq(1).html(ejs.render($('#shipin2').html(), data));
                        if (data.Result.TotleCount != 0) {
                            var counts = data.Result.TotleCount;
                            //分页
                            $("#pageContainer2").pagination(
                                counts,
                                {
                                    items_per_page: 20, //每页显示多少条记录（默认为20条）
                                    callback: function (currPage, jg) {
                                        // ajax请求
                                        setAjax({
                                              url: seleceurl,
                                            type: "GET",
                                            data: {
                                                'pageIndex': currPage,
                                                'pageSize': 20,
                                                'BusinessType': 14004,
                                                'Status': 15002,
                                                'Number': account,
                                                'Name': accountname,
                                                'EndTime': expiredate,
                                                'Platform': weixin,
                                                'Source': origin
                                            }
                                            //"pageIndex=" + currPage + "&pageSize=20&BusinessType=14004&Status=15002&Number=" + account + "&Name=" + accountname + "&EndTime=" + expiredate + "&Platform=" + weixin + "&Source=" + origin
                                        }, function (data) {

                                            if (data.Result.List != null) {
                                                // 后台返回数据处理
                                                for (var i = 0; i < data.Result.List.length; i++) {
                                                    if (data.Result.List[i].BeginTime) {
                                                        data.Result.List[i].BeginTime = data.Result.List[i].BeginTime.split(" ")[0];
                                                    }
                                                    if (data.Result.List[i].EndTime) {
                                                        data.Result.List[i].EndTime = data.Result.List[i].EndTime.split(" ")[0];
                                                    }
                                                    if (data.Result.List[i].FistName) {
                                                        data.Result.List[i].FistName = data.Result.List[i].FistName.split("#");
                                                    } else {
                                                        data.Result.List[i].FistName = []
                                                    }
                                                    if (data.Result.List[i].SecondName) {
                                                        data.Result.List[i].SecondName = data.Result.List[i].SecondName.split("#");
                                                    } else {
                                                        data.Result.List[i].SecondName = []
                                                    }
                                                    if (data.Result.List[i].ThridName) {
                                                        data.Result.List[i].ThridName = data.Result.List[i].ThridName.split("#");
                                                    } else {
                                                        data.Result.List[i].ThridName = []
                                                    }
                                                    if (data.Result.List[i].FourthName) {
                                                        data.Result.List[i].FourthName = data.Result.List[i].FourthName.split("#");
                                                    } else {
                                                        data.Result.List[i].FourthName = []
                                                    }

                                                }
                                            }
                                            $('tbody').eq(1).html(ejs.render($('#shipin2').html(), data));
                                            // 公共功能
                                            sppublicFunction(1)

                                        })
                                    }
                                });

                        } else {
                            $('#pageContainer2').html('<img src="/images/no_data.png" style="display: block;margin: 70px auto;">')
                        }
                        // 公共功能
                        sppublicFunction(1)
                    })
                })
            })
            // 通过
            $('.tab_menu li').eq(2).on('click', function () {
                setAjax({
                      url: seleceurl,
                    type: "GET",
                    data: {'pageIndex': 1, 'pageSize': 20, 'BusinessType': 14004, 'Status': 15003}
                    //"pageIndex=1&pageSize=20&BusinessType=14004&Status=15003"
                }, function (data) {

                    if (data.Result.List != null) {
                        // 后台返回数据处理
                        for (var i = 0; i < data.Result.List.length; i++) {
                            if (data.Result.List[i].BeginTime) {
                                data.Result.List[i].BeginTime = data.Result.List[i].BeginTime.split(" ")[0];
                            }
                            if (data.Result.List[i].EndTime) {
                                data.Result.List[i].EndTime = data.Result.List[i].EndTime.split(" ")[0];
                            }
                            if (data.Result.List[i].FistName) {
                                data.Result.List[i].FistName = data.Result.List[i].FistName.split("#");
                            } else {
                                data.Result.List[i].FistName = []
                            }
                            if (data.Result.List[i].SecondName) {
                                data.Result.List[i].SecondName = data.Result.List[i].SecondName.split("#");
                            } else {
                                data.Result.List[i].SecondName = []
                            }
                            if (data.Result.List[i].ThridName) {
                                data.Result.List[i].ThridName = data.Result.List[i].ThridName.split("#");
                            } else {
                                data.Result.List[i].ThridName = []
                            }
                            if (data.Result.List[i].FourthName) {
                                data.Result.List[i].FourthName = data.Result.List[i].FourthName.split("#");
                            } else {
                                data.Result.List[i].FourthName = []
                            }

                        }
                    }
                    $('tbody').eq(2).html(ejs.render($('#shipin3').html(), data));
                    if (data.Result.TotleCount != 0) {
                        var counts = data.Result.TotleCount;
                        //分页
                        $("#pageContainer3").pagination(
                            counts,
                            {
                                items_per_page: 20, //每页显示多少条记录（默认为20条）
                                callback: function (currPage, jg) {
                                    // ajax请求
                                    setAjax({
                                          url: seleceurl,
                                        type: "GET",
                                        data: {
                                            'pageIndex': currPage,
                                            'pageSize': 20,
                                            'BusinessType': 14004,
                                            'Status': 15003
                                        }
                                        // "pageIndex=" + currPage + "&pageSize=20&BusinessType=14004&Status=15003"
                                    }, function (data) {

                                        if (data.Result.List != null) {
                                            // 后台返回数据处理
                                            for (var i = 0; i < data.Result.List.length; i++) {
                                                if (data.Result.List[i].BeginTime) {
                                                    data.Result.List[i].BeginTime = data.Result.List[i].BeginTime.split(" ")[0];
                                                }
                                                if (data.Result.List[i].EndTime) {
                                                    data.Result.List[i].EndTime = data.Result.List[i].EndTime.split(" ")[0];
                                                }
                                                if (data.Result.List[i].FistName) {
                                                    data.Result.List[i].FistName = data.Result.List[i].FistName.split("#");
                                                } else {
                                                    data.Result.List[i].FistName = []
                                                }
                                                if (data.Result.List[i].SecondName) {
                                                    data.Result.List[i].SecondName = data.Result.List[i].SecondName.split("#");
                                                } else {
                                                    data.Result.List[i].SecondName = []
                                                }
                                                if (data.Result.List[i].ThridName) {
                                                    data.Result.List[i].ThridName = data.Result.List[i].ThridName.split("#");
                                                } else {
                                                    data.Result.List[i].ThridName = []
                                                }
                                                if (data.Result.List[i].FourthName) {
                                                    data.Result.List[i].FourthName = data.Result.List[i].FourthName.split("#");
                                                } else {
                                                    data.Result.List[i].FourthName = []
                                                }

                                            }
                                        }
                                        $('tbody').eq(2).html(ejs.render($('#shipin3').html(), data));
                                        // 公共功能
                                        sppublicFunction(2)

                                    })
                                }
                            });

                    } else {
                        $('#pageContainer3').html('<img src="/images/no_data.png" style="display: block;margin: 70px auto;">')
                    }
                    // 公共功能
                    sppublicFunction(2)
                })
                //查询
                $('.but_query').eq(2).off('click').on('click', function () {
                    // 获取平台
                    var weixin = $('.state').eq(2).find('li:eq(0) select option:selected').attr('name');
                    // 获取账号
                    var account = $('.state').eq(2).find('li:eq(1) input').val();
                    // 获取名称
                    var accountname = $('.state').eq(2).find('li:eq(2) input').val();
                    // 获取来源
                    var origin = $('.state').eq(2).find('li:eq(3) select option:selected').attr('name');
                    // 获取到期时间
                    var expiredate = $('.state').eq(2).find('li:eq(4) select option:selected').attr('name');
                    setAjax({
                          url: seleceurl,
                        type: "GET",
                        data: {
                            'pageIndex': 1,
                            'pageSize': 20,
                            'BusinessType': 14004,
                            'Status': 15003,
                            'Number': account,
                            'Name': accountname,
                            'EndTime': expiredate,
                            'Platform': weixin,
                            'Source': origin
                        }
                        //"pageIndex=1&pageSize=20&BusinessType=14004&Status=15003&Number=" + account + "&Name=" + accountname + "&EndTime=" + expiredate + "&Platform=" + weixin + "&Source=" + origin
                    }, function (data) {

                        if (data.Result.List != null) {
                            // 后台返回数据处理
                            for (var i = 0; i < data.Result.List.length; i++) {
                                if (data.Result.List[i].BeginTime) {
                                    data.Result.List[i].BeginTime = data.Result.List[i].BeginTime.split(" ")[0];
                                }
                                if (data.Result.List[i].EndTime) {
                                    data.Result.List[i].EndTime = data.Result.List[i].EndTime.split(" ")[0];
                                }
                                if (data.Result.List[i].FistName) {
                                    data.Result.List[i].FistName = data.Result.List[i].FistName.split("#");
                                } else {
                                    data.Result.List[i].FistName = []
                                }
                                if (data.Result.List[i].SecondName) {
                                    data.Result.List[i].SecondName = data.Result.List[i].SecondName.split("#");
                                } else {
                                    data.Result.List[i].SecondName = []
                                }
                                if (data.Result.List[i].ThridName) {
                                    data.Result.List[i].ThridName = data.Result.List[i].ThridName.split("#");
                                } else {
                                    data.Result.List[i].ThridName = []
                                }
                                if (data.Result.List[i].FourthName) {
                                    data.Result.List[i].FourthName = data.Result.List[i].FourthName.split("#");
                                } else {
                                    data.Result.List[i].FourthName = []
                                }

                            }
                        }
                        $('tbody').eq(2).html(ejs.render($('#shipin3').html(), data));
                        if (data.Result.TotleCount != 0) {
                            var counts = data.Result.TotleCount;
                            //分页
                            $("#pageContainer3").pagination(
                                counts,
                                {
                                    items_per_page: 20, //每页显示多少条记录（默认为20条）
                                    callback: function (currPage, jg) {
                                        // ajax请求
                                        setAjax({
                                              url: seleceurl,
                                            type: "GET",
                                            data: {
                                                'pageIndex': currPage,
                                                'pageSize': 20,
                                                'BusinessType': 14004,
                                                'Status': 15003,
                                                'Number': account,
                                                'Name': accountname,
                                                'EndTime': expiredate,
                                                'Platform': weixin,
                                                'Source': origin
                                            }
                                            //"pageIndex=" + currPage + "&pageSize=20&BusinessType=14004&Status=15003&Number=" + account + "&Name=" + accountname + "&EndTime=" + expiredate + "&Platform=" + weixin + "&Source=" + origin,
                                        }, function (data) {
                                            if (data.Result.List != null) {
                                                // 后台返回数据处理
                                                for (var i = 0; i < data.Result.List.length; i++) {
                                                    if (data.Result.List[i].BeginTime) {
                                                        data.Result.List[i].BeginTime = data.Result.List[i].BeginTime.split(" ")[0];
                                                    }
                                                    if (data.Result.List[i].EndTime) {
                                                        data.Result.List[i].EndTime = data.Result.List[i].EndTime.split(" ")[0];
                                                    }
                                                    if (data.Result.List[i].FistName) {
                                                        data.Result.List[i].FistName = data.Result.List[i].FistName.split("#");
                                                    } else {
                                                        data.Result.List[i].FistName = []
                                                    }
                                                    if (data.Result.List[i].SecondName) {
                                                        data.Result.List[i].SecondName = data.Result.List[i].SecondName.split("#");
                                                    } else {
                                                        data.Result.List[i].SecondName = []
                                                    }
                                                    if (data.Result.List[i].ThridName) {
                                                        data.Result.List[i].ThridName = data.Result.List[i].ThridName.split("#");
                                                    } else {
                                                        data.Result.List[i].ThridName = []
                                                    }
                                                    if (data.Result.List[i].FourthName) {
                                                        data.Result.List[i].FourthName = data.Result.List[i].FourthName.split("#");
                                                    } else {
                                                        data.Result.List[i].FourthName = []
                                                    }

                                                }
                                            }
                                            $('tbody').eq(2).html(ejs.render($('#shipin3').html(), data));
                                            // 公共功能
                                            sppublicFunction(2)

                                        })
                                    }
                                });

                        } else {
                            $('#pageContainer3').html('<img src="/images/no_data.png" style="display: block;margin: 70px auto;">')
                        }
                        // 公共功能
                        sppublicFunction(2)
                    })
                })
            })
            // 驳回
            $('.tab_menu li').eq(3).on('click', function () {
                setAjax({
                      url: seleceurl,
                    type: "GET",
                    data: {'pageIndex': 1, 'pageSize': 20, 'BusinessType': 14004, 'Status': 15004}
                    //"pageIndex=1&pageSize=20&BusinessType=14004&Status=15004"
                }, function (data) {
                    if (data.Result.List != null) {
                        // 后台返回数据处理
                        for (var i = 0; i < data.Result.List.length; i++) {
                            if (data.Result.List[i].BeginTime) {
                                data.Result.List[i].BeginTime = data.Result.List[i].BeginTime.split(" ")[0];
                            }
                            if (data.Result.List[i].EndTime) {
                                data.Result.List[i].EndTime = data.Result.List[i].EndTime.split(" ")[0];
                            }
                            if (data.Result.List[i].FistName) {
                                data.Result.List[i].FistName = data.Result.List[i].FistName.split("#");
                            } else {
                                data.Result.List[i].FistName = []
                            }
                            if (data.Result.List[i].SecondName) {
                                data.Result.List[i].SecondName = data.Result.List[i].SecondName.split("#");
                            } else {
                                data.Result.List[i].SecondName = []
                            }
                            if (data.Result.List[i].ThridName) {
                                data.Result.List[i].ThridName = data.Result.List[i].ThridName.split("#");
                            } else {
                                data.Result.List[i].ThridName = []
                            }
                            if (data.Result.List[i].FourthName) {
                                data.Result.List[i].FourthName = data.Result.List[i].FourthName.split("#");
                            } else {
                                data.Result.List[i].FourthName = []
                            }

                        }
                    }
                    $('tbody').eq(3).html(ejs.render($('#shipin4').html(), data));
                    if (data.Result.TotleCount != 0) {
                        var counts = data.Result.TotleCount;
                        //分页
                        $("#pageContainer4").pagination(
                            counts,
                            {
                                items_per_page: 20, //每页显示多少条记录（默认为20条）
                                callback: function (currPage, jg) {
                                    // ajax请求
                                    setAjax({
                                          url: seleceurl,
                                        type: "GET",
                                        data: {
                                            'pageIndex': currPage,
                                            'pageSize': 20,
                                            'BusinessType': 14004,
                                            'Status': 15004
                                        }
                                        //"pageIndex=" + currPage + "&pageSize=20&BusinessType=14004&Status=15004"
                                    }, function (data) {

                                        if (data.Result.List != null) {
                                            // 后台返回数据处理
                                            for (var i = 0; i < data.Result.List.length; i++) {
                                                if (data.Result.List[i].BeginTime) {
                                                    data.Result.List[i].BeginTime = data.Result.List[i].BeginTime.split(" ")[0];
                                                }
                                                if (data.Result.List[i].EndTime) {
                                                    data.Result.List[i].EndTime = data.Result.List[i].EndTime.split(" ")[0];
                                                }
                                                if (data.Result.List[i].FistName) {
                                                    data.Result.List[i].FistName = data.Result.List[i].FistName.split("#");
                                                } else {
                                                    data.Result.List[i].FistName = []
                                                }
                                                if (data.Result.List[i].SecondName) {
                                                    data.Result.List[i].SecondName = data.Result.List[i].SecondName.split("#");
                                                } else {
                                                    data.Result.List[i].SecondName = []
                                                }
                                                if (data.Result.List[i].ThridName) {
                                                    data.Result.List[i].ThridName = data.Result.List[i].ThridName.split("#");
                                                } else {
                                                    data.Result.List[i].ThridName = []
                                                }
                                                if (data.Result.List[i].FourthName) {
                                                    data.Result.List[i].FourthName = data.Result.List[i].FourthName.split("#");
                                                } else {
                                                    data.Result.List[i].FourthName = []
                                                }

                                            }
                                        }
                                        $('tbody').eq(3).html(ejs.render($('#shipin4').html(), data));
                                        // 公共功能
                                        sppublicFunction(3)

                                    })
                                }
                            });

                    } else {
                        $('#pageContainer4').html('<img src="/images/no_data.png" style="display: block;margin: 70px auto;">')
                    }
                    // 公共功能
                    sppublicFunction(3)
                })
                //查询
                $('.but_query').eq(3).off('click').on('click', function () {
                    // 获取平台
                    var weixin = $('.state').eq(3).find('li:eq(0) select option:selected').attr('name');
                    // 获取账号
                    var account = $('.state').eq(3).find('li:eq(1) input').val();
                    // 获取名称
                    var accountname = $('.state').eq(3).find('li:eq(2) input').val();
                    // 获取来源
                    var origin = $('.state').eq(3).find('li:eq(3) select option:selected').attr('name');
                    // 获取到期时间
                    var expiredate = $('.state').eq(3).find('li:eq(4) select option:selected').attr('name');
                    setAjax({
                          url: seleceurl,
                        type: "GET",
                        data: {
                            'pageIndex': 1,
                            'pageSize': 20,
                            'BusinessType': 14004,
                            'Status': 15004,
                            'Number': account,
                            'Name': accountname,
                            'EndTime': expiredate,
                            'Platform': weixin,
                            'Source': origin
                        }
                        //"pageIndex=1&pageSize=20&BusinessType=14004&Status=15004&Number=" + account + "&Name=" + accountname + "&EndTime=" + expiredate + "&Platform=" + weixin + "&Source=" + origin,
                    }, function (data) {
                        if (data.Result.List != null) {
                            // 后台返回数据处理
                            for (var i = 0; i < data.Result.List.length; i++) {
                                if (data.Result.List[i].BeginTime) {
                                    data.Result.List[i].BeginTime = data.Result.List[i].BeginTime.split(" ")[0];
                                }
                                if (data.Result.List[i].EndTime) {
                                    data.Result.List[i].EndTime = data.Result.List[i].EndTime.split(" ")[0];
                                }
                                if (data.Result.List[i].FistName) {
                                    data.Result.List[i].FistName = data.Result.List[i].FistName.split("#");
                                } else {
                                    data.Result.List[i].FistName = []
                                }
                                if (data.Result.List[i].SecondName) {
                                    data.Result.List[i].SecondName = data.Result.List[i].SecondName.split("#");
                                } else {
                                    data.Result.List[i].SecondName = []
                                }
                                if (data.Result.List[i].ThridName) {
                                    data.Result.List[i].ThridName = data.Result.List[i].ThridName.split("#");
                                } else {
                                    data.Result.List[i].ThridName = []
                                }
                                if (data.Result.List[i].FourthName) {
                                    data.Result.List[i].FourthName = data.Result.List[i].FourthName.split("#");
                                } else {
                                    data.Result.List[i].FourthName = []
                                }

                            }
                        }
                        $('tbody').eq(3).html(ejs.render($('#shipin4').html(), data));
                        if (data.Result.TotleCount != 0) {
                            var counts = data.Result.TotleCount;
                            //分页
                            $("#pageContainer4").pagination(
                                counts,
                                {
                                    items_per_page: 20, //每页显示多少条记录（默认为20条）
                                    callback: function (currPage, jg) {
                                        // ajax请求
                                        setAjax({
                                              url: seleceurl,
                                            type: "GET",
                                            data: {
                                                'pageIndex': currPage,
                                                'pageSize': 20,
                                                'BusinessType': 14004,
                                                'Status': 15004,
                                                'Number': account,
                                                'Name': accountname,
                                                'EndTime': expiredate,
                                                'Platform': weixin,
                                                'Source': origin
                                            }
                                            //"pageIndex=" + currPage + "&pageSize=20&BusinessType=14004&Status=15004&Number=" + account + "&Name=" + accountname + "&EndTime=" + expiredate + "&Platform=" + weixin + "&Source=" + origin,
                                        }, function (data) {
                                            if (data.Result.List != null) {
                                                // 后台返回数据处理
                                                for (var i = 0; i < data.Result.List.length; i++) {
                                                    if (data.Result.List[i].BeginTime) {
                                                        data.Result.List[i].BeginTime = data.Result.List[i].BeginTime.split(" ")[0];
                                                    }
                                                    if (data.Result.List[i].EndTime) {
                                                        data.Result.List[i].EndTime = data.Result.List[i].EndTime.split(" ")[0];
                                                    }
                                                    if (data.Result.List[i].FistName) {
                                                        data.Result.List[i].FistName = data.Result.List[i].FistName.split("#");
                                                    } else {
                                                        data.Result.List[i].FistName = []
                                                    }
                                                    if (data.Result.List[i].SecondName) {
                                                        data.Result.List[i].SecondName = data.Result.List[i].SecondName.split("#");
                                                    } else {
                                                        data.Result.List[i].SecondName = []
                                                    }
                                                    if (data.Result.List[i].ThridName) {
                                                        data.Result.List[i].ThridName = data.Result.List[i].ThridName.split("#");
                                                    } else {
                                                        data.Result.List[i].ThridName = []
                                                    }
                                                    if (data.Result.List[i].FourthName) {
                                                        data.Result.List[i].FourthName = data.Result.List[i].FourthName.split("#");
                                                    } else {
                                                        data.Result.List[i].FourthName = []
                                                    }

                                                }
                                            }
                                            $('tbody').eq(3).html(ejs.render($('#shipin4').html(), data));
                                            // 公共功能
                                            sppublicFunction(3)

                                        })
                                    }
                                });

                        } else {
                            $('#pageContainer4').html('<img src="/images/no_data.png" style="display: block;margin: 70px auto;">')
                        }
                        // 公共功能
                        sppublicFunction(3)
                    })
                })
            })
        }
        // 直播刊例列表
        if ($('div[name="operate-zhibo"]').attr('name') == 'operate-zhibo') {
            $('div[name="operate-zhibo"]').css({display: 'block'})
            function zbpublicFunction(i) {
                judgeAuthority(4)
                // 点击申请上架
                $('tbody').find("tr").find('td:last a[name^=15002]').off('click').on('click', function () {
                    var data = $(this).attr('name').split(',')
                    if(Date.parse(new Date().format("yyyy/MM/dd")) - Date.parse($(this).parent().attr('name').split(' ')[0].split('-').join('/')) < 0 ) {
                    $.openPopupLayer({
                        name: "shipingclickApply",
                        url: "pricelist-application.html",
                        error: function (dd) {
                            alert(dd.status);
                        },
                        success: function () {
                            // 点击确定
                            $('.layer_con').find('.button').off('click').on('click', function () {
                                setAjax({
                                    url: Ajaxurl,
                                    type: "POST",
                                    data: {
                                        "PubID": data[1],
                                        "Status": data[0]
                                    }
                                }, function (data) {
                                    $.closePopupLayer('shipingclickApply');
                                    if (data.Result) {
                                        window.location.reload();
                                    }
                                })
                            })                 // 点击取消
                                .end().find('.but_keep').off('click').on('click', function () {
                                $.closePopupLayer('shipingclickApply')
                            })

                        }
                    });
                    }else {
                        alert('请调整刊例时间！')
                    }
                })
                // 点击驳回原因
                $('tbody').eq(i).find('tr').find('td[name=15004]').find('a').off('click').on('click', function () {
                    // 驳回原因的内容
                    var data = $(this).next().text();
                    $.openPopupLayer({
                        name: "Reject",
                        url: "pricelist-reason.html",
                        error: function (dd) {
                            alert(dd.status);
                        },
                        success: function () {
                            $('.mb25').html(data)
                            //点击取消
                            $('.button').off('click').on('click', function () {
                                $.closePopupLayer('Reject');
                            });
                            $('.fr>a').off('click').on('click', function () {
                                $.closePopupLayer('Reject');
                            });
                        }
                    });
                })
                // 上下架切换
                $('tbody tr').find('td:last span a').off('click').on('click', function (e) {
                    if(e.target.innerHTML=='编辑'){
                        return
                    }
                    var $this = $(this);
                    var data1 = $this.attr('name').split(',');
                    if($this.text()=='下架'){
                        setAjax({
                            url: Ajaxurl,
                            type: "POST",
                            data: {
                                "PubID": data1[1],
                                "Status": data1[0]
                            }
                        }, function (data) {
                            if (data.Result) {
                                // window.location.reload();
                                if (data1[0] == 15005) {
                                    $this.attr('name', '15006,' + data1[1]).text('下架');
                                    $this.attr('name', '15006,' + data1[1]).next().css('display','none')
                                } else {
                                    $this.attr('name', '15005,' + data1[1]).text('上架');
                                    $this.attr('name', '15005,' + data1[1]).next().css('display','inline-block')
                                }

                            }
                        })
                        return
                    }
                    if (Date.parse(new Date().format("yyyy/MM/dd")) - Date.parse($(this).parent().parent().attr('name').split(' ')[0].split('-').join('/')) < 0 ) {
                        setAjax({
                            url: Ajaxurl,
                            type: "POST",
                            data: {
                                "PubID": data1[1],
                                "Status": data1[0]
                            }
                        }, function (data) {
                            if (data.Result) {
                                // window.location.reload();
                                if (data1[0] == 15005) {
                                    $this.attr('name', '15006,' + data1[1]).text('下架').next().css('display','none')
                                } else {
                                    $this.attr('name', '15005,' + data1[1]).text('上架').next().css('display','inline-block')
                                };
                            };
                        });
                    } else {
                        alert('请调整刊例时间！')
                    };
                });
            };
            // 平台
            setAjax({
                url: "/api/DictInfo/GetDictInfoByTypeID",
                type: "GET",
                data: "typeID=34"
            }, function (data) {
                $('.platform2').html(ejs.render($('#platform1').html(), data));
            });
            // 全部
            $('.tab_menu li').eq(0).on('click', function () {
                setAjax({
                      url: seleceurl,
                    type: "GET",
                    data: {'pageIndex': 1, 'pageSize': 20, 'BusinessType': 14005}
                    //"pageIndex=1&pageSize=20&BusinessType=14005"
                }, function (data) {
                    if (data.Result.List != null) {
                        // 后台返回数据处理
                        for (var i = 0; i < data.Result.List.length; i++) {
                            if (data.Result.List[i].BeginTime) {
                                data.Result.List[i].BeginTime = data.Result.List[i].BeginTime.split(" ")[0];
                            }
                            if (data.Result.List[i].EndTime) {
                                data.Result.List[i].EndTime = data.Result.List[i].EndTime.split(" ")[0];
                            }
                            if (data.Result.List[i].FistName) {
                                data.Result.List[i].FistName = data.Result.List[i].FistName.split("#");
                            } else {
                                data.Result.List[i].FistName = []
                            }
                            if (data.Result.List[i].SecondName) {
                                data.Result.List[i].SecondName = data.Result.List[i].SecondName.split("#");
                            } else {
                                data.Result.List[i].SecondName = []
                            }
                            if (data.Result.List[i].ThridName) {
                                data.Result.List[i].ThridName = data.Result.List[i].ThridName.split("#");
                            } else {
                                data.Result.List[i].ThridName = []
                            }
                            if (data.Result.List[i].FourthName) {
                                data.Result.List[i].FourthName = data.Result.List[i].FourthName.split("#");
                            } else {
                                data.Result.List[i].FourthName = []
                            }

                        }
                    }
                    $('tbody').eq(0).html(ejs.render($('#zhibo1').html(), data));
                    if (data.Result.TotleCount != 0) {
                        var counts = data.Result.TotleCount;
                        //分页
                        $("#pageContainer1").pagination(
                            counts,
                            {
                                items_per_page: 20, //每页显示多少条记录（默认为20条）
                                callback: function (currPage, jg) {
                                    // ajax请求
                                    setAjax({
                                          url: seleceurl,
                                        type: "GET",
                                        data: {'pageIndex': currPage, 'pageSize': 20, 'BusinessType': 14005}
                                        //"pageIndex=" + currPage + "&pageSize=20&BusinessType=14005"
                                    }, function (data) {

                                        if (data.Result.List != null) {
                                            // 后台返回数据处理
                                            for (var i = 0; i < data.Result.List.length; i++) {
                                                if (data.Result.List[i].BeginTime) {
                                                    data.Result.List[i].BeginTime = data.Result.List[i].BeginTime.split(" ")[0];
                                                }
                                                if (data.Result.List[i].EndTime) {
                                                    data.Result.List[i].EndTime = data.Result.List[i].EndTime.split(" ")[0];
                                                }
                                                if (data.Result.List[i].FistName) {
                                                    data.Result.List[i].FistName = data.Result.List[i].FistName.split("#");
                                                } else {
                                                    data.Result.List[i].FistName = []
                                                }
                                                if (data.Result.List[i].SecondName) {
                                                    data.Result.List[i].SecondName = data.Result.List[i].SecondName.split("#");
                                                } else {
                                                    data.Result.List[i].SecondName = []
                                                }
                                                if (data.Result.List[i].ThridName) {
                                                    data.Result.List[i].ThridName = data.Result.List[i].ThridName.split("#");
                                                } else {
                                                    data.Result.List[i].ThridName = []
                                                }
                                                if (data.Result.List[i].FourthName) {
                                                    data.Result.List[i].FourthName = data.Result.List[i].FourthName.split("#");
                                                } else {
                                                    data.Result.List[i].FourthName = []
                                                }

                                            }
                                        }
                                        $('tbody').eq(0).html(ejs.render($('#zhibo1').html(), data));
                                        // 公共功能
                                        zbpublicFunction(0)

                                    })
                                }
                            });

                    } else {
                        $('#pageContainer1').html('<img src="/images/no_data.png" style="margin: 70px auto; display: block">')
                    }

                    // 公共功能
                    zbpublicFunction(0);
                })
                //查询
                $('.but_query').eq(0).off('click').on('click', function () {
                    // 获取平台
                    var weixin = $('.state').eq(0).find('li:eq(0) select option:selected').attr('name');
                    // 获取账号
                    var account = $('.state').eq(0).find('li:eq(1) input').val();
                    // 获取名称
                    var accountname = $('.state').eq(0).find('li:eq(2) input').val();
                    // 获取来源
                    var origin = $('.state').eq(0).find('li:eq(3) select option:selected').attr('name');
                    // 获取到期时间
                    var expiredate = $('.state').eq(0).find('li:eq(4) select option:selected').attr('name');
                    setAjax({
                          url: seleceurl,
                        type: "GET",
                        data: {
                            'pageIndex': 1,
                            'pageSize': 20,
                            'BusinessType': 14005,
                            'Number': account,
                            'Name': accountname,
                            'EndTime': expiredate,
                            'Platform': weixin,
                            'Source': origin
                        }
                        //"pageIndex=1&pageSize=20&BusinessType=14005&Number=" + account + "&Name=" + accountname + "&EndTime=" + expiredate + "&Platform=" + weixin + "&Source=" + origin
                    }, function (data) {

                        if (data.Result.List != null) {
                            // 后台返回数据处理
                            for (var i = 0; i < data.Result.List.length; i++) {
                                if (data.Result.List[i].BeginTime) {
                                    data.Result.List[i].BeginTime = data.Result.List[i].BeginTime.split(" ")[0];
                                }
                                if (data.Result.List[i].EndTime) {
                                    data.Result.List[i].EndTime = data.Result.List[i].EndTime.split(" ")[0];
                                }
                                if (data.Result.List[i].FistName) {
                                    data.Result.List[i].FistName = data.Result.List[i].FistName.split("#");
                                } else {
                                    data.Result.List[i].FistName = []
                                }
                                if (data.Result.List[i].SecondName) {
                                    data.Result.List[i].SecondName = data.Result.List[i].SecondName.split("#");
                                } else {
                                    data.Result.List[i].SecondName = []
                                }
                                if (data.Result.List[i].ThridName) {
                                    data.Result.List[i].ThridName = data.Result.List[i].ThridName.split("#");
                                } else {
                                    data.Result.List[i].ThridName = []
                                }
                                if (data.Result.List[i].FourthName) {
                                    data.Result.List[i].FourthName = data.Result.List[i].FourthName.split("#");
                                } else {
                                    data.Result.List[i].FourthName = []
                                }

                            }
                        }
                        $('tbody').eq(0).html(ejs.render($('#zhibo1').html(), data));
                        if (data.Result.TotleCount != 0) {
                            var counts = data.Result.TotleCount;
                            //分页
                            $("#pageContainer1").pagination(
                                counts,
                                {
                                    items_per_page: 20, //每页显示多少条记录（默认为20条）
                                    callback: function (currPage, jg) {
                                        // ajax请求
                                        setAjax({
                                              url: seleceurl,
                                            type: "GET",
                                            data: {
                                                'pageIndex': currPage,
                                                'pageSize': 20,
                                                'BusinessType': 14005,
                                                'Number': account,
                                                'Name': accountname,
                                                'EndTime': expiredate,
                                                'Platform': weixin,
                                                'Source': origin
                                            }
                                            //"pageIndex=" + currPage + "&pageSize=20&BusinessType=14005&Number=" + account + "&Name=" + accountname + "&EndTime=" + expiredate + "&Platform=" + weixin + "&Source=" + origin
                                        }, function (data) {

                                            if (data.Result.List != null) {
                                                // 后台返回数据处理
                                                for (var i = 0; i < data.Result.List.length; i++) {
                                                    if (data.Result.List[i].BeginTime) {
                                                        data.Result.List[i].BeginTime = data.Result.List[i].BeginTime.split(" ")[0];
                                                    }
                                                    if (data.Result.List[i].EndTime) {
                                                        data.Result.List[i].EndTime = data.Result.List[i].EndTime.split(" ")[0];
                                                    }
                                                    if (data.Result.List[i].FistName) {
                                                        data.Result.List[i].FistName = data.Result.List[i].FistName.split("#");
                                                    } else {
                                                        data.Result.List[i].FistName = []
                                                    }
                                                    if (data.Result.List[i].SecondName) {
                                                        data.Result.List[i].SecondName = data.Result.List[i].SecondName.split("#");
                                                    } else {
                                                        data.Result.List[i].SecondName = []
                                                    }
                                                    if (data.Result.List[i].ThridName) {
                                                        data.Result.List[i].ThridName = data.Result.List[i].ThridName.split("#");
                                                    } else {
                                                        data.Result.List[i].ThridName = []
                                                    }
                                                    if (data.Result.List[i].FourthName) {
                                                        data.Result.List[i].FourthName = data.Result.List[i].FourthName.split("#");
                                                    } else {
                                                        data.Result.List[i].FourthName = []
                                                    }

                                                }
                                            }
                                            $('tbody').eq(0).html(ejs.render($('#zhibo1').html(), data));
                                            // 公共功能
                                            zbpublicFunction(0)

                                        })
                                    }
                                });

                        } else {
                            $('#pageContainer1').html('<img src="/images/no_data.png" style="margin: 70px auto; display: block">')
                        }

                        // 公共功能
                        zbpublicFunction(0)
                    })
                })
            })
            $('.tab_menu li').eq(0).click();
            // 待审核
            $('.tab_menu li').eq(1).on('click', function () {
                setAjax({
                      url: seleceurl,
                    type: "GET",
                    data: {'pageIndex': 1, 'pageSize': 20, 'BusinessType': 14005, 'Status': 15002}
                    //"pageIndex=1&pageSize=20&BusinessType=14005&Status=15002"
                }, function (data) {

                    if (data.Result.List != null) {
                        // 后台返回数据处理
                        for (var i = 0; i < data.Result.List.length; i++) {
                            if (data.Result.List[i].BeginTime) {
                                data.Result.List[i].BeginTime = data.Result.List[i].BeginTime.split(" ")[0];
                            }
                            if (data.Result.List[i].EndTime) {
                                data.Result.List[i].EndTime = data.Result.List[i].EndTime.split(" ")[0];
                            }
                            if (data.Result.List[i].FistName) {
                                data.Result.List[i].FistName = data.Result.List[i].FistName.split("#");
                            } else {
                                data.Result.List[i].FistName = []
                            }
                            if (data.Result.List[i].SecondName) {
                                data.Result.List[i].SecondName = data.Result.List[i].SecondName.split("#");
                            } else {
                                data.Result.List[i].SecondName = []
                            }
                            if (data.Result.List[i].ThridName) {
                                data.Result.List[i].ThridName = data.Result.List[i].ThridName.split("#");
                            } else {
                                data.Result.List[i].ThridName = []
                            }
                            if (data.Result.List[i].FourthName) {
                                data.Result.List[i].FourthName = data.Result.List[i].FourthName.split("#");
                            } else {
                                data.Result.List[i].FourthName = []
                            }

                        }
                    }
                    $('tbody').eq(1).html(ejs.render($('#zhibo2').html(), data));
                    if (data.Result.TotleCount != 0) {
                        var counts = data.Result.TotleCount;
                        //分页
                        $("#pageContainer2").pagination(
                            counts,
                            {
                                items_per_page: 20, //每页显示多少条记录（默认为20条）
                                callback: function (currPage, jg) {
                                    // ajax请求
                                    setAjax({
                                          url: seleceurl,
                                        type: "GET",
                                        data: {
                                            'pageIndex': currPage,
                                            'pageSize': 20,
                                            'BusinessType': 14005,
                                            'Status': 15002
                                        }
                                        //"pageIndex=" + currPage + "&pageSize=20&BusinessType=14005&Status=15002"
                                    }, function (data) {

                                        if (data.Result.List != null) {
                                            // 后台返回数据处理
                                            for (var i = 0; i < data.Result.List.length; i++) {
                                                if (data.Result.List[i].BeginTime) {
                                                    data.Result.List[i].BeginTime = data.Result.List[i].BeginTime.split(" ")[0];
                                                }
                                                if (data.Result.List[i].EndTime) {
                                                    data.Result.List[i].EndTime = data.Result.List[i].EndTime.split(" ")[0];
                                                }
                                                if (data.Result.List[i].FistName) {
                                                    data.Result.List[i].FistName = data.Result.List[i].FistName.split("#");
                                                } else {
                                                    data.Result.List[i].FistName = []
                                                }
                                                if (data.Result.List[i].SecondName) {
                                                    data.Result.List[i].SecondName = data.Result.List[i].SecondName.split("#");
                                                } else {
                                                    data.Result.List[i].SecondName = []
                                                }
                                                if (data.Result.List[i].ThridName) {
                                                    data.Result.List[i].ThridName = data.Result.List[i].ThridName.split("#");
                                                } else {
                                                    data.Result.List[i].ThridName = []
                                                }
                                                if (data.Result.List[i].FourthName) {
                                                    data.Result.List[i].FourthName = data.Result.List[i].FourthName.split("#");
                                                } else {
                                                    data.Result.List[i].FourthName = []
                                                }

                                            }
                                        }
                                        $('tbody').eq(1).html(ejs.render($('#zhibo2').html(), data));
                                        // 公共功能
                                        zbpublicFunction(1)

                                    })
                                }
                            });

                    } else {
                        $('#pageContainer2').html('<img src="/images/no_data.png" style="margin: 70px auto;display: block">')
                    }
                    // 公共功能
                    zbpublicFunction(1);
                })
                //查询
                $('.but_query').eq(1).off('click').on('click', function () {
                    // 获取平台
                    var weixin = $('.state').eq(1).find('li:eq(0) select option:selected').attr('name');
                    // 获取账号
                    var account = $('.state').eq(1).find('li:eq(1) input').val();
                    // 获取名称
                    var accountname = $('.state').eq(1).find('li:eq(2) input').val();
                    // 获取来源
                    var origin = $('.state').eq(1).find('li:eq(3) select option:selected').attr('name');
                    // 获取到期时间
                    var expiredate = $('.state').eq(1).find('li:eq(4) select option:selected').attr('name');
                    setAjax({
                          url: seleceurl,
                        type: "GET",
                        data: {
                            'pageIndex': 1,
                            'pageSize': 20,
                            'BusinessType': 14005,
                            'Status': 15002,
                            'Number': account,
                            'Name': accountname,
                            'EndTime': expiredate,
                            'Platform': weixin,
                            'Source': origin
                        }
                        //"pageIndex=1&pageSize=20&BusinessType=14005&Status=15002&Number=" + account + "&Name=" + accountname + "&EndTime=" + expiredate + "&Platform=" + weixin + "&Source=" + origin
                    }, function (data) {

                        if (data.Result.List != null) {
                            // 后台返回数据处理
                            for (var i = 0; i < data.Result.List.length; i++) {
                                if (data.Result.List[i].BeginTime) {
                                    data.Result.List[i].BeginTime = data.Result.List[i].BeginTime.split(" ")[0];
                                }
                                if (data.Result.List[i].EndTime) {
                                    data.Result.List[i].EndTime = data.Result.List[i].EndTime.split(" ")[0];
                                }
                                if (data.Result.List[i].FistName) {
                                    data.Result.List[i].FistName = data.Result.List[i].FistName.split("#");
                                } else {
                                    data.Result.List[i].FistName = []
                                }
                                if (data.Result.List[i].SecondName) {
                                    data.Result.List[i].SecondName = data.Result.List[i].SecondName.split("#");
                                } else {
                                    data.Result.List[i].SecondName = []
                                }
                                if (data.Result.List[i].ThridName) {
                                    data.Result.List[i].ThridName = data.Result.List[i].ThridName.split("#");
                                } else {
                                    data.Result.List[i].ThridName = []
                                }
                                if (data.Result.List[i].FourthName) {
                                    data.Result.List[i].FourthName = data.Result.List[i].FourthName.split("#");
                                } else {
                                    data.Result.List[i].FourthName = []
                                }

                            }
                        }
                        $('tbody').eq(1).html(ejs.render($('#zhibo2').html(), data));

                        if (data.Result.TotleCount != 0) {
                            var counts = data.Result.TotleCount;
                            //分页
                            $("#pageContainer2").pagination(
                                counts,
                                {
                                    items_per_page: 20, //每页显示多少条记录（默认为20条）
                                    callback: function (currPage, jg) {
                                        // ajax请求
                                        setAjax({
                                              url: seleceurl,
                                            type: "GET",
                                            data: {
                                                'pageIndex': currPage,
                                                'pageSize': 20,
                                                'BusinessType': 14005,
                                                'Status': 15002,
                                                'Number': account,
                                                'Name': accountname,
                                                'EndTime': expiredate,
                                                'Platform': weixin,
                                                'Source': origin
                                            }
                                            //"pageIndex=" + currPage + "&pageSize=20&BusinessType=14005&Status=15002&Number=" + account + "&Name=" + accountname + "&EndTime=" + expiredate + "&Platform=" + weixin + "&Source=" + origin
                                        }, function (data) {

                                            if (data.Result.List != null) {
                                                // 后台返回数据处理
                                                for (var i = 0; i < data.Result.List.length; i++) {
                                                    if (data.Result.List[i].BeginTime) {
                                                        data.Result.List[i].BeginTime = data.Result.List[i].BeginTime.split(" ")[0];
                                                    }
                                                    if (data.Result.List[i].EndTime) {
                                                        data.Result.List[i].EndTime = data.Result.List[i].EndTime.split(" ")[0];
                                                    }
                                                    if (data.Result.List[i].FistName) {
                                                        data.Result.List[i].FistName = data.Result.List[i].FistName.split("#");
                                                    } else {
                                                        data.Result.List[i].FistName = []
                                                    }
                                                    if (data.Result.List[i].SecondName) {
                                                        data.Result.List[i].SecondName = data.Result.List[i].SecondName.split("#");
                                                    } else {
                                                        data.Result.List[i].SecondName = []
                                                    }
                                                    if (data.Result.List[i].ThridName) {
                                                        data.Result.List[i].ThridName = data.Result.List[i].ThridName.split("#");
                                                    } else {
                                                        data.Result.List[i].ThridName = []
                                                    }
                                                    if (data.Result.List[i].FourthName) {
                                                        data.Result.List[i].FourthName = data.Result.List[i].FourthName.split("#");
                                                    } else {
                                                        data.Result.List[i].FourthName = []
                                                    }

                                                }
                                            }
                                            $('tbody').eq(1).html(ejs.render($('#zhibo2').html(), data));
                                            // 公共功能
                                            zbpublicFunction(1)

                                        })
                                    }
                                });

                        } else {
                            $('#pageContainer2').html('<img src="/images/no_data.png" style="margin: 70px auto;display: block">')
                        }

                        // 公共功能
                        zbpublicFunction(1)
                    })
                })
            })
            // 通过
            $('.tab_menu li').eq(2).on('click', function () {
                setAjax({
                      url: seleceurl,
                    type: "GET",
                    data: {'pageIndex': 1, 'pageSize': 20, 'BusinessType': 14005, 'Status': 15003}
                    //"pageIndex=1&pageSize=20&BusinessType=14005&Status=15003"
                }, function (data) {

                    if (data.Result.List != null) {
                        // 后台返回数据处理
                        for (var i = 0; i < data.Result.List.length; i++) {
                            if (data.Result.List[i].BeginTime) {
                                data.Result.List[i].BeginTime = data.Result.List[i].BeginTime.split(" ")[0];
                            }
                            if (data.Result.List[i].EndTime) {
                                data.Result.List[i].EndTime = data.Result.List[i].EndTime.split(" ")[0];
                            }
                            if (data.Result.List[i].FistName) {
                                data.Result.List[i].FistName = data.Result.List[i].FistName.split("#");
                            } else {
                                data.Result.List[i].FistName = []
                            }
                            if (data.Result.List[i].SecondName) {
                                data.Result.List[i].SecondName = data.Result.List[i].SecondName.split("#");
                            } else {
                                data.Result.List[i].SecondName = []
                            }
                            if (data.Result.List[i].ThridName) {
                                data.Result.List[i].ThridName = data.Result.List[i].ThridName.split("#");
                            } else {
                                data.Result.List[i].ThridName = []
                            }
                            if (data.Result.List[i].FourthName) {
                                data.Result.List[i].FourthName = data.Result.List[i].FourthName.split("#");
                            } else {
                                data.Result.List[i].FourthName = []
                            }

                        }
                    }
                    $('tbody').eq(2).html(ejs.render($('#zhibo3').html(), data));
                    if (data.Result.TotleCount != 0) {
                        var counts = data.Result.TotleCount;
                        //分页
                        $("#pageContainer3").pagination(
                            counts,
                            {
                                items_per_page: 20, //每页显示多少条记录（默认为20条）
                                callback: function (currPage, jg) {
                                    // ajax请求
                                    setAjax({
                                          url: seleceurl,
                                        type: "GET",
                                        data: {
                                            'pageIndex': currPage,
                                            'pageSize': 20,
                                            'Status': 15003,
                                            'BusinessType': 14005
                                        }
                                        //"pageIndex=" + currPage + "&pageSize=20&Status=15003&BusinessType=14005"
                                    }, function (data) {

                                        if (data.Result.List != null) {
                                            // 后台返回数据处理
                                            for (var i = 0; i < data.Result.List.length; i++) {
                                                if (data.Result.List[i].BeginTime) {
                                                    data.Result.List[i].BeginTime = data.Result.List[i].BeginTime.split(" ")[0];
                                                }
                                                if (data.Result.List[i].EndTime) {
                                                    data.Result.List[i].EndTime = data.Result.List[i].EndTime.split(" ")[0];
                                                }
                                                if (data.Result.List[i].FistName) {
                                                    data.Result.List[i].FistName = data.Result.List[i].FistName.split("#");
                                                } else {
                                                    data.Result.List[i].FistName = []
                                                }
                                                if (data.Result.List[i].SecondName) {
                                                    data.Result.List[i].SecondName = data.Result.List[i].SecondName.split("#");
                                                } else {
                                                    data.Result.List[i].SecondName = []
                                                }
                                                if (data.Result.List[i].ThridName) {
                                                    data.Result.List[i].ThridName = data.Result.List[i].ThridName.split("#");
                                                } else {
                                                    data.Result.List[i].ThridName = []
                                                }
                                                if (data.Result.List[i].FourthName) {
                                                    data.Result.List[i].FourthName = data.Result.List[i].FourthName.split("#");
                                                } else {
                                                    data.Result.List[i].FourthName = []
                                                }

                                            }
                                        }
                                        $('tbody').eq(2).html(ejs.render($('#zhibo3').html(), data));
                                        // 公共功能
                                        zbpublicFunction(2)

                                    })
                                }
                            });

                    } else {
                        $('#pageContainer3').html('<img src="/images/no_data.png" style="margin: 70px auto;display: block">')
                    }
                    // 公共功能
                    zbpublicFunction(2);
                })
                //查询
                $('.but_query').eq(2).off('click').on('click', function () {
                    // 获取平台
                    var weixin = $('.state').eq(2).find('li:eq(0) select option:selected').attr('name');
                    // 获取账号
                    var account = $('.state').eq(2).find('li:eq(1) input').val();
                    // 获取名称
                    var accountname = $('.state').eq(2).find('li:eq(2) input').val();
                    // 获取来源
                    var origin = $('.state').eq(2).find('li:eq(3) select option:selected').attr('name');
                    // 获取到期时间
                    var expiredate = $('.state').eq(2).find('li:eq(4) select option:selected').attr('name');
                    setAjax({
                          url: seleceurl,
                        type: "GET",
                        data: {
                            'pageIndex': 1,
                            'pageSize': 20,
                            'BusinessType': 14005,
                            'Status': 15003,
                            'Number': account,
                            'Name': accountname,
                            'EndTime': expiredate,
                            'Platform': weixin,
                            'Source': origin
                        }
                        //"pageIndex=1&pageSize=20&BusinessType=14005&Status=15003&Number=" + account + "&Name=" + accountname + "&EndTime=" + expiredate + "&Platform=" + weixin + "&Source=" + origin
                    }, function (data) {
                        if (data.Result.List != null) {
                            // 后台返回数据处理
                            for (var i = 0; i < data.Result.List.length; i++) {
                                if (data.Result.List[i].BeginTime) {
                                    data.Result.List[i].BeginTime = data.Result.List[i].BeginTime.split(" ")[0];
                                }
                                if (data.Result.List[i].EndTime) {
                                    data.Result.List[i].EndTime = data.Result.List[i].EndTime.split(" ")[0];
                                }
                                if (data.Result.List[i].FistName) {
                                    data.Result.List[i].FistName = data.Result.List[i].FistName.split("#");
                                } else {
                                    data.Result.List[i].FistName = []
                                }
                                if (data.Result.List[i].SecondName) {
                                    data.Result.List[i].SecondName = data.Result.List[i].SecondName.split("#");
                                } else {
                                    data.Result.List[i].SecondName = []
                                }
                                if (data.Result.List[i].ThridName) {
                                    data.Result.List[i].ThridName = data.Result.List[i].ThridName.split("#");
                                } else {
                                    data.Result.List[i].ThridName = []
                                }
                                if (data.Result.List[i].FourthName) {
                                    data.Result.List[i].FourthName = data.Result.List[i].FourthName.split("#");
                                } else {
                                    data.Result.List[i].FourthName = []
                                }

                            }
                        }
                        $('tbody').eq(2).html(ejs.render($('#zhibo3').html(), data));

                        if (data.Result.TotleCount != 0) {
                            var counts = data.Result.TotleCount;
                            //分页
                            $("#pageContainer3").pagination(
                                counts,
                                {
                                    items_per_page: 20, //每页显示多少条记录（默认为20条）
                                    callback: function (currPage, jg) {
                                        // ajax请求
                                        setAjax({
                                              url: seleceurl,
                                            type: "GET",
                                            data: {
                                                'pageIndex': currPage,
                                                'pageSize': 20,
                                                'BusinessType': 14005,
                                                'Status': 15003,
                                                'Number': account,
                                                'Name': accountname,
                                                'EndTime': expiredate,
                                                'Platform': weixin,
                                                'Source': origin
                                            }
                                            //"pageIndex=" + currPage + "&pageSize=20&BusinessType=14005&Status=15003&Number=" + account + "&Name=" + accountname + "&EndTime=" + expiredate + "&Platform=" + weixin + "&Source=" + origin
                                        }, function (data) {

                                            if (data.Result.List != null) {
                                                // 后台返回数据处理
                                                for (var i = 0; i < data.Result.List.length; i++) {
                                                    if (data.Result.List[i].BeginTime) {
                                                        data.Result.List[i].BeginTime = data.Result.List[i].BeginTime.split(" ")[0];
                                                    }
                                                    if (data.Result.List[i].EndTime) {
                                                        data.Result.List[i].EndTime = data.Result.List[i].EndTime.split(" ")[0];
                                                    }
                                                    if (data.Result.List[i].FistName) {
                                                        data.Result.List[i].FistName = data.Result.List[i].FistName.split("#");
                                                    } else {
                                                        data.Result.List[i].FistName = []
                                                    }
                                                    if (data.Result.List[i].SecondName) {
                                                        data.Result.List[i].SecondName = data.Result.List[i].SecondName.split("#");
                                                    } else {
                                                        data.Result.List[i].SecondName = []
                                                    }
                                                    if (data.Result.List[i].ThridName) {
                                                        data.Result.List[i].ThridName = data.Result.List[i].ThridName.split("#");
                                                    } else {
                                                        data.Result.List[i].ThridName = []
                                                    }
                                                    if (data.Result.List[i].FourthName) {
                                                        data.Result.List[i].FourthName = data.Result.List[i].FourthName.split("#");
                                                    } else {
                                                        data.Result.List[i].FourthName = []
                                                    }

                                                }
                                            }
                                            $('tbody').eq(2).html(ejs.render($('#zhibo3').html(), data));
                                            // 公共功能
                                            zbpublicFunction(2)

                                        })
                                    }
                                });

                        } else {
                            $('#pageContainer3').html('<img src="/images/no_data.png" style="margin: 70px auto;display: block">')
                        }

                        // 公共功能
                        zbpublicFunction(2)
                    })
                })
            })
            // 驳回
            $('.tab_menu li').eq(3).on('click', function () {
                setAjax({
                      url: seleceurl,
                    type: "GET",
                    data: {'pageIndex': 1, 'pageSize': 20, 'BusinessType': 14005, 'Status': 15004}
                    //"pageIndex=1&pageSize=20&BusinessType=14005&Status=15004"
                }, function (data) {

                    if (data.Result.List != null) {
                        // 后台返回数据处理
                        for (var i = 0; i < data.Result.List.length; i++) {
                            if (data.Result.List[i].BeginTime) {
                                data.Result.List[i].BeginTime = data.Result.List[i].BeginTime.split(" ")[0];
                            }
                            if (data.Result.List[i].EndTime) {
                                data.Result.List[i].EndTime = data.Result.List[i].EndTime.split(" ")[0];
                            }
                            if (data.Result.List[i].FistName) {
                                data.Result.List[i].FistName = data.Result.List[i].FistName.split("#");
                            } else {
                                data.Result.List[i].FistName = []
                            }
                            if (data.Result.List[i].SecondName) {
                                data.Result.List[i].SecondName = data.Result.List[i].SecondName.split("#");
                            } else {
                                data.Result.List[i].SecondName = []
                            }
                            if (data.Result.List[i].ThridName) {
                                data.Result.List[i].ThridName = data.Result.List[i].ThridName.split("#");
                            } else {
                                data.Result.List[i].ThridName = []
                            }
                            if (data.Result.List[i].FourthName) {
                                data.Result.List[i].FourthName = data.Result.List[i].FourthName.split("#");
                            } else {
                                data.Result.List[i].FourthName = []
                            }

                        }
                    }
                    $('tbody').eq(3).html(ejs.render($('#zhibo4').html(), data));
                    if (data.Result.TotleCount != 0) {
                        var counts = data.Result.TotleCount;
                        //分页
                        $("#pageContainer4").pagination(
                            counts,
                            {
                                items_per_page: 20, //每页显示多少条记录（默认为20条）
                                callback: function (currPage, jg) {
                                    // ajax请求
                                    setAjax({
                                          url: seleceurl,
                                        type: "GET",
                                        data: {
                                            'pageIndex': currPage,
                                            'pageSize': 20,
                                            'Status': 15004,
                                            'BusinessType': 14005
                                        }
                                        //"pageIndex=" + currPage + "&pageSize=20&Status=15004&BusinessType=14005"
                                    }, function (data) {
                                        if (data.Result.List != null) {
                                            // 后台返回数据处理
                                            for (var i = 0; i < data.Result.List.length; i++) {
                                                if (data.Result.List[i].BeginTime) {
                                                    data.Result.List[i].BeginTime = data.Result.List[i].BeginTime.split(" ")[0];
                                                }
                                                if (data.Result.List[i].EndTime) {
                                                    data.Result.List[i].EndTime = data.Result.List[i].EndTime.split(" ")[0];
                                                }
                                                if (data.Result.List[i].FistName) {
                                                    data.Result.List[i].FistName = data.Result.List[i].FistName.split("#");
                                                } else {
                                                    data.Result.List[i].FistName = []
                                                }
                                                if (data.Result.List[i].SecondName) {
                                                    data.Result.List[i].SecondName = data.Result.List[i].SecondName.split("#");
                                                } else {
                                                    data.Result.List[i].SecondName = []
                                                }
                                                if (data.Result.List[i].ThridName) {
                                                    data.Result.List[i].ThridName = data.Result.List[i].ThridName.split("#");
                                                } else {
                                                    data.Result.List[i].ThridName = []
                                                }
                                                if (data.Result.List[i].FourthName) {
                                                    data.Result.List[i].FourthName = data.Result.List[i].FourthName.split("#");
                                                } else {
                                                    data.Result.List[i].FourthName = []
                                                }

                                            }
                                        }
                                        $('tbody').eq(3).html(ejs.render($('#zhibo4').html(), data));
                                        // 公共功能
                                        zbpublicFunction(3)

                                    })
                                }
                            });

                    } else {
                        $('#pageContainer4').html('<img src="/images/no_data.png" style="margin: 70px auto;display: block">')
                    }
                    // 公共功能
                    zbpublicFunction(3);
                })
                //查询
                $('.but_query').eq(3).off('click').on('click', function () {
                    // 获取平台
                    var weixin = $('.state').eq(3).find('li:eq(0) select option:selected').attr('name');
                    // 获取账号
                    var account = $('.state').eq(3).find('li:eq(1) input').val();
                    // 获取名称
                    var accountname = $('.state').eq(3).find('li:eq(2) input').val();
                    // 获取来源
                    var origin = $('.state').eq(3).find('li:eq(3) select option:selected').attr('name');
                    // 获取到期时间
                    var expiredate = $('.state').eq(3).find('li:eq(4) select option:selected').attr('name');
                    setAjax({
                          url: seleceurl,
                        type: "GET",
                        data: {
                            'pageIndex': 1,
                            'pageSize': 20,
                            'BusinessType': 14005,
                            'Status': 15004,
                            'Number': account,
                            'Name': accountname,
                            'EndTime': expiredate,
                            'Platform': weixin,
                            'Source': origin
                        }
                        //"pageIndex=1&pageSize=20&BusinessType=14005&Status=15004&Number=" + account + "&Name=" + accountname + "&EndTime=" + expiredate + "&Platform=" + weixin + "&Source=" + origin
                    }, function (data) {
                        if (data.Result.List != null) {
                            // 后台返回数据处理
                            for (var i = 0; i < data.Result.List.length; i++) {
                                if (data.Result.List[i].BeginTime) {
                                    data.Result.List[i].BeginTime = data.Result.List[i].BeginTime.split(" ")[0];
                                }
                                if (data.Result.List[i].EndTime) {
                                    data.Result.List[i].EndTime = data.Result.List[i].EndTime.split(" ")[0];
                                }
                                if (data.Result.List[i].FistName) {
                                    data.Result.List[i].FistName = data.Result.List[i].FistName.split("#");
                                } else {
                                    data.Result.List[i].FistName = []
                                }
                                if (data.Result.List[i].SecondName) {
                                    data.Result.List[i].SecondName = data.Result.List[i].SecondName.split("#");
                                } else {
                                    data.Result.List[i].SecondName = []
                                }
                                if (data.Result.List[i].ThridName) {
                                    data.Result.List[i].ThridName = data.Result.List[i].ThridName.split("#");
                                } else {
                                    data.Result.List[i].ThridName = []
                                }
                                if (data.Result.List[i].FourthName) {
                                    data.Result.List[i].FourthName = data.Result.List[i].FourthName.split("#");
                                } else {
                                    data.Result.List[i].FourthName = []
                                }

                            }
                        }
                        $('tbody').eq(3).html(ejs.render($('#zhibo4').html(), data));

                        if (data.Result.TotleCount != 0) {
                            var counts = data.Result.TotleCount;
                            //分页
                            $("#pageContainer4").pagination(
                                counts,
                                {
                                    items_per_page: 20, //每页显示多少条记录（默认为20条）
                                    callback: function (currPage, jg) {
                                        // ajax请求
                                        setAjax({
                                              url: seleceurl,
                                            type: "GET",
                                            data: {
                                                'pageIndex': currPage,
                                                'pageSize': 20,
                                                'BusinessType': 14005,
                                                'Status': 15004,
                                                'Number': account,
                                                'Name': accountname,
                                                'EndTime': expiredate,
                                                'Platform': weixin,
                                                'Source': origin
                                            }
                                            //"pageIndex=" + currPage + "&pageSize=20&BusinessType=14005&Status=15004&Number=" + account + "&Name=" + accountname + "&EndTime=" + expiredate + "&Platform=" + weixin + "&Source=" + origin
                                        }, function (data) {
                                            if (data.Result.List != null) {
                                                // 后台返回数据处理
                                                for (var i = 0; i < data.Result.List.length; i++) {
                                                    if (data.Result.List[i].BeginTime) {
                                                        data.Result.List[i].BeginTime = data.Result.List[i].BeginTime.split(" ")[0];
                                                    }
                                                    if (data.Result.List[i].EndTime) {
                                                        data.Result.List[i].EndTime = data.Result.List[i].EndTime.split(" ")[0];
                                                    }
                                                    if (data.Result.List[i].FistName) {
                                                        data.Result.List[i].FistName = data.Result.List[i].FistName.split("#");
                                                    } else {
                                                        data.Result.List[i].FistName = []
                                                    }
                                                    if (data.Result.List[i].SecondName) {
                                                        data.Result.List[i].SecondName = data.Result.List[i].SecondName.split("#");
                                                    } else {
                                                        data.Result.List[i].SecondName = []
                                                    }
                                                    if (data.Result.List[i].ThridName) {
                                                        data.Result.List[i].ThridName = data.Result.List[i].ThridName.split("#");
                                                    } else {
                                                        data.Result.List[i].ThridName = []
                                                    }
                                                    if (data.Result.List[i].FourthName) {
                                                        data.Result.List[i].FourthName = data.Result.List[i].FourthName.split("#");
                                                    } else {
                                                        data.Result.List[i].FourthName = []
                                                    }

                                                }
                                            }
                                            $('tbody').eq(3).html(ejs.render($('#zhibo4').html(), data));
                                            // 公共功能
                                            zbpublicFunction(3)

                                        })
                                    }
                                });

                        } else {
                            $('#pageContainer4').html('<img src="/images/no_data.png" style="margin: 70px auto;display: block">')
                        }

                        // 公共功能
                        zbpublicFunction(3)
                    })
                })
            })
        }
        // app刊例列表
        if ($('div[name="operate-app"]').attr('name') == 'operate-app') {
            $('div[name="operate-app"]').css({display: 'block'})
            function chaxu() {
                // 获取行业分类
                var industry=$('select[name=industry] option:selected').eq(0).attr('dictid')||-2;
                // 获取名称
                var accountname = $('.state').eq(0).find('li:eq(0) input').val()||'';
                // 获取来源
                var origin = $('.state').eq(0).find('li:eq(1) select option:selected').attr('name')||'';
                return '&CategoryId='+industry+'&Name='+accountname+'&Source='+origin
            }
            function apppublicFunction(i) {
                judgeAuthority(5)
                // 点击申请上架
                $('tbody').find("tr").find('td:last a[name^=15002]').off('click').on('click', function () {
                    var data = $(this).attr('name').split(',')
                    if(Date.parse(new Date().format("yyyy/MM/dd")) - Date.parse($(this).parent().attr('name').split(' ')[0].split('-').join('/')) < 0) {
                    if ($(this).parent().prev().prev().prev().prev().prev().prev().prev().prev().html() == '0') {
                        alert("广告位数量为0，不能申请上架！");
                    }else {
                    $.openPopupLayer({
                        name: "shipingclickApply",
                        url: "pricelist-application.html",
                        error: function (dd) {
                            alert(dd.status);
                        },
                        success: function () {
                            // 点击确定
                            $('.layer_con').find('.button').off('click').on('click', function () {
                                setAjax({
                                    url: Ajaxurl,
                                    type: "POST",
                                    data: {
                                        "PubID": data[1],
                                        "Status": data[0]
                                    }
                                }, function (data) {
                                    $.closePopupLayer('shipingclickApply');
                                    if (data.Result) {
                                        window.location.reload();
                                    }
                                })
                            })                 // 点击取消
                                .end().find('.but_keep').off('click').on('click', function () {
                                $.closePopupLayer('shipingclickApply')
                            })

                        }
                    });
                }
                    }else {
                        alert('请调整刊例时间！')
                    }
                })
                // 点击驳回原因
                $('tbody').eq(i).find('tr').find('td[name=15004]').find('a').off('click').on('click', function () {
                    // 驳回原因的内容
                    var data = $(this).next().text();
                    $.openPopupLayer({
                        name: "Reject",
                        url: "pricelist-reason.html",
                        error: function (dd) {
                            alert(dd.status);
                        },
                        success: function () {
                            $('.mb25').html(data)
                            //点击取消
                            $('.button').off('click').on('click', function () {
                                $.closePopupLayer('Reject');
                            });
                            $('.fr>a').off('click').on('click', function () {
                                $.closePopupLayer('Reject');
                            });
                        }
                    });
                })
                // 上下架切换
                $('tbody tr').find('td:last span a').off('click').on('click', function (e) {
                    if(e.target.innerHTML=='编辑'){
                        return
                    }
                    var $this = $(this);
                    var data1 = $this.attr('name').split(',');
                    if($this.text()=='下架'){
                        setAjax({
                            url: Ajaxurl,
                            type: "POST",
                            data: {
                                "PubID": data1[1],
                                "Status": data1[0]
                            }
                        }, function (data) {
                            if (data.Result) {
                                // window.location.reload();
                                if (data1[0] == 15005) {
                                    $this.attr('name', '15006,' + data1[1]).text('下架').parents('td').prev().text('上架');
                                    $this.attr('name', '15006,' + data1[1]).next().css('display','none')
                                } else {
                                    $this.attr('name', '15005,' + data1[1]).text('上架').parents('td').prev().text('下架');
                                    $this.attr('name', '15005,' + data1[1]).next().css('display','inline-block')
                                }

                            }
                        })
                        return
                    }
                    if (Date.parse(new Date().format("yyyy/MM/dd")) - Date.parse($(this).parent().parent().attr('name').split(' ')[0].split('-').join('/')) < 0 ) {
                        setAjax({
                            url: Ajaxurl,
                            type: "POST",
                            data: {
                                "PubID": data1[1],
                                "Status": data1[0]
                            }
                        }, function (data) {
                            if (data.Result) {
                                if (data1[0] == 15005) {
                                    $this.attr('name', '15006,' + data1[1]).text('下架').next().css('display','none')
                                } else {
                                    $this.attr('name', '15005,' + data1[1]).text('上架').next().css('display','inline-block')
                                }

                            }
                        })
                    } else {
                        alert('请调整刊例时间！')
                    }
                })

            };
            // 全部
            $('.tab_menu li').eq(0).on('click', function () {
                setAjax({
                      url: seleceurl,
                    type: "GET",
                    data: "pageIndex=1&pageSize=20&BusinessType=14002"+chaxu()
                }, function (data) {
                    if (data.Result.List != null) {
                        // 后台返回数据处理
                        for (var i = 0; i < data.Result.List.length; i++) {
                            if (data.Result.List[i].BeginTime) {
                                data.Result.List[i].BeginTime = data.Result.List[i].BeginTime.split(" ")[0];
                            }
                            if (data.Result.List[i].EndTime) {
                                data.Result.List[i].EndTime = data.Result.List[i].EndTime.split(" ")[0];
                            }
                        }
                    }
                    $('tbody').eq(0).html(ejs.render($('#app1').html(), data));
                    if (data.Result.TotleCount != 0) {
                        var counts = data.Result.TotleCount;
                        //分页
                        $("#pageContainer1").pagination(
                            counts,
                            {
                                items_per_page: 20, //每页显示多少条记录（默认为20条）
                                callback: function (currPage, jg) {
                                    // ajax请求
                                    setAjax({
                                          url: seleceurl,
                                        type: "GET",
                                        data: "pageIndex=" + currPage + "&pageSize=20&BusinessType=14002"+chaxu()
                                    }, function (data) {
                                        if (data.Result.List != null) {
                                            // 后台返回数据处理
                                            for (var i = 0; i < data.Result.List.length; i++) {
                                                if (data.Result.List[i].BeginTime) {
                                                    data.Result.List[i].BeginTime = data.Result.List[i].BeginTime.split(" ")[0];
                                                }
                                                if (data.Result.List[i].EndTime) {
                                                    data.Result.List[i].EndTime = data.Result.List[i].EndTime.split(" ")[0];
                                                }
                                            }
                                        }
                                        $('tbody').eq(0).html(ejs.render($('#app1').html(), data));
                                        // 公共功能
                                        apppublicFunction(0)

                                    })
                                }
                            });

                    } else {
                        $('#pageContainer1').html('<img src="/images/no_data.png" style="display: block;margin: 70px auto;">')
                    }
                    // 公共功能
                    apppublicFunction(0);
                })
                //查询
                $('.but_query').eq(0).off('click').on('click', function () {
                    // 获取行业分类
                    var industry=$('select[name=industry] option:selected').eq(0).attr('dictid');
                    // 获取名称
                    var accountname = $('.state').eq(0).find('li:eq(0) input').val();
                    // 获取来源
                    var origin = $('.state').eq(0).find('li:eq(1) select option:selected').attr('name');
                    setAjax({
                          url: seleceurl,
                        type: "GET",
                        data: {
                            'pageIndex': 1,
                            'pageSize': 20,
                            'BusinessType': 14002,
                            'Name': accountname,
                            'Source': origin,
                            'CategoryID':industry
                        }
                        //"pageIndex=1&pageSize=20&BusinessType=14002&Name=" + accountname + "&Source=" + origin,
                    }, function (data) {
                        if (data.Result.List != null) {
                            // 后台返回数据处理
                            for (var i = 0; i < data.Result.List.length; i++) {
                                if (data.Result.List[i].BeginTime) {
                                    data.Result.List[i].BeginTime = data.Result.List[i].BeginTime.split(" ")[0];
                                }
                                if (data.Result.List[i].EndTime) {
                                    data.Result.List[i].EndTime = data.Result.List[i].EndTime.split(" ")[0];
                                }
                            }
                        }
                        $('tbody').eq(0).html(ejs.render($('#app1').html(), data));
                        if (data.Result.TotleCount != 0) {
                            var counts = data.Result.TotleCount;
                            //分页
                            $("#pageContainer1").pagination(
                                counts,
                                {
                                    items_per_page: 20, //每页显示多少条记录（默认为20条）
                                    callback: function (currPage, jg) {
                                        // ajax请求
                                        setAjax({
                                              url: seleceurl,
                                            type: "GET",
                                            data: {
                                                'pageIndex': currPage,
                                                'pageSize': 20,
                                                'BusinessType': 14002,
                                                'Name': accountname,
                                                'Source': origin,
                                                'CategoryID':industry
                                            }
                                            //"pageIndex=" + currPage + "&pageSize=20&BusinessType=14002&Name=" + accountname + "&Source=" + origin
                                        }, function (data) {
                                            if (data.Result.List != null) {
                                                // 后台返回数据处理
                                                for (var i = 0; i < data.Result.List.length; i++) {
                                                    if (data.Result.List[i].BeginTime) {
                                                        data.Result.List[i].BeginTime = data.Result.List[i].BeginTime.split(" ")[0];
                                                    }
                                                    if (data.Result.List[i].EndTime) {
                                                        data.Result.List[i].EndTime = data.Result.List[i].EndTime.split(" ")[0];
                                                    }
                                                }
                                            }
                                            $('tbody').eq(0).html(ejs.render($('#app1').html(), data));
                                            // 公共功能
                                            apppublicFunction(0)

                                        })
                                    }
                                });

                        } else {
                            $('#pageContainer1').html('<img src="/images/no_data.png" style="display: block;margin: 70px auto;">')
                        }
                        // 公共功能
                        apppublicFunction(0)
                    })
                })
            })
            $('.tab_menu li').eq(0).click();
            // 待审核
            $('.tab_menu li').eq(1).on('click', function () {
                setAjax({
                      url: seleceurl,
                    type: "GET",
                    data: "pageIndex=1&pageSize=20&BusinessType=14002&Status=15002"+chaxu()
                    //"pageIndex=1&pageSize=20&BusinessType=14002&Status=15002"
                }, function (data) {
                    if (data.Result.List != null) {
                        // 后台返回数据处理
                        for (var i = 0; i < data.Result.List.length; i++) {
                            if (data.Result.List[i].BeginTime) {
                                data.Result.List[i].BeginTime = data.Result.List[i].BeginTime.split(" ")[0];
                            }
                            if (data.Result.List[i].EndTime) {
                                data.Result.List[i].EndTime = data.Result.List[i].EndTime.split(" ")[0];
                            }
                        }
                    }
                    $('tbody').eq(1).html(ejs.render($('#app2').html(), data));
                    if (data.Result.TotleCount != 0) {
                        var counts = data.Result.TotleCount;
                        //分页
                        $("#pageContainer2").pagination(
                            counts,
                            {
                                items_per_page: 20, //每页显示多少条记录（默认为20条）
                                callback: function (currPage, jg) {
                                    // ajax请求
                                    setAjax({
                                          url: seleceurl,
                                        type: "GET",
                                        data: "pageIndex=" + currPage + "&pageSize=20&Status=15002&BusinessType=14002"+chaxu()
                                    }, function (data) {
                                        $('tbody').eq(1).html(ejs.render($('#app2').html(), data));
                                        // 公共功能
                                        apppublicFunction(1)

                                    })
                                }
                            });

                    } else {
                        $('#pageContainer2').html('<img src="/images/no_data.png" style="display: block;margin: 70px auto;">')
                    }
                    // 公共功能
                    apppublicFunction(1);
                })
                //查询
                $('.but_query').eq(1).off('click').on('click', function () {
                    // 获取行业分类
                    var industry=$('select[name=industry] option:selected').eq(1).attr('dictid');
                    // 获取名称
                    var accountname = $('.state').eq(1).find('li:eq(0) input').val();
                    // 获取来源
                    var origin = $('.state').eq(1).find('li:eq(1) select option:selected').attr('name');
                    setAjax({
                          url: seleceurl,
                        type: "GET",
                        data: {
                            'pageIndex': 1,
                            'pageSize': 20,
                            'Status': 15002,
                            'BusinessType': 14002,
                            'Name': accountname,
                            'Source': origin,
                            'CategoryID':industry
                        }
                        //"pageIndex=1&pageSize=20&BusinessType=14002&Status=15002&Name=" + accountname + "&Source=" + origin,
                    }, function (data) {
                        if (data.Result.List != null) {
                            // 后台返回数据处理
                            for (var i = 0; i < data.Result.List.length; i++) {
                                if (data.Result.List[i].BeginTime) {
                                    data.Result.List[i].BeginTime = data.Result.List[i].BeginTime.split(" ")[0];
                                }
                                if (data.Result.List[i].EndTime) {
                                    data.Result.List[i].EndTime = data.Result.List[i].EndTime.split(" ")[0];
                                }
                            }
                        }
                        $('tbody').eq(1).html(ejs.render($('#app2').html(), data));
                        if (data.Result.TotleCount != 0) {
                            var counts = data.Result.TotleCount;
                            //分页
                            $("#pageContainer2").pagination(
                                counts,
                                {
                                    items_per_page: 20, //每页显示多少条记录（默认为20条）
                                    callback: function (currPage, jg) {
                                        // ajax请求
                                        setAjax({
                                              url: seleceurl,
                                            type: "GET",
                                            data: {
                                                'pageIndex': currPage,
                                                'pageSize': 20,
                                                'Status': 15002,
                                                'BusinessType': 14002,
                                                'Name': accountname,
                                                'Source': origin,
                                                'CategoryID':industry
                                            }
                                            //"pageIndex=" + currPage + "&pageSize=20&BusinessType=14002&Status=15002&Name=" + accountname + "&Source=" + origin
                                        }, function (data) {
                                            if (data.Result.List != null) {
                                                // 后台返回数据处理
                                                for (var i = 0; i < data.Result.List.length; i++) {
                                                    if (data.Result.List[i].BeginTime) {
                                                        data.Result.List[i].BeginTime = data.Result.List[i].BeginTime.split(" ")[0];
                                                    }
                                                    if (data.Result.List[i].EndTime) {
                                                        data.Result.List[i].EndTime = data.Result.List[i].EndTime.split(" ")[0];
                                                    }
                                                }
                                            }
                                            $('tbody').eq(1).html(ejs.render($('#app2').html(), data));
                                            // 公共功能
                                            apppublicFunction(1)

                                        })
                                    }
                                });

                        } else {
                            $('#pageContainer2').html('<img src="/images/no_data.png" style="display: block;margin: 70px auto;">')
                        }
                        // 公共功能
                        apppublicFunction(1)
                    })
                })
            })
            // 通过
            $('.tab_menu li').eq(2).on('click', function () {
                setAjax({
                      url: seleceurl,
                    type: "GET",
                    data: "pageIndex=1&pageSize=20&BusinessType=14002&Status=15003"+chaxu()
                }, function (data) {
                    if (data.Result.List != null) {
                        // 后台返回数据处理
                        for (var i = 0; i < data.Result.List.length; i++) {
                            if (data.Result.List[i].BeginTime) {
                                data.Result.List[i].BeginTime = data.Result.List[i].BeginTime.split(" ")[0];
                            }
                            if (data.Result.List[i].EndTime) {
                                data.Result.List[i].EndTime = data.Result.List[i].EndTime.split(" ")[0];
                            }
                        }
                    }
                    $('tbody').eq(2).html(ejs.render($('#app3').html(), data));
                    if (data.Result.TotleCount != 0) {
                        var counts = data.Result.TotleCount;
                        //分页
                        $("#pageContainer3").pagination(
                            counts,
                            {
                                items_per_page: 20, //每页显示多少条记录（默认为20条）
                                callback: function (currPage, jg) {
                                    // ajax请求
                                    setAjax({
                                          url: seleceurl,
                                        type: "GET",
                                        data: "pageIndex=" + currPage + "&pageSize=20&Status=15003&BusinessType=14002"+chaxu()
                                    }, function (data) {
                                        if (data.Result.List != null) {
                                            // 后台返回数据处理
                                            for (var i = 0; i < data.Result.List.length; i++) {
                                                if (data.Result.List[i].BeginTime) {
                                                    data.Result.List[i].BeginTime = data.Result.List[i].BeginTime.split(" ")[0];
                                                }
                                                if (data.Result.List[i].EndTime) {
                                                    data.Result.List[i].EndTime = data.Result.List[i].EndTime.split(" ")[0];
                                                }
                                            }
                                        }
                                        $('tbody').eq(2).html(ejs.render($('#app3').html(), data));
                                        // 公共功能
                                        apppublicFunction(2)

                                    })
                                }
                            });

                    } else {
                        $('#pageContainer3').html('<img src="/images/no_data.png" style="display: block;margin: 70px auto;">')
                    }
                    // 公共功能
                    apppublicFunction(2);
                })
                //查询
                $('.but_query').eq(2).off('click').on('click', function () {
                    // 获取行业分类
                    var industry=$('select[name=industry] option:selected').eq(2).attr('dictid');
                    // 获取名称
                    var accountname = $('.state').eq(2).find('li:eq(0) input').val();
                    // 获取来源
                    var origin = $('.state').eq(2).find('li:eq(1) select option:selected').attr('name');
                    setAjax({
                          url: seleceurl,
                        type: "GET",
                        data: {
                            'pageIndex': 1,
                            'pageSize': 20,
                            'BusinessType': 14002,
                            'Status': 15003,
                            'Name': accountname,
                            'Source': origin,
                            'CategoryID':industry
                        }
                        //"pageIndex=1&pageSize=20&BusinessType=14002&Status=15003&Name=" + accountname + "&Source=" + origin,
                    }, function (data) {
                        if (data.Result.List != null) {
                            // 后台返回数据处理
                            for (var i = 0; i < data.Result.List.length; i++) {
                                if (data.Result.List[i].BeginTime) {
                                    data.Result.List[i].BeginTime = data.Result.List[i].BeginTime.split(" ")[0];
                                }
                                if (data.Result.List[i].EndTime) {
                                    data.Result.List[i].EndTime = data.Result.List[i].EndTime.split(" ")[0];
                                }
                            }
                        }
                        $('tbody').eq(2).html(ejs.render($('#app3').html(), data));
                        if (data.Result.TotleCount != 0) {
                            var counts = data.Result.TotleCount;
                            //分页
                            $("#pageContainer3").pagination(
                                counts,
                                {
                                    items_per_page: 20, //每页显示多少条记录（默认为20条）
                                    callback: function (currPage, jg) {
                                        // ajax请求
                                        setAjax({
                                              url: seleceurl,
                                            type: "GET",
                                            data: {
                                                'pageIndex': currPage,
                                                'pageSize': 20,
                                                'BusinessType': 14002,
                                                'Status': 15003,
                                                'Name': accountname,
                                                'Source': origin,
                                                'CategoryID':industry
                                            }
                                            //"pageIndex=" + currPage + "&pageSize=20&BusinessType=14002&Status=15003&Name=" + accountname + "&Source=" + origin
                                        }, function (data) {
                                            $('tbody').eq(2).html(ejs.render($('#app3').html(), data));
                                            // 公共功能
                                            apppublicFunction(2)

                                        })
                                    }
                                });

                        } else {
                            $('#pageContainer3').html('<img src="/images/no_data.png" style="display: block;margin: 70px auto;">')
                        }
                        // 公共功能
                        apppublicFunction(2)
                    })
                })
            })
            // 驳回
            $('.tab_menu li').eq(3).on('click', function () {
                setAjax({
                      url: seleceurl,
                    type: "GET",
                    data: "pageIndex=1&pageSize=20&BusinessType=14002&Status=15004"+chaxu()
                }, function (data) {
                    if (data.Result.List != null) {
                        // 后台返回数据处理
                        for (var i = 0; i < data.Result.List.length; i++) {
                            if (data.Result.List[i].BeginTime) {
                                data.Result.List[i].BeginTime = data.Result.List[i].BeginTime.split(" ")[0];
                            }
                            if (data.Result.List[i].EndTime) {
                                data.Result.List[i].EndTime = data.Result.List[i].EndTime.split(" ")[0];
                            }
                        }
                    }
                    $('tbody').eq(3).html(ejs.render($('#app4').html(), data));
                    if (data.Result.TotleCount != 0) {
                        var counts = data.Result.TotleCount;
                        //分页
                        $("#pageContainer4").pagination(
                            counts,
                            {
                                items_per_page: 20, //每页显示多少条记录（默认为20条）
                                callback: function (currPage, jg) {
                                    // ajax请求
                                    setAjax({
                                          url: seleceurl,
                                        type: "GET",
                                        data: "pageIndex=" + currPage + "&pageSize=20&Status=15004&BusinessType=14002"+chaxu()
                                    }, function (data) {
                                        if (data.Result.List != null) {
                                            // 后台返回数据处理
                                            for (var i = 0; i < data.Result.List.length; i++) {
                                                if (data.Result.List[i].BeginTime) {
                                                    data.Result.List[i].BeginTime = data.Result.List[i].BeginTime.split(" ")[0];
                                                }
                                                if (data.Result.List[i].EndTime) {
                                                    data.Result.List[i].EndTime = data.Result.List[i].EndTime.split(" ")[0];
                                                }
                                            }
                                        }
                                        $('tbody').eq(3).html(ejs.render($('#app4').html(), data));
                                        // 公共功能
                                        apppublicFunction(3)

                                    })
                                }
                            });

                    } else {
                        $('#pageContainer4').html('<img src="/images/no_data.png" style="display: block;margin: 70px auto;">')
                    }
                    // 公共功能
                    apppublicFunction(3);
                })
                //查询
                $('.but_query').eq(3).off('click').on('click', function () {
                    // 获取行业分类
                    var industry=$('select[name=industry] option:selected').eq(3).attr('dictid');
                    // 获取名称
                    var accountname = $('.state').eq(3).find('li:eq(0) input').val();
                    // 获取来源
                    var origin = $('.state').eq(3).find('li:eq(1) select option:selected').attr('name');
                    setAjax({
                          url: seleceurl,
                        type: "GET",
                        data: {
                            'pageIndex': 1,
                            'pageSize': 20,
                            'BusinessType': 14002,
                            'Status': 15004,
                            'Name': accountname,
                            'Source': origin,
                            'CategoryID':industry
                        }
                        //"pageIndex=1&pageSize=20&BusinessType=14002&Status=15004&Name=" + accountname + "&Source=" + origin,
                    }, function (data) {
                        if (data.Result.List != null) {
                            // 后台返回数据处理
                            for (var i = 0; i < data.Result.List.length; i++) {
                                if (data.Result.List[i].BeginTime) {
                                    data.Result.List[i].BeginTime = data.Result.List[i].BeginTime.split(" ")[0];
                                }
                                if (data.Result.List[i].EndTime) {
                                    data.Result.List[i].EndTime = data.Result.List[i].EndTime.split(" ")[0];
                                }
                            }
                        }
                        $('tbody').eq(3).html(ejs.render($('#app4').html(), data));
                        if (data.Result.TotleCount != 0) {
                            var counts = data.Result.TotleCount;
                            //分页
                            $("#pageContainer4").pagination(
                                counts,
                                {
                                    items_per_page: 20, //每页显示多少条记录（默认为20条）
                                    callback: function (currPage, jg) {
                                        // ajax请求
                                        setAjax({
                                              url: seleceurl,
                                            type: "GET",
                                            data: {
                                                'pageIndex': currPage,
                                                'pageSize': 20,
                                                'BusinessType': 14002,
                                                'Status': 15004,
                                                'Name': accountname,
                                                'Source': origin,
                                                'CategoryID':industry
                                            }
                                            //"pageIndex=" + currPage + "&pageSize=20&BusinessType=14002&Status=15004&Name=" + accountname + "&Source=" + origin
                                        }, function (data) {
                                            if (data.Result.List != null) {
                                                // 后台返回数据处理
                                                for (var i = 0; i < data.Result.List.length; i++) {
                                                    if (data.Result.List[i].BeginTime) {
                                                        data.Result.List[i].BeginTime = data.Result.List[i].BeginTime.split(" ")[0];
                                                    }
                                                    if (data.Result.List[i].EndTime) {
                                                        data.Result.List[i].EndTime = data.Result.List[i].EndTime.split(" ")[0];
                                                    }
                                                }
                                            }
                                            $('tbody').eq(3).html(ejs.render($('#app4').html(), data));
                                            // 公共功能
                                            apppublicFunction(3)

                                        })
                                    }
                                });

                        } else {
                            $('#pageContainer4').html('<img src="/images/no_data.png" style="display: block;margin: 70px auto;">')
                        }

                        // 公共功能
                        apppublicFunction(3)
                    })
                })
            })
        }
    }
})
window.onload=function () {
    // app行业分类
    setAjax({
        url:'/api/DictInfo/GetDictInfoByTypeID',
        type:'get',
        data:{
            typeID:22
        }
    },function (data) {
        var str = '';
        var res = data.Result;
        str += '<option DictID="-2">不限</option>';
        for (var i = 0; i < res.length; i++) {
            str += ' <option DictID="' + res[i].DictId + '">' + res[i].DictName + '</option>'
        }
        $('select[name="industry"]').html(str);

        if(GetRequest().categoryId!=undefined){
            var a=true;
            $("select[name=industry] option").each(function () {
                if($(this).attr('dictid')==GetRequest().categoryId){
                    $(this).attr("selected","selected");
                    $('.but_query').click();
                    a=false;
                }
            })

        }
    })


}
