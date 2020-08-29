/**
 * Created by fengb on 2017/7/24.
 */

$(function () {

    var DemandBillNo = GetQueryString('DemandBillNo')!=null&&GetQueryString('DemandBillNo')!='undefined'?GetQueryString('DemandBillNo'):null;
    var isPass = GetQueryString('isPass')!=null&&GetQueryString('isPass')!='undefined'?GetQueryString('isPass'):null;

    var config = {};

    var DetailOfOrder = {
        init : function () {
            var url = 'http://www.chitunion.com/api/Demand/GetDemandDetail';
            setAjax({
                url : url,
                type : 'get',
                data : {
                    DemandBillNo : DemandBillNo
                }
            },function (data) {
                var Result = data.Result;
                config = Result;
                if(data.Status == 0){
                    $('.install_box').html(ejs.render($('#detail-demand').html(),Result));
                    
                    //根据是通过的查看   还是驳回的查看
                    if(isPass == 0){
                        $('.react_reason').show();
                        $('.to_b_list').hide();
                    }else{
                        $('.react_reason').hide();
                        $('.to_b_list').show();
                    }
                    DetailOfOrder.operation();
                }else{
                    layer.msg(data.Message,{'time':1000});
                }
            })
        },
        operation : function(){//操作

            //关联广告
            $('.relevance_btn').off('click').on('click',function(){
                //console.log(config);
                var that = $(this);
                var DemandBillNo = config.Demand.DemandBillNo;
                var AuditStatusName = config.Demand.AuditStatusName;//状态
                var _width = document.documentElement.clientWidth;
                var _height = document.documentElement.clientHeight;

                var ADGroupList = [];//要关联的广告组ID数组

                /*$('.channel_box').show();
                $('.channel_box .layer').show();

                var layer_height = $('.channel_box .layer').height();
                var _left = (_width-500)/2;
                var _top = (_height-layer_height)/2;

                $('.channel_box').css({'width':_width,'height':_height,'position':'fixed','left':0,'top':0,'background':'rgba(0,0,0,0.7)'});
                $('.channel_box .layer').css({'position':'absolute','left':_left,'top':_top});*/


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
                                        $('.install_box').html(ejs.render($('#detail-demand').html(),config));
                                    }else{
                                        layer.msg(data.Message);
                                    }
                                })
                            }
                        })
                    }else{
                        layer.msg(data.Message,{'time':1000});
                    }
                })
            })


            //创建广告
            $('.add_po_btn').off('click').on('click',function(){
                window.open('http://e.qq.com/atlas/ad/create');
            })


            //查看整体数据
            $('.see_all_btn').off('click').on('click',function(){
                window.open('/JointManager/effect_data.html');
            })

            //效果数据
            $('.effect_data').off('click').on('click',function(){
                var that = $(this);
                var AdgroupId = that.parents('tr').attr('AdgroupId');
                
                window.open('/JointManager/adver_data.html?AdgroupId='+AdgroupId);
            })

            //广点通查看
            $('.see_position').off('click').on('click',function(){
                window.open('http://e.qq.com/atlas/index');
            })

        }        
    };

    DetailOfOrder.init();

})


//获取url 地址参数方法
function GetQueryString(name) {
    var reg = new RegExp("(^|&)"+ name +"=([^&]*)(&|$)");
    var r = window.location.search.substr(1).match(reg);
    if(r!=null)return r[2]; return null;
}


