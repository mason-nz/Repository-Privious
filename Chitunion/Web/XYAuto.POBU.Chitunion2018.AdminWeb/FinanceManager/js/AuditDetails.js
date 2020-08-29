/// <reference path="../../js/jquery.1.11.3.min.js" />
/// <reference path="../../js/Common_chitu.js" />
/// <reference path="../../js/layer/layer.js" />
/// <reference path="../../js/ejs.min.js" />

/*
auth:lixiong
date:2018-02-05
desc:资源管理后台-提现审核详情页面
*/

"use strict";

(function ($) {
    $.AuditWith = {};
    $.extend($.AuditWith, {
        ajaxQueryUrls: {
            queryAuditDetails: "/api/Withdrawals/GetAuditDetails",
            postAudit: "/api/Withdrawals/Audit"
        },
        ajaxPostAuditArgs: {
            WithdrawalsId: 0,
            AuditStatus: 0
        },
        constAuditStatus: {
            "Pass": 197001,
            "NotPass": 197002,
            "Pending": 197003
        },
        btnSubmitAudit: $("#SubmitAudit"),
        init: function (options) {

            $.extend($.AuditWith, options);

            var request = $.AuditWith.commonFunction.GetRequest();
            $.AuditWith.ajaxFunction.queryAuditDetails(request.recid);
        },
        ajaxFunction: {
            queryAuditDetails: function (recId) {
                setAjax({
                    url: public_url + $.AuditWith.ajaxQueryUrls.queryAuditDetails,
                    type: "GET",
                    data: { WithdrawalsId: recId }
                }, function (data) {
                    if (data.Status === 0) {
                        $('.aud').html(ejs.render($('#aud').html(), data));
                        $.AuditWith.ajaxFunction.queryAuditDetailsCallBack(recId, data.Result.AuditStatus);
                        $.AuditWith.ajaxFunction.bindInsideEmpolyee(data.Result.IsInsideEmployee);
                    } else {
                        layer.msg(data.Message);
                    }
                });
            },
            bindInsideEmpolyee: function (isInsideEmployee) {
                if (isInsideEmployee) {
                    $(':radio[name="shenhe"][i="197001"]').removeAttr('checked').attr('disabled', 'disabled');
                    $(':radio[name="shenhe"][i="197002"]').trigger('click');
                    $('#txtRejectMsg').text('因存在财务税务风险，行圆内部员工不可在赤兔联盟后台提现，具体流程请联系吕琳燕，15650708968');
                }
            },
            queryAuditDetailsCallBack: function (recid, auditStatus) {
                //带审核才显示出驳回按钮
                if (auditStatus == $.AuditWith.constAuditStatus.Pending) {
                    $('input[name="shenhe"]').on("click", function () {
                        if ($(this).attr("i") == 197002) {
                            $("#parameter").show();
                        } else {
                            $("#parameter").hide();
                        }
                    });
                }

                //提交审核
                $("#SubmitAudit").on("click", function () {
                    var postArgs = $.AuditWith.commonFunction.GetPostAuditParams(recid);
                    if (postArgs.AuditStatus == 197002 && (postArgs.RejectMsg == null || postArgs.RejectMsg.length === 0)) {
                        layer.msg("请输入驳回原因");
                        return;
                    }
                    $.AuditWith.ajaxFunction.postAudit(postArgs);
                });
            },
            postAudit: function (parameter) {
                layer.load(1); //风格1的加载
                setAjax({
                    url: public_url + $.AuditWith.ajaxQueryUrls.postAudit,
                    type: "POST",
                    data: parameter
                }, function (data) {
                    layer.closeAll();
                    if (data.Status === 0) {
                        layer.open({
                            content: '审核成功',
                            success: function (layero, index) {
                                window.location.href = "/financemanager/presentmanagement-1.html?AuditStatus=197003";
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
            GetPostAuditParams: function (recId) {
                var obj = {
                    WithdrawalsId: recId,
                    AuditStatus: $('input[name="shenhe"]:checked').attr("i"),
                    RejectMsg: $("#txtRejectMsg").val() || "无"
                }

                return obj;
            }
        }
    });
})(jQuery);