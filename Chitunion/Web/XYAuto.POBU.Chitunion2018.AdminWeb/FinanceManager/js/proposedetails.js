// 获取url？后面的参数
function GetRequest() {
    var url = location.search; //获取url中"?"符后的字串
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
$(function () {
    setAjax({
        url:public_url+'/api/Withdrawals/GetInfo',
        // url:"json/proposedetails.json",
        type:'get',
        data:{
            WithdrawalsId:GetRequest().recid
        }
    },function (data) {
        if(data.Status==0){
            $('.aud').html(ejs.render($('#aud').html(), data));
        }else {
            layer.msg(data.Message)
        }
    })
})