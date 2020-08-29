//获取Url上的参数
function GetRequest() {
    var url = location.search; //获取url中"?"符后的字串
    var theRequest = new Object();
    if (url.indexOf("?") != -1) {
        var str = url.substr(1);
        strs = str.split("&");
        for (var i = 0; i < strs.length; i++) {

            theRequest[strs[i].split("=")[0]] = strs[i].split("=")[1];
        }
    }
    return theRequest;
}
if(GetRequest().MediaId==undefined){
    layer.msg('媒体id不能为空',{time:1300},function () {
        window.location='/mediamanager/mediaapp.html'
    })
};

$(function () {
    // 基本信息
    setAjax({
        url:'/api/media/GetInfo?v=1_1',
        type:'get',
        data:{
            businesstype:14002,
            MediaId:GetRequest().MediaId,
            baseMediaId:GetRequest().BaseMediaId,
        }
    },function (data) {
        if(data.Result?data.Result:null){
            if(data.Result.AuditStatus!=43001){
                layer.msg('该媒体已审核，即将跳转到待审核列表页',{time:2000},function () {
                    window.location='/mediamanager/mediaapp.html';
                });
            }
            $('#essentia').html(ejs.render($('#essentialinformation').html(), data));
            if(data.Result.BaseMediaID){
                $('#record').hide();
            }
        }
    })



        // 相关资质
        setAjax({
            // url: '/api/media/GetInfo?v=1_1',
            url:'/api/Media/GetAppQualification?v=1_1',
            type: 'get',
            data: {
                MediaType: 14002,
                MediaID: GetRequest().MediaId
            }
        }, function (data) {
            if (data.Result ? data.Result : null) {
                $('#Relevant').html(ejs.render($('#Relevantquali').html(), data));
                // 身份证
                $('.idcard').off('mouseover').on('mouseover',function () {
                    $(this).next().show().css({'position': 'absolute','left':'75px','top':0});
                });
                $('.idcard').off('mouseout').on('mouseout',function () {
                    $(this).next().hide();
                });
                // 营业执照
                $('.businesslicense').off('mouseover').on('mouseover',function () {
                    $(this).next().show().css({'position': 'absolute','left':'75px','top':0});
                });
                $('.businesslicense').off('mouseout').on('mouseout',function () {
                    $(this).next().hide();
                });
                // 代理合同
                $('.Q1').off('mouseover').on('mouseover',function () {
                    $(this).next().show().css({'position': 'absolute','left':'75px','top':0});
                });
                $('.Q1').off('mouseout').on('mouseout',function () {
                    $(this).next().hide();
                });
                $('.Q2').off('mouseover').on('mouseover',function () {
                    $(this).next().show().css({'position': 'absolute','left':'75px','top':0});
                });
                $('.Q2').off('mouseout').on('mouseout',function () {
                    $(this).next().hide();
                });
            }
        })



    // 切换审核结果
    $('input[name=shenhe]').off('change').on('change',function(){
        console.log($(this).attr('class') == 'shenhe1');
        if($(this).attr('class') == 'shenhe1'){
            $('#parameter').hide();
        }else{
            $('#parameter').show();
        }
    });

    // 点击审核
    $('#Submit1').off('click').on('click',function () {
        var obj={};
        obj.MediaID=GetRequest().MediaId;
        obj.Status=$('input[name=shenhe]:checked').attr('i');
        if(obj.Status==43003){
            obj.RejectMsg=$('textarea').val()
            if(!$('textarea').val()){
                layer.msg('驳回信息不能为空')
                return
            }
        }
        if(obj.Status==43002){
            obj.RejectMsg=''
        }
        obj.MediaType=14002
        setAjax({
            url:'/api/Media/ToExamineMedia?v=1_1',
            type:'get',
            data:obj
        },function (data) {
            if(data.Result!=null){
                if(data.Result!=0){
                    layer.msg('审核成功',{time: 2000},function () {
                        window.location = "/MediaManager/appAuditing.html?MediaId=" + data.Result
                    })
                }else {
                    layer.msg('没有下一条审核信息了',{time: 2000},function () {
                        window.location='/MediaManager/mediaAPP.html'
                    })
                }

            }else {
                layer.msg('没有下一条审核信息了',{time: 2000},function () {
                    window.location='/MediaManager/mediaAPP.html'
                })
            }
        })
    });

    // 点击取消
    $('#Submit2').off('click').on('click',function () {
        window.location='/mediamanager/mediaAPP.html'
    })
})
