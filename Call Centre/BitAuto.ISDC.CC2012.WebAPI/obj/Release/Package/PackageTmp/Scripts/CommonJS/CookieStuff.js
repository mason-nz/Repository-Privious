/**
* Read the JavaScript cookies tutorial at:
*   http://www.netspade.com/articles/javascript/cookies.xml
*/

/**
* Sets a Cookie with the given name and value.
*
* name       Name of the cookie
* value      Value of the cookie
* [expires]  Expiration date of the cookie (default: end of current session)
* [path]     Path where the cookie is valid (default: path of calling document)
* [domain]   Domain where the cookie is valid
*              (default: domain of calling document)
* [secure]   Boolean value indicating if the cookie transmission requires a
*              secure transmission
*/
function setCookie(name, value, expires, path, domain, secure) {
    document.cookie = name + "=" + escape(value) +
        ((expires) ? "; expires=" + expires.toGMTString() : "") +
        ((path) ? "; path=" + path : "") +
        ((domain) ? "; domain=" + domain : "") +
        ((secure) ? "; secure" : "");
};
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
};
/**
* Gets the value of the specified cookie.
*
* name  Name of the desired cookie.
*
* Returns a string containing value of specified cookie,
*   or null if cookie does not exist.
*/
function getCookie(name) {
    var dc = document.cookie;
    var prefix = name + "=";
    var begin = dc.indexOf("; " + prefix);
    if (begin == -1) {
        begin = dc.indexOf(prefix);
        if (begin != 0) return null;
    }
    else {
        begin += 2;
    }
    var end = document.cookie.indexOf(";", begin);
    if (end == -1) {
        end = dc.length;
    }
    return unescape(dc.substring(begin + prefix.length, end));
};

/**
* Deletes the specified cookie.
*
* name      name of the cookie
* [path]    path of the cookie (must be same as path used to create cookie)
* [domain]  domain of the cookie (must be same as domain used to create cookie)
*/
function deleteCookie(name, path, domain) {
    if (getCookie(name)) {
        document.cookie = name + "=" +
            ((path) ? "; path=" + path : "") +
            ((domain) ? "; domain=" + domain : "") +
            "; expires=Thu, 01-Jan-70 00:00:01 GMT";
    }
};


//输入回车时产生的事件，兼容IE和火狐
function KeyDown(evt) {
    //这行代码用于兼容IE和火狐
    evt = evt = (evt) ? evt : ((window.event) ? window.event : "");
    if (evt.keyCode == 13) {
        evt.returnValue = false;
        evt.cancel = true;
        lgnP.login();
    }
};

function RemoveStyleAndText(obj) {
    if (obj.style.color != "black") {
        obj.value = "";
        obj.style.color = "black";
    }
};
function ChangeText(obj) {
    if (obj.value.length == 0) {
        obj.value = "请使用域帐号登录";
        obj.style.color = "#cccccc";
    }
    else {
        obj.style.color = "black";
    }
};
function URLencode(sStr) {
    return escape(sStr).replace(/\+/g, '%2B').replace(/\"/g, '%22').replace(/\'/g, '%27').replace(/\//g, '%2F');
};
