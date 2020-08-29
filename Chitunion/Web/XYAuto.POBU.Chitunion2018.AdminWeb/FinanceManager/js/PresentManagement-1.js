/// <reference path="../../js/jquery.1.11.3.min.js" />
/// <reference path="../../js/Common_chitu.js" />
/// <reference path="../../js/layer/layer.js" />
/// <reference path="../../js/ejs.min.js" />

/*
auth:lixiong
date:2018-02-02
desc:资源管理后台-提现管理页面
*/

"use strict";

(function ($) {
    $.withdws = {};
    $.extend($.withdws, {
        btnInquiry: $("#Inquiry"),
        lbCumulative: $("#Cumulative"),
        applicant: $("#Applicant"),
        selPayStatus: $("#selPayStatus"),
        liPayDate: $("#Li_PayDate"),
        tabMenuLi: $(".tab_menu li"),
        ajaxQueryUrls: {
            queryWithdrawalsList: "/api/Withdrawals/GetList",
            postAudit: "/api/Withdrawals/Audit"
        },
        constAuditStatus: {
            "Pass": 197001,
            "NotPass": 197002,
            "Pending": 197003
        },
        constPayStatus: {
            "PaySuccess": 195003
        },
        init: function (options) {
            //初始化参数
            $.extend($.withdws, options);

            //页面初始化查询
            var query = $.withdws.commonFunction.GetAjaxQueryArgs(1);
            var request = $.withdws.commonFunction.GetRequest();
            if (request.finish == 1) {
                $.withdws.applicant.val(request.UserName);
                //接收参数处理逻辑()
                if (request.AuditStatus == $.withdws.constAuditStatus.Pass) {
                    //如果是审核通过，把支付状态置为【支付成功】
                    $.withdws.selPayStatus.val($.withdws.constPayStatus.PaySuccess);
                    $.withdws.selPayStatus.show();
                    $.withdws.liPayDate.show();
                } else {
                    $.withdws.selPayStatus.hide();
                    $.withdws.liPayDate.hide();
                }
                $("#li_" + request.AuditStatus).attr('class', 'selected').siblings().attr('class', '');
                query.AuditStatus = request.AuditStatus || $.withdws.constAuditStatus.Pending;
                query.UserName = request.UserName;
                $.withdws.ajaxFunction.ajaxQueryList(query);
            } else {
                $.withdws.ajaxFunction.ajaxQueryList(query);;
            }

            // 点击查询
            $.withdws.btnInquiry.off('click').on('click', function () {
                var query = $.withdws.commonFunction.GetAjaxQueryArgs(1);
                $.withdws.ajaxFunction.ajaxQueryList(query);
            });
        
            // 切换tab
            $.withdws.tabMenuLi.off('click').on('click', function () {
                $(this).attr('class', 'selected').siblings().attr('class', '');
                if ($(this).attr('value') == $.withdws.constAuditStatus.Pass) {
                    //审核通过，有【支付结果】的筛选条件
                    $.withdws.selPayStatus.show();
                    $.withdws.liPayDate.show();
                } else {
                    $.withdws.selPayStatus.hide();
                    $.withdws.liPayDate.hide();
                }
                $('#PayStatus').val(-2);
                $("#Li_PayDate input").val("");
                $.withdws.btnInquiry.click();
            });
            //下载数据
            $('#ExportData').off('click').on('click', function () {
                var query = $.withdws.commonFunction.GetAjaxQueryArgs(1);
                var that = $(this);
                var _url = public_url + '/api/ExcelOperation/ExportResources.aspx?Export=presentmanage&UserName=' + query.UserName + '&StartDate=' + query.StartDate + '&EndDate=' + query.EndDate + '&AuditStatus=' + query.AuditStatus + '&OrderStatus=' + query.OrderStatus + '&BeginPayDate=' + query.BeginPayDate + '&EndPayDate=' + query.EndPayDate;
                that.attr('href', _url);
            })
        },
        ajaxFunction: {
            ajaxQueryList: function (parameter) {
                setAjax({
                    url: public_url + $.withdws.ajaxQueryUrls.queryWithdrawalsList,
                    // url:'json/PresentManagement.json',
                    type: 'GET',
                    data: parameter
                }, function (data) {
                    if (data.Status === 0) {
                        var ejsHtmlChannelObj = $('#channel_' + parameter.AuditStatus);
                        $('.channel').html(ejs.render(ejsHtmlChannelObj.html(), data));

                        $.withdws.commonFunction.LoadMouseoverFailMsg();
                        if (data.Result.TotleCount !== 0) {
                            var counts = data.Result.TotleCount;
                            //分页
                            $("#pageContainer").pagination(
                                counts,
                                {
                                    items_per_page: 20, //每页显示多少条记录（默认为20条）
                                    callback: function (currPage, jg) {
                                        parameter.PageIndex = currPage;
                                        // ajax请求
                                        setAjax({
                                            url: public_url + $.withdws.ajaxQueryUrls.queryWithdrawalsList,
                                            type: "GET",
                                            data: parameter
                                        }, function (data) {
                                            $('.channel').html(ejs.render(ejsHtmlChannelObj.html(), data));
                                            $.withdws.commonFunction.LoadMouseoverFailMsg();
                                        });
                                    }
                                });

                        } else {
                            $('#pageContainer').html('<img src="/ImagesNew/no_data.png" style="display: block;margin: 70px auto;">')
                        }
                        if (data.Result.Extend) {
                            $.withdws.lbCumulative.text('累计金额：' + removePoint0(formatMoney(data.Result.Extend.TotalMoney, 2, ''), 2) + '元')
                        } else {
                            $.withdws.lbCumulative.text("");
                        }

                    } else {
                        layer.msg(data.Message);
                    }
                });
            },
            postAudit: function (recId) {
                layer.load(1); //风格1的加载
                var parameter = {
                    WithdrawalsId: recId,
                    AuditStatus: $.withdws.constAuditStatus.Pass,
                    RejectMsg: "无"
                };
                setAjax({
                    url: public_url + $.withdws.ajaxQueryUrls.postAudit,
                    type: "POST",
                    data: parameter
                }, function (data) {
                    layer.closeAll();
                    if (data.Status === 0) {
                        layer.open({
                            content: '审核成功',
                            success: function (layero, index) {
                                window.location.href = "/financemanager/presentmanagement-1.html?finish=1&AuditStatus=197001";
                            }
                        });
                    } else {
                        layer.open({
                            content: data.Message,
                            success: function (layero, index) {
                            }
                        });
                    }
                });
            }
        },
        commonFunction: {
            // 获取url？后面的参数
            GetRequest: function () {
                var url = location.search; //获取url中"?"符后的字串
                var theRequest = new Object();
                if (url.indexOf("?") != -1) {
                    var str = url.substr(1);
                    var strs = str.split("&");
                    for (var i = 0; i < strs.length; i++) {
                        theRequest[strs[i].split("=")[0]] = decodeURI(strs[i].split("=")[1]);
                    }
                }
                return theRequest;
            },
            GetAjaxQueryArgs: function (page) {
                // 审核状态
                var auditStatus = $('.tab_menu .selected').attr('value');
                // 支付状态
                var payStatus = $('#PayStatus').val() || -2;
                // 申请人
                var userName = $('#Applicant').val();
                // 开始时间
                var createTime = $('#CreateTime').val();
                // 结束时间
                var endTime = $('#endTime').val();
                // 支付开始时间
                var beginPayDate = $('#BeginPayDate').val();
                // 支付结束时间
                var endPayDate = $('#EndPayDate').val();
                var obj = {
                    UserName: userName,
                    StartDate: createTime,
                    EndDate: endTime,
                    AuditStatus: auditStatus,
                    OrderStatus: payStatus,
                    BeginPayDate: beginPayDate,
                    EndPayDate: endPayDate,
                    PageSize: 20
                }
                if (page != undefined) {
                    obj.PageIndex = page;
                }
               
                return obj;
            },
            LoadMouseoverFailMsg: function () {
                $('.fail').on('mouseover', function () {
                    var h = $(this).next().height() - 1;
                    var w = $(this).next().width();
                    console.log("next:", $(this).next());
                    $(this).next().show().css({ 'bottom': -h + 'px', left: '50%', 'margin-left': -w / 2 + 'px' });
                }).on('mouseout', function () {
                    $(this).next().hide();
                });
            }
        }
    });

})(jQuery);


