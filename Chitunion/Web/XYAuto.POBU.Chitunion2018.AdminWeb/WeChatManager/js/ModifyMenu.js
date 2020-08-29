$(function () {

    function ModifyMenuData() {
        this.init();
    }

    ModifyMenuData.prototype = {
        constructor: ModifyMenuData,
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
                $('#ddlWxNum').data(val, result[wx].MenuClickList);
            }

            //一级菜单下拉列表change事件
            $('#divMenu div[name="divRootMenu"] select[name="ddlMenuType"]').change(function () {
                var val = $(this).val();
                var currentTextObj = $(this).nextAll('input[type="text"][name="txtMenuVal"]');
                var obj = $(this).nextAll('div[name="divSubMenu"]').find('input[type="text"],select');
                if (val != "subMenu") {
                    currentTextObj.removeAttr('disabled');
                    obj.attr('disabled', 'disabled');
                } else {
                    currentTextObj.attr('disabled', 'disabled');
                    obj.removeAttr('disabled');
                }
            });

            ////公众号下拉列表change事件
            $('#ddlWxNum').change(function () {
                var appid = $(this).val();
                $('#divMenu div[name="divRootMenu"] input[type="text"]').val('');
                $('#divMenu div[name="divRootMenu"] select').val(-1);
                //var menuClickData = $('#ddlWxNum').data(appid);
                setAjax({
                    url: public_url + '/api/WeChatManage/GetMenuData?appid=' + appid,
                    type: 'get'
                    //data: parameter
                }, function (data) {
                    //Console.log(data);
                    if (data && data.Status == 0) {
                        //console.log(data.Result);
                        setAjax({
                            url: public_url + '/api/WeChatManage/GetMenuData',
                            type: 'get'
                            //data: parameter
                        }, function (data2) {
                            //Console.log(data);
                            if (data2 && data2.Status == 0) {
                                for (var wx in data2.Result) {
                                    var val = data2.Result[wx].AppId;
                                    if (appid == val) {
                                        _this.bindMenuData(data.Result, data2.Result[wx].MenuClickList);
                                        return;
                                    }
                                }
                            } else {
                                layer.msg(data.Message);
                            }
                        });


                    } else {
                        layer.msg(data.Message);
                    }

                });
            });

            //绑定保存菜单按钮
            $('#liSaveMenuData').unbind('click').bind('click', function () {
                if (_this.verifyLogic()) {
                    var parameter = {};
                    parameter.AppId = $('#ddlWxNum').val();
                    parameter.Buttons = [];

                    $.each($('#divMenu div[name="divRootMenu"]'), function (i, n) {
                        var _this = $(this);
                        var name = _this.find('input[type="text"][name="txtMenuName"]').val().trim();
                        var type = _this.find('select[name="ddlMenuType"]').val();
                        var val = _this.find('input[type="text"][name="txtMenuVal"]').val().trim();

                        if (type != '-1') {
                            var button = {};
                            button.Name = name;
                            button.Type = type;
                            button.Level = 1;
                            if (type == "view") {
                                button.Value = val;
                            }
                            else if (type == "click") {
                                button.MediaId = val;
                                button.Value = '[' + button.Level + ']' + name;
                            }
                            button.SubButtons = [];

                            $.each(_this.find('div[name="divSubMenu"]'), function (j, m) {
                                var _this = $(this);
                                var name = _this.find('input[type="text"][name="txtSubMenuName"]').val().trim();
                                var type = _this.find('select[name="ddlSubMenuType"]').val();
                                var val = _this.find('input[type="text"][name="txtSubMenuVal"]').val().trim();
                                if (type != '-1') {
                                    var subbutton = {};
                                    subbutton.Name = name;
                                    subbutton.Type = type;
                                    subbutton.Level = 2;
                                    if (type == "view") {
                                        subbutton.Value = val;
                                    }
                                    else if (type == "click") {
                                        subbutton.MediaId = val;
                                        subbutton.Value = '[' + subbutton.Level + ']' + name;
                                    }
                                    button.SubButtons.push(subbutton);
                                }
                            });
                            parameter.Buttons.push(button);

                        }
                    });
                    console.log(parameter);

                    setAjax({
                        url: public_url + '/api/WeChatManage/SaveMenuData',
                        type: 'post',
                        data: parameter
                    }, function (data) {
                        //Console.log(data);
                        $('#liSaveMenuData').removeAttr('disabled');
                        var spanOjb = $('#spanResultMsg');
                        if (data && data.Status == 0) {
                            layer.msg("微信菜单数据，保存成功！");
                            spanOjb.append('<br/>微信菜单数据，保存成功！');
                        } else {
                            layer.msg(data.Message);
                            spanOjb.append('<br/>' + data.Message);
                        }

                    });
                }
            });

            //绑定清空菜单按钮
            $('#liClearMenuData').unbind('click').bind('click', function () {
                var appId = $('#ddlWxNum').val();
                if (appId == '-1') {
                    layer.msg("微信公众号必须选择一项");
                    return;
                }
                setAjax({
                    url: public_url + '/api/WeChatManage/ClearMenuData',
                    type: 'post',
                    data: { 'AppId': appId }
                }, function (data) {
                    //Console.log(data);
                    $('#liClearMenuData').removeAttr('disabled');
                    var spanOjb = $('#spanResultMsg');
                    if (data && data.Status == 0) {
                        $('#divMenu div[name="divRootMenu"] input[type="text"]').val('');
                        $('#divMenu div[name="divRootMenu"] select').val(-1);
                        layer.msg("微信菜单数据，清空成功！");
                        spanOjb.append('<br/>微信菜单数据，清空成功！');
                    } else {
                        layer.msg(data.Message);
                        spanOjb.append('<br/>' + data.Message);
                    }

                });
            });
        },
        //根据Click类型的Key，查询MediaId
        queryMeadiaIdByKey: function (menuClickData, key) {
            if (menuClickData && menuClickData.length > 0) {
                for (var wx in menuClickData) {
                    if (menuClickData[wx].EventKey == key) {
                        return menuClickData[wx].MediaId;
                    }
                }
            }
            return '';
        },

        //根据公众号下拉列表，绑定微信菜单数据
        bindMenuData: function (result, menuClickData) {
            var _this = this;
            for (var i = 0; i < result.length; i++) {
                var name = result[i].name;
                var menuObj = $('#divMenu div[name="divRootMenu"]').eq(i);
                menuObj.find('input[type="text"][name="txtMenuName"]').val(name);

                var type = result[i].type;
                var key = result[i].key;
                menuObj.find('select[name="ddlMenuType"]').val(type);
                if (type == 'click') {
                    menuObj.find('input[type="text"][name="txtMenuVal"]').val(_this.queryMeadiaIdByKey(menuClickData, key));
                } else {
                    menuObj.find('input[type="text"][name="txtMenuVal"]').val(result[i].url);
                }

                var subObj = result[i].sub_button;
                if (subObj && subObj.length > 0) {
                    menuObj.find('select[name="ddlMenuType"]').val('subMenu');
                    menuObj.find('select[name="ddlMenuType"]').trigger("change", "subMenu");
                    for (var j = 0; j < subObj.length; j++) {
                        var subName = subObj[j].name;
                        var subType = subObj[j].type;
                        var subUrl = subObj[j].url;
                        var subKey = subObj[j].key;
                        var subMenuObj = menuObj.find('div[name="divSubMenu"]').eq(j);
                        subMenuObj.find('input[type="text"][name="txtSubMenuName"]').val(subName);
                        subMenuObj.find('select[name="ddlSubMenuType"]').val(subType);
                        if (subType == 'click') {
                            subMenuObj.find('input[type="text"][name="txtSubMenuVal"]').val(_this.queryMeadiaIdByKey(menuClickData, subKey));
                        } else {
                            subMenuObj.find('input[type="text"][name="txtSubMenuVal"]').val(subUrl);
                        }
                    }
                }
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
                var rootMenu = false;
                $('#divMenu div[name="divRootMenu"] > select').val(function () {
                    var val = this.value;
                    if (val != '-1')
                        rootMenu = rootMenu || true;
                    return val;
                });
                if (!rootMenu) {
                    layer.msg('微信公众号，至少要设置一个一级菜单');
                } else {
                    flag = true;
                }
            }

            return flag;
        }


    }
    new ModifyMenuData();
})