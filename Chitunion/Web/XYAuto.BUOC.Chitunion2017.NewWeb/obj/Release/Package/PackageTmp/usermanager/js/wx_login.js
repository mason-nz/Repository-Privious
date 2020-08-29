
var WXLoginHelper = window.WXLoginHelper = (function () {
    //调用接口前缀地址
    var posturl = '/wx';//http://wxtest-ct.qichedaquan.com
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

    var setCookie = function (name, value, expir) {
        var cookie = name + '=' + (value);
        if (expir !== void 0) {
            var now = new Date();
            now.setDate(now.getDate() + ~~expir);
            cookie += '; expires=' + now.toGMTString();
        }
        cookie += ';domain=.www1.chitunion.com;path=/;';
        document.cookie = cookie;
    };

    var Load_QRCode = function (categoryid) {
        //var imgError = $('img.imgerror');
        $('div.main div.left div.two_rules div.we_code div[name="qr_timeoutTag"]').remove();
        var loginTypeype = -2;
        if (categoryid == 29001) {//广告主
            loginTypeype = 2;
            $('div.main div.left div.two_rules div.role_advertisers div.we_code').show();
        }
        else if (categoryid == 29002) {//媒体主
            loginTypeype = 1;
            $('div.main div.left div.two_rules div.role_media div.we_code').show();
        }
        var imgObj = $('div.main div.left div.two_rules div.we_code img.code');
        if (!imgObj || loginTypeype <= 0) {
            alert('参数错误');
            return;
        }

        var url = posturl + "/api/OAuth2/GetLoginQr";
        var setting = {};
        setting.data = { LoginType: loginTypeype };
        setting.url = url;
        setting.type = 'post';
        setAjax(setting, function (data) {
            if (data && data.Result && data.Result.Url) {
                var imgUrl = data.Result.Url;
                var ticket = data.Result.Ticket;
                imgObj.attr('src', imgUrl).show();
                imgObj.parent('div.we_code').data('loadTicket', ticket);
                VerifyLoad_QRCode(imgObj);
            }
        }, function (error) {
            //alert('调用失败');
        });
    };
    var timeOutIndex;
    var SetTimeOutIndex = function () {
        return timeOutIndex;
    }

    //轮询验证二维码登陆
    var VerifyLoad_QRCode = function (imgObj) {
        var countdown = 180;
        settime(imgObj);
        function settime(imgObj) {
            if (countdown == 0) {
                console.log('调用终止');
                var category = $('ul.nav li.selected:first').attr('category');
                var divobj = "<div name='qr_timeoutTag'>请<a style='color:#0D74F5' href='javascript:void(0);' onclick='javascript:WXLoginHelper.Load_QRCode(" + category + ");'>刷新</a>后重试</div>";
                imgObj.siblings('div[name="qr_timeoutTag"]').remove();
                imgObj.removeAttr('src').hide();
                imgObj.after(divobj);
                return;
            }
            else {
                countdown--;
            }
            //var imgError = $('img.imgerror');
            if (!imgObj || !imgObj.parent('div.we_code').data('loadTicket')) {
                alert('参数错误');
                return;
            }
            var ticket = imgObj.parent('div.we_code').data('loadTicket');

            var url = posturl + "/api/OAuth2/VerifyLogin";
            var setting = {};
            setting.data = { Ticket: ticket };
            setting.url = url;
            setting.type = 'post';
            setAjax(setting, function (data) {
                if (data && data.Status == 0) {
                    //alert('扫码登陆成功');
                    var category = $('ul.nav li.selected:first').attr('category');
                    //alert(data.Ticket);
                    setCookie('ct-ouinfo', data.Result.Ticket);
                    imgObj.siblings('p[name="pwx_tag"]').hide();
                    imgObj.siblings('p.bottom').hide();
                    imgObj.siblings('p[name="pwx_OK"]').show();
                    
                    setTimeout(function () {//两秒后跳转  
                        switch (category) {
                            case '29001':
                                window.location = '/manager/advertister/personal/PersonCenter.html';
                                break;
                            case '29002':
                                window.location = '/manager/media/personal/personalCenter.html';
                                break;
                            default:
                                window.location = '/usermanager/NotAccessMsgPage.html';
                                break;
                        }
                    }, 2000);
                }
            }, function (error) {
                console.log('调用接口失败');
            });

            timeOutIndex = setTimeout(function () {
                settime(imgObj)
            }, 1000);
        }

    };

    return {
        Load_QRCode: Load_QRCode,
        VerifyLoad_QRCode: VerifyLoad_QRCode,
        SetTimeOutIndex: SetTimeOutIndex
    }
})();

$(function () {
    //绑定微信登陆事件
    $('a[name="aWxLogin"]').unbind('click').bind('click', function () {
        var categoryID = $('ul.nav li.selected:first').attr('category');
        var divObj = null;
        if (categoryID == 29001) {
            divObj = $('div.role_advertisers');
        }
        else if (categoryID == 29002) {
            divObj = $('div.role_media');
        }

        if (!divObj.find('div.we_code').is(':visible')) {
            divObj.find('div.account').hide();
            divObj.find('div.we_code').show();
            //WXLoginHelper.Load_QRCode(categoryID, divObj.find('div.we_code img.code'));
            WXLoginHelper.Load_QRCode(categoryID);
        }
    });

    //绑定账号登陆事件
    $('a[name="aAcctLogin"]').unbind('click').bind('click', function () {
        var categoryID = $('ul.nav li.selected:first').attr('category');

        if (WXLoginHelper.SetTimeOutIndex()) {
            clearTimeout(WXLoginHelper.SetTimeOutIndex());
        }
        var url = (window.location.pathname + '').toLowerCase();

        if (url.indexOf('login.aspx') >= 0) {
            var divObj = null;
            if (categoryID == 29001) {
                divObj = $('div.role_advertisers');
            }
            else if (categoryID == 29002) {
                divObj = $('div.role_media');
            }

            if (divObj.find('div.we_code').is(':visible')) {
                divObj.find('div.account').show();
                divObj.find('div.we_code').hide();
            }
        }
        else {
            var paraType = request('Type');
            var gourl = request('gourl');
            var loginUrl = '/login.aspx';
            if (loginUrl != '') {
                gourl = gourl + '?gourl=' + loginUrl;
            }
            if (paraType != '') {
                loginUrl = loginUrl + '?Type=' + paraType;
            }
            window.location = loginUrl;
        }


    });
});