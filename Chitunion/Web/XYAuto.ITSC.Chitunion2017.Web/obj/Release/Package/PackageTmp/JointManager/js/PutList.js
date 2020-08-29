$(function () {

    var config = {};

    config.state == 'pass';

    function weChatnews() {
        this.queryparameters();
        $('#searchBtn').click();
    }

    weChatnews.prototype = {
        constructor: weChatnews,
        // 获取查询参数
        queryparameters: function () {
            var _this = this;
            // 切换
            $('.tab_menu li').off('click').on('click', function () {
                $(this).addClass('selected').siblings('li').removeClass('selected');
                $('#searchBtn').click();
            })

            if(CTLogin.RoleIDs=='SYS001RL00001'){//超管
                $('#list_switching').show();  
            }else if(CTLogin.RoleIDs=='SYS001RL00011'){//广告运营
                $('#list_switching').hide(); 
            }

            // 搜索
            $('#searchBtn').off('click').on('click', function () {
                // 创建人
                var CreateUser = $.trim($('#CreateUser').val());
                // 广告名称
                var DemandName   = $.trim($('#DemandName').val());
                //所属运营
                var BelongYY = $.trim($('#BelongYY').val());
                // 需求状态
                var AuditStatus = $('#AuditStatus option:checked').attr('value');
               

                // 获取通过、待审、驳回 的状态
                var state = '';
                $('.tab_menu li').each(function () {
                    if ($(this).attr('class') == 'selected') {
                        state = $(this).attr('name');
                        config.state = state;
                    }
                })

                
                //初始化的所有的变量
                var obj = {};

                //默认已通过
                obj = {
                    CreateUser: CreateUser,
                    DemandName: DemandName,
                    BelongYY: BelongYY,
                    AuditStatus : AuditStatus,
                    TabType : state,
                    PageSize : 20,
                    PageIndex : 1,
                }
                $('.state .BelongYY').show();//所属运营
                $('.state .adStatus').show();//需求状态


                if(state == 'wait'){//待审核
                    obj = {
                        CreateUser: CreateUser,
                        DemandName: DemandName,
                        BelongYY: BelongYY,
                        AuditStatus : AuditStatus,
                        TabType : state,
                        PageSize : 20,
                        PageIndex : 1,
                    }
                    $('.state .BelongYY').hide();//所属运营
                    $('.state .adStatus').hide();//需求状态
                }
                if(state == 'reject'){//已驳回
                    obj = {
                        CreateUser: CreateUser,
                        DemandName: DemandName,
                        BelongYY: BelongYY,
                        AuditStatus : AuditStatus,
                        TabType : state,
                        PageSize : 20,
                        PageIndex : 1,
                    }
                    $('.state .BelongYY').hide();//所属运营
                    $('.state .adStatus').hide();//需求状态
                }

                if(CTLogin.RoleIDs=='SYS001RL00011'){//广告运营
                    $('.state .BelongYY').hide();//所属运营 
                }
                
                _this.requestdata(obj);
            })
        },
        // 请求数据
        requestdata: function (obj) {
            var _this = this;

            var url = 'http://www.chitunion.com/api/Demand/GetDemandList';
            setAjax({
                url: url,
                type: 'get',
                data: obj,
            }, function (data) {

                var Result = data.Result;
                // 渲染数据
                _this.renderdata(_this, data);
                _this.operation();

                // 如果数据为0显示图片
                if (data.Result.TotalCount != 0) {
                    //分页
                    $("#pageContainer").pagination(
                        data.Result.TotalCount,
                        {
                            items_per_page: 20, //每页显示多少条记录（默认为20条）
                            callback: function (currPage, jg) {
                                obj.pageIndex = currPage;
                                setAjax({
                                    url: 'http://www.chitunion.com/api/Demand/GetDemandList',
                                    type: 'get',
                                    data: obj
                                }, function (data) {
                                    var Result = data.Result;
                                    // 渲染数据
                                    _this.renderdata(_this, data);
                                    _this.operation();
                                })
                            }
                        });
                } else {
                    $('#pageContainer').html('<img src="/images/no_data.png" style="display: block;margin: 70px auto;">');
                }
            })
        },
        // 渲染数据
        renderdata: function (_this, data) {
            if(CTLogin.RoleIDs=='SYS001RL00001'){
                $('.table table').html(ejs.render($('#Pricelist-cc').html(), data));
                //超管的各个状态下显示的行不一样  需要动态控制
                if(config.state == 'pass'){
                    $('.tab_box').find('.UpdateTime').hide();
                    $('.tab_box').find('.audit_po').hide();
                }else if(config.state == 'wait'){
                    $('.tab_box').find('.AuditTime').hide();
                    $('.tab_box').find('.BelongYY').hide();
                    $('.tab_box').find('.ADCount').hide();
                    $('.tab_box').find('.AuditStatusName').hide();
                    $('.tab_box').find('.see_po').hide();
                    $('.tab_box').find('.relevance_po').hide();
                    $('.tab_box').find('.forbidden_po').hide();
                }else if(config.state == 'reject'){
                    $('.tab_box').find('.UpdateTime').hide();
                    $('.tab_box').find('.BelongYY').hide();
                    $('.tab_box').find('.ADCount').hide();
                    $('.tab_box').find('.AuditStatusName').hide();
                    $('.tab_box').find('.audit_po').hide();
                    $('.tab_box').find('.relevance_po').hide();
                    $('.tab_box').find('.forbidden_po').hide();
                }
            }
            if(CTLogin.RoleIDs=='SYS001RL00011'){//广告运营
                $('.table table').html(ejs.render($('#Pricelist-yy').html(), data));
            }
        },
        // 操作
        operation: function () {

            var _this = this;

            //关联广告
            $('.relevance_po').off('click').on('click',function(){
                var that = $(this);
                var DemandBillNo = that.parents('tr').attr('DemandBillNo');
                var AuditStatusName = that.parents('tr').attr('AuditStatusName');
                var _width = document.documentElement.clientWidth;
                var _height = document.documentElement.clientHeight;

                var ADGroupList = [];//要关联的广告组ID数组

                //$('.channel_box').show();
                //$('.channel_box .layer').show();

                /*var layer_height = $('.channel_box .layer').height();
                var _left = (_width-500)/2;
                var _top = (_height-layer_height)/2;

                $('.channel_box').css({'width':_width,'height':_height,'position':'fixed','left':0,'top':0,'background':'rgba(0,0,0,0.7)'});
                $('.channel_box .layer').css({'position':'absolute','left':_left,'top':_top});
*/

                var url = 'http://www.chitunion.com/api/Demand/GetWaittingADList';
                setAjax({
                    url : url,
                    type : 'get',
                    data : {
                        DemandBillNo : DemandBillNo
                    }
                },function(data){
                    var Result = data.Result;
                    if(data.Status == 0){

                        $('.channel_box').show();
                        $('.channel_box .layer').show();
                        var layer_height = $('.channel_box .layer').height();
                        var _left = (_width-550)/2;
                        var _top = (_height-layer_height)/2;
                        $('.channel_box').css({'width':_width,'height':_height,'position':'fixed','left':0,'top':0,'background':'rgba(0,0,0,0.7)'});
                        $('.channel_box .layer').css({'position':'absolute','left':_left,'top':_top});


                        if(Result.length > 0){
                            $('#selectAll').show();
                            $('.ChooseChannel').html(ejs.render($('#choose-channel').html(),{data:Result}));
                        }else{
                            //全选隐藏
                            $('#selectAll').hide();
                            $('.ChooseChannel').html('<img src="/images/no_data.png" style="margin-left:78px;">');
                        }


                        if($('.channel_box input[name=PitchUp]').length == $('.channel_box .onebox:checked').length){
                            $('#selectAll').prop('checked',true);
                        }

                        //关闭
                        $('#closebt1').off('click').on('click',function(){
                            $('.channel_box').hide();
                            $('.channel_box .layer').hide();
                        })
                        //取消
                        $('#cancleMessage').off('click').on('click',function(){
                            $('.channel_box').hide();
                            $('.channel_box .layer').hide();
                        })

                        //全选
                        $('#selectAll').on('change',function () {
                            var that = $(this);
                            if(that.prop('checked') == true){
                                $('.channel_box input[name=PitchUp]').prop('checked',true);
                            }else{
                                $('.channel_box input[name=PitchUp]').prop('checked',false);
                            }
                        })

                        //单选
                        $('.channel_box .onebox').on('change',function () {
                            if($('.channel_box input[name=PitchUp]').length == $('.channel_box .onebox:checked').length){
                                $('#selectAll').prop('checked',true);
                            }else{
                                $('#selectAll').prop('checked',false);
                            }
                        })


                        //确定
                        $('#submitMessage').off('click').on('click',function(){
                            var ADGroupList = [];
                            $('.channel_box .onebox').each(function () {
                                if($(this).prop('checked')==true){
                                    ADGroupList.push($(this).parents('tr').attr('AdgroupId'));
                                }
                            })

                            if(AuditStatusName == '投放中'){
                                if(ADGroupList.length == 0){
                                    layer.msg("请至少关联一个广告",{'time':1000});
                                }else{
                                    setAjax({
                                        url : 'http://www.chitunion.com/api/Demand/RelateToADGroup',
                                        type : 'post',
                                        data : {
                                            DemandBillNo : DemandBillNo,
                                            ADGroupList :ADGroupList
                                        }
                                    },function(data){
                                        if(data.Status == 0){
                                            $('.channel_box').hide();
                                            $('.channel_box .layer').hide();
                                            $('#searchBtn').click();
                                        }else{
                                            layer.msg(data.Message);
                                        }
                                    })
                                }
                            }else{
                                setAjax({
                                    url : 'http://www.chitunion.com/api/Demand/RelateToADGroup',
                                    type : 'post',
                                    data : {
                                        DemandBillNo : DemandBillNo,
                                        ADGroupList :ADGroupList
                                    }
                                },function(data){
                                    if(data.Status == 0){
                                        $('.channel_box').hide();
                                        $('.channel_box .layer').hide();
                                        $('#searchBtn').click();
                                    }else{
                                        layer.msg(data.Message);
                                    }
                                })
                            }
                        })
                    }else{
                        $('.channel_box').hide();
                        $('.channel_box .layer').hide();
                        layer.msg(data.Message,{'time':1000});
                    }
                })
            })


            //终止投放
            $('.forbidden_po').off('click').on('click',function(){
                var that = $(this);
                var DemandBillNo = that.parents('tr').attr('DemandBillNo');

                layer.confirm('您确定要终止投放？终止后，不再同步效果数据', {
                    btn: ['确认','取消'] //按钮
                }, function(){
                    layer.closeAll();
                    setAjax({
                        url : 'http://www.chitunion.com/api/Demand/AuditDemand',
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

                if(config.state == 'pass'){
                    window.open('/JointManager/PutIn_detail.html?DemandBillNo='+DemandBillNo +'&isPass=1');
                }else if(config.state == 'reject'){
                    window.open('/JointManager/PutIn_detail.html?DemandBillNo='+DemandBillNo +'&isPass=0');
                }
           })

        },
    }
    var wechatnews = new weChatnews();

})
