/**
 * Created by fengb on 2017/8/9.
 */

$(function () {

    var MaterielId = GetQueryString('MaterielID')!=null&&GetQueryString('MaterielID')!='undefined'?GetQueryString('MaterielID'):null;

    var config = {};
    var DetailOfOrder = {
        init : function () {
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
                config = Result;
                if(data.Status == 0){
                    $('.install_box').html(ejs.render($("#release-detail").html(),Result));
                    if(config.ChannelItem == null){//添加按钮  添加渠道
                        $('.add_btn_channel').parent().show();
                        $('.edit_btn_channel').parent().hide();
                    }else{//编辑按钮 编辑渠道
                        $('.add_btn_channel').parent().hide();
                        $('.edit_btn_channel').parent().show();
                    }
                    DetailOfOrder.operatingBtn();
                }else{
                    layer.msg(data.Message,{'time':1000});
                }
            })
        },
        operatingBtn : function(){//操作
            //全选
            $('#selectAll').on('change',function () {
                var that = $(this);
                if(that.prop('checked') == true){
                    $('.to_b_list_box input[name=PitchUp]').prop('checked',true);
                }else{
                    $('.to_b_list_box input[name=PitchUp]').prop('checked',false);
                }
            })

            //单选
            $('.onecheck').off('click').on('click',function () {
                if($('.to_b_list_box input[name=PitchUp]').length == $('.onecheck:checked').length){
                    $('#selectAll').prop('checked',true);
                }else{
                    $('#selectAll').prop('checked',false);
                }
            })

            //打包下载
            $('#PackageDownload').off('click').on('click',function () {
                var arrid=[];
                $('.onebox').each(function () {
                    if($(this).prop('checked')==true){
                        arrid.push($(this).attr('arrid'));
                    }
                })
                if(arrid.length){
                    $('#Form_Code input[name=ChannelIds]').val(arrid.join(','));
                    $('#Form_Code').submit();
                }else {
                    layer.msg("请选择渠道",{'time':1000});
                }
            })

            //每行下载
            $('.download').off('click').on('click',function () {
                var _this=$(this);
                    $('#Form_Code input[name=ChannelIds]').val($(this).attr('arrid'));
                    $('#Form_Code').submit();
            })
            
            //复制内容到粘贴板
            $('.clone').off('click').on('click',function(){
                var clipboard = new Clipboard('.clone');
                clipboard.on('success', function(e) {
                    layer.msg('Copied!',{'time':300});
                    e.clearSelection();
                });
                clipboard.on('error', function(e) {
                    console.error('Action:', e.action);
                    console.error('Trigger:', e.trigger);
                });
            })


            //添加渠道
           $('.add_btn_channel').off('click').on('click',function(){
                window.location = '/MaterielManger/release_channel.html?MaterielID='+MaterielId;
            })
           //编辑渠道 
            $('.edit_btn_channel').off('click').on('click',function(){
                window.location = '/MaterielManger/edit_chanel.html?MaterielID='+MaterielId;
            })
            
            //查看数据
            $('.see_channel_btn').off('click').on('click',function(){
                window.location = '/MaterielManger/Ad_Data.html?MaterielID='+MaterielId;
            })
            
        },
    };

    DetailOfOrder.init();

})


//获取url 地址参数方法
function GetQueryString(name) {
    var reg = new RegExp("(^|&)"+ name +"=([^&]*)(&|$)");
    var r = window.location.search.substr(1).match(reg);
    if(r!=null)return r[2]; return null;
}


