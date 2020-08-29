// 获取url？后面的参数
function GetRequest() {
    var url = location.search; //获取url中"?"符后的字串
    var theRequest = new Object();
    if (url.indexOf("?") != -1) {
        var str = url.substr(1);
        strs = str.split("&");
        for (var i = 0; i < strs.length; i++) {
            theRequest[strs[i].split("=")[0]] = decodeURI(strs[i].split("=")[1]);
        }
    }
    return theRequest;
}
$(function () {
    function PresentManagement() {
        this.init()
    }
    PresentManagement.prototype = {
        constructor: PresentManagement,
        // 获取条件
        ObtainConditions: function (i) {
            // 支付状态
            var OrderStatus = $('.tab_menu .selected').attr('value');
            // 申请人
            var UserName = $('#Applicant').val();
            // 开始时间
            var CreateTime = $('#CreateTime').val();
            // 结束时间
            var endTime = $('#endTime').val();
            var obj = {
                UserName: UserName,
                StartDate: CreateTime,
                EndDate: endTime,
                OrderStatus: OrderStatus,
                PageSize: 20
            }
            if (i != undefined) {
                obj.PageIndex = i
            }
            return obj
        },
        init: function () {
            var _this = this;

            // 点击查询
            $('#Inquiry').off('click').on('click', function () {
                _this.query(_this.ObtainConditions(1), _this)
            })
            // 切换
            $('.tab_menu li').off('click').on('click', function () {
                $(this).attr('class', 'selected').siblings().attr('class', '');
                $('#Inquiry').click();
            })
            if (GetRequest().finish == 1) {
                $('#Applicant').val(GetRequest().UserName)
                if (GetRequest().OrderStatus == 95001) {
                    $('.tab_menu li').eq(0).click();
                } else {
                    $('.tab_menu li').eq(1).click();
                }
            } else {
                $('#Inquiry').click();
            }
        },
        query: function (parameter, _this) {
            setAjax({
                url: public_url + '/api/Withdrawals/GetList',
                // url:'json/PresentManagement.json',
                type: 'get',
                data: parameter
            }, function (data) {
                if (data.Status == 0) {
                    $('.channel').html(ejs.render($('#channel').html(), data));
                    _this.operation()
                    if (data.Result.TotleCount != 0) {
                        var counts = data.Result.TotleCount;
                        //分页
                        $("#pageContainer").pagination(
                            counts,
                            {
                                items_per_page: 20, //每页显示多少条记录（默认为20条）
                                callback: function (currPage, jg) {
                                    parameter.PageIndex = currPage
                                    // ajax请求
                                    setAjax({
                                        url: public_url + '/api/Withdrawals/GetList',
                                        type: "GET",
                                        data: parameter
                                    }, function (data) {
                                        $('.channel').html(ejs.render($('#channel').html(), data));
                                        _this.operation()
                                    })
                                }
                            });

                    } else {
                        $('#pageContainer').html('<img src="/ImagesNew/no_data.png" style="display: block;margin: 70px auto;">')
                    }
                    if (data.Result.Extend) {
                        $('#Cumulative').text('累计金额：' + removePoint0(formatMoney(data.Result.Extend.TotalMoney, 2, ''), 2) + '元')
                    } else {
                        $('#Cumulative').text('')
                    }

                } else {
                    layer.msg(data.Message)
                }
            })
        },
        operation: function () {
            $('.audit').off('click').on('click', function () {
                localStorage.setItem('recid', $(this).attr('recid'))
                $.openPopupLayer({
                    name: "popLayer",
                    url: "/FinanceManager/PresentAuditReview.html",
                    success: function () {
                        var i = false;
                        $('#headWaist').off('click').on('click', function () {
                            if (i) {
                                return false
                            }
                            $.ajax({
                                url: public_url + '/api/Withdrawals/Audit',
                                type: 'post',
                                data: {
                                    WithdrawalsId: localStorage.getItem('recid')
                                },
                                dataType: 'json',
                                xhrFields: {
                                    withCredentials: true
                                },
                                crossDomain: true,
                                success: function (data) {
                                    if (data.Status == 0) {
                                        $.closePopupLayer('popLayer');
                                        location.reload()
                                    } else {
                                        layer.msg(data.Message, { time: 3000 }, function () {
                                            location.reload()
                                        })
                                    }
                                },
                                beforeSend: function () {
                                    i = true
                                }
                            });
                        });
                        $('.closebt').off('click').on('click', function () {
                            $.closePopupLayer('popLayer');
                        })
                    }
                })
            })

            $('.fail').on('mouseover', function () {
                var h = $(this).next().height() - 1;
                var w = $(this).next().width();
                $(this).next().show().css({ 'bottom': -h + 'px', left: '50%', 'margin-left': -w / 2 + 'px' })
            }).on('mouseout', function () {
                $(this).next().hide()
            })
        }
    }
    new PresentManagement()
})