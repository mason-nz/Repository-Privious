var nickname = "";
 var setval=null
$.ajax({
    url: public_url + '/api/UserManage/GetExamineStatus',
    dataType: 'json',
    type: 'get',
    data: {},
    xhrFields: {
        withCredentials: true
    },
    crossDomain: true,
    async: false,
    success: function (data) {
        if (data.Status == 0) {
            nickname = data.Result.NickName;
        } else {
            layer.open({
                content: data.Message
                , skin: 'msg'
                , time: 2 //2秒后自动关闭
            });
        }
    }
})
$(function () {
    wx.ready(function () {
        wx.getNetworkType({
            success: function (res) {
                var networkType = res.networkType; // 返回网络类型2g，3g，4g，wifi
                $('.netOk').show().siblings('.no_data').remove();
            },
            fail: function () {
                $('.no_data').show().siblings('.netOk').remove();
                return false;
            }
        });
    })
    var app = new Vue({
        el: '#app',
        data: {
            // 顶部提示
            header_hide: 'header_hide',
            header_text: '信息审核中，不可修改',
            header_click: 1,
            // 微信昵称
            nickname: nickname,
            //手机号

            MobilePhone: '',
            // 验证码
            VerificationCode: '',
            // 验证码颜色
            ntifyingCode: '',
            //类型颜色
            typecolourblue: 'blue',
            typecolour: '',
            // 个人系列
            personal: true,
            // 真实姓名
            RealName: '',
            // 身份证号
            IdNumber: '',
            // 手持身份证
            photoimg: '',
            // 企业系列
            enterprise: false,
            // 公司名称
            CorporateName: '',
            // 营业执照
            BLicenceURL: '',
            // 支付宝账号
            Alipay: ''
        },
        created() {
            this.useriformation();
        },
        methods: {
            // 获取用户信息
            useriformation() {
                var _this = this;
                $.ajax({
                    url: public_url + '/api/UserManage/QueryUserBasicInfo',
                    type: 'get',
                    dataType: 'json',
                    data: {},
                    xhrFields: {
                        withCredentials: true
                    },
                    crossDomain: true,
                    success: function (data) {
                        if (data.Status == 0) {
                            _this.MobilePhone = data.Result.BasicInfo.Mobile
                            if (data.Result.AuthenticationInfo.Type == 1001) {
                                _this.typecolourblue = '';
                                _this.typecolour = 'blue';
                                _this.personal = false
                                _this.enterprise = true

                                _this.CorporateName = data.Result.AuthenticationInfo.TrueName
                                _this.BLicenceURL = data.Result.AuthenticationInfo.BLicenceURL

                            } else {
                                _this.RealName = data.Result.AuthenticationInfo.TrueName
                                _this.IdNumber = data.Result.AuthenticationInfo.IdentityNo
                                _this.photoimg = data.Result.AuthenticationInfo.IDCardFrontURL
                            }
                            _this.Alipay = data.Result.BankAccountInfo.AccountName

                            // 判断状态
                            if (data.Result.AuthenticationInfo.Status != 2) {
                                _this.header_hide = ""
                            } else {
                                _this.header_hide = "header_hide"
                            }

                        } else {
                            layer.open({
                                content: data.Message
                                , skin: 'msg'
                                , time: 2 //2秒后自动关闭
                            });
                        }
                    }
                })
            },
            // 点击手机号
            ModifiedNumber() {
                var _this = this
                layer.open({
                    title: [
                        '修改手机号',
                        'background: linear-gradient(#1b89f8, #1973ed);; color:#fff;'
                    ]
                    , content: '<p class="ElasticLayeriWidth"><input type="number" style="width: 100%" placeholder="请输入手机号码" pattern="[0-9]*" class="getiphone"></p><p class="ElasticLayeriWidth flex_justify_spaceBetween" style="justify-content:flex-start"><input class="getyzmval" type="text"  style="width: 60%" placeholder="验证码"><button class="getyzm" style="width: 40%">获取验证码</button></p>' +
                    '<p style="text-align: left;width: 100%;color: #ff5050" class="bug"></p>',
                    btn: ['确认', '取消'],
                    yes: function (index) {          //点确定按钮触发的回调函数，返回一个参数为当前层的索引
                        $.post(public_url + "/AjaxServers/LoginManager.ashx", {
                            action: 'checksmscode',
                            mobile: $('.getiphone').val(),
                            mobileCheckCode: $('.getyzmval').val(),
                            r: Math.random()
                        },
                            function (data) {
                                if (data.result == 0) {
                                    _this.MobilePhone = $('.getiphone').val();
                                    layer.close(index)
                                    _this.header_click = 0
                                    $('#baocun').attr('class','btn_bg')
                                    //初始化定时器
                                    clearInterval(setval)
                                    $('.getyzm').text('获取验证码')
                                } else {
                                    $('.bug').text(data.msg)
                                }
                            }, "json");
                    }, no: function (index) {                   //点取消按钮触发的回调函数
                        layer.close(index)
                        //初始化定时器
                        clearInterval(setval)
                        $('.getyzm').text('获取验证码')
                    }
                });
                $('.getiphone').on('keyup', function () {
                    $('.bug').text('')
                    if ($('.getiphone').val() == '') {
                        $('.getyzm').attr('id', '')
                    } else {
                        $('.getyzm').attr('id', 'getyzm')
                    }
                })
                var getyzm=true;
                $('.getyzm').off('click').on('click', function () {
                    if (!isMobileNumber($('.getiphone').val()) && !($('.getyzm').text() == '获取验证码')) {
                        return false
                    }
                    if(!getyzm){
                        return false
                    }
                    $.ajax({
                        url: public_url + '/api/UserManage/GetMobileStatus',
                        type: 'get',
                        dataType: 'json',
                        data: {
                            Mobile: $('.getiphone').val()
                        },
                        xhrFields: {
                            withCredentials: true
                        },
                        crossDomain: true,
                        success: function (data) {
                            if (data.Status == 0) {
                                if (data.Result.MobileStatus == 0) {
                                    // 发送验证码接口
                                    $.post(public_url + "/AjaxServers/LoginManager.ashx", {
                                        action: "sendmobilemsg_modifymobile",
                                        mobile: $('.getiphone').val(),
                                        r: Math.random()
                                    },
                                        function (data) {
                                            if (data == 0) {
                                                var time = 60
                                                setval = setInterval(function () {

                                                    $('.getyzm').text(time + 's');
                                                    time = time - 1
                                                    if (time < 0) {
                                                        clearInterval(setval)
                                                        $('.getyzm').text('获取验证码')
                                                    }
                                                }, 1000)
                                                $('.bug').text("短信已发送")
                                            } else if (data == -10) {

                                                $('.bug').text("验证码超时时间未到")
                                            } else if (data == -11) {
                                                $('.bug').text("发送短信失败")
                                            } else {
                                                layer.open({
                                                    content: "未知错误"
                                                    , skin: 'msg'
                                                    , time: 2 //2秒后自动关闭
                                                });
                                            }
                                        }, "json");

                                } else if (data.Result.MobileStatus == 1) {
                                    $('.bug').text("该手机已注册，请更换手机号")
                                }
                            } else {
                                $('.bug').text(data.Message)
                            }
                            getyzm=true;
                        },
                        beforeSend:function (data) {
                            getyzm=false;
                        }
                    })
                })

                // 判断手机号合法
                function isMobileNumber(phone) {
                    var flag = false;
                    var message = "";
                    var myreg = /^(((13[0-9]{1})|(14[0-9]{1})|(17[0-9]{1})|(15[0-3]{1})|(15[4-9]{1})|(18[0-9]{1})|(199))+\d{8})$/;
                    if (phone == '') {
                        // console.log("手机号码不能为空");
                        message = "手机号码不能为空！";
                    } else if (phone.length != 11) {
                        //console.log("请输入11位手机号码！");
                        message = "请输入11位手机号码！";
                    } else if (!myreg.test(phone)) {
                        //console.log("请输入有效的手机号码！");
                        message = "请输入有效的手机号码！";
                    } else {
                        flag = true;
                    }
                    if (message != "") {
                        $('.bug').text(message)
                    }
                    return flag;
                }
            },
            // 判断手机号合法
            isMobileNumber(phone) {
                var flag = false;
                var message = "";
                var myreg = /^(((13[0-9]{1})|(14[0-9]{1})|(17[0-9]{1})|(15[0-3]{1})|(15[4-9]{1})|(18[0-9]{1})|(199))+\d{8})$/;
                if (phone == '') {
                    // console.log("手机号码不能为空");
                    message = "手机号码不能为空！";
                } else if (phone.length != 11) {
                    //console.log("请输入11位手机号码！");
                    message = "请输入11位手机号码！";
                } else if (!myreg.test(phone)) {
                    //console.log("请输入有效的手机号码！");
                    message = "请输入有效的手机号码！";
                } else {
                    flag = true;
                }
                if (message != "") {
                    layer.open({
                        content: message
                        , skin: 'msg'
                        , time: 2 //2秒后自动关闭
                    });
                }
                return flag;
            },
            // 点击支付宝账号
            clickAlipay() {
                var _this = this;
                layer.open({
                    title: [
                        '修改支付宝账号',
                        'background: linear-gradient(#1b89f8, #1973ed);; color:#fff;'
                    ]
                    , content: '<p class="ElasticLayeriWidth"><input type="text" style="width: 100%" placeholder="请输入旧支付宝账号" class="OldAccountName" ></p><p class="ElasticLayeriWidth"><input type="text" class="NewAccountName" style="width: 100%" placeholder="请输入新支付宝账号" ></p>' +
                    '<p style="text-align: left;width: 100%;color: #ff5050" class="bug"></p>',
                    btn: ['确认', '取消'],
                    yes: function (index) {          //点确定按钮触发的回调函数，返回一个参数为当前层的索引
                        $.ajax({
                            url: public_url + '/api/UserManage/VerifBankAccount',
                            type: 'post',
                            dataType: 'json',
                            data: {
                                AccountType: 96001,
                                OldAccountName: $('.OldAccountName').val(),
                                NewAccountName: $('.NewAccountName').val()
                            },
                            xhrFields: {
                                withCredentials: true
                            },
                            crossDomain: true,
                            success: function (data) {
                                if (data.Status == 0) {
                                    _this.Alipay = $('.NewAccountName').val()
                                    layer.close(index)
                                    _this.header_click = 0
                                    $('#baocun').attr('class','btn_bg')
                                } else {
                                    $('.bug').text(data.Message)
                                }
                            }
                        })
                    }, no: function (index) {                   //点取消按钮触发的回调函数
                        layer.close(index)
                    }
                });
            },
            // 保存
            Preservation() {
                var obj = {};
                var _this = this;
                if (_this.header_click == 1) {
                    return false;
                }
                // 手机号
                if (!this.MobilePhone) {
                    layer.open({
                        content: '手机号不能为空'
                        , skin: 'msg'
                        , time: 2 //2秒后自动关闭
                    });
                    return false
                }
                if (!this.isMobileNumber(this.MobilePhone)) {
                    return false
                }
                // 支付宝账号
                if (!this.Alipay) {
                    layer.open({
                        content: '请输入支付宝账号！'
                        , skin: 'msg'
                        , time: 2 //2秒后自动关闭
                    });
                    return false
                }
                if (/^(0|86|17951)?(13[0-9]|15[012356789]|17[678]|18[0-9]|14[57])[0-9]{8}$/.test(this.Alipay) == false
                    &&
                    /\w+([-+.]\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*/.test(this.Alipay) == false) {
                    layer.open({
                        content: '支付宝账号格式不正确！'
                        , skin: 'msg'
                        , time: 2 //2秒后自动关闭
                    });
                    return false

                } else if (this.Alipay.length > 100) {
                    layer.open({
                        content: '支付宝账号格式不正确！'
                        , skin: 'msg'
                        , time: 2 //2秒后自动关闭
                    });
                    return false
                }
                obj.Mobile = _this.MobilePhone
                obj.AccountType = 96001
                obj.AccountName = _this.Alipay
                layer.open({
                    type: 2,
                    shadeClose: false
                });
                // 保存接口
                $.ajax({
                    url: public_url + '/api/UserManage/ModifyMobileAccount',
                    type: 'post',
                    dataType: 'json',
                    data: obj,
                    xhrFields: {
                        withCredentials: true
                    },
                    crossDomain: true,
                    success: function (data) {
                        if (data.Status == 0) {
                            layer.closeAll()
                            layer.open({
                                content: "成功"
                                , skin: 'msg'
                                , time: 2 //2秒后自动关闭
                                ,success: function(elem){
                                    window.location.reload()
                                }
                            });
                        } else {
                            layer.closeAll()
                            layer.open({
                                content: data.Message
                                , skin: 'msg'
                                , time: 2 //2秒后自动关闭
                            });
                        }
                    }
                })
            }
        },
        watch: {
            MobilePhone(val) {
                if (isNaN(Number(val)) || $.trim(val) == '') {
                    this.ntifyingCode = ""
                } else {
                    this.ntifyingCode = "ntifyingCode"
                }
            }
        }
    })
})