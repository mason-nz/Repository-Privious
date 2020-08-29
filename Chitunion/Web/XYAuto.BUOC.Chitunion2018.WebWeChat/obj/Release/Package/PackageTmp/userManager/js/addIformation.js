var nickname=""
$.ajax({
    url:public_url+'/api/UserManage/GetExamineStatus',
    dataType:'json',
    type:'get',
    data:{},
    xhrFields: {
        withCredentials: true
    },
    crossDomain: true,
    async:false,
    success:function (data) {
        if(data.Status==0){
            nickname=data.Result.NickName;
            // 跳转
            if(data.Result.Status==2||data.Result.Status==1){
                // 已认证和待审核
                window.location=public_pre+"/userManager/Auditingiformation.html"
            }else if (data.Result.Status==3){
                // 认证未通过
                window.location=public_pre+"/userManager/RejectIformation.html"
            }
        }else {
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
            fail:function () {
                $('.no_data').show().siblings('.netOk').remove();
                return false;
            }
        });
    })

    var app = new Vue({
        el: '#app',
        data: {
            // 顶部提示
            // header_hide:'header_hide',
            // header_text:'',
            // 微信昵称
            nickname:nickname,
            //手机号
            MobilePhone: '',
            // 验证码
            VerificationCode: '',
            // 验证码颜色
            ntifyingCode: '',
            getyzm:"获取验证码",
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
            sfz: '请上传',
            // 企业系列
            enterprise: false,
            // 公司名称
            CorporateName: '',
            // 营业执照
            BLicenceURL:'',
            yyzz:'请上传',
            // 支付宝账号
            Alipay: '',
            // serverId
            serverId:''
        },
        methods: {
            // 点击验证码
            getVerificationCode() {
                var _this = this;
                console.log(_this.getyzm,10);
                if (this.isMobileNumber(this.MobilePhone)&&_this.getyzm=='获取验证码') {
                    // 请求获取验证码接口
                    $.ajax({
                        url: public_url+'/api/UserManage/GetMobileStatus',
                        type: 'get',
                        dataType: 'json',
                        data: {
                            mobile: _this.MobilePhone,
                        },
                        xhrFields: {
                            withCredentials: true
                        },
                        crossDomain: true,
                        success: function (data) {
                            if (data.Status == 0) {
                                if (data.Result.MobileStatus == 0) {
                                    // 发送验证码接口
                                    $.post(public_url+"/AjaxServers/LoginManager.ashx", {
                                            action: "sendmobilemsg_register",
                                            mobile: _this.MobilePhone,
                                            r: Math.random()
                                        }, function(data){
                                            if (data == 0) {
                                                var time=60
                                                var setval=setInterval(function () {
                                                    _this.getyzm=time+'s'
                                                    time=time-1
                                                    // alert(time)
                                                    if(time<0){
                                                        clearInterval(setval);
                                                        _this.getyzm='获取验证码'
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
                                            }else {
                                                layer.open({
                                                    content: "未知错误"
                                                    , skin: 'msg'
                                                    , time: 2 //2秒后自动关闭
                                                });
                                            }
                                        }, "json");
                                } else if (data.Result.MobileStatus == 1) {
                                    layer.open({
                                        content: "该手机号码已注册，请更换手机号"
                                        , skin: 'msg'
                                        , time: 2 //2秒后自动关闭
                                    });
                                } else if (data.Result.MobileStatus == 2) {
                                    // 合并信息
                                    layer.open({
                                        content: '该手机号已在pc端注册，是否做信息合并'
                                        ,btn: ['合并信息', '修改手机号'],
                                        yes: function(index){          //点确定按钮触发的回调函数，返回一个参数为当前层的索引
                                            layer.close(index);
                                            // 发送验证码接口
                                            $.post(public_url+"/AjaxServers/LoginManager.ashx", {
                                                action: "sendmobilemsg_register",
                                                mobile: _this.MobilePhone,
                                                r: Math.random()
                                            }, function(data){
                                                if (data == 0) {
                                                    var time=60
                                                    var setval=setInterval(function () {
                                                        _this.getyzm=time+'s'
                                                        time=time-1
                                                        // alert(time)
                                                        if(time<0){
                                                            clearInterval(setval);
                                                            _this.getyzm='获取验证码'
                                                        }
                                                    },1000)
                                                    layer.open({
                                                        content: "短信已发送"
                                                        , skin: 'msg'
                                                        , time: 2 //2秒后自动关闭
                                                    });
                                                    $('#Verification').on('keyup',function () {
                                                        if($(this).val().length==4){
                                                            $.post(public_url + "/AjaxServers/LoginManager.ashx", {
                                                                    action: 'checksmscode',
                                                                    mobile: _this.MobilePhone,
                                                                    mobileCheckCode: _this.VerificationCode,
                                                                    r: Math.random()
                                                                },
                                                                function (data) {
                                                                    if (data.result == 0) {
                                                                        $.ajax({
                                                                            url:public_url+'/api/UserManage/QueryUserInfo',
                                                                            type: 'get',
                                                                            dataType: 'json',
                                                                            data: {
                                                                                Mobile: _this.MobilePhone,
                                                                            },
                                                                            xhrFields: {
                                                                                withCredentials: true
                                                                            },
                                                                            crossDomain: true,
                                                                            success: function (data) {
                                                                                if (data.Status == 0) {
                                                                                    if (data.Result.AuthenticationInfo.Status == 1 || data.Result.AuthenticationInfo.Status == 2) {
                                                                                        $('#Mask').show()
                                                                                    } else {
                                                                                        $('#Mask').hide()
                                                                                    }
                                                                                    if (data.Result.AuthenticationInfo.Type == 1001) {
                                                                                        _this.typecolourblue = '';
                                                                                        _this.typecolour = 'blue';
                                                                                        _this.personal = false
                                                                                        _this.enterprise = true

                                                                                        _this.CorporateName = data.Result.AuthenticationInfo.TrueName

                                                                                        if(data.Result.AuthenticationInfo.BLicenceURL){
                                                                                            _this.BLicenceURL = data.Result.AuthenticationInfo.BLicenceURL
                                                                                            _this.yyzz = "";
                                                                                            $('#BLicenceURL').show();
                                                                                        }
                                                                                    } else {
                                                                                        _this.RealName = data.Result.AuthenticationInfo.TrueName
                                                                                        _this.IdNumber = data.Result.AuthenticationInfo.IdentityNo;
                                                                                        if(data.Result.AuthenticationInfo.IDCardFrontURL){
                                                                                            _this.photoimg = data.Result.AuthenticationInfo.IDCardFrontURL
                                                                                            _this.sfz = "";
                                                                                            $('#photoimg').show();
                                                                                        }

                                                                                    }
                                                                                    _this.Alipay = data.Result.BankAccountInfo.AccountName?data.Result.BankAccountInfo.AccountName:''
                                                                                    if(data.Result.AuthenticationInfo.Status==1||data.Result.AuthenticationInfo.Status==2){
                                                                                        $('#Mask').show()
                                                                                    }else {
                                                                                        $('#Mask').hide()
                                                                                    }
                                                                                }else {
                                                                                    layer.open({
                                                                                        content: data.Message
                                                                                        , skin: 'msg'
                                                                                        , time: 2 //2秒后自动关闭
                                                                                    });
                                                                                }
                                                                            }
                                                                        })
                                                                    } else {
                                                                        $('.bug').text(data.msg)
                                                                    }
                                                                }, "json");

                                                        }else if($(this).val().length>4){
                                                            layer.open({
                                                                content: "验证码应为4位数"
                                                                , skin: 'msg'
                                                                , time: 2 //2秒后自动关闭
                                                            });
                                                        }
                                                    })
                                                } else if (data == -10) {
                                                    layer.open({
                                                        content: "验证码超时时间未到，不能获取"
                                                        , skin: 'msg'
                                                        , time: 2 //2秒后自动关闭
                                                    });
                                                    $('#Verification').on('keyup',function () {
                                                        if($(this).val().length==4){
                                                            $.post(public_url + "/AjaxServers/LoginManager.ashx", {
                                                                    action: 'checksmscode',
                                                                    mobile: _this.MobilePhone,
                                                                    mobileCheckCode: _this.VerificationCode,
                                                                    r: Math.random()
                                                                },
                                                                function (data) {
                                                                    if (data.result == 0) {
                                                                        $.ajax({
                                                                            url:public_url+'/api/UserManage/QueryUserInfo',
                                                                            type: 'get',
                                                                            dataType: 'json',
                                                                            data: {
                                                                                Mobile: _this.MobilePhone,
                                                                            },
                                                                            xhrFields: {
                                                                                withCredentials: true
                                                                            },
                                                                            crossDomain: true,
                                                                            success: function (data) {
                                                                                if (data.Status == 0) {
                                                                                    if (data.Result.AuthenticationInfo.Status == 1 || data.Result.AuthenticationInfo.Status == 2) {
                                                                                        $('#Mask').show()
                                                                                    } else {
                                                                                        $('#Mask').hide()
                                                                                    }
                                                                                    if (data.Result.AuthenticationInfo.Type == 1001) {
                                                                                        _this.typecolourblue = '';
                                                                                        _this.typecolour = 'blue';
                                                                                        _this.personal = false
                                                                                        _this.enterprise = true

                                                                                        _this.CorporateName = data.Result.AuthenticationInfo.TrueName

                                                                                        if(data.Result.AuthenticationInfo.BLicenceURL){
                                                                                            _this.BLicenceURL = data.Result.AuthenticationInfo.BLicenceURL
                                                                                            _this.yyzz = "";
                                                                                            $('#BLicenceURL').show();
                                                                                        }
                                                                                    } else {
                                                                                        _this.RealName = data.Result.AuthenticationInfo.TrueName
                                                                                        _this.IdNumber = data.Result.AuthenticationInfo.IdentityNo;
                                                                                        if(data.Result.AuthenticationInfo.IDCardFrontURL){
                                                                                            _this.photoimg = data.Result.AuthenticationInfo.IDCardFrontURL
                                                                                            _this.sfz = "";
                                                                                            $('#photoimg').show();
                                                                                        }

                                                                                    }
                                                                                    _this.Alipay = data.Result.BankAccountInfo.AccountName?data.Result.BankAccountInfo.AccountName:''
                                                                                    if(data.Result.AuthenticationInfo.Status==1||data.Result.AuthenticationInfo.Status==2){
                                                                                        $('#Mask').show()
                                                                                    }else {
                                                                                        $('#Mask').hide()
                                                                                    }
                                                                                }else {
                                                                                    layer.open({
                                                                                        content: data.Message
                                                                                        , skin: 'msg'
                                                                                        , time: 2 //2秒后自动关闭
                                                                                    });
                                                                                }
                                                                            }
                                                                        })
                                                                    } else {
                                                                        $('.bug').text(data.msg)
                                                                    }
                                                                }, "json");

                                                        }else if($(this).val().length>4){
                                                            layer.open({
                                                                content: "验证码应为4位数"
                                                                , skin: 'msg'
                                                                , time: 2 //2秒后自动关闭
                                                            });
                                                        }
                                                    })
                                                } else if (data == -11) {
                                                    layer.open({
                                                        content: "发送短信失败"
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
                                        },no:function(index){                   //点取消按钮触发的回调函数
                                            layer.close(index)
                                        }
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
            // 判断身份证合法
            checkIdentity(identity) {
                var reg = /^[1-9]{1}[0-9]{14}$|^[1-9]{1}[0-9]{16}([0-9]|[xX])$/;
                if (reg.test(identity)) {
                    return true;
                } else {
                    return false;
                }
            },
            // 点击个人
            personalClick() {
                // 个人和企业按钮
                var colour = this.typecolourblue;
                this.typecolourblue = this.typecolour;
                this.typecolour = colour;

                this.personal = true
                this.enterprise = false
            },
            // 点击企业
            enterpriseClick() {
                // 个人和企业按钮
                var colour = this.typecolour;
                this.typecolour = this.typecolourblue;
                this.typecolourblue = colour;
                this.personal = false
                this.enterprise = true
            },
            // 点击手持身份证
            IDCardUploading() {
                var _this = this
                wx.chooseImage({
                    count: 1, // 默认9
                    sizeType: [], // 可以指定是原图还是压缩图，默认二者都有
                    sourceType: [], // 可以指定来源是相册还是相机，默认二者都有
                    success: function (res) {
                        var localIds = res.localIds; // 返回选定照片的本地ID列表，localId可以作为img标签的src属性显示图片
                        wx.uploadImage({
                            localId: localIds[0], isShowProgressTips: 1, success: function (data) {
                                _this.serverId = data.serverId;
                            }
                        })
                        //<img id="photoimg" @click="photo_img" :src="photoimg" alt="">
                        _this.sfz = "";
                        $('#photoimg').show();
                        _this.photoimg = localIds[0]
                    }
                });
            },
            // 点击身份证图片
            photo_img() {
                var _this = this
                wx.previewImage({
                    current: _this.photoimg, // 当前显示图片的http链接
                    urls: [_this.photoimg] // 需要预览的图片http链接列表
                });
            },
            // 点击营业执照
            ClickBLicenceURL(){
                var _this = this
                wx.chooseImage({
                    count: 1, // 默认9
                    sizeType: [], // 可以指定是原图还是压缩图，默认二者都有
                    sourceType: [], // 可以指定来源是相册还是相机，默认二者都有
                    success: function (res) {
                        var localIds = res.localIds; // 返回选定照片的本地ID列表，localId可以作为img标签的src属性显示图片
                        wx.uploadImage({
                            localId: localIds[0], isShowProgressTips: 1, success: function (data) {
                                _this.serverId = data.serverId;
                            }
                        })
                        //<img id="photoimg" @click="photo_img" :src="photoimg" alt="">
                        _this.yyzz = "";
                        $('#BLicenceURL').show();
                        _this.BLicenceURL = localIds[0]
                    }
                });
            },
            // 点击营业执照图片
            BLicence_URL(){
                var _this = this
                wx.previewImage({
                    current: _this.BLicenceURL, // 当前显示图片的http链接
                    urls: [_this.BLicenceURL] // 需要预览的图片http链接列表
                });
            },
            // 保存
            Preservation() {
                // 置灰不点击
                if($('#PreserVation').attr('class')=="Thegrey"){
                    return false
                }

                var obj={};
                var _this=this;
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
                // 验证码
                if (!this.VerificationCode) {
                    layer.open({
                        content: '验证码不能为空'
                        , skin: 'msg'
                        , time: 2 //2秒后自动关闭
                    });
                    return false
                }
                // 个人
                if (this.personal && !this.enterprise) {
                    // 真实姓名
                    if (!this.RealName) {
                        layer.open({
                            content: '真实姓名不能为空'
                            , skin: 'msg'
                            , time: 2 //2秒后自动关闭
                        });
                        return false
                    }
                    // 身份证号
                    if (!this.IdNumber) {
                        layer.open({
                            content: '身份证号不能为空'
                            , skin: 'msg'
                            , time: 2 //2秒后自动关闭
                        });
                        return false
                    }
                    if (!this.checkIdentity(this.IdNumber)) {
                        layer.open({
                            content: '身份证号不合法'
                            , skin: 'msg'
                            , time: 2 //2秒后自动关闭
                        });
                        return false
                    }
                    // 手持身份证
                    if (!this.IdNumber) {
                        layer.open({
                            content: '手持身份证不能为空'
                            , skin: 'msg'
                            , time: 2 //2秒后自动关闭
                        });
                        return false
                    }
                } else {//企业
                    // 企业名称
                    if (!this.CorporateName) {
                        layer.open({
                            content: '企业名称不能为空'
                            , skin: 'msg'
                            , time: 2 //2秒后自动关闭
                        });
                        return false
                    }
                    // 营业执照
                    if (!this.isMobileNumber) {
                        layer.open({
                            content: '营业执照不能为空'
                            , skin: 'msg'
                            , time: 2 //2秒后自动关闭
                        });
                        return false
                    }
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
                if(this.personal && !this.enterprise){
                    obj={
                        Mobile: _this.MobilePhone,
                        ValidateCode:_this.VerificationCode,
                        Type:1002,
                        TrueName:_this.RealName,
                        IdentityNo:_this.IdNumber,
                        IDCardFrontURL:_this.serverId?_this.serverId:_this.photoimg
                    }
                }else {
                    obj={
                        Mobile: _this.MobilePhone,
                        ValidateCode:_this.VerificationCode,
                        Type:1001,
                        TrueName:_this.CorporateName,
                        BLicenceURL:_this.serverId?_this.serverId:_this.BLicenceURL
                    }
                }
                obj.AccountType=96001
                obj.AccountName=_this.Alipay;
                obj.action="SaveAttestation";
                layer.open({
                    content: '保存后，除手机号和支付宝账号，其他信息不可修改'
                    ,btn: ['确认保存', '取消'],
                    yes: function(index){          //点确定按钮触发的回调函数，返回一个参数为当前层的索引
                        layer.close(index);
                        layer.open({
                            type: 2,
                            shadeClose:false
                        });
                        // 保存接口
                        $.ajax({
                            url:public_url+'/AjaxServers/LoginManager.ashx',
                            type:'post',
                            dataType:'json',
                            data:obj,
                            xhrFields: {
                                withCredentials: true
                            },
                            crossDomain: true,
                            success:function (data) {
                                if (data.Status==0){
                                    if (GetRequest().return==1){
                                        layer.closeAll()
                                        layer.open({
                                            content: "成功"
                                            , skin: 'msg'
                                            , time: 2 //2秒后自动关闭
                                            ,success: function(elem){
                                                window.location=public_pre+"/cashManager/accountInfo.html"
                                            }
                                        });
                                    }else {
                                        $('#PreserVation').attr('class','Thegrey');
                                        layer.closeAll();
                                        layer.open({
                                            content: "成功"
                                            , skin: 'msg'
                                            , time: 2 //2秒后自动关闭
                                            ,success: function(elem){
                                                window.location.reload()
                                            }
                                        });
                                    }
                                }else {
                                    layer.closeAll()
                                    layer.open({
                                        content: data.Message
                                        , skin: 'msg'
                                        , time: 2 //2秒后自动关闭
                                    });
                                }
                            }
                        })
                    },no:function(index){                   //点取消按钮触发的回调函数
                        layer.close(index)
                    }
                });

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