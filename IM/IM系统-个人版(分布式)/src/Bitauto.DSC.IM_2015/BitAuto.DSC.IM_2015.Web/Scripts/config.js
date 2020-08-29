/*配置js文件，如果common.js或controlParams.js文件有修改，只需要修改版本号即可*/

var jsHash = {
    common: [{ src: "/Scripts/common.js", version: "1.4.9.12333" }],
    OnlineService: [{ src: "OnlineService.js", version: "1.8.729223323.1"}],
    OnlineServiceCom: [{ src: "OnlineServiceCom.js", version: "1.4.23" }],
    AgentChat: [{ src: "AgentChat.js", version: "1.1.11452.9"}],
    AspNetComet: [{ src: "/Scripts/AspNetComet.js", version: "1.3.1254.363"}]
};

function loadJS(key) {
    var nodeJs = jsHash[key];
    for (var i = 0; i < nodeJs.length; i++) {
        document.writeln("<script type='text/javascript' src='" + nodeJs[i].src + "?version=" + nodeJs[i].version + "'></script>");
    }
}