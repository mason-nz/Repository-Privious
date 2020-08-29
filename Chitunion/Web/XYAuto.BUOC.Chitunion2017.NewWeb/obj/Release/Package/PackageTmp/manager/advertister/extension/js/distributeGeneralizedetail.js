$(function(){
    //查看内容分发
    var DetailContentDistribution = {
        init : function () {
            // url = './json/distributeGenralizedetail.json'
            var RecID = GetRequest().RecID;
            setAjax({
                url : public_url+'/api/ContentDistribute/GetContentDistributeDetailInfo',
                type : 'get',
                data : {
                    RecID : RecID
                }
            },function (data) {
                if(data.Status == 0){
                    $('.examinemedia_realization').html(ejs.render($('#examinemedia_realization').html(),{data:data.Result}))
                    // 订单图片：鼠标移入时可下载
                    $('.display_img').off('mouseover').on('mouseover',function(){
                        $(this).find('.download_img').show();
                    }).off('mouseout').on('mouseout',function(){
                        $(this).find('.download_img').hide();
                    })
                }else{
                    layer.msg(data.Message,{'time':2000});
                }
            })
        }
    };

    DetailContentDistribution.init();
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
})