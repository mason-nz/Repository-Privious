$(function () {
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

    setAjax({
        selector: '.list',
        url: '/api/ADOrderInfo/GetByOrderID_ADOrderInfo',
        type: "GET",
        data: {'orderid': GetRequest().orderid}//CT20170321804
    },function (data) {
        if(data.Result.ADOrderInfo.MediaType==14002){
            data.Result.ADOrderInfo.BeginTime=data.Result.ADOrderInfo.BeginTime.split(" ")[0];
            data.Result.ADOrderInfo.EndTime=data.Result.ADOrderInfo.EndTime.split(" ")[0];
        }
        $('.see').html(ejs.render($('#project1').html(), {data:data,RoleID:CTLogin.RoleIDs}));
    })
    setAjax({
        selector: '.list',
        url: '/api/ADOrderInfo/SelectSubOrderByOrderID',
        type: "GET",
        data: {'orderid': GetRequest().orderid}
    },function (data) {
        if(data.Result.length!=0){
        $('#tbody1').html(ejs.render($('#project2').html(), data));
        }else {
            $('#bimage').html('<img src="/images/no_data.png" style="margin: 70px auto; display: block">')
        }
    })
})