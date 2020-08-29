
// api路径
var labelApi={
    tag:'http://ip1.chitunion.com'
};

// 获取url？后面的参数
function GetRequest() {
    var url = location.search; //获取url中"?"符后的字串
    var theRequest = new Object();
    if (url.indexOf("?") != -1) {
        var str = url.substr(1);
        strs = str.split("&");
        for (var i = 0; i < strs.length; i++) {
            theRequest[strs[i].split("=")[0]] = decodeURI(strs[i].split("=")[1]);
        }
    }
    return theRequest;
}
function sortLable(arr){
    return arr.sort(function(x,y){
        y.DictId = x.DictId
    })
}

