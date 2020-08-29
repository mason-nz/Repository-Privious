$(function () {

    function SendTempMsg() {
        this.init();
    }

    SendTempMsg.prototype = {
        constructor: SendTempMsg,
        //加载模板信息
        init: function () {
            var _this = this;
            setAjax({
                url: public_url + '/api/WeChatManage/GetWxTempData',
                type: 'get'
                //data: parameter
            }, function (data) {
                //Console.log(data);
                if (data && data.Status == 0) {
                    _this.bindData(data.Result);
                } else {
                    layer.msg(data.Message);
                }

            });
        },

        //绑定下拉列表等查询条件
        bindData: function (result) {
            var _this = this;
            $('#ddlWxNum,#ddlWxTemp').empty();
            $('#ddlWxNum,#ddlWxTemp').append("<option value='-1' selected='selected'>请选择</option>");

            //绑定公众号下拉列表数据
            for (var wx in result) {
                var val = result[wx].AppId;
                var name = result[wx].WxNum;
                $('#ddlWxNum').append("<option value='" + val + "' >" + name + "</option>");
                $('#ddlWxNum').data(val, result[wx].TempList);
            }

            //公众号下拉列表change事件
            $('#ddlWxNum').change(function () {
                var appid = $(this).val();
                $('#ddlWxTemp').empty();
                $('#ddlWxTemp').append("<option value='-1' selected='selected'>请选择</option>");
                var tempData = $('#ddlWxNum').data(appid);
                if (tempData && tempData.length > 0) {
                    for (var t in tempData) {
                        var val = tempData[t].Id;
                        var name = tempData[t].Title;;
                        $('#ddlWxTemp').append("<option value='" + val + "' >" + name + "</option>");
                        $('#ddlWxTemp').data(val, tempData[t]);
                    }
                }
            });

            //模板下拉列表change事件
            $('#ddlWxTemp').change(function () {
                var tempId = $(this).val();
                $('#divTempContent,#divTempDesc').html('');
                var tempData = $('#ddlWxTemp').data(tempId);
                if (tempData) {
                    $('#divTempContent').html(tempData.Content);
                    $('#divTempDesc').html(tempData.Desc);

                    $('#divPara').html('');
                    var paras = tempData.Paras.split(',');
                    if (paras && paras.length > 0) {
                        $.each(paras, function (i, n) {
                            var spanObj = $('<span>').text(n + ":");
                            var textObj = $('<input>').attr('type', 'text')
                                        .attr('id', tempId + '_' + n)
                                        .attr('paraName', n).width(500);
                            $('#divPara').append(spanObj).append(textObj).append('<br/>');
                        });
                    }
                }
            });

            //绑定发送按钮
            $('#liSend').unbind('click').bind('click', function () {
                if (_this.verifyLogic()) {
                    //layer.msg('OK');
                    var appId = $('#ddlWxNum').val();
                    var tempId = $('#ddlWxTemp').val();
                    var tempUrl = $('#txtTempUrl').val();
                    var openIds = $('#txtOpenIds').val();

                    var parameter = {};
                    parameter.AppId = appId;
                    parameter.WxTempId = tempId;
                    parameter.WxTempUrl = tempUrl;
                    parameter.SendOpenIds = openIds;
                    var tempPara = {};
                    $('#divPara :text[paraName]').each(function () {
                        var paraName = $(this).attr('paraName');
                        var textContent = $(this).val();
                        tempPara[paraName] = textContent;
                    });
                    parameter.WxTempParas = JSON.stringify(tempPara);

                    $('#liSend').attr('disabled', 'disabled');
                    setAjax({
                        url: public_url + '/api/WeChatManage/SendWxTempData',
                        type: 'post',
                        data: parameter
                    }, function (data) {
                        //Console.log(data);
                        $('#liSend').removeAttr('disabled');
                        var spanOjb = $('#spanResultMsg');
                        if (data && data.Status == 0) {
                            layer.msg("发送模板操作成功！");
                            spanOjb.append('<br/>发送模板操作成功！');
                        } else {
                            layer.msg(data.Message);
                            spanOjb.append('<br/>' + data.Message);
                        }

                    });
                }
            });


            //绑定异步发送按钮
            $('#liSendAsync').unbind('click').bind('click', function () {
                if (_this.verifyLogic()) {
                    //layer.msg('OK');
                    var appId = $('#ddlWxNum').val();
                    var tempId = $('#ddlWxTemp').val();
                    var tempUrl = $('#txtTempUrl').val();
                    var openIds = $('#txtOpenIds').val();

                    var parameter = {};
                    parameter.AppId = appId;
                    parameter.WxTempId = tempId;
                    parameter.WxTempUrl = tempUrl;
                    parameter.SendOpenIds = openIds;
                    var tempPara = {};
                    $('#divPara :text[paraName]').each(function () {
                        var paraName = $(this).attr('paraName');
                        var textContent = $(this).val();
                        tempPara[paraName] = textContent;
                    });
                    parameter.WxTempParas = JSON.stringify(tempPara);

                    $('#liSend').attr('disabled', 'disabled');
                    setAjax({
                        url: public_url + '/api/WeChatManage/SendWxTempDataAsync',
                        type: 'post',
                        data: parameter
                    }, function (data) {
                        //Console.log(data);
                        $('#liSend').removeAttr('disabled');
                        var spanOjb = $('#spanResultMsg');
                        if (data && data.Status == 0) {
                            layer.msg("发送模板操作成功！");
                            spanOjb.append('<br/>发送模板操作成功！');
                        } else {
                            layer.msg(data.Message);
                            spanOjb.append('<br/>' + data.Message);
                        }

                    });
                }
            });
        },

        //验证发送逻辑
        verifyLogic: function () {
            var flag = false;
            var appId = $('#ddlWxNum').val();
            var tempId = $('#ddlWxTemp').val();
            //var tempUrl = $('#txtTempUrl').val();
            var openIds = $('#txtOpenIds').val();
            if (appId == '-1') {
                layer.msg('微信公众号必须选择一项');
            }
            else if (tempId == '-1') {
                layer.msg('模板必须选择一项');
            }
            else if (openIds == '') {
                layer.msg('必须输入要发送的人（OpenID）');
            }
            else {
                var hasEmpty = false;
                $('#divPara :text[paraName]').each(function () {
                    var textContent = $(this).val();
                    if ($.trim(textContent) == '') {
                        hasEmpty = true;
                        return false;
                    }
                });
                if (hasEmpty) {
                    layer.msg('参数内容还有为空的情况，请输入具体参数值');
                } else {
                    flag = true;
                }
            }

            return flag;
        }


    }
    new SendTempMsg();
})