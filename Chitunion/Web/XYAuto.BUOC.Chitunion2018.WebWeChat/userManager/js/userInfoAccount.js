//获取url的type
var getUrlParam = GetRequest(),
getType = getUrlParam.type;
document.getElementById('type').value = getType;
if(getType == 'addAccount'){//完善支付宝账号
var tit = '<title>完善支付宝账号</title>';
} else if(getType == 'changeAccount'){
var tit = '<title>修改支付宝账号</title>';
}
$('head').prepend(tit);

var vm = new Vue({
    el: '#app',
    data: {
        type: '',
        Alipay: '',
        newAlipay: ''
    },
    mounted:function () {
        var _self = this;
        _self.type = document.getElementById('type').value;
    },
    watch: {
        Alipay:function(val) {
            var _self = this;
            var nextBtn = $('#js_save_btn');
            var newAlipay = $.trim(_self.newAlipay);
            var newAlipayLen = newAlipay.length;
            if(_self.type == 'addAccount'){//完善支付宝账号
                if ($.trim(val) == '') {
                    nextBtn.addClass('Thegrey');
                } else {
                    nextBtn.removeClass('Thegrey');
                }
            } else if(_self.type == 'changeAccount'){//修改支付宝账号
                if ($.trim(val) == '' || newAlipayLen == 0) {
                    nextBtn.addClass('Thegrey');
                } else if ($.trim(val) != '' && newAlipayLen > 0){
                    nextBtn.removeClass('Thegrey');
                    console.log(newAlipayLen+'+newAlipayLen')
                }
            }

        },
        newAlipay:function(val) {
            var _self = this;
            var nextBtn = $('#js_save_btn');
            var Alipay = $.trim(_self.Alipay);
            var alipayLen = Alipay.length;
            if(_self.type == 'addAccount'){//完善支付宝账号
                if ($.trim(val) == '') {
                    nextBtn.addClass('Thegrey');
                } else {
                    nextBtn.removeClass('Thegrey');
                }
            } else if(_self.type == 'changeAccount'){//修改支付宝账号
                if ($.trim(val) == '' || alipayLen == 0) {
                    nextBtn.addClass('Thegrey');
                } else if ($.trim(val) != '' && alipayLen > 0){
                    nextBtn.removeClass('Thegrey');
                    console.log(alipayLen+'+alipayLen')
                }
            }
        },
    },
    methods: {
        //点击保存
        toClickSaveBtn:function(){
            var saveBtnDom = $('#js_save_btn');
            if(saveBtnDom.hasClass('Thegrey')){
                return false;
            }
            // 支付宝账号
            var _self = this,
                alipay = $.trim(_self.Alipay),
                postUrl = public_url + '/api/UserManage/SavePayInfo',
                postData = {};
            if(_self.type == 'addAccount'){//完善支付宝账号
                var layerConNull = '请输入支付宝账号！',
                    layerConVer = '请输入正确的支付宝账号',
                    checkFlag = _self.toCheckIsNull(alipay, layerConNull),
                    verifyFlag = _self.toVerifyAlipay(alipay,layerConVer);
                    console.log(checkFlag, verifyFlag)
                if(checkFlag && verifyFlag){
                    postData = {
                        IsAdd: true, //新增
                        AccountName: alipay,
                        AccountType: '96001'
                    }
                    _self.toSaveAjax(postUrl, postData, function(data){
                        if (data.Status == 0){
                            layer.closeAll()
                                layer.open({
                                    content: "成功"
                                    , skin: 'msg'
                                    , time: 2 //2秒后自动关闭
                                    ,success: function(elem){
                                        window.location = public_pre + "/userManager/userInfo.html"
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
                    })
                }
            } else if(_self.type == 'changeAccount'){//修改支付宝账号
                var newAlipay = $.trim(_self.newAlipay),
                    layerConIsNullNew = '请输入新支付宝账号！',
                    layerConIsNullOld = '请输入旧支付宝账号！',
                    layerConVerifyOld = '旧支付宝账号格式不正确！',
                    layerConVerifyNew = '新支付宝账号格式不正确！';
                if(_self.toCheckIsNull(alipay, layerConIsNullOld) && _self.toVerifyAlipay(alipay, layerConVerifyOld)){
                    if(_self.toCheckIsNull(newAlipay, layerConIsNullNew) && _self.toVerifyAlipay(newAlipay, layerConVerifyNew)){
                        if(alipay == newAlipay){
                            layer.open({
                                content: '新支付宝账号与旧支付宝账号一致'
                                , skin: 'msg'
                                , time: 2 //2秒后自动关闭
                            });
                            return false
                        }
                        postData = {
                            IsAdd: false,
                            AccountName: newAlipay,
                            AccountType: '96001',
                            OldAccountName: alipay,
                            OldAccountType: '96001'
                        };
                        _self.toSaveAjax(postUrl, postData, function(data){
                            if (data.Status == 0){
                                layer.closeAll()
                                    layer.open({
                                        content: "成功"
                                        , skin: 'msg'
                                        , time: 2 //2秒后自动关闭
                                        ,success: function(elem){
                                            window.location = public_pre + "/userManager/userInfo.html"
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
                        })
                    }
                }
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
        //支付宝账号是否为空
        toCheckIsNull:function(account, layerCon){
            if (!account) {
                layer.open({
                    content: layerCon
                    , skin: 'msg'
                    , time: 2 //2秒后自动关闭
                });
                return false
            } else {
                return true
            }
        },
        //支付宝账号验证
        toVerifyAlipay:function(account, layerCon){
            console.log(/^(0|86|17951)?(13[0-9]|15[012356789]|17[678]|18[0-9]|14[57])[0-9]{8}$/.test(account)+'+000')
            console.log(/\w+([-+.]\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*/.test(account)+'+111')
            if (/^(0|86|17951)?(13[0-9]|15[012356789]|17[678]|18[0-9]|14[57])[0-9]{8}$/.test(account) == false
                &&
                /\w+([-+.]\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*/.test(account) == false) {
                layer.open({
                    content: layerCon
                    , skin: 'msg'
                    , time: 2 //2秒后自动关闭
                });
                return false
            } else if (account.length > 100) {
                layer.open({
                    content: layerCon
                    , skin: 'msg'
                    , time: 2 //2秒后自动关闭
                });
                return false
            } else {
                return true
            }
        }
    }
})
//监听微信返回
window.addEventListener("popstate", function(e) {
    console.log("监听到返回account");
    pushHistory();
}, false);
function pushHistory() {
    var state = {
        title: "个人信息",
        url: public_pre + "/userManager/userInfo.html"
    };
    window.history.pushState(state, "个人信息", "#");
}
