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
    // 基本信息
    setAjax({
        url:'http://www.chitunion.com/api/media/GetItemForBack?v=1_1',
        type: 'get',
        data:GetRequest()
    },function (data) {
        console.log(data);
        if(data.Result.AuditStatus!=43001){
            layer.msg('该媒体已审核，即将跳转到待审核列表页',{time:2000},function () {
                window.location='/mediamanager/mediaauditall.html';
            });
        }
        if(!(!data.Result.CommonlyClassStr)){
            data.Result.CommonlyClassStr=(data.Result.CommonlyClassStr).split(',');
            if(data.Result.CommonlyClassStr.length>0){
                data.Result.CommonlyClassStr.length=data.Result.CommonlyClassStr.length-1
            }
            console.log(data.Result.CommonlyClassStr);
        }else {
            data.Result.CommonlyClassStr=[]
        }
        $('#information').html(ejs.render($('#information1').html(), data));
        $.ajax({
            url:'http://www.chitunion.com/api/media/GetAuditDetailInfo?v=1_1',
            type: 'get',
            data: GetRequest(),
            async:false,
            dataType: 'json',
            xhrFields: {
                withCredentials: true
            },
            crossDomain: true,
            success: function (data) {
                var a=true;
                if(data.Result.BaseMediaInfo!=null){
                    var obj1={
                        FansCount:data.Result.BaseMediaInfo.FansCount,
                        FansCountURL:data.Result.BaseMediaInfo.FansCountURL,
                        CommonlyClassStr:data.Result.BaseMediaInfo.FansCountURL,
                        FansArea:data.Result.BaseMediaInfo.FansArea,
                        FansAreaShotUrl:data.Result.BaseMediaInfo.FansArea,
                        FansFemalePer:data.Result.BaseMediaInfo.FansFemalePer,
                        FansMalePer:data.Result.BaseMediaInfo.FansMalePer,
                        FansSexScaleUrl:data.Result.BaseMediaInfo.FansSexScaleUrl,
                        OrderRemark:data.Result.BaseMediaInfo.OrderRemark,
                        CityID:data.Result.BaseMediaInfo.CityID,
                        ProvinceID:data.Result.BaseMediaInfo.ProvinceID
                    };
                    var obj2={
                        FansCount:data.Result.MediaInfo.FansCount,
                        FansCountURL:data.Result.MediaInfo.FansCountURL,
                        CommonlyClassStr:data.Result.MediaInfo.FansCountURL,
                        FansArea:data.Result.MediaInfo.FansArea,
                        FansAreaShotUrl:data.Result.MediaInfo.FansArea,
                        FansFemalePer:data.Result.MediaInfo.FansFemalePer,
                        FansMalePer:data.Result.MediaInfo.FansMalePer,
                        FansSexScaleUrl:data.Result.MediaInfo.FansSexScaleUrl,
                        OrderRemark:data.Result.MediaInfo.OrderRemark,
                        CityID:data.Result.MediaInfo.CityID,
                        ProvinceID:data.Result.MediaInfo.ProvinceID
                    }
                    if(JSON.stringify(obj1)!=JSON.stringify(obj2)){
                        a=false;
                    }
                }else {
                    a=false;
                }
                if(a){
                    $('#Mediacomparison').hide();
                }else {
                    $('#Mediacomparison').show();
                }
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
            obj.MediaID=data.Result.MediaID;
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
            obj.MediaType=GetRequest().MediaType
            setAjax({
                url:'http://www.chitunion.com/api/Media/ToExamineMedia?v=1_1',
                type:'get',
                data:obj
            },function (data) {
                // return
                if(data.Status==0){
                    if(data.Result!=null){
                        if(data.Result!=0){
                            layer.msg('审核成功',{time: 2000},function () {
                                window.location = "/MediaManager/mediaaudit.html?MediaType=14001&Wx_Status=43001&MediaID=" + data.Result
                            })
                        }else {
                            layer.msg('没有下一条审核信息了',{time: 2000},function () {
                                window.location='/mediamanager/mediaauditall.html'
                            })
                        }

                    }else {
                        layer.msg('没有下一条审核信息了',{time: 2000},function () {
                            window.location='/mediamanager/mediaauditall.html'
                        })
                    }
                }else {
                    layer.msg(data.Message);
                }

            })
        });
        // 媒体主信息
        setAjax({
            url: 'http://www.chitunion.com/api/media/GetQualification?v=1_1',
            type: 'get',
            data: {
                MediaId:GetRequest().MediaID,
                IsInsert:false
            }
        }, function (data) {
            console.log(data);
            if (data.Result != null) {
                if (data.Result.EnterpriseName != null) {
                    // 企业全称
                    $('#enterpriseName').html(data.Result.EnterpriseName)
                    // 营业执照
                    $('#businessLicense img').attr('src', data.Result.BusinessLicense).parent().next().find('img').attr('src', data.Result.BusinessLicense);
                    // 授权资质
                    $('#license1').attr('src', data.Result.QualificationOne).parent().next().find('img').attr('src', data.Result.QualificationOne);
                    if(!data.Result.QualificationOne){
                        $('.Qualificationsimg').hide();
                    }
                    $('#license2').attr('src', data.Result.QualificationTwo).parent().next().find('img').attr('src', data.Result.QualificationTwo);
                    if(!data.Result.QualificationTwo){
                        $('.Qualificationsimg1').hide();
                    }
                    $('.Qualificationsimg').off('mouseover').on('mouseover',function () {
                        $(this).next().show().css({'position': 'absolute','left':'75px','top':0});
                    });
                    $('.Qualificationsimg').off('mouseout').on('mouseout',function () {
                        $(this).next().hide();
                    })
                    $('.Qualificationsimg1').off('mouseover').on('mouseover',function () {
                        $(this).next().show().css({'position': 'absolute','right':'-180px','top':0});
                    });
                    $('.Qualificationsimg1').off('mouseout').on('mouseout',function () {
                        $(this).next().hide();
                    })
                } else {
                    $('#Relevantqualifications').hide();
                }
            } else {
                $('#Relevantqualifications').hide();
            }

        })
        // 点击取消
        $('#Submit2').off('click').on('click',function () {
            window.location='/mediamanager/mediaauditall.html'
        })
    })
})