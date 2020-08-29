/**
 * Created by fengb on 2017/4/25.
 */

$(function () {

    var PubID = GetQueryString('PubID')!=null&&GetQueryString('PubID')!='undefined'?parseFloat  (GetQueryString('PubID')):null;


    var config = {};
    config.PubID = PubID;

    var publishCheck = {
        constructor : publishCheck,
        init : function () {
            var url = '/api/Periodication/SelectWXPublishByIDAndType';
            //var url = './js/adCheck.json';
            setAjax({
                url:url,
                type:'get',
                data: {
                    PubID : config.PubID,
                    MediaType : 14001
                }
            },function(data){
                if(data.Status == 0){
                    config.DetailMeg = data.Result;
                    publishCheck.showTemplate(config.DetailMeg,'#publishCheckTemplte');
                    publishCheck.rejectOrPass();
                }else{
                    layer.alert(data.Message);
                }
            });
        },
        showTemplate : function (data,id) {
            var _this = this;
            //->首先把页面中模板的innerHTML获取到
            var str=$(id).html();
            //->然后把str和data交给EJS解析处理，得到我们最终想要的字符串
            var result = ejs.render(str, {
                data: data
            });
            //->最后把获取的HTML放入到MENU
            $(".install_box").html(result);
        },
        rejectOrPass : function () {//审核成功或者驳回

            var passMeg = '';
            var rejectMeg = '';
            var rejectMessage = '';

            $('.ins_a').find('input[type=radio]').on('click',function () {
                $('#submitMessage').css('background',"#FF4F4F");
                $('#submitMessage').removeAttr('flag');
            })

            if($('.ins_a').find('input[type=radio]').is(':checked')){
                $('#submitMessage').css('background',"#FF4F4F");
                $('#submitMessage').removeAttr('flag');
            }else{
                $('#submitMessage').css('background',"grey");
                $('#submitMessage').attr('flag','');
            }

            //驳回
            $('#reject').on('click',function () {
                rejectMeg = true;
                if($(this).is(':checked')){
                    $('#submitMessage').css('background',"#FF4F4F");
                    $('#submitMessage').removeAttr('flag');
                    $(this).parent().next().find('textarea').show();
                    $('textarea').on('blur',function () {
                        rejectMessage = $(this).val();
                        if(rejectMessage == ''){
                            alert('请输入驳回原因');
                        }
                    })
                }else{
                    //$('textarea').hide();
                }
            })

            //通过
            $('#passOrder').on('click',function () {
                passMeg = true;
            })

            //当点击提交的时候
            $('#submitMessage').on('click',function () {
                if($(this).attr('flag') == ''){
                    return;
                }else if(rejectMeg == true){
                    if(rejectMessage == ''){
                        layer.alert('请填写驳回原因');
                    }else{
                        //驳回通过接口啊
                        var url = '/api/Publish/AuditPublish?v=1_1';
                        setAjax({
                            url : url,
                            type : 'post',
                            data : {
                                PubID : config.PubID,//当前的刊例id
                                RejectReason : rejectMessage,//驳回原因
                                OpType : 42003
                            }
                        },function (data) {
                            if(data.Status == 0){
                                layer.alert(data.Message);
                                var NextPubID = data.Result.NextPubID;  //获取下一条刊例id  然后再掉接口  从新渲染
                                if(NextPubID == 0){
                                    layer.alert('已经是最后一条喽');
                                }else{
                                    window.location = '/PublishManager/advertisementCheck.html?PubID='+NextPubID;
                                }
                            }else{
                                layer.alert(data.Message);
                            }
                        })
                    }
                }else {
                    publishCheck.nextCheck();//调用审核接口  得到下一个
                }
            })

            //点击取消的时候回到列表页面
            $('#cancleMessage').on('click',function () {
                layer.confirm('你确定要取消吗', {
                    btn: ['确认','取消'] //按钮
                }, function(){
                    window.location = '/publishmanager/newsreview.html';
                    layer.closeAll();
                },function(){
                    layer.closeAll();
                });

            })
        },
        nextCheck : function () {//审核成功以后  下一条信息  刊例id从接口获取出来  通过以后
            var url = '/api/Publish/AuditPublish?v=1_1';
            setAjax({
                url : url,
                type : 'post',
                data : {
                    PubID : config.PubID,//当前的刊例id
                    OpType : 42002
                }
            },function (data) {
                if(data.Status == 0){
                    layer.alert(data.Result.Message);//审核信息
                    var NextPubID = data.Result.NextPubID;  //获取下一条刊例id  然后再掉接口  从新渲染
                    if(NextPubID == 0){
                        layer.alert('已经是最后一条喽');
                    }else{
                        window.location = '/PublishManager/advertisementCheck.html?PubID='+NextPubID;
                    }
                    //window.location = '/PublishManager/advertisementCheck.html?PubID='+NextPubID;
                }else{
                    layer.alert(data.Message);
                }
            })
        }
    };

    publishCheck.init();

    //获取url 地址参数方法
    function GetQueryString(name) {
        var reg = new RegExp("(^|&)"+ name +"=([^&]*)(&|$)");
        var r = window.location.search.substr(1).match(reg);
        if(r!=null)return unescape(r[2]); return null;
    }

})
