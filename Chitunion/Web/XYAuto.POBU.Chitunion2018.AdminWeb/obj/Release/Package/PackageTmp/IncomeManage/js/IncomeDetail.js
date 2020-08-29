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
    function PresentManagement() {
        this.init()
    }
    var obj = {
        UserID: GetRequest().UserID,
        PageSize: 20
    }
    PresentManagement.prototype = {
        constructor: PresentManagement,
        // 获取条件
        ObtainConditions: function (i) {
            obj.IncomeBeginTime = $("#txt_IncomeBeginTime").val();

            obj.Keyword = $('#u_name').val();
            obj.IncomeEndTime = $("#txt_IncomeEndTime").val();

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
            $("#select_IncomeType").change(function () {
                obj.CategoryID = $("#select_IncomeType option:selected").val();
                _this.query(_this.ObtainConditions(1), _this)
            });
            if (GetRequest().UserID = undefined || isNaN(parseInt(GetRequest().UserID))) {
                $('#txt_IncomeBeginTime').val(laydate.now(-6, 'YYYY-MM-DD'));
                $('#txt_IncomeEndTime').val(laydate.now(0, 'YYYY-MM-DD'));
            }
            //下载数据
            $('#ExportData').off('click').on('click', function () {
                var parameter = _this.ObtainConditions(1);
                var that = $(this);
                var _url = public_url + '/api/ExcelOperation/ExportResources.aspx?Export=incomedetail&UserName=' + parameter.UserName + '&UserID=' + parameter.UserID + '&IncomeBeginTime=' + parameter.IncomeBeginTime + '&IncomeEndTime=' + parameter.IncomeEndTime;
                that.attr('href', _url);
            })
            if (GetRequest().UserName != undefined || GetRequest().UserName != '')
            {
                $('#u_name').val(GetRequest().UserName)
             
            }
            $('#Inquiry').click();
        },
        query: function (parameter, _this) {
            setAjax({
                //url: public_url + '/api/IncomeManage/GetWithdrawalsStatisticsList',
                url: public_url + '/api/IncomeManage/GetIncomeDetailModelList',
                type: 'get',
                data: parameter
            }, function (data) {
                if (data.Status == 0) {
                    $('#tbd_List').html(ejs.render($('#IncomeDetailList').html(), data));
                    $('.TotalBar').html(ejs.render($('#IncomeDetailTotal').html(), data));
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
                                        url: public_url + '/api/IncomeManage/GetIncomeDetailModelList',
                                        type: "GET",
                                        data: parameter
                                    }, function (data) {
                                        $('#tbd_List').html(ejs.render($('#IncomeDetailList').html(), data));
                                        $('.TotalBar').html(ejs.render($('#IncomeDetailTotal').html(), data));
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