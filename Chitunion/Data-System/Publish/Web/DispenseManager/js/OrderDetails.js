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
    function OrderDetails() {
        this.init()
    }
    OrderDetails.prototype={
        constructor:OrderDetails,
        init:function () {
            setAjax({
                url:public_url+'/api/chitu/OrderDetial',
                // url:'json/OrderDetial.json',
                type:'get',
                data:{
                    OrderId:GetRequest().orderid
                }
            },function (data) {
                if(data.Status==0){
                    $('.ad_audit').html(ejs.render($('#details').html(), data));
                }else {
                    layer.msg(data.Message)
                }
            })
        }
    }
    new OrderDetails()
})