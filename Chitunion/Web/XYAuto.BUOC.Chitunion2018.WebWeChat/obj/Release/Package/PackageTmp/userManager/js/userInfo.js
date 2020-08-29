var vm = new Vue({
    el: '#app',
    data: {
        NickName: '',
        MobilePhone: '',
        VerificationCode: '',
        isCertificated: '',
        Alipay: '',
        Status: '',
        editClassTel: false, //显示编辑图标
        editClassAlipay: false //显示编辑图标
    },
    mounted:function () {
        var _self = this;
        _self.toGetUserInfo();
    },
    methods: {
        //获取个人信息
        toGetUserInfo:function(){
            var _self = this;
            $.ajax({
                url:public_url+'/api/UserManage/QueryUserInfo',
                type: 'get',
                dataType: 'json',
                data: {},
                xhrFields: {
                    withCredentials: true
                },
                crossDomain: true,
                success: function (data) {
                    if (data.Status == 0) {
                        _self.MobilePhone = data.Result.Mobile;
                        _self.NickName = data.Result.NickName;
                        _self.Alipay = data.Result.AccountName;
                        _self.Status = data.Result.Status;
                        //电话号码
                        if(_self.MobilePhone == ''){
                            _self.MobilePhone = '未完善';
                        } else {
                            _self.editClassTel = true;
                        }
                        //支付宝账号
                        if(_self.Alipay == ''){
                            _self.Alipay = '未完善';
                        } else {
                            _self.editClassAlipay = true;
                            _self.Alipay = _self.toHideInfo(_self.Alipay);
                        }
                        //认证信息
                        if(_self.Status == '0'){
                            _self.isCertificated = '未认证';
                        } else if(_self.Status == '1'){
                            _self.isCertificated = '待审核';
                        } else if(_self.Status == '2'){
                            _self.isCertificated = '已认证';
                        } else if(_self.Status == '3'){
                            _self.isCertificated = '认证未通过';
                        }
                    }
                }
            })
        },
        //支付宝账号*显示
        toHideInfo:function (txt){//支付宝账号两种形式：手机号码或邮箱
            var txtInfo = txt,
                txtInfoLen = txtInfo.length,
                newStr = '';
            if(!isNaN(Number(txtInfo)) && txtInfoLen == 11){//手机号码
                var subStr = txtInfo.substr(4, 4);//substr(start,length)
                newStr = txtInfo.replace(subStr, '****');
            } else {//邮箱
                var str = txtInfo.substring(0, txtInfo.indexOf('@'));//substring(start,stop)
                var strLen = str.length;
                if(strLen > 4){
                    var strMiddle = Math.ceil((strLen - 4) / 2);//获取中间四位
                    var subStr = str.substr(strMiddle, 4);//substr(start,length)
                    newStr = txtInfo.replace(subStr, '****');
                } else if(strLen < 4 && strLen > 2){//最后一位*
                    var subStr = str.substr(str.length - 1, 1);//substr(start,length)
                    newStr = txtInfo.replace(subStr, '*');
                } else {
                    newStr = txtInfo;
                }
            }
            return newStr;
        },
        //点击手机号码
        toClickMobilePhone:function(){
            var _self = this;
            if(_self.MobilePhone == '未完善'){//手机号码为空，填写
                _self.editClassTel = false;
                window.location = public_pre + "/userManager/userInfoTel.html?type=addTel";//完善手机号码
            } else {//修改手机号码，需要验证，跳转手机号码验证
                _self.editClassTel = true;
                var phone = _self.MobilePhone;
                sessionStorage.setItem('phone', _self.MobilePhone);
                window.location = public_pre + "/userManager/userInfoTel.html?type=yzChangeTel&tel"+_self.MobilePhone;
            }
        },
        //点击支付宝
        toClickAlipay:function(){
            var _self = this,
            checkPhone = _self.isCheckMobilePhone();
            console.log(checkPhone+'+点击支付宝')
            if(checkPhone){
                //有电话号码，跳转
                if(_self.Alipay == '未完善'){
                    _self.editClassAlipay = false;
                    window.location = public_pre + "/userManager/userInfoAccount.html?type=addAccount";
                } else {//修改支付宝账号，需要验证手机号码
                    _self.editClassAlipay = true;
                    sessionStorage.setItem('phone', _self.MobilePhone);
                    window.location = public_pre + "/userManager/userInfoTel.html?type=yzAlipayTel&tel"+_self.MobilePhone;
                }
            }
        },
        //点击认证
        toClickIsCertificated:function(){
            var _self = this;
            var checkPhone = _self.isCheckMobilePhone();
            console.log(checkPhone+'+点击认证')
            if(checkPhone){
                //有电话号码，跳转
                window.location = public_pre + "/userManager/certification.html";
            }
        },
        isCheckMobilePhone:function(){
            var _self = this;
            if(_self.MobilePhone == '未完善'){
                layer.open({
                    content: '请先完善手机号'
                    , skin: 'msg'
                    , time: 2 //2秒后自动关闭
                });
                return false;
            } else {
                return true;
            }
        }
    },
})
