
var udCapCtl = null; //插件对象
var UDCAPTURE_VERSION = "1.6.1"; //当前最新的控件版本号
var UDCAPTURE_MIME_TYPE = "application/udcapture-plugin"; //mimeType
var UDCAPTURE_SAVEFILE = "AjaxServers/FileLoad.ashx"; //后台保存图片的文件路径,aspx可以换成php或jsp
var UDCAPTURE_LICENSE = ""; //注册授权许可号
var UDCAPTURE_SETUP = "Setup/UdCapture.msi"; //安装文件

var supportActiveX = (window.ActiveXObject !== undefined); //是否支持ActiveX,IE
if (supportActiveX && (window.navigator.platform == "Win64" || window.navigator.cpuClass == "x64"))//64位浏览器安装文件
    UDCAPTURE_SETUP = "Setup/UdCapture64.msi";

var controlLoaded = false; //是否已经加载

//显示事件消息
function f_log(str) {
    if (document.getElementById("logInfo"))
        document.getElementById("logInfo").innerHTML = str;
}

//版本比较，检查是否安装了新版本
function f_hasNewVer(instVer) {
    if (instVer.substring(0, 1) == 'v')
        instVer = instVer.substring(1, instVer.length);

    var newVer = UDCAPTURE_VERSION.split(".");
    var curVer = instVer.split(".");
    if (parseInt(newVer[0]) > parseInt(curVer[0]))
        return true;
    if (parseInt(newVer[0]) == parseInt(curVer[0]) && parseInt(newVer[1]) > parseInt(curVer[1]))
        return true;
    if (parseInt(newVer[0]) == parseInt(curVer[0]) && parseInt(newVer[1]) == parseInt(curVer[1])
             && parseInt(newVer[2]) > parseInt(curVer[2]))
        return true;
    return false;
}

