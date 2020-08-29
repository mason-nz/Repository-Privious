//获取url的type
var getUrlParam = GetRequest(),
    getType = getUrlParam.type,
    getTel = '';
document.getElementById('type').value = getType;
if(getType == 'addTel'){
    var tit = '<title>完善手机号码</title>',
        tit_text = '完善手机号码';
    sessionStorage.removeItem('phone');
} else if(getType == 'yzChangeTel' || getType == 'yzAlipayTel'){
    var tit = '<title>验证手机号码</title>',
        tit_text = '验证手机号码';
    getTel = sessionStorage.getItem('phone') || '';
    if(getTel == ''){
        window.location = "/userManager/userInfo.html";
    }
} else if(getType == 'newTel'){
    var tit = '<title>输入新手机号码</title>',
        tit_text = '输入新手机号码';
    sessionStorage.removeItem('phone');
}
$('head').prepend(tit);
var vm = new Vue({
    el: '#app',
    data: {
        ntifyingCode: '',// 验证码颜色
        MobilePhone: '',
        VerificationCode: '',
        type: '',
        getyzm: '获取验证码',
        btnTxt:'',//按钮文字
        placeholderTel: '',
        clickFlag: 0,
        mobileStatus: '',
        page_title:tit_text
    },
    mounted:function(){
        var _self = this;
        _self.type = document.getElementById('type').value;
        if(_self.type == 'addTel' || _self.type == 'newTel'){//完善手机号码  //输入新手机号码
            _self.btnTxt = '保存';
            _self.MobilePhone = '';
        } else if(_self.type == 'yzChangeTel' || _self.type == 'yzAlipayTel'){//修改手机号码   //修改支付宝验证手机号码
            _self.btnTxt = '下一步';
            _self.ntifyingCode = "ntifyingCode";
            _self.MobilePhone = getTel;
        }
        if(_self.type == 'addTel'){//完善手机号码
            _self.placeholderTel = '请输入11位手机号码';
        } else if(_self.type == 'newTel'){//输入新手机号码
            _self.placeholderTel = '请输入新手机号码';
        }
    },
    watch: {
        MobilePhone:function(val) {
            var _self = this,
                thisVal = $.trim(val),
                thisValLen = thisVal.length;
                // console.log(thisValLen+'+thisValLen')
            if (isNaN(Number(val)) || thisVal == '' || thisValLen < 11) {
                _self.ntifyingCode = "";
            } else if (thisValLen == 11){
                _self.ntifyingCode = "ntifyingCode";
            }
        },
        VerificationCode:function(val){
            var _self = this,
                thisVal = $.trim(val),
                thisValLen = thisVal.length,
                nextBtn = $('#js_save_btn');
            if (isNaN(Number(thisVal)) || thisVal == '' || thisValLen < 4) {
                nextBtn.addClass('Thegrey');
            } else if(thisValLen > 4){
                nextBtn.addClass('Thegrey');
                layer.open({
                    content: "验证码应为4位数"
                    , skin: 'msg'
                    , time: 2 //2秒后自动关闭
                });
            }
            if(_self.clickFlag > 0){
                var phone = $.trim(_self.MobilePhone);
                if(phone.length == 11 && thisValLen == 4){
                    nextBtn.removeClass('Thegrey');
                }
            }
        }
    },
    methods: {
        // 点击验证码
        getVerificationCode:function() {
            var _self = this,
                phone = $.trim(_self.MobilePhone);
            if (_self.isMobileNumber(phone) && _self.getyzm == '获取验证码') {
                _self.clickFlag ++;
                //判断是修改手机号码还是完善手机号码
                if(_self.type == 'addTel' || _self.type == 'newTel'){//完善手机号码  //输入新手机号码
                    // 请求获取验证码接口
                    $.ajax({
                        url: public_url+'/api/UserManage/GetMobileStatus',
                        type: 'get',
                        dataType: 'json',
                        data: {
                            mobile: phone,
                        },
                        xhrFields: {
                            withCredentials: true
                        },
                        crossDomain: true,
                        success: function (data) {
                            if (data.Status == 0) {
                                // alert(data.Result.MobileStatus)
                                // console.log(data.Result.MobileStatus+'+data.Result.MobileStatus')
                                if (data.Result.MobileStatus == 0) {//注册手机号
                                    // 发送验证码接口
                                    _self.toSendVerificationCode('sendmobilemsg_register', phone);
                                } else if (data.Result.MobileStatus == 1) {//该手机号码已注册，请更换手机号
                                    layer.open({
                                        content: "该手机号码已注册，请更换手机号"
                                        , skin: 'msg'
                                        , time: 2 //2秒后自动关闭
                                    });
                                } else if (data.Result.MobileStatus == 2) {// 合并信息
                                    _self.mobileStatus = data.Result.MobileStatus;//记录手机号码状态
                                    _self.toSendVerificationCode('sendmobilemsg_register', phone);
                                    // layer.open({
                                    //     content: '该手机号已在pc端注册，是否做绑定'
                                    //     ,btn: ['绑定', '取消'],
                                    //     yes: function(index){          //点确定按钮触发的回调函数，返回一个参数为当前层的索引
                                    //         layer.close(index);
                                    //         // 发送验证码接口
                                    //         _self.toSendVerificationCode('sendmobilemsg_register', phone);
                                    //     },no:function(index){                   //点取消按钮触发的回调函数
                                    //         layer.close(index)
                                    //     }
                                    // });
                                } else {
                                    layer.open({
                                        content: "未知错误"
                                        , skin: 'msg'
                                        , time: 2 //2秒后自动关闭
                                    });
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
                } else if(_self.type == 'yzChangeTel' || _self.type == 'yzAlipayTel'){//修改手机号码   //修改支付宝验证手机号码
                    // 发送验证码接口
                    _self.toSendVerificationCode('sendmobilemsg_modifymobile', phone);
                }
            }
        },
        // 判断手机号合法
        isMobileNumber:function(phone) {
            var flag = false;
            var message = "";
            var myreg = /^(((13[0-9]{1})|(14[0-9]{1})|(17[0-9]{1})|(15[0-3]{1})|(15[4-9]{1})|(18[0-9]{1})|(199)|(166))+\d{8})$/;
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
        // 保存
        toClickSaveBtn:function() {
            // 置灰不点击
            var saveBtnDom = $('#js_save_btn'),
                obj = {},
                 _self = this,
                phone = $.trim(_self.MobilePhone),
                code = $.trim(_self.VerificationCode),
                postData = {
                    r: Math.random()
                },
                postUrl = public_url + '/AjaxServers/LoginManager.ashx';
            if(saveBtnDom.hasClass('Thegrey')){
                return false;
            }
            //手机号码
            if (!phone) {
                layer.open({
                    content: '手机号码不能为空'
                    , skin: 'msg'
                    , time: 2 //2秒后自动关闭
                });
                return false
            }
            if (!_self.isMobileNumber(phone)) {
                return false
            }
            // 验证码
            if (!code) {
                layer.open({
                    content: '验证码不能为空'
                    , skin: 'msg'
                    , time: 2 //2秒后自动关闭
                });
                return false
            } else if (code.length < 4) {
                layer.open({
                    content: '验证码输入不正确'
                    , skin: 'msg'
                    , time: 2 //2秒后自动关闭
                });
                return false;
            }
            postData.mobile = phone;
            postData.mobileCheckCode = code;
            console.log(_self.type+'+_self.type')
            if(_self.type == 'addTel' || _self.type == 'newTel'){//完善手机号码   //输入新手机号码
                //_self.mobileStatus == 2，先验证手机号码和验证码是否正确，然后再调用其他接口
                if(_self.mobileStatus == 2){
                    postData.action = 'checksmscode';
                    //点击完善和输入新手机号码——保存
                    _self.toSaveAjax(postUrl, postData, function(data){
                        if (data.result == 0){//验证通过
                            layer.open({
                                content: '该手机号已在pc端注册，是否做绑定'
                                ,btn: ['绑定', '取消'],
                                yes: function(index){          //点确定按钮触发的回调函数，返回一个参数为当前层的索引
                                    layer.close(index);
                                    postData.action = 'SaveAttestation';
                                    //点击完善和输入新手机号码——保存
                                    _self.toSaveAjax(postUrl, postData, function(data){
                                        if (data.result == 0){//验证通过
                                            layer.closeAll()
                                            layer.open({
                                                content: "成功"
                                                , skin: 'msg'
                                                , time: 2 //2秒后自动关闭
                                                ,success: function(elem){
                                                    if(_self.type == 'addTel' || _self.type == 'newTel'){//完善手机号码
                                                        window.location = "/userManager/userInfo.html";
                                                    } else if(_self.type == 'yzChangeTel'){//修改手机号码
                                                        window.location = "/userManager/userInfoTel.html?type=newTel";
                                                    } else if(_self.type == 'yzAlipayTel'){//修改支付宝验证手机号码
                                                        window.location = "/userManager/userInfoAccount.html?type=changeAccount";
                                                    }
                                                }
                                            });
                                        } else {
                                            layer.closeAll()
                                            layer.open({
                                                content: data.msg
                                                , skin: 'msg'
                                                , time: 2 //2秒后自动关闭
                                            });
                                        }
                                    })
                                },no:function(index){                   //点取消按钮触发的回调函数
                                    layer.close(index)
                                }
                            });

                        } else {
                            layer.closeAll()
                            layer.open({
                                content: data.msg
                                , skin: 'msg'
                                , time: 2 //2秒后自动关闭
                            });
                        }
                    })
                } else {
                    postData.action = 'SaveAttestation';
                    //点击完善和输入新手机号码——保存
                    _self.toSaveAjax(postUrl, postData, function(data){
                        if (data.result == 0){//验证通过
                            layer.closeAll()
                            layer.open({
                                content: "成功"
                                , skin: 'msg'
                                , time: 2 //2秒后自动关闭
                                ,success: function(elem){
                                    if(_self.type == 'addTel' || _self.type == 'newTel'){//完善手机号码
                                        window.location = "/userManager/userInfo.html";
                                    } else if(_self.type == 'yzChangeTel'){//修改手机号码
                                        window.location = "/userManager/userInfoTel.html?type=newTel";
                                    } else if(_self.type == 'yzAlipayTel'){//修改支付宝验证手机号码
                                        window.location = "/userManager/userInfoAccount.html?type=changeAccount";
                                    }
                                }
                            });
                        } else {
                            layer.closeAll()
                            layer.open({
                                content: data.msg
                                , skin: 'msg'
                                , time: 2 //2秒后自动关闭
                            });
                        }
                    })
                }
            } else if(_self.type == 'yzChangeTel' || _self.type == 'yzAlipayTel'){//修改手机号码 //修改支付宝验证手机号码
                postData.action = 'checksmscode';//点击下一步，验证验证码
                //点击完善和输入新手机号码——保存
                _self.toSaveAjax(postUrl, postData, function(data){
                    if (data.result == 0){//验证通过
                        layer.closeAll()
                        layer.open({
                            content: "成功"
                            , skin: 'msg'
                            , time: 2 //2秒后自动关闭
                            ,success: function(elem){
                                if(_self.type == 'addTel' || _self.type == 'newTel'){//完善手机号码
                                    window.location = "/userManager/userInfo.html";
                                } else if(_self.type == 'yzChangeTel'){//修改手机号码
                                    window.location = "/userManager/userInfoTel.html?type=newTel";
                                } else if(_self.type == 'yzAlipayTel'){//修改支付宝验证手机号码
                                    window.location = "/userManager/userInfoAccount.html?type=changeAccount";
                                }
                            }
                        });
                    } else {
                        layer.closeAll()
                        layer.open({
                            content: data.msg
                            , skin: 'msg'
                            , time: 2 //2秒后自动关闭
                        });
                    }
                })
            }
        },
        //点击保存请求
        toSaveAjax:function(url, pdata, callback, errorback){
            $.ajax({
                url: url,
                type:'post',
                dataType:'json',
                data: pdata,
                xhrFields: {
                    withCredentials: true
                },
                crossDomain: true,
                success: callback,
                error: errorback
            })
        },
        //发送验证码
        toSendVerificationCode:function(action, phone, code){
            var _self = this;
            $.post(public_url+"/AjaxServers/LoginManager.ashx", {
                action: action,
                mobile: phone,
                r: Math.random()
            }, function(data){
                if (data == 0) {
                    var time = 60;
                    var setval = setInterval(function () {
                        _self.getyzm = time + 's';
                        time = time - 1;
                        _self.ntifyingCode = "";
                        // alert(time)
                        if(time < 0){
                            clearInterval(setval);
                            _self.getyzm = '获取验证码';
                            _self.ntifyingCode = "ntifyingCode";
                        }
                    },1000)
                    layer.open({
                        content: "短信已发送"
                        , skin: 'msg'
                        , time: 2 //2秒后自动关闭
                    });
                } else if (data == -10) {
                    layer.open({
                        content: "验证码超时时间未到，不能获取"
                        , skin: 'msg'
                        , time: 2 //2秒后自动关闭
                    });
                } else if (data == -11) {
                    layer.open({
                        content: "发送短信失败"
                        , skin: 'msg'
                        , time: 2 //2秒后自动关闭
                    });
                } else if (data == -12) {
                    layer.open({
                        content: "手机号码格式不正确"
                        , skin: 'msg'
                        , time: 2 //2秒后自动关闭
                    });
                } else if (data == -13) {
                    layer.open({
                        content: "新手机号码与老手机号码一致"
                        , skin: 'msg'
                        , time: 2 //2秒后自动关闭
                    });
                } else {
                    layer.open({
                        content: "未知错误"
                        , skin: 'msg'
                        , time: 2 //2秒后自动关闭
                    });
                }
            }, "json");
        },
        //顶部返回按钮
        history_go:function(){//没有前一个页面，就跳转到抢单赚钱页，否则，跳转到前一个页面
            if (document.referrer === '') {
                window.location = '/index.html';
            }else{
                history.go(-1);
            }
        }
    }
})
//监听微信返回
window.addEventListener("popstate", function(e) {
    console.log("监听到返回");
    pushHistory();
}, false);
function pushHistory() {
    var state = {
        title: "个人信息",
        url: "/userManager/userInfo.html"
    };
    window.history.pushState(state, "个人信息", "#");
}
