$(function () {

    function ModifyCustomMsg() {
        this.init();
    }

    ModifyCustomMsg.prototype = {
        constructor: ModifyCustomMsg,
        //加载模板信息
        init: function () {
            var _this = this;
            setAjax({
                url: public_url + '/api/WeChatManage/GetMenuData',
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
            $('#ddlWxNum').empty();
            $('#ddlWxNum').append("<option value='-1' selected='selected'>请选择</option>");

            //绑定公众号下拉列表数据
            for (var wx in result) {
                var val = result[wx].AppId;
                var name = result[wx].WxNum;
                $('#ddlWxNum').append("<option value='" + val + "' >" + name + "</option>");
                $('#ddlWxNum').data(val, result[wx]);
            }

            //绑定“添加自定义消息”按钮事件
            $('#btnAppendCustomItem').unbind('click').bind('click', function () {
                _this.appendKeyItemData();
            });

            ////公众号下拉列表change事件
            $('#ddlWxNum').change(function () {
                var appid = $(this).val();
                _this.clearData();
                if (appid != "-1") {
                    var jsonObj = $(this).data(appid);
                    if (jsonObj && jsonObj.SubscribeMsg) {
                        $('#txtSubscribeMsg').val(jsonObj.SubscribeMsg);
                        $('#txtDefaultCustomMsg').val(jsonObj.DefaultCustomMsg);
                        //初始化，关键词配置信息
                        _this.loadCustomKeyData(jsonObj.CustomMsgList);
                    }
                }
            });

            //绑定保存菜单按钮
            $('#liSaveCustomMsg').unbind('click').bind('click', function () {
                if (_this.verifyLogic()) {
                    var parameter = {};
                    parameter.AppId = $('#ddlWxNum').val();
                    parameter.SubscribeMsg = $('#txtSubscribeMsg').val();
                    parameter.DefaultCustomMsg = $('#txtDefaultCustomMsg').val();
                    parameter.CustomMsgList = [];

                    $.each($('#divCustomData div[name="divCustomItem"]'), function (i, n) {
                        var _this = $(this);
                        var customKey = _this.find('input[type="text"][name="txtCustomKey"]').val();
                        var type = _this.find('select[name="ddlCustomType"]').val();
                        var val = _this.find('textarea[name="txtCustomMsg"]').val();

                        if (type != '-1') {
                            var button = {};
                            button.Key = $.trim(customKey);
                            button.Type = type;
                            button.Value = $.trim(val);
                            parameter.CustomMsgList.push(button);
                        }
                    });
                    //添加关注后，（1元提现）新用户发送文章消息通知
                    parameter.SubArticleInfo = {
                        Title: "1元红包待领取，逾期未领取将作废",
                        Description: "已有9万人免费领到了！还在等什么，快去领取吧！",
                        PicUrl: "http://admin.chitunion.com/images/hd/1yuantx_banner.jpg",
                        Url: "/moneyManager/index.html"
                    }
                    console.log(parameter);

                    setAjax({
                        url: public_url + '/api/WeChatManage/ModifyCustomData',
                        type: 'post',
                        data: parameter
                    }, function (data) {
                        //Console.log(data);
                        $('#liSaveCustomMsg').removeAttr('disabled');
                        var spanOjb = $('#spanResultMsg');
                        if (data && data.Status == 0) {
                            layer.msg("微信关注回复、自定义关键词数据，保存成功！");
                            spanOjb.append('<br/>微信关注回复、自定义关键词数据，保存成功！');
                            $('#ddlWxNum').data(parameter.AppId, parameter);
                        } else {
                            layer.msg(data.Message);
                            spanOjb.append('<br/>' + data.Message);
                        }

                    });
                }
            });

            //绑定清空菜单按钮
            $('#liClearCustomMsg').unbind('click').bind('click', function () {
                var appId = $('#ddlWxNum').val();
                if (appId == '-1') {
                    layer.msg("微信公众号必须选择一项");
                    return;
                }
                setAjax({
                    url: public_url + '/api/WeChatManage/ClearCustomData',
                    type: 'post',
                    data: { 'AppId': appId }
                }, function (data) {
                    //Console.log(data);
                    $('#liClearCustomMsg').removeAttr('disabled');
                    var spanOjb = $('#spanResultMsg');
                    if (data && data.Status == 0) {
                        _this.clearData();
                        layer.msg("微信关注回复、自定义关键词数据，清空成功！");
                        spanOjb.append('<br/>微信关注回复、自定义关键词数据，清空成功！');
                        $('#ddlWxNum').removeData(appId);
                    } else {
                        layer.msg(data.Message);
                        spanOjb.append('<br/>' + data.Message);
                    }

                });
            });
        },
        //清空当前页面中，当前公众号下的数据
        clearData: function () {
            $('#txtSubscribeMsg').val('');
            $('#txtDefaultCustomMsg').val('');
            $('#divCustomData').empty();
        },

        //加载自定义关键词数据
        loadCustomKeyData: function (customMsgList) {
            var _this = this;
            if (customMsgList != null) {
                $.each(customMsgList, function (i, n) {
                    _this.appendKeyItemData();
                    var divObj = $('#divCustomData div[name="divCustomItem"]:last');
                    divObj.find('input[type="text"][name="txtCustomKey"]').val(n.Key);
                    divObj.find('select[name="ddlCustomType"]').val(n.Type);
                    divObj.find('textarea[name="txtCustomMsg"]').text(n.Value);
                });
            }
        },

        //在当前行下面，新增一行
        appendKeyItemData: function (currentObj) {
            var _this = this;
            var divObj = $('<div>').attr('name', 'divCustomItem');
            divObj.append('<br/>');
            divObj.append($('<label>').text('请输入关键词：'))
                .append($('<input>').attr('type', 'text').attr('name', 'txtCustomKey').width(100))
                .append($('<label>').text('类型：'))
                .append($('<select>').attr('name', 'ddlCustomType')
                    .append($('<option>').attr('value', '-1').attr('selected', 'selected').text('请选择'))
                    .append($('<option>').attr('value', 'msg').text('文本'))
                    .append($('<option>').attr('value', 'media').text('图片')))
                .append($('<label>').text('回复消息内容为：'))
                .append($('<textarea>').attr('name', 'txtCustomMsg')
                    .attr('cols', '50').attr('rows', '3'))
                .append($('<input>').attr('type', 'button').attr('value', '追加')
                                    .unbind('click').bind('click', function () {
                                        _this.appendKeyItemData($(this));
                                    }))
                .append($('<input>').attr('type', 'button').attr('value', '删除')
                                    .unbind('click').bind('click', function () {
                                        if (confirm('是否要删除当前行？')) {
                                            $(this).parent().remove();
                                        }
                                    }))
                .append('<br/>');
            if (currentObj) {
                currentObj.parent().after(divObj);
            } else {
                $('#divCustomData').append(divObj);
            }
        },



        //验证发送逻辑
        verifyLogic: function () {
            var flag = false;
            var appId = $('#ddlWxNum').val();
            //var tempUrl = $('#txtTempUrl').val();
            if (appId == '-1') {
                layer.msg('微信公众号必须选择一项');
            }
            else {
                var flag2 = false;
                $.each($('#divCustomData div[name="divCustomItem"]'), function (i, n) {
                    var _this = $(this);
                    var customKey = _this.find('input[type="text"][name="txtCustomKey"]').val();
                    var type = _this.find('select[name="ddlCustomType"]').val();
                    var val = _this.find('textarea[name="txtCustomMsg"]').val();

                    if (type == '-1') {
                        layer.msg('第' + (i + 1) + '行，自定义关键词的类型，必须选择一项');
                        flag2 = true;
                        return false;
                    }
                    else if ($.trim(customKey) == '') {
                        layer.msg('第' + (i + 1) + '行，自定义关键词，不能为空');
                        flag2 = true;
                        return false;
                    }
                    else if ($.trim(val) == '') {
                        layer.msg('第' + (i + 1) + '行，自定义关键词的值，不能为空');
                        flag2 = true;
                        return false;
                    }
                });
                if (!flag2) {
                    flag = true;
                }
            }
            return flag;
        }


    }
    new ModifyCustomMsg();
})