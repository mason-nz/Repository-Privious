/**
 * Created by fengb on 2017/8/8.
 */

$(function () {

    var MaterielId = GetQueryString('MaterielID')!=null&&GetQueryString('MaterielID')!='undefined'?GetQueryString('MaterielID'):null;

    var config = {};

    var PayType,
        PayType_text,
        PayMode,
        PayMode_text,
        UnitCost,
        ChannelType,
        ChannelType_text,
        MediaTypeName,
        MediaTypeName_text,
        MediaNumber_Name;

    
    var Media_Arr = [];//账号和密码的数组
    var Item_Info_arr = [];//需要渲染的大数组

    var DetailOfOrder = {
        Init : function () {
            //渲染基本信息
            var url = 'http://www.chitunion.com/api/Materiel/getInfo';
            setAjax({
                url : url,
                type : 'get',
                data : {
                    MaterielId : MaterielId,
                    IsGetChannel : true
                }
            },function (data) {
                var Result = data.Result;
                if(data.Status == 0){
                    $('.main_message .ThirdMaterielId').text(Result.MaterielId);
                    $('.main_message .MaterieName').text(Result.Name);
                    $('.main_message .MaterieSource').text(Result.ArticleFromName);
                    $('.main_message .SerialName').text(Result.SerialName);

                    DetailOfOrder.CheckoutRequire();
                    DetailOfOrder.SaferToGenerate();
                }else{
                    layer.msg(data.Message,{'time':1000});
                }
            })
            
        },
        CheckoutRequire : function(){//校验

            //费用类型
            $('.install_box input[name=CostType]').on('change',function(){
                var that = $(this);
                if(that.val() == '71002'){//免费
                    that.parents('ul').next('.collect_feeds').hide();
                }else if(that.val() == '71001'){//收费
                    that.parents('ul').next('.collect_feeds').show();
                }
            })

            //单位成本
            $('.unite input[name=UnitCost]').on('change',function(){
                var that = $(this);
                var reg = /^-?(?!0+(\.0*)?$)\d+(\.\d{1,2})?$/;
                if(!reg.test(that.val())){
                    that.parent().next().show();
                    that.parent().next().html('单位成本格式不正确');
                }else{
                    that.parent().next().hide();
                    that.parent().next('.channel_info').html('单位成本不能为空');
                } 
            })

            //其它类型
            $('.metia_condition input[name=OtherType]').on('change',function(){
                var that = $(this);
                var reg = /^([\u4e00-\u9fa5]|[a-z]|[A-Z]|[0-9]|\s)+$/;
                if(!reg.test(that.val())){
                    that.parent().next().show();
                }else{
                    that.parent().next().hide();
                }
            }) 

            //媒体
            $('.install_box input[name=MetiaType]').on('change',function(){
                var that = $(this);
                var val = that.val();
                if(val == '其它'){
                    that.parents('ul').next().find('.media_type').show();
                }else{
                    that.parents('ul').next().find('.media_type').hide();
                }
            })

            //媒体账号
            $('.install_box .account_number').on('change',function(){
                var that = $(this);
                var val = $.trim(that.val());
                if(val == ''){
                    that.parent().next().show();
                }else{
                    that.parent().next().hide();
                }
            })   
            
        },
        SaferToGenerate : function(){//保存

            var flag1 = false;//单位成本
            var flag2 = false;//账号
            var flag3 = false;//其它的类型

            //添加账号按钮
            $('#add_button').off('click').on('click',function(){

                MediaNumber_Name = [];
                Media_Arr = [];

                PayType = $('.install_box input[name=CostType]:checked').val()*1;//费用类型
                PayType_text = $('.install_box input[name=CostType]:checked').next().text();

                PayMode = $('.install_box input[name=PayType]:checked').val()*1;//付费模式
                PayMode_text = $('.install_box input[name=PayType]:checked').next().text();

                UnitCost = $.trim($('.unite input[name=UnitCost]').val())*1;//单位成本

                ChannelType = $('.install_box input[name=ChannelType]:checked').val()*1;//渠道类型
                ChannelType_text = $('.install_box input[name=ChannelType]:checked').next().text();

                MediaTypeName = $('.install_box input[name=MetiaType]:checked').val();//媒体类型
                MediaTypeName_text = $('.install_box input[name=MetiaType]:checked').next().text();


                MediaNumber_Name = $.trim($('.install_box .account_number').val()).split('；');//多个账号和名称
                Media_Arr = Media_Arr.concat(MediaNumber_Name);
                //去空
                for(var i = Media_Arr.length;i >= 0;i--){
                    if(Media_Arr[i] == ''){
                        Media_Arr.splice(i,1);
                    }
                }


                //收费或者免费的时候的验证
                if(PayType == 71001){//收费  

                    //验证单位成本
                    if(UnitCost == 0){
                        $('.install_box input[name=UnitCost]').parent().next().show();
                        $('.install_box input[name=UnitCost]').parent().next('.channel_info').html('单位成本不能为空');
                        //单位成本格式验证
                        $('.install_box input[name=UnitCost]').on('change',function(){
                            var that = $(this);
                            var reg = /^-?(?!0+(\.0*)?$)\d+(\.\d{1,2})?$/;
                            if(!reg.test(that.val())){
                                that.parent().next().show();
                                that.parent().next().html('单位成本格式不正确');
                                flag1 = false;
                            }else{
                                that.parent().next().hide();
                                flag1 = true;
                            } 
                        })
                    }else{
                        var reg = /^-?(?!0+(\.0*)?$)\d+(\.\d{1,2})?$/;
                        if(!reg.test(UnitCost)){
                            //单位成本格式验证
                            $('.install_box input[name=UnitCost]').on('change',function(){
                                var that = $(this);
                                if(!reg.test(that.val())){
                                    that.parent().next().show();
                                    flag1 = false;
                                }else{
                                    that.parent().next().hide();
                                    flag1 = true;
                                } 
                            })
                        }else{
                            flag1 = true;
                        }
                    }
                    //账号验证
                    if(MediaNumber_Name == ''){
                        $('.install_box .account_number').parent().next().show();
                    }else{
                        $('.install_box .account_number').parent().next().hide();
                        flag2 = true;
                    }

                    //判断账号类型为其它的时候  或者微信等
                    if(MediaTypeName == '其它'){
                        var other_type = $('.metia_condition input[name=OtherType]').val();
                        if(other_type == ''){
                            $('.metia_condition input[name=OtherType]').parent().next().show();
                            $('.metia_condition input[name=OtherType]').parent().next().html('类型不能为空');
                            //格式验证
                            $('.metia_condition input[name=OtherType]').on('change',function(){
                                var that = $(this);
                                var reg = /^([\u4e00-\u9fa5]|[a-z]|[A-Z]|[0-9]|\s)+$/;
                                if(!reg.test(that.val())){
                                    that.parent().next().show();
                                    that.parent().next().html('类型格式不正确');
                                    flag3 = false;
                                }else{
                                    that.parent().next().hide();
                                    flag3 = true;
                                    MediaTypeName_text = other_type;//更新其它的类型名称
                                }
                            }) 
                        }else{
                            var reg = /^([\u4e00-\u9fa5]|[a-z]|[A-Z]|[0-9]|\s)+$/;
                            if(!reg.test(other_type)){
                                //格式验证
                                $('.metia_condition input[name=OtherType]').on('change',function(){
                                    var that = $(this);
                                    if(!reg.test(that.val())){
                                        that.parent().next().show();
                                        flag3 = false;
                                    }else{
                                        that.parent().next().hide();
                                        flag3 = true;
                                        MediaTypeName_text = other_type;//更新其它的类型名称
                                    }
                                })
                            }else{
                                flag3 = true;
                                MediaTypeName_text = other_type;//更新其它的类型名称
                            }
                        }

                        if(flag1 && flag2 && flag3){//有其它
                            SendToData ();
                        }
                    }else{
                        if(flag1 && flag2){//木有其它
                            SendToData ();
                        }
                    }

                }else if(PayType == 71002){//免费

                    //账号验证
                    if(MediaNumber_Name == ''){
                        $('.install_box .account_number').parent().next().show();
                    }else{
                        $('.install_box .account_number').parent().next().hide();
                        flag2 = true;
                    }

                    //判断账号类型为其它的时候  或者微信等
                    if(MediaTypeName == '其它'){
                        var other_type = $('.metia_condition input[name=OtherType]').val();
                        if(other_type == ''){
                            $('.metia_condition input[name=OtherType]').parent().next().show();
                            $('.metia_condition input[name=OtherType]').parent().next().html('类型不能为空');
                            //格式验证
                            $('.metia_condition input[name=OtherType]').on('change',function(){
                                var that = $(this);
                                var reg = /^([\u4e00-\u9fa5]|[a-z]|[A-Z]|[0-9]|\s)+$/;
                                if(!reg.test(that.val())){
                                    that.parent().next().show();
                                    that.parent().next().html('类型格式不正确');
                                    flag3 = false;
                                }else{
                                    that.parent().next().hide();
                                    flag3 = true;
                                    MediaTypeName_text = other_type;//更新其它的类型名称
                                }
                            }) 
                        }else{
                            var reg = /^([\u4e00-\u9fa5]|[a-z]|[A-Z]|[0-9]|\s)+$/;
                            if(!reg.test(other_type)){
                                //格式验证
                                $('.metia_condition input[name=OtherType]').on('change',function(){
                                    var that = $(this);
                                    if(!reg.test(that.val())){
                                        that.parent().next().show();
                                        flag3 = false;
                                    }else{
                                        that.parent().next().hide();
                                        flag3 = true;
                                        MediaTypeName_text = other_type;//更新其它的类型名称
                                    }
                                })
                            }else{
                                flag3 = true;
                                MediaTypeName_text = other_type;//更新其它的类型名称
                            }
                        }

                        if(flag2 && flag3){
                            SendToData ();
                        }
                    }else{
                        if(flag2){//木有其它
                            SendToData ();
                        }
                    }

                }


                //渲染表格
                function SendToData (){
                    //获取列表数据
                    for(var i = 0;i<Media_Arr.length;i++){
                        var num_name = Media_Arr[i];
                        Item_Info_arr.push({
                            PayType : PayType,
                            PayType_text : PayType_text,
                            PayMode : PayMode,
                            PayMode_text : PayMode_text,
                            UnitCost : UnitCost,
                            ChannelType : ChannelType,
                            ChannelType_text : ChannelType_text,
                            PayType1 : PayType,
                            PayType_text1 : PayType_text,
                            PayMode1 : PayMode,
                            PayMode_text1 : PayMode_text,
                            UnitCost1 : UnitCost,
                            ChannelType1 : ChannelType,
                            ChannelType_text1 : ChannelType_text,
                            MediaTypeName : MediaTypeName_text,//媒体
                            MediaNumber : num_name.split('|')[0],//账号
                            MediaName : num_name.split('|')[1]//名称
                        });
                    }
                    $('.adposition_table').show();//显示表格
                    $('.adposition_table table  tbody').html(ejs.render($('#orderInfo').html(),{data:Item_Info_arr}));
                    $('.adposition_table table  tbody').find('.begin_edit').hide();//默认隐藏编辑的样式
                    $('.PromoCode').show();//显示生成推广码的按钮

                    //恢复之前的初始化样式
                    $('.install_box .account_number').val('');//账号
                    $('.unite input[name=UnitCost]').val('');//成本
                    $('.install_box input[value="微信"]').prop('checked',true);
                    $('.metia_condition .media_type').hide();//类型
                    $('.metia_condition input[name=OtherType]').val('');//类型输入框
                    
                    DetailOfOrder.CheckoutRequire();//重新生成一遍验证
                    DetailOfOrder.SaferToGenerate();
                    DetailOfOrder.Operation();//操作按钮
                    DetailOfOrder.SafeToPromoCode();//保存并生成推广码
                }
            })        
            
        },
        Operation : function(){//  编辑  更新  删除  取消
            //编辑
            $('.adposition_table table tbody .edit').off('click').on('click',function(){
                var that = $(this);
                var idx = that.parents('tr').index();
                var Tr = that.parents('tr');


                var ChannelTypeName = Tr.attr('ChannelType_text');//渠道类型
                var PayTypeName = Tr.attr('PayType_text');//费用类型
                var PayModeName = Tr.attr('PayMode_text');//付费模式

                var idx1,idx2,idx3;
                
                if(ChannelTypeName == '众包'){
                    idx1 = 0;
                }else if(ChannelTypeName == 'UGC'){
                    idx1 = 1;
                }else if(ChannelTypeName == '媒体节点'){
                    idx1 = 2;   
                }else if(ChannelTypeName == '其它'){
                    idx1 = 3;
                }

                if(PayTypeName == '收费'){
                    idx2 = 0;
                    that.parents('tr').find('.init').hide();
                    that.parents('tr').find('.begin_edit').show();
                    Tr.find('select[name=PayMode]').show();
                    Tr.find('input[name=UnitCost]').show();
                }else if(PayTypeName == '免费'){
                    idx2 = 1;
                    that.parents('tr').find('.init').hide();
                    that.parents('tr').find('.begin_edit').show();
                    Tr.find('select[name=PayMode]').hide();
                    Tr.find('input[name=UnitCost]').hide();
                }

                if(PayModeName == 'CPM'){
                    idx3 = 0;
                }else if(PayModeName == 'CPC'){
                    idx3 = 1;
                }else if(PayModeName == 'CPL'){
                    idx3 = 2;   
                }

                //默认选中
                Tr.find('select[name=ChannelType] option').eq(idx1).prop('selected',true);
                Tr.find('select[name=PayType] option').eq(idx2).prop('selected',true);
                Tr.find('select[name=PayMode] option').eq(idx3).prop('selected',true);


                //渠道类型
                Tr.find('select[name=ChannelType]').on('change',function(){
                    var ChannelType = $(this).find('option:checked').val();
                    var ChannelType_text = $(this).find('option:checked').text();
                    Tr.attr('ChannelType1',ChannelType);
                    Tr.attr('ChannelType_text1',ChannelType_text);
                    Item_Info_arr[idx].ChannelType = ChannelType;
                    Item_Info_arr[idx].ChannelType_text = ChannelType_text;
                })

                //费用类型
                Tr.find('select[name=PayType]').on('change',function(){
                    var PayType = $(this).find('option:checked').val();
                    var PayType_text = $(this).find('option:checked').text();
                    Tr.attr('PayType1',PayType);
                    Tr.attr('PayType_text1',PayType_text);
                    Item_Info_arr[idx].PayType = PayType;
                    Item_Info_arr[idx].PayType_text = PayType_text;
                    if(PayType_text == '收费'){
                        Tr.find('select[name=PayMode]').show();
                        Tr.find('input[name=UnitCost]').show();
                    }else{
                        Tr.find('select[name=PayMode]').hide();
                        Tr.find('input[name=UnitCost]').hide();
                    }
                })

                //付费模式
                Tr.find('select[name=PayMode]').on('change',function(){
                    var PayMode = $(this).find('option:checked').val();
                    var PayMode_text = $(this).find('option:checked').text();
                    Tr.attr('PayMode1',PayMode);
                    Tr.attr('PayMode_text1',PayMode_text);
                    Item_Info_arr[idx].PayMode = PayMode;
                    Item_Info_arr[idx].PayMode_text = PayMode_text;
                })

                //单位成本
                Tr.find('input[name=UnitCost]').on('change',function(){
                    var reg = /^-?(?!0+(\.0*)?$)\d+(\.\d{1,2})?$/;
                    var UnitCost = $.trim($(this).val());
                    if(!reg.test(UnitCost)){
                        layer.msg('单位成本格式不正确',{'time':1000});
                        that.parents('tr').find('.init').hide();
                        that.parents('tr').find('.begin_edit').show();
                        Tr.find('select[name=PayMode]').show();
                        Tr.find('input[name=UnitCost]').show();
                    }else{
                        Tr.attr('UnitCost1',UnitCost);
                    }
                })


                //that.parents('tr').find('.init').hide();
                //that.parents('tr').find('.begin_edit').show();
                that.parents('tr').find('.update').show();//更新
                that.parents('tr').find('.cancel').show();//取消
                that.parents('tr').find('.edit').hide();//编辑
                that.parents('tr').find('.delete').hide();//删除
            })

            //更新  只是界面给用户更新数据的感觉
            $('.adposition_table table tbody .update').off('click').on('click',function(){
                var that = $(this);
                var idx = that.parents('tr').index();
                var Tr = that.parents('tr');
                
                var ChannelType_text1 = Tr.attr('ChannelType_text1');
                var PayType_text1 = Tr.attr('PayType_text1');
                var PayMode_text1 = Tr.attr('PayMode_text1');
                var UnitCost1 = Tr.attr('UnitCost1');

                //更新了  当ChannelType1不等于ChannelType的时候  说明  更新了 则需要更新文字和ChannelType的属性
                if(Tr.attr('ChannelType1') != Tr.attr('ChannelType')){
                    Tr.find('select[name=ChannelType]').prev().text(ChannelType_text1);
                    Tr.attr('ChannelType',Tr.attr('ChannelType1'));
                    Tr.attr('ChannelType_text',ChannelType_text1);
                }

                if(Tr.attr('PayType1') != Tr.attr('PayType')){
                    Tr.find('select[name=PayType]').prev().text(PayType_text1);
                    Tr.attr('PayType',Tr.attr('PayType1'));
                    Tr.attr('PayType_text',PayType_text1);
                }
                if(Tr.attr('PayMode1') != Tr.attr('PayMode')){
                    Tr.find('select[name=PayMode]').prev().text(PayMode_text1);
                    Tr.attr('PayMode',Tr.attr('PayMode1'));
                    Tr.attr('PayMode_text',PayMode_text1);
                }
                if(Tr.attr('UnitCost1') != Tr.attr('UnitCost')){
                    Tr.find('input[name=UnitCost]').prev().text(UnitCost1);
                    Tr.attr('UnitCost',Tr.attr('UnitCost1'));
                }

                
                //判断是否未收费还是付费
                if(Tr.attr('PayType') == '71002'){//免费
                    
                    that.parents('tr').find('.init').show();
                    that.parents('tr').find('.begin_edit').hide();
                    Tr.find('.PayMode').hide();
                    Tr.find('.UnitCost').hide();
                    that.parents('tr').find('.update').hide();//更新
                    that.parents('tr').find('.cancel').hide();//取消
                    that.parents('tr').find('.edit').show();//编辑
                    that.parents('tr').find('.delete').show();//删除

                }else if(Tr.attr('PayType') == '71001'){//收费的时候    需要验证一下单位成本
                    var UnitCost_val = Tr.find('input[name=UnitCost]').val();
                    if(UnitCost_val == ''){
                        layer.msg('单位成本不能为空',{'time':1000});
                        that.parents('tr').find('.update').show();//更新
                        that.parents('tr').find('.cancel').show();//取消
                        that.parents('tr').find('.edit').hide();//编辑
                        that.parents('tr').find('.delete').hide();//删除
                    }else{
                        var reg = /^-?(?!0+(\.0*)?$)\d+(\.\d{1,2})?$/;
                        if(!reg.test(UnitCost_val)){
                            layer.msg('单位成本格式不正确',{'time':1000});
                            //单位成本
                            Tr.find('input[name=UnitCost]').on('change',function(){
                                var that = $(this);
                                if(!reg.test(that.val())){
                                    layer.msg('单位成本格式不正确',{'time':1000});
                                    that.parents('tr').find('.init').hide();
                                    that.parents('tr').find('.begin_edit').show();
                                }else{
                                    that.parents('tr').find('.init').show();
                                    that.parents('tr').find('.begin_edit').hide();
                                    Tr.find('.PayMode').show();
                                    Tr.find('.UnitCost').show();
                                    Tr.find('input[name=UnitCost]').prev().text(that.val());
                                    Tr.attr('UnitCost',that.val());
                                    that.parents('tr').find('.update').hide();//更新
                                    that.parents('tr').find('.cancel').hide();//取消
                                    that.parents('tr').find('.edit').show();//编辑
                                    that.parents('tr').find('.delete').show();//删除
                                } 
                            })
                        }else{
                            that.parents('tr').find('.begin_edit').hide();
                            that.parents('tr').find('.init').show();
                            Tr.find('.PayMode').show();
                            Tr.find('.UnitCost').show();
                            that.parents('tr').find('.update').hide();//更新
                            that.parents('tr').find('.cancel').hide();//取消
                            that.parents('tr').find('.edit').show();//编辑
                            that.parents('tr').find('.delete').show();//删除
                        }
                    }
                }
                
            })

            //取消
            $('.adposition_table table tbody .cancel').off('click').on('click',function(){
                var that = $(this);
                var idx = that.parents('tr').index();
                var Tr = that.parents('tr');

                if(Tr.attr('PayType') == '71002'){//免费
                    that.parents('tr').find('.init').show();
                    that.parents('tr').find('.begin_edit').hide();
                    Tr.find('.PayMode').hide();
                    Tr.find('.UnitCost').hide();
                }else{//收费
                    that.parents('tr').find('.init').show();
                    that.parents('tr').find('.begin_edit').hide();
                }


                if(Tr.attr('ChannelType1') != Tr.attr('ChannelType')){
                    Tr.attr('ChannelType',Tr.attr('ChannelType'));
                }

                if(Tr.attr('PayType1') != Tr.attr('PayType')){
                    Tr.attr('PayType',Tr.attr('PayType'));
                }
                if(Tr.attr('PayMode1') != Tr.attr('PayMode')){
                    Tr.attr('PayMode',Tr.attr('PayMode'));
                }

                that.parents('tr').find('.update').hide();//更新
                that.parents('tr').find('.cancel').hide();//取消
                that.parents('tr').find('.edit').show();//编辑
                that.parents('tr').find('.delete').show();//删除

            })
            //删除
            $('.adposition_table table tbody .delete').off('click').on('click',function(){
                var that = $(this);
                var idx = that.parents('tr').index();
                var len = that.parents('tbody').find('tr').length;

                layer.confirm('确认要删除数据吗', {
                    btn: ['确认','取消'] //按钮
                }, function(){
                    layer.closeAll();
                    if(len > 1){
                        that.parents('tr').remove();
                        Item_Info_arr.splice(idx,1);
                    }else{
                        that.parents('tr').remove();
                        Item_Info_arr.splice(idx,1);
                    }
                })
            })
            DetailOfOrder.SaferToGenerate();
            DetailOfOrder.SafeToPromoCode();//保存并生成推广码

        },
        SafeToPromoCode : function(){//保存并生成推广码

            $('#PromoCode').off('click').on('click',function(){
                var ChannelList = [];
                var tr_len = $('.get_tab tr').length;

                if(tr_len > 0){
                    $('.get_tab tr').each(function(){
                        var that = $(this);
                        var obj = {
                            "ChannelID":0,
                            "MediaTypeName": that.attr('MediaTypeName'),
                            "ChannelType": that.attr('ChannelType')*1,
                            "MediaNumber": that.attr('MediaNumber'),
                            "MediaName": that.attr('MediaName'),
                            "PayType": that.attr('PayType'),
                            "PayMode": that.attr('PayMode'),
                            "UnitCost": that.attr('UnitCost'),
                        }
                        if(that.attr('MediaName') == 'undefined'){
                            obj.MediaName = '';
                        }
                        ChannelList.push(obj);
                    })
                }else{
                    ChannelList = [];
                }

                setAjax({
                    url : 'http://www.chitunion.com/api/MaterielChannel/SaveChannel',
                    type : 'post',
                    data : {
                        MaterielID : MaterielId,
                        ChannelList : ChannelList
                    }
                },function(data){
                    if(data.Status == 0){
                        window.location = '/MaterielManger/release_detail.html?MaterielID='+MaterielId;
                    }else{
                        layer.msg(data.Message,{'time':1000});
                    }
                })
            })

        }
    };

    DetailOfOrder.Init();

})


//获取url 地址参数方法
function GetQueryString(name) {
    var reg = new RegExp("(^|&)"+ name +"=([^&]*)(&|$)");
    var r = window.location.search.substr(1).match(reg);
    if(r!=null)return r[2]; return null;
}


Array.prototype.norepeatArray=function(){
    var obj={},temp=[];//创建临时对象和数组
    //循环遍历数组
    for(var i=0;i<this.length;i++){
        if(!obj[this[i]]){
            temp.push(this[i]);
            obj[this[i]]=true;
        }
    }
    return temp;
}