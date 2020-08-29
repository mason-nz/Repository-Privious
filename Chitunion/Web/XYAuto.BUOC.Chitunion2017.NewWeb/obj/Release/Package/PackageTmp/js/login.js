




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

var LoginHelper = window.LoginHelper = (function () {




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

    var Login = function (categoryid) {
        var imgError = $('img.imgerror');
        if (categoryid == 29001) {//广告主
            var name = $('#txtGGZUserName');
            var pwd = $('#txtGGZPwd');
            var checkCode = $('#txtGGZCheckCode');
            var errorMsg = $('#divErrorGGZMsg');
            var imgCheck = $('#imgGGZ');
            //var chx_Readme = $('#chx_GGXReadme');
        }
        else if (categoryid == 29002) {//媒体主
            var name = $('#txtMTZUserName');
            var pwd = $('#txtMTZPwd');
            var checkCode = $('#txtMTZCheckCode');
            var errorMsg = $('#divErrorMTZMsg');
            var imgCheck = $('#imgMTZ');
            //var chx_Readme = $('#chx_MTZReadme');
        }
        
        if (Verify(categoryid)) {
            var url = "/AjaxServers/LoginManager.ashx";
            var postBody = "action=login&username=" + URLencode(name.val()) + "&pwd=" + URLencode(pwd.val()) + "&checkCode=" + URLencode(checkCode.val()) + "&category=" + categoryid + '&gourl=' + URLencode(request('gourl')); //构造要携带的数据 
            AjaxPost(url, postBody, function (data, textStatus, xhr) {
                if (textStatus == 'success') {
                    var s = data.split(',')[0];

                    switch (s) {
                        case '-6':
                            errorMsg.text("验证码不正确"); checkCode.focus();
                            imgError.css('visibility', 'visible');
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
                            errorMsg.text("帐号已停用");
                            imgError.css('visibility', 'visible');
                            imgCheck.triggerHandler('click');
                            break;
                        case '-2':
                            errorMsg.text("帐户不存在");
                            imgError.css('visibility', 'visible');
                            imgCheck.triggerHandler('click');
                            break;
                        case '1'://登陆验证成功
                            //if (categoryid == 29001) {//广告主
                            //    _hmt.push(['_trackEvent', 'button', 'click', 'login']);//添加百度代码统计，事件追踪[登陆逻辑]
                            //}
                            window.location = data.split(',')[1];
                            break;
                        default:
                            errorMsg.text("用户名密码不匹配");
                            imgError.css('visibility', 'visible');
                            imgCheck.triggerHandler('click');
                            break;
                    }
                }
                else {
                    //                alert(req.status);
                    //                alert(req.responseText);
                    alert('请求错误');
                }
            });
            return false;
        }
    };

    var Verify = function (categoryid) {
        var flag = false;
        var imgError = $('img.imgerror');
        imgError.css('visibility', 'hidden');
        if (categoryid == 29001) {//广告主
            var name = $('#txtGGZUserName');
            var pwd = $('#txtGGZPwd');
            var checkCode = $('#txtGGZCheckCode');
            var errorMsg = $('#divErrorGGZMsg');
            var imgCheck = $('#imgGGZ');
            //var chx_Readme = $('#chx_GGXReadme');
        }
        else if (categoryid == 29002) {//媒体主
            var name = $('#txtMTZUserName');
            var pwd = $('#txtMTZPwd');
            var checkCode = $('#txtMTZCheckCode');
            var errorMsg = $('#divErrorMTZMsg');
            var imgCheck = $('#imgMTZ');
            //var chx_Readme = $('#chx_MTZReadme');
        }
        $('div.two_rules.role_advertisers img:not([id]),div.two_rules.role_media img:not([id])').css('visibility', 'hidden');
        errorMsg.html('&nbsp;');

        if (name.val() == '') {
            errorMsg.text('请输入手机号'); name.focus();
            //name.parent().find('img').css('visibility', 'visible');
            imgError.css('visibility', 'visible');
        }
        else if (pwd.val() == '') {
            errorMsg.text('请输入密码'); pwd.focus();
            imgError.css('visibility', 'visible');
        }
        else if (checkCode.val() == '') {
            errorMsg.text('请输入验证码'); checkCode.focus();
            imgError.css('visibility', 'visible');
        }
        else {
            flag = true;
        }

        return flag;
    };

    //输入回车时产生的事件，兼容IE和火狐
    var KeyDown = function (evt) {
        //这行代码用于兼容IE和火狐
        evt = evt = (evt) ? evt : ((window.event) ? window.event : "");
        if (evt.keyCode == 13) {
            evt.returnValue = false;
            evt.cancel = true;
            Login($('ul.nav li.selected:first').attr('category'));
        }
    }

    return {
        Login: Login,
        KeyDown: KeyDown,
        Verify: Verify
    }
})();
