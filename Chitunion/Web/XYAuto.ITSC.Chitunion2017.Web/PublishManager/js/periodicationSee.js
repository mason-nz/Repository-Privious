// 获取url参数
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
};
$(function () {


    function periodicationSee(parameter) {
        // 渲染
        this.Judgerole(parameter);
    }

    periodicationSee.prototype = {
        constructor: periodicationSee,
        // 渲染
        Judgerole: function (parameter) {
            // 渲染
            this.Rendering(parameter);
        },
        // 渲染
        Rendering: function (parameter) {
            // 请求数据
            this.Requestdata(parameter);
        },
        // 请求数据
        Requestdata: function (parameter) {
            var _this = this;
            setAjax({
                url: '/api/Periodication/SelectWXPublishByIDAndType',
                type: 'GET',
                data: parameter.GetRequest
            }, function (data) {
                if(data.Status == 0){
                    $('.install_box').html(ejs.render($('#rendering').html(), data))
                    _this.Bigpicture();
                    // 操作
                    _this.operationbutton();
                }else{
                    layer.msg(data.Message,{'time':1000});
                    window.close();
                }
            })
        },
        // 显示大图
        Bigpicture: function () {
            // 添加样式
            $('.ad_map').css('position', 'relative')
                .find('.Bigpicture').css({'position': 'absolute', 'left': '100%', 'top': 0,'z-index': 100})
            // 默认大图隐藏
            $('.Bigpicture').hide()
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

        },
        // 操作按钮
        operationbutton: function () {
            // 点击关闭
            $('#shut').off('click').on('click',function () {
                window.location = '/publishmanager/pricelist-wechatnew.html';
            })
        }

    }
    var parameter = {
        CTLogin: CTLogin,//用户信息
        GetRequest: GetRequest()//url参数
    }
    var periodicationsee = new periodicationSee(parameter);
})
