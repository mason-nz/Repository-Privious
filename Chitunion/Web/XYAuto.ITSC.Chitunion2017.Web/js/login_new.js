

var LoginHelper = window.LoginHelper = (function () {

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

    var Login = function () {
        var name = $('#txtNBYHUserName');
        var pwd = $('#txtNBYHPwd');
        var checkCode = $('#txtNBYHUCheckCode');
        var errorMsg = $('#H2ErrorNBYHMsg');
        var imgCheck = $('#imgNBYH');

        if (name.val() == '') {
            errorMsg.text('请输入手机号'); name.focus(); return;
        }
        else if (pwd.val() == '') {
            errorMsg.text('请输入密码'); pwd.focus(); return;
        }
        else if (checkCode.val() == '') {
            errorMsg.text('请输入验证码'); checkCode.focus(); return;
        }


        var url = "/AjaxServers/LoginManager.ashx";

        var agentNum = '';
        var postBody = "username=" + URLencode(name.val()) + "&pwd=" + URLencode(pwd.val()) + "&checkCode=" + URLencode(checkCode.val()) + "&isInside=1&gourl=" + URLencode(request('gourl')); //构造要携带的数据 
        AjaxPost(url, postBody, PostPalyCallbackLoad);

        function PostPalyCallbackLoad(data, textStatus, xhr) {
            if (textStatus == 'success') {
                var s = data.split(',')[0];

                switch (s) {
                    case '-6':
                        errorMsg.text("验证码不正确"); checkCode.focus();
                        imgCheck.triggerHandler('click');
                        break;
                    case '-7'://库存经销商登陆接口，验证失败时
                        errorMsg.text(unescape(data.split(',')[1]));
                        imgCheck.triggerHandler('click');
                        break;
                    case '-8'://库存经销商登陆接口，登陆成功后，但账号状态为停用时
                        window.location = unescape(data.split(',')[1]);
                        break;
                    case '-3':
                        errorMsg.text("帐号已停用"); imgCheck.triggerHandler('click');
                        break;
                    case '-2':
                        errorMsg.text("帐户不存在"); imgCheck.triggerHandler('click');
                        break;
                    case '1'://登陆验证成功
                        window.location = data.split(',')[1];
                        break;
                    default:
                        errorMsg.text("用户名密码不匹配"); imgCheck.triggerHandler('click');
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
            Login();
        }
    }

    return {
        Login: Login,
        KeyDown: KeyDown
    }
})();