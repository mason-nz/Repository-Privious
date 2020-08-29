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
};
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
            BaseMediaId:GetRequest().BaseMediaId,
        }
    },function (data) {
        if(data.Result?data.Result:null){
            $('#essentia').html(ejs.render($('#essentialinformation').html(), data));
            //案例展示部分
            setAjax({
                url:'/api/media/GetAuditInfo?v=1_1',
                type:'get',
                data:{
                    BusinessType:14002,
                    MediaId:GetRequest().MediaId,
                    // PubId:-2
                }
            },function (data) {
                console.log(data.Result[0].PubStatus);
                if(data.Result[0].PubStatus == 43002){
                    $(".switch").show();
                    $("#case").on("click", function () {
                        $("#case").addClass('current');
                        $("#basic").removeClass('current');
                        $("#case_div").show();
                        $("#Hidebasicinformation").hide();
                        $("#Relevant").hide();
                        $("#Audit").hide();
                        setAjax({
                            url: "/api/Media/SelectMediaCaseInfo?v=1_1&MediaType=14002&MediaID=" + GetRequest().MediaId + "&CaseStatus=1"
                        }, function (data) {
                            console.log(data);
                            //填充案例页面
                            if (data.Result.length!=0&&data.Result[0].CaseContent) {
                                $("#case_div").html(data.Result[0].CaseContent);
                                if($("#case_div").html().trim().length==0){
                                    $("#case").hide();
                                }
                            }
                        })
                    });
                    setAjax({
                        url: "/api/Media/SelectMediaCaseInfo?v=1_1&MediaType=14002&MediaID=" + GetRequest().MediaId + "&CaseStatus=1"
                    }, function (data) {
                        console.log(data);
                        //填充案例页面
                        if (data.Result.length==0||data.Result[0].CaseContent=="") {
                            $("#case").hide();
                        }
                    })
                    $("#basic").on("click", function () {
                        $("#basic").addClass('current');
                        $("#case").removeClass('current');
                        $("#case_div").hide();
                        $("#Hidebasicinformation").show();
                        $("#Relevant").show();
                        $("#Audit").show();
                    });
                }
            })
        }
    })


    if(CTLogin.RoleIDs=='SYS001RL00003'||GetRequest().See==1){
        // 相关资质
        setAjax({
            url:'/api/Media/GetAppQualification?v=1_1',
            type:'get',
            data:{
                MediaType:14002,
                MediaId:GetRequest().MediaId
            }
        },function (data) {
            if(data.Result?data.Result:null){
                $('#Relevant').html(ejs.render($('#Relevantquali').html(), data));
            }
        })

// 审核信息
        setAjax({
            url: '/api/media/GetAuditInfo?v=1_1',
            type: 'get',
            data: {
                BusinessType:14002,
                MediaId:GetRequest().MediaId
            }
        },function (data) {
            if(data.Result.length?data.Result:null){
                console.log(1);
                $('#Audit').html(ejs.render($('#Auditinformation').html(), data));

                if(data.Result[0].PubStatus==43003){
                    $('.button').show();
                }else {
                    $('.button').hide();
                }
            }
        })

    };


    // 提交按钮
    if(CTLogin.RoleIDs=='SYS001RL00003'){
        $('.button').html('修改再提交').off('click').on('click',function () {
            window.location='/mediamanager/addAppmedia.html?MediaId='+GetRequest().MediaId+'&OperateType=2&BaseMediaId='+GetRequest().BaseMediaId
        })
        if(GetRequest().Reject!=1){
            $('.keep').hide();
        }
    }else {
        $('.button').html('编辑媒体信息').off('click').on('click',function () {
            window.location='/mediamanager/addAppmedia.html?MediaId='+GetRequest().MediaId+'&OperateType=2&BaseMediaId='+GetRequest().BaseMediaId
        })
    };
    if(GetRequest().See==1){
        $('.keep').hide();
    }




})
