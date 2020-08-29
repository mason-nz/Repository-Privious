/**
 * Created by fengb on 2017/7/24.
 */

$(function () {

    var DemandBillNo = GetQueryString('DemandBillNo')!=null&&GetQueryString('DemandBillNo')!='undefined'?GetQueryString('DemandBillNo'):null;
    var status = GetQueryString('status')!=null&&GetQueryString('status')!='undefined'?GetQueryString('status'):null;
    var GroundId = GetQueryString('GroundId')!=null&&GetQueryString('GroundId')!='undefined'?GetQueryString('GroundId'):null;

    var xyConfig = {
        url : {
            'GetDeliverys' : public_url + '/api/GroundPage/GetDeliverys',//渲染数据
            'GetWaittingAdList' : public_url + '/api/GroundPage/GetWaittingAdList',//关联广告Load
            'AddDelivery' : public_url + '/api/GroundPage/AddDelivery',//保存加参
            'DeleteDelivery' : public_url + '/api/GroundPage/DeleteDelivery',//删除加参
            'GetDictInfoByTypeId' : public_url + '/api/DictInfo/GetDictInfoByTypeId',//枚举信息
            'RelateToAdGroup' : public_url + '/api/GroundPage/RelateToAdGroup'//关联广告组
        }
    } 

    var DetailOfOrder = {
        init : function () {
            setAjax({
                //url : 'json/GetDeliverys.json',
                url : xyConfig.url.GetDeliverys,
                type : 'get',
                data : {
                    DemandBillNo : DemandBillNo
                }
            },function (data) {
                var Result = data.Result;
                if(data.Status == 0){
                    $('.install_box').html(ejs.render($('#detail-table').html(),Result)); 
                    if(status == 89003){
                        $('.Detail_table .relevance_po').hide();
                        $('.Detail_table .Data_detail').hide();
                    }else if(status == 89006){
                        $('.Detail_table .relevance_po').hide();
                        $('.Detail_table .delete_url').hide();
                        $('.AddCanShu').hide();
                    }else if(status == 89005){
                        $('.Detail_table .relevance_po').hide();
                        $('.Detail_table .delete_url').hide();
                        $('.Detail_table .Data_detail').hide();
                        $('.AddCanShu').hide();
                        $('.Detail_table .last_td').html('一');
                    }
                    DetailOfOrder.operation(Result);
                }else{
                    layer.msg(data.Message,{'time':1000});
                }
            })

        },
        operation : function(Result){//操作

            /*var idx = null;
            Result.GroundInfo.forEach(function(item,i){
                if(item.GroundId == GroundId){
                    idx = i;
                }
            })
            var bg = $('.to_b_list .each_item').eq(idx).find('.bg_color');
            bg.css({'background':'#555','opacity':'0.4'});
            var _scroll = 220+190*idx
            console.log(_scroll)
            $('html,body').animate({scrollTop:_scroll},50);*/

            //关联广告
            $('.Detail_table .relevance_po').off('click').on('click',function(){
                var that = $(this);
                var DeliveryId = that.parents('tr').attr('DeliveryId');
                var AuditStatusName = that.parents('tr').attr('AuditStatusName');
                var _width = document.documentElement.clientWidth;
                var _height = document.documentElement.clientHeight;

                var url = xyConfig.url.GetWaittingAdList;
                setAjax({
                    url : url,
                    type : 'get',
                    data : {
                        DeliveryId : DeliveryId
                    }
                },function(data){
                    var Result = data.Result;
                    if(data.Status == 0){

                        if(Result.length > 0){
                            $('.ChooseChannel').html(ejs.render($('#choose-channel').html(),{data:Result}));
                        }else{
                            $('.ChooseChannel').html('<img src="/images/no_data.png" style="margin-left:78px;">');
                        }

                        //高度计算要在渲染完数据以后才能计算出来
                        $('.channel_box').show();
                        $('.channel_box .layer').show();
                        var layer_height = $('.channel_box .layer').height();
                        var _left = (_width-550)/2;
                        var _top = (_height-layer_height)/2;
                        console.log(_height);
                        console.log(layer_height);
                        $('.channel_box').css({'width':_width,'height':_height,'position':'fixed','left':0,'top':0,'background':'rgba(0,0,0,0.7)'});
                        $('.channel_box .layer').css({'position':'absolute','left':_left,'top':_top});

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

                        //确定
                        $('#submitMessage').off('click').on('click',function(){
                            var ADGroupList = [];
                            $('.channel_box .onebox').each(function () {
                                if($(this).prop('checked')==true){
                                    ADGroupList.push($(this).parents('tr').attr('AdgroupId'));
                                }
                            })
                            
                            if(ADGroupList.length == 0){
                                layer.msg("请至少关联一个广告",{'time':1000});
                            }else{
                                setAjax({
                                    url : xyConfig.url.RelateToAdGroup,
                                    type : 'post',
                                    data : {
                                        DeliveryId : DeliveryId,
                                        AdGroupId :ADGroupList[0]
                                    }
                                },function(data){
                                    if(data.Status == 0){
                                        $('.channel_box').hide();
                                        $('.channel_box .layer').hide();
                                        DetailOfOrder.init();
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


            //落地页添加参数
            $('.AddCanShu').off('click').on('click',function(){
                var that = $(this);
                var _width = document.documentElement.clientWidth;
                var _height = document.documentElement.clientHeight;
                var GroundId = that.parents('ul').next('table').attr('GroundId');

                //广告版位的下拉框
                setAjax({
                    url : xyConfig.url.GetDictInfoByTypeId,
                    type : 'get',
                    data : {
                        typeId : 96
                    }
                },function(data){
                    var Result = data.Result;
                    if(data.Status == 0){

                        var str = '<option>请选择广告创意</option>';
                        for(var i = 0;i<= Result.length-1;i++){
                            str += '<option AdCreative='+ Result[i].DictId +'> '+ Result[i].DictName +' </option>';
                        }
                        $('#AdCreative').html(str);

                        //弹层
                        $('.Addurl_box').show();
                        $('.Addurl_box .layer').show();
                        var layer_height = $('.Addurl_box .layer').height();
                        var _left = (_width-370)/2;
                        var _top = (_height-layer_height)/2;
                        $('.Addurl_box').css({'width':_width,'height':_height,'position':'fixed','left':0,'top':0,'background':'rgba(0,0,0,0.7)'});
                        $('.Addurl_box .layer').css({'position':'absolute','left':_left,'top':_top});


                        //关闭
                        $('#closebt2').off('click').on('click',function(){
                            $('.Addurl_box').hide();
                            $('.Addurl_box .layer').hide();
                        })
                        //取消
                        $('#cancleMessage1').off('click').on('click',function(){
                            $('.Addurl_box').hide();
                            $('.Addurl_box .layer').hide();
                        })     


                        //确定
                        $('#submitMessage1').off('click').on('click',function(){
                            if($.trim($('#AdCreative').val()) == '请选择广告创意'){
                                $('.meg_tip').css('visibility','visible');
                                $('.meg_tip').html('请选择广告创意');
                            }else if($('#AdName').val() == ''){
                                $('.meg_tip').css('visibility','visible');
                                $('.meg_tip').html('请输入广告名称');
                            }else{
                                $('.meg_tip').css('visibility','hidden');
                                $('.meg_tip').html('请选择广告创意');
                                setAjax({
                                    url : xyConfig.url.AddDelivery,
                                    type : 'post',
                                    data : {
                                        GroundId : GroundId,
                                        DeliveryType : 95001,//广点通 95001 今日头条 95002
                                        AdCreative : $('#AdCreative').find('option:checked').attr('AdCreative')*1,
                                        AdName : $.trim($('#AdName').val()) 
                                    }
                                },function(data){
                                    if(data.Status == 0){
                                        $('.Addurl_box').hide();
                                        $('.Addurl_box .layer').hide();
                                        DetailOfOrder.init();
                                        $('#AdName').val('');
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


            //删除
            $('.Detail_table .delete_url').off('click').on('click',function(){
                var that = $(this);
                var DeliveryId = that.parents('tr').attr('DeliveryId');
                layer.confirm('确认要删除该数据吗', {
                    btn: ['确认','取消'] //按钮
                }, function(){
                    layer.closeAll();
                    setAjax({
                        url : xyConfig.url.DeleteDelivery,
                        type : 'post',
                        data : {
                            DeliveryId : DeliveryId
                        }
                    },function(data){
                        if(data.Status == 0){
                            DetailOfOrder.init();
                        }else{
                            layer.msg(data.Message,{'time':1000});
                        }
                    })
                })
            })


            //复制内容到粘贴板
            $('.Detail_table .Copy_url').off('click').on('click',function(){
                var clipboard = new Clipboard('.Copy_url');
                clipboard.on('success', function(e) {
                    layer.msg('Copied!',{'time':300});
                    e.clearSelection();
                });
                clipboard.on('error', function(e) {
                    console.error('Action:', e.action);
                    console.error('Trigger:', e.trigger);
                });
            })

            //效果数据  AdgroupId这个字段暂时不确定
            $('.Detail_table .Data_detail').off('click').on('click',function(){
                var that = $(this);
                var AdgroupId = that.parents('tr').attr('AdgroupId');
                window.open('/JointManager/adver_data.html?AdgroupId='+AdgroupId);
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


