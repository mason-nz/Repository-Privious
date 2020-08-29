$(function(){
	function GetRequest() {// 获取url？后面的参数
        var url = location.search;
        var theRequest = new Object();
        if (url.indexOf("?") != -1) {
            var str = url.substr(1);
            strs = str.split("&");
            for (var i = 0; i < strs.length; i++) {
                theRequest[strs[i].split("=")[0]] = unescape(strs[i].split("=")[1]);
            }
        }
        return theRequest;
    }
    $('#viewDetail').attr('href','/manager/advertister/extension/viewOrderDetail.html?RecID='+GetRequest().RecID);
})