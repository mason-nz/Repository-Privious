/*配置js文件，如果common.js或controlParams.js文件有修改，只需要修改版本号即可*/

var jsHash = {
    common: [{ src: "/Js/common.js", version: "2.6"}],
    CTITool: [{ src: "/CTI/CTITool.js", version: "2.1"}],
    controlParams: [{ src: "/Js/controlParams.js", version: "1.7"}],
    ucCommon: [{ src: "/WorkOrder/UControl/ucCommon.js", version: "1.0"}],
    TemplateFiledData: [{ src: "/Js/TemplateFiledData.js", version: "1.0"}],
    bitdropdownlist: [{ src: "/Js/bit.dropdownlist.js", version: "1.0"}],
    UserControl: [{ src: "/Js/UserControl.js", version: "2.1"}]
}

function loadJS(key) {
    var nodeJs = jsHash[key];
    for (var i = 0; i < nodeJs.length; i++) {
        document.writeln("<script type='text/javascript' src='" + nodeJs[i].src + "?version=" + nodeJs[i].version + "'></script>");
    }
}