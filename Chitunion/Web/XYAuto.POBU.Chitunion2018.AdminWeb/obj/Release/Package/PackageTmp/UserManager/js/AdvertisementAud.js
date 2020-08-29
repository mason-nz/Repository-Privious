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
        url:public_url+'/api/MediaUser/GetMediaUserDetailInfo',
        // url:'json/AdvertisementAud.json',
        type:'get',
        data:{
            UserID:GetRequest().UserID
        }
    },function (data) {
        if(data.Status==0){
            $('.aud').html(ejs.render($('#aud').html(), data));
            operation()
        }else {
            layer.msg(data.Message)
        }
    }) 
    function operation() {
        $('input[type=radio]').eq(0).off('click').on('click',function () {
            $('#s_h').hide();
        })
        $('input[type=radio]').eq(1).off('click').on('click',function () {
            $('#s_h').show();
        })
        $('.button').off('click').on('click',function () {
            if($('input[type=radio]:checked').length==0){
                layer.msg('请选择通过或不通过')
                return false;
            }
            if($('input[type=radio]:checked').val()==3&&$.trim($('#s_h input').val())==''){
                layer.msg('请填写驳回原因')
                return false;
            }
            console.log(1);
            setAjax({
                url:public_url+'/api/MediaUser/UserCertificationAudit',
                type:'post',
                data:{
                    "UserID":GetRequest().UserID,
                    "Status":$('input[type=radio]:checked').val(),
                    "Reason":$('#s_h input').val()
                }
            },function (data) {
                if(data.Status==0){
                    layer.msg('成功',{time:1500},function () {
                        window.location="/usermanager/advertisementuserlist.html"
                    })
                }else {
                    layer.msg(data.Message)
                }
            })
        })
        $(window).scroll(function(){
            var ws = $(window).scrollTop();
            var dh = $(document).height();
            var wh = $(window).height();
            //底部操作栏：提交，放弃等
            if(ws < dh - wh){
                $('.keep').addClass("keep_fixed3").removeClass("keep_fixed")
            }else{
                $('.keep').removeClass("keep_fixed3").addClass("keep_fixed")
            }

        })
    }
})
