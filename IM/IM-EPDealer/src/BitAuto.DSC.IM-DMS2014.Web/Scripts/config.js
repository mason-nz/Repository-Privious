/*配置js文件，如果common.js或controlParams.js文件有修改，只需要修改版本号即可*/

var jsHash = {
    common: [{ src: "/Scripts/common.js", version: "1.2.448"}],
    OnlineService: [{ src: "OnlineService.js", version: "1.3334.7"}],
    AgentChat:[{ src: "AgentChat.js", version: "1.1.449"}],
    AspNetComet: [{ src: "/Scripts/AspNetComet.js", version: "1.443.8"}]
}

function loadJS(key) {
    var nodeJs = jsHash[key];
    for (var i = 0; i < nodeJs.length; i++) {
        document.writeln("<script type='text/javascript' src='" + nodeJs[i].src + "?version=" + nodeJs[i].version + "'></script>");
    }
}