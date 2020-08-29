/*
* Written by:     wangcan
* function:       广告模板详情
* Created Date:   2017-06-09
* Modified Date:
*/


$(function () {
    $.ajax({
        url: '/api/Template/GetInfo',
        type: 'get',
        async: false,
        dataType: 'json',
        xhrFields: {
            withCredentials: true
        },
        crossDomain: true,
        data: {
            "AdTempId": GetRequest().AdTempId,
            "BusinessType":15000
        },
        success:function(data){
            if(data.Status == 0){
               $('#container').html(ejs.render($('#advDetail').html(), {data:data.Result}));
               $('.AdTempStyle').find('.add_earlier:first').hide();
               $('.SellingPlatform').find('.add_earlier:first').hide();
               $('.AdSaleAreaGroup').find('.add_earlier:last').hide();
            }
        }
    })
    var upData = {
        BusinessType:14002,
        TemplateId:GetRequest().AdTempId,
        PubId:-2,
        MediaId:-2
    }
    $.ajax({
        url:'/api/media/GetAuditInfo?v=1_1',
        type:'get',
        dataType: 'json',
        xhrFields: {
            withCredentials: true
        },
        crossDomain: true,
        data:upData,
        success:function(data){
            if(data.Status == 0){
                $('.AdSaleAreaGroup').on('mouseover','.groupName',function(){
                    if($(this).parent('li').attr('GroupType') == 1){
                        $(this).next().next().show();
                    }
                }).on('mouseout','.groupName',function(){
                    $(this).next().next().hide();
                })
                if($('.auditResult').attr('AuditStatus')!='48002'){

                    $('.auditTime').html(data.Result[0].CreateTime);
                    if($('.auditResult').find('li').eq(1).html() == '驳回'){
                        $('#rejectReson').html(data.Result[0].RejectMsg);
                    }else{
                        $('.rejectReson').remove();
                        $('.keep').remove();
                    }
                }
                if(data.Result.length != 0 && $('.auditResult').attr('AuditStatus')=='48002'){
                    $('.auditTime').html(data.Result[0].CreateTime);
                }
            }
        }
    })

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
// 通过url获取文件名称
function getFileName(o){
    if(o == null) return '';
    var pos=o.lastIndexOf("/");
    var str = o.substring(pos+1);
    var pos1 = str.indexOf('$');
    return str.substr(0,pos1);
}
