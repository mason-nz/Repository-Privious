


var ModifyPwdHelper = window.ModifyPwdHelper = (function () {



    //解析参数
    function request(paras) {
        var url = location.href;
        var paraString = url.substring(url.indexOf("?") + 1, url.length).split("&");
        var paraObj = {}
        for (i = 0; j = paraString[i]; i++) {
            paraObj[j.substring(0, j.indexOf("=")).toLowerCase()] = j.substring(j.indexOf("=") + 1, j.length);
        }
        var returnValue = paraObj[paras.toLowerCase()];
        if (typeof (returnValue) == "undefined") {
            return "";
        } else {
            return returnValue;
        }
    }
    function URLencode(sStr) {
        return escape(sStr).replace(/\+/g, '%2B').replace(/\"/g, '%22').replace(/\'/g, '%27').replace(/\//g, '%2F');
    }

    //是否满足用户名格式（字母、数字、-、_两种及以上组合的4-20个字符）
    function isUserName(strVal) {
        if (!(/(?!^[a-zA-Z]+$)(?!^[\d]+$)(?!^[^a-zA-Z-_\d]+$)^.{4,20}$/.test(strVal))) {
            //alert("手机号码有误，请重填");
            return false;
        }
        return true;
    }
    function AjaxPost(url, postBody, PostPalyCallbackName) {
        $.post(url, postBody, PostPalyCallbackName);
    }
    var browerObj = jQuery.browser;
    //检测是否是IE6，是-则关闭页面
    if (browerObj.msie && browerObj.version < 8) {
        alert("请升级IE版本！");
        window.opener = null; window.open('', '_self'); window.close();
        return false;
    }

    var ModifyPwd = function () {
        //var name = $('#txtGGZUserName');
        var oldPwd = $('#txtOldPwd');
        var newPwd = $('#txtNewPwd');
        var newPwdConfirm = $('#txtNewPwdConfirm');

        var errorMsg = $('#spanErrorMsg');
        $('div.order_r div.install ul li').find('img').css('visibility', 'hidden');
        $('div.order_r div.install ul li').find('span').html('');
        errorMsg.html('');

        if (oldPwd.val() == '') {
            oldPwd.parent().next('li').find('span').text('旧密码不能为空'); oldPwd.focus();
            oldPwd.parent().next('li').find('img').css('visibility', 'visible');
            return;
        }
        if (newPwd.val() == '') {
            newPwd.parent().next('li').find('span').text('密码不能为空'); newPwd.focus();
            newPwd.parent().next('li').find('img').css('visibility', 'visible');
            return;
        }
        if (newPwd.val() != newPwdConfirm.val()) {
            newPwdConfirm.parent().next('li').find('span').text('新密码与旧密码必须要一致'); newPwdConfirm.focus();
            newPwdConfirm.parent().next('li').find('img').css('visibility', 'visible');
            return;
        }

        var url = "/AjaxServers/LoginManager.ashx";

        var agentNum = '';
        var postBody = {
            action: "modifypwd",
            oldPwd: URLencode(oldPwd.val()),
            newPwd: URLencode(newPwd.val()),
            newPwdConfirm: URLencode(newPwdConfirm.val()),
            r: Math.random()
        };
        AjaxPost(url, postBody, ForgetpwdCallbackLoad);

        function ForgetpwdCallbackLoad(data, textStatus, xhr) {
            if (textStatus == 'success') {
                var jsonData = $.evalJSON(data);

                switch (jsonData.result) {
                    case -1:
                        errorMsg.text(jsonData.msg);
                        break;
                    case 0://注册成功
                        layer.msg('修改密码成功', { 'time': 2000 }, function () {
                            window.location = window.location;
                        });
                        break;
                    default:
                        errorMsg.text("修改密码失败"); 
                        break;
                }
            }
            else {
                alert('请求错误');
            }
        }
        return false;
    };

    //输入回车时产生的事件，兼容IE和火狐
    var KeyDown = function (evt) {
        //这行代码用于兼容IE和火狐
        evt = evt = (evt) ? evt : ((window.event) ? window.event : "");
        if (evt.keyCode == 13) {
            evt.returnValue = false;
            evt.cancel = true;
            ModifyPwd();
        }
    }

    return {
        ModifyPwd: ModifyPwd,
        KeyDown: KeyDown
    }
})();
