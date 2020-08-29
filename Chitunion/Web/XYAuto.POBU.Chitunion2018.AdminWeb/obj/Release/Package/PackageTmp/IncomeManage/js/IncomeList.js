var OrderField = {
    "/ImagesNew/icon16_c.png": ["2", "/ImagesNew/icon16_b.png"],
    "/ImagesNew/icon16_a.png": ["2", "/ImagesNew/icon16_b.png"],
    "/ImagesNew/icon16_b.png": ["1", "/ImagesNew/icon16_a.png"],
}
// 获取url？后面的参数
function GetRequest() {
    var url = location.search; //获取url中"?"符后的字串
    var theRequest = new Object();
    if (url.indexOf("?") != -1) {
        var str = url.substr(1);
        strs = str.split("&");
        for (var i = 0; i < strs.length; i++) {
            theRequest[strs[i].split("=")[0]] = decodeURI(strs[i].split("=")[1]);
        }
    }
    return theRequest;
}
$(function () {
    function PresentManagement() {
        this.init()
    }
    var obj = {
        OrderBy: 1002,
        PageSize: 20
    }
    PresentManagement.prototype = {
        constructor: PresentManagement,
        // 获取条件
        ObtainConditions: function (i) {
            obj.Keyword = $('#u_name').val();
            if (i != undefined) {
                obj.PageIndex = i
            }
            return obj
        },
        init: function () {
            var _this = this;
            // 点击查询
            $('#Inquiry').off('click').on('click', function () {
                _this.query(_this.ObtainConditions(1), _this)
            })
            $('.PV').click(function () {
                var vImg = $(this).find("img").attr("src");
                $('.PV img').attr("src", "/ImagesNew/icon16_c.png");

                $(this).find("img").attr("src", OrderField[vImg][1])
                obj.OrderBy = parseInt($(this).attr("orderN")) + parseInt(OrderField[vImg][0]);
                _this.query(_this.ObtainConditions(1), _this);

            })
            //下载数据
            $('#ExportData').off('click').on('click', function () {
                var parameter = _this.ObtainConditions(1);
                var that = $(this);
                var _url = public_url + '/api/ExcelOperation/ExportResources.aspx?Export=income&Keyword=' + parameter.Keyword + '&OrderBy=' + parameter.OrderBy;
                that.attr('href', _url);
            })
            $('#Inquiry').click();

        },
        query: function (parameter, _this) {
            setAjax({
                //url: public_url + '/api/IncomeManage/GetWithdrawalsStatisticsList',
                url: public_url + '/api/IncomeManage/GetWithdrawalsStatisticsList',
                type: 'get',
                data: parameter
            }, function (data) {
                if (data.Status == 0) {
                    $('#tbd_List').html(ejs.render($('#IncomeList').html(), data));
                    $('.TotalBar').html(ejs.render($('#IncomeTotal').html(), data));
                    if (data.Result.TotalCount != 0) {
                        var counts = data.Result.TotalCount;
                        //分页
                        $("#pageContainer").pagination(
                            counts,
                            {
                                items_per_page: 20, //每页显示多少条记录（默认为20条）
                                callback: function (currPage, jg) {
                                    parameter.PageIndex = currPage
                                    // ajax请求
                                    setAjax({
                                        //url: public_url + '/api/MediaUser/GetMediaUserList',
                                        url: public_url + '/api/IncomeManage/GetWithdrawalsStatisticsList',
                                        type: "GET",
                                        data: parameter
                                    }, function (data) {
                                        $('#tbd_List').html(ejs.render($('#IncomeList').html(), data));

                                    })
                                }
                            });

                    } else {
                        $('#pageContainer').html('<img src="/ImagesNew/no_data.png" style="display: block;margin: 70px auto;">')
                    }

                } else {
                    layer.msg(data.Message)
                }
            })
        }
    }
    new PresentManagement()
})