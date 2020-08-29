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


    var OAuthType;

    function Viewedia(GetRequest) {
        // 切换
        this.switch();
        // 请求数据
        this.Requestdata(GetRequest)
    }

    Viewedia.prototype = {
        constructor: Viewedia,
        // 切换
        switch: function () {
            $('#switch1').off('click').on('click', function () {
                $(this).css({'color': '#FF4F4F'})
                $('#switch2').css({'color': '#666'})
                $('#information').css('display', 'block')
                $('#case').css('display', 'none');
                $('#qualifications').show();
                $('#Auditinformation').show()
            })
            $('#switch2').off('click').on('click', function () {
                $(this).css({'color': '#FF4F4F'})
                $('#switch1').css({'color': '#666'})
                $('#case').css('display', 'block')
                $('#information').css('display', 'none');
                $('#qualifications').hide();
                $('#Auditinformation').hide()
            })
        },
        // 请求数据
        Requestdata: function (GetRequest) {
            // 基本信息
            setAjax({
                url: 'http://www.chitunion.com/api/media/GetItemForBack?v=1_1',
                type: 'get',
                data: GetRequest
            }, function (data) {
                console.log(data);
                if (!(!data.Result.CommonlyClassStr)) {
                    data.Result.CommonlyClassStr = (data.Result.CommonlyClassStr).split(',');
                    if (data.Result.CommonlyClassStr.length > 0) {
                        data.Result.CommonlyClassStr.length = data.Result.CommonlyClassStr.length - 1
                    }
                } else {
                    data.Result.CommonlyClassStr = []
                }
                $('.frame_n img').attr('src', data.Result.HeadIconURL);
                $('.wechat_name').html('<div>微信名称：' + data.Result.Name + '</div><div>微信帐号：' + data.Result.Number + '</div>')
                $('#information').html(ejs.render($('#information1').html(), data));
                OAuthType = data.Result.AuthType
            })
            // 审核信息
            setAjax({
                url: 'http://www.chitunion.com/api/media/GetAuditInfo?v=1_1',
                type: 'get',
                data: GetRequest
            }, function (data) {
                if (data.Result.length > 0) {
                    $('#Auditinformation').html(ejs.render($('#Auditinformation1').html(), data));
                    if (data.Result[0].PubStatusName == '驳回') {
                        $('#Editbasicinformation').show()
                    } else {
                        $('#Editbasicinformation').hide()
                    }
                }


            });
            // 关联媒体主
            setAjax({
                url: 'http://www.chitunion.com/api/media/GetItemForMediaRole?v=1_1',
                type: 'get',
                data: {mediaId: GetRequest.MediaID}
            }, function (data) {
                if (data.Result != null) {
                    $('#Associatedmedia').html(ejs.render($('#Associatedmedia1').html(), data))
                }
            })
            // 案例信息
            GetRequest.CaseStatus = 1
            setAjax({
                url: 'http://www.chitunion.com/api/Media/SelectMediaCaseInfo?v=1_1',
                type: 'get',
                data: GetRequest
            }, function (data) {
                if (data.Result.length) {
                    $('#case').html(data.Result[0].CaseContent);
                } else {
                    $('#switch2').hide();
                    $('.switch').hide();
                }
            })

            // 相关资质
            setAjax({
                url: 'http://www.chitunion.com/api/media/GetQualification?v=1_1',
                type: 'get',
                data: {MediaId: GetRequest.MediaID,
                    IsInsert:false}
            }, function (data) {
                if (CTLogin.RoleIDs != 'SYS001RL00005') {
                    if (data.Result != null) {
                        if (data.Result.EnterpriseName != null) {
                            $('#qualifications').html(ejs.render($('#qualifications1').html(), data))
                        } else {
                        }
                    }
                }


            })
        }
    }
    var viewedia = new Viewedia(GetRequest());
    // 判断显示隐藏
    if (CTLogin.RoleIDs == 'SYS001RL00004' || CTLogin.RoleIDs == 'SYS001RL00001') {
        $('#switch2').hide();
        $('#Editbasicinformation').hide();
    }
    // 判断显示隐藏
    if (CTLogin.RoleIDs == 'SYS001RL00003') {
        $('#Editbasicinformation').hide();
    }
    // 关闭
    $('#Close').off('click').on('click', function () {
        window.location = '/MediaManager/mediaWeChatList_new.html'
    })

    $('#Editbasicinformation').off("click").on('click', function () {
        var a;
        if (CTLogin.RoleIDs != 'SYS001RL00003') {
            a = 1;
        }
        window.location = '/MediaManager/addWeChatmedia.html?businesstype=' + GetRequest().MediaType + '&wxae=' + a + '&WxID=' + GetRequest().MediaID + '&OAuthType=' + OAuthType
    })

})