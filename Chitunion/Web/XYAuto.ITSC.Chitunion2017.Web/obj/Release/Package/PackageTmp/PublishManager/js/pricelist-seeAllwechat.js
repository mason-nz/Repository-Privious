/**
 * Created by fengb on 2017/5/15.
 */

$(function () {

    var MediaID = GetQueryString('MediaID')!=null&&GetQueryString('MediaID')!='undefined'?parseFloat(GetQueryString('MediaID')):null;

    var SeeAllWeChatData = {
        constructor : SeeAllWeChatData,
        getRequest : function () {
            var url = '/api/Periodication/SelectPublishesByMediaID';
            setAjax({
                url : url,
                type : 'get',
                data : {
                    MediaID : MediaID,
                    MediaType : 14001
                },
                dataType :'json'
            },function (data) {
                if(data.Status == 0){
                    var data = data.Result;
                    SeeAllWeChatData.showTemplate(data);
                    SeeAllWeChatData.Bigpicture();
                }
            })
        },
        showTemplate : function (data) {
            $('.install_box').html(ejs.render($('#rendData').html(), data));
            // 点击关闭
            $('#shut').off('click').on('click',function () {
                window.location = '/publishmanager/pricelist-wechatnew.html';
            })
        },
        // 显示大图
        Bigpicture: function () {
            // 添加样式
            $('.ad_map').css('position', 'relative')
                .find('.Bigpicture').css({'position': 'absolute', 'left': '100%', 'top': 0,'z-index': 100})
            // 默认大图隐藏
            $('.Bigpicture').hide();
            // 当没有图片时，不显示大图
            $('.ad_map').each(function () {
                if($(this).children('img').attr('src')!=''){
                    // 鼠标经过显示
                    $('.ad_map').off('mouseover').on('mouseover', function () {
                        $(this).find('.Bigpicture').show()
                    })
                    // 鼠标离开隐藏
                    $('.ad_map').off('mouseout').on('mouseout', function () {
                        $(this).find('.Bigpicture').hide()
                    })
                }
            })
        }
    }

    SeeAllWeChatData.getRequest();

    //获取url 地址参数方法
    function GetQueryString(name) {
        var reg = new RegExp("(^|&)"+ name +"=([^&]*)(&|$)");
        var r = window.location.search.substr(1).match(reg);
        if(r!=null)return unescape(r[2]); return null;
    }

})