//IE事件注册
function f_addEvent(element, type, handler) {
    if (element.attachEvent) {
        element.attachEvent(type, handler);
    } else {
        f_attachIE11Event(element, type, handler);
    }
}
//单独处理IE11的事件
function f_attachIE11Event(obj, eventId, _functionCallback) {
    var nameFromToStringRegex = /^function\s?([^\s(]*)/;
    var paramsFromToStringRegex = /\(\)|\(.+\)/;
    var params = _functionCallback.toString().match(paramsFromToStringRegex)[0];
    var functionName = _functionCallback.name || _functionCallback.toString().match(nameFromToStringRegex)[1];
    var handler = document.createElement("script");
    handler.setAttribute("for", obj.id);
    handler.event = eventId + params;
    handler.appendChild(document.createTextNode(functionName + params + ";"));
    document.body.appendChild(handler);
};

//检查是否安装了插件
function f_installCheck() {
    if (udCapCtl)//已经启用
    {
        controlLoaded = true;
        return true;
    }

    if (supportActiveX) {//if IE               
        document.getElementById("udCapSpan").innerHTML = "<object id=\"udCaptureCtl\" width=\"0\" height=\"0\" classid=\"CLSID:0FAE7655-7C34-4DEE-9620-CD7ED969B3F2\"></object>";
        var axObj = document.getElementById("udCaptureCtl");
        if (axObj.PostUrl != undefined) {
            if (f_hasNewVer(axObj.GetVersion())) {
                if (confirm("屏幕截图控件有新版本，升级后才能使用！\r\n点确定进行升级，升级时需关闭浏览器窗口...\r\n如果您已经升级安装,请关闭后重新打开浏览器...")) {
                    f_startSetup();
                }
                return false;
            }
            else {
                udCapCtl = document.getElementById("udCaptureCtl");
                udCapCtl.PostUrl = UDCAPTURE_SAVEFILE;
                udCapCtl.License = UDCAPTURE_LICENSE;
                //以下IE事件注册
                f_addEvent(udCapCtl, "OnBeforeCapture", f_onBeforeCapture);
                f_addEvent(udCapCtl, "OnCaptureCanceled", f_onCaptureCanceled);
                f_addEvent(udCapCtl, "OnCaptureCompleted", f_onCaptureCompleted);
                f_addEvent(udCapCtl, "OnBeforeUpload", f_onBeforeUpload);
                f_addEvent(udCapCtl, "OnUploadCompleted", f_onUploadCompleted);
                f_addEvent(udCapCtl, "OnUploadFailed", f_onUploadFailed);
                return true;
            }
        }
        else {
            if (confirm("您尚未安装屏幕截图控件，马上安装？\u000d提醒：安装完毕后，请重启浏览器或者F5刷新浏览器")) {
                document.getElementById("udCapSpan").innerHTML = "";
                f_startSetup();
            }
            return false;
        }
    }
    else if (navigator.plugins)//NP
    {
        var plugin = (navigator.mimeTypes && navigator.mimeTypes[UDCAPTURE_MIME_TYPE]) ?
            navigator.mimeTypes[UDCAPTURE_MIME_TYPE].enabledPlugin : 0;
        if (plugin) {
            var pluginVersion = "v1.0.0";
            var words = plugin.description.split(" ");
            if (words[words.length - 1].substring(0, 1) == "v")
                pluginVersion = words[words.length - 1];

            if (f_hasNewVer(pluginVersion)) {
                if (confirm("屏幕截图插件有新版本，升级后才能使用！\r\n点确定进行升级...")) {
                    f_startSetup();
                }
                return false;
            }
            else {
                document.getElementById("udCapSpan").innerHTML = "<embed id=\"udCapturePlugin\" width=\"0\" height=\"0\" type=\"" + UDCAPTURE_MIME_TYPE + "\"></embed>";
                udCapCtl = document.getElementById("udCapturePlugin");
                udCapCtl.PostUrl = UDCAPTURE_SAVEFILE;
                udCapCtl.License = UDCAPTURE_LICENSE;
                //事件处理
                udCapCtl.OnBeforeCapture = "f_onBeforeCapture";
                udCapCtl.OnCaptureCanceled = "f_onCaptureCanceled";
                udCapCtl.OnCaptureCompleted = "f_onCaptureCompleted";
                udCapCtl.OnBeforeUpload = "f_onBeforeUpload";
                udCapCtl.OnUploadCompleted = "f_onUploadCompleted";
                udCapCtl.OnUploadFailed = "f_onUploadFailed";
                return true;
            }
        }
        if (confirm("您尚未安装屏幕截图控件，马上安装？\u000d提醒：安装完毕后，请重启浏览器或者F5刷新浏览器")) {
            f_startSetup();
        }
    }
    return false;
}

//开始安装插件
function f_startSetup() {
    document.getElementById("setupFrame").setAttribute("src", UDCAPTURE_SETUP); //下载文件用的隐藏iframe
    //document.getElementById("btnCapture").style.display = "none";
    //document.getElementById("btnReload").style.display = "inline";
    f_log("没有下载成功，可<a href='" + UDCAPTURE_SETUP + "' target='setupFrame'>点击此处</a>下载");
}

//重新加载插件
function f_loadPlugin() {
    if (navigator.plugins)
        navigator.plugins.refresh(false);
    //document.getElementById("btnCapture").style.display = "inline";
    //document.getElementById("btnReload").style.display = "none";
    f_log("请点击上面的按钮开始进行屏幕截图.");
}

//开始屏幕截图
function f_capture() {
    if (f_installCheck()) {
        udCapCtl.AutoMinimize = false;
        if (supportActiveX || controlLoaded || !udCapCtl.AutoMinimize) {
            //            if (document.getElementById("captureScreen").checked)
            //                udCapCtl.CaptureScreen(); //支持直接捕捉屏幕
            //            else
            udCapCtl.StartCapture();
        }
        else {
            //最小化后截图有些情况需要延迟启动才有效,主要是Google Chrome
            setTimeout(function () {
                if (document.getElementById("captureScreen").checked)
                    udCapCtl.CaptureScreen();
                else
                    udCapCtl.StartCapture();
            }, 300);
        }
    }
}

//事件处理函数
function f_onBeforeCapture() {
    f_log("开始截图...");
}
function f_onCaptureCanceled() {
    f_log("已取消截图。");
}
function f_onCaptureCompleted(file) {
    f_log("截图完成:" + file);
}
function f_onBeforeUpload(file, size) {
    f_log("<img src=\"css/img/Loading.gif\" style=\"vertical-align:text-bottom\" />&nbsp;正在上传截图...");
}
//function f_onUploadCompleted(responseText) {
//    f_log("图片上传完成.");
//    var json = eval("(" + responseText + ")"); //转换json格式，随机数防止缓存
//    document.getElementById("snapImg").innerHTML = "<img src=\"" + json.filepath + "?" + Math.random() + "\">"
//}
function f_onUploadFailed(errorCode) {
    f_log("图片上传失败,错误代码:" + errorCode);
}

//输出控件内容
document.writeln('<span id="udCapSpan"></span>');
document.writeln('<iframe src="" id="setupFrame" style="display:none"></iframe>'); //下载文件用的隐藏iframe
