$(function () {

    var config = {};

    var xyConfig = {
        url : {
            'GetDemandList' : public_url + '/api/Demand/GetDemandList',
            'GetWaittingADList' : public_url + '/api/Demand/GetWaittingADList',
            'AuditDemand' : public_url + '/api/Demand/AuditDemand',
            'RelateToADGroup' : public_url + '/api/Demand/RelateToADGroup'
        }
    }


    function weChatnews() {
        this.queryparameters();
        $('#searchBtn').click();
    }

    weChatnews.prototype = {
        constructor: weChatnews,
        // 获取查询参数
        queryparameters: function () {
            var _this = this;
            
            if(CTLogin.RoleIDs == 'SYS005RL00021'){//广告运营
                $('#Pending').hide();
                $('#Reject').hide();
                $('#Chexiao').hide();
                $('#wait_put').addClass('selected').siblings().removeClass('selected');
            }

            // 切换
            $('.tab_menu li').off('click').on('click', function () {
                $(this).addClass('selected').siblings('li').removeClass('selected');
                $('#searchBtn').click();
            })

            // 搜索
            $('#searchBtn').off('click').on('click', function () {
                // 创建人
                var CreateUser = $.trim($('#CreateUser').val());
                // 广告名称
                var DemandName   = $.trim($('#DemandName').val());
                //所属运营
                var BelongYY = $.trim($('#BelongYY').val());
               
                // 获取通过、待审、驳回 的状态
                var state = '';
                $('.tab_menu li').each(function () {
                    if ($(this).attr('class') == 'selected') {
                        state = $(this).attr('name');
                        config.state = state;
                    }
                })

                //console.log(config.state)

                if(config.state == 89001 || config.state == 89002 || config.state == 89007){
                    $('.BelongYY').hide();
                }else{
                    $('.BelongYY').show();
                }

                //初始化的所有的变量
                var obj = {};
                //默认待审核
                obj = {
                    CreateUser: CreateUser,
                    DemandName: DemandName,
                    BelongYY: BelongYY,
                    AuditStatus : state,
                    PageSize : 20,
                    PageIndex : 1,
                }
                _this.requestdata(obj);
            })
        },
        // 请求数据
        requestdata: function (obj) {
            var _this = this;
            setAjax({
                url: xyConfig.url.GetDemandList,
                type: 'get',
                data: obj,
            }, function (data) {
                var Result = data.Result;
                // 渲染数据
                _this.renderdata(_this, data);
                // 如果数据为0显示图片
                if(data.Result.TotalCount != 0) {
                    //分页
                    $("#pageContainer").pagination(
                        data.Result.TotalCount,
                        {
                            items_per_page: 20, //每页显示多少条记录（默认为20条）
                            callback: function (currPage, jg) {
                                obj.PageIndex = currPage;
                                setAjax({
                                    url: xyConfig.url.GetDemandList,
                                    type: 'get',
                                    data: obj
                                }, function (data) {
                                    var Result = data.Result;
                                    // 渲染数据
                                    _this.renderdata(_this, data);
                                })
                            }
                        });
                }else{
                    $('#pageContainer').html('<img src="/images/no_data.png" style="display: block;margin: 70px auto;">');
                }
            })
        },
        // 渲染数据
        renderdata: function (_this, data) {
            
            $('.table table').html(ejs.render($('#Pricelist-cc').html(), data));

            if(config.state == '89001'){//待审核
                $('.tab_box').find('.AuditTime').hide();
                $('.tab_box').find('.RevokeTime').hide();
                $('.tab_box').find('.BelongYY').hide();
                $('.tab_box').find('.ADCount').hide();
                $('.tab_box .RechargeNumber').hide();

                $('.tab_box').find('.see_po').show();
                $('.tab_box').find('.audit_po').show();

            }else if(config.state == '89003'){//待投放
                $('.tab_box').find('.UpdateTime').hide();
                $('.tab_box').find('.RevokeTime').hide();

                $('.tab_box').find('.see_po').show();
                $('.tab_box').find('.Url_manger').show();
                $('.tab_box').find('.forbidden_po').show();

            }else if(config.state == '89004'){//投放中
                $('.tab_box').find('.UpdateTime').hide();
                $('.tab_box').find('.RevokeTime').hide();

                $('.tab_box').find('.see_po').show();
                $('.tab_box').find('.Url_manger').show();
                $('.tab_box').find('.ending_put').show();

            }else if(config.state == '89006'){//已结束
                $('.tab_box').find('.AuditTime').hide();
                $('.tab_box').find('.RevokeTime').hide();
                $('.tab_box .UpdateTr').html('结束时间');

                $('.tab_box').find('.see_po').show();
                $('.tab_box').find('.Data_detail').show();

            }else if(config.state == '89005'){//已终止
                $('.tab_box').find('.AuditTime').hide();
                $('.tab_box').find('.RevokeTime').hide();
                $('.tab_box .UpdateTr').html('终止时间');

                $('.tab_box').find('.see_po').show();
                $('.tab_box').find('.Data_detail').hide();

            }else if(config.state == '89002'){//已驳回
                $('.tab_box').find('.AuditTime').hide();
                $('.tab_box').find('.RevokeTime').hide();
                $('.tab_box .UpdateTr').html('驳回时间');
                $('.tab_box .RechargeNumber').hide();
                $('.tab_box .BelongYY').hide();
                $('.tab_box .ADCount').hide();

                $('.tab_box').find('.see_po').show();
            }else if(config.state == '89007'){//已撤销
                $('.tab_box').find('.AuditTime').hide();
                $('.tab_box').find('.UpdateTime').hide();
                $('.tab_box .RechargeNumber').hide();
                $('.tab_box .BelongYY').hide();
                $('.tab_box .ADCount').hide();

                $('.tab_box').find('.see_po').show();
            }
            _this.operation();
        },
        // 操作
        operation: function () {

            var _this = this;

            //终止投放
            $('.forbidden_po').off('click').on('click',function(){
                var that = $(this);
                var DemandBillNo = that.parents('tr').attr('DemandBillNo');
                layer.confirm('您确定要终止投放？', {
                    btn: ['确认','取消'] //按钮
                }, function(){
                    layer.closeAll();
                    setAjax({
                        url : xyConfig.url.AuditDemand,
                        type : 'post',
                        data : {
                            DemandBillNo : DemandBillNo,
                            AuditStatus : 89005,
                            Reason : ''
                        }
                    },function(data){
                        if(data.Status == 0){
                            $('#searchBtn').click();
                        }else{
                            layer.msg(data.Message,{'time':1000});
                        }
                    })
                })
            })

            //结束投放
            $('.ending_put').off('click').on('click',function(){
                var that = $(this);
                var DemandBillNo = that.parents('tr').attr('DemandBillNo');
                layer.confirm('您确定要结束投放？', {
                    btn: ['确认','取消'] //按钮
                }, function(){
                    layer.closeAll();
                    setAjax({
                        url : xyConfig.url.AuditDemand,
                        type : 'post',
                        data : {
                            DemandBillNo : DemandBillNo,
                            AuditStatus : 89006,
                            Reason : ''
                        }
                    },function(data){
                        if(data.Status == 0){
                            $('#searchBtn').click();
                        }else{
                            layer.msg(data.Message,{'time':1000});
                        }
                    })
                })
            })

           
           //审核
           $('.audit_po').off('click').on('click',function(){
                var that = $(this);
                var DemandBillNo = that.parents('tr').attr('DemandBillNo');

                window.open('/JointManager/DemandAudit.html?DemandBillNo='+DemandBillNo);
           })

           //查看
           $('.see_po').off('click').on('click',function(){
                var that = $(this);
                var DemandBillNo = that.parents('tr').attr('DemandBillNo');
                var status = $('#list_switching .selected').attr('name')*1;
                window.open('/JointManager/PutIn_detail.html?DemandBillNo='+DemandBillNo +'&status='+status);
           })

            //落地页管理
            $('.Url_manger').off('click').on('click',function(){
                var that = $(this);
                var DemandBillNo = that.parents('tr').attr('DemandBillNo');
                var status = $('#list_switching .selected').attr('name')*1;
                window.open('/JointManager/PutIn_detail.html?DemandBillNo='+DemandBillNo +'&status='+status);
            })

            //效果数据 
            $('.Data_detail').off('click').on('click',function(){
                var that = $(this);
                var DemandBillNo = that.parents('tr').attr('DemandBillNo');
                window.open('/JointManager/demand_data.html?DemandBillNo='+DemandBillNo);
            })

        },
    }
    var wechatnews = new weChatnews();

})
