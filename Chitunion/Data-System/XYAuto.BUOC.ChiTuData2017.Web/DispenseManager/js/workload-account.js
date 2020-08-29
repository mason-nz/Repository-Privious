/*
* Written by:     fengb
* function:       工作量统计
* Created Date:   2017-11-17
*/

$(function () {

    var config = {};

    function workloadAccout() {
        $('#BeginTime').off('click').on('click', function () {
            laydate({
                fixed: false,
                elem: '#BeginTime',
                choose: function (date) {
                    if (date > $('#EndTime').val() && $('#EndTime').val()) {
                        layer.msg('起始时间不能大于结束时间！',{'time':1000});
                        $('#BeginTime').val('');
                    }
                }
            });
        });
        $('#EndTime').off('click').on('click', function () {
            laydate({
                fixed: false,
                elem: '#EndTime',
                choose: function (date) {
                    if (date < $('#BeginTime').val() && $('#BeginTime').val()) {
                        layer.msg('结束时间不能小于起始时间！',{'time':1000});
                        $('#EndTime').val('');
                    }
                }
            });
        });
        this.queryparameters();
        $('#searchBtn').click();
    }

    workloadAccout.prototype = {
        constructor: workloadAccout,
        // 获取查询参数
        queryparameters: function () {
            var _this = this;
            
            // 切换
            $('.tab_menu li').off('click').on('click', function () {
                var that = $(this);
                $(this).addClass('selected').siblings('li').removeClass('selected');
                $('#searchBtn').click();
            })

            // 搜索
            $('#searchBtn').off('click').on('click', function () {
                // 创建人
                var UserName = $.trim($('#UserName').val());
                // 查询时间
                var BeginTime = $.trim($('#BeginTime').val());
                var EndTime = $.trim($('#EndTime').val());
                //类型
                var Operator = $('.tab_menu li.selected').attr('name')*1;

                //初始化的所有的变量
                var obj = {
                    PageSize : 20,
                    PageIndex : 1,
                    BeginTime : BeginTime,
                    EndTime : EndTime,
                    Operator : Operator,
                    UserName : UserName
                }
                _this.requestdata(obj);
            })
            
        },
        // 请求数据
        requestdata: function (obj) {
            var _this = this;
            $.ajax({
                url: public_url + '/api/Workload/WorkloadStatisticsList',
                type: 'get',
                data: obj,
                async : false,
                beforeSend: function(){
                    $('#listLoading').html('<img src="/ImagesNew/icon_loading.gif" style="display: block;margin: 70px auto;">');
                },
                success : function (data) {
                    $('#listLoading').html('');
                    if(data.Status == 0){
                        var Result = data.Result;
                        var idx = $('.tab_menu li.selected').index();
                        $('.table tbody').html('');
                        if(Result.List != null){
                            if(Result.List.length > 0){
                                // 渲染数据
                                _this.renderdata(_this, Result);
                                _this.createPageController(_this, obj, Result);
                            }else{
                                $('#pageContainer').hide();
                                $('.no_data').html('<img src="/ImagesNew/no_data.png" style="display: block;margin: 70px auto;">');
                            }
                        }else{
                            $('#pageContainer').hide();
                            $('.no_data').html('<img src="/ImagesNew/no_data.png" style="display: block;margin: 70px auto;">');
                        }
                    }else{
                        layer.msg(data.Message);
                    }  
                }              
            })

            //下载数据
            $('#download').off('click').on('click',function(){
                var that = $(this);
                var _url = public_url + '/api/ExcelOperation/WorkloadExportExcel.aspx?BeginTime=' + obj.BeginTime + '&EndTime=' + obj.EndTime + '&Operator=' + obj.Operator + '&UserName=' + obj.UserName;
                that.attr('href',_url);
                return;
                setAjax({
                    url: public_url + '/api/Workload/WorkloadStatisticsExport',
                    type: 'get',
                    data: obj,
                }, function (data) {
                    if(data.Status == 0){
                        var Result = data.Result;
                        window.open(Result);
                    }else{
                        layer.msg(data.Message);
                    }                
                })
            })
        },
        // 分页
        createPageController : function(_this, obj, Result){
            var counts = Result.TotalCount;
            $("#pageContainer").pagination(counts, {
                current_page: (obj.PageIndex ? obj.PageIndex : 1),
                items_per_page: 20, 
                callback: function (currPage) {
                    var obj1 = obj;
                    obj1.PageIndex = currPage;
                    _this.requestdata(obj1);
                }
            });
        },
        // 渲染数据
        renderdata: function (_this, Result) {
            var idx = $('.tab_menu li.selected').index();
            $('.table').html(ejs.render($('#Workload' + idx).html(), Result));
        }
    }
    var workloadAccout = new workloadAccout();

})


