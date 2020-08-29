/**
 * Created by fengb on 2017/7/24.
 */

$(function () {

    var DemandBillNo = GetQueryString('DemandBillNo')!=null&&GetQueryString('DemandBillNo')!='undefined'?GetQueryString('DemandBillNo'):null;
    var status = GetQueryString('status')!=null&&GetQueryString('status')!='undefined'?GetQueryString('status'):null;

    var xyConfig = {
        url : {
            'GetDemandDetail' : public_url + '/api/Demand/GetDemandDetail',//基本信息
            'GetList' : public_url + '/api/GroundPage/GetList',//基本信息下面的table
            'AuditDemand' : public_url + '/api/Demand/AuditDemand',//审核
            'RelateToADGroup' : public_url + '/api/Demand/RelateToADGroup',
            'DeletePage' : public_url + '/api/GroundPage/DeletePage',//删除
            'AddPage' : public_url + '/api/GroundPage/AddPage',//添加
            'GetCarAndCityInfos' : public_url + '/api/GroundPage/GetCarAndCityInfos'//车型和城市的接口 添加落地页
        }
    } 


    var DetailOfOrder = {
        init : function () {
            //var url = 'json/GetDemandDetail.json';
            setAjax({
                url : xyConfig.url.GetDemandDetail,
                type : 'get',
                data : {
                    DemandBillNo : DemandBillNo
                }
            },function (data) {
                var Result = data.Result;
                if(data.Status == 0){
                    $('.Detail_basic').html(ejs.render($('#detail-demand').html(),Result));                    
                    DetailOfOrder.operation();
                }else{
                    layer.msg(data.Message,{'time':1000});
                }
            })

            //var url = 'json/GetList.json'
            setAjax({
                url : xyConfig.url.GetList,
                type : 'get',
                data : {
                    DemandBillNo : DemandBillNo
                }
            },function (data) {
                var Result = data.Result;
                if(data.Status == 0){
                    $('.Detail_table').html(ejs.render($('#detail-table').html(),{List:Result}));                    
                    DetailOfOrder.operation();
                }else{
                    layer.msg(data.Message,{'time':1000});
                }
            })

        },
        operation : function(){//操作
            /*
              根据不同的状态来判断操作
              驳回原因只有驳回状态下才能看到
              只有待投放和投放中数据 可以管理落地页和加参
            */ 
           if(status == 89001 || status == 89002 || status == 89007 || status == 89005){//待审核 已撤销 已驳回 已终止
                $('.to_b_list').hide();
            }else{
                $('.to_b_list').show();
            }

            //驳回原因
            if(status == 89002){//待审核 只能看
                $('.react_reason').show();
            }else{
                $('.react_reason').hide();
            }
            
            if(status == 89003 || status == 89004){
                $('.add_url').show();
                $('.delete_url').show();
            }else{
                $('.add_url').hide();
                $('.delete_url').hide();
            }

            //加参管理
            $('.manger_url').off('click').on('click',function(){
                var that = $(this);
                var GroundId = that.parents('tr').attr('GroundId');
                window.open('/JointManager/PutInAddCan.html?DemandBillNo='+DemandBillNo + '&status='+status +'&GroundId='+ GroundId);
            })

            //添加落地页URL
            $('.add_url').off('click').on('click',function(){
                var that = $(this);
                var _width = document.documentElement.clientWidth;
                var _height = document.documentElement.clientHeight;

                //var url = 'json/BrandCityProvince.json';
                setAjax({
                    url : xyConfig.url.GetCarAndCityInfos,
                    type : 'get',
                    data : {
                        DemandBillNo : DemandBillNo
                    }
                },function(data){
                    var Result = data.Result;
                    if(data.Status == 0){

                        //弹层
                        $('.channel_box').show();
                        $('.channel_box .layer').show();
                        var layer_height = $('.channel_box .layer').height();
                        var _left = (_width-370)/2;
                        var _top = (_height-layer_height)/2;
                        $('.channel_box').css({'width':_width,'height':_height,'position':'fixed','left':0,'top':0,'background':'rgba(0,0,0,0.7)'});
                        $('.channel_box .layer').css({'position':'absolute','left':_left,'top':_top});

                        //品牌 车型 
                        var BrandInfos = Result.BrandInfos;
                        var SerieInfos = Result.SerieInfos;
                        //省份城市
                        var ProvinceInfos = Result.ProvinceInfos;
                        var CitysInfos = Result.CitysInfos;

                        
                        //默认加载品牌
                        var Brand_str = '';

                        if(BrandInfos.length > 1){
                            Brand_str += '<option BrandId="-1">全部</option>';
                            for(var i=0;i<BrandInfos.length;i++){
                                Brand_str += '<option BrandId='+BrandInfos[i].BrandId+'>'+BrandInfos[i].BrandName+'</option>';
                            }
                            $('.BrandInfos').html(Brand_str);

                            //品牌联动的时候  加载车型
                            $('.BrandInfos').off('change').on('change',function(){
                                var BrandId = $(this).find('option:checked').attr('BrandId')*1;
                                if(BrandId == -1){
                                    $('.SerieInfos').html('<option SerielId="-1">全部</option>');
                                }else{
                                    var SerieArrs = [];
                                    var Serie_str = '';
                                    for(var i = 0; i<= SerieInfos.length-1;i++){
                                        if(SerieInfos[i].BrandId == BrandId){
                                            SerieArrs.push(SerieInfos[i]);
                                        }                                    
                                    }
                                    if(SerieArrs.length > 1){
                                        Serie_str += '<option SerielId="-1">全部</option>';
                                    }else{
                                        Serie_str += '';
                                    }
                                    for(var i=0;i<SerieArrs.length;i++){
                                        Serie_str += '<option SerielId='+SerieArrs[i].SerielId+'>'+SerieArrs[i].SerialName+'</option>';
                                    }
                                    $('.SerieInfos').html(Serie_str);
                                }
                            })

                        }else{
                            for(var i=0;i<BrandInfos.length;i++){
                                Brand_str += '<option BrandId='+BrandInfos[i].BrandId+'>'+BrandInfos[i].BrandName+'</option>';
                            }
                            $('.BrandInfos').html(Brand_str);
                            var Serie_str = '';
                            if(SerieInfos.length > 1){
                                var SerieArrs = [];
                                var BrandId = $('.BrandInfos').find('option:checked').attr('BrandId')*1;
                                for(var i = 0; i<= SerieInfos.length-1;i++){
                                    if(SerieInfos[i].BrandId == BrandId){
                                        SerieArrs.push(SerieInfos[i]);
                                    }                                    
                                }
                                Serie_str += '<option SerielId="-1">全部</option>';
                                for(var i=0;i<SerieArrs.length;i++){
                                    Serie_str += '<option SerielId='+SerieArrs[i].SerielId+'>'+SerieArrs[i].SerialName+'</option>';
                                }
                                $('.SerieInfos').html(Serie_str);
                            }else{
                                for(var i=0;i<SerieInfos.length;i++){
                                    Serie_str += '<option SerielId='+SerieInfos[i].SerielId+'>'+SerieInfos[i].SerialName+'</option>';
                                }
                                $('.SerieInfos').html(Serie_str);
                            }
                        }


                        //默认加载省份
                        var Province_str = '';
                        //当大于一个省份的时候  会有多选   二级城市可以下拉
                        if(ProvinceInfos.length > 1){
                            Province_str = '<option ProvinceId="-1">全部</option>';
                            for(var i=0;i<ProvinceInfos.length;i++){
                                Province_str += '<option ProvinceId='+ProvinceInfos[i].ProvinceId+'>'+ProvinceInfos[i].ProvinceName+'</option>';
                            }
                            $('.ProvinceInfos').html(Province_str);

                            //省份联动的时候
                            $('.ProvinceInfos').off('change').on('change',function(){
                                var ProvinceId = $(this).find('option:checked').attr('ProvinceId');
                                if(ProvinceId == -1){
                                    $('.CitysInfos').html('<option CityId="-1">全部</option>');
                                }else{
                                    var CitysArrs = [];
                                    var City_str = '';
                                    for(var i = 0; i<= CitysInfos.length-1;i++){
                                        if(CitysInfos[i].ProvinceId == ProvinceId){
                                            CitysArrs.push(CitysInfos[i]);                                        
                                        }
                                    }
                                    if(CitysArrs.length > 1){
                                        City_str += '<option CityId="-1">全部</option>';
                                    }else{
                                        City_str += '';
                                    }
                                    for(var i=0;i<CitysArrs.length;i++){
                                        City_str += '<option CityId='+CitysArrs[i].CityId+'>'+CitysArrs[i].CityName+'</option>';
                                    }
                                    $('.CitysInfos').html(City_str);
                                }
                            })
                        }else{//当只有一个省份的时候   则默认选中这一个    二级城市可以下拉可以不下拉
                            for(var i=0;i<ProvinceInfos.length;i++){
                                Province_str += '<option ProvinceId='+ProvinceInfos[i].ProvinceId+'>'+ProvinceInfos[i].ProvinceName+'</option>';
                            }
                            $('.ProvinceInfos').html(Province_str);

                            var City_str = '';
                            if(CitysInfos.length > 1){
                                var CitysArrs = [];
                                var ProvinceId = $('.ProvinceInfos').find('option:checked').attr('ProvinceId');
                                for(var i = 0; i<= CitysInfos.length-1;i++){
                                    if(CitysInfos[i].ProvinceId == ProvinceId){
                                        CitysArrs.push(CitysInfos[i]);                                        
                                    }
                                }
                                City_str += '<option CityId="-1">全部</option>';
                                for(var i=0;i<CitysArrs.length;i++){
                                    City_str += '<option CityId='+CitysArrs[i].CityId+'>'+CitysArrs[i].CityName+'</option>';
                                }
                                $('.CitysInfos').html(City_str);
                            }else{
                                for(var i=0;i<CitysInfos.length;i++){
                                    City_str += '<option CityId='+CitysInfos[i].CityId+'>'+CitysInfos[i].CityName+'</option>';
                                }
                                $('.CitysInfos').html(City_str);
                            }
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

                        var reg = /^https?:\/\/(([a-zA-Z0-9_-])+(\.)?)*(:\d+)?(\/((\.)?(\?)?=?&?[a-zA-Z0-9_-](\?)?)*)*$/i;
                        $('#PromotionUrl').on('blur',function(){
                            if(!reg.test($(this).val())){
                                $('.meg_tip').css('visibility','visible');
                                $('.meg_tip').html('请输入正确的URL');
                            }else{
                                $('.meg_tip').css('visibility','hidden');
                                $('.meg_tip').html('该落地页已被添加');
                            }
                        })                  

                        //确定
                        $('#submitMessage').off('click').on('click',function(){
                            if($.trim($('#PromotionUrl').val()) == ''){
                                $('.meg_tip').css('visibility','visible');
                                $('.meg_tip').html('请填写链接URL');
                            }else if(!reg.test($('#PromotionUrl').val())){
                                $('.meg_tip').css('visibility','visible');
                                $('.meg_tip').html('请输入正确的URL');
                            }else{
                                $('.meg_tip').css('visibility','hidden');
                                $('.meg_tip').html('该落地页已被添加');
                                setAjax({
                                    url : xyConfig.url.AddPage,
                                    type : 'post',
                                    data : {
                                        DemandBillNo : DemandBillNo,
                                        BrandId : $('.BrandInfos').find('option:checked').attr('BrandId')*1,
                                        SerielId : $('.SerieInfos').find('option:checked').attr('SerielId')*1,
                                        ProvinceId : $('.ProvinceInfos').find('option:checked').attr('ProvinceId')*1,
                                        CityId : $('.CitysInfos').find('option:checked').attr('CityId')*1,
                                        PromotionUrl : $.trim($('#PromotionUrl').val()) 
                                    }
                                },function(data){
                                    if(data.Status == 0){
                                        $('.channel_box').hide();
                                        $('.channel_box .layer').hide();
                                        DetailOfOrder.init();
                                        //弹层初始化
                                        $('#PromotionUrl').val('');
                                        $('.CitysInfos option').eq(0).prop('selected',true);
                                        $('.SerieInfos option').eq(0).prop('selected',true);
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
            $('.delete_url').off('click').on('click',function(){
                var that = $(this);
                var GroundId = that.parents('tr').attr('GroundId');
                layer.confirm('确认要删除该URL吗？删除后该URL的加参链接也会被删除？', {
                    btn: ['确认','取消'] //按钮
                }, function(){
                    layer.closeAll();
                    setAjax({
                        url : xyConfig.url.DeletePage,
                        type : 'post',
                        data : {
                            DemandBillNo : DemandBillNo,
                            GroundId : GroundId,
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

            /*//查看整体数据
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
            })*/
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


